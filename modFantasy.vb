Imports System.Text.RegularExpressions
Module modFantasy
    Sub Check(message As String)
        If getMessage(message).Substring(0, 1) = "!" Then
            If Regex.IsMatch(getMessage(message), "\d+d\d+", RegexOptions.IgnoreCase) Then fantDiceRoll(getNickname(message), getChannel(message), getMessage(message))
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
End Module
