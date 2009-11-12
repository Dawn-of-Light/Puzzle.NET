Imports Puzzle.NPersist.Framework.Mapping

Public Class ClassMapNode
    Inherits MapNode

    Private m_Map As IClassMap

    Public Sub New(ByVal map As IClassMap)
        MyBase.New(map)

        m_Map = map

        Dim arrName() As String = Split(map.Name, ".")

        Me.Text = arrName(UBound(arrName))

        SetIcons()

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 6
        Dim selImgIndex As Integer = 7

        IconManager.GetIconIndexesForMap(m_Map, imgIndex, selImgIndex)

        ApplyErrorIcons(imgIndex, selImgIndex)

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        If Me.m_Map.ClassType = NPersist.Framework.Enumerations.ClassType.Enum Then

            OnExpandEnum()
            Return

        End If

        Dim propertyMap As IPropertyMap
        Dim newNode As PropertyMapNode

        Dim tableMap As ITableMap = m_Map.GetTableMap
        Dim tableNode As TableMapNode

        Dim sourceMap As ISourceMap = m_Map.GetSourceMap
        Dim sourceNode As SourceMapNode

        Dim columnMap As IColumnMap = m_Map.GetTypeColumnMap
        Dim columnNode As ColumnMapNode

        Dim listSorted As ArrayList
        Dim arrAdded As New ArrayList

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

            If Not tableMap Is Nothing Then

                tableNode = New TableMapNode(tableMap, True)

                'Add empty child node
                tableNode.Nodes.Add(New MapNode)

                Me.Nodes.Add(tableNode)

                'Set regular font
                tableNode.SetRegularFont()

            End If

            If Not columnMap Is Nothing Then

                columnNode = New ColumnMapNode(columnMap)

                Me.Nodes.Add(columnNode)

                'Set regular font
                columnNode.SetRegularFont()

            End If

        End If

        listSorted = m_Map.GetNonInheritedIdentityPropertyMaps
        'listSorted.Sort()
        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newNode = New PropertyMapNode(propertyMap)

                'Add empty child node
                newNode.Nodes.Add(New MapNode)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

        If envSettings.ShowInheritedNodes Then

            listSorted = m_Map.GetInheritedIdentityPropertyMaps
            'listSorted.Sort()

            For Each propertyMap In listSorted

                If Not arrAdded.Contains(propertyMap) Then

                    arrAdded.Add(propertyMap)

                    newNode = New PropertyMapNode(propertyMap, True)

                    'Add empty child node
                    newNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(newNode)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            Next

        End If

        listSorted = m_Map.GetNonInheritedPropertyMaps
        listSorted.Sort()
        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newNode = New PropertyMapNode(propertyMap)

                'Add empty child node
                newNode.Nodes.Add(New MapNode)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

        If envSettings.ShowInheritedNodes Then

            listSorted = m_Map.GetInheritedPropertyMaps
            listSorted.Sort()

            For Each propertyMap In listSorted

                If Not arrAdded.Contains(propertyMap) Then

                    arrAdded.Add(propertyMap)

                    newNode = New PropertyMapNode(propertyMap, True)

                    'Add empty child node
                    newNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(newNode)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            Next

        End If

    End Sub


    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Dim arrName() As String = Split(Map.Name, ".")

        Me.Text = arrName(UBound(arrName))

        SetIcons()

        If Me.m_Map.ClassType = NPersist.Framework.Enumerations.ClassType.Enum Then

            RefreshEnum()
            Return

        End If

        Dim addDummy As Boolean

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings


        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If m_Map.GetAllPropertyMaps.Count > 0 Then addDummy = True

                If Not m_Map.GetTableMap Is Nothing Then addDummy = True

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

        Dim propertyMap As IPropertyMap
        Dim newNode As PropertyMapNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String
        Dim arrFound As New ArrayList

        Dim tableMap As ITableMap = m_Map.GetTableMap
        Dim tableNode As TableMapNode

        Dim sourceMap As ISourceMap = m_Map.GetSourceMap
        Dim sourceNode As SourceMapNode

        Dim columnMap As IColumnMap = m_Map.GetTypeColumnMap
        Dim columnNode As ColumnMapNode

        Dim foundNode As MapNode
        Dim noAdd As Boolean

        Dim listSorted As ArrayList

        foundNode = Nothing
        noAdd = False
        For Each childNode In Nodes
            If childNode.GetType Is GetType(ColumnMapNode) Then
                foundNode = childNode
                Exit For
            End If
        Next

        If envSettings.ShowMappedNodes Then


            If Len(m_Map.TypeColumn) > 0 Then

                If Not columnMap Is Nothing Then

                    If Not foundNode Is Nothing Then
                        If Not foundNode.Map Is columnMap Then
                            If Not foundNode Is Nothing Then Nodes.Remove(foundNode)
                        Else
                            noAdd = True
                            Me.Nodes.Remove(foundNode)
                            Me.Nodes.Insert(0, foundNode)
                        End If
                    End If

                    If Not noAdd Then

                        columnNode = New ColumnMapNode(columnMap)

                        Me.Nodes.Insert(0, columnNode)

                        'Set regular font
                        columnNode.SetRegularFont()

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

        End If




        For Each childNode In Me.Nodes
            If childNode.GetType Is GetType(PropertyMapNode) Then
                hashOrg(CType(childNode, PropertyMapNode).Map) = childNode
            End If
        Next


        listSorted = m_Map.GetNonInheritedIdentityPropertyMaps
        'listSorted.Sort()

        For Each propertyMap In listSorted

            If Not arrFound.Contains(propertyMap) Then

                arrFound.Add(propertyMap)

                If Not hashOrg.ContainsKey(propertyMap) Then

                    newNode = New PropertyMapNode(propertyMap)

                    Me.Nodes.Add(newNode)

                    newNode.Refresh(False)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            End If

        Next

        If envSettings.ShowInheritedNodes Then

            listSorted = m_Map.GetInheritedIdentityPropertyMaps
            'listSorted.Sort()

            For Each propertyMap In listSorted

                If Not arrFound.Contains(propertyMap) Then

                    arrFound.Add(propertyMap)

                    If Not hashOrg.ContainsKey(propertyMap) Then

                        newNode = New PropertyMapNode(propertyMap, True)

                        Me.Nodes.Add(newNode)

                        newNode.Refresh(False)

                        'Set regular font
                        newNode.SetRegularFont()

                    End If

                End If

            Next

        End If


        listSorted = m_Map.GetNonInheritedPropertyMaps
        listSorted.Sort()

        For Each propertyMap In listSorted

            If Not arrFound.Contains(propertyMap) Then

                arrFound.Add(propertyMap)

                If Not hashOrg.ContainsKey(propertyMap) Then

                    newNode = New PropertyMapNode(propertyMap)

                    Me.Nodes.Add(newNode)

                    newNode.Refresh(False)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            End If

        Next

        If envSettings.ShowInheritedNodes Then

            listSorted = m_Map.GetInheritedPropertyMaps
            listSorted.Sort()

            For Each propertyMap In listSorted

                If Not arrFound.Contains(propertyMap) Then

                    arrFound.Add(propertyMap)

                    If Not hashOrg.ContainsKey(propertyMap) Then

                        newNode = New PropertyMapNode(propertyMap, True)

                        Me.Nodes.Add(newNode)

                        newNode.Refresh(False)

                        'Set regular font
                        newNode.SetRegularFont()

                    End If

                End If

            Next

        End If


        For Each propertyMap In hashOrg.Keys

            If Not arrFound.Contains(propertyMap) Then

                Me.Nodes.Remove(hashOrg(propertyMap))

            End If

        Next



    End Sub


    Public Sub OnExpandEnum()

        Dim enumValueMap As IEnumValueMap
        Dim newNode As EnumValueMapNode

        Dim listSorted As ArrayList
        Dim arrAdded As New ArrayList

        listSorted = m_Map.GetEnumValueMaps()

        'listSorted.Sort()
        For Each enumValueMap In listSorted

            If Not arrAdded.Contains(enumValueMap) Then

                arrAdded.Add(enumValueMap)

                newNode = New EnumValueMapNode(enumValueMap)

                Me.Nodes.Add(newNode)

                'Set regular font
                newNode.SetRegularFont()

            End If

        Next

    End Sub


    Public Sub RefreshEnum()

        Dim addDummy As Boolean

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If m_Map.EnumValueMaps.Count > 0 Then addDummy = True

                If addDummy Then
                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim enumValueMap As IEnumValueMap
        Dim newNode As EnumValueMapNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String
        Dim arrFound As New ArrayList

        Dim foundNode As MapNode
        Dim noAdd As Boolean

        Dim listSorted As ArrayList


        For Each childNode In Me.Nodes
            If childNode.GetType Is GetType(EnumValueMapNode) Then
                hashOrg(CType(childNode, EnumValueMapNode).Map) = childNode
            End If
        Next


        listSorted = m_Map.GetEnumValueMaps
        'listSorted.Sort()

        For Each enumValueMap In listSorted

            If Not arrFound.Contains(enumValueMap) Then

                arrFound.Add(enumValueMap)

                If Not hashOrg.ContainsKey(enumValueMap) Then

                    newNode = New EnumValueMapNode(enumValueMap)

                    Me.Nodes.Add(newNode)

                    newNode.Refresh(False)

                    'Set regular font
                    newNode.SetRegularFont()

                End If

            End If

        Next

        For Each enumValueMap In hashOrg.Keys

            If Not arrFound.Contains(enumValueMap) Then

                Me.Nodes.Remove(hashOrg(enumValueMap))

            End If

        Next

    End Sub




End Class