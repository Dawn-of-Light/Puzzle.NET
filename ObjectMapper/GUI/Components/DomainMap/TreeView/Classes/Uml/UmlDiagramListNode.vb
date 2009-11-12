Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Public Class UmlDiagramListNode
    Inherits MapNode

    Public Sub New()
        MyBase.New()

        Me.Text = "Uml Diagrams"

        Me.ImageIndex = 76
        Me.SelectedImageIndex = 77

    End Sub

    Public Overrides Sub OnExpand()

        Dim resource As ProjectModel.IResource
        Dim diagram As UmlDiagram
        Dim newNode As UmlDiagramNode
        Dim listSorted As ArrayList

        'Clear the dummy child node
        Me.Nodes.Clear()

        resource = CType(CType(Me.Parent.Parent, ProjectNode).Map, ProjectModel.IProject).GetResource(GetMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        listSorted = resource.Diagrams
        listSorted.Sort()

        For Each diagram In listSorted

            'Add the node for the list of data sources
            newNode = New UmlDiagramNode(diagram)

            'Add empty child node
            newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            newNode.SetRegularFont()

        Next

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Dim resource As ProjectModel.IResource

        resource = CType(CType(Me.Parent.Parent, ProjectNode).Map, ProjectModel.IProject).GetResource(GetMap.Name, ProjectModel.ResourceTypeEnum.XmlDomainMap)

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If resource.Diagrams.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim diagram As UmlDiagram
        Dim newNode As UmlDiagramNode

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        Dim listSorted As ArrayList

        listSorted = resource.Diagrams
        listSorted.Sort()

        For Each diagram In listSorted

            If Not hashFound.ContainsKey(LCase(diagram.Name)) Then

                hashFound(LCase(diagram.Name)) = True

                If Not hashOrg.ContainsKey(LCase(diagram.Name)) Then

                    newNode = New UmlDiagramNode(diagram)

                    Me.Nodes.Add(newNode)

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


    Public Overrides Function GetMap() As Object
        Return CType(Me.Parent, DomainMapNode).Map
    End Function

End Class

