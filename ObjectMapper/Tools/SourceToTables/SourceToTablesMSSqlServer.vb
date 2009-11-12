Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Persistence

Public Class SourceToTablesMSSqlServer
    Inherits SourceToTablesBase

    'select * from dbo.sysobjects where OBJECTPROPERTY(id, N'IsUserTable') = 1

    'select * from dbo.syscolumns where id = object_id(N'[dbo].[tblProjects]')

    'OBS: type and usertype is not unique, thus we must go with xtype
    'However, to dicern IDENTITY, we have to compare type
    '(56 for ID, 38 for int)
    'name, type, xtype, usertype
    'bigint	        108	127	0
    'binary	        37	173	3
    'bit	            50	104	16
    'char	            39	175	1
    'datetime	        111	61	12
    'decimal	        106	106	24
    'float	            109	62	8
    'image	            34	34	20
    'int	            38	56	7
    'money	            110	60	11
    'nchar	            39	239	0
    'ntext	            35	99	0
    'numeric	        108	108	10
    'nvarchar	        39	231	0
    'real	            109	59	23
    'smalldatetime	    111	58	22
    'smallint	        38	52	6
    'smallmoney	    110	122	21
    'sql_variant	    39	98	0
    'text	            35	35	19
    'timestamp	        37	189	80
    'tinyint	        38	48	5
    'uniqueidentifier	37	36	0
    'varbinary	        37	165	4
    'varchar	        39	167	2
    'ID	            56	56	7

    Public Overloads Overrides Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByRef hashDiff As Hashtable)

    End Sub

    Public Overloads Overrides Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(sourceMap.DomainMap)

        Dim sql As String = "Select [name] From [" & sourceMap.Schema & "].[sysobjects] Where OBJECTPROPERTY(id, N'IsUserTable') = 1"

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(sourceMap)

        Dim i As Long

        Dim name As String
        Dim tableMap As ITableMap
        Dim result As Object

        Try

            result = ctx.SqlExecutor.ExecuteArray(sql, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            ctx.Dispose()

            Throw New Exception("Encountered an exception when communicating with database: " & ex.Message, ex)

        End Try

        ds.ReturnConnection()

        Try

            If IsArray(result) Then

                For i = 0 To UBound(result, 2)

                    name = result(0, i)

                    If Not name = "dtproperties" Then

                        tableMap = sourceMap.GetTableMap(name)

                        If tableMap Is Nothing Then

                            tableMap = New tableMap

                            tableMap.Name = name

                            If addToDomainMap Is Nothing Then

                                tableMap.SourceMap = sourceMap

                            Else

                                tableMap.SourceMap = addToDomainMap.GetSourceMap(sourceMap.Name)

                            End If

                            LogDiff(tableMap, "The table '" & tableMap.Name & "' was found in the source but not in the model", DiffInfoEnum.NotFound, "Name", hashDiff)

                        End If

                        GetColumns(tableMap, ctx, addToDomainMap, hashDiff)

                    End If

                Next

            End If

        Catch ex As Exception

            ctx.Dispose()

            Throw ex

        End Try


        ctx.Dispose()

    End Sub

    Public Overloads Overrides Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(tableMap.SourceMap.DomainMap)

        Try

            GetColumns(tableMap, ctx, addToDomainMap, hashDiff)

        Catch ex As Exception

            ctx.Dispose()

            Throw ex

        End Try

        ctx.Dispose()

    End Sub

    Public Overloads Overrides Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(tableMap.SourceMap.DomainMap)

        Try

            GetColumns(tableMap, ctx, Nothing, hashDiff)

        Catch ex As Exception

            ctx.Dispose()

            Throw ex

        End Try

        ctx.Dispose()

    End Sub

    Protected Overridable Sub GetColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal ctx As IContext, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

        Dim sql As String = "Select [name], [xtype], [length], [isnullable], [type], [prec], [scale] From [" & tableMap.SourceMap.Schema & "].[syscolumns] Where [id] = object_id(N'[" & tableMap.SourceMap.Schema & "].[" & tableMap.Name & "]')"

        Dim sqlConst As String = "Select [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[CONSTRAINT_TYPE], [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[COLUMN_NAME]" & _
            " From [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS], [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE]" & _
            " Where [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[TABLE_NAME]='" & tableMap.Name & "'" & _
            " And [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[TABLE_NAME]='" & tableMap.Name & "'" & _
            " And [INFORMATION_SCHEMA].[KEY_COLUMN_USAGE].[CONSTRAINT_NAME] =" & _
            " [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS].[CONSTRAINT_NAME]"

        Dim sqlForKey As String = "Select col_name(r.fkeyid, fk.fkey) ForiegnKeyColumn," & _
            " object_name(r.rkeyid) PrimaryKeyTable, col_name(r.rkeyid, fk.rkey) PrimaryKeyColumn, [name]" & _
            " From sysreferences r join sysforeignkeys fk on r.constid = fk.constid" & _
            " Join sysobjects s on r.constid = s.id and parent_obj = object_id('" & tableMap.Name & "')"

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

        Dim sqlAutoInc As String = "Select SC.Name As FieldName, " & _
            " Ident_Seed(Info.Table_Name) As IdSeed, " & _
            " Ident_Incr(Info.Table_Name) As IdIncrement" & _
            " From SysObjects AS SO" & _
            " Inner Join SysColumns As SC" & _
            " On SO.Id = SC.Id" & _
            " Inner Join information_schema.Tables As Info" & _
            " On Info.Table_Name = SO.Name" & _
            " Where SO.Name = '" & tableMap.Name & "'" & _
            " And ColumnProperty(OBJECT_ID('" & tableMap.Name & "'), SC.Name, 'IsIdentity') = 1"

        Dim sqlDef As String = "select [COLUMN_NAME], [COLUMN_DEFAULT] from [INFORMATION_SCHEMA].[COLUMNS] where [TABLE_NAME]='" & tableMap.Name & "'"

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(tableMap.SourceMap)



        Dim i As Long
        Dim j As Long
        Dim k As Long

        Dim columnMap As IColumnMap
        Dim result As Object
        Dim resultConst As Object
        Dim resultForKey As Object
        Dim resultAutoInc As Object
        Dim resultDef As Object

        Dim addToSourceMap As ISourceMap
        Dim addToTableMap As ITableMap

        Dim colName As String
        Dim colXtype As Integer
        Dim colLength As Integer
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
        Dim primKeyTable As String
        Dim primKeyCol As String
        Dim forKeyName As String

        Dim incCol As String
        Dim incSeed As Integer
        Dim incInc As Integer

        Dim hasDef As Boolean
        Dim defCol As String
        Dim defVal As String

        Dim dteFormat As String

        Dim diffMsgs As New ArrayList
        Dim diffSettings As New ArrayList

        Try

            result = ctx.SqlExecutor.ExecuteArray(sql, ds)
            resultConst = ctx.SqlExecutor.ExecuteArray(sqlConst, ds)
            resultForKey = ctx.SqlExecutor.ExecuteArray(sqlForKey, ds)
            resultAutoInc = ctx.SqlExecutor.ExecuteArray(sqlAutoInc, ds)
            resultDef = ctx.SqlExecutor.ExecuteArray(sqlDef, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            'ctx.Dispose()

            Throw New Exception("Encountered an exception when communicating with database: " & ex.Message, ex)

        End Try

        ds.ReturnConnection()

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                diffMsgs.Clear()
                diffSettings.Clear()

                colName = ""
                colXtype = 0
                colLength = 0
                colIsNullable = 0
                colType = 0
                colPrec = 0
                colScale = 0

                isPrimKey = False
                isAutoInc = False

                isForKey = False
                primKeyCol = ""
                primKeyTable = ""
                forKeyName = ""

                incSeed = 0
                incInc = 0

                hasDef = False
                defVal = ""

                dteFormat = ""

                If Not IsDBNull(result(0, i)) Then colName = result(0, i)
                If Not IsDBNull(result(1, i)) Then colXtype = result(1, i)
                If Not IsDBNull(result(2, i)) Then colLength = result(2, i)
                If Not IsDBNull(result(3, i)) Then colIsNullable = result(3, i)
                If Not IsDBNull(result(4, i)) Then colType = result(4, i)
                If Not IsDBNull(result(5, i)) Then colPrec = result(5, i)
                If Not IsDBNull(result(6, i)) Then colScale = result(6, i)

                If IsArray(resultConst) Then

                    For j = 0 To UBound(resultConst, 2)

                        constColName = ""
                        constType = ""

                        If Not IsDBNull(resultConst(0, j)) Then constType = resultConst(0, j)
                        If Not IsDBNull(resultConst(1, j)) Then constColName = resultConst(1, j)

                        If constColName = colName Then

                            If UCase(constType) = "PRIMARY KEY" Then

                                isPrimKey = True
                                Exit For

                            End If

                        End If

                    Next

                End If

                If IsArray(resultForKey) Then

                    For j = 0 To UBound(resultForKey, 2)

                        forKeyCol = ""
                        forKeyName = ""

                        If Not IsDBNull(resultForKey(0, j)) Then forKeyCol = resultForKey(0, j)

                        If forKeyCol = colName Then

                            isForKey = True
                            If Not IsDBNull(resultForKey(1, j)) Then primKeyTable = resultForKey(1, j)
                            If Not IsDBNull(resultForKey(2, j)) Then primKeyCol = resultForKey(2, j)
                            If Not IsDBNull(resultForKey(3, j)) Then forKeyName = resultForKey(3, j)

                            Exit For

                        End If

                    Next

                End If

                If IsArray(resultAutoInc) Then

                    For j = 0 To UBound(resultAutoInc, 2)

                        incCol = ""

                        If Not IsDBNull(resultAutoInc(0, j)) Then incCol = resultAutoInc(0, j)

                        If incCol = colName Then

                            isAutoInc = True
                            If Not IsDBNull(resultAutoInc(1, j)) Then incSeed = resultAutoInc(1, j)
                            If Not IsDBNull(resultAutoInc(2, j)) Then incInc = resultAutoInc(2, j)

                        End If

                    Next

                End If

                If IsArray(resultDef) Then

                    For j = 0 To UBound(resultDef, 2)

                        defCol = ""

                        If Not IsDBNull(resultDef(0, j)) Then defCol = resultDef(0, j)

                        If defCol = colName Then

                            hasDef = True
                            If Not IsDBNull(resultDef(1, j)) Then defVal = resultDef(1, j)

                        End If

                    Next

                End If

                columnMap = tableMap.GetColumnMap(colName)

                If Not columnMap Is Nothing Then

                    If Len(columnMap.Format) > 0 Then
                        dteFormat = columnMap.Format
                    End If

                    If Not CompareTypes(colType, colXtype, columnMap) Then
                        diffMsgs.Add("The data type for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & GetDbType(colType, colXtype, columnMap).ToString & "', model: '" & columnMap.DataType.ToString & "')")
                        diffSettings.Add("DataType")
                    End If

                    If colXtype = 173 Then

                        If Not columnMap.IsFixedLength Then

                            diffMsgs.Add("The 'fixed length' setting for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'True', model: 'False')")
                            diffSettings.Add("DataType")

                        End If

                    End If

                    If colXtype = 165 Then

                        If columnMap.IsFixedLength Then

                            diffMsgs.Add("The 'fixed length' setting for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: 'False', model: 'True')")
                            diffSettings.Add("DataType")

                        End If

                    End If

                    If Not columnMap.Length = colLength Then
                        diffMsgs.Add("The length for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colLength & "', model: '" & columnMap.Length & "')")
                        diffSettings.Add("Length")
                    End If

                    If Not columnMap.Precision = colPrec Then
                        diffMsgs.Add("The precision for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colPrec & "', model: '" & columnMap.Precision & "')")
                        diffSettings.Add("Precision")
                    End If

                    If Not columnMap.Scale = colScale Then
                        diffMsgs.Add("The scale for the column '" & columnMap.Name & "' in the data source did not match the model. (data source: '" & colScale & "', model: '" & columnMap.Scale & "')")
                        diffSettings.Add("Scale")
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
                        If Not columnMap.ForeignKeyName = forKeyName Then
                            diffMsgs.Add("The foreign key column '" & columnMap.Name & "' in the data source and the corresponding column in the model are not part of the same foreign key. (data source: '" & forKeyName & "', model: '" & columnMap.ForeignKeyName & "')")
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

                End If

                'If there are differences we should throw away the existing column map
                'and create a new one with the right settings.
                If diffMsgs.Count > 0 Then

                    columnMap = Nothing

                End If

                If columnMap Is Nothing Then

                    columnMap = New columnMap

                    columnMap.Name = colName

                    If addToDomainMap Is Nothing Then

                        columnMap.TableMap = tableMap

                    Else

                        addToSourceMap = addToDomainMap.GetSourceMap(tableMap.SourceMap.Name)

                        addToTableMap = addToSourceMap.GetTableMap(tableMap.Name)

                        If addToTableMap Is Nothing Then

                            addToTableMap = New tableMap

                            addToTableMap.Name = tableMap.Name

                            addToTableMap.SourceMap = addToSourceMap

                        End If

                        columnMap.TableMap = addToTableMap

                        If diffMsgs.Count > 0 Then

                            For k = 0 To diffMsgs.Count - 1

                                LogDiff(columnMap, diffMsgs(k), DiffInfoEnum.NotEqual, diffSettings(k), hashDiff)

                            Next

                        Else

                            LogDiff(columnMap, "The column '" & columnMap.Name & "' was found in the source but not in the model", DiffInfoEnum.NotFound, "Name", hashDiff)

                        End If

                    End If

                End If

                columnMap.DataType = GetDbType(colType, colXtype, columnMap)
                If columnMap.DataType = DbType.Binary Then
                    'Timestamp column
                    If colXtype = 189 Then

                        'check that it works then perhaps remove comment....
                        'columnMap.Precision = 8

                        If colIsNullable Then
                            columnMap.IsFixedLength = False
                        Else
                            columnMap.IsFixedLength = True
                        End If

                        columnMap.SpecificDataType = "TIMESTAMP"

                    Else

                        'Binary column
                        If colXtype = 165 Then
                            columnMap.IsFixedLength = False
                        Else
                            columnMap.IsFixedLength = True
                        End If

                    End If
                End If
                'ECO hack..
                Select Case colXtype
                    Case 99
                        columnMap.SpecificDataType = "NTEXT"
                    Case 35
                        columnMap.SpecificDataType = "TEXT"
                End Select
                columnMap.Length = colLength
                columnMap.Precision = colPrec
                columnMap.Scale = colScale
                columnMap.IsAutoIncrease = isAutoInc
                columnMap.IsPrimaryKey = isPrimKey
                columnMap.IsForeignKey = isForKey
                columnMap.Seed = incSeed
                columnMap.Increment = incInc
                columnMap.ForeignKeyName = forKeyName
                columnMap.PrimaryKeyColumn = primKeyCol
                columnMap.PrimaryKeyTable = primKeyTable
                columnMap.DefaultValue = defVal
                columnMap.Format = dteFormat

                If colIsNullable = 1 Then
                    columnMap.AllowNulls = True
                Else
                    columnMap.AllowNulls = False
                End If

            Next

        End If



    End Sub




    Protected Overridable Function CompareTypes(ByVal systype As Integer, ByVal xtype As Integer, ByVal columnMap As IColumnMap) As Boolean

        Dim dataType As DbType = columnMap.DataType

        Select Case xtype
            Case 127
                If dataType = DbType.Int64 Then Return True
            Case 173
                If dataType = DbType.Binary Then Return True
            Case 104
                If dataType = DbType.Boolean Then Return True
            Case 175
                If dataType = DbType.AnsiStringFixedLength And (columnMap.Precision > 0 And columnMap.Precision < 8000) Then Return True
            Case 61
                If dataType = DbType.DateTime Then Return True
            Case 106
                If dataType = DbType.Decimal Then Return True
            Case 62
                If dataType = DbType.Double Then Return True
            Case 34
                If dataType = DbType.Object Then Return True
            Case 56
                If dataType = DbType.Int32 Then Return True
            Case 60
                If dataType = DbType.Currency And columnMap.Length = 8 Then Return True
            Case 239
                If dataType = DbType.StringFixedLength And (columnMap.Precision > 0 And columnMap.Precision < 4000) Then Return True
            Case 99
                If (dataType = DbType.String Or dataType = DbType.StringFixedLength) And (columnMap.Precision < 1 Or columnMap.Precision > 4000) Then Return True
            Case 108
                If dataType = DbType.VarNumeric Then Return True
            Case 231
                If dataType = DbType.String And (columnMap.Precision > 0 And columnMap.Precision < 4000) Then Return True
            Case 59
                If dataType = DbType.Single Then Return True
            Case 58
                If dataType = DbType.Date Or dataType = DbType.Time Then Return True
            Case 52
                If dataType = DbType.Int16 Then Return True
            Case 122
                If dataType = DbType.Currency And columnMap.Length = 4 Then Return True
                'Case 98
                'If dataType = DbType.Binary Then Return True
            Case 35
                If (dataType = DbType.AnsiString Or dataType = DbType.AnsiStringFixedLength) And (columnMap.Precision < 1 Or columnMap.Precision > 8000) Then Return True
            Case 189
                'If dataType = DbType.DateTime Then Return True
                If columnMap.Precision = 8 Then
                    If dataType = DbType.Binary Then
                        If columnMap.AllowNulls Then
                            If columnMap.IsFixedLength = False Then Return True
                        Else
                            If columnMap.IsFixedLength = True Then Return True
                        End If
                    End If
                End If
            Case 48
                If dataType = DbType.Byte Then Return True
            Case 36
                If dataType = DbType.Guid Then Return True
            Case 165
                If dataType = DbType.Binary Then Return True
            Case 167
                If dataType = DbType.AnsiString And (columnMap.Precision > 0 And columnMap.Precision < 8000) Then Return True

        End Select

        Return False

    End Function


    Protected Overridable Function GetDbType(ByVal systype As Integer, ByVal xtype As Integer, ByVal columnMap As IColumnMap) As DbType

        Dim dataType As DbType = columnMap.DataType

        Select Case xtype
            Case 127
                Return DbType.Int64
            Case 173
                Return DbType.Binary
            Case 104
                Return DbType.Boolean
            Case 175
                Return DbType.AnsiStringFixedLength
            Case 61
                Return DbType.DateTime
            Case 106
                Return DbType.Decimal
            Case 62
                Return DbType.Double
            Case 34
                Return DbType.Object
            Case 56
                Return DbType.Int32
            Case 60
                Return DbType.Currency
            Case 239
                Return DbType.StringFixedLength
            Case 99
                Return DbType.String
            Case 108
                Return DbType.VarNumeric
            Case 231
                Return DbType.String
            Case 59
                Return DbType.Single
            Case 58
                Return DbType.Date
            Case 52
                Return DbType.Int16
            Case 122
                Return DbType.Currency
                'Case 98
                'Return DbType.Binary 
            Case 35
                Return DbType.AnsiString
            Case 189
                'Return DbType.DateTime

                Return DbType.Binary
            Case 48
                Return DbType.Byte
            Case 36
                Return DbType.Guid
            Case 165
                Return DbType.Binary
            Case 167
                Return DbType.AnsiString

        End Select

    End Function


    Protected Overridable Sub LogDiff(ByVal mapObject As IMap, ByVal message As String, ByVal diffInfo As DiffInfoEnum, ByVal setting As String, ByRef hashDiff As Hashtable)

        Dim key As String = mapObject.GetKey

        If Not hashDiff.ContainsKey(key) Then

            hashDiff(key) = New ArrayList

        End If

        CType(hashDiff(key), ArrayList).Add(New diffInfo(mapObject, message, diffInfo, setting))

    End Sub

End Class
