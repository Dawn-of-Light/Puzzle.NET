Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping


Public Class SourceMapItem
    Inherits MapListItem

    Private m_Map As ISourceMap

    Public Sub New(ByVal map As ISourceMap)
        MyBase.New(map)

        m_Map = map

        Me.ImageIndex = 12

        Me.SubItems.Add("")
        Me.SubItems.Add("")

        Me.Refresh()

    End Sub



    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        Me.SubItems(1).Text = m_Map.SourceType.ToString

        Me.SubItems(2).Text = m_Map.ProviderType.ToString

    End Sub

End Class
