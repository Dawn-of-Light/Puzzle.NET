Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class ClassesToTablesConfigNode
    Inherits MapNode

    Private m_Map As ClassesToTablesConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As ClassesToTablesConfig)
        MyBase.New(map)

        m_Map = map

        Me.Text = "From Classes To Tables"

        Me.ImageIndex = 106
        Me.SelectedImageIndex = 106

    End Sub

    Public Sub New(ByVal map As ClassesToTablesConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = "From Classes To Tables"

        Me.ImageIndex = 106
        Me.SelectedImageIndex = 106

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



    End Sub



End Class
