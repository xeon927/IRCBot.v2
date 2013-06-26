Imports System.Text.RegularExpressions
Module miscItems
    Function removeSpaces(message As String)
        Return message.Replace(" ", "")
    End Function
    Function numberGen(min As Integer, max As Integer)
        Return gen.Next(min, max + 1)
    End Function
    Function isChanMsg(message As String)
        If Regex.IsMatch(message, ":\w+!\w+@.+\ PRIVMSG\ #\w+\ .+") Then Return True Else Return False
    End Function
End Module
