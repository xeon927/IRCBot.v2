Module modMisc
    Sub Check(message As String)
        If InStr(getMessage(message), String.Format("Goodnight, {0}", nickname)) Then
            mscSleep(getNickname(message), getChannel(message))
        End If
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
End Module
