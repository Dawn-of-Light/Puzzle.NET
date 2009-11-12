Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Reflection
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class CodeToClasses
    Implements ICodeToClasses

    Public Overridable Overloads Sub AssemblyCodeToClasses(ByVal asm As System.Reflection.Assembly, ByVal targetDomainMap As IDomainMap) Implements ICodeToClasses.AssemblyCodeToClasses

        Dim classType As Type
        Dim superClassType As Type
        Dim propertyInfo As propertyInfo
        Dim basePropertyInfo As propertyInfo
        Dim propertyType As Type
        Dim itemType As Type
        Dim classMap As IClassMap
        Dim superClassMap As IClassMap
        Dim refClassMap As IClassMap
        Dim propertyMap As IPropertyMap
        'Dim attr As ListItemTypeAttribute
        'Dim iterAttr As ListItemTypeAttribute
        Dim rootNamespace As String = targetDomainMap.RootNamespace

        Dim ok As Boolean

        For Each classType In asm.GetTypes

            If Not classType.IsInterface Then

                classMap = New classMap

                classMap.Name = classType.ToString

                classMap.DomainMap = targetDomainMap

                classMap.IsAbstract = classType.IsAbstract

            End If

        Next

        For Each classType In asm.GetTypes

            If Not classType.IsInterface Then

                classMap = targetDomainMap.GetClassMap(classType.ToString)

                superClassType = classType.BaseType

                superClassMap = targetDomainMap.GetClassMap(superClassType.ToString)

                If Not superClassMap Is Nothing Then

                    classMap.InheritsClass = superClassType.ToString

                End If

                For Each propertyInfo In classType.GetProperties(BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)

                    ok = True

                    If Not propertyInfo.CanRead Then

                        ok = False

                    End If

                    If ok Then

                        If Not superClassMap Is Nothing Then

                            basePropertyInfo = classType.BaseType.GetProperty(propertyInfo.Name, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.DeclaredOnly)

                            If Not basePropertyInfo Is Nothing Then

                                ok = False

                            End If

                        End If

                    End If

                    If ok Then

                        propertyMap = New propertyMap

                        propertyMap.Name = propertyInfo.Name

                        propertyMap.ClassMap = classMap

                        propertyType = propertyInfo.PropertyType

                        If Not propertyInfo.CanWrite Then

                            propertyMap.IsReadOnly = True

                        End If

                        If propertyInfo.GetGetMethod.IsPublic Then

                            propertyMap.Accessibility = AccessibilityType.PublicAccess

                        ElseIf propertyInfo.GetGetMethod.IsPrivate Then

                            propertyMap.Accessibility = AccessibilityType.PrivateAccess

                        Else

                            propertyMap.Accessibility = AccessibilityType.ProtectedAccess

                        End If

                        If Not propertyType.GetInterface(GetType(IList).ToString) Is Nothing Then

                            propertyMap.IsCollection = True

                            propertyMap.DataType = propertyType.ToString

                            'propertyMap.DefaultValue = "new " & propertyType.ToString & "()"

                            'For Each iterAttr In propertyInfo.GetCustomAttributes(GetType(ListItemTypeAttribute), True)

                            '    attr = iterAttr

                            '    Exit For

                            'Next

                            'If Not attr Is Nothing Then

                            '    itemType = attr.ItemType

                            '    propertyMap.ItemType = itemType.ToString

                            '    refClassMap = targetDomainMap.GetClassMap(itemType.ToString)

                            '    If Not refClassMap Is Nothing Then

                            '        propertyMap.ReferenceType = ReferenceType.ManyToOne

                            '    Else


                            '    End If

                            'Else

                            'End If

                        Else

                            propertyMap.DataType = propertyType.ToString

                            refClassMap = targetDomainMap.GetClassMap(propertyType.ToString)

                            If Not refClassMap Is Nothing Then

                                propertyMap.ReferenceType = ReferenceType.OneToMany

                            Else

                            End If

                        End If

                    End If

                Next

            End If

        Next

        If Len(rootNamespace) > 0 Then

            TrimNamespaces(targetDomainMap, rootNamespace)

        Else

            ExtractRootNamespace(targetDomainMap)

        End If

    End Sub

    Protected Overridable Sub TrimNamespaces(ByVal targetDomainMap As IDomainMap, ByVal rootNamespace As String)

        Dim classMap As IClassMap

        For Each classMap In targetDomainMap.ClassMaps

            If Len(classMap.Name) > Len(rootNamespace) Then

                If LCase(Left(classMap.Name, Len(rootNamespace) + 1)) = LCase(rootNamespace) & "." Then

                    classMap.Name = Right(classMap.Name, Len(classMap.Name) - Len(rootNamespace) - 1)

                End If

            End If

        Next

    End Sub

    Protected Overridable Sub ExtractRootNamespace(ByVal targetDomainMap As IDomainMap)

        Dim classMap As IClassMap
        Dim ns As String
        Dim currNs As String

        For Each classMap In targetDomainMap.ClassMaps

            ns = classMap.GetNamespace

            If Len(ns) > 0 Then

                If Len(currNs) < 1 Then

                    currNs = ns

                Else

                    If Len(ns) = Len(currNs) Then

                        If Not LCase(ns) = LCase(currNs) Then

                            'Can't extract root namespace!
                            Exit Sub

                        End If

                    Else

                        If Len(ns) > Len(currNs) Then

                            If Not LCase(Left(ns, Len(currNs) + 1)) = LCase(currNs) & "." Then

                                'Can't extract root namespace!
                                Exit Sub

                            End If

                        Else

                            If Not LCase(Left(currNs, Len(ns) + 1)) = LCase(ns) & "." Then

                                'Can't extract root namespace!
                                Exit Sub

                            Else

                                currNs = ns

                            End If

                        End If

                    End If

                End If

            Else

                'One class did not have any namespace, so no root namespace can be extracted.
                Exit Sub

            End If

        Next

        If Len(currNs) > 0 Then

            For Each classMap In targetDomainMap.ClassMaps

                classMap.Name = Right(classMap.Name, Len(classMap.Name) - Len(currNs) - 1)

            Next

            targetDomainMap.RootNamespace = currNs

        End If


    End Sub


    Public Overridable Overloads Sub AssemblyCodeToClasses(ByVal asm As System.Reflection.Assembly, ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap) Implements ICodeToClasses.AssemblyCodeToClasses

        Dim classMap As IClassMap
        Dim sourceClassMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim removeClassMaps As New ArrayList
        Dim removePropertyMaps As New ArrayList
        Dim removeClassPropertyMaps As New ArrayList
        Dim hasNew As Boolean

        AssemblyCodeToClasses(asm, targetDomainMap)

        If Len(targetDomainMap.RootNamespace) > 0 Then

            sourceDomainMap.RootNamespace = targetDomainMap.RootNamespace

        End If

        For Each classMap In targetDomainMap.ClassMaps

            sourceClassMap = sourceDomainMap.GetClassMap(classMap.Name)

            If Not sourceClassMap Is Nothing Then

                hasNew = False

                removeClassPropertyMaps.Clear()

                For Each propertyMap In classMap.PropertyMaps

                    If Not sourceClassMap.GetPropertyMap(propertyMap.Name) Is Nothing Then

                        removeClassPropertyMaps.Add(propertyMap)

                    Else

                        hasNew = True

                    End If

                Next

                If Not hasNew Then

                    removeClassMaps.Add(classMap)

                Else

                    For Each propertyMap In removeClassPropertyMaps

                        removePropertyMaps.Add(propertyMap)

                    Next


                End If

            End If

        Next

        For Each propertyMap In removePropertyMaps

            propertyMap.ClassMap.PropertyMaps.Remove(propertyMap)

        Next


        For Each classMap In removeClassMaps

            classMap.DomainMap.ClassMaps.Remove(classMap)

        Next

    End Sub

End Class
