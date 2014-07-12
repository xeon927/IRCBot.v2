Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Net
Imports System.Web
Imports System.IO
Module deviantART
    Sub Check(nick As String, chan As String, message As String)
        For Each DALink As Match In Regex.Matches(message, "(http://\S+\.deviantart\.com/art/\S+)|(http://fav\.me/\S+)|(http://sta\.sh/\S+)|(http://\S+\.deviantart\.com/\S+\#/d\S+)")
            Try
                'Shitty method because XDocument.Load() won't do it (connection drops) - need to manually send headers
                Dim req As HttpWebRequest = WebRequest.Create(String.Format("http://backend.deviantart.com/oembed?format=xml&url={0}", Uri.EscapeDataString(DALink.Value)))
                req.UserAgent = String.Format("xeon927IRCBot/{0} (+https://github.com/xeon927/IRCBot.v2)", version)
                Dim xDoc As XDocument = XDocument.Load(req.GetResponse().GetResponseStream())

                'sendMessage(chan, String.Format("{0}: ""{1}"" ({2}*{3}), uploaded by {4} in category [{5}]", nick, _
                '        xDoc.<oembed>.<title>.Value, _
                '        xDoc.<oembed>.<width>.Value, _
                '        xDoc.<oembed>.<height>.Value, _
                '        xDoc.<oembed>.<author_name>.Value, _
                '        HttpUtility.HtmlDecode(xDoc.<oembed>.<category>.Value)))

            Catch ex As Exception
                sendMessage(chan, String.Format("{0}: Sorry, but something went wrong. Please check back later.", nick))
                sendNotice(owner, "Something went wrong with deviantART. Please check botlogs as soon as possible.")
                logging.append(ex.ToString())
#If DEBUG Then
                Console.WriteLine(ex.ToString())
#End If
            End Try
        Next
    End Sub
End Module