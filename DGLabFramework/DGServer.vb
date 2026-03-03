Imports System.Drawing
Imports System.Net
Imports QRCoder
Imports WebSocketSharp.Server
Imports System.Math
''' <summary>
''' 表示一个郊狼服务端
''' </summary>
Public Class DGServer
    ''' <summary>
    ''' 获取或设置服务端的端口号
    ''' </summary>
    ''' <returns></returns>
    Public Property Port As Integer = 9000
    ''' <summary>
    ''' 获取或设置服务端的地址
    ''' </summary>
    ''' <returns></returns>
    Public Property Address As String
    ''' <summary>
    ''' 获取或设置服务端的ID，格式为XXXX-XXXXXXXXX-XXXXX-XXXXX-XX
    ''' </summary>
    ''' <returns></returns>
    Public Property ID As String
    ''' <summary>
    ''' 包含当前连接的设备ID列表，每个设备ID格式为XXXX-XXXXXXXXX-XXXXX-XXXXX-XX
    ''' </summary>
    Public Devices As New List(Of String)
    ''' <summary>
    ''' 预留给下一个设备的编号
    ''' </summary>
    Private DeviceN As Integer = 1
    Private WSSV As WebSocketServer
    ''' <summary>
    ''' 创建新的郊狼服务端
    ''' </summary>
    Public Sub New()
        Port = 9000
        Address = GetLocalIPv4Address()
        ID = "1111-222222222-33333-44444-01"
        AddHandler ControlService.ReceivedMessage, AddressOf ControlService_ReceivedMessage
        AddHandler ControlService.Connected, AddressOf ControlService_Connected
        AddHandler ControlService.Disconnected, AddressOf ControlService_Disconnected
    End Sub
    ''' <summary>
    ''' 启动服务器
    ''' </summary>
    Public Sub Start()
        WSSV = New WebSocketServer("ws://0.0.0.0:" + CStr(Port))
        WSSV.AddWebSocketService(Of ControlService)("/1234-123456789-12345-12345-" + DeviceN.ToString("D2"))
        WSSV.Start()
    End Sub
    ''' <summary>
    ''' 设置输出强度
    ''' </summary>
    ''' <param name="channel">通道</param>
    ''' <param name="strength">强度</param>
    Public Sub SetStrength(channel As Channel, strength As Integer, device As Integer)
        If strength > 200 Then strength = 200
        If strength < 0 Then strength = 0
        Dim m As New DGMessage(DGMessage.Messagetype.msg)
        m.TargetID = ID
        m.ClientID = Devices(device)
        If channel = Channel.A Then
            m.Message = "strength-1+2+" + CStr(strength)
        Else
            m.Message = "strength-2+2+" + CStr(strength)
        End If
        Send(m)

    End Sub
    ''' <summary>
    ''' 增加/减少输出强度   
    ''' </summary>
    ''' <param name="channel">通道</param>
    ''' <param name="strength">变化的强度</param>
    Public Sub AddStrength(channel As Channel, strength As Integer, device As Integer)
        If strength > 200 Then strength = 200
        If strength < -200 Then strength = -200
        Dim s As String
        If channel = Channel.A Then
            s = "strength-1+"
        Else
            s = "strength-2+"
        End If
        If strength < 0 Then
            s += "0+" + CStr(Abs(strength))
        Else
            s += "1+" + CStr(strength)
        End If
        Dim m As New DGMessage(DGMessage.Messagetype.msg)
        m.TargetID = ID
        m.ClientID = Devices(device)
        m.Message = s
        Send(m)
    End Sub
    ''' <summary>
    ''' 将波形发送到郊狼设备
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <param name="wave"></param>
    ''' <param name="device"></param>
    Public Sub SendWave(channel As Channel, wave As Wave, device As Integer)
        Dim m As New DGMessage(DGMessage.Messagetype.msg)
        m.TargetID = ID
        m.ClientID = Devices(device)
        If channel = Channel.A Then
            m.Message = "pulse-A:" + wave.ToString
        Else
            m.Message = "pulse-B:" + wave.ToString
        End If
        Send(m)
        Console.WriteLine(m.ToString)
    End Sub
    ''' <summary>
    ''' 关闭服务器
    ''' </summary>
    Public Sub [Stop]()
        WSSV.Stop()
        Devices.Clear()
        DeviceN = 1
    End Sub
    ''' <summary>
    ''' 获取郊狼服务端的二维码
    ''' </summary>
    ''' <returns></returns>
    Public Function GetQRCode() As Bitmap
        Dim g As New QRCodeGenerator()
        Dim qr As New QRCode(g.CreateQrCode("https://www.dungeon-lab.com/app-download.php#DGLAB-SOCKET#ws://" + Address + ":" + CStr(Port) + "/1234-123456789-12345-12345-" + DeviceN.ToString("D2"), QRCodeGenerator.ECCLevel.Default))
        Return qr.GetGraphic(4)
    End Function
    ''' <summary>
    ''' 枚举通道
    ''' </summary>
    Public Enum Channel
        A = 1
        B = 2
    End Enum

    Public Shared Function GetLocalIPv4Address() As String
        Try
            Dim hostName As String = Dns.GetHostName()
            Dim hostEntry As IPHostEntry = Dns.GetHostEntry(hostName)
            For Each ip As IPAddress In hostEntry.AddressList
                If ip.AddressFamily = Sockets.AddressFamily.InterNetwork AndAlso
                   Not IPAddress.IsLoopback(ip) Then
                    Return ip.ToString()
                End If
            Next
            Return "127.0.0.1"
        Catch ex As Exception
            Return "127.0.0.1"
        End Try
    End Function
    Public Sub ControlService_ReceivedMessage(data As DGMessage)
        If data.Type = "bind" AndAlso data.TargetID = ID Then
            Dim m As New DGMessage(DGMessage.Messagetype.bind)
            m.ClientID = data.ClientID
            m.TargetID = ID
            m.Message = "200"
            Send(m)
            DeviceN += 1
            Devices.Add(data.ClientID)
            WSSV.AddWebSocketService(Of ControlService)("/1234-123456789-12345-12345-" + DeviceN.ToString("D2"))
        End If
    End Sub
    Public Sub ControlService_Connected()
        Dim m As New DGMessage(DGMessage.Messagetype.bind)
        m.ClientID = ID
        m.Message = "targetId"
        Send(m)
    End Sub
    Public Sub ControlService_Disconnected()

    End Sub
    ''' <summary>
    ''' 将消息发送给郊狼
    ''' </summary>
    ''' <param name="data"></param>
    Public Sub Send(data As DGMessage)
        If data.TargetID = "" Then
            Dim svc = WSSV.WebSocketServices("/1234-123456789-12345-12345-" + DeviceN.ToString("D2"))
            svc.Sessions.Broadcast(data.ToString)
        Else
            Dim svc = WSSV.WebSocketServices("/" + data.ClientID)
            svc.Sessions.Broadcast(data.ToString)
        End If
    End Sub
End Class
