﻿Module operations
    Sub login()
        If FirstRun Then
            If servPassUse Then sendData(String.Format("PASS {0}", servPass))
            FirstRun = False
            Exit Sub
        End If
        If Not nickSent Then
            sendData(String.Format("NICK {0}", nickname.ToString()))
            nickSent = True
            Exit Sub
        End If
        If firstPing Then
            If Not userSent Then
                sendData(String.Format("USER {0} {1} {1} :{2}", username, "null", realname))
                userSent = True
                Exit Sub
            End If
            If nsUse Then sendNickServ()
            joinChan(channel)
            loggedIn = True
            CanRegex = True
        End If
    End Sub
    Sub sendNickServ()
        sendData(String.Format("PRIVMSG NickServ IDENTIFY {0}", nsPass))
    End Sub

    Sub joinChan(chan As String)
        sendData(String.Format("JOIN {0}", chan))
    End Sub
    Sub partChan(chan As String)
        sendData(String.Format("PART {0}", chan))
    End Sub

    Sub sendNotice(user As String, message As String)
        sendData(String.Format("NOTICE {0} {1}", user, message))
    End Sub
    Sub sendMessage(dest As String, message As String)
        sendData(String.Format("PRIVMSG {0} {1}", dest, message))
    End Sub

    Sub setNick(nick As String)
        nick = removeSpaces(nick)
        sendData(String.Format("NICK {0}", nick))
        nickname = nick
    End Sub

    Sub sendAction(chan As String, action As String)
        sendMessage(chan, String.Format("{0}ACTION {1}{0}", Chr(&H1), action))
    End Sub
    Sub sendPM(nick As String, message As String)
        sendData(String.Format("PRIVMSG {0} :{1}", nick, message))
    End Sub
End Module
