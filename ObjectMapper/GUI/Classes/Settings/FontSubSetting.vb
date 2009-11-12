Imports Puzzle.ObjectMapper.GUI.Uml

Public Class FontSubSetting

    Private m_name As String

    Private m_FontForeColor As UmlColor = New UmlColor(Color.Black)
    Private m_FontBackColor As UmlColor = New UmlColor(Color.White)
    Private m_FontStyle As FontStyle = FontStyle.Regular

    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property

    Public Property FontForeColor() As UmlColor
        Get
            Return m_FontForeColor
        End Get
        Set(ByVal Value As UmlColor)
            m_FontForeColor = Value
        End Set
    End Property

    Public Property FontBackColor() As UmlColor
        Get
            Return m_FontBackColor
        End Get
        Set(ByVal Value As UmlColor)
            m_FontBackColor = Value
        End Set
    End Property

    Public Property FontStyle() As FontStyle
        Get
            Return m_FontStyle
        End Get
        Set(ByVal Value As FontStyle)
            m_FontStyle = Value
        End Set
    End Property


End Class
