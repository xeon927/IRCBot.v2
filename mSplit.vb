Imports System.Text.RegularExpressions
Module mSplit
    Function getNickname(message As String)
        Return msgRegex("nickname", message)
    End Function
    Function getUsername(message As String)
        Return msgRegex("username", message)
    End Function
    Function getHostname(message As String)
        Return msgRegex("hostname", message)
    End Function
    Function getChannel(message As String)
        Dim chan As String = msgRegex("channel", message)
        If chan.Substring(0, 1) = "#" Then Return chan Else Return getNickname(message)
    End Function
    Function getMessage(message As String)
        Return msgRegex("message", message)
    End Function
    Function msgRegex(output As String, input As String)
        If CanRegex Then
            Dim regexPattern As String = "^:(?<nickname>.+?)!(?<username>.+?)@(?<hostname>.+?)\ PRIVMSG\ (?<channel>.+?)\ :(?<message>.+?)$"
            Dim r As New Regex(regexPattern)
            Dim m As Match = r.Match(input)
            Dim nickname, username, hostname, channel, message As String
            If m.Success Then
                Try
                    nickname = r.Match(input).Result("${nickname}")
                    username = r.Match(input).Result("${username}")
                    hostname = r.Match(input).Result("${hostname}")
                    channel = r.Match(input).Result("${channel}")
                    message = r.Match(input).Result("${message}")
                Catch ex As Exception
                    Console.WriteLine(ex.ToString())
                End Try
            Else
                Return "---Operation Failed---"
                Exit Function
            End If
            If output = "nickname" Then Return nickname
            If output = "username" Then Return username
            If output = "hostname" Then Return hostname
            If output = "channel" Then Return channel
            If output = "message" Then Return message
        Else
            Return "---Cannot Regex---"
            Exit Function
        End If
        Return "---Operation Failed---"
    End Function
End Module
