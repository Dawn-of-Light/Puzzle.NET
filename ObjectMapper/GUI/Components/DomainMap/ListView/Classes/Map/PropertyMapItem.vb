Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping

Public Class PropertyMapItem
    Inherits MapListItem

    Private m_Map As IPropertyMap
    Private m_IsInherited As Boolean

    Public Sub New(ByVal map As IPropertyMap)
        MyBase.New(map)

        m_Map = map


        Me.ImageIndex = 6

        Refresh()


    End Sub


    Public Sub New(ByVal map As IPropertyMap, ByVal isInherited As Boolean)
        MyBase.New(map)

        m_Map = map

        m_IsInherited = isInherited

        Refresh()


    End Sub


    Public Property IsInherited() As Boolean
        Get
            Return m_IsInherited
        End Get
        Set(ByVal Value As Boolean)
            m_IsInherited = Value
        End Set
    End Property

    Private Sub SelectIcon()

        If m_Map.IsIdentity Then

            If m_IsInherited Then

                Me.ImageIndex = 34

            Else

                Me.ImageIndex = 18

            End If

        Else

            If m_IsInherited Then

                Me.ImageIndex = 32

            Else

                Me.ImageIndex = 8

            End If

        End If
    End Sub

    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        If Me.SubItems.Count < 2 Then

            Me.SubItems.Add("")
            Me.SubItems.Add("")
            Me.SubItems.Add("")
            Me.SubItems.Add("")
            Me.SubItems.Add("")

        End If

        If m_Map.IsCollection Then

            Me.SubItems(1).Text = m_Map.ItemType

        Else

            Me.SubItems(1).Text = m_Map.DataType

        End If

        Me.SubItems(2).Text = GetFieldName()
        Me.SubItems(3).Text = GetMapsToColumns()
        Me.SubItems(4).Text = GetMapsToTable()

        If m_Map.LazyLoad Then
            Me.SubItems(5).Text = "Yes"
        Else
            Me.SubItems(5).Text = ""
        End If

        SelectIcon()

    End Sub

    Private Function GetFieldName() As String

        Return m_Map.GetFieldName()

    End Function

    Private Function GetMapsToColumns() As String

        Dim mapsTo As String
        Dim colName As String

        If Len(m_Map.Column) > 0 Then

            mapsTo += "[" & m_Map.Column & "], "

        End If

        For Each colName In m_Map.AdditionalColumns

            mapsTo += "[" & colName & "], "

        Next

        If Len(m_Map.IdColumn) > 0 Then

            mapsTo += "[" & m_Map.IdColumn & "], "

        End If

        For Each colName In m_Map.AdditionalIdColumns

            mapsTo += "[" & colName & "], "

        Next

        If Len(mapsTo) > 0 Then

            mapsTo = mapsTo.Substring(0, mapsTo.Length - 2)

        End If

        Return mapsTo

    End Function

    Private Function GetMapsToTable() As String

        Dim mapsTo As String
        Dim colName As String

        If Len(m_Map.Source) > 0 Then

            mapsTo += "[" & m_Map.Source & "]."

        End If

        If Len(m_Map.Table) > 0 Then

            mapsTo += "[" & m_Map.Table & "]"

        End If

        Return mapsTo

    End Function


End Class
