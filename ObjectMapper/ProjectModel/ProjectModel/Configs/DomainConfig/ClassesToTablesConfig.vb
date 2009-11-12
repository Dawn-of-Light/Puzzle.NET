Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization

Namespace ProjectModel

    Public Class ClassesToTablesConfig
        Inherits MapBase


        Private m_TablePrefix As String = "tbl"
        Private m_TableSuffix As String = ""

        Public Property TablePrefix() As String
            Get
                Return m_TablePrefix
            End Get
            Set(ByVal Value As String)
                m_TablePrefix = Value
            End Set
        End Property

        Public Property TableSuffix() As String
            Get
                Return m_TableSuffix
            End Get
            Set(ByVal Value As String)
                m_TableSuffix = Value
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
