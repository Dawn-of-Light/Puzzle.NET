Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class TablesToClassesConfigNode
    Inherits MapNode

    Private m_Map As TablesToClassesConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As TablesToClassesConfig)
        MyBase.New(map)

        m_Map = map

        Me.Text = "From Tables To Classes"

        Me.ImageIndex = 107
        Me.SelectedImageIndex = 107

    End Sub

    Public Sub New(ByVal map As TablesToClassesConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = "From Tables To Classes"

        Me.ImageIndex = 107
        Me.SelectedImageIndex = 107

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

