Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Text
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class ClassesToCodeDelphi
    Inherits ClassesToCodeBase

    Private m_DocCommentPrefix As String = "///"

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
        AddRegion(codeBuilder, "unit '" & ns & "'")

        If Len(ns) > 0 Then

            'Namespace Declaration
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("unit ")
            codeBuilder.Append(ns)
            codeBuilder.Append(";")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)

        End If

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("interface")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(vbCrLf)

        'using System;
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("uses")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("System;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(vbCrLf)

        codeBuilder.Append("type")
        codeBuilder.Append(vbCrLf)

        For Each classMap In domainMap.GetNamespaceClassMaps(name)

            'Class
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append(classMap.Name & " = class;")
            codeBuilder.Append(vbCrLf)

        Next

        codeBuilder.Append(vbCrLf)

        For Each classMap In domainMap.GetNamespaceClassMaps(name)

            'Class
            codeBuilder.Append(ClassToInterface(classMap, True, True))
            codeBuilder.Append(vbCrLf)

        Next

        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("implementation")
        codeBuilder.Append(vbCrLf)


        For Each classMap In domainMap.GetNamespaceClassMaps(name)

            'Class
            codeBuilder.Append(ClassToCode(classMap, True, True))
            codeBuilder.Append(vbCrLf)

        Next

        If Len(ns) > 0 Then

            'End Namespace
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("end.")
            codeBuilder.Append(vbCrLf)

        End If

        'End namespace region
        AddEndRegion(codeBuilder, "Unit '" & ns & "'")

        code = codeBuilder.ToString

        Return code

    End Function




    Public Overloads Function ClassToInterface(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

        Return ClassToInterface(classMap, False, False, "")

    End Function

    Public Overloads Function ClassToInterface(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean) As String

        Return ClassToInterface(classMap, noNamespace, noRootNamespace, "")

    End Function

    Public Overloads Function ClassToInterface(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean, ByVal customCode As String) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim propertyMap As IPropertyMap
        Dim superClass As IClassMap = classMap.GetInheritedClassMap
        Dim ns As String
        Dim implSep As String = " : "
        Dim convDataType As String

        Dim isShadow As Boolean

        'Generated Code region
        AddForcedRegion(codeBuilder, "Generated Interface Region")


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

            'Namespace region
            AddRegion(codeBuilder, "Namespace '" & ns & "'")

            If Len(ns) > 0 Then

                'Namespace Declaration
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("unit ")
                codeBuilder.Append(ns & ";")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append(vbCrLf)

            End If

            'using System;
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("uses")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("System;")
            codeBuilder.Append(vbCrLf)

        End If

        'Class region
        AddRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        'Class doc comments
        codeBuilder.Append(GetDocComment(classMap, DocCommentPrefix))


        'Class Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append(classMap.GetName)
        codeBuilder.Append(" = class(")

        'Inheritance
        superClass = classMap.GetInheritedClassMap
        If Not superClass Is Nothing Then
            codeBuilder.Append(implSep)
            codeBuilder.Append(superClass.GetFullName())
            implSep = ", "
        Else
            codeBuilder.Append(implSep)
            codeBuilder.Append("System.Object")
            implSep = ", "
        End If

        If superClass Is Nothing Then

            'Interface implementation declarations
            codeBuilder.Append(implSep)

            If ImplementIInterceptable Then

                codeBuilder.Append("Puzzle.NPersist.Framework.IInterceptable")

                If ImplementIObjectHelper Then

                    codeBuilder.Append(", ")

                End If

            End If

            If ImplementIObjectHelper Then

                codeBuilder.Append("Puzzle.NPersist.Framework.IObjectHelper")

            End If

        End If

        codeBuilder.Append(")")
        codeBuilder.Append(vbCrLf)

        'Private fields

        'Fields region
        AddRegion(codeBuilder, "Private Fields For Class '" & classMap.GetName & "'")

        AddComment(codeBuilder, "Private field variables", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
        codeBuilder.Append(vbCrLf)

        If superClass Is Nothing Then

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("private")
            codeBuilder.Append(vbCrLf)

            AddComment(codeBuilder, "Holds reference to interceptor manager. Required for implementation of IInterceptable", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
            codeBuilder.Append("m_Interceptor: Puzzle.NPersist.Framework.IInterceptor = nil;")

        End If

        AddComment(codeBuilder, "Holds property values", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                convDataType = propertyMap.DataType

                'Field
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                Select Case propertyMap.FieldAccessibility
                    Case AccessibilityType.PublicAccess
                        codeBuilder.Append("public")

                    Case AccessibilityType.ProtectedAccess
                        codeBuilder.Append("protected")

                    Case AccessibilityType.InternalAccess
                        codeBuilder.Append("strict private")

                    Case AccessibilityType.ProtectedInternalAccess
                        codeBuilder.Append("strict protected")

                    Case AccessibilityType.PrivateAccess
                        codeBuilder.Append("private")
                End Select

                codeBuilder.Append(propertyMap.GetFieldName)
                codeBuilder.Append(": ")
                codeBuilder.Append(convDataType)

                codeBuilder.Append(";")
                codeBuilder.Append(vbCrLf)


            End If

        Next

        'End Fields region
        AddEndRegion(codeBuilder, "Private Fields For Class '" & classMap.GetName & "'")

        codeBuilder.Append(vbCrLf)

        'Constructor region
        AddRegion(codeBuilder, "Constructor For Class '" & classMap.GetName & "'")

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("public")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
        codeBuilder.Append("constructor Create;")
        codeBuilder.Append(vbCrLf)

        'End Constructor region
        AddEndRegion(codeBuilder, "Constructor For Class '" & classMap.GetName & "'")

        'Properties region
        AddRegion(codeBuilder, "Public Properties For Class '" & classMap.GetName & "'")

        'Properties
        AddComment(codeBuilder, "Public properties", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            isShadow = classMap.IsShadowingProperty(propertyMap)

            If ImplementShadows OrElse isShadow = False Then

                'Property
                codeBuilder.Append(GetPropertyToInterface(propertyMap, isShadow))
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'End Properties region
        AddEndRegion(codeBuilder, "Public Properties For Class '" & classMap.GetName & "'")

        'Interface implementations

        'Interfaces region
        AddRegion(codeBuilder, "Interfaces Implementations For Class '" & classMap.GetName & "'")

        AddComment(codeBuilder, "Interface implementations", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
        codeBuilder.Append(vbCrLf)

        'IInterceptable

        'codeBuilder.Append(GetIInterceptableInterface(classMap))


        ''IObjectHelper
        'codeBuilder.Append(GetIObjectHelperInterface(classMap))

        'End Interfaces region
        AddEndRegion(codeBuilder, "Interfaces Implementations For Class '" & classMap.GetName & "'")

        codeBuilder.Append(vbCrLf)

        'End Generated Code region
        AddForcedEndRegion(codeBuilder, "Generated Interface Region")

        codeBuilder.Append(vbCrLf)

        'Custom Code region
        AddForcedRegion(codeBuilder, "Custom Interface Region")

        If Len(customCode) > 0 Then

            codeBuilder.Append(customCode)

        Else

            AddComment(codeBuilder, "Add your custom interface declarations here:", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)

        End If

        'End Custom Code region
        AddForcedEndRegion(codeBuilder, "Custom Interface Region")

        codeBuilder.Append(vbCrLf)

        'End Class
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        'End Class region
        AddEndRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        If Not noNamespace Then

            If Len(ns) > 0 Then

                'End Namespace
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("end;")
                codeBuilder.Append(vbCrLf)

            End If

            'End Namespace region
            AddEndRegion(codeBuilder, "Namespace '" & ns & "'")

        End If

        code = codeBuilder.ToString

        Return code

    End Function

    Protected Overridable Function PropertyToInterface(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String

        Return GetPropertyToInterface(propertyMap, False)

    End Function

    Protected Overridable Function InheritedPropertyToInterface(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String

        Return GetPropertyToInterface(propertyMap, True)

    End Function

    Protected Overridable Function GetPropertyToInterface(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap, ByVal isShadow As Boolean) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder
        Dim dataType As String
        Dim convDataType As String
        Dim makeReadOnly As Boolean

        If propertyMap.IsCollection Then
            dataType = propertyMap.ItemType
        Else
            dataType = propertyMap.DataType
        End If

        convDataType = propertyMap.DataType


        If Not propertyMap.GetColumnMap Is Nothing Then

            If propertyMap.GetColumnMap.IsAutoIncrease Then

                makeReadOnly = True

            End If

        End If

        'Property region
        AddRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

        'Property doc comments
        codeBuilder.Append(GetDocComment(propertyMap, DocCommentPrefix))

        'Property Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))


        codeBuilder.Append("function get_")
        codeBuilder.Append(propertyMap.Name)
        codeBuilder.Append(": ")
        codeBuilder.Append(convDataType)
        codeBuilder.Append(";")

        If isShadow Then
            codeBuilder.Append(" override;")
        Else
            codeBuilder.Append(" virtual;")
        End If

        codeBuilder.Append(vbCrLf)

        If Not makeReadOnly Then

            codeBuilder.Append("procedure set_")
            codeBuilder.Append(propertyMap.Name)
            codeBuilder.Append("(value: ")
            codeBuilder.Append(convDataType)
            codeBuilder.Append(");")

            If isShadow Then
                codeBuilder.Append(" override;")
            Else
                codeBuilder.Append(" virtual;")
            End If

            codeBuilder.Append(vbCrLf)

        End If

        If propertyMap.IsCollection Then

            codeBuilder.Append("[Puzzle.NPersist.Framework.ListItemType(typeof(" & dataType & "))]")
            codeBuilder.Append(vbCrLf)

        End If

        codeBuilder.Append("property ")
        codeBuilder.Append(propertyMap.Name)
        codeBuilder.Append(": ")
        codeBuilder.Append(convDataType)
        codeBuilder.Append(" read get_" & propertyMap.Name)

        If Not makeReadOnly Then

            codeBuilder.Append(" write set_" & propertyMap.Name)

        End If

        codeBuilder.Append(";")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(vbCrLf)

        'End Property region
        AddEndRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

        code = codeBuilder.ToString

        Return code

    End Function

    'Protected Overridable Function GetIInterceptableInterface(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

    '    Dim code As String
    '    Dim codeBuilder As New StringBuilder

    '    Dim propertyMap As IPropertyMap
    '    Dim superClass As IClassMap = classMap.GetInheritedClassMap

    '    If Not superClass Is Nothing Then Return ""

    '    'IInterceptable Interface region
    '    AddRegion(codeBuilder, "Interface Implementation 'IInterceptable'")

    '    AddComment(codeBuilder, "IInterceptable interface implementation", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

    '    'Method region
    '    AddRegion(codeBuilder, "IInterceptable Method 'GetInterceptor'")

    '    'GetInterceptor Method

    '    'Method Declaration
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("function GetInterceptor()")
    '    codeBuilder.Append(vbCrLf)
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)


    '    'Return interceptor manager
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("return m_Interceptor;")
    '    codeBuilder.Append(vbCrLf)


    '    'End Method
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method region
    '    AddEndRegion(codeBuilder, "IInterceptable Method 'GetInterceptor'")

    '    codeBuilder.Append(vbCrLf)

    '    'SetInterceptor Method

    '    'Method region
    '    AddRegion(codeBuilder, "IInterceptable Method 'SetInterceptor'")

    '    'Method Declaration
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("void Puzzle.NPersist.Framework.IInterceptable.SetInterceptor(Puzzle.NPersist.Framework.IInterceptor value)")
    '    codeBuilder.Append(vbCrLf)
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)

    '    'Return interceptor manager
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("m_Interceptor = value;")
    '    codeBuilder.Append(vbCrLf)


    '    'End Method
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method region
    '    AddEndRegion(codeBuilder, "IInterceptable Method 'SetInterceptor'")

    '    codeBuilder.Append(vbCrLf)

    '    'End IInterceptable Interface region
    '    AddEndRegion(codeBuilder, "Interface Implementation 'IInterceptable'")

    '    code = codeBuilder.ToString

    '    Return code

    'End Function

    'Protected Overridable Function GetIObjectHelperInterface(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap) As String

    '    Dim code As String
    '    Dim codeBuilder As New StringBuilder

    '    Dim propertyMap As IPropertyMap
    '    Dim superClass As IClassMap = classMap.GetInheritedClassMap
    '    Dim convDataType As String

    '    'IObjectHelper Interface region
    '    AddRegion(codeBuilder, "Interface Implementation 'IObjectHelper'")

    '    AddComment(codeBuilder, "IObjectHelper interface implementation", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

    '    'Method region
    '    AddRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")


    '    'codeBuilder.Append("protected virtual object IObjectHelper_GetPropertyValue(string propertyName) As Object Implements Puzzle.NPersist.Framework.IObjectHelper.GetPropertyValue")

    '    If superClass Is Nothing Then
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("object Puzzle.NPersist.Framework.IObjectHelper.GetPropertyValue(string propertyName)")
    '        codeBuilder.Append(vbCrLf)
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("{")
    '        codeBuilder.Append(vbCrLf)

    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '        codeBuilder.Append("return this.IObjectHelper_GetPropertyValue(propertyName);")
    '        codeBuilder.Append(vbCrLf)

    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("}")
    '        codeBuilder.Append(vbCrLf)

    '    End If

    '    'Method Declaration
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))

    '    If superClass Is Nothing Then
    '        codeBuilder.Append("protected virtual object IObjectHelper_GetPropertyValue(string propertyName)")
    '    Else
    '        codeBuilder.Append("protected override object IObjectHelper_GetPropertyValue(ByVal propertyName As String) As Object")
    '    End If

    '    codeBuilder.Append(vbCrLf)
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)

    '    'Select case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("switch(propertyName)")
    '    codeBuilder.Append(vbCrLf)

    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)

    '    For Each propertyMap In classMap.GetNonInheritedPropertyMaps

    '        'Case
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
    '        codeBuilder.Append("case """)
    '        codeBuilder.Append(propertyMap.Name)
    '        codeBuilder.Append(""":")
    '        codeBuilder.Append(vbCrLf)

    '        'Return value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("return ")
    '        codeBuilder.Append(propertyMap.GetFieldName & ";")
    '        codeBuilder.Append(vbCrLf)

    '    Next

    '    'Case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
    '    codeBuilder.Append("default:")
    '    codeBuilder.Append(vbCrLf)

    '    If Not superClass Is Nothing Then

    '        'Return value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("return base.IObjectHelper_GetPropertyValue(propertyName);")
    '        codeBuilder.Append(vbCrLf)

    '    Else

    '        'Return value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("throw new Exception(""IObjectHelper Error: Property with name '"" + propertyName + ""' not found!"");")
    '        codeBuilder.Append(vbCrLf)

    '    End If

    '    'End Select case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method region
    '    AddEndRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")

    '    codeBuilder.Append(vbCrLf)

    '    'SetPropertyValue Method

    '    'Method region
    '    AddRegion(codeBuilder, "IObjectHelper Method 'SetPropertyValue'")

    '    If superClass Is Nothing Then
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("void Puzzle.NPersist.Framework.IObjectHelper.SetPropertyValue(string propertyName, object value)")
    '        codeBuilder.Append(vbCrLf)
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("{")
    '        codeBuilder.Append(vbCrLf)

    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '        codeBuilder.Append("this.IObjectHelper_SetPropertyValue(propertyName, value);")
    '        codeBuilder.Append(vbCrLf)

    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '        codeBuilder.Append("}")
    '        codeBuilder.Append(vbCrLf)

    '    End If

    '    'Method Declaration
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    If superClass Is Nothing Then
    '        codeBuilder.Append("protected virtual void IObjectHelper_SetPropertyValue(string propertyName, object value)")
    '    Else
    '        codeBuilder.Append("protected override void IObjectHelper_SetPropertyValue(string propertyName, object value)")
    '    End If

    '    codeBuilder.Append(vbCrLf)
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)


    '    'Select case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("switch(propertyName)")
    '    codeBuilder.Append(vbCrLf)

    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("{")
    '    codeBuilder.Append(vbCrLf)

    '    For Each propertyMap In classMap.GetNonInheritedPropertyMaps

    '        convDataType = propertyMap.DataType


    '        'Case
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
    '        codeBuilder.Append("case """)
    '        codeBuilder.Append(propertyMap.Name)
    '        codeBuilder.Append(""":")
    '        codeBuilder.Append(vbCrLf)

    '        'Set value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append(propertyMap.GetFieldName)
    '        codeBuilder.Append(" = (" & convDataType & ") value;")
    '        codeBuilder.Append(vbCrLf)

    '        'break
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("break;")
    '        codeBuilder.Append(vbCrLf)

    '    Next

    '    'Case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 2))
    '    codeBuilder.Append("default:")
    '    codeBuilder.Append(vbCrLf)

    '    If Not superClass Is Nothing Then

    '        'Set value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("base.IObjectHelper_SetPropertyValue(propertyName, value);")
    '        codeBuilder.Append(vbCrLf)

    '        'break
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("break;")
    '        codeBuilder.Append(vbCrLf)

    '    Else

    '        'Return value
    '        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 3))
    '        codeBuilder.Append("throw new Exception(""IObjectHelper Error: Property with name '"" + propertyName + ""' not found!"");")
    '        codeBuilder.Append(vbCrLf)

    '    End If

    '    'End Select case
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent, 1))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method
    '    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
    '    codeBuilder.Append("}")
    '    codeBuilder.Append(vbCrLf)

    '    'End Method region
    '    AddEndRegion(codeBuilder, "IObjectHelper Method 'SetPropertyValue'")

    '    codeBuilder.Append(vbCrLf)

    '    'End IObjectHelper Interface region
    '    AddEndRegion(codeBuilder, "Interface Implementation 'IObjectHelper'")

    '    code = codeBuilder.ToString

    '    Return code

    'End Function




    Public Overloads Overrides Function ClassToCode(ByVal classMap As Puzzle.NPersist.Framework.Mapping.IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean, ByVal customCode As String) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder

        Dim propertyMap As IPropertyMap
        Dim superClass As IClassMap = classMap.GetInheritedClassMap
        Dim ns As String
        Dim implSep As String = " : "
        Dim convDataType As String

        Dim isShadow As Boolean

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

            'Namespace region
            AddRegion(codeBuilder, "Unit '" & ns & "'")

            If Len(ns) > 0 Then

                'Namespace Declaration
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("unit ")
                codeBuilder.Append(ns)
                codeBuilder.Append(";")
                codeBuilder.Append(vbCrLf)
                codeBuilder.Append(vbCrLf)

            End If

            'using System;
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("uses")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("System;")
            codeBuilder.Append(vbCrLf)

        End If

        'Class region
        AddRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        'Class doc comments
        codeBuilder.Append(GetDocComment(classMap, DocCommentPrefix))



        'Generated Code region
        AddForcedRegion(codeBuilder, "Generated Code Region")

        codeBuilder.Append(vbCrLf)

        'Constructor region
        AddRegion(codeBuilder, "Constructor For Class '" & classMap.GetName & "'")

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("constructor ")
        codeBuilder.Append(classMap.GetName)
        codeBuilder.Append(".Create;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("inherited Create;")
        codeBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                If propertyMap.IsCollection Then


                Else

                    If Len(propertyMap.DefaultValue) > 0 Then

                        'Instantiate private fields
                        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                        codeBuilder.Append("self." & propertyMap.GetFieldName)
                        codeBuilder.Append(" := ")
                        If IsStringProperty(propertyMap) Then

                            codeBuilder.Append("'" & propertyMap.DefaultValue & "'")

                        Else

                            codeBuilder.Append(propertyMap.DefaultValue)

                        End If
                        codeBuilder.Append(";")
                        codeBuilder.Append(vbCrLf)

                    End If

                End If

            End If

        Next


        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(vbCrLf)

        'End Constructor region
        AddEndRegion(codeBuilder, "Constructor For Class '" & classMap.GetName & "'")

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

        'Interface implementations

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

        'End Generated Code region
        AddForcedEndRegion(codeBuilder, "Generated Code Region")

        codeBuilder.Append(vbCrLf)


        AddComment(codeBuilder, "Add your synchronized custom code here:", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        'Custom Code region
        AddForcedRegion(codeBuilder, "Synchronized Custom Code Region")

        Dim synchedCode As String = ""
        Dim codeMap As ICodeMap = classMap.GetCodeMap(CodeLanguage.Delphi)

        If Not codeMap Is Nothing Then

            synchedCode = codeMap.Code

        End If

        If Len(synchedCode) > 0 Then

            codeBuilder.Append(synchedCode)

        End If

        'End Custom Code region
        AddForcedEndRegion(codeBuilder, "Synchronized Custom Code Region")

        codeBuilder.Append(vbCrLf)

        'Custom Code region
        AddForcedRegion(codeBuilder, "Unsynchronized Custom Code Region")

        If Len(customCode) > 0 Then

            codeBuilder.Append(customCode)

        Else

            AddComment(codeBuilder, "Add your custom code here:", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append(vbCrLf)

        End If

        'End Custom Code region
        AddForcedEndRegion(codeBuilder, "Unsynchronized Custom Code Region")

        codeBuilder.Append(vbCrLf)

        'End Class region
        AddEndRegion(codeBuilder, "Class '" & classMap.GetName & "'")

        If Not noNamespace Then

            If Len(ns) > 0 Then

                'End Namespace
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
                codeBuilder.Append("end.")
                codeBuilder.Append(vbCrLf)

            End If

            'End Namespace region
            AddEndRegion(codeBuilder, "Namespace '" & ns & "'")

        End If

        code = codeBuilder.ToString

        Return code

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

        Dim offset As Long = 0

        If propertyMap.IsCollection Then
            dataType = propertyMap.ItemType
        Else
            dataType = propertyMap.DataType
        End If

        convDataType = propertyMap.DataType


        If Not propertyMap.GetColumnMap Is Nothing Then

            If propertyMap.GetColumnMap.IsAutoIncrease Then

                makeReadOnly = True

            End If

        End If

        'Property region
        AddRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

        'Property doc comments
        codeBuilder.Append(GetDocComment(propertyMap, DocCommentPrefix))

        'Property Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))

        codeBuilder.Append("function ")
        codeBuilder.Append(propertyMap.ClassMap.Name)
        codeBuilder.Append(".get_")
        codeBuilder.Append(propertyMap.Name)
        codeBuilder.Append(": ")
        codeBuilder.Append(convDataType)
        codeBuilder.Append(";")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("var")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("value: ")
        codeBuilder.Append(convDataType)
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("refValue: System.Object;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("cancel: System.Boolean;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("interceptor: Puzzle.NPersist.Framework.IInterceptor;")
        codeBuilder.Append(vbCrLf)


        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        'Notify Get region
        AddRegionAsComment(codeBuilder, "Notify Property Get '" & propertyMap.Name & "'")


        If Not NotifyOnlyWhenRequired OrElse (propertyMap.IsCollection Or propertyMap.ReferenceType <> ReferenceType.None Or propertyMap.LazyLoad Or Not propertyMap.GetTableMap Is propertyMap.ClassMap.GetTableMap) OrElse HasReferencesByIdentityProperties(propertyMap.ClassMap) Then

            If Not NotifyLightWeight Then

                'Declare return variable 
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("value := self." & propertyMap.GetFieldName & ";")
                codeBuilder.Append(vbCrLf)

                'Declare ref variable 
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("refValue := System.Object(value);")
                codeBuilder.Append(vbCrLf)

                'Declare cancel variable
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("cancel := false;")
                codeBuilder.Append(vbCrLf)

            End If

            'Notify interceptor manager
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("interceptor := Puzzle.NPersist.Framework.IInterceptable(self).GetInterceptor();")
            codeBuilder.Append(vbCrLf)

            If NotifyLightWeight Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyPropertyGet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("');")
                codeBuilder.Append(vbCrLf)

            Else

                offset = 1

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyPropertyGet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("', refValue, cancel);")
                codeBuilder.Append(vbCrLf)

                'Check cancel variable
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (cancel) then")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("begin")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                codeBuilder.Append("result := " & GetCancelReturnValue(propertyMap) & ";")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("end")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("else")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("begin")
                codeBuilder.Append(vbCrLf)

                'Convert from ref variable 
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("value := " & convDataType & "(refValue);")
                codeBuilder.Append(vbCrLf)

            End If


        End If

        'End Notify Get region
        AddEndRegionAsComment(codeBuilder, "Notify Property Get '" & propertyMap.Name & "'")


        'Notify After Get region
        AddRegionAsComment(codeBuilder, "Notify After Property Get '" & propertyMap.Name & "'")


        If Not NotifyOnlyWhenRequired Or (propertyMap.IsCollection Or propertyMap.ReferenceType <> ReferenceType.None Or propertyMap.LazyLoad Or Not propertyMap.GetTableMap Is propertyMap.ClassMap.GetTableMap) Then


            If NotifyAfterGet And Not NotifyLightWeight Then

                'Notify interceptor manager
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, offset))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyAfterPropertyGet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("', value);")
                codeBuilder.Append(vbCrLf)

            End If


        End If

        'End Notify After Get region
        AddEndRegionAsComment(codeBuilder, "Notify After Property Get '" & propertyMap.Name & "'")

        If NotifyLightWeight Then

            'Return Value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, offset))
            codeBuilder.Append("result := ")
            codeBuilder.Append(propertyMap.GetFieldName)
            codeBuilder.Append(";")
            codeBuilder.Append(vbCrLf)

        Else

            'Return Value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, offset))
            codeBuilder.Append("result := value;")
            codeBuilder.Append(vbCrLf)

        End If

        If offset = 1 Then

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("end;")
            codeBuilder.Append(vbCrLf)

        End If

        'End Property Get
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(vbCrLf)


        offset = 0

        If Not makeReadOnly Then

            'Property Declaration
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))

            codeBuilder.Append("procedure ")
            codeBuilder.Append(propertyMap.ClassMap.Name)
            codeBuilder.Append(".set_")
            codeBuilder.Append(propertyMap.Name)
            codeBuilder.Append("(value: ")
            codeBuilder.Append(convDataType)
            codeBuilder.Append(");")
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("var")
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("refValue: System.Object;")
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("cancel: System.Boolean;")
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("interceptor: Puzzle.NPersist.Framework.IInterceptor;")
            codeBuilder.Append(vbCrLf)


            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("begin")
            codeBuilder.Append(vbCrLf)

            'Notify Set region
            AddRegionAsComment(codeBuilder, "Notify Property Set '" & propertyMap.Name & "'")

            'Declare reference value variable
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("refValue := System.Object(value);")
            codeBuilder.Append(vbCrLf)

            If Not NotifyLightWeight Then

                'Declare cancel variable
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("cancel := false;")
                codeBuilder.Append(vbCrLf)

            End If

            'Notify interceptor manager
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
            codeBuilder.Append("interceptor := Puzzle.NPersist.Framework.IInterceptable(self).GetInterceptor();")
            codeBuilder.Append(vbCrLf)

            If NotifyLightWeight Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyPropertySet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("', refValue);")
                codeBuilder.Append(vbCrLf)

            Else

                offset = 1

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyPropertySet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("', refValue, cancel);")
                codeBuilder.Append(vbCrLf)

                'Check cancel variable
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("if (cancel = false) then")
                codeBuilder.Append(vbCrLf)

            End If


            'End Notify Set region
            AddEndRegionAsComment(codeBuilder, "Notify Property Set '" & propertyMap.Name & "'")

            'Set Value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, offset))
            codeBuilder.Append(propertyMap.GetFieldName)
            codeBuilder.Append(" := " & convDataType & "(refValue);")
            codeBuilder.Append(vbCrLf)

            'Notify After Set region
            AddRegionAsComment(codeBuilder, "Notify After Property Set '" & propertyMap.Name & "'")

            If NotifyAfterSet And Not NotifyLightWeight Then


                'Notify interceptor manager
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent, offset))
                codeBuilder.Append("if (interceptor <> nil) then interceptor.NotifyAfterPropertySet(self, '")
                codeBuilder.Append(propertyMap.Name)
                codeBuilder.Append("', value);")
                codeBuilder.Append(vbCrLf)

            End If

            'End Notify After Set region
            AddEndRegionAsComment(codeBuilder, "Notify After Property Set '" & propertyMap.Name & "'")


            If offset = 1 Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("end;")
                codeBuilder.Append(vbCrLf)

            End If

            'End Property Set
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
            codeBuilder.Append("end;")
            codeBuilder.Append(vbCrLf)

            codeBuilder.Append(vbCrLf)

        End If

        'End Property region
        AddEndRegion(codeBuilder, "Property '" & propertyMap.Name & "'")

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
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("function " & classMap.Name & ".GetInterceptor: Puzzle.NPersist.Framework.IInterceptor;")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)


        'Return interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("result := m_Interceptor;")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IInterceptable Method 'GetInterceptor'")

        codeBuilder.Append(vbCrLf)

        'SetInterceptor Method

        'Method region
        AddRegion(codeBuilder, "IInterceptable Method 'SetInterceptor'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("procedure " & classMap.Name & ".SetInterceptor(value: Puzzle.NPersist.Framework.IInterceptor);")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        'Return interceptor manager
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("self.m_Interceptor := value;")
        codeBuilder.Append(vbCrLf)


        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
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
        Dim convDataType As String

        Dim flag As Boolean

        'IObjectHelper Interface region
        AddRegion(codeBuilder, "Interface Implementation 'IObjectHelper'")

        AddComment(codeBuilder, "IObjectHelper interface implementation", IClassesToCode.IndentationLevelEnum.MemberIndent, 0)

        'Method region
        AddRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")


        'codeBuilder.Append("protected virtual object IObjectHelper_GetPropertyValue(string propertyName) As Object Implements Puzzle.NPersist.Framework.IObjectHelper.GetPropertyValue")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))

        codeBuilder.Append("function " & classMap.Name & ".GetPropertyValue(propertyName: System.String): System.Object;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("var")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("propertyNameLow: System.String;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("foundName: System.Boolean;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("propertyNameLow := System.String(propertyName).ToLower();")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("foundName := false;")
        codeBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))

                'Case
                codeBuilder.Append("if (propertyNameLow = '" & propertyMap.Name.ToLower() & "') then")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("begin")
                codeBuilder.Append(vbCrLf)

                'Return value
                If propertyMap.ReferenceType = ReferenceType.None Then

                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                    codeBuilder.Append("result := System.Object(self.")
                    codeBuilder.Append(propertyMap.GetFieldName & ");")
                    codeBuilder.Append(vbCrLf)

                Else

                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                    codeBuilder.Append("result := self.")
                    codeBuilder.Append(propertyMap.GetFieldName & ";")
                    codeBuilder.Append(vbCrLf)

                End If

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                codeBuilder.Append("foundName := true;")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("end;")
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'Case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("if (foundName = false) then")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        If Not superClass Is Nothing Then

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
            codeBuilder.Append("result := inherited GetPropertyValue(propertyName);")
            codeBuilder.Append(vbCrLf)

        Else

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
            codeBuilder.Append("raise Exception.Create('IObjectHelper Error: Property with name (' + propertyName + ') not found!');")
            codeBuilder.Append(vbCrLf)

        End If

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        'End Method region
        AddEndRegion(codeBuilder, "IObjectHelper Method 'GetPropertyValue'")

        codeBuilder.Append(vbCrLf)

        'SetPropertyValue Method

        'Method region
        AddRegion(codeBuilder, "IObjectHelper Method 'SetPropertyValue'")

        'Method Declaration
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("procedure " & classMap.Name & ".SetPropertyValue(propertyName: System.String, value: System.Object);")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("var")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("propertyNameLow: System.String;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("foundName: System.Boolean;")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("propertyNameLow := System.String(propertyName).ToLower();")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("foundName := false;")
        codeBuilder.Append(vbCrLf)

        For Each propertyMap In classMap.GetNonInheritedPropertyMaps

            If ImplementShadows OrElse classMap.IsShadowingProperty(propertyMap) = False Then

                convDataType = propertyMap.DataType

                'Case
                codeBuilder.Append("if (propertyNameLow = '" & propertyMap.Name.ToLower() & "') then")
                codeBuilder.Append(vbCrLf)

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("begin")
                codeBuilder.Append(vbCrLf)

                'Set value
                If propertyMap.ReferenceType = ReferenceType.None Then

                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                    codeBuilder.Append("self." & propertyMap.GetFieldName)
                    codeBuilder.Append(" = " & convDataType & "(value);")
                    codeBuilder.Append(vbCrLf)

                Else

                    codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                    codeBuilder.Append("self." & propertyMap.GetFieldName)
                    codeBuilder.Append(" = value;")
                    codeBuilder.Append(vbCrLf)

                End If

                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
                codeBuilder.Append("foundName := true;")
                codeBuilder.Append(vbCrLf)

                'break
                codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
                codeBuilder.Append("end;")
                codeBuilder.Append(vbCrLf)

            End If

        Next

        'Case
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("if (foundName = false) then")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("begin")
        codeBuilder.Append(vbCrLf)

        If Not superClass Is Nothing Then

            'Set value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
            codeBuilder.Append("inherited SetPropertyValue(propertyName, value);")
            codeBuilder.Append(vbCrLf)

        Else

            'Return value
            codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.MemberIndent))
            codeBuilder.Append("raise Exception.Create('IObjectHelper Error: Property with name (' + propertyName + ') not found!');")
            codeBuilder.Append(vbCrLf)

        End If

        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.ClassIndent))
        codeBuilder.Append("end;")
        codeBuilder.Append(vbCrLf)

        'End Method
        codeBuilder.Append(GetIndentation(IClassesToCode.IndentationLevelEnum.NamespaceIndent))
        codeBuilder.Append("end;")
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
        codeBuilder.Append("//" & GetRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Sub AddEndRegionAsComment(ByRef codeBuilder As StringBuilder, ByVal name As String)

        If Not IncludeRegions Then Exit Sub
        codeBuilder.Append("//" & GetEndRegion(name))
        codeBuilder.Append(vbCrLf)

    End Sub

    Protected Overridable Function GetRegion(ByVal name As String) As String

        'If Len(name) > 0 Then name = ": " & name

        Return "#region "" " & name & " """

    End Function


    Protected Overridable Function GetEndRegion(ByVal name As String) As String

        'If Len(name) > 0 Then name = ": " & name

        Return "#endregion //" & name

    End Function



    Protected Overridable Sub AddComment(ByRef codeBuilder As StringBuilder, ByVal comment As String, ByVal IndentationLevel As Puzzle.ObjectMapper.Tools.IClassesToCode.IndentationLevelEnum, ByVal additionalLevel As Integer)

        If Not IncludeComments Then Exit Sub
        codeBuilder.Append(GetComment(comment, IndentationLevel, additionalLevel))
        codeBuilder.Append(vbCrLf)

    End Sub


    Protected Overridable Function GetComment(ByVal comment As String, ByVal IndentationLevel As Puzzle.ObjectMapper.Tools.IClassesToCode.IndentationLevelEnum, ByVal additionalLevel As Integer) As String

        Return GetIndentation(IndentationLevel, additionalLevel) & "//" & comment

    End Function


    Public Overrides Function DomainToProject(ByVal domainMap As IDomainMap, ByVal projPath As String, ByVal classMapsAndFiles As Hashtable, ByVal embeddedFiles As ArrayList) As String

        Dim code As String
        Dim codeBuilder As New StringBuilder
        Dim classMap As IClassMap

        'Begin
        codeBuilder.Append("<VisualStudioProject>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("    <CSHARP")
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
        'codeBuilder.Append("                AssemblyOriginatorKeyMode = ""None""")
        'codeBuilder.Append(vbCrLf)
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
        codeBuilder.Append("                PreBuildEvent  = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                PostBuildEvent  = """"")
        codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                OptionCompare = ""Binary""")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                OptionExplicit = ""On""")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                OptionStrict = ""Off""")
        'codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                RootNamespace = """ & domainMap.RootNamespace & """")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                RunPostBuildEvent = ""OnBuildSuccess""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                StartupObject = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            >")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Config")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""Debug""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    AllowUnsafeBlocks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BaseAddress = ""285212672""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    CheckForOverflowUnderflow = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    ConfigurationOverrideFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineConstants = ""DEBUG;TRACE""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DocumentationFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DebugSymbols = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    FileAlignment = ""4096""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    IncrementalBuild = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    NoStdLib = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    NoWarn = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Optimize = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    OutputPath = ""bin\Debug\""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RegisterForComInterop = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RemoveIntegerChecks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    TreatWarningsAsErrors = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    WarningLevel = ""4""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                <Config")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Name = ""Release""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    AllowUnsafeBlocks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BaseAddress = ""285212672""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    CheckForOverflowUnderflow = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    ConfigurationOverrideFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DefineConstants = ""TRACE""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DocumentationFile = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    DebugSymbols = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    FileAlignment = ""4096""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    IncrementalBuild = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    NoStdLib = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    NoWarn = """"")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    Optimize = ""true""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    OutputPath = ""bin\Release\""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RegisterForComInterop = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RemoveIntegerChecks = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    TreatWarningsAsErrors = ""false""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    WarningLevel = ""4""")
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
        'codeBuilder.Append("                <Reference")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                    Name = ""Norm.Specification""")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                    AssemblyName = ""Norm.Specification""")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                    HintPath = ""bin/norm.specification.dll""")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                />")
        'codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            </References>")
        codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("            <Imports>")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                <Import Namespace = ""Microsoft.VisualBasic"" />")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                <Import Namespace = ""System"" />")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                <Import Namespace = ""System.Collections"" />")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                <Import Namespace = ""System.Data"" />")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("                <Import Namespace = ""System.Diagnostics"" />")
        'codeBuilder.Append(vbCrLf)
        'codeBuilder.Append("            </Imports>")
        'codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        </Build>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("        <Files>")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("            <Include>")
        codeBuilder.Append(vbCrLf)

        codeBuilder.Append("                <File")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    RelPath = ""AssemblyInfo.cs""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    SubType = ""Code""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                    BuildAction = ""Compile""")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("                />")
        codeBuilder.Append(vbCrLf)

        For Each classMap In classMapsAndFiles.Keys

            codeBuilder.Append("                <File")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    RelPath = """ & GetFilePathRelativeToProject(projPath, classMapsAndFiles(classMap)) & """")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    SubType = ""Code""")
            codeBuilder.Append(vbCrLf)
            codeBuilder.Append("                    BuildAction = ""Compile""")
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

        codeBuilder.Append("using System.Reflection;")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("using System.Runtime.CompilerServices;")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// General Information about an assembly is controlled through the following ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// set of attributes. Change these attribute values to modify the information")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// associated with an assembly.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyTitle(""" & domainMap.Name & """)]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyDescription(""Persistent Domain Classes For Domain " & domainMap.Name & "."")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyConfiguration("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyCompany("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyProduct("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyCopyright("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyTrademark("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyCulture("""")]		")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// Version information for an assembly consists of the following four values:")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//      Major Version")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//      Minor Version ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//      Build Number")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//      Revision")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// You can specify all the values or you can default the Revision and Build Numbers ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// by using the '*' as shown below:")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyVersion(""1.0.*"")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// In order to sign your assembly you must specify a key to use. Refer to the ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// Microsoft .NET Framework documentation for more information on assembly signing.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// Use the attributes below to control which key is used for signing. ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("// Notes: ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//   (*) If no key is specified, the assembly is not signed.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//   (*) KeyName refers to a key that has been installed in the Crypto Service")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       Provider (CSP) on your machine. KeyFile refers to a file which contains")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       a key.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//   (*) If the KeyFile and the KeyName values are both specified, the ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       following processing occurs:")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       (1) If the KeyName can be found in the CSP, that key is used.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       (2) If the KeyName does not exist and the KeyFile does exist, the key ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//           in the KeyFile is installed into the CSP and used.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       When specifying the KeyFile, the location of the KeyFile should be")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       relative to the project output directory which is")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       located in the project directory, you would specify the AssemblyKeyFile ")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       attribute as [assembly: AssemblyKeyFile(""..\\..\\mykey.snk"")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//       documentation for more information on this.")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("//")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyDelaySign(false)]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyKeyFile("""")]")
        codeBuilder.Append(vbCrLf)
        codeBuilder.Append("[assembly: AssemblyKeyName("""")]")
        codeBuilder.Append(vbCrLf)


        code = codeBuilder.ToString

        Return code

    End Function

    Protected Overridable Function GetCancelReturnValue(ByVal propertyMap As IPropertyMap) As String

        If propertyMap.IsCollection Then
            Return "null"
        Else
            Select Case LCase(propertyMap.DataType)
                Case "string", "system.string"
                    Return "null"
                Case "long", "integer", "int32", "int", "system.int32"
                    Return "0"
                Case "int16", "system.int16"
                    Return "0"
                Case "int64", "system.int64"
                    Return "0"
                Case "uint16", "system.uint16"
                    Return "0"
                Case "uint32", "system.uint32"
                    Return "0"
                Case "uint64", "system.uint64"
                    Return "0"
                Case "date", "time", "datetime", "system.datetime"
                    Return "System.DateTime.MinValue"
                Case "bool", "boolean", "system.boolean"
                    Return "false"
                Case "byte", "system.byte", "char", "system.char"
                    Return "0"
                Case "decimal", "system.decimal"
                    Return "0"
                Case "double", "system.double"
                    Return "0"
                Case "object", "system.object"
                    Return "null"
                Case "single", "system.single"
                    Return "0"
                Case Else
                    Return "null"
            End Select

        End If


    End Function

    Public Overrides Property DocCommentPrefix() As String
        Get
            Return m_DocCommentPrefix
        End Get
        Set(ByVal Value As String)
            m_DocCommentPrefix = Value
        End Set
    End Property

End Class

