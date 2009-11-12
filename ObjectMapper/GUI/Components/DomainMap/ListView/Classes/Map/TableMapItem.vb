Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping


Public Class TableMapItem
    Inherits MapListItem

    Private m_Map As ITableMap

    Public Sub New(ByVal map As ITableMap)
        MyBase.New(map)

        m_Map = map

        Me.ImageIndex = 14

        'Me.SubItems.Add("")

        Me.Refresh()

    End Sub



    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        'Me.SubItems(1).Text = m_Map.SourceType.ToString


    End Sub

End Class

