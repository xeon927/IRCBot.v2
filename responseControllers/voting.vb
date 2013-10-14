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
            sendNotice(fromNick, String.Format("Vote started on {0}: ""{1}""", fromChan, VoteMessage))
        Else
            sendNotice(fromNick, String.Format("Sorry, a vote is currently running. {0} is asking ""{1}"" in the channel {2}.", voteStarter, currentVote, voteChan))
        End If
    End Sub
    Sub EndVote(fromNick As String, fromChan As String)
        If VoteInProgress And fromNick = voteStarter Then
            sendMessage(voteChan, String.Format("Finishing vote. Question was ""{0}"", asked by {1}", currentVote, voteStarter))
            sendMessage(voteChan, String.Format("Final scores - Yes: {0} | No: {1}", countYes, countNo))
            VoteInProgress = False
        ElseIf Not VoteInProgress Then
            sendNotice(fromNick, "There is no vote currently underway")
        ElseIf Not fromNick = voteStarter Then
            sendNotice(fromNick, "Sorry, only the person who started the vote can finish it")
        End If
    End Sub
    Sub VoteStats(fromNick As String, fromChan As String)
        If VoteInProgress = True Then
            sendNotice(fromNick, String.Format("Current question: {0} asking ""{1}"" in {2}", voteStarter, currentVote, voteChan))
            sendNotice(fromNick, String.Format("Current votes are as follows - Yes: {0} | No: {1}", countYes, countNo))
        Else
            sendNotice(fromNick, "There is no vote currently underway")
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
            sendNotice(fromNick, String.Format("Sorry, but your vote has already been counted. You cannot vote twice."))
        Else
            voters = String.Format("{0} {1}", fromNick, voters)
            Select Case choice.ToLower()
                Case "yes" : countYes = countYes + 1
                Case "no" : countNo = countNo + 1
            End Select
            sendNotice(fromNick, String.Format("Thanks for voting! You voted: {0}", choice))
        End If
    End Sub
End Module
