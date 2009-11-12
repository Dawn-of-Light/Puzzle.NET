Imports System.IO

Public Class frmImportDTB
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
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonImport As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents checkIncludeCmpInTbl As System.Windows.Forms.CheckBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents textMdbPath As System.Windows.Forms.TextBox
    Friend WithEvents buttonBrowseMdb As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmImportDTB))
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.buttonBrowseMdb = New System.Windows.Forms.Button
        Me.textMdbPath = New System.Windows.Forms.TextBox
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.buttonImport = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.checkIncludeCmpInTbl = New System.Windows.Forms.CheckBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Location = New System.Drawing.Point(8, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 16)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Known Limitations:"
        '
        'Label4
        '
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(408, 48)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "When importing from the Pragmatier Data Tier Builder format, you must specify the" & _
        " path to the Microsoft Access database file containing the class definitions (it" & _
        " is the file that has the same name as your component but with a ""cmp"" prefix)."
        '
        'GroupBox2
        '
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox2.Location = New System.Drawing.Point(8, 144)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(445, 8)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(8, 248)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(445, 8)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        '
        'buttonBrowseMdb
        '
        Me.buttonBrowseMdb.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonBrowseMdb.Location = New System.Drawing.Point(420, 184)
        Me.buttonBrowseMdb.Name = "buttonBrowseMdb"
        Me.buttonBrowseMdb.Size = New System.Drawing.Size(32, 20)
        Me.buttonBrowseMdb.TabIndex = 17
        Me.buttonBrowseMdb.Text = "..."
        '
        'textMdbPath
        '
        Me.textMdbPath.Location = New System.Drawing.Point(16, 184)
        Me.textMdbPath.Name = "textMdbPath"
        Me.textMdbPath.Size = New System.Drawing.Size(404, 20)
        Me.textMdbPath.TabIndex = 16
        Me.textMdbPath.Text = ""
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(373, 272)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.TabIndex = 15
        Me.buttonCancel.Text = "Cancel"
        '
        'buttonImport
        '
        Me.buttonImport.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonImport.Location = New System.Drawing.Point(293, 272)
        Me.buttonImport.Name = "buttonImport"
        Me.buttonImport.TabIndex = 14
        Me.buttonImport.Text = "Import"
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(16, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(368, 23)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Path to MS Access file (.mdb) containing Pragmatier DTB class definitions::"
        '
        'checkIncludeCmpInTbl
        '
        Me.checkIncludeCmpInTbl.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkIncludeCmpInTbl.Location = New System.Drawing.Point(16, 216)
        Me.checkIncludeCmpInTbl.Name = "checkIncludeCmpInTbl"
        Me.checkIncludeCmpInTbl.Size = New System.Drawing.Size(304, 24)
        Me.checkIncludeCmpInTbl.TabIndex = 28
        Me.checkIncludeCmpInTbl.Text = "Include component name in autogenerated table names"
        '
        'ListBox1
        '
        Me.ListBox1.BackColor = System.Drawing.SystemColors.Control
        Me.ListBox1.Items.AddRange(New Object() {"Pragmatier DTB Generic References Using The 'Object' Data Type Are Not Supported", "References Using The DTB-Specific 'ByQualifier' Refernce Type Are Not Supported", "MatsSoft DTB Static Properties Are Not Supported"})
        Me.ListBox1.Location = New System.Drawing.Point(8, 80)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(445, 30)
        Me.ListBox1.TabIndex = 29
        '
        'frmImportDTB
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(459, 305)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.checkIncludeCmpInTbl)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.buttonBrowseMdb)
        Me.Controls.Add(Me.textMdbPath)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonImport)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmImportDTB"
        Me.Text = " Import From Pragmatier Data Tier Builder Format"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private m_Cancel As Boolean

    Public Function IsCanceled() As Boolean
        Return m_Cancel
    End Function

    Public Function GetMdbPath() As String
        Return textMdbPath.Text
    End Function


    Public Function GetUseCmpInTbl() As Boolean
        Return checkIncludeCmpInTbl.Checked
    End Function

    Private Sub buttonBrowseMdb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonBrowseMdb.Click

        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "MS Access Files (*.mdb)|*.mdb|All files (*.*)|*.*"
        OpenFileDialog1.Multiselect = False

        OpenFileDialog1.ShowDialog(Me)

        textMdbPath.Text = OpenFileDialog1.FileName

    End Sub

    Private Sub buttonImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonImport.Click

        If CheckPaths() Then

            m_Cancel = False

            Me.Close()

        End If

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        m_Cancel = True

        Me.Close()

    End Sub


    Private Function CheckPaths() As Boolean

        If Len(textMdbPath.Text) < 1 Then

            MsgBox("You must enter a path to the xml file first!")
            textMdbPath.Focus()
            Exit Function

        End If

        If Not File.Exists(textMdbPath.Text) Then

            MsgBox("The specified xml file does not exist!")
            textMdbPath.Focus()
            Exit Function

        End If

        Return True

    End Function

End Class
