Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Class DomainMerger

    Public Shared Sub MergeDomains(ByVal targetDomainMap As IDomainMap, ByVal sourceDomainMap As IDomainMap)

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap

        Dim sourceMap As ISourceMap
        Dim tableMap As ITableMap
        Dim columnMap As IColumnMap

        Dim orgClassMap As IClassMap
        Dim orgPropertyMap As IPropertyMap

        Dim orgSourceMap As ISourceMap
        Dim orgTableMap As ITableMap
        Dim orgColumnMap As IColumnMap

        targetDomainMap.Dirty = True

        For Each classMap In sourceDomainMap.ClassMaps.Clone

            orgClassMap = targetDomainMap.GetClassMap(classMap.Name)

            If orgClassMap Is Nothing Then

                classMap.DomainMap = targetDomainMap
                orgClassMap = classMap

            Else

                classMap.Copy(orgClassMap)

            End If

            For Each propertyMap In classMap.PropertyMaps.Clone

                orgPropertyMap = orgClassMap.GetPropertyMap(propertyMap.Name)

                If orgPropertyMap Is Nothing Then

                    propertyMap.ClassMap = orgClassMap
                    orgPropertyMap = propertyMap

                Else

                    propertyMap.Copy(orgPropertyMap)

                End If

            Next

        Next


        For Each sourceMap In sourceDomainMap.SourceMaps.Clone

            orgSourceMap = targetDomainMap.GetSourceMap(sourceMap.Name)

            If orgSourceMap Is Nothing Then

                sourceMap.DomainMap = targetDomainMap
                orgSourceMap = sourceMap

            Else

                sourceMap.Copy(orgSourceMap)

            End If

            For Each tableMap In sourceMap.TableMaps.Clone

                orgTableMap = orgSourceMap.GetTableMap(tableMap.Name)

                If orgTableMap Is Nothing Then

                    tableMap.SourceMap = orgSourceMap
                    orgTableMap = tableMap

                Else

                    tableMap.Copy(orgTableMap)

                End If

                For Each columnMap In tableMap.ColumnMaps.Clone

                    orgColumnMap = orgTableMap.GetColumnMap(columnMap.Name)

                    If orgColumnMap Is Nothing Then

                        columnMap.TableMap = orgTableMap
                        orgColumnMap = columnMap

                    Else

                        columnMap.Copy(orgColumnMap)

                    End If

                Next

            Next

        Next

    End Sub

End Class
