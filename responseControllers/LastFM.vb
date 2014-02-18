Imports System.Text.RegularExpressions
Imports System.IO
Module LastFM
    'Database file is stored in format:
    '  IRCNick|LFMUsername

    'Database is loaded then accessed by accessing dictionary with key of IRC nickname:
    '  LFMUsers(IRCNick_string)

    Sub Load()
        LFMUsers.Clear()
        If File.Exists(LFMPath) Then
            Dim LFMLoadArray() As String = File.ReadAllLines(LFMPath)
            For Each line As String In LFMLoadArray
                Dim splitArray() As String = line.Split(New Char() {vbTab}, 2)
                LFMUsers.Add(splitArray(0), splitArray(1))
            Next
        End If
    End Sub
    Sub Save()
        File.Delete(LFMPath)
        For Each user In LFMUsers
            File.AppendAllLines(LFMPath, {String.Format("{0}{1}{2}", user.Key, vbTab, user.Value)})
        Next
    End Sub
    Sub Check(nick As String, chan As String, message As String)
        'Regex Patterns:
        '!np              - ^!np$
        '!np <IRCUser>    - ^!np\ (?<nick>.+?)$
        '!npadd <LFMUser> - ^!npadd\ (?<user>.+?)$
        '!npdel           - ^!npdel$
        '!npchk <LFMUser> - ^!npchk\ (?<user>.+?)$

        'Get Now Playing Data for sender
        If Regex.IsMatch(message, "^!np$") Then getNPData(nick, chan, nick)

        'Get Now Playing Data for another user (registered with bot)
        If Regex.IsMatch(message, "^!np\ (?<nick>.+?)$") Then getNPData(nick, chan, Regex.Match(message, "^!np\ (?<nick>.+?)$").Result("${nick}"))

        'Allow users to associate a Last.FM account with their IRC nickname
        If Regex.IsMatch(message, "^!npadd\ (?<user>.+?)$") Then addUser(nick, chan, Regex.Match(message, "^!npadd\ (?<user>.+?)$").Result("${user}"))

        'Allow users to disassociate a Last.FM account with their IRC nickname
        If Regex.IsMatch(message, "^!npdel$") Then delUser(nick, chan)

        'Allow users to check what nicknames are associated with a given Last.FM username
        If Regex.IsMatch(message, "^!npchk\ (?<user>.+?)$") Then chkUser(nick, chan, Regex.Match(message, "^!npchk\ (?<user>.+?)$").Result("${user}"))

        'Similar to above - but Owner-Only force options
        If Regex.IsMatch(message, "^!npaddforce\ (?<nick>.+?)\ (?<user>.+?)$") Then addUserForce(Regex.Match(message, "^!npaddforce\ (?<nick>.+?)\ (?<user>.+?)$").Result("${nick}"), chan, Regex.Match(message, "^!npaddforce\ (?<nick>.+?)\ (?<user>.+?)$").Result("${user}"), nick)
        If Regex.IsMatch(message, "^!npdelforce\ (?<nick>.+?)$") Then delUserForce(Regex.Match(message, "^!npdelforce\ (?<nick>.+?)$").Result("${nick}"), chan, nick)
    End Sub

    Sub addUser(nick As String, chan As String, user As String)
        If LFMUsers.ContainsKey(nick.ToLower()) Then
            sendMessage(chan, String.Format("{0}: Cannot add username. IRC nickname already associated with Last.FM account {1}. To delete association, use ""!npdel""", nick, LFMUsers(nick)))
        Else
            LFMUsers.Add(nick.ToLower(), user)
            Save()
            sendMessage(chan, String.Format("{0}: Username added to database", nick))
        End If
    End Sub
    Sub addUserForce(nick As String, chan As String, user As String, origin As String)
        If origin = owner Then
            If LFMUsers.ContainsKey(nick.ToLower()) Then
                sendMessage(chan, String.Format("{0}: Nickname already associated to account {1}", origin))
            Else
                LFMUsers.Add(nick.ToLower(), user)
                Save()
                sendMessage(chan, String.Format("{0}: Username added to database", origin))
            End If
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub delUser(nick As String, chan As String)
        If LFMUsers.ContainsKey(nick.ToLower()) Then
            LFMUsers.Remove(nick.ToLower())
            Save()
            sendMessage(chan, String.Format("{0}: Username deleted from database", nick))
        Else
            sendMessage(chan, String.Format("{0}: Cannot delete username. No Last.FM account associated with your IRC nickname", nick))
        End If
    End Sub
    Sub delUserForce(nick As String, chan As String, origin As String)
        If origin = owner Then
            If LFMUsers.ContainsKey(nick.ToLower()) Then
                LFMUsers.Remove(nick.ToLower())
                Save()
                sendMessage(chan, String.Format("{0}: Username deleted from database", origin))
            Else
                sendMessage(chan, String.Format("{0}: Nickname not associated to account", origin))
            End If
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub

    Sub chkUser(nick As String, chan As String, target As String)
        Dim associatedAccounts As New List(Of String)
        For Each user In LFMUsers
            If user.Value = target.ToLower() Then associatedAccounts.Add(user.Key)
        Next
        If associatedAccounts.Count = 0 Then
            sendMessage(chan, String.Format("{0}: User {1} is not associated with any IRC nicknames", nick, target))
        Else
            sendMessage(chan, String.Format("{0}: User {1} is associated with the following nickname(s): {2}", nick, target, String.Join(", ", associatedAccounts)))
        End If
    End Sub
    Sub getNPData(nick As String, chan As String, target As String)
        If LFMUsers.ContainsKey(target.ToLower()) Then
            Try
                Dim xDoc As XDocument = XDocument.Load(String.Format("http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&limit=1&user={0}&api_key={1}", Uri.EscapeDataString(LFMUsers(target)), "56283dc1dd302d0400bdbcd3e03ddddd"))
                If xDoc.<lfm>.@status = "ok" Then
                    If xDoc.<lfm>.<recenttracks>.<track>.First.HasAttributes And xDoc.<lfm>.<recenttracks>.<track>.First.@nowplaying = "true" Then
                        sendMessage(chan, String.Format("{0} is listening to ""{1} - {2}""", target, xDoc.<lfm>.<recenttracks>.<track>.First.<name>.Value, xDoc.<lfm>.<recenttracks>.<track>.First.<artist>.Value))
                    Else
                        sendMessage(chan, String.Format("{0} was last listening to ""{1} - {2}"" on {3} UTC", target, xDoc.<lfm>.<recenttracks>.<track>.First.<name>.Value, xDoc.<lfm>.<recenttracks>.<track>.First.<artist>.Value, xDoc.<lfm>.<recenttracks>.<track>.First.<date>.Value))
                    End If
                End If
            Catch ex As Exception
                sendMessage(chan, String.Format("{0}: Sorry, but something went wrong. Please check back later.", nick))
                sendNotice(owner, "Something went wrong with Last.FM. Please check botlogs as soon as possible.")
                logging.append(ex.ToString())
#If DEBUG Then
                Console.WriteLine(ex.ToString())
#End If
            End Try
        Else
            If nick = target Then
                sendMessage(chan, String.Format("{0}: Your nickname is not associated with a Last.FM account. Use ""!npadd <username>"" (where ""username"" is your Last.FM username) to associate your nickname.", nick))
            Else
                sendMessage(chan, String.Format("{0}: {1} is not associated with a Last.FM account.", nick, target))
            End If
        End If
    End Sub
End Module
