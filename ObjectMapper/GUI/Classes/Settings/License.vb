Imports System.Xml.Serialization
Imports System.IO

Public Class License

    Private m_Email As String = ""
    Private m_Serial As String = ""

    Public Property Email() As String
        Get
            Return m_Email
        End Get
        Set(ByVal Value As String)
            m_Email = Value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return m_Serial
        End Get
        Set(ByVal Value As String)
            m_Serial = Value
        End Set
    End Property

    Public Sub Save(ByVal path As String)

        Try

            Dim mySerializer As XmlSerializer = New XmlSerializer(Me.GetType)
            ' To write to a file, create a StreamWriter object.
            Dim myWriter As StreamWriter = New StreamWriter(path)
            mySerializer.Serialize(myWriter, Me)
            myWriter.Close()

        Catch ex As Exception

        End Try

    End Sub

End Class
