Imports System.IO
Imports System.Collections.Generic
Module tellHandle
    Sub cmdLoad(nick As String, chan As String)
        If nick = owner Then
            tellHandle.Load()
            sendMessage(chan, String.Format("{0}: Message database loaded from file"))
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub Load()
        If File.Exists(tellfilePath) Then
            waitingTells = File.ReadAllLines(tellfilePath)
        Else
            waitingTells = {}
        End If
        For i As Integer = 1 To waitingTells.Length
            Dim tempArray(2) As String
            tempArray = waitingTells(i - 1).Split(New Char() {"|"}, 3)
            waitingTellsList.Add({tempArray(0), tempArray(1), tempArray(2)})
        Next
    End Sub
    Sub Save()
        'Delete file and rewrite
        If File.Exists(tellfilePath) Then File.Delete(tellfilePath)
        For i As Integer = 1 To waitingTellsList.Count
            File.WriteAllLines(tellfilePath, {String.Format("{0}|{1}|{2}", waitingTellsList(i - 1)(0), waitingTellsList(i - 1)(1), waitingTellsList(i - 1)(2))})
        Next
    End Sub
    Sub Add(fromNick As String, fromChan As String, destUser As String, message As String)
        waitingTellsList.Add({destUser, fromChan, String.Format("<{0}> {1}", fromNick, message)})
        tellHandle.Save()
        sendMessage(fromChan, String.Format("{0}: I'll pass that along.", fromNick))
    End Sub
    Sub Check(nickname As String, channel As String)
        If waitingTellsList.Count = 0 Then Exit Sub
        For i As Integer = 1 To waitingTellsList.Count
            If nickname = waitingTellsList(i - 1)(0) Then
                If channel = waitingTellsList(i - 1)(1) Then
                    sendMessage(channel, String.Format("{0}: {1}", nickname, waitingTellsList(i - 1)(2)))
                    waitingTellsList.RemoveAt(i - 1)
                    tellHandle.Save()
                End If
            End If
        Next
    End Sub
End Module
