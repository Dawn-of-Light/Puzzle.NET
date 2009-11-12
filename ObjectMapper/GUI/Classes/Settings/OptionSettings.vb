Public Class OptionSettings

    Private m_EnvironmentSettings As New EnvironmentSettings()
    Private m_VerificationSettings As New VerificationSettings()
    Private m_SynchronizationSettings As New SynchronizationSettings()
    Private m_XmlBehindSettings As New XmlBehindSettings

    Public Property EnvironmentSettings() As EnvironmentSettings
        Get
            Return m_EnvironmentSettings
        End Get
        Set(ByVal Value As EnvironmentSettings)
            m_EnvironmentSettings = Value
        End Set
    End Property

    Public Property VerificationSettings() As VerificationSettings
        Get
            Return m_VerificationSettings
        End Get
        Set(ByVal Value As VerificationSettings)
            m_VerificationSettings = Value
        End Set
    End Property

    Public Property SynchronizationSettings() As SynchronizationSettings
        Get
            Return m_SynchronizationSettings
        End Get
        Set(ByVal Value As SynchronizationSettings)
            m_SynchronizationSettings = Value
        End Set
    End Property

    Public Property XmlBehindSettings() As XmlBehindSettings
        Get
            Return m_XmlBehindSettings
        End Get
        Set(ByVal Value As XmlBehindSettings)
            m_XmlBehindSettings = Value
        End Set
    End Property

End Class
