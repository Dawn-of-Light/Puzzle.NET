Imports Puzzle.NPersist.Framework.Mapping
Imports System.IO

Public Class frmExportToNHibernate
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
    Friend WithEvents textTargetDir As System.Windows.Forms.TextBox
    Friend WithEvents buttonBrowseTargetDir As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents comboTargetLanuage As System.Windows.Forms.ComboBox
    Friend WithEvents buttonOK As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents labelDomainName As System.Windows.Forms.Label
    Friend WithEvents checkXmlPerClass As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmExportToNHibernate))
        Me.textTargetDir = New System.Windows.Forms.TextBox
        Me.buttonBrowseTargetDir = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.comboTargetLanuage = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.buttonOK = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.labelDomainName = New System.Windows.Forms.Label
        Me.checkXmlPerClass = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'textTargetDir
        '
        Me.textTargetDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.textTargetDir.Location = New System.Drawing.Point(13, 118)
        Me.textTargetDir.Name = "textTargetDir"
        Me.textTargetDir.Size = New System.Drawing.Size(254, 20)
        Me.textTargetDir.TabIndex = 0
        Me.textTargetDir.Text = ""
        '
        'buttonBrowseTargetDir
        '
        Me.buttonBrowseTargetDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonBrowseTargetDir.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonBrowseTargetDir.Location = New System.Drawing.Point(267, 118)
        Me.buttonBrowseTargetDir.Name = "buttonBrowseTargetDir"
        Me.buttonBrowseTargetDir.Size = New System.Drawing.Size(26, 20)
        Me.buttonBrowseTargetDir.TabIndex = 1
        Me.buttonBrowseTargetDir.Text = "..."
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(13, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(267, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Please select the target directory for generated files:"
        '
        'comboTargetLanuage
        '
        Me.comboTargetLanuage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboTargetLanuage.Items.AddRange(New Object() {"C#", "VB.NET", "Delphi"})
        Me.comboTargetLanuage.Location = New System.Drawing.Point(13, 159)
        Me.comboTargetLanuage.Name = "comboTargetLanuage"
        Me.comboTargetLanuage.Size = New System.Drawing.Size(280, 21)
        Me.comboTargetLanuage.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(13, 146)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(240, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Please select target language:"
        '
        'buttonOK
        '
        Me.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOK.Location = New System.Drawing.Point(167, 229)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.Size = New System.Drawing.Size(62, 20)
        Me.buttonOK.TabIndex = 5
        Me.buttonOK.Text = "OK"
        '
        'Label3
        '
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(13, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(254, 41)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Note: Existing files will be overwritten! Use the Model To Code Synchronizer to p" & _
        "reserve custom code sections!"
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(233, 229)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(63, 20)
        Me.buttonCancel.TabIndex = 7
        Me.buttonCancel.Text = "Cancel"
        '
        'labelDomainName
        '
        Me.labelDomainName.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.labelDomainName.Location = New System.Drawing.Point(13, 14)
        Me.labelDomainName.Name = "labelDomainName"
        Me.labelDomainName.Size = New System.Drawing.Size(254, 28)
        Me.labelDomainName.TabIndex = 8
        Me.labelDomainName.Text = "Export domain ''"
        '
        'checkXmlPerClass
        '
        Me.checkXmlPerClass.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.checkXmlPerClass.Location = New System.Drawing.Point(13, 194)
        Me.checkXmlPerClass.Name = "checkXmlPerClass"
        Me.checkXmlPerClass.Size = New System.Drawing.Size(267, 21)
        Me.checkXmlPerClass.TabIndex = 9
        Me.checkXmlPerClass.Text = "Create separate xml file for each root class"
        '
        'frmExportToNHibernate
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(306, 256)
        Me.Controls.Add(Me.checkXmlPerClass)
        Me.Controls.Add(Me.labelDomainName)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.buttonOK)
        Me.Controls.Add(Me.comboTargetLanuage)
        Me.Controls.Add(Me.buttonBrowseTargetDir)
        Me.Controls.Add(Me.textTargetDir)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmExportToNHibernate"
        Me.Text = "Export To NHibernate"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_frmOwner As frmDomainMapBrowser
    Private m_domainMap As IDomainMap

    Private m_NPersistToNHibernate As New Tools.NPersistToNHibernate

    Public Sub Setup(ByVal frmOwner As frmDomainMapBrowser, ByVal domainMap As IDomainMap)

        m_frmOwner = frmOwner
        m_domainMap = domainMap

        labelDomainName.Text = "Export domain: " & domainMap.Name

    End Sub


    Private Sub frmExportToNHibernate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        comboTargetLanuage.SelectedIndex = 0

    End Sub

    Private Sub buttonBrowseTargetDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonBrowseTargetDir.Click

        If FolderBrowserDialog1.ShowDialog() Then

            textTargetDir.Text = FolderBrowserDialog1.SelectedPath

        End If

    End Sub

    Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click

        Export()

        MsgBox("Exported domain '" & m_domainMap.Name & "' to '" & textTargetDir.Text & "' using NHibernate format!")

        Me.Hide()

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        Me.Hide()

    End Sub

    Public Sub Export()

        If Not Verify() Then Exit Sub

        Dim xml As String
        Dim code As String
        Dim fileName As String
        Dim classMap As IClassMap
        Dim classMapsAndFiles As New Hashtable
        Dim embeddedFiles As New ArrayList
        Dim projPath As String = textTargetDir.Text

        If checkXmlPerClass.Checked Then

            For Each classMap In m_NPersistToNHibernate.GetRootClasses(m_domainMap)

                xml = m_NPersistToNHibernate.Serialize(classMap)

                'fileName = classMap.Name & ".hbm.xml"
                fileName = classMap.GetFullName & ".hbm.xml"

                WriteFile(fileName, xml)

                embeddedFiles.Add(projPath & "\" & fileName)

            Next

        Else

            xml = m_NPersistToNHibernate.Serialize(m_domainMap)

            fileName = m_domainMap.Name & ".hbm.xml"

            WriteFile(fileName, xml)

            embeddedFiles.Add(projPath & "\" & fileName)

        End If

        For Each classMap In m_domainMap.ClassMaps

            fileName = ""

            Select Case comboTargetLanuage.SelectedIndex

                Case 0

                    code = m_NPersistToNHibernate.ToCSharp(classMap)
                    fileName = classMap.Name & ".cs"

                Case 1

                    code = m_NPersistToNHibernate.ToVb(classMap)
                    fileName = classMap.Name & ".vb"

                Case 2

                    code = m_NPersistToNHibernate.ToDelphi(classMap)
                    fileName = classMap.Name & ".pas"

            End Select

            WriteFile(fileName, code)

            classMapsAndFiles(classMap) = projPath & "\" & fileName

        Next

        Select Case comboTargetLanuage.SelectedIndex

            Case 0

                code = m_NPersistToNHibernate.ToAssemblyInfoCSharp(m_domainMap)
                fileName = "AssemblyInfo.cs"

            Case 1

                code = m_NPersistToNHibernate.ToAssemblyInfoVb(m_domainMap)
                fileName = "AssemblyInfo.vb"

            Case 2

                fileName = ""
                'code = m_NPersistToNHibernate.ToAssemblyInfoDelphi(m_domainMap)
                'fileName = classMap.Name & ".pas"

        End Select

        If Not fileName = "" Then

            WriteFile(fileName, code)

        End If

        Select Case comboTargetLanuage.SelectedIndex

            Case 0

                code = m_NPersistToNHibernate.ToProjectCSharp(m_domainMap, projPath, classMapsAndFiles, embeddedFiles)
                fileName = m_domainMap.Name & ".csproj"

            Case 1

                code = m_NPersistToNHibernate.ToProjectVb(m_domainMap, projPath, classMapsAndFiles, embeddedFiles)
                fileName = m_domainMap.Name & ".vbproj"

            Case 2

                fileName = ""
                'code = m_NPersistToNHibernate.ToProjectDelphi(m_domainMap, projPath, classMapsAndFiles)
                'fileName = m_domainMap.Name & ".pas"

        End Select

        If Not fileName = "" Then

            WriteFile(fileName, code)

        End If

    End Sub

    Private Sub WriteFile(ByVal fileName As String, ByVal body As String)

        Dim fileWriter As StreamWriter

        fileWriter = File.CreateText(textTargetDir.Text & "\" & fileName)

        fileWriter.Write(body)

        fileWriter.Close()

    End Sub

    Private Function Verify() As Boolean

        If Len(textTargetDir.Text) < 1 Then

            MsgBox("Please select a target directory first!")

            Exit Function

        End If

        If Not Directory.Exists(textTargetDir.Text) Then

            MsgBox("Please select an existing target directory first!" & vbCrLf & vbCrLf & "Directory '" & textTargetDir.Text & "' could not be found!")

            Exit Function

        End If

        Return True

    End Function

End Class
