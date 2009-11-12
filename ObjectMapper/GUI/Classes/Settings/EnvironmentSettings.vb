Imports System.Xml.Serialization
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class EnvironmentSettings

    Private m_ShowStatusBar As Boolean = True
    Private m_ShowToolBar As Boolean = True

    Private m_ShowExplorerPanel As Boolean = True
    Private m_ShowPropertiesPanel As Boolean = True
    Private m_ShowPropAndDocPanel As Boolean = True
    Private m_ShowToolsPanel As Boolean = True
    Private m_ShowListPanel As Boolean = True

    Private m_PanelExplorerDock As DockStyle = DockStyle.Left
    Private m_PanelToolsDock As DockStyle = DockStyle.Right
    Private m_PanelListDock As DockStyle = DockStyle.Bottom

    Private m_ShowNamespaceNodes As Boolean = True
    Private m_ShowMappedNodes As Boolean = True
    Private m_ShowInheritedNodes As Boolean = True

    Private m_HidePropGridCategories As New ArrayList

    Private m_AutoOpenLastProjectOnStartup As Boolean = True
    Private m_AlphabeticPropertyGrid As Boolean = False

    Private m_FontSettings As New ArrayList

    Public Property ShowStatusBar() As Boolean
        Get
            Return m_ShowStatusBar
        End Get
        Set(ByVal Value As Boolean)
            m_ShowStatusBar = Value
        End Set
    End Property

    Public Property ShowToolBar() As Boolean
        Get
            Return m_ShowToolBar
        End Get
        Set(ByVal Value As Boolean)
            m_ShowToolBar = Value
        End Set
    End Property


    Public Property ShowExplorerPanel() As Boolean
        Get
            Return m_ShowExplorerPanel
        End Get
        Set(ByVal Value As Boolean)
            m_ShowExplorerPanel = Value
        End Set
    End Property


    Public Property ShowPropAndDocPanel() As Boolean
        Get
            Return m_ShowPropAndDocPanel
        End Get
        Set(ByVal Value As Boolean)
            m_ShowPropAndDocPanel = Value
        End Set
    End Property

    Public Property ShowPropertiesPanel() As Boolean
        Get
            Return m_ShowPropertiesPanel
        End Get
        Set(ByVal Value As Boolean)
            m_ShowPropertiesPanel = Value
        End Set
    End Property

    Public Property ShowToolsPanel() As Boolean
        Get
            Return m_ShowToolsPanel
        End Get
        Set(ByVal Value As Boolean)
            m_ShowToolsPanel = Value
        End Set
    End Property


    Public Property ShowListPanel() As Boolean
        Get
            Return m_ShowListPanel
        End Get
        Set(ByVal Value As Boolean)
            m_ShowListPanel = Value
        End Set
    End Property


    Public Property PanelExplorerDock() As DockStyle
        Get
            Return m_PanelExplorerDock
        End Get
        Set(ByVal Value As DockStyle)
            m_PanelExplorerDock = Value
        End Set
    End Property

    Public Property PanelToolsDock() As DockStyle
        Get
            Return m_PanelToolsDock
        End Get
        Set(ByVal Value As DockStyle)
            m_PanelToolsDock = Value
        End Set
    End Property

    Public Property PanelListDock() As DockStyle
        Get
            Return m_PanelListDock
        End Get
        Set(ByVal Value As DockStyle)
            m_PanelListDock = Value
        End Set
    End Property

    Public Property ShowNamespaceNodes() As Boolean
        Get
            Return m_ShowNamespaceNodes
        End Get
        Set(ByVal Value As Boolean)
            m_ShowNamespaceNodes = Value
        End Set
    End Property

    Public Property ShowMappedNodes() As Boolean
        Get
            Return m_ShowMappedNodes
        End Get
        Set(ByVal Value As Boolean)
            m_ShowMappedNodes = Value
        End Set
    End Property

    Public Property ShowInheritedNodes() As Boolean
        Get
            Return m_ShowInheritedNodes
        End Get
        Set(ByVal Value As Boolean)
            m_ShowInheritedNodes = Value
        End Set
    End Property

    Public Property AlphabeticPropertyGrid() As Boolean
        Get
            Return m_AlphabeticPropertyGrid
        End Get
        Set(ByVal Value As Boolean)
            m_AlphabeticPropertyGrid = Value
        End Set
    End Property

    Public Property AutoOpenLastProjectOnStartup() As Boolean
        Get
            Return m_AutoOpenLastProjectOnStartup
        End Get
        Set(ByVal Value As Boolean)
            m_AutoOpenLastProjectOnStartup = Value
        End Set
    End Property


    <XmlArrayItem(GetType(String))> Public Property HidePropGridCategories() As ArrayList
        Get
            Return m_HidePropGridCategories
        End Get
        Set(ByVal Value As ArrayList)
            m_HidePropGridCategories = Value
        End Set
    End Property

    <XmlArrayItem(GetType(FontSetting))> Public Property FontSettings() As ArrayList
        Get
            Return m_FontSettings
        End Get
        Set(ByVal Value As ArrayList)
            m_FontSettings = Value
        End Set
    End Property

    Public Sub SetupFontSettings()

        Dim fs As FontSetting
        Dim fss As FontSubSetting

        If Not hasFontSetting("Text Editor") Then

            fs = GetFontSetting("Text Editor")

            fs.FontFamily = "Courier New"
            fs.FontSize = 10.0F

            If Not fs.HasFontSubSetting("Text") Then

                fss = fs.GetFontSubSetting("Text")

                fss.FontBackColor.FromColor(Color.Black)
                fss.FontForeColor.FromColor(Color.White)

            End If

        End If

    End Sub

    Public Function hasFontSetting(ByVal name As String) As Boolean

        Dim lname As String = LCase(name)

        Dim setting As FontSetting

        For Each setting In m_FontSettings

            If LCase(setting.Name) = lname Then

                Return True

            End If

        Next

    End Function

    Public Function GetFontSetting(ByVal name As String) As FontSetting

        Dim lname As String = LCase(name)

        Dim setting As FontSetting

        For Each setting In m_FontSettings

            If LCase(setting.Name) = lname Then

                Return setting

            End If

        Next

        setting = New FontSetting

        setting.Name = name

        Return setting

    End Function


End Class
