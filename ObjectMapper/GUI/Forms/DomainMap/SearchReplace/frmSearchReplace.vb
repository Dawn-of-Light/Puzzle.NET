Public Class frmSearchReplace
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents comboFind As System.Windows.Forms.ComboBox
    Friend WithEvents comboReplace As System.Windows.Forms.ComboBox
    Friend WithEvents checkMatchCase As System.Windows.Forms.CheckBox
    Friend WithEvents checkSearchUp As System.Windows.Forms.CheckBox
    Friend WithEvents checkUse As System.Windows.Forms.CheckBox
    Friend WithEvents comboUse As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radioAllDocs As System.Windows.Forms.RadioButton
    Friend WithEvents radioCurrentDoc As System.Windows.Forms.RadioButton
    Friend WithEvents buttonFindNext As System.Windows.Forms.Button
    Friend WithEvents buttonReplace As System.Windows.Forms.Button
    Friend WithEvents buttonReplaceAll As System.Windows.Forms.Button
    Friend WithEvents buttonClose As System.Windows.Forms.Button
    Friend WithEvents radioSelectionOnly As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSearchReplace))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.comboFind = New System.Windows.Forms.ComboBox
        Me.comboReplace = New System.Windows.Forms.ComboBox
        Me.checkMatchCase = New System.Windows.Forms.CheckBox
        Me.checkSearchUp = New System.Windows.Forms.CheckBox
        Me.checkUse = New System.Windows.Forms.CheckBox
        Me.comboUse = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.radioSelectionOnly = New System.Windows.Forms.RadioButton
        Me.radioCurrentDoc = New System.Windows.Forms.RadioButton
        Me.radioAllDocs = New System.Windows.Forms.RadioButton
        Me.buttonFindNext = New System.Windows.Forms.Button
        Me.buttonReplace = New System.Windows.Forms.Button
        Me.buttonReplaceAll = New System.Windows.Forms.Button
        Me.buttonClose = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fi&nd what:"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Re&place with:"
        '
        'comboFind
        '
        Me.comboFind.Location = New System.Drawing.Point(96, 8)
        Me.comboFind.Name = "comboFind"
        Me.comboFind.Size = New System.Drawing.Size(288, 21)
        Me.comboFind.TabIndex = 2
        '
        'comboReplace
        '
        Me.comboReplace.Location = New System.Drawing.Point(96, 32)
        Me.comboReplace.Name = "comboReplace"
        Me.comboReplace.Size = New System.Drawing.Size(288, 21)
        Me.comboReplace.TabIndex = 3
        '
        'checkMatchCase
        '
        Me.checkMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkMatchCase.Location = New System.Drawing.Point(8, 56)
        Me.checkMatchCase.Name = "checkMatchCase"
        Me.checkMatchCase.TabIndex = 4
        Me.checkMatchCase.Text = "Match &case"
        '
        'checkSearchUp
        '
        Me.checkSearchUp.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkSearchUp.Location = New System.Drawing.Point(8, 80)
        Me.checkSearchUp.Name = "checkSearchUp"
        Me.checkSearchUp.TabIndex = 5
        Me.checkSearchUp.Text = "Search &up"
        '
        'checkUse
        '
        Me.checkUse.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkUse.Location = New System.Drawing.Point(8, 104)
        Me.checkUse.Name = "checkUse"
        Me.checkUse.Size = New System.Drawing.Size(56, 24)
        Me.checkUse.TabIndex = 6
        Me.checkUse.Text = "Us&e:"
        Me.checkUse.Visible = False
        '
        'comboUse
        '
        Me.comboUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboUse.Items.AddRange(New Object() {"Regular Expressions"})
        Me.comboUse.Location = New System.Drawing.Point(56, 104)
        Me.comboUse.Name = "comboUse"
        Me.comboUse.Size = New System.Drawing.Size(136, 21)
        Me.comboUse.TabIndex = 7
        Me.comboUse.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radioSelectionOnly)
        Me.GroupBox1.Controls.Add(Me.radioCurrentDoc)
        Me.GroupBox1.Controls.Add(Me.radioAllDocs)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(200, 56)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(184, 74)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'radioSelectionOnly
        '
        Me.radioSelectionOnly.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.radioSelectionOnly.Location = New System.Drawing.Point(8, 48)
        Me.radioSelectionOnly.Name = "radioSelectionOnly"
        Me.radioSelectionOnly.Size = New System.Drawing.Size(104, 16)
        Me.radioSelectionOnly.TabIndex = 2
        Me.radioSelectionOnly.Text = "Se&lection only"
        '
        'radioCurrentDoc
        '
        Me.radioCurrentDoc.Checked = True
        Me.radioCurrentDoc.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.radioCurrentDoc.Location = New System.Drawing.Point(8, 32)
        Me.radioCurrentDoc.Name = "radioCurrentDoc"
        Me.radioCurrentDoc.Size = New System.Drawing.Size(136, 16)
        Me.radioCurrentDoc.TabIndex = 1
        Me.radioCurrentDoc.TabStop = True
        Me.radioCurrentDoc.Text = "Current document"
        '
        'radioAllDocs
        '
        Me.radioAllDocs.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.radioAllDocs.Location = New System.Drawing.Point(8, 16)
        Me.radioAllDocs.Name = "radioAllDocs"
        Me.radioAllDocs.Size = New System.Drawing.Size(144, 16)
        Me.radioAllDocs.TabIndex = 0
        Me.radioAllDocs.Text = "All open documents"
        '
        'buttonFindNext
        '
        Me.buttonFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonFindNext.Location = New System.Drawing.Point(392, 8)
        Me.buttonFindNext.Name = "buttonFindNext"
        Me.buttonFindNext.TabIndex = 9
        Me.buttonFindNext.Text = "&Find Next"
        '
        'buttonReplace
        '
        Me.buttonReplace.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonReplace.Location = New System.Drawing.Point(392, 40)
        Me.buttonReplace.Name = "buttonReplace"
        Me.buttonReplace.TabIndex = 10
        Me.buttonReplace.Text = "&Replace"
        '
        'buttonReplaceAll
        '
        Me.buttonReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonReplaceAll.Location = New System.Drawing.Point(392, 72)
        Me.buttonReplaceAll.Name = "buttonReplaceAll"
        Me.buttonReplaceAll.TabIndex = 11
        Me.buttonReplaceAll.Text = "Replace &All"
        '
        'buttonClose
        '
        Me.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonClose.Location = New System.Drawing.Point(392, 104)
        Me.buttonClose.Name = "buttonClose"
        Me.buttonClose.TabIndex = 12
        Me.buttonClose.Text = "Close"
        '
        'frmSearchReplace
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(473, 137)
        Me.Controls.Add(Me.buttonClose)
        Me.Controls.Add(Me.buttonReplaceAll)
        Me.Controls.Add(Me.buttonReplace)
        Me.Controls.Add(Me.buttonFindNext)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.comboUse)
        Me.Controls.Add(Me.checkUse)
        Me.Controls.Add(Me.checkSearchUp)
        Me.Controls.Add(Me.checkMatchCase)
        Me.Controls.Add(Me.comboReplace)
        Me.Controls.Add(Me.comboFind)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSearchReplace"
        Me.Text = "Find and Replace"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_frmDomainMapBrowser As frmDomainMapBrowser

    Public Sub OpenForm(ByVal frm As frmDomainMapBrowser)

        m_frmDomainMapBrowser = frm

        Dim sel As String

        If Not frm.GetCurrentOpenDoc Is Nothing Then

            'sel = frm.GetCurrentOpenDoc.TextBox.SelectedText
            sel = frm.GetCurrentOpenDoc.TextBox.Selection.Text

            If Len(sel) > 0 Then

                If InStr(sel, vbCrLf) < 1 Then

                    comboFind.Text = sel

                End If

            End If

        End If

    End Sub

    Private Sub buttonFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFindNext.Click

        FindNext()

    End Sub

    Private Sub buttonReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonReplace.Click

        Replace()

    End Sub

    Private Sub buttonReplaceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonReplaceAll.Click

        ReplaceAll()

    End Sub

    Private Sub buttonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonClose.Click

        Me.Hide()

    End Sub

    Private Sub frmSearchReplace_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        CheckHasSelection()

        CheckHasCurrent()


    End Sub

    Private Sub checkUse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkUse.CheckedChanged

        comboUse.Enabled = checkUse.Checked

    End Sub


    Private Sub comboFind_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboFind.SelectedIndexChanged

        comboFind.Text = comboFind.SelectedItem

    End Sub

    Private Sub comboReplace_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboReplace.SelectedIndexChanged

        comboReplace.Text = comboReplace.SelectedItem

    End Sub


    Private Sub FindNext()

        If Len(comboFind.Text) < 1 Then Exit Sub

        Dim compare As CompareMethod = CompareMethod.Text
        Dim doc As UserDocTabPage
        Dim docs As ArrayList
        Dim found As Boolean
        Dim passed As Boolean

        Dim currDoc As UserDocTabPage

        Dim cacheStart As Integer
        Dim cacheLength As Integer

        Dim i As Integer
        Dim iFrom As Integer
        Dim iTo As Integer
        Dim iStep As Integer

        Dim searchUp As Boolean = checkSearchUp.Checked

        If checkMatchCase.Checked Then compare = CompareMethod.Binary


        RegisterFindWords()

        If radioCurrentDoc.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                If Not m_frmDomainMapBrowser.GetCurrentOpenDoc.FindNext(comboFind.Text, searchUp, compare, True, False) Then

                    MsgBox("No occurrences found in the specified documents.")

                End If

            End If

        ElseIf radioSelectionOnly.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                If Not m_frmDomainMapBrowser.GetCurrentOpenDoc.FindNext(comboFind.Text, searchUp, compare, True, True) Then

                    MsgBox("No occurrences found in the specified documents.")

                End If

            End If

        ElseIf radioAllDocs.Checked Then

            currDoc = m_frmDomainMapBrowser.GetCurrentOpenDoc

            If Not currDoc Is Nothing Then

                If currDoc.FindNext(comboFind.Text, searchUp, compare, False, False) Then

                    found = True

                End If

            End If

            If Not found Then

                If currDoc Is Nothing Then passed = True

                docs = m_frmDomainMapBrowser.GetAllOpenDocs

                If searchUp Then

                    iFrom = docs.Count - 1
                    iTo = 0
                    iStep = -1

                Else

                    iFrom = 0
                    iTo = docs.Count - 1
                    iStep = 1

                End If

                For i = iFrom To iTo Step iStep

                    doc = docs(i)

                    If Not passed Then

                        If doc Is currDoc Then passed = True

                    Else

                        cacheStart = doc.TextBox.Selection.SelStart
                        cacheLength = doc.TextBox.Selection.SelLength


                        If searchUp Then

                            doc.TextBox.Selection.SelStart = doc.TextBox.Text.Length - comboFind.Text.Length

                        Else

                            doc.TextBox.Selection.SelStart = 0

                        End If

                        doc.TextBox.Selection.SelLength = 0

                        If doc.FindNext(comboFind.Text, searchUp, compare, False, False) Then

                            found = True

                            m_frmDomainMapBrowser.SetCurrentOpenDoc(doc)

                            doc.TextBox.Focus()

                            'doc.TextBox.ScrollToCaret()
                            doc.TextBox.ScrollIntoView()

                            Exit For

                        Else

                            If cacheStart >= 0 Then doc.TextBox.Selection.SelStart = cacheStart
                            If cacheLength >= 0 Then doc.TextBox.Selection.SelLength = cacheLength

                        End If

                    End If

                Next

            End If

            If Not found And Not currDoc Is Nothing Then

                docs = m_frmDomainMapBrowser.GetAllOpenDocs

                If searchUp Then

                    iFrom = docs.Count - 1
                    iTo = 0
                    iStep = -1

                Else

                    iFrom = 0
                    iTo = docs.Count - 1
                    iStep = 1

                End If

                For i = iFrom To iTo Step iStep

                    doc = docs(i)

                    If doc Is currDoc Then

                        Exit For

                    Else

                        cacheStart = doc.TextBox.Selection.SelStart
                        cacheLength = doc.TextBox.Selection.SelLength

                        If searchUp Then

                            doc.TextBox.Selection.SelStart = doc.TextBox.Text.Length - comboFind.Text.Length

                        Else

                            doc.TextBox.Selection.SelStart = 0

                        End If

                        doc.TextBox.Selection.SelLength = 0

                        If doc.FindNext(comboFind.Text, searchUp, compare, False, False) Then

                            found = True

                            m_frmDomainMapBrowser.SetCurrentOpenDoc(doc)

                            doc.TextBox.Focus()

                            'doc.TextBox.ScrollToCaret()
                            doc.TextBox.ScrollIntoView()

                            Exit For

                        Else

                            If cacheStart >= 0 Then doc.TextBox.Selection.SelStart = cacheStart
                            If cacheLength >= 0 Then doc.TextBox.Selection.SelLength = cacheLength

                        End If

                    End If

                Next

            End If

            If Not found Then

                MsgBox("No occurrences found in the specified documents.")

            End If


        End If



    End Sub


    Private Sub Replace()

        If Len(comboFind.Text) < 1 Then Exit Sub

        Dim compare As CompareMethod = CompareMethod.Text

        Dim doc As UserDocTabPage
        Dim docs As ArrayList
        Dim found As Boolean
        Dim passed As Boolean

        Dim currDoc As UserDocTabPage

        Dim cacheStart As Integer
        Dim cacheLength As Integer

        Dim i As Integer
        Dim iFrom As Integer
        Dim iTo As Integer
        Dim iStep As Integer

        Dim searchUp As Boolean = checkSearchUp.Checked

        If checkMatchCase.Checked Then compare = CompareMethod.Binary

        Select Case compare

            Case CompareMethod.Binary

                If comboFind.Text = comboReplace.Text Then Exit Sub

            Case CompareMethod.Text

                If LCase(comboFind.Text) = LCase(comboReplace.Text) Then Exit Sub

        End Select

        RegisterFindWords()
        RegisterReplaceWords()

        If radioCurrentDoc.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                If Not m_frmDomainMapBrowser.GetCurrentOpenDoc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, True, False) Then

                    MsgBox("No occurrences found in the specified documents.")

                End If

            End If

        ElseIf radioSelectionOnly.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                If Not m_frmDomainMapBrowser.GetCurrentOpenDoc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, True, True) Then

                    MsgBox("No occurrences found in the specified documents.")

                End If

            End If

        ElseIf radioAllDocs.Checked Then

            currDoc = m_frmDomainMapBrowser.GetCurrentOpenDoc

            If Not currDoc Is Nothing Then

                If currDoc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, False, False) Then

                    found = True

                End If

            End If

            If Not found Then

                If currDoc Is Nothing Then passed = True

                docs = m_frmDomainMapBrowser.GetAllOpenDocs

                If searchUp Then

                    iFrom = docs.Count - 1
                    iTo = 0
                    iStep = -1

                Else

                    iFrom = 0
                    iTo = docs.Count - 1
                    iStep = 1

                End If

                For i = iFrom To iTo Step iStep

                    doc = docs(i)

                    If Not passed Then

                        If doc Is currDoc Then passed = True

                    Else

                        cacheStart = doc.TextBox.Selection.SelStart
                        cacheLength = doc.TextBox.Selection.SelLength


                        If searchUp Then

                            doc.TextBox.Selection.SelStart = doc.TextBox.Text.Length - comboFind.Text.Length

                        Else

                            doc.TextBox.Selection.SelStart = 0

                        End If

                        doc.TextBox.Selection.SelLength = 0

                        If doc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, False, False) Then

                            found = True

                            m_frmDomainMapBrowser.SetCurrentOpenDoc(doc)

                            doc.TextBox.Focus()

                            'doc.TextBox.ScrollToCaret()
                            doc.TextBox.ScrollIntoView()

                            Exit For

                        Else

                            If cacheStart >= 0 Then doc.TextBox.Selection.SelStart = cacheStart
                            If cacheLength >= 0 Then doc.TextBox.Selection.SelLength = cacheLength

                        End If

                    End If

                Next

            End If

            If Not found And Not currDoc Is Nothing Then

                docs = m_frmDomainMapBrowser.GetAllOpenDocs

                If searchUp Then

                    iFrom = docs.Count - 1
                    iTo = 0
                    iStep = -1

                Else

                    iFrom = 0
                    iTo = docs.Count - 1
                    iStep = 1

                End If

                For i = iFrom To iTo Step iStep

                    doc = docs(i)

                    If doc Is currDoc Then

                        Exit For

                    Else

                        cacheStart = doc.TextBox.Selection.SelStart
                        cacheLength = doc.TextBox.Selection.SelLength

                        If searchUp Then

                            doc.TextBox.Selection.SelStart = doc.TextBox.Text.Length - comboFind.Text.Length

                        Else

                            doc.TextBox.Selection.SelStart = 0

                        End If

                        doc.TextBox.Selection.SelLength = 0

                        If doc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, False, False) Then

                            found = True

                            m_frmDomainMapBrowser.SetCurrentOpenDoc(doc)

                            doc.TextBox.Focus()

                            'doc.TextBox.ScrollToCaret()
                            doc.TextBox.ScrollIntoView()

                            Exit For

                        Else

                            If cacheStart >= 0 Then doc.TextBox.Selection.SelStart = cacheStart
                            If cacheLength >= 0 Then doc.TextBox.Selection.SelLength = cacheLength

                        End If

                    End If

                Next

            End If

            If Not found Then

                MsgBox("No occurrences found in the specified documents.")

            End If


        End If

    End Sub


    Private Sub ReplaceAll()

        If Len(comboFind.Text) < 1 Then Exit Sub

        Dim compare As CompareMethod = CompareMethod.Text

        Dim doc As UserDocTabPage
        Dim docs As ArrayList
        Dim found As Boolean
        Dim passed As Boolean

        Dim currDoc As UserDocTabPage

        Dim cacheStart As Integer
        Dim cacheLength As Integer

        Dim i As Integer
        Dim iFrom As Integer
        Dim iTo As Integer
        Dim iStep As Integer

        Dim searchUp As Boolean = checkSearchUp.Checked

        Dim cntReplaced As Integer

        If checkMatchCase.Checked Then compare = CompareMethod.Binary

        Select Case compare

            Case CompareMethod.Binary

                If comboFind.Text = comboReplace.Text Then Exit Sub

            Case CompareMethod.Text

                If LCase(comboFind.Text) = LCase(comboReplace.Text) Then Exit Sub

        End Select

        RegisterFindWords()
        RegisterReplaceWords()

        If radioCurrentDoc.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                While m_frmDomainMapBrowser.GetCurrentOpenDoc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, True, False)

                    cntReplaced += 1

                End While

            End If

        ElseIf radioSelectionOnly.Checked Then

            If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

                cntReplaced = m_frmDomainMapBrowser.GetCurrentOpenDoc.ReplaceAllInSelection(comboFind.Text, comboReplace.Text, searchUp, compare, True, True)

            End If

        ElseIf radioAllDocs.Checked Then

            docs = m_frmDomainMapBrowser.GetAllOpenDocs

            If searchUp Then

                iFrom = docs.Count - 1
                iTo = 0
                iStep = -1

            Else

                iFrom = 0
                iTo = docs.Count - 1
                iStep = 1

            End If

            For i = iFrom To iTo Step iStep

                doc = docs(i)

                cacheStart = doc.TextBox.Selection.SelStart
                cacheLength = doc.TextBox.Selection.SelLength

                If searchUp Then

                    doc.TextBox.Selection.SelStart = doc.TextBox.Text.Length - comboFind.Text.Length

                Else

                    doc.TextBox.Selection.SelStart = 0

                End If

                doc.TextBox.Selection.SelLength = 0

                While doc.ReplaceNext(comboFind.Text, comboReplace.Text, searchUp, compare, False, False)

                    cntReplaced += 1

                End While

                If cacheStart >= 0 Then doc.TextBox.Selection.SelStart = cacheStart
                If cacheLength >= 0 Then doc.TextBox.Selection.SelLength = cacheLength

            Next

        End If

        If cntReplaced > 0 Then

            MsgBox(cntReplaced & " occurrence(s) replaced.")

        Else

            MsgBox("The specified text was not found.")

        End If
    End Sub

    Private Sub RegisterFindWords()

        Dim words As String = comboFind.Text

        If Len(words) > 0 Then

            comboFind.Items.Remove(words)

            comboFind.Items.Insert(0, words)

        End If

    End Sub

    Private Sub RegisterReplaceWords()

        Dim words As String = comboReplace.Text

        If Len(words) > 0 Then

            comboReplace.Items.Remove(words)

            comboReplace.Items.Insert(0, words)

        End If

    End Sub

    Private Sub frmSearchReplace_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Me.Hide()

        e.Cancel = True

    End Sub

    Private Sub CheckHasSelection()

        radioSelectionOnly.Enabled = False

        Dim sel As String

        If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

            sel = m_frmDomainMapBrowser.GetCurrentOpenDoc.TextBox.Selection.Text

            If Len(sel) > 0 Then

                radioSelectionOnly.Enabled = True

            End If

        End If


    End Sub

    Private Sub CheckHasCurrent()

        If Not m_frmDomainMapBrowser.GetCurrentOpenDoc Is Nothing Then

            radioCurrentDoc.Enabled = True

        Else

            radioAllDocs.Checked = True
            radioCurrentDoc.Enabled = False
            radioSelectionOnly.Enabled = False

        End If

        If Not radioSelectionOnly.Enabled Then

            If radioSelectionOnly.Checked Then

                If radioCurrentDoc.Enabled Then

                    radioCurrentDoc.Checked = True

                Else

                    radioAllDocs.Checked = True

                End If

            End If

        End If

    End Sub

End Class
