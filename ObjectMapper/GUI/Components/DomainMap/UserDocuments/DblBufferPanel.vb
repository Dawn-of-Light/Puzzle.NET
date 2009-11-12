Public Class DblBufferPanel
    Inherits Panel


    Public Sub New()
        MyBase.New()

        'Add any initialization after the InitializeComponent() call
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)

    End Sub

End Class
