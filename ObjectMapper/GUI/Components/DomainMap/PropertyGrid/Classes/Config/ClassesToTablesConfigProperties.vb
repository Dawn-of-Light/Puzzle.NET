Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel

Public Class ClassesToTablesConfigProperties
    Inherits PropertiesBase

    Private m_ClassesToTablesConfig As ClassesToTablesConfig

    Public Event BeforePropertySet(ByVal mapObject As ClassesToTablesConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As ClassesToTablesConfig, ByVal propertyName As String)

    Public Function GetClassesToTablesConfig() As ClassesToTablesConfig
        Return m_ClassesToTablesConfig
    End Function

    Public Sub SetClassesToTablesConfig(ByVal value As ClassesToTablesConfig)
        m_ClassesToTablesConfig = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_ClassesToTablesConfig

    End Function


    <Category("Naming"), _
        Description("A string that will prefix the suggested table name for a table generated from a class."), _
        DisplayName("Table prefix"), _
        DefaultValue(True)> Public Property TablePrefix() As String
        Get
            Return m_ClassesToTablesConfig.TablePrefix
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassesToTablesConfig.TablePrefix Then
                m_ClassesToTablesConfig.TablePrefix = Value
                RaiseEvent AfterPropertySet(m_ClassesToTablesConfig, "TablePrefix")
            End If
        End Set
    End Property

    <Category("Naming"), _
        Description("A string that will suffix the suggested table name for a table generated from a class."), _
        DisplayName("Table suffix"), _
        DefaultValue(True)> Public Property TableSuffix() As String
        Get
            Return m_ClassesToTablesConfig.TableSuffix
        End Get
        Set(ByVal Value As String)
            If Value <> m_ClassesToTablesConfig.TableSuffix Then
                m_ClassesToTablesConfig.TableSuffix = Value
                RaiseEvent AfterPropertySet(m_ClassesToTablesConfig, "TableSuffix")
            End If
        End Set
    End Property


End Class
