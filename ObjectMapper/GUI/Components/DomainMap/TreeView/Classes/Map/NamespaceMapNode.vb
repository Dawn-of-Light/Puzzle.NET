Imports Puzzle.NPersist.Framework.Mapping

Public Class NamespaceMapNode
    Inherits MapNode

    Private m_Map As DomainMap

    Public Sub New(ByVal domainMap As IDomainMap, ByVal text As String)
        MyBase.New(text)

        SetIcons()

        m_Map = domainMap


    End Sub

    Public Sub New(ByVal text As String)
        MyBase.New(text)

        SetIcons()

        Me.Map = Nothing

    End Sub

    Public Overrides Sub SetIcons()

        Dim imgIndex As Integer = 4
        Dim selImageIndex As Integer = 5

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

        Dim level As Long = GetLevel()
        Dim path As String = GetPath()

        Dim listSorted As ArrayList

        listSorted = CType(GetMap(), IDomainMap).ClassMaps
        listSorted.Sort()

        For Each classMap In listSorted

            arrName = Split(classMap.Name, ".")

            If UBound(arrName) > level Then

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, level) Then

                        If Not hashAdded.ContainsKey(LCase(arrName(level))) Then

                            hashAdded(LCase(arrName(level))) = True

                            namespaceNode = New NamespaceMapNode(arrName(level))

                            'Add empty child node
                            namespaceNode.Nodes.Add(New MapNode)

                            Me.Nodes.Add(namespaceNode)

                            'Set regular font
                            namespaceNode.SetRegularFont()

                        End If

                    End If

                End If

            ElseIf UBound(arrName) < level Then

            Else    ' = level

                arrName = Split(classMap.Name, ".")

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, -1) Then

                        classNode = New ClassMapNode(classMap)

                        'Add empty child node
                        classNode.Nodes.Add(New MapNode)

                        Me.Nodes.Add(classNode)

                        'Set regular font
                        classNode.SetRegularFont()

                    End If

                End If

            End If

        Next

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If resetIcons OrElse Not (Me.ImageIndex > 19 And Me.ImageIndex < 28) Then
            SetIcons()
        End If

        If Not Me.IsExpanded Then Exit Sub


        Dim classMap As IClassMap
        Dim classNode As ClassMapNode
        Dim namespaceNode As NamespaceMapNode

        Dim arrName() As String
        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashAdded As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        Dim level As Long = GetLevel()
        Dim path As String = GetPath()

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
            hashAdded(LCase(childNode.Text)) = True
        Next

        Dim listSorted As ArrayList

        listSorted = CType(GetMap(), IDomainMap).ClassMaps
        listSorted.Sort()

        For Each classMap In listSorted

            arrName = Split(classMap.Name, ".")

            If UBound(arrName) > level Then

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, level) Then

                        hashFound(LCase(arrName(level))) = True

                        If Not hashAdded.ContainsKey(LCase(arrName(level))) Then

                            hashAdded(LCase(arrName(level))) = True

                            namespaceNode = New NamespaceMapNode(arrName(level))

                            'Add empty child node
                            namespaceNode.Nodes.Add(New MapNode)

                            Me.Nodes.Add(namespaceNode)

                            'Set regular font
                            namespaceNode.SetRegularFont()

                        End If

                    End If

                End If

            ElseIf UBound(arrName) < level Then

            Else    ' = level

                arrName = Split(classMap.Name, ".")

                If Len(classMap.Name) > Len(path) Then

                    If ComparePaths(classMap.Name, path, -1) Then

                        hashFound(LCase(arrName(UBound(arrName)))) = True

                        If Not hashAdded.ContainsKey(LCase(arrName(level))) Then

                            hashAdded(LCase(arrName(UBound(arrName)))) = True

                            classNode = New ClassMapNode(classMap)

                            'Add empty child node
                            classNode.Nodes.Add(New MapNode)

                            Me.Nodes.Add(classNode)

                            'Set regular font
                            classNode.SetRegularFont()

                        End If

                    End If

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
        Return CType(Me.Parent, MapNode).GetMap
    End Function

    Public Function GetLevel() As Long

        Dim level As Long = 1
        Dim p As MapNode = Parent

        While Not p.GetType Is GetType(ClassListMapNode)
            p = p.Parent
            level += 1
        End While

        Return level

    End Function

    Public Function GetPath() As String

        Dim path As String = Me.Text
        Dim p As MapNode = Parent

        While Not p.GetType Is GetType(ClassListMapNode)
            path = p.Text & "." & path
            p = p.Parent
        End While

        Return path

    End Function

    Public Function ComparePaths(ByVal path1 As String, ByVal path2 As String, ByVal level As Long) As Boolean

        path1 = LCase(path1)
        path2 = LCase(path2)

        Dim i As Long

        Dim arrPath1() As String = Split(path1, ".")
        Dim arrPath2() As String = Split(path2, ".")

        If level = -1 Then level = UBound(arrPath1)

        For i = 0 To level - 1
            If Not arrPath1(i) = arrPath2(i) Then Return False
        Next

        Return True

    End Function
End Class