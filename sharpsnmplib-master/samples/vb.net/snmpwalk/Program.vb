﻿'
' Created by SharpDevelop.
' User: Administrator
' Date: 2010/4/25
' Time: 14:14
'
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports Lextm.SharpSnmpLib
Imports Lextm.SharpSnmpLib.Security
Imports Lextm.SharpSnmpLib.Messaging
Imports Mono.Options
Imports System.Net
Imports System.Net.Sockets

Module Program
    Sub Main(ByVal args As String())
        Dim community As String = "public"
        Dim dump As Boolean = True
        Dim showHelp__1 As Boolean = False
        Dim showVersion As Boolean = False
        Dim version As VersionCode = VersionCode.V1
        Dim timeout As Integer = 1000
        Dim retry As Integer = 0
        Dim maxRepetitions As Integer = 10
        Dim level As Levels = Levels.Reportable
        Dim user As String = String.Empty
        Dim authentication As String = String.Empty
        Dim authPhrase As String = String.Empty
        Dim privacy As String = String.Empty
        Dim privPhrase As String = String.Empty
        Dim mode As WalkMode = WalkMode.WithinSubtree

        Dim p As OptionSet = New OptionSet().Add("c:", "Community name, (default is public)", Sub(v As String)
                                                                                                  If v IsNot Nothing Then
                                                                                                      community = v
                                                                                                  End If
                                                                                              End Sub) _
                                            .Add("l:", "Security level, (default is noAuthNoPriv)", Sub(v As String)
                                                                                                        If v.ToUpperInvariant() = "NOAUTHNOPRIV" Then
                                                                                                            level = Levels.Reportable
                                                                                                        ElseIf v.ToUpperInvariant() = "AUTHNOPRIV" Then
                                                                                                            level = Levels.Authentication Or Levels.Reportable
                                                                                                        ElseIf v.ToUpperInvariant() = "AUTHPRIV" Then
                                                                                                            level = Levels.Authentication Or Levels.Privacy Or Levels.Reportable
                                                                                                        Else
                                                                                                            Throw New ArgumentException("no such security mode: " & v)
                                                                                                        End If
                                                                                                    End Sub) _
                                            .Add("a:", "Authentication method (MD5 or SHA)", Sub(v As String)
                                                                                                 authentication = v
                                                                                             End Sub) _
                                            .Add("A:", "Authentication passphrase", Sub(v As String)
                                                                                        authPhrase = v
                                                                                    End Sub) _
                                            .Add("x:", "Privacy method", Sub(v As String)
                                                                             privacy = v
                                                                         End Sub) _
                                            .Add("X:", "Privacy passphrase", Sub(v As String)
                                                                                 privPhrase = v
                                                                             End Sub) _
                                            .Add("u:", "Security name", Sub(v As String)
                                                                            user = v
                                                                        End Sub) _
                                            .Add("h|?|help", "Print this help information.", Sub(v As String)
                                                                                                 showHelp__1 = v IsNot Nothing
                                                                                             End Sub) _
                                            .Add("V", "Display version number of this application.", Sub(v As String)
                                                                                                         showVersion = v IsNot Nothing
                                                                                                     End Sub) _
                                            .Add("d", "Display message dump", Sub(v As String)
                                                                                  dump = True
                                                                              End Sub) _
                                            .Add("t:", "Timeout value (unit is second).", Sub(v As String)
                                                                                              timeout = Integer.Parse(v) * 1000
                                                                                          End Sub) _
                                            .Add("r:", "Retry count (default is 0)", Sub(v As String)
                                                                                         retry = Integer.Parse(v)
                                                                                     End Sub) _
                                            .Add("v|version:", "SNMP version (1, 2, and 3 are currently supported)", Sub(v As String)
                                                                                                                         Select Case Integer.Parse(v)
                                                                                                                             Case 1
                                                                                                                                 version = VersionCode.V1
                                                                                                                                 Exit Select
                                                                                                                             Case 2
                                                                                                                                 version = VersionCode.V2
                                                                                                                                 Exit Select
                                                                                                                             Case 3
                                                                                                                                 version = VersionCode.V3
                                                                                                                                 Exit Select
                                                                                                                             Case Else
                                                                                                                                 Throw New ArgumentException("no such version: " & v)
                                                                                                                         End Select
                                                                                                                     End Sub) _
                                            .Add("m|mode:", "WALK mode (subtree, all are supported)", Sub(v As String)
                                                                                                          If v = "subtree" Then
                                                                                                              mode = WalkMode.WithinSubtree
                                                                                                          ElseIf v = "all" Then
                                                                                                              mode = WalkMode.[Default]
                                                                                                          Else
                                                                                                              Throw New ArgumentException("unknown argument: " & v)
                                                                                                          End If
                                                                                                      End Sub) _
                                            .Add("Cr:", "Max-repetitions (default is 10)", Sub(v As String)
                                                                                               maxRepetitions = Integer.Parse(v)
                                                                                           End Sub)
        If args.Length = 0 Then
            ShowHelp(p)
            Return
        End If

        Dim extra As List(Of String)
        Try
            extra = p.Parse(args)
        Catch ex As OptionException
            Console.WriteLine(ex.Message)
            Return
        End Try

        If showHelp__1 Then
            ShowHelp(p)
            Return
        End If

        If extra.Count < 1 OrElse extra.Count > 2 Then
            Console.WriteLine("invalid variable number: " & extra.Count)
            Return
        End If

        If showVersion Then
            Console.WriteLine(Reflection.Assembly.GetExecutingAssembly().GetName().Version)
            Return
        End If

        Dim ip As IPAddress
        Dim parsed As Boolean = IPAddress.TryParse(extra(0), ip)
        If Not parsed Then
            For Each address As IPAddress In Dns.GetHostAddresses(extra(0))
                If address.AddressFamily <> AddressFamily.InterNetwork Then
                    Continue For
                End If

                ip = address
                Exit For
            Next

            If ip Is Nothing Then
                Console.WriteLine("invalid host or wrong IP address found: " & extra(0))
                Return
            End If
        End If

        Try
            Dim test As ObjectIdentifier = If(extra.Count = 1, New ObjectIdentifier("1.3.6.1.2.1"), New ObjectIdentifier(extra(1)))
            Dim result As IList(Of Variable) = New List(Of Variable)()
            Dim receiver As New IPEndPoint(ip, 161)
            If version = VersionCode.V1 Then
                Messenger.Walk(version, receiver, New OctetString(community), test, result, timeout, _
                 mode)
            ElseIf version = VersionCode.V2 Then
                Messenger.BulkWalk(version, receiver, New OctetString(community), test, result, timeout, _
                 maxRepetitions, mode, Nothing, Nothing)
            Else
                If String.IsNullOrEmpty(user) Then
                    Console.WriteLine("User name need to be specified for v3.")
                    Return
                End If

                Dim auth As IAuthenticationProvider
                If (level And Levels.Authentication) = Levels.Authentication Then
                    auth = GetAuthenticationProviderByName(authentication, authPhrase)
                Else
                    auth = DefaultAuthenticationProvider.Instance
                End If

                Dim priv As IPrivacyProvider
                If (level And Levels.Privacy) = Levels.Privacy Then
                    priv = New DESPrivacyProvider(New OctetString(privPhrase), auth)
                Else
                    priv = New DefaultPrivacyProvider(auth)
                End If

                Dim report As ReportMessage = Messenger.GetNextDiscovery(SnmpType.GetBulkRequestPdu).GetResponse(timeout, receiver)

                Messenger.BulkWalk(version, receiver, New OctetString(user), test, result, timeout, _
                 maxRepetitions, mode, priv, report)
            End If
            For Each variable As Variable In result
                Console.WriteLine(variable)
            Next
        Catch ex As SnmpException
            Console.WriteLine(ex)
        Catch ex As SocketException
            Console.WriteLine(ex)
        End Try
    End Sub

    Private Function GetAuthenticationProviderByName(ByVal authentication As String, ByVal phrase As String) As IAuthenticationProvider
        If authentication.ToUpperInvariant() = "MD5" Then
            Return New MD5AuthenticationProvider(New OctetString(phrase))
        End If

        If authentication.ToUpperInvariant() = "SHA" Then
            Return New SHA1AuthenticationProvider(New OctetString(phrase))
        End If

        Throw New ArgumentException("unknown name", "authentication")
    End Function

    Private Sub ShowHelp(ByRef optionSet As OptionSet)
        Console.WriteLine("#SNMP is available at http://sharpsnmplib.codeplex.com")
        Console.WriteLine("snmpwalk [Options] IP-address|host-name [OID]")
        Console.WriteLine("Options:")
        optionSet.WriteOptionDescriptions(Console.Out)
    End Sub
End Module