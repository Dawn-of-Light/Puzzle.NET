Public Class frmOptions
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
    Friend WithEvents panelVerificationGeneral As System.Windows.Forms.Panel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents buttonHelp As System.Windows.Forms.Button
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonOK As System.Windows.Forms.Button
    Friend WithEvents panelSynchronizationGeneral As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents checkVerifyMappings As System.Windows.Forms.CheckBox
    Friend WithEvents checkAutoVerify As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents checkShowStatusBar As System.Windows.Forms.CheckBox
    Friend WithEvents panelEnvironmentGeneral As System.Windows.Forms.Panel
    Friend WithEvents checkAutoAcceptPreview As System.Windows.Forms.CheckBox
    Friend WithEvents checkAutoSetTargetLanguages As System.Windows.Forms.CheckBox
    Friend WithEvents checkShowNamespaceNodes As System.Windows.Forms.CheckBox
    Friend WithEvents checkShowMappedNodes As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents checkShowInheritedNodes As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents listHidePropGridCategories As System.Windows.Forms.ListView
    Friend WithEvents panelEnvironmentFontsAndColors As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents comboFontSettings As System.Windows.Forms.ComboBox
    Friend WithEvents comboFontFamily As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents comboFontSize As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents buttonUseDefaultFontSettings As System.Windows.Forms.Button
    Friend WithEvents buttonCustomFontForeColor As System.Windows.Forms.Button
    Friend WithEvents comboFontItemForeColor As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents buttonCustomFontBackColor As System.Windows.Forms.Button
    Friend WithEvents comboFontItemBackColor As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents checkFontItemBold As System.Windows.Forms.CheckBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents checkAutoOpenLastProjectOnStartup As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmOptions))
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.panelVerificationGeneral = New System.Windows.Forms.Panel
        Me.checkAutoSetTargetLanguages = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.checkVerifyMappings = New System.Windows.Forms.CheckBox
        Me.checkAutoVerify = New System.Windows.Forms.CheckBox
        Me.buttonHelp = New System.Windows.Forms.Button
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.buttonOK = New System.Windows.Forms.Button
        Me.panelSynchronizationGeneral = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.checkAutoAcceptPreview = New System.Windows.Forms.CheckBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.panelEnvironmentGeneral = New System.Windows.Forms.Panel
        Me.checkAutoOpenLastProjectOnStartup = New System.Windows.Forms.CheckBox
        Me.listHidePropGridCategories = New System.Windows.Forms.ListView
        Me.Label6 = New System.Windows.Forms.Label
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.checkShowInheritedNodes = New System.Windows.Forms.CheckBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.checkShowMappedNodes = New System.Windows.Forms.CheckBox
        Me.checkShowNamespaceNodes = New System.Windows.Forms.CheckBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.checkShowStatusBar = New System.Windows.Forms.CheckBox
        Me.panelEnvironmentFontsAndColors = New System.Windows.Forms.Panel
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.checkFontItemBold = New System.Windows.Forms.CheckBox
        Me.buttonCustomFontBackColor = New System.Windows.Forms.Button
        Me.comboFontItemBackColor = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.buttonCustomFontForeColor = New System.Windows.Forms.Button
        Me.comboFontItemForeColor = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.buttonUseDefaultFontSettings = New System.Windows.Forms.Button
        Me.comboFontSize = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.comboFontFamily = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.comboFontSettings = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog
        Me.panelVerificationGeneral.SuspendLayout()
        Me.panelSynchronizationGeneral.SuspendLayout()
        Me.panelEnvironmentGeneral.SuspendLayout()
        Me.panelEnvironmentFontsAndColors.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.ImageList = Me.ImageList1
        Me.TreeView1.Location = New System.Drawing.Point(8, 8)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.ShowLines = False
        Me.TreeView1.ShowPlusMinus = False
        Me.TreeView1.ShowRootLines = False
        Me.TreeView1.Size = New System.Drawing.Size(160, 304)
        Me.TreeView1.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'panelVerificationGeneral
        '
        Me.panelVerificationGeneral.Controls.Add(Me.checkAutoSetTargetLanguages)
        Me.panelVerificationGeneral.Controls.Add(Me.GroupBox1)
        Me.panelVerificationGeneral.Controls.Add(Me.Label2)
        Me.panelVerificationGeneral.Controls.Add(Me.checkVerifyMappings)
        Me.panelVerificationGeneral.Controls.Add(Me.checkAutoVerify)
        Me.panelVerificationGeneral.Location = New System.Drawing.Point(168, 0)
        Me.panelVerificationGeneral.Name = "panelVerificationGeneral"
        Me.panelVerificationGeneral.Size = New System.Drawing.Size(424, 304)
        Me.panelVerificationGeneral.TabIndex = 1
        Me.panelVerificationGeneral.Visible = False
        '
        'checkAutoSetTargetLanguages
        '
        Me.checkAutoSetTargetLanguages.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkAutoSetTargetLanguages.Location = New System.Drawing.Point(20, 277)
        Me.checkAutoSetTargetLanguages.Name = "checkAutoSetTargetLanguages"
        Me.checkAutoSetTargetLanguages.Size = New System.Drawing.Size(377, 17)
        Me.checkAutoSetTargetLanguages.TabIndex = 7
        Me.checkAutoSetTargetLanguages.Text = "Auto-set domain target languages from available synch configs"
        Me.checkAutoSetTargetLanguages.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(64, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(352, 8)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(16, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Settings"
        '
        'checkVerifyMappings
        '
        Me.checkVerifyMappings.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkVerifyMappings.Location = New System.Drawing.Point(32, 55)
        Me.checkVerifyMappings.Name = "checkVerifyMappings"
        Me.checkVerifyMappings.Size = New System.Drawing.Size(104, 17)
        Me.checkVerifyMappings.TabIndex = 4
        Me.checkVerifyMappings.Text = "Verify Mappings"
        '
        'checkAutoVerify
        '
        Me.checkAutoVerify.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkAutoVerify.Location = New System.Drawing.Point(32, 32)
        Me.checkAutoVerify.Name = "checkAutoVerify"
        Me.checkAutoVerify.Size = New System.Drawing.Size(168, 16)
        Me.checkAutoVerify.TabIndex = 3
        Me.checkAutoVerify.Text = "Auto Verify"
        '
        'buttonHelp
        '
        Me.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonHelp.Location = New System.Drawing.Point(504, 328)
        Me.buttonHelp.Name = "buttonHelp"
        Me.buttonHelp.TabIndex = 2
        Me.buttonHelp.Text = "Help"
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(424, 328)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.TabIndex = 3
        Me.buttonCancel.Text = "Cancel"
        '
        'buttonOK
        '
        Me.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOK.Location = New System.Drawing.Point(344, 328)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.TabIndex = 4
        Me.buttonOK.Text = "OK"
        '
        'panelSynchronizationGeneral
        '
        Me.panelSynchronizationGeneral.Controls.Add(Me.GroupBox2)
        Me.panelSynchronizationGeneral.Controls.Add(Me.Label1)
        Me.panelSynchronizationGeneral.Controls.Add(Me.checkAutoAcceptPreview)
        Me.panelSynchronizationGeneral.Location = New System.Drawing.Point(168, 0)
        Me.panelSynchronizationGeneral.Name = "panelSynchronizationGeneral"
        Me.panelSynchronizationGeneral.Size = New System.Drawing.Size(424, 304)
        Me.panelSynchronizationGeneral.TabIndex = 3
        Me.panelSynchronizationGeneral.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox2.Location = New System.Drawing.Point(64, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(352, 8)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(16, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Settings"
        '
        'checkAutoAcceptPreview
        '
        Me.checkAutoAcceptPreview.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkAutoAcceptPreview.Location = New System.Drawing.Point(32, 32)
        Me.checkAutoAcceptPreview.Name = "checkAutoAcceptPreview"
        Me.checkAutoAcceptPreview.Size = New System.Drawing.Size(216, 16)
        Me.checkAutoAcceptPreview.TabIndex = 1
        Me.checkAutoAcceptPreview.Text = "Accept previews automatically"
        '
        'GroupBox3
        '
        Me.GroupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox3.Location = New System.Drawing.Point(176, 304)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(408, 8)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        '
        'panelEnvironmentGeneral
        '
        Me.panelEnvironmentGeneral.Controls.Add(Me.checkAutoOpenLastProjectOnStartup)
        Me.panelEnvironmentGeneral.Controls.Add(Me.listHidePropGridCategories)
        Me.panelEnvironmentGeneral.Controls.Add(Me.Label6)
        Me.panelEnvironmentGeneral.Controls.Add(Me.GroupBox6)
        Me.panelEnvironmentGeneral.Controls.Add(Me.Label5)
        Me.panelEnvironmentGeneral.Controls.Add(Me.checkShowInheritedNodes)
        Me.panelEnvironmentGeneral.Controls.Add(Me.GroupBox5)
        Me.panelEnvironmentGeneral.Controls.Add(Me.Label4)
        Me.panelEnvironmentGeneral.Controls.Add(Me.checkShowMappedNodes)
        Me.panelEnvironmentGeneral.Controls.Add(Me.checkShowNamespaceNodes)
        Me.panelEnvironmentGeneral.Controls.Add(Me.GroupBox4)
        Me.panelEnvironmentGeneral.Controls.Add(Me.Label3)
        Me.panelEnvironmentGeneral.Controls.Add(Me.checkShowStatusBar)
        Me.panelEnvironmentGeneral.Location = New System.Drawing.Point(168, 0)
        Me.panelEnvironmentGeneral.Name = "panelEnvironmentGeneral"
        Me.panelEnvironmentGeneral.Size = New System.Drawing.Size(424, 304)
        Me.panelEnvironmentGeneral.TabIndex = 6
        Me.panelEnvironmentGeneral.Visible = False
        '
        'checkAutoOpenLastProjectOnStartup
        '
        Me.checkAutoOpenLastProjectOnStartup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkAutoOpenLastProjectOnStartup.Location = New System.Drawing.Point(176, 32)
        Me.checkAutoOpenLastProjectOnStartup.Name = "checkAutoOpenLastProjectOnStartup"
        Me.checkAutoOpenLastProjectOnStartup.Size = New System.Drawing.Size(200, 16)
        Me.checkAutoOpenLastProjectOnStartup.TabIndex = 13
        Me.checkAutoOpenLastProjectOnStartup.Text = "Re-open last open project at startup"
        '
        'listHidePropGridCategories
        '
        Me.listHidePropGridCategories.CheckBoxes = True
        Me.listHidePropGridCategories.Location = New System.Drawing.Point(33, 201)
        Me.listHidePropGridCategories.MultiSelect = False
        Me.listHidePropGridCategories.Name = "listHidePropGridCategories"
        Me.listHidePropGridCategories.Size = New System.Drawing.Size(154, 97)
        Me.listHidePropGridCategories.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listHidePropGridCategories.TabIndex = 12
        Me.listHidePropGridCategories.View = System.Windows.Forms.View.Details
        '
        'Label6
        '
        Me.Label6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label6.Location = New System.Drawing.Point(200, 201)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(193, 42)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Hide the checked categories from the property grid"
        '
        'GroupBox6
        '
        Me.GroupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox6.Location = New System.Drawing.Point(93, 173)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(320, 8)
        Me.GroupBox6.TabIndex = 8
        Me.GroupBox6.TabStop = False
        '
        'Label5
        '
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label5.Location = New System.Drawing.Point(13, 173)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 16)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Property Grid"
        '
        'checkShowInheritedNodes
        '
        Me.checkShowInheritedNodes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkShowInheritedNodes.Location = New System.Drawing.Point(32, 136)
        Me.checkShowInheritedNodes.Name = "checkShowInheritedNodes"
        Me.checkShowInheritedNodes.Size = New System.Drawing.Size(176, 16)
        Me.checkShowInheritedNodes.TabIndex = 7
        Me.checkShowInheritedNodes.Text = "Show inherited properties"
        '
        'GroupBox5
        '
        Me.GroupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox5.Location = New System.Drawing.Point(80, 64)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(336, 8)
        Me.GroupBox5.TabIndex = 5
        Me.GroupBox5.TabStop = False
        '
        'Label4
        '
        Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label4.Location = New System.Drawing.Point(16, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Tree View"
        '
        'checkShowMappedNodes
        '
        Me.checkShowMappedNodes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkShowMappedNodes.Location = New System.Drawing.Point(32, 112)
        Me.checkShowMappedNodes.Name = "checkShowMappedNodes"
        Me.checkShowMappedNodes.Size = New System.Drawing.Size(200, 16)
        Me.checkShowMappedNodes.TabIndex = 4
        Me.checkShowMappedNodes.Text = "Show mapped tables and columns"
        '
        'checkShowNamespaceNodes
        '
        Me.checkShowNamespaceNodes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkShowNamespaceNodes.Location = New System.Drawing.Point(32, 88)
        Me.checkShowNamespaceNodes.Name = "checkShowNamespaceNodes"
        Me.checkShowNamespaceNodes.Size = New System.Drawing.Size(160, 16)
        Me.checkShowNamespaceNodes.TabIndex = 3
        Me.checkShowNamespaceNodes.Text = "Show namespaces"
        '
        'GroupBox4
        '
        Me.GroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox4.Location = New System.Drawing.Point(73, 8)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(340, 8)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        '
        'Label3
        '
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(16, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Settings"
        '
        'checkShowStatusBar
        '
        Me.checkShowStatusBar.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkShowStatusBar.Location = New System.Drawing.Point(32, 32)
        Me.checkShowStatusBar.Name = "checkShowStatusBar"
        Me.checkShowStatusBar.Size = New System.Drawing.Size(104, 16)
        Me.checkShowStatusBar.TabIndex = 1
        Me.checkShowStatusBar.Text = "Show status bar"
        '
        'panelEnvironmentFontsAndColors
        '
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.TextBox1)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.ListBox1)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.checkFontItemBold)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.buttonCustomFontBackColor)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.comboFontItemBackColor)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.Label11)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.buttonCustomFontForeColor)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.comboFontItemForeColor)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.Label10)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.buttonUseDefaultFontSettings)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.comboFontSize)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.Label9)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.comboFontFamily)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.Label8)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.comboFontSettings)
        Me.panelEnvironmentFontsAndColors.Controls.Add(Me.Label7)
        Me.panelEnvironmentFontsAndColors.Location = New System.Drawing.Point(167, 0)
        Me.panelEnvironmentFontsAndColors.Name = "panelEnvironmentFontsAndColors"
        Me.panelEnvironmentFontsAndColors.Size = New System.Drawing.Size(423, 304)
        Me.panelEnvironmentFontsAndColors.TabIndex = 7
        Me.panelEnvironmentFontsAndColors.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Location = New System.Drawing.Point(13, 270)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(394, 20)
        Me.TextBox1.TabIndex = 17
        Me.TextBox1.Text = "AaBbCcXxYyZz"
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(13, 132)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(167, 95)
        Me.ListBox1.TabIndex = 16
        '
        'checkFontItemBold
        '
        Me.checkFontItemBold.Location = New System.Drawing.Point(193, 236)
        Me.checkFontItemBold.Name = "checkFontItemBold"
        Me.checkFontItemBold.Size = New System.Drawing.Size(87, 21)
        Me.checkFontItemBold.TabIndex = 15
        Me.checkFontItemBold.Text = "Bold"
        '
        'buttonCustomFontBackColor
        '
        Me.buttonCustomFontBackColor.Location = New System.Drawing.Point(320, 201)
        Me.buttonCustomFontBackColor.Name = "buttonCustomFontBackColor"
        Me.buttonCustomFontBackColor.Size = New System.Drawing.Size(87, 23)
        Me.buttonCustomFontBackColor.TabIndex = 14
        Me.buttonCustomFontBackColor.Text = "Custom..."
        '
        'comboFontItemBackColor
        '
        Me.comboFontItemBackColor.Location = New System.Drawing.Point(193, 201)
        Me.comboFontItemBackColor.Name = "comboFontItemBackColor"
        Me.comboFontItemBackColor.Size = New System.Drawing.Size(120, 21)
        Me.comboFontItemBackColor.TabIndex = 13
        Me.comboFontItemBackColor.Text = "ComboBox2"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label11.Location = New System.Drawing.Point(193, 180)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 16)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Item background:"
        '
        'buttonCustomFontForeColor
        '
        Me.buttonCustomFontForeColor.Location = New System.Drawing.Point(320, 146)
        Me.buttonCustomFontForeColor.Name = "buttonCustomFontForeColor"
        Me.buttonCustomFontForeColor.Size = New System.Drawing.Size(87, 23)
        Me.buttonCustomFontForeColor.TabIndex = 11
        Me.buttonCustomFontForeColor.Text = "Custom..."
        '
        'comboFontItemForeColor
        '
        Me.comboFontItemForeColor.Location = New System.Drawing.Point(193, 146)
        Me.comboFontItemForeColor.Name = "comboFontItemForeColor"
        Me.comboFontItemForeColor.Size = New System.Drawing.Size(120, 21)
        Me.comboFontItemForeColor.TabIndex = 10
        Me.comboFontItemForeColor.Text = "ComboBox1"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label10.Location = New System.Drawing.Point(193, 125)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 16)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Item foreground:"
        '
        'buttonUseDefaultFontSettings
        '
        Me.buttonUseDefaultFontSettings.Location = New System.Drawing.Point(320, 28)
        Me.buttonUseDefaultFontSettings.Name = "buttonUseDefaultFontSettings"
        Me.buttonUseDefaultFontSettings.Size = New System.Drawing.Size(87, 23)
        Me.buttonUseDefaultFontSettings.TabIndex = 8
        Me.buttonUseDefaultFontSettings.Text = "Use Defaults"
        '
        'comboFontSize
        '
        Me.comboFontSize.Location = New System.Drawing.Point(320, 83)
        Me.comboFontSize.Name = "comboFontSize"
        Me.comboFontSize.Size = New System.Drawing.Size(88, 21)
        Me.comboFontSize.TabIndex = 7
        Me.comboFontSize.Text = "ComboBox1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label9.Location = New System.Drawing.Point(320, 62)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(29, 16)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Size:"
        '
        'comboFontFamily
        '
        Me.comboFontFamily.Location = New System.Drawing.Point(13, 83)
        Me.comboFontFamily.Name = "comboFontFamily"
        Me.comboFontFamily.Size = New System.Drawing.Size(294, 21)
        Me.comboFontFamily.TabIndex = 5
        Me.comboFontFamily.Text = "ComboBox1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label8.Location = New System.Drawing.Point(13, 62)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 16)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Font:"
        '
        'comboFontSettings
        '
        Me.comboFontSettings.Location = New System.Drawing.Point(13, 28)
        Me.comboFontSettings.Name = "comboFontSettings"
        Me.comboFontSettings.Size = New System.Drawing.Size(294, 21)
        Me.comboFontSettings.TabIndex = 3
        Me.comboFontSettings.Text = "ComboBox1"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label7.Location = New System.Drawing.Point(16, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 16)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Show settings for:"
        '
        'frmOptions
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(591, 358)
        Me.Controls.Add(Me.panelEnvironmentGeneral)
        Me.Controls.Add(Me.panelEnvironmentFontsAndColors)
        Me.Controls.Add(Me.panelSynchronizationGeneral)
        Me.Controls.Add(Me.panelVerificationGeneral)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.buttonOK)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonHelp)
        Me.Controls.Add(Me.TreeView1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.Text = "Options"
        Me.panelVerificationGeneral.ResumeLayout(False)
        Me.panelSynchronizationGeneral.ResumeLayout(False)
        Me.panelEnvironmentGeneral.ResumeLayout(False)
        Me.panelEnvironmentFontsAndColors.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_currPanel As Panel

    Private m_ApplicationSettings As ApplicationSettings

    Public Sub LoadOptions(ByVal applicationSettings As ApplicationSettings)

        SetupTree()

        m_ApplicationSettings = applicationSettings

        LoadSettings()

    End Sub

    Private Sub SetupTree()

        Dim newNode As OptionsNode
        Dim parentNode As OptionsNode

        TreeView1.Nodes.Clear()

        'Environment
        newNode = New OptionsNode("Environment", 0, 1)

        TreeView1.Nodes.Add(newNode)

        parentNode = newNode

        newNode = New OptionsNode("General", 2, 3, panelEnvironmentGeneral)

        parentNode.Nodes.Add(newNode)


        'Verification
        newNode = New OptionsNode("Verification", 0, 1)

        TreeView1.Nodes.Add(newNode)

        parentNode = newNode

        newNode = New OptionsNode("General", 2, 3, panelVerificationGeneral)

        parentNode.Nodes.Add(newNode)



        'Synchronization
        newNode = New OptionsNode("Synchronization", 0, 1)

        TreeView1.Nodes.Add(newNode)

        parentNode = newNode

        newNode = New OptionsNode("General", 2, 3, panelSynchronizationGeneral)

        parentNode.Nodes.Add(newNode)

    End Sub


    Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        Select Case e.Node.ImageIndex

            Case 0

                If Not e.Node.IsExpanded Then

                    e.Node.Expand()

                    CloseNodes()

                End If

            Case 2

                If Not m_currPanel Is Nothing Then

                    m_currPanel.Hide()

                End If

                m_currPanel = CType(e.Node, OptionsNode).OptionsPanel

                m_currPanel.Show()

        End Select

    End Sub

    Private Sub TreeView1_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterExpand

        Select Case e.Node.ImageIndex

            Case 0

                If Not e.Node.FirstNode Is Nothing Then

                    TreeView1.SelectedNode = e.Node.FirstNode

                End If

            Case 2

        End Select

    End Sub

    Private Sub CloseNodes()

        Dim node As TreeNode

        For Each node In TreeView1.Nodes

            CloseNode(node)

        Next

    End Sub

    Private Sub CloseNode(ByVal node As TreeNode)

        Dim childNode As TreeNode

        For Each childNode In node.Nodes

            CloseNode(childNode)

        Next

        If Not ContainsSelectedNode(node) Then

            node.Collapse()

        End If

    End Sub

    Private Function ContainsSelectedNode(ByVal node As TreeNode) As Boolean

        Dim childNode As TreeNode

        If node Is TreeView1.SelectedNode Then Return True

        For Each childNode In node.Nodes

            If ContainsSelectedNode(childNode) Then Return True

        Next

    End Function

    Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click

        SaveSettings()

        Me.Close()

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        Me.Close()

    End Sub


    Private Sub SetupPropGridCategories()

        Dim type As type
        Dim asm As System.Reflection.Assembly
        Dim prop As System.Reflection.PropertyInfo
        Dim attr As System.ComponentModel.CategoryAttribute
        Dim categories As New ArrayList
        Dim category As String

        listHidePropGridCategories.Items.Clear()

        listHidePropGridCategories.Columns.Add("Category", 250, HorizontalAlignment.Left)

        For Each asm In AppDomain.CurrentDomain.GetAssemblies

            Try

                If asm.Location = Application.ExecutablePath Then

                    For Each type In asm.GetTypes

                        If type.IsSubclassOf(GetType(PropertiesBase)) Then

                            For Each prop In type.GetProperties

                                For Each attr In prop.GetCustomAttributes(GetType(System.ComponentModel.CategoryAttribute), False)

                                    If Not categories.Contains(attr.Category) Then

                                        categories.Add(attr.Category)

                                    End If

                                    Exit For

                                Next

                            Next

                        End If

                        'listHidePropGridCategories.Items.Add("Design")
                        'listHidePropGridCategories.Items.Add("Mapping")
                        'listHidePropGridCategories.Items.Add("NHibernate")

                    Next

                    Exit For

                End If

            Catch ex As Exception

            End Try

        Next

        For Each category In categories

            listHidePropGridCategories.Items.Add(New ListViewItem(category))

        Next

    End Sub


    Private Sub LoadSettings()

        Dim listItem As ListViewItem

        With m_ApplicationSettings.OptionSettings

            With .EnvironmentSettings

                checkShowStatusBar.Checked = .ShowStatusBar
                checkAutoOpenLastProjectOnStartup.Checked = .AutoOpenLastProjectOnStartup

                checkShowNamespaceNodes.Checked = .ShowNamespaceNodes
                checkShowMappedNodes.Checked = .ShowMappedNodes
                checkShowInheritedNodes.Checked = .ShowInheritedNodes

                SetupPropGridCategories()

                For Each listItem In listHidePropGridCategories.Items

                    If .HidePropGridCategories.Contains(listItem.Text) Then

                        listItem.Checked = True

                    End If

                Next

            End With

            With .VerificationSettings

                checkAutoVerify.Checked = .AutoVerify
                checkVerifyMappings.Checked = .VerifyMappings
                checkAutoSetTargetLanguages.Checked = .AutoSetTargetLanguages

            End With

            With .SynchronizationSettings

                checkAutoAcceptPreview.Checked = .AutoAcceptPreview

            End With

        End With

    End Sub

    Private Sub SaveSettings()

        Dim listItem As ListViewItem

        With m_ApplicationSettings.OptionSettings

            With .EnvironmentSettings

                .ShowStatusBar = checkShowStatusBar.Checked
                .AutoOpenLastProjectOnStartup = checkAutoOpenLastProjectOnStartup.Checked

                .ShowNamespaceNodes = checkShowNamespaceNodes.Checked
                .ShowMappedNodes = checkShowMappedNodes.Checked
                .ShowInheritedNodes = checkShowInheritedNodes.Checked

                .HidePropGridCategories.Clear()

                For Each listItem In listHidePropGridCategories.CheckedItems

                    .HidePropGridCategories.Add(listItem.Text)

                Next

            End With

            With .VerificationSettings

                .AutoVerify = checkAutoVerify.Checked
                .VerifyMappings = checkVerifyMappings.Checked
                .AutoSetTargetLanguages = checkAutoSetTargetLanguages.Checked

            End With

            With .SynchronizationSettings

                .AutoAcceptPreview = checkAutoAcceptPreview.Checked

            End With

        End With

    End Sub
End Class
