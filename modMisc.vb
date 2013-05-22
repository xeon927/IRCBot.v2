Module modMisc
    Sub Check(message As String)
        If InStr(getMessage(message), String.Format("Goodnight, {0}", nickname)) Then mscSleep(getNickname(message), getChannel(message))
        If InStr(getMessage(message), String.Format("hugs {0}", nickname)) Then mscHug(getNickname(message), getChannel(message), getMessage(message))
    End Sub
    Sub mscSleep(nick As String, chan As String)
        If nick = owner Then
            sendNotice(owner, String.Format("Goodnight, {0}", owner))
            sendData("QUIT :Shutting Down")
            System.Threading.Thread.Sleep(1000)
            End
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub mscHug(nick As String, chan As String, message As String)
        If nick = owner Then
            Select Case numberGen(1, 3)
                Case 1 : sendAction(chan, String.Format("snuggles up to {0}", nick))
                Case 2 : sendAction(chan, String.Format("cuddles {0}", nick))
                Case 3 : sendAction(chan, String.Format("hugs {0} in return", nick))
                Case Else
                    sendAction(chan, String.Format("hugs {0}", nick))
                    sendNotice(owner, "Something went wrong with mscHug... :(")
            End Select
        End If
    End Sub
End Module
