Imports System.Text.Encoding

Module setup
    Dim tempVal As String
    Sub loadCurrent()
        config.Load()
    End Sub
    Sub runSetup()
        loadCurrent()
        setupMain()
    End Sub

    Sub setupMain()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Server Settings")
        Console.WriteLine("  2) Bot Settings")
        Console.WriteLine("  3) NickServ Settings")
        Console.WriteLine("  4) Miscellaneous Settings")
        Console.WriteLine("  5) DiceRoll Settings")
        Console.WriteLine()
        Console.WriteLine("  9) Save + Exit")
        Select Case Console.ReadLine()
            Case "1" : settings_Server()
            Case "2" : settings_Bot()
            Case "3" : settings_NickServ()
            Case "4" : settings_Misc()
            Case "5" : settings_DiceRoll()
            Case "9" : config.WriteMod() : End
            Case Else : runSetup()
        End Select
    End Sub

    'Server Settings
    Sub settings_Server()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Host")
        Console.WriteLine("  2) Port")
        Console.WriteLine("  3) Password")
        Console.WriteLine("  4) Channel(s)")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_Server_Host()
            Case "2" : settings_Server_Port()
            Case "3" : settings_Server_Password()
            Case "4" : settings_Server_Channel()
            Case "9" : setupMain()
            Case Else : settings_Server()
        End Select
    End Sub
    Sub settings_Server_Host()
        Console.Clear()
        Console.WriteLine("Host - The IRC server address (without port) for the IRC Bot to connect to")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.host))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.host = tempVal
        settings_Server()
    End Sub
    Sub settings_Server_Port()
        Console.Clear()
        Console.WriteLine("Port - The port of the IRC server to be connected to")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.port))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.port = tempVal
        settings_Server()
    End Sub
    Sub settings_Server_Password()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable/Disable Password")
        Console.WriteLine("  2) Set Password")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_Server_Password_Toggle()
            Case "2" : settings_Server_Password_Set()
            Case "9" : settings_Server()
            Case Else : settings_Server_Password()
        End Select
    End Sub
    Sub settings_Server_Password_Toggle()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable Server Password")
        Console.WriteLine("  2) Disable Server Password")
        Console.WriteLine()
        If main.servPassUse = False Then
            Console.WriteLine("Server Password is currently disabled")
        ElseIf main.servPassUse = True Then
            Console.WriteLine("Server Password is currently enabled")
        End If
        Select Case Console.ReadLine()
            Case "1" : main.servPassUse = True
            Case "2" : main.servPassUse = False
            Case Else : settings_Server_Password_Toggle()
        End Select
        settings_Server_Password()
    End Sub
    Sub settings_Server_Password_Set()
        Console.Clear()
        Console.WriteLine("Server Password - The password to be sent to the server in order to connect (if enabled)")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.servPass))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.servPass = tempVal
        settings_Server_Password()
    End Sub
    Sub settings_Server_Channel()
        Console.Clear()
        Console.WriteLine("Channel - The channel in which to join upon connection to the IRC server")
        Console.WriteLine("Note: Multiple channels can be specified with use of a comma (eg. #chanOne,#chanTwo)")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.channel))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.channel = tempVal
        settings_Server()
    End Sub

    'Bot Settings
    Sub settings_Bot()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Nickname")
        Console.WriteLine("  2) Username")
        Console.WriteLine("  3) Realname")
        Console.WriteLine("  4) Owner")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_Bot_Nickname()
            Case "2" : settings_Bot_Username()
            Case "3" : settings_Bot_RealName()
            Case "4" : settings_Bot_Owner()
            Case "9" : setupMain()
            Case Else : settings_Bot()
        End Select
    End Sub
    Sub settings_Bot_Nickname()
        Console.Clear()
        Console.WriteLine("Nickname - The nickname of the IRC Bot")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.nickname))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.nickname = tempVal
        settings_Bot()
    End Sub
    Sub settings_Bot_Username()
        Console.Clear()
        Console.WriteLine("Username - The username of the IRC Bot")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.username))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.username = tempVal
        settings_Bot()
    End Sub
    Sub settings_Bot_RealName()
        Console.Clear()
        Console.WriteLine("Realname - The real name of the IRC Bot")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.realname))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.realname = tempVal
        settings_Bot()
    End Sub
    Sub settings_Bot_Owner()
        Console.Clear()
        Console.WriteLine("Owner - The user that owns/runs the IRC Bot. Has access to higher-privilege commands")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.owner))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.owner = tempVal
        settings_Bot()
    End Sub

    'NickServ Settings
    Sub settings_NickServ()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Authentication Toggle")
        Console.WriteLine("  2) NickServ Password")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_NickServ_NSToggle()
            Case "2" : settings_NickServ_NSPass()
            Case "9" : setupMain()
            Case Else : settings_NickServ()
        End Select
    End Sub
    Sub settings_NickServ_NSToggle()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable NickServ Auth")
        Console.WriteLine("  2) Disable NickServ Auth")
        Console.WriteLine()
        If main.nsUse = False Then
            Console.WriteLine("NickServ Auth is currently off")
        ElseIf main.nsUse = True Then
            Console.WriteLine("NickServ Auth is currently on")
        End If
        Select Case Console.ReadLine()
            Case "1" : main.nsUse = True
            Case "2" : main.nsUse = False
            Case Else : settings_NickServ_NSToggle()
        End Select
        settings_NickServ()
    End Sub
    Sub settings_NickServ_NSPass()
        Console.Clear()
        Console.WriteLine("NickServ Pass - The password in which is sent to NickServ for identification")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.nsPass))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.nsPass = tempVal
        settings_NickServ()
    End Sub

    'Miscellaneous Settings
    Sub settings_Misc()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Owner Failure Message")
        Console.WriteLine("  2) Permanent Quiet Startup")
        Console.WriteLine("  3) Logging")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_Misc_ownerFail()
            Case "2" : settings_Misc_AlwaysQuiet()
            Case "3" : settings_Misc_Logging()
            Case "9" : setupMain()
            Case Else : settings_Misc()
        End Select
    End Sub
    Sub settings_Misc_ownerFail()
        Console.Clear()
        Console.WriteLine("Owner Failure Message - The message to be displayed to a user when they attempt to perform an owner action when they do not own the bot")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.ownerfail))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.ownerfail = tempVal
        settings_Misc()
    End Sub
    Sub settings_Misc_AlwaysQuiet()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable Persistent Quiet Start")
        Console.WriteLine("  2) Disable Persistent Quiet Start")
        Console.WriteLine()
        If main.QuietStart = False Then
            Console.WriteLine("Persistent Quiet Start is currently off")
        ElseIf main.QuietStart = True Then
            Console.WriteLine("Persistent Quiet Start is currently on")
        End If
        Select Case Console.ReadLine()
            Case "1" : main.QuietStart = True
            Case "2" : main.QuietStart = False
            Case Else : settings_Misc_AlwaysQuiet()
        End Select
        settings_Misc()
    End Sub
    Sub settings_Misc_Logging()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable/Disable Logging")
        Console.WriteLine("  2) Set Logging Directory")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_Misc_Logging_Toggle()
            Case "2" : settings_Misc_Logging_Path()
            Case "9" : settings_Misc()
            Case Else : settings_Misc_Logging()
        End Select
    End Sub
    Sub settings_Misc_Logging_Toggle()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Enable Logging")
        Console.WriteLine("  2) Disable Logging")
        Console.WriteLine()
        If main.loggingEnabled = False Then
            Console.WriteLine("Logging is currently disabled")
        ElseIf main.loggingEnabled = True Then
            Console.WriteLine("Logging is currently enabled")
        End If
        Select Case Console.ReadLine()
            Case "1" : main.loggingEnabled = True
            Case "2" : main.loggingEnabled = False
            Case Else : settings_Misc_Logging_Toggle()
        End Select
        settings_Misc_Logging()
    End Sub
    Sub settings_Misc_Logging_Path()
        Console.Clear()
        Console.WriteLine("Path to log file - This can be either relative (to current directory), or absolute. This path must point to a file, not a folder.")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.logfilePath))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.logfilePath = tempVal
        settings_Misc_Logging()
    End Sub

    'DiceRoll Settings
    Sub settings_DiceRoll()
        Console.Clear()
        Console.WriteLine("Please choose an option:")
        Console.WriteLine("  1) Maximum Sides")
        Console.WriteLine("  2) Maximum Rolls")
        Console.WriteLine()
        Console.WriteLine("  9) Back")
        Select Case Console.ReadLine()
            Case "1" : settings_DiceRoll_maxSides()
            Case "2" : settings_DiceRoll_maxRolls()
            Case "9" : setupMain()
            Case Else : settings_DiceRoll()
        End Select
    End Sub
    Sub settings_DiceRoll_maxSides()
        Console.Clear()
        Console.WriteLine("Diceroll Maximum Sides - The maximum number of possibilities a diceroll will generate")
        Console.WriteLine("Note: This value MUST be a WHOLE number. The minimum allowed is 2")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.diceMaxSides))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.diceMaxSides = tempVal
        settings_DiceRoll()
    End Sub
    Sub settings_DiceRoll_maxRolls()
        Console.Clear()
        Console.WriteLine("Diceroll Maximum Rolls - The maximum number of generated numbers a diceroll will generate")
        Console.WriteLine("Note: This value MUST be a WHOLE number. The maximum allowed is 75, due to IRC message length constraints")
        Console.WriteLine()
        Console.Write(String.Format("[{0}] ", main.diceMaxRolls))
        tempVal = Console.ReadLine()
        If Not tempVal = "" Then main.diceMaxRolls = tempVal
        settings_DiceRoll()
    End Sub
End Module