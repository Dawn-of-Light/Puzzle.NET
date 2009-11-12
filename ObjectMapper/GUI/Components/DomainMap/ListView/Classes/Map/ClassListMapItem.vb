Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping

Public Class ClassListMapItem
    Inherits MapListItem

    Private m_Map As IDomainMap

    Public Sub New(ByVal map As IDomainMap)
        MyBase.New(map)

        Me.Text = "Domain"

        m_Map = map

        Me.SubItems.Add("The classes, interfaces, enums and structs in this domain model")

        Me.ImageIndex = 2

    End Sub



End Class
