Module help
    Sub showHelp(fromNick As String, fromChan As String)
        If fromChan.Substring(0, 1) = "#" Then
            sendMessage(fromChan, String.Format("{0}: Sorry, I don't give help in channels, due to possible spam. Please PM me instead.", fromNick))
        Else
            'TODO: Sort this crap into alphabetical order
            sendPM(fromNick, "Please specify an item for help to be provided.")
            sendPM(fromNick, String.Format("(USAGE: ""{0}: HELP ITEM"", where ITEM is the item you wish to get help for)", main.nickname))
            sendPM(fromNick, "I support the following items:")
            sendPM(fromNick, "  OWNERINFO")
            sendPM(fromNick, "  VERSION")
            sendPM(fromNick, "  DICEROLL")
            sendPM(fromNick, "  HUG")
            sendPM(fromNick, "  HELP")
            If fromNick = main.owner Then
                sendPM(fromNick, "Owner-Only Items:")
                sendPM(fromNick, "  GETVAR")
                sendPM(fromNick, "  CHANGENICK")
                sendPM(fromNick, "  CHANGEOWNER")
                sendPM(fromNick, "  JOINCHAN")
                sendPM(fromNick, "  PARTCHAN")
                sendPM(fromNick, "  IDENTIFY")
                sendPM(fromNick, "  GOODNIGHT")
            End If
        End If
    End Sub
    Sub getHelp(fromNick As String, fromChan As String, arguments As String)
        Select Case arguments.ToLower()
            Case "ownerinfo" : help_OwnerInfo(fromNick)
            Case "version" : help_Version(fromNick)
            Case "diceroll" : help_DiceRoll(fromNick)
            Case "hug" : help_Hug(fromNick)
            Case "getvar" : If fromNick = owner Then help_GetVar(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "changenick" : If fromNick = owner Then help_changeNick(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "changeowner" : If fromNick = owner Then help_changeOwner(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "joinchan" : If fromNick = owner Then help_joinChan(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "partchan" : If fromNick = owner Then help_partChan(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "identify" : If fromNick = owner Then help_identify(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case "goodnight" : If fromNick = owner Then help_goodnight(fromNick) Else sendPM(fromNick, String.Format("{0}: Sorry, only my owner can ask about that", fromNick))
            Case Else : sendPM(fromNick, String.Format("{0}: Sorry, I don't know that command.", fromNick))
        End Select
    End Sub
    'TODO: Sort this crap into alphabetical order
    Sub help_OwnerInfo(fromNick As String)
        sendPM(fromNick, "OwnerInfo - Gets owner information")
        sendPM(fromNick, String.Format("Usage: ""{0}: OwnerInfo""", main.nickname))
    End Sub
    Sub help_Version(fromNick As String)
        sendPM(fromNick, "Version - Gets IRC Bot version information")
        sendPM(fromNick, String.Format("Usage: ""{0}: Version""", main.nickname))
    End Sub
    Sub help_DiceRoll(fromNick As String)
        sendPM(fromNick, "DiceRoll - Rolls dice and produces results")
        sendPM(fromNick, String.Format("Usage: ""!<num1>d<num2>"" (Where <num1> = A number of choosing, up to {0} / <num2> = A number of choosing, up to {1})", main.diceMaxRolls, main.diceMaxSides))
        sendPM(fromNick, "Useful for Role-Playing or other times when you need to decide something based on the rolling of dice.")
    End Sub
    Sub help_Hug(fromNick As String)
        sendPM(fromNick, "Hug - Responds to hugs")
        sendPM(fromNick, String.Format("Usage: "" * {0} hugs {1}"" (/me action)", fromNick, main.nickname))
        sendPM(fromNick, "Owners get special responses to hugs")
    End Sub
    Sub help_Help(fromNick As String)
        sendPM(fromNick, "Help - Gives help information")
        sendPM(fromNick, String.Format("Usage: ""{0} Help"" OR ""{0}: Help <variable>"", where <variable> is the help object you wish to attain", main.nickname))
    End Sub
    Sub help_GetVar(fromNick As String)
        sendPM(fromNick, "GetVar - Gets variable information")
        sendPM(fromNick, String.Format("Usage: ""{0}: GetVar <var>""", main.nickname))
        sendPM(fromNick, "Available variables are:")
        sendPM(fromNick, "  settingsFile - Location of settings.xml file")
        sendPM(fromNick, "  diceMaxRolls - Maximum number of dicerolls allowed in a single message")
        sendPM(fromNick, "  diceMaxSides - Maximum number of sides/chances allowed in a single diceroll")
    End Sub
    Sub help_changeNick(fromNick As String)
        sendPM(fromNick, "ChangeNick - Changes nickname of IRC Bot")
        sendPM(fromNick, String.Format("Usage: ""{0}: ChangeNick <newNick>"" (where <newNick> is the desired new username)", main.nickname))
    End Sub
    Sub help_changeOwner(fromNick As String)
        sendPM(fromNick, "ChangeOwner - Changes owner of IRC Bot")
        sendPM(fromNick, String.Format("Usage: ""{0}: ChangeOwner <newOwner>"" (where <newOwner> is the desired new username)", main.nickname))
        sendPM(fromNick, "Note: Be careful about changing ownership. Unless the new owner returns ownership to you, you cannot regain ownership unless you restart the bot")
    End Sub
    Sub help_joinChan(fromNick As String)
        sendPM(fromNick, "JoinChan - Joins a channel")
        sendPM(fromNick, String.Format("Usage: ""{0}: JoinChan <newChan>"" (where <newChan> is the desired channel to be joined)", main.nickname))
        sendPM(fromNick, "Note: You can chain multiple channels together to join multiple at once (eg, ""#chanOne,#chanTwo,#chanThree"")")
    End Sub
    Sub help_partChan(fromNick As String)
        sendPM(fromNick, "PartChan - Parts (leaves) a channel")
        sendPM(fromNick, String.Format("Usage: ""{0}: PartChan <oldChan>"" (where <oldChan> is the desired channel to be left)", main.nickname))
    End Sub
    Sub help_identify(fromNick As String)
        sendPM(fromNick, "Identify - Identifies (logs in) with NickServ")
        sendPM(fromNick, String.Format("Usage: ""{0}: Identify""", main.nickname))
        sendPM(fromNick, "Note: Will attempt to identify whether the account exists or not")
    End Sub
    Sub help_goodnight(fromNick As String)
        sendPM(fromNick, "Goodnight - Disconnects and terminates the IRC bot")
        sendPM(fromNick, String.Format("Usage: ""Goodnight, {0}""", main.nickname))
    End Sub
End Module
