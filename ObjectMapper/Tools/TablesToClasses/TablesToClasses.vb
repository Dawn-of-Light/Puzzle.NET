Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports System.Reflection
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class TablesToClasses
    Implements ITablesToClasses

    Private m_GenerateInverseProperties As Boolean = True
    Private m_SetCascadeDeleteForManyOneInverseProperties As Boolean = False

    Private m_PreventIntelliNaming As Boolean

    Private m_RemoveIllegalCharactersStrictly As Boolean

    Private m_CheckReservedNamesCSharp As Boolean = True
    Private m_CheckReservedNamesVb As Boolean = True
    Private m_CheckReservedNamesDelphi As Boolean = True

    Private m_GetClassNameForTableConverter As Object
    Private m_GetClassNameForTableConverterMethod As MethodInfo

    Private m_GetPropertyNameForColumnConverter As Object
    Private m_GetPropertyNameForColumnConverterMethod As MethodInfo

    Private m_GetInverseNameForPropertyConverter As Object
    Private m_GetInverseNameForPropertyConverterMethod As MethodInfo

    Public Property GetClassNameForTableConverter() As Object Implements ITablesToClasses.GetClassNameForTableConverter
        Get
            Return m_GetClassNameForTableConverter
        End Get
        Set(ByVal Value As Object)
            m_GetClassNameForTableConverter = Value
        End Set
    End Property

    Public Property GetClassNameForTableConverterMethod() As MethodInfo Implements ITablesToClasses.GetClassNameForTableConverterMethod
        Get
            Return m_GetClassNameForTableConverterMethod
        End Get
        Set(ByVal Value As MethodInfo)
            m_GetClassNameForTableConverterMethod = Value
        End Set
    End Property

    Public Property GetPropertyNameForColumnConverter() As Object Implements ITablesToClasses.GetPropertyNameForColumnConverter
        Get
            Return m_GetPropertyNameForColumnConverter
        End Get
        Set(ByVal Value As Object)
            m_GetPropertyNameForColumnConverter = Value
        End Set
    End Property

    Public Property GetPropertyNameForColumnConverterMethod() As MethodInfo Implements ITablesToClasses.GetPropertyNameForColumnConverterMethod
        Get
            Return m_GetPropertyNameForColumnConverterMethod
        End Get
        Set(ByVal Value As MethodInfo)
            m_GetPropertyNameForColumnConverterMethod = Value
        End Set
    End Property

    Public Property GetInverseNameForPropertyConverter() As Object Implements ITablesToClasses.GetInverseNameForPropertyConverter
        Get
            Return m_GetInverseNameForPropertyConverter
        End Get
        Set(ByVal Value As Object)
            m_GetInverseNameForPropertyConverter = Value
        End Set
    End Property

    Public Property GetInverseNameForPropertyConverterMethod() As MethodInfo Implements ITablesToClasses.GetInverseNameForPropertyConverterMethod
        Get
            Return m_GetInverseNameForPropertyConverterMethod
        End Get
        Set(ByVal Value As MethodInfo)
            m_GetInverseNameForPropertyConverterMethod = Value
        End Set
    End Property

    Protected Function CanConvertTableToClassName() As Boolean

        If m_GetClassNameForTableConverter Is Nothing Then Return False

        If m_GetClassNameForTableConverterMethod Is Nothing Then Return False

        Return True

    End Function

    Protected Function CanConvertColumnToPropertyName() As Boolean

        If m_GetPropertyNameForColumnConverter Is Nothing Then Return False

        If m_GetPropertyNameForColumnConverterMethod Is Nothing Then Return False

        Return True

    End Function

    Protected Function CanConvertPropertyToInverseName() As Boolean

        If m_GetInverseNameForPropertyConverter Is Nothing Then Return False

        If m_GetInverseNameForPropertyConverterMethod Is Nothing Then Return False

        Return True

    End Function

    Protected Function ConvertTableToClassName(ByVal tableName As String) As String

        Return Convert(m_GetClassNameForTableConverter, m_GetClassNameForTableConverterMethod, tableName)

    End Function

    Protected Function ConvertColumnToPropertyName(ByVal columnMap As IColumnMap) As String

        Return Convert(m_GetPropertyNameForColumnConverter, m_GetPropertyNameForColumnConverterMethod, columnMap)

    End Function

    Protected Function ConvertPropertyToInverseName(ByVal propertyMap As IPropertyMap) As String

        Return Convert(m_GetInverseNameForPropertyConverter, m_GetInverseNameForPropertyConverterMethod, propertyMap)

    End Function

    Protected Function Convert(ByVal converter As Object, ByVal converterMethod As MethodInfo, ByVal obj As Object) As Object

        Dim parameters() As Object
        Dim result As Object

        ReDim parameters(0)

        parameters(0) = obj

        result = converterMethod.Invoke(converter, parameters)

        Return result

    End Function


    Public Overridable Property RemoveIllegalCharactersStrictly() As Boolean Implements ITablesToClasses.RemoveIllegalCharactersStrictly
        Get
            Return m_RemoveIllegalCharactersStrictly
        End Get
        Set(ByVal Value As Boolean)
            m_RemoveIllegalCharactersStrictly = Value
        End Set
    End Property


    Public Overridable Property CheckReservedNamesCSharp() As Boolean Implements ITablesToClasses.CheckReservedNamesCSharp
        Get
            Return m_CheckReservedNamesCSharp
        End Get
        Set(ByVal Value As Boolean)
            m_CheckReservedNamesCSharp = Value
        End Set
    End Property

    Public Overridable Property CheckReservedNamesVb() As Boolean Implements ITablesToClasses.CheckReservedNamesVb
        Get
            Return m_CheckReservedNamesVb
        End Get
        Set(ByVal Value As Boolean)
            m_CheckReservedNamesVb = Value
        End Set
    End Property

    Public Overridable Property CheckReservedNamesDelphi() As Boolean Implements ITablesToClasses.CheckReservedNamesDelphi
        Get
            Return m_CheckReservedNamesDelphi
        End Get
        Set(ByVal Value As Boolean)
            m_CheckReservedNamesDelphi = Value
        End Set
    End Property


    Public Overridable Function GetClassNameForTable(ByVal tableMap As ITableMap) As String Implements ITablesToClasses.GetClassNameForTable

        Return DoGetClassNameForTable(tableMap.Name)

    End Function

    Protected Overridable Function RemoveIllegalCharactersFromName(ByVal name As String) As String

        If RemoveIllegalCharactersStrictly Then

            name = RemoveIllegalCharactersFromNameStrict(name)

        Else

            name = RemoveIllegalCharactersFromNameLenient(name)

        End If

        If Not m_PreventIntelliNaming Then

            If name = UCase(name) Then

                name = Left(name, 1) & LCase(Right(name, Len(name) - 1))

            End If

        End If

        Return name

    End Function

    'ContainsIllegalCharacters(classMap.Name)

    Protected Overridable Function ContainsIllegalCharacters(ByVal name As String) As Boolean

        Dim checkName As String = RemoveIllegalCharactersFromName(name)

        If Not UCase(checkName) = UCase(name) Then Return True

    End Function

    Protected Overridable Function RemoveIllegalCharactersFromNameLenient(ByVal name As String) As String

        name = Replace(name, " ", "_")
        name = Replace(name, ".", "_")
        name = Replace(name, ",", "_")
        name = Replace(name, "!", "_")
        name = Replace(name, """", "_")
        name = Replace(name, "#", "_")
        name = Replace(name, "¤", "_")
        name = Replace(name, "%", "_")
        name = Replace(name, "&", "_")
        name = Replace(name, "/", "_")
        name = Replace(name, "(", "_")
        name = Replace(name, ")", "_")
        name = Replace(name, "=", "_")
        name = Replace(name, "?", "_")
        name = Replace(name, "'", "_")
        name = Replace(name, "<", "_")
        name = Replace(name, ">", "_")
        name = Replace(name, "[", "_")
        name = Replace(name, "]", "_")
        name = Replace(name, "{", "_")
        name = Replace(name, "}", "_")
        name = Replace(name, "+", "_")
        name = Replace(name, "\", "_")
        name = Replace(name, "@", "_")
        name = Replace(name, "£", "_")
        name = Replace(name, "$", "_")
        name = Replace(name, "|", "_")
        name = Replace(name, "-", "_")
        name = Replace(name, ";", "_")
        name = Replace(name, ":", "_")
        name = Replace(name, "§", "_")

        If Not m_PreventIntelliNaming Then name = Replace(name, "_", "")

        Return name

    End Function


    Protected Overridable Function RemoveIllegalCharactersFromNameStrict(ByVal name As String) As String

        Dim ch As String
        Dim i As Integer
        Dim fixed As String

        For i = 0 To Len(name) - 1

            ch = name.Substring(i, 1)

            If Asc(ch) >= Asc("a") And Asc(ch) <= Asc("z") Then

            Else

                If Asc(ch) >= Asc("A") And Asc(ch) <= Asc("Z") Then

                Else

                    If Asc(ch) >= Asc("0") And Asc(ch) <= Asc("9") Then

                    Else

                        ch = ""

                    End If

                End If

            End If

            fixed += ch

        Next

        Return fixed

    End Function

    Protected Overridable Function DoGetClassNameForTable(ByVal name As String) As String

        Dim retName As String

        If CanConvertTableToClassName() Then

            Try

                retName = ConvertTableToClassName(name)

                If Not retName = "" Then Return retName

            Catch ex As Exception

            End Try

        End If

        'Remove eventual tbl prefix
        If Len(name) > 3 Then
            If LCase(Left(name, 3)) = "tbl" Then name = Right(name, Len(name) - 3)
        End If

        'Make singular
        name = MakeSingularName(name)

        name = RemoveIllegalCharactersFromName(name)

        Return name

    End Function

    Public Property PreventIntelliNaming() As Boolean Implements ITablesToClasses.PreventIntelliNaming
        Get
            Return m_PreventIntelliNaming
        End Get
        Set(ByVal Value As Boolean)
            m_PreventIntelliNaming = Value
        End Set
    End Property

    Public Overridable Overloads Function GetPropertyNameForColumn(ByVal columnMap As IColumnMap) As String Implements ITablesToClasses.GetPropertyNameForColumn

        Dim name As String

        If CanConvertColumnToPropertyName() Then

            Try

                name = ConvertColumnToPropertyName(columnMap)

                If Not name = "" Then Return name

            Catch ex As Exception

            End Try

        End If

        Return GetPropertyNameForColumn(columnMap.Name, columnMap.IsPrimaryKey, columnMap.IsForeignKey, columnMap.TableMap.GetPrimaryKeyColumnMaps.Count)

    End Function


    Public Overridable Overloads Function GetPropertyNameForColumn(ByVal name As String, ByVal isPrimaryKey As Boolean, ByVal isForeignKey As Boolean) As String Implements ITablesToClasses.GetPropertyNameForColumn

        Return GetPropertyNameForColumn(name, isPrimaryKey, isForeignKey, 1)

    End Function


    Public Overridable Overloads Function GetPropertyNameForColumn(ByVal name As String, ByVal isPrimaryKey As Boolean, ByVal isForeignKey As Boolean, ByVal cntPrimaryKeys As Integer) As String Implements ITablesToClasses.GetPropertyNameForColumn

        'Remove eventual tbl prefix
        If Len(name) > 3 Then
            If LCase(Left(name, 3)) = "col" Then name = Right(name, Len(name) - 3)
        End If

        'Remove eventual ID suffix
        If Len(name) > 2 Then
            If LCase(Right(name, 2)) = "id" Then
                If isPrimaryKey And isForeignKey Then
                    name = Left(name, Len(name) - 2)
                Else
                    If isPrimaryKey Then
                        If cntPrimaryKeys < 2 Then
                            name = "Id"
                        End If
                    ElseIf isForeignKey Then
                        name = Left(name, Len(name) - 2)
                    End If
                End If
            End If
        End If

        name = RemoveIllegalCharactersFromName(name)

        Return name

    End Function

    Public Overridable Function GetPropertyTypeFromColumn(ByVal columnMap As IColumnMap) As String Implements ITablesToClasses.GetPropertyTypeFromColumn

        Dim propertyType As String

        Select Case columnMap.DataType

            Case DbType.AnsiString, DbType.AnsiStringFixedLength, DbType.String, DbType.StringFixedLength
                propertyType = "System.String"
            Case DbType.Binary
                propertyType = "System.Byte()"
            Case DbType.Boolean
                propertyType = "System.Boolean"
            Case DbType.Byte
                propertyType = "System.Byte"
            Case DbType.Currency
                propertyType = "System.Decimal"
            Case DbType.Date, DbType.DateTime, DbType.Time
                propertyType = "System.DateTime"
            Case DbType.Decimal
                propertyType = "System.Decimal"
            Case DbType.Double
                propertyType = "System.Double"
            Case DbType.Guid
                propertyType = "System.Guid"
            Case DbType.Int16
                propertyType = "System.Int16"
            Case DbType.Int32
                propertyType = "System.Int32"
            Case DbType.Int64
                propertyType = "System.Int64"
            Case DbType.Object
                propertyType = "System.Byte()"
            Case DbType.[SByte]
                propertyType = "System.Byte"
            Case DbType.Single
                propertyType = "System.Single"
            Case DbType.UInt16
                propertyType = "System.UInt16"
            Case DbType.UInt32
                propertyType = "System.UInt32"
            Case DbType.UInt64
                propertyType = "System.UInt64"
            Case DbType.VarNumeric
                propertyType = "System.Decimal"

        End Select

        Return propertyType


    End Function

    Public Overridable Sub SetPropertyTypeFromColumn(ByVal propertyMap As IPropertyMap, ByVal columnMap As IColumnMap) Implements ITablesToClasses.SetPropertyTypeFromColumn

        If Not columnMap.IsForeignKey Then

            propertyMap.ReferenceType = ReferenceType.None

            propertyMap.DataType = GetPropertyTypeFromColumn(columnMap)

            If UCase(columnMap.SpecificDataType) = "TIMESTAMP" Then

                propertyMap.IsReadOnly = True

            End If

        Else

            If IsOneOneRelation(propertyMap, columnMap) Then

                propertyMap.ReferenceType = ReferenceType.OneToOne

            Else

                propertyMap.ReferenceType = ReferenceType.OneToMany

            End If

            Dim primaryTableMap As ITableMap = columnMap.GetPrimaryKeyTableMap

            If primaryTableMap Is Nothing Then Exit Sub

            Dim refClassMap As IClassMap

            Dim refClassMaps As ArrayList = propertyMap.ClassMap.DomainMap.GetClassMapsForTable(primaryTableMap)

            Dim invName As String
            Dim invPropertyMap As IPropertyMap

            For Each refClassMap In refClassMaps

                If Len(refClassMap.InheritsClass) < 1 Then

                    propertyMap.DataType = refClassMap.Name

                    If GenerateInverseProperties Then

                        GenerateInverseProperty(propertyMap, refClassMap)

                    End If

                    Exit For

                End If

            Next


        End If

    End Sub

    Public Overridable Overloads Sub GenerateInverseProperty(ByVal propertyMap As IPropertyMap) Implements ITablesToClasses.GenerateInverseProperty

        Dim refClassMap As IClassMap = propertyMap.GetReferencedClassMap

        GenerateInverseProperty(propertyMap, refClassMap)

    End Sub


    Private Function GetUniquePropertyName(ByVal classMap As IClassMap, ByVal name As String) As String

        Dim propertyMap As IPropertyMap
        Dim i As Long = 1
        Dim basename As String = name

        While Not classMap.GetPropertyMap(name) Is Nothing

            name = basename & i

            i += 1

        End While

        Return name

    End Function

    Public Overridable Function GetInversePropertyName(ByVal propertyMap As IPropertyMap) As String

        Dim name As String

        If CanConvertPropertyToInverseName() Then

            Try

                name = ConvertPropertyToInverseName(propertyMap)

                If Not name = "" Then Return name

            Catch ex As Exception

            End Try

        End If


        Dim invName As String
        Dim refClassMap As IClassMap = propertyMap.GetReferencedClassMap

        If propertyMap.ReferenceType = ReferenceType.OneToOne Then

            invName = propertyMap.ClassMap.GetName

        Else

            invName = MakePluralName(propertyMap.ClassMap.GetName)

            If Not refClassMap.GetPropertyMap(invName) Is Nothing Then

                invName = propertyMap.Name & MakePluralName(propertyMap.ClassMap.GetName)

            End If

        End If

        invName = GetUniquePropertyName(refClassMap, invName)

        Return invName

    End Function

    Public Overridable Overloads Sub GenerateInverseProperty(ByVal propertyMap As IPropertyMap, ByVal refClassMap As IClassMap) Implements ITablesToClasses.GenerateInverseProperty

        Dim invPropertyMap As IPropertyMap
        Dim invName As String = GetInversePropertyName(propertyMap)

        propertyMap.Inverse = invName

        invPropertyMap = New propertyMap

        invPropertyMap.Name = invName

        invPropertyMap.ClassMap = refClassMap

        Select Case propertyMap.ReferenceType

            Case ReferenceType.ManyToMany

                invPropertyMap.IsCollection = True
                invPropertyMap.DataType = "System.Collections.IList"
                'invPropertyMap.DataType = "Puzzle.NPersist.Framework.ManagedList"
                'invPropertyMap.DataType = "System.Collections.ArrayList"
                invPropertyMap.ItemType = propertyMap.ClassMap.Name
                invPropertyMap.IsSlave = True
                invPropertyMap.InheritInverseMappings = True

            Case ReferenceType.OneToMany

                invPropertyMap.IsCollection = True
                invPropertyMap.DataType = "System.Collections.IList"
                'invPropertyMap.DataType = "System.Collections.ArrayList"
                invPropertyMap.ItemType = propertyMap.ClassMap.Name
                invPropertyMap.IsSlave = True
                invPropertyMap.InheritInverseMappings = True

            Case ReferenceType.ManyToOne

                invPropertyMap.DataType = propertyMap.ClassMap.Name
                propertyMap.IsSlave = True
                propertyMap.InheritInverseMappings = True

            Case ReferenceType.OneToOne

                invPropertyMap.DataType = propertyMap.ClassMap.Name
                invPropertyMap.IsSlave = True
                invPropertyMap.InheritInverseMappings = True

        End Select

        Select Case propertyMap.ReferenceType

            Case ReferenceType.ManyToMany

                invPropertyMap.ReferenceType = ReferenceType.ManyToMany

            Case ReferenceType.ManyToOne

                invPropertyMap.ReferenceType = ReferenceType.OneToMany

            Case ReferenceType.OneToMany

                invPropertyMap.ReferenceType = ReferenceType.ManyToOne

                If SetCascadeDeleteForManyOneInverseProperties Then

                    invPropertyMap.CascadingDelete = True

                End If

            Case ReferenceType.OneToOne

                invPropertyMap.ReferenceType = ReferenceType.OneToOne

        End Select

        invPropertyMap.Inverse = propertyMap.Name

    End Sub

    Public Overridable Overloads Sub GenerateClassesForSources(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap) Implements ITablesToClasses.GenerateClassesForSources

        GenerateClassesForSources(sourceDomainMap, targetDomainMap, False)

    End Sub

    Public Overridable Overloads Sub GenerateClassesForSources(ByVal sourceDomainMap As IDomainMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean) Implements ITablesToClasses.GenerateClassesForSources

        Dim sourceMap As ISourceMap

        For Each sourceMap In targetDomainMap.SourceMaps
            '        For Each sourceMap In sourceDomainMap.SourceMaps

            GenerateClassesForTables(sourceMap, targetDomainMap, generateMappings)

        Next

    End Sub


    Public Overridable Overloads Sub GenerateClassesForTables(ByVal sourceMap As ISourceMap, ByVal targetDomainMap As IDomainMap) Implements ITablesToClasses.GenerateClassesForTables

        GenerateClassesForTables(sourceMap, targetDomainMap, False)

    End Sub


    Public Overridable Overloads Sub GenerateClassesForTables(ByVal sourceMap As ISourceMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean) Implements ITablesToClasses.GenerateClassesForTables

        Dim tableMap As ITableMap
        Dim columnMap As IColumnMap

        For Each tableMap In sourceMap.TableMaps

            If HasPrimaryKey(tableMap) Then

                CreateClassForTable(tableMap, targetDomainMap, generateMappings)

            End If

        Next

        For Each tableMap In sourceMap.TableMaps

            If HasPrimaryKey(tableMap) Then

                If IsManyManyTable(tableMap) Then

                    CreatePropertiesForManyManyTable(tableMap, targetDomainMap, generateMappings)

                Else

                    'For Each columnMap In tableMap.ColumnMaps

                    '    CreatePropertyForColumn(columnMap, targetDomainMap, generateMappings)

                    'Next

                End If

            End If

        Next

        For Each tableMap In sourceMap.TableMaps

            If HasPrimaryKey(tableMap) Then

                If IsManyManyTable(tableMap) Then

                    'CreatePropertiesForManyManyTable(tableMap, targetDomainMap, generateMappings)

                Else

                    For Each columnMap In tableMap.ColumnMaps

                        CreatePropertyForColumn(columnMap, targetDomainMap, generateMappings)

                    Next

                End If

            End If

        Next

    End Sub


    Protected Overridable Sub CreateClassForTable(ByVal tableMap As ITableMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim classMap As IClassMap
        Dim classMapClone As IClassMap
        Dim domainMap As IDomainMap = tableMap.SourceMap.DomainMap
        Dim sourceMapClone As ISourceMap
        Dim tableMapClone As ITableMap
        Dim columnMap As IColumnMap

        'a table with only foreign key columns to two tables should not get a class
        If IsManyManyTable(tableMap) Then Exit Sub

        Dim mappedClasses As ArrayList = domainMap.GetClassMapsForTable(tableMap)

        Dim name As String

        If mappedClasses.Count < 1 Then

            name = GetClassNameForTable(tableMap)

            'check if there is a name conflict:
            classMap = domainMap.GetClassMap(name)

            'If Not classMap Is Nothing Then

            '    If Not classMap.GetTableMap Is tableMap Then

            '        classMap = Nothing

            '    End If

            'End If

            'The source domain already contains a class with the suggested name! It should just be mapped!
            If Not classMap Is Nothing Then

                If generateMappings Then

                    'The class map is already mapping to a table and this mapping should not be overwritten!
                    If Len(classMap.Table) > 0 Then

                        Exit Sub

                    End If

                    classMapClone = targetDomainMap.GetClassMap(name)

                    If classMapClone Is Nothing Then

                        classMapClone = classMap.Clone

                    End If

                    classMapClone.Table = tableMap.Name

                    sourceMapClone = targetDomainMap.GetSourceMap(tableMap.SourceMap.Name)

                    If sourceMapClone Is Nothing Then

                        sourceMapClone = tableMap.SourceMap.Clone

                        sourceMapClone.DomainMap = targetDomainMap

                    End If

                    tableMapClone = sourceMapClone.GetTableMap(tableMap.Name)

                    If tableMapClone Is Nothing Then

                        tableMapClone = tableMap.Clone

                        tableMapClone.SourceMap = sourceMapClone

                    End If

                End If

            Else

                classMap = targetDomainMap.GetClassMap(name)

                'If Not classMap Is Nothing Then

                '    If Not classMap.GetTableMap Is tableMap Then

                '        classMap = Nothing

                '    End If

                'End If

                If classMap Is Nothing Then

                    classMap = New classMap

                    classMap.Name = name

                    classMap.DomainMap = targetDomainMap

                End If

                If generateMappings Then

                    classMap.Table = tableMap.Name

                    sourceMapClone = targetDomainMap.GetSourceMap(tableMap.SourceMap.Name)

                    If sourceMapClone Is Nothing Then

                        sourceMapClone = tableMap.SourceMap.Clone

                        sourceMapClone.DomainMap = targetDomainMap

                    End If

                    tableMapClone = sourceMapClone.GetTableMap(tableMap.Name)

                    If tableMapClone Is Nothing Then

                        tableMapClone = tableMap.Clone

                        tableMapClone.SourceMap = sourceMapClone

                    End If

                End If

            End If

        End If

    End Sub


    Protected Overridable Sub CreatePropertyForColumn(ByVal columnMap As IColumnMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim classMap As IClassMap
        Dim classMapClone As IClassMap
        Dim domainMap As IDomainMap = columnMap.TableMap.SourceMap.DomainMap
        Dim sourceMapClone As ISourceMap
        Dim tableMapClone As ITableMap
        Dim columnMapClone As IColumnMap
        Dim className As String
        Dim propertyMap As IPropertyMap
        Dim propertyMapClone As IPropertyMap
        Dim checkColumnMap As IColumnMap
        Dim primaryColumnMap As IColumnMap
        Dim primaryTableMap As ITableMap
        Dim isInComposite As Boolean
        Dim isFirstInComposite As Boolean
        Dim cnt As Int16
        Dim listSorted As ArrayList
        Dim additionalColumns As New ArrayList
        Dim tblMap As ITableMap

        'is this the first foreign key in a composite key relationship?
        If columnMap.IsForeignKey Then


            If Len(columnMap.ForeignKeyName) > 0 Then

                listSorted = columnMap.TableMap.GetForeignKeyColumnMaps(columnMap.ForeignKeyName)

                'If listSorted.Count > 0 Then
                If listSorted.Count > 1 Then

                    isInComposite = True

                    listSorted.Sort()

                    cnt = 0

                    For Each checkColumnMap In listSorted

                        If LCase(columnMap.Name) = LCase(checkColumnMap.Name) Then

                            If cnt = 0 Then

                                isFirstInComposite = True

                            End If

                        End If

                        cnt += 1

                    Next

                End If

                If isInComposite Then

                    If isFirstInComposite Then

                        For Each checkColumnMap In listSorted

                            If Not LCase(columnMap.Name) = LCase(checkColumnMap.Name) Then

                                additionalColumns.Add(checkColumnMap)

                            End If

                        Next

                    Else

                        Exit Sub

                    End If

                End If

            Else

                primaryTableMap = columnMap.GetPrimaryKeyTableMap

                listSorted = primaryTableMap.GetPrimaryKeyColumnMaps

                If listSorted.Count > 1 Then

                    listSorted.Sort()

                    For Each primaryColumnMap In listSorted

                        If LCase(primaryColumnMap.Name) = LCase(columnMap.Name) Then

                            isInComposite = True

                            If cnt = 0 Then

                                isFirstInComposite = True

                            End If

                        End If

                        cnt += 1

                    Next

                End If

                If isInComposite Then

                    If isFirstInComposite Then

                        'find the other columns that are part of the relationship
                        'Multiple relationships from a table to a different table
                        'using composite keys is currently not possible.
                        'It could have been done if we had map objects for the actual contraints...
                        For Each checkColumnMap In columnMap.TableMap.ColumnMaps

                            If Not checkColumnMap Is columnMap Then

                                If checkColumnMap.IsForeignKey Then

                                    If LCase(checkColumnMap.PrimaryKeyTable) = LCase(primaryTableMap.Name) Then

                                        For Each primaryColumnMap In listSorted

                                            If LCase(primaryColumnMap.Name) = LCase(checkColumnMap.Name) Then

                                                additionalColumns.Add(checkColumnMap)

                                                Exit For

                                            End If

                                        Next

                                    End If

                                End If

                            End If

                        Next

                    Else

                        Exit Sub

                    End If

                End If

            End If

        End If


        Dim mappedProperties As ArrayList = domainMap.GetPropertyMapsForColumn(columnMap)

        Dim name As String

        If mappedProperties.Count < 1 Then

            className = GetClassNameForTable(columnMap.TableMap)

            name = GetPropertyNameForColumn(columnMap)

            classMapClone = targetDomainMap.GetClassMap(className)

            If classMapClone Is Nothing Then

                classMap = domainMap.GetClassMap(className)

                If Not classMap Is Nothing Then

                    classMapClone = classMap.Clone

                    classMapClone.DomainMap = targetDomainMap

                Else

                    'The table has no class mapping to it!
                    Exit Sub

                End If

            End If

            If Not LCase(classMapClone.Table) = LCase(columnMap.TableMap.Name) Then

                'The table has no class mapping to it!
                Exit Sub

            End If

            'check if there is a name conflict:
            propertyMap = classMapClone.GetPropertyMap(name)

            'The target class map already contains the property with the suggested name!
            If Not propertyMap Is Nothing Then

                If generateMappings Then

                    If Len(propertyMap.Column) > 0 Then

                        Exit Sub

                    End If

                    propertyMap.Column = columnMap.Name



                    sourceMapClone = targetDomainMap.GetSourceMap(columnMap.TableMap.SourceMap.Name)

                    If sourceMapClone Is Nothing Then

                        sourceMapClone = columnMap.TableMap.SourceMap.Clone

                        sourceMapClone.DomainMap = targetDomainMap

                    End If

                    tableMapClone = sourceMapClone.GetTableMap(columnMap.TableMap.Name)

                    If tableMapClone Is Nothing Then

                        tableMapClone = columnMap.TableMap.Clone

                        tableMapClone.SourceMap = sourceMapClone

                    End If

                    columnMapClone = tableMapClone.GetColumnMap(columnMap.Name)

                    If columnMapClone Is Nothing Then

                        columnMapClone = columnMap.Clone

                        columnMapClone.TableMap = tableMapClone

                    End If


                End If

            Else

                propertyMap = classMapClone.GetPropertyMap(name)

                If propertyMap Is Nothing Then

                    propertyMap = New propertyMap

                    propertyMap.Name = name

                    propertyMap.ClassMap = classMapClone

                End If

                If generateMappings Then

                    propertyMap.Column = columnMap.Name

                    If columnMap.IsPrimaryKey Then

                        propertyMap.IsIdentity = True

                        propertyMap.IdentityIndex = propertyMap.ClassMap.GetIdentityPropertyMaps.Count - 1

                    End If



                    sourceMapClone = targetDomainMap.GetSourceMap(columnMap.TableMap.SourceMap.Name)

                    If sourceMapClone Is Nothing Then

                        sourceMapClone = columnMap.TableMap.SourceMap.Clone

                        sourceMapClone.DomainMap = targetDomainMap

                    End If

                    tableMapClone = sourceMapClone.GetTableMap(columnMap.TableMap.Name)

                    If tableMapClone Is Nothing Then

                        tableMapClone = columnMap.TableMap.Clone

                        tableMapClone.SourceMap = sourceMapClone

                    End If

                    columnMapClone = tableMapClone.GetColumnMap(columnMap.Name)

                    If columnMapClone Is Nothing Then

                        columnMapClone = columnMap.Clone

                        columnMapClone.TableMap = tableMapClone

                    End If

                    If Len(columnMapClone.PrimaryKeyTable) > 0 Then

                        tableMapClone = sourceMapClone.GetTableMap(columnMapClone.PrimaryKeyTable)

                        If tableMapClone Is Nothing Then

                            tblMap = columnMap.TableMap.SourceMap.GetTableMap(columnMapClone.PrimaryKeyTable)

                            If Not tblMap Is Nothing Then

                                tableMapClone = columnMap.TableMap.SourceMap.GetTableMap(columnMapClone.PrimaryKeyTable).Clone

                                tableMapClone.SourceMap = sourceMapClone

                            End If

                        End If

                        If Not tableMapClone Is Nothing Then

                            className = GetClassNameForTable(tableMapClone)

                            classMapClone = targetDomainMap.GetClassMap(className)

                            If classMapClone Is Nothing Then

                                classMap = domainMap.GetClassMap(className)

                                If Not classMap Is Nothing Then

                                    classMapClone = classMap.Clone

                                    classMapClone.DomainMap = targetDomainMap

                                    If Not classMapClone.Table = tableMapClone.Name Then

                                        Exit Sub

                                    End If

                                End If

                            End If

                        End If

                    End If

                    SetPropertyTypeFromColumn(propertyMap, columnMapClone)

                    If additionalColumns.Count > 0 Then

                        For Each checkColumnMap In additionalColumns

                            propertyMap.AdditionalColumns.Add(checkColumnMap.Name)

                        Next

                    End If

                End If

            End If

        End If

    End Sub


    Public Overridable Function MakeSingularName(ByVal name As String) As String Implements ITablesToClasses.MakeSingularName

        If m_PreventIntelliNaming Then Return name

        Dim isUcase As Boolean

        If UCase(name) = name Then isUcase = True

        If Len(name) > 3 Then

            If LCase(Right(name, 3)) = "ies" Then name = Left(name, Len(name) - 3) & "y"

        End If

        If Len(name) > 2 Then

            If LCase(Right(name, 2)) = "es" Then name = Left(name, Len(name) - 2) & "e"

        End If

        If Len(name) > 2 Then

            If LCase(Right(name, 2)) = "ss" Then name += "s"

        End If

        If Len(name) > 1 Then

            If LCase(Right(name, 1)) = "s" Then name = Left(name, Len(name) - 1)

        End If

        If isUcase Then name = UCase(name)

        Return name

    End Function

    Public Overridable Function MakePluralName(ByVal name As String) As String Implements ITablesToClasses.MakePluralName

        If m_PreventIntelliNaming Then Return name

        Dim isUcase As Boolean

        If UCase(name) = name Then isUcase = True

        If Len(name) > 2 Then

            If LCase(Right(name, 1)) = "y" Then
                name = Left(name, Len(name) - 1) & "ies"

                If isUcase Then name = UCase(name)

                Return name

            End If

        End If

        If Len(name) > 1 Then

            If LCase(Right(name, 1)) = "x" Then

                name += "es"

                If isUcase Then name = UCase(name)

                Return name

            End If

        End If

        If Len(name) > 2 Then

            If LCase(Right(name, 2)) = "ss" Then

                name += "es"

                If isUcase Then name = UCase(name)

                Return name

            End If

        End If

        If Len(name) > 1 Then

            If Not LCase(Right(name, 1)) = "s" Then

                name += "s"

                If isUcase Then name = UCase(name)

                Return name

            End If

        End If

        If isUcase Then name = UCase(name)

        Return name

    End Function


    Protected Overridable Sub CreatePropertiesForManyManyTable(ByVal tableMap As ITableMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean)

        Dim classMaps1 As ArrayList
        Dim classMaps2 As ArrayList
        Dim classMap As IClassMap
        Dim classMap1 As IClassMap
        Dim classMap2 As IClassMap
        Dim classMap1Clone As IClassMap
        Dim classMap2Clone As IClassMap
        Dim classMapClone As IClassMap
        Dim domainMap As IDomainMap = tableMap.SourceMap.DomainMap
        Dim sourceMapClone As ISourceMap
        Dim tableMapClone As ITableMap
        Dim columnMap1 As IColumnMap
        Dim columnMap1Clone As IColumnMap
        Dim columnMap2 As IColumnMap
        Dim columnMap2Clone As IColumnMap
        Dim className As String
        Dim propertyMap1 As IPropertyMap
        Dim propertyMap2 As IPropertyMap
        Dim propertyMapClone As IPropertyMap
        Dim primaryKeyTable1 As ITableMap
        Dim primaryKeyTable2 As ITableMap
        Dim isSlave As Boolean

        Dim mappedProperties As ArrayList = targetDomainMap.GetPropertyMapsForTable(tableMap)

        Dim name1 As String
        Dim name2 As String

        If mappedProperties.Count < 1 Then

            'Currently does not work with composite key relationships
            If tableMap.ColumnMaps.Count = 2 Then

                columnMap1 = tableMap.ColumnMaps(0)
                columnMap2 = tableMap.ColumnMaps(1)

                primaryKeyTable1 = columnMap1.GetPrimaryKeyTableMap
                primaryKeyTable2 = columnMap2.GetPrimaryKeyTableMap

                classMaps1 = primaryKeyTable1.SourceMap.DomainMap.GetClassMapsForTable(primaryKeyTable1)
                classMaps2 = primaryKeyTable2.SourceMap.DomainMap.GetClassMapsForTable(primaryKeyTable2)

                For Each classMap In classMaps1

                    If Len(classMap.InheritsClass) < 1 Then

                        classMap1 = classMap

                        Exit For

                    End If

                Next

                For Each classMap In classMaps2

                    If Len(classMap.InheritsClass) < 1 Then

                        classMap2 = classMap

                        Exit For

                    End If

                Next

                If classMap1 Is Nothing Then

                    classMap1 = primaryKeyTable1.SourceMap.DomainMap.GetClassMap(GetClassNameForTable(primaryKeyTable1))

                End If

                If classMap2 Is Nothing Then

                    classMap2 = primaryKeyTable1.SourceMap.DomainMap.GetClassMap(GetClassNameForTable(primaryKeyTable2))

                End If

                If classMap1 Is Nothing Then

                    name2 = MakePluralName(DoGetClassNameForTable(primaryKeyTable1.Name))

                Else

                    name2 = MakePluralName(classMap1.GetName)
                    propertyMap2 = classMap1.GetPropertyMap(name2)

                End If

                If classMap2 Is Nothing Then

                    name1 = MakePluralName(DoGetClassNameForTable(primaryKeyTable2.Name))

                Else

                    name1 = MakePluralName(classMap2.GetName)
                    propertyMap1 = classMap2.GetPropertyMap(name1)

                End If



                If propertyMap1 Is Nothing Or propertyMap2 Is Nothing Then

                    CloneColumnsAndTable(columnMap1, columnMap2, targetDomainMap, generateMappings, columnMap1Clone, columnMap2Clone)

                    ClonePrimaryTableAndClass(primaryKeyTable1, tableMap, targetDomainMap, generateMappings, classMap1Clone)

                    ClonePrimaryTableAndClass(primaryKeyTable2, tableMap, targetDomainMap, generateMappings, classMap2Clone)

                End If

                If propertyMap1 Is Nothing Then

                    propertyMap1 = New PropertyMap

                    propertyMap1.Name = name1

                    propertyMap1.ClassMap = classMap1Clone

                    propertyMap1.IsCollection = True

                    propertyMap1.ReferenceType = ReferenceType.ManyToMany

                    propertyMap1.DataType = "System.Collections.IList"
                    'propertyMap1.DataType = "System.Collections.ArrayList"
                    'propertyMap1.DataType = "Puzzle.NPersist.Framework.ManagedList"

                    propertyMap1.ItemType = classMap2Clone.Name

                    propertyMap1.Inverse = name2

                    propertyMap1.Table = columnMap1.TableMap.Name

                    propertyMap1.Column = columnMap2.Name

                    propertyMap1.IdColumn = columnMap1.Name

                    propertyMap1.InheritInverseMappings = True

                    propertyMap1.IsSlave = True

                    isSlave = False

                Else

                    isSlave = Not propertyMap1.IsSlave

                End If

                If propertyMap2 Is Nothing Then


                    propertyMap2 = New PropertyMap

                    propertyMap2.Name = name2

                    propertyMap2.ClassMap = classMap2Clone

                    propertyMap2.IsCollection = True

                    propertyMap2.ReferenceType = ReferenceType.ManyToMany

                    propertyMap2.DataType = "System.Collections.IList"
                    'propertyMap2.DataType = "System.Collections.ArrayList"

                    propertyMap2.ItemType = classMap1Clone.Name

                    propertyMap2.Inverse = name1

                    propertyMap2.Table = columnMap2.TableMap.Name

                    propertyMap2.Column = columnMap1.Name

                    propertyMap2.IdColumn = columnMap2.Name

                    If isSlave Then

                        If Not propertyMap1 Is Nothing Then

                            If Not propertyMap1.InheritInverseMappings Then

                                propertyMap2.InheritInverseMappings = True

                                propertyMap2.DataType = "System.Collections.IList"
                                'propertyMap2.DataType = "System.Collections.ArrayList"

                            End If

                        End If

                    End If

                    propertyMap2.IsSlave = isSlave

                End If

            End If

        End If

    End Sub

    Protected Overridable Sub ClonePrimaryTableAndClass(ByVal primaryKeyTable As ITableMap, ByVal tableMap As ITableMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean, ByRef classMapClone As IClassMap)

        Dim classMap As IClassMap
        Dim domainMap As IDomainMap = tableMap.SourceMap.DomainMap
        Dim sourceMapClone As ISourceMap
        Dim tableMapClone As ITableMap
        Dim className As String

        sourceMapClone = targetDomainMap.GetSourceMap(primaryKeyTable.SourceMap.Name)

        If sourceMapClone Is Nothing Then

            sourceMapClone = primaryKeyTable.SourceMap.Clone

            sourceMapClone.DomainMap = targetDomainMap

        End If

        tableMapClone = sourceMapClone.GetTableMap(primaryKeyTable.Name)

        If tableMapClone Is Nothing Then

            tableMapClone = primaryKeyTable.Clone

            tableMapClone.SourceMap = sourceMapClone

        End If

        className = GetClassNameForTable(tableMapClone)

        classMapClone = targetDomainMap.GetClassMap(className)

        If classMapClone Is Nothing Then

            classMap = domainMap.GetClassMap(className)

            If Not classMap Is Nothing Then

                classMapClone = classMap.Clone

                classMapClone.DomainMap = targetDomainMap

            Else

                Exit Sub

            End If

        End If

    End Sub

    Protected Overridable Sub CloneColumnsAndTable(ByVal columnMap1 As IColumnMap, ByVal columnMap2 As IColumnMap, ByVal targetDomainMap As IDomainMap, ByVal generateMappings As Boolean, ByRef columnMap1Clone As IColumnMap, ByRef columnMap2Clone As IColumnMap)

        Dim columnMap As IColumnMap = columnMap1
        Dim classMap As IClassMap
        Dim domainMap As IDomainMap = columnMap.TableMap.SourceMap.DomainMap
        Dim sourceMapClone As ISourceMap
        Dim tableMapClone As ITableMap
        Dim classMapClone As IClassMap

        sourceMapClone = targetDomainMap.GetSourceMap(columnMap.TableMap.SourceMap.Name)

        If sourceMapClone Is Nothing Then

            sourceMapClone = columnMap.TableMap.SourceMap.Clone

            sourceMapClone.DomainMap = targetDomainMap

        End If

        tableMapClone = sourceMapClone.GetTableMap(columnMap.TableMap.Name)

        If tableMapClone Is Nothing Then

            tableMapClone = columnMap.TableMap.Clone

            tableMapClone.SourceMap = sourceMapClone

        End If

        columnMap1Clone = tableMapClone.GetColumnMap(columnMap1.Name)

        If columnMap1Clone Is Nothing Then

            columnMap1Clone = columnMap1.Clone

            columnMap1Clone.TableMap = tableMapClone

        End If

        columnMap2Clone = tableMapClone.GetColumnMap(columnMap2.Name)

        If columnMap2Clone Is Nothing Then

            columnMap2Clone = columnMap2.Clone

            columnMap2Clone.TableMap = tableMapClone

        End If


    End Sub




    Public Overridable Property GenerateInverseProperties() As Boolean Implements ITablesToClasses.GenerateInverseProperties
        Get
            Return m_GenerateInverseProperties
        End Get
        Set(ByVal Value As Boolean)
            m_GenerateInverseProperties = Value
        End Set
    End Property

    Public Overridable Property SetCascadeDeleteForManyOneInverseProperties() As Boolean Implements ITablesToClasses.SetCascadeDeleteForManyOneInverseProperties
        Get
            Return m_SetCascadeDeleteForManyOneInverseProperties
        End Get
        Set(ByVal Value As Boolean)
            m_SetCascadeDeleteForManyOneInverseProperties = Value
        End Set
    End Property

    Protected Overridable Function IsManyManyTable(ByVal tableMap As ITableMap) As Boolean

        Dim primaryTableName As String
        Dim hashPrimaryTables As New Hashtable
        Dim cntPrimaryTables As Integer
        Dim columnMap As IColumnMap

        For Each columnMap In tableMap.ColumnMaps

            If Not columnMap.IsForeignKey Then

                Exit Function

            Else

                primaryTableName = columnMap.PrimaryKeyTable
                If Len(primaryTableName) < 1 Then Exit Function
                If Len(columnMap.PrimaryKeyColumn) < 1 Then Exit Function

                If Not hashPrimaryTables.ContainsKey(primaryTableName) Then

                    cntPrimaryTables += 1

                    hashPrimaryTables(primaryTableName) = New ArrayList

                    CType(hashPrimaryTables(primaryTableName), ArrayList).Add(columnMap.PrimaryKeyColumn)

                    If cntPrimaryTables > 2 Then Exit Function

                Else

                    If CType(hashPrimaryTables(primaryTableName), ArrayList).Contains(columnMap.PrimaryKeyColumn) Then

                        Exit Function

                    End If

                End If

            End If

        Next

        If cntPrimaryTables = 2 Then Return True


    End Function

    'Currently this method presupposes named foreign keys to work
    Protected Overridable Function IsOneOneRelation(ByVal propertyMap As IPropertyMap, ByVal columnMap As IColumnMap) As Boolean

        Dim forCols As ArrayList
        Dim primCols As ArrayList
        Dim checkColumnMap As IColumnMap

        If Len(columnMap.ForeignKeyName) > 0 Then

            forCols = columnMap.TableMap.GetForeignKeyColumnMaps(columnMap.ForeignKeyName)

            primCols = columnMap.TableMap.GetPrimaryKeyColumnMaps

            If primCols.Count = forCols.Count Then

                For Each checkColumnMap In forCols

                    If Not primCols.Contains(checkColumnMap) Then

                        Return False

                    End If

                Next

                Return True

            End If

        End If

    End Function


    Public Sub MakeSureNamesAreLegal(ByVal domainMap As IDomainMap) Implements ITablesToClasses.MakeSureNamesAreLegal

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim ok As Boolean

        For Each classMap In domainMap.ClassMaps

            If m_GetClassNameForTableConverter Is Nothing Then

                If ContainsIllegalCharacters(classMap.Name) Or IsUCase(classMap.Name) Then

                    MakeLegalName(classMap, 0)

                End If

            End If

            If m_GetPropertyNameForColumnConverter Is Nothing Then

                For Each propertyMap In classMap.PropertyMaps

                    If ContainsIllegalCharacters(propertyMap.Name) Or IsUCase(propertyMap.Name) Then

                        MakeLegalName(propertyMap, 0)

                    End If

                Next

            End If

        Next


        For Each classMap In domainMap.ClassMaps

            ok = True

            If m_GetClassNameForTableConverter Is Nothing Then

                If CheckReservedNamesCSharp Then

                    If MapBase.IsReservedCSharp(classMap.Name) Then

                        ok = False

                    End If

                End If

                If ok Then

                    If CheckReservedNamesVb Then

                        If MapBase.IsReservedVBNet(classMap.Name) Then

                            ok = False

                        End If

                    End If

                End If

                If ok Then

                    If CheckReservedNamesDelphi Then

                        If MapBase.IsReservedDelphi(classMap.Name) Then

                            ok = False

                        End If

                    End If

                End If

            End If

            If Not ok Then

                MakeLegalName(classMap, 1)

            End If

            If m_GetPropertyNameForColumnConverter Is Nothing Then

                For Each propertyMap In classMap.PropertyMaps

                    If IsIllegalPropertyName(propertyMap) Then

                        MakeLegalName(propertyMap, 1)

                    End If

                Next

            End If

        Next



    End Sub

    Private Function IsUCase(ByVal name As String) As Boolean

        If name = UCase(name) Then Return True

    End Function

    Private Function IsIllegalPropertyName(ByVal propertyMap As IPropertyMap) As Boolean

        If CheckReservedNamesCSharp Then

            If MapBase.IsReservedCSharp(propertyMap.Name) Then Return True

        End If

        If CheckReservedNamesVb Then

            If MapBase.IsReservedVBNet(propertyMap.Name) Then Return True

        End If

        If CheckReservedNamesDelphi Then

            If MapBase.IsReservedDelphi(propertyMap.Name) Then Return True

        End If

        If CheckReservedNamesCSharp Then

            If LCase(propertyMap.Name) = LCase(propertyMap.ClassMap.Name) Then Return True

        End If

    End Function

    Private Overloads Sub MakeLegalName(ByVal classMap As IClassMap, ByVal i As Long)

        Dim checkClassMap As IClassMap
        Dim testName As String = ""
        Dim finalName As String = ""
        Dim ok As Boolean

        Dim name As String = RemoveIllegalCharactersFromName(classMap.Name)

        While finalName = ""

            If i = 0 Then

                testName = name

            Else

                testName = name & i

            End If

            i += 1

            ok = True

            For Each checkClassMap In classMap.DomainMap.ClassMaps

                If Not checkClassMap Is classMap Then

                    If LCase(checkClassMap.Name) = LCase(testName) Then

                        ok = False
                        Exit For

                    End If

                End If

            Next

            If ok Then

                finalName = testName

            End If

        End While

        classMap.UpdateName(finalName)

        'classMap.Name = finalName

    End Sub

    Private Overloads Sub MakeLegalName(ByVal propertyMap As IPropertyMap, ByVal i As Long)

        Dim checkPropertyMap As IPropertyMap
        Dim testName As String = ""
        Dim finalName As String = ""
        Dim ok As Boolean

        Dim name As String = RemoveIllegalCharactersFromName(propertyMap.Name)

        While finalName = ""

            If i = 0 Then

                testName = name

            Else

                testName = name & i

            End If

            i += 1

            ok = True

            For Each checkPropertyMap In propertyMap.ClassMap.GetAllPropertyMaps

                If Not checkPropertyMap Is propertyMap Then

                    If LCase(checkPropertyMap.Name) = LCase(testName) Then

                        ok = False
                        Exit For

                    End If

                End If

            Next

            If ok Then

                finalName = testName

            End If

        End While

        propertyMap.UpdateName(finalName)

        'propertyMap.Name = finalName

    End Sub

    Public Function HasPrimaryKey(ByVal tableMap As ITableMap) As Boolean

        If tableMap.GetPrimaryKeyColumnMaps.Count > 0 Then Return True

    End Function

End Class
