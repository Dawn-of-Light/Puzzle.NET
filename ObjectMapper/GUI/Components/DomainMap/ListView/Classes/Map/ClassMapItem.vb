Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping

Public Class ClassMapItem
    Inherits MapListItem

    Private m_Map As IClassMap

    Public Sub New(ByVal map As IClassMap)
        MyBase.New(map)

        m_Map = map

        Me.Text = GetText()

        Me.ImageIndex = 6

        Me.SubItems.Add(GetNamespace)

    End Sub

    Private Function GetText() As String

        Dim arrName() As String = Split(m_Map.Name, ".")

        Return arrName(UBound(arrName))

    End Function


    Private Function GetNamespace() As String

        Dim arrName() As String = Split(m_Map.Name, ".")

        If UBound(arrName) > 0 Then

            ReDim Preserve arrName(UBound(arrName) - 1)

            Return Join(arrName, ".")

        Else

            Return ""

        End If

    End Function

    Public Overrides Sub Refresh()

        Me.Text = GetText()

        Me.SubItems(1).Text = GetNamespace()

    End Sub

End Class
