Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.ComponentModel

Public Class SourceProperties
    Inherits PropertiesBase

    Private m_SourceMap As ISourceMap

    Public Event BeforePropertySet(ByVal mapObject As ISourceMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As ISourceMap, ByVal propertyName As String)

    Public Function GetSourceMap() As ISourceMap
        Return m_SourceMap
    End Function

    Public Sub SetSourceMap(ByVal value As ISourceMap)
        m_SourceMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_SourceMap

    End Function

    <Category("Design"), _
    Description("The name of this data source."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_SourceMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_SourceMap.Name Then
                m_SourceMap.UpdateName(Value)
                RaiseEvent AfterPropertySet(m_SourceMap, "Name")
            End If
        End Set
    End Property

    '<Category("Connection"), _
    '    Description("The connection string to this data source."), _
    '    DisplayName("Connection string"), _
    '    DefaultValue("")> Public Property ConnectionString() As String
    '    Get
    '        Return m_SourceMap.ConnectionString
    '    End Get
    '    Set(ByVal Value As String)
    '        If Value <> m_SourceMap.ConnectionString Then
    '            m_SourceMap.ConnectionString = Value
    '            RaiseEvent AfterPropertySet(m_SourceMap, "ConnectionString")
    '        End If
    '    End Set
    'End Property


    <Category("Connection"), _
        Description("The connection string to this data source."), _
        DisplayName("Connection string"), _
        DefaultValue("")> Public Property ConnectionString() As String
        Get
            Return m_SourceMap.ConnectionString
        End Get
        Set(ByVal Value As String)
            If Value <> m_SourceMap.ConnectionString Then
                m_SourceMap.ConnectionString = Value
                RaiseEvent AfterPropertySet(m_SourceMap, "ConnectionString")
            End If
        End Set
    End Property

    <Category("Mapping"), _
        Description("The type of the provider to be used for communication with this data source."), _
        DisplayName("Provider type"), _
        DefaultValue(ProviderType.SqlClient)> Public Property Provider() As ProviderType
        Get
            Return m_SourceMap.ProviderType
        End Get
        Set(ByVal Value As ProviderType)
            m_SourceMap.ProviderType = Value
            RaiseEvent AfterPropertySet(m_SourceMap, "ProviderType")
        End Set
    End Property

    <Category("Mapping"), _
        Description("The type of the data source."), _
        DisplayName("Data source type"), _
        DefaultValue(SourceType.MSSqlServer)> Public Property Source() As SourceType
        Get
            Return m_SourceMap.SourceType
        End Get
        Set(ByVal Value As SourceType)
            m_SourceMap.SourceType = Value
            RaiseEvent AfterPropertySet(m_SourceMap, "SourceType")
        End Set
    End Property

    <Category("Mapping"), _
        Description("The name of the schema for this data source."), _
        DisplayName("Schema"), _
        DefaultValue("")> Public Property Schema() As String
        Get
            Return m_SourceMap.Schema
        End Get
        Set(ByVal Value As String)
            m_SourceMap.Schema = Value
            RaiseEvent AfterPropertySet(m_SourceMap, "Schema")
        End Set
    End Property


    <Category("Mapping"), _
        Description("The name of the catalog for this data source."), _
        DisplayName("Catalog"), _
        DefaultValue("")> Public Property Catalog() As String
        Get
            Return m_SourceMap.Catalog
        End Get
        Set(ByVal Value As String)
            m_SourceMap.Catalog = Value
            RaiseEvent AfterPropertySet(m_SourceMap, "Catalog")
        End Set
    End Property

End Class