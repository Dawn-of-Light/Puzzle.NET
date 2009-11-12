<AttributeUsage(AttributeTargets.Method, Inherited:=True)> Public Class ConverterMethodAttribute
    Inherits Attribute


    Private m_DisplayName As String

    Public Sub New()

        m_DisplayName = Me.GetType.Name

    End Sub

    Public Sub New(ByVal displayName As String)

        m_DisplayName = displayName

    End Sub

    Public Property DisplayName() As String
        Get
            Return m_DisplayName
        End Get
        Set(ByVal Value As String)
            m_DisplayName = Value
        End Set
    End Property


End Class
