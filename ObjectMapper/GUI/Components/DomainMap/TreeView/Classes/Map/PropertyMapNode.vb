Imports Puzzle.NPersist.Framework.Mapping

Public Class PropertyMapNode
    Inherits MapNode

    Private m_Map As IPropertyMap
    Private m_IsInherited As Boolean

    Public Sub New()
        MyBase.New()

        SetIcons()

    End Sub

    Public Sub New(ByVal map As IPropertyMap)
        MyBase.New(map)

        m_Map = map

        SetIcons()

    End Sub


    Public Sub New(ByVal map As IPropertyMap, ByVal isInherited As Boolean)
        MyBase.New(map)

        m_Map = map
        m_IsInherited = isInherited

        SetIcons()

    End Sub

    Public Property IsInherited() As Boolean
        Get
            Return m_IsInherited
        End Get
        Set(ByVal Value As Boolean)
            m_IsInherited = Value
        End Set
    End Property

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 8
        Dim selImgIndex As Integer = 9


        If m_Map.IsIdentity Then

            If m_IsInherited Then

                imgIndex = 34
                selImgIndex = 35

            Else

                imgIndex = 18
                selImgIndex = 19

            End If

        Else

            If m_IsInherited Then

                imgIndex = 32
                selImgIndex = 33

            Else

                imgIndex = 8
                selImgIndex = 9

            End If

        End If

        ApplyErrorIcons(imgIndex, selImgIndex)


    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim tableMap As ITableMap = m_Map.GetTableMap
        Dim tableNode As TableMapNode

        Dim columnMap As IColumnMap
        Dim columnNode As ColumnMapNode

        Dim sourceMap As ISourceMap = m_Map.GetSourceMap
        Dim sourceNode As SourceMapNode

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings

        If envSettings.ShowMappedNodes Then

            If Len(m_Map.Source) > 0 Then

                If Not sourceMap Is Nothing Then

                    sourceNode = New SourceMapNode(sourceMap, True)

                    'Add empty child node
                    sourceNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(sourceNode)

                    'Set regular font
                    sourceNode.SetRegularFont()

                End If

            End If

            If Len(m_Map.Table) > 0 Then

                If Not tableMap Is Nothing Then

                    If Not tableMap Is m_Map.ClassMap.GetTableMap Then

                        tableNode = New TableMapNode(tableMap, True)

                        'Add empty child node
                        tableNode.Nodes.Add(New MapNode)

                        Me.Nodes.Add(tableNode)

                        'Set regular font
                        tableNode.SetRegularFont()

                    End If

                End If


            End If


            If Len(m_Map.IdColumn) > 0 Then

                columnMap = m_Map.GetIdColumnMap

                If Not columnMap Is Nothing Then

                    columnNode = New ColumnMapNode(columnMap)

                    Me.Nodes.Add(columnNode)

                    'Set regular font
                    columnNode.SetRegularFont()

                End If

            End If


            For Each columnMap In m_Map.GetAdditionalIdColumnMaps

                columnNode = New ColumnMapNode(columnMap)

                Me.Nodes.Add(columnNode)

                'Set regular font
                columnNode.SetRegularFont()

            Next

            If Len(m_Map.Column) > 0 Then

                columnMap = m_Map.GetColumnMap

                If Not columnMap Is Nothing Then

                    columnNode = New ColumnMapNode(columnMap)

                    Me.Nodes.Add(columnNode)

                    'Set regular font
                    columnNode.SetRegularFont()

                End If

            End If

            For Each columnMap In m_Map.GetAdditionalColumnMaps

                columnNode = New ColumnMapNode(columnMap)

                Me.Nodes.Add(columnNode)

                'Set regular font
                columnNode.SetRegularFont()

            Next

        End If

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        Dim addDummy As Boolean

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If

        If Not Me.IsExpanded Then

            If envSettings.ShowMappedNodes Then

                If Me.FirstNode Is Nothing Then

                    If Not m_Map.GetColumnMap Is Nothing Then addDummy = True

                    If Not m_Map.GetIdColumnMap Is Nothing Then addDummy = True

                    If Len(m_Map.Table) > 0 Then
                        If Not m_Map.GetTableMap Is Nothing Then addDummy = True
                    End If

                    If Len(m_Map.Source) > 0 Then
                        If Not m_Map.GetSourceMap Is Nothing Then addDummy = True
                    End If

                    If addDummy Then

                        'Add empty child node
                        Me.Nodes.Add(New MapNode)

                    End If

                End If

                Exit Sub

            End If

        End If

        Dim childNode As MapNode

        Dim tableMap As ITableMap = m_Map.GetTableMap
        Dim tableNode As TableMapNode

        Dim sourceMap As ISourceMap = m_Map.GetSourceMap
        Dim sourceNode As SourceMapNode

        Dim columnMap As IColumnMap

        Dim foundNode As MapNode
        Dim noAdd As Boolean
        Dim addedNodes As New ArrayList
        Dim removeNodes As New ArrayList

        If envSettings.ShowMappedNodes Then

            For Each columnMap In m_Map.GetAdditionalColumnMaps

                RefreshColumn(columnMap, addedNodes)

            Next

            columnMap = m_Map.GetColumnMap
            RefreshColumn(columnMap, addedNodes)

            For Each columnMap In m_Map.GetAdditionalIdColumnMaps

                RefreshColumn(columnMap, addedNodes)

            Next

            columnMap = m_Map.GetIdColumnMap
            RefreshColumn(columnMap, addedNodes)

            For Each childNode In Nodes
                If childNode.GetType Is GetType(ColumnMapNode) Then
                    If Not addedNodes.Contains(childNode) Then
                        removeNodes.Add(childNode)
                    End If
                End If
            Next

            For Each childNode In removeNodes
                Me.Nodes.Remove(childNode)
            Next

            foundNode = Nothing
            noAdd = False
            For Each childNode In Nodes
                If childNode.GetType Is GetType(TableMapNode) Then
                    foundNode = childNode
                    Exit For
                End If
            Next

            If Len(m_Map.Table) > 0 Then

                If Not tableMap Is Nothing Then

                    If Not foundNode Is Nothing Then
                        If Not foundNode.Map Is tableMap Then
                            If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                        Else
                            noAdd = True
                            Me.Nodes.Remove(foundNode)
                            Me.Nodes.Insert(0, foundNode)
                        End If
                    End If

                    If Not noAdd Then

                        tableNode = New TableMapNode(tableMap, True)

                        'Add empty child node
                        tableNode.Nodes.Add(New MapNode)

                        Me.Nodes.Insert(0, tableNode)

                        'Set regular font
                        tableNode.SetRegularFont()

                    End If

                Else
                    If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                End If
            Else
                If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
            End If

            foundNode = Nothing
            noAdd = False
            For Each childNode In Nodes
                If childNode.GetType Is GetType(SourceMapNode) Then
                    foundNode = childNode
                    Exit For
                End If
            Next

            If Len(m_Map.Source) > 0 Then

                If Not sourceMap Is Nothing Then

                    If Not foundNode Is Nothing Then
                        If Not foundNode.Map Is sourceMap Then
                            If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                        Else
                            noAdd = True
                            Me.Nodes.Remove(foundNode)
                            Me.Nodes.Insert(0, foundNode)
                        End If
                    End If

                    If Not noAdd Then

                        sourceNode = New SourceMapNode(sourceMap, True)

                        'Add empty child node
                        sourceNode.Nodes.Add(New MapNode)

                        Me.Nodes.Insert(0, sourceNode)

                        'Set regular font
                        sourceNode.SetRegularFont()

                    End If

                Else
                    If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                End If
            Else
                If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
            End If

        Else

            Me.Nodes.Clear()

        End If

    End Sub

    Protected Sub RefreshColumn(ByVal columnMap As IColumnMap, ByRef addedNodes As ArrayList)

        Dim columnNode As ColumnMapNode
        Dim foundNode As MapNode
        Dim noAdd As Boolean
        Dim childNode As MapNode

        foundNode = Nothing
        noAdd = False
        For Each childNode In Nodes
            If childNode.GetType Is GetType(ColumnMapNode) Then
                If CType(childNode, ColumnMapNode).Map Is columnMap Then
                    foundNode = childNode
                    Exit For
                End If
            End If
        Next

        If Not columnMap Is Nothing Then

            If Not foundNode Is Nothing Then
                If Not foundNode.Map Is columnMap Then
                    If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                Else
                    noAdd = True
                    addedNodes.Add(foundNode)
                    Me.Nodes.Remove(foundNode)
                    Me.Nodes.Insert(0, foundNode)
                End If
            End If

            If Not noAdd Then

                columnNode = New ColumnMapNode(columnMap)

                Me.Nodes.Insert(0, columnNode)

                addedNodes.Add(columnNode)

                'Set regular font
                columnNode.SetRegularFont()

            End If

        Else
            If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
        End If

    End Sub

End Class