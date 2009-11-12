Imports Puzzle.NPersist.Framework
Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations
Imports System.Data
Imports System.IO
'Imports MSDASC
'Imports ADODB

Public Class frmWrapDbWizard
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
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonBack As System.Windows.Forms.Button
    Friend WithEvents buttonNext As System.Windows.Forms.Button
    Friend WithEvents buttonFinish As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents panelStep0 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents panelStep2 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents buttonTestConnection As System.Windows.Forms.Button
    Friend WithEvents buttonOpenConnStrEditor As System.Windows.Forms.Button
    Friend WithEvents textConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents comboProviderType As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents comboSourceType As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents panelStep1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents listViewDomainModels As System.Windows.Forms.ListView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents panelStep3 As System.Windows.Forms.Panel
    Friend WithEvents Panel8 As System.Windows.Forms.Panel
    Friend WithEvents textTargetDirectory As System.Windows.Forms.TextBox
    Friend WithEvents comboTargetLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents panelStep4 As System.Windows.Forms.Panel
    Friend WithEvents Panel10 As System.Windows.Forms.Panel
    Friend WithEvents textSummary As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Panel11 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents imageListSmall As System.Windows.Forms.ImageList
    Friend WithEvents buttonBrowseTargetDirectory As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents checkCopyUpdated As System.Windows.Forms.CheckBox
    Friend WithEvents checkCopyUnmodified As System.Windows.Forms.CheckBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents buttonOpenDomain As System.Windows.Forms.Button
    Friend WithEvents buttonNewDomain As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWrapDbWizard))
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.buttonBack = New System.Windows.Forms.Button
        Me.buttonNext = New System.Windows.Forms.Button
        Me.buttonFinish = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.panelStep0 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.panelStep2 = New System.Windows.Forms.Panel
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.buttonTestConnection = New System.Windows.Forms.Button
        Me.buttonOpenConnStrEditor = New System.Windows.Forms.Button
        Me.textConnectionString = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.comboProviderType = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.comboSourceType = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.panelStep1 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.buttonOpenDomain = New System.Windows.Forms.Button
        Me.buttonNewDomain = New System.Windows.Forms.Button
        Me.listViewDomainModels = New System.Windows.Forms.ListView
        Me.imageListSmall = New System.Windows.Forms.ImageList(Me.components)
        Me.Label11 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.panelStep3 = New System.Windows.Forms.Panel
        Me.Panel8 = New System.Windows.Forms.Panel
        Me.Label26 = New System.Windows.Forms.Label
        Me.checkCopyUpdated = New System.Windows.Forms.CheckBox
        Me.checkCopyUnmodified = New System.Windows.Forms.CheckBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.buttonBrowseTargetDirectory = New System.Windows.Forms.Button
        Me.textTargetDirectory = New System.Windows.Forms.TextBox
        Me.comboTargetLanguage = New System.Windows.Forms.ComboBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Panel9 = New System.Windows.Forms.Panel
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.panelStep4 = New System.Windows.Forms.Panel
        Me.Panel10 = New System.Windows.Forms.Panel
        Me.textSummary = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Panel11 = New System.Windows.Forms.Panel
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.panelStep0.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.panelStep2.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.panelStep1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.panelStep3.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.panelStep4.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(160, 384)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(75, 25)
        Me.buttonCancel.TabIndex = 9
        Me.buttonCancel.Text = "Cancel"
        '
        'buttonBack
        '
        Me.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonBack.Location = New System.Drawing.Point(242, 384)
        Me.buttonBack.Name = "buttonBack"
        Me.buttonBack.Size = New System.Drawing.Size(75, 25)
        Me.buttonBack.TabIndex = 8
        Me.buttonBack.Text = "< Back"
        '
        'buttonNext
        '
        Me.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonNext.Location = New System.Drawing.Point(324, 384)
        Me.buttonNext.Name = "buttonNext"
        Me.buttonNext.Size = New System.Drawing.Size(75, 25)
        Me.buttonNext.TabIndex = 7
        Me.buttonNext.Text = "Next >"
        '
        'buttonFinish
        '
        Me.buttonFinish.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonFinish.Location = New System.Drawing.Point(406, 384)
        Me.buttonFinish.Name = "buttonFinish"
        Me.buttonFinish.Size = New System.Drawing.Size(75, 25)
        Me.buttonFinish.TabIndex = 6
        Me.buttonFinish.Text = "Finish"
        '
        'GroupBox1
        '
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(0, 368)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(496, 8)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'panelStep0
        '
        Me.panelStep0.Controls.Add(Me.Panel2)
        Me.panelStep0.Controls.Add(Me.Panel1)
        Me.panelStep0.Location = New System.Drawing.Point(0, 0)
        Me.panelStep0.Name = "panelStep0"
        Me.panelStep0.Size = New System.Drawing.Size(504, 368)
        Me.panelStep0.TabIndex = 10
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(168, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(336, 368)
        Me.Panel2.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Location = New System.Drawing.Point(16, 304)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(272, 32)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Click Next to wrap your database, or Cancel to exit the wizard."
        '
        'Label4
        '
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Location = New System.Drawing.Point(16, 224)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(272, 67)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Your domain model will now be updated to include the tables, classes and o/r mapp" & _
        "ing information. The wizard will then proceed to generate the source code for yo" & _
        "ur classes in the programming language of your choice."
        '
        'Label3
        '
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(16, 136)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(272, 80)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Wrapping your database means that the wizard will connect to your databse, analyz" & _
        "e it and  generate a table model from it. It will then generate a class model fr" & _
        "om the table model and the object/relational mapping information relating your g" & _
        "enerated classes to tables. "
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(16, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(264, 45)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "This wizard will lead you through the steps for wrapping your existing database w" & _
        "ith classes and object/relational mapping information."
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(304, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Welcome to the Wrap Database Wizard"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(192, Byte))
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(168, 368)
        Me.Panel1.TabIndex = 0
        '
        'panelStep2
        '
        Me.panelStep2.Controls.Add(Me.Panel7)
        Me.panelStep2.Controls.Add(Me.Panel6)
        Me.panelStep2.Location = New System.Drawing.Point(0, 0)
        Me.panelStep2.Name = "panelStep2"
        Me.panelStep2.Size = New System.Drawing.Size(504, 368)
        Me.panelStep2.TabIndex = 11
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.White
        Me.Panel7.Controls.Add(Me.PictureBox1)
        Me.Panel7.Controls.Add(Me.Label7)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel7.Location = New System.Drawing.Point(0, 0)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(504, 64)
        Me.Panel7.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(440, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(56, 50)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Label7
        '
        Me.Label7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label7.Location = New System.Drawing.Point(32, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(352, 32)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "The database you choose will be connected to, analyzed and wrapped with classes a" & _
        "nd object/relational mapping information."
        '
        'Label6
        '
        Me.Label6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(152, 23)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Choose database"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.buttonTestConnection)
        Me.Panel6.Controls.Add(Me.buttonOpenConnStrEditor)
        Me.Panel6.Controls.Add(Me.textConnectionString)
        Me.Panel6.Controls.Add(Me.Label16)
        Me.Panel6.Controls.Add(Me.comboProviderType)
        Me.Panel6.Controls.Add(Me.Label15)
        Me.Panel6.Controls.Add(Me.comboSourceType)
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(504, 368)
        Me.Panel6.TabIndex = 1
        '
        'buttonTestConnection
        '
        Me.buttonTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonTestConnection.Location = New System.Drawing.Point(272, 224)
        Me.buttonTestConnection.Name = "buttonTestConnection"
        Me.buttonTestConnection.Size = New System.Drawing.Size(120, 23)
        Me.buttonTestConnection.TabIndex = 9
        Me.buttonTestConnection.Text = "Test Connection"
        '
        'buttonOpenConnStrEditor
        '
        Me.buttonOpenConnStrEditor.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOpenConnStrEditor.Location = New System.Drawing.Point(368, 176)
        Me.buttonOpenConnStrEditor.Name = "buttonOpenConnStrEditor"
        Me.buttonOpenConnStrEditor.Size = New System.Drawing.Size(24, 20)
        Me.buttonOpenConnStrEditor.TabIndex = 8
        Me.buttonOpenConnStrEditor.Text = "..."
        Me.buttonOpenConnStrEditor.Visible = False
        '
        'textConnectionString
        '
        Me.textConnectionString.Location = New System.Drawing.Point(16, 176)
        Me.textConnectionString.Name = "textConnectionString"
        Me.textConnectionString.Size = New System.Drawing.Size(352, 20)
        Me.textConnectionString.TabIndex = 7
        Me.textConnectionString.Text = ""
        '
        'Label16
        '
        Me.Label16.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(16, 152)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(368, 23)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "Please enter the connection string to the database:"
        '
        'comboProviderType
        '
        Me.comboProviderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboProviderType.Location = New System.Drawing.Point(16, 112)
        Me.comboProviderType.Name = "comboProviderType"
        Me.comboProviderType.Size = New System.Drawing.Size(376, 21)
        Me.comboProviderType.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(16, 88)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(368, 23)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Which database provider do you want to use?"
        '
        'comboSourceType
        '
        Me.comboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboSourceType.Location = New System.Drawing.Point(16, 48)
        Me.comboSourceType.Name = "comboSourceType"
        Me.comboSourceType.Size = New System.Drawing.Size(376, 21)
        Me.comboSourceType.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(368, 23)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Which type of database do you want to use?"
        '
        'panelStep1
        '
        Me.panelStep1.Controls.Add(Me.Panel4)
        Me.panelStep1.Controls.Add(Me.Panel3)
        Me.panelStep1.Location = New System.Drawing.Point(0, 0)
        Me.panelStep1.Name = "panelStep1"
        Me.panelStep1.Size = New System.Drawing.Size(504, 368)
        Me.panelStep1.TabIndex = 12
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.buttonOpenDomain)
        Me.Panel4.Controls.Add(Me.buttonNewDomain)
        Me.Panel4.Controls.Add(Me.listViewDomainModels)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 64)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(504, 304)
        Me.Panel4.TabIndex = 1
        '
        'buttonOpenDomain
        '
        Me.buttonOpenDomain.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOpenDomain.Location = New System.Drawing.Point(408, 80)
        Me.buttonOpenDomain.Name = "buttonOpenDomain"
        Me.buttonOpenDomain.TabIndex = 10
        Me.buttonOpenDomain.Text = "Open"
        '
        'buttonNewDomain
        '
        Me.buttonNewDomain.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonNewDomain.Location = New System.Drawing.Point(408, 48)
        Me.buttonNewDomain.Name = "buttonNewDomain"
        Me.buttonNewDomain.TabIndex = 9
        Me.buttonNewDomain.Text = "New"
        '
        'listViewDomainModels
        '
        Me.listViewDomainModels.HideSelection = False
        Me.listViewDomainModels.Location = New System.Drawing.Point(16, 48)
        Me.listViewDomainModels.MultiSelect = False
        Me.listViewDomainModels.Name = "listViewDomainModels"
        Me.listViewDomainModels.Size = New System.Drawing.Size(376, 224)
        Me.listViewDomainModels.SmallImageList = Me.imageListSmall
        Me.listViewDomainModels.TabIndex = 6
        Me.listViewDomainModels.View = System.Windows.Forms.View.Details
        '
        'imageListSmall
        '
        Me.imageListSmall.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageListSmall.ImageStream = CType(resources.GetObject("imageListSmall.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageListSmall.TransparentColor = System.Drawing.Color.Transparent
        '
        'Label11
        '
        Me.Label11.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(20, 14)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(384, 35)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Which domain model do you want to fill with o/r mapping information?"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.PictureBox2)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(504, 64)
        Me.Panel3.TabIndex = 0
        '
        'PictureBox2
        '
        Me.PictureBox2.Location = New System.Drawing.Point(436, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(56, 50)
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'Label9
        '
        Me.Label9.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label9.Location = New System.Drawing.Point(32, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(352, 32)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "The wizard will fill the selected domain model with the generated object/relation" & _
        "al mapping information."
        '
        'Label10
        '
        Me.Label10.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(8, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(152, 23)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Choose domain model"
        '
        'panelStep3
        '
        Me.panelStep3.Controls.Add(Me.Panel8)
        Me.panelStep3.Controls.Add(Me.Panel9)
        Me.panelStep3.Location = New System.Drawing.Point(0, 0)
        Me.panelStep3.Name = "panelStep3"
        Me.panelStep3.Size = New System.Drawing.Size(504, 368)
        Me.panelStep3.TabIndex = 13
        '
        'Panel8
        '
        Me.Panel8.Controls.Add(Me.Label26)
        Me.Panel8.Controls.Add(Me.checkCopyUpdated)
        Me.Panel8.Controls.Add(Me.checkCopyUnmodified)
        Me.Panel8.Controls.Add(Me.Label24)
        Me.Panel8.Controls.Add(Me.Label25)
        Me.Panel8.Controls.Add(Me.buttonBrowseTargetDirectory)
        Me.Panel8.Controls.Add(Me.textTargetDirectory)
        Me.Panel8.Controls.Add(Me.comboTargetLanguage)
        Me.Panel8.Controls.Add(Me.Label20)
        Me.Panel8.Controls.Add(Me.Label12)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel8.Location = New System.Drawing.Point(0, 64)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(504, 304)
        Me.Panel8.TabIndex = 1
        '
        'Label26
        '
        Me.Label26.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label26.Location = New System.Drawing.Point(408, 112)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(80, 104)
        Me.Label26.TabIndex = 15
        Me.Label26.Text = "Note: Existing files with conflicting names in the target directory will be overw" & _
        "ritten!!"
        '
        'checkCopyUpdated
        '
        Me.checkCopyUpdated.Checked = True
        Me.checkCopyUpdated.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkCopyUpdated.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkCopyUpdated.Location = New System.Drawing.Point(56, 248)
        Me.checkCopyUpdated.Name = "checkCopyUpdated"
        Me.checkCopyUpdated.Size = New System.Drawing.Size(328, 48)
        Me.checkCopyUpdated.TabIndex = 13
        Me.checkCopyUpdated.Text = "Save a copy of the updated domain model (the way it looks after the wizard has co" & _
        "mpleted, including the new tables and o/r mapping information) to the target dir" & _
        "ectory"
        '
        'checkCopyUnmodified
        '
        Me.checkCopyUnmodified.Checked = True
        Me.checkCopyUnmodified.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkCopyUnmodified.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkCopyUnmodified.Location = New System.Drawing.Point(56, 176)
        Me.checkCopyUnmodified.Name = "checkCopyUnmodified"
        Me.checkCopyUnmodified.Size = New System.Drawing.Size(328, 32)
        Me.checkCopyUnmodified.TabIndex = 12
        Me.checkCopyUnmodified.Text = "Save a copy of the unmodified domain model (the way it looks now) to the target d" & _
        "ircetory"
        '
        'Label24
        '
        Me.Label24.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(16, 152)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(384, 23)
        Me.Label24.TabIndex = 11
        Me.Label24.Text = "Do you want to save a copy of the unmodified domain model?"
        '
        'Label25
        '
        Me.Label25.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(16, 224)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(368, 23)
        Me.Label25.TabIndex = 14
        Me.Label25.Text = "Do you want to save a copy of the updated domain model?"
        '
        'buttonBrowseTargetDirectory
        '
        Me.buttonBrowseTargetDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonBrowseTargetDirectory.Location = New System.Drawing.Point(368, 112)
        Me.buttonBrowseTargetDirectory.Name = "buttonBrowseTargetDirectory"
        Me.buttonBrowseTargetDirectory.Size = New System.Drawing.Size(24, 20)
        Me.buttonBrowseTargetDirectory.TabIndex = 6
        Me.buttonBrowseTargetDirectory.Text = "..."
        '
        'textTargetDirectory
        '
        Me.textTargetDirectory.Location = New System.Drawing.Point(16, 112)
        Me.textTargetDirectory.Name = "textTargetDirectory"
        Me.textTargetDirectory.Size = New System.Drawing.Size(352, 20)
        Me.textTargetDirectory.TabIndex = 5
        Me.textTargetDirectory.Text = ""
        '
        'comboTargetLanguage
        '
        Me.comboTargetLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboTargetLanguage.Items.AddRange(New Object() {"Microsoft C#", "Microsoft Visual Basic.NET"})
        Me.comboTargetLanguage.Location = New System.Drawing.Point(16, 48)
        Me.comboTargetLanguage.Name = "comboTargetLanguage"
        Me.comboTargetLanguage.Size = New System.Drawing.Size(376, 21)
        Me.comboTargetLanguage.TabIndex = 4
        '
        'Label20
        '
        Me.Label20.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(16, 24)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(437, 23)
        Me.Label20.TabIndex = 3
        Me.Label20.Text = "Which programming language do you want for your source code files?"
        '
        'Label12
        '
        Me.Label12.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 88)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(424, 23)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Which directory do you want the souce code files to be placed in?"
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.White
        Me.Panel9.Controls.Add(Me.PictureBox3)
        Me.Panel9.Controls.Add(Me.Label13)
        Me.Panel9.Controls.Add(Me.Label14)
        Me.Panel9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel9.Location = New System.Drawing.Point(0, 0)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(504, 64)
        Me.Panel9.TabIndex = 0
        '
        'PictureBox3
        '
        Me.PictureBox3.Location = New System.Drawing.Point(440, 8)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(56, 50)
        Me.PictureBox3.TabIndex = 2
        Me.PictureBox3.TabStop = False
        '
        'Label13
        '
        Me.Label13.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label13.Location = New System.Drawing.Point(32, 24)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(352, 32)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "The target directory you choose will be filled with the source code files that th" & _
        "e wizard will generate."
        '
        'Label14
        '
        Me.Label14.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(8, 8)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(256, 23)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Choose target language and directory"
        '
        'panelStep4
        '
        Me.panelStep4.Controls.Add(Me.Panel10)
        Me.panelStep4.Controls.Add(Me.Panel11)
        Me.panelStep4.Location = New System.Drawing.Point(0, 0)
        Me.panelStep4.Name = "panelStep4"
        Me.panelStep4.Size = New System.Drawing.Size(504, 368)
        Me.panelStep4.TabIndex = 14
        '
        'Panel10
        '
        Me.Panel10.Controls.Add(Me.textSummary)
        Me.Panel10.Controls.Add(Me.Label17)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel10.Location = New System.Drawing.Point(0, 64)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(504, 304)
        Me.Panel10.TabIndex = 1
        '
        'textSummary
        '
        Me.textSummary.Location = New System.Drawing.Point(16, 40)
        Me.textSummary.Multiline = True
        Me.textSummary.Name = "textSummary"
        Me.textSummary.ReadOnly = True
        Me.textSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textSummary.Size = New System.Drawing.Size(464, 240)
        Me.textSummary.TabIndex = 1
        Me.textSummary.Text = ""
        Me.textSummary.WordWrap = False
        '
        'Label17
        '
        Me.Label17.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label17.Location = New System.Drawing.Point(16, 16)
        Me.Label17.Name = "Label17"
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Summary:"
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.White
        Me.Panel11.Controls.Add(Me.PictureBox4)
        Me.Panel11.Controls.Add(Me.Label18)
        Me.Panel11.Controls.Add(Me.Label19)
        Me.Panel11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel11.Location = New System.Drawing.Point(0, 0)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(504, 64)
        Me.Panel11.TabIndex = 0
        '
        'PictureBox4
        '
        Me.PictureBox4.Location = New System.Drawing.Point(440, 8)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(56, 50)
        Me.PictureBox4.TabIndex = 2
        Me.PictureBox4.TabStop = False
        '
        'Label18
        '
        Me.Label18.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label18.Location = New System.Drawing.Point(32, 24)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(360, 32)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "The wizard will now wrap the database based on your choices."
        '
        'Label19
        '
        Me.Label19.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(8, 8)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(179, 23)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Wrap Database"
        '
        'frmWrapDbWizard
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(497, 417)
        Me.ControlBox = False
        Me.Controls.Add(Me.panelStep3)
        Me.Controls.Add(Me.panelStep2)
        Me.Controls.Add(Me.panelStep1)
        Me.Controls.Add(Me.panelStep0)
        Me.Controls.Add(Me.panelStep4)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonBack)
        Me.Controls.Add(Me.buttonNext)
        Me.Controls.Add(Me.buttonFinish)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmWrapDbWizard"
        Me.Text = "Wrap Database Wizard"
        Me.panelStep0.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.panelStep2.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.panelStep1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.panelStep3.ResumeLayout(False)
        Me.Panel8.ResumeLayout(False)
        Me.Panel9.ResumeLayout(False)
        Me.panelStep4.ResumeLayout(False)
        Me.Panel10.ResumeLayout(False)
        Me.Panel11.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region " Wizard Generic Code "

#Region " Private variables "

    Private m_callerForm As frmDomainMapBrowser

    Private m_project As ProjectModel.IProject

    Private m_CurrStep As Long = 0
    Private m_LastStep As Long = 4

    Private m_WinTitle As String

#End Region

#Region " Form setup "

    Public Sub Setup(ByVal callerForm As frmDomainMapBrowser, ByVal project As ProjectModel.IProject)

        m_callerForm = callerForm

        m_project = project

        m_WinTitle = Me.Text

        Start()

    End Sub

#End Region

#Region " Main Button event handlers "

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        m_callerForm.ReturnWizardStatus(frmDomainMapBrowser.WizardResultStatusEnum.Canceled, New ArrayList)

        Me.Close()

    End Sub

    Private Sub buttonBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonBack.Click

        GoBack()

    End Sub

    Private Sub buttonNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNext.Click

        GoNext()

    End Sub

    Private Sub buttonFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFinish.Click

        Finish()

    End Sub

#End Region

#Region " Wizard navigation "

    Private Sub Start()

        m_CurrStep = 0

        SetupControls()

        ShowCurrentPanel()

    End Sub

    Private Sub GoBack()

        If m_CurrStep > 0 Then m_CurrStep -= 1

        ShowCurrentPanel()

    End Sub

    Private Sub GoNext()

        If Not VerifyStep() Then Exit Sub

        If m_CurrStep < m_LastStep Then m_CurrStep += 1

        ShowCurrentPanel()

    End Sub

    Private Sub Finish()

        Dim msgs As ArrayList

        Try

            msgs = ExecuteWizard()

            m_callerForm.SetProjectDirty()

        Catch ex As Exception

            msgs = New ArrayList

            msgs.Add(ex.Message)

            m_callerForm.ReturnWizardStatus(frmDomainMapBrowser.WizardResultStatusEnum.ErrorOccurred, msgs)

            Me.Close()

            Exit Sub

        End Try

        m_callerForm.ReturnWizardStatus(frmDomainMapBrowser.WizardResultStatusEnum.OK, msgs)

        Me.Close()


    End Sub

    Private Sub SetWinTitle()

        Me.Text = m_WinTitle & " (" & m_CurrStep + 1 & " of " & m_LastStep + 1 & ")"

    End Sub

#End Region

#Region " Panel Display "

    Private Sub ShowCurrentPanel()

        If m_CurrStep = 0 Then panelStep0.Visible = True Else panelStep0.Visible = False
        If m_CurrStep = 1 Then panelStep1.Visible = True Else panelStep1.Visible = False
        If m_CurrStep = 2 Then panelStep2.Visible = True Else panelStep2.Visible = False
        If m_CurrStep = 3 Then panelStep3.Visible = True Else panelStep3.Visible = False
        If m_CurrStep = 4 Then panelStep4.Visible = True Else panelStep4.Visible = False

        If panelStep0.Visible Then SetupPanelStep0()
        If panelStep1.Visible Then SetupPanelStep1()
        If panelStep2.Visible Then SetupPanelStep2()
        If panelStep3.Visible Then SetupPanelStep3()
        If panelStep4.Visible Then SetupPanelStep4()

        SetButtonsEnabledState()

        SetWinTitle()

        If m_CurrStep = m_LastStep Then GenerateSummary()

    End Sub

#End Region

#Region " Main Button Enabling "

    Private Sub SetButtonsEnabledState()

        buttonBack.Enabled = m_CurrStep > 0

        buttonNext.Enabled = m_CurrStep < m_LastStep

        buttonFinish.Enabled = m_CurrStep >= m_LastStep

    End Sub

#End Region

#Region " Panel Setup "

    Private Sub SetupPanelStep0()

    End Sub

    Private Sub SetupPanelStep1()

    End Sub

    Private Sub SetupPanelStep2()

    End Sub

    Private Sub SetupPanelStep3()

    End Sub

    Private Sub SetupPanelStep4()

    End Sub

#End Region

#Region " Step Verification "

    Private Function VerifyStep() As Boolean

        Select Case m_CurrStep
            Case 0
                Return VerifyStep0()
            Case 1
                Return VerifyStep1()
            Case 2
                Return VerifyStep2()
            Case 3
                Return VerifyStep3()
            Case 4
                Return VerifyStep4()
        End Select

    End Function

#End Region

#End Region

#Region " Wizard Specific Code "

#Region " Custom Private variables "

    Private m_SourceInfoDirty As Boolean

#End Region

#Region " Wizard Control Setup "

    Private Sub SetupControls()

        SetupDomainMapList()

        SetupSourceTypes()

        comboTargetLanguage.SelectedIndex = 0

        SetupTargetDirectory()

        m_SourceInfoDirty = False

    End Sub

    Private Sub SetupDomainMapList()

        Dim item As ListViewItem
        Dim domainMap As IDomainMap

        Dim resource As ProjectModel.IResource

        listViewDomainModels.Columns.Add("Domain Model", 250, HorizontalAlignment.Left)

        If Not m_project Is Nothing Then

            For Each resource In m_project.Resources

                If resource.ResourceType = ProjectModel.ResourceTypeEnum.XmlDomainMap Then

                    item = New ListViewItem(resource.Name)
                    item.ImageIndex = 0

                    listViewDomainModels.Items.Add(item)

                End If

            Next

        End If

        If listViewDomainModels.Items.Count > 0 Then
            listViewDomainModels.Items(0).Selected = True
        End If

    End Sub

    Private Sub SetupSourceTypes()

        comboSourceType.Items.Add("Microsoft SQL Server/MSDE")
        comboSourceType.Items.Add("Microsoft Access 4.0")
        comboSourceType.Items.Add("Borland InterBase")
        'comboSourceType.Items.Add("Oracle")
        'comboSourceType.Items.Add("IBM DB2")
        'comboSourceType.Items.Add("Sybase")

        comboSourceType.SelectedIndex = 0

    End Sub

    Private Sub SetupProviderTypes()

        comboProviderType.Items.Clear()

        Dim addTypes As New ArrayList
        Dim t As String

        Try

            Select Case comboSourceType.SelectedIndex

                Case 0
                    'MS SQL Server
                    addTypes.Add("System.Data.SqlClient")

                Case 1
                    'MS Access 4.0
                    addTypes.Add("System.Data.OleDb")

                Case 2
                    'Borland InterBase
                    addTypes.Add("Borland.Data.Provider")


            End Select

            For Each t In addTypes

                comboProviderType.Items.Add(t)

            Next

            comboProviderType.SelectedIndex = 0

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetupDataSourceInfo()

        If m_SourceInfoDirty Then Exit Sub

        If listViewDomainModels.SelectedItems.Count < 1 Then Exit Sub

        Dim item As ListViewItem

        Dim domainMapName As String

        Dim domainMap As IDomainMap

        Dim sourceMap As ISourceMap

        Dim resource As ProjectModel.IResource

        For Each item In listViewDomainModels.SelectedItems

            domainMapName = item.Text

            Exit For

        Next

        If Not m_project Is Nothing Then

            resource = m_project.GetResource(domainMapName, ProjectModel.ResourceTypeEnum.XmlDomainMap)

            domainMap = resource.GetResource

            For Each sourceMap In domainMap.SourceMaps

                If sourceMap.Name = domainMap.Source Then

                    textConnectionString.Text = sourceMap.ConnectionString

                    Select Case sourceMap.SourceType

                        Case SourceType.MSSqlServer

                            comboSourceType.SelectedIndex = 0

                        Case SourceType.MSAccess

                            comboSourceType.SelectedIndex = 1

                        Case SourceType.Interbase

                            comboSourceType.SelectedIndex = 2

                        Case Else

                    End Select

                    m_SourceInfoDirty = False

                    Exit Sub

                End If

            Next

        End If


    End Sub


    Private Sub SetupTargetDirectory()

        textTargetDirectory.Text = Application.LocalUserAppDataPath

    End Sub

#End Region

#Region " Wizard Control Event handlers "

    Private Sub comboSourceType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboSourceType.SelectedIndexChanged

        SetupProviderTypes()

        m_SourceInfoDirty = True

    End Sub

    Private Sub comboProviderType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboProviderType.SelectedIndexChanged

        m_SourceInfoDirty = True

    End Sub

    Private Sub textConnectionString_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textConnectionString.TextChanged

        m_SourceInfoDirty = True

    End Sub

#End Region

#Region " Wizard Control Verification "

#Region " Test Database Connection "

    Private Function TestConnection() As Boolean

        Return ConnectionServices.TestConnection(textConnectionString.Text, GetProviderType)

    End Function

#End Region

#Region " Check Target Directory "

    Private Function CheckTargetDirectory() As Boolean

        If Not Directory.Exists(textTargetDirectory.Text) Then

            Return False

        End If

        Return True

    End Function

#End Region

#End Region

#Region " Wizard Button event handlers "


    Private Sub buttonNewDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNewDomain.Click

        AddNewDomain()

    End Sub

    Private Sub buttonOpenDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOpenDomain.Click

        OpenDomain()

    End Sub

    Private Sub buttonTestConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonTestConnection.Click

        If Not TestConnection() Then

            MsgBox("Error! Database could not be contacted!")

        Else

            MsgBox("Database Connection OK!")

        End If

    End Sub

    Private Sub buttonOpenConnStrEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOpenConnStrEditor.Click

        OpenConnectionStringEditor()

    End Sub

    Private Sub buttonBrowseTargetDirectory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonBrowseTargetDirectory.Click

        BrowseForTargetDirectory()

    End Sub

#End Region

#Region " Wizard Button Execution "

    Private Sub OpenConnectionStringEditor()

        'Dim dl As New DataLinksClass
        'Dim cn As New ConnectionClass
        'Dim providerType As ProviderType = GetProviderType
        'Dim cns As String = textConnectionString.Text
        'If Len(cns) > 0 Then
        '    cn.ConnectionString = cns
        'End If
        'If (dl.PromptEdit(cn)) Then
        '    cns = cn.ConnectionString
        '    cns = FormattingServices.CleanUpConnectionString(cns, providerType)
        '    textConnectionString.Text = cns
        'End If

    End Sub

    Private Sub BrowseForTargetDirectory()

        If Len(textTargetDirectory.Text) > 0 Then

            If Directory.Exists(textTargetDirectory.Text) Then

                FolderBrowserDialog1.SelectedPath = textTargetDirectory.Text

            End If

        End If

        FolderBrowserDialog1.ShowDialog(Me)

        textTargetDirectory.Text = FolderBrowserDialog1.SelectedPath

    End Sub


    Private Sub AddNewDomain()

        Dim newDomainMap As IDomainMap
        Dim item As ListViewItem

        Try

            newDomainMap = m_callerForm.NewMap

            If Not newDomainMap Is Nothing Then

                item = New ListViewItem(newDomainMap.Name)
                item.ImageIndex = 0

                listViewDomainModels.Items.Add(item)

                item.Selected = True

            End If

            m_project = m_callerForm.GetCurrentProject

        Catch ex As Exception

        End Try

    End Sub


    Private Sub OpenDomain()

        Dim domainMap As IDomainMap
        Dim item As ListViewItem

        Try

            domainMap = m_callerForm.OpenMap()

            If Not domainMap Is Nothing Then

                item = New ListViewItem(domainMap.Name)
                item.ImageIndex = 0

                listViewDomainModels.Items.Add(item)

                item.Selected = True

            End If

            m_project = m_callerForm.GetCurrentProject

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region " Step Verification "

    Private Function VerifyStep0() As Boolean

        Return True

    End Function

    Private Function VerifyStep1() As Boolean

        If Not listViewDomainModels.SelectedItems.Count > 0 Then

            MsgBox("You must first choose the domain model containing the class model that you want to implement!")

            Return False

        End If

        'Setup the data source info
        SetupDataSourceInfo()

        Return True

    End Function

    Private Function VerifyStep2() As Boolean

        If Len(textConnectionString.Text) < 1 Then

            MsgBox("You must first enter the connection string to your database!")

            Return False

        End If

        If Not TestConnection() Then

            MsgBox("Could not contact database!")

            Return False

        End If

        Return True

    End Function

    Private Function VerifyStep3() As Boolean

        If Len(textTargetDirectory.Text) < 1 Then

            MsgBox("You must first enter the path to the target directory where you want the generated source code files to be placed!")

            Return False

        End If

        If Not CheckTargetDirectory() Then

            MsgBox("Must have valid target directory!")

            Return False

        End If

        Return True

    End Function

    Private Function VerifyStep4() As Boolean

        Return True

    End Function


#End Region

#Region " Summary "

    Private Sub GenerateSummary()

        Dim item As ListViewItem

        If listViewDomainModels.SelectedItems.Count > 0 Then

            For Each item In listViewDomainModels.SelectedItems

                textSummary.Text = "Domain Model: " & item.Text & vbCrLf

                Exit For

            Next

        Else

            textSummary.Text = "Domain Model: (none)" & vbCrLf

        End If

        textSummary.Text += vbCrLf

        textSummary.Text += "Database type: " & comboSourceType.Text & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Database provider: " & comboProviderType.Text & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Connection string: " & textConnectionString.Text & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Target language: " & comboTargetLanguage.Text & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Target directory: " & textTargetDirectory.Text & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Save copy of unmodified domain model: " & YesNo(checkCopyUnmodified.Checked) & vbCrLf

        textSummary.Text += vbCrLf

        textSummary.Text += "Save copy of updated domain model: " & YesNo(checkCopyUpdated.Checked) & vbCrLf

        textSummary.Text += vbCrLf

    End Sub

    Private Function YesNo(ByVal b As Boolean) As String

        If b Then
            Return "Yes"
        Else
            Return "No"
        End If
    End Function

#End Region

#Region " Wizard Execution "

    Private Function ExecuteWizard() As ArrayList

        Dim wizService As New WrapDbWizardService
        Dim domainMapName As String
        Dim item As ListViewItem

        Dim newConfigName As String

        Dim msgs As New ArrayList

        m_project = m_callerForm.GetCurrentProject


        Try

            For Each item In listViewDomainModels.SelectedItems

                domainMapName = item.Text

                Exit For

            Next

            newConfigName = wizService.ExecuteWizard( _
                m_project, _
                domainMapName, _
                comboSourceType.Text, _
                comboProviderType.Text, _
                textConnectionString.Text, _
                comboTargetLanguage.Text, _
                textTargetDirectory.Text, _
                checkCopyUnmodified.Checked, _
                checkCopyUpdated.Checked)

        Catch ex As Exception

            Throw New Exception(ex.Message, ex)

        End Try

        If Len(newConfigName) > 0 Then

            msgs.Add("The wizard has created a new tool configuration called '" & newConfigName & "'")
            msgs.Add("Please look in the Project Explorer tree view under:")
            msgs.Add(m_project.Name & "\" & domainMapName & "\Synch Configs\" & newConfigName)

        End If

        Return msgs

    End Function

#End Region

#Region " Misc "

    Private Function GetProviderType() As ProviderType

        Dim providerType As ProviderType

        Select Case comboProviderType.Text

            Case "System.Data.SqlClient"

                providerType = ProviderType.SqlClient

            Case "System.Data.OleDb"

                providerType = ProviderType.OleDb

            Case "System.Data.Odbc"

                providerType = ProviderType.Odbc

            Case "Borland.Data.Provider"

                providerType = ProviderType.Bdp

        End Select


        Return providerType

    End Function

#End Region

#End Region

End Class
