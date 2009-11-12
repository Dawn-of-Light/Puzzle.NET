Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization

Namespace ProjectModel

    Public Class TablesToClassesConfig
        Inherits MapBase


        Private m_TableToClassNameConverter As String = ""
        Private m_ColumnToPropertyNameConverter As String = ""
        Private m_PropertyToInverseNameConverter As String = ""

        Private m_GenerateInverseProperties As Boolean = True

        Public Property TableToClassNameConverter() As String
            Get
                Return m_TableToClassNameConverter
            End Get
            Set(ByVal Value As String)
                m_TableToClassNameConverter = Value
            End Set
        End Property

        Public Property ColumnToPropertyNameConverter() As String
            Get
                Return m_ColumnToPropertyNameConverter
            End Get
            Set(ByVal Value As String)
                m_ColumnToPropertyNameConverter = Value
            End Set
        End Property

        Public Property PropertyToInverseNameConverter() As String
            Get
                Return m_PropertyToInverseNameConverter
            End Get
            Set(ByVal Value As String)
                m_PropertyToInverseNameConverter = Value
            End Set
        End Property

        Public Overridable Property GenerateInverseProperties() As Boolean
            Get
                Return m_GenerateInverseProperties
            End Get
            Set(ByVal Value As Boolean)
                m_GenerateInverseProperties = Value
            End Set
        End Property


        Public Overrides Function Clone() As IMap

        End Function

        Public Overrides Function Compare(ByVal compareTo As IMap) As Boolean

        End Function

        Public Overrides Sub Copy(ByVal mapObject As IMap)

        End Sub

        Public Overrides Function DeepClone() As IMap

        End Function

        Public Overrides Function DeepCompare(ByVal compareTo As IMap) As Boolean

        End Function

        Public Overrides Sub DeepCopy(ByVal mapObject As IMap)

        End Sub

        Public Overrides Sub DeepMerge(ByVal mapObject As IMap)

        End Sub

        Public Overrides Function GetKey() As String

        End Function

        Public Overrides Sub Accept(ByVal mapVisitor As Puzzle.NPersist.Framework.Mapping.Visitor.IMapVisitor)

        End Sub
    End Class

End Namespace

