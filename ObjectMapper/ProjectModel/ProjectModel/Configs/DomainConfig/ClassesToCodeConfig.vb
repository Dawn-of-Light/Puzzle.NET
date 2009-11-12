Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization

Namespace ProjectModel

    Public Class ClassesToCodeConfig
        Inherits MapBase


        Private m_TargetFolder As String = ""

        Private m_ImplementIInterceptable As Boolean = True
        Private m_ImplementIObjectHelper As Boolean = True
        Private m_ImplementIObservable As Boolean = False

        Private m_IncludeRegions As Boolean = False
        Private m_IncludeComments As Boolean = True

        Private m_IncludeDocComments As Boolean = True
        Private m_IncludeModelInfoInDocComments As Boolean = True

        Private m_NotifyOnlyWhenRequired As Boolean = True
        Private m_NotifyLightWeight As Boolean = True
        Private m_NotifyAfterGet As Boolean = True
        Private m_NotifyAfterSet As Boolean = True

        Private m_ImplementShadows As Boolean = False
        Private m_AddPropertyNotifyMethods As Boolean = False
        Private m_GeneratePOCO As Boolean = True ' False

        Private m_TargetLanguage As TargetLanguageEnum = TargetLanguageEnum.VB
        Private m_TargetMapper As TargetMapperEnum = TargetMapperEnum.NPersist

        Private m_ReallyNoRegions As Boolean = False

        Private m_UseTypedCollections As Boolean = False
        Private m_WrapCollections As Boolean = False
        Private m_EmbedXml As Boolean = False

        Private m_PutAllClassesInOneFile As Boolean = False
        Private m_XmlFilePerClass As Boolean = False

        Private m_UseGenericCollections As Boolean

        Private m_SourceCodeFiles As New ArrayList

        Public Property TargetFolder() As String
            Get
                Return m_TargetFolder
            End Get
            Set(ByVal Value As String)
                m_TargetFolder = Value
            End Set
        End Property

        Public Property ImplementIInterceptable() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_ImplementIInterceptable
            End Get
            Set(ByVal Value As Boolean)
                m_ImplementIInterceptable = Value
            End Set
        End Property

        Public Property ImplementIObjectHelper() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_ImplementIObjectHelper
            End Get
            Set(ByVal Value As Boolean)
                m_ImplementIObjectHelper = Value
            End Set
        End Property

        Public Property ImplementIObservable() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_ImplementIObservable
            End Get
            Set(ByVal Value As Boolean)
                m_ImplementIObservable = Value
            End Set
        End Property



        Public Property IncludeRegions() As Boolean
            Get
                Return m_IncludeRegions
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeRegions = Value
            End Set
        End Property

        Public Property IncludeComments() As Boolean
            Get
                Return m_IncludeComments
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeComments = Value
            End Set
        End Property

        Public Property IncludeDocComments() As Boolean
            Get
                Return m_IncludeDocComments
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeDocComments = Value
            End Set
        End Property

        Public Property IncludeModelInfoInDocComments() As Boolean
            Get
                Return m_IncludeModelInfoInDocComments
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeModelInfoInDocComments = Value
            End Set
        End Property


        Public Property NotifyOnlyWhenRequired() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_NotifyOnlyWhenRequired
            End Get
            Set(ByVal Value As Boolean)
                m_NotifyOnlyWhenRequired = Value
            End Set
        End Property

        Public Property NotifyLightWeight() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_NotifyLightWeight
            End Get
            Set(ByVal Value As Boolean)
                m_NotifyLightWeight = Value
            End Set
        End Property

        Public Property NotifyAfterGet() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_NotifyAfterGet
            End Get
            Set(ByVal Value As Boolean)
                m_NotifyAfterGet = Value
            End Set
        End Property

        Public Property NotifyAfterSet() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_NotifyAfterSet
            End Get
            Set(ByVal Value As Boolean)
                m_NotifyAfterSet = Value
            End Set
        End Property


        Public Property ImplementShadows() As Boolean
            Get
                Return m_ImplementShadows
            End Get
            Set(ByVal Value As Boolean)
                m_ImplementShadows = Value
            End Set
        End Property

        Public Property AddPropertyNotifyMethods() As Boolean
            Get
                If GeneratePOCO Then Return False
                Return m_AddPropertyNotifyMethods
            End Get
            Set(ByVal Value As Boolean)
                m_AddPropertyNotifyMethods = Value
            End Set
        End Property

        Public Property GeneratePOCO() As Boolean
            Get
                If m_TargetMapper = TargetMapperEnum.NHibernate Then Return True
                If m_TargetMapper = TargetMapperEnum.POCO Then Return True
                Return m_GeneratePOCO
            End Get
            Set(ByVal Value As Boolean)
                m_GeneratePOCO = Value
            End Set
        End Property


        Public Property TargetLanguage() As TargetLanguageEnum
            Get
                Return m_TargetLanguage
            End Get
            Set(ByVal Value As TargetLanguageEnum)
                m_TargetLanguage = Value
            End Set
        End Property

        Public Property TargetMapper() As TargetMapperEnum
            Get
                Return m_TargetMapper
            End Get
            Set(ByVal Value As TargetMapperEnum)
                m_TargetMapper = Value
            End Set
        End Property

        Public Property ReallyNoRegions() As Boolean
            Get
                Return m_ReallyNoRegions
            End Get
            Set(ByVal Value As Boolean)
                m_ReallyNoRegions = Value
            End Set
        End Property

        Public Property UseTypedCollections() As Boolean
            Get
                Return m_UseTypedCollections
            End Get
            Set(ByVal Value As Boolean)
                m_UseTypedCollections = Value
            End Set
        End Property

        Public Property WrapCollections() As Boolean
            Get
                Return m_WrapCollections
            End Get
            Set(ByVal Value As Boolean)
                m_WrapCollections = Value
            End Set
        End Property

        Public Property EmbedXml() As Boolean
            Get
                Return m_EmbedXml
            End Get
            Set(ByVal Value As Boolean)
                m_EmbedXml = Value
            End Set
        End Property

        <XmlArrayItem(GetType(SourceCodeFile))> Public Overridable Property SourceCodeFiles() As System.Collections.ArrayList
            Get
                Return m_SourceCodeFiles
            End Get
            Set(ByVal Value As System.Collections.ArrayList)
                m_SourceCodeFiles = Value
            End Set
        End Property

        Public Property PutAllClassesInOneFile() As Boolean
            Get
                Return m_PutAllClassesInOneFile
            End Get
            Set(ByVal Value As Boolean)
                m_PutAllClassesInOneFile = Value
            End Set
        End Property

        Public Property XmlFilePerClass() As Boolean
            Get
                Return m_XmlFilePerClass
            End Get
            Set(ByVal Value As Boolean)
                m_XmlFilePerClass = Value
            End Set
        End Property

        Public Property UseGenericCollections() As Boolean
            Get
                Return m_UseGenericCollections
            End Get
            Set(ByVal Value As Boolean)
                m_UseGenericCollections = Value
            End Set
        End Property


        Public Function UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String) As SourceCodeFile

            Dim src As SourceCodeFile

            For Each src In m_SourceCodeFiles

                If src.MapObjectType = CType(mapObject, Object).GetType.ToString Then

                    If src.MapObjectName = mapObject.Name Then

                        src.MapObjectName = newName

                    End If

                End If

            Next

        End Function

        Public Overloads Function GetSourceCodeFile(ByVal Name As String) As SourceCodeFile

            Dim src As SourceCodeFile

            For Each src In m_SourceCodeFiles

                If src.MapObjectType = "" Then

                    If src.MapObjectName = Name Then

                        Return src

                    End If

                End If

            Next

        End Function

        Public Overloads Function SetSourceCodeFile(ByVal Name As String, ByVal filePath As String) As SourceCodeFile

            Dim src As SourceCodeFile

            For Each src In m_SourceCodeFiles

                If src.MapObjectType = "" Then

                    If src.MapObjectName = Name Then

                        src.FilePath = filePath
                        Return src

                    End If

                End If

            Next

            src = New SourceCodeFile

            src.MapObjectType = ""
            src.MapObjectName = Name

            src.FilePath = filePath

            m_SourceCodeFiles.Add(src)

            Return src

        End Function


        Public Overloads Function GetSourceCodeFile(ByVal mapObject As IMap) As SourceCodeFile

            Dim src As SourceCodeFile

            For Each src In m_SourceCodeFiles

                If src.MapObjectType = CType(mapObject, Object).GetType.ToString Then

                    If src.MapObjectName = mapObject.Name Then

                        Return src

                    End If

                End If

            Next

        End Function

        Public Overloads Function SetSourceCodeFile(ByVal mapObject As IMap, ByVal filePath As String) As SourceCodeFile

            Dim src As SourceCodeFile

            For Each src In m_SourceCodeFiles

                If src.MapObjectType = CType(mapObject, Object).GetType.ToString Then

                    If src.MapObjectName = mapObject.Name Then

                        src.FilePath = filePath
                        Return src

                    End If

                End If

            Next

            src = New SourceCodeFile

            src.MapObjectType = CType(mapObject, Object).GetType.ToString
            src.MapObjectName = mapObject.Name

            src.FilePath = filePath

            m_SourceCodeFiles.Add(src)

            Return src

        End Function

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
