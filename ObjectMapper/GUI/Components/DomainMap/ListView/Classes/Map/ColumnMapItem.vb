Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping


Public Class ColumnMapItem
    Inherits MapListItem

    Private m_Map As IColumnMap

    Public Sub New(ByVal map As IColumnMap)
        MyBase.New(map)

        m_Map = map

        Me.SubItems.Add("")
        Me.SubItems.Add("")
        Me.SubItems.Add("")
        Me.SubItems.Add("")
        Me.SubItems.Add("")
        Me.SubItems.Add("")

        Me.Refresh()

    End Sub



    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        SelectIcon()

        Me.SubItems(1).Text = m_Map.DataType.ToString
        Me.SubItems(2).Text = m_Map.Precision
        If m_Map.AllowNulls Then Me.SubItems(3).Text = "Yes" Else Me.SubItems(3).Text = ""
        If m_Map.IsPrimaryKey Then Me.SubItems(4).Text = "Yes" Else Me.SubItems(4).Text = ""
        If m_Map.IsForeignKey Then
            Me.SubItems(5).Text = "[" & m_Map.PrimaryKeyTable & "].[" & m_Map.PrimaryKeyColumn & "]"
        Else
            Me.SubItems(5).Text = ""
        End If
        Me.SubItems(6).Text = m_Map.DefaultValue

    End Sub


    Private Sub SelectIcon()

        If m_Map.IsPrimaryKey Then

            If m_Map.IsForeignKey Then

                Me.ImageIndex = 44

            Else

                Me.ImageIndex = 40

            End If

        ElseIf m_Map.IsForeignKey Then

            Me.ImageIndex = 42

        Else

            Me.ImageIndex = 16

        End If

    End Sub

End Class

