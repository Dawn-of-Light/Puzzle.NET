Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping


Public Class MapExceptionItem
    Inherits MapListItem

    Private m_Map As IMap
    Private m_MappingException As MappingException

    Public Sub New(ByVal mapException As MappingException)
        MyBase.New(mapException.MapObject)

        m_Map = mapException.MapObject
        m_MappingException = mapException

        'Me.SubItems.Add("")

        Me.Refresh()

    End Sub



    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        'Me.SubItems(1).Text = m_Map.SourceType.ToString


    End Sub

End Class
