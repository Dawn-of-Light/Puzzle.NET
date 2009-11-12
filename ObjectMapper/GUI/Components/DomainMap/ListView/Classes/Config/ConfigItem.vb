Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel


Public Class ConfigItem
    Inherits MapListItem

    Private m_Map As DomainConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As DomainConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap


        Me.Text = m_Map.Name

        Me.SubItems.Add("")


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

        Me.Text = m_Map.Name

        Me.ImageIndex = 50

        If m_Map.IsActive Then
            Me.SubItems(1).Text = "Yes"
        Else
            Me.SubItems(1).Text = ""
        End If

        SetNodeStyle()

    End Sub


    Public Sub SetNodeStyle()

        If m_Map.IsActive Then
            Me.Font = New Font(Me.ListView.Font, FontStyle.Bold)
        Else
            Me.Font = New Font(Me.ListView.Font, FontStyle.Regular)
        End If

    End Sub

End Class
