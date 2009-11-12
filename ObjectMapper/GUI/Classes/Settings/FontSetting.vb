Imports System.Xml.Serialization

Public Class FontSetting

    Private m_name As String

    'Private m_FontSize As Single = 14.0F
    'Private m_FontFamily As String = "Lucida Console"
    Private m_FontSize As Single = 10.0F
    Private m_FontFamily As String = "Courier New"

    Private m_FontSubSettings As New ArrayList

    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal Value As String)
            m_name = Value
        End Set
    End Property

    Public Property FontFamily() As String
        Get
            Return m_fontFamily
        End Get
        Set(ByVal Value As String)
            m_fontFamily = Value
        End Set
    End Property

    Public Property FontSize() As Single
        Get
            Return m_FontSize
        End Get
        Set(ByVal Value As Single)
            m_FontSize = Value
        End Set
    End Property

    <XmlArrayItem(GetType(FontSubSetting))> Public Property FontSubSettings() As ArrayList
        Get
            Return m_FontSubSettings
        End Get
        Set(ByVal Value As ArrayList)
            m_FontSubSettings = Value
        End Set
    End Property

    Public Function HasFontSubSetting(ByVal name As String) As Boolean

        Dim lname As String = LCase(name)

        Dim subSetting As FontSubSetting

        For Each subSetting In m_FontSubSettings

            If LCase(subSetting.Name) = lname Then

                Return True

            End If

        Next

    End Function

    Public Function GetFontSubSetting(ByVal name As String) As FontSubSetting

        Dim lname As String = LCase(name)

        Dim subSetting As FontSubSetting

        For Each subSetting In m_FontSubSettings

            If LCase(subSetting.Name) = lname Then

                Return subSetting

            End If

        Next

        subSetting = New FontSubSetting

        subSetting.Name = name

        Return subSetting

    End Function

End Class
