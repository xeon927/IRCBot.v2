﻿Module modMisc
    Sub Check(message As String)
        If InStr(getMessage(message).ToLower(), String.Format("goodnight, {0}", nickname.ToLower())) Then mscSleep(getNickname(message), getChannel(message))
        If InStr(getMessage(message), "hugs") Or _
            InStr(getMessage(message), "cuddles") Or _
            InStr(getMessage(message), "snuggles") Then
            If InStr(getMessage(message), nickname) Then mscHug(getNickname(message), getChannel(message))
        End If
        If InStr(getMessage(message), "<3") Then
            If InStr(getMessage(message), nickname) Then
                mscLove(getNickname(message), getChannel(message))
            End If
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
    Sub mscHug(nick As String, chan As String)
        If nick = owner Then
            Select Case numberGen(1, 3)
                Case 1 : sendAction(chan, String.Format("holds on to {0}, not letting go", nick))
                Case 2 : sendAction(chan, String.Format("grips {0} tightly", nick))
                Case 3 : sendAction(chan, String.Format("softly nuzzles {0}", nick))
            End Select
        Else
            Select Case numberGen(1, 3)
                Case 1 : sendAction(chan, String.Format("snuggles up to {0}", nick))
                Case 2 : sendAction(chan, String.Format("cuddles {0}", nick))
                Case 3 : sendAction(chan, String.Format("hugs {0} in return", nick))
            End Select
        End If
    End Sub
    Sub mscLove(nick As String, chan As String)
        If nick = owner Then
            sendAction(chan, String.Format("tackle-hugs {0}", nick))
            sendMessage(chan, String.Format("{0}: I love you!", nick))
        Else
            sendMessage(chan, String.Format("{0}: <3", nick))
        End If
    End Sub
End Module
