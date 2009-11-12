Public Class frmAssociateFiles
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
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAssociateFiles))
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.buttonOK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.CheckBoxes = True
        Me.TreeView1.ImageIndex = -1
        Me.TreeView1.Location = New System.Drawing.Point(113, 62)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Nodes.AddRange(New System.Windows.Forms.TreeNode() {New System.Windows.Forms.TreeNode(".npersist"), New System.Windows.Forms.TreeNode(".omproj")})
        Me.TreeView1.SelectedImageIndex = -1
        Me.TreeView1.ShowRootLines = False
        Me.TreeView1.Size = New System.Drawing.Size(147, 132)
        Me.TreeView1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(16, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 111)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Mark  the file extensions that you want Windows to associate with ObjectMapper 20" & _
        "04"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(16, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(240, 41)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Please select the file extensions that you would like Windows to associate with O" & _
        "bjectMapper"
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(200, 215)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(62, 20)
        Me.buttonCancel.TabIndex = 3
        Me.buttonCancel.Text = "Cancel"
        '
        'buttonOK
        '
        Me.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOK.Location = New System.Drawing.Point(133, 215)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.Size = New System.Drawing.Size(63, 20)
        Me.buttonOK.TabIndex = 4
        Me.buttonOK.Text = "OK"
        '
        'GroupBox1
        '
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(7, 201)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(253, 7)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'frmAssociateFiles
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(273, 242)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.buttonOK)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TreeView1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAssociateFiles"
        Me.Text = "Associate File Extensions"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub



    Private Function AssociateNPersist()
        Dim FA As New FileAssociation
        FA.Extension = "npersist"
        FA.ContentType = "application/npersist"
        FA.FullName = "NPersist Mapping Files"
        FA.ProperName = "NPersist File"
        FA.IconPath = (Application.StartupPath & "\Puzzle.ObjectMapper.GUI.exe")
        FA.IconIndex = 0
        FA.AddCommand("open", Application.StartupPath & "\Puzzle.ObjectMapper.GUI.exe %1")
        FA.Create()
        'Remove the file type from the registry
        'FA.Remove()
    End Function

    Private Function AssociateOMProj()
        Dim FA As New FileAssociation
        FA.Extension = "omproj"
        FA.ContentType = "application/objectmapper"
        FA.FullName = "ObjectMapper Project Files"
        FA.ProperName = "ObjectMapper Project"
        FA.IconPath = (Application.StartupPath & "\Puzzle.ObjectMapper.GUI.exe")
        FA.IconIndex = 0
        FA.AddCommand("open", Application.StartupPath & "\Puzzle.ObjectMapper.GUI.exe %1")
        FA.Create()
        'Remove the file type from the registry
        'FA.Remove()
    End Function

    Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click

        Dim node As TreeNode

        For Each node In TreeView1.Nodes

            If node.Checked Then

                Select Case node.Text

                    Case ".npersist", ".npe"

                        AssociateNPersist()

                    Case ".omproj"

                        AssociateOMProj()

                End Select

            End If

        Next

        Me.Close()

    End Sub

    Private Sub frmAssociateFiles_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim node As TreeNode

        For Each node In TreeView1.Nodes

            node.Checked = True

        Next

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        Me.Close()

    End Sub
End Class
