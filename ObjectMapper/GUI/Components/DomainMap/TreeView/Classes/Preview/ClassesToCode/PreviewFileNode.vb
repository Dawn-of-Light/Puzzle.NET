Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class PreviewFileNode
    Inherits MapNode

    Private m_domainMap As IDomainMap
    Private m_classMap As IClassMap
    Private m_TagName As String

    Private m_src As SourceCodeFile

    Private m_FileType As SourceCodeFileTypeEnum

    Public Property DomainMap() As IDomainMap
        Get
            Return m_domainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_domainMap = Value
        End Set
    End Property

    Public Property ClassMap() As IClassMap
        Get
            Return m_classMap
        End Get
        Set(ByVal Value As IClassMap)
            m_classMap = Value
        End Set
    End Property

    Public Property TagName()
        Get
            Return m_TagName
        End Get
        Set(ByVal Value)
            m_TagName = Value
        End Set
    End Property

    Public Property FileType() As SourceCodeFileTypeEnum
        Get
            Return m_FileType
        End Get
        Set(ByVal Value As SourceCodeFileTypeEnum)
            m_FileType = Value
        End Set
    End Property

    Public Property Src() As SourceCodeFile
        Get
            Return m_src
        End Get
        Set(ByVal Value As SourceCodeFile)
            m_src = Value
        End Set
    End Property

    Public Overloads Sub Refresh()

        Refresh(True)

    End Sub

    Public Overloads Overrides Sub Refresh(ByVal resetIcons As Boolean)

        SetIcon()


    End Sub

    Public Function SetIcon()

        Dim imgIndex As Integer
        Dim selImgIndex As Integer

        Select Case FileType

            Case SourceCodeFileTypeEnum.CSharp

                If Not DomainMap Is Nothing Then

                    imgIndex = 60
                    selImgIndex = 61

                ElseIf Not ClassMap Is Nothing Then

                    imgIndex = 62
                    selImgIndex = 63

                Else

                    imgIndex = 62
                    selImgIndex = 63

                End If

            Case SourceCodeFileTypeEnum.VB

                If Not DomainMap Is Nothing Then

                    imgIndex = 56
                    selImgIndex = 57

                ElseIf Not ClassMap Is Nothing Then

                    imgIndex = 58
                    selImgIndex = 59

                    'AssemblyInfo
                Else

                    imgIndex = 58
                    selImgIndex = 59

                End If

            Case Else

                Select Case m_FileType

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

        If Not Src Is Nothing Then

            If LCase(Src.FilePath) = LCase(Me.Parent.Text & "\" & Me.Text) Then

                If Not System.IO.File.Exists(Src.FilePath) Then

                Else

                    If Not Src.IsSynched Then

                        If imgIndex >= 100 Then

                            imgIndex += 2
                            selImgIndex += 2

                        Else

                            imgIndex += 10
                            selImgIndex += 10

                        End If

                    End If

                End If

            Else


            End If

        Else

            If System.IO.File.Exists(Me.Parent.Text & "\" & Me.Text) Then

                If imgIndex >= 100 Then

                    imgIndex += 2
                    selImgIndex += 2

                Else

                    imgIndex += 10
                    selImgIndex += 10

                End If

            End If

        End If

        If Not Me.ImageIndex = imgIndex Then Me.ImageIndex = imgIndex
        If Not Me.SelectedImageIndex = selImgIndex Then Me.SelectedImageIndex = selImgIndex

    End Function


    Public Function GetFileName() As String

        Dim name As String

        If Not Me.Parent Is Nothing Then

            If Me.FileType = SourceCodeFileTypeEnum.Other Then

                name = Me.Parent.Text & "\bin\" & Me.Text

            Else

                name = Me.Parent.Text & "\" & Me.Text

            End If

        End If

        Return name

    End Function


End Class
