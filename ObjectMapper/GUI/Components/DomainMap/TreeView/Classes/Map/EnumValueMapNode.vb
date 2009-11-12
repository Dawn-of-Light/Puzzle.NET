Imports Puzzle.NPersist.Framework.Mapping

Public Class EnumValueMapNode
    Inherits MapNode

    Private m_Map As IEnumValueMap

    Public Sub New()
        MyBase.New()

        SetIcons()

    End Sub

    Public Sub New(ByVal map As IEnumValueMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 117
        Dim selImgIndex As Integer = 117

        ApplyErrorIcons(imgIndex, selImgIndex)


    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

    End Sub

End Class