Imports System.Text.RegularExpressions
Module modHighlight
    Sub Check(message As String)
        Dim regexPattern As String = String.Format("{0}: (?<instruction>\w+)", nickname)
        Dim regexArgsPattern As String = String.Format("{0}: (?<instruction>\w+) (?<argument>[A-Za-z0-9#]+)", nickname)
        'With Arguments
        If Regex.IsMatch(getMessage(message), regexArgsPattern, RegexOptions.IgnoreCase) Then
            Dim fromNick, fromChan, inst, arg As String
            fromNick = getNickname(message)
            fromChan = getChannel(message)
            inst = Regex.Match(getMessage(message), regexArgsPattern, RegexOptions.IgnoreCase).Result("${instruction}")
            arg = Regex.Match(getMessage(message), regexArgsPattern, RegexOptions.IgnoreCase).Result("${argument}")
            Select Case inst.ToLower()
                Case "getvar" : hltGetVar(fromNick, fromChan, arg)
                Case "changenick" : hltChangeNick(fromNick, fromChan, arg)
                Case "changeowner" : hltChangeOwner(fromNick, fromChan, arg)
                Case "joinchan" : hltJoinChan(fromNick, fromChan, arg)
                Case "partchan" : hltPartChan(fromNick, fromChan, arg)
                Case "help" : hltHelp(fromNick, fromChan, arg)
            End Select
            Exit Sub
        End If
        'Without Arguments
        If Regex.IsMatch(getMessage(message), regexPattern, RegexOptions.IgnoreCase) Then
            Dim fromNick, fromChan, inst As String
            fromNick = getNickname(message)
            fromChan = getChannel(message)
            inst = Regex.Match(getMessage(message), regexPattern, RegexOptions.IgnoreCase).Result("${instruction}")
            Select Case inst.ToLower()
                Case "ownerinfo" : hltGetOwner(fromNick, fromChan)
                Case "version" : hltVersion(fromNick, fromChan)
                Case "identify" : hltNickServ(fromNick, fromChan)
                Case "help" : hltHelp(fromNick, fromChan)
                Case "loadtells" : tellHandle.cmdLoad(fromNick, fromChan)
            End Select
            Exit Sub
        End If
    End Sub
    Sub hltGetOwner(nick As String, chan As String)
        sendMessage(chan, String.Format("{0}: {1} is my owner!", nick, owner))
    End Sub
    Sub hltVersion(nick As String, chan As String)
        sendMessage(chan, String.Format("{0}: I am running version {1} of xeon927's IRC Bot!", nick, version))
    End Sub
    Sub hltNickServ(nick As String, chan As String)
        If nick = owner Then
            sendNickServ()
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub

    Sub hltGetVar(nick As String, chan As String, argument As String)
        If nick = owner Then
            Select Case argument.ToLower()
                Case "settingsfile" : sendMessage(chan, settingsFile)
                Case "dicemaxrolls" : sendMessage(chan, diceMaxRolls)
                Case "dicemaxsides" : sendMessage(chan, diceMaxSides)
            End Select
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub hltChangeNick(nick As String, chan As String, argument As String)
        If nick = owner Then
            setNick(argument)
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub hltChangeOwner(nick As String, chan As String, argument As String)
        If nick = owner Then
            owner = removeSpaces(argument)
            sendNotice(owner, "You are my new owner!")
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub hltJoinChan(nick As String, chan As String, argument As String)
        If nick = owner Then
            joinChan(argument)
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub hltPartChan(nick As String, chan As String, argument As String)
        If nick = owner Then
            partChan(argument)
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub

    Sub hltHelp(nick As String, chan As String)
        help.showHelp(nick, chan)
    End Sub
    Sub hltHelp(nick As String, chan As String, argument As String)
        help.getHelp(nick, chan, argument)
    End Sub
End Module
