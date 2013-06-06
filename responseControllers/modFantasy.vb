﻿Imports System.Text.RegularExpressions
Module modFantasy
    Sub Check(message As String)
        If getMessage(message).Substring(0, 1) = "!" Then
            If Regex.IsMatch(getMessage(message), "\d+d\d+", RegexOptions.IgnoreCase) Then fantDiceRoll(getNickname(message), getChannel(message), getMessage(message))
            If Regex.IsMatch(getMessage(message), "!dose\ \d+\ \d+", RegexOptions.IgnoreCase) Then fantGetDose(getNickname(message), getChannel(message), getMessage(message))
            If InStr(getMessage(message), "!8b") Or InStr(getMessage(message), "!8ball") Then fantEightBall(getNickname(message), getChannel(message))
            If InStr(getMessage(message), "!vote") Then fantVote(getNickname(message), getChannel(message), getMessage(message))
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
    Sub fantEightBall(nick As String, chan As String)
        Select numberGen(1, 20)
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
            Console.WriteLine(String.Format("---{0}---", arg))
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
End Module