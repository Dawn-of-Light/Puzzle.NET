Public Class frmImportDtbStatus
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
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents labelTitle As System.Windows.Forms.Label
    Friend WithEvents labelTick As System.Windows.Forms.Label
    Friend WithEvents labelMsg As System.Windows.Forms.Label
    Friend WithEvents listLog As System.Windows.Forms.ListView
    Friend WithEvents buttonDetails As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmImportDtbStatus))
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.labelTick = New System.Windows.Forms.Label
        Me.labelTitle = New System.Windows.Forms.Label
        Me.labelMsg = New System.Windows.Forms.Label
        Me.listLog = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.buttonDetails = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(440, 40)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.TabIndex = 0
        Me.buttonCancel.Text = "Cancel"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 72)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(424, 23)
        Me.ProgressBar1.TabIndex = 1
        '
        'labelTick
        '
        Me.labelTick.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.labelTick.Location = New System.Drawing.Point(8, 32)
        Me.labelTick.Name = "labelTick"
        Me.labelTick.Size = New System.Drawing.Size(424, 23)
        Me.labelTick.TabIndex = 2
        '
        'labelTitle
        '
        Me.labelTitle.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.labelTitle.Location = New System.Drawing.Point(8, 8)
        Me.labelTitle.Name = "labelTitle"
        Me.labelTitle.Size = New System.Drawing.Size(520, 23)
        Me.labelTitle.TabIndex = 3
        '
        'labelMsg
        '
        Me.labelMsg.Location = New System.Drawing.Point(8, 56)
        Me.labelMsg.Name = "labelMsg"
        Me.labelMsg.Size = New System.Drawing.Size(424, 23)
        Me.labelMsg.TabIndex = 4
        '
        'listLog
        '
        Me.listLog.Location = New System.Drawing.Point(8, 112)
        Me.listLog.Name = "listLog"
        Me.listLog.Size = New System.Drawing.Size(512, 328)
        Me.listLog.SmallImageList = Me.ImageList1
        Me.listLog.TabIndex = 5
        Me.listLog.View = System.Windows.Forms.View.Details
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'buttonDetails
        '
        Me.buttonDetails.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonDetails.Location = New System.Drawing.Point(440, 72)
        Me.buttonDetails.Name = "buttonDetails"
        Me.buttonDetails.TabIndex = 6
        Me.buttonDetails.Text = "Details >>"
        '
        'frmImportDtbStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(529, 102)
        Me.ControlBox = False
        Me.Controls.Add(Me.buttonDetails)
        Me.Controls.Add(Me.listLog)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.labelTick)
        Me.Controls.Add(Me.labelTitle)
        Me.Controls.Add(Me.labelMsg)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmImportDtbStatus"
        Me.Text = "Importing From Pragmatier Data Tier Builder Format"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_Canceled As Boolean

    Public Sub CheckCanceled(ByRef canceled As Boolean)

        canceled = m_Canceled

    End Sub

    Public Sub Setup(ByVal maxTicks As Integer, ByVal path As String)

        labelTitle.Text = "Importing from '" & path & "'..."

        m_Canceled = False

        ProgressBar1.Maximum = maxTicks + 2

        ProgressBar1.Step = 1

        listLog.Clear()

        listLog.Columns.Add("Description", listLog.Width - 104, HorizontalAlignment.Left)

        listLog.Columns.Add("Time", 80, HorizontalAlignment.Left)

    End Sub

    Public Sub SetMessage(ByVal msg As String, ByRef canceled As Boolean)

        Dim listItem As ListViewItem

        labelMsg.Text = msg
        canceled = m_Canceled

        listItem = New ListViewItem(msg, 1)

        listItem.SubItems.Add(Now.ToLongTimeString)

        listLog.Items.Add(listItem)

        Application.DoEvents()

    End Sub


    Public Sub AddTick(ByVal msg As String, ByRef canceled As Boolean)

        Dim listItem As ListViewItem

        ProgressBar1.PerformStep()

        labelTick.Text = msg
        canceled = m_Canceled

        listItem = New ListViewItem(msg, 0)

        listItem.SubItems.Add(Now.ToLongTimeString)

        listLog.Items.Add(listItem)

        Application.DoEvents()

    End Sub

    Public Sub Finish()

        labelMsg.Text = "Import complete!"

        Application.DoEvents()

        Me.Hide()

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        labelTitle.Text = "Canceled!"
        labelMsg.Text = "Finishing..."
        m_Canceled = True

    End Sub

    Private Sub buttonDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonDetails.Click

        If Me.Size.Height = 480 Then

            Me.Size = New Size(Me.Size.Width, 128)
            buttonDetails.Text = "Details >>"

        Else

            Me.Size = New Size(Me.Size.Width, 480)
            buttonDetails.Text = "Details <<"

        End If
    End Sub
End Class
