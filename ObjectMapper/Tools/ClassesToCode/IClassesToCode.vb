Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface IClassesToCode

    Enum IndentationLevelEnum
        NamespaceIndent = 0
        ClassIndent = 1
        MemberIndent = 2
        PropertyGetSetIndent = 3
    End Enum

    Enum CodeLanguageEnum
        VbNet = 0
        CSharp = 1
        Delphi = 2
    End Enum

    Function DomainToProject(ByVal domainMap As IDomainMap, ByVal projPath As String, ByVal classMapsAndFiles As Hashtable, ByVal embeddedFiles As ArrayList) As String

    Function DomainToAssemblyInfo(ByVal domainMap As IDomainMap) As String

    Function DomainToCode(ByVal domainMap As IDomainMap, ByVal noRootNamespace As Boolean) As String

    Function NamespaceToCode(ByVal domainMap As IDomainMap, ByVal name As String, ByVal noRootNamespace As Boolean) As String

    Function ClassToCode(ByVal classMap As IClassMap) As String

    Function ClassToCode(ByVal classMap As IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean) As String

    Function ClassToCode(ByVal classMap As IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean, ByVal customCode As String) As String

    Function ClassToTypedCollection(ByVal classMap As IClassMap, ByVal noNamespace As Boolean, ByVal noRootNamespace As Boolean) As String

    Function PropertyToCode(ByVal propertyMap As IPropertyMap) As String

    Function InheritedPropertyToCode(ByVal propertyMap As IPropertyMap) As String

    Function GetIndentation(ByVal IndentationLevel As IndentationLevelEnum) As String

    Function GetIndentation(ByVal IndentationLevel As IndentationLevelEnum, ByVal additionalLevel As Integer) As String

    Property ImplementIInterceptable() As Boolean

    Property ImplementIObjectHelper() As Boolean

    Property ImplementIObservable() As Boolean

    Property IncludeRegions() As Boolean

    Property IncludeComments() As Boolean

    Property IncludeDocComments() As Boolean

    Property DocCommentPrefix() As String

    Property IncludeModelInfoInDocComments() As Boolean

    Property NotifyOnlyWhenRequired() As Boolean

    Property NotifyLightWeight() As Boolean

    Property NotifyAfterGet() As Boolean

    Property NotifyAfterSet() As Boolean

    Property ImplementShadows() As Boolean

    Property AddPropertyNotifyMethods() As Boolean

    Property GeneratePOCO() As Boolean

    Property TargetPlatform() As TargetPlatformEnum

    Property ReallyNoRegions() As Boolean


    Property UseTypedCollections() As Boolean

    Property WrapCollections() As Boolean

    Property XmlFilePerClass() As Boolean

    Property EmbedXml() As Boolean

    Property UseGenericCollections() As Boolean

    'Property 

    Function GetFilePathRelativeToProject(ByVal projPath As String, ByVal filePath As String) As String

    Overloads Function GetDocComment(ByVal classMap As IClassMap, ByVal prefix As String) As String

    Overloads Function GetDocComment(ByVal propertyMap As IPropertyMap, ByVal prefix As String) As String

    Overloads Function GetDocComment(ByVal prefix As String, ByVal summary As String, ByVal remarks As String, ByVal params As ArrayList, ByVal returns As String) As String

    Overloads Function GetInfoComments(ByVal obj As IMap) As String

    Overloads Function GetInfoComments(ByVal obj As IMap, ByVal indent As String) As String

    Overloads Function GetInfoComments(ByVal obj As IMap, ByVal indent As String, ByVal commentPrefix As String) As String

    Function HasReferencesByIdentityProperties(ByVal classMap As IClassMap) As Boolean


End Interface
