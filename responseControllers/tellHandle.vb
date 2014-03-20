Imports System.IO
Imports System.Collections.Generic
Module tellHandle
    Sub cmdLoad(nick As String, chan As String)
        If nick = owner Then
            tellHandle.Load()
            sendMessage(chan, String.Format("{0}: Message database loaded from file", nick))
        Else
            sendMessage(chan, String.Format("{0}: {1}", nick, ownerfail))
        End If
    End Sub
    Sub Load()
        waitingTellsList.Clear()
        If File.Exists(tellfilePath) Then
            waitingTells = File.ReadAllLines(tellfilePath)
            For i As Integer = 1 To waitingTells.Length
                Dim tempArray(3) As String
                tempArray = waitingTells(i - 1).Split(New Char() {"|"}, 4)
                waitingTellsList.Add({tempArray(0), tempArray(1), tempArray(2), tempArray(3)})
            Next
        Else
            waitingTells = {}
        End If
    End Sub
    Sub Save()
        'Delete file and rewrite
        If File.Exists(tellfilePath) Then File.Delete(tellfilePath)
        For i As Integer = 0 To (waitingTellsList.Count - 1)
            File.AppendAllLines(tellfilePath, {String.Format("{0}|{1}|{2}|{3}", waitingTellsList(i)(0), waitingTellsList(i)(1), waitingTellsList(i)(2), waitingTellsList(i)(3))})
        Next
    End Sub
    Sub Add(fromNick As String, fromChan As String, destUser As String, message As String)
        waitingTellsList.Add({destUser, fromChan, DateTime.Now, String.Format("<{0}> {1}", fromNick, message)})
        tellHandle.Save()
        sendMessage(fromChan, String.Format("{0}: I'll pass that along.", fromNick))
    End Sub
    Sub Check(nickname As String, channel As String)
        If waitingTellsList.Count = 0 Then Exit Sub
        For i As Integer = 0 To (waitingTellsList.Count - 1)
            If i < waitingTellsList.Count Then
                If nickname.ToLower() = waitingTellsList(i)(0).ToLower() Then
                    If channel.ToLower() = waitingTellsList(i)(1).ToLower() Then
                        sendMessage(channel, String.Format("{0}: [{1}] {2}", nickname, timeDiff(DateTime.Parse(waitingTellsList(i)(2))), waitingTellsList(i)(3)))
                        waitingTellsList.RemoveAt(i)
                        tellHandle.Save()
                        i = i - 1
                    End If
                End If
            End If
        Next
    End Sub
    Function timeDiff(time As DateTime)
        Dim ts As TimeSpan = DateTime.Now - time
        If ts.TotalDays > 1 Then Return String.Format("{0} days, {1}:{2}:{3} ago", ts.Days, ts.Hours.ToString().PadLeft(2, "0"), ts.Minutes.ToString().PadLeft(2, "0"), ts.Seconds.ToString().PadLeft(2, "0"))
        If ts.TotalHours > 1 Then Return String.Format("{0}:{1}:{2} ago", ts.Hours.ToString().PadLeft(2, "0"), ts.Minutes.ToString().PadLeft(2, "0"), ts.Seconds.ToString().PadLeft(2, "0"))
        If ts.TotalMinutes > 1 Then Return String.Format("{0} minutes, {1} seconds ago", ts.Minutes, ts.Seconds)
        If ts.TotalSeconds > 1 Then Return String.Format("{0} seconds ago", ts.Seconds)
        Return Nothing
    End Function
End Module
