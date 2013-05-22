Module startFlags
    Sub Check()
        For Each s As String In My.Application.CommandLineArgs
            If s.ToLower() = "/help" Then displayFlags()
            If s.ToLower() = "/?" Then displayFlags()
            If s.ToLower() = "/quiet" Then QuietStart = True
            If s.ToLower() = "/gencfg" Then config.genNew()
            If s.ToLower() = "/setup" Then setup.runSetup()
        Next
    End Sub
    Sub displayFlags()
        Console.WriteLine("Welcome to xeon927's IRC Bot")
        Console.WriteLine("Commandline flags are to be prefixed with /")
        Console.WriteLine("Current Command line flags are:")
        Console.WriteLine("     HELP - Displays this message")
        Console.WriteLine("        ? - Displays this message")
        Console.WriteLine("    QUIET - Boots quietly (does not ask for configuration - ")
        Console.WriteLine("            boots with settings.xml values)")
        Console.WriteLine("   GENCFG - Regenerates settings.xml file. Doing this WILL")
        Console.WriteLine("            overwrite custom settings")
        End
    End Sub
End Module
