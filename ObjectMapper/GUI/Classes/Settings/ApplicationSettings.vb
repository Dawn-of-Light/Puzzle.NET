Imports System.Xml.Serialization
Imports System.IO

Public Class ApplicationSettings

    Private m_OptionSettings As New OptionSettings

    Private m_WinSize As Size = New Size(760, 578)
    Private m_WinLocation As Point = New Point(0, 0)
    Private m_WinState As FormWindowState = FormWindowState.Normal

    Private m_LastProjectPath As String = ""
    Private m_LastLoadDirPath As String = ""
    Private m_LastSaveDirPath As String = ""

    Private m_FileAssociationsOffered As Boolean = False

    Private m_LatestFilesMax As Integer = 4
    Private m_LatestFiles As New ArrayList


    Public Property OptionSettings() As OptionSettings
        Get
            Return m_OptionSettings
        End Get
        Set(ByVal Value As OptionSettings)
            m_OptionSettings = Value
        End Set
    End Property


    Public Property WinSize() As Size
        Get
            Return m_WinSize
        End Get
        Set(ByVal Value As Size)
            m_WinSize = Value
        End Set
    End Property

    Public Property WinLocation() As Point
        Get
            Return m_WinLocation
        End Get
        Set(ByVal Value As Point)
            m_WinLocation = Value
        End Set
    End Property

    Public Property WinState() As FormWindowState
        Get
            Return m_WinState
        End Get
        Set(ByVal Value As FormWindowState)
            m_WinState = Value
        End Set
    End Property


    Public Property LastProjectPath() As String
        Get
            Return m_LastProjectPath
        End Get
        Set(ByVal Value As String)
            m_LastProjectPath = Value
        End Set
    End Property


    Public Property LastLoadDirPath() As String
        Get
            Return m_LastLoadDirPath
        End Get
        Set(ByVal Value As String)
            m_LastLoadDirPath = Value
        End Set
    End Property

    Public Property LastSaveDirPath() As String
        Get
            Return m_LastSaveDirPath
        End Get
        Set(ByVal Value As String)
            m_LastSaveDirPath = Value
        End Set
    End Property

    Public Property FileAssociationsOffered() As Boolean
        Get
            Return m_FileAssociationsOffered
        End Get
        Set(ByVal Value As Boolean)
            m_FileAssociationsOffered = Value
        End Set
    End Property


    Public Property LatestFilesMax() As Integer
        Get
            Return m_LatestFilesMax
        End Get
        Set(ByVal Value As Integer)
            m_LatestFilesMax = Value
        End Set
    End Property


    <XmlArrayItem(GetType(String))> Public Property LatestFiles() As ArrayList
        Get
            Return m_LatestFiles
        End Get
        Set(ByVal Value As ArrayList)
            m_LatestFiles = Value
        End Set
    End Property

    Public Sub AddToLatestFiles(ByVal path As String)

        If Len(path) < 1 Then Exit Sub

        Dim test As String
        Dim i As Long

        m_LatestFiles.Remove(path)

        m_LatestFiles.Insert(0, path)

        If m_LatestFiles.Count > m_LatestFilesMax Then

            For i = m_LatestFiles.Count - 1 To m_LatestFilesMax Step -1

                m_LatestFiles.RemoveAt(i)

            Next

        End If

    End Sub

    Public Sub ResetAll()

        m_LatestFiles.Clear()

    End Sub

    Public Sub Setup()

        m_OptionSettings.EnvironmentSettings.SetupFontSettings()

    End Sub

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
