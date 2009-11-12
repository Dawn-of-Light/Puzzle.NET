Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization

Namespace ProjectModel

    Public Class DomainConfig
        Inherits MapBase


        Private m_Name As String
        Private m_Description As String
        Private m_IsActive As Boolean

        Private m_ClassesToCodeConfig As New ClassesToCodeConfig
        Private m_ClassesToTablesConfig As New ClassesToTablesConfig
        Private m_TablesToClassesConfig As New TablesToClassesConfig

        Public Property ClassesToCodeConfig() As ClassesToCodeConfig
            Get
                Return m_ClassesToCodeConfig
            End Get
            Set(ByVal Value As ClassesToCodeConfig)
                m_ClassesToCodeConfig = Value
            End Set
        End Property

        Public Property ClassesToTablesConfig() As ClassesToTablesConfig
            Get
                Return m_ClassesToTablesConfig
            End Get
            Set(ByVal Value As ClassesToTablesConfig)
                m_ClassesToTablesConfig = Value
            End Set
        End Property

        Public Property TablesToClassesConfig() As TablesToClassesConfig
            Get
                Return m_TablesToClassesConfig
            End Get
            Set(ByVal Value As TablesToClassesConfig)
                m_TablesToClassesConfig = Value
            End Set
        End Property

        Public Property Description()
            Get
                Return m_Description
            End Get
            Set(ByVal Value)
                m_Description = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

            m_ClassesToCodeConfig.UpdateMapObjectName(mapObject, newName)

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



        Public Sub setup()

        End Sub

        Public Overrides Sub Accept(ByVal mapVisitor As Puzzle.NPersist.Framework.Mapping.Visitor.IMapVisitor)

        End Sub
    End Class

End Namespace
