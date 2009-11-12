Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Reflection
Imports Puzzle.NPersist.Framework.Mapping

Public Interface ITablesToClasses

    Property GetClassNameForTableConverter() As Object

    Property GetClassNameForTableConverterMethod() As MethodInfo

    Property GetPropertyNameForColumnConverter() As Object

    Property GetPropertyNameForColumnConverterMethod() As MethodInfo

    Property GetInverseNameForPropertyConverter() As Object

    Property GetInverseNameForPropertyConverterMethod() As MethodInfo

    Property RemoveIllegalCharactersStrictly() As Boolean

    Property GenerateInverseProperties() As Boolean

    Property SetCascadeDeleteForManyOneInverseProperties() As Boolean

    Property CheckReservedNamesCSharp() As Boolean

    Property CheckReservedNamesVb() As Boolean

    Property CheckReservedNamesDelphi() As Boolean

    Function GetClassNameForTable(ByVal tableMap As ITableMap) As String

    Overloads Function GetPropertyNameForColumn(ByVal columnMap As IColumnMap) As String

    Overloads Function GetPropertyNameForColumn(ByVal name As String, ByVal isPrimaryKey As Boolean, ByVal isForeignKey As Boolean) As String

    Overloads Function GetPropertyNameForColumn(ByVal name As String, ByVal isPrimaryKey As Boolean, ByVal isForeignKey As Boolean, ByVal cntPrimaryKeys As Integer) As String

    Function GetPropertyTypeFromColumn(ByVal columnMap As IColumnMap) As String

    Sub SetPropertyTypeFromColumn(ByVal propertyMap As IPropertyMap, ByVal columnMap As IColumnMap)

    Sub GenerateClassesForSources(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap)

    Sub GenerateClassesForSources(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

    Sub GenerateClassesForTables(ByVal sourceMap As ISourceMap, ByVal targetDomainMap As IDomainMap)

    Sub GenerateClassesForTables(ByVal sourceMap As ISourceMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

    Overloads Sub GenerateInverseProperty(ByVal propertyMap As IPropertyMap)

    Overloads Sub GenerateInverseProperty(ByVal propertyMap As IPropertyMap, ByVal refClassMap As IClassMap)

    Function MakeSingularName(ByVal name As String) As String

    Function MakePluralName(ByVal name As String) As String

    Property PreventIntelliNaming() As Boolean

    Sub MakeSureNamesAreLegal(ByVal domainMap As IDomainMap)

End Interface
