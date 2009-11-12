Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel

Public Class ProjectProperties
    Inherits PropertiesBase

    Private m_Project As IProject

    Public Event BeforePropertySet(ByVal mapObject As IProject, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As IProject, ByVal propertyName As String)

    Public Function GetProject() As IProject
        Return m_Project
    End Function

    Public Sub SetProject(ByVal value As IProject)
        m_Project = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_Project

    End Function

    <Category("Design"), _
        Description("The name of this project."), _
        DisplayName("Name"), _
        DefaultValue("")> Public Property Name() As String
        Get
            Return m_Project.Name
        End Get
        Set(ByVal Value As String)
            If Value <> m_Project.Name Then
                m_Project.Name = Value
                RaiseEvent AfterPropertySet(m_Project, "Name")
            End If
        End Set
    End Property

    <Category("File"), _
        Description("The name of the file contaning the information about the project."), _
        DisplayName("File path"), _
        DefaultValue("")> Public ReadOnly Property FilePath() As String
        Get
            Return m_Project.FilePath
        End Get
    End Property


End Class
