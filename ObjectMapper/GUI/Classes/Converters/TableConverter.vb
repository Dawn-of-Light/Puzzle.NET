Imports System.ComponentModel
Imports System.ComponentModel.TypeConverter
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class TableConverter
    Inherits StringConverter


    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection

        Dim listTableNames As New ArrayList()
        Dim sourceMap As ISourceMap
        Dim tableMap As ITableMap
        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim columnMap As IColumnMap
        Dim addClassMap As IClassMap
        Dim umlClass As umlClass

        If context.Instance.GetType Is GetType(ClassProperties) Then

            classMap = CType(context.Instance, ClassProperties).GetClassMap
            sourceMap = classMap.GetSourceMap

        ElseIf context.Instance.GetType Is GetType(PropertyProperties) Then

            propertyMap = CType(context.Instance, PropertyProperties).GetPropertyMap
            sourceMap = propertyMap.GetSourceMap

        ElseIf context.Instance.GetType Is GetType(ColumnProperties) Then

            columnMap = CType(context.Instance, ColumnProperties).GetColumnMap
            sourceMap = columnMap.TableMap.SourceMap

        ElseIf context.Instance.GetType Is GetType(UmlClassProperties) Then

            umlClass = CType(context.Instance, UmlClassProperties).GetUmlClass
            classMap = umlClass.GetClassMap
            If Not classMap Is Nothing Then

                sourceMap = classMap.GetSourceMap

            End If

        ElseIf context.Instance.GetType Is GetType(UmlLineEndProperties) Then

            propertyMap = CType(context.Instance, UmlLineEndProperties).GetpropertyMap
            If Not propertyMap Is Nothing Then

                sourceMap = propertyMap.GetSourceMap

            End If

        End If

        If Not sourceMap Is Nothing Then

            For Each tableMap In sourceMap.TableMaps

                listTableNames.Add(tableMap.Name)

            Next

        End If

        listTableNames.Sort()

        Return New StandardValuesCollection(listTableNames)

    End Function

End Class
