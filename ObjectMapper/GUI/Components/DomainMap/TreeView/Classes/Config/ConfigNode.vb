Imports Puzzle.NPersist.Framework.Mapping

Public Class ConfigNode
    Inherits MapNode

    Private m_Map As ProjectModel.DomainConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As ProjectModel.DomainConfig)
        MyBase.New(map)

        m_Map = map

        Me.Text = m_Map.Name

    End Sub

    Public Sub New(ByVal map As ProjectModel.DomainConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = m_Map.Name

    End Sub

    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

    Public Sub SetNodeStyle()

        Me.ImageIndex = 50
        Me.SelectedImageIndex = 51

        If m_Map.IsActive Then
            Me.NodeFont = New Font(Me.TreeView.Font, FontStyle.Bold)
        Else
            Me.NodeFont = New Font(Me.TreeView.Font, FontStyle.Regular)
        End If

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim newNode As MapNode

        'Add the node for the classes to code config
        newNode = New ClassesToCodeConfigNode(m_Map.ClassesToCodeConfig)

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()


        'Add the node for the classes to tables config
        newNode = New ClassesToTablesConfigNode(m_Map.ClassesToTablesConfig)

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()


        'Add the node for the tables to classes  config
        newNode = New TablesToClassesConfigNode(m_Map.TablesToClassesConfig)

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Me.Text = m_Map.Name

        SetNodeStyle()

    End Sub

End Class
