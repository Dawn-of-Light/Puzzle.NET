Imports Puzzle.NPersist.Framework.Mapping

Namespace ProjectModel

    Public Interface IResource

        Property Name() As String

        Property FilePath() As String

        Property HintPath() As String

        Property ResourceType() As ResourceTypeEnum

        Function GetResource() As Object

        Sub SetResource(ByVal value As Object)

        Property Resources() As ArrayList

        Property Configs() As ArrayList

        Property Diagrams() As ArrayList

        Function GetDiagram(ByVal name As String) As Uml.UmlDiagram

        Sub SetActiveConfig(ByVal config As DomainConfig)

        Sub Setup()

        Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

        Property OwnerResource() As IResource

        Sub SetOwnerResource(ByVal value As IResource)

    End Interface

End Namespace
