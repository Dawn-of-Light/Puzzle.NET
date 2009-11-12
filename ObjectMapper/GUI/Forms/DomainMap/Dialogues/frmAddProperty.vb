Public Class frmAddProperty
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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents SyntaxBoxControl1 As Puzzle.Windows.Forms.SyntaxBoxControl
    Friend WithEvents SyntaxDocument1 As Puzzle.SourceCode.SyntaxDocument
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAddProperty))
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.SyntaxBoxControl1 = New Puzzle.Windows.Forms.SyntaxBoxControl
        Me.SyntaxDocument1 = New Puzzle.SourceCode.SyntaxDocument(Me.components)
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(40, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "TextBox1"
        '
        'SyntaxBoxControl1
        '
        'Me.SyntaxBoxControl1.ActiveView = Puzzle.Windows.Forms.SyntaxBox.ActiveView.BottomRight
        Me.SyntaxBoxControl1.AutoListPosition = Nothing
        Me.SyntaxBoxControl1.AutoListSelectedText = "a123"
        Me.SyntaxBoxControl1.AutoListVisible = False
        Me.SyntaxBoxControl1.BackColor = System.Drawing.Color.White
        Me.SyntaxBoxControl1.CopyAsRTF = False
        Me.SyntaxBoxControl1.Document = Me.SyntaxDocument1
        Me.SyntaxBoxControl1.FontName = "Courier new"
        Me.SyntaxBoxControl1.InfoTipCount = 1
        Me.SyntaxBoxControl1.InfoTipPosition = Nothing
        Me.SyntaxBoxControl1.InfoTipSelectedIndex = 1
        Me.SyntaxBoxControl1.InfoTipVisible = False
        Me.SyntaxBoxControl1.Location = New System.Drawing.Point(240, 194)
        Me.SyntaxBoxControl1.LockCursorUpdate = False
        Me.SyntaxBoxControl1.Name = "SyntaxBoxControl1"
        Me.SyntaxBoxControl1.Size = New System.Drawing.Size(83, 87)
        Me.SyntaxBoxControl1.SmoothScroll = False
        Me.SyntaxBoxControl1.SplitviewH = -3
        Me.SyntaxBoxControl1.SplitviewV = -3
        Me.SyntaxBoxControl1.TabGuideColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(243, Byte), CType(234, Byte))
        Me.SyntaxBoxControl1.TabIndex = 1
        Me.SyntaxBoxControl1.Text = "SyntaxBoxControl1"
        Me.SyntaxBoxControl1.WhitespaceColor = System.Drawing.SystemColors.ControlDark
        '
        'SyntaxDocument1
        '
        Me.SyntaxDocument1.Lines = New String() {""}
        Me.SyntaxDocument1.MaxUndoBufferSize = 1000
        Me.SyntaxDocument1.Modified = False
        Me.SyntaxDocument1.UndoStep = 0
        '
        'frmAddProperty
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(543, 372)
        Me.Controls.Add(Me.SyntaxBoxControl1)
        Me.Controls.Add(Me.TextBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAddProperty"
        Me.Text = "frmAddProperty"
        Me.ResumeLayout(False)

    End Sub

#End Region

End Class
