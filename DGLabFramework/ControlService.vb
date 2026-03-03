Imports WebSocketSharp
Imports WebSocketSharp.Server
Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading

Public Class ControlService
    Inherits WebSocketBehavior
    Public Shared Event ReceivedMessage(data As DGMessage)
    Public Shared Event Connected()
    Public Shared Event Disconnected()
    Protected Overrides Sub OnMessage(e As MessageEventArgs)
        Dim m As New DGMessage(e.Data)
        RaiseEvent ReceivedMessage(m)
    End Sub

    ' 当客户端连接时触发
    Protected Overrides Sub OnOpen()
        MyBase.OnOpen()
        RaiseEvent Connected()
    End Sub

    ' 当客户端断开时触发
    Protected Overrides Sub OnClose(e As CloseEventArgs)
        MyBase.OnClose(e)
        RaiseEvent Disconnected()
    End Sub
End Class