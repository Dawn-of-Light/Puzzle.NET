Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class SuperClassConverter
    Inherits StringConverter


    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listClassNames As New ArrayList()
        Dim classMap As IClassMap
        Dim umlClass As umlClass
        Dim addClassMap As IClassMap

        If context.Instance.GetType Is GetType(ClassProperties) Then

            classMap = CType(context.Instance, ClassProperties).GetClassMap

        ElseIf context.Instance.GetType Is GetType(UmlClassProperties) Then

            umlClass = CType(context.Instance, UmlClassProperties).GetUmlClass

            classMap = umlClass.GetClassMap

        End If

        If Not classMap Is Nothing Then

            For Each addClassMap In classMap.DomainMap.ClassMaps

                If classMap.IsLegalAsSuperClass(addClassMap) Then

                    listClassNames.Add(FormattingServices.GetClassMapName(addClassMap, classMap))

                End If

            Next

        End If

        listClassNames.Sort()

        Return New StandardValuesCollection(listClassNames)

    End Function

End Class
