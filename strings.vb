Module strings
    Sub Check(message As String)
        Try
            checkPing(message)
            checkDisconnect(message)
            ignore.check(message)
            If Not isIgnoring Then
                If isChanMsg(message) Then tellHandle.Check(getNickname(message), getChannel(message))
                LastFM.Check(getNickname(message), getChannel(message), getMessage(message))
                YouTube.Check(getNickname(message), getChannel(message), getMessage(message))
                'deviantART.Check(getNickname(message), getChannel(message), getMessage(message))
                Twitter.Check(getNickname(message), getChannel(message), getMessage(message))
                modFantasy.Check(message)
                modHighlight.Check(message)
                modMisc.Check(message)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub
    Sub checkPing(message)
        If Len(message) > 6 And message.Substring(0, 6) = "PING :" Then
            Dim pongMsg(1) As String
            pongMsg = message.Split(":")
            pongMsg(1) = pongMsg(1).TrimEnd(vbCr, vbLf)
            main.sendData("PONG :" + pongMsg(1))
            If Not firstPing Then firstPing = True
        End If
    End Sub
    Sub checkDisconnect(message)
        If Len(message) >= Len("ERROR :Closing Link:") Then
            If message.Substring(0, Len("ERROR :Closing Link:")) = "ERROR :Closing Link:" Then
                CanRegex = False
                loggedIn = False
                firstPing = False
                nickSent = False
                userSent = False
                FirstRun = False

                System.Threading.Thread.Sleep(15000) 'Wait 15s before reconnecting

                servConnect()
                runLoop()
            End If
        End If
    End Sub
End Module
