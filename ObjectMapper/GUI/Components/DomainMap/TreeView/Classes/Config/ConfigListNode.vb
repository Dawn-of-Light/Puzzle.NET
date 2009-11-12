Imports Puzzle.NPersist.Framework.Mapping

Public Class ConfigListNode
    Inherits MapNode

    Private m_Map As DomainMap

    Public Sub New(ByVal domainMap As IDomainMap)
        MyBase.New("Synch Configs")

        Me.ImageIndex = 37
        Me.SelectedImageIndex = 37

        m_Map = domainMap


    End Sub

    Public Sub New()
        MyBase.New()

        Me.Text = "Synch Configs"

        Me.ImageIndex = 37
        Me.SelectedImageIndex = 37

    End Sub

    Public Overrides Sub OnExpand()

        Dim resource As ProjectModel.IResource
        Dim config As ProjectModel.DomainConfig
        Dim newNode As ConfigNode
        Dim listSorted As ArrayList

        'Clear the dummy child node
        Me.Nodes.Clear()

        resource = CType(CType(Me.Parent.Parent, ProjectNode).Map, ProjectModel.IProject).GetResource(GetMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        listSorted = resource.Configs
        listSorted.Sort()

        For Each config In listSorted

            'Add the node for the list of data sources
            newNode = New ConfigNode(config)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            CType(newNode, ConfigNode).SetNodeStyle()

        Next

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Dim resource As ProjectModel.IResource

        resource = CType(CType(Me.Parent.Parent, ProjectNode).Map, ProjectModel.IProject).GetResource(GetMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        If resource Is Nothing Then Exit Sub

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If resource.Configs.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim config As ProjectModel.DomainConfig
        Dim newNode As ConfigNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        Dim listSorted As ArrayList

        listSorted = resource.Configs
        listSorted.Sort()

        For Each config In listSorted

            If Not hashFound.ContainsKey(LCase(config.Name)) Then

                hashFound(LCase(config.Name)) = True

                If Not hashOrg.ContainsKey(LCase(config.Name)) Then

                    newNode = New ConfigNode(config)

                    Me.Nodes.Add(newNode)

                    'Add empty child node
                    newNode.Nodes.Add(New MapNode)

                    'Set font and icon
                    CType(newNode, ConfigNode).SetNodeStyle()

                End If

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
        Return CType(Me.Parent, DomainMapNode).Map
    End Function

End Class
