Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web
Imports System.Net
Imports System.IO 'Remove later
Module Twitter
    Sub Check(nick As String, chan As String, message As String)
        Try
            For Each TwitterLink As Match In Regex.Matches(message, "https?://(?:www\.)?twitter.com\/(?:\#!\/)?\S+\/status(?:es)?\/(?<tweetID>\d+)")
                Console.WriteLine(getInfo(TwitterLink.Result("${tweetID}")).ToString)
            Next
        Catch ex As WebException
            Console.WriteLine("Error:")
            Using responseReader As New StreamReader(ex.Response.GetResponseStream())
                Console.WriteLine(responseReader.ReadToEnd())
            End Using
        Catch ex As Exception
            sendMessage(chan, String.Format("{0}: Sorry, but something went wrong. Please check back later.", nick))
            sendNotice(owner, "Something went wrong with Twitter. Please check botlogs as soon as possible.")
            logging.append(ex.ToString())
#If DEBUG Then
            Console.WriteLine(ex.ToString())
#End If
        End Try
    End Sub

    Function createSignature(params As Dictionary(Of String, String)) As String
        Dim paramString As String = ""
        For Each element In params
            If element.Key = "url" Then Continue For
            If Not paramString = "" Then paramString = paramString + "&"
            paramString = paramString + String.Format("{0}={1}", percentEncode(element.Key), percentEncode(element.Value))
        Next
        Dim sigBase As String = String.Format("{0}&{1}&{2}", "GET", percentEncode(params("url")), percentEncode(paramString))
        Dim signKey As String = String.Format("{0}&{1}", percentEncode(TwitterAPISecret), percentEncode(TwitterAccessTokenSecret))
        Dim hasher As New HMACSHA1(Encoding.ASCII.GetBytes(signKey))
        Return Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(sigBase)))
    End Function
    Function createNonce() As String
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim builder As New StringBuilder
        For i As Integer = 1 To 32
            builder.Append(chars.Substring(numberGen(0, chars.Length - 1), 1))
        Next
        Dim nonce As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()))
        Return Regex.Replace(nonce, "[^A-Za-z0-9]", "")
    End Function

    Function percentEncode(message As String) As String
        Return Uri.EscapeDataString(message)
    End Function

    Function getInfo(tweetID As String) As XDocument
        'Deal with parameters
        Dim params As New Dictionary(Of String, String) 'Because I need to separate shit later - else I'd use a List

        'Misc parameters
        params.Add("id", tweetID)
        params.Add("url", "https://api.twitter.com/1.1/statuses/show.xml")

        'oAuth parameters
        params.Add("oauth_consumer_key", TwitterAPIKey)
        params.Add("oauth_nonce", createNonce())
        params.Add("oauth_signature_method", "HMAC-SHA1")
        params.Add("oauth_timestamp", Convert.ToInt32((DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds))
        params.Add("oauth_token", TwitterAccessToken)
        params.Add("oauth_version", "1.0")
        params.Add("oauth_signature", createSignature(params)) 'When all other params are in place, create signature and add to dictionary

        'Create header string
        Dim header As String = "OAuth "
        For Each element In params
            If InStr(element.Key, "oauth") Then
                If Not header = "OAuth " Then header = header + ", "
                header = header + String.Format("{0}=""{1}""", percentEncode(element.Key), percentEncode(element.Value))
            End If
        Next
        Dim req As HttpWebRequest = HttpWebRequest.Create(String.Format("{0}?id={1}", params("url"), params("id")))
        req.UserAgent = String.Format("xeon927IRCBot/{0} (+https://github.com/xeon927/IRCBot.v2)", version)
        req.Accept = "application/xml"
        req.Headers.Add("Authorization", header)
        Using reader As New StreamReader(req.GetResponse().GetResponseStream())
            Console.WriteLine(reader.ReadToEnd())
        End Using
        Return XDocument.Load(req.GetResponse().GetResponseStream())
    End Function
End Module
'https ://api.twitter.com/1/statuses/oembed.xml?id=<tweetID>


'Module deviantART
'    Sub Check(nick As String, chan As String, message As String)
'        For Each DALink As Match In Regex.Matches(message, "(http://\S+\.deviantart\.com/art/\S+)|(http://fav\.me/\S+)|(http://sta\.sh/\S+)|(http://\S+\.deviantart\.com/\S+\#/d\S+)")
'            Try
'                'Shitty method because XDocument.Load() won't do it (connection drops) - need to manually send headers
'                Dim req As HttpWebRequest = WebRequest.Create(String.Format("http://backend.deviantart.com/oembed?format=xml&url={0}", Uri.EscapeDataString(DALink.Value)))
'                req.UserAgent = String.Format("xeon927IRCBot/{0} (+https://github.com/xeon927/IRCBot.v2)", version)
'                Dim xDoc As XDocument = XDocument.Load(req.GetResponse().GetResponseStream())

'                'sendMessage(chan, String.Format("{0}: ""{1}"" ({2}*{3}), uploaded by {4} in category [{5}]", nick, _
'                '        xDoc.<oembed>.<title>.Value, _
'                '        xDoc.<oembed>.<width>.Value, _
'                '        xDoc.<oembed>.<height>.Value, _
'                '        xDoc.<oembed>.<author_name>.Value, _
'                '        HttpUtility.HtmlDecode(xDoc.<oembed>.<category>.Value)))

'            Catch ex As Exception
'                sendMessage(chan, String.Format("{0}: Sorry, but something went wrong. Please check back later.", nick))
'                sendNotice(owner, "Something went wrong with deviantART. Please check botlogs as soon as possible.")
'                logging.append(ex.ToString())
'#If DEBUG Then
'                Console.WriteLine(ex.ToString())
'#End If
'            End Try
'        Next
'    End Sub
'End Module