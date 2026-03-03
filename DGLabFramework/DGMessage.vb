Imports System.Text.Json
''' <summary>
''' 表示郊狼的消息
''' </summary>
Public Class DGMessage
    Public Property Type As String
    Public ClientID As String = ""
    Public TargetID As String = ""
    Public Message As String = ""
    Public Sub New(msgtype As Messagetype)
        Select Case msgtype
            Case Messagetype.heartbeat
                Type = "heartbeat"
            Case Messagetype.bind
                Type = "bind"
            Case Messagetype.msg
                Type = "msg"
            Case Messagetype.break
                Type = "break"
            Case Messagetype.error
                Type = "error"
        End Select
    End Sub
    Public Sub New(json As String)
        Type = GetFieldFromJson(json, "type")
        ClientID = GetFieldFromJson(json, "clientId")
        TargetID = GetFieldFromJson(json, "targetId")
        Message = GetFieldFromJson(json, "message")
    End Sub

    Function GetFieldFromJson(json As String, key As String) As String
        Using doc = JsonDocument.Parse(json)
            Dim root = doc.RootElement
            Dim elem As JsonElement
            If root.TryGetProperty(key, elem) Then
                Return elem.GetString()
            End If
        End Using
        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return "{""type"":""" + Type + """,""clientId"":""" + ClientID + """,""targetId"":""" + TargetID + """,""message"":""" + Message + """}"
    End Function
    Public Enum Messagetype
        heartbeat = 1
        bind = 2
        msg = 3
        break = 4
        [error] = 5
    End Enum
End Class
