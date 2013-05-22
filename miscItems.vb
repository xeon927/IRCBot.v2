Module miscItems
    Function removeSpaces(message As String)
        Return message.Replace(" ", "")
    End Function
    Function numberGen(min As Integer, max As Integer)
        Return gen.Next(min, max + 1)
    End Function
End Module
