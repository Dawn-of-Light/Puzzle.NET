Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface ISourceToTables

    Sub SourceToTables(ByVal sourceMap As ISourceMap, ByRef hashDiff As Hashtable)

    Sub SourceToTables(ByVal sourceMap As ISourceMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

    Sub SourceToColumns(ByVal tableMap As ITableMap, ByRef hashDiff As Hashtable)

    Sub SourceToColumns(ByVal tableMap As ITableMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

End Interface
