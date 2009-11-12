Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.Plugin

<ConverterClass("From Tables To Classes Converter")> Public Class TableNameConverter

    <ConverterMethod("Name Class As Table")> Public Function GetClassNameFromTableNoConversion(ByVal tableName As String)

        Return tableName

    End Function

    <ConverterMethod("Name Property As Column")> Public Function GetPropertyNameFromColumnNoConversion(ByVal columnMap As IColumnMap)

        Return columnMap.Name

    End Function

    <ConverterMethod("Name Inverse As Property With Inv Prefix")> Public Function GetInverseNameFromPropertyAsInvPrefix(ByVal propertyMap As IPropertyMap)

        Return "Inv" & propertyMap.Name

    End Function

End Class
