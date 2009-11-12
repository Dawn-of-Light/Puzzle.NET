Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Persistence
Imports System.Data.OleDb

Public Class SourceToTablesOleDbGeneric
    Inherits SourceToTablesBase

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


    Public Overloads Overrides Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByRef hashDiff As Hashtable)

    End Sub

    Public Overloads Overrides Sub SourceToTables(ByVal sourceMap As Puzzle.NPersist.Framework.Mapping.ISourceMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(sourceMap.DomainMap)

        Dim ds As IDataSource = ctx.DataSourceManager.GetDataSource(sourceMap)

        Dim i As Long

        Dim name As String
        Dim tableMap As ITableMap
        Dim result As Object

        Dim dt As System.Data.DataTable

        Dim con As OleDbConnection

        Try

            con = ds.GetConnection

            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            result = DataTableToArray(dt)

            dt.Dispose()

            ds.ReturnConnection()

        Catch ex As Exception

            ds.ReturnConnection()

            ctx.Dispose()

            Throw New Exception("Encountered an exception when communicating with database: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                name = result(2, i)

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

            Next

        End If

        ctx.Dispose()

    End Sub

    Public Overloads Overrides Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(tableMap.SourceMap.DomainMap)

        GetColumns(tableMap, ctx, addToDomainMap, hashDiff)

        ctx.Dispose()

    End Sub

    Public Overloads Overrides Sub SourceToColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByRef hashDiff As Hashtable)

        Dim ctx As IContext = New Context(tableMap.SourceMap.DomainMap)

        GetColumns(tableMap, ctx, Nothing, hashDiff)

        ctx.Dispose()

    End Sub

    Protected Overridable Sub GetColumns(ByVal tableMap As Puzzle.NPersist.Framework.Mapping.ITableMap, ByVal ctx As IContext, ByVal addToDomainMap As IDomainMap, ByRef hashDiff As Hashtable)

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

        Dim i As Long
        Dim j As Long
        Dim k As Long

        Dim columnMap As IColumnMap
        Dim result As Object
        Dim resultTypes As Object
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
        Dim forKeyName As String
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

            'resultConst = ctx.SqlExecutor.ExecuteArray(sqlConst, ds)
            'resultForKey = ctx.SqlExecutor.ExecuteArray(sqlForKey, ds)
            'resultAutoInc = ctx.SqlExecutor.ExecuteArray(sqlAutoInc, ds)
            'resultDef = ctx.SqlExecutor.ExecuteArray(sqlDef, ds)

        Catch ex As Exception

            ds.ReturnConnection()

            ctx.Dispose()

            Throw New Exception("Encountered an exception when communicating with database: " & ex.Message, ex)

        End Try

        If IsArray(result) Then

            For i = 0 To UBound(result, 2)

                diffMsgs.Clear()
                diffSettings.Clear()

                colName = ""
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
                forKeyName = ""

                incSeed = 0
                incInc = 0

                hasDef = False
                defVal = ""

                dteFormat = ""

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
                                If Not IsDBNull(resultForKey(16, j)) Then forKeyName = resultForKey(16, j)

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

                columnMap = tableMap.GetColumnMap(colName)

                If Not columnMap Is Nothing Then

                    If Len(columnMap.Format) > 0 Then
                        dteFormat = columnMap.Format
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

                columnMap.DataType = GetDbType(colType, colLength, columnMap)
                'If columnMap.DataType = DbType.Binary Then
                '    If colLength = 165 Then
                '        columnMap.IsFixedLength = False
                '    Else
                '        columnMap.IsFixedLength = True
                '    End If
                'End If
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
