Imports System.IO
Module logging
    Sub append(message As String)
        File.AppendAllText(logfilePath, String.Format("[{0}] {1}{2}", DateTime.Now, message, vbCrLf))
    End Sub
End Module
