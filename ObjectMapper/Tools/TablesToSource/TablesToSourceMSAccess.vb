Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Persistence
Imports Puzzle.NPersist.Framework.Mapping
Imports System.Text

Imports System.Data.OleDb

Public Class TablesToSourceMSAccess
    Inherits TablesToSourceBase


    Public Overloads Overrides Function CommitTablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String

        Return CommitTablesToSource(sourceMap, "")

    End Function

    Public Overloads Overrides Function CommitTablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal dtd As String) As String

        Dim ctx As IContext = New Context(sourceMap.DomainMap)

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(sourceMap)

        Dim arrDtd() As String

        If dtd = "" Then dtd = GetSourceToDTDEvolve(sourceMap, ctx)


        arrDtd = Split(dtd, "GO" & vbCrLf)

        Try

            For Each dtd In arrDtd

                dtd = Replace(dtd, vbCrLf, " ")
                dtd = Replace(dtd, vbTab, " ")

                If Len(Trim(dtd)) > 0 Then

                    ctx.SqlExecutor.ExecuteNonQuery(dtd, ds)

                End If

            Next

        Catch ex As Exception

            ctx.Dispose()

            Throw New Exception("An exception occurred when communication with the data source: " & ex.Message, ex)

        End Try

        ctx.Dispose()

        Return dtd

    End Function

    Public Overrides Function SourceToDTDEvolve(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String

        Dim ctx As IContext = New Context(sourceMap.DomainMap)

        Dim dtd As String = GetSourceToDTDEvolve(sourceMap, ctx)

        ctx.Dispose()

        Return dtd

    End Function

    Protected Overridable Function GetSourceToDTDEvolve(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal ctx As IContext) As String

        'Dim sql As String = "Select [name] From [" & sourceMap.Schema & "].[sysobjects] Where OBJECTPROPERTY(id, N'IsUserTable') = 1"

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(sourceMap)

        Dim dt As DataTable

        Dim con As OleDbConnection

        Dim dtd As String

        Dim i As Long

        Dim name As String
        Dim tableMap As ITableMap
        Dim tableMapClone As ITableMap
        Dim hashExists As New Hashtable
        Dim result As Object

        Try

            con = ds.GetConnection

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            result = DataTableToArray(dt)

            dt.Dispose()

            ds.ReturnConnection()

            'result = ctx.SqlExecutor.ExecuteArray(sql, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            Throw New Exception("An exception occurred when communication with the data source: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                name = result(2, i)

                hashExists(LCase(name)) = True

            Next

        End If

        For Each tableMap In sourceMap.TableMaps

            If Not hashExists.ContainsKey(LCase(tableMap.Name)) Then

                dtd += GetTableToDTD(tableMap, False, True, True)

            Else

                If HasNonInsertableColumns(tableMap) Then

                    dtd += GetTableToDTD(tableMap, True, True, True)

                Else

                    dtd += AddColumnsToSource(tableMap, ctx)

                End If

            End If

        Next

        For Each tableMap In sourceMap.TableMaps

            'add foreign key constraint
            dtd += TableForeignKeyConstraintsToDTD(tableMap)

        Next

        Return dtd

    End Function



    Protected Overridable Function AddColumnsToSource(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal ctx As IContext) As String

        Dim columnMap As IColumnMap
        Dim columnMapClone As IColumnMap
        Dim dtd As String

        'Dim sql As String = "Select [name] From [" & tableMap.SourceMap.Schema & "].[syscolumns] Where [id] = object_id(N'[" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "]')"


        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(tableMap.SourceMap)

        Dim dt As DataTable

        Dim con As OleDbConnection

        Dim i As Long

        Dim result As Object

        Dim colName As String

        Dim name As String
        Dim hashExists As New Hashtable

        Try

            con = ds.GetConnection

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, tableMap.Name})

            result = DataTableToArray(dt)

            dt.Dispose()

            ds.ReturnConnection()

            'result = ctx.SqlExecutor.ExecuteArray(sql, ds)

        Catch ex As Exception

            Throw New Exception("An exception occurred when communication with the data source: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                name = result(3, i)

                hashExists(LCase(name)) = True

            Next

        End If

        For Each columnMap In tableMap.ColumnMaps

            If hashExists.ContainsKey(LCase(columnMap.Name)) Then

                'We must drop column first
                dtd += DropColumnToDTD(columnMap)

            End If

            dtd += AddColumnToTableToDTD(columnMap)

        Next

        Return dtd

    End Function

    Public Overrides Function DropColumnToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        'alter table
        'dtdBuilder.Append("ALTER TABLE [" & columnMap.TableMap.SourceMap.Schema & "].[" & columnMap.TableMap.Name & "]")
        dtdBuilder.Append("ALTER TABLE [" & columnMap.TableMap.Name & "]")

        'drop column
        dtdBuilder.Append(" DROP COLUMN [" & columnMap.Name & "] ")
        dtdBuilder.Append(vbCrLf)

        dtdBuilder.Append("GO")
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append(vbCrLf)

        dtd = dtdBuilder.ToString

        Return dtd

    End Function

    Public Overrides Function AddColumnToTableToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        'alter table
        'dtdBuilder.Append("ALTER TABLE [" & columnMap.TableMap.SourceMap.Schema & "].[" & columnMap.TableMap.Name & "]")
        dtdBuilder.Append("ALTER TABLE [" & columnMap.TableMap.Name & "]")

        'add column
        dtdBuilder.Append(" ADD [" & columnMap.Name & "] ")


        If columnMap.IsAutoIncrease Then

            dtdBuilder.Append("COUNTER ")

        Else

            'Data type
            dtdBuilder.Append(GetDataType(columnMap) & " ")

        End If

        'Allow/disallow null
        'If Not columnMap.AllowNulls Or columnMap.IsPrimaryKey Or columnMap.IsAutoIncrease Then
        'dtdBuilder.Append("NOT NULL")
        'Else
        dtdBuilder.Append("NULL")
        'End If
        dtdBuilder.Append(vbCrLf)

        dtdBuilder.Append("GO")
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append(vbCrLf)

        dtd = dtdBuilder.ToString

        Return dtd

    End Function




    Public Overloads Overrides Function SourceToDTD(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap) As String

        Return SourceToDTD(sourceMap, False)

    End Function

    Public Overloads Overrides Function SourceToDTD(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal dropTables As Boolean) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim tableMap As ITableMap

        For Each tableMap In sourceMap.TableMaps

            'drop foreign key constraint
            dtdBuilder.Append(DropTableForeignKeyConstraintsToDTD(tableMap))

        Next

        If dropTables Then

            For Each tableMap In sourceMap.TableMaps

                'drop table
                dtdBuilder.Append(DropTableToDTD(tableMap))

            Next

        End If

        For Each tableMap In sourceMap.TableMaps

            'create table
            dtdBuilder.Append(TableToDTD(tableMap))

        Next

        For Each tableMap In sourceMap.TableMaps

            'add key constraint
            dtdBuilder.Append(TableKeyConstraintToDTD(tableMap))

        Next

        For Each tableMap In sourceMap.TableMaps

            'add default value constraint
            dtdBuilder.Append(TableDefaultConstraintsToDTD(tableMap))

        Next

        For Each tableMap In sourceMap.TableMaps

            'add foreign key constraint
            dtdBuilder.Append(TableForeignKeyConstraintsToDTD(tableMap))

        Next

        dtd = dtdBuilder.ToString

        Return dtd

    End Function

    Public Overloads Overrides Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Return TableToDTD(tableMap, False)

    End Function

    Public Overloads Overrides Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal dropTable As Boolean) As String

        Return TableToDTD(tableMap, False, False)

    End Function

    Public Overloads Overrides Function TableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal dropTable As Boolean, ByVal includeConstraints As Boolean) As String

        Return GetTableToDTD(tableMap, False, False, False)

    End Function

    Private Function GetTableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal dropTable As Boolean, ByVal includeConstraints As Boolean, ByVal noForeignKeyConstraint As Boolean) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap
        Dim columnIndent As String = "        "
        Dim hasComma As Boolean
        Dim hasText As Boolean

        If dropTable Then

            dtdBuilder.Append(DropTableForeignKeyConstraintsToDTD(tableMap))

            'drop table
            dtdBuilder.Append(DropTableToDTD(tableMap))

        End If

        'create table
        'dtdBuilder.Append("CREATE TABLE [" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "] (")
        dtdBuilder.Append("CREATE TABLE [" & tableMap.Name & "] (")
        dtdBuilder.Append(vbCrLf)

        hasComma = False
        For Each columnMap In tableMap.ColumnMaps

            'Select Case columnMap.DataType
            '    Case DbType.AnsiString
            '        If columnMap.Precision < 1 Or columnMap.Precision > 8000 Then
            '            hasText = True
            '        End If
            '    Case DbType.String
            '        If columnMap.Precision < 1 Or columnMap.Precision > 4000 Then
            '            hasText = True
            '        End If
            'End Select
            If columnMap.IsAutoIncrease Then

                'Column
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append(ColumnToDTD(columnMap))
                dtdBuilder.Append(" ,")
                dtdBuilder.Append(vbCrLf)
                hasComma = True

            End If

        Next

        For Each columnMap In tableMap.ColumnMaps

            If columnMap.IsPrimaryKey And Not columnMap.IsAutoIncrease Then

                'Column
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append(ColumnToDTD(columnMap))
                dtdBuilder.Append(" ,")
                dtdBuilder.Append(vbCrLf)
                hasComma = True

            End If

        Next

        For Each columnMap In tableMap.ColumnMaps

            If Not (columnMap.IsPrimaryKey Or columnMap.IsAutoIncrease) Then

                'Column
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append(ColumnToDTD(columnMap))
                dtdBuilder.Append(" ,")
                dtdBuilder.Append(vbCrLf)
                hasComma = True

            End If

        Next

        If hasComma Then

            dtdBuilder.Length -= 4
            dtdBuilder.Append(vbCrLf)

        End If

        'TODO: Can/should this be switched back for indexing?
        'dtdBuilder.Append(") ON [PRIMARY]")
        dtdBuilder.Append(")")

        'TODO: Can/should this be switched back for indexing?
        'If hasText Then
        '    dtdBuilder.Append(" TEXTIMAGE_ON [PRIMARY]")
        'End If
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append("GO")
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append(vbCrLf)

        If includeConstraints Then

            'key constraint
            dtdBuilder.Append(TableKeyConstraintToDTD(tableMap))

        End If

        If includeConstraints Then

            'default value constraint
            dtdBuilder.Append(TableDefaultConstraintsToDTD(tableMap))

        End If

        If includeConstraints And Not noForeignKeyConstraint Then

            'foreign key constraint
            dtdBuilder.Append(TableForeignKeyConstraintsToDTD(tableMap))

        End If

        dtd = dtdBuilder.ToString

        Return dtd

    End Function


    Public Overrides Function TableKeyConstraintToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap
        Dim columnIndent As String = "        "
        Dim hasComma As Boolean
        Dim keyColumnMaps As ArrayList = tableMap.GetPrimaryKeyColumnMaps

        If keyColumnMaps.Count > 0 Then

            'alter table
            'dtdBuilder.Append("ALTER TABLE [" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "] WITH NOCHECK ADD")
            dtdBuilder.Append("ALTER TABLE [" & tableMap.Name & "] ADD")
            dtdBuilder.Append(vbCrLf)

            'Constraint
            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append("CONSTRAINT [PK_")
            dtdBuilder.Append(tableMap.Name)
            'TODO: ?
            'dtdBuilder.Append("] PRIMARY KEY  CLUSTERED ")
            dtdBuilder.Append("] PRIMARY KEY ")
            dtdBuilder.Append(vbCrLf)

            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append("(")
            dtdBuilder.Append(vbCrLf)

            For Each columnMap In keyColumnMaps

                'Column
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append("[")
                dtdBuilder.Append(columnMap.Name)
                dtdBuilder.Append("] ,")
                dtdBuilder.Append(vbCrLf)
                hasComma = True

            Next

            If hasComma Then

                dtdBuilder.Length -= 4
                dtdBuilder.Append(vbCrLf)

            End If

            dtdBuilder.Append(columnIndent)
            'TODO: ?
            'dtdBuilder.Append(") ON [PRIMARY]")
            dtdBuilder.Append(")")
            dtdBuilder.Append(vbCrLf)
            dtdBuilder.Append("GO")
            dtdBuilder.Append(vbCrLf)
            dtdBuilder.Append(vbCrLf)

        End If

        dtd = dtdBuilder.ToString

        Return dtd

    End Function



    Public Overrides Function TableForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap
        Dim testColumnMap As IColumnMap
        Dim keyColumnMaps As ArrayList = tableMap.GetForeignKeyColumnMaps
        Dim hasComma As Boolean
        Dim hashColumnMaps As New Hashtable
        Dim columnMapsList As ArrayList
        Dim columnMaps As ArrayList
        Dim ok As Boolean
        Dim i As Long
        Dim add As Boolean

        If keyColumnMaps.Count > 0 Then

            'alter table
            'dtdBuilder.Append("ALTER TABLE [" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "] WITH NOCHECK ADD")
            dtdBuilder.Append("ALTER TABLE [" & tableMap.Name & "] ADD")
            dtdBuilder.Append(vbCrLf)

            For Each columnMap In keyColumnMaps

                If Not hashColumnMaps.ContainsKey(LCase(columnMap.PrimaryKeyTable)) Then

                    hashColumnMaps(LCase(columnMap.PrimaryKeyTable)) = New ArrayList

                    columnMapsList = CType(hashColumnMaps(LCase(columnMap.PrimaryKeyTable)), ArrayList)

                    columnMapsList.Add(New ArrayList)

                Else

                    columnMapsList = CType(hashColumnMaps(LCase(columnMap.PrimaryKeyTable)), ArrayList)

                End If

                i = 0
                For Each columnMaps In columnMapsList

                    i += 1

                    ok = True

                    For Each testColumnMap In columnMaps

                        If testColumnMap.PrimaryKeyColumn = columnMap.PrimaryKeyColumn Then

                            ok = False
                            Exit For

                        End If

                    Next

                    If Not ok Then

                        If i = columnMapsList.Count Then

                            add = True

                            Exit For

                        Else

                            'Just continue looping

                        End If

                    Else

                        columnMaps.Add(columnMap)

                        Exit For

                    End If

                Next

            Next

            If add Then

                columnMaps = New ArrayList

                columnMaps.Add(columnMap)

                columnMapsList.Add(columnMaps)

            End If

            For Each columnMapsList In hashColumnMaps.Values

                For Each columnMaps In columnMapsList

                    dtdBuilder.Append(ColumnForeignKeyConstraintsToDTD(tableMap, columnMaps))

                    hasComma = True

                Next

            Next

            If hasComma Then

                dtdBuilder.Length -= 4
                dtdBuilder.Append(vbCrLf)

            End If

            dtdBuilder.Append("GO")
            dtdBuilder.Append(vbCrLf)
            dtdBuilder.Append(vbCrLf)

        End If

        dtd = dtdBuilder.ToString

        Return dtd


    End Function

    Protected Function ColumnForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal columnMaps As ArrayList) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder
        Dim columnIndent As String = "        "

        Dim columnMap As IColumnMap

        'Constraint
        dtdBuilder.Append(columnIndent)

        Dim fkName As String = ""

        'Construct an FK name

        For Each columnMap In columnMaps

            If columnMap.ForeignKeyName.Length > 0 Then

                fkName = columnMap.ForeignKeyName
                Exit For

            End If

        Next

        If fkName.Length < 1 Then

            fkName = "FK_" & tableMap.Name

            For Each columnMap In columnMaps

                fkName += "_"
                fkName += columnMap.Name

            Next

        End If

        dtdBuilder.Append("CONSTRAINT [" + fkName + "] FOREIGN KEY ")
        dtdBuilder.Append(vbCrLf)

        'dtdBuilder.Append("CONSTRAINT [FK_")
        'dtdBuilder.Append(tableMap.Name)

        'For Each columnMap In columnMaps

        '    dtdBuilder.Append("_")
        '    dtdBuilder.Append(columnMap.Name)

        'Next

        'dtdBuilder.Append("] FOREIGN KEY ")
        'dtdBuilder.Append(vbCrLf)

        dtdBuilder.Append(columnIndent)
        dtdBuilder.Append("(")
        dtdBuilder.Append(vbCrLf)

        For Each columnMap In columnMaps

            'Column
            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append("[")
            dtdBuilder.Append(columnMap.Name)
            dtdBuilder.Append("], ")
            dtdBuilder.Append(vbCrLf)

        Next

        dtdBuilder.Length -= 4
        dtdBuilder.Append(vbCrLf)

        dtdBuilder.Append(columnIndent)
        dtdBuilder.Append(") REFERENCES ")
        If Len(tableMap.SourceMap.Schema) > 0 Then
            dtdBuilder.Append("[" & tableMap.SourceMap.Schema)
            dtdBuilder.Append("].[")
        Else
            dtdBuilder.Append("[")
        End If
        dtdBuilder.Append(columnMap.PrimaryKeyTable)
        dtdBuilder.Append("] (")
        dtdBuilder.Append(vbCrLf)

        For Each columnMap In columnMaps

            'Primary Column
            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append(columnIndent)
            dtdBuilder.Append("[")
            dtdBuilder.Append(columnMap.PrimaryKeyColumn)
            dtdBuilder.Append("], ")
            dtdBuilder.Append(vbCrLf)

        Next

        dtdBuilder.Length -= 4
        dtdBuilder.Append(vbCrLf)

        dtdBuilder.Append(columnIndent)
        dtdBuilder.Append("), ")
        dtdBuilder.Append(vbCrLf)

        dtd = dtdBuilder.ToString

        Return dtd

    End Function

    Public Overrides Function TableDefaultConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap
        Dim columnIndent As String = "        "
        Dim defColumnMaps As ArrayList = tableMap.GetDefaultValueColumnMaps
        Dim hasComma As Boolean

        If defColumnMaps.Count > 0 Then

            'alter table
            'dtdBuilder.Append("ALTER TABLE [" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "] WITH NOCHECK ADD")
            dtdBuilder.Append("ALTER TABLE [" & tableMap.Name & "] ADD")
            dtdBuilder.Append(vbCrLf)

            For Each columnMap In defColumnMaps

                'Constraint
                dtdBuilder.Append(columnIndent)
                dtdBuilder.Append("CONSTRAINT [DF_")
                dtdBuilder.Append(tableMap.Name)
                dtdBuilder.Append("_")
                dtdBuilder.Append(columnMap.Name)
                dtdBuilder.Append("] DEFAULT ")
                dtdBuilder.Append(columnMap.DefaultValue)
                dtdBuilder.Append(" FOR [")
                dtdBuilder.Append(columnMap.Name)
                dtdBuilder.Append("], ")
                dtdBuilder.Append(vbCrLf)
                hasComma = True

            Next

            If hasComma Then

                dtdBuilder.Length -= 4
                dtdBuilder.Append(vbCrLf)

            End If

            dtdBuilder.Append("GO")
            dtdBuilder.Append(vbCrLf)
            dtdBuilder.Append(vbCrLf)

        End If

        dtd = dtdBuilder.ToString

        Return dtd


    End Function


    Public Overrides Function DropTableToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        'drop table
        'dtdBuilder.Append("if exists (select * from ")
        'dtdBuilder.Append(tableMap.SourceMap.Schema)
        'dtdBuilder.Append(".sysobjects where id = object_id(N'[")
        'dtdBuilder.Append(tableMap.SourceMap.Schema)
        'dtdBuilder.Append("].[")
        'dtdBuilder.Append(tableMap.Name)
        'dtdBuilder.Append("]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)")
        'dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append("drop table ")
        If Len(tableMap.SourceMap.Schema) > 0 Then
            dtdBuilder.Append("[" & tableMap.SourceMap.Schema)
            dtdBuilder.Append("].[")
        Else
            dtdBuilder.Append("[")
        End If
        dtdBuilder.Append(tableMap.Name)
        dtdBuilder.Append("]")
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append("GO")
        dtdBuilder.Append(vbCrLf)
        dtdBuilder.Append(vbCrLf)

        dtd = dtdBuilder.ToString

        Return dtd

    End Function


    Public Overrides Function DropTableForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap
        Dim testColumnMap As IColumnMap
        Dim keyColumnMaps As ArrayList = tableMap.GetForeignKeyColumnMaps
        Dim hasComma As Boolean
        Dim hashColumnMaps As New Hashtable
        Dim columnMapsList As ArrayList
        Dim columnMaps As ArrayList
        Dim ok As Boolean
        Dim i As Long
        Dim add As Boolean

        If keyColumnMaps.Count > 0 Then

            For Each columnMap In keyColumnMaps

                If Not hashColumnMaps.ContainsKey(LCase(columnMap.PrimaryKeyTable)) Then

                    hashColumnMaps(LCase(columnMap.PrimaryKeyTable)) = New ArrayList

                    columnMapsList = CType(hashColumnMaps(LCase(columnMap.PrimaryKeyTable)), ArrayList)

                    columnMapsList.Add(New ArrayList)

                Else

                    columnMapsList = CType(hashColumnMaps(LCase(columnMap.PrimaryKeyTable)), ArrayList)

                End If

                i = 0
                For Each columnMaps In columnMapsList

                    i += 1

                    ok = True

                    For Each testColumnMap In columnMaps

                        If testColumnMap.PrimaryKeyColumn = columnMap.PrimaryKeyColumn Then

                            ok = False
                            Exit For

                        End If

                    Next

                    If Not ok Then

                        If i = columnMapsList.Count Then

                            add = True

                            Exit For

                        Else

                            'Just continue looping

                        End If

                    Else

                        columnMaps.Add(columnMap)

                        Exit For

                    End If

                Next

            Next

            If add Then

                columnMaps = New ArrayList

                columnMaps.Add(columnMap)

                columnMapsList.Add(columnMaps)

            End If

            For Each columnMapsList In hashColumnMaps.Values

                For Each columnMaps In columnMapsList

                    dtdBuilder.Append(DropColumnsForeignKeyConstraintsToDTD(tableMap, columnMaps))

                    hasComma = True

                Next

            Next

        End If

        dtd = dtdBuilder.ToString

        Return dtd


    End Function


    Public Function DropColumnsForeignKeyConstraintsToDTD(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal columnMaps As ArrayList) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        Dim columnMap As IColumnMap

        'drop constraint
        'dtdBuilder.Append("if exists (select * from ")
        'dtdBuilder.Append(tableMap.SourceMap.Schema)
        'dtdBuilder.Append(".sysobjects where id = object_id(N'[")
        'dtdBuilder.Append(tableMap.SourceMap.Schema)
        'dtdBuilder.Append("].[FK_")
        'dtdBuilder.Append(tableMap.Name)

        'For Each columnMap In columnMaps

        '    dtdBuilder.Append("_")
        '    dtdBuilder.Append(columnMap.Name)

        'Next

        'dtdBuilder.Append("]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)")
        'dtdBuilder.Append(vbCrLf)


        'dtdBuilder.Append("ALTER TABLE ")
        'If Len(tableMap.SourceMap.Schema) > 0 Then
        '    dtdBuilder.Append("[" & tableMap.SourceMap.Schema)
        '    dtdBuilder.Append("].[")
        'Else
        '    dtdBuilder.Append("[")
        'End If
        'dtdBuilder.Append(tableMap.Name)
        'dtdBuilder.Append("]")
        'dtdBuilder.Append(" DROP CONSTRAINT ")
        'dtdBuilder.Append("FK_")
        'dtdBuilder.Append(tableMap.Name)
        'For Each columnMap In columnMaps

        '    dtdBuilder.Append("_")
        '    dtdBuilder.Append(columnMap.Name)

        'Next
        'dtdBuilder.Append(vbCrLf)
        'dtdBuilder.Append("GO")
        'dtdBuilder.Append(vbCrLf)
        'dtdBuilder.Append(vbCrLf)

        dtd = dtdBuilder.ToString

        Return dtd

    End Function


    Public Overrides Function ColumnToDTD(ByVal columnMap As Puzzle.NPersist.Framework.Mapping.IColumnMap) As String

        Dim dtd As String
        Dim dtdBuilder As New StringBuilder

        'create column
        dtdBuilder.Append("[" & columnMap.Name & "] ")

        If columnMap.IsAutoIncrease Then

            dtdBuilder.Append("COUNTER ")

        Else

            'Data type
            dtdBuilder.Append(GetDataType(columnMap) & " ")

        End If

        'Primary Key Constraint
        'If columnMap.IsPrimaryKey Then

        '    dtdBuilder.Append("CONSTRAINT pk_" & columnMap.TableMap.Name & " PRIMARY KEY")

        'Else

        'Allow/disallow null
        If Not columnMap.AllowNulls Then
            dtdBuilder.Append("NOT NULL")
        Else
            dtdBuilder.Append("NULL")
        End If

        'End If

        dtd = dtdBuilder.ToString

        Return dtd

    End Function


    Public Overrides Function GetDataType(ByVal columnMap As IColumnMap) As String

        Select Case columnMap.DataType

            Case DbType.AnsiString
                'TODO; Log diff?
                If columnMap.Precision < 1 Or columnMap.Precision > 255 Then
                    Return "text"
                Else
                    Return "varchar(" & columnMap.Precision & ")"
                End If
            Case DbType.AnsiStringFixedLength
                'TODO; Log diff?
                'Return "[char] (" & columnMap.Precision & ")"
                If columnMap.Precision < 1 Or columnMap.Precision > 255 Then
                    Return "text"
                Else
                    Return "varchar(" & columnMap.Precision & ")"
                End If
            Case DbType.Binary
                'If columnMap.IsFixedLength Then
                '    Return "[binary](" & columnMap.Length & ")"
                'Else
                '    Return "[varbinary](" & columnMap.Length & ")"
                'End If
                'Return "[binary](" & columnMap.Length & ")"
                Return "binary"
            Case DbType.Boolean
                Return "bit"
            Case DbType.Byte
                Return "byte"
            Case DbType.Currency
                'If columnMap.Length = 4 Then
                '    Return "[smallmoney]"
                'Else
                '    Return "[money]"
                'End If
                Return "currency"
            Case DbType.Date, DbType.Time
                'Return "[smalldatetime]"
                Return "datetime"
            Case DbType.DateTime
                If columnMap.Precision = 8 Then
                    'Return "[timestamp]"
                    Return "datetime"
                Else
                    Return "datetime"
                End If
            Case DbType.Decimal
                'Return "[decimal](" & columnMap.Precision & "," & columnMap.Scale & ")"
                Return "double"
            Case DbType.Double
                Return "double"
            Case DbType.Guid
                Return "guid"
            Case DbType.Int16
                Return "short"
            Case DbType.Int32
                Return "long"
            Case DbType.Int64
                Return "single"
            Case DbType.Object
                '                Return "[image]"
            Case DbType.[SByte]
                'Return "[tinyint]"
                Return "byte"
            Case DbType.Single
                Return "single"
            Case DbType.String
                If columnMap.Precision < 1 Or columnMap.Precision > 255 Then
                    Return "text"
                Else
                    Return "varchar(" & columnMap.Precision & ")"
                End If
            Case DbType.StringFixedLength
                'TODO; Log diff?
                'Return "[nchar] (" & columnMap.Precision & ")"
                If columnMap.Precision < 1 Or columnMap.Precision > 255 Then
                    Return "text"
                Else
                    Return "varchar(" & columnMap.Precision & ")"
                End If
            Case DbType.UInt16
                Return "short"
            Case DbType.UInt32
                Return "long"
            Case DbType.UInt64
                Return "single"
            Case DbType.VarNumeric
                'Return "[numeric](" & columnMap.Precision & "," & columnMap.Scale & ")"
                Return "double"

        End Select

    End Function

    Public Overrides Sub TablesToSource(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As System.Collections.Hashtable)

        Dim ctx As IContext = New Context(sourceMap.DomainMap)

        'Dim sql As String = "Select [name] From [" & sourceMap.Schema & "].[sysobjects] Where OBJECTPROPERTY(id, N'IsUserTable') = 1"

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(sourceMap)

        Dim i As Long

        Dim dt As System.Data.DataTable

        Dim con As OleDbConnection


        Dim name As String
        Dim tableMap As ITableMap
        Dim tableMapClone As ITableMap
        Dim hashExists As New Hashtable
        Dim result As Object
        Dim tableExists As Boolean

        Dim hasNonInsertableDirty As Boolean

        Try

            con = ds.GetConnection

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            result = DataTableToArray(dt)

            dt.Dispose()

            ds.ReturnConnection()

            'result = ctx.SqlExecutor.ExecuteArray(sql, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            Throw New Exception("An exception occurred when communication with the data source: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                name = result(2, i)

                hashExists(LCase(name)) = True

            Next

        End If

        For Each tableMap In sourceMap.TableMaps

            tableExists = True

            If Not hashExists.ContainsKey(LCase(tableMap.Name)) Then

                tableMapClone = tableMap.Clone

                tableMapClone.SourceMap = addToDomainMap.GetSourceMap(sourceMap.Name)

                LogDiff(tableMap, "The table '" & tableMap.Name & "' was found in the model but not in the data source.", DiffInfoEnum.NotFound, "Name", hashDiff)

                tableExists = False

            End If

            GetColumnsToSource(tableMap, addToDomainMap, hashDiff, tableExists, hasNonInsertableDirty, ctx)

        Next

        ctx.Dispose()

    End Sub

    Public Overrides Sub ColumnsToSource(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As System.Collections.Hashtable)

        Dim ctx As IContext = New Context(tableMap.SourceMap.DomainMap)

        Dim hasNonInsertableDirty As Boolean

        GetColumnsToSource(tableMap, addToDomainMap, hashDiff, False, hasNonInsertableDirty, ctx)

        ctx.Dispose()

    End Sub

    Protected Overridable Sub GetColumnsToSource(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByRef hashDiff As System.Collections.Hashtable, ByVal tableExists As Boolean, ByRef hasNonInsertableDirty As Boolean, ByVal ctx As IContext)

        Dim columnMap As IColumnMap
        Dim columnMapClone As IColumnMap

        If Not tableExists Then

            For Each columnMap In tableMap.ColumnMaps

                columnMapClone = columnMap.Clone

                columnMapClone.TableMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name).GetTableMap(tableMap.Name)

                LogDiff(columnMap, "The column '" & columnMap.Name & "' was found in the model but not in the data source", DiffInfoEnum.NotFound, "Name", hashDiff)

            Next

            Exit Sub

        End If

        'Dim sql As String = "Select [name], [xtype], [length], [isnullable], [type], [prec], [scale] From [" & tableMap.SourceMap.Schema & "].[syscolumns] Where [id] = object_id(N'[" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "]')"

        'Dim sqlConst As String = "Select [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[CONSTRAINT_TYPE], [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[COLUMN_NAME]" & _
        '    " From [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS], [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE]" & _
        '    " Where [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[TABLE_NAME]='" & tableMap.Name & "'" & _
        '    " And [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[TABLE_NAME]='" & tableMap.Name & "'" & _
        '    " And [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[CONSTRAINT_NAME] =" & _
        '    " [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[CONSTRAINT_NAME]"

        'Dim sqlForKey As String = "Select col_name(r.fkeyid, fk.fkey) ForiegnKeyColumn," & _
        '    " object_name(r.rkeyid) PrimaryKeyTable, col_name(r.rkeyid, fk.rkey) PrimaryKeyColumn" & _
        '    " From sysreferences r join sysforeignkeys fk on r.constid = fk.constid" & _
        '    " Join sysobjects s on r.constid = s.id and parent_obj = object_id('" & tableMap.Name & "')"

        'Dim sqlAutoInc As String = "Select SC.Name As FieldName, " & _
        '    " Ident_Seed(Info.Table_Name) As IdSeed, " & _
        '    " Ident_Incr(Info.Table_Name) As IdIncrement," & _
        '    " Ident_Current(Info.Table_Name) As IdCurrent" & _
        '    " From SysObjects AS SO" & _
        '    " Inner Join SysColumns As SC" & _
        '    " On SO.Id = SC.Id" & _
        '    " Inner Join information_schema.Tables As Info" & _
        '    " On Info.Table_Name = SO.Name" & _
        '    " Where SO.Name = '" & tableMap.Name & "'" & _
        '    " And ColumnProperty(OBJECT_ID('" & tableMap.Name & "'), SC.Name, 'IsIdentity') = 1"

        'Dim sqlDef As String = "select [COLUMN_NAME], [COLUMN_DEFAULT] from [INFORMATION_SCHEMA].[COLUMNS] where [TABLE_NAME]='" & tableMap.Name & "'"

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(tableMap.SourceMap)

        Dim dt As System.Data.DataTable

        Dim con As OleDbConnection

        Dim orgColumnMap As IColumnMap

        Dim i As Long
        Dim j As Long
        Dim k As Long

        Dim result As Object
        Dim resultConst As Object
        Dim resultForKey As Object
        Dim resultAutoInc As Object
        Dim resultDef As Object

        Dim addToSourceMap As ISourceMap
        Dim addToTableMap As ITableMap

        Dim colName As String
        'Dim colXtype As Integer
        Dim colFlags As Integer
        Dim colLength As Long
        Dim colOctLength As Long
        Dim colIsNullable As Integer
        Dim colType As Integer
        Dim colPrec As Integer
        Dim colScale As Integer

        Dim constColName As String
        Dim constType As String

        Dim isPrimKey As Boolean
        Dim isAutoInc As Boolean

        Dim isForKey As Boolean
        Dim forKeyCol As String
        Dim forKeyTbl As String
        Dim primKeyTable As String
        Dim primKeyCol As String

        Dim incCol As String
        Dim incSeed As Integer
        Dim incInc As Integer

        Dim hasDef As Boolean
        Dim defCol As String
        Dim defVal As String

        Dim dteFormat As String

        Dim diffMsgs As New ArrayList
        Dim diffSettings As New ArrayList

        Dim name As String
        Dim hashExists As New Hashtable

        Try

            con = ds.GetConnection

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, tableMap.Name})

            result = DataTableToArray(dt)

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, New Object() {Nothing, Nothing, tableMap.Name})

            resultConst = DataTableToArray(dt)

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, Nothing)

            resultForKey = DataTableToArray(dt)

            dt.Dispose()

            ds.ReturnConnection()

            'result = ctx.SqlExecutor.ExecuteArray(sql, ds)
            'resultConst = ctx.SqlExecutor.ExecuteArray(sqlConst, ds)
            'resultForKey = ctx.SqlExecutor.ExecuteArray(sqlForKey, ds)
            'resultAutoInc = ctx.SqlExecutor.ExecuteArray(sqlAutoInc, ds)
            'resultDef = ctx.SqlExecutor.ExecuteArray(sqlDef, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            Throw New Exception("An exception occurred when communication with the data source: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                name = result(3, i)

                hashExists(LCase(name)) = True

            Next

        End If

        For Each columnMap In tableMap.ColumnMaps

            If Not hashExists.ContainsKey(LCase(columnMap.Name)) Then

                If IsNonInsertableColumn(columnMap) Then

                    addToSourceMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name)
                    addToTableMap = addToSourceMap.GetTableMap(tableMap.Name)

                    If addToTableMap Is Nothing Then

                        addToTableMap = tableMap.Clone

                        addToTableMap.SourceMap = addToSourceMap

                        LogDiff(tableMap, "The table '" & tableMap.Name & "' must  be dropped because it contains non insertable columns that were found in the model but not in the data source, or differed in some setting.", DiffInfoEnum.NotFound, "ColumnMaps", hashDiff)

                    End If

                    For Each orgColumnMap In tableMap.ColumnMaps

                        columnMapClone = orgColumnMap.Clone

                        columnMapClone.TableMap = addToTableMap

                        If orgColumnMap Is columnMap Then

                            LogDiff(columnMapClone, "OBS! Because this column is not insertable (it does not allow nulls and does not have a default value) it will cause the table to be dropped!", DiffInfoEnum.NotEqual, "AllowNulls", hashDiff)
                            LogDiff(columnMapClone, "The column '" & columnMapClone.Name & "' was found in the model but not in the data source.", DiffInfoEnum.NotEqual, "Name", hashDiff)

                        Else

                            LogDiff(columnMapClone, "The table that the column '" & columnMapClone.Name & "' belongs to must be dropped because it contains non insertable columns that are not found in the data source or differ from the model.", DiffInfoEnum.NotEqual, "Name", hashDiff)

                        End If

                    Next

                    Exit Sub

                End If

                columnMapClone = columnMap.Clone

                addToSourceMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name)
                addToTableMap = addToSourceMap.GetTableMap(tableMap.Name)

                If addToTableMap Is Nothing Then

                    addToTableMap = tableMap.Clone

                    addToTableMap.SourceMap = addToSourceMap

                    LogDiff(tableMap, "The table '" & tableMap.Name & "' contains columns that were found in the model but not in the data source, or differed in some setting.", DiffInfoEnum.NotFound, "ColumnMaps", hashDiff)

                End If

                columnMapClone.TableMap = addToTableMap

                LogDiff(columnMap, "The column '" & columnMap.Name & "' was found in the model but not in the data source.", DiffInfoEnum.NotFound, "Name", hashDiff)

            Else

                diffMsgs.Clear()
                diffSettings.Clear()

                colName = columnMap.Name
                'colXtype = 0
                colFlags = 0
                colLength = 0
                colOctLength = 0
                colIsNullable = 0
                colType = 0
                colPrec = 0
                colScale = 0

                isPrimKey = False
                isAutoInc = False

                isForKey = False
                primKeyCol = ""
                primKeyTable = ""

                incSeed = 0
                incInc = 0

                hasDef = False
                defVal = ""

                dteFormat = ""

                i = 0

                For i = 0 To UBound(result, 2)
                    name = result(3, i)
                    If LCase(name) = LCase(columnMap.Name) Then
                        Exit For
                    End If
                Next

                If Not IsDBNull(result(3, i)) Then colName = result(3, i)
                'If Not IsDBNull(result(1, i)) Then colXtype = result(1, i)
                If Not IsDBNull(result(9, i)) Then colFlags = result(9, i)
                If Not IsDBNull(result(10, i)) Then colIsNullable = result(10, i)
                If Not IsDBNull(result(11, i)) Then colType = result(11, i)
                If Not IsDBNull(result(13, i)) Then colLength = result(13, i)
                If Not IsDBNull(result(14, i)) Then colOctLength = result(14, i)
                If Not IsDBNull(result(15, i)) Then colPrec = result(15, i)
                If Not IsDBNull(result(16, i)) Then colScale = result(16, i)

                If Not IsDBNull(result(7, i)) Then hasDef = CBool(result(7, i))

                If hasDef Then
                    If Not IsDBNull(result(8, i)) Then defVal = result(8, i)
                End If

                If colType = 130 Then
                    colPrec = colLength
                    colLength = colOctLength
                End If

                If IsArray(resultConst) Then

                    For j = 0 To UBound(resultConst, 2)

                        constColName = ""

                        If Not IsDBNull(resultConst(3, j)) Then constColName = resultConst(3, j)

                        If constColName = colName Then

                            isPrimKey = True
                            Exit For

                        End If

                    Next

                End If

                If IsArray(resultForKey) Then

                    For j = 0 To UBound(resultForKey, 2)

                        forKeyCol = ""
                        forKeyTbl = ""

                        If Not IsDBNull(resultForKey(8, j)) Then forKeyTbl = resultForKey(8, j)
                        If Not IsDBNull(resultForKey(9, j)) Then forKeyCol = resultForKey(9, j)

                        If forKeyTbl = tableMap.Name Then

                            If forKeyCol = colName Then

                                isForKey = True
                                If Not IsDBNull(resultForKey(2, j)) Then primKeyTable = resultForKey(2, j)
                                If Not IsDBNull(resultForKey(3, j)) Then primKeyCol = resultForKey(3, j)

                                Exit For

                            End If

                        End If

                    Next

                End If

                If colType = 3 And colFlags = 90 Then

                    isAutoInc = True
                    incSeed = 1
                    incInc = 1

                End If


                If Not CompareTypes(colType, colLength, columnMap) Then
                    diffMsgs.Add("The data type for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & GetDbType(colType, colLength, columnMap).ToString & "', model: '" & columnMap.DataType.ToString & "')")
                    diffSettings.Add("DataType")
                End If

                'If colXtype = 173 Then

                '    If Not columnMap.IsFixedLength Then

                '        diffMsgs.Add("The 'fixed length' setting for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'True', model: 'False')")
                '        diffSettings.Add("DataType")

                '    End If

                'End If

                'If colXtype = 165 Then

                '    If columnMap.IsFixedLength Then

                '        diffMsgs.Add("The 'fixed length' setting for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'False', model: 'True')")
                '        diffSettings.Add("DataType")

                '    End If

                'End If

                If Not columnMap.Length = colLength Then
                    If colType = 130 Then
                        diffMsgs.Add("The length for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colLength & "', model: '" & columnMap.Length & "')")
                        diffSettings.Add("Length")
                    End If
                End If

                If Not columnMap.Precision = colPrec Then
                    If colType = 130 Then
                        diffMsgs.Add("The precision for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colPrec & "', model: '" & columnMap.Precision & "')")
                        diffSettings.Add("Precision")
                    End If
                End If

                If Not columnMap.Scale = colScale Then
                    If colType = 130 Then
                        diffMsgs.Add("The scale for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colScale & "', model: '" & columnMap.Scale & "')")
                        diffSettings.Add("Scale")
                    End If
                End If


                If colIsNullable = 1 And columnMap.AllowNulls = False Then
                    diffMsgs.Add("The nullability for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'True', model: '" & columnMap.AllowNulls & "')")
                    diffSettings.Add("AllowNulls")
                ElseIf colIsNullable = 0 And columnMap.AllowNulls = True Then
                    diffMsgs.Add("The nullability for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'False', model: '" & columnMap.AllowNulls & "')")
                    diffSettings.Add("AllowNulls")
                End If

                If isAutoInc Then
                    If Not columnMap.IsAutoIncrease Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is an auto increaser.")
                        diffSettings.Add("IsAutoIncrease")
                    End If
                Else
                    If columnMap.IsAutoIncrease Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is not an auto increaser.")
                        diffSettings.Add("IsAutoIncrease")
                    End If
                End If

                If Not columnMap.Seed = incSeed Then
                    diffMsgs.Add("The auto increaser seed for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & incSeed & "', model: '" & columnMap.Seed & "')")
                    diffSettings.Add("Seed")
                End If

                If Not columnMap.Increment = incInc Then
                    diffMsgs.Add("The auto increaser increment for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & incInc & "', model: '" & columnMap.Increment & "')")
                    diffSettings.Add("Increment")
                End If

                If isPrimKey Then
                    If Not columnMap.IsPrimaryKey Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is a primary key.")
                        diffSettings.Add("IsPrimaryKey")
                    End If
                Else
                    If columnMap.IsPrimaryKey Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is not a primary key.")
                        diffSettings.Add("IsPrimaryKey")
                    End If
                End If

                If isForKey Then
                    If Not columnMap.IsForeignKey Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is a foreign key.")
                        diffSettings.Add("IsPrimaryKey")
                    End If
                    If Not columnMap.PrimaryKeyColumn = primKeyCol Then
                        diffMsgs.Add("The primary column referenced by the foreign key column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & primKeyCol & "', model: '" & columnMap.PrimaryKeyColumn & "')")
                        diffSettings.Add("PrimaryKeyColumn")
                    End If
                    If Not columnMap.PrimaryKeyTable = primKeyTable Then
                        diffMsgs.Add("The primary table referenced by the foreign key column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & primKeyTable & "', model: '" & columnMap.PrimaryKeyTable & "')")
                        diffSettings.Add("PrimaryKeyTable")
                    End If
                Else
                    If columnMap.IsForeignKey Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is not a foreign key.")
                        diffSettings.Add("IsPrimaryKey")
                    End If
                    If Len(columnMap.PrimaryKeyColumn) Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is not a foreign key, but the column in the model has a reference to a primary key column.")
                        diffSettings.Add("PrimaryKeyColumn")
                    End If
                    If Len(columnMap.PrimaryKeyTable) Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source is not a foreign key, but the column in the model has a reference to a primary key table.")
                        diffSettings.Add("PrimaryKeyTable")
                    End If
                End If

                If hasDef Then
                    If Not columnMap.DefaultValue = defVal Then
                        diffMsgs.Add("The default value for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & defVal & "', model: '" & columnMap.DefaultValue & "')")
                        diffSettings.Add("PrimaryKeyTable")
                    End If
                Else
                    If Len(columnMap.DefaultValue) > 0 Then
                        diffMsgs.Add("The column '" & columnMap.Name & "' in the data source has no default value. (model: '" & columnMap.DefaultValue & "')")
                        diffSettings.Add("PrimaryKeyTable")
                    End If
                End If

                If diffMsgs.Count > 0 Then

                    If IsNonInsertableColumn(columnMap) Then

                        addToSourceMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name)
                        addToTableMap = addToSourceMap.GetTableMap(tableMap.Name)

                        If addToTableMap Is Nothing Then

                            addToTableMap = tableMap.Clone

                            addToTableMap.SourceMap = addToSourceMap

                            LogDiff(tableMap, "The table '" & tableMap.Name & "' must  be dropped because it contains non insertable columns that were found in the model but not in the data source, or differed in some setting.", DiffInfoEnum.NotFound, "ColumnMaps", hashDiff)

                        Else

                            'Obs! Must clear because other columns may already have been added!
                            addToTableMap.ColumnMaps.Clear()

                        End If

                        For Each orgColumnMap In tableMap.ColumnMaps

                            columnMapClone = orgColumnMap.Clone

                            columnMapClone.TableMap = addToTableMap

                            If Not orgColumnMap Is columnMap Then

                                LogDiff(columnMapClone, "The table that the column '" & columnMapClone.Name & "' belongs to must be dropped because it contains non insertable columns that are not found in the data source or differ from the model.", DiffInfoEnum.NotEqual, "Name", hashDiff)

                            Else

                                LogDiff(columnMapClone, "OBS! Because this column is not insertable (it does not allow nulls and does not have a default value) it will cause the table to be dropped!", DiffInfoEnum.NotEqual, "AllowNulls", hashDiff)

                                For k = 0 To diffMsgs.Count - 1

                                    LogDiff(columnMapClone, diffMsgs(k), DiffInfoEnum.NotEqual, diffSettings(k), hashDiff)

                                Next

                            End If

                        Next

                        Exit Sub

                    End If

                    columnMapClone = columnMap.Clone

                    addToSourceMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name)
                    addToTableMap = addToSourceMap.GetTableMap(tableMap.Name)

                    If addToTableMap Is Nothing Then

                        addToTableMap = tableMap.Clone

                        addToTableMap.SourceMap = addToSourceMap

                        LogDiff(tableMap, "The table '" & tableMap.Name & "' contains columns that were found in the model but not in the data source, or differed in some setting.", DiffInfoEnum.NotFound, "ColumnMaps", hashDiff)

                    End If

                    columnMapClone.TableMap = addToTableMap

                    For k = 0 To diffMsgs.Count - 1

                        LogDiff(columnMapClone, diffMsgs(k), DiffInfoEnum.NotEqual, diffSettings(k), hashDiff)

                    Next

                End If

            End If

        Next

    End Sub

    Protected Overridable Function HasNonInsertableColumns(ByVal tableMap As ITableMap) As Boolean

        Dim columnMap As IColumnMap

        For Each columnMap In tableMap.ColumnMaps

            If IsNonInsertableColumn(columnMap) Then

                Return True

            End If

        Next

    End Function

    Protected Overridable Function IsNonInsertableColumn(ByVal columnMap As IColumnMap) As Boolean

        If Not columnMap.AllowNulls Then

            If Not Len(columnMap.DefaultValue) > 0 Then

                Return True

            End If

        End If

    End Function
    Protected Overridable Function CompareTypes(ByVal colType As Integer, ByVal colLength As Long, ByVal columnMap As IColumnMap) As Boolean

        Dim dataType As DbType = columnMap.DataType

        Select Case colType

            Case 2
                If dataType = DbType.Int16 Then Return True
            Case 3
                If dataType = DbType.Int32 Then Return True
            Case 4
                If dataType = DbType.Single Then Return True
            Case 5
                If dataType = DbType.Double Then Return True
            Case 6
                If dataType = DbType.Currency Then Return True
            Case 7
                If dataType = DbType.DateTime Then Return True
            Case 11
                If dataType = DbType.Boolean Then Return True
            Case 17
                If dataType = DbType.Byte Then Return True
            Case 72
                If dataType = DbType.Guid Then Return True
            Case 128
                If dataType = DbType.Binary Then Return True
                'TODO: Check colLength
            Case 130
                If dataType = DbType.String Then Return True
                'TODO: Check colLength
            Case 131
                If dataType = DbType.Decimal Then Return True


        End Select

        Return False

    End Function


    Protected Overridable Function GetDbType(ByVal colType As Integer, ByVal colLength As Long, ByVal columnMap As IColumnMap) As DbType

        Dim dataType As DbType = columnMap.DataType

        'Short	2	5
        'Long	3	10
        'Single	4	7
        'Double	5	15
        'Currency	6	19
        'DateTime	7	8
        'Bit	11	2
        'Byte	17	3
        'GUID	72	16
        'BigBinary	128	4000
        'LongBinary	128	1073741823
        'VarBinary	128	510
        'LongText	130	536870910
        'VarChar	130	255
        'Decimal	131	28

        Select Case colType
            Case 2
                Return DbType.Int16
            Case 3
                Return DbType.Int32
            Case 4
                Return DbType.Single
            Case 5
                Return DbType.Double
            Case 6
                Return DbType.Currency
            Case 7
                Return DbType.DateTime
            Case 11
                Return DbType.Boolean
            Case 17
                Return DbType.Byte
            Case 72
                Return DbType.Guid
            Case 128
                Return DbType.Binary
                'TODO: Check colLength
            Case 130
                Return DbType.String
                'TODO: Check colLength
            Case 131
                Return DbType.Decimal


        End Select

    End Function


    Protected Overridable Sub LogDiff(ByVal mapObject As IMap, ByVal message As String, ByVal diffInfo As DiffInfoEnum, ByVal setting As String, ByRef hashDiff As Hashtable)

        Dim key As String = mapObject.GetKey

        If Not hashDiff.ContainsKey(key) Then

            hashDiff(key) = New ArrayList

        End If

        CType(hashDiff(key), ArrayList).Add(New diffInfo(mapObject, message, diffInfo, setting))

    End Sub


    Protected Overridable Function DataTableToArray(ByVal dt As DataTable) As Object

        Dim row As DataRow

        Dim colCnt As Integer = dt.Columns.Count - 1

        Dim arrRet(colCnt, 0) As Object
        Dim i As Integer
        Dim HadRows As Boolean


        For Each row In dt.Rows

            'Fill array row with values from reader
            For i = 0 To colCnt
                arrRet(i, UBound(arrRet, 2)) = row(i)
            Next

            'Add row to array
            ReDim Preserve arrRet(colCnt, UBound(arrRet, 2) + 1)

            HadRows = True

        Next

        'Trim last (empty) row
        ReDim Preserve arrRet(colCnt, UBound(arrRet, 2) - 1)

        If Not HadRows Then arrRet = Nothing

        Return arrRet

    End Function

End Class

