Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web
Imports System.Net
Module Twitterold
    Sub Check(nick As String, chan As String, message As String)
        Try
            For Each TwitterLink As Match In Regex.Matches(message, "https?://(?:www\.)?twitter.com\/(?:\#!\/)?\S+\/statuses\/(?<tweetID>\d+)")
                Console.WriteLine(getInfo(TwitterLink.Result("${tweetID}")).ToString)
            Next
        Catch ex As WebException
            Console.WriteLine("Error:")
            Using responseReader As New System.IO.StreamReader(ex.Response.GetResponseStream())
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
    Function getInfo(TweetID As String) As XDocument
        Dim resource_url As String = "https://api.twitter.com/1.1/statuses/show.xml"
        Dim oauth_version As String = "1.0"
        Dim oauth_signature_method As String = "HMAC-SHA1"
        Dim oauth_nonce As String = Convert.ToBase64String(New ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()))
        Dim oauth_timestamp As String = Convert.ToInt64((DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds)
        Dim oauth_signature As String = generateSignature(resource_url, oauth_nonce, oauth_signature_method, oauth_timestamp, oauth_version)

        Dim authHeader As String = String.Format("OAuth oauth_nonce=""{0}"", oauth_signature_method=""{1}"", oauth_timestamp=""{2}"", oauth_consumer_key=""{3}"", oauth_token=""{4}"", oauth_signature=""{5}"", oauth_version=""{6}""", _
                                                 percentEncode(oauth_nonce), _
                                                 percentEncode(oauth_signature_method), _
                                                 percentEncode(oauth_timestamp), _
                                                 percentEncode(TwitterAPIKey), _
                                                 percentEncode(TwitterAccessToken), _
                                                 percentEncode(oauth_signature), _
                                                 percentEncode(oauth_version))
        Dim req As WebRequest
        Dim res As HttpWebResponse
        Dim responseString As String

        req = WebRequest.Create(String.Format("{0}?id={1}", resource_url, TweetID))
        req.Headers.Add("Authorization", authHeader)
        req.ContentType = "application/xml"
        res = req.GetResponse()

        Using streamReader As New System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.UTF8)
            responseString = streamReader.ReadToEnd()
        End Using

        res.Close()
        Return XDocument.Parse(responseString)
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

    Function generateSignature(stream_url As String, oauth_nonce As String, oauth_signature_method As String, oauth_timestamp As String, oauth_version As String) As String
        Dim baseString As String = String.Format("oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}", _
                                                 TwitterAPIKey, _
                                                 oauth_nonce, _
                                                 oauth_signature_method, _
                                                 oauth_timestamp, _
                                                 TwitterAccessToken, _
                                                 oauth_version)
        baseString = String.Format("GET&{0}&{1}", percentEncode(stream_url), percentEncode(baseString))
        Dim compositeKey As String = String.Format("{0}&{1}", TwitterAPISecret, TwitterAccessTokenSecret)
        Dim signature As String
        Using hasher As HMACSHA1 = New HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey))
            signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)))
        End Using
        Return signature
    End Function
End Module
