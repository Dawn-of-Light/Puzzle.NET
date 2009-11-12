Imports System.IO
Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports Puzzle.NPersist.Framework.Mapping.Serialization
Imports System.Xml
Imports System.Xml.Serialization
Imports Puzzle.ObjectMapper.GUI.Uml
Imports Puzzle.SourceCode
Imports Puzzle.Windows.Forms
Imports Puzzle.Windows.Forms.SyntaxBox
Imports Puzzle.Syntaxbox.DefaultSyntaxFiles

Public Class UserDocTabPage
    Inherits TabPage

    Private m_MazZoomSize As Double = 5

    Private m_Title As String
    Private m_FullText As String
    Private m_OriginalText As Boolean
    Private m_DocumentText As String
    Private m_LoadedFromPath As String
    Private m_SavedToPath As String
    Private m_DocumentType As frmDomainMapBrowser.MainDocumentType
    Private m_Dirty As Boolean
    Private m_FileUpdated As DateTime

    'Private WithEvents m_TextBox As TextBox
    'Private WithEvents m_SyntaxBox As Puzzle.Windows.Forms.SyntaxBoxControl

    'Private WithEvents m_TextBox As TextBox
    Private WithEvents m_TextBox As Puzzle.Windows.Forms.SyntaxBoxControl

    Private WithEvents m_Panel As Panel

    Private m_DomainMap As IDomainMap
    Private m_ClassMap As IClassMap
    Private m_CodeMap As ICodeMap

    Private m_UmlDiagram As UmlDiagram


    Private m_RefreshingXml As Boolean

    Private m_RefreshingUml As Boolean

    Public Event UpdatedDomain(ByVal domainMap As IDomainMap)

    Public Event TextBoxTextChanged()

    Public Event TextBoxMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

    Public Event TextBoxEnter(ByVal sender As Object)

    Public Event TextBoxLeave(ByVal sender As Object)

    Private m_frmDomainMapBrowser As frmDomainMapBrowser

    Private m_MouseDownPoint As Point
    Private m_MouseMoveLastPoint As Point
    Private m_MouseLeftIsDown As Boolean
    Private m_MouseRightIsDown As Boolean

    Private m_MouseDrag As Boolean
    Private m_MouseSelecting As Boolean
    Private m_MouseDragTriggerDistance As Integer = 5

    Private m_CtrlIsDown As Boolean

    Public Sub New()
        MyBase.New()

        SetupControls()

    End Sub


    Public Sub New(ByVal frm As frmDomainMapBrowser, ByVal domainMap As IDomainMap)
        MyBase.New()

        m_frmDomainMapBrowser = frm
        m_DomainMap = domainMap

        SetupControls(frmDomainMapBrowser.MainDocumentType.XML)

    End Sub

    Public Sub New(ByVal frm As frmDomainMapBrowser, ByVal text As String, ByVal title As String, ByVal documentType As frmDomainMapBrowser.MainDocumentType, ByVal classMap As IClassMap, ByVal codeMap As ICodeMap)
        MyBase.New()

        m_frmDomainMapBrowser = frm
        m_ClassMap = classMap
        m_CodeMap = codeMap

        m_FullText = text

        Me.Text = GetText(text)

        m_Title = title
        m_DocumentText = codeMap.Code
        m_DocumentType = documentType

        SetupControls(documentType)

        m_Dirty = False

    End Sub

    Public Sub New(ByVal frm As frmDomainMapBrowser, ByVal text As String, ByVal title As String, ByVal documentType As frmDomainMapBrowser.MainDocumentType, ByVal domainMap As IDomainMap, ByVal codeMap As ICodeMap)
        MyBase.New()

        m_frmDomainMapBrowser = frm
        m_DomainMap = domainMap
        m_CodeMap = codeMap

        m_FullText = text

        Me.Text = GetText(text)

        m_Title = title
        m_DocumentText = codeMap.Code
        m_DocumentType = documentType

        SetupControls(documentType)

        m_Dirty = False

    End Sub


    Public Sub New(ByVal frm As frmDomainMapBrowser, ByVal umlDiagram As UmlDiagram)
        MyBase.New()

        m_frmDomainMapBrowser = frm
        m_UmlDiagram = umlDiagram

        SetupUmlControls()

    End Sub

    Public Sub New(ByVal text As String)
        MyBase.New()

        m_FullText = text

        Me.Text = GetText(text)

        SetupControls()

        m_Dirty = True

    End Sub

    Public Sub New(ByVal text As String, ByVal title As String, ByVal documentText As String, ByVal documentType As frmDomainMapBrowser.MainDocumentType)
        MyBase.New()

        m_FullText = text

        Me.Text = GetText(text)

        m_Title = title
        m_DocumentText = documentText
        m_DocumentType = documentType

        SetupControls(documentType)

        m_Dirty = True

    End Sub

    Public Sub New(ByVal text As String, ByVal title As String, ByVal documentText As String, ByVal documentType As frmDomainMapBrowser.MainDocumentType, ByVal loadedFromPath As String)
        MyBase.New()

        Dim fileInfo As fileInfo
        Dim hadFile As Boolean

        m_FullText = text

        Me.Text = GetText(text)

        m_Title = title
        m_DocumentText = documentText
        m_DocumentType = documentType
        m_LoadedFromPath = loadedFromPath

        If File.Exists(loadedFromPath) Then

            fileInfo = New fileInfo(loadedFromPath)

            m_FileUpdated = fileInfo.LastWriteTime

            hadFile = True

        End If

        SetupControls(documentType)

        If hadFile Then m_Dirty = False

    End Sub


    Private Sub SetupControls(Optional ByVal documentType As frmDomainMapBrowser.MainDocumentType = frmDomainMapBrowser.MainDocumentType.Text)

        m_RefreshingXml = True

        Dim newSyntaxBox As New Puzzle.Windows.Forms.SyntaxBoxControl

        SetupSyntaxBox(newSyntaxBox)

        Me.Controls.Add(newSyntaxBox)
        newSyntaxBox.Dock = DockStyle.Fill
        newSyntaxBox.Document.Text = m_DocumentText
        newSyntaxBox.ScrollBars = ScrollBars.Both

        Dim fileName As String

        Dim configurator As SyntaxBoxConfigurator = New SyntaxBoxConfigurator(newSyntaxBox)

        Select Case documentType

            Case frmDomainMapBrowser.MainDocumentType.CodeCSharp

                configurator.SetupCSharp()

            Case frmDomainMapBrowser.MainDocumentType.CodeDelphi

                configurator.SetupDelphi()

            Case frmDomainMapBrowser.MainDocumentType.CodeVbNet

                configurator.SetupVBNet()

            Case frmDomainMapBrowser.MainDocumentType.DTD

                configurator.SetupSqlServer2KSql()

            Case frmDomainMapBrowser.MainDocumentType.Text

                configurator.SetupText()

            Case frmDomainMapBrowser.MainDocumentType.XML

                configurator.SetupXml()

            Case frmDomainMapBrowser.MainDocumentType.NPersist

                configurator.SetupXml()

            Case Else

                configurator.SetupText()

        End Select


        If Not Me.m_CodeMap Is Nothing Then

            AddHandler newSyntaxBox.TextChanged, AddressOf Me.HandleCodeMapTextChange

        End If

        'AddHandler newTextBox.Enter, AddressOf Me.HandleTextEnter
        'AddHandler newTextBox.Leave, AddressOf Me.HandleTextLeave

        'AddHandler newTextBox.TextChanged, AddressOf Me.HandleTextChange

        m_TextBox = newSyntaxBox

        m_RefreshingXml = False

    End Sub


    Public Sub HandleCodeMapTextChange(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not Me.m_CodeMap Is Nothing Then

            Me.m_CodeMap.Code = Me.TextBoxText
            If Not Me.DomainMap Is Nothing Then

                Me.DomainMap.Dirty = True

            ElseIf Not Me.ClassMap Is Nothing Then

                Me.ClassMap.DomainMap.Dirty = True

            End If

        End If

    End Sub

    Private Sub SetupSyntaxBox(ByVal syntaxBox As Puzzle.Windows.Forms.SyntaxBoxControl)

        Dim envSettings As EnvironmentSettings = LogServices.mainForm.m_ApplicationSettings.OptionSettings.EnvironmentSettings
        Dim fs As FontSetting
        Dim fss As FontSubSetting

        syntaxBox.AllowBreakPoints = True

        syntaxBox.AutoListVisible = False
        syntaxBox.BorderColor = System.Drawing.Color.Black
        syntaxBox.BorderStyle = Puzzle.Windows.Forms.BorderStyle.None
        syntaxBox.BracketBackColor = System.Drawing.Color.LightSteelBlue
        syntaxBox.BracketBorderColor = System.Drawing.Color.DarkBlue
        syntaxBox.BracketForeColor = System.Drawing.Color.Black
        syntaxBox.BracketMatching = True
        syntaxBox.BreakPointBackColor = System.Drawing.Color.DarkRed
        syntaxBox.BreakPointForeColor = System.Drawing.Color.White
        syntaxBox.ChildBorderColor = System.Drawing.Color.White
        syntaxBox.ChildBorderStyle = Puzzle.Windows.Forms.BorderStyle.None
        syntaxBox.Dock = System.Windows.Forms.DockStyle.Fill

        syntaxBox.FontSize = envSettings.GetFontSetting("Text Editor").FontSize
        syntaxBox.FontName = envSettings.GetFontSetting("Text Editor").FontFamily

        'syntaxBox.FontName = "Courier new"
        'syntaxBox.FontSize = 10.0F
        'syntaxBox.FontName = "Lucida Console"
        'syntaxBox.FontSize = 14.0F
        syntaxBox.GutterMarginBorderColor = System.Drawing.SystemColors.ControlDark
        syntaxBox.GutterMarginColor = System.Drawing.SystemColors.Control
        syntaxBox.GutterMarginWidth = 19
        syntaxBox.HighLightActiveLine = False
        syntaxBox.HighLightedLineColor = System.Drawing.Color.LightYellow
        syntaxBox.InactiveSelectionBackColor = System.Drawing.SystemColors.Highlight
        syntaxBox.Indent = Puzzle.Windows.Forms.SyntaxBox.IndentStyle.LastRow
        syntaxBox.InfoTipCount = 1
        syntaxBox.InfoTipImage = Nothing
        syntaxBox.InfoTipPosition = Nothing
        syntaxBox.InfoTipSelectedIndex = 0
        syntaxBox.InfoTipText = ""
        syntaxBox.InfoTipVisible = False
        syntaxBox.LineNumberBackColor = System.Drawing.SystemColors.Window
        syntaxBox.LineNumberBorderColor = System.Drawing.Color.Teal
        syntaxBox.LineNumberForeColor = System.Drawing.Color.Teal
        syntaxBox.LockCursorUpdate = False
        syntaxBox.Name = "syntaxBox"
        syntaxBox.OutlineColor = System.Drawing.SystemColors.ControlDark
        syntaxBox.ParseOnPaste = False
        syntaxBox.ReadOnly = False
        syntaxBox.RowPadding = 0
        syntaxBox.ScopeBackColor = System.Drawing.Color.Transparent
        syntaxBox.ScopeIndicatorColor = System.Drawing.Color.Transparent
        syntaxBox.SelectionBackColor = System.Drawing.SystemColors.Highlight
        syntaxBox.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        syntaxBox.SeparatorColor = System.Drawing.SystemColors.Control
        syntaxBox.ShowGutterMargin = True
        syntaxBox.ShowLineNumbers = True
        syntaxBox.ShowTabGuides = False
        syntaxBox.ShowWhitespace = False
        syntaxBox.Size = New System.Drawing.Size(672, 445)
        syntaxBox.SmoothScroll = False
        syntaxBox.SmoothScrollSpeed = 2
        syntaxBox.SplitviewH = -4
        syntaxBox.SplitviewV = -4
        syntaxBox.TabGuideColor = System.Drawing.Color.FromArgb((CByte(244)), (CByte(243)), (CByte(234)))
        syntaxBox.TabIndex = 0
        syntaxBox.TabSize = 4
        syntaxBox.TextDrawStyle = Puzzle.Windows.Forms.SyntaxBox.TextDraw.TextDrawType.StarBorder
        syntaxBox.TooltipDelay = 240
        syntaxBox.VirtualWhitespace = False
        syntaxBox.WhitespaceColor = System.Drawing.SystemColors.ControlDark
        'syntaxBox.KeyboardActions.Add(new SyntaxBox.KeyboardAction(Keys.C, False,True,False,True) 

    End Sub

    Private Sub SetupControlsOld()

        'm_RefreshingXml = True

        'Dim newTextBox As New TextBox

        'newTextBox.Multiline = True
        'Me.Controls.Add(newTextBox)
        'newTextBox.Dock = DockStyle.Fill
        'newTextBox.Text = m_DocumentText
        'newTextBox.Font = New Font("Courier New", 8.25, FontStyle.Regular, GraphicsUnit.Point)
        'newTextBox.ScrollBars = ScrollBars.Both
        'newTextBox.WordWrap = False
        'newTextBox.HideSelection = False

        ''AddHandler newTextBox.TextChanged, AddressOf Me.HandleTextChange

        ''AddHandler newTextBox.Enter, AddressOf Me.HandleTextEnter
        ''AddHandler newTextBox.Leave, AddressOf Me.HandleTextLeave

        ''AddHandler newTextBox.TextChanged, AddressOf Me.HandleTextChange

        'm_TextBox = newTextBox

        'm_RefreshingXml = False

    End Sub


    Private Sub SetupUmlControls()

        m_RefreshingUml = True

        Dim newPanel As New DblBufferPanel

        Me.Controls.Add(newPanel)
        newPanel.Dock = DockStyle.Fill
        newPanel.Text = m_DocumentText
        newPanel.BackColor = Color.White
        newPanel.Font = New Font("Courier New", 8.25, FontStyle.Regular, GraphicsUnit.Point)
        newPanel.AllowDrop = True

        'AddHandler newTextBox.TextChanged, AddressOf Me.HandleTextChange

        'AddHandler newTextBox.Enter, AddressOf Me.HandleTextEnter
        'AddHandler newTextBox.Leave, AddressOf Me.HandleTextLeave

        'AddHandler newTextBox.TextChanged, AddressOf Me.HandleTextChange

        m_Panel = newPanel

        m_RefreshingUml = False

    End Sub

    'Public Property TextBox() As TextBox
    '    Get
    '        Return m_TextBox
    '    End Get
    '    Set(ByVal Value As TextBox)
    '        m_TextBox = Value
    '    End Set
    'End Property

    Public Property ClassMap() As IClassMap
        Get
            Return m_ClassMap
        End Get
        Set(ByVal Value As IClassMap)
            m_ClassMap = Value
        End Set
    End Property

    Public Property CodeMap() As ICodeMap
        Get
            Return m_CodeMap
        End Get
        Set(ByVal Value As ICodeMap)
            m_CodeMap = Value
        End Set
    End Property

    Public Property TextBox() As Puzzle.Windows.Forms.SyntaxBoxControl
        Get
            Return m_TextBox
        End Get
        Set(ByVal Value As Puzzle.Windows.Forms.SyntaxBoxControl)
            m_TextBox = Value
        End Set
    End Property

    Public Property Panel() As Panel
        Get
            Return m_Panel
        End Get
        Set(ByVal Value As Panel)
            m_Panel = Value
        End Set
    End Property

    Public Property DomainMap() As IDomainMap
        Get
            Return m_DomainMap
        End Get
        Set(ByVal Value As IDomainMap)
            m_DomainMap = Value
        End Set
    End Property

    Public Property UmlDiagram() As UmlDiagram
        Get
            Return m_UmlDiagram
        End Get
        Set(ByVal Value As UmlDiagram)
            m_UmlDiagram = Value
        End Set
    End Property


    Public Property Title() As String
        Get
            If Not m_DomainMap Is Nothing Then
                Return m_DomainMap.Name
            ElseIf Not m_UmlDiagram Is Nothing Then
                Return m_UmlDiagram.Name
            Else
                Return m_Title
            End If
        End Get
        Set(ByVal Value As String)
            m_Title = Value
        End Set
    End Property

    Public Property Dirty() As Boolean
        Get
            Return m_Dirty
        End Get
        Set(ByVal Value As Boolean)
            m_Dirty = Value
        End Set
    End Property

    Public Property LoadedFromPath() As String
        Get
            Return m_LoadedFromPath
        End Get
        Set(ByVal Value As String)
            m_LoadedFromPath = Value
        End Set
    End Property


    Public Property SavedToPath() As String
        Get
            Return m_SavedToPath
        End Get
        Set(ByVal Value As String)
            m_SavedToPath = Value
        End Set
    End Property

    Public Property TextBoxText() As String
        Get
            Return m_TextBox.Document.Text
        End Get
        Set(ByVal Value As String)
            m_DocumentText = Value
            m_TextBox.Document.Text = Value
        End Set
    End Property


    Public Property DocumentText() As String
        Get
            Return m_DocumentText
        End Get
        Set(ByVal Value As String)
            m_DocumentText = Value
            'm_TextBox.Text = Value
            m_TextBox.Document.Text = Value
        End Set
    End Property

    Public Property DocumentType() As frmDomainMapBrowser.MainDocumentType
        Get
            Return m_DocumentType
        End Get
        Set(ByVal Value As frmDomainMapBrowser.MainDocumentType)
            m_DocumentType = Value
        End Set
    End Property

    Public Property FileUpdated() As DateTime
        Get
            Return m_FileUpdated
        End Get
        Set(ByVal Value As DateTime)
            m_FileUpdated = Value
        End Set
    End Property

    Public Sub Save(ByVal path As String)

        Dim fileOut As StreamWriter

        Try

            If File.Exists(path) Then

                File.Delete(path)

            End If

            fileOut = File.CreateText(path)

            fileOut.Write(m_DocumentText)
            fileOut.Close()

            SavedToPath = path
            Dirty = False
            m_FullText = path
            Me.Text = GetText(path)

        Catch ex As Exception

            MsgBox("Could not save file to path '" & path & "'")

        End Try

    End Sub

    Public Function GetFileName() As String

        Dim name As String = m_SavedToPath
        If name = "" Then name = m_LoadedFromPath
        If name = "" Then name = m_FullText

        Return name

    End Function

    Private Function GetText(ByVal str As String) As String

        Dim arr() As String = Split(str, "\")
        Dim arrPath() As String
        Dim path As String
        Dim name As String


        If UBound(arr) > 0 Then

            arrPath = arr.Clone
            ReDim Preserve arrPath(UBound(arrPath) - 1)

            path = Join(arrPath, "\")

            If Len(path) > 16 Then

                If UBound(arrPath) > 1 Then

                    path = arrPath(1)

                Else

                    path = arrPath(0)

                End If

                If Len(path) > 16 Then

                    path = path.Substring(path.Length - 16)

                End If

                path += "..."

            End If

        End If

        name = arr(UBound(arr))

        If Len(name) > 16 Then

            name = "..." & name.Substring(name.Length - 16)

        End If

        str = path & name

        Return str

    End Function

    Public Function GetFullText() As String
        Return m_FullText
    End Function

    Public Function GetFilePath() As String

        If Len(m_SavedToPath) > 0 Then Return m_SavedToPath
        Return m_LoadedFromPath

    End Function

    'Public Property TextFont() As Font
    '    Get
    '        Return m_TextBox.Font
    '    End Get
    '    Set(ByVal Value As Font)
    '        m_TextBox.Font = Value
    '    End Set
    'End Property

    Public Function ReplaceAllInSelection(ByVal find As String, ByVal replaceWith As String, Optional ByVal searchUp As Boolean = False, Optional ByVal compare As CompareMethod = CompareMethod.Binary, Optional ByVal startOver As Boolean = False, Optional ByVal inSelection As Boolean = False) As Integer

        'Begin by counting instances of the find term

        Dim cacheStart As Integer

        Dim text As String = m_TextBox.Selection.Text

        Dim i As Integer
        Dim cnt As Integer

        If Len(text) > 0 Then

            For i = 0 To Len(text) - Len(find)

                If compare = CompareMethod.Binary Then

                    If text.Substring(i, Len(find)) = find Then

                        cnt += 1

                    End If

                Else

                    If LCase(text.Substring(i, Len(find))) = LCase(find) Then

                        cnt += 1

                    End If

                End If

            Next

        End If

        If cnt > 0 Then

            text = Replace(text, find, replaceWith, 1, -1, compare)

            m_Dirty = True

            cacheStart = m_TextBox.Selection.SelStart
            m_TextBox.Selection.Text = text

            If Not cacheStart < 0 Then

                m_TextBox.Selection.SelStart = cacheStart

                m_TextBox.Selection.SelLength = text.Length

            End If

        End If

        Return cnt


    End Function

    Public Function ReplaceNext(ByVal find As String, ByVal replaceWith As String, Optional ByVal searchUp As Boolean = False, Optional ByVal compare As CompareMethod = CompareMethod.Binary, Optional ByVal startOver As Boolean = False, Optional ByVal inSelection As Boolean = False) As Boolean

        Dim cacheStart As Integer

        If FindNext(find, searchUp, compare, startOver, inSelection, True) Then

            m_Dirty = True

            cacheStart = m_TextBox.Selection.SelStart
            m_TextBox.Selection.Text = replaceWith

            If Not cacheStart < 0 Then

                m_TextBox.Selection.SelStart = cacheStart

                m_TextBox.Selection.SelLength = replaceWith.Length

            End If

            Return True

        End If

        Return False

    End Function


    Public Function FindNext(ByVal find As String, Optional ByVal searchUp As Boolean = False, Optional ByVal compare As CompareMethod = CompareMethod.Binary, Optional ByVal startOver As Boolean = False, Optional ByVal inSelection As Boolean = False, Optional ByVal forReplace As Boolean = False) As Boolean

        Dim start As Integer = m_TextBox.Selection.SelStart + 1
        Dim pos As Integer

        If Not forReplace Then

            If searchUp Then

                'If start < m_TextBox.Text.Length - find.Length Then
                If start < m_TextBox.Document.Text.Length - find.Length Then

                    If InStr(start, m_TextBox.Document.Text, find, compare) = start Then

                        start -= 1

                    End If

                End If

            Else

                If start > 1 Then

                    If InStr(start, m_TextBox.Document.Text, find, compare) = start Then

                        start += 1

                    End If

                End If

            End If

        End If

        If start > m_TextBox.Document.Text.Length Then

            'start = 0

        End If

        If searchUp Then

            pos = InStrRev(m_TextBox.Document.Text, find, start, compare)

        Else

            pos = InStr(start, m_TextBox.Document.Text, find, compare)

        End If

        If pos < 1 And startOver Then

            If searchUp Then

                If start < m_TextBox.Document.Text.Length - find.Length Then

                    pos = InStrRev(m_TextBox.Document.Text, find, m_TextBox.Document.Text.Length - find.Length, compare)

                End If

            Else

                If start > 1 Then

                    pos = InStr(m_TextBox.Document.Text, find, compare)

                End If

            End If

        End If


        If pos > 0 Then

            If inSelection Then

                If Not (pos >= m_TextBox.Selection.SelStart And pos <= m_TextBox.Selection.SelStart + m_TextBox.Selection.SelLength) Then

                    pos = 0

                End If

            End If

        End If

        If pos > 0 Then

            m_TextBox.Selection.SelStart = pos - 1
            m_TextBox.Selection.SelLength = find.Length

            m_TextBox.Caret.SetPos(New Puzzle.SourceCode.TextPoint(m_TextBox.Selection.Bounds.LastColumn, m_TextBox.Selection.Bounds.LastRow))

            'm_TextBox.ScrollToCaret()
            m_TextBox.ScrollIntoView()

            m_TextBox.Refresh()

            Return True

        Else

            Return False

        End If

    End Function

    Public Sub RefreshXML()

        If m_Dirty Then Exit Sub

        If m_RefreshingXml Then Exit Sub

        m_RefreshingXml = True

        Dim xml As String

        Dim cacheStart As Integer
        Dim cacheLength As Integer

        xml = GetXml()

        If xml <> m_TextBox.Document.Text Then

            cacheStart = m_TextBox.Selection.SelStart
            cacheLength = m_TextBox.Selection.SelLength

            m_TextBox.Document.Text = xml

            If cacheStart >= 0 Then m_TextBox.Selection.SelStart = cacheStart
            If cacheLength >= 0 Then m_TextBox.Selection.SelLength = cacheLength

            m_TextBox.Caret.SetPos(New Puzzle.SourceCode.TextPoint(m_TextBox.Selection.Bounds.LastColumn, m_TextBox.Selection.Bounds.LastRow))

            'm_TextBox.ScrollToCaret()
            m_TextBox.ScrollIntoView()

            m_TextBox.Refresh()

        End If

        m_Dirty = False

        m_RefreshingXml = False

        RaiseEvent TextBoxTextChanged()

    End Sub

    Private Function GetXml() As String

        Dim xml As String

        Select Case m_DomainMap.MapSerializer

            Case mapSerializer.DefaultSerializer

                Dim mapSerializer As New DefaultMapSerializer

                xml = mapSerializer.Serialize(m_DomainMap)

            Case mapSerializer.DotNetSerializer


                Try

                    Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(DomainMap))
                    ' To write to a file, create a StreamWriter object.
                    Dim myWriter As StringWriter = New StringWriter
                    mySerializer.Serialize(myWriter, m_DomainMap)
                    xml = myWriter.ToString
                    myWriter.Close()

                Catch ex As Exception

                    Throw New Exception("Could not serialize Domain Map! " & ex.Message, ex)

                End Try

            Case mapSerializer.CustomSerializer

                MsgBox("Can't serialize using custom serializer! Please select a different serialiser for you domain map!")

                Exit Function

        End Select

        Return xml

    End Function

    Private Sub SetXml()

        If m_RefreshingXml Then Exit Sub

        Dim xml As String = m_TextBox.Document.Text

        If xml = "" Then Exit Sub

        If Not IsValidXml(xml) Then Exit Sub

        Dim newDomainMap As IDomainMap



        Select Case m_DomainMap.MapSerializer

            Case mapSerializer.DefaultSerializer

                Dim mapSerializer As New DefaultMapSerializer

                Try

                    newDomainMap = mapSerializer.Deserialize(xml)

                Catch ex As Exception

                    newDomainMap = Nothing

                End Try

            Case mapSerializer.DotNetSerializer

                Try

                    Dim mySerializer As XmlSerializer = New XmlSerializer(GetType(DomainMap))
                    ' To read the file, create a FileStream object.
                    Dim myReader As StringReader = New StringReader(xml)

                    newDomainMap = CType( _
                    mySerializer.Deserialize(myReader), DomainMap)

                    myReader.Close()


                    newDomainMap.Setup()

                Catch ex As Exception

                    newDomainMap = Nothing

                End Try

            Case mapSerializer.CustomSerializer

                MsgBox("Can't serialize using custom serializer! Please select a different serialiser for you domain map!")

                Exit Sub

        End Select

        If Not newDomainMap Is Nothing Then

            If Len(newDomainMap.Name) > 0 Then

                'by uncommenting this line,
                'xml outside the spec will keep the document dirty
                m_Dirty = False

                If Not m_DomainMap.DeepCompare(newDomainMap) Then

                    newDomainMap.DeepMerge(m_DomainMap)

                    m_DomainMap.Dirty = True

                    m_Dirty = False

                End If

                RaiseEvent UpdatedDomain(m_DomainMap)

            End If

        End If

        If m_Dirty Then

            MsgBox("The Xml Behind does not represent a valid npersist xml mapping file!", MsgBoxStyle.OKOnly, "Could not apply changes!")

        End If

    End Sub

    Private Function IsValidXml(ByVal xml As String)

        Try

            Dim xmlDoc As New XmlDocument

            xmlDoc.LoadXml(xml)

            Dim xmlDom As XmlNode

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function


    Private Sub m_TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_TextBox.TextChanged

        'm_DocumentText = CType(sender, TextBox).Text
        m_DocumentText = CType(sender, Puzzle.Windows.Forms.SyntaxBoxControl).Document.Text
        m_Dirty = True

        If Not m_DomainMap Is Nothing Then

            Select Case m_frmDomainMapBrowser.m_ApplicationSettings.OptionSettings.XmlBehindSettings.CompileXmlBehindTrigger

                Case CompileXmlBehindTrigger.OnLineChange

                    SetXml()

            End Select

        End If

        RaiseEvent TextBoxTextChanged()

    End Sub


    Private Sub m_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles m_TextBox.KeyDown

        If Not m_DomainMap Is Nothing Then

            Select Case e.KeyCode


                Case Keys.C

                    If e.Control Then

                        'MsgBox("YO")

                    End If

                Case Keys.U

                    If e.Control Then

                        SetXml()

                        e.Handled = True

                        Exit Sub

                    End If

                Case Keys.D

                    If e.Control Then

                        CancelChanges()

                        e.Handled = True

                        Exit Sub

                    End If

            End Select

            Select Case m_frmDomainMapBrowser.m_ApplicationSettings.OptionSettings.XmlBehindSettings.CompileXmlBehindTrigger

                Case CompileXmlBehindTrigger.OnEveryChange

                    SetXml()

                Case CompileXmlBehindTrigger.OnLineChange

                    Select Case e.KeyCode

                        Case Keys.Enter, Keys.Up, Keys.Down

                    End Select

            End Select

        End If

    End Sub


    Private Sub CancelChanges()

        If m_Dirty Then

            If MsgBox("Do you really want to discard changes to the XML Behind for domain model '" & m_DomainMap.Name & "'?", MsgBoxStyle.OKCancel, "Discard Changes") = MsgBoxResult.OK Then

                m_Dirty = False

                RefreshXML()

                RaiseEvent TextBoxTextChanged()

            End If

        End If

    End Sub

    Public Sub ApplyChanges()

        SetXml()

    End Sub

    Public Sub DiscardChanges()

        CancelChanges()

    End Sub


    Private Sub InitializeComponent()
        '
        'UserDocTabPage
        '
        Me.AllowDrop = True

    End Sub

    Private Sub m_TextBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_TextBox.MouseUp

        RaiseEvent TextBoxMouseUp(sender, e)

    End Sub

    Private Sub m_TextBox_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_TextBox.Enter

        RaiseEvent TextBoxEnter(sender)

    End Sub

    Private Sub m_TextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_TextBox.Leave

        RaiseEvent TextBoxLeave(sender)

    End Sub



    Public Sub RefreshUml()

        If Not m_Panel Is Nothing Then

            m_Panel.Refresh()

        End If

    End Sub

    Private Sub m_Panel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles m_Panel.Paint

        Dim checkMappings As Boolean

        If Not m_UmlDiagram Is Nothing Then

            If Not m_UmlDiagram.Moving Then

                If Not m_frmDomainMapBrowser Is Nothing Then

                    checkMappings = m_frmDomainMapBrowser.m_ApplicationSettings.OptionSettings.VerificationSettings.VerifyMappings

                End If

            End If

            m_UmlDiagram.Size = Me.Size
            m_UmlDiagram.Render(e.Graphics, checkMappings)

            If m_MouseSelecting Then

                DrawSelectionRectangle(m_UmlDiagram, e.Graphics)

            End If

        End If

    End Sub

    Private Sub m_Panel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_Panel.MouseDown

        If m_UmlDiagram Is Nothing Then Exit Sub

        m_MouseDownPoint = New Point(e.X, e.Y)
        m_MouseMoveLastPoint = New Point(e.X, e.Y)


        m_MouseDrag = False
        m_MouseSelecting = False

        If e.Button = MouseButtons.Left Then

            m_MouseLeftIsDown = True

        ElseIf e.Button = MouseButtons.Right Then

            m_MouseRightIsDown = True

        End If

    End Sub

    Private Sub m_Panel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_Panel.MouseMove

        If m_UmlDiagram Is Nothing Then Exit Sub

        Dim doRefresh As Boolean

        Dim addToSelection As Boolean
        Dim lineStart As Boolean
        Dim lineEnd As Boolean

        Dim hitObjects As ArrayList
        Dim hitPropertyMap As IPropertyMap = Nothing

        If m_MouseLeftIsDown = True Then

            If Not m_MouseSelecting Then

                If Not m_MouseDrag Then

                    If TriggerMouseDrag(e.X, e.Y) Then

                        m_MouseDrag = True

                        'logic:
                        'if a control is clicked, and
                        'A) No shift/ctrl
                        '1) It was not already selected, then all other controls should be deselected
                        '2) Then the control should always become selected
                        'B) Shift/ctrl
                        '1) The other controls should not become deselected
                        '2) Then the control should /always become selected/

                        If m_frmDomainMapBrowser.ModifierKeys = Keys.Shift Then

                            addToSelection = True

                        ElseIf m_frmDomainMapBrowser.ModifierKeys = Keys.Control Then

                            addToSelection = True

                        End If

                        hitObjects = New ArrayList

                        m_UmlDiagram.HitTest(m_MouseDownPoint.X, m_MouseDownPoint.Y, hitObjects, lineStart, lineEnd, hitPropertyMap)
                        'Dim umlClass As UmlClass = m_UmlDiagram.HitTestClass(e.X, e.Y)

                        If Not hitObjects.Count < 1 Then

                            If Not addToSelection Then

                                If Not m_UmlDiagram.AreAllSelected(hitObjects, lineStart, lineEnd) Then

                                    m_UmlDiagram.DeselectAll(hitObjects, lineStart, lineEnd)

                                End If

                                m_UmlDiagram.SelectAll(hitObjects, lineStart, lineEnd)

                            Else

                                'm_UmlDiagram.FlipSelection(hitObjects, lineStart, lineEnd)
                                m_UmlDiagram.SelectAll(hitObjects, lineStart, lineEnd)

                            End If

                            doRefresh = True

                        Else


                        End If

                    End If

                End If

                If m_MouseDrag Then

                    MoveSelected(e.X, e.Y)

                    doRefresh = True

                End If

            End If

        ElseIf m_MouseRightIsDown Then

            If Not m_MouseDrag Then

                If Not m_MouseSelecting Then

                    If TriggerMouseDrag(e.X, e.Y) Then

                        m_MouseSelecting = True

                    End If

                End If

                If m_MouseSelecting Then

                    m_MouseMoveLastPoint = New Point(e.X, e.Y)

                    doRefresh = True

                End If

            End If

        End If

        If doRefresh Then

            m_UmlDiagram.Moving = True

            RefreshUml()

            m_UmlDiagram.Moving = False

        End If

    End Sub

    Private Sub m_Panel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_Panel.MouseUp

        m_MouseLeftIsDown = False
        m_MouseRightIsDown = False

        Dim addToSelection As Boolean
        Dim lineStart As Boolean
        Dim lineEnd As Boolean

        Dim hitObjects As ArrayList
        Dim hitPropertyMap As IPropertyMap = Nothing

        Dim listExceptions As ArrayList = Nothing

        Dim didRefresh As Boolean

        If m_UmlDiagram Is Nothing Then Exit Sub

        m_UmlDiagram.Moving = False

        hitObjects = New ArrayList

        Dim classMap As IClassMap

        m_UmlDiagram.HitTest(e.X, e.Y, hitObjects, lineStart, lineEnd, hitPropertyMap)

        If e.Button = MouseButtons.Left Then

            If m_MouseDrag Then

                MoveSelected(e.X, e.Y, False)

            Else
                'Mouse was simply clicked

                'logic:
                'if a control is clicked, and
                'A) No shift/ctrl
                '1) It was not already selected, then all other controls should be deselected
                '2) Then the control should always become selected
                'B) Shift/ctrl
                '1) The other controls should not become deselected
                '2) Then the control should /flip selected state/

                If m_frmDomainMapBrowser.ModifierKeys = Keys.Shift Then

                    addToSelection = True

                ElseIf m_frmDomainMapBrowser.ModifierKeys = Keys.Control Then

                    addToSelection = True

                End If

                'Dim umlClass As UmlClass = m_UmlDiagram.HitTestClass(e.X, e.Y)

                If Not hitObjects.Count < 1 Then

                    If Not addToSelection Then

                        If Not m_UmlDiagram.AreAllSelected(hitObjects, lineStart, lineEnd) Then

                            m_UmlDiagram.DeselectAll(hitObjects, lineStart, lineEnd)

                        End If

                        m_UmlDiagram.SelectAll(hitObjects, lineStart, lineEnd)

                    Else

                        m_UmlDiagram.FlipSelection(hitObjects, lineStart, lineEnd)

                    End If

                    RefreshUml()

                Else


                End If

            End If


        ElseIf e.Button = MouseButtons.Right Then

            If m_MouseSelecting Then

                If m_frmDomainMapBrowser.ModifierKeys = Keys.Shift Then

                    addToSelection = True

                ElseIf m_frmDomainMapBrowser.ModifierKeys = Keys.Control Then

                    addToSelection = True

                End If

                m_MouseMoveLastPoint = New Point(e.X, e.Y)

                SelectAllInsideRectangle(addToSelection)

                m_MouseSelecting = False

                RefreshUml()

            Else

                'We want popup

                If Not hitPropertyMap Is Nothing Then

                    m_frmDomainMapBrowser.ShowMapObjectContextMenu(hitPropertyMap, e.X, e.Y, m_Panel, True)

                Else

                    Select Case hitObjects.Count

                        Case 0

                        Case 1

                            If hitObjects(0).GetType Is GetType(UmlClass) Then

                                classMap = CType(hitObjects(0), UmlClass).GetClassMap

                                If Not classMap Is Nothing Then

                                    m_frmDomainMapBrowser.ShowMapObjectContextMenu(classMap, e.X, e.Y, m_Panel, True)

                                Else

                                    '??? show context manu for just shape here ???

                                End If

                            ElseIf hitObjects(0).GetType Is GetType(UmlDiagram) Then

                                m_frmDomainMapBrowser.ShowMapObjectContextMenu(hitObjects(0), e.X, e.Y, m_Panel, True)


                            ElseIf hitObjects(0).GetType Is GetType(UmlLine) Then

                                m_frmDomainMapBrowser.ShowMapObjectContextMenu(hitObjects(0), e.X, e.Y, m_Panel, True, lineStart)

                            ElseIf hitObjects(0).GetType Is GetType(UmlLinePoint) Then

                                m_frmDomainMapBrowser.ShowMapObjectContextMenu(hitObjects(0), e.X, e.Y, m_Panel, True)

                            Else

                            End If

                        Case Else

                    End Select

                End If

            End If

        End If

        m_MouseDrag = False
        m_MouseSelecting = False

        If Not hitPropertyMap Is Nothing Then

            'm_frmDomainMapBrowser.SelectPropertiesForMapObject(hitPropertyMap, lineStart, lineEnd)
            m_frmDomainMapBrowser.SelectTreeNodeForMapObject(hitPropertyMap, lineStart, lineEnd)

            Select Case hitObjects.Count

                Case 0

                Case 1

                    If hitObjects(0).GetType Is GetType(UmlClass) Then

                        listExceptions = CType(hitObjects(0), UmlClass).PropertyExceptions(hitPropertyMap)

                        If listExceptions Is Nothing Then

                            listExceptions = CType(hitObjects(0), UmlClass).ListExceptions

                        Else

                            If listExceptions.Count < 1 Then

                                listExceptions = CType(hitObjects(0), UmlClass).ListExceptions

                            End If

                        End If

                        'ElseIf hitObjects(0).GetType Is GetType(UmlLine) Then

                        '    listExceptions = CType(hitObjects(0), UmlLine).PropertyExceptions(hitPropertyMap)

                    End If

                Case Else

            End Select

        Else

            Select Case hitObjects.Count

                Case 0

                Case 1

                    'm_frmDomainMapBrowser.SelectPropertiesForMapObject(hitObjects(0), lineStart, lineEnd)
                    m_frmDomainMapBrowser.SelectTreeNodeForMapObject(hitObjects(0), lineStart, lineEnd)

                    If hitObjects(0).GetType Is GetType(UmlClass) Then

                        listExceptions = CType(hitObjects(0), UmlClass).ListExceptions

                    ElseIf hitObjects(0).GetType Is GetType(UmlLine) Then

                        listExceptions = CType(hitObjects(0), UmlLine).ListExceptions

                    End If

                Case Else

            End Select

        End If



        If Not listExceptions Is Nothing Then

            If listExceptions.Count > 0 Then

                m_frmDomainMapBrowser.ClearErrorMsgs()

                Dim mapException As MappingException

                For Each mapException In listExceptions

                    m_frmDomainMapBrowser.AddErrorMsg(mapException)

                Next

            End If

        End If


    End Sub

    Private Sub m_Panel_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Panel.MouseLeave

        'm_MouseDrag = False

        m_MouseLeftIsDown = False
        m_MouseRightIsDown = False

    End Sub


    Private Function TriggerMouseDrag(ByVal X As Integer, ByVal Y As Integer) As Boolean

        Dim diffX As Integer = X - m_MouseDownPoint.X
        Dim diffY As Integer = Y - m_MouseDownPoint.Y

        If diffX > m_MouseDragTriggerDistance Or diffX < -m_MouseDragTriggerDistance Then Return True
        If diffY > m_MouseDragTriggerDistance Or diffY < -m_MouseDragTriggerDistance Then Return True

    End Function

    Private Sub MoveSelected(ByVal X As Integer, ByVal Y As Integer, Optional ByVal Moving As Boolean = True)

        m_UmlDiagram.Moving = Moving

        m_UmlDiagram.MoveSelected(X, Y, m_MouseMoveLastPoint.X, m_MouseMoveLastPoint.Y)

        m_MouseMoveLastPoint = New Point(X, Y)

        RefreshUml()

        m_frmDomainMapBrowser.m_ProjectDirty = True

        m_UmlDiagram.Moving = False

    End Sub

    Public Sub m_Panel_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) 'Handles m_Panel.MouseWheel

        Dim numberOfStepsLinesToZoom As Integer = e.Delta * SystemInformation.MouseWheelScrollLines / 120

        If numberOfStepsLinesToZoom <> 0 Then

            numberOfStepsLinesToZoom = numberOfStepsLinesToZoom / SystemInformation.MouseWheelScrollLines

            Zoom(0.1 * numberOfStepsLinesToZoom)

        End If

    End Sub

    Public Sub Zoom(ByVal stepSize As Double)

        If m_UmlDiagram Is Nothing Then Exit Sub

        m_UmlDiagram.ZoomInOut(stepSize)

    End Sub



    Private Sub m_Panel_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_Panel.DragDrop

        Dim data As String
        Dim dataType As String

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String
        Dim domName As String

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim umlDiagram As umlDiagram
        Dim DoHilite As Boolean

        e.Effect = DragDropEffects.None

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                ok = False

                                Exit For

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            arr = Split(data, "|")

            key = arr(1)
            domName = arr(2)

            sourceDomainMap = m_frmDomainMapBrowser.m_Project.GetDomainMap(domName)

            If dataType = "umldiagram" Then

                umlDiagram = m_frmDomainMapBrowser.GetMapObjectFromKey(sourceDomainMap, key, GetType(umlDiagram))

                m_frmDomainMapBrowser.ViewUmlDiagram(umlDiagram)

            Else

                If sourceDomainMap Is m_UmlDiagram.GetDomainMap Then

                    Select Case dataType

                        Case "propertymap"

                            propertyMap = m_frmDomainMapBrowser.GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                            If Not propertyMap.ReferenceType = ReferenceType.None Then

                                m_frmDomainMapBrowser.AddPropertyAssociationLineToUmlDoc(propertyMap, Me)

                            End If

                        Case "classmap"

                            classMap = m_frmDomainMapBrowser.GetMapObjectFromKey(sourceDomainMap, key, GetType(classMap))

                            m_frmDomainMapBrowser.AddClassToUmlDoc(classMap, Me.PointToClient(New Point(e.X, e.Y)), Me)

                    End Select

                End If

            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub m_Panel_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_Panel.DragEnter

    End Sub

    Private Sub m_Panel_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Panel.DragLeave

    End Sub

    Private Sub m_Panel_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_Panel.DragOver

        Dim data As String
        Dim dataType As String

        Dim arr() As String
        Dim arrFiles() As String

        Dim ok As Boolean

        Dim sourceDomainMap As IDomainMap
        Dim sourceObject As Object

        Dim key As String
        Dim domName As String

        Dim classMap As IClassMap
        Dim propertyMap As IPropertyMap
        Dim DoHilite As Boolean

        e.Effect = DragDropEffects.None

        Try

            If e.Data.GetDataPresent(GetType(String)) Then

                data = e.Data.GetData(GetType(String))

            Else

                If e.Data.GetDataPresent("FileNameW") Then

                    arrFiles = e.Data.GetData("FileNameW")

                    ok = True

                    For Each data In arrFiles

                        arr = Split(data, "\")

                        arr = Split(arr(UBound(arr)), ".")

                        Select Case LCase(arr(UBound(arr)))

                            Case "npersist"

                                ok = False

                                Exit For

                            Case Else

                                ok = False

                                Exit For

                        End Select

                    Next

                    If ok Then
                        e.Effect = DragDropEffects.Copy
                    End If

                    Exit Sub

                End If

            End If

            dataType = LCase(GetDropDataType(data))

            arr = Split(data, "|")

            key = arr(1)
            domName = arr(2)

            sourceDomainMap = m_frmDomainMapBrowser.m_Project.GetDomainMap(domName)

            If dataType = "umldiagram" Then

                DoHilite = True

            Else

                If sourceDomainMap Is m_UmlDiagram.GetDomainMap Then

                    Select Case dataType

                        Case "propertymap"

                            propertyMap = m_frmDomainMapBrowser.GetMapObjectFromKey(sourceDomainMap, key, GetType(propertyMap))

                            If Not propertyMap.ReferenceType = ReferenceType.None Then

                                DoHilite = True

                            End If

                        Case "classmap"

                            DoHilite = True

                    End Select

                End If

            End If

            If DoHilite Then
                e.Effect = DragDropEffects.Copy
            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function GetDropDataType(ByVal data As String) As String

        Dim arr() As String

        arr = Split(data, "|")

        Return arr(0)

    End Function

    Public Function DisplaysClassMap(ByVal classMap As IClassMap) As Boolean

        If Not GetUmlClassForClassMap(classMap) Is Nothing Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function GetUmlClassForClassMap(ByVal classMap As IClassMap) As UmlClass

        If m_UmlDiagram Is Nothing Then Return Nothing

        Dim domainMap As IDomainMap = m_UmlDiagram.GetDomainMap

        If domainMap Is Nothing Then Return Nothing

        If Not classMap.DomainMap Is domainMap Then Return Nothing

        Dim umlClass As umlClass
        Dim checkClassMap As IClassMap

        For Each umlClass In m_UmlDiagram.UmlClasses

            checkClassMap = umlClass.GetClassMap

            If Not checkClassMap Is Nothing Then

                If checkClassMap Is classMap Then

                    Return umlClass

                End If

            End If

        Next

        Return Nothing

    End Function

    Public Function DisplaysClassMapGeneralization(ByVal classMap As IClassMap) As Boolean

        If Not GetGeneralizationUmlLineForClassMap(classMap) Is Nothing Then

            Return True

        Else

            Return False

        End If

    End Function

    Public Function GetGeneralizationUmlLineForClassMap(ByVal classMap As IClassMap) As UmlLine

        If m_UmlDiagram Is Nothing Then Return Nothing

        Dim domainMap As IDomainMap = m_UmlDiagram.GetDomainMap

        If domainMap Is Nothing Then Return Nothing

        If Not classMap.DomainMap Is domainMap Then Return Nothing

        Dim superClassMap As IClassMap = classMap.GetInheritedClassMap

        If superClassMap Is Nothing Then Return Nothing

        Dim umlLine As umlLine
        Dim checkClassMap As IClassMap
        Dim checkClassMap2 As IClassMap

        For Each umlLine In m_UmlDiagram.UmlLines

            If umlLine.LineType = LineTypeEnum.Generalization Then

                checkClassMap = umlLine.GetStartPropertyMap

                If Not checkClassMap Is Nothing Then

                    If checkClassMap Is classMap Then

                        checkClassMap2 = umlLine.GetEndClassMap

                        If Not checkClassMap2 Is Nothing Then

                            If checkClassMap2 Is superClassMap Then

                                Return umlLine

                            End If

                        End If

                    End If

                End If

                checkClassMap = umlLine.GetEndPropertyMap

                If Not checkClassMap Is Nothing Then

                    If checkClassMap Is classMap Then

                        checkClassMap2 = umlLine.GetStartClassMap

                        If Not checkClassMap2 Is Nothing Then

                            If checkClassMap2 Is superClassMap Then

                                Return umlLine

                            End If

                        End If

                    End If

                End If

            End If

        Next

        Return Nothing

    End Function


    Public Function DisplaysPropertyMap(ByVal propertyMap As IPropertyMap) As Boolean

        If Not GetUmlLineForPropertyMap(propertyMap) Is Nothing Then

            Return True

        Else

            Return False

        End If

    End Function


    Public Function GetUmlLineForPropertyMap(ByVal propertyMap As IPropertyMap) As UmlLine

        If m_UmlDiagram Is Nothing Then Return Nothing

        Dim domainMap As IDomainMap = m_UmlDiagram.GetDomainMap

        If domainMap Is Nothing Then Return Nothing

        Dim classMap As IClassMap = propertyMap.ClassMap

        If Not classMap.DomainMap Is domainMap Then Return Nothing

        Dim umlLine As umlLine
        Dim checkPropertyMap As IPropertyMap

        For Each umlLine In m_UmlDiagram.UmlLines

            If umlLine.LineType = LineTypeEnum.Association Then

                checkPropertyMap = umlLine.GetStartPropertyMap

                If Not checkPropertyMap Is Nothing Then

                    If checkPropertyMap Is propertyMap Then

                        Return umlLine

                    End If

                End If

                checkPropertyMap = umlLine.GetEndPropertyMap

                If Not checkPropertyMap Is Nothing Then

                    If checkPropertyMap Is propertyMap Then

                        Return umlLine

                    End If

                End If

            End If

        Next

        Return Nothing

    End Function

    Public Sub FitUmlDiagram()

        If m_UmlDiagram Is Nothing Then Exit Sub

        Dim g As Graphics

        m_UmlDiagram.Size = Me.Size

        Dim i As Long

        For i = 0 To 5

            g = Me.CreateGraphics

            m_UmlDiagram.Fit(g)

            g.Dispose()

            RefreshUml()

        Next

    End Sub

    Private Sub DrawSelectionRectangle(ByVal diagram As UmlDiagram, ByVal g As Graphics)

        Dim pen As New pen(Color.Blue)

        pen.DashStyle = Drawing2D.DashStyle.DashDot

        g.DrawRectangle(pen, GetRectangle(m_MouseDownPoint.X, m_MouseDownPoint.Y, m_MouseMoveLastPoint.X, m_MouseMoveLastPoint.Y))

    End Sub

    Private Function GetRectangle(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer) As Rectangle

        Dim swap As Integer

        If x2 < x1 Then

            swap = x2
            x2 = x1
            x1 = swap

        End If

        If y2 < y1 Then

            swap = y2
            y2 = y1
            y1 = swap

        End If

        Return New Rectangle(x1, y1, x2 - x1, y2 - y1)

    End Function

    Private Sub SelectAllInsideRectangle(ByVal addToSelection As Boolean)

        If m_UmlDiagram Is Nothing Then Exit Sub

        If Not addToSelection Then

            m_UmlDiagram.DeselectAll()

        End If

        Dim rect As Rectangle = GetRectangle(m_MouseDownPoint.X, m_MouseDownPoint.Y, m_MouseMoveLastPoint.X, m_MouseMoveLastPoint.Y)

        m_UmlDiagram.SelectAllInsideRectangle(rect)

    End Sub

End Class
