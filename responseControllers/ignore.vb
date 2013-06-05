Module ignore
    Public isIgnoring As Boolean = False
    Sub Check(message As String)
        If getNickname(message) = owner Then
            If InStr(getMessage(message), String.Format("{0}: stopignore", nickname)) Then isIgnoring = False
            If InStr(getMessage(message), String.Format("{0}: startignore", nickname)) Then isIgnoring = True
        End If
    End Sub
End Module
