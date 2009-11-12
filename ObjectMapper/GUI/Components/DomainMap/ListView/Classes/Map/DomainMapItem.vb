Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class DomainMapItem
    Inherits MapListItem


    Private m_Map As IDomainMap

    Public Sub New(ByVal map As IDomainMap)
        MyBase.New(map)

        m_Map = map

        Me.SubItems.Add(m_Map.Source)
        Me.SubItems.Add(m_Map.RootNamespace)

        Me.ImageIndex = 0

    End Sub

    Public Overrides Sub Refresh()

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        Me.SubItems(1).Text = m_Map.Source
        Me.SubItems(2).Text = m_Map.RootNamespace

        Me.ImageIndex = 0


    End Sub


End Class
