Imports System.IO
Imports System.Xml
Module config
    Sub WriteSample()
        Dim defaultConfig As New XDocument(
            New XElement("settings",
                         New XElement("server",
                                      New XElement("host", "irc.example.net"),
                                      New XElement("port", "6667"),
                                      New XElement("channel", "#channel")),
                         New XElement("bot",
                                      New XElement("nickname", "IRCBot"),
                                      New XElement("username", "IRCBot"),
                                      New XElement("realname", "xeon927's IRC Bot"),
                                      New XElement("owner", "botOwner")),
                         New XElement("nickserv",
                                      New XElement("useNickServ", "False"),
                                      New XElement("password", "password")),
                         New XElement("misc",
                                      New XElement("ownerfail", "Sorry, only my owner can make me do that."),
                                      New XElement("alwaysQuiet", "False")),
                         New XElement("diceroll",
                                      New XElement("diceMaxRolls", "75"),
                                      New XElement("diceMaxSides", "500"))))
        defaultConfig.Save(settingsFile)
    End Sub
    Sub genNew()
        If File.Exists(settingsFile) Then File.Delete(settingsFile)
        WriteSample()
        End
    End Sub
    Sub Load()
        If Not File.Exists(settingsFile) Then WriteSample()

        Dim xmlDoc = XDocument.Load(settingsFile)
        'Server Settings
        host = xmlDoc.<settings>.<server>.<host>.Value
        port = xmlDoc.<settings>.<server>.<port>.Value
        channel = xmlDoc.<settings>.<server>.<channel>.Value
        'Bot Settings
        nickname = xmlDoc.<settings>.<bot>.<nickname>.Value
        username = xmlDoc.<settings>.<bot>.<username>.Value
        realname = xmlDoc.<settings>.<bot>.<realname>.Value
        owner = xmlDoc.<settings>.<bot>.<owner>.Value
        'NickServ Settings
        If xmlDoc.<settings>.<nickserv>.<useNickServ>.Value = "True" Then nsUse = True Else nsUse = False
        nsPass = xmlDoc.<settings>.<nickserv>.<password>.Value
        'Diceroll Settings
        diceMaxRolls = xmlDoc.<settings>.<diceroll>.<diceMaxRolls>.Value
        diceMaxSides = xmlDoc.<settings>.<diceroll>.<diceMaxSides>.Value
        If diceMaxRolls > 75 Then
            Console.WriteLine("[WARN] settings.diceroll.diceMaxRolls is greater than 75. Defaulting to 75.")
            diceMaxRolls = 75
        End If
        If diceMaxSides = 1 Then
            Console.WriteLine("[WARN] settings.diceroll.diceMaxSides must be more than 1. Defaulting to 500.")
            diceMaxSides = 500
        End If
        'Miscellaneous Settings
        ownerfail = xmlDoc.<settings>.<misc>.<ownerfail>.Value
        If xmlDoc.<settings>.<misc>.<alwaysQuiet>.Value = True Then QuietStart = True
    End Sub
End Module
