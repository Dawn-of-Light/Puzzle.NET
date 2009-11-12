Imports Puzzle.NPersist.Framework.Mapping

Public Class SourceListMapNode
    Inherits MapNode

    Private m_Map As DomainMap

    Public Sub New(ByVal domainMap As IDomainMap)
        MyBase.New("Data Sources")

        m_Map = domainMap

        SetIcons()

    End Sub

    Public Sub New()
        MyBase.New("Data Sources")

        SetIcons()

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 10
        Dim selImageIndex As Integer = 11

        ApplyErrorIcons(imgIndex, selImageIndex)

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim sourceMap As ISourceMap
        Dim newNode As SourceMapNode

        Dim listSorted As ArrayList

        listSorted = CType(GetMap(), IDomainMap).SourceMaps
        listSorted.Sort()

        For Each sourceMap In listSorted

            newNode = New SourceMapNode(sourceMap)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()

        Next


    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If CType(GetMap(), IDomainMap).SourceMaps.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If


        Dim sourceMap As ISourceMap
        Dim newNode As SourceMapNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        Dim listSorted As ArrayList

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        listSorted = CType(GetMap(), IDomainMap).SourceMaps
        listSorted.Sort()

        For Each sourceMap In listSorted

            hashFound(LCase(sourceMap.Name)) = True

            If Not hashOrg.ContainsKey(LCase(sourceMap.Name)) Then

                newNode = New SourceMapNode(sourceMap)

                'Add empty child node
                newNode.Nodes.Add(New MapNode)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

        For Each key In hashOrg.Keys

            If Not hashFound.ContainsKey(key) Then

                Me.Nodes.Remove(hashOrg(key))

            End If

        Next

    End Sub


    Public Overrides Function GetMap() As Object
        If Not m_Map Is Nothing Then Return m_Map
        Return CType(Me.Parent, MapNode).Map
    End Function


End Class