Module modFantasy
    Sub Check(message As String)
        If getMessage(message).Substring(0, 1) = "!" Then
            Select Case getMessage(message).ToLower()
                'Case "!version" : fantVersion(getNickname(message), getChannel(message))
            End Select
        End If
    End Sub
    'Sub fantVersion(nick As String, chan As String)
    '    sendMessage(chan, String.Format("{0}: I am running version {1} of xeon927's IRC Bot!", nick, version))
    'End Sub
End Module
