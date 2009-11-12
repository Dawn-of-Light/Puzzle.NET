Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel

Public Class TableProperties
    Inherits PropertiesBase

    Private m_TableMap As ITableMap

    Public Event BeforePropertySet(ByVal mapObject As ITableMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As ITableMap, ByVal propertyName As String)

    Public Function GetTableMap() As ITableMap
        Return m_TableMap
    End Function

    Public Sub SetTableMap(ByVal value As ITableMap)
        m_TableMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_TableMap

    End Function

    <Category("Design"), _
    Description("The name of this table."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_TableMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_TableMap.Name Then
                m_TableMap.UpdateName(Value)
                RaiseEvent AfterPropertySet(m_TableMap, "Name")
            End If
        End Set
    End Property


End Class