Imports Puzzle.NPersist.Framework.Mapping

Public Class ClassListMapNode
    Inherits MapNode

    Private m_Map As DomainMap

    Public Sub New(ByVal domainMap As IDomainMap)
        MyBase.New("Domain")

        m_Map = domainMap

        SetIcons()

    End Sub

    Public Sub New()
        MyBase.New("Domain")

        SetIcons()

    End Sub


    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 2
        Dim selImageIndex As Integer = 3

        ApplyErrorIcons(imgIndex, selImageIndex)

    End Sub

    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        Dim classMap As IClassMap
        Dim classNode As ClassMapNode
        Dim namespaceNode As NamespaceMapNode

        Dim arrName() As String
        Dim hashAdded As New Hashtable

        Dim listSorted As ArrayList

        listSorted = CType(GetMap(), IDomainMap).ClassMaps.Clone
        listSorted.Sort()

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings

        If envSettings.ShowNamespaceNodes Then

            For Each classMap In listSorted

                arrName = Split(classMap.Name, ".")

                If UBound(arrName) > 0 Then

                    If Not hashAdded.ContainsKey(LCase(arrName(0))) Then

                        hashAdded(LCase(arrName(0))) = True

                        namespaceNode = New NamespaceMapNode(arrName(0))

                        'Add empty child node
                        namespaceNode.Nodes.Add(New MapNode)

                        Me.Nodes.Add(namespaceNode)

                        'Set regular font
                        namespaceNode.SetRegularFont()

                    End If

                Else

                    classNode = New ClassMapNode(classMap)

                    'Add empty child node
                    classNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(classNode)

                    'Set regular font
                    classNode.SetRegularFont()

                End If

            Next


        Else

            For Each classMap In listSorted

                classNode = New ClassMapNode(classMap)

                'Add empty child node
                classNode.Nodes.Add(New MapNode)

                Me.Nodes.Add(classNode)

                'Set regular font
                classNode.SetRegularFont()

            Next

        End If

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then

            SetIcons()

        End If


        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If CType(GetMap(), IDomainMap).ClassMaps.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim classMap As IClassMap
        Dim classNode As ClassMapNode
        Dim namespaceNode As NamespaceMapNode

        Dim arrName() As String
        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashAdded As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        Dim listSorted As ArrayList

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings

        If envSettings.ShowNamespaceNodes Then

            For Each childNode In Me.Nodes
                hashOrg(LCase(childNode.Text)) = childNode
                hashAdded(LCase(childNode.Text)) = True
            Next

        Else

            For Each childNode In Me.Nodes
                hashOrg(LCase(childNode.Map.Name)) = childNode
                hashAdded(LCase(childNode.Map.Name)) = True
            Next

        End If

        listSorted = CType(GetMap(), IDomainMap).ClassMaps
        listSorted.Sort()

        If envSettings.ShowNamespaceNodes Then

            For Each classMap In listSorted

                arrName = Split(classMap.Name, ".")

                hashFound(LCase(arrName(0))) = True

                If Not hashAdded.ContainsKey(LCase(arrName(0))) Then

                    hashAdded(LCase(arrName(0))) = True

                    If UBound(arrName) > 0 Then

                        namespaceNode = New NamespaceMapNode(arrName(0))

                        'Add empty child node
                        namespaceNode.Nodes.Add(New MapNode)

                        Me.Nodes.Add(namespaceNode)

                        'Set regular font
                        namespaceNode.SetRegularFont()

                    Else

                        classNode = New ClassMapNode(classMap)

                        'Add empty child node
                        classNode.Nodes.Add(New MapNode)

                        Me.Nodes.Add(classNode)

                        'Set regular font
                        classNode.SetRegularFont()

                    End If

                End If

            Next

        Else

            For Each classMap In listSorted

                hashFound(LCase(classMap.Name)) = True

                If Not hashAdded.ContainsKey(LCase(classMap.Name)) Then

                    hashAdded(LCase(classMap.Name)) = True

                    classNode = New ClassMapNode(classMap)

                    'Add empty child node
                    classNode.Nodes.Add(New MapNode)

                    Me.Nodes.Add(classNode)

                    'Set regular font
                    classNode.SetRegularFont()

                End If

            Next

        End If

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