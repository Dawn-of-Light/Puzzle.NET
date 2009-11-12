Imports Puzzle.NPersist.Framework.Mapping

Public Class ColumnMapNode
    Inherits MapNode

    Private m_Map As IColumnMap

    Public Sub New()
        MyBase.New()

        SetIcons()

    End Sub

    Public Sub New(ByVal map As IColumnMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 16
        Dim selImgIndex As Integer = 17

        IconManager.GetIconIndexesForMap(m_Map, imgIndex, selImgIndex)

        ApplyErrorIcons(imgIndex, selImgIndex)

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()


    End Sub


    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If

        If Not Me.IsExpanded Then Exit Sub

    End Sub

End Class