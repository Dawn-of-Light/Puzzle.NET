Imports System.Xml.Serialization
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI
Imports Puzzle.ObjectMapper.GUI.Uml

Namespace ProjectModel

    Public Class Resource
        Implements IResource


        Private m_FilePath As String
        Private m_HintPath As String
        Private m_Name As String
        Private m_ResourceType As ResourceTypeEnum
        Private m_Resource As Object
        Private m_Resources As New ArrayList
        Private m_Configs As New ArrayList
        Private m_Diagrams As New ArrayList

        Private m_OwnerResource As IResource

        <XmlIgnore()> Public Property OwnerResource() As IResource Implements IResource.OwnerResource
            Get
                Return m_OwnerResource
            End Get
            Set(ByVal Value As IResource)
                If Not m_OwnerResource Is Nothing Then
                    m_OwnerResource.Resources.Remove(Me)
                End If
                m_OwnerResource = Value
                If Not m_OwnerResource Is Nothing Then
                    m_OwnerResource.Resources.Add(Me)
                End If
            End Set
        End Property

        Public Overridable Sub SetOwnerResource(ByVal Value As IResource) Implements IResource.SetOwnerResource

            m_OwnerResource = Value

        End Sub

        Public Sub New()
            MyBase.new()

        End Sub

        Public Sub New(ByVal domainMap As IDomainMap)

            m_Resource = domainMap
            m_ResourceType = ResourceTypeEnum.XmlDomainMap

        End Sub

        Public Sub New(ByVal domainMap As IDomainMap, ByVal loadedFromPath As String)

            m_Resource = domainMap
            m_ResourceType = ResourceTypeEnum.XmlDomainMap
            m_FilePath = loadedFromPath

        End Sub

        Public Overridable Property FilePath() As String Implements Puzzle.ObjectMapper.GUI.ProjectModel.IResource.FilePath
            Get
                Return m_FilePath
            End Get
            Set(ByVal Value As String)
                m_FilePath = Value
            End Set
        End Property

        Public Overridable Property HintPath() As String Implements Puzzle.ObjectMapper.GUI.ProjectModel.IResource.HintPath
            Get
                Return m_HintPath
            End Get
            Set(ByVal Value As String)
                m_HintPath = Value
            End Set
        End Property

        Public Overridable Property Name() As String Implements Puzzle.ObjectMapper.GUI.ProjectModel.IResource.Name
            Get
                Select Case m_ResourceType

                    Case ResourceTypeEnum.XmlDomainMap

                        If Not m_Resource Is Nothing Then Return CType(m_Resource, IDomainMap).Name

                End Select

                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Overridable Property ResourceType() As Puzzle.ObjectMapper.GUI.ProjectModel.ResourceTypeEnum Implements Puzzle.ObjectMapper.GUI.ProjectModel.IResource.ResourceType
            Get
                Return m_ResourceType
            End Get
            Set(ByVal Value As Puzzle.ObjectMapper.GUI.ProjectModel.ResourceTypeEnum)
                m_ResourceType = Value
            End Set
        End Property

        Public Overridable Function GetResource() As Object Implements IResource.GetResource
            Return m_Resource
        End Function

        Public Overridable Sub SetResource(ByVal value As Object) Implements IResource.SetResource
            m_Resource = value
        End Sub

        <XmlArrayItem(GetType(Resource))> Public Overridable Property Resources() As System.Collections.ArrayList Implements IResource.Resources
            Get
                Return m_Resources
            End Get
            Set(ByVal Value As System.Collections.ArrayList)
                m_Resources = Value
            End Set
        End Property

        <XmlArrayItem(GetType(DomainConfig))> Public Overridable Property Configs() As System.Collections.ArrayList Implements IResource.Configs
            Get
                Return m_Configs
            End Get
            Set(ByVal Value As System.Collections.ArrayList)
                m_Configs = Value
            End Set
        End Property

        <XmlArrayItem(GetType(Uml.UmlDiagram))> Public Overridable Property Diagrams() As System.Collections.ArrayList Implements IResource.Diagrams
            Get
                Return m_Diagrams
            End Get
            Set(ByVal Value As System.Collections.ArrayList)
                m_Diagrams = Value
            End Set
        End Property

        Public Sub SetActiveConfig(ByVal config As DomainConfig) Implements IResource.SetActiveConfig

            Dim cfg As DomainConfig

            For Each cfg In m_Configs

                If cfg Is config Then

                    cfg.IsActive = True

                Else

                    cfg.IsActive = False

                End If

            Next

        End Sub

        Public Sub Setup() Implements IResource.Setup

            Dim res As IResource
            Dim config As DomainConfig
            Dim diagram As Uml.UmlDiagram

            For Each res In m_Resources

                res.SetOwnerResource(Me)
                res.Setup()

            Next

            For Each config In m_Configs

                config.setup()

            Next

            For Each diagram In m_Diagrams

                diagram.SetOwnerResource(Me)
                diagram.Setup()

            Next

        End Sub


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String) Implements IResource.UpdateMapObjectName

            Dim res As IResource
            Dim cfg As DomainConfig
            Dim diagram As UmlDiagram

            For Each res In m_Resources

                res.UpdateMapObjectName(mapObject, newName)

            Next

            For Each cfg In Me.Configs

                cfg.UpdateMapObjectName(mapObject, newName)

            Next

            For Each diagram In Me.Diagrams

                diagram.UpdateMapObjectName(mapObject, newName)

            Next

        End Sub

        Public Function GetDiagram(ByVal name As String) As Uml.UmlDiagram Implements IResource.GetDiagram

            Dim diagram As Uml.UmlDiagram

            For Each diagram In m_Diagrams

                'OBS!!! Problems for two diagrams with the same names!
                'Could be solved by assigning (transient or persistent, both would work)
                'GUIDs to the diagrams and use them instead.
                If diagram.Name = name Then

                    Return diagram

                End If

            Next

        End Function
    End Class

End Namespace
