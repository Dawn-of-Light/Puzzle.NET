Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Class DiffInfo
    Implements IDiffInfo

    Private m_MapObject As Puzzle.NPersist.Framework.Mapping.IMap
    Private m_Message As String
    Private m_DifferenceType As Puzzle.ObjectMapper.Tools.DiffInfoEnum
    Private m_Setting As String

    Public Sub New()
        MyBase.New()

    End Sub

    Public Sub New(ByVal mapObject As IMap, ByVal message As String, ByVal diffInfo As DiffInfoEnum, ByVal setting As String)
        MyBase.New()

        m_MapObject = mapObject
        m_Message = message
        m_DifferenceType = DiffInfo
        m_Setting = setting

    End Sub

    Public Overridable Property MapObject() As Puzzle.NPersist.Framework.Mapping.IMap Implements Puzzle.ObjectMapper.Tools.IDiffInfo.MapObject
        Get
            Return m_MapObject
        End Get
        Set(ByVal Value As Puzzle.NPersist.Framework.Mapping.IMap)
            m_MapObject = Value
        End Set
    End Property

    Public Overridable Property Message() As String Implements Puzzle.ObjectMapper.Tools.IDiffInfo.Message
        Get
            Return m_Message
        End Get
        Set(ByVal Value As String)
            m_Message = Value
        End Set
    End Property

    Public Overridable Property DifferenceType() As Puzzle.ObjectMapper.Tools.DiffInfoEnum Implements Puzzle.ObjectMapper.Tools.IDiffInfo.DifferenceType
        Get
            Return m_DifferenceType
        End Get
        Set(ByVal Value As Puzzle.ObjectMapper.Tools.DiffInfoEnum)
            m_DifferenceType = Value
        End Set
    End Property

    Public Overridable Property Setting() As String Implements Puzzle.ObjectMapper.Tools.IDiffInfo.Setting
        Get
            Return m_Setting
        End Get
        Set(ByVal Value As String)
            m_Setting = Value
        End Set
    End Property
End Class
