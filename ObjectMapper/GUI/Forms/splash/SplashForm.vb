Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Threading

Public Class SplashForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        SetRightSize()

        'Dim gp As GraphicsPath = New GraphicsPath(FillMode.Winding)
        'Dim r As Rectangle = Me.Bounds
        'r.Inflate(-1, -1)
        'gp.AddEllipse(r)
        'gp.AddRectangle(r)
        'Me.Region = New Region(gp)

        Dim ts As ThreadStart = New ThreadStart(AddressOf myThread)
        Dim t As Thread = New Thread(ts)
        'Dim t As Thread = New Thread(AddressOf myThread)
        t.Start()

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SplashForm))
        '
        'SplashForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(424, 224)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SplashForm"
        Me.Opacity = 0
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SplashForm"

    End Sub

#End Region


    Private Sub myThread()

        Try

            Dim dOpac As Double = 0.1
            Dim dOpac2 As Double

            For dOpac = 0.1 To 1.0 Step 0.1

                Me.Opacity = dOpac
                Thread.Sleep(100)

            Next

            Thread.Sleep(3000)

            dOpac2 = dOpac

            For dOpac = dOpac2 To 0.1 Step -0.1

                Me.Opacity = dOpac
                Thread.Sleep(100)

            Next

        Catch ex As Exception

        End Try

		Me.Invoke(New MethodInvoker(AddressOf CloseForm))

	End Sub

	Private Sub CloseForm()

		Me.Close()

	End Sub

    Private Sub SetRightSize()

        Me.Height = Me.BackgroundImage.Height
        Me.Width = Me.BackgroundImage.Width

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        MyBase.OnMouseDown(e)

        Me.Close()

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub
End Class
