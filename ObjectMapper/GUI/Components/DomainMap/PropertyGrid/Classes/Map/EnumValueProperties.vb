Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel

Public Class EnumValueProperties
    Inherits PropertiesBase

    Private m_EnumValueMap As IEnumValueMap

    Public Event BeforePropertySet(ByVal mapObject As IEnumValueMap, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IEnumValueMap, ByVal propertyName As String)

    Public Function GetEnumValueMap() As IEnumValueMap
        Return m_EnumValueMap
    End Function

    Public Sub SetEnumValueMap(ByVal value As IEnumValueMap)
        m_EnumValueMap = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_EnumValueMap

    End Function

    <Category("Design"), _
    Description("The name of this enumeration value."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_EnumValueMap.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_EnumValueMap.Name Then
                m_EnumValueMap.Name = Value
                RaiseEvent AfterPropertySet(m_EnumValueMap, "Name")
            End If
        End Set
    End Property

    <Category("Design"), _
    Description("The index of this enumeration value."), _
    DisplayName("Name"), _
    DefaultValue(0)> Public Property Index() As Integer
        Get
            Return m_EnumValueMap.Index
        End Get
        Set(ByVal Value As Integer)
            If Value <> m_EnumValueMap.Index Then
                m_EnumValueMap.Index = Value
                RaiseEvent AfterPropertySet(m_EnumValueMap, "Index")
            End If
        End Set
    End Property


End Class