Imports Puzzle.NPersist.Framework.Mapping

Public Class DomainMapNode
    Inherits MapNode

    Private m_Map As IDomainMap

    Public Sub New(ByVal map As IDomainMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub


    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 0
        Dim selImgIndex As Integer = 1

        IconManager.GetIconIndexesForMap(m_Map, imgIndex, selImgIndex)

        ApplyErrorIcons(imgIndex, selImgIndex)


    End Sub


    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim newNode As MapNode

        'Add the node for the list of classes
        newNode = New ClassListMapNode

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()


        'Add the node for the list of data sources
        newNode = New SourceListMapNode

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()

        If Not Me.Parent Is Nothing Then

            'Add the node for the list of uml diagrams
            newNode = New UmlDiagramListNode

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()


            'Add the node for the list of configurations
            newNode = New ConfigListNode

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()


        End If

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If


    End Sub


End Class
