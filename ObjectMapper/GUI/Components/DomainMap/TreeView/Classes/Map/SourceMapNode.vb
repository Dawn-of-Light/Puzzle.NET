Imports Puzzle.NPersist.Framework.Mapping

Public Class SourceMapNode
    Inherits MapNode

    Private m_Map As ISourceMap

    Private m_IsMappingNode As Boolean


    Public Sub New()
        MyBase.New()

        SetIcons()

    End Sub


    Public Sub New(ByVal map As ISourceMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub


    Public Sub New(ByVal map As ISourceMap, ByVal isMappingNode As Boolean)
        MyBase.New(map)

        m_Map = map

        m_IsMappingNode = isMappingNode

        SetIcons()

    End Sub


    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 12
        Dim selImageIndex As Integer = 13

        IconManager.GetIconIndexesForMap(m_Map, imgIndex, selImageIndex)

        ApplyErrorIcons(imgIndex, selImageIndex)

    End Sub

    Public Property IsMappingNode() As Boolean
        Get
            Return m_IsMappingNode
        End Get
        Set(ByVal Value As Boolean)
            m_IsMappingNode = Value
        End Set
    End Property

    Public Overrides Sub OnExpand()

        'Me.TreeView.BeginUpdate()

        'Clear the dummy child node
        Me.Nodes.Clear()

        If CType(Me.TreeView, MapTreeView).IsPreviewTree Then

            If m_IsMappingNode Then

                'Me.TreeView.EndUpdate()

                Exit Sub

            End If

        End If

        Dim tableMap As ITableMap
        Dim newNode As TableMapNode

        Dim listSorted As ArrayList

        listSorted = m_Map.TableMaps
        listSorted.Sort()

        For Each tableMap In listSorted

            newNode = New TableMapNode(tableMap)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()

        Next

        'Me.TreeView.EndUpdate()

    End Sub


    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If

        If CType(Me.TreeView, MapTreeView).IsPreviewTree Then

            If m_IsMappingNode Then

                Exit Sub

            End If

        End If

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If m_Map.TableMaps.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim tableMap As ITableMap
        Dim newNode As TableMapNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        Dim listSorted As ArrayList

        listSorted = m_Map.TableMaps
        listSorted.Sort()

        For Each tableMap In listSorted

            If Len(tableMap.Name) > 0 Then

                hashFound(LCase(tableMap.Name)) = True

                If Not hashOrg.ContainsKey(LCase(tableMap.Name)) Then

                    newNode = New TableMapNode(tableMap)

                    'Add empty child node
                    newNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(newNode)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            End If

        Next

        For Each key In hashOrg.Keys

            If Not hashFound.ContainsKey(key) Then

                Me.Nodes.Remove(hashOrg(key))

            End If

        Next

    End Sub

End Class