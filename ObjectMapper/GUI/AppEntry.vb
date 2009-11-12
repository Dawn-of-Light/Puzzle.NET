Imports System.IO
Imports System.Threading

Public Class AppEntry

    Public Shared frm As frmDomainMapBrowser

    Public Shared Sub Main(ByVal CmdArgs() As String)

        Application.EnableVisualStyles()
        Application.DoEvents()

        Dim i As Long
        Dim ArgNum As Integer
        Dim openFiles As New ArrayList
        Dim FixedArgs As ArrayList
        Dim path As String

        If CmdArgs.Length > 0 Then

            FixedArgs = FixArgs(CmdArgs)

            For Each path In FixedArgs

                If IsFileToOpen(path) Then
                    openFiles.Add(path)
                End If

            Next

        End If

        Dim eh As CustomExceptionHandler = New CustomExceptionHandler

        AddHandler Application.ThreadException, AddressOf eh.OnThreadException

        Dim splash As New SplashForm

        Try

            splash.Show()

        Catch ex As Exception

        End Try

        Dim t As DateTime = Now

        frm = New frmDomainMapBrowser

        frm.Starting()

        If openFiles.Count < 1 Then

            If frm.m_ApplicationSettings.OptionSettings.EnvironmentSettings.AutoOpenLastProjectOnStartup Then

                If frm.m_ApplicationSettings.LatestFiles.Count > 0 Then

                    openFiles.Add(frm.m_ApplicationSettings.LatestFiles(0))

                End If

            End If

        End If

        frm.Open(openFiles)

        Application.Run(frm)

    End Sub

    Public Shared Function FixArgs(ByVal CmdArgs() As String) As ArrayList

        Dim FixedArgs As New ArrayList

        Dim path As String = ""
        Dim argNum As Integer

        For argNum = 0 To UBound(CmdArgs)

            If Len(path) > 0 Then
                path += " "
            End If
            path += CmdArgs(argNum)
            If IsFile(path) Then
                If IsFileToOpen(path) Then
                    FixedArgs.Add(path)
                End If
                path = ""
            End If

        Next argNum

        Return FixedArgs

    End Function

    Public Shared Function IsFile(ByVal path As String) As Boolean

        Dim fi As New FileInfo(path)

        If fi.Exists Then

            Return True

        End If

    End Function


    Public Shared Function IsFileToOpen(ByVal path As String) As Boolean

        Dim fi As New FileInfo(path)

        If Not fi.Exists Then

            fi = New FileInfo(System.IO.Path.GetFullPath(path))

        End If

        If fi.Exists Then

            Select Case LCase(fi.Extension)

                Case ".npersist", ".npe"

                    Return True

                Case ".omproj"

                    Return True

            End Select

        End If

    End Function




    ' Creates a class to handle the exception event.
    Private Class CustomExceptionHandler

        ' Handles the exception event.
        Public Sub OnThreadException(ByVal sender As Object, ByVal t As ThreadExceptionEventArgs)
            Dim result As DialogResult = System.Windows.Forms.DialogResult.Cancel
            Try
                result = Me.ShowThreadExceptionDialog(t.Exception)
            Catch
                Try
                    MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)
                Finally
                    Application.Exit()
                End Try
            End Try

            ' Exits the program when the user clicks Abort.
            If (result = System.Windows.Forms.DialogResult.Abort) Then

                If frm.SaveAll(True) Then

                    Application.Exit()

                End If

            End If
        End Sub

        ' Creates the error message and displays it.
        Private Function ShowThreadExceptionDialog(ByVal e As Exception) As DialogResult
            Dim errorMsg As StringWriter = New StringWriter
            errorMsg.WriteLine("An error occurred please contact the adminstrator with the following information:")
            errorMsg.WriteLine("")
            errorMsg.WriteLine(e.Message)
            errorMsg.WriteLine("")
            errorMsg.WriteLine("Stack Trace:")
            errorMsg.WriteLine(e.StackTrace)
            Return MessageBox.Show(errorMsg.ToString(), "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)
        End Function
    End Class

End Class
