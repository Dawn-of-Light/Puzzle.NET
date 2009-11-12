Imports Puzzle.ObjectMapper.Tools
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.ObjectMapper.GUI.Uml
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class MapServices

#Region " Create and delete methods "


    Public Shared Sub DeleteNamespace(ByVal domainMap As IDomainMap, ByVal namespaceToDelete As String)

        Dim classMap As IClassMap
        Dim classMaps As New ArrayList()

        Dim arrName() As String

        domainMap.Dirty = True

        Dim level As Long = GetNamespaceLevel(namespaceToDelete)
        Dim path As String = namespaceToDelete

        'We must copy the arraylist since the original will be modified
        'in the iteration (items will be removed)
        For Each classMap In domainMap.ClassMaps
            classMaps.Add(classMap)
        Next

        LogServices.LogMsg("Deleting all classes in namespace '" & namespaceToDelete & "':", TraceLevel.Info)

        For Each classMap In classMaps

            arrName = Split(classMap.Name, ".")

            If UBound(arrName) > level Then

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, level) Then

                        DeleteClassMap(classMap)

                    End If

                End If

            ElseIf UBound(arrName) < level Then

            Else    ' = level

                arrName = Split(classMap.Name, ".")

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, -1) Then

                        DeleteClassMap(classMap)

                    End If

                End If

            End If

        Next

        LogServices.LogMsg("Namespace '" & namespaceToDelete & "' deleted.", TraceLevel.Info)

    End Sub


    Public Shared Function GetNamespaceLevel(ByVal namespaceName As String) As Long

        Return UBound(Split(namespaceName, ".")) + 1

    End Function


    Public Shared Function ComparePaths(ByVal path1 As String, ByVal path2 As String, ByVal level As Long) As Boolean

        path1 = LCase(path1)
        path2 = LCase(path2)

        Dim i As Long

        Dim arrPath1() As String = Split(path1, ".")
        Dim arrPath2() As String = Split(path2, ".")

        If level = -1 Then level = UBound(arrPath1)

        For i = 0 To level - 1
            If Not arrPath1(i) = arrPath2(i) Then Return False
        Next

        Return True

    End Function

    Public Shared Function CreateClassMap(ByVal name As String, ByVal domainMap As IDomainMap) As IClassMap

        Dim newClassMap As IClassMap = New ClassMap()

        domainMap.Dirty = True

        newClassMap.Name = name

        newClassMap.DomainMap = domainMap

        LogServices.LogMsg("Class '" & name & "' created.", TraceLevel.Info)

        Return newClassMap

    End Function

    Public Shared Sub DeleteClassMap(ByVal classMap As IClassMap)

        classMap.DomainMap.Dirty = True

        LogServices.LogMsg("Class '" & classMap.Name & "' deleted.", TraceLevel.Info)

        LogServices.mainForm.m_Project.DeletingClassMap(classMap)

        classMap.DomainMap.ClassMaps.Remove(classMap)

    End Sub

    Public Shared Function CreatePropertyMap(ByVal name As String, ByVal classMap As IClassMap) As IPropertyMap

        classMap.DomainMap.Dirty = True

        Dim newPropertyMap As IPropertyMap = New PropertyMap()

        newPropertyMap.Name = name

        newPropertyMap.ClassMap = classMap

        LogServices.LogMsg("Property '" & name & "' created.", TraceLevel.Info)

        Return newPropertyMap

    End Function


    Public Shared Sub DeletePropertyMap(ByVal propertyMap As IPropertyMap)

        propertyMap.ClassMap.DomainMap.Dirty = True

        LogServices.LogMsg("Property '" & propertyMap.Name & "' deleted.", TraceLevel.Info)

        propertyMap.ClassMap.PropertyMaps.Remove(propertyMap)

    End Sub


    Public Shared Function CreateEnumValueMap(ByVal name As String, ByVal classMap As IClassMap) As IEnumValueMap

        Dim newIndex As Integer = GetNextFreeEnumValueIndex(classMap)

        classMap.DomainMap.Dirty = True

        Dim newEnumValueMap As IEnumValueMap = New EnumValueMap

        newEnumValueMap.Name = name

        newEnumValueMap.ClassMap = classMap

        newEnumValueMap.Index = newIndex

        LogServices.LogMsg("EnumValue '" & name & "' created.", TraceLevel.Info)

        Return newEnumValueMap

    End Function

    Public Shared Function GetNextFreeEnumValueIndex(ByVal classMap As IClassMap) As Integer

        Dim highest As Integer = -1

        For Each enumValue As IEnumValueMap In classMap.EnumValueMaps

            If enumValue.Index > highest Then

                highest = enumValue.Index

            End If

        Next

        Return highest + 1

    End Function

    Public Shared Sub DeleteEnumValueMap(ByVal enumValueMap As IEnumValueMap)

        enumValueMap.ClassMap.DomainMap.Dirty = True

        LogServices.LogMsg("EnumValue '" & enumValueMap.Name & "' deleted.", TraceLevel.Info)

        enumValueMap.ClassMap.EnumValueMaps.Remove(enumValueMap)

    End Sub




    Public Shared Function CreateSourceMap(ByVal name As String, ByVal domainMap As IDomainMap) As ISourceMap

        domainMap.Dirty = True

        Dim newSourceMap As ISourceMap = New SourceMap

        newSourceMap.Name = name

        newSourceMap.DomainMap = domainMap

        LogServices.LogMsg("Source '" & name & "' created.", TraceLevel.Info)

        Return newSourceMap

    End Function


    Public Shared Sub DeleteSourceMap(ByVal sourceMap As ISourceMap)

        sourceMap.DomainMap.Dirty = True

        LogServices.LogMsg("Source '" & sourceMap.Name & "' deleted.", TraceLevel.Info)

        sourceMap.DomainMap.SourceMaps.Remove(sourceMap)

    End Sub


    Public Shared Function CreateTableMap(ByVal name As String, ByVal sourceMap As ISourceMap) As ITableMap

        sourceMap.DomainMap.Dirty = True

        Dim newTableMap As ITableMap = New TableMap

        newTableMap.Name = name

        newTableMap.SourceMap = sourceMap

        LogServices.LogMsg("Table '" & name & "' created.", TraceLevel.Info)

        Return newTableMap

    End Function


    Public Shared Sub DeleteTableMap(ByVal tableMap As ITableMap)

        tableMap.SourceMap.DomainMap.Dirty = True

        LogServices.LogMsg("Table '" & tableMap.Name & "' deleted.", TraceLevel.Info)

        tableMap.SourceMap.TableMaps.Remove(tableMap)

    End Sub


    Public Shared Function CreateColumnMap(ByVal name As String, ByVal tableMap As ITableMap) As IColumnMap

        tableMap.SourceMap.DomainMap.Dirty = True

        Dim newColumnMap As IColumnMap = New ColumnMap

        newColumnMap.Name = name
        newColumnMap.DataType = DbType.AnsiStringFixedLength
        newColumnMap.Length = 255
        newColumnMap.Precision = 255
        newColumnMap.AllowNulls = True

        newColumnMap.TableMap = tableMap

        LogServices.LogMsg("Column '" & name & "' created.", TraceLevel.Info)

        Return newColumnMap

    End Function


    Public Shared Sub DeleteColumnMap(ByVal columnMap As IColumnMap)

        columnMap.TableMap.SourceMap.DomainMap.Dirty = True

        LogServices.LogMsg("Column '" & columnMap.Name & "' deleted.", TraceLevel.Info)

        columnMap.TableMap.ColumnMaps.Remove(columnMap)

    End Sub

#End Region

    Public Overloads Shared Sub SetSuperClass(ByVal classMap As IClassMap)

        Dim superClassMap As IClassMap = classMap.GetInheritedClassMap

        SetSuperClass(classMap, superClassMap)

    End Sub

    Public Overloads Shared Sub SetSuperClass(ByVal classMap As IClassMap, ByVal superClassMap As IClassMap)

        If superClassMap Is Nothing Then
            classMap.InheritsClass = ""
            If classMap.GetSubClassMaps.Count < 1 Then
                classMap.InheritanceType = InheritanceType.None
                classMap.TypeValue = ""
                classMap.TypeColumn = ""
            End If
        Else

            If Not classMap.IsLegalAsSuperClass(superClassMap) Then

                Exit Sub

            Else

                classMap.InheritsClass = superClassMap.Name

            End If

            If Not classMap.GetInheritedClassMap Is Nothing Then
                If classMap.GetInheritedClassMap.InheritanceType = InheritanceType.None Then
                    classMap.GetInheritedClassMap.InheritanceType = InheritanceType.SingleTableInheritance
                End If
                If classMap.GetInheritedClassMap.TypeValue = "" Then
                    classMap.GetInheritedClassMap.TypeValue = UCase(Left(classMap.GetInheritedClassMap.GetName, 1))
                End If
            End If
            If classMap.InheritanceType = InheritanceType.None Then
                If Not classMap.GetInheritedClassMap Is Nothing Then
                    classMap.InheritanceType = classMap.GetInheritedClassMap.InheritanceType
                End If
                If classMap.InheritanceType = InheritanceType.None Then
                    classMap.InheritanceType = InheritanceType.SingleTableInheritance
                End If
                'If classMap.InheritanceType = InheritanceType.SingleTableInheritance Then
                If classMap.TypeValue = "" Then
                    classMap.TypeValue = UCase(Left(classMap.GetName, 1))
                End If
                'End If
            End If
            If classMap.InheritanceType = InheritanceType.ConcreteTableInheritance Then
                RemoveShadowProperties(classMap, True)
                AddShadowProperties(classMap)
            Else
                RemoveShadowProperties(classMap)
            End If
        End If

    End Sub

    Public Shared Sub RemoveShadowProperties(ByVal classMap As IClassMap, Optional ByVal justIdProperties As Boolean = False, Optional ByVal useMsg As String = "", Optional ByVal justPropertyMap As IPropertyMap = Nothing)

        Dim propertyMap As IPropertyMap
        Dim inheritedNames As New Hashtable
        Dim nonInheritedPropertyMaps As ArrayList
        Dim removePropertyMaps As New ArrayList
        Dim superClassMap As IClassMap = classMap.GetInheritedClassMap
        Dim msg As String

        If Not superClassMap Is Nothing Then

            For Each propertyMap In superClassMap.GetAllPropertyMaps

                inheritedNames(LCase(propertyMap.Name)) = True

            Next

            nonInheritedPropertyMaps = classMap.GetNonInheritedPropertyMaps

            For Each propertyMap In nonInheritedPropertyMaps

                If inheritedNames.ContainsKey(LCase(propertyMap.Name)) Then

                    If justPropertyMap Is Nothing OrElse justPropertyMap Is propertyMap Then

                        If justIdProperties = False OrElse propertyMap.IsIdentity Then

                            removePropertyMaps.Add(propertyMap)

                        End If

                    End If

                End If

            Next

            If removePropertyMaps.Count > 0 Then

                If Len(useMsg) > 0 Then

                    msg = useMsg

                Else

                    If justIdProperties Then

                        msg = "Would you like to remove the shadow identity properties for the class '" & classMap.Name & "'? (Recommended)"

                    Else

                        msg = "Would you like to remove the shadow properties for the class '" & classMap.Name & "'? (Recommended)"

                    End If

                    msg += vbCrLf & vbCrLf & "The following shadow properties would be removed, and would instead be inherited from the superclass '" & superClassMap.Name & "':" & vbCrLf & vbCrLf

                End If

                For Each propertyMap In removePropertyMaps

                    msg += "'" & propertyMap.Name & "'" & vbCrLf

                Next


                If MsgBox(msg, MsgBoxStyle.YesNo, "Remove Shadow Properties") = MsgBoxResult.Yes Then

                    For Each propertyMap In removePropertyMaps

                        classMap.PropertyMaps.Remove(propertyMap)

                    Next

                End If

            Else

                If Len(useMsg) > 0 Then

                    MsgBox("No shadow properties to remove could be found!")

                End If

            End If

        End If

    End Sub

    Public Shared Sub AddShadowProperties(ByVal classMap As IClassMap, Optional ByVal useMsg As String = "", Optional ByVal justPropertyMap As IPropertyMap = Nothing)

        Dim propertyMap As IPropertyMap
        Dim propertyMapClone As IPropertyMap
        Dim existingNames As New Hashtable
        Dim inheritedPropertyMaps As ArrayList
        Dim addPropertyMaps As New ArrayList
        Dim superClassMap As IClassMap = classMap.GetInheritedClassMap
        Dim msg As String

        If Not superClassMap Is Nothing Then

            For Each propertyMap In classMap.GetNonInheritedPropertyMaps

                existingNames(LCase(propertyMap.Name)) = True

            Next

            inheritedPropertyMaps = classMap.GetInheritedPropertyMaps

            For Each propertyMap In inheritedPropertyMaps

                If Not existingNames.ContainsKey(LCase(propertyMap.Name)) Then

                    If justPropertyMap Is Nothing OrElse justPropertyMap Is propertyMap Then

                        If Not propertyMap.IsIdentity And Not propertyMap.IsCollection Then

                            addPropertyMaps.Add(propertyMap)

                        End If

                    End If

                End If

            Next

            If addPropertyMaps.Count > 0 Then

                If Len(useMsg) > 0 Then

                    msg = useMsg

                Else

                    msg = "Would you like to add shadow properties for the class '" & classMap.Name & "'? (Recommended)"

                    msg += vbCrLf & vbCrLf & "The following shadow properties would be added, and would thereby shadow properties inherited by the superclass '" & superClassMap.Name & "':" & vbCrLf & vbCrLf

                End If

                For Each propertyMap In addPropertyMaps

                    msg += "'" & propertyMap.Name & "'" & vbCrLf

                Next

                If MsgBox(msg, MsgBoxStyle.YesNo, "Add Shadow Properties") = MsgBoxResult.Yes Then

                    For Each propertyMap In addPropertyMaps

                        propertyMapClone = propertyMap.Clone

                        propertyMapClone.ClassMap = classMap

                    Next

                End If

            Else

                If Len(useMsg) > 0 Then

                    MsgBox("No shadow properties to add could be found!")

                End If

            End If

        End If

    End Sub


    Public Shared Sub MargeDomains(ByVal targetDomainMap As IDomainMap, ByVal sourceDomainMap As IDomainMap)

        DomainMerger.MergeDomains(targetDomainMap, sourceDomainMap)

    End Sub

    Public Shared Sub DeleteUmlDiagram(ByVal umlDiagram As UmlDiagram)

        LogServices.LogMsg("Diagram '" & umlDiagram.Name & "' deleted.", TraceLevel.Info)

        umlDiagram.OwnerResource.Diagrams.Remove(umlDiagram)

    End Sub

End Class
