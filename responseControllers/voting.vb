Module voting
    Dim VoteInProgress As Boolean = False
    Dim voteStarter, voteChan, currentVote As String
    Dim countYes, countNo As Integer
    Dim voters As String
    Sub StartVote(fromNick As String, fromChan As String, VoteMessage As String)
        If Not VoteInProgress Then
            VoteInProgress = True
            voteStarter = fromNick
            voteChan = fromChan
            currentVote = VoteMessage
            voters = ""
            countYes = 0
            countNo = 0
        Else
            sendMessage(fromChan, String.Format("{0}: Sorry, a vote is currently running. {1} is asking ""{2}"" in the channel {3}.", voteStarter, voteStarter, currentVote, voteChan))
        End If
    End Sub
    Sub EndVote(fromNick As String, fromChan As String)
        If VoteInProgress And fromNick = voteStarter Then
            sendMessage(voteChan, String.Format("Finishing vote. Question was ""{0}"", asked by {1}", currentVote, voteStarter))
            sendMessage(voteChan, String.Format("Final scores - Yes: {0} | No: {1}", countYes, countNo))
            VoteInProgress = False
        ElseIf Not VoteInProgress Then
            sendMessage(fromChan, String.Format("{0}: There is no vote currently underway", fromNick))
        ElseIf Not fromNick = voteStarter Then
            sendMessage(fromChan, String.Format("{0}: Sorry, only the person who started the vote can finish it", fromNick))
        End If
    End Sub
    Sub VoteStats(fromNick As String, fromChan As String)
        If VoteInProgress = True Then
            sendMessage(fromChan, String.Format("{0}: Current question: {1} asking ""{2}"" in {3}", fromNick, voteStarter, currentVote, voteChan))
            sendMessage(fromChan, String.Format("{0}: Current votes are as follows - Yes: {1} | No: {2}", fromNick, countYes, countNo))
        Else
            sendMessage(fromChan, String.Format("{0}: There is no vote currently underway", fromNick))
        End If
    End Sub
    Sub voteOverride(fromNick As String, fromChan As String, operation As String, argument As String)
        If fromNick = owner Then
            Select Case operation.ToLower()
                Case "stopvote" : EndVote(voteStarter, fromChan)
                Case "setyes" : countYes = argument
                Case "setno" : countNo = argument
            End Select
        Else
            sendMessage(fromChan, String.Format("{0}: {1}", fromNick, ownerfail))
        End If
    End Sub
    Sub vote(fromNick As String, fromChan As String, choice As String)
        If InStr(voters, fromNick) Then
            sendMessage(fromChan, String.Format("{0}: Sorry, but your vote has already been counted. You cannot vote twice.", fromNick))
        Else
            voters = String.Format("{0} {1}", fromNick, voters)
            Select Case choice.ToLower()
                Case "yes" : countYes = countYes + 1
                Case "no" : countNo = countNo + 1
            End Select
        End If
    End Sub
End Module
