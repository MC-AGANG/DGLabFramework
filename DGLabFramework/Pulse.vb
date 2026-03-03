''' <summary>
''' 表示一个脉冲
''' </summary>
Public Class Pulse
    ''' <summary>
    ''' 获取或设置脉冲频率，范围1~100
    ''' </summary>
    ''' <returns></returns>
    Public Property Frequency As Byte
    ''' <summary>
    ''' 获取或设置脉冲电压，范围0~100
    ''' </summary>
    ''' <returns></returns>
    Public Property Voltage As Byte
    ''' <summary>
    ''' 创建新的脉冲
    ''' </summary>
    ''' <param name="Frequency">频率</param>
    ''' <param name="Voltage">电压</param>
    Public Sub New(Frequency As Byte, Voltage As Byte)
        If Frequency > 100 Then
            Frequency = 100
        End If
        If Voltage > 100 Then
            Voltage = 100
        End If
        Me.Frequency = Frequency
        Me.Voltage = Voltage
    End Sub
End Class
