Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class MapListView
    Inherits ListView

#Region " Public API "

    Public Function CanGoBack() As Boolean

        Dim ok As Boolean

        ok = m_Back.Count > 0

        Return ok

    End Function

    Public Function CanGoForward() As Boolean

        Dim ok As Boolean

        ok = m_Forward.Count > 0

        Return ok

    End Function

    Public Function CanGoUp() As Boolean

        If m_ParentNode Is Nothing Then Exit Function

        If m_ParentNode.GetType Is GetType(ProjectNode) Then Exit Function

        Return True

    End Function


    Public Sub Go(ByVal parentNode As MapNode)

        If Not m_ParentNode Is Nothing Then

            If Not m_ParentNode Is parentNode Then

                m_Back.Add(m_ParentNode)

            End If

        End If

        m_Forward.Clear()

        SetParentNode(parentNode)

    End Sub

    Public Sub GoBack()

        If Not CanGoBack() Then Exit Sub

        If m_Back.Count > 0 Then

            If Not m_ParentNode Is Nothing Then

                m_Forward.Add(m_ParentNode)

            End If

            SetParentNode(m_Back(m_Back.Count - 1))

            m_Back.RemoveAt(m_Back.Count - 1)

        End If

    End Sub

    Public Sub GoForward()

        If Not CanGoForward() Then Exit Sub

        If m_Forward.Count > 0 Then

            If Not m_ParentNode Is Nothing Then

                m_Back.Add(m_ParentNode)

            End If

            SetParentNode(m_Forward(m_Forward.Count - 1))

            m_Forward.RemoveAt(m_Forward.Count - 1)

        End If

    End Sub

    Public Sub GoUp()

        Dim newNode As MapNode

        If Not CanGoUp() Then Exit Sub

        If m_ParentNode Is Nothing Then Exit Sub

        Dim domainMap As IDomainMap
        Dim config As DomainConfig
        Dim clsToCodeConfig As ClassesToCodeConfig
        Dim resource As IResource
        Dim src As SourceCodeFile

        If m_ParentNode.GetType Is GetType(ProjectNode) Then

        ElseIf m_ParentNode.GetType Is GetType(DomainMapNode) Then

            newNode = New ProjectNode(m_frmDomainMapBrowser.m_Project)

        ElseIf m_ParentNode.GetType Is GetType(ClassListMapNode) Then

            newNode = New DomainMapNode(CType(m_ParentNode, ClassListMapNode).GetMap)

        ElseIf m_ParentNode.GetType Is GetType(SourceListMapNode) Then

            newNode = New DomainMapNode(CType(m_ParentNode, SourceListMapNode).GetMap)

        ElseIf m_ParentNode.GetType Is GetType(ConfigListNode) Then

            newNode = New DomainMapNode(CType(m_ParentNode, ConfigListNode).GetMap)

        ElseIf m_ParentNode.GetType Is GetType(ClassMapNode) Then

            newNode = New ClassListMapNode(CType(CType(m_ParentNode, ClassMapNode).GetMap, IClassMap).DomainMap)

        ElseIf m_ParentNode.GetType Is GetType(SourceMapNode) Then

            newNode = New SourceListMapNode(CType(CType(m_ParentNode, SourceMapNode).GetMap, ISourceMap).DomainMap)

        ElseIf m_ParentNode.GetType Is GetType(TableMapNode) Then

            newNode = New SourceMapNode(CType(CType(m_ParentNode, TableMapNode).GetMap, ITableMap).SourceMap)

        ElseIf m_ParentNode.GetType Is GetType(ConfigNode) Then

            newNode = New ConfigListNode(CType(m_ParentNode, ConfigNode).DomainMap)

        ElseIf m_ParentNode.GetType Is GetType(ClassesToCodeConfigNode) Then

            domainMap = CType(m_ParentNode, ClassesToCodeConfigNode).DomainMap
            clsToCodeConfig = CType(m_ParentNode, ClassesToCodeConfigNode).GetMap

            resource = m_frmDomainMapBrowser.m_Project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

            For Each config In resource.Configs

                If config.ClassesToCodeConfig Is clsToCodeConfig Then

                    newNode = New ConfigNode(config, CType(m_ParentNode, ClassesToCodeConfigNode).DomainMap)

                    Exit For

                End If

            Next

        ElseIf m_ParentNode.GetType Is GetType(SourceCodeFileListNode) Then

            domainMap = CType(m_ParentNode, SourceCodeFileListNode).DomainMap
            clsToCodeConfig = CType(m_ParentNode, SourceCodeFileListNode).GetMap

            resource = m_frmDomainMapBrowser.m_Project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

            For Each config In resource.Configs

                If config.ClassesToCodeConfig Is clsToCodeConfig Then

                    newNode = New ClassesToCodeConfigNode(clsToCodeConfig, CType(m_ParentNode, SourceCodeFileListNode).DomainMap)

                    Exit For

                End If

            Next
        End If

        If Not newNode Is Nothing Then

            Go(newNode)

        End If

    End Sub

#End Region


#Region " Misc "

    Private m_frmDomainMapBrowser As frmDomainMapBrowser

    Private m_ParentNode As MapNode

    Private m_ColumnsNode As MapNode


    Private m_Back As New ArrayList
    Private m_Forward As New ArrayList

    Public Property frmDomainMapBrowser() As frmDomainMapBrowser
        Get
            Return m_frmDomainMapBrowser
        End Get
        Set(ByVal Value As frmDomainMapBrowser)
            m_frmDomainMapBrowser = Value
        End Set
    End Property

    Public Property ParentNode() As MapNode
        Get
            Return m_ParentNode
        End Get
        Set(ByVal Value As MapNode)
            m_ParentNode = Value
        End Set
    End Property


    Private Sub SetParentNode(ByVal parentNode As MapNode)

        m_ParentNode = parentNode

        SetupColumns()

        If m_ParentNode Is Nothing Then

            Exit Sub

        End If

        If m_ParentNode.GetType Is GetType(ProjectNode) Then

            OpenProject()

        ElseIf m_ParentNode.GetType Is GetType(DomainMapNode) Then

            OpenDomainMap()

        ElseIf m_ParentNode.GetType Is GetType(ClassListMapNode) Then

            OpenClassList()

        ElseIf m_ParentNode.GetType Is GetType(SourceListMapNode) Then

            OpenSourceList()

        ElseIf m_ParentNode.GetType Is GetType(ConfigListNode) Then

            OpenConfigList()

        ElseIf m_ParentNode.GetType Is GetType(ClassMapNode) Then

            OpenClassMap()

        ElseIf m_ParentNode.GetType Is GetType(SourceMapNode) Then

            OpenSourceMap()

        ElseIf m_ParentNode.GetType Is GetType(TableMapNode) Then

            OpenTableMap()

        ElseIf m_ParentNode.GetType Is GetType(ConfigNode) Then

            OpenConfig()

        ElseIf m_ParentNode.GetType Is GetType(ClassesToCodeConfigNode) Then

            OpenClassesToCodeConfig()

        ElseIf m_ParentNode.GetType Is GetType(SourceCodeFileListNode) Then

            OpenSourceCodeFileList()

        End If

    End Sub


    Public Sub RefreshItems()

        If m_ParentNode Is Nothing Then

            Me.Clear()

            Exit Sub

        End If

        If m_ParentNode.GetType Is GetType(ProjectNode) Then

            RefreshProject()

        ElseIf m_ParentNode.GetType Is GetType(DomainMapNode) Then

            'RefreshDomainMap()

        ElseIf m_ParentNode.GetType Is GetType(ClassListMapNode) Then

            'RefreshClassList()

        End If

        Dim item As MapListItem

        For Each item In Me.Items

            item.Refresh()

        Next

    End Sub

    Private Sub SetupColumns()

        If m_ParentNode Is Nothing Then

            Me.Clear()

            Exit Sub

        End If

        'If Not m_ParentNode Is m_ColumnsNode Then

        Me.Clear()

        If m_ParentNode.GetType Is GetType(ProjectNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Default source", 150, HorizontalAlignment.Left)
            Me.Columns.Add("Root Namespace", 150, HorizontalAlignment.Left)


        ElseIf m_ParentNode.GetType Is GetType(DomainMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Description", 450, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(ClassListMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Namespace", 150, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(ClassMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Type", 100, HorizontalAlignment.Left)
            Me.Columns.Add("Field", 150, HorizontalAlignment.Left)
            Me.Columns.Add("Columns", 150, HorizontalAlignment.Left)
            Me.Columns.Add("Table", 150, HorizontalAlignment.Left)
            Me.Columns.Add("Lazy", 50, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(SourceListMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Type", 100, HorizontalAlignment.Left)
            Me.Columns.Add("Provider", 150, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(SourceMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            'Me.Columns.Add("Type", 100, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(TableMapNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Type", 100, HorizontalAlignment.Left)
            Me.Columns.Add("Precision", 50, HorizontalAlignment.Left)
            Me.Columns.Add("Nulls", 50, HorizontalAlignment.Left)
            Me.Columns.Add("Key", 50, HorizontalAlignment.Left)
            Me.Columns.Add("Foreign", 150, HorizontalAlignment.Left)
            Me.Columns.Add("Default", 150, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(ConfigListNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            Me.Columns.Add("Active", 150, HorizontalAlignment.Left)


        ElseIf m_ParentNode.GetType Is GetType(ConfigNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            'Me.Columns.Add("Active", 150, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(ClassesToCodeConfigNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            'Me.Columns.Add("Active", 150, HorizontalAlignment.Left)

        ElseIf m_ParentNode.GetType Is GetType(SourceCodeFileListNode) Then

            Me.Columns.Add("Name", 250, HorizontalAlignment.Left)
            'Me.Columns.Add("Active", 150, HorizontalAlignment.Left)

        End If

        'End If

        m_ColumnsNode = m_ParentNode

    End Sub

#End Region

#Region " Open "

    Private Sub OpenProject()

        Dim project As IProject
        Dim domainMap As IDomainMap
        Dim newItem As DomainMapItem

        Dim listSorted As New ArrayList
        Dim resource As ProjectModel.IResource


        project = CType(m_ParentNode, ProjectNode).GetMap

        If Not project Is Nothing Then

            For Each resource In project.Resources

                If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                    If Not resource.GetResource Is Nothing Then

                        listSorted.Add(resource.GetResource)

                    End If

                End If

            Next


        End If

        listSorted.Sort()


        For Each domainMap In listSorted

            newItem = New DomainMapItem(domainMap)


            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()

        Next


    End Sub

    Private Sub OpenDomainMap()

        Dim domainMap As IDomainMap = m_ParentNode.GetMap

        If Not domainMap Is Nothing Then

            Dim newItem As MapListItem

            newItem = New ClassListMapItem(domainMap)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()


            newItem = New SourceListMapItem(domainMap)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()


            newItem = New ConfigListItem(domainMap)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()


        End If

    End Sub

    Public Sub OpenClassList()

        Dim domainMap As IDomainMap = m_ParentNode.GetMap

        If Not domainMap Is Nothing Then

            Dim classMap As IClassMap
            Dim classItem As ClassMapItem

            Dim listSorted As ArrayList

            listSorted = domainMap.ClassMaps.Clone
            listSorted.Sort()

            For Each classMap In listSorted

                classItem = New ClassMapItem(classMap)

                Me.Items.Add(classItem)

                classItem.Refresh()

                'Set regular font
                classItem.SetRegularFont()


            Next

        End If

    End Sub

    Public Sub OpenClassMap()


        Dim classMap As IClassMap = m_ParentNode.GetMap

        Dim propertyMap As IPropertyMap
        Dim newItem As PropertyMapItem

        Dim listSorted As ArrayList
        Dim arrAdded As New ArrayList

        listSorted = classMap.GetNonInheritedIdentityPropertyMaps
        'listSorted.Sort()
        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newItem = New PropertyMapItem(propertyMap)

                Me.Items.Add(newItem)

                'Set regular font
                newItem.SetRegularFont()

            End If

        Next

        listSorted = classMap.GetInheritedIdentityPropertyMaps
        'listSorted.Sort()

        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newItem = New PropertyMapItem(propertyMap, True)


                Me.Items.Add(newItem)

                'Set regular font
                newItem.SetRegularFont()

            End If

        Next


        listSorted = classMap.GetNonInheritedPropertyMaps
        listSorted.Sort()
        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newItem = New PropertyMapItem(propertyMap)


                Me.Items.Add(newItem)

                'Set regular font
                newItem.SetRegularFont()

            End If

        Next

        listSorted = classMap.GetInheritedPropertyMaps
        listSorted.Sort()

        For Each propertyMap In listSorted

            If Not arrAdded.Contains(propertyMap) Then

                arrAdded.Add(propertyMap)

                newItem = New PropertyMapItem(propertyMap, True)


                Me.Items.Add(newItem)

                'Set regular font
                newItem.SetRegularFont()

            End If

        Next

    End Sub

    Public Sub OpenSourceList()

        Dim domainMap As IDomainMap = m_ParentNode.GetMap

        If Not domainMap Is Nothing Then

            Dim sourceMap As ISourceMap
            Dim sourceItem As SourceMapItem

            Dim listSorted As ArrayList

            listSorted = domainMap.SourceMaps.Clone
            listSorted.Sort()

            For Each sourceMap In listSorted

                sourceItem = New SourceMapItem(sourceMap)

                Me.Items.Add(sourceItem)

                sourceItem.Refresh()

                'Set regular font
                sourceItem.SetRegularFont()


            Next

        End If

    End Sub

    Public Sub OpenSourceMap()

        Dim sourceMap As ISourceMap = m_ParentNode.GetMap

        If Not sourceMap Is Nothing Then

            Dim tableMap As ITableMap
            Dim tableItem As TableMapItem

            Dim listSorted As ArrayList

            listSorted = sourceMap.TableMaps.Clone
            listSorted.Sort()

            For Each tableMap In listSorted

                tableItem = New TableMapItem(tableMap)

                Me.Items.Add(tableItem)

                tableItem.Refresh()

                'Set regular font
                tableItem.SetRegularFont()


            Next

        End If

    End Sub


    Public Sub OpenTableMap()

        Dim tableMap As ITableMap = m_ParentNode.GetMap

        If Not tableMap Is Nothing Then

            Dim columnMap As IColumnMap
            Dim columnItem As ColumnMapItem

            Dim listSorted As ArrayList

            listSorted = tableMap.ColumnMaps.Clone
            listSorted.Sort()

            For Each columnMap In listSorted

                columnItem = New ColumnMapItem(columnMap)

                Me.Items.Add(columnItem)

                columnItem.Refresh()

                'Set regular font
                columnItem.SetRegularFont()


            Next

        End If

    End Sub


    Public Sub OpenConfigList()

        Dim resource As ProjectModel.IResource
        Dim config As ProjectModel.DomainConfig
        Dim newItem As ConfigItem
        Dim listSorted As ArrayList

        Dim domainMap As IDomainMap = m_ParentNode.GetMap

        resource = m_frmDomainMapBrowser.m_Project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        listSorted = resource.Configs
        listSorted.Sort()

        For Each config In listSorted

            'Add the node for the list of data sources
            newItem = New ConfigItem(config, domainMap)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()

        Next

    End Sub

    Public Sub OpenConfig()

        Dim domainMap As IDomainMap = CType(m_ParentNode, ConfigNode).DomainMap
        Dim config As DomainConfig = m_ParentNode.GetMap

        Dim newItem As ClassesToCodeConfigItem
        newItem = New ClassesToCodeConfigItem(config.ClassesToCodeConfig, domainMap)

        Me.Items.Add(newItem)

        newItem.Refresh()

        'Set regular font
        newItem.SetRegularFont()

    End Sub

    Public Sub OpenClassesToCodeConfig()

        Dim domainMap As IDomainMap = CType(m_ParentNode, ClassesToCodeConfigNode).DomainMap
        Dim config As ClassesToCodeConfig = m_ParentNode.GetMap

        Dim newItem As SourceCodeFileListItem
        newItem = New SourceCodeFileListItem(config, domainMap)

        Me.Items.Add(newItem)

        newItem.Refresh()

        'Set regular font
        newItem.SetRegularFont()

    End Sub

    Public Sub OpenSourceCodeFileList()

        Dim domainMap As IDomainMap = CType(m_ParentNode, SourceCodeFileListNode).DomainMap
        Dim config As ClassesToCodeConfig = m_ParentNode.GetMap

        Dim newItem As SourceCodeFileItem
        Dim src As SourceCodeFile

        Dim listSorted As ArrayList

        listSorted = config.SourceCodeFiles
        listSorted.Sort()

        For Each src In listSorted

            'Add the node for the list of data sources
            newItem = New SourceCodeFileItem(src, domainMap)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()

        Next

    End Sub
#End Region


#Region " Refresh "

    Private Sub RefreshProject()

        Dim project As IProject
        Dim domainMap As IDomainMap
        Dim newItem As DomainMapItem
        Dim existingItem As DomainMapItem

        Dim listSorted As New ArrayList
        Dim resource As ProjectModel.IResource

        Dim remove As New ArrayList


        project = CType(m_ParentNode, ProjectNode).GetMap

        If Not project Is Nothing Then

            For Each resource In project.Resources

                If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                    If Not resource.GetResource Is Nothing Then

                        listSorted.Add(resource.GetResource)

                    End If

                End If

            Next


        End If

        listSorted.Sort()

        For Each existingItem In Me.Items

            If existingItem.GetType Is GetType(DomainMapItem) Then

                If Not listSorted.Contains(existingItem.GetMap) Then

                    remove.Add(existingItem)

                End If

            Else

                remove.Add(existingItem)

            End If

        Next

        For Each existingItem In remove

            Me.Items.Remove(existingItem)

        Next

        For Each existingItem In Me.Items

            existingItem.Refresh()

            listSorted.Remove(existingItem.GetMap)

        Next

        For Each domainMap In listSorted

            newItem = New DomainMapItem(domainMap)

            newItem.SubItems.Add(domainMap.RootNamespace)
            newItem.SubItems.Add(domainMap.Source)

            Me.Items.Add(newItem)

            newItem.Refresh()

            'Set regular font
            newItem.SetRegularFont()

        Next


    End Sub


#End Region

End Class
