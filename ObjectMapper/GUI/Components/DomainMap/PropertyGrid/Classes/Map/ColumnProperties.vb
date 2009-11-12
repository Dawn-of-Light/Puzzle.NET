Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel

Public Class ColumnProperties
    Inherits PropertiesBase

    Private m_ColumnMap As IColumnMap

    Public Event BeforePropertySet(ByVal mapObject As IColumnMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IColumnMap, ByVal propertyName As String)

    Public Function GetColumnMap() As IColumnMap
        Return m_ColumnMap
    End Function

    Public Sub SetColumnMap(ByVal value As IColumnMap)
        m_ColumnMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_ColumnMap

    End Function

    <Category("Design"), _
        Description("The name of this column."), _
        DisplayName("Name"), _
        DefaultValue("")> Public Property Name() As String
        Get
            Return m_ColumnMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_ColumnMap.Name Then
                m_ColumnMap.UpdateName(Value)
                RaiseEvent AfterPropertySet(m_ColumnMap, "Name")
            End If
        End Set
    End Property


    <Category("Design"), _
        Description("The default value for this column."), _
        DisplayName("Default value"), _
        DefaultValue("")> Public Property DefaultValue() As String
        Get
            Return m_ColumnMap.DefaultValue
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.DefaultValue = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "DefaultValue")
        End Set
    End Property

    <Category("Type"), _
        Description("The data type of this column."), _
        DisplayName("Data type"), _
        DefaultValue(DbType.AnsiString)> Public Property DataType() As DbType
        Get
            Return m_ColumnMap.DataType
        End Get
        Set(ByVal Value As DbType)
            m_ColumnMap.DataType = Value
            SetLengthScaleAndPrecision()
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ColumnMap, "DataType")
        End Set
    End Property

    <Category("Type"), _
        Description("The number of bytes used for storing values to this column. Note that for Currency column, you can specify either 8 for a regular 'money' column in SQL Server or 4 for a 'smallmoney' column."), _
        DisplayName("Length"), _
        DefaultValue(0)> Public Property Length() As Integer
        Get
            Return m_ColumnMap.Length
        End Get
        Set(ByVal Value As Integer)
            m_ColumnMap.Length = Value
            Select Case m_ColumnMap.DataType
                Case DbType.Currency
                    If Not (Value = 4 Or Value = 8) Then
                        m_ColumnMap.Precision = 8
                    End If
                Case DbType.AnsiStringFixedLength
                    m_ColumnMap.Precision = Value
                    SetShouldReloadProperties()
                Case DbType.StringFixedLength
                    m_ColumnMap.Precision = Value / 2
                    SetShouldReloadProperties()
            End Select
            RaiseEvent AfterPropertySet(m_ColumnMap, "Length")
        End Set
    End Property


    <Category("Type"), _
        Description("The precision used for values to this column. Note that for String, StringFixedLength, AnsiString and AnsiStringFixedLength columns, this is where you enter the maximum number of characters for your string. By setting this value to 0 for String and AnsiString properties they will be generated as ntext and text columns respectively in a MS SQL Server. Also note that for DateTime column, you may select between the values 23 for a normal datetime column and 8 for a timestamp column."), _
        DisplayName("Precision"), _
        DefaultValue(0)> Public Property Precision() As Integer
        Get
            Return m_ColumnMap.Precision
        End Get
        Set(ByVal Value As Integer)
            m_ColumnMap.Precision = Value
            Select Case m_ColumnMap.DataType
                Case DbType.DateTime
                    If Not (Value = 8 Or Value = 23) Then
                        m_ColumnMap.Precision = 23
                    End If
                Case DbType.AnsiStringFixedLength
                    m_ColumnMap.Length = Value
                Case DbType.AnsiString
                    If Value = 0 Then
                        m_ColumnMap.Length = 16
                    Else
                        m_ColumnMap.Length = Value
                    End If
                Case DbType.String
                    If Value = 0 Then
                        m_ColumnMap.Length = 16
                    Else
                        m_ColumnMap.Length = CInt(Value * 2)
                    End If
                Case DbType.StringFixedLength
                    m_ColumnMap.Length = CInt(Value * 2)
            End Select
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ColumnMap, "Precision")
        End Set
    End Property

    <Category("Type"), _
        Description("The scale for values to this column. Defines the maximum number of decimal digits that can be stored to the right of the decimal point. Must be a value between 0 and the specified precision. APplies only to columns with a numeric data type that accepts decimals."), _
        DisplayName("Scale"), _
        DefaultValue(0)> Public Property Scale() As Integer
        Get
            Return m_ColumnMap.Scale
        End Get
        Set(ByVal Value As Integer)
            If Value > m_ColumnMap.Precision Then
                m_ColumnMap.Scale = m_ColumnMap.Precision
            Else
                m_ColumnMap.Scale = Value
            End If
            RaiseEvent AfterPropertySet(m_ColumnMap, "Scale")
        End Set
    End Property

    <Category("Type"), _
        Description("Specifies if a binary field is fixed or variable length. Applies only to Binary columns"), _
        DisplayName("Fixed length"), _
        DefaultValue(False)> Public Property IsFixedLength() As Boolean
        Get
            Return m_ColumnMap.IsFixedLength
        End Get
        Set(ByVal Value As Boolean)
            m_ColumnMap.IsFixedLength = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "IsFixedLength")
        End Set
    End Property

    <Category("Format"), _
        Description("The format that should be used when storing the date and/or time values for this column. Leave empty to use the default format 'yyyy-MM-dd HH.mm:ss'"), _
        DisplayName("Date format"), _
        DefaultValue("yyyy-MM-dd HH.mm:ss")> Public Property Format() As String
        Get
            Return m_ColumnMap.Format
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.Format = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "Format")
        End Set
    End Property


    <Category("Constraints"), _
        Description("If set to True, this column allows NULL values."), _
        DisplayName("Allow nulls"), _
        DefaultValue(False)> Public Property AllowNulls() As Boolean
        Get
            Return m_ColumnMap.AllowNulls
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                If m_ColumnMap.IsPrimaryKey Then
                    MsgBox("Allow nulls can not be set to True on a column which is part of a primary key!")
                    Exit Property
                End If
            End If
            m_ColumnMap.AllowNulls = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "AllowNulls")
        End Set
    End Property

    <Category("Primary key"), _
    Description("If set to True, this column is part of the primary key for this table."), _
    DisplayName("Primary key"), _
    DefaultValue(False)> Public Property IsPrimaryKey() As Boolean
        Get
            Return m_ColumnMap.IsPrimaryKey
        End Get
        Set(ByVal Value As Boolean)
            m_ColumnMap.IsPrimaryKey = Value
            If Value Then
                m_ColumnMap.AllowNulls = False
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ColumnMap, "IsPrimaryKey")
        End Set
    End Property

    <Category("Foreign Key"), _
        Description("If set to True, this column has a foreign key constraint such that values in this column must correspond to the primary keys of existing rows."), _
        DisplayName("Foreign key"), _
        DefaultValue(False)> Public Property IsForeignKey() As Boolean
        Get
            Return m_ColumnMap.IsForeignKey
        End Get
        Set(ByVal Value As Boolean)
            m_ColumnMap.IsForeignKey = Value
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ColumnMap, "IsForeignKey")
        End Set
    End Property

    <Category("Foreign Key"), _
    Description("The name of the foreign key. Group columns that form a foreign key together by giving them the same Foreign key name."), _
    DisplayName("Foreign key name"), _
    DefaultValue(False)> Public Property ForeignKeyName() As String
        Get
            Return m_ColumnMap.ForeignKeyName
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.ForeignKeyName = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "ForeignKeyName")
        End Set
    End Property

    <Category("Foreign Key"), _
        TypeConverter(GetType(TableConverter)), _
        Description("The name of the table withe the primary key column that this foreign key is mapping to."), _
        DisplayName("Primary key table"), _
        DefaultValue(False)> Public Property PrimaryKeyTable() As String
        Get
            Return m_ColumnMap.PrimaryKeyTable
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.PrimaryKeyTable = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "PrimaryKeyTable")
        End Set
    End Property


    <Category("Foreign Key"), _
        TypeConverter(GetType(ColumnConverter)), _
        Description("The name of the primary key column that this foreign key is mapping to."), _
        DisplayName("Primary key column"), _
        DefaultValue(False)> Public Property PrimaryKeyColumn() As String
        Get
            Return m_ColumnMap.PrimaryKeyColumn
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.PrimaryKeyColumn = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "PrimaryKeyColumn")
        End Set
    End Property



    <Category("Auto increaser"), _
        Description("If set to True, this column is filled with new values automatically when rows are inserted."), _
        DisplayName("Auto increasing column"), _
        DefaultValue(False)> Public Property IsAutoIncrease() As Boolean
        Get
            Return m_ColumnMap.IsAutoIncrease
        End Get
        Set(ByVal Value As Boolean)
            m_ColumnMap.IsAutoIncrease = Value
            If Value Then
                If m_ColumnMap.Seed = 0 Then Me.Seed = 1
                If m_ColumnMap.Increment = 0 Then Me.Increment = 1
                m_ColumnMap.DataType = DbType.Int32
                m_ColumnMap.AllowNulls = False
            Else
                m_ColumnMap.Seed = 0
                m_ColumnMap.Increment = 0
            End If
            SetShouldReloadProperties()
            RaiseEvent AfterPropertySet(m_ColumnMap, "IsAutoIncrease")
        End Set
    End Property

    <Category("Auto increaser"), _
        Description("The number of steps that the auto increaser should move for each new row."), _
        DisplayName("Increment"), _
        DefaultValue(1)> Public Property Increment() As Integer
        Get
            Return m_ColumnMap.Increment
        End Get
        Set(ByVal Value As Integer)
            m_ColumnMap.Increment = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "Increment")
        End Set
    End Property

    <Category("Auto increaser"), _
        Description("The value that the auto increaser should give to the first row being inserted."), _
        DisplayName("Seed"), _
        DefaultValue(1)> Public Property Seed() As Integer
        Get
            Return m_ColumnMap.Seed
        End Get
        Set(ByVal Value As Integer)
            m_ColumnMap.Seed = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "Seed")
        End Set
    End Property


    <Category("Auto increaser"), _
        Description("The name of the sequence from which the auto increaser gets its values."), _
        DisplayName("Sequence name"), _
        DefaultValue("")> Public Property Sequence() As String
        Get
            Return m_ColumnMap.Sequence
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.Sequence = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "Sequence")
        End Set
    End Property

    <Category("Custom"), _
        Description("The name of the physical data type in the database. Sometimes used when the types in System.Data.DbType are not enough, such as to identify an SQL Server timestamp column."), _
        DisplayName("Specific data type"), _
        DefaultValue("")> Public Property SpecificDataType() As String
        Get
            Return m_ColumnMap.SpecificDataType
        End Get
        Set(ByVal Value As String)
            m_ColumnMap.SpecificDataType = Value
            RaiseEvent AfterPropertySet(m_ColumnMap, "SpecificDataType")
        End Set
    End Property



    Public Overrides Function CustomizeProperty(ByVal propDesc As PropertyDescriptorBase) As PropertyDescriptorBase

        Select Case propDesc.Name

            Case "Format"
                If Not (m_ColumnMap.DataType = DbType.Date Or m_ColumnMap.DataType = DbType.Time Or m_ColumnMap.DataType = DbType.DateTime) Then propDesc.SetReadOnly()

            Case "AllowNulls"
                If m_ColumnMap.IsPrimaryKey Then propDesc.SetReadOnly()

            Case "ForeignKeyName"
                If Not m_ColumnMap.IsForeignKey Then propDesc.SetReadOnly()

            Case "PrimaryKeyTable"
                If Not m_ColumnMap.IsForeignKey Then propDesc.SetReadOnly()

            Case "PrimaryKeyColumn"
                If Not m_ColumnMap.IsForeignKey Then propDesc.SetReadOnly()

            Case "Increment"
                If Not m_ColumnMap.IsAutoIncrease Then propDesc.SetReadOnly()

            Case "Seed"
                If Not m_ColumnMap.IsAutoIncrease Then propDesc.SetReadOnly()

            Case "Sequence"
                If Not m_ColumnMap.IsAutoIncrease Then propDesc.SetReadOnly()

            Case "Length"
                If Not MayWrite("Length") Then propDesc.SetReadOnly()

            Case "Precision"
                If Not MayWrite("Precision") Then propDesc.SetReadOnly()

            Case "Scale"
                If Not MayWrite("Scale") Then propDesc.SetReadOnly()

            Case "IsFixedLength"
                If Not m_ColumnMap.DataType = DbType.Binary Then propDesc.SetReadOnly()


        End Select

        Return propDesc
    End Function

    Private Function MayWrite(ByVal propertyName As String) As Boolean

        Select Case m_ColumnMap.DataType

            Case DbType.AnsiString

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return False

            Case DbType.AnsiStringFixedLength

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return False

            Case DbType.Binary

                If propertyName = "Length" Then Return True
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.Boolean

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.Byte

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.Currency

                If propertyName = "Length" Then Return True
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.Date

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.DateTime

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return False


            Case DbType.Decimal

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return True


            Case DbType.Double

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return True


            Case DbType.Guid

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.Int16

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.Int32

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.Int64

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.Object

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.[SByte]

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.Single

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.String

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return False


            Case DbType.StringFixedLength

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return True
                If propertyName = "Scale" Then Return False


            Case DbType.Time

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False

            Case DbType.UInt16

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.UInt32

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.UInt64

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


            Case DbType.VarNumeric

                If propertyName = "Length" Then Return False
                If propertyName = "Precision" Then Return False
                If propertyName = "Scale" Then Return False


        End Select

    End Function

    Private Sub SetLengthScaleAndPrecision()

        Dim length As Integer
        Dim scale As Integer
        Dim prec As Integer

        Select Case m_ColumnMap.DataType

            Case DbType.AnsiString

                length = 16
                prec = 0
                scale = 0

            Case DbType.AnsiStringFixedLength

                length = 10
                prec = 10
                scale = 0

            Case DbType.Binary

                length = 16
                prec = 0
                scale = 0

            Case DbType.Boolean

                length = 1
                prec = 1
                scale = 0

            Case DbType.Byte

                length = 1
                prec = 3
                scale = 0

            Case DbType.Currency

                length = 8
                prec = 19
                scale = 4

            Case DbType.Date

                length = 4
                prec = 16
                scale = 0

            Case DbType.DateTime

                length = 8
                prec = 23
                scale = 3

            Case DbType.Decimal

                length = 9
                prec = 18
                scale = 0

            Case DbType.Double

                length = 9
                prec = 18
                scale = 0

            Case DbType.Guid

                length = 16
                prec = 0
                scale = 0

            Case DbType.Int16

                length = 2
                prec = 5
                scale = 0

            Case DbType.Int32

                length = 4
                prec = 10
                scale = 0

            Case DbType.Int64

                length = 8
                prec = 19
                scale = 0

            Case DbType.Object

                length = 16
                prec = 0
                scale = 0

            Case DbType.[SByte]

                length = 1
                prec = 4
                scale = 0

            Case DbType.Single

                length = 8
                prec = 19
                scale = 0

            Case DbType.String

                length = 16
                prec = 0
                scale = 0

            Case DbType.StringFixedLength

                length = 20
                prec = 10
                scale = 0

            Case DbType.Time

                length = 4
                prec = 16
                scale = 0

            Case DbType.UInt16

                length = 2
                prec = 6
                scale = 0

            Case DbType.UInt32

                length = 4
                prec = 11
                scale = 0

            Case DbType.UInt64

                length = 8
                prec = 20
                scale = 0

            Case DbType.VarNumeric

                length = 16
                prec = 0
                scale = 0

        End Select

        m_ColumnMap.Length = length
        m_ColumnMap.Scale = scale
        m_ColumnMap.Precision = prec

    End Sub

End Class