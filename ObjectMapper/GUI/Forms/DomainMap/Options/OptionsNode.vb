Public Class OptionsNode
    Inherits TreeNode

    Private m_OptionsPanel As Panel

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal text As String)
        MyBase.New(text)

    End Sub

    Public Sub New(ByVal text As String, ByVal imageIndex As Integer, ByVal selectedImageIndex As Integer)
        MyBase.New(text, imageIndex, selectedImageIndex)

    End Sub


    Public Sub New(ByVal text As String, ByVal imageIndex As Integer, ByVal selectedImageIndex As Integer, ByVal optionsPanel As Panel)
        MyBase.New(text, imageIndex, selectedImageIndex)

        m_OptionsPanel = optionsPanel

    End Sub

    Public Property OptionsPanel() As Panel
        Get
            Return m_OptionsPanel
        End Get
        Set(ByVal Value As Panel)
            m_OptionsPanel = Value
        End Set
    End Property

End Class
