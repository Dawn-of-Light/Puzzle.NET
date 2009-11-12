Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class SynchronizationSettings

    Private m_AutoAcceptPreview As Boolean = False

    Private m_DefaultTargetLanguage As TargetLanguageEnum = TargetLanguageEnum.VB
    Private m_DefaultTargetMapper As TargetMapperEnum = TargetMapperEnum.NPersist

    Public Property AutoAcceptPreview() As Boolean
        Get
            Return m_AutoAcceptPreview
        End Get
        Set(ByVal Value As Boolean)
            m_AutoAcceptPreview = Value
        End Set
    End Property

    Public Property DefaultTargetLanguage() As TargetLanguageEnum
        Get
            Return m_DefaultTargetLanguage
        End Get
        Set(ByVal Value As TargetLanguageEnum)
            m_DefaultTargetLanguage = Value
        End Set
    End Property

    Public Property DefaultTargetMapper() As TargetMapperEnum
        Get
            Return m_DefaultTargetMapper
        End Get
        Set(ByVal Value As TargetMapperEnum)
            m_DefaultTargetMapper = Value
        End Set
    End Property

End Class
