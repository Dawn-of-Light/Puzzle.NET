Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel


Public Class ClassesToCodeConfigItem
    Inherits MapListItem

    Private m_Map As ClassesToCodeConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As ClassesToCodeConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = "Update Code From Model"


        Me.ImageIndex = 37

        'Me.SubItems.Add("")


    End Sub


    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property



    Public Overrides Sub Refresh()





    End Sub


End Class

