Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class MapListItem
    Inherits ListViewItem

    Private m_Map As IMap

    Public Sub New()
        MyBase.New()


    End Sub

    Public Sub New(ByVal text As String)
        MyBase.New(text)


    End Sub

    Public Sub New(ByVal map As IMap)
        MyBase.New(map.Name)

        m_Map = map

    End Sub

    Public Overridable Sub SetRegularFont()

        Me.Font = New Font(Me.ListView.Font, FontStyle.Regular)

        'CType(Me.ListView, MapListView).SetNodeCheckedMode(Me)

        'CType(Me.ListView, MapListView).SetNodeExpandedMode(Me)

        'CType(Me.ListView, MapListView).SetNodeIcons(Me)

    End Sub

    Public Overridable Property Map() As IMap
        Get
            Return m_Map
        End Get
        Set(ByVal Value As IMap)
            m_Map = Value
        End Set
    End Property

    Public Overridable Sub Refresh()


    End Sub


    Public Overridable Function GetMap() As Object
        Return m_Map
    End Function
End Class
