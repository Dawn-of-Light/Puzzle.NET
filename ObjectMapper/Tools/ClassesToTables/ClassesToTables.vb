Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class ClassesToTables
    Implements IClassesToTables

    Private m_TablePrefix As String = "tbl"
    Private m_TableSuffix As String = ""

    Private m_MaxTableLength As Integer = 0
    Private m_MaxColumnLength As Integer = 0
    Private m_DefaultStringLength As Integer = 0

    Public Overridable Overloads Function GetTableNameForClass(ByVal classMap As IClassMap) As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetTableNameForClass

        Return GetTableNameForClass(classMap, False)

    End Function

    Public Overridable Overloads Function GetTableNameForClass(ByVal classMap As IClassMap, ByVal useThisClass As Boolean) As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetTableNameForClass

        Dim name As String
        Dim useClassMap As IClassMap = classMap

        If Not useThisClass Then

            If useClassMap.IsInHierarchy Then

                If Not useClassMap.InheritanceType = InheritanceType.None Then

                    While Not useClassMap.GetInheritedClassMap Is Nothing

                        useClassMap = useClassMap.GetInheritedClassMap

                    End While

                End If

            End If

        End If

        If Len(useClassMap.Table) > 0 And Not useThisClass Then

            Return useClassMap.Table

        Else

            name = TablePrefix & useClassMap.GetName & TableSuffix

            If m_MaxTableLength > 0 Then

                If Len(name) > m_MaxTableLength Then name = Left(name, m_MaxTableLength)

            End If

            Return name

        End If

    End Function

    Public Overridable Function GetTypeColumnNameForClass(ByVal classMap As IClassMap) As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetTypeColumnNameForClass

        Dim name As String

        Dim useClassMap As IClassMap = classMap

        If useClassMap.IsInHierarchy Then

            If Not useClassMap.InheritanceType = InheritanceType.None Then

                While Not useClassMap.GetInheritedClassMap Is Nothing

                    useClassMap = useClassMap.GetInheritedClassMap

                End While

            End If

        End If

        If Len(useClassMap.TypeColumn) > 0 Then

            Return useClassMap.TypeColumn

        Else

            name = useClassMap.GetName & "Type"

            If m_MaxColumnLength > 0 Then

                If Len(name) > m_MaxColumnLength Then name = Left(name, m_MaxColumnLength)

            End If

            Return name

        End If

    End Function


    Public Overridable Function GetTableNameForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetTableNameForProperty

        Dim name As String
        Dim clsName As String
        Dim refClassMap As IClassMap
        Dim refClsName As String

        If propertyMap.IsCollection Then

            If Not propertyMap.ReferenceType = ReferenceType.None Then

                clsName = propertyMap.ClassMap.GetName

                refClassMap = propertyMap.GetReferencedClassMap

                If Not propertyMap.GetReferencedClassMap Is Nothing Then

                    refClsName = refClassMap.GetName

                    If clsName > refClsName Then

                        name = TablePrefix & refClsName & "_" & clsName & TableSuffix

                    Else

                        name = TablePrefix & clsName & "_" & refClsName & TableSuffix

                    End If

                Else

                    Throw New Exception("Referenced class does not exists! Can't create table name for property!")

                End If

            Else

                name = TablePrefix & propertyMap.ClassMap.GetName & "_" & propertyMap.Name & TableSuffix

            End If


        Else

            name = GetTableNameForClass(propertyMap.ClassMap, True)
            'If a = a Then

            'Else

            '    Throw New Exception("Only collection properties and inherited properties using ClassTableInheritance or ConcreteTableInheritance need table name suggestions!")

            'End If

        End If

        If m_MaxColumnLength > 0 Then

            If Len(name) > m_MaxColumnLength Then name = Left(name, m_MaxColumnLength)

        End If

        Return name


    End Function


    Public Overridable Function GetColumnNameForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetColumnNameForProperty

        Dim name As String
        Dim refClassMap As IClassMap
        Dim idProp As IPropertyMap
        Dim useClassMap As IClassMap

        If propertyMap.IsIdentity Then

            useClassMap = propertyMap.ClassMap

            If useClassMap.IsInHierarchy Then

                If Not useClassMap.InheritanceType = InheritanceType.None Then

                    While Not useClassMap.GetInheritedClassMap Is Nothing

                        useClassMap = useClassMap.GetInheritedClassMap

                    End While

                End If

            End If

            Select Case LCase(propertyMap.Name)

                Case "id", "key"

                    name = useClassMap.GetName & propertyMap.Name

            End Select

        End If

        If name = "" Then

            If Not propertyMap.ReferenceType = ReferenceType.None Then

                refClassMap = propertyMap.GetReferencedClassMap

                If Not refClassMap Is Nothing Then

                    For Each idProp In refClassMap.GetIdentityPropertyMaps

                        If idProp.ReferenceType = ReferenceType.None Then

                            If Len(idProp.Column) > 0 Then

                                'name = idProp.Column
                                name = propertyMap.Name & "_" & idProp.Column

                            Else

                                'name = GetColumnNameForProperty(idProp)
                                name = propertyMap.Name & "_" & GetColumnNameForProperty(idProp)

                            End If

                        End If

                        Exit For

                    Next

                End If

            End If

        End If

        If name = "" Then name = propertyMap.Name

        If m_MaxColumnLength > 0 Then

            If Len(name) > m_MaxColumnLength Then name = Left(name, m_MaxColumnLength)

        End If

        Return name

    End Function


    Public Overridable Property TablePrefix() As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.TablePrefix
        Get
            Return m_TablePrefix
        End Get
        Set(ByVal Value As String)
            m_TablePrefix = Value
        End Set
    End Property

    Public Overridable Property TableSuffix() As String Implements Puzzle.ObjectMapper.Tools.IClassesToTables.TableSuffix
        Get
            Return m_TableSuffix
        End Get
        Set(ByVal Value As String)
            m_TableSuffix = Value
        End Set
    End Property

    Public Overridable Property MaxColumnLength() As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.MaxColumnLength
        Get
            Return m_MaxColumnLength
        End Get
        Set(ByVal Value As Integer)
            m_MaxColumnLength = Value
        End Set
    End Property

    Public Overridable Property MaxTableLength() As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.MaxTableLength
        Get
            Return m_MaxTableLength
        End Get
        Set(ByVal Value As Integer)
            m_MaxTableLength = Value
        End Set
    End Property

    Public Overridable Property DefaultStringLength() As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.DefaultStringLength
        Get
            Return m_DefaultStringLength
        End Get
        Set(ByVal Value As Integer)
            m_DefaultStringLength = Value
        End Set
    End Property


    Public Overridable Function GetColumnTypeForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As System.Data.DbType Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetColumnTypeForProperty
        Dim dataType As String
        Dim columnType As System.Data.DbType
        Dim refProp As IPropertyMap
        Dim refClassMap As IClassMap
        Dim idProp As IPropertyMap

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each idProp In refClassMap.GetIdentityPropertyMaps

                    refProp = idProp
                    Exit For

                Next

                If Not refProp Is Nothing Then

                    If refProp.IsCollection Then
                        dataType = refProp.ItemType
                    Else
                        dataType = refProp.DataType
                    End If

                Else

                    'Throw New Exception("")

                End If

            End If


        Else

            If propertyMap.IsCollection Then
                dataType = propertyMap.ItemType
            Else
                dataType = propertyMap.DataType
            End If

        End If

        Select Case LCase(Trim(dataType))
            Case "string", "system.string"
                columnType = DbType.AnsiString
            Case "guid", "system.guid"
                columnType = DbType.Guid
            Case "long", "integer", "int32", "int", "system.int32"
                columnType = DbType.Int32
            Case "int16", "system.int16"
                columnType = DbType.Int16
            Case "int64", "system.int64"
                columnType = DbType.Int64
            Case "uint16", "system.uint16"
                columnType = DbType.UInt16
            Case "uint32", "system.uint32"
                columnType = DbType.UInt32
            Case "uint64", "system.uint64"
                columnType = DbType.UInt64
            Case "date", "time", "datetime", "system.datetime"
                columnType = DbType.DateTime
            Case "bool", "boolean", "system.boolean"
                columnType = DbType.Boolean
            Case "byte", "system.byte", "char", "system.char"
                columnType = DbType.Byte
            Case "decimal", "system.decimal"
                columnType = DbType.Decimal
            Case "double", "system.double"
                columnType = DbType.Double
            Case "object", "system.object"
                columnType = DbType.Object
            Case "single", "system.single"
                columnType = DbType.Single
            Case "byte()", "system.byte()", "byte[]", "system.byte[]"
                columnType = DbType.Object
            Case Else
                columnType = DbType.Int32 'Assume enumeration

        End Select

        Return columnType

    End Function

    Public Overridable Function GetColumnLengthForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetColumnLengthForProperty

        Dim dataType As String
        Dim length As Integer
        Dim refProp As IPropertyMap
        Dim idProp As IPropertyMap
        Dim refClassMap As IClassMap

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each idProp In refClassMap.GetIdentityPropertyMaps

                    refProp = idProp
                    Exit For

                Next

                If Not refProp Is Nothing Then

                    If refProp.IsCollection Then
                        dataType = refProp.ItemType
                    Else
                        dataType = refProp.DataType
                    End If

                Else

                    'Throw New Exception("")

                End If

            End If

        Else

            If propertyMap.IsCollection Then
                dataType = propertyMap.ItemType
            Else
                dataType = propertyMap.DataType
            End If

        End If

        Select Case LCase(Trim(dataType))
            Case "string", "system.string"
                If propertyMap.MaxLength > -1 Then
                    length = propertyMap.MaxLength
                Else
                    length = 16
                End If
            Case "long", "integer", "int32", "int", "system.int32"
                length = 4
            Case "int16", "system.int16"
                length = 2
            Case "int64", "system.int64"
                length = 8
            Case "uint16", "system.uint16"
                length = 2
            Case "uint32", "system.uint32"
                length = 4
            Case "uint64", "system.uint64"
                length = 8
            Case "date", "time", "datetime", "system.datetime"
                length = 8
            Case "bool", "boolean", "system.boolean"
                length = 1
            Case "byte", "system.byte", "char", "system.char"
                length = 1
            Case "decimal", "system.decimal"
                length = 9
            Case "double", "system.double"
                length = 9
            Case "object", "system.object"
                length = 0
            Case "single", "system.single"
                length = 8
            Case "byte()", "system.byte()", "byte[]", "system.byte[]"
                length = 16
            Case Else
                length = 4 'Assume enumeration


        End Select

        Return length
    End Function


    Public Overridable Function GetColumnPrecisionForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetColumnPrecisionForProperty

        Dim dataType As String
        Dim prec As Integer
        Dim refProp As IPropertyMap
        Dim idProp As IPropertyMap
        Dim refClassMap As IClassMap

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each idProp In propertyMap.GetReferencedClassMap.GetIdentityPropertyMaps

                    refProp = idProp
                    Exit For

                Next

                If Not refProp Is Nothing Then

                    If refProp.IsCollection Then
                        dataType = refProp.ItemType
                    Else
                        dataType = refProp.DataType
                    End If

                Else

                    'Throw New Exception("")

                End If

            End If

        Else

            If propertyMap.IsCollection Then
                dataType = propertyMap.ItemType
            Else
                dataType = propertyMap.DataType
            End If

        End If

        Select Case LCase(Trim(dataType))
            Case "string", "system.string"

                If propertyMap.MaxLength > -1 Then
                    prec = propertyMap.MaxLength
                Else
                    prec = 0
                End If

            Case "long", "integer", "int32", "int", "system.int32"
                prec = 10
            Case "int16", "system.int16"
                prec = 5
            Case "int64", "system.int64"
                prec = 19
            Case "uint16", "system.uint16"
                prec = 5
            Case "uint32", "system.uint32"
                prec = 10
            Case "uint64", "system.uint64"
                prec = 15
            Case "date", "time", "datetime", "system.datetime"
                prec = 23
            Case "bool", "boolean", "system.boolean"
                prec = 1
            Case "byte", "system.byte", "char", "system.char"
                prec = 3
            Case "decimal", "system.decimal"
                prec = 18
            Case "double", "system.double"
                prec = 18
            Case "object", "system.object"
                prec = 0
            Case "single", "system.single"
                prec = 19
            Case "byte()", "system.byte()", "byte[]", "system.byte[]"
                prec = 0
            Case Else
                prec = 10 'Assume enumeration

        End Select

        Return prec
    End Function


    Public Overridable Function GetColumnScaleForProperty(ByVal propertyMap As Puzzle.NPersist.Framework.Mapping.IPropertyMap) As Integer Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GetColumnScaleForProperty

        Dim dataType As String
        Dim scale As Integer
        Dim refProp As IPropertyMap
        Dim idProp As IPropertyMap
        Dim refClassMap As IClassMap

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each idProp In propertyMap.GetReferencedClassMap.GetIdentityPropertyMaps

                    refProp = idProp
                    Exit For

                Next

                If Not refProp Is Nothing Then

                    If refProp.IsCollection Then
                        dataType = refProp.ItemType
                    Else
                        dataType = refProp.DataType
                    End If

                Else

                    'Throw New Exception("")

                End If

            End If

        Else

            If propertyMap.IsCollection Then
                dataType = propertyMap.ItemType
            Else
                dataType = propertyMap.DataType
            End If

        End If

        Select Case LCase(Trim(dataType))
            Case "string", "system.string"
                scale = 0
            Case "long", "integer", "int32", "int", "system.int32"
                scale = 0
            Case "int16", "system.int16"
                scale = 0
            Case "int64", "system.int64"
                scale = 0
            Case "uint16", "system.uint16"
                scale = 0
            Case "uint32", "system.uint32"
                scale = 0
            Case "uint64", "system.uint64"
                scale = 0
            Case "date", "time", "datetime", "system.datetime"
                scale = 3
            Case "bool", "boolean", "system.boolean"
                scale = 0
            Case "byte", "system.byte", "char", "system.char"
                scale = 0
            Case "decimal", "system.decimal"
                scale = 0
            Case "double", "system.double"
                scale = 0
            Case "object", "system.object"
                scale = 0
            Case "single", "system.single"
                scale = 0
            Case "byte()", "system.byte()", "byte[]", "system.byte[]"
                scale = 0
            Case Else
                scale = 0

        End Select

        Return scale
    End Function


    Public Overridable Overloads Sub GenerateTablesForClasses(ByVal sourceDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal targetDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap) Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GenerateTablesForClasses

        GenerateTablesForClasses(sourceDomainMap, targetDomainMap, False)

    End Sub


    Public Overridable Overloads Sub GenerateTablesForClasses(ByVal sourceDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal targetDomainMap As Puzzle.NPersist.Framework.Mapping.IDomainMap, ByVal generateMappings As Boolean) Implements Puzzle.ObjectMapper.Tools.IClassesToTables.GenerateTablesForClasses

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim addPropertyMap As IPropertyMap
        Dim propertyMaps As ArrayList
        Dim allPropertyMaps As ArrayList
        Dim ok As Boolean

        Dim classMaps As IList = sourceDomainMap.GetPersistentClassMaps

        'create tables and type columns for classes
        For Each classMap In classMaps

            If Not classMap.SourceClass.Length > 0 Then

                If Not (classMap.IsAbstract And (classMap.InheritanceType = InheritanceType.None Or _
                    (classMap.InheritanceType = InheritanceType.ConcreteTableInheritance And _
                    Not classMap.GetInheritedClassMap Is Nothing))) Then

                    If classMap.GetTableMap Is Nothing Then

                        CreateTableForClass(classMap, targetDomainMap, generateMappings)

                    End If

                    If Len(classMap.TypeColumn) > 0 Or classMap.InheritanceType <> InheritanceType.None Then

                        If classMap.GetTypeColumnMap Is Nothing Then

                            CreateTypeColumnForClass(classMap, targetDomainMap, generateMappings)

                        End If

                    End If

                End If

            End If

        Next

        'Create tables for properties
        For Each classMap In classMaps

            If Not classMap.SourceClass.Length > 0 Then

                If Not (classMap.IsAbstract And (classMap.InheritanceType = InheritanceType.None Or _
                    (classMap.InheritanceType = InheritanceType.ConcreteTableInheritance And Not classMap.GetInheritedClassMap Is Nothing))) Then

                    Select Case classMap.InheritanceType
                        Case InheritanceType.None

                            propertyMaps = classMap.GetAllPropertyMaps

                        Case InheritanceType.SingleTableInheritance, InheritanceType.ClassTableInheritance

                            propertyMaps = classMap.GetNonInheritedPropertyMaps

                        Case InheritanceType.ConcreteTableInheritance

                            If classMap.GetInheritedClassMap Is Nothing Then

                                propertyMaps = classMap.GetAllPropertyMaps

                            Else

                                allPropertyMaps = classMap.GetAllPropertyMaps

                                propertyMaps = New ArrayList

                                For Each addPropertyMap In allPropertyMaps

                                    If Not addPropertyMap.IsIdentity Then

                                        propertyMaps.Add(addPropertyMap)

                                    End If

                                Next

                            End If

                    End Select

                    For Each propertyMap In propertyMaps

                        If Not propertyMap.InheritInverseMappings Then

                            If Len(propertyMap.Table) > 0 OrElse propertyMap.IsCollection OrElse _
                                IsOutbrokenByInheritance(propertyMap, classMap) Then

                                ok = True
                                'We should not create any table for a ManyOne reference collection property!
                                If propertyMap.IsCollection Then
                                    If propertyMap.ReferenceType = ReferenceType.ManyToOne Then
                                        ok = False
                                    End If
                                End If

                                If ok Then

                                    If propertyMap.GetTableMap Is Nothing Then

                                        CreateTableForProperty(propertyMap, targetDomainMap, generateMappings)

                                    End If

                                End If

                            End If

                        End If

                    Next

                End If

            End If

        Next

        'create id column and column for properties
        For Each classMap In classMaps

            If Not classMap.SourceClass.Length > 0 Then

                If Not (classMap.IsAbstract And (classMap.InheritanceType = InheritanceType.None Or _
    (classMap.InheritanceType = InheritanceType.ConcreteTableInheritance And Not classMap.GetInheritedClassMap Is Nothing))) Then

                    Select Case classMap.InheritanceType
                        Case InheritanceType.None

                            propertyMaps = classMap.GetAllPropertyMaps

                        Case InheritanceType.SingleTableInheritance, InheritanceType.ClassTableInheritance

                            propertyMaps = classMap.GetNonInheritedPropertyMaps

                        Case InheritanceType.ConcreteTableInheritance

                            If classMap.GetInheritedClassMap Is Nothing Then

                                propertyMaps = classMap.GetAllPropertyMaps

                            Else

                                allPropertyMaps = classMap.GetAllPropertyMaps

                                propertyMaps = New ArrayList

                                For Each addPropertyMap In allPropertyMaps

                                    If Not addPropertyMap.IsIdentity Then

                                        propertyMaps.Add(addPropertyMap)

                                    End If

                                Next

                            End If

                    End Select

                    For Each propertyMap In propertyMaps

                        If Not propertyMap.InheritInverseMappings Then

                            ok = True
                            'We should not create any column or id column for a ManyOne reference collection property!
                            If propertyMap.IsCollection Then
                                If propertyMap.ReferenceType = ReferenceType.ManyToOne Then
                                    ok = False
                                End If
                            End If

                            If ok Then

                                If Len(propertyMap.IdColumn) > 0 OrElse propertyMap.IsCollection OrElse Len(propertyMap.Table) > 0 OrElse _
                                    IsOutbrokenByInheritance(propertyMap, classMap) Then

                                    'If Len(propertyMap.IdColumn) > 0 OrElse propertyMap.IsCollection OrElse Len(propertyMap.Table) > 0 Then

                                    If propertyMap.GetIdColumnMap Is Nothing Then

                                        CreateIDColumnForProperty(propertyMap, targetDomainMap, generateMappings, classMap)

                                    End If

                                End If

                                If propertyMap.GetColumnMap Is Nothing Then

                                    CreateColumnForProperty(propertyMap, targetDomainMap, generateMappings, classMap)

                                End If

                            End If

                        End If

                    Next

                End If

            End If

        Next

    End Sub


    Protected Overridable Sub CreateTableForClass(ByVal classMap As IClassMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim tableMap As ITableMap
        Dim sourceMap As ISourceMap
        Dim addToSourceMap As ISourceMap
        Dim classMapClone As IClassMap
        Dim columnMap As IColumnMap
        Dim name As String

        sourceMap = classMap.GetSourceMap

        If sourceMap Is Nothing Then

            sourceMap = classMap.DomainMap.GetSourceMap

            If sourceMap Is Nothing Then

                Throw New Exception("No default data source specified for domain model! Can't add table for class!")

            End If

        End If

        addToSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

        If addToSourceMap Is Nothing Then

            addToSourceMap = sourceMap.Clone

            addToSourceMap.DomainMap = targetDomainMap

        End If


        If Len(classMap.Table) > 0 Then

            name = classMap.Table

        Else

            name = GetTableNameForClass(classMap)

        End If

        tableMap = addToSourceMap.GetTableMap(name)

        If tableMap Is Nothing Then

            tableMap = New tableMap

            tableMap.Name = name

            tableMap.SourceMap = addToSourceMap

        End If

        If generateMappings And Len(classMap.Table) < 1 Then

            classMapClone = targetDomainMap.GetClassMap(classMap.Name)

            If classMapClone Is Nothing Then

                classMapClone = classMap.Clone

                classMapClone.DomainMap = targetDomainMap

            End If

            classMapClone.Table = tableMap.Name

            If Not addToSourceMap.Name = targetDomainMap.Source Then

                classMapClone.Source = addToSourceMap.Name

            End If

            classMapClone.DomainMap = targetDomainMap

        End If


    End Sub


    Protected Overridable Sub CreateTypeColumnForClass(ByVal classMap As IClassMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim tableMap As ITableMap
        Dim sourceMap As ISourceMap
        Dim addToSourceMap As ISourceMap
        Dim classMapClone As IClassMap
        Dim columnMap As IColumnMap
        Dim tableName As String
        Dim name As String

        sourceMap = classMap.GetSourceMap

        If sourceMap Is Nothing Then

            sourceMap = classMap.DomainMap.GetSourceMap

            If sourceMap Is Nothing Then

                Throw New Exception("No default data source specified for domain model! Can't add table for class!")

            End If

        End If

        addToSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

        If addToSourceMap Is Nothing Then

            addToSourceMap = sourceMap.Clone

            addToSourceMap.DomainMap = targetDomainMap

        End If

        If Len(classMap.Table) > 0 Then

            tableName = classMap.Table

        Else

            tableName = GetTableNameForClass(classMap)

        End If

        tableMap = addToSourceMap.GetTableMap(tableName)

        If tableMap Is Nothing Then

            tableMap = New tableMap

            tableMap.Name = tableName

            tableMap.SourceMap = addToSourceMap

        End If

        If Len(classMap.TypeColumn) > 0 Then

            name = classMap.TypeColumn

        Else

            name = GetTypeColumnNameForClass(classMap)

        End If


        columnMap = tableMap.GetColumnMap(name)

        If columnMap Is Nothing Then

            columnMap = New columnMap

            columnMap.Name = name

            columnMap.TableMap = tableMap

        End If

        columnMap.DataType = DbType.AnsiStringFixedLength
        columnMap.Length = 1
        columnMap.Precision = 1
        columnMap.Scale = 0

        columnMap.IsPrimaryKey = True
        columnMap.AllowNulls = False

        If generateMappings And Len(classMap.TypeColumn) < 1 Then

            classMapClone = targetDomainMap.GetClassMap(classMap.Name)

            If classMapClone Is Nothing Then

                classMapClone = classMap.Clone

                classMapClone.DomainMap = targetDomainMap

            End If

            classMapClone.TypeColumn = name

        End If


    End Sub


    Protected Overridable Sub CreateTableForProperty(ByVal propertyMap As IPropertyMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim classMap As IClassMap = propertyMap.ClassMap
        Dim tableMap As ITableMap
        Dim sourceMap As ISourceMap
        Dim addToSourceMap As ISourceMap
        Dim classMapClone As IClassMap
        Dim propertyMapClone As IPropertyMap
        Dim columnMap As IColumnMap

        Dim name As String

        sourceMap = classMap.GetSourceMap

        If sourceMap Is Nothing Then

            sourceMap = classMap.DomainMap.GetSourceMap

            If sourceMap Is Nothing Then

                Throw New Exception("No default data source specified for domain model! Can't add table for class!")

            End If

        End If

        addToSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

        If addToSourceMap Is Nothing Then

            addToSourceMap = sourceMap.Clone

            addToSourceMap.DomainMap = targetDomainMap

        End If

        If Len(propertyMap.Table) > 0 Then

            name = propertyMap.Table

        Else

            name = GetTableNameForProperty(propertyMap)

        End If

        tableMap = addToSourceMap.GetTableMap(name)

        If tableMap Is Nothing Then

            tableMap = New tableMap

            tableMap.Name = name

            tableMap.SourceMap = addToSourceMap

        End If

        If generateMappings And Len(propertyMap.Table) < 1 Then

            classMapClone = targetDomainMap.GetClassMap(classMap.Name)

            If classMapClone Is Nothing Then

                classMapClone = classMap.Clone

                classMapClone.Table = tableMap.Name

                If Not addToSourceMap.Name = targetDomainMap.Source Then

                    classMapClone.Source = addToSourceMap.Name

                End If

                classMapClone.DomainMap = targetDomainMap

            End If

            propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

            If propertyMapClone Is Nothing Then

                'Clone property
                propertyMapClone = propertyMap.Clone

                propertyMapClone.ClassMap = classMapClone

            End If

            propertyMapClone.Table = tableMap.Name

            If Not addToSourceMap.Name = classMapClone.Source Then

                If Not addToSourceMap.Name = targetDomainMap.Source Then

                    propertyMapClone.Source = addToSourceMap.Name

                End If

            End If

        End If


    End Sub


    Protected Overridable Sub CreateColumnForProperty(ByVal propertyMap As IPropertyMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean, ByVal ownerClassMap As IClassMap)

        Dim classMap As IClassMap = propertyMap.ClassMap
        Dim tableMap As ITableMap
        Dim sourceMap As ISourceMap
        Dim addToSourceMap As ISourceMap
        Dim classMapClone As IClassMap
        Dim propertyMapClone As IPropertyMap
        Dim columnMap As IColumnMap
        Dim classTableName As String
        Dim name As String
        Dim tableName As String
        Dim allowNulls As Boolean

        Dim refClassMap As IClassMap
        Dim forTableName As String
        Dim forColumnName As String
        Dim idProp As IPropertyMap

        Dim cnt As Integer
        Dim found As Boolean
        Dim primColName As String

        Dim isClassTableOrConcrete As Boolean
        Dim ownerClassTableName As String

        Dim foreignKeyName As String

        sourceMap = classMap.GetSourceMap

        If sourceMap Is Nothing Then

            sourceMap = classMap.DomainMap.GetSourceMap

            If sourceMap Is Nothing Then

                Throw New Exception("No default data source specified for domain model! Can't add table for class!")

            End If

        End If

        addToSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

        If addToSourceMap Is Nothing Then

            addToSourceMap = sourceMap.Clone

            addToSourceMap.DomainMap = targetDomainMap

        End If


        If Len(propertyMap.ClassMap.Table) > 0 Then

            classTableName = propertyMap.ClassMap.Table

        Else

            classTableName = GetTableNameForClass(propertyMap.ClassMap)

        End If

        If Len(propertyMap.Table) > 0 Then

            tableName = propertyMap.Table

        ElseIf propertyMap.IsCollection Then

            tableName = GetTableNameForProperty(propertyMap)

        Else

            If IsOutbrokenByInheritance(propertyMap, ownerClassMap) Then

                isClassTableOrConcrete = True

                ownerClassTableName = GetTableNameForClass(ownerClassMap, True)

                tableName = ownerClassTableName

            Else

                tableName = classTableName

            End If

        End If

        tableMap = addToSourceMap.GetTableMap(tableName)


        If tableMap Is Nothing Then

            tableMap = New tableMap

            tableMap.Name = tableName

            tableMap.SourceMap = addToSourceMap

        End If



        If Len(propertyMap.Column) > 0 Then

            name = propertyMap.Column

        Else

            name = GetColumnNameForProperty(propertyMap)

        End If


        columnMap = tableMap.GetColumnMap(name)

        If columnMap Is Nothing Then

            columnMap = New columnMap

            columnMap.Name = name

            columnMap.TableMap = tableMap

        End If

        columnMap.DataType = GetColumnTypeForProperty(propertyMap)
        columnMap.Length = GetColumnLengthForProperty(propertyMap)
        columnMap.Precision = GetColumnPrecisionForProperty(propertyMap)
        columnMap.Scale = GetColumnScaleForProperty(propertyMap)

        allowNulls = propertyMap.IsNullable

        If propertyMap.IsIdentity Then

            allowNulls = False

        End If
        'allowNulls = False

        'If classMap.InheritanceType = InheritanceType.SingleTableInheritance Then

        '    If Not classMap.GetInheritedClassMap Is Nothing Then

        '        If Not classMap.IsInheritedProperty(propertyMap) Then

        '            allowNulls = True

        '        End If

        '    End If

        'End If

        columnMap.AllowNulls = allowNulls

        If propertyMap.IsIdentity Then

            columnMap.IsPrimaryKey = True
            columnMap.AllowNulls = False

            If propertyMap.ReferenceType = ReferenceType.None Then

                If columnMap.DataType = DbType.Int16 Or columnMap.DataType = DbType.Int32 Or columnMap.DataType = DbType.Int64 Then

                    If propertyMap.ClassMap.GetIdentityPropertyMaps.Count = 1 Then

                        If propertyMap.GetIsAssignedBySource Then

                            columnMap.IsAutoIncrease = True
                            columnMap.Seed = 1
                            columnMap.Increment = 1

                        End If

                    End If

                End If

            End If

        End If

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            foreignKeyName = "FK_" & columnMap.TableMap.Name & "_" & columnMap.Name

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                For Each idProp In refClassMap.GetIdentityPropertyMaps

                    'skip first
                    If cnt > 0 Then

                        If Len(idProp.Column) > 0 Then

                            name = idProp.Column

                        Else

                            name = GetColumnNameForProperty(idProp)

                        End If

                        foreignKeyName += "_" & name

                    End If

                Next

            End If

            primColName = ""

            If Len(refClassMap.TypeColumn) > 0 Then

                primColName = refClassMap.TypeColumn

            Else

                If Not refClassMap.InheritanceType = InheritanceType.None Then

                    primColName = GetTypeColumnNameForClass(refClassMap)

                End If

            End If

            If Len(primColName) > 0 Then

                name = propertyMap.Name & "_" & primColName

                foreignKeyName += "_" & name

            Else

                name = propertyMap.Name

            End If


        End If

        If Not propertyMap.ReferenceType = ReferenceType.None Then

            'Create foreign key mappings
            columnMap.IsForeignKey = True

            columnMap.ForeignKeyName = foreignKeyName

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then

                If Len(refClassMap.Table) > 0 Then

                    forTableName = refClassMap.Table

                Else

                    forTableName = GetTableNameForClass(refClassMap)

                End If

                columnMap.PrimaryKeyTable = forTableName

                For Each idProp In refClassMap.GetIdentityPropertyMaps

                    If Len(idProp.Column) > 0 Then

                        forColumnName = idProp.Column

                    Else

                        forColumnName = GetColumnNameForProperty(idProp)

                    End If

                    Exit For

                Next

                columnMap.PrimaryKeyColumn = forColumnName

            End If

        End If

        If generateMappings And Len(propertyMap.Column) < 1 Then

            classMapClone = targetDomainMap.GetClassMap(classMap.Name)

            If classMapClone Is Nothing Then

                classMapClone = classMap.Clone

                If tableMap.Name = classTableName Then

                    classMapClone.Table = tableMap.Name

                    If Not addToSourceMap.Name = targetDomainMap.Source Then

                        classMapClone.Source = addToSourceMap.Name

                    End If

                End If

                classMapClone.DomainMap = targetDomainMap

            End If

            propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

            If propertyMapClone Is Nothing Then

                'Clone property
                propertyMapClone = propertyMap.Clone

                propertyMapClone.ClassMap = classMapClone

            End If

            propertyMapClone.Column = columnMap.Name

            If Not tableMap.Name = classTableName Then

                propertyMapClone.Table = tableMap.Name

                If Not addToSourceMap.Name = classMapClone.Source Then

                    If Not addToSourceMap.Name = targetDomainMap.Source Then

                        propertyMapClone.Source = addToSourceMap.Name

                    End If

                End If

            End If

        End If


        If Not propertyMap.ReferenceType = ReferenceType.None Then

            refClassMap = propertyMap.GetReferencedClassMap

            If Not refClassMap Is Nothing Then


                For Each idProp In refClassMap.GetIdentityPropertyMaps

                    'skip first
                    If cnt > 0 Then

                        If Len(idProp.Column) > 0 Then

                            name = idProp.Column

                        Else

                            name = GetColumnNameForProperty(idProp)

                        End If


                        columnMap = tableMap.GetColumnMap(name)

                        If columnMap Is Nothing Then

                            columnMap = New columnMap

                            columnMap.Name = name

                            columnMap.TableMap = tableMap

                        End If

                        columnMap.DataType = GetColumnTypeForProperty(idProp)
                        columnMap.Length = GetColumnLengthForProperty(idProp)
                        columnMap.Precision = GetColumnPrecisionForProperty(idProp)
                        columnMap.Scale = GetColumnScaleForProperty(idProp)

                        'Create foreign key mappings
                        columnMap.IsForeignKey = True

                        columnMap.ForeignKeyName = foreignKeyName

                        'We got the forTableName earlier...
                        columnMap.PrimaryKeyTable = forTableName

                        If Len(idProp.Column) > 0 Then

                            forColumnName = idProp.Column

                        Else

                            forColumnName = GetColumnNameForProperty(idProp)

                        End If

                        columnMap.PrimaryKeyColumn = forColumnName

                        If generateMappings Then

                            classMapClone = targetDomainMap.GetClassMap(classMap.Name)

                            If classMapClone Is Nothing Then

                                classMapClone = classMap.Clone

                                If tableMap.Name = classTableName Then

                                    classMapClone.Table = tableMap.Name

                                    If Not addToSourceMap.Name = targetDomainMap.Source Then

                                        classMapClone.Source = addToSourceMap.Name

                                    End If

                                End If

                                classMapClone.DomainMap = targetDomainMap

                            End If

                            propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

                            If propertyMapClone Is Nothing Then

                                'Clone property
                                propertyMapClone = propertyMap.Clone

                                propertyMapClone.ClassMap = classMapClone

                                If Not tableMap.Name = classTableName Then

                                    propertyMapClone.Table = tableMap.Name

                                    If Not addToSourceMap.Name = classMapClone.Source Then

                                        If Not addToSourceMap.Name = targetDomainMap.Source Then

                                            propertyMapClone.Source = addToSourceMap.Name

                                        End If

                                    End If

                                End If

                            End If

                            found = False

                            For Each name In propertyMapClone.AdditionalColumns

                                If name = columnMap.Name Then found = True

                            Next

                            If Not found Then

                                propertyMapClone.AdditionalColumns.Add(columnMap.Name)

                            End If

                        End If

                    End If

                    cnt += 1

                Next



                'Add type column
                If refClassMap.InheritanceType <> InheritanceType.None Then

                    If Len(refClassMap.TypeColumn) > 0 Then

                        primColName = refClassMap.TypeColumn

                    Else

                        primColName = GetTypeColumnNameForClass(refClassMap)

                    End If

                    name = propertyMap.Name & "_" & primColName


                    columnMap = tableMap.GetColumnMap(name)

                    If columnMap Is Nothing Then

                        columnMap = New columnMap

                        columnMap.Name = name

                        columnMap.TableMap = tableMap

                    End If

                    columnMap.DataType = DbType.AnsiStringFixedLength
                    columnMap.Length = 1
                    columnMap.Precision = 1
                    columnMap.Scale = 0

                    columnMap.AllowNulls = True

                    If propertyMap.IsIdentity Then

                        columnMap.AllowNulls = False

                    End If

                    'Create foreign key mappings
                    columnMap.IsForeignKey = True

                    columnMap.ForeignKeyName = foreignKeyName

                    'We got the forTableName earlier...
                    columnMap.PrimaryKeyTable = forTableName

                    columnMap.PrimaryKeyColumn = primColName

                    If generateMappings Then

                        classMapClone = targetDomainMap.GetClassMap(classMap.Name)

                        If classMapClone Is Nothing Then

                            classMapClone = classMap.Clone

                            If tableMap.Name = classTableName Then

                                classMapClone.Table = tableMap.Name

                                If Not addToSourceMap.Name = targetDomainMap.Source Then

                                    classMapClone.Source = addToSourceMap.Name

                                End If

                            End If

                            classMapClone.DomainMap = targetDomainMap

                        End If

                        propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

                        If propertyMapClone Is Nothing Then

                            'Clone property
                            propertyMapClone = propertyMap.Clone

                            propertyMapClone.ClassMap = classMapClone

                            If Not tableMap.Name = classTableName Then

                                propertyMapClone.Table = tableMap.Name

                                If Not addToSourceMap.Name = classMapClone.Source Then

                                    If Not addToSourceMap.Name = targetDomainMap.Source Then

                                        propertyMapClone.Source = addToSourceMap.Name

                                    End If

                                End If

                            End If

                        End If

                        found = False

                        For Each name In propertyMapClone.AdditionalColumns

                            If name = columnMap.Name Then found = True

                        Next

                        If Not found Then

                            propertyMapClone.AdditionalColumns.Add(columnMap.Name)

                        End If

                    End If


                End If

            End If

        End If


    End Sub


    Protected Overridable Sub CreateIDColumnForProperty(ByVal propertyMap As IPropertyMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean, ByVal ownerClassMap As IClassMap)

        Dim classMap As IClassMap = propertyMap.ClassMap
        Dim tableMap As ITableMap
        Dim sourceMap As ISourceMap
        Dim addToSourceMap As ISourceMap
        Dim classMapClone As IClassMap
        Dim propertyMapClone As IPropertyMap
        Dim columnMap As IColumnMap
        Dim classTableName As String
        Dim ownerClassTableName As String
        Dim name As String
        Dim forColName As String
        Dim tableName As String
        Dim idPropertyMap As IPropertyMap
        Dim typeColumnName As String
        Dim typeColumnMap As IColumnMap

        Dim foreignKeyName As String

        Dim isClassTableOrConcrete As Boolean

        sourceMap = classMap.GetSourceMap

        If sourceMap Is Nothing Then

            sourceMap = classMap.DomainMap.GetSourceMap

            If sourceMap Is Nothing Then

                Throw New Exception("No default data source specified for domain model! Can't add table for class!")

            End If

        End If

        addToSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

        If addToSourceMap Is Nothing Then

            addToSourceMap = sourceMap.Clone

            addToSourceMap.DomainMap = targetDomainMap

        End If


        If Len(propertyMap.ClassMap.Table) > 0 Then

            classTableName = propertyMap.ClassMap.Table

        Else

            classTableName = GetTableNameForClass(propertyMap.ClassMap)

        End If

        If Len(propertyMap.Table) > 0 Then

            tableName = propertyMap.Table

        ElseIf propertyMap.IsCollection Then

            tableName = GetTableNameForProperty(propertyMap)

        Else

            If IsOutbrokenByInheritance(propertyMap, ownerClassMap) Then

                isClassTableOrConcrete = True

                ownerClassTableName = GetTableNameForClass(ownerClassMap, True)

                tableName = ownerClassTableName

            Else

                tableName = classTableName

            End If


        End If

        tableMap = addToSourceMap.GetTableMap(tableName)


        If tableMap Is Nothing Then

            tableMap = New tableMap

            tableMap.Name = tableName

            tableMap.SourceMap = addToSourceMap

        End If

        foreignKeyName = "FK_" & tableMap.Name

        For Each idPropertyMap In propertyMap.ClassMap.GetIdentityPropertyMaps

            If Len(idPropertyMap.Column) > 0 Then

                forColName = idPropertyMap.Column

            Else

                forColName = GetColumnNameForProperty(idPropertyMap)

            End If

            foreignKeyName += "_" & forColName

        Next


        If Not classMap.InheritanceType = InheritanceType.None Then

            If Len(classMap.TypeColumn) > 0 Then

                foreignKeyName += "_" & classMap.TypeColumn

            Else

                foreignKeyName += "_" & GetTypeColumnNameForClass(classMap)

            End If

        End If


        For Each idPropertyMap In propertyMap.ClassMap.GetIdentityPropertyMaps

            If Len(idPropertyMap.Column) > 0 Then

                forColName = idPropertyMap.Column

            Else

                forColName = GetColumnNameForProperty(idPropertyMap)

            End If

            If Len(propertyMap.IdColumn) > 0 Then

                name = propertyMap.IdColumn

            Else

                name = forColName

            End If


            columnMap = tableMap.GetColumnMap(name)

            If columnMap Is Nothing Then

                columnMap = New columnMap

                columnMap.Name = name

                columnMap.TableMap = tableMap

            End If

            columnMap.DataType = GetColumnTypeForProperty(idPropertyMap)
            columnMap.Length = GetColumnLengthForProperty(idPropertyMap)
            columnMap.Precision = GetColumnPrecisionForProperty(idPropertyMap)
            columnMap.Scale = GetColumnScaleForProperty(idPropertyMap)

            If Not propertyMap.IsCollection Then

                columnMap.IsPrimaryKey = True
                columnMap.AllowNulls = False

            End If


            If propertyMap.IsCollection Or Len(propertyMap.Table) > 0 Or isClassTableOrConcrete Then

                columnMap.IsForeignKey = True
                columnMap.PrimaryKeyTable = classTableName
                columnMap.PrimaryKeyColumn = forColName

                columnMap.ForeignKeyName = foreignKeyName

            End If

            If generateMappings And Len(propertyMap.IdColumn) < 1 Then

                classMapClone = targetDomainMap.GetClassMap(classMap.Name)

                If classMapClone Is Nothing Then

                    classMapClone = classMap.Clone

                    classMapClone.DomainMap = targetDomainMap

                End If

                propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

                If propertyMapClone Is Nothing Then

                    'Clone property
                    propertyMapClone = propertyMap.Clone

                    propertyMapClone.ClassMap = classMapClone

                End If

                propertyMapClone.IdColumn = columnMap.Name

                If Not tableMap.Name = classTableName Then

                    propertyMapClone.Table = tableMap.Name

                    If Not addToSourceMap.Name = classMapClone.Source Then

                        If Not addToSourceMap.Name = targetDomainMap.Source Then

                            propertyMapClone.Source = addToSourceMap.Name

                        End If

                    End If

                End If

            End If


        Next

        'Type column
        If Not classMap.InheritanceType = InheritanceType.None Then

            If Len(classMap.TypeColumn) > 0 Then

                name = classMap.TypeColumn

            Else

                name = GetTypeColumnNameForClass(classMap)

            End If


            columnMap = tableMap.GetColumnMap(name)

            If columnMap Is Nothing Then

                columnMap = New columnMap

                columnMap.Name = name

                columnMap.TableMap = tableMap

            End If

            columnMap.DataType = DbType.AnsiStringFixedLength
            columnMap.Length = 1
            columnMap.Precision = 1
            columnMap.Scale = 0

            If Not propertyMap.IsCollection Then

                columnMap.IsPrimaryKey = True
                columnMap.ForeignKeyName = foreignKeyName
                columnMap.AllowNulls = False

            End If

            columnMap.IsForeignKey = True
            columnMap.PrimaryKeyTable = classTableName
            columnMap.PrimaryKeyColumn = name

            If generateMappings Then

                classMapClone = targetDomainMap.GetClassMap(classMap.Name)

                If classMapClone Is Nothing Then

                    classMapClone = classMap.Clone

                    classMapClone.DomainMap = targetDomainMap

                End If

                propertyMapClone = classMapClone.GetPropertyMap(propertyMap.Name)

                If propertyMapClone Is Nothing Then

                    'Clone property
                    propertyMapClone = propertyMap.Clone

                    propertyMapClone.ClassMap = classMapClone

                End If

                For Each name In propertyMapClone.AdditionalIdColumns

                    If name = columnMap.Name Then Exit Sub

                Next

                propertyMapClone.AdditionalIdColumns.Add(columnMap.Name)

            End If


        End If


    End Sub

    Protected Overridable Function IsOutbrokenByInheritance(ByVal propertyMap As IPropertyMap, ByVal classMap As IClassMap) As Boolean

        If ((classMap.InheritanceType = InheritanceType.ClassTableInheritance Or _
            classMap.InheritanceType = InheritanceType.ConcreteTableInheritance) AndAlso _
            Not classMap.GetInheritedClassMap Is Nothing AndAlso _
            (classMap.InheritanceType = InheritanceType.ConcreteTableInheritance OrElse classMap.IsInheritedProperty(propertyMap) = False)) Then

            Return True

        End If

        Return False

    End Function

End Class
