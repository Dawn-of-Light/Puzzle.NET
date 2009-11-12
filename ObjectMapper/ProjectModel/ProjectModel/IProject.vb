Imports Puzzle.NPersist.Framework.Mapping

Namespace ProjectModel

    Public Interface IProject
        Inherits IMap

        Property Resources() As ArrayList

        Sub Save(ByVal path As String)

        Function GetDomainMaps() As IList

        Function GetResources(ByVal resourceType As ResourceTypeEnum) As IList

        Function GetResource(ByVal name As String, ByVal resourceType As ResourceTypeEnum) As IResource

        Function GetDomainMap(ByVal name As String) As IDomainMap

        Function GetDiagram(ByVal domainMap As IDomainMap, ByVal name As String) As Uml.UmlDiagram

        Property FilePath() As String

        Sub SetHintPaths(ByVal projectPath As String)

        Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

        Sub Setup()

        Sub DeletingClassMap(ByVal classMap As IClassMap)

        Function GetFilePathRelativeToProject(ByVal projPath As String, ByVal filePath As String) As String

    End Interface

End Namespace
