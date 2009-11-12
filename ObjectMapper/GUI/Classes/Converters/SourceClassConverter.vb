Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class SourceClassConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listClassNames As New ArrayList
        Dim sourceClassMap As IClassMap
        Dim classMap As IClassMap
        Dim domainMap As IDomainMap

        If context.Instance.GetType Is GetType(ClassProperties) Then

            classMap = CType(context.Instance, ClassProperties).GetClassMap
            domainMap = classMap.DomainMap

        ElseIf context.Instance.GetType Is GetType(UmlClassProperties) Then

            classMap = CType(context.Instance, UmlClassProperties).GetUmlClass.GetClassMap
            domainMap = classMap.DomainMap

        End If

        If Not domainMap Is Nothing Then

            For Each sourceClassMap In domainMap.ClassMaps

                listClassNames.Add(sourceClassMap.Name)

            Next

        End If

        listClassNames.Sort()

        Return New StandardValuesCollection(listClassNames)

    End Function

End Class
