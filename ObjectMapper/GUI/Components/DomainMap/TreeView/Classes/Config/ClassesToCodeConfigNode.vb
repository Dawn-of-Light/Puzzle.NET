Imports Puzzle.NPersist.Framework.Mapping

Public Class ClassesToCodeConfigNode
    Inherits MapNode

    Private m_Map As ProjectModel.ClassesToCodeConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As ProjectModel.ClassesToCodeConfig)
        MyBase.New(map)

        m_Map = map

        Me.Text = "From Model To Code"

        Me.ImageIndex = 108
        Me.SelectedImageIndex = 108

    End Sub

    Public Sub New(ByVal map As ProjectModel.ClassesToCodeConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = "Update Code From Model"

        Me.ImageIndex = 108
        Me.SelectedImageIndex = 108

    End Sub

    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim newNode As MapNode

        'Add the node for the list of classes
        newNode = New SourceCodeFileListNode

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()

    End Sub

End Class
