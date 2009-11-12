Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class PreviewFolderNode
    Inherits MapNode

    Private m_config As DomainConfig
    Private m_domainMap As IDomainMap

    Public Sub New()

    End Sub


    Public Sub New(ByVal config As DomainConfig, ByVal domainMap As IDomainMap)

        m_config = config
        m_domainMap = domainMap

    End Sub

    Public Property DomainMap() As IDomainMap
        Get
            Return m_domainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_domainMap = Value
        End Set
    End Property

    Public Property Config() As DomainConfig
        Get
            Return m_config
        End Get
        Set(ByVal Value As DomainConfig)
            m_config = Value
        End Set
    End Property


    Public Overloads Sub Refresh()

        Refresh(True)

    End Sub

    Public Overloads Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Me.Text = Config.ClassesToCodeConfig.TargetFolder

    End Sub


End Class
