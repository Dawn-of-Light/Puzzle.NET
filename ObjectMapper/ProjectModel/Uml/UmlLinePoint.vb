Imports System.Drawing
Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization

Namespace Uml

    Public Class UmlLinePoint
        Inherits MapBase


        Private m_UmlLine As UmlLine

        Private m_X As Integer = 0
        Private m_Y As Integer = 0


        Private m_Selected As Boolean = False


        <XmlIgnore()> Public Property UmlLine() As UmlLine
            Get
                Return m_UmlLine
            End Get
            Set(ByVal Value As UmlLine)
                If Not m_UmlLine Is Nothing Then
                    m_UmlLine.UmlLinePoints.Remove(Me)
                End If
                m_UmlLine = Value
                If Not m_UmlLine Is Nothing Then
                    m_UmlLine.UmlLinePoints.Add(Me)
                End If
            End Set
        End Property


        Public Overridable Sub SetUmlLine(ByVal Value As UmlLine)

            m_UmlLine = Value

        End Sub

        Public Property X() As Integer
            Get
                Return m_X
            End Get
            Set(ByVal Value As Integer)
                m_X = Value
            End Set
        End Property


        Public Property Y() As Integer
            Get
                Return m_Y
            End Get
            Set(ByVal Value As Integer)
                m_Y = Value
            End Set
        End Property

        Public Property Selected() As Boolean
            Get
                Return m_Selected
            End Get
            Set(ByVal Value As Boolean)
                m_Selected = Value
            End Set
        End Property


        Public Function GetLocation(Optional ByVal offsetX As Integer = 0, Optional ByVal offsetY As Integer = 0) As Point

            Dim zoom As Decimal = GetZoom()

            Dim diagramLocation As Point = m_UmlLine.UmlDiagram.Location

            Dim location As New Point(CInt(X * zoom) + offsetX - diagramLocation.X, CInt(Y * zoom) + offsetY - diagramLocation.Y)

            Return location

        End Function


        Public Function GetZoom() As Decimal

            Return m_UmlLine.UmlDiagram.Zoom

        End Function

        Public Sub MoveSelected(ByVal X As Integer, ByVal Y As Integer, ByVal lastX As Integer, ByVal lastY As Integer)

            If m_Selected Then

                Dim diffX As Integer = X - lastX
                Dim diffY As Integer = Y - lastY

                Dim zoom As Double = GetZoom()

                If zoom = 0 Then zoom = 1

                Me.X = Me.X + CInt(diffX / zoom)
                Me.Y = Me.Y + CInt(diffY / zoom)

            End If

        End Sub


        Public Overloads Sub DeselectAll(ByVal exceptObjects As ArrayList)

            If Not exceptObjects.Contains(Me) Then

                m_Selected = False

            End If

        End Sub

        Public Overloads Sub SelectAll()

            m_Selected = True

        End Sub

        Public Overloads Sub SelectAll(ByVal onObjects As ArrayList)

            If onObjects.Contains(Me) Then

                m_Selected = True

            End If

        End Sub

        Public Overloads Sub FlipSelection(ByVal onObjects As ArrayList)

            If onObjects.Contains(Me) Then

                FlipSelection()

            End If

        End Sub

        Public Overloads Sub FlipSelection()

            If m_Selected Then

                m_Selected = False

            Else

                m_Selected = True

            End If

        End Sub


        Public Function HitTest(ByVal x As Integer, ByVal y As Integer) As Boolean

            Dim sqSize As Integer = 16

            Dim startLocation As Point = GetLocation()

            Dim location As Point = New Point(startLocation.X - CInt(sqSize / 2), startLocation.Y - CInt(sqSize / 2))

            If x > location.X And x < (location.X + sqSize) Then

                If y > location.Y And y < (location.Y + sqSize) Then

                    Return True

                End If

            End If

            Return False

        End Function


        Public Function AddUmlLinePoint() As UmlLinePoint

            Dim newUmlLinePoint As UmlLinePoint = New UmlLinePoint

            Dim startPt As Point
            Dim endPt As Point

            Dim index As Integer = m_UmlLine.GetPointIndex(Me)

            startPt = m_UmlLine.GetPoint(index)
            endPt = m_UmlLine.GetPoint(index + 1)


            newUmlLinePoint.Selected = True

            Dim zoom As Double = GetZoom()

            newUmlLinePoint.X = ((startPt.X + ((endPt.X - startPt.X) / 2)) / zoom) + m_UmlLine.UmlDiagram.Location.X
            newUmlLinePoint.Y = ((startPt.Y + ((endPt.Y - startPt.Y) / 2)) / zoom) + m_UmlLine.UmlDiagram.Location.Y

            m_UmlLine.UmlLinePoints.Insert(index, newUmlLinePoint)
            newUmlLinePoint.SetUmlLine(m_UmlLine)

            Return newUmlLinePoint

        End Function




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
