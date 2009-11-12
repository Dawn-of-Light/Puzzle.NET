Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class UmlDiagramNode
    Inherits MapNode

    Private m_Map As UmlDiagram

    Public Sub New(ByVal map As UmlDiagram)
        MyBase.New(map)

        m_Map = map

        Me.Text = m_Map.Name

        Me.ImageIndex = 78
        Me.SelectedImageIndex = 79

    End Sub

    Public Sub SetNodeStyle()


    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim newNode As MapNode

        ''Add the node for the list of classes
        'newNode = New ClassesToCodeConfigNode(m_Map.ClassesToCodeConfig)

        ''Add empty child node
        'newNode.Nodes.Add(New MapNode)

        'Me.Nodes.Add(newNode)

        ''Set regular font
        'newNode.SetRegularFont()


    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Me.Text = m_Map.Name

        Me.ImageIndex = 78
        Me.SelectedImageIndex = 79

    End Sub

End Class


