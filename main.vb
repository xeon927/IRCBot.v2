Imports System
Imports System.IO
Imports System.Net.Sockets
Imports System.Text.Encoding
Imports System.Text.RegularExpressions
Imports System.Xml

Module main
    Public version As String = "2.1.5"
    Public host, port, channel, nickname, username, realname, owner, ownerfail, nsPass As String
    Public settingsFile As String = Path.Combine(Directory.GetCurrentDirectory(), "settings.xml")

    Dim client As TcpClient
    Dim ReadBuf As String = ""

    Public CanRegex As Boolean = True
    Public QuietStart As Boolean = False
    Public nsUse As Boolean = False

    Public gen As New Random

    Public loggedIn As Boolean = False
    Public firstPing As Boolean = False
    Public nickSent As Boolean = False
    Public userSent As Boolean = False
    Public FirstRun As Boolean = True

    Public diceMaxRolls, diceMaxSides As Integer
    Sub Main()
        startFlags.Check()
        config.Load()
        If Not QuietStart Then getParams()
        servConnect()
        runLoop()
    End Sub
    Sub getParams()
        'Get Server Address
        Console.Write(String.Format("Server [{0}]: ", host.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then host = host Else host = ReadBuf

        'Get Server Port
        Console.Write(String.Format("Port [{0}]: ", port.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then port = port Else port = ReadBuf

        'Get Channel
        Console.Write(String.Format("Channel [{0}]: ", channel.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then channel = channel Else channel = ReadBuf

        'Get Nickname
        Console.Write(String.Format("Nickname [{0}]: ", nickname.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then nickname = nickname Else nickname = ReadBuf

        'Get Username
        Console.Write(String.Format("Username [{0}]: ", username.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then username = username Else username = ReadBuf

        'Get Realname
        Console.Write(String.Format("Realname [{0}]: ", realname.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then realname = realname Else realname = ReadBuf

        'Get Bot Owner
        Console.Write(String.Format("Owner [{0}]: ", owner.ToString()))
        ReadBuf = Console.ReadLine()
        If ReadBuf = "" Then owner = owner Else owner = ReadBuf

        ReadBuf = String.Empty
    End Sub
    Sub servConnect()
        Try
            client = New TcpClient(host, port)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub
    Sub runLoop()
        Dim stream As NetworkStream = client.GetStream()
        Dim responseData As [String] = [String].Empty

        Do
            Dim data As Byte()
            data = New [Byte](0) {}
            Dim out As String = ""
            Dim charIn As String = ""
            Try
                Do
                    stream.Read(data, 0, 1)
                    charIn = ASCII.GetString(data)
                    If charIn = vbCr Then
                        stream.Read(data, 0, 1)
                        If Not out.Substring(0, 4) = "PING" Then
                            Console.WriteLine("<<< " + out)
                        End If
                        strings.Check(out)
                        Exit Do
                    End If
                    out = out + charIn
                Loop
                If Not loggedIn Then operations.login()
            Catch ex As Exception
#If DEBUG Then
                Console.WriteLine(ex.ToString())
#Else
                Console.WriteLine("Something went wrong.")
#End If

            End Try
        Loop

    End Sub
    Sub sendData(message As String)
        message = message + vbCrLf
        Dim stream As NetworkStream = client.GetStream()
        Dim data As Byte()
        data = New [Byte](65535) {}
        data = ASCII.GetBytes(message)
        stream.Write(data, 0, data.Length)
        If Not message.Substring(0, 4) = "PONG" Then
            Console.Write(">>> " + message)
        End If
    End Sub
End Module
