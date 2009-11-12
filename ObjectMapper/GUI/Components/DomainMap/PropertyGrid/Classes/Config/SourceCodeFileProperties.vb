Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel
Imports System.ComponentModel
Imports System.IO

Public Class SourceCodeFileProperties
    Inherits PropertiesBase

    Private m_SourceCodeFile As SourceCodeFile

    Public Event BeforePropertySet(ByVal mapObject As DomainConfig, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As DomainConfig, ByVal propertyName As String)

    Public Function GetSourceCodeFile() As SourceCodeFile
        Return m_SourceCodeFile
    End Function

    Public Sub SetSourceCodeFile(ByVal value As SourceCodeFile)
        m_SourceCodeFile = value
    End Sub

    Public Overrides Function GetMapObject() As IMap

        Return m_SourceCodeFile

    End Function


    <Category("Design"), _
        Description("The physical path to this file."), _
        DisplayName("Path"), _
        DefaultValue("")> Public ReadOnly Property FilePath() As String
        Get
            Return m_SourceCodeFile.FilePath
        End Get
    End Property

    <Category("Design"), _
        Description("The time this file was last written to by this application."), _
        DisplayName("Last updated by application"), _
        DefaultValue("")> Public ReadOnly Property LastWrittenTo() As DateTime
        Get
            Return m_SourceCodeFile.LastWrittenTo
        End Get
    End Property

    <Category("Synchronization"), _
    Description("Indicates if the physical file has been modified since it was last written to by this application."), _
    DisplayName("Synchronized"), _
    DefaultValue(True)> Public ReadOnly Property IsSynched() As Boolean
        Get
            Return m_SourceCodeFile.IsSynched
        End Get
    End Property

    <Category("File"), _
    Description("Indicates if the physical file still exists."), _
    DisplayName("Exists"), _
    DefaultValue(True)> Public ReadOnly Property FileExists() As Boolean
        Get
            If File.Exists(m_SourceCodeFile.FilePath) Then
                Return True
            End If
        End Get
    End Property

    <Category("File"), _
        Description("The time this file was last written to."), _
        DisplayName("Last updated"), _
        DefaultValue("")> Public ReadOnly Property LastWrittenToReal() As DateTime
        Get
            If File.Exists(m_SourceCodeFile.FilePath) Then
                Return File.GetLastWriteTime(m_SourceCodeFile.FilePath)
            End If
        End Get
    End Property

    <Category("Design"), _
        Description("The type of this file."), _
        DisplayName("Type"), _
        DefaultValue("")> Public ReadOnly Property FileType() As String
        Get
            Select Case m_SourceCodeFile.FileType
                Case SourceCodeFileTypeEnum.CSharp
                    Select Case m_SourceCodeFile.MapObjectType
                        Case GetType(DomainMap).ToString
                            Return "C# Project"
                        Case GetType(ClassMap).ToString
                            Return "C# Class"
                        Case Else
                            Return "C# Class"
                    End Select
                Case SourceCodeFileTypeEnum.VB
                    Select Case m_SourceCodeFile.MapObjectType
                        Case GetType(DomainMap).ToString
                            Return "VB Project"
                        Case GetType(ClassMap).ToString
                            Return "VB Class"
                        Case Else
                            Return "VB Class"
                    End Select
                Case SourceCodeFileTypeEnum.Delphi
                    Select Case m_SourceCodeFile.MapObjectType
                        Case GetType(DomainMap).ToString
                            Return "Delphi Project"
                        Case GetType(ClassMap).ToString
                            Return "Delphi Class"
                        Case Else
                            Return "Delphi Class"
                    End Select
                Case Else
                    Return "Assembly"
            End Select
        End Get
    End Property

    <Category("Design"), _
        Description("The object in the domain model that this file is mapping to."), _
        DisplayName("Maps to model object"), _
        DefaultValue("")> Public ReadOnly Property MapObject() As String
        Get
            Select Case m_SourceCodeFile.MapObjectType
                Case GetType(DomainMap).ToString
                    Return "Domain " & m_SourceCodeFile.MapObjectName
                Case GetType(ClassMap).ToString
                    Return "Class " & m_SourceCodeFile.MapObjectName
            End Select
            Return "(None)"
        End Get
    End Property

    <Category("Design"), _
        Description("A description of this file."), _
        DisplayName("Description"), _
        DefaultValue("")> Public ReadOnly Property Description() As String
        Get
            Select Case m_SourceCodeFile.MapObjectType
                Case GetType(DomainMap).ToString
                    Return "Microsoft Visual Studio.NET project file for domain " & m_SourceCodeFile.MapObjectName
                Case GetType(ClassMap).ToString
                    Return "Source code file for class " & m_SourceCodeFile.MapObjectName
                Case ""
                    Select Case m_SourceCodeFile.MapObjectName
                        Case "Puzzle.NPersist.Framework.dll"
                            Return "Assembly file containing the NPersist framework."
                            '                            Case "Norm.Specification.dll"
                            '                                Return "Assembly file containing the Norm specification. Norm is a free, open source, vendor independent specification for general interaction with persistent objects."
                        Case "AssemblyInfo"
                            Return "Source code file containing assembly information for the Visual Studio.NET project."
                    End Select
            End Select
            Return "(None)"
        End Get
    End Property

End Class


