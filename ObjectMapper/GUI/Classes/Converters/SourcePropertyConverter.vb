Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class SourcePropertyConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listPropertyNames As New ArrayList
        Dim sourcePropertyMap As IPropertyMap
        Dim classMap As IClassMap
        Dim sourceClassMap As IClassMap
        Dim propertyMap As IPropertyMap

        If context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetPropertyMap

        End If

        If Not propertyMap Is Nothing Then

            classMap = propertyMap.ClassMap

            sourceClassMap = classMap.GetSourceClassMap

        End If

        If Not sourceClassMap Is Nothing Then

            For Each sourcePropertyMap In sourceClassMap.GetAllPropertyMaps

                If sourcePropertyMap.DataType = propertyMap.DataType Then

                    listPropertyNames.Add(sourcePropertyMap.Name)

                End If

            Next

        End If

        listPropertyNames.Sort()

        Return New StandardValuesCollection(listPropertyNames)

    End Function

End Class

