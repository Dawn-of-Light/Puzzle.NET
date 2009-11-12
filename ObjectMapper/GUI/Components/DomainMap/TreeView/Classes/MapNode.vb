Imports Puzzle.NPersist.Framework.Mapping

Public Class MapNode
    Inherits System.Windows.Forms.TreeNode

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

        Me.NodeFont = New Font(Me.TreeView.Font, FontStyle.Regular)

        CType(Me.TreeView, MapTreeView).SetNodeCheckedMode(Me)

        CType(Me.TreeView, MapTreeView).SetNodeExpandedMode(Me)

        CType(Me.TreeView, MapTreeView).SetNodeIcons(Me)

    End Sub

    Public Overridable Property Map() As IMap
        Get
            Return m_Map
        End Get
        Set(ByVal Value As IMap)
            m_Map = Value
        End Set
    End Property

    Public Overridable Sub OnExpand()


    End Sub


    Public Overridable Sub Refresh(ByVal resetIcons As Boolean)


    End Sub


    Public Overridable Sub SetIcons()


    End Sub

    Public Overridable Sub ApplyErrorIcons(ByVal imgIndex As Integer, ByVal selImgIndex As Integer)

        If Not Me.TreeView Is Nothing Then

            Dim mapTreeView As mapTreeView = Me.TreeView

            If mapTreeView.HasMapNodeExceptions(Me, True) Then

                Dim offset As Integer = mapTreeView.ImageList.Images.Count / 2

                imgIndex += offset
                selImgIndex += offset

            End If

        End If

        Me.ImageIndex = imgIndex
        Me.SelectedImageIndex = selImgIndex

    End Sub

    Public Overridable Function GetMap() As Object
        Return m_Map
    End Function



End Class
