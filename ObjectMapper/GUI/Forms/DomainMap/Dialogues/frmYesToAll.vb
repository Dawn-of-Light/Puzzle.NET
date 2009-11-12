Public Class frmYesToAll
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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SyntaxDocument1 As Puzzle.SourceCode.SyntaxDocument
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmYesToAll))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SyntaxDocument1 = New Puzzle.SourceCode.SyntaxDocument(Me.components)
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(7, 42)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(62, 20)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(73, 42)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(63, 20)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Button2"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(140, 42)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(62, 20)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Button3"
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(7, 69)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(86, 21)
        Me.CheckBox1.TabIndex = 3
        Me.CheckBox1.Text = "Apply to all"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Label1"
        '
        'SyntaxDocument1
        '
        Me.SyntaxDocument1.Lines = New String() {""}
        Me.SyntaxDocument1.MaxUndoBufferSize = 1000
        Me.SyntaxDocument1.Modified = False
        Me.SyntaxDocument1.UndoStep = 0
        '
        'frmYesToAll
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(214, 91)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmYesToAll"
        Me.Text = "frmYesToAll"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub test()
        'MessageBox.Show("", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1 


    End Sub
End Class
