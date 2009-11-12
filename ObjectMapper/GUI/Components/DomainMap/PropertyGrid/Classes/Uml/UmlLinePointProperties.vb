Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class UmlLinePointProperties
    Inherits PropertiesBase

    Private m_UmlLinePoint As UmlLinePoint

    Public Event BeforePropertySet(ByVal mapObject As UmlLinePoint, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As UmlLinePoint, ByVal propertyName As String)

    Public Function GetUmlLinePoint() As UmlLinePoint
        Return m_UmlLinePoint
    End Function

    Public Sub SetUmlLinePoint(ByVal value As UmlLinePoint)
        m_UmlLinePoint = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_UmlLinePoint

    End Function

    <Category("Layout"), _
    Description("The location of this point on the diagram."), _
    DisplayName("Location"), _
    DefaultValue("")> Public Property Location() As Point
        Get
            Return New Point(m_UmlLinePoint.X, m_UmlLinePoint.Y)
        End Get
        Set(ByVal Value As Point)
            m_UmlLinePoint.X = Value.X
            m_UmlLinePoint.Y = Value.Y
            RaiseEvent AfterPropertySet(m_UmlLinePoint, "Location")
        End Set
    End Property


End Class

