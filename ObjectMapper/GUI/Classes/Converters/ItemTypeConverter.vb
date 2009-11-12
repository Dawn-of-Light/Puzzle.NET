Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter

Public Class ItemTypeConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listDataTypes As New ArrayList()
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap

        If context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap

        End If


        If Not propertyMap Is Nothing Then


            For Each classMap In propertyMap.ClassMap.DomainMap.ClassMaps

                listDataTypes.Add(FormattingServices.GetClassMapName(classMap, propertyMap.ClassMap))

            Next

            listDataTypes.Sort()

            listDataTypes.Add("System.Boolean")
            listDataTypes.Add("System.Byte")
            listDataTypes.Add("System.DateTime")
            listDataTypes.Add("System.Decimal")
            listDataTypes.Add("System.Double")
            listDataTypes.Add("System.Guid")
            listDataTypes.Add("System.Int16")
            listDataTypes.Add("System.Int32")
            listDataTypes.Add("System.Int64")
            listDataTypes.Add("System.Object")
            listDataTypes.Add("System.Single")
            listDataTypes.Add("System.String")
            listDataTypes.Add("System.UInt16")
            listDataTypes.Add("System.UInt32")
            listDataTypes.Add("System.UInt64")

        End If

        Return New StandardValuesCollection(listDataTypes)

    End Function

End Class