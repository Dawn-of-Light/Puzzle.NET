Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Class SourceToTablesBase
    Implements ISourceToTables


    Public Overridable Overloads Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByRef hashDiff As Hashtable) Implements Puzzle.ObjectMapper.Tools.ISourceToTables.SourceToColumns

    End Sub

    Public Overridable Overloads Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As Hashtable) Implements Puzzle.ObjectMapper.Tools.ISourceToTables.SourceToColumns

    End Sub

    Public Overridable Overloads Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As Hashtable) Implements Puzzle.ObjectMapper.Tools.ISourceToTables.SourceToTables

    End Sub

    Public Overridable Overloads Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByRef hashDiff As Hashtable) Implements Puzzle.ObjectMapper.Tools.ISourceToTables.SourceToTables

    End Sub


End Class
