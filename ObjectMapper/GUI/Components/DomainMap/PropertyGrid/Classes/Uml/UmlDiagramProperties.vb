Imports Puzzle.NPersist.Framework.Mapping
Imports System.ComponentModel
Imports Puzzle.ObjectMapper.GUI.Uml
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class UmlDiagramProperties
    Inherits PropertiesBase

    Private m_UmlDiagram As UmlDiagram

    Public Event BeforePropertySet(ByVal mapObject As UmlDiagram, ByVal propertyName As String, ByVal value As Object, ByVal oldValue As Object)

    Public Event AfterPropertySet(ByVal mapObject As UmlDiagram, ByVal propertyName As String)

    Public Function GetUmlDiagram() As UmlDiagram
        Return m_UmlDiagram
    End Function

    Public Sub SetUmlDiagram(ByVal value As UmlDiagram)
        m_UmlDiagram = value
    End Sub


    Public Overrides Function GetMapObject() As IMap

        Return m_UmlDiagram

    End Function

    <Category("Design"), _
    Description("The name of this diagram."), _
    DisplayName("Name"), _
    DefaultValue("")> Public Property Name() As String
        Get
            Return m_UmlDiagram.Name
        End Get
        Set(ByVal Value As String)
            m_UmlDiagram.Name = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "Name")
        End Set
    End Property

    <Category("Layout"), _
        Description("The amount of zoom used when displaying this diagram. Zoom must be a value larger than 0, where a value of between 0 and 1 indicates a zoom-out, a value larger than 1 indicates a zoom-in and the value 1 indicates no zoom in or out."), _
        DisplayName("Zoom"), _
        DefaultValue(1)> Public Property Zoom() As Decimal
        Get
            Return m_UmlDiagram.Zoom
        End Get
        Set(ByVal Value As Decimal)
            m_UmlDiagram.Zoom = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "Zoom")
        End Set
    End Property

    '<Category("Layout"), _
    '    Description(""), _
    '    DisplayName("Selected"), _
    '    DefaultValue("")> Public Property Selected() As Boolean
    '    Get
    '        Return m_UmlDiagram.Selected
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        m_UmlDiagram.Selected = Value
    '        RaiseEvent AfterPropertySet(m_UmlDiagram, "Selected")
    '    End Set
    'End Property

    <Category("Grid"), _
        Description("Indicates whether the backgroud grid is displayed"), _
        DisplayName("Display grid"), _
        DefaultValue(True)> Public Property UseGrid() As Boolean
        Get
            Return m_UmlDiagram.UseGrid
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.UseGrid = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "UseGrid")
        End Set
    End Property

    <Category("Grid"), _
        Description("The size in pixels of the squares in the grid."), _
        DisplayName("Grid size"), _
        DefaultValue(50)> Public Property GridSize() As Double
        Get
            Return m_UmlDiagram.GridSize
        End Get
        Set(ByVal Value As Double)
            m_UmlDiagram.GridSize = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "GridSize")
        End Set
    End Property

    <Category("Grid"), _
        Description("The first color used when drawing the grid."), _
        DisplayName("GridColor1"), _
        DefaultValue("")> Public Property GridColor1() As Color
        Get
            Return m_UmlDiagram.GridColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.GridColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "GridColor1")
        End Set
    End Property

    <Category("Grid"), _
        Description("The second color used when drawing the grid."), _
        DisplayName("GridColor2"), _
        DefaultValue("")> Public Property GridColor2() As Color
        Get
            Return m_UmlDiagram.GridColor2.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.GridColor2.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "GridColor2")
        End Set
    End Property


    <Category("Shadows"), _
        Description("Indicates whether shadows are displayed on the diagram."), _
        DisplayName("Display shadows"), _
        DefaultValue(True)> Public Property DrawShadows() As Boolean
        Get
            Return m_UmlDiagram.DrawShadows
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DrawShadows = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DrawShadows")
        End Set
    End Property

    <Category("Shadows"), _
        Description("Indicates if shadows are drawn on top of objects behind the object making the shadow. If true, make sure that you set an appropriate Alpha value of your shadow colors so that the shadows become transparent. If false, all thadows are drawn in a layer of their own behind all the other objects (and the shadow colors need not be transparent."), _
        DisplayName("Draw shadows on top"), _
        DefaultValue(True)> Public Property DrawShadowsPass2() As Boolean
        Get
            Return m_UmlDiagram.DrawShadowsPass2
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DrawShadowsPass2 = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DrawShadowsPass2")
        End Set
    End Property

    <Category("Shadows"), _
        Description("The first color used when drawing shadows on the diagram."), _
        DisplayName("ShadowColor1"), _
        DefaultValue("")> Public Property ShadowColor1() As Color
        Get
            Return m_UmlDiagram.ShadowColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ShadowColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ShadowColor1")
        End Set
    End Property

    <Category("Shadows"), _
        Description("The second color used when drawing shadows on the diagram."), _
        DisplayName("ShadowColor2"), _
        DefaultValue("")> Public Property ShadowColor2() As Color
        Get
            Return m_UmlDiagram.ShadowColor2.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ShadowColor2.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ShadowColor2")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The brush used when drawing the background on this diagram."), _
        DisplayName("Brush"), _
        DefaultValue("")> Public Property BgBrushStyle() As BrushEnum
        Get
            Return m_UmlDiagram.BgBrushStyle
        End Get
        Set(ByVal Value As BrushEnum)
            m_UmlDiagram.BgBrushStyle = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "BgBrushStyle")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The gradient mode used when drawing the background on this diagram. Applies only if the GradientBrush brush has been selected."), _
        DisplayName("Gradient mode"), _
        DefaultValue("")> Public Property BgGradientMode() As LinearGradientMode
        Get
            Return m_UmlDiagram.BgGradientMode
        End Get
        Set(ByVal Value As LinearGradientMode)
            m_UmlDiagram.BgGradientMode = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "BgGradientMode")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The color used to display synchronization errors on this diagram. Synchronization errors are when the shapes on the diagram are out of synch with the domain model - for instance if there is a class on your diagram that does not have a class with the same name in the domain model."), _
        DisplayName("SynchErrorColor"), _
        DefaultValue("")> Public Property ColorSynchError() As Color
        Get
            Return m_UmlDiagram.ColorSynchError.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ColorSynchError.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ColorSynchError")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The color used to display verification errors on this diagram. Verification errors are the errors that you can also find by the red nodes in the verification tree view."), _
        DisplayName("ErrorColor"), _
        DefaultValue("")> Public Property ColorError() As Color
        Get
            Return m_UmlDiagram.ColorError.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ColorError.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ColorError")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The color used for drawing lines on this diagram."), _
        DisplayName("ForeColor"), _
        DefaultValue("")> Public Property ForeColor() As Color
        Get
            Return m_UmlDiagram.ForeColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ForeColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ForeColor")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The first color used when drawing the background for this diagram."), _
        DisplayName("BackColor1"), _
        DefaultValue("")> Public Property BackColor1() As Color
        Get
            Return m_UmlDiagram.BackColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.BackColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "BackColor1")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The second color used when drawing the background for this diagram."), _
        DisplayName("BackColor2"), _
        DefaultValue("")> Public Property BackColor2() As Color
        Get
            Return m_UmlDiagram.BackColor2.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.BackColor2.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "BackColor2")
        End Set
    End Property

    <Category("Shadows"), _
        Description("The brush used when drawing shadows on this diagram."), _
        DisplayName("Shadow brush"), _
        DefaultValue("")> Public Property ShadowBrushStyle() As BrushEnum
        Get
            Return m_UmlDiagram.ShadowBrushStyle
        End Get
        Set(ByVal Value As BrushEnum)
            m_UmlDiagram.ShadowBrushStyle = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ShadowBrushStyle")
        End Set
    End Property

    <Category("Shadows"), _
        Description("The gradient mode used when drawing shadows on this diagram. Applies only if the GradientBrush shadow brush has been selected."), _
        DisplayName("Shadow gradient mode"), _
        DefaultValue("")> Public Property ShadowGradientMode() As LinearGradientMode
        Get
            Return m_UmlDiagram.ShadowGradientMode
        End Get
        Set(ByVal Value As LinearGradientMode)
            m_UmlDiagram.ShadowGradientMode = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ShadowGradientMode")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The font used for displaying text on this diagram."), _
        DisplayName("Font"), _
        DefaultValue("")> Public Property Font() As Font
        Get
            Return New Font(m_UmlDiagram.FontFamily, m_UmlDiagram.FontSize, m_UmlDiagram.FontStyle)
        End Get
        Set(ByVal Value As Font)
            m_UmlDiagram.FontFamily = Value.FontFamily.Name
            m_UmlDiagram.FontSize = Value.Size
            m_UmlDiagram.FontStyle = Value.Style
            RaiseEvent AfterPropertySet(m_UmlDiagram, "Font")
        End Set
    End Property


    '<Category("Appearance"), _
    '    Description(""), _
    '    DisplayName("FontSize"), _
    '    DefaultValue("")> Public Property FontSize() As Single
    '    Get
    '        Return m_UmlDiagram.FontSize
    '    End Get
    '    Set(ByVal Value As Single)
    '        m_UmlDiagram.FontSize = Value
    '        RaiseEvent AfterPropertySet(m_UmlDiagram, "FontSize")
    '    End Set
    'End Property

    '<Category("Appearance"), _
    '    Description(""), _
    '    DisplayName("FontFamily"), _
    '    DefaultValue("")> Public Property FontFamily() As String
    '    Get
    '        Return m_UmlDiagram.FontFamily
    '    End Get
    '    Set(ByVal Value As String)
    '        m_UmlDiagram.FontFamily = Value
    '        RaiseEvent AfterPropertySet(m_UmlDiagram, "FontFamily")
    '    End Set
    'End Property

    <Category("Appearance"), _
        Description("The color used when displaying text on this diagram."), _
        DisplayName("FontColor"), _
        DefaultValue("")> Public Property FontColor() As Color
        Get
            Return m_UmlDiagram.FontColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.FontColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "FontColor")
        End Set
    End Property

    '<Category("Appearance"), _
    '    Description(""), _
    '    DisplayName("FontStyle"), _
    '    DefaultValue("")> Public Property FontStyle() As FontStyle
    '    Get
    '        Return m_UmlDiagram.FontStyle
    '    End Get
    '    Set(ByVal Value As FontStyle)
    '        m_UmlDiagram.FontStyle = Value
    '        RaiseEvent AfterPropertySet(m_UmlDiagram, "FontStyle")
    '    End Set
    'End Property

    <Category("Appearance"), _
        Description("The brush used when drawing class shapes on this diagram."), _
        DisplayName("Class brush"), _
        DefaultValue("")> Public Property ClassBgBrushStyle() As BrushEnum
        Get
            Return m_UmlDiagram.ClassBgBrushStyle
        End Get
        Set(ByVal Value As BrushEnum)
            m_UmlDiagram.ClassBgBrushStyle = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ClassBgBrushStyle")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The gradient mode used when drawing class shapes on this diagram. Applies only if the GradientBrush class brush has been selected."), _
        DisplayName("Class gradient mode"), _
        DefaultValue("")> Public Property ClassBgGradientMode() As LinearGradientMode
        Get
            Return m_UmlDiagram.ClassBgGradientMode
        End Get
        Set(ByVal Value As LinearGradientMode)
            m_UmlDiagram.ClassBgGradientMode = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ClassBgGradientMode")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The first color used when drawing class shapes on this diagram."), _
        DisplayName("Class back color 1"), _
        DefaultValue("")> Public Property ClassBackColor1() As Color
        Get
            Return m_UmlDiagram.ClassBackColor1.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ClassBackColor1.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ClassBackColor1")
        End Set
    End Property

    <Category("Appearance"), _
        Description("The second color used when drawing class shapes on this diagram."), _
        DisplayName("Class back color 2"), _
        DefaultValue("")> Public Property ClassBackColor2() As Color
        Get
            Return m_UmlDiagram.ClassBackColor2.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.ClassBackColor2.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ClassBackColor2")
        End Set
    End Property

    <Category("Appearance"), _
        Description("Indicates whether arrow heads are displayed for line ends representing associations on this diagram."), _
        DisplayName("Display arrow heads"), _
        DefaultValue(False)> Public Property DisplayAssociationArrows() As Boolean
        Get
            Return m_UmlDiagram.DisplayAssociationArrows
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DisplayAssociationArrows = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DisplayAssociationArrows")
        End Set
    End Property


    <Category("Appearance"), _
        Description("The length in pixels of the base of the arrow heads drawn at line ends on this diagram."), _
        DisplayName("Arrow base length"), _
        DefaultValue("")> Public Property ArrowBaseLength() As Double
        Get
            Return m_UmlDiagram.ArrowBaseLength
        End Get
        Set(ByVal Value As Double)
            m_UmlDiagram.ArrowBaseLength = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "ArrowBaseLength")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The height and width in pixels of the navigator window."), _
        DisplayName("Navigator size"), _
        DefaultValue("")> Public Property RadarSize() As Integer
        Get
            Return m_UmlDiagram.RadarSize
        End Get
        Set(ByVal Value As Integer)
            m_UmlDiagram.RadarSize = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarSize")
        End Set
    End Property

    <Category("Navigator"), _
        Description("Indicates whether the navigator window is displayed."), _
        DisplayName("Display navigator"), _
        DefaultValue("")> Public Property DisplayRadar() As Boolean
        Get
            Return m_UmlDiagram.DisplayRadar
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DisplayRadar = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DisplayRadar")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The color used when drawing the navigator outline."), _
        DisplayName("Navigator fore color"), _
        DefaultValue("")> Public Property RadarForeColor() As Color
        Get
            Return m_UmlDiagram.RadarForeColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.RadarForeColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarForeColor")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The color used when drawing the navigator background."), _
        DisplayName("Navigator back color"), _
        DefaultValue("")> Public Property RadarBackColor() As Color
        Get
            Return m_UmlDiagram.RadarBackColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.RadarBackColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarBackColor")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The color used when drawing the outlines of the class shapes in the navigator."), _
        DisplayName("Navigator class fore color"), _
        DefaultValue("")> Public Property RadarClassForeColor() As Color
        Get
            Return m_UmlDiagram.RadarClassForeColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.RadarClassForeColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarClassForeColor")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The color used when drawing the background of the class shapes in the navigator."), _
        DisplayName("Navigator class back color"), _
        DefaultValue("")> Public Property RadarClassBackColor() As Color
        Get
            Return m_UmlDiagram.RadarClassBackColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.RadarClassBackColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarClassBackColor")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The color used when drawing the rectangle representing the current view area in the navigator."), _
        DisplayName("Navigator view color"), _
        DefaultValue("")> Public Property RadarViewColor() As Color
        Get
            Return m_UmlDiagram.RadarViewColor.ToColor
        End Get
        Set(ByVal Value As Color)
            m_UmlDiagram.RadarViewColor.FromColor(Value)
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarViewColor")
        End Set
    End Property

    <Category("Navigator"), _
        Description("The style used when drawing the rectangle representing the current view area in the navigator."), _
        DisplayName("Navigator view style"), _
        DefaultValue("")> Public Property RadarViewStyle() As DashStyle
        Get
            Return m_UmlDiagram.RadarViewStyle
        End Get
        Set(ByVal Value As DashStyle)
            m_UmlDiagram.RadarViewStyle = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "RadarViewStyle")
        End Set
    End Property

    <Category("Behavior"), _
        Description("Indicates whether inherited properties are displayed in the class shapes"), _
        DisplayName("Display inherited properties"), _
        DefaultValue(False)> Public Property DisplayInheritedProperties() As Boolean
        Get
            Return m_UmlDiagram.DisplayInheritedProperties
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DisplayInheritedProperties = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DisplayInheritedProperties")
        End Set
    End Property

    <Category("Behavior"), _
        Description("Indicates whether reference properties that are displayed with lines in the diagram will also be displayed in the class shapes."), _
        DisplayName("Display reference properties"), _
        DefaultValue(False)> Public Property DisplayLineProperties() As Boolean
        Get
            Return m_UmlDiagram.DisplayLineProperties
        End Get
        Set(ByVal Value As Boolean)
            m_UmlDiagram.DisplayLineProperties = Value
            RaiseEvent AfterPropertySet(m_UmlDiagram, "DisplayLineProperties")
        End Set
    End Property

End Class
