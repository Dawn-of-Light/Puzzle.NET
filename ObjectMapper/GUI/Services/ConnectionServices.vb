Imports Puzzle.NPersist.Framework.Enumerations

Public Class ConnectionServices

    Public Shared Function TestConnection(ByVal connStr As String, ByVal providerType As ProviderType, Optional ByRef errMsg As String = "") As Boolean

        Dim cn As IDbConnection

        If Len(connStr) < 1 Then Return False

        Try


            Select Case providerType

                Case providerType.SqlClient

                    cn = New SqlClient.SqlConnection(connStr)

                Case providerType.OleDb

                    cn = New OleDb.OleDbConnection(connStr)

                Case providerType.Odbc

                    cn = New Odbc.OdbcConnection(connStr)

                Case providerType.Bdp

                    cn = LoadBorlandProviderConnection(connStr)

            End Select

        Catch ex As Exception

            cn = Nothing

            errMsg += ex.Message & vbCrLf & vbCrLf

        End Try

        If Not cn Is Nothing Then

            Try

                cn.Open()

                cn.Close()

                cn.Dispose()

            Catch ex As Exception

                If Not cn.State = ConnectionState.Closed Then cn.Close()

                cn.Dispose()

                errMsg += ex.Message & vbCrLf & vbCrLf

                Return False

            End Try

        Else

            Return False

        End If

        Return True

    End Function

    Public Shared Function LoadBorlandProviderConnection(ByVal connStr As String) As IDbConnection

        Dim bdp As Reflection.Assembly

        Dim cn As Object

        Try

            bdp = Reflection.Assembly.LoadFrom("Borland.Data.Provider.dll")

            cn = bdp.CreateInstance("Borland.Data.Provider.BdpConnection")

            cn.ConnectionString = connStr

            Return cn

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

End Class
