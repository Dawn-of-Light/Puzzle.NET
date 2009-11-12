Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Public Class SourceCodeFileListItem
    Inherits MapListItem

    Private m_Map As ClassesToCodeConfig
    Private m_DomainMap As IDomainMap

    Public Sub New(ByVal map As ClassesToCodeConfig, ByVal domainMap As IDomainMap)
        MyBase.New(map)

        Me.Text = "Files"

        m_Map = map

        m_DomainMap = domainMap



        Me.SubItems.Add("The source code files generated under this tool config")

        Me.ImageIndex = 54

    End Sub

    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

End Class


