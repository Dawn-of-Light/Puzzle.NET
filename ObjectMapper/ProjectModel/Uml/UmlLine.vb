Imports System.Xml.Serialization
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Mapping.Visitor
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace Uml

    Public Enum Direction
        Left = 0
        Up = 1
        Right = 2
        Down = 3
    End Enum

    Public Enum LineTypeEnum
        Association = 0
        Generalization = 1
    End Enum

    Public Enum AggregationEnum
        None = 0
        Aggregation = 1
        Composition = 2
    End Enum

    Public Class UmlLine
        Inherits MapBase


        Private m_Name As String

        Private m_LineType As LineTypeEnum = LineTypeEnum.Association

        Private m_StartClass As String
        Private m_StartProperty As String
        Private m_EndClass As String
        Private m_EndProperty As String

        Private m_UmlDiagram As UmlDiagram

        Private m_UmlLinePoints As New ArrayList

        Private m_StartSide As Direction = Direction.Up
        Private m_StartSlot As Integer = 0

        Private m_EndSide As Direction = Direction.Up
        Private m_EndSlot As Integer = 0

        Private m_FixedStart As Boolean = False
        Private m_FixedEnd As Boolean = False

        Private m_StartAggregation As AggregationEnum = AggregationEnum.None
        Private m_EndAggregation As AggregationEnum = AggregationEnum.None

        Private m_HasSynchError As Boolean
        Private m_HasError As Boolean

        Private m_listExceptions As ArrayList
        Private m_propertyExceptions As New Hashtable

        Private m_UnConnected As Boolean

        Private m_StartSelected As Boolean = False
        Private m_EndSelected As Boolean = False

        'Inheritable
        Private m_InheritSettings As Boolean = True

        Private m_ForeColor As UmlColor = New UmlColor(Color.Black)
        Private m_BackColor1 As UmlColor = New UmlColor(Color.White)

        Private m_FontSize As Single = 8.25
        Private m_FontFamily As String = "Microsoft Sans Serif"
        Private m_FontColor As UmlColor = New UmlColor(Color.Black)
        Private m_FontStyle As FontStyle = FontStyle.Regular


        'Inheritable

        <XmlIgnore()> Public Property MoveReduction() As Byte
            Get
                Return m_UmlDiagram.MoveReduction
            End Get
            Set(ByVal Value As Byte)
                m_UmlDiagram.MoveReduction = Value
            End Set
        End Property

        <XmlIgnore()> Public Property UmlDiagram() As UmlDiagram
            Get
                Return m_UmlDiagram
            End Get
            Set(ByVal Value As UmlDiagram)
                If Not m_UmlDiagram Is Nothing Then
                    m_UmlDiagram.UmlLines.Remove(Me)
                End If
                m_UmlDiagram = Value
                If Not m_UmlDiagram Is Nothing Then
                    m_UmlDiagram.UmlLines.Add(Me)
                End If
            End Set
        End Property


        <XmlIgnore()> Public Property ListExceptions() As ArrayList
            Get
                Return m_listExceptions
            End Get
            Set(ByVal Value As ArrayList)
                m_listExceptions = Value
            End Set
        End Property

        Public Overridable Sub SetUmlDiagram(ByVal Value As UmlDiagram)

            m_UmlDiagram = Value

        End Sub


        Public Property StartProperty() As String
            Get
                Return m_StartProperty
            End Get
            Set(ByVal Value As String)
                m_StartProperty = Value
            End Set
        End Property


        Public Property EndClass() As String
            Get
                Return m_EndClass
            End Get
            Set(ByVal Value As String)
                m_EndClass = Value
            End Set
        End Property


        Public Property EndProperty() As String
            Get
                Return m_EndProperty
            End Get
            Set(ByVal Value As String)
                m_EndProperty = Value
            End Set
        End Property


        Public Property StartClass() As String
            Get
                Return m_StartClass
            End Get
            Set(ByVal Value As String)
                m_StartClass = Value
            End Set
        End Property


        <XmlArrayItem(GetType(UmlLinePoint))> Public Property UmlLinePoints() As ArrayList
            Get
                Return m_UmlLinePoints
            End Get
            Set(ByVal Value As ArrayList)
                m_UmlLinePoints = Value
            End Set
        End Property

        Public Property StartSide() As Direction
            Get
                Return m_StartSide
            End Get
            Set(ByVal Value As Direction)
                m_StartSide = Value
            End Set
        End Property

        Public Property StartSlot() As Integer
            Get
                Return m_StartSlot
            End Get
            Set(ByVal Value As Integer)
                m_StartSlot = Value
            End Set
        End Property


        Public Property EndSide() As Direction
            Get
                Return m_EndSide
            End Get
            Set(ByVal Value As Direction)
                m_EndSide = Value
            End Set
        End Property

        Public Property EndSlot() As Integer
            Get
                Return m_EndSlot
            End Get
            Set(ByVal Value As Integer)
                m_EndSlot = Value
            End Set
        End Property

        Public Property FixedStart() As Boolean
            Get
                Return m_FixedStart
            End Get
            Set(ByVal Value As Boolean)
                m_FixedStart = Value
            End Set
        End Property

        Public Property FixedEnd() As Boolean
            Get
                Return m_FixedEnd
            End Get
            Set(ByVal Value As Boolean)
                m_FixedEnd = Value
            End Set
        End Property

        Public Property StartAggregation() As AggregationEnum
            Get
                Return m_StartAggregation
            End Get
            Set(ByVal Value As AggregationEnum)
                m_StartAggregation = Value
            End Set
        End Property

        Public Property EndAggregation() As AggregationEnum
            Get
                Return m_EndAggregation
            End Get
            Set(ByVal Value As AggregationEnum)
                m_EndAggregation = Value
            End Set
        End Property

        Public Property UnConnected() As Boolean
            Get
                Return m_UnConnected
            End Get
            Set(ByVal Value As Boolean)
                m_UnConnected = Value
            End Set
        End Property

        Public Property StartSelected() As Boolean
            Get
                Return m_StartSelected
            End Get
            Set(ByVal Value As Boolean)
                m_StartSelected = Value
            End Set
        End Property

        Public Property EndSelected() As Boolean
            Get
                Return m_EndSelected
            End Get
            Set(ByVal Value As Boolean)
                m_EndSelected = Value
            End Set
        End Property


        Public Property InheritSettings() As Boolean
            Get
                Return m_InheritSettings
            End Get
            Set(ByVal Value As Boolean)
                m_InheritSettings = Value
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



        Public Property BackColor1() As UmlColor
            Get
                Return m_BackColor1
            End Get
            Set(ByVal Value As UmlColor)
                m_BackColor1 = Value
            End Set
        End Property


        Public Function GetArrowBaseLength() As Double
            Return m_UmlDiagram.ArrowBaseLength * GetZoom()
        End Function

        Public Function GetColorError() As Color
            Return m_UmlDiagram.ColorError.ToColor
        End Function

        Public Function GetColorSynchError() As Color
            Return m_UmlDiagram.ColorSynchError.ToColor
        End Function


        Public Function GetForeColor() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.ForeColor.ToColor
            Else
                Return ForeColor.ToColor
            End If
        End Function

        Public Function GetFontSize() As Single
            If m_InheritSettings Then
                Return m_UmlDiagram.FontSize
            Else
                Return m_FontSize
            End If
        End Function

        Public Function GetFontFamily() As String
            If m_InheritSettings Then
                Return m_UmlDiagram.FontFamily
            Else
                Return m_FontFamily
            End If
        End Function

        Public Function GetFontColor() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.FontColor.ToColor
            Else
                Return FontColor.ToColor
            End If
        End Function

        Public Function GetFontStyle() As FontStyle
            If m_InheritSettings Then
                Return m_UmlDiagram.FontStyle
            Else
                Return m_FontStyle
            End If
        End Function



        Public Function GetBackColor1() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.BackColor1.ToColor
            Else
                Return BackColor1.ToColor
            End If
        End Function




        Public Function HasSelected() As Boolean

            If m_StartSelected Then Return True
            If m_EndSelected Then Return True

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                If linePoint.Selected Then

                    Return True

                End If

            Next

        End Function

        Public Function GetPoint(ByVal index As Integer) As Point

            If index = 0 Then

                Return GetStartPoint()

            Else

                If index > m_UmlLinePoints.Count + 1 Then

                    Return Point.Empty

                ElseIf index > m_UmlLinePoints.Count Then

                    Return GetEndPoint()

                Else

                    Dim umlLinePoint As umlLinePoint = m_UmlLinePoints(index - 1)

                    'Return New Point(umlLinePoint.X, umlLinePoint.Y)
                    Return umlLinePoint.GetLocation

                End If

            End If

        End Function


        Public Function IsSelectedPoint(ByVal index As Integer) As Boolean

            If index = 0 Then

                Return m_StartSelected

            Else

                If index > m_UmlLinePoints.Count + 1 Then

                    Return False

                ElseIf index > m_UmlLinePoints.Count Then

                    Return m_EndSelected

                Else

                    Dim umlLinePoint As umlLinePoint = m_UmlLinePoints(index - 1)

                    Return umlLinePoint.Selected

                End If

            End If

        End Function

        Public Function GetNrOfPoints()

            Return 2 + m_UmlLinePoints.Count

        End Function

        Public Function GetDomainMap() As IDomainMap

            Return UmlDiagram.GetDomainMap

        End Function

        <XmlIgnore()> Public Property Moving() As Boolean
            Get
                Return m_UmlDiagram.Moving
            End Get
            Set(ByVal Value As Boolean)
                m_UmlDiagram.Moving = Value
            End Set
        End Property

        Private m_StartUmlClass As UmlClass

        Public Function GetStartUmlClass() As UmlClass

            If Me.Moving Then

                Return m_StartUmlClass

            End If

            Dim umlClass As umlClass

            For Each umlClass In UmlDiagram.UmlClasses

                If LCase(umlClass.Name) = LCase(StartClass) Then

                    m_StartUmlClass = umlClass
                    Return umlClass

                End If

            Next

            m_StartUmlClass = Nothing

        End Function

        Private m_EndUmlClass As UmlClass

        Public Function GetEndUmlClass() As UmlClass

            If Me.Moving Then

                Return m_EndUmlClass

            End If

            Dim umlClass As umlClass

            For Each umlClass In UmlDiagram.UmlClasses

                If LCase(umlClass.Name) = LCase(EndClass) Then

                    m_EndUmlClass = umlClass
                    Return umlClass

                End If

            Next

            m_EndUmlClass = Nothing

        End Function


        Function GetStartClassMap() As IClassMap

            Dim umlClass As umlClass = GetStartUmlClass()

            If Not umlClass Is Nothing Then

                Return umlClass.GetClassMap

            End If

        End Function


        Function GetEndClassMap() As IClassMap

            Dim umlClass As umlClass = GetEndUmlClass()

            If Not umlClass Is Nothing Then

                Return umlClass.GetClassMap

            End If

        End Function

        Private m_StartPropertyMap As IPropertyMap

        Function GetStartPropertyMap() As IPropertyMap

            If Me.Moving Then

                Return m_StartPropertyMap

            End If

            Dim umlClass As umlClass = GetStartUmlClass()
            Dim classMap As IClassMap

            If Not umlClass Is Nothing Then

                classMap = umlClass.GetClassMap

                If Not classMap Is Nothing Then

                    m_StartPropertyMap = classMap.GetPropertyMap(StartProperty)
                    Return m_StartPropertyMap

                End If

            End If

            m_StartPropertyMap = Nothing

        End Function

        Private m_EndPropertyMap As IPropertyMap

        Function GetEndPropertyMap() As IPropertyMap

            If Me.Moving Then

                Return m_EndPropertyMap

            End If

            Dim umlClass As umlClass = GetEndUmlClass()
            Dim classMap As IClassMap

            If Not umlClass Is Nothing Then

                classMap = umlClass.GetClassMap

                If Not classMap Is Nothing Then

                    m_EndPropertyMap = classMap.GetPropertyMap(EndProperty)
                    Return m_EndPropertyMap

                End If

            End If

            m_EndPropertyMap = Nothing

        End Function

        Public Function GetArrowLength() As Double

            Return 20 * GetZoom()

        End Function

        Public Sub SetSlotsAndSides()

            If Not m_FixedStart Then

                SetStartSlotAndSide()

            End If

            If Not m_FixedEnd Then

                SetEndSlotAndSide()

            End If

        End Sub

        Public Overloads Sub SetStartSlotAndSide()

            SetStartSlotAndSide(Point.Empty)

        End Sub

        Public Overloads Sub SetStartSlotAndSide(ByVal toPoint As Point)

            If Me.Moving Then

                Return

            End If

            Dim fromPoint As Point
            Dim location As Point
            Dim size As size
            Dim location2 As Point
            Dim size2 As size

            Dim side As Integer
            Dim slot As Integer
            Dim umlClass As umlClass
            Dim umlClass2 As umlClass

            Dim umlLinePoint As umlLinePoint

            umlClass = GetStartUmlClass()

            If Not umlClass Is Nothing Then

                location = umlClass.GetLocation
                size = umlClass.GetSize

                fromPoint = New Point(location.X + CInt(size.Width / 2), location.Y + CInt(size.Height / 2))

                If toPoint.Equals(Point.Empty) Then

                    If m_UmlLinePoints.Count > 0 Then

                        umlLinePoint = m_UmlLinePoints(0)

                        toPoint = umlLinePoint.GetLocation

                    Else

                        umlClass2 = GetEndUmlClass()

                        If Not umlClass2 Is Nothing Then

                            location2 = umlClass2.GetLocation
                            size2 = umlClass2.GetSize

                            toPoint = New Point(location2.X + CInt(size2.Width / 2), location2.Y + CInt(size2.Height / 2))

                        End If

                    End If

                End If

                GetSlotAndSide(umlClass, fromPoint, toPoint, side, slot)

                StartSide = side
                StartSlot = slot

            End If

        End Sub

        Public Overloads Sub SetEndSlotAndSide()

            SetEndSlotAndSide(Point.Empty)

        End Sub

        Public Overloads Sub SetEndSlotAndSide(ByVal toPoint As Point)

            If Moving Then Return

            Dim umlClass As umlClass = GetEndUmlClass()
            Dim umlClass2 As umlClass
            Dim fromPoint As Point
            Dim location As Point
            Dim size As size
            Dim location2 As Point
            Dim size2 As size

            Dim side As Integer
            Dim slot As Integer

            Dim umlLinePoint As umlLinePoint

            If Not umlClass Is Nothing Then

                location = umlClass.GetLocation
                size = umlClass.GetSize

                fromPoint = New Point(location.X + CInt(size.Width / 2), location.Y + CInt(size.Height / 2))

                If toPoint.Equals(Point.Empty) Then

                    If m_UmlLinePoints.Count > 0 Then

                        umlLinePoint = m_UmlLinePoints(m_UmlLinePoints.Count - 1)

                        toPoint = umlLinePoint.GetLocation

                    Else

                        umlClass2 = GetStartUmlClass()

                        If Not umlClass2 Is Nothing Then

                            location2 = umlClass2.GetLocation
                            size2 = umlClass2.GetSize

                            toPoint = New Point(location2.X + CInt(size2.Width / 2), location2.Y + CInt(size2.Height / 2))

                        End If

                    End If

                End If

                GetSlotAndSide(umlClass, fromPoint, toPoint, side, slot)

                EndSide = side
                EndSlot = slot

            End If

        End Sub

        'Private Sub GetSlotAndSide(ByVal umlClass As UmlClass, ByVal startPoint As Point, ByVal endPoint As Point, ByRef side As Integer, ByRef slot As Integer)

        '    Dim lngSocketNr As Long = 1

        '    Dim slotsPerSide As Integer
        '    Dim totalSlots As Integer

        '    Dim lngDiffX As Long
        '    Dim lngDiffY As Long

        '    Dim v As Double

        '    Dim stepSize As Integer
        '    Dim cntSlot As Integer
        '    Dim cnt As Integer

        '    lngDiffX = endPoint.X - startPoint.X
        '    'lngDiffY = startPoint.Y - endPoint.Y
        '    lngDiffY = endPoint.Y - startPoint.Y

        '    v = Math.Atan2(lngDiffY, lngDiffX)

        '    v = v * (180 / Math.PI)

        '    v = CLng(v)

        '    v += 180 + 45

        '    If v > 360 Then v = v - 360

        '    slotsPerSide = umlClass.SlotsPerSide

        '    totalSlots = (slotsPerSide + 1) * 4

        '    Dim slotNr As Integer = (totalSlots * (v / 360))

        '    slot = slotNr Mod slotsPerSide
        '    side = (slotNr / slotsPerSide) Mod 4

        '    'side = v / slotsPerSide Mod 4
        '    'slot = (v + 1) Mod slotsPerSide

        '    'slot -= 1

        '    'If slot < 0 Then

        '    '    slot = 0

        '    '    side -= 1

        '    '    If side < 0 Then side = 3

        '    'End If

        'End Sub


        Private Sub GetSlotAndSide(ByVal umlClass As UmlClass, ByVal startPoint As Point, ByVal endPoint As Point, ByRef side As Integer, ByRef slot As Integer)

            Dim lngSocketNr As Long = 1

            Dim slotsPerSide As Integer
            Dim totalSlots As Integer

            Dim lngDiffX As Long
            Dim lngDiffY As Long

            Dim v As Double
            Dim d As Double

            Dim stepSize As Integer
            Dim cntSlot As Integer
            Dim cnt As Integer

            lngDiffX = endPoint.X - startPoint.X
            'lngDiffY = startPoint.Y - endPoint.Y
            lngDiffY = endPoint.Y - startPoint.Y

            d = lngDiffY / lngDiffX
            v = Math.Atan2(lngDiffY, lngDiffX)

            v = v * (180 / Math.PI)

            v = CLng(v)

            v += 180 + 45

            If v > 360 Then v = v - 360

            slotsPerSide = umlClass.SlotsPerSide

            totalSlots = (slotsPerSide + 1) * 4

            stepSize = 360 / totalSlots

            side = 3
            slot = slotsPerSide

            For cnt = 0 To v Step stepSize

                slot += 1

                If slot > slotsPerSide Then

                    slot = 0

                    side += 1

                    If side > 3 Then side = 0

                End If

            Next

        End Sub


        Public Function GetStartPoint() As Point

            Dim umlClass As umlClass = GetStartUmlClass()
            Dim x As Integer
            Dim y As Integer
            Dim location As Point
            Dim size As size
            Dim maxSlot As Integer
            Dim div As Decimal

            If Not umlClass Is Nothing Then

                location = umlClass.GetLocation
                size = umlClass.GetSize

                maxSlot = umlClass.SlotsPerSide

                If maxSlot > 0 Then

                    div = m_StartSlot / maxSlot

                Else

                    div = 0

                End If

                Select Case m_StartSide

                    Case Direction.Up

                        y = location.Y
                        x = location.X + CInt(div * size.Width)

                    Case Direction.Right

                        y = location.Y + CInt(div * size.Height)
                        x = location.X + size.Width

                    Case Direction.Down

                        y = location.Y + size.Height
                        x = location.X + size.Width - CInt(div * size.Width)

                    Case Direction.Left

                        y = location.Y + size.Height - CInt(div * size.Height)
                        x = location.X

                End Select

                Return New Point(x, y)

            End If

            Return Point.Empty

        End Function

        Public Function GetEndPoint() As Point

            Dim umlClass As umlClass = GetEndUmlClass()
            Dim x As Integer
            Dim y As Integer
            Dim location As Point
            Dim size As size
            Dim maxSlot As Integer
            Dim div As Decimal

            If Not umlClass Is Nothing Then

                location = umlClass.GetLocation
                size = umlClass.GetSize

                maxSlot = umlClass.SlotsPerSide

                If maxSlot > 0 Then

                    div = m_EndSlot / maxSlot

                Else

                    div = 0

                End If

                Select Case m_EndSide

                    Case Direction.Up

                        y = location.Y
                        x = location.X + CInt(div * size.Width)

                    Case Direction.Right

                        y = location.Y + CInt(div * size.Height)
                        x = location.X + size.Width

                    Case Direction.Down

                        y = location.Y + size.Height
                        x = location.X + size.Width - CInt(div * size.Width)

                    Case Direction.Left

                        y = location.Y + size.Height - CInt(div * size.Height)
                        x = location.X


                End Select

                Return New Point(x, y)

            End If

            Return Point.Empty

        End Function

        Public Function HasStartDiamond() As Boolean

            If m_LineType = LineTypeEnum.Association Then

                If Not m_StartAggregation = AggregationEnum.None Then

                    Return True

                End If

            End If

            Return False

        End Function

        Public Function HasEndDiamond() As Boolean

            If m_LineType = LineTypeEnum.Association Then

                If Not m_EndAggregation = AggregationEnum.None Then

                    Return True

                End If

            End If

            Return False

        End Function


        Public Function HasStartArrow() As Boolean

            If Not m_UmlDiagram Is Nothing Then

                Return m_UmlDiagram.DisplayAssociationArrows

            End If

        End Function

        Public Function HasEndArrow() As Boolean

            If Not m_UmlDiagram Is Nothing Then

                Return m_UmlDiagram.DisplayAssociationArrows

            End If

        End Function

        Public Overridable Sub Verify(ByVal checkMappings As Boolean)

            Dim visitor As New MapVerificationVisitor(False, checkMappings)

            Accept(visitor)

        End Sub

        Public Overrides Sub Accept(ByVal verificationVisitor As IMapVisitor)

            Dim visitor As MapVerificationVisitor = verificationVisitor

            m_HasSynchError = False
            m_HasError = False
            m_UnConnected = False

            Dim domainMap As IDomainMap = GetDomainMap()
            Dim startUmlClass As UmlClass
            Dim endUmlClass As UmlClass
            Dim startPropertyMap As IPropertyMap
            Dim endPropertyMap As IPropertyMap

            Select Case m_LineType

                Case LineTypeEnum.Association, LineTypeEnum.Generalization

                    startUmlClass = GetStartUmlClass()

                    If startUmlClass Is Nothing Then

                        m_HasSynchError = True
                        m_UnConnected = True

                        Exit Sub

                    End If

                    endUmlClass = GetEndUmlClass()

                    If endUmlClass Is Nothing Then

                        m_HasSynchError = True
                        m_UnConnected = True

                        Exit Sub

                    End If

            End Select

            Select Case m_LineType

                Case LineTypeEnum.Association, LineTypeEnum.Generalization

                    If domainMap Is Nothing Then

                        m_HasSynchError = True
                        visitor.Exceptions.Add(New MappingException("The diagram that this line belongs to is not sunched with a domain map!", Me, "Domain Map"))

                    Else

                        Dim startClassMap As IClassMap = GetStartClassMap()

                        If startClassMap Is Nothing Then

                            m_HasSynchError = True
                            visitor.Exceptions.Add(New MappingException("A class with the name '" & m_StartClass & "' does not exist in the domain '" & domainMap.Name & "'", Me, "Start Class"))

                        Else

                            Select Case m_LineType

                                Case LineTypeEnum.Association

                                    If m_StartProperty = "" And m_EndProperty = "" Then

                                        'Done below and enough to do once
                                        'm_HasSynchError = True
                                        'listExceptions.Add(New MappingException("The line association is not connected to any property", Me, "Start Property"))

                                    Else

                                        If Len(m_StartProperty) > 0 Then


                                            startPropertyMap = GetStartPropertyMap()

                                            If startPropertyMap Is Nothing Then

                                                m_HasSynchError = True
                                                visitor.Exceptions.Add(New MappingException("A property with the name '" & m_StartProperty & "' does not exist in the class '" & startClassMap.Name & "'", Me, "Start Property"))

                                            Else

                                                startClassMap.Accept(visitor)

                                                If visitor.Exceptions.Count > 0 Then

                                                    m_HasError = True

                                                End If

                                                startPropertyMap.Accept(visitor)

                                                If visitor.Exceptions.Count > 0 Then

                                                    m_HasError = True

                                                End If

                                            End If


                                        End If

                                    End If

                                Case LineTypeEnum.Generalization

                                    'Done below and enough to do once
                                    'If Not VerifyInheritance(listExceptions) Then

                                    '    m_HasSynchError = True

                                    'Else

                                    '    startClassMap.Verify(False, listExceptions, checkMappings)

                                    '    If listExceptions.Count > 0 Then

                                    '        m_HasError = True

                                    '    End If

                                    'End If

                            End Select

                        End If

                        Dim endClassMap As IClassMap = GetEndClassMap()

                        If endClassMap Is Nothing Then

                            m_HasSynchError = True
                            visitor.Exceptions.Add(New MappingException("A class with the name '" & m_EndClass & "' does not exist in the domain '" & domainMap.Name & "'", Me, "end Class"))

                        Else

                            Select Case m_LineType

                                Case LineTypeEnum.Association


                                    If m_StartProperty = "" And m_EndProperty = "" Then

                                        m_HasSynchError = True
                                        visitor.Exceptions.Add(New MappingException("The line association is not connected to any property", Me, "Start Property"))

                                    Else

                                        If Len(m_EndProperty) > 0 Then

                                            endPropertyMap = GetEndPropertyMap()

                                            If endPropertyMap Is Nothing Then

                                                m_HasSynchError = True
                                                visitor.Exceptions.Add(New MappingException("A property with the name '" & m_EndProperty & "' does not exist in the class '" & endClassMap.Name & "'", Me, "end Property"))

                                            Else

                                                endClassMap.Accept(visitor)

                                                If visitor.Exceptions.Count > 0 Then

                                                    m_HasError = True

                                                End If

                                                endPropertyMap.Accept(visitor)

                                                If visitor.Exceptions.Count > 0 Then

                                                    m_HasError = True

                                                End If

                                            End If

                                        End If

                                    End If


                                Case LineTypeEnum.Generalization

                                    If Not VerifyInheritance(ListExceptions) Then

                                        m_HasSynchError = True

                                    Else

                                        startClassMap.Accept(visitor)

                                        If visitor.Exceptions.Count > 0 Then

                                            m_HasError = True

                                        End If

                                        endClassMap.Accept(visitor)

                                        If visitor.Exceptions.Count > 0 Then

                                            m_HasError = True

                                        End If

                                    End If


                            End Select

                        End If

                    End If

            End Select


            m_listExceptions = visitor.Exceptions

        End Sub


        Private Function VerifyInheritance(ByVal listExceptions As ArrayList) As Boolean

            Dim startClassMap As IClassMap = GetStartClassMap()
            Dim endClassMap As IClassMap = GetEndClassMap()

            If startClassMap Is Nothing Then Return False
            If endClassMap Is Nothing Then Return False

            If Len(startClassMap.InheritsClass) < 1 And Len(endClassMap.InheritsClass) < 1 Then

                listExceptions.Add(New MappingException("Neither class '" & startClassMap.Name & "' nor class '" & endClassMap.Name & "' inherits from any class.", Me, "Classes"))

                Return False

            End If


            If Len(startClassMap.InheritsClass) > 0 And Len(endClassMap.InheritsClass) > 0 Then

                If Not endClassMap Is startClassMap.GetInheritedClassMap And Not startClassMap Is endClassMap.GetInheritedClassMap Then

                    listExceptions.Add(New MappingException("Neither class '" & startClassMap.Name & "' nor class '" & endClassMap.Name & "' inherits from the other class." & vbCrLf & vbCrLf & "'" & startClassMap.Name & "' inherits from '" & startClassMap.InheritsClass & "' and '" & endClassMap.Name & "' inherits from '" & endClassMap.InheritsClass & "'", startClassMap, "InheritsClass"))

                    Return False

                End If

                'Cyclic errors should already have been checked for, but ok then...
                If endClassMap Is startClassMap.GetInheritedClassMap And startClassMap Is endClassMap.GetInheritedClassMap Then

                    listExceptions.Add(New MappingException("Classes '" & startClassMap.Name & "' and '" & endClassMap.Name & "' inherit from each other! Cyclic inheritance graph error!", startClassMap, "InheritsClass"))

                    Return False

                End If


            End If

            If Len(startClassMap.InheritsClass) > 0 Then

                If Not endClassMap Is startClassMap.GetInheritedClassMap Then

                    listExceptions.Add(New MappingException("Class '" & startClassMap.Name & "' does not inherit from class '" & endClassMap.Name & "' but from the class '" & startClassMap.InheritsClass & "'.", startClassMap, "InheritsClass"))

                    Return False

                End If

            End If

            If Len(endClassMap.InheritsClass) > 0 Then

                If Not startClassMap Is endClassMap.GetInheritedClassMap Then

                    listExceptions.Add(New MappingException("Class '" & endClassMap.Name & "' does not inherit from class '" & startClassMap.Name & "' but from the class '" & endClassMap.InheritsClass & "'.", endClassMap, "InheritsClass"))

                    Return False

                End If

            End If

            Return True

        End Function

        Public Sub RenderSelectionFrame(ByVal g As Graphics)

            Dim linePoint As UmlLinePoint

            Dim location As Point
            Dim sqSize As Integer = 6

            'draw little squares...
            Dim sqBrush As Brush = New SolidBrush(Color.LightGray)
            Dim sqPen As Pen = New Pen(Color.Black)

            If m_StartSelected Then

                location = GetStartPoint()

                'first filled...
                g.FillRectangle(sqBrush, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

                'then lines...
                g.DrawRectangle(sqPen, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

            End If

            If m_EndSelected Then

                location = GetEndPoint()

                'first filled...
                g.FillRectangle(sqBrush, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

                'then lines...
                g.DrawRectangle(sqPen, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

            End If

            For Each linePoint In m_UmlLinePoints

                If linePoint.Selected Then

                    location = linePoint.GetLocation

                    'first filled...
                    g.FillRectangle(sqBrush, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

                    'then lines...
                    g.DrawRectangle(sqPen, location.X - CInt(sqSize / 2), location.Y - CInt(sqSize / 2), sqSize, sqSize)

                End If

            Next


        End Sub


        Public Sub Render(ByVal g As Graphics, Optional ByVal checkMappings As Boolean = True)

            Verify(checkMappings)

            If m_UnConnected Then Exit Sub

            Dim pen As pen
            Dim selPen As pen
            Dim prevPoint As Point
            Dim nextPoint As Point
            Dim i As Integer = 0

            pen = GetPen()
            selPen = GetSelectedPen()

            SetSlotsAndSides()

            prevPoint = GetPoint(i)
            i += 1

            nextPoint = GetPoint(i)
            i += 1

            While Not (prevPoint.Equals(Point.Empty) Or nextPoint.Equals(Point.Empty))

                If IsSelectedPoint(i - 1) AndAlso IsSelectedPoint(i - 2) Then

                    g.DrawLine(selPen, prevPoint, nextPoint)

                Else

                    g.DrawLine(pen, prevPoint, nextPoint)

                End If

                prevPoint = nextPoint

                nextPoint = GetPoint(i)
                i += 1

            End While

            Select Case m_LineType

                Case LineTypeEnum.Association

                    RenderAssociationArrows(g)

                    If Not (Me.Moving And Me.MoveReduction > 0) Then

                        RenderAssociationLabels(g)

                    End If

                Case LineTypeEnum.Generalization

                    RenderGeneralizationArrow(g)


            End Select

        End Sub



        'Generalization

        Private Sub RenderGeneralizationArrow(ByVal g As Graphics)

            If Not VerifyInheritance(New ArrayList) Then

                Exit Sub

            End If

            RenderStartGeneralizationArrow(g)

            RenderEndGeneralizationArrow(g)

        End Sub

        Private Sub RenderStartGeneralizationArrow(ByVal g As Graphics)

            Dim startClassMap As IClassMap = GetStartClassMap()
            Dim endClassMap As IClassMap = GetEndClassMap()

            If startClassMap Is Nothing Then Exit Sub
            If endClassMap Is Nothing Then Exit Sub

            If Len(startClassMap.InheritsClass) > 0 Then

                If endClassMap Is startClassMap.GetInheritedClassMap Then

                    Exit Sub

                End If

            End If

            Dim pen As pen = GetPen()
            Dim brush As brush = GetBgBrush()

            Dim pntA As Point = GetStartPoint()
            Dim pntB As Point = GetPoint(1)
            'Dim p As Double = 0.9 ' for scaling arrows
            Dim diffX As Double = pntB.X - pntA.X
            Dim diffY As Double = pntB.Y - pntA.Y
            Dim L As Double = Math.Sqrt((diffX * diffX) + (diffY * diffY))
            Dim p As Double
            Dim arrowLength As Double = GetArrowLength()
            If L < arrowLength Then
                p = 1
            Else
                p = 1 - (arrowLength / L)
            End If
            Dim pntC As Point = New Point((pntA.X * p) + (pntB.X * (1 - p)), (pntA.Y * p) + (pntB.Y * (1 - p)))

            Dim fBaseLength As Double = GetArrowBaseLength()

            DrawGeneralizationArrowHead(g, pen, brush, pntA, pntC, fBaseLength)

        End Sub

        Private Sub RenderEndGeneralizationArrow(ByVal g As Graphics)

            Dim startClassMap As IClassMap = GetStartClassMap()
            Dim endClassMap As IClassMap = GetEndClassMap()

            If startClassMap Is Nothing Then Exit Sub
            If endClassMap Is Nothing Then Exit Sub

            If Len(endClassMap.InheritsClass) > 0 Then

                If startClassMap Is endClassMap.GetInheritedClassMap Then

                    Exit Sub

                End If

            End If

            Dim pen As pen = GetPen()
            Dim brush As brush = GetBgBrush()

            Dim pntA As Point = GetEndPoint()
            Dim pntB As Point = GetPoint(GetNrOfPoints() - 2)
            'Dim p As Double = 0.9 ' for scaling arrows
            Dim diffX As Double = pntB.X - pntA.X
            Dim diffY As Double = pntB.Y - pntA.Y
            Dim L As Double = Math.Sqrt((diffX * diffX) + (diffY * diffY))
            Dim p As Double
            Dim arrowLength As Double = GetArrowLength()
            If L < arrowLength Then
                p = 1
            Else
                p = 1 - (arrowLength / L)
            End If
            Dim pntC As Point = New Point((pntA.X * p) + (pntB.X * (1 - p)), (pntA.Y * p) + (pntB.Y * (1 - p)))

            Dim fBaseLength As Double = GetArrowBaseLength()

            DrawGeneralizationArrowHead(g, pen, brush, pntA, pntC, fBaseLength)

        End Sub


        'Association

        Private Sub RenderAssociationArrows(ByVal g As Graphics)

            RenderStartAssociationArrow(g)

            RenderEndAssociationArrow(g)

        End Sub

        Private Sub RenderStartAssociationArrow(ByVal g As Graphics)

            If Len(m_EndProperty) < 1 Then Exit Sub

            Dim pen As pen = GetPen()

            Dim pntA As Point = GetStartPoint()
            Dim pntB As Point = GetPoint(1)
            'Dim p As Double = 0.9 ' for scaling arrows
            Dim diffX As Double = pntB.X - pntA.X
            Dim diffY As Double = pntB.Y - pntA.Y
            Dim L As Double = Math.Sqrt((diffX * diffX) + (diffY * diffY))
            Dim p As Double
            Dim arrowLength As Double = GetArrowLength()
            If L < arrowLength Then
                p = 1
            Else
                p = 1 - (arrowLength / L)
            End If

            Dim pntC As Point = New Point((pntA.X * p) + (pntB.X * (1 - p)), (pntA.Y * p) + (pntB.Y * (1 - p)))

            Dim fBaseLength As Double = GetArrowBaseLength()

            DrawAssociationArrowHead(g, pen, pntA, pntC, fBaseLength, m_StartAggregation, HasStartArrow)


        End Sub

        Private Sub RenderEndAssociationArrow(ByVal g As Graphics)

            If Len(m_StartProperty) < 1 Then Exit Sub

            Dim pen As pen = GetPen()

            Dim pntA As Point = GetEndPoint()
            Dim pntB As Point = GetPoint(GetNrOfPoints() - 2)
            'Dim p As Double = 0.9 ' for scaling arrows
            Dim diffX As Double = pntB.X - pntA.X
            Dim diffY As Double = pntB.Y - pntA.Y
            Dim L As Double = Math.Sqrt((diffX * diffX) + (diffY * diffY))
            Dim p As Double
            Dim arrowLength As Double = GetArrowLength()
            If L < arrowLength Then
                p = 1
            Else
                p = 1 - (arrowLength / L)
            End If
            Dim pntC As Point = New Point((pntA.X * p) + (pntB.X * (1 - p)), (pntA.Y * p) + (pntB.Y * (1 - p)))

            Dim fBaseLength As Double = GetArrowBaseLength()

            DrawAssociationArrowHead(g, pen, pntA, pntC, fBaseLength, m_EndAggregation, HasStartArrow)

        End Sub



        Private Sub RenderAssociationLabels(ByVal g As Graphics)

            RenderStartAssociationLabels(g)

            RenderEndAssociationLabels(g)

        End Sub

        Private Sub RenderStartAssociationLabels(ByVal g As Graphics)

            If Len(m_EndProperty) < 1 Then Exit Sub

            Dim pntA As Point = GetStartPoint()
            Dim pntB As Point = GetPoint(1)

            Dim text1 As String = GetStartMultiplicity()
            Dim text2 As String = m_EndProperty

            Dim propertyMap As IPropertyMap = GetEndPropertyMap()

            If Not propertyMap Is Nothing Then

                Select Case propertyMap.Accessibility

                    Case AccessibilityType.PublicAccess

                        text2 = "+" & text2

                    Case AccessibilityType.PrivateAccess

                        text2 = "-" & text2

                    Case Else

                        text2 = "#" & text2

                End Select

            End If

            RenderLineLabels(g, pntA, pntB, text1, text2, HasStartDiamond, HasStartArrow)

        End Sub

        Private Sub RenderEndAssociationLabels(ByVal g As Graphics)

            If Len(m_StartProperty) < 1 Then Exit Sub

            Dim pntA As Point = GetEndPoint()
            Dim pntB As Point = GetPoint(GetNrOfPoints() - 2)

            Dim text1 As String = m_StartProperty
            Dim text2 As String = GetEndMultiplicity()

            Dim propertyMap As IPropertyMap = GetStartPropertyMap()

            If Not propertyMap Is Nothing Then

                Select Case propertyMap.Accessibility

                    Case AccessibilityType.PublicAccess

                        text1 = "+" & text1

                    Case AccessibilityType.PrivateAccess

                        text1 = "-" & text1

                    Case Else

                        text1 = "#" & text1

                End Select

            End If

            RenderLineLabels(g, pntA, pntB, text1, text2, HasEndDiamond, HasStartArrow)

        End Sub


        Private Sub RenderLineLabels(ByVal g As Graphics, ByVal pntA As Point, ByVal pntB As Point, ByVal text1 As String, ByVal text2 As String, ByVal hasDiamond As Boolean, ByVal hasArrow As Boolean)

            Dim pen As pen = GetPen()

            'Dim p As Double = 0.9 ' for scaling Labels
            Dim diffX As Double = pntB.X - pntA.X
            Dim diffY As Double = pntB.Y - pntA.Y
            Dim L As Double = Math.Sqrt((diffX * diffX) + (diffY * diffY))
            Dim p1 As Double
            Dim p2 As Double
            Dim v As Double
            Dim font As font = GetFont()
            Dim arrowLength As Double = GetArrowLength()
            Dim offset1 As Integer = 0
            Dim offset2 As Integer = 0

            If hasArrow Then

                offset1 += (arrowLength * 1.5)
                offset2 += (arrowLength * 1.5)

            Else

                offset1 += (arrowLength * 0.5)
                offset2 += (arrowLength * 0.5)

            End If

            If hasDiamond Then

                offset1 += (arrowLength * 1.5)
                offset2 += (arrowLength * 1.5)

            End If

            Dim textSize As SizeF

            textSize = g.MeasureString(text1, font)
            offset1 += (textSize.Width / 2)

            textSize = g.MeasureString(text2, font)
            offset2 += (textSize.Width / 2)

            If L < offset1 Then
                p1 = 1
            Else
                p1 = 1 - (offset1 / L)
            End If

            If L < offset2 Then
                p2 = 1
            Else
                p2 = 1 - (offset2 / L)
            End If

            Dim pntC1 As Point = New Point((pntA.X * p1) + (pntB.X * (1 - p1)), (pntA.Y * p1) + (pntB.Y * (1 - p1)))
            Dim pntC2 As Point = New Point((pntA.X * p2) + (pntB.X * (1 - p2)), (pntA.Y * p2) + (pntB.Y * (1 - p2)))

            Dim fBaseLength As Double = GetArrowBaseLength()

            v = Math.Atan2(diffY, diffX)

            v = v * (180 / Math.PI)

            v = CLng(v)

            If v < 120 Then v = v + 180

            If v > 120 Then v = v - 180

            Dim fXDiff As Double = pntA.X - pntC1.X
            Dim fYDiff As Double = pntA.Y - pntC1.Y
            Dim fDistAB As Double = Math.Sqrt(fXDiff * fXDiff + fYDiff * fYDiff)

            Dim pts As PointF() = New PointF(2) {New PointF(0, 0), New PointF(-fBaseLength / 2, fDistAB), New PointF(fBaseLength / 2, fDistAB)}

            Dim m As Matrix = New Matrix
            m.Translate(pntA.X, pntA.Y)
            m.Rotate((Math.Atan2(fXDiff, -fYDiff) * 180 / Math.PI))
            m.TransformPoints(pts)

            DrawText(g, text1, pts(1), v)


            Dim fXDiff2 As Double = pntA.X - pntC2.X
            Dim fYDiff2 As Double = pntA.Y - pntC2.Y
            Dim fDistAB2 As Double = Math.Sqrt(fXDiff2 * fXDiff2 + fYDiff2 * fYDiff2)

            Dim pts2 As PointF() = New PointF(2) {New PointF(0, 0), New PointF(-fBaseLength / 2, fDistAB2), New PointF(fBaseLength / 2, fDistAB2)}

            Dim m2 As Matrix = New Matrix
            m2.Translate(pntA.X, pntA.Y)
            m2.Rotate((Math.Atan2(fXDiff2, -fYDiff2) * 180 / Math.PI))
            m2.TransformPoints(pts2)

            DrawText(g, text2, pts2(2), v)

        End Sub


        Private Sub DrawText(ByVal g As Graphics, ByVal str As String, ByVal location As PointF, ByVal angle As Double)

            Dim font As font = GetFont()

            g.TranslateTransform(location.X, location.Y)

            g.RotateTransform(angle)

            Dim format As StringFormat = New StringFormat(StringFormatFlags.NoClip)
            format.Alignment = StringAlignment.Center
            format.LineAlignment = StringAlignment.Center
            g.DrawString(str, font, GetBrush, 0, 0, format)

            g.ResetTransform()

        End Sub



        Public Function GetPen() As Pen

            If m_HasSynchError Then

                Return New Pen(GetColorSynchError)

            End If

            If m_HasError Then

                Return New Pen(GetColorError)

            End If

            Return New Pen(GetForeColor)

        End Function


        Public Function GetSelectedPen() As Pen

            Dim pen As pen = GetPen()

            pen.DashStyle = DashStyle.Dash

            Return pen

        End Function


        Public Function GetBrush() As Brush

            Dim brush As brush

            If m_HasSynchError Then

                brush = New SolidBrush(GetColorSynchError)

            ElseIf m_HasError Then

                brush = New SolidBrush(GetColorError)

            Else

                brush = New SolidBrush(GetFontColor)

            End If

            Return brush

        End Function

        Public Function GetBgBrush() As Brush

            Dim brush As brush

            brush = New SolidBrush(GetBackColor1)

            Return brush

        End Function

        Public Function GetCompositionBrush() As Brush

            Return GetBrush()

        End Function

        Public Function GetAggregationBrush() As Brush

            Return GetBgBrush()

        End Function

        Public Function GetZoom() As Decimal

            Return m_UmlDiagram.Zoom

        End Function


        Public Function GetFont() As Font

            Dim zoom As Decimal = GetZoom()

            Dim emSize As Single = GetFontSize() * zoom

            If emSize < 1 Then emSize = 1

            Dim font As font


            Try

                font = New font(New FontFamily(GetFontFamily), emSize, GetFontStyle)

            Catch ex As Exception

                font = New font(New FontFamily(Text.GenericFontFamilies.SansSerif), emSize, FontStyle.Regular)

            End Try

            Return font

        End Function

        Public Function GetStartMultiplicity() As String

            Dim str As String = "*"

            Dim propertyMap As IPropertyMap = GetEndPropertyMap()

            If Not propertyMap Is Nothing Then

                str = GetMultiplicity(propertyMap)

            End If

            Return str

        End Function

        Public Function GetEndMultiplicity() As String

            Dim str As String = "*"

            Dim propertyMap As IPropertyMap = GetStartPropertyMap()

            If Not propertyMap Is Nothing Then

                str = GetMultiplicity(propertyMap)

            End If

            Return str

        End Function


        Private Function GetMultiplicity(ByVal propertyMap As IPropertyMap) As String

            Dim columnMap As IColumnMap

            If propertyMap.IsCollection Then

                Dim min As String = "0"
                Dim max As String = "*"

                If propertyMap.CascadingCreate Then

                    min = "1"

                End If

                If propertyMap.MinLength > -1 Then

                    min = propertyMap.MinLength

                End If

                If propertyMap.MaxLength > -1 Then

                    max = propertyMap.MaxLength

                End If

                Return min & ".." & max

            Else


                If Not propertyMap.GetIsNullable Then

                    Return "1"

                End If

                Return "0..1"

            End If

        End Function


        'Just so I never think I was able to write this stuff myself:
        'Dan Byström wrote this!
        Private Sub FillTriangle(ByVal g As Graphics, _
            ByVal brush As Brush, _
            ByVal pntA As PointF, _
            ByVal pntB As PointF, _
            ByVal fBaseLength As Double)

            Dim fXDiff As Double = pntA.X - pntB.X
            Dim fYDiff As Double = pntA.Y - pntB.Y
            Dim fDistAB As Double = Math.Sqrt(fXDiff * fXDiff + fYDiff * fYDiff)

            Dim pts As PointF() = New PointF(2) {New PointF(0, 0), New PointF(-fBaseLength / 2, fDistAB), New PointF(fBaseLength / 2, fDistAB)}

            Dim m As Matrix = New Matrix
            m.Translate(pntA.X, pntA.Y)
            m.Rotate((Math.Atan2(fXDiff, -fYDiff) * 180 / Math.PI))
            m.TransformPoints(pts)

            g.FillPolygon(brush, pts)
        End Sub

        Private Sub DrawGeneralizationArrowHead(ByVal g As Graphics, _
            ByVal pen As Pen, _
            ByVal brush As Brush, _
            ByVal pntA As Point, _
            ByVal pntB As Point, _
            ByVal fBaseLength As Double)

            Dim fXDiff As Double = pntA.X - pntB.X
            Dim fYDiff As Double = pntA.Y - pntB.Y
            Dim fDistAB As Double = Math.Sqrt(fXDiff * fXDiff + fYDiff * fYDiff)

            Dim pts As PointF() = New PointF(2) {New PointF(0, 0), New PointF(-fBaseLength / 2, fDistAB), New PointF(fBaseLength / 2, fDistAB)}

            Dim m As Matrix = New Matrix
            m.Translate(pntA.X, pntA.Y)
            m.Rotate((Math.Atan2(fXDiff, -fYDiff) * 180 / Math.PI))
            m.TransformPoints(pts)

            g.FillPolygon(brush, pts)
            g.DrawLine(pen, pts(0), pts(1))
            g.DrawLine(pen, pts(0), pts(2))
            g.DrawLine(pen, pts(1), pts(2))

        End Sub


        Private Sub DrawAssociationArrowHead(ByVal g As Graphics, _
            ByVal pen As Pen, _
            ByVal pntA As Point, _
            ByVal pntB As Point, _
            ByVal fBaseLength As Double, _
            ByVal aggregation As AggregationEnum, ByVal hasArrow As Boolean)

            Dim offset As Integer = DrawAssociationDiamond(g, pen, pntA, pntB, fBaseLength, aggregation)

            If hasArrow Then

                Dim fXDiff As Double = pntA.X - pntB.X
                Dim fYDiff As Double = pntA.Y - pntB.Y
                Dim fDistAB As Double = Math.Sqrt(fXDiff * fXDiff + fYDiff * fYDiff)

                Dim pts As PointF() = New PointF(2) {New PointF(0, offset), New PointF(-fBaseLength / 2, fDistAB + offset), New PointF(fBaseLength / 2, fDistAB + offset)}

                Dim m As Matrix = New Matrix
                m.Translate(pntA.X, pntA.Y)
                m.Rotate((Math.Atan2(fXDiff, -fYDiff) * 180 / Math.PI))
                m.TransformPoints(pts)

                'g.FillPolygon(brush, pts)
                g.DrawLine(pen, pts(0), pts(1))
                g.DrawLine(pen, pts(0), pts(2))

            End If

        End Sub

        Private Function DrawAssociationDiamond(ByVal g As Graphics, _
            ByVal pen As Pen, _
            ByVal pntA As Point, _
            ByVal pntB As Point, _
            ByVal fBaseLength As Double, _
            ByVal aggregation As AggregationEnum) As Integer

            Dim brush As brush

            Select Case aggregation

                Case AggregationEnum.None

                    Return 0

                Case AggregationEnum.Aggregation

                    brush = GetAggregationBrush()

                Case AggregationEnum.Composition

                    brush = GetCompositionBrush()

            End Select

            Dim fXDiff As Double = pntA.X - pntB.X
            Dim fYDiff As Double = pntA.Y - pntB.Y
            Dim fDistAB As Double = Math.Sqrt(fXDiff * fXDiff + fYDiff * fYDiff) * 0.75

            Dim pts As PointF() = New PointF(3) {New PointF(0, 0), New PointF(-fBaseLength / 3, fDistAB), New PointF(0, fDistAB * 2), New PointF(fBaseLength / 3, fDistAB)}

            Dim m As Matrix = New Matrix
            m.Translate(pntA.X, pntA.Y)
            m.Rotate((Math.Atan2(fXDiff, -fYDiff) * 180 / Math.PI))
            m.TransformPoints(pts)

            g.FillPolygon(brush, pts)
            g.DrawLine(pen, pts(0), pts(1))
            g.DrawLine(pen, pts(0), pts(3))
            g.DrawLine(pen, pts(2), pts(1))
            g.DrawLine(pen, pts(2), pts(3))

            Return fDistAB * 2

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

        Public Overrides Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Overridable Property LineType() As LineTypeEnum
            Get
                Return m_LineType
            End Get
            Set(ByVal Value As LineTypeEnum)
                m_LineType = Value
            End Set
        End Property


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

            Dim classMap As IClassMap
            Dim propertyMap As IPropertyMap

            If CType(mapObject, Object).GetType Is GetType(classMap) Then

                classMap = GetStartClassMap()

                If Not classMap Is Nothing Then

                    If classMap Is mapObject Then

                        m_StartClass = newName

                    End If

                End If

                classMap = GetEndClassMap()

                If Not classMap Is Nothing Then

                    If classMap Is mapObject Then

                        m_EndClass = newName

                    End If

                End If

            ElseIf CType(mapObject, Object).GetType Is GetType(propertyMap) Then

                propertyMap = GetStartPropertyMap()

                If Not propertyMap Is Nothing Then

                    If propertyMap Is mapObject Then

                        m_StartProperty = newName

                    End If

                End If

                propertyMap = GetEndPropertyMap()

                If Not propertyMap Is Nothing Then

                    If propertyMap Is mapObject Then

                        m_EndProperty = newName

                    End If

                End If


            End If




        End Sub


        Public Sub MakeSShape()

            'Dim pntA As UmlLinePoint
            'Dim pntB As UmlLinePoint

            'If Not m_UmlLinePoints.Count = 2 Then

            '    m_UmlLinePoints.Clear()

            '    pntA = New UmlLinePoint
            '    pntB = New UmlLinePoint

            'Else

            '    pntA = m_UmlLinePoints(0)
            '    pntB = m_UmlLinePoints(1)

            'End If

            'Dim startPoint As Point = GetStartPoint()
            'Dim endPoint As Point = GetEndPoint()

            'Dim diffX As Double = startPoint.X - endPoint.X
            'Dim diffY As Double = startPoint.Y - endPoint.Y

            'If diffX > diffY Then

            '    If startPoint.X > endPoint.X Then

            '    Else

            '    End If

            'Else

            'End If

            'm_UmlLinePoints.Add(pntA)
            'm_UmlLinePoints.Add(pntB)

        End Sub

        Public Function GetSelectedLinePoints() As ArrayList

            Dim list As New ArrayList

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                If linePoint.Selected Then

                    list.Add(linePoint)

                End If

            Next

            Return list

        End Function

        Public Sub MoveSelected(ByVal X As Integer, ByVal Y As Integer, ByVal lastX As Integer, ByVal lastY As Integer)

            Dim zoom As Double = GetZoom()

            If zoom = 0 Then zoom = 1

            If StartAloneSelected() Then

                m_FixedStart = True

                SetStartSlotAndSide(New Point(X, Y))

                Exit Sub

            End If

            If EndAloneSelected() Then

                m_FixedEnd = True

                SetEndSlotAndSide(New Point(X, Y))

                Exit Sub

            End If

            Dim linePoint As UmlLinePoint

            For Each linePoint In GetSelectedLinePoints()

                linePoint.MoveSelected(X, Y, lastX, lastY)

            Next

        End Sub

        Private Function StartAloneSelected() As Boolean

            If Not m_StartSelected Then Return False

            If m_EndSelected Then Return False

            If Not m_UmlDiagram.GetSelectedObjects.Count = 1 Then Return False

            Return True

        End Function


        Private Function EndAloneSelected() As Boolean

            If Not m_EndSelected Then Return False

            If m_StartSelected Then Return False

            If Not m_UmlDiagram.GetSelectedObjects.Count = 1 Then Return False

            Return True

        End Function

        Public Overloads Sub DeselectAll(ByVal exceptObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                linePoint.DeselectAll(exceptObjects)

            Next

            If Not exceptObjects.Contains(Me) Then

                m_StartSelected = False
                m_EndSelected = False

            Else

                If Not lineStart Then

                    m_StartSelected = False

                End If

                If Not lineEnd Then

                    m_EndSelected = False

                End If

            End If

        End Sub


        Public Overloads Sub SelectAll()

            m_StartSelected = True
            m_EndSelected = True

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                linePoint.SelectAll()

            Next

        End Sub

        Public Overloads Sub SelectAll(ByVal onObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                linePoint.SelectAll(onObjects)

            Next

            If onObjects.Contains(Me) Then

                If lineStart Then

                    m_StartSelected = True

                End If

                If lineEnd Then

                    m_EndSelected = True

                End If

            End If

        End Sub

        Public Overloads Sub FlipSelection(ByVal onObjects As ArrayList, ByVal lineStart As Boolean, ByVal lineEnd As Boolean)

            Dim linePoint As UmlLinePoint

            For Each linePoint In m_UmlLinePoints

                linePoint.FlipSelection(onObjects)

            Next

            If onObjects.Contains(Me) Then

                If lineStart Then

                    FlipStartSelection()

                End If

                If lineEnd Then

                    FlipEndSelection()

                End If

            End If

        End Sub

        Public Overloads Sub FlipStartSelection()

            If m_StartSelected Then

                m_StartSelected = False

            Else

                m_StartSelected = True

            End If

        End Sub

        Public Overloads Sub FlipEndSelection()

            If m_EndSelected Then

                m_EndSelected = False

            Else

                m_EndSelected = True

            End If

        End Sub


        Public Sub HitTest(ByVal x As Integer, ByVal y As Integer, ByRef hitObjects As ArrayList, ByRef lineStart As Boolean, ByRef lineEnd As Boolean)

            Dim linePoint As UmlLinePoint

            Dim i As Integer

            HitTestStart(x, y, hitObjects, lineStart, lineEnd)

            If Not hitObjects.Count > 0 Then

                HitTestEnd(x, y, hitObjects, lineStart, lineEnd)

            End If

            If Not hitObjects.Count > 0 Then

                For i = m_UmlLinePoints.Count - 1 To 0 Step -1

                    linePoint = m_UmlLinePoints(i)

                    If linePoint.HitTest(x, y) Then

                        hitObjects.Add(linePoint)

                        Exit Sub

                    End If

                Next

            End If

        End Sub

        Public Sub HitTestStart(ByVal x As Integer, ByVal y As Integer, ByRef hitObjects As ArrayList, ByRef lineStart As Boolean, ByRef lineEnd As Boolean)

            Dim sqSize As Integer = 16

            Dim startLocation As Point = GetStartPoint()

            Dim location As Point = New Point(startLocation.X - CInt(sqSize / 2), startLocation.Y - CInt(sqSize / 2))

            If x > location.X And x < (location.X + sqSize) Then

                If y > location.Y And y < (location.Y + sqSize) Then

                    If Not hitObjects.Contains(Me) Then

                        hitObjects.Add(Me)

                    End If

                    lineStart = True

                    Exit Sub

                End If

            End If

        End Sub

        Public Sub HitTestEnd(ByVal x As Integer, ByVal y As Integer, ByRef hitObjects As ArrayList, ByRef lineStart As Boolean, ByRef lineEnd As Boolean)

            Dim sqSize As Integer = 16

            Dim startLocation As Point = GetEndPoint()

            Dim location As Point = New Point(startLocation.X - CInt(sqSize / 2), startLocation.Y - CInt(sqSize / 2))

            If x > location.X And x < (location.X + sqSize) Then

                If y > location.Y And y < (location.Y + sqSize) Then

                    If Not hitObjects.Contains(Me) Then

                        hitObjects.Add(Me)

                    End If

                    lineEnd = True

                    Exit Sub

                End If

            End If

        End Sub


        Public Function AddUmlLinePoint(ByVal fromStart As Boolean) As UmlLinePoint

            Dim newUmlLinePoint As UmlLinePoint = New UmlLinePoint

            Dim startPt As Point
            Dim endPt As Point

            If fromStart Then

                startPt = GetPoint(0)
                endPt = GetPoint(1)

            Else

                startPt = GetEndPoint()
                endPt = GetPoint(GetNrOfPoints() - 2)

            End If

            newUmlLinePoint.Selected = True

            Dim zoom As Double = GetZoom()

            newUmlLinePoint.X = ((startPt.X + ((endPt.X - startPt.X) / 2)) / zoom) + m_UmlDiagram.Location.X
            newUmlLinePoint.Y = ((startPt.Y + ((endPt.Y - startPt.Y) / 2)) / zoom) + m_UmlDiagram.Location.Y

            If fromStart Then

                m_UmlLinePoints.Insert(0, newUmlLinePoint)
                newUmlLinePoint.SetUmlLine(Me)

            Else

                newUmlLinePoint.UmlLine = Me

            End If

            Return newUmlLinePoint

        End Function

        Public Function GetPointIndex(ByVal umlLinePoint As UmlLinePoint) As Integer

            Dim index As Integer

            index = m_UmlLinePoints.IndexOf(umlLinePoint) + 1

            Return index

        End Function

    End Class

End Namespace
