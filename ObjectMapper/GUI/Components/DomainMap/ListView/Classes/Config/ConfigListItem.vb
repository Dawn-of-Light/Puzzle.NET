Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping

Public Class ConfigListItem
    Inherits MapListItem

    Private m_Map As IDomainMap

    Public Sub New(ByVal map As IDomainMap)
        MyBase.New(map)

        Me.Text = "Synch Configs"

        m_Map = map


        Me.SubItems.Add("The tool configurations available for this domain model")

        Me.ImageIndex = 37

    End Sub

End Class
