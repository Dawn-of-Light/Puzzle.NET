Public Class XmlBehindSettings

    Private m_CompileXmlBehindTrigger As CompileXmlBehindTrigger = CompileXmlBehindTrigger.OnSaveShortcutOnly

    Public Property CompileXmlBehindTrigger() As CompileXmlBehindTrigger
        Get
            Return m_CompileXmlBehindTrigger
        End Get
        Set(ByVal Value As CompileXmlBehindTrigger)
            m_CompileXmlBehindTrigger = Value
        End Set
    End Property

End Class
