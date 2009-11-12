Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface ITablesToSource

    Sub TablesToSource(ByVal sourceMap As ISourceMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

    Sub ColumnsToSource(ByVal tableMap As ITableMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)


    Overloads Function CommitTablesToSource(ByVal sourceMap As ISourceMap) As String

    Overloads Function CommitTablesToSource(ByVal sourceMap As ISourceMap, ByVal dtd As String) As String

    Function SourceToDTDEvolve(ByVal sourceMap As ISourceMap) As String

    Function SourceToDTD(ByVal sourceMap As ISourceMap, ByVal dropTables As Boolean) As String

    Function TableToDTD(ByVal tableMap As ITableMap) As String

    Function TableToDTD(ByVal tableMap As ITableMap, ByVal dropTable As Boolean) As String

    Function TableToDTD(ByVal tableMap As ITableMap, ByVal dropTable As Boolean, ByVal includeConstraints As Boolean) As String

    Function DropTableToDTD(ByVal tableMap As ITableMap) As String

    Function DropTableForeignKeyConstraintsToDTD(ByVal tableMap As ITableMap) As String

    Function TableKeyConstraintToDTD(ByVal tableMap As ITableMap) As String

    Function TableForeignKeyConstraintsToDTD(ByVal tableMap As ITableMap) As String

    Function TableDefaultConstraintsToDTD(ByVal tableMap As ITableMap) As String

    Function ColumnToDTD(ByVal columnMap As IColumnMap) As String

    Function AddColumnToTableToDTD(ByVal columnMap As IColumnMap) As String

    Function DropColumnToDTD(ByVal columnMap As IColumnMap) As String

    Function GetDataType(ByVal columnMap As IColumnMap) As String

    Function SourceToDTD(ByVal sourceMap As ISourceMap) As String


End Interface
