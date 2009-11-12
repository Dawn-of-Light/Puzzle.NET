Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Class TablesToSourceBase
    Implements ITablesToSource

    Public Overridable Overloads Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableToDTD

    End Function

    Public Overridable Overloads Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal dropTable As Boolean) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableToDTD

    End Function


    Public Overridable Overloads Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal dropTable As Boolean, ByVal includeConstraints As Boolean) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableToDTD

    End Function

    Public Overridable Function ColumnToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.ColumnToDTD

    End Function

    Public Overridable Function GetDataType(ByVal columnMap As IColumnMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.GetDataType

    End Function

    Public Overridable Function DropTableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.DropTableToDTD

    End Function

    Public Overridable Function DropTableForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.DropTableForeignKeyConstraintsToDTD

    End Function

    Public Overridable Overloads Function SourceToDTD(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal dropTables As Boolean) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.SourceToDTD

    End Function

    Public Overridable Overloads Function SourceToDTD(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.SourceToDTD

    End Function

    Public Overridable Function TableKeyConstraintToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableKeyConstraintToDTD

    End Function

    Public Overridable Function TableForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableForeignKeyConstraintsToDTD

    End Function

    Public Overridable Function TableDefaultConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TableDefaultConstraintsToDTD

    End Function

    Public Overridable Sub TablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As System.Collections.Hashtable) Implements Puzzle.ObjectMapper.Tools.ITablesToSource.TablesToSource

    End Sub

    Public Overridable Sub ColumnsToSource(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As System.Collections.Hashtable) Implements Puzzle.ObjectMapper.Tools.ITablesToSource.ColumnsToSource

    End Sub

    Public Overridable Function SourceToDTDEvolve(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.SourceToDTDEvolve

    End Function


    Public Overridable Overloads Function CommitTablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.CommitTablesToSource

    End Function

    Public Overridable Overloads Function CommitTablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal dtd As String) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.CommitTablesToSource

    End Function

    Public Overridable Function AddColumnToTableToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.AddColumnToTableToDTD

    End Function

    Public Overridable Function DropColumnToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String Implements Puzzle.ObjectMapper.Tools.ITablesToSource.DropColumnToDTD

    End Function

End Class
