Imports Puzzle.NPersist.Framework.Mapping

Public Class TableMapNode
    Inherits MapNode

    Private m_Map As ITableMap

    Private m_IsMappingNode As Boolean

    Public Sub New()
        MyBase.New()

        SetIcons()

    End Sub


    Public Sub New(ByVal map As ITableMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub

    Public Sub New(ByVal map As ITableMap, ByVal isMappingNode As Boolean)
        MyBase.New(map)

        m_Map = map

        m_IsMappingNode = isMappingNode

        SetIcons()

    End Sub


    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 14
        Dim selImageIndex As Integer = 15

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

        'Clear the dummy child node
        Me.Nodes.Clear()

        If CType(Me.TreeView, MapTreeView).IsPreviewTree Then

            If m_IsMappingNode Then

                Exit Sub

            End If

        End If

        Dim columnMap As IColumnMap
        Dim newNode As ColumnMapNode
        Dim arrFound As New ArrayList

        Dim listSorted As ArrayList

        listSorted = m_Map.GetPrimaryKeyColumnMaps
        listSorted.Sort()

        For Each columnMap In listSorted

            If Not arrFound.Contains(columnMap) Then

                arrFound.Add(columnMap)

                newNode = New ColumnMapNode(columnMap)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

        listSorted = m_Map.ColumnMaps
        listSorted.Sort()

        For Each columnMap In listSorted

            If Not arrFound.Contains(columnMap) Then

                arrFound.Add(columnMap)

                newNode = New ColumnMapNode(columnMap)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

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

                If m_Map.ColumnMaps.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim columnMap As IColumnMap
        Dim newNode As ColumnMapNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        Dim listSorted As ArrayList

        listSorted = m_Map.GetPrimaryKeyColumnMaps
        listSorted.Sort()

        For Each columnMap In listSorted

            If Not hashFound.ContainsKey(LCase(columnMap.Name)) Then

                hashFound(LCase(columnMap.Name)) = True

                If Not hashOrg.ContainsKey(LCase(columnMap.Name)) Then

                    newNode = New ColumnMapNode(columnMap)

                    Me.Nodes.Add(newNode)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            End If

        Next

        listSorted = m_Map.ColumnMaps
        listSorted.Sort()

        For Each columnMap In listSorted

            If Not hashFound.ContainsKey(LCase(columnMap.Name)) Then

                hashFound(LCase(columnMap.Name)) = True

                If Not hashOrg.ContainsKey(LCase(columnMap.Name)) Then

                    newNode = New ColumnMapNode(columnMap)

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