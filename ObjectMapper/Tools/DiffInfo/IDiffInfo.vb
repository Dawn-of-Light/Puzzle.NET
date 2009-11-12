Imports System
Imports System.Collections
Imports Microsoft.VisualBasic
Imports Puzzle.NPersist.Framework.Mapping

Public Interface IDiffInfo

    Property MapObject() As IMap

    Property Message() As String

    Property DifferenceType() As DiffInfoEnum

    Property Setting() As String


End Interface
