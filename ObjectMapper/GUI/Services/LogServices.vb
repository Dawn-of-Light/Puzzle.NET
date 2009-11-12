Public Class LogServices

    Public Shared mainForm As frmDomainMapBrowser

    Public Overloads Shared Sub LogMsg(ByVal msg As String, ByVal logLevel As TraceLevel)

        LogMsg(msg, logLevel, "")

    End Sub

    Public Overloads Shared Sub LogMsg(ByVal msg As String, ByVal logLevel As TraceLevel, ByVal source As String)

        mainForm.LogMsg(msg, logLevel, source)

    End Sub

End Class
