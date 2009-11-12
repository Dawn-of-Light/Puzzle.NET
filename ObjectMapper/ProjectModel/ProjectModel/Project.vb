Imports System.Xml.Serialization
Imports System.IO
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.Uml

Namespace ProjectModel

    Public Class Project
        Inherits MapBase
        Implements IProject


        Private m_Resources As New ArrayList
        Private m_FilePath As String

        Public Shared Function Load(ByVal path As String) As IProject

            Dim proj As Project

            Try

                Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(Project))
                ' To read the file, create a FileStream object.
                Dim myFileStream As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read)

                proj = CType( _
                mySerializer.Deserialize(myFileStream), Project)

                myFileStream.Close()

            Catch ex As Exception

                Throw New Exception("Could not load project from path: '" & path & "': " & ex.Message, ex)

            End Try

            proj.Setup()

            Return proj

        End Function


        Public Sub Setup() Implements IProject.Setup

            Dim resource As IResource

            For Each resource In Me.Resources

                resource.Setup()

            Next

        End Sub

        Public Overridable Sub Save(ByVal path As String) Implements IProject.Save

            Try

                SetHintPaths(path)

            Catch ex As Exception

            End Try

            Try

                Dim mySerializer As XmlSerializer = New XmlSerializer(Me.GetType)
                ' To write to a file, create a StreamWriter object.
                Dim myWriter As StreamWriter = New StreamWriter(path)
                mySerializer.Serialize(myWriter, Me)
                myWriter.Close()

            Catch ex As Exception

                Throw New Exception("Could not serialize project to path: '" & path & "': " & ex.Message, ex)

            End Try

        End Sub

        <XmlArrayItem(GetType(Resource))> Public Overridable Property Resources() As System.Collections.ArrayList Implements IProject.Resources
            Get
                Return m_Resources
            End Get
            Set(ByVal Value As System.Collections.ArrayList)
                m_Resources = Value
            End Set
        End Property


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String) Implements IProject.UpdateMapObjectName

            Dim res As IResource
            Dim cfg As DomainConfig

            For Each res In m_Resources

                res.UpdateMapObjectName(mapObject, newName)

            Next

        End Sub


        Public Function GetDomainMaps() As IList Implements IProject.GetDomainMaps

            Dim resource As IResource
            Dim domainMap As IDomainMap
            Dim result As IList = New ArrayList

            For Each resource In m_Resources

                If resource.ResourceType = ResourceTypeEnum.XmlDomainMap Then

                    domainMap = resource.GetResource

                    If Not domainMap Is Nothing Then

                        result.Add(domainMap)

                    End If

                End If

            Next

            Return result

        End Function

        Public Function GetResources(ByVal resourceType As ResourceTypeEnum) As IList Implements IProject.GetResources

            Dim resource As IResource
            Dim result As IList = New ArrayList

            For Each resource In m_Resources

                If resource.ResourceType = resourceType Then

                    result.Add(resource)

                End If

            Next

            Return result

        End Function


        Public Function GetResource(ByVal name As String, ByVal resourceType As ResourceTypeEnum) As IResource Implements IProject.GetResource

            Dim resource As IResource

            For Each resource In m_Resources

                If resource.ResourceType = resourceType Then

                    If resource.Name = name Then

                        Return resource

                    End If

                End If

            Next

        End Function

        Public Function GetDomainMap(ByVal name As String) As IDomainMap Implements IProject.GetDomainMap

            Dim resource As IResource = GetResource(name, ResourceTypeEnum.XmlDomainMap)

            If Not resource Is Nothing Then

                Return resource.GetResource

            End If

        End Function

        <XmlIgnore()> Public Property FilePath() As String Implements IProject.FilePath
            Get
                Return m_FilePath
            End Get
            Set(ByVal Value As String)
                m_FilePath = Value
            End Set
        End Property


        Public Function GetDiagram(ByVal domainMap As IDomainMap, ByVal name As String) As Uml.UmlDiagram Implements IProject.GetDiagram

            Dim resource As IResource = GetResource(domainMap.Name, ResourceTypeEnum.XmlDomainMap)

            Return resource.GetDiagram(name)

        End Function


        Public Sub DeletingClassMap(ByVal classMap As IClassMap) Implements IProject.DeletingClassMap

            Dim resource As IResource = GetResource(classMap.DomainMap.Name, ResourceTypeEnum.XmlDomainMap)
            Dim umlDiagram As umlDiagram
            Dim umlClass As umlClass
            Dim checkClassMap As IClassMap
            Dim remove As New ArrayList

            If Not resource Is Nothing Then

                For Each umlDiagram In resource.Diagrams

                    remove.Clear()

                    For Each umlClass In umlDiagram.UmlClasses

                        checkClassMap = umlClass.GetClassMap

                        If Not checkClassMap Is Nothing Then

                            If checkClassMap Is classMap Then

                                remove.Add(umlClass)

                            End If

                        End If

                    Next

                    For Each umlClass In remove

                        umlClass.Remove()

                    Next

                Next

            End If

        End Sub


        Public Overridable Function GetFilePathRelativeToProject(ByVal projPath As String, ByVal filePath As String) As String Implements IProject.GetFilePathRelativeToProject

            Dim arrProj() As String = Split(projPath, "\")
            Dim arrFile() As String = Split(filePath, "\")
            Dim path As String

            Dim i As Integer
            Dim str As String
            Dim str2 As String

            If UBound(arrFile) > UBound(arrProj) Then

                For i = 0 To UBound(arrProj)

                    If LCase(arrFile(i)) = LCase(arrProj(i)) Then

                    Else

                        path = "..\" & path & arrFile(i) & "\"

                    End If

                Next

                For i = UBound(arrProj) + 1 To UBound(arrFile)

                    If i > UBound(arrProj) + 1 Then

                        path += "\"

                    End If

                    path += arrFile(i)

                Next

            Else

                For i = 0 To UBound(arrFile)

                    If LCase(arrFile(i)) = LCase(arrProj(i)) Then

                    Else

                        path = "..\" & path & arrFile(i)

                        If i < UBound(arrFile) Then

                            path += "\"

                        End If

                    End If

                Next

                For i = UBound(arrFile) + 1 To UBound(arrProj)

                    If i > UBound(arrFile) + 1 Then

                        path = "..\" & path

                    End If

                Next

            End If

            Return path

        End Function

        Public Sub SetHintPaths(ByVal projectPath As String) Implements IProject.SetHintPaths

            Dim resource As IResource

            For Each resource In m_Resources

                SetHintPath(resource, projectPath)

            Next

        End Sub

        Protected Sub SetHintPath(ByVal resource As IResource, ByVal projectPath As String)

            Dim arr() As String

            If Len(projectPath) < 1 Then

                projectPath = m_FilePath

            End If

            If Len(projectPath) > 0 Then

                arr = Split(projectPath, "\")

                ReDim Preserve arr(UBound(arr) - 1)

                projectPath = Join(arr, "\")

                If Len(resource.FilePath) > 0 Then



                    resource.HintPath = GetFilePathRelativeToProject(projectPath, resource.FilePath)

                End If

            End If

        End Sub

        Public Overrides Function Clone() As IMap

        End Function

        Public Overrides Function DeepClone() As IMap

        End Function

        Public Overrides Function Compare(ByVal compareTo As IMap) As Boolean

        End Function

        Public Overrides Function DeepCompare(ByVal compareTo As IMap) As Boolean

        End Function

        Public Overrides Sub DeepMerge(ByVal mapObject As IMap)

        End Sub


        Public Overrides Sub Copy(ByVal mapObject As IMap)

        End Sub

        Public Overrides Sub DeepCopy(ByVal mapObject As IMap)

        End Sub

        Public Overrides Function GetKey() As String

        End Function



        Public Overrides Sub Accept(ByVal mapVisitor As Puzzle.NPersist.Framework.Mapping.Visitor.IMapVisitor)

        End Sub
    End Class

End Namespace
