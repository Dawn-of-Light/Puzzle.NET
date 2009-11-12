Imports Puzzle.NPersist.Framework.Mapping

Public Class frmColumnListEditor
    Inherits System.Windows.Forms.Form

    Private m_Columns As String()

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
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents labelTableName As System.Windows.Forms.Label
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents buttonOK As System.Windows.Forms.Button
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents buttonSelect As System.Windows.Forms.Button
    Friend WithEvents buttonRemove As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmColumnListEditor))
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.labelTableName = New System.Windows.Forms.Label
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.buttonOK = New System.Windows.Forms.Button
        Me.buttonCancel = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.buttonSelect = New System.Windows.Forms.Button
        Me.buttonRemove = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Location = New System.Drawing.Point(8, 48)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(208, 192)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 0
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Label1
        '
        Me.Label1.ImageIndex = 1
        Me.Label1.ImageList = Me.ImageList1
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 16)
        Me.Label1.TabIndex = 1
        '
        'labelTableName
        '
        Me.labelTableName.AutoSize = True
        Me.labelTableName.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.labelTableName.Location = New System.Drawing.Point(32, 8)
        Me.labelTableName.Name = "labelTableName"
        Me.labelTableName.Size = New System.Drawing.Size(64, 16)
        Me.labelTableName.TabIndex = 2
        Me.labelTableName.Text = "Table name"
        '
        'ListView2
        '
        Me.ListView2.Location = New System.Drawing.Point(264, 48)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(208, 192)
        Me.ListView2.SmallImageList = Me.ImageList1
        Me.ListView2.TabIndex = 3
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'buttonOK
        '
        Me.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonOK.Location = New System.Drawing.Point(320, 263)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.TabIndex = 4
        Me.buttonOK.Text = "OK"
        '
        'buttonCancel
        '
        Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonCancel.Location = New System.Drawing.Point(400, 263)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.TabIndex = 5
        Me.buttonCancel.Text = "Cancel"
        '
        'GroupBox1
        '
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Location = New System.Drawing.Point(8, 248)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(464, 8)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'buttonSelect
        '
        Me.buttonSelect.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonSelect.Location = New System.Drawing.Point(224, 112)
        Me.buttonSelect.Name = "buttonSelect"
        Me.buttonSelect.Size = New System.Drawing.Size(32, 23)
        Me.buttonSelect.TabIndex = 7
        Me.buttonSelect.Text = "<<"
        '
        'buttonRemove
        '
        Me.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonRemove.Location = New System.Drawing.Point(224, 144)
        Me.buttonRemove.Name = "buttonRemove"
        Me.buttonRemove.Size = New System.Drawing.Size(32, 23)
        Me.buttonRemove.TabIndex = 8
        Me.buttonRemove.Text = ">>"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(208, 23)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Selected columns:"
        '
        'Label3
        '
        Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label3.Location = New System.Drawing.Point(264, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(208, 23)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Unmapped columns:"
        '
        'frmColumnListEditor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(481, 294)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.buttonRemove)
        Me.Controls.Add(Me.buttonSelect)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonOK)
        Me.Controls.Add(Me.labelTableName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColumnListEditor"
        Me.Text = "frmColumnListEditor"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub Setup(ByVal propertyMap As IPropertyMap, ByVal isId As Boolean)

        Dim column As String
        Dim columns As ArrayList
        Dim columnMap As IColumnMap
        Dim tableMap As ITableMap
        Dim mapped As ArrayList

        If isId Then
            Me.Text = "Additional Id Columns For " & propertyMap.ClassMap.GetName & "." & propertyMap.Name
            columns = propertyMap.AdditionalIdColumns.Clone
            mapped = propertyMap.AdditionalColumns.Clone
        Else
            Me.Text = "Additional Columns For " & propertyMap.ClassMap.GetName & "." & propertyMap.Name
            columns = propertyMap.AdditionalColumns.Clone
            mapped = propertyMap.AdditionalIdColumns.Clone
        End If

        ListView1.Clear()
        ListView2.Clear()

        ListView1.Columns.Add("Column", 150, HorizontalAlignment.Left)
        ListView2.Columns.Add("Column", 150, HorizontalAlignment.Left)

        If Not Len(propertyMap.Column) < 1 Then

            mapped.Add(propertyMap.Column)

        End If

        If Not Len(propertyMap.IdColumn) < 1 Then

            mapped.Add(propertyMap.IdColumn)

        End If

        For Each column In columns

            ListView1.Items.Add(column, 0)

        Next

        tableMap = propertyMap.GetTableMap

        If Not tableMap Is Nothing Then

            labelTableName.Text = tableMap.Name

            For Each columnMap In tableMap.ColumnMaps

                If Not columns.Contains(columnMap.Name) And Not mapped.Contains(columnMap.Name) Then

                    ListView2.Items.Add(columnMap.Name, 0)

                End If

            Next

        End If

        SetColumns()

    End Sub

    Public Function GetColumns() As String()

        Return m_Columns

    End Function

    Private Sub SetColumns()

        Dim columns As String()
        Dim item As ListViewItem
        Dim i As Integer

        ReDim columns(ListView1.Items.Count - 1)

        For Each item In ListView1.Items
            columns(i) = item.Text
        Next

        m_Columns = columns

    End Sub

    Private Sub buttonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOK.Click

        SetColumns()
        Me.Close()

    End Sub

    Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click

        Me.Close()

    End Sub

    Private Sub buttonSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonSelect.Click

        Dim item As ListViewItem
        Dim items As New ArrayList()

        For Each item In ListView2.SelectedItems

            items.Add(item)

        Next

        For Each item In items

            ListView1.Items.Add(item.Text, 0)
            ListView2.Items.Remove(item)

        Next

    End Sub

    Private Sub buttonRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonRemove.Click


        Dim item As ListViewItem
        Dim items As New ArrayList()

        For Each item In ListView1.SelectedItems

            items.Add(item)

        Next

        For Each item In items

            ListView2.Items.Add(item.Text, 0)
            ListView1.Items.Remove(item)

        Next

    End Sub

End Class
