Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface IClassesToTables

    Property TablePrefix() As String

    Property TableSuffix() As String

    Property MaxTableLength() As Integer

    Property MaxColumnLength() As Integer

    Property DefaultStringLength() As Integer

    Overloads Function GetTableNameForClass(ByVal classMap As IClassMap) As String

    Overloads Function GetTableNameForClass(ByVal classMap As IClassMap, ByVal useThisClass As Boolean) As String

    Function GetTypeColumnNameForClass(ByVal classMap As IClassMap) As String

    Overloads Function GetTableNameForProperty(ByVal propertyMap As IPropertyMap) As String

    Function GetColumnNameForProperty(ByVal propertyMap As IPropertyMap) As String

    Function GetColumnTypeForProperty(ByVal propertyMap As IPropertyMap) As System.Data.DbType

    Function GetColumnLengthForProperty(ByVal propertyMap As IPropertyMap) As Integer

    Function GetColumnPrecisionForProperty(ByVal propertyMap As IPropertyMap) As Integer

    Function GetColumnScaleForProperty(ByVal propertyMap As IPropertyMap) As Integer


    Sub GenerateTablesForClasses(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap)

    Sub GenerateTablesForClasses(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)


End Interface
