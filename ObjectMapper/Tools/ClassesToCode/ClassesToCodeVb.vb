Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Text
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class ClassesToCodeVb
    Inherits ClassesToCodeBase

    Private m_DocCommentPrefix As String = "'''"

    Public Overrides Function DomainToCode(ByVal domainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal noRootNamespace As Boolean) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim ns As String

        'Root namespace
        codeBuilder.Append(NamespaceToCode(domainMap, "", noRootNamespace))
        codeBuilder.Append(vbCrLf)

        For Each ns In domainMap.GetNamespaces

            'namespace
            codeBuilder.Append(NamespaceToCode(domainMap, ns, noRootNamespace))
            codeBuilder.Append(vbCrLf)

        Next

        code = codeBuilder.ToString

        Return code

    End Function


    Public Overrides Function NamespaceToCode(ByVal domainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal name As String, ByVal noRootNamespace As Boolean) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim classMap As IClassMap
        Dim ns As String = name

        If Not noRootNamespace Then

            If Len(domainMap.RootNamespace) > 0 Then

                If Len(ns) > 0 Then

                    ns = domainMap.RootNamespace & "." & ns

                Else

                    ns = domainMap.RootNamespace

                End If

            End If

        End If

        'Namespace region
        AddRegion(codeBuilder, "Namespace '" & ns & "'")

        If Len(ns) > 0 Then

            'Namespace Declaration
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("Namespace ")
            codeBuilder.Append(ns)
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(vbCrLf)

        End If

        If Me.UseTypedCollections Then

            For Each classMap In domainMap.GetNamespaceClassMaps(name)

                If classMap.ClassType = ClassType.Class Or classMap.ClassType = ClassType.Default Then

                    'Class
                    codeBuilder.Append(ClassToTypedCollection(classMap, True, True))
                    codeBuilder.Append(vbCrLf)

                End If

            Next

        End If


        For Each classMap In domainMap.GetNamespaceClassMaps(name)

            'Class
            codeBuilder.Append(ClassToCode(classMap, True, True))
            codeBuilder.Append(vbCrLf)

        Next

        If Len(ns) > 0 Then

            'End Namespace
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("End Namespace")
            codeBuilder.Append(vbCrLf)

        End If

        'End namespace region
        AddEndRegion(codeBuilder, "Namespace '" & ns & "'")

        code = codeBuilder.ToString

        Return code

    End Function



    Public Overloads Overrides Function ClassToCode(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean, ByVal customCode As String) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim propertyMap As IPropertyMap
        Dim superClass As IClassMap = classMap.GetInheritedClassMap
        Dim ns As String

        Dim isShadow As Boolean

        Dim transientSuperClass As String
        Dim namespaceName As String
        Dim interfaces As ArrayList
        Dim interfaceName As String
        Dim found As Boolean

        Dim i As Long

        Dim convDataType As String

        'Import namespaces section
        found = False
        For Each namespaceName In classMap.GetImportsNamespaces

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("Imports ")
            codeBuilder.Append(namespaceName)
            codeBuilder.Append(vbCrLf)

            found = True

        Next

        If found Then codeBuilder.Append(vbCrLf)

        If Not noNamespace Then

            ns = classMap.GetNamespace

            If Not noRootNamespace Then

                If Len(classMap.DomainMap.RootNamespace) > 0 Then

                    If Len(ns) > 0 Then

                        ns = classMap.DomainMap.RootNamespace & "." & ns

                    Else

                        ns = classMap.DomainMap.RootNamespace

                    End If

                End If

            End If

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("Imports System" & vbCrLf)

            codeBuilder.Append(vbCrLf)

            'Namespace region
            AddRegion(codeBuilder, "Namespace '" & ns & "'")

            If Len(ns) > 0 Then

                'Namespace Declaration
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("Namespace ")
                codeBuilder.Append(ns)
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(vbCrLf)

            End If

        End If

        'Class region
        AddRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        'Class doc comments
        codeBuilder.Append(GetDocComment(classMap, DocCommentPrefix))

        'Class Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("Public Class ")
        codeBuilder.Append(classMap.GetName)
        codeBuilder.Append(vbCrLf)

        'Inheritance
        superClass = classMap.GetInheritedClassMap
        If Not superClass Is Nothing Then
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 1))
            codeBuilder.Append("Inherits ")
            codeBuilder.Append(superClass.GetFullName())
            codeBuilder.Append(vbCrLf)
        Else

            transientSuperClass = classMap.GetInheritsTransientClass

            If Len(transientSuperClass) > 0 Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 1))
                codeBuilder.Append("Inherits ")
                codeBuilder.Append(transientSuperClass)
                codeBuilder.Append(vbCrLf)

            End If

        End If

        If Not GeneratePOCO Then

            If superClass Is Nothing Then

                interfaces = classMap.GetImplementsInterfaces

                'Interface implementation declarations
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 1))

                If interfaces.Count > 0 Or ImplementIInterceptable Or ImplementIObjectHelper Or ImplementIObservable Then

                    codeBuilder.Append("Implements _")
                    codeBuilder.Append(vbCrLf)

                    If ImplementIInterceptable Then

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 2))
                        codeBuilder.Append("Puzzle.NPersist.Framework.IInterceptable, _" & vbCrLf)

                    End If

                    If ImplementIObjectHelper Then

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 2))
                        codeBuilder.Append("Puzzle.NPersist.Framework.IObjectHelper, _" & vbCrLf)

                    End If


                    'Additional interfaces section
                    For Each interfaceName In interfaces

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 2))
                        codeBuilder.Append(interfaceName & ", _" & vbCrLf)

                    Next

                    codeBuilder.Length = codeBuilder.Length - 5

                End If

            End If

        End If

        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(vbCrLf)

        'Generated Code region
        AddForcedRegion(codeBuilder, "Generated Code Region")


        'Private fields

        'Fields region
        AddRegion(codeBuilder, "Private Fields For Class '" & classMap.GetName & "'")

        AddComment(codeBuilder, "Private field variables", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
        codeBuilder.Append(vbCrLf)

        If Not GeneratePOCO Then

            If superClass Is Nothing Then

                AddComment(codeBuilder, "Holds reference to interceptor. Required for implementation of IInterceptable", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                codeBuilder.Append("Private m_Interceptor As Puzzle.NPersist.Framework.IInterceptor = Nothing")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append(vbCrLf)

            End If

        End If

        AddComment(codeBuilder, "Holds property values", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                'Field
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))

                Select Case propertyMap.FieldAccessibility
                    Case AccessibilityType.PublicAccess
                        codeBuilder.Append("Public ")

                    Case AccessibilityType.ProtectedAccess
                        codeBuilder.Append("Protected ")

                    Case AccessibilityType.InternalAccess
                        codeBuilder.Append("Friend ")

                    Case AccessibilityType.ProtectedInternalAccess
                        codeBuilder.Append("Protected Friend ")

                    Case AccessibilityType.PrivateAccess
                        codeBuilder.Append("Private ")
                End Select
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(" As ")

                If (propertyMap.ReferenceType = ReferenceType.ManyToMany Or propertyMap.ReferenceType = ReferenceType.ManyToOne) Then

                    If UseTypedCollections Then

                        convDataType = GetListType(propertyMap.GetReferencedClassMap, propertyMap.DataType)

                    ElseIf UseGenericCollections Then

                        convDataType = GetGenericListType(propertyMap.GetReferencedClassMap, propertyMap.DataType)

                    End If

                Else

                    If GeneratePOCO Then

                        If Len(propertyMap.DataType) > 0 Then

                            If InStr(propertyMap.DataType, "managedlist", CompareMethod.Text) > 0 Then

                                'codeBuilder.Append("System.Collections.ArrayList")
                                convDataType = "System.Collections.IList"

                            Else

                                If TargetPlatform = TargetPlatformEnum.NHibernate And propertyMap.IsCollection Then

                                    convDataType = "System.Collections.IList"

                                Else

                                    convDataType = propertyMap.DataType

                                End If

                            End If

                        End If

                    Else

                        convDataType = propertyMap.DataType

                    End If

                End If



                If TargetPlatform = TargetPlatformEnum.NHibernate And propertyMap.IsCollection Then

                    convDataType = "System.Collections.IList"

                End If

                codeBuilder.Append(convDataType)

                If propertyMap.IsCollection Then


                Else

                    If Len(propertyMap.DefaultValue) > 0 Then

                        codeBuilder.Append(" = ")

                        If IsStringProperty(propertyMap) Then

                            codeBuilder.Append("""" & propertyMap.DefaultValue & """")

                        Else

                            codeBuilder.Append(propertyMap.DefaultValue)

                        End If

                    End If

                End If

                codeBuilder.Append(vbCrLf)

            End If

        Next

        'End Fields region
        AddEndRegion(codeBuilder, "Private Fields For Class '" & classMap.GetName & "'")

        codeBuilder.Append(vbCrLf)

        'Properties region
        AddRegion(codeBuilder, "Public Properties For Class '" & classMap.GetName & "'")

        'Properties
        AddComment(codeBuilder, "Public properties", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            isShadow = classMap.IsShadowingProperty(propertyMap)

            If ImplementShadows OrElse isShadow = False Then

                'Property
                codeBuilder.Append(GetPropertyToCode(propertyMap, isShadow))
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'End Properties region
        AddEndRegion(codeBuilder, "Public Properties For Class '" & classMap.GetName & "'")

        codeBuilder.Append(vbCrLf)

        If Not GeneratePOCO Then

            If superClass Is Nothing Then

                If AddPropertyNotifyMethods Then

                    'Protected Methods

                    'Protected Methods region
                    AddRegion(codeBuilder, "Protected Methhods For Class '" & classMap.GetName & "'")

                    AddComment(codeBuilder, "Protected Methods", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
                    codeBuilder.Append(vbCrLf)

                    'Property Notification 

                    codeBuilder.Append(GetPropertyNotificationMethods(classMap))

                End If

            End If

        End If

        'Interface implementations

        If Not GeneratePOCO Then

            'Interfaces region
            AddRegion(codeBuilder, "Interfaces Implementations For Class '" & classMap.GetName & "'")

            AddComment(codeBuilder, "Interface implementations", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
            codeBuilder.Append(vbCrLf)

            'IInterceptable

            codeBuilder.Append(GetIInterceptable(classMap))


            'IObjectHelper
            codeBuilder.Append(GetIObjectHelper(classMap))

            'End Interfaces region
            AddEndRegion(codeBuilder, "Interfaces Implementations For Class '" & classMap.GetName & "'")

            codeBuilder.Append(vbCrLf)

        End If

        'End Generated Code region
        AddForcedEndRegion(codeBuilder, "Generated Code Region")

        codeBuilder.Append(vbCrLf)


        'Custom Code region
        AddForcedRegion(codeBuilder, "Synchronized Custom Code Region")

        Dim synchedCode As String = ""
        Dim codeMap As ICodeMap = classMap.GetCodeMap(CodeLanguage.VB)

        If Not codeMap Is Nothing Then

            synchedCode = codeMap.Code

        End If

        If Len(synchedCode) > 0 Then

            codeBuilder.Append(synchedCode)

        End If

        codeBuilder.Append(vbCrLf)

        AddComment(codeBuilder, "Add your custom code here:", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        'Custom Code region
        AddForcedRegion(codeBuilder, "Unsynchronized Custom Code Region")

        If Len(customCode) > 0 Then

            codeBuilder.Append(customCode)

        Else

            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)

        End If

        'End Custom Code region
        AddForcedEndRegion(codeBuilder, "Unsynchronized Custom Code Region")

        codeBuilder.Append(vbCrLf)

        'End Class
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("End Class")
        codeBuilder.Append(vbCrLf)

        'End Class region
        AddEndRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        If Not noNamespace Then

            If Len(ns) > 0 Then

                'End Namespace
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("End Namespace")
                codeBuilder.Append(vbCrLf)

            End If

            'End Namespace region
            AddEndRegion(codeBuilder, "Namespace '" & ns & "'")

        End If

        code = codeBuilder.ToString

        Return code

    End Function


    Public Overrides Function ClassToTypedCollection(ByVal classMap As IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean) As String

        Dim codeBuilder As New StringBuilder

        Dim ns As String
        Dim typeName As String = classMap.Name

        If Not noNamespace Then

            ns = classMap.GetNamespace

            If Not noRootNamespace Then

                If Len(classMap.DomainMap.RootNamespace) > 0 Then

                    If Len(ns) > 0 Then

                        ns = classMap.DomainMap.RootNamespace & "." & ns

                    Else

                        ns = classMap.DomainMap.RootNamespace

                    End If

                End If

            End If

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("Imports System" & vbCrLf)
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("Imports System.Collections" & vbCrLf)

            codeBuilder.Append(vbCrLf)

            If Len(ns) > 0 Then

                'Namespace Declaration
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("Namespace ")
                codeBuilder.Append(ns)
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(vbCrLf)

            End If

        End If

        'Class Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("Public Class ")
        codeBuilder.Append(classMap.GetName & "Collection")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, 1))
        codeBuilder.Append("Inherits CollectionBase" & vbCrLf)

        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Public Overridable Function Add(ByVal item As " & classMap.Name & ") As Integer" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Return List.Add(item)" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Function" & vbCrLf)
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Public Overridable Sub Remove(ByVal item As " & classMap.Name & ")" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("List.Remove(item)" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub" & vbCrLf)
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Public Overridable Function IndexOf(ByVal item As " & classMap.Name & ") As Integer" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Return List.IndexOf(item)" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Function" & vbCrLf)
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Default Public Overridable Property Item(ByVal index As Integer) As " & classMap.Name & vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Get" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
        codeBuilder.Append("Return CType(List(index), " & classMap.Name & ")" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("End Get" & vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Set(ByVal Value As " & classMap.Name & ")" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
        codeBuilder.Append("List(index) = Value" & vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("End Set" & vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Property" & vbCrLf)
        codeBuilder.Append(vbCrLf)

        'End Class
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("End Class")
        codeBuilder.Append(vbCrLf)

        If Not noNamespace Then

            If Len(ns) > 0 Then

                'End Namespace
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("End Namespace")
                codeBuilder.Append(vbCrLf)

            End If

        End If

        Return codeBuilder.ToString

    End Function


    Public Overrides Function PropertyToCode(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String

        Return GetPropertyToCode(propertyMap, False)

    End Function

    Public Overrides Function InheritedPropertyToCode(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String

        Return GetPropertyToCode(propertyMap, True)

    End Function

    Protected Overridable Function GetPropertyToCode(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap, ByVal isShadow As Boolean) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder
        Dim dataType As String
        Dim convDataType As String

        Dim makeReadOnly As Boolean
        Dim modifier As String

        If propertyMap.IsCollection Then
            dataType = propertyMap.ItemType
        Else
            dataType = propertyMap.DataType
        End If

        If Not propertyMap.GetColumnMap Is Nothing Then

            If propertyMap.GetColumnMap.IsAutoIncrease Then

                makeReadOnly = True

            End If

        End If

        convDataType = propertyMap.DataType

        If GeneratePOCO Then

            If InStr(LCase(convDataType), "managedlist") Then

                convDataType = "System.Collections.IList"

            End If

        End If

        If (propertyMap.ReferenceType = ReferenceType.ManyToMany Or propertyMap.ReferenceType = ReferenceType.ManyToOne) Then

            If UseTypedCollections Then

                convDataType = GetListType(propertyMap.GetReferencedClassMap, convDataType)

            ElseIf UseGenericCollections Then

                convDataType = GetGenericListType(propertyMap.GetReferencedClassMap, convDataType)

            End If

        End If


        If TargetPlatform = TargetPlatformEnum.NHibernate And propertyMap.IsCollection Then

            convDataType = "System.Collections.IList"

        End If

        'Property region
        AddRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

        'Property doc comments
        codeBuilder.Append(GetDocComment(propertyMap, DocCommentPrefix))

        'Property Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))

        If Not GeneratePOCO Then

            If propertyMap.IsCollection Then

                codeBuilder.Append("<Puzzle.NPersist.Framework.ListItemType(GetType(" & dataType & "))> ")

            End If

        End If

        Select Case propertyMap.Accessibility
            Case AccessibilityType.PublicAccess
                codeBuilder.Append("Public ")

            Case AccessibilityType.ProtectedAccess
                codeBuilder.Append("Protected ")

            Case AccessibilityType.InternalAccess
                codeBuilder.Append("Friend ")

            Case AccessibilityType.ProtectedInternalAccess
                codeBuilder.Append("Protected Friend ")

            Case AccessibilityType.PrivateAccess
                codeBuilder.Append("Private ")
        End Select

        modifier = GetModifier(propertyMap)

        If isShadow Then
            modifier = "Overrides"
            '    codeBuilder.Append("Overrides ")
            'Else
            '    codeBuilder.Append("Overridable ")
        End If

        codeBuilder.Append(modifier & " ")

        If makeReadOnly Then
            codeBuilder.Append("ReadOnly ")
        End If

        codeBuilder.Append("Property ")
        codeBuilder.Append(propertyMap.Name)
        codeBuilder.Append("() As ")
        codeBuilder.Append(convDataType)
        codeBuilder.Append(vbCrLf)

        'Property Get
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent))
        codeBuilder.Append("Get")
        codeBuilder.Append(vbCrLf)

        If GeneratePOCO Then

            'Return Value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
            codeBuilder.Append("Return ")
            codeBuilder.Append(propertyMap.GetFieldName)
            codeBuilder.Append(vbCrLf)

        Else

            'Notify Get region
            AddRegionAsComment(codeBuilder, "Notify Property Get '" & propertyMap.Name & "'")

            If Not NotifyOnlyWhenRequired OrElse (propertyMap.IsCollection Or propertyMap.ReferenceType <> ReferenceType.None Or propertyMap.LazyLoad Or Not propertyMap.GetTableMap Is propertyMap.ClassMap.GetTableMap) OrElse HasReferencesByIdentityProperties(propertyMap.ClassMap) Then

                If AddPropertyNotifyMethods Then

                    If Not NotifyLightWeight Then

                        'Declare return variable 
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim value As " & convDataType & " = " & propertyMap.GetFieldName)
                        codeBuilder.Append(vbCrLf)

                        'Declare cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim cancel As Boolean = False")
                        codeBuilder.Append(vbCrLf)

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyPropertyGet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", value, cancel)")
                        codeBuilder.Append(vbCrLf)

                        'Check cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If cancel Then Return Nothing")
                        codeBuilder.Append(vbCrLf)


                    Else

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyPropertyGet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""")")
                        codeBuilder.Append(vbCrLf)

                    End If


                Else

                    If Not NotifyLightWeight Then

                        'Declare return variable 
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim value As " & convDataType & " = " & propertyMap.GetFieldName)
                        codeBuilder.Append(vbCrLf)

                        'Declare cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim cancel As Boolean = False")
                        codeBuilder.Append(vbCrLf)

                    End If

                    'Notify interceptor manager
                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                    codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor")
                    codeBuilder.Append(vbCrLf)

                    If NotifyLightWeight Then

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertyGet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""")")
                        codeBuilder.Append(vbCrLf)

                    Else

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertyGet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", value, cancel)")
                        codeBuilder.Append(vbCrLf)

                        'Check cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If cancel Then Return Nothing")
                        codeBuilder.Append(vbCrLf)

                    End If

                End If

            End If

            'End Notify Get region
            AddEndRegionAsComment(codeBuilder, "Notify Property Get '" & propertyMap.Name & "'")

            'Notify After Get region
            AddRegionAsComment(codeBuilder, "Notify After Property Get '" & propertyMap.Name & "'")

            If Not NotifyOnlyWhenRequired Or (propertyMap.IsCollection Or propertyMap.ReferenceType <> ReferenceType.None Or propertyMap.LazyLoad Or Not propertyMap.GetTableMap Is propertyMap.ClassMap.GetTableMap) Then

                If NotifyAfterGet And Not NotifyLightWeight Then

                    If AddPropertyNotifyMethods Then

                        'Notify interceptor manager
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyAfterPropertyGet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", value)")
                        codeBuilder.Append(vbCrLf)

                    Else

                        'Notify interceptor manager
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyAfterPropertyGet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", value)")
                        codeBuilder.Append(vbCrLf)

                    End If

                End If


            End If

            'End Notify After Get region
            AddEndRegionAsComment(codeBuilder, "Notify After Property Get '" & propertyMap.Name & "'")

            If NotifyLightWeight Then

                'Return Value
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                codeBuilder.Append("Return ")
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(vbCrLf)

            Else

                'Return Value
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                codeBuilder.Append("Return value")
                codeBuilder.Append(vbCrLf)

            End If

        End If

        'End Property Get
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent))
        codeBuilder.Append("End Get")
        codeBuilder.Append(vbCrLf)

        If Not makeReadOnly Then

            'Property Set
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent))
            codeBuilder.Append("Set(ByVal Value As ")
            codeBuilder.Append(convDataType)
            codeBuilder.Append(")")
            codeBuilder.Append(vbCrLf)

            If GeneratePOCO Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(" = Value")
                codeBuilder.Append(vbCrLf)

            Else

                'Notify Set region
                AddRegionAsComment(codeBuilder, "Notify Property Set '" & propertyMap.Name & "'")

                If AddPropertyNotifyMethods Then

                    If Not NotifyLightWeight Then

                        'Declare cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim cancel As Boolean = False")
                        codeBuilder.Append(vbCrLf)

                    End If

                    If NotifyLightWeight Then

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyPropertySet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value)")
                        codeBuilder.Append(vbCrLf)

                    Else

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyPropertySet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value, cancel)")
                        codeBuilder.Append(vbCrLf)

                        'Check cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If cancel Then Return")
                        codeBuilder.Append(vbCrLf)

                    End If

                Else

                    If Not NotifyLightWeight Then

                        'Declare cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("Dim cancel As Boolean = False")
                        codeBuilder.Append(vbCrLf)

                    End If

                    'Notify interceptor manager
                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                    codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor")
                    codeBuilder.Append(vbCrLf)

                    If NotifyLightWeight Then

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertySet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value)")
                        codeBuilder.Append(vbCrLf)

                    Else

                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertySet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value, cancel)")
                        codeBuilder.Append(vbCrLf)

                        'Check cancel variable
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If cancel Then Return")
                        codeBuilder.Append(vbCrLf)

                    End If

                End If

                'End Notify Set region
                AddEndRegionAsComment(codeBuilder, "Notify Property Set '" & propertyMap.Name & "'")


                'Set Value
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(" = Value")
                codeBuilder.Append(vbCrLf)

                'Notify After Set region
                AddRegionAsComment(codeBuilder, "Notify After Property Set '" & propertyMap.Name & "'")

                If AddPropertyNotifyMethods Then

                    If NotifyAfterSet And Not NotifyLightWeight Then


                        'Notify interceptor manager
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("NotifyAfterPropertySet(""")
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value)")
                        codeBuilder.Append(vbCrLf)

                    End If

                Else

                    If NotifyAfterSet And Not NotifyLightWeight Then


                        'Notify interceptor manager
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent, 1))
                        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyAfterPropertySet(Me, """)
                        codeBuilder.Append(propertyMap.Name)
                        codeBuilder.Append(""", Value)")
                        codeBuilder.Append(vbCrLf)

                    End If

                End If

                'End Notify After Set region
                AddEndRegionAsComment(codeBuilder, "Notify After Property Set '" & propertyMap.Name & "'")

            End If


            'End Property Set
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.PropertyGetSetIndent))
            codeBuilder.Append("End Set")
            codeBuilder.Append(vbCrLf)

        End If

        'End Property
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Property")
        codeBuilder.Append(vbCrLf)

        'End Property region
        AddEndRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

        code = codeBuilder.ToString

        Return code

    End Function

    Protected Overridable Function GetPropertyNotificationMethods(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

        If Not AddPropertyNotifyMethods Then Return ""

        Dim code As String
        Dim codeBuilder As New StringBuilder


        'IInterceptable Interface region
        AddRegion(codeBuilder, "Property Notification Methods")

        AddComment(codeBuilder, "Property Notification Methods", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)



        'We should generate both overloads always, because someone may want to tailor this
        'manually in a subclass, meaning both methods have to be there...plus in the futute,
        'it could be an option to specify for each property what type of notification it should use.


        'Method region
        AddRegion(codeBuilder, "Property Notification Method 'NotifyPropertyGet'")

        'NotifyPropertyGet Method

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Overloads Sub NotifyPropertyGet(ByVal propertyName As String)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertyGet(Me, propertyName)")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)



        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Overloads Sub NotifyPropertyGet(ByVal propertyName As String, ByRef value As Object, ByRef cancel As Boolean)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertyGet(Me, propertyName, value, cancel)")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)


        'End Method region
        AddEndRegion(codeBuilder, "Property Notification Method 'NotifyPropertyGet'")

        codeBuilder.Append(vbCrLf)

        'NotifyAfterPropertyGet Method

        'Method region
        AddRegion(codeBuilder, "Property Notification Method 'NotifyAfterPropertyGet'")


        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Sub NotifyAfterPropertyGet(ByVal propertyName As String, ByVal value As Object)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyAfterPropertyGet(Me, propertyName, value)")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)


        'End Method region
        AddEndRegion(codeBuilder, "Property Notification Method 'NotifyAfterPropertyGet'")

        codeBuilder.Append(vbCrLf)



        'NotifyPropertySet Method

        'Method region
        AddRegion(codeBuilder, "Property Notification Method 'NotifyPropertySet'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Overloads Sub NotifyPropertySet(ByVal propertyName As String, ByRef value As Object)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertySet(Me, propertyName, value)")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)


        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Overloads Sub NotifyPropertySet(ByVal propertyName As String, ByRef value As Object, ByRef cancel As Boolean)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyPropertySet(Me, propertyName, value, cancel)")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)


        'End Method region
        AddEndRegion(codeBuilder, "Property Notification Method 'NotifyPropertySet'")

        codeBuilder.Append(vbCrLf)


        'NotifyAfterPropertySet Method

        'Method region
        AddRegion(codeBuilder, "Property Notification Method 'NotifyAfterPropertySet'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Protected Overloads Sub NotifyAfterPropertySet(ByVal propertyName As String, ByVal value As Object)")
        codeBuilder.Append(vbCrLf)


        'Get interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Dim interceptor As Puzzle.NPersist.Framework.IInterceptor = CType(Me, Puzzle.NPersist.Framework.IInterceptable).GetInterceptor()")
        codeBuilder.Append(vbCrLf)

        'Notify
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("If Not interceptor Is Nothing Then interceptor.NotifyAfterPropertySet(Me, propertyName, value)")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)


        'End Method region
        AddEndRegion(codeBuilder, "Property Notification Method 'NotifyAfterPropertySet'")

        codeBuilder.Append(vbCrLf)



        'End Property Notification Methods region
        AddEndRegion(codeBuilder, "Property Notification Methods")



        code = codeBuilder.ToString

        Return code

    End Function

    Protected Overridable Function GetIInterceptable(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

        If Not ImplementIInterceptable Then Return ""

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim propertyMap As IPropertyMap
        Dim superClass As IClassMap = classMap.GetInheritedClassMap

        If Not superClass Is Nothing Then Return ""

        'IInterceptable Interface region
        AddRegion(codeBuilder, "Interface Implementation 'IInterceptable'")

        AddComment(codeBuilder, "IInterceptable interface implementation", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        'Method region
        AddRegion(codeBuilder, "IInterceptable Method 'GetInterceptor'")

        'GetInterceptor Method

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Private Function IInterceptable_GetInterceptor() As Puzzle.NPersist.Framework.IInterceptor Implements Puzzle.NPersist.Framework.IInterceptable.GetInterceptor")
        codeBuilder.Append(vbCrLf)


        'Return interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Return m_Interceptor")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Function")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IInterceptable Method 'GetInterceptor'")

        codeBuilder.Append(vbCrLf)

        'SetInterceptor Method

        'Method region
        AddRegion(codeBuilder, "IInterceptable Method 'SetInterceptor'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("Private Sub IInterceptable_SetInterceptor(ByVal value As Puzzle.NPersist.Framework.IInterceptor) Implements Puzzle.NPersist.Framework.IInterceptable.SetInterceptor")
        codeBuilder.Append(vbCrLf)

        'Return interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("m_Interceptor = value")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IInterceptable Method 'SetInterceptor'")

        codeBuilder.Append(vbCrLf)

        'End IInterceptable Interface region
        AddEndRegion(codeBuilder, "Interface Implementation 'IInterceptable'")

        code = codeBuilder.ToString

        Return code

    End Function

    Protected Overridable Function GetIObjectHelper(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

        If Not ImplementIObjectHelper Then Return ""

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim propertyMap As IPropertyMap
        Dim superClass As IClassMap = classMap.GetInheritedClassMap

        'IObjectHelper Interface region
        AddRegion(codeBuilder, "Interface Implementation 'IObjectHelper'")

        AddComment(codeBuilder, "IObjectHelper interface implementation", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        'Method region
        AddRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))

        If superClass Is Nothing Then
            codeBuilder.Append("Protected Overridable Function IObjectHelper_GetPropertyValue(ByVal propertyName As String) As Object Implements Puzzle.NPersist.Framework.IObjectHelper.GetPropertyValue")
        Else
            codeBuilder.Append("Protected Overrides Function IObjectHelper_GetPropertyValue(ByVal propertyName As String) As Object")
        End If

        codeBuilder.Append(vbCrLf)

        'Select case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Select Case LCase(propertyName)")
        codeBuilder.Append(vbCrLf)


        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                'Case
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
                codeBuilder.Append("Case """)
                codeBuilder.Append(LCase(propertyMap.Name))
                codeBuilder.Append("""")
                codeBuilder.Append(vbCrLf)

                'Return value
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
                codeBuilder.Append("Return ")
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'Case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
        codeBuilder.Append("Case Else")
        codeBuilder.Append(vbCrLf)

        If Not superClass Is Nothing Then

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
            codeBuilder.Append("Return MyBase.IObjectHelper_GetPropertyValue(propertyName)")
            codeBuilder.Append(vbCrLf)

        Else

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
            codeBuilder.Append("Throw New Exception(""IObjectHelper Error: Property with name '"" & propertyName & ""' not found!"")")
            codeBuilder.Append(vbCrLf)

        End If

        'End Select case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("End Select")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Function")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")

        codeBuilder.Append(vbCrLf)

        'SetPropertyValue Method

        'Method region
        AddRegion(codeBuilder, "IObjectHelper Method 'SetPropertyValue'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        If superClass Is Nothing Then
            codeBuilder.Append("Protected Overridable Sub IObjectHelper_SetPropertyValue(ByVal propertyName As String, ByVal value As Object) Implements Puzzle.NPersist.Framework.IObjectHelper.SetPropertyValue")
        Else
            codeBuilder.Append("Protected Overrides Sub IObjectHelper_SetPropertyValue(ByVal propertyName As String, ByVal value As Object)")
        End If

        codeBuilder.Append(vbCrLf)


        'Select case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("Select Case LCase(propertyName)")
        codeBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                'Case
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
                codeBuilder.Append("Case """)
                codeBuilder.Append(LCase(propertyMap.Name))
                codeBuilder.Append("""")
                codeBuilder.Append(vbCrLf)

                'Set value
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(" = Value")
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'Case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
        codeBuilder.Append("Case Else")
        codeBuilder.Append(vbCrLf)

        If Not superClass Is Nothing Then

            'Set value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
            codeBuilder.Append("MyBase.IObjectHelper_SetPropertyValue(propertyName, value)")
            codeBuilder.Append(vbCrLf)

        Else

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
            codeBuilder.Append("Throw New Exception(""IObjectHelper Error: Property with name '"" & propertyName & ""' not found!"")")
            codeBuilder.Append(vbCrLf)

        End If

        'End Select case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
        codeBuilder.Append("End Select")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("End Sub")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IObjectHelper Method 'SetPropertyValue'")

        codeBuilder.Append(vbCrLf)

        'End IObjectHelper Interface region
        AddEndRegion(codeBuilder, "Interface Implementation 'IObjectHelper'")

        code = codeBuilder.ToString

        Return code

    End Function


    Protected Overridable Sub AddRegion(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If Not IncludeRegions Then Exit Sub
        codeBuilder.Append(GetRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Sub AddEndRegion(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If Not IncludeRegions Then Exit Sub
        codeBuilder.Append(GetEndRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub

    Protected Overridable Sub AddForcedRegion(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If ReallyNoRegions Then Exit Sub

        codeBuilder.Append(GetRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Sub AddForcedEndRegion(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If ReallyNoRegions Then Exit Sub

        codeBuilder.Append(GetEndRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub

    Protected Overridable Sub AddRegionAsComment(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If Not IncludeRegions Then Exit Sub
        codeBuilder.Append("'" & GetRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Sub AddEndRegionAsComment(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If Not IncludeRegions Then Exit Sub
        codeBuilder.Append("'" & GetEndRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub

    Protected Overridable Function GetRegion(ByVal name As String) As String

        'If Len(name) > 0 Then name = ": " & name

        Return "#Region "" " & name & " """

    End Function


    Protected Overridable Function GetEndRegion(ByVal name As String) As String

        'If Len(name) > 0 Then name = ": " & name

        Return "#End Region '" & name

    End Function



    Protected Overridable Sub AddComment(ByRef codeBuilder As StringBuilder, ByVal comment As String, ByVal IndentationLevel As Puzzle.ObjectMapper.Tools.IClassesToCode.IndentationLevelEnum, ByVal additionalLevel As Integer)

        If Not IncludeComments Then Exit Sub
        codeBuilder.Append(GetComment(comment, IndentationLevel, additionalLevel))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Function GetComment(ByVal comment As String, ByVal IndentationLevel As Puzzle.ObjectMapper.Tools.IClassesToCode.IndentationLevelEnum, ByVal additionalLevel As Integer) As String

        Return GetIndentation(IndentationLevel, additionalLevel) & "'" & comment

    End Function



    Public Overrides Function DomainToProject(ByVal domainMap As IDomainMap, ByVal projPath As String, ByVal classMapsAndFiles As Hashtable, ByVal embeddedFiles As ArrayList) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder
        Dim classMap As IClassMap

        'Begin
        codeBuilder.Append("<VisualStudioProject>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("    <VisualBasic")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        ProjectType = ""Local""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        ProductVersion = ""7.10.3077""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        SchemaVersion = ""2.0""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        ProjectGuid = ""{" & Guid.NewGuid.ToString & "}""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("    >")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        <Build>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            <Settings")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                ApplicationIcon = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                AssemblyKeyContainerName = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                AssemblyName = """ & domainMap.Name & """")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                AssemblyOriginatorKeyFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                AssemblyOriginatorKeyMode = ""None""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                DefaultClientScript = ""JScript""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                DefaultHTMLPageLayout = ""Grid""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                DefaultTargetSchema = ""IE50""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                DelaySign = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                OutputType = ""Library""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                OptionCompare = ""Binary""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                OptionExplicit = ""On""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                OptionStrict = ""Off""")
        codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                RootNamespace = """ & domainMap.RootNamespace & """")
        codeBuilder.Append("                RootNamespace = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                StartupObject = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            >")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Config")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""Debug""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BaseAddress = ""285212672""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    ConfigurationOverrideFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineConstants = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineDebug = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineTrace = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DebugSymbols = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    IncrementalBuild = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Optimize = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    OutputPath = ""bin\""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RegisterForComInterop = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RemoveIntegerChecks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    TreatWarningsAsErrors = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    WarningLevel = ""1""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Config")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""Release""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BaseAddress = ""285212672""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    ConfigurationOverrideFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineConstants = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineDebug = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineTrace = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DebugSymbols = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    IncrementalBuild = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Optimize = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    OutputPath = ""bin\""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RegisterForComInterop = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RemoveIntegerChecks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    TreatWarningsAsErrors = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    WarningLevel = ""1""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            </Settings>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            <References>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Reference")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""System""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    AssemblyName = ""System""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Reference")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""System.Data""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    AssemblyName = ""System.Data""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Reference")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""System.XML""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    AssemblyName = ""System.Xml""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)

        If TargetPlatform = TargetPlatformEnum.NPersist Then

            If Not GeneratePOCO Then

                codeBuilder.Append("                <Reference")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append("                    Name = ""Puzzle.NPersist.Framework""")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append("                    AssemblyName = ""Puzzle.NPersist.Framework""")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append("                    HintPath = ""bin/Puzzle.NPersist.Framework.dll""")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append("                />")
                codeBuilder.Append(vbCrLf)

            End If

        End If

        codeBuilder.Append("            </References>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            <Imports>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Import Namespace = ""Microsoft.VisualBasic"" />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Import Namespace = ""System"" />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Import Namespace = ""System.Collections"" />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Import Namespace = ""System.Data"" />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Import Namespace = ""System.Diagnostics"" />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            </Imports>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        </Build>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        <Files>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            <Include>")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append("                <File")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RelPath = ""AssemblyInfo.vb""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    SubType = ""Code""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BuildAction = ""Compile""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)

        Dim filePath As String
        For Each filePath In classMapsAndFiles.Values

            codeBuilder.Append("                <File")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    RelPath = """ & GetFilePathRelativeToProject(projPath, filePath) & """")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    SubType = ""Code""")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    BuildAction = ""Compile""")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                />")

        Next

        For Each filePath In embeddedFiles

            codeBuilder.Append("                <File")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    RelPath = """ & GetFilePathRelativeToProject(projPath, filePath) & """")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    BuildAction = ""EmbeddedResource""")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                />")

        Next

        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            </Include>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        </Files>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("    </VisualBasic>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("</VisualStudioProject>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)


        code = codeBuilder.ToString

        Return code

    End Function



    Public Overrides Function DomainToAssemblyInfo(ByVal domainMap As IDomainMap) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        codeBuilder.Append("Imports System.Reflection")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("Imports System.Runtime.InteropServices")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' General Information about an assembly is controlled through the following ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' set of attributes. Change these attribute values to modify the information")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' associated with an assembly.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' Review the values of the assembly attributes")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyTitle(""" & domainMap.Name & """)> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyDescription(""Persistent Domain Classes For Domain " & domainMap.Name & "."")> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyCompany("""")> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyProduct("""")> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyCopyright("""")> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyTrademark("""")> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: CLSCompliant(True)> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'The following GUID is for the ID of the typelib if this project is exposed to COM")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: Guid(""" & Guid.NewGuid.ToString & """)> ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' Version information for an assembly consists of the following four values:")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'      Major Version")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'      Minor Version ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'      Build Number")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'      Revision")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("'")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' You can specify all the values or you can default the Build and Revision Numbers ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("' by using the '*' as shown below:")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("<Assembly: AssemblyVersion(""1.0.*"")> ")
        codeBuilder.Append(vbCrLf)

        code = codeBuilder.ToString

        Return code

    End Function

    Public Overrides Property DocCommentPrefix() As String
        Get
            Return m_DocCommentPrefix
        End Get
        Set(ByVal Value As String)
            m_DocCommentPrefix = Value
        End Set
    End Property


    Public Function GetModifier(ByVal propertyMap As IPropertyMap) As String

        Dim modifier As String

        Select Case propertyMap.PropertyModifier

            Case PropertyModifier.Abstract

                modifier = "MustOverride"

            Case PropertyModifier.Default

                Select Case TargetPlatform

                    Case TargetPlatformEnum.NPersist

                        modifier = "Overridable"

                    Case TargetPlatformEnum.NHibernate

                        modifier = ""

                    Case TargetPlatformEnum.POCO

                        modifier = ""

                End Select

            Case PropertyModifier.Override

                modifier = "Overrides"

            Case PropertyModifier.None

                modifier = ""

            Case PropertyModifier.Virtual

                modifier = "Overridable"

        End Select

        Return modifier

    End Function


    Protected Overrides Function GetGenericListType(ByVal refClassMap As ClassMap, ByVal defaultName As String) As String

        If UseGenericCollections Then

            If Not refClassMap Is Nothing Then

                Return "System.Collections.Generic.IList(Of " & refClassMap.Name & ")"

            End If

        End If

        Return defaultName

    End Function

End Class
