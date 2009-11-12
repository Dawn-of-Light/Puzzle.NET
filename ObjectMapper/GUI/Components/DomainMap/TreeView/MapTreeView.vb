Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Mapping.Visitor
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class MapTreeView
    Inherits System.Windows.Forms.TreeView

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Private m_ListExceptions As New ArrayList()

    Private m_Verifying As Boolean

    Private m_IsVerifyTree As Boolean

    Private m_IsPreviewTree As Boolean

    Private m_ShowMappings As Boolean

    Private m_ShowInherited As Boolean

    Private m_unCheckedNodes As New ArrayList

    Private m_ExpandedNodes As New ArrayList

    Private m_NodeIcons As New Hashtable

    Private m_AutoExpandVerifyNodes As Boolean = False

    Public Event SelectedExceptionNode(ByVal mapNode As MapNode, ByVal listExceptions As ArrayList)

    Public Property AutoCollapseVerifyNodes() As Boolean
        Get
            Return m_AutoExpandVerifyNodes
        End Get
        Set(ByVal Value As Boolean)
            m_AutoExpandVerifyNodes = Value
        End Set
    End Property

    'This is the one place where new domain maps are loaded! Yes, here of all places......I know, I know, I should not be let near a keyboard....
    Public Overloads Function LoadDomain(ByVal path As String, Optional ByVal addNotToTree As Boolean = False) As IDomainMap

        Dim newDomainMap As IDomainMap = DomainMap.Load(path, Nothing, False, False)

        newDomainMap.UnFixate()

        If addNotToTree Then

            Return newDomainMap

        Else

            Return LoadDomain(newDomainMap)

        End If

    End Function

    Public Sub CloseProject()

        Me.Nodes.Clear()

    End Sub

    Public Sub LoadProject(ByVal project As ProjectModel.IProject)

        CloseProject()

        Dim newNode As ProjectNode

        newNode = New ProjectNode(project)

        'Add empty child node
        newNode.Nodes.Add(New MapNode)

        Me.Nodes.Add(newNode)

        'Set regular font
        newNode.SetRegularFont()

    End Sub

    Public Overloads Function LoadDomain(ByVal domainMap As IDomainMap) As IDomainMap

        Dim newNode As DomainMapNode
        Dim projNode As ProjectNode = GetProjectNode()

        If Not projNode Is Nothing Then

            If Not projNode.IsExpanded Then

                projNode.Expand()

            End If

        Else

            newNode = New DomainMapNode(domainMap)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()

        End If

        Return domainMap

    End Function

    Public Function RemoveDomainMap(ByVal domainMap As IDomainMap)

        Dim mapNode As mapNode

        mapNode = GetDomainMapNode(domainMap)

        If Not mapNode Is Nothing Then

            If mapNode.Parent Is Nothing Then

                Me.Nodes.Remove(mapNode)

            Else

                mapNode.Parent.Nodes.Remove(mapNode)

            End If
        End If

    End Function

    Public Property IsPreviewTree() As Boolean
        Get
            Return m_IsPreviewTree
        End Get
        Set(ByVal Value As Boolean)
            m_IsPreviewTree = Value
        End Set
    End Property


    Public Property IsVerifyTree() As Boolean
        Get
            Return m_IsVerifyTree
        End Get
        Set(ByVal Value As Boolean)
            m_IsVerifyTree = Value
        End Set
    End Property

    Private Sub MapTreeView_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles MyBase.BeforeExpand

        CType(e.Node, MapNode).OnExpand()

    End Sub

    Public Sub RefreshAllNodes(ByVal resetIcons As Boolean)

        Dim mn As MapNode

        Me.BeginUpdate()

        For Each mn In Nodes

            RefreshNode(mn, resetIcons)

        Next

        Me.EndUpdate()

    End Sub

    Private Sub RefreshNode(ByVal mapNode As MapNode, ByVal resetIcons As Boolean)

        Dim mn As mapNode

        If mapNode.IsExpanded Then

            'Refresh child nodes first, allowing them to update their names
            For Each mn In mapNode.Nodes

                RefreshNode(mn, resetIcons)

            Next

        End If

        mapNode.Refresh(resetIcons)

    End Sub


    Public Sub RefreshAllFileNodes()

        Dim mn As MapNode

        For Each mn In Nodes

            RefreshFileNode(mn)

        Next

    End Sub

    Private Sub RefreshFileNode(ByVal mapNode As MapNode)

        Dim mn As mapNode

        If mapNode.IsExpanded Then

            'Refresh child nodes first, allowing them to update their names
            For Each mn In mapNode.Nodes

                RefreshFileNode(mn)

            Next

        End If

        If mapNode.GetType Is GetType(SourceCodeFileNode) Then

            mapNode.Refresh(False)

        End If

    End Sub

    Public Function VerifyNew(ByVal m_Project As ProjectModel.IProject, Optional ByVal checkMappings As Boolean = True) As Boolean

        m_ListExceptions.Clear()

        m_NodeIcons.Clear()

        If m_Project Is Nothing Then Return True

        m_Verifying = True

        Dim domainMaps As IList = m_Project.GetDomainMaps()

        Dim visitor As New MapVerificationVisitor(True, checkMappings)

        visitor.Exceptions = m_ListExceptions

        For Each domainMap As IDomainMap In domainMaps

            domainMap.Accept(visitor)

        Next

        'Dim mn As MapNode

        'For Each mn In Nodes

        '    VerifyNodeNew(m_Project, mn, m_ListExceptions, checkMappings)

        'Next

        ''error node
        'If Not SelectedNode Is Nothing Then

        '    If SelectedNode.ImageIndex = 26 Then

        '        RaiseSelectedExceptionNodeEvent(SelectedNode)

        '    End If

        'End If

        m_Verifying = False

        If m_ListExceptions.Count > 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub VerifyNodeNew(ByVal m_Project As ProjectModel.IProject, ByVal mapNode As MapNode, ByRef listExceptions As ArrayList, Optional ByVal checkMappings As Boolean = True)

        Dim mn As MapNode
        Dim oldCnt As Integer
        Dim oldChildCnt As Integer
        Dim newChildCnt As Integer
        Dim wasExpanded As Boolean = True

        'If Not mapNode.Map Is Nothing Then

        '    If Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

        '        SetVerificationSettings(m_Project, mapNode.Map, setTargetLanguages)

        '    End If

        'End If

        'If Not mapNode.IsExpanded Then

        '    mapNode.Expand()
        '    wasExpanded = False

        'End If

        oldChildCnt = listExceptions.Count

        'Refresh child nodes first, allowing them to update their names
        If mapNode.IsExpanded Then

            For Each mn In mapNode.Nodes

                VerifyNodeNew(m_Project, mn, listExceptions, checkMappings)

            Next

        End If


        newChildCnt = listExceptions.Count


        If Not mapNode.Map Is Nothing Then

            oldCnt = listExceptions.Count

            Dim visitor As New MapVerificationVisitor(False, checkMappings)
            visitor.Exceptions = listExceptions

            mapNode.Map.Accept(visitor)

            If listExceptions.Count > oldCnt Then

                mapNode.ImageIndex = 26
                mapNode.SelectedImageIndex = 27

            Else

                If newChildCnt > oldChildCnt Then

                    mapNode.ImageIndex = 24
                    mapNode.SelectedImageIndex = 25

                Else

                    'mapNode.ImageIndex = 22
                    'mapNode.SelectedImageIndex = 23

                End If

            End If

        Else

            If newChildCnt > oldChildCnt Then

                mapNode.ImageIndex = 24
                mapNode.SelectedImageIndex = 25

            Else

                mapNode.ImageIndex = 22
                mapNode.SelectedImageIndex = 23

            End If

        End If

        m_NodeIcons(mapNode.FullPath) = mapNode.ImageIndex

    End Sub



    Public Function Verify(ByVal m_Project As ProjectModel.IProject, Optional ByVal closeErrorNodes As Boolean = False, Optional ByVal closeOkNodes As Boolean = True, Optional ByVal checkMappings As Boolean = True, Optional ByVal setTargetLanguages As Boolean = True) As Boolean

        m_ListExceptions.Clear()

        m_NodeIcons.Clear()

        m_Verifying = True

        Dim mn As MapNode

        For Each mn In Nodes

            VerifyNode(m_Project, mn, m_ListExceptions, closeErrorNodes, closeOkNodes, checkMappings, setTargetLanguages)

        Next

        'error node
        If Not SelectedNode Is Nothing Then

            If SelectedNode.ImageIndex = 26 Then

                RaiseSelectedExceptionNodeEvent(SelectedNode)

            End If

        End If

        m_Verifying = False

        If m_ListExceptions.Count > 0 Then
            Return False
        Else
            Return True
        End If

    End Function



    Public Property ListExceptions() As ArrayList
        Get
            Return m_ListExceptions
        End Get
        Set(ByVal Value As ArrayList)
            m_ListExceptions = Value
        End Set
    End Property

    Private Sub VerifyNode(ByVal m_Project As ProjectModel.IProject, ByVal mapNode As MapNode, ByRef listExceptions As ArrayList, Optional ByVal closeErrorNodes As Boolean = False, Optional ByVal closeOkNodes As Boolean = True, Optional ByVal checkMappings As Boolean = True, Optional ByVal setTargetLanguages As Boolean = True)

        Dim mn As mapNode
        Dim oldCnt As Integer
        Dim oldChildCnt As Integer
        Dim newChildCnt As Integer
        Dim wasExpanded As Boolean = True

        If Not mapNode.Map Is Nothing Then

            If Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

                SetVerificationSettings(m_Project, mapNode.Map, setTargetLanguages)

            End If

        End If

        If Not mapNode.IsExpanded Then

            mapNode.Expand()
            wasExpanded = False

        End If

        oldChildCnt = listExceptions.Count

        'Refresh child nodes first, allowing them to update their names
        For Each mn In mapNode.Nodes

            VerifyNode(m_Project, mn, listExceptions, closeErrorNodes, closeOkNodes, checkMappings, setTargetLanguages)

        Next

        newChildCnt = listExceptions.Count


        If Not mapNode.Map Is Nothing Then

            oldCnt = listExceptions.Count

            Dim visitor As New MapVerificationVisitor(False, checkMappings)
            visitor.Exceptions = listExceptions

            mapNode.Map.Accept(visitor)

            If listExceptions.Count > oldCnt Then

                mapNode.ImageIndex = 26
                mapNode.SelectedImageIndex = 27

            Else

                If newChildCnt > oldChildCnt Then

                    mapNode.ImageIndex = 24
                    mapNode.SelectedImageIndex = 25

                Else

                    mapNode.ImageIndex = 22
                    mapNode.SelectedImageIndex = 23

                End If

            End If

        Else

            If newChildCnt > oldChildCnt Then

                mapNode.ImageIndex = 24
                mapNode.SelectedImageIndex = 25

            Else

                mapNode.ImageIndex = 22
                mapNode.SelectedImageIndex = 23

            End If

        End If

        m_NodeIcons(mapNode.FullPath) = mapNode.ImageIndex

        If mapNode.ImageIndex = 22 Or closeErrorNodes Or wasExpanded = False Then ' Or (wasExpanded = False And m_AutoExpandVerifyNodes = False) Then

            If Not wasExpanded Or closeOkNodes Then

                mapNode.Collapse()

            End If

        End If

    End Sub


    Private Sub SetVerificationSettings(ByVal m_Project As ProjectModel.IProject, ByVal domainMap As IDomainMap, Optional ByVal setTargetLanguages As Boolean = True)


        If setTargetLanguages Then

            Dim resource As ProjectModel.IResource = m_Project.GetResource(domainMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)
            Dim cfg As ProjectModel.DomainConfig

            domainMap.VerifyCSharpReservedWords = False
            domainMap.VerifyVbReservedWords = False
            domainMap.VerifyDelphiReservedWords = False

            If Not resource Is Nothing Then

                For Each cfg In resource.Configs

                    Select Case cfg.ClassesToCodeConfig.TargetLanguage

                        Case ProjectModel.SourceCodeFileTypeEnum.CSharp

                            domainMap.VerifyCSharpReservedWords = True

                        Case ProjectModel.SourceCodeFileTypeEnum.VB

                            domainMap.VerifyVbReservedWords = True

                        Case ProjectModel.SourceCodeFileTypeEnum.Delphi

                            domainMap.VerifyDelphiReservedWords = True

                    End Select

                Next

            End If

        End If

    End Sub


    Private Sub MapTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles MyBase.AfterSelect

        'error node
        If e.Node.ImageIndex = 26 Then

            RaiseSelectedExceptionNodeEvent(e.Node)


        End If

    End Sub


    Public Function GetMapNodeExceptions(ByVal mapNode As MapNode, ByVal withSubExceptions As Boolean) As IList

        Dim listExceptions As IList = New ArrayList

        If mapNode Is Nothing Then Return listExceptions

        If mapNode.GetType() Is GetType(ProjectNode) Then

            Return m_ListExceptions
            'For Each mapException In m_ListExceptions

            '    listExceptions.Add(mapException)

            'Next

        ElseIf mapNode.GetType() Is GetType(ClassListMapNode) Then

            Dim domainMap As IDomainMap = mapNode.GetMap

            If Not domainMap Is Nothing Then

                For Each classMap As IClassMap In domainMap.ClassMaps

                    DoGetMapNodeExceptions(classMap, withSubExceptions, listExceptions)

                Next

            End If

        ElseIf mapNode.GetType() Is GetType(SourceListMapNode) Then

            Dim domainMap As IDomainMap = mapNode.GetMap

            If Not domainMap Is Nothing Then

                For Each sourceMap As ISourceMap In domainMap.SourceMaps

                    DoGetMapNodeExceptions(sourceMap, withSubExceptions, listExceptions)

                Next

            End If

        Else

            DoGetMapNodeExceptions(mapNode.GetMap, withSubExceptions, listExceptions)

        End If


        Return listExceptions

    End Function

    Private Function DoGetMapNodeExceptions(ByVal map As IMap, ByVal withSubExceptions As Boolean, ByVal listExceptions As IList) As IList

        Dim mapException As MappingException

        For Each mapException In m_ListExceptions

            If Not mapException.MapObject Is Nothing Then

                If mapException.MapObject Is map Then

                    listExceptions.Add(mapException)

                ElseIf withSubExceptions Then

                    'If mapNode.Map.IsInParents(mapException.MapObject) Then
                    If mapException.MapObject.IsInParents(map) Then

                        listExceptions.Add(mapException)

                    End If

                End If

            End If

        Next

    End Function

    Public Function HasMapNodeExceptions(ByVal mapNode As MapNode, ByVal withSubExceptions As Boolean) As Boolean

        If mapNode Is Nothing Then Return False

        If mapNode.GetType() Is GetType(ProjectNode) Then

            If m_ListExceptions.Count > 0 Then

                Return True

            End If

            Return False

        ElseIf mapNode.GetType() Is GetType(ClassListMapNode) Then

            Dim domainMap As IDomainMap = mapNode.GetMap

            If Not domainMap Is Nothing Then

                For Each classMap As IClassMap In domainMap.ClassMaps

                    If DoHasMapNodeExceptions(classMap, withSubExceptions) Then

                        Return True

                    End If

                Next

            End If

        ElseIf mapNode.GetType() Is GetType(SourceListMapNode) Then

            Dim domainMap As IDomainMap = mapNode.GetMap

            If Not domainMap Is Nothing Then

                For Each sourceMap As ISourceMap In domainMap.SourceMaps

                    If DoHasMapNodeExceptions(sourceMap, withSubExceptions) Then

                        Return True

                    End If

                Next

            End If

        Else

            If DoHasMapNodeExceptions(mapNode.GetMap, withSubExceptions) Then

                Return True

            End If

        End If

        Return False

    End Function

    Private Function DoHasMapNodeExceptions(ByVal map As IMap, ByVal withSubExceptions As Boolean) As Boolean

        Dim mapException As MappingException

        For Each mapException In m_ListExceptions

            If Not mapException.MapObject Is Nothing Then

                If mapException.MapObject Is map Then

                    Return True

                ElseIf withSubExceptions Then

                    If mapException.MapObject.IsInParents(map) Then

                        Return True

                    End If

                End If

            End If

        Next

        Return False

    End Function


    Private Sub RaiseSelectedExceptionNodeEvent(ByVal mapNode As MapNode)

        Dim listExceptions As IList = GetMapNodeExceptions(mapNode, True)

        RaiseEvent SelectedExceptionNode(mapNode, listExceptions)

    End Sub

    Public Sub ExpandAllNodes()

        Dim mn As MapNode

        For Each mn In Nodes

            ExpandNode(mn)

        Next

    End Sub

    Private Sub ExpandNode(ByVal mapNode As MapNode)

        Dim mn As mapNode

        If Not mapNode.IsExpanded Then

            mapNode.Expand()

        End If

        For Each mn In mapNode.Nodes

            ExpandNode(mn)

        Next

    End Sub


    Public Sub MarkAllNodes(ByVal value As Boolean)

        ClearUnCheckedNodesMemory()

        Dim mn As MapNode

        For Each mn In Nodes

            MarkNode(mn, value)

        Next

    End Sub

    Private Sub MarkNode(ByVal mapNode As MapNode, ByVal value As Boolean)

        Dim mn As mapNode

        If Not mapNode.IsExpanded Then

            mapNode.Expand()

        End If

        mapNode.Checked = value

        For Each mn In mapNode.Nodes

            MarkNode(mn, value)

        Next

    End Sub

    Public Function GetDomainMapsForMarkedNodes(ByVal value As Boolean, Optional ByVal cloneAll As Boolean = False) As ArrayList

        Dim mn As MapNode

        Dim domainMap As IDomainMap

        Dim list As New ArrayList

        For Each mn In Nodes

            domainMap = CloneMarkedNode(mn, value, Nothing, cloneAll)

            If Not domainMap Is Nothing Then

                list.Add(domainMap)

            End If

        Next

        Return list

    End Function


    Public Function CloneMarkedNodes(ByVal value As Boolean, Optional ByVal cloneAll As Boolean = False) As IDomainMap

        Dim mn As MapNode

        Dim domainMap As IDomainMap

        For Each mn In Nodes

            domainMap = CloneMarkedNode(mn, value, Nothing, cloneAll)

        Next

        Return domainMap

    End Function

    Private Function CloneMarkedNode(ByVal mapNode As MapNode, ByVal value As Boolean, ByRef domainMap As IDomainMap, Optional ByVal cloneAll As Boolean = False) As IDomainMap

        Dim mn As mapNode
        Dim mapObject As Object
        Dim cloneDomain As IDomainMap
        Dim cloneClass As IClassMap
        Dim cloneProperty As IPropertyMap
        Dim cloneSource As ISourceMap
        Dim cloneTable As ITableMap
        Dim cloneColumn As IColumnMap

        If Not cloneAll Then

            If Not mapNode.Checked = value Then Exit Function

        End If

        If Not mapNode.Map Is Nothing Then

            If Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

                cloneDomain = CType(mapNode.Map, IDomainMap).Clone

                domainMap = cloneDomain

            ElseIf Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IClassMap).ToString) Is Nothing Then

                cloneClass = CType(mapNode.Map, IClassMap).Clone

                cloneClass.DomainMap = domainMap

            ElseIf Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IPropertyMap).ToString) Is Nothing Then

                If Not (mapNode.ImageIndex = 32 Or mapNode.ImageIndex = 34) Then

                    cloneProperty = CType(mapNode.Map, IPropertyMap).Clone

                    cloneProperty.ClassMap = domainMap.GetClassMap(CType(mapNode.Map, IPropertyMap).ClassMap.Name)

                End If

            ElseIf Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(ISourceMap).ToString) Is Nothing Then

                cloneSource = CType(mapNode.Map, ISourceMap).Clone

                cloneSource.DomainMap = domainMap

            ElseIf Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(ITableMap).ToString) Is Nothing Then

                'if we see a column or table via a class or property, the source has not been added yet..
                Try

                    cloneTable = CType(mapNode.Map, ITableMap).Clone

                    cloneTable.SourceMap = domainMap.GetSourceMap(CType(mapNode.Map, ITableMap).SourceMap.Name)

                Catch ex As Exception

                End Try

            ElseIf Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IColumnMap).ToString) Is Nothing Then

                'if we see a column or table via a class or property, the source has not been added yet..
                Try

                    cloneColumn = CType(mapNode.Map, IColumnMap).Clone

                    cloneColumn.TableMap = domainMap.GetSourceMap(CType(mapNode.Map, IColumnMap).TableMap.SourceMap.Name).GetTableMap(CType(mapNode.Map, IColumnMap).TableMap.Name)

                Catch ex As Exception

                End Try

            End If

        End If

        For Each mn In mapNode.Nodes

            CloneMarkedNode(mn, value, domainMap)

        Next

        Return domainMap

    End Function


    Public Function GetProjectNode() As ProjectNode

        Dim mapNode As mapNode

        For Each mapNode In Me.Nodes

            If mapNode.GetType Is GetType(ProjectNode) Then

                Return mapNode

            End If

        Next

    End Function

    Public Function GetDomainMap(ByVal name As String) As IDomainMap

        Dim domainMap As IDomainMap
        Dim projNode As ProjectNode
        Dim mapNode As mapNode
        Dim resource As ProjectModel.IResource


        projNode = GetProjectNode()

        If Not projNode Is Nothing Then

            For Each resource In CType(projNode.Map, ProjectModel.IProject).Resources

                If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                    If Not resource.GetResource Is Nothing Then

                        If CType(resource.GetResource, IDomainMap).Name = name Then

                            domainMap = CType(resource.GetResource, IDomainMap)

                            Exit For

                        End If

                    End If

                End If

            Next

        Else

            For Each mapNode In Me.Nodes

                If Not CType(mapNode.Map, Object).GetType.GetInterface(GetType(IDomainMap).ToString) Is Nothing Then

                    If mapNode.Map.Name = name Then

                        domainMap = mapNode.Map

                        Exit For

                    End If

                End If

            Next

        End If


        Return domainMap

    End Function

    Public Function GetSelectedDomainMap() As IDomainMap

        Dim node As MapNode = Me.SelectedNode

        While Not node Is Nothing

            If node.GetType Is GetType(DomainMapNode) Then

                Return node.Map

            End If

            node = node.Parent

        End While


    End Function

    Public Function GetAllDomainMaps() As ArrayList

        Dim domainMaps As New ArrayList
        Dim node As MapNode
        Dim projNode As ProjectNode = GetProjectNode()

        If Not projNode Is Nothing Then

            For Each node In projNode.Nodes

                If node.GetType Is GetType(DomainMapNode) Then

                    domainMaps.Add(CType(node, DomainMapNode).Map)

                End If

            Next

        Else

            For Each node In Me.Nodes

                If node.GetType Is GetType(DomainMapNode) Then

                    domainMaps.Add(CType(node, DomainMapNode).Map)

                End If

            Next

        End If


        Return domainMaps

    End Function

    Public Sub ClearAll()

        Me.Nodes.Clear()

    End Sub

    Public Sub ExpandProject()

        Dim node As ProjectNode = GetProjectNode()

        If Not node Is Nothing Then

            If Not node.IsExpanded Then

                node.Expand()

            End If

            Me.SelectedNode = node

        End If

    End Sub

    Public Function GetDomainMapNode(ByVal domainMap As IDomainMap) As DomainMapNode

        Dim projNode As ProjectNode
        Dim mapNode As mapNode
        Dim resource As ProjectModel.IResource


        projNode = GetProjectNode()

        If Not projNode Is Nothing Then

            For Each mapNode In projNode.Nodes

                If mapNode.GetMap Is domainMap Then

                    Return mapNode

                End If

            Next

        Else

            For Each mapNode In Me.Nodes

                If mapNode.GetMap Is domainMap Then

                    Return mapNode

                End If

            Next

        End If

    End Function


    Protected Overrides Sub OnAfterCheck(ByVal e As System.Windows.Forms.TreeViewEventArgs)

        If e.Node.Checked Then

            If m_unCheckedNodes.Contains(e.Node.FullPath) Then

                m_unCheckedNodes.Remove(e.Node.FullPath)

            End If

        Else

            If Not m_unCheckedNodes.Contains(e.Node.FullPath) Then

                m_unCheckedNodes.Add(e.Node.FullPath)

            End If

        End If

    End Sub

    Public Sub ClearUnCheckedNodesMemory()

        m_unCheckedNodes.Clear()

        ClearExpandedNodesMemory()

    End Sub

    Public Sub ClearExpandedNodesMemory()

        m_unCheckedNodes.Clear()

    End Sub


    Public Sub ClearNodeIconMemory()

        m_NodeIcons.Clear()

    End Sub

    Public Sub SetNodeIcons(ByVal node As TreeNode)

        Dim mapNode As mapNode = node

        mapNode.SetIcons()

        'If m_IsVerifyTree Then

        '    If m_NodeIcons.Contains(node.FullPath) Then

        '        node.ImageIndex = m_NodeIcons(node.FullPath)
        '        node.SelectedImageIndex = node.ImageIndex + 1

        '    End If

        'End If

    End Sub

    Public Sub SetNodeCheckedMode(ByVal node As TreeNode)

        If Me.CheckBoxes Then

            If m_unCheckedNodes.Contains(node.FullPath) Then

                node.Checked = False

            Else

                node.Checked = True

            End If

        End If

    End Sub

    Public Sub SetNodeExpandedMode(ByVal node As TreeNode)

        If m_ExpandedNodes.Contains(node.FullPath) Then

            If Not node.IsExpanded Then

                node.Expand()

            End If

        Else

            If node.IsExpanded Then

                node.Collapse()

            End If

        End If

    End Sub

    Protected Overrides Sub OnAfterExpand(ByVal e As System.Windows.Forms.TreeViewEventArgs)

        If Not m_ExpandedNodes.Contains(e.Node.FullPath) Then

            m_ExpandedNodes.Add(e.Node.FullPath)

        End If

    End Sub

    Protected Overrides Sub OnAfterCollapse(ByVal e As System.Windows.Forms.TreeViewEventArgs)

        If m_ExpandedNodes.Contains(e.Node.FullPath) Then

            m_ExpandedNodes.Remove(e.Node.FullPath)

        End If

    End Sub

    Public Sub ExtractToConfig(ByVal config As DomainConfig, ByVal domainMap As IDomainMap, Optional ByVal classesToCode As Boolean = False)

        Dim Node As MapNode

        For Each Node In Me.Nodes

            ExtractToConfigNode(config, domainMap, Node, classesToCode)

        Next

    End Sub

    Private Sub ExtractToConfigNode(ByVal config As DomainConfig, ByVal domainMap As IDomainMap, ByVal Node As MapNode, Optional ByVal classesToCode As Boolean = False)

        Dim Child As MapNode
        Dim Child2 As MapNode
        Dim Child3 As MapNode

        Dim mapObject As Object = Node.GetMap
        Dim testDomainMap As IDomainMap

        If Not mapObject Is Nothing Then

            If mapObject.GetType Is GetType(domainMap) Then

                testDomainMap = mapObject

                If testDomainMap Is domainMap Then

                    If Not Node.IsExpanded Then

                        Node.Expand()

                    End If

                    For Each Child In Node.Nodes

                        If Child.ImageIndex = 37 Then

                            Node = Child

                            If Not Node.IsExpanded Then

                                Node.Expand()

                            End If

                            For Each Child2 In Node.Nodes

                                If Child2.GetMap Is config Then

                                    Node = Child2

                                    If Not Node.IsExpanded Then

                                        Node.Expand()

                                    End If

                                    Me.SelectedNode = Node

                                    For Each Child3 In Node.Nodes

                                        If Child3.GetType Is GetType(ClassesToCodeConfigNode) Then

                                            If classesToCode Then

                                                Me.SelectedNode = Child3

                                            End If

                                        End If

                                    Next


                                    Exit For

                                End If

                            Next

                            Exit For

                        End If

                    Next

                End If

                Exit Sub

            End If

        End If

        If Not Node.IsExpanded Then

            Node.Expand()

        End If

        For Each Child In Node.Nodes

            ExtractToConfigNode(config, domainMap, Child, classesToCode)

        Next

    End Sub

    Public Function GetPreviewFileNode(ByVal fileName As String) As PreviewFileNode

        Dim fileNode As PreviewFileNode
        Dim folderNode As PreviewFolderNode

        fileName = LCase(fileName)

        For Each folderNode In Me.Nodes

            For Each fileNode In folderNode.Nodes

                If LCase(fileNode.GetFileName) = fileName Then

                    Return fileNode

                End If


                'If LCase(fileNode.Src.FilePath) = fileName Then

                '    Return fileNode

                'End If

            Next

        Next

        Return Nothing

    End Function

    Public Function GetPreviewFolderNode(ByVal domainMap As IDomainMap) As PreviewFolderNode

        Dim folderNode As PreviewFolderNode

        For Each folderNode In Me.Nodes

            If folderNode.DomainMap Is domainMap Then

                Return folderNode

            End If

        Next

        Return Nothing

    End Function

    Public Function ExpandToMapObject(ByVal map As IMap) As MapNode

        If map Is Nothing Then Return Nothing

        Dim mapType As Type = CType(map, Object).GetType()

        If Not mapType.GetInterface(GetType(IDomainMap).ToString()) Is Nothing Then

            Return ExpandToDomainMap(map)

        ElseIf Not mapType.GetInterface(GetType(IClassMap).ToString()) Is Nothing Then

            Return ExpandToClassMap(map)

        ElseIf Not mapType.GetInterface(GetType(IPropertyMap).ToString()) Is Nothing Then

            Return ExpandToPropertyMap(map)

        ElseIf Not mapType.GetInterface(GetType(ISourceMap).ToString()) Is Nothing Then

            Return ExpandToSourceMap(map)

        ElseIf Not mapType.GetInterface(GetType(ITableMap).ToString()) Is Nothing Then

            Return ExpandToTableMap(map)

        ElseIf Not mapType.GetInterface(GetType(IColumnMap).ToString()) Is Nothing Then

            Return ExpandToColumnMap(map)

        End If

        Return Nothing

    End Function

    Public Function ExpandToDomainMap(ByVal domainMap As IDomainMap) As DomainMapNode

        If domainMap Is Nothing Then Return Nothing

        For Each mapNode As mapNode In Me.Nodes

            If CType(mapNode, Object).GetType() Is GetType(ProjectNode) Then

                mapNode.Expand()

                For Each domainMapNode As mapNode In mapNode.Nodes

                    If CType(domainMapNode, Object).GetType() Is GetType(domainMapNode) Then

                        If domainMapNode.GetMap Is domainMap Then

                            Return domainMapNode

                        End If

                    End If

                Next

            End If

        Next

        Return Nothing

    End Function

    Public Function ExpandToClassMap(ByVal classMap As IClassMap) As ClassMapNode

        If classMap Is Nothing Then Return Nothing

        Dim domainMapNode As DomainMapNode = ExpandToDomainMap(classMap.DomainMap)

        If domainMapNode Is Nothing Then Return Nothing

        domainMapNode.Expand()

        For Each mapNode As mapNode In domainMapNode.Nodes

            If CType(mapNode, Object).GetType() Is GetType(ClassListMapNode) Then

                mapNode.Expand()

                For Each classMapNode As mapNode In mapNode.Nodes

                    If CType(classMapNode, Object).GetType() Is GetType(classMapNode) Then

                        If classMapNode.GetMap Is classMap Then

                            Return classMapNode

                        End If

                    End If

                Next

            End If

        Next

        Return Nothing

    End Function

    Public Function ExpandToPropertyMap(ByVal propertyMap As IPropertyMap) As PropertyMapNode

        If propertyMap Is Nothing Then Return Nothing

        Dim classMapNode As ClassMapNode = ExpandToClassMap(propertyMap.ClassMap)

        If classMapNode Is Nothing Then Return Nothing

        classMapNode.Expand()

        For Each mapNode As mapNode In classMapNode.Nodes

            If CType(mapNode, Object).GetType() Is GetType(PropertyMapNode) Then

                If mapNode.GetMap Is propertyMap Then

                    Return mapNode

                End If

            End If

        Next

        Return Nothing

    End Function


    Public Function ExpandToSourceMap(ByVal sourceMap As ISourceMap) As SourceMapNode

        If SourceMap Is Nothing Then Return Nothing

        Dim domainMapNode As domainMapNode = ExpandToDomainMap(SourceMap.DomainMap)

        If domainMapNode Is Nothing Then Return Nothing

        domainMapNode.Expand()

        For Each mapNode As mapNode In domainMapNode.Nodes

            If CType(mapNode, Object).GetType() Is GetType(SourceListMapNode) Then

                mapNode.Expand()

                For Each sourceMapNode As mapNode In mapNode.Nodes

                    If CType(sourceMapNode, Object).GetType() Is GetType(sourceMapNode) Then

                        If sourceMapNode.GetMap Is SourceMap Then

                            Return sourceMapNode

                        End If

                    End If

                Next

            End If

        Next

        Return Nothing

    End Function


    Public Function ExpandToTableMap(ByVal tableMap As ITableMap) As TableMapNode

        If TableMap Is Nothing Then Return Nothing

        Dim sourceMapNode As sourceMapNode = ExpandToSourceMap(TableMap.SourceMap)

        If sourceMapNode Is Nothing Then Return Nothing

        sourceMapNode.Expand()

        For Each mapNode As mapNode In sourceMapNode.Nodes

            If CType(mapNode, Object).GetType() Is GetType(TableMapNode) Then

                If mapNode.GetMap Is TableMap Then

                    Return mapNode

                End If

            End If

        Next

        Return Nothing

    End Function


    Public Function ExpandToColumnMap(ByVal columnMap As IColumnMap) As ColumnMapNode

        If ColumnMap Is Nothing Then Return Nothing

        Dim tableMapNode As tableMapNode = ExpandToTableMap(columnMap.TableMap)

        If tableMapNode Is Nothing Then Return Nothing

        tableMapNode.Expand()

        For Each mapNode As mapNode In tableMapNode.Nodes

            If CType(mapNode, Object).GetType() Is GetType(ColumnMapNode) Then

                If mapNode.GetMap Is columnMap Then

                    Return mapNode

                End If

            End If

        Next

        Return Nothing

    End Function


End Class
