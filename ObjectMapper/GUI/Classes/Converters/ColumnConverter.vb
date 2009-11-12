Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class ColumnConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listColumnNames As New ArrayList()
        Dim tableMap As ITableMap
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim columnMap As IColumnMap
        Dim addClassMap As IClassMap

        If context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap
            tableMap = propertyMap.GetTableMap

        ElseIf context.Instance.GetType Is GetType(ColumnProperties) Then

            columnMap = CType(context.Instance, ColumnProperties).GetColumnMap
            tableMap = columnMap.GetPrimaryKeyTableMap

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap
            If Not propertyMap Is Nothing Then
                tableMap = propertyMap.GetTableMap
            End If

        End If

        If Not tableMap Is Nothing Then

            For Each columnMap In tableMap.ColumnMaps

                listColumnNames.Add(columnMap.Name)

            Next

        End If

        listColumnNames.Sort()

        Return New StandardValuesCollection(listColumnNames)

    End Function

End Class
