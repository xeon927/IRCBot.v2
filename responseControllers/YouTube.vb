Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Module YouTube
    Sub Check(nick As String, chan As String, message As String)
        If InStr(message, "youtu.be") Or InStr(message, "youtube.com") Then
            If Regex.IsMatch(message, "https?://youtu\.be/(?<ID>[a-zA-Z0-9_-]{11})") Then getVideoInformation(nick, chan, Regex.Match(message, "https?://youtu\.be/(?<ID>[a-zA-Z0-9_-]{11})").Result("${ID}"))
            If InStr(message, "/watch?") Then
                If Regex.IsMatch(message, "[?&]v=(?<ID>[a-zA-Z0-9_-]{11})") Then getVideoInformation(nick, chan, Regex.Match(message, "[?&]v=(?<ID>[a-zA-Z0-9_-]{11})").Result("${ID}"))
            End If
        End If
    End Sub
    Sub getVideoInformation(nick As String, chan As String, vidID As String)
        Dim InfoStream As StreamReader
        Dim responseQueue As New Queue(Of String)
        Dim vidTitle, vidCreator, vidLength, vidUploadDate, resultCount As String

        InfoStream = New StreamReader(WebRequest.Create(String.Format("https://www.googleapis.com/youtube/v3/videos?part=contentDetails%2Csnippet&key={0}&id={1}", YTAPIKey, vidID)).GetResponse().GetResponseStream())
        While Not InfoStream.EndOfStream
            responseQueue.Enqueue(InfoStream.ReadLine())
        End While
        While responseQueue.Count > 0
            Dim currentLine As String = responseQueue.Dequeue()
            If Regex.IsMatch(currentLine, """totalResults"": (?<ResultCount>.+),$") Then resultCount = Regex.Match(currentLine, """totalResults"": (?<ResultCount>.+),$").Result("${ResultCount}")
            If Regex.IsMatch(currentLine, """publishedAt"": ""(?<PublishTime>.+)"",$") Then vidUploadDate = Regex.Match(currentLine, """publishedAt"": ""(?<PublishTime>.+)"",$").Result("${PublishTime}")
            If Regex.IsMatch(currentLine, """title"": ""(?<VideoTitle>.+)"",$") Then vidTitle = Regex.Match(currentLine, """title"": ""(?<VideoTitle>.+)"",$").Result("${VideoTitle}")
            If Regex.IsMatch(currentLine, """channelTitle"": ""(?<VideoCreator>.+)"",$") Then vidCreator = Regex.Match(currentLine, """channelTitle"": ""(?<VideoCreator>.+)"",$").Result("${VideoCreator}")
            If Regex.IsMatch(currentLine, """duration"": ""(?<VideoLength>.+)"",$") Then vidLength = Regex.Match(currentLine, """duration"": ""(?<VideoLength>.+)"",$").Result("${VideoLength}")
        End While
        If resultCount = "1" Then
            sendMessage(chan, String.Format("{0}: {1} - length: {2} - Uploaded by {3} on {4}", nick, Uri.UnescapeDataString(vidTitle), YTGetLength(vidLength.Substring(2)), Uri.UnescapeDataString(vidCreator), YTGetDateTime(vidUploadDate)))
        Else
            sendMessage(chan, String.Format("{0}: No results for video ID: {1}", nick, vidID))
        End If
    End Sub
    Function YTGetLength(LenStr As String) As String
        Dim pattern As String = "((?<hours>\d+)H)?((?<minutes>\d+)M)?((?<seconds>\d+)S)?"
        Dim r As Match = Regex.Match(LenStr, pattern)
        Dim vidTime As New List(Of String)
        If Not r.Result("${hours}") = "" Then If r.Result("${hours}") = "1" Then vidTime.Add(String.Format("{0} hour", r.Result("${hours}"))) Else vidTime.Add(String.Format("{0} hours", r.Result("${hours}")))
        If Not r.Result("${minutes}") = "" Then If r.Result("${minutes}") = "1" Then vidTime.Add(String.Format("{0} minute", r.Result("${minutes}"))) Else vidTime.Add(String.Format("{0} minutes", r.Result("${minutes}")))
        If Not r.Result("${seconds}") = "" Then If r.Result("${seconds}") = "1" Then vidTime.Add(String.Format("{0} second", r.Result("${seconds}"))) Else vidTime.Add(String.Format("{0} seconds", r.Result("${seconds}")))
        Return String.Join(", ", vidTime)
    End Function
    Function YTGetDateTime(DTStr As String)
        Return DateTime.Parse(DTStr).ToLongDateString()
    End Function
End Module