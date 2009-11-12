Imports Puzzle.NPersist.Framework.Mapping
Imports System.Xml.Serialization
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Puzzle.ObjectMapper.GUI.ProjectModel

Namespace Uml

    Public Class UmlDiagram
        Inherits MapBase



        Private m_Moving As Boolean
        Private m_MoveReduction As Byte = 1

        'Transient
        Private m_MazZoomSize As Double = 5

        '/Transient

        Private m_Name As String

        Private m_UmlClasses As New ArrayList
        Private m_UmlLines As New ArrayList

        Private m_OwnerResource As IResource

        Private m_Location As Point
        Private m_Size As Size
        Private m_Zoom As Decimal = 1

        Private m_Selected As Boolean = False

        Private m_DisplayAssociationArrows As Boolean = False

        Private m_UseGrid As Boolean = True
        Private m_GridSize As Double = 50

        Private m_GridColor1 As UmlColor = New UmlColor(Color.FromArgb(255, 208, 208, 208))
        Private m_GridColor2 As UmlColor = New UmlColor(Color.FromArgb(255, 240, 240, 240))

        Private m_DrawShadows As Boolean = True
        Private m_DrawShadowsPass2 As Boolean = True

        Private m_ShadowColor1 As UmlColor = New UmlColor(Color.FromArgb(128, 196, 196, 196))
        Private m_ShadowColor2 As UmlColor = New UmlColor(Color.FromArgb(128, 128, 128, 128))

        Private m_BgBrushStyle As BrushEnum = BrushEnum.SolidBrush
        Private m_BgGradientMode As LinearGradientMode = LinearGradientMode.ForwardDiagonal

        Private m_ColorSynchError As UmlColor = New UmlColor(Color.DarkRed)
        Private m_ColorError As UmlColor = New UmlColor(Color.Red)

        Private m_ForeColor As UmlColor = New UmlColor(Color.Black)
        Private m_BackColor1 As UmlColor = New UmlColor(Color.White)
        Private m_BackColor2 As UmlColor = New UmlColor(Color.FromArgb(255, 224, 224, 224))


        Private m_ShadowBrushStyle As BrushEnum = BrushEnum.GradientBrush
        Private m_ShadowGradientMode As LinearGradientMode = LinearGradientMode.ForwardDiagonal

        Private m_FontSize As Single = 8.25
        Private m_FontFamily As String = "Microsoft Sans Serif"
        Private m_FontColor As UmlColor = New UmlColor(Color.Black)
        Private m_FontStyle As FontStyle = FontStyle.Regular


        Private m_ClassBgBrushStyle As BrushEnum = BrushEnum.GradientBrush
        Private m_ClassBgGradientMode As LinearGradientMode = LinearGradientMode.ForwardDiagonal

        Private m_ClassBackColor1 As UmlColor = New UmlColor(Color.White)
        Private m_ClassBackColor2 As UmlColor = New UmlColor(Color.FromArgb(255, 224, 224, 224))

        Private m_ArrowBaseLength As Double = 20

        Private m_RadarSize As Integer = 100
        Private m_DisplayRadar As Boolean = True

        Private m_RadarForeColor As UmlColor = New UmlColor(Color.Black)
        Private m_RadarBackColor As UmlColor = New UmlColor(Color.LightGray)

        Private m_RadarClassForeColor As UmlColor = New UmlColor(Color.Black)
        Private m_RadarClassBackColor As UmlColor = New UmlColor(Color.White)

        Private m_RadarViewColor As UmlColor = New UmlColor(Color.Brown)
        Private m_RadarViewStyle As DashStyle = DashStyle.Dash

        Private m_DisplayInheritedProperties As Boolean = False
        Private m_DisplayLineProperties As Boolean = False

        <XmlIgnore()> Public Property OwnerResource() As IResource
            Get
                Return m_OwnerResource
            End Get
            Set(ByVal Value As IResource)
                If Not m_OwnerResource Is Nothing Then
                    m_OwnerResource.Diagrams.Remove(Me)
                End If
                m_OwnerResource = Value
                If Not m_OwnerResource Is Nothing Then
                    m_OwnerResource.Diagrams.Add(Me)
                End If
            End Set
        End Property

        '<XmlIgnore()> Public Property ListExceptions() As ArrayList
        '    Get
        '        Return m_listExceptions
        '    End Get
        '    Set(ByVal Value As ArrayList)
        '        m_listExceptions = Value
        '    End Set
        'End Property

        Public Overridable Sub SetOwnerResource(ByVal Value As IResource)

            m_OwnerResource = Value

        End Sub


        <XmlArrayItem(GetType(UmlClass))> Public Property UmlClasses() As ArrayList
            Get
                Return m_UmlClasses
            End Get
            Set(ByVal Value As ArrayList)
                m_UmlClasses = Value
            End Set
        End Property

        <XmlArrayItem(GetType(UmlLine))> Public Property UmlLines() As ArrayList
            Get
                Return m_UmlLines
            End Get
            Set(ByVal Value As ArrayList)
                m_UmlLines = Value
            End Set
        End Property

        <XmlIgnore()> Public Property Moving() As Boolean
            Get
                Return m_Moving
            End Get
            Set(ByVal Value As Boolean)
                m_Moving = Value
            End Set
        End Property

        <XmlIgnore()> Public Property MoveReduction() As Byte
            Get
                Return m_MoveReduction
            End Get
            Set(ByVal Value As Byte)
                m_MoveReduction = Value
            End Set
        End Property

        Public Sub Setup()

            Dim umlClass As umlClass
            Dim umlLine As umlLine
            Dim umlLinePoint As umlLinePoint

            For Each umlClass In m_UmlClasses

                umlClass.SetUmlDiagram(Me)

            Next

            For Each umlLine In m_UmlLines

                umlLine.SetUmlDiagram(Me)

                For Each umlLinePoint In umlLine.UmlLinePoints

                    umlLinePoint.SetUmlLine(umlLine)

                Next

            Next

        End Sub



        Public Function GetDomainMap() As IDomainMap

            Dim domainMap As IDomainMap
            Dim resource As IResource

            resource = OwnerResource

            While Not resource Is Nothing

                If resource.ResourceType = ResourceTypeEnum.XmlDomainMap Then

                    Return resource.GetResource

                End If

                resource = resource.OwnerResource

            End While


        End Function



        Public Property Location() As Point
            Get
                Return m_Location
            End Get
            Set(ByVal Value As Point)
                m_Location = Value
            End Set
        End Property

        Public Property Size() As Size
            Get
                Return m_Size
            End Get
            Set(ByVal Value As Size)
                m_Size = Value
            End Set
        End Property

        Public Property Zoom() As Decimal
            Get
                Return m_Zoom
            End Get
            Set(ByVal Value As Decimal)
                m_Zoom = Value
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

        Public Property DisplayAssociationArrows() As Boolean
            Get
                Return m_DisplayAssociationArrows
            End Get
            Set(ByVal Value As Boolean)
                m_DisplayAssociationArrows = Value
            End Set
        End Property

        Public Property DrawShadows() As Boolean
            Get
                Return m_DrawShadows
            End Get
            Set(ByVal Value As Boolean)
                m_DrawShadows = Value
            End Set
        End Property

        Public Property DrawShadowsPass2() As Boolean
            Get
                Return m_DrawShadowsPass2
            End Get
            Set(ByVal Value As Boolean)
                m_DrawShadowsPass2 = Value
            End Set
        End Property

        Public Property ShadowColor1() As UmlColor
            Get
                Return m_ShadowColor1
            End Get
            Set(ByVal Value As UmlColor)
                m_ShadowColor1 = Value
            End Set
        End Property

        Public Property ShadowColor2() As UmlColor
            Get
                Return m_ShadowColor2
            End Get
            Set(ByVal Value As UmlColor)
                m_ShadowColor2 = Value
            End Set
        End Property

        Public Property ShadowBrushStyle() As BrushEnum
            Get
                Return m_ShadowBrushStyle
            End Get
            Set(ByVal Value As BrushEnum)
                m_ShadowBrushStyle = Value
            End Set
        End Property

        Public Property ShadowGradientMode() As LinearGradientMode
            Get
                Return m_ShadowGradientMode
            End Get
            Set(ByVal Value As LinearGradientMode)
                m_ShadowGradientMode = Value
            End Set
        End Property

        Public Property ColorSynchError() As UmlColor
            Get
                Return m_ColorSynchError
            End Get
            Set(ByVal Value As UmlColor)
                m_ColorSynchError = Value
            End Set
        End Property

        Public Property ColorError() As UmlColor
            Get
                Return m_ColorError
            End Get
            Set(ByVal Value As UmlColor)
                m_ColorError = Value
            End Set
        End Property

        Public Property UseGrid() As Boolean
            Get
                Return m_UseGrid
            End Get
            Set(ByVal Value As Boolean)
                m_UseGrid = Value
            End Set
        End Property

        Public Property GridSize() As Double
            Get
                Return m_GridSize
            End Get
            Set(ByVal Value As Double)
                m_GridSize = Value
            End Set
        End Property

        Public Property GridColor1() As UmlColor
            Get
                Return m_GridColor1
            End Get
            Set(ByVal Value As UmlColor)
                m_GridColor1 = Value
            End Set
        End Property

        Public Property GridColor2() As UmlColor
            Get
                Return m_GridColor2
            End Get
            Set(ByVal Value As UmlColor)
                m_GridColor2 = Value
            End Set
        End Property

        Public Property ForeColor() As UmlColor
            Get
                Return m_ForeColor
            End Get
            Set(ByVal Value As UmlColor)
                m_ForeColor = Value
            End Set
        End Property

        Public Property BackColor1() As UmlColor
            Get
                Return m_BackColor1
            End Get
            Set(ByVal Value As UmlColor)
                m_BackColor1 = Value
            End Set
        End Property

        Public Property BackColor2() As UmlColor
            Get
                Return m_BackColor2
            End Get
            Set(ByVal Value As UmlColor)
                m_BackColor2 = Value
            End Set
        End Property

        Public Property BgBrushStyle() As BrushEnum
            Get
                Return m_BgBrushStyle
            End Get
            Set(ByVal Value As BrushEnum)
                m_BgBrushStyle = Value
            End Set
        End Property

        Public Property BgGradientMode() As LinearGradientMode
            Get
                Return m_BgGradientMode
            End Get
            Set(ByVal Value As LinearGradientMode)
                m_BgGradientMode = Value
            End Set
        End Property

        Public Property FontSize() As Single
            Get
                Return m_FontSize
            End Get
            Set(ByVal Value As Single)
                m_FontSize = Value
            End Set
        End Property

        Public Property FontFamily() As String
            Get
                Return m_FontFamily
            End Get
            Set(ByVal Value As String)
                m_FontFamily = Value
            End Set
        End Property

        Public Property FontColor() As UmlColor
            Get
                Return m_FontColor
            End Get
            Set(ByVal Value As UmlColor)
                m_FontColor = Value
            End Set
        End Property

        Public Property FontStyle() As FontStyle
            Get
                Return m_FontStyle
            End Get
            Set(ByVal Value As FontStyle)
                m_FontStyle = Value
            End Set
        End Property

        Public Property ClassBgBrushStyle() As BrushEnum
            Get
                Return m_ClassBgBrushStyle
            End Get
            Set(ByVal Value As BrushEnum)
                m_ClassBgBrushStyle = Value
            End Set
        End Property

        Public Property ClassBgGradientMode() As LinearGradientMode
            Get
                Return m_ClassBgGradientMode
            End Get
            Set(ByVal Value As LinearGradientMode)
                m_ClassBgGradientMode = Value
            End Set
        End Property

        Public Property ClassBackColor1() As UmlColor
            Get
                Return m_ClassBackColor1
            End Get
            Set(ByVal Value As UmlColor)
                m_ClassBackColor1 = Value
            End Set
        End Property

        Public Property ClassBackColor2() As UmlColor
            Get
                Return m_ClassBackColor2
            End Get
            Set(ByVal Value As UmlColor)
                m_ClassBackColor2 = Value
            End Set
        End Property



        Public Property ArrowBaseLength() As Double
            Get
                Return m_ArrowBaseLength
            End Get
            Set(ByVal Value As Double)
                m_ArrowBaseLength = Value
            End Set
        End Property

        Public Property RadarSize() As Integer
            Get
                Return m_RadarSize
            End Get
            Set(ByVal Value As Integer)
                m_RadarSize = Value
            End Set
        End Property

        Public Property DisplayRadar() As Boolean
            Get
                Return m_DisplayRadar
            End Get
            Set(ByVal Value As Boolean)
                m_DisplayRadar = Value
            End Set
        End Property

        Public Property RadarForeColor() As UmlColor
            Get
                Return m_RadarForeColor
            End Get
            Set(ByVal Value As UmlColor)
                m_RadarForeColor = Value
            End Set
        End Property

        Public Property RadarBackColor() As UmlColor
            Get
                Return m_RadarBackColor
            End Get
            Set(ByVal Value As UmlColor)
                m_RadarBackColor = Value
            End Set
        End Property

        Public Property RadarClassForeColor() As UmlColor
            Get
                Return m_RadarClassForeColor
            End Get
            Set(ByVal Value As UmlColor)
                m_RadarClassForeColor = Value
            End Set
        End Property

        Public Property RadarClassBackColor() As UmlColor
            Get
                Return m_RadarClassBackColor
            End Get
            Set(ByVal Value As UmlColor)
                m_RadarClassBackColor = Value
            End Set
        End Property

        Public Property RadarViewColor() As UmlColor
            Get
                Return m_RadarViewColor
            End Get
            Set(ByVal Value As UmlColor)
                m_RadarViewColor = Value
            End Set
        End Property

        Public Property RadarViewStyle() As DashStyle
            Get
                Return m_RadarViewStyle
            End Get
            Set(ByVal Value As DashStyle)
                m_RadarViewStyle = Value
            End Set
        End Property


        Public Property DisplayInheritedProperties() As Boolean
            Get
                Return m_DisplayInheritedProperties
            End Get
            Set(ByVal Value As Boolean)
                m_DisplayInheritedProperties = Value
            End Set
        End Property

        Public Property DisplayLineProperties() As Boolean
            Get
                Return m_DisplayLineProperties
            End Get
            Set(ByVal Value As Boolean)
                m_DisplayLineProperties = Value
            End Set
        End Property




        Public Function GetSelectedObjects() As ArrayList

            Dim list As New ArrayList

            Dim umlClass As umlClass
            Dim umlLine As umlLine
            Dim umlLinePoint As umlLinePoint

            For Each umlClass In m_UmlClasses

                If umlClass.Selected Then

                    list.Add(umlClass)

                End If

            Next

            For Each umlLine In m_UmlLines

                If umlLine.StartSelected Or umlLine.EndSelected Then

                    list.Add(umlLine)

                End If

                For Each umlLinePoint In umlLine.UmlLinePoints

                    If umlLinePoint.Selected Then

                        list.Add(umlLinePoint)

                    End If

                Next

            Next

            Return list

        End Function

        Public Function GetSelectedUmlClasses() As ArrayList

            Dim list As New ArrayList

            Dim umlClass As umlClass

            For Each umlClass In m_UmlClasses

                If umlClass.Selected Then

                    list.Add(umlClass)

                End If

            Next

            Return list

        End Function

        Public Function GetSelectedUmlLines() As ArrayList

            Dim list As New ArrayList

            Dim umlLine As umlLine

            For Each umlLine In m_UmlLines

                If umlLine.HasSelected Then

                    list.Add(umlLine)

                End If

            Next

            Return list

        End Function

        Private m_BestCount As Long = 0
        Private m_SlowCount As Integer = 0

        Public Sub Render(ByVal g As Graphics, Optional ByVal checkMappings As Boolean = True, Optional ByVal includeGrid As Boolean = True, Optional ByVal includeRadar As Boolean = True)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            Dim startTime As DateTime

            If Moving Then

                startTime = Now

            Else

                MoveReduction = 0
                m_SlowCount = 0
                m_BestCount = 0

            End If

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            If includeGrid Then

                DrawGrid(g, 0)
                DrawGrid(g, 1)

            End If

            If Not (m_Moving And m_MoveReduction > 3) Then

                For Each umlClass In m_UmlClasses

                    umlClass.Render(g, 0, checkMappings)

                Next

                For Each umlLine In m_UmlLines

                    umlLine.Render(g, checkMappings)

                Next

            End If

            For Each umlClass In m_UmlClasses

                umlClass.Render(g, 1, checkMappings)

            Next

            If Not (m_Moving And m_MoveReduction > 4) Then

                For Each umlClass In GetSelectedUmlClasses()

                    umlClass.RenderSelectionFrame(g, 0)

                Next

                For Each umlLine In GetSelectedUmlLines()

                    umlLine.RenderSelectionFrame(g)

                Next

                If includeRadar Then

                    If m_DisplayRadar Then

                        DrawRadar(g)

                    End If

                End If

            End If

            If Me.MoveReduction < 10 Then

                If Moving Then

                    Dim endTime As DateTime = Now

                    Dim diff As Long = endTime.Subtract(startTime).TotalMilliseconds

                    If m_BestCount = 0 Then m_BestCount = diff

                    If diff < m_BestCount Then m_BestCount = diff

                    If diff > 100 Then

                        If diff < m_BestCount * 3 Then

                            m_SlowCount += 1

                            If m_SlowCount > 5 Then

                                Me.MoveReduction += 1

                                m_SlowCount = 0
                                m_BestCount = 0

                            End If

                        End If

                    Else

                        m_SlowCount = 0
                        m_BestCount = 0

                    End If

                End If

            End If

        End Sub



        Private Sub DrawGrid(ByVal g As Graphics, Optional ByVal pass As Byte = 0)

            If Not m_UseGrid Then Exit Sub

            Dim i As Long
            Dim NrOfLines As Long = 10
            Dim LineSkip As Long = m_GridSize
            Dim sign As Integer

            If LineSkip < 1 Then LineSkip = 1
            If LineSkip > 256 Then LineSkip = 256
            Dim Offset As Long

            Dim size As SizeF = g.VisibleClipBounds.Size

            LineSkip = LineSkip * Zoom

            NrOfLines = CLng(size.Width / LineSkip) + 1

            Offset = -Location.X  '* Zoom

            If pass = 1 Then

                Offset += LineSkip / 2

            End If

            If Offset > 0 Then
                sign = 1
            Else
                sign = -1
            End If

            Offset = Math.Abs(Offset)

            If Offset > LineSkip Then

                Offset = CLng(Offset Mod LineSkip)

            End If

            Offset = Offset * sign

            For i = 0 To NrOfLines

                DrawGridLine(pass, (i * LineSkip) + Offset, 0, (i * LineSkip) + Offset, size.Height, g)

            Next

            NrOfLines = CLng(size.Height / LineSkip) + 1

            Offset = -Location.Y '* Zoom

            If pass = 1 Then

                Offset += LineSkip / 2

            End If

            If Offset > 0 Then
                sign = 1
            Else
                sign = -1
            End If

            Offset = Math.Abs(Offset)

            If Offset > LineSkip Then

                Offset = CLng(Offset Mod LineSkip)

            End If

            Offset = Offset * sign

            For i = 0 To NrOfLines

                DrawGridLine(pass, 0, (i * LineSkip) + Offset, size.Width, (i * LineSkip) + Offset, g)

            Next

        End Sub


        Private Sub DrawGridLine( _
            ByVal pass As Byte, _
            ByVal FromX As Long, ByVal FromY As Long, ByVal ToX As Long, ByVal ToY As Long, _
            ByVal g As Graphics)


            Dim clrColor As Color
            Dim lngLineWidth As Long

            Dim Zoom As Double

            'Zoom = dalDiagram.Zoom

            'lngLineWidth = dalDiagram.LineWidth * Zoom

            If pass = 0 Then

                clrColor = GridColor1.ToColor

            Else

                clrColor = GridColor2.ToColor

            End If

            Dim gfxPen As New Pen(clrColor, lngLineWidth)
            Dim startPoint As New Point(FromX, FromY)
            Dim endPoint As New Point(ToX, ToY)

            g.DrawLine(gfxPen, startPoint, endPoint)

        End Sub

        Public Function HitTest(ByVal x As Integer, ByVal y As Integer, ByRef hitObjects As ArrayList, ByRef lineStart As Boolean, ByRef lineEnd As Boolean, ByRef hitPropertyMap As IPropertyMap) As UmlClass

            Dim umlClass As umlClass

            hitObjects.Clear()

            HitTestLine(x, y, hitObjects, lineStart, lineEnd)

            If Not hitObjects.Count > 0 Then

                umlClass = HitTestClass(x, y, hitPropertyMap)

                If Not umlClass Is Nothing Then

                    hitObjects.Add(umlClass)

                End If

            End If

            If Not hitObjects.Count > 0 Then

                hitObjects.Add(Me)

            End If

        End Function

        Public Function HitTestClass(ByVal x As Integer, ByVal y As Integer, ByRef hitPropertyMap As IPropertyMap) As UmlClass

            Dim umlClass As umlClass

            Dim i As Integer

            For i = m_UmlClasses.Count - 1 To 0 Step -1

                umlClass = m_UmlClasses(i)

                If umlClass.HitTest(x, y, hitPropertyMap) Then

                    Return umlClass

                End If

            Next

            Return Nothing

        End Function

        Public Sub HitTestLine(ByVal x As Integer, ByVal y As Integer, ByRef hitObjects As ArrayList, ByRef lineStart As Boolean, ByRef lineEnd As Boolean)

            Dim umlLine As umlLine

            Dim i As Integer

            For i = m_UmlLines.Count - 1 To 0 Step -1

                umlLine = m_UmlLines(i)

                umlLine.HitTest(x, y, hitObjects, lineStart, lineEnd)

                If hitObjects.Count > 0 Then

                    Exit Sub

                End If

            Next

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

            Return Name

        End Function

        Public Overrides Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            'Lines need to go first,
            'because they are connected to the umlClasses, 
            'that have to have their names updated after the lines...
            For Each umlLine In m_UmlLines

                umlLine.UpdateMapObjectName(mapObject, newName)

            Next

            For Each umlClass In m_UmlClasses

                umlClass.UpdateMapObjectName(mapObject, newName)

            Next

        End Sub


        Public Sub MoveSelected(ByVal X As Integer, ByVal Y As Integer, ByVal lastX As Integer, ByVal lastY As Integer)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            If m_Selected Then

                Dim diffX As Integer = X - lastX
                Dim diffY As Integer = Y - lastY

                Dim zoom As Double = m_Zoom

                If zoom = 0 Then zoom = 1

                'Me.Location = New Point(Me.Location.X - CInt(diffX / zoom), Me.Location.Y - CInt(diffY / zoom))
                Me.Location = New Point(Me.Location.X - diffX, Me.Location.Y - diffY)

            Else

                For Each umlClass In GetSelectedUmlClasses()

                    umlClass.MoveSelected(X, Y, lastX, lastY)

                Next

                For Each umlLine In GetSelectedUmlLines()

                    umlLine.MoveSelected(X, Y, lastX, lastY)

                Next

            End If

        End Sub

        Public Overloads Sub DeselectAll()

            DeselectAll(New ArrayList, False, False)

        End Sub

        Public Overloads Sub DeselectAll(ByVal exceptObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            For Each umlClass In m_UmlClasses

                umlClass.DeselectAll(exceptObjects)

            Next

            For Each umlLine In m_UmlLines

                umlLine.DeselectAll(exceptObjects, lineStart, lineEnd)

            Next

            If Not exceptObjects.Contains(Me) Then

                m_Selected = False

            End If

        End Sub

        Public Overloads Sub SelectAll()

            m_Selected = False '????

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            For Each umlClass In m_UmlClasses

                umlClass.SelectAll()

            Next

            For Each umlLine In m_UmlLines

                umlLine.SelectAll()

            Next

        End Sub

        Public Overloads Sub SelectAll(ByVal onObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            For Each umlClass In m_UmlClasses

                umlClass.SelectAll(onObjects)

            Next

            For Each umlLine In m_UmlLines

                umlLine.SelectAll(onObjects, lineStart, lineEnd)

            Next

            If onObjects.Contains(Me) Then

                m_Selected = True

            End If

        End Sub


        Public Overloads Sub FlipSelection(ByVal onObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim umlClass As umlClass
            Dim umlLine As umlLine

            For Each umlClass In m_UmlClasses

                umlClass.FlipSelection(onObjects)

            Next

            For Each umlLine In m_UmlLines

                umlLine.FlipSelection(onObjects, lineStart, lineEnd)

            Next

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


        Public Function AreAllSelected(ByVal onObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean) As Boolean

            Dim umlClass As umlClass
            Dim umlLine As umlLine
            Dim linePoint As UmlLinePoint

            If onObjects.Count < 1 Then Return False

            If onObjects.Contains(Me) Then

                If Not Me.Selected Then

                    Return False

                End If

            End If

            For Each umlClass In m_UmlClasses

                If onObjects.Contains(umlClass) Then

                    If Not umlClass.Selected Then

                        Return False

                    End If

                End If

            Next

            For Each umlLine In m_UmlLines

                If onObjects.Contains(umlLine) Then

                    If lineStart Then

                        If Not umlLine.StartSelected Then

                            Return False

                        End If

                    End If

                    If lineEnd Then

                        If Not umlLine.EndSelected Then

                            Return False

                        End If

                    End If

                End If

                For Each linePoint In umlLine.UmlLinePoints

                    If onObjects.Contains(linePoint) Then

                        If Not linePoint.Selected Then

                            Return False

                        End If

                    End If

                Next

            Next

            Return True

        End Function



        Public Function GetTotalRectangle(Optional ByVal includeLocation As Boolean = False, Optional ByVal unZoomed As Boolean = False, Optional ByVal includeLinePoints As Boolean = False) As Rectangle

            Dim loc As Point
            Dim s As Size

            Dim umlClass As umlClass
            Dim umlLine As umlLine
            Dim linePoint As UmlLinePoint

            Dim minX As Integer = 0
            Dim maxX As Integer = 0
            Dim minY As Integer = 0
            Dim maxY As Integer = 0

            Dim first As Boolean = True

            If includeLocation Then

                minY = Location.Y
                minX = Location.X
                maxY = Location.Y + Size.Height
                maxX = Location.X + Size.Width


                first = False

            End If

            For Each umlClass In m_UmlClasses

                If unZoomed Then

                    loc = umlClass.Location
                    s = umlClass.Size

                Else

                    loc = New Point(CInt(umlClass.Location.X * Zoom), CInt(umlClass.Location.Y * Zoom))
                    s = umlClass.GetSize

                End If

                If first Then

                    minY = loc.Y
                    minX = loc.X
                    maxY = loc.Y + s.Height
                    maxX = loc.X + s.Width

                    first = False

                Else

                    If loc.Y < minY Then minY = loc.Y
                    If loc.X < minX Then minX = loc.X
                    If loc.Y + s.Height > maxY Then maxY = loc.Y + s.Height
                    If loc.X + s.Width > maxX Then maxX = loc.X + s.Width

                End If

            Next

            If includeLinePoints Then

                For Each umlLine In m_UmlLines

                    For Each linePoint In umlLine.UmlLinePoints

                        If unZoomed Then

                            loc = linePoint.GetLocation()

                        Else

                            loc = New Point(CInt(linePoint.X * Zoom), CInt(linePoint.Y * Zoom))

                        End If

                        If first Then

                            minY = loc.Y
                            minX = loc.X
                            maxY = loc.Y
                            maxX = loc.X

                            first = False

                        Else

                            If loc.Y < minY Then minY = loc.Y
                            If loc.X < minX Then minX = loc.X
                            If loc.Y > maxY Then maxY = loc.Y
                            If loc.X > maxX Then maxX = loc.X

                        End If

                    Next

                Next


            End If

            Return New Rectangle(minX, minY, maxX - minX, maxY - minY)

        End Function


        Public Function DrawRadar(ByVal g As Graphics)

            Dim s As Size = New Size(m_RadarSize, m_RadarSize)
            Dim loc As Point = New Point(Size.Width - s.Width - 10, Size.Height - s.Height - 10)

            Dim radarRect As Rectangle = New Rectangle(loc, s)

            Dim totalRect As Rectangle = GetTotalRectangle(True)
            Dim classRect As Rectangle = GetTotalRectangle()
            Dim clsRect As Rectangle = GetTotalRectangle()
            Dim clsLoc As Point
            Dim clsSize As Size

            Dim diff As Double
            Dim offsetClass As Size
            Dim offsetWin As Size
            Dim offsetCls As Size

            Dim scaledTotalRect As Rectangle
            Dim scaledClassRect As Rectangle
            Dim scaledWinRect As Rectangle

            Dim umlClass As umlClass

            Dim winPen As Pen
            Dim clsPen As Pen = New Pen(RadarClassForeColor.ToColor)
            Dim clsBrush As Brush = New SolidBrush(RadarClassBackColor.ToColor)

            If totalRect.Height > totalRect.Width Then

                diff = totalRect.Height / s.Height

            Else

                diff = totalRect.Width / s.Width

            End If

            offsetWin = New Size(Location.X - totalRect.X, Location.Y - totalRect.Y)
            offsetClass = New Size(classRect.X - totalRect.X, classRect.Y - totalRect.Y)

            scaledTotalRect = New Rectangle(loc, New Size(CInt(totalRect.Width / diff), CInt(totalRect.Height / diff)))
            scaledClassRect = New Rectangle(loc.X + CInt(offsetClass.Width / diff), loc.Y + CInt(offsetClass.Height / diff), CInt(classRect.Width / diff), CInt(classRect.Height / diff))
            scaledWinRect = New Rectangle(loc.X + CInt(offsetWin.Width / diff), loc.Y + CInt(offsetWin.Height / diff), CInt(Size.Width / diff), CInt(Size.Height / diff))

            'g.FillRectangle(New SolidBrush(Color.White), radarRect)
            g.FillRectangle(New SolidBrush(RadarBackColor.ToColor), radarRect)

            g.DrawRectangle(New Pen(RadarForeColor.ToColor), radarRect.X - 1, radarRect.Y - 1, radarRect.Width + 2, radarRect.Height + 2)

            For Each umlClass In m_UmlClasses

                clsLoc = New Point(CInt(umlClass.Location.X * Zoom), CInt(umlClass.Location.Y * Zoom))
                clsSize = umlClass.GetSize

                offsetCls = New Size(clsLoc.X - totalRect.X, clsLoc.Y - totalRect.Y)

                clsRect = New Rectangle(loc.X + (offsetCls.Width / diff), loc.Y + (offsetCls.Height / diff), clsSize.Width / diff, clsSize.Height / diff)

                g.FillRectangle(clsBrush, clsRect)
                g.DrawRectangle(clsPen, clsRect)

            Next

            'g.DrawRectangle(New Pen(Color.Green), scaledClassRect)
            winPen = New Pen(RadarViewColor.ToColor)
            winPen.DashStyle = RadarViewStyle
            g.DrawRectangle(winPen, scaledWinRect)

        End Function

        Public Sub Fit(ByVal g As Graphics)

            Dim lastZoom As Double
            Dim i As Integer

            While Not ((lastZoom > (m_Zoom - 0.01) And lastZoom < (m_Zoom + 0.01)) Or i > 10)

                DoFit()

                CalcSizes(g)

                DoFit()

                i += 1

            End While


        End Sub

        Private Sub DoFit()

            Dim totalRect As Rectangle = GetTotalRectangle(False, True)

            If totalRect.Height = 0 And totalRect.Width = 0 Then Exit Sub

            Dim margin As Integer = 20

            totalRect = New Rectangle(totalRect.X - margin, totalRect.Y - margin, totalRect.Width + (2 * margin), totalRect.Height + (2 * margin))

            Dim diff As Double

            If totalRect.Height > totalRect.Width Then

                'diff = totalRect.Height / Size.Height
                diff = Size.Height / totalRect.Height

            Else

                'diff = totalRect.Width / Size.Width
                diff = Size.Width / totalRect.Width

            End If

            Me.Location = New Point(totalRect.X * diff, totalRect.Y * diff)

            m_Zoom = diff

        End Sub

        Private Sub CalcSizes(ByVal g As Graphics)

            Dim umlClass As umlClass

            For Each umlClass In m_UmlClasses

                umlClass.CalcSize(g)

            Next

        End Sub

        Public Sub SelectAllInsideRectangle(ByVal rect As Rectangle)

            Dim umlClass As umlClass
            Dim umlLine As umlLine
            Dim umlLinePoint As umlLinePoint
            Dim loc As Point
            Dim sz As Size

            For Each umlClass In m_UmlClasses

                loc = umlClass.GetLocation
                sz = umlClass.GetSize

                If IsInsideRectangle(loc, sz, rect) Then

                    umlClass.Selected = True

                End If

            Next

            For Each umlLine In m_UmlLines

                loc = umlLine.GetStartPoint

                If IsInsideRectangle(loc, rect) Then

                    umlLine.StartSelected = True

                End If

                loc = umlLine.GetEndPoint

                If IsInsideRectangle(loc, rect) Then

                    umlLine.EndSelected = True

                End If

                For Each umlLinePoint In umlLine.UmlLinePoints

                    loc = umlLinePoint.GetLocation

                    If IsInsideRectangle(loc, rect) Then

                        umlLinePoint.Selected = True

                    End If

                Next

            Next

        End Sub

        Private Overloads Function IsInsideRectangle(ByVal loc As Point, ByVal sz As Size, ByVal rect As Rectangle, Optional ByVal allInside As Boolean = True) As Boolean

            Dim ok As Boolean

            If allInside Then

                ok = True

                If Not IsInsideRectangle(loc, rect) Then ok = False

                If Not IsInsideRectangle(New Point(loc.X + sz.Width, loc.Y), rect) Then ok = False

                If Not IsInsideRectangle(New Point(loc.X, loc.Y + sz.Height), rect) Then ok = False

                If Not IsInsideRectangle(New Point(loc.X + sz.Width, loc.Y + sz.Height), rect) Then ok = False

                Return ok

            Else

                If IsInsideRectangle(loc, rect) Then Return True

                If IsInsideRectangle(New Point(loc.X + sz.Width, loc.Y), rect) Then Return True

                If IsInsideRectangle(New Point(loc.X, loc.Y + sz.Height), rect) Then Return True

                If IsInsideRectangle(New Point(loc.X + sz.Width, loc.Y + sz.Height), rect) Then Return True

            End If

        End Function

        Private Overloads Function IsInsideRectangle(ByVal loc As Point, ByVal rect As Rectangle) As Boolean

            If loc.X >= rect.X And loc.X <= (rect.X + rect.Width) Then

                If loc.Y >= rect.Y And loc.Y <= (rect.Y + rect.Height) Then

                    Return True

                End If

            End If

        End Function


        Public Sub ZoomInOut(ByVal stepSize As Double)

            If Zoom + stepSize < m_MazZoomSize And Zoom + stepSize > 0 Then

                Zoom += stepSize

            End If

        End Sub

        Public Overrides Sub Accept(ByVal mapVisitor As Puzzle.NPersist.Framework.Mapping.Visitor.IMapVisitor)

        End Sub
    End Class

End Namespace
