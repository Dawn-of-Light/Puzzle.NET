Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel


Public Class SourceCodeFileItem
    Inherits MapListItem

    Private m_Map As SourceCodeFile
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As SourceCodeFile, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        m_Map = map

        m_DomainMap = domainMap


        Me.Text = m_Map.Name

        Me.SubItems.Add("")


    End Sub


    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property



    Public Overrides Sub Refresh()

        Me.Text = m_Map.Name

        SetIcon()

    End Sub


    Public Function SetIcon()

        Dim imgIndex As Integer
        Dim selImgIndex As Integer

        Select Case m_Map.FileType

            Case SourceCodeFileTypeEnum.CSharp

                Select Case m_Map.MapObjectType

                    Case GetType(DomainMap).ToString

                        imgIndex = 60

                    Case GetType(ClassMap).ToString

                        imgIndex = 62

                    Case Else

                        imgIndex = 62

                End Select

            Case SourceCodeFileTypeEnum.VB

                Select Case m_Map.MapObjectType

                    Case GetType(DomainMap).ToString

                        imgIndex = 56

                    Case GetType(ClassMap).ToString

                        imgIndex = 58

                        'AssemblyInfo
                    Case Else

                        imgIndex = 58

                End Select

            Case Else


                Select Case m_Map.FileType

                    Case SourceCodeFileTypeEnum.NPersist

                        imgIndex = 100

                    Case SourceCodeFileTypeEnum.Xml

                        imgIndex = 100

                    Case Else

                        imgIndex = 64

                End Select

        End Select

        If Not m_Map.IsSynched Then


            If imgIndex >= 100 Then

                imgIndex += 2

            Else

                imgIndex += 10

            End If

        End If

        If Not Me.ImageIndex = imgIndex Then Me.ImageIndex = imgIndex

    End Function


End Class

