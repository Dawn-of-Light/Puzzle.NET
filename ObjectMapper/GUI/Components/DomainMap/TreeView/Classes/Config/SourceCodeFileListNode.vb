Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class SourceCodeFileListNode
    Inherits MapNode

    Private m_Map As ClassesToCodeConfig
    Private m_DomainMap As IDomainMap

    Public Sub New()
        MyBase.New()

        Me.Text = "Files"

        Me.ImageIndex = 54
        Me.SelectedImageIndex = 55

    End Sub

    Public Sub New(ByVal map As ClassesToCodeConfig, ByVal domainMap As IDomainMap)
        MyBase.New()

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = "Files"

        Me.ImageIndex = 54
        Me.SelectedImageIndex = 55

    End Sub


    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

    Public Overrides Sub OnExpand()

        Dim config As ClassesToCodeConfig
        Dim newNode As SourceCodeFileNode
        Dim src As SourceCodeFile

        Dim listSorted As ArrayList

        'Clear the dummy child node
        Me.Nodes.Clear()

        config = CType(CType(Me.Parent, ClassesToCodeConfigNode).Map, ClassesToCodeConfig)

        listSorted = config.SourceCodeFiles
        listSorted.Sort()

        For Each src In listSorted

            'Add the node for the list of data sources
            newNode = New SourceCodeFileNode(src)

            'Add empty child node
            'newNode.Nodes.Add(New MapNode)

            Me.Nodes.Add(newNode)

            'Set regular font
            newNode.SetRegularFont()


        Next

    End Sub

    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        Dim config As ClassesToCodeConfig

        config = CType(CType(Me.Parent, ClassesToCodeConfigNode).Map, ClassesToCodeConfig)

        If Not Me.IsExpanded Then

            If Me.FirstNode Is Nothing Then

                If config.SourceCodeFiles.Count > 0 Then

                    'Add empty child node
                    Me.Nodes.Add(New MapNode)

                End If

            End If

            Exit Sub

        End If

        Dim newNode As SourceCodeFileNode
        Dim src As SourceCodeFile

        Dim childNode As MapNode
        Dim hashOrg As New Hashtable
        Dim hashFound As New Hashtable
        Dim key As String

        For Each childNode In Me.Nodes
            hashOrg(LCase(childNode.Text)) = childNode
        Next

        Dim listSorted As ArrayList

        listSorted = config.SourceCodeFiles
        listSorted.Sort()

        For Each src In listSorted

            If Not hashFound.ContainsKey(LCase(src.Name)) Then

                hashFound(LCase(src.Name)) = True

                If Not hashOrg.ContainsKey(LCase(src.Name)) Then

                    newNode = New SourceCodeFileNode(src)

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


    Public Overrides Function GetMap() As Object
        If Not m_Map Is Nothing Then Return m_Map
        Return CType(Me.Parent, ClassesToCodeConfigNode).Map
    End Function

End Class
