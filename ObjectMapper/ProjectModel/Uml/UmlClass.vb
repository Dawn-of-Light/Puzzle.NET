Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Mapping.Visitor
Imports System.Xml.Serialization
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Uml

    Public Enum BrushEnum
        SolidBrush = 0
        GradientBrush = 1
    End Enum

    Public Class UmlClass
        Inherits MapBase


        Private m_VisiblePropertyMaps As ArrayList

        Private m_Name As String

        Private m_UmlDiagram As UmlDiagram

        Private m_Location As Point = New Point(0, 0)
        Private m_Size As Size = New Size(0, 0)

        Private m_Selected As Boolean = False

        Private m_Collapsed As Boolean = False

        Private m_Methods As String()

        Private m_SlotsPerSide As Integer = 4

        'Inheritable
        Private m_InheritSettings As Boolean = True

        Private m_ForeColor As UmlColor = New UmlColor(Color.Black)
        Private m_BackColor1 As UmlColor = New UmlColor(Color.White)
        Private m_BackColor2 As UmlColor = New UmlColor(Color.FromArgb(255, 224, 224, 224))

        Private m_BgBrushStyle As BrushEnum = BrushEnum.GradientBrush
        Private m_BgGradientMode As LinearGradientMode = LinearGradientMode.ForwardDiagonal

        Private m_FontSize As Single = 8.25
        Private m_FontFamily As String = "Microsoft Sans Serif"
        Private m_FontColor As UmlColor = New UmlColor(Color.Black)
        Private m_FontStyle As FontStyle = FontStyle.Regular

        Private m_DisplayInheritedProperties As Boolean = False
        Private m_DisplayLineProperties As Boolean = False

        '/Inheritable

        Private m_TextSizes As ArrayList
        Private m_PropertyOffset As Point

        Private m_HasSynchError As Boolean
        Private m_HasError As Boolean

        Private m_listExceptions As ArrayList
        Private m_propertyExceptions As New Hashtable

        <XmlIgnore()> Public Property UmlDiagram() As UmlDiagram
            Get
                Return m_UmlDiagram
            End Get
            Set(ByVal Value As UmlDiagram)
                If Not m_UmlDiagram Is Nothing Then
                    m_UmlDiagram.UmlClasses.Remove(Me)
                End If
                m_UmlDiagram = Value
                If Not m_UmlDiagram Is Nothing Then
                    m_UmlDiagram.UmlClasses.Add(Me)
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


        <XmlIgnore()> Public Property PropertyExceptions() As Hashtable
            Get
                Return m_propertyExceptions
            End Get
            Set(ByVal Value As Hashtable)
                m_propertyExceptions = Value
            End Set
        End Property



        <XmlArrayItem(GetType(String))> Public Property Methods() As String()
            Get
                Return m_Methods
            End Get
            Set(ByVal Value As String())
                m_Methods = Value
            End Set
        End Property

        <XmlIgnore()> Public Property Moving() As Boolean
            Get
                Return m_UmlDiagram.Moving
            End Get
            Set(ByVal Value As Boolean)
                m_UmlDiagram.Moving = Value
            End Set
        End Property


        <XmlIgnore()> Public Property MoveReduction() As Byte
            Get
                Return m_UmlDiagram.MoveReduction
            End Get
            Set(ByVal Value As Byte)
                m_UmlDiagram.MoveReduction = Value
            End Set
        End Property

        Public Overridable Sub SetUmlDiagram(ByVal Value As UmlDiagram)

            m_UmlDiagram = Value

        End Sub


        Public Function GetDomainMap() As IDomainMap

            Return UmlDiagram.GetDomainMap

        End Function

        Function GetClassMap() As IClassMap

            Dim classMap As IClassMap

            For Each classMap In GetDomainMap.ClassMaps

                If LCase(classMap.Name) = LCase(Me.Name) Then

                    Return classMap

                End If

            Next

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

        Public Property Collapsed() As Boolean
            Get
                Return m_Collapsed
            End Get
            Set(ByVal Value As Boolean)
                m_Collapsed = Value
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

        Public Property SlotsPerSide() As Integer
            Get
                Return m_SlotsPerSide
            End Get
            Set(ByVal Value As Integer)
                m_SlotsPerSide = Value
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



        'Inheritance
        Public Function GetDrawShadows() As Boolean
            Return m_UmlDiagram.DrawShadows
        End Function

        Public Function GetDrawShadowsPass2() As Boolean
            Return m_UmlDiagram.DrawShadowsPass2
        End Function

        Public Function GetShadowColor1() As Color
            Return m_UmlDiagram.ShadowColor1.ToColor
        End Function

        Public Function GetShadowColor2() As Color
            Return m_UmlDiagram.ShadowColor2.ToColor
        End Function

        Public Function GetColorSynchError() As Color
            Return m_UmlDiagram.ColorSynchError.ToColor
        End Function

        Public Function GetColorError() As Color
            Return m_UmlDiagram.ColorError.ToColor
        End Function

        Public Function GetShadowBrushStyle() As BrushEnum
            Return m_UmlDiagram.ShadowBrushStyle
        End Function

        Public Function GetShadowGradientMode() As LinearGradientMode
            Return m_UmlDiagram.ShadowGradientMode
        End Function


        Public Function GetForeColor() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.ForeColor.ToColor
            Else
                Return ForeColor.ToColor
            End If
        End Function

        Public Function GetBackColor1() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.ClassBackColor1.ToColor
            Else
                Return BackColor1.ToColor
            End If
        End Function

        Public Function GetBackColor2() As Color
            If m_InheritSettings Then
                Return m_UmlDiagram.ClassBackColor2.ToColor
            Else
                Return BackColor2.ToColor
            End If
        End Function

        Public Function GetBgBrushStyle() As BrushEnum
            If m_InheritSettings Then
                Return m_UmlDiagram.ClassBgBrushStyle
            Else
                Return m_BgBrushStyle
            End If
        End Function

        Public Function GetBgGradientMode() As LinearGradientMode
            If m_InheritSettings Then
                Return m_UmlDiagram.ClassBgGradientMode
            Else
                Return m_BgGradientMode
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

        Public Function GetDisplayInheritedProperties() As Boolean
            If m_InheritSettings Then
                Return m_UmlDiagram.DisplayInheritedProperties
            Else
                Return m_DisplayInheritedProperties
            End If
        End Function

        Public Function GetDisplayLineProperties() As Boolean
            If m_InheritSettings Then
                Return m_UmlDiagram.DisplayLineProperties
            Else
                Return m_DisplayLineProperties
            End If
        End Function





        Public Function GetLocation(Optional ByVal offsetX As Integer = 0, Optional ByVal offsetY As Integer = 0) As Point

            Dim zoom As Decimal = GetZoom()

            'Dim location As New Point(CInt((m_Location.X + offsetX) * zoom) - m_UmlDiagram.Location.X, CInt((m_Location.Y + offsetY) * zoom) - m_UmlDiagram.Location.Y)

            Dim location As New Point(CInt(m_Location.X * zoom) + offsetX - m_UmlDiagram.Location.X, CInt(m_Location.Y * zoom) + offsetY - m_UmlDiagram.Location.Y)

            Return location

        End Function

        Public Function GetSize() As Size

            Return m_Size

            'Dim zoom As Decimal = GetZoom()

            'Dim size As New size(CInt(m_Size.Width * zoom), CInt(m_Size.Height * zoom))

            'Return size

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

                font = New font(New FontFamily(GetFontFamily), emSize, GetFontStyle())

            Catch ex As Exception

                font = New font(New FontFamily(Text.GenericFontFamilies.SansSerif), emSize, FontStyle.Regular)

            End Try

            Return font

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

        Public Function GetOkBrush() As Brush

            Return New SolidBrush(GetFontColor)

        End Function

        Public Function GetErrorBrush() As Brush

            Return New SolidBrush(GetColorError)

        End Function

        Public Function GetBgBrush(ByVal rect As Rectangle) As Brush

            'Dim brush As New SolidBrush(Color.White)

            Dim brush As brush

            Select Case GetBgBrushStyle()

                Case BrushEnum.GradientBrush

                    brush = New LinearGradientBrush(rect, GetBackColor1, GetBackColor2, GetBgGradientMode)

                Case BrushEnum.SolidBrush

                    brush = New SolidBrush(GetBackColor1)

            End Select

            Return brush

        End Function


        Public Function GetShadowBrush(ByVal rect As Rectangle) As Brush

            Dim brush As brush

            Select Case GetBgBrushStyle()

                Case BrushEnum.GradientBrush

                    brush = New LinearGradientBrush(rect, GetShadowColor1, GetShadowColor2, LinearGradientMode.ForwardDiagonal)

                Case BrushEnum.SolidBrush

                    brush = New SolidBrush(GetShadowColor1)

            End Select

            Return brush

        End Function


        Public Function GetMargin(ByVal g As Graphics) As Decimal

            Dim font As font = GetFont()

            Dim margin As Decimal = g.MeasureString("X", font).Height / 4

            If margin < 1 Then margin = 1

            Return margin

        End Function

        Public Function GetVisiblePropertyMaps() As ArrayList

            If Me.Moving Then

                If Not m_VisiblePropertyMaps Is Nothing Then

                    Return m_VisiblePropertyMaps

                End If

            End If

            Dim propertyMaps As New ArrayList
            Dim lines As ArrayList = GetConnectedUmlLines()
            Dim propertyMap As IPropertyMap
            Dim classMap As IClassMap = GetClassMap()

            Dim sorted As ArrayList
            Dim added As New ArrayList

            Dim dispInheritedProps As Boolean = GetDisplayInheritedProperties()
            Dim dispLineProps As Boolean = GetDisplayLineProperties()

            If Not classMap Is Nothing Then

                sorted = classMap.GetNonInheritedIdentityPropertyMaps

                sorted.Sort()

                For Each propertyMap In sorted

                    If dispLineProps OrElse Not PropertyHasLine(propertyMap, lines) Then

                        propertyMaps.Add(propertyMap)

                        added.Add(LCase(propertyMap.Name))

                    End If

                Next

                If dispInheritedProps Then

                    sorted = classMap.GetInheritedIdentityPropertyMaps

                    sorted.Sort()

                    For Each propertyMap In sorted

                        If Not added.Contains(LCase(propertyMap.Name)) Then

                            If dispLineProps OrElse Not PropertyHasLine(propertyMap, lines) Then

                                propertyMaps.Add(propertyMap)

                                added.Add(LCase(propertyMap.Name))

                            End If

                        End If

                    Next


                End If

                sorted = classMap.GetNonInheritedPropertyMaps

                sorted.Sort()

                For Each propertyMap In sorted

                    If Not added.Contains(LCase(propertyMap.Name)) Then

                        If dispLineProps OrElse Not PropertyHasLine(propertyMap, lines) Then

                            propertyMaps.Add(propertyMap)

                            added.Add(LCase(propertyMap.Name))

                        End If

                    End If

                Next

                If dispInheritedProps Then

                    sorted = classMap.GetInheritedPropertyMaps

                    sorted.Sort()

                    For Each propertyMap In sorted

                        If Not added.Contains(LCase(propertyMap.Name)) Then

                            If dispLineProps OrElse Not PropertyHasLine(propertyMap, lines) Then

                                propertyMaps.Add(propertyMap)

                                added.Add(LCase(propertyMap.Name))

                            End If

                        End If

                    Next


                End If


            End If

            m_VisiblePropertyMaps = propertyMaps

            Return propertyMaps

        End Function


        Public Function GetVisibleMethods() As ArrayList

            Dim methods As New ArrayList
            Dim method As String

            If Not Me.Methods Is Nothing Then

                For Each method In Me.Methods

                    methods.Add(method)

                Next

            End If

            Return methods

        End Function

        Public Overloads Function PropertyHasLine(ByVal propertyMap As IPropertyMap) As Boolean

            Return PropertyHasLine(propertyMap, GetConnectedUmlLines)

        End Function

        Private Overloads Function PropertyHasLine(ByVal propertyMap As IPropertyMap, ByVal lines As ArrayList) As Boolean

            Dim umlLine As umlLine
            Dim testPropertyMap As IPropertyMap

            For Each umlLine In lines

                testPropertyMap = umlLine.GetStartPropertyMap

                If Not testPropertyMap Is Nothing Then

                    If testPropertyMap Is propertyMap Then

                        Return True

                    End If

                End If

                testPropertyMap = umlLine.GetEndPropertyMap

                If Not testPropertyMap Is Nothing Then

                    If testPropertyMap Is propertyMap Then

                        Return True

                    End If

                End If

            Next

            Return False

        End Function

        Public Sub Verify(ByVal checkMappings As Boolean)

            'Dim visitor As MapVerificationVisitor = New MapVerificationVisitor(False, checkMappings)
            Dim visitor As MapVerificationVisitor = New MapVerificationVisitor(True, checkMappings)

            Accept(visitor)

        End Sub

        Public Overloads Sub Accept(ByVal visitor As MapVerificationVisitor)

            m_propertyExceptions.Clear()
            m_HasSynchError = False
            m_HasError = False

            Dim domainMap As IDomainMap = GetDomainMap()
            Dim classMap As IClassMap = GetClassMap()

            If domainMap Is Nothing Then

                m_HasSynchError = True
                visitor.Exceptions.Add(New MappingException("The diagram that this class belongs to is not synched with a domain map!", Me, "Domain Map"))

            Else

                If classMap Is Nothing Then

                    m_HasSynchError = True
                    visitor.Exceptions.Add(New MappingException("A class with the name '" & m_Name & "' does not exist in the domain '" & domainMap.Name & "'", Me, "Name"))

                Else

                    classMap.Accept(visitor)

                    If visitor.Exceptions.Count > 0 Then

                        m_HasError = True

                    End If

                End If

            End If

            m_listExceptions = visitor.Exceptions

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

        Public Sub CalcSize(ByVal g As Graphics)

            If Me.Moving Then

                Return

            End If

            Dim nameOffsetY As Integer
            Dim membersOffsetY As Integer
            Dim methodsOffsetY As Integer
            Dim textSizes As New ArrayList
            Dim margin As Integer = GetMargin(g)

            nameOffsetY = CalcNameSize(g, textSizes)

            If Not m_Collapsed Then

                membersOffsetY = CalcMemberSizes(g, textSizes, nameOffsetY, methodsOffsetY)

            End If

            m_PropertyOffset = GetLocation(margin * 2, (margin * 3) + nameOffsetY)

            m_TextSizes = textSizes

        End Sub

        Public Sub Render(ByVal g As Graphics, ByVal pass As Byte, Optional ByVal checkMappings As Boolean = True)

            If pass = 0 Then

                Verify(checkMappings)

            End If

            Dim pen As pen
            Dim location As Point = GetLocation()
            Dim size As size = GetSize()
            Dim textSizes As New ArrayList
            Dim nameOffsetY As Integer
            Dim membersOffsetY As Integer
            Dim methodsOffsetY As Integer
            Dim margin As Integer = GetMargin(g)
            Dim sizeX As Integer
            Dim sizeY As Integer
            Dim nameLineY As Integer
            Dim rect As Rectangle
            Dim shadowRect As Rectangle
            Dim bgBrush As Brush
            Dim shadowBrush As Brush

            pen = GetPen()

            nameOffsetY = CalcNameSize(g, textSizes)

            If Not m_Collapsed Then

                membersOffsetY = CalcMemberSizes(g, textSizes, nameOffsetY, methodsOffsetY)

            End If

            m_PropertyOffset = GetLocation(margin * 2, (margin * 3) + nameOffsetY)

            m_TextSizes = textSizes

            nameLineY = location.Y + nameOffsetY + (margin * 2)

            If pass = 0 Then

                sizeX = GetSizeX(textSizes) + (margin * 4)

                sizeY = nameOffsetY + membersOffsetY + (margin * 6)

                m_Size = New size(sizeX, sizeY)

                If Not Me.Moving Then

                    If GetDrawShadows() Then

                        If Not GetDrawShadowsPass2() Then

                            shadowRect = New Rectangle(location.X + (margin * 2), location.Y + (margin * 2), sizeX, sizeY)

                            shadowBrush = GetShadowBrush(shadowRect)

                            g.FillRectangle(shadowBrush, shadowRect)

                        End If

                    End If

                End If

            ElseIf pass = 1 Then

                sizeX = m_Size.Width

                sizeY = m_Size.Height

                If Not (Me.Moving And Me.MoveReduction > 3) Then

                    If Not (Me.Moving And Me.MoveReduction > 2) Then

                        If GetDrawShadows() Then

                            If GetDrawShadowsPass2() Then

                                shadowRect = New Rectangle(location.X + (margin * 2), location.Y + (margin * 2), sizeX, sizeY)

                                shadowBrush = GetShadowBrush(shadowRect)

                                g.FillRectangle(shadowBrush, shadowRect)

                            End If

                        End If

                    End If

                    rect = New Rectangle(location.X, location.Y, sizeX, sizeY)

                    bgBrush = GetBgBrush(rect)

                    g.FillRectangle(bgBrush, rect)

                    g.DrawRectangle(pen, rect)

                    rect = New Rectangle(location.X, location.Y, sizeX + 1, sizeY + 1)

                    DrawName(g, textSizes)

                    If Not m_Collapsed Then

                        If Not (Me.Moving And Me.MoveReduction > 1) Then

                            DrawMembers(g, nameOffsetY, checkMappings)

                        End If

                    End If

                    g.DrawLine(pen, location.X, nameLineY, location.X + sizeX, nameLineY)

                    g.DrawLine(pen, location.X, location.Y + sizeY - (margin * 2) - methodsOffsetY, location.X + sizeX, location.Y + sizeY - (margin * 2) - methodsOffsetY)

                Else

                    rect = New Rectangle(location.X, location.Y, sizeX, sizeY)

                    g.DrawRectangle(pen, rect)

                End If

            End If

        End Sub



        Public Sub RenderSelectionFrame(ByVal g As Graphics, ByVal pass As Byte)

            Dim location As Point = GetLocation()
            Dim size As size = GetSize()
            Dim sqSize As Integer = 6
            Dim sizeX As Integer
            Dim sizeY As Integer
            Dim shadowBrush As Brush
            Dim selRectOuter As Rectangle
            Dim selRectInner As Rectangle

            sizeX = m_Size.Width

            sizeY = m_Size.Height

            selRectInner = New Rectangle(location.X, location.Y, sizeX + 1, sizeY + 1)
            selRectOuter = New Rectangle(location.X - sqSize, location.Y - sqSize, sizeX + (2 * sqSize) + 1, sizeY + (2 * sqSize) + 1)

            ControlPaint.DrawSelectionFrame(g, True, selRectOuter, selRectInner, Color.Black)

            'draw little squares...
            Dim sqBrush As Brush = GetSelectionSquareBrush()
            Dim sqPen As Pen = New Pen(Color.Black)

            'first filled...
            g.FillRectangle(sqBrush, location.X - sqSize, location.Y - sqSize, sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X + sizeX + 1, location.Y - sqSize, sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X - sqSize, location.Y + sizeY + 1, sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X + sizeX + 1, location.Y + sizeY + 1, sqSize, sqSize)

            g.FillRectangle(sqBrush, location.X + CInt(sizeX / 2) - CInt(sqSize / 2), location.Y - sqSize, sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X + sizeX + 1, location.Y + CInt(sizeY / 2) - CInt(sqSize / 2), sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X - sqSize, location.Y + CInt(sizeY / 2) - CInt(sqSize / 2), sqSize, sqSize)
            g.FillRectangle(sqBrush, location.X + CInt(sizeX / 2) - CInt(sqSize / 2), location.Y + sizeY + 1, sqSize, sqSize)

            'then lines...
            g.DrawRectangle(sqPen, location.X - sqSize, location.Y - sqSize, sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X + sizeX + 1, location.Y - sqSize, sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X - sqSize, location.Y + sizeY + 1, sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X + sizeX + 1, location.Y + sizeY + 1, sqSize, sqSize)

            g.DrawRectangle(sqPen, location.X + CInt(sizeX / 2) - CInt(sqSize / 2), location.Y - sqSize, sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X + sizeX + 1, location.Y + CInt(sizeY / 2) - CInt(sqSize / 2), sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X - sqSize, location.Y + CInt(sizeY / 2) - CInt(sqSize / 2), sqSize, sqSize)
            g.DrawRectangle(sqPen, location.X + CInt(sizeX / 2) - CInt(sqSize / 2), location.Y + sizeY + 1, sqSize, sqSize)

        End Sub


        Private Function GetSelectionSquareBrush() As Brush

            Dim brush As brush = New SolidBrush(Color.LightGray)

            Return brush

        End Function


        Private Function GetSizeX(ByVal textSizes As ArrayList) As Decimal

            Dim textSize As SizeF

            Dim maxSizeX As Decimal = 0

            For Each textSize In textSizes

                If textSize.Width > maxSizeX Then

                    maxSizeX = textSize.Width

                End If

            Next

            Return maxSizeX

        End Function

        Private Sub DrawName(ByVal g As Graphics, ByVal textSizes As ArrayList)

            Dim font As font = GetFont()

            Dim brush As brush = GetBrush()

            Dim margin As Integer = GetMargin(g)

            Dim textSize As SizeF = textSizes(0)

            Dim location As Point = GetLocation((m_Size.Width / 2) - (textSize.Width / 2), margin)

            'Dim location As Point = GetLocation((margin * 2), margin)

            g.DrawString(Me.Name, font, brush, location.X, location.Y)

        End Sub



        Private Sub DrawMembers(ByVal g As Graphics, ByVal nameOffsetY As Integer, ByVal checkMappings As Boolean)

            Dim font As font = GetFont()

            Dim brush As brush
            Dim okBrush As brush = GetOkBrush()
            Dim errorBrush As brush = GetErrorBrush()

            Dim margin As Integer = GetMargin(g)

            Dim location As Point = GetLocation((margin * 2), margin * 3)

            Dim propertyMap As IPropertyMap

            Dim offsetY As Integer = nameOffsetY

            Dim textSize As SizeF

            Dim str As String

            Dim dataType As String

            Dim prefix As String

            Dim defaultValue As String

            Dim propertyMaps As ArrayList = GetVisiblePropertyMaps()

            Dim methods As ArrayList = GetVisibleMethods()

            Dim method As String

            If propertyMaps.Count < 1 Then

                str = "X"

                textSize = g.MeasureString(str, font)

                offsetY += CInt(textSize.Height + 0.5)

            Else

                For Each propertyMap In propertyMaps

                    prefix = GetPrefix(propertyMap)

                    dataType = FixDataType(propertyMap.DataType)

                    defaultValue = FixDefaultValue(propertyMap.DefaultValue)

                    str = prefix & propertyMap.Name & " : " & dataType & defaultValue

                    Dim visitor As New MapVerificationVisitor(False, checkMappings)

                    propertyMap.Accept(visitor)

                    m_propertyExceptions(propertyMap) = visitor.Exceptions

                    If visitor.Exceptions.Count < 1 Then

                        brush = okBrush

                    Else

                        brush = errorBrush

                    End If

                    g.DrawString(str, font, brush, location.X, location.Y + offsetY)

                    textSize = g.MeasureString(str, font)

                    offsetY += CInt(textSize.Height + 0.5)

                Next

            End If

            offsetY += margin * 2

            If methods.Count < 1 Then

                str = "X"

                textSize = g.MeasureString(str, font)

                offsetY += CInt(textSize.Height + 0.5)

            Else

                For Each method In methods

                    brush = okBrush

                    g.DrawString(method, font, brush, location.X, location.Y + offsetY)

                    textSize = g.MeasureString(method, font)

                    offsetY += CInt(textSize.Height + 0.5)

                Next

            End If

        End Sub

        Private Function GetPrefix(ByVal propertyMap As IPropertyMap) As String

            Dim prefix As String

            Select Case propertyMap.Accessibility

                Case AccessibilityType.PublicAccess

                    prefix = "+"

                Case AccessibilityType.PrivateAccess

                    prefix = "-"

                Case AccessibilityType.ProtectedAccess

                    prefix = "#"

                Case AccessibilityType.ProtectedInternalAccess

                    prefix = "#"

                Case AccessibilityType.InternalAccess

                    prefix = "#"

            End Select

            Return prefix

        End Function

        Private Function FixDataType(ByVal dataType As String) As String

            Dim lDataType As String = LCase(dataType)

            Select Case lDataType

                Case "system.collections.arraylist"

                    Return "ArrayList"

                Case "matssoft.npersist.framework.managedlist"

                    Return "ManagedList"

                Case Else

                    If Len(dataType) > 6 Then

                        If Left(lDataType, 7) = "system." Then

                            Return Right(dataType, Len(dataType) - 7)

                        End If

                    End If

                    Return dataType

            End Select

        End Function

        Private Function FixDefaultValue(ByVal defaultValue As String) As String

            Dim lDefaultValue As String = LCase(defaultValue)

            Select Case lDefaultValue

                Case "new system.collections.arraylist()"

                    Return ""

                Case "new matssoft.npersist.framework.managedlist()"

                    Return ""

                Case "new matssoft.npersist.framework.managedlist(this, """")"

                    Return ""

                Case "new matssoft.npersist.framework.managedlist(me, """")"

                    Return ""

                Case Else

                    If InStr(lDefaultValue, "managedlist") Then

                        Return ""

                    End If

                    If InStr(lDefaultValue, "arraylist") Then

                        Return ""

                    End If

                    If Len(defaultValue) > 6 Then

                        If Left(lDefaultValue, 7) = "system." Then

                            defaultValue = Right(defaultValue, Len(defaultValue) - 7)

                        End If

                    End If

                    If Len(defaultValue) > 0 Then

                        defaultValue = " = " & defaultValue

                    End If

                    Return defaultValue

            End Select

        End Function


        Private Function CalcNameSize(ByVal g As Graphics, ByRef textSizes As ArrayList) As Integer

            Dim font As font = GetFont()

            Dim textSize As SizeF = g.MeasureString(Me.Name, font)

            textSizes.Add(textSize)

            Return CInt(textSize.Height + 0.5)

        End Function

        Private m_CalcMemberSizes As Integer
        Private m_CalcMemberSizes_textSizes As ArrayList
        Private m_CalcMemberSizes_methodsOffsetY As Integer

        Private Function CalcMemberSizes(ByVal g As Graphics, ByRef textSizes As ArrayList, ByVal nameOffsetY As Integer, ByRef methodsOffsetY As Integer) As Integer

            If Me.Moving Then

                textSizes = m_CalcMemberSizes_textSizes
                methodsOffsetY = m_CalcMemberSizes_methodsOffsetY
                Return m_CalcMemberSizes

            End If

            Dim font As font = GetFont()

            Dim membersOffsetY As Long

            Dim propertyMap As IPropertyMap

            Dim textSize As SizeF

            Dim str As String

            Dim dataType As String

            Dim prefix As String

            Dim defaultValue As String

            Dim propertyMaps As ArrayList = GetVisiblePropertyMaps()

            Dim methods As ArrayList = GetVisibleMethods()

            Dim method As String

            If propertyMaps.Count < 1 Then

                str = "X"

                textSize = g.MeasureString(str, font)

                'textSizes.Add(textSize)

                membersOffsetY += CInt(textSize.Height + 0.5)


            Else

                For Each propertyMap In propertyMaps

                    prefix = GetPrefix(propertyMap)

                    dataType = FixDataType(propertyMap.DataType)

                    defaultValue = FixDefaultValue(propertyMap.DefaultValue)

                    str = prefix & propertyMap.Name & " : " & dataType & defaultValue

                    textSize = g.MeasureString(str, font)

                    textSizes.Add(textSize)

                    membersOffsetY += CInt(textSize.Height + 0.5)

                Next

            End If


            If methods.Count < 1 Then

                str = "X"

                textSize = g.MeasureString(str, font)

                'textSizes.Add(textSize)

                membersOffsetY += CInt(textSize.Height + 0.5)

                methodsOffsetY += CInt(textSize.Height + 0.5)

            Else

                For Each method In methods

                    textSize = g.MeasureString(method, font)

                    textSizes.Add(textSize)

                    membersOffsetY += CInt(textSize.Height + 0.5)

                    methodsOffsetY += CInt(textSize.Height + 0.5)

                Next

            End If

            m_CalcMemberSizes_textSizes = textSizes
            m_CalcMemberSizes_methodsOffsetY = methodsOffsetY
            m_CalcMemberSizes = membersOffsetY

            Return membersOffsetY

        End Function


        Public Function HitTest(ByVal x As Integer, ByVal y As Integer, ByRef hitPropertyMap As IPropertyMap) As Boolean

            Dim location As Point = GetLocation()
            Dim propertyMap As IPropertyMap
            Dim size As size = GetSize()
            Dim i As Integer = 0

            If x > location.X And x < (location.X + size.Width) Then

                If y > location.Y And y < (location.Y + size.Height) Then

                    If Not m_Collapsed Then

                        'check if a property has been hit
                        For Each propertyMap In GetVisiblePropertyMaps()

                            If HitTestProperty(x, y, i, propertyMap, hitPropertyMap) Then

                                Return True

                            End If

                            i += 1

                        Next

                    End If

                    Return True

                End If

            End If

            Return False

        End Function

        Public Function HitTestProperty(ByVal x As Integer, ByVal y As Integer, ByVal propertyIndex As Integer, ByVal propertyMap As PropertyMap, ByRef hitPropertyMap As IPropertyMap) As Boolean

            If m_Collapsed Then Return False
            If m_TextSizes Is Nothing Then Return False

            If propertyIndex < 0 Then Return False
            If propertyIndex + 1 >= m_TextSizes.Count Then Return False

            Dim textSize As SizeF

            Dim i As Long

            Dim offsetY As Integer

            For i = 0 To propertyIndex - 1

                textSize = m_TextSizes(i + 1)

                'offsetY += textSize.Height
                offsetY += CInt(textSize.Height + 0.5)

            Next

            textSize = m_TextSizes(propertyIndex + 1)

            Dim location As Point = New Point(m_PropertyOffset.X, m_PropertyOffset.Y + offsetY)

            If x > location.X And x < (location.X + textSize.Width) Then

                If y > location.Y And y < (location.Y + textSize.Height) Then

                    hitPropertyMap = propertyMap

                    Return True

                End If

            End If

            Return False

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


        Public Sub UpdateMapObjectName(ByVal mapObject As IMap, ByVal newName As String)

            Dim classMap As IClassMap

            If CType(mapObject, Object).GetType Is GetType(classMap) Then

                classMap = GetClassMap()

                If Not classMap Is Nothing Then

                    If classMap Is mapObject Then

                        m_Name = newName

                    End If

                End If

            End If

        End Sub


        Public Function GetConnectedUmlLines() As ArrayList

            Dim lines As New ArrayList
            Dim umlLine As umlLine
            Dim umlClass As umlClass

            For Each umlLine In m_UmlDiagram.UmlLines

                umlClass = umlLine.GetStartUmlClass

                If Not umlClass Is Nothing Then

                    If umlClass Is Me Then

                        If Not lines.Contains(umlLine) Then

                            lines.Add(umlLine)

                        End If

                    End If

                End If

                umlClass = umlLine.GetEndUmlClass

                If Not umlClass Is Nothing Then

                    If umlClass Is Me Then

                        If Not lines.Contains(umlLine) Then

                            lines.Add(umlLine)

                        End If

                    End If

                End If

            Next

            Return lines

        End Function


        Public Sub MoveSelected(ByVal X As Integer, ByVal Y As Integer, ByVal lastX As Integer, ByVal lastY As Integer)

            If m_Selected Then

                Dim diffX As Integer = X - lastX
                Dim diffY As Integer = Y - lastY

                Dim zoom As Double = GetZoom()

                If zoom = 0 Then zoom = 1

                Me.Location = New Point(Me.Location.X + CInt(diffX / zoom), Me.Location.Y + CInt(diffY / zoom))

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


        Public Sub Remove()

            If m_UmlDiagram Is Nothing Then Exit Sub

            RemoveLines()

            Me.UmlDiagram.UmlClasses.Remove(Me)

        End Sub


        Public Sub RemoveLines()

            If m_UmlDiagram Is Nothing Then Exit Sub

            Dim umlLine As umlLine

            For Each umlLine In GetConnectedUmlLines()

                umlLine.UmlDiagram.UmlLines.Remove(umlLine)

            Next

        End Sub

        Public Overloads Overrides Sub Accept(ByVal mapVisitor As Puzzle.NPersist.Framework.Mapping.Visitor.IMapVisitor)

        End Sub
    End Class

End Namespace
