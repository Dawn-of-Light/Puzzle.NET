<AttributeUsage(AttributeTargets.Class, Inherited:=True)> Public Class PluginClassAttribute
    Inherits Attribute

    Private m_PluginGroup As String
    Private m_DisplayName As String

    Public Sub New(ByVal pluginGroup As String)
        MyBase.New()

        m_PluginGroup = pluginGroup

    End Sub

    Public Sub New(ByVal pluginGroup As String, ByVal displayName As String)
        MyBase.New()

        m_PluginGroup = pluginGroup
        m_DisplayName = displayName

    End Sub

    Public Property PluginGroup() As String
        Get
            Return m_PluginGroup
        End Get
        Set(ByVal Value As String)
            m_PluginGroup = Value
        End Set
    End Property


    Public Property DisplayName() As String
        Get
            Return m_DisplayName
        End Get
        Set(ByVal Value As String)
            m_DisplayName = Value
        End Set
    End Property

End Class
