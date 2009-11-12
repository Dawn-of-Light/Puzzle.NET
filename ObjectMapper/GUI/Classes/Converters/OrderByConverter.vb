Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class OrderByConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listPropertyNames As New ArrayList
        Dim refClassMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim refPropertyMap As IPropertyMap

        If context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap

        End If


        If Not propertyMap Is Nothing Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each refPropertyMap In refClassMap.GetAllPropertyMaps

                    If refPropertyMap.ReferenceType = ReferenceType.None Then

                        listPropertyNames.Add(refPropertyMap.Name)

                    End If

                Next

            End If

        End If

        listPropertyNames.Sort()

        Return New StandardValuesCollection(listPropertyNames)

    End Function

End Class

