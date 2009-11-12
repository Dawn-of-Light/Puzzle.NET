Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping

Public Class SourceListMapItem
    Inherits MapListItem

    Private m_Map As IDomainMap

    Public Sub New(ByVal map As IDomainMap)
        MyBase.New(map)

        Me.Text = "Data Sources"

        m_Map = map

        Me.SubItems.Add("The data sources mapped to by this domain model")

        Me.ImageIndex = 10

    End Sub


End Class
