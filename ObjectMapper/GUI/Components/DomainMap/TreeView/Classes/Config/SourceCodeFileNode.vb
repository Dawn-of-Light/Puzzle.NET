Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class SourceCodeFileNode
    Inherits MapNode

    Private m_Map As SourceCodeFile
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As SourceCodeFile)
        MyBase.New(map)

        m_Map = map

        Me.Text = m_Map.GetName()

        SetIcon()

    End Sub


    Public Sub New(ByVal map As SourceCodeFile, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap

        Me.Text = m_Map.GetName()

        SetIcon()

    End Sub

    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

    Public Function SetIcon()

        Dim imgIndex As Integer
        Dim selImgIndex As Integer

        Select Case m_Map.FileType

            Case SourceCodeFileTypeEnum.CSharp

                Select Case m_Map.MapObjectType

                    Case GetType(DomainMap).ToString

                        imgIndex = 60
                        selImgIndex = 61

                    Case GetType(ClassMap).ToString

                        imgIndex = 62
                        selImgIndex = 63

                    Case Else

                        imgIndex = 62
                        selImgIndex = 63

                End Select

            Case SourceCodeFileTypeEnum.VB

                Select Case m_Map.MapObjectType

                    Case GetType(DomainMap).ToString

                        imgIndex = 56
                        selImgIndex = 57

                    Case GetType(ClassMap).ToString

                        imgIndex = 58
                        selImgIndex = 59

                        'AssemblyInfo
                    Case Else

                        imgIndex = 58
                        selImgIndex = 59

                End Select

            Case Else


                Select Case m_Map.FileType

                    Case SourceCodeFileTypeEnum.NPersist

                        imgIndex = 100
                        selImgIndex = 101

                    Case SourceCodeFileTypeEnum.Xml

                        imgIndex = 100
                        selImgIndex = 101

                    Case Else

                        imgIndex = 64
                        selImgIndex = 65

                End Select

        End Select

        If Not m_Map.IsSynched Then

            If imgIndex >= 100 Then

                imgIndex += 2
                selImgIndex += 2

            Else

                imgIndex += 10
                selImgIndex += 10

            End If

        End If

        If Not Me.ImageIndex = imgIndex Then Me.ImageIndex = imgIndex
        If Not Me.SelectedImageIndex = selImgIndex Then Me.SelectedImageIndex = selImgIndex

    End Function



    Public Overrides Sub OnExpand()

        'Clear the dummy child node
        Me.Nodes.Clear()

        'Dim newNode As MapNode

        ''Add the node for the list of classes
        'newNode = New SourceCodeFileListNode

        ''Add empty child node
        'newNode.Nodes.Add(New MapNode)

        'Me.Nodes.Add(newNode)

    End Sub


    Public Overrides Sub Refresh(ByVal resetIcons As Boolean)

        If Not Me.Text = m_Map.GetName() Then Me.Text = m_Map.GetName()

        SetIcon()

    End Sub

End Class
