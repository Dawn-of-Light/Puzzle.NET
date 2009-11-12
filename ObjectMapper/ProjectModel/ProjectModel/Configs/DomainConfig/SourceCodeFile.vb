Imports System.Xml.Serialization
Imports Puzzle.NPersist.Framework.Mapping
Imports System.IO

Namespace ProjectModel

    Public Class SourceCodeFile
        Inherits MapBase


        Private m_Name As String
        Private m_FilePath As String
        Private m_LastWrittenTo As DateTime

        Private m_FileType As SourceCodeFileTypeEnum
        Private m_MapObjectType As String
        Private m_MapObjectName As String

        Public Property FilePath() As String
            Get
                Return m_FilePath
            End Get
            Set(ByVal Value As String)
                m_FilePath = Value
            End Set
        End Property

        Public Property MapObjectType() As String
            Get
                Return m_MapObjectType
            End Get
            Set(ByVal Value As String)
                m_MapObjectType = Value
            End Set
        End Property

        Public Property MapObjectName() As String
            Get
                Return m_MapObjectName
            End Get
            Set(ByVal Value As String)
                m_MapObjectName = Value
            End Set
        End Property

        Public Property FileType() As SourceCodeFileTypeEnum
            Get
                Return m_FileType
            End Get
            Set(ByVal Value As SourceCodeFileTypeEnum)
                m_FileType = Value
            End Set
        End Property

        Public Property LastWrittenTo() As DateTime
            Get
                Return m_LastWrittenTo
            End Get
            Set(ByVal Value As DateTime)
                m_LastWrittenTo = Value
            End Set
        End Property

        Public Function GetName()

            Dim arr() As String = Split(Me.FilePath, "\")

            Return arr(UBound(arr))

        End Function

        Public Function IsSynched() As Boolean

            If File.Exists(m_FilePath) Then

                If File.GetLastWriteTime(m_FilePath) = m_LastWrittenTo Then

                    Return True

                End If


            End If

        End Function


        Public Overrides Property Name() As String
            Get
                If Len(m_Name) > 0 Then
                    Return m_Name
                Else
                    Return Me.GetName
                End If
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

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
