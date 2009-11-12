Imports Puzzle.NPersist.Framework.Mapping
Imports Puzzle.NPersist.Framework.Enumerations

Public Class FormattingServices

    Public Shared Function GetClassMapName(ByVal classMap As IClassMap, ByVal seenFromClassMap As IClassMap) As String

        Dim arrNs() As String = Split(classMap.Name, ".")
        Dim arrNs2() As String = Split(seenFromClassMap.Name, ".")
        Dim arrNs3() As String = Split(classMap.Name, ".")
        Dim i As Integer
        Dim j As Integer

        If UBound(arrNs) >= UBound(arrNs2) Then

            For i = 0 To UBound(arrNs2) - 1

                If LCase(arrNs(i)) = LCase(arrNs2(i)) Then

                    For j = 0 To UBound(arrNs3) - 1

                        arrNs3(j) = arrNs3(j + 1)

                    Next

                    If UBound(arrNs3) = 0 Then

                        Return arrNs3(0)

                    End If

                    ReDim Preserve arrNs3(UBound(arrNs3) - 1)

                End If

            Next

        End If

        Return Join(arrNs3, ".")

    End Function


    Public Shared Sub VerifyConnectionParameters(ByVal connStr As String, ByVal providerType As ProviderType, ByVal sourceType As SourceType)

        If Len(connStr) < 1 Then

            Throw New Exception("The connection string is empty")

        End If

        Select Case providerType

            Case providerType.SqlClient

                If Not sourceType = sourceType.MSSqlServer Then

                    Throw New Exception("System.Data.SqlClient provider can only be used with SQL Server databases")

                End If

                If InStr(LCase(connStr), "provider=") > 0 Then

                    Throw New Exception("System.Data.SqlClient provider connection strings should not include any 'Provider=' parameter!")

                End If

                'TODO:
                'How is it with this....?
                'Case ProviderType.OleDb

                '    If Not InStr(LCase(connStr), "provider=") > 0 Then

                '        Throw New Exception("System.Data.OleDb provider connection strings must include a 'Provider=' parameter!")

                '    End If

                'Case ProviderType.Odbc

                '    If Not InStr(LCase(connStr), "provider=") > 0 Then

                '        Throw New Exception("System.Data.OleDb provider connection strings must include a 'Provider=' parameter!")

                '    End If

        End Select

    End Sub



    Public Shared Function CleanUpConnectionString(ByVal connStr As String, ByVal providerType As ProviderType) As String

        Select Case providerType

            Case providerType.SqlClient

                Try

                    If InStr(LCase(connStr), "provider=") > 0 Then

                        Dim arr() As String
                        Dim arr2() As String
                        Dim fixed As String
                        Dim i As Long
                        arr = Split(connStr, ";")

                        For i = 0 To UBound(arr)

                            arr2 = Split(arr(i), "=", 2)

                            If Not LCase(arr2(0)) = "provider" Then

                                fixed += arr(i) & ";"

                            End If

                        Next

                        connStr = fixed

                    End If

                Catch ex As Exception

                    'So what....

                End Try

        End Select

        Return connStr

    End Function

End Class
