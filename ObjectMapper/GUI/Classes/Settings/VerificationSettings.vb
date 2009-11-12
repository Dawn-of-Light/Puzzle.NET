Public Class VerificationSettings

    Private m_AutoVerify As Boolean = True
    Private m_VerifyMappings As Boolean = True

    Private m_AutoSetTargetLanguages As Boolean = True

    Public Property AutoVerify() As Boolean
        Get
            Return m_AutoVerify
        End Get
        Set(ByVal Value As Boolean)
            m_AutoVerify = Value
        End Set
    End Property

    Public Property VerifyMappings() As Boolean
        Get
            Return m_VerifyMappings
        End Get
        Set(ByVal Value As Boolean)
            m_VerifyMappings = Value
        End Set
    End Property

    Public Property AutoSetTargetLanguages() As Boolean
        Get
            Return m_AutoSetTargetLanguages
        End Get
        Set(ByVal Value As Boolean)
            m_AutoSetTargetLanguages = Value
        End Set
    End Property


End Class
