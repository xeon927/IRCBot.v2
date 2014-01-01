Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports System.Net
Imports System.IO
Module modFantasy
    Sub Check(message As String)
        If getMessage(message).Substring(0, 1) = "!" Then
            If Regex.IsMatch(getMessage(message), "!\d+d\d+", RegexOptions.IgnoreCase) Then fantDiceRoll(getNickname(message), getChannel(message), getMessage(message))
            If Regex.IsMatch(getMessage(message), "^!dose\ \d+\ \d+", RegexOptions.IgnoreCase) Then fantGetDose(getNickname(message), getChannel(message), getMessage(message))
            If Regex.IsMatch(getMessage(message), "^!tell\ \w+\ .+", RegexOptions.IgnoreCase) Then fantTellAdd(message)
            If Regex.IsMatch(getMessage(message), "^!wa\ .+$", RegexOptions.IgnoreCase) Then fantAlpha(getNickname(message), getChannel(message), getMessage(message))
            If InStr(getMessage(message), "!hug") Then fantHug(message)
            If InStr(getMessage(message), "!8b") Then fantEightBall(getNickname(message), getChannel(message))
            If InStr(getMessage(message), "!vote") Then fantVote(getNickname(message), getChannel(message), getMessage(message))
            If InStr(getMessage(message), "!uptime") Then fantUptime(getNickname(message), getChannel(message))
            If InStr(getMessage(message), "!ping") Then fantPing(getNickname(message), getChannel(message))
            If InStr(getMessage(message), "!choose") Then fantChoose(getNickname(message), getChannel(message), getMessage(message))
            If InStr(getMessage(message), "!isup") Then fantDown(getNickname(message), getChannel(message), getMessage(message))
        End If
    End Sub
    Sub fantDiceRoll(nick As String, chan As String, message As String)
        Dim pattern As String = "(?<rolls>\d+)d(?<max>\d+)"
        Dim rolls As Integer = Regex.Match(message, pattern, RegexOptions.IgnoreCase).Result("${rolls}")
        Dim max As Integer = Regex.Match(message, pattern, RegexOptions.IgnoreCase).Result("${max}")
        Dim resultArray(rolls - 1) As String
        If rolls > diceMaxRolls Or max > diceMaxSides Then
            sendMessage(chan, String.Format("{0}: Sorry, but I can't work with numbers that large. I only support up to {1} rolls at a time, each roll generating numbers 1 - {2}", nick, diceMaxRolls, diceMaxSides))
            Exit Sub
        End If
        If rolls = 0 Or max = 0 Then
            sendMessage(chan, String.Format("{0}: Sorry, but I can't make a diceroll with 0 rolls or chances.", nick))
            Exit Sub
        End If
        If max = 1 Then
            sendMessage(chan, String.Format("{0}: Sorry, but I can't generate a whole number between 1 and 1", nick))
            Exit Sub
        End If
        For i As Integer = 1 To rolls
            resultArray(i - 1) = numberGen(1, max)
        Next
        sendMessage(chan, String.Format("Rolling {0} dice with {1} sides... Results: [{2}]", rolls, max, String.Join(", ", resultArray)))
    End Sub
    Sub fantGetDose(nick As String, chan As String, message As String)
        Dim min As Integer = Regex.Match(message, "!dose\ (?<min>\d+)\ (?<max>\d+)", RegexOptions.IgnoreCase).Result("${min}")
        Dim max As Integer = Regex.Match(message, "!dose\ (?<min>\d+)\ (?<max>\d+)", RegexOptions.IgnoreCase).Result("${max}")
        sendMessage(chan, String.Format("{0}: You should take {1}mg!", nick, numberGen(min, max).ToString()))
    End Sub
    Sub fantTellAdd(message As String)
        Dim fromNick As String = getNickname(message)
        Dim fromChan As String = getChannel(message)
        Dim destUser As String = Regex.Match(message, "!tell\ (?<destination>\w+)\ (?<text>.+)", RegexOptions.IgnoreCase).Result("${destination}")
        Dim tellMessage As String = Regex.Match(message, "!tell\ (?<destination>\w+)\ (?<text>.+)", RegexOptions.IgnoreCase).Result("${text}")
        tellHandle.Add(fromNick, fromChan, destUser, tellMessage)
    End Sub
    Sub fantEightBall(nick As String, chan As String)
        Select Case numberGen(1, 20)
            Case 1 : sendMessage(chan, String.Format("{0}: It is certain.", nick))
            Case 2 : sendMessage(chan, String.Format("{0}: It is decidedly so.", nick))
            Case 3 : sendMessage(chan, String.Format("{0}: Without a doubt.", nick))
            Case 4 : sendMessage(chan, String.Format("{0}: Yes, definitely.", nick))
            Case 5 : sendMessage(chan, String.Format("{0}: You may rely on it.", nick))
            Case 6 : sendMessage(chan, String.Format("{0}: As I see it, yes.", nick))
            Case 7 : sendMessage(chan, String.Format("{0}: Most likely.", nick))
            Case 8 : sendMessage(chan, String.Format("{0}: Outlook good.", nick))
            Case 9 : sendMessage(chan, String.Format("{0}: Yes.", nick))
            Case 10 : sendMessage(chan, String.Format("{0}: Signs point to yes.", nick))
            Case 11 : sendMessage(chan, String.Format("{0}: Reply hazy. Try again.", nick))
            Case 12 : sendMessage(chan, String.Format("{0}: Ask again later.", nick))
            Case 13 : sendMessage(chan, String.Format("{0}: Better not tell you now.", nick))
            Case 14 : sendMessage(chan, String.Format("{0}: Cannot predict now.", nick))
            Case 15 : sendMessage(chan, String.Format("{0}: Concentrate and ask again.", nick))
            Case 16 : sendMessage(chan, String.Format("{0}: Don't count on it.", nick))
            Case 17 : sendMessage(chan, String.Format("{0}: My reply is no.", nick))
            Case 18 : sendMessage(chan, String.Format("{0}: My sources say no.", nick))
            Case 19 : sendMessage(chan, String.Format("{0}: Outlook not so good.", nick))
            Case 20 : sendMessage(chan, String.Format("{0}: Very doubtful.", nick))
        End Select
    End Sub
    Sub fantVote(nick As String, chan As String, message As String)
        If removeSpaces(message) = "!vote" Then
            Exit Sub
        End If
        If InStr(message.ToLower(), "start") Then
            Dim voteString As String = Regex.Match(message, "!vote\ start\ ""(?<voteString>.+)""", RegexOptions.IgnoreCase).Result("${voteString}")
            voting.StartVote(nick, chan, voteString)
        ElseIf InStr(message.ToLower(), "override") Then
            If InStr(message.ToLower(), "stopvote") Then
                voting.voteOverride(nick, chan, "stopvote", "")
                Exit Sub
            End If
            Dim op As String = Regex.Match(message, "!vote\ override\ (?<op>\w+)\ (?<arg>\d+)", RegexOptions.IgnoreCase).Result("${op}")
            Dim arg As String = Regex.Match(message, "!vote\ override\ (?<op>\w+)\ (?<arg>\d+)", RegexOptions.IgnoreCase).Result("${arg}")
            voting.voteOverride(nick, chan, op, arg)
            Exit Sub
        ElseIf InStr(message.ToLower(), "stats") Then
            voting.VoteStats(nick, chan)
            Exit Sub
        ElseIf InStr(message.ToLower(), "finish") Then
            voting.EndVote(nick, chan)
            Exit Sub
        Else
            Dim arg As String = Regex.Match(message, "!vote\ (?<arg>\w+)", RegexOptions.IgnoreCase).Result("${arg}")
            Select Case arg.ToLower()
                Case "yay" : voting.vote(nick, chan, "yes")
                Case "yeah" : voting.vote(nick, chan, "yes")
                Case "yes" : voting.vote(nick, chan, "yes")
                Case "nay" : voting.vote(nick, chan, "no")
                Case "nope" : voting.vote(nick, chan, "no")
                Case "no" : voting.vote(nick, chan, "no")
            End Select
        End If
    End Sub
    Sub fantUptime(nick As String, chan As String)
        Dim days, hours, minutes, seconds As String
        If InStr(Environment.OSVersion.ToString(), "NT") Then
            Dim systemUptime As New PerformanceCounter("System", "System Up Time")
            systemUptime.NextValue()
            Dim TS As TimeSpan = TimeSpan.FromSeconds(systemUptime.NextValue())

            'Set all values before sending message
            days = TS.Days
            hours = TS.Hours
            minutes = TS.Minutes
            seconds = TS.Seconds
        ElseIf InStr(Environment.OSVersion.ToString(), "Unix") Then
            Dim input As String = File.ReadAllText("/proc/uptime").Trim(vbCr, vbLf)
            Dim regexPattern As String = "(?<uptime>\-?\d+\.\d+)\ (?<idletime>\-?\d+\.\d+)"
            Dim bootSecFromNow As Double = Regex.Match(input, regexPattern).Result("${uptime}")
            Dim TS As TimeSpan = TimeSpan.FromSeconds(bootSecFromNow)

            'Set all values before sending message
            days = TS.Days
            hours = TS.Hours
            minutes = TS.Minutes
            seconds = TS.Seconds
        Else
            sendMessage(chan, String.Format("{0}: Sorry. My host doesn't run a system that I know how to get uptime from", nick))
            Exit Sub
        End If
        sendMessage(chan, String.Format("{0}: My host has been running for {1} days, {2}:{3}:{4}", nick, days, hours.PadLeft(2, "0"), minutes.PadLeft(2, "0"), seconds.PadLeft(2, "0")))
    End Sub
    Sub fantPing(nick As String, chan As String)
        sendMessage(chan, String.Format("{0}: Pong! :D", nick))
    End Sub
    Sub fantHug(message As String)
        If Regex.IsMatch(getMessage(message), "!hug\ \w+", RegexOptions.IgnoreCase) Then
            Dim destNick As String = Regex.Match(getMessage(message), "!hug\ (?<destNick>\w+)", RegexOptions.IgnoreCase).Result("${destNick}")
            mscHug(destNick, getChannel(message))
        Else
            mscHug(getNickname(message), getChannel(message))
        End If
    End Sub
    Sub fantChoose(nick As String, chan As String, message As String)
        If message.Length < 8 Then
            sendMessage(chan, String.Format("{0}: Sorry, you didn't give me any options to choose from.", nick))
            Exit Sub
        End If
        Dim chooseArray As String()
        message = message.Remove(0, 8)
        chooseArray = Regex.Split(message, ", ")
        If chooseArray.Length = 1 Then
            sendMessage(chan, String.Format("{0}: Sorry, but you only gave me one option to pick from.", nick))
            Exit Sub
        End If
        sendMessage(chan, String.Format("{0}: {1}", nick, chooseArray(numberGen(0, chooseArray.Length - 1))))
    End Sub
    Sub fantAlpha(nick As String, chan As String, message As String)
        Try
            Dim query As String = Regex.Match(message, "!wa\ (?<query>.+?)$", RegexOptions.IgnoreCase).Result("${query}")
            Dim results As New List(Of String)
            Dim tempResult As New List(Of String)
            Dim xDoc As XDocument = XDocument.Load(String.Format("http://api.wolframalpha.com/v2/query?format=plaintext&appid={0}&input={1}", waAppID, Uri.EscapeDataString(query)))
            If xDoc.Element("queryresult").Attribute("success").Value = "true" Then
                For Each pod As XElement In xDoc.<queryresult>.<pod>
                    For Each subpod As XElement In pod.<subpod>
                        If InStr(subpod.@title, "Result") Or InStr(pod.@title, "Result") Or pod.@primary = "true" Then tempResult.Add(subpod.<plaintext>.Value)
                    Next
                    If tempResult.Count > 0 Then results.Add(String.Format("{0}: {1}", pod.@title, String.Join(" || ", tempResult)))
                    tempResult.Clear()
                Next
                If results.Count > 0 Then
                    For Each result In results
                        sendMessage(chan, String.Format("{0}: {1}", nick, result.Replace(vbLf, " ")))
                    Next
                Else
                    sendMessage(chan, String.Format("{0}: No results for query: {1}", nick, query))
                End If
            Else
                sendMessage(chan, String.Format("{0}: No results for query: {1}", nick, query))
            End If
        Catch ex As Exception
            sendMessage(chan, String.Format("{0}: Sorry, but something went wrong. Please check back later.", nick))
            sendNotice(owner, "Something went wrong with WolframAlpha. Please check botlogs as soon as possible.")
            logging.append(ex.ToString())
#If DEBUG Then
            Console.WriteLine(ex.ToString())
#End If
        End Try
    End Sub
    Sub fantDown(nick As String, chan As String, message As String)
        If Regex.IsMatch(message, "^!isup\ (?<host>.+)$") Then
            If Regex.IsMatch(message, "\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}") Then
                Dim pingDest As String = Regex.Match(message, "(?<IP>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})").Result("${IP}")
                If IPAddress.TryParse(pingDest, System.Net.IPAddress.Any) Then
                    Dim pingSender As New NetworkInformation.Ping
                    Select Case pingSender.Send(pingDest).Status
                        Case NetworkInformation.IPStatus.Success : sendMessage(chan, String.Format("{0}: {1} - Host appears to be up", nick, pingDest))
                        Case Else : sendMessage(chan, String.Format("{0}: {1} - Host appears to be down", nick, pingDest))
                    End Select
                End If
            Else
                Dim URL As String = Regex.Match(message, "^!isup\ (?<host>.+)$").Result("${host}")
                If Not URL.StartsWith("http://") And Not URL.StartsWith("https://") Then URL = "http://" + URL
                Try
                    Dim req As WebResponse = WebRequest.Create(URL).GetResponse()
                    req.Close()
                    sendMessage(chan, String.Format("{0}: {1} - Host appears to be up", nick, URL))
                Catch ex As Exception
                    sendMessage(chan, String.Format("{0}: {1} - Host appears to be down", nick, URL))
                End Try
            End If
        End If
    End Sub
End Module
