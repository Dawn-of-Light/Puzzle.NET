Imports Puzzle.NPersist.Framework.Mapping

Public Class ProjectNode
    Inherits MapNode

    Private m_Map As ProjectModel.IProject

    Public Sub New(ByVal map As ProjectModel.IProject)
        MyBase.New(map)

        m_Map = map

        Me.Text = m_Map.Name

        SetIcons()

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 46
        Dim selImageIndex As Integer = 47

        ApplyErrorIcons(imgIndex, selImageIndex)

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim domainMap As IDomainMap
        Dim newNode As DomainMapNode

        Dim listSorted As New ArrayList
        Dim resource As ProjectModel.IResource

        Dim added As Boolean

        For Each resource In m_Map.Resources

            If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                If Not resource.GetResource Is Nothing Then

                    listSorted.Add(resource.GetResource)

                End If

            End If

        Next

        listSorted.Sort()

        For Each domainMap In listSorted

            newNode = New DomainMapNode(domainMap)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()

            added = True

        Next

        If Not added Then

            Me.Nodes.Add(New MapNode)

        End If

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.Name Then Me.Text = m_Map.Name

        Dim domainMap As IDomainMap
        Dim newNode As DomainMapNode

        Dim listSorted As New ArrayList
        Dim resource As ProjectModel.IResource

        If resetIcons Then
            SetIcons()
        End If

        For Each resource In m_Map.Resources

            If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                If Not resource.GetResource Is Nothing Then

                    listSorted.Add(resource.GetResource)

                End If

            End If

        Next

        listSorted.Sort()

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If listSorted.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        For Each domainMap In listSorted

            If Len(domainMap.Name) > 0 Then

                hashFound(LCase(domainMap.Name)) = True

                If Not hashOrg.ContainsKey(LCase(domainMap.Name)) Then

                    newNode = New DomainMapNode(domainMap)

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
