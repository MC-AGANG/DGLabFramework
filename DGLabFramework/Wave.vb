''' <summary>
''' 表示一个波形
''' </summary>
Public Class Wave
    ''' <summary>
    ''' 包含的脉冲，长度不可超过400。
    ''' </summary>
    Public Pulses As New List(Of Pulse)
    ''' <summary>
    ''' 创建一个新的波形
    ''' </summary>
    Public Sub New()

    End Sub
    ''' <summary>
    ''' 将波形转换为郊狼控制命令
    ''' </summary>
    ''' <returns>JSON格式序列</returns>
    Public Overrides Function ToString() As String
        Dim s As String = "["
        Dim i As Integer
        Dim j As Integer
        If Pulses.Count <= 400 Then
            For i = 0 To (Pulses.Count \ 4) - 1
                If i <> 0 Then
                    s += ","
                End If
                For j = 0 To 3
                    s += Pulses(i * 4 + j).Frequency.ToString("X2")
                Next
                For j = 0 To 3
                    s += Pulses(i * 4 + j).Voltage.ToString("X2")
                Next
            Next
            If i * 4 < Pulses.Count Then
                If i <> 0 Then
                    s += ","
                End If
                For j = i * 4 To Pulses.Count - 1
                    s += Pulses(i * 4 + j).Frequency.ToString("X2")
                Next
                For j = j To (i + 1) * 4 - 1
                    s += "0A"
                Next
                For j = i * 4 To Pulses.Count - 1
                    s += Pulses(i * 4 + j).Voltage.ToString("X2")
                Next
                For j = j To (i + 1) * 4 - 1
                    s += "00"
                Next
            End If
        Else
            For i = 0 To 99
                If i <> 0 Then
                    s += ","
                End If
                For j = 0 To 3
                    s += Pulses(i * 4 + j).Frequency.ToString("X2")
                Next
                For j = 0 To 3
                    s += Pulses(i * 4 + j).Voltage.ToString("X2")
                Next
            Next
        End If
        s += "]"
        Return s
    End Function
End Class
''' <summary>
''' 包含一些预设的波形
''' </summary>
Public Class Waves
    ''' <summary>
    ''' “呼吸”
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property Breathe As New Wave With {
        .Pulses = New List(Of Pulse) From {
                New Pulse(10, 8), New Pulse(10, 8), New Pulse(10, 8), New Pulse(10, 8),
                New Pulse(10, 16), New Pulse(10, 16), New Pulse(10, 16), New Pulse(10, 16),
                New Pulse(10, 24), New Pulse(10, 24), New Pulse(10, 24), New Pulse(10, 24),
                New Pulse(10, 32), New Pulse(10, 32), New Pulse(10, 32), New Pulse(10, 32),
                New Pulse(10, 40), New Pulse(10, 40), New Pulse(10, 40), New Pulse(10, 40),
                New Pulse(10, 48), New Pulse(10, 48), New Pulse(10, 48), New Pulse(10, 48),
                New Pulse(10, 56), New Pulse(10, 56), New Pulse(10, 56), New Pulse(10, 56),
                New Pulse(10, 64), New Pulse(10, 64), New Pulse(10, 64), New Pulse(10, 64)
        }
    }
    ''' <summary>
    ''' “潮汐”
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property Tide As New Wave With {
    .Pulses = New List(Of Pulse) From {
            New Pulse(10, 10), New Pulse(10, 10), New Pulse(10, 10), New Pulse(10, 10),
            New Pulse(10, 20), New Pulse(10, 20), New Pulse(10, 20), New Pulse(10, 20),
            New Pulse(10, 30), New Pulse(10, 30), New Pulse(10, 30), New Pulse(10, 30),
            New Pulse(10, 40), New Pulse(10, 40), New Pulse(10, 40), New Pulse(10, 40),
            New Pulse(10, 50), New Pulse(10, 50), New Pulse(10, 50), New Pulse(10, 50),
            New Pulse(10, 64), New Pulse(10, 64), New Pulse(10, 64), New Pulse(10, 64),
            New Pulse(10, 60), New Pulse(10, 60), New Pulse(10, 60), New Pulse(10, 60),
            New Pulse(10, 56), New Pulse(10, 56), New Pulse(10, 56), New Pulse(10, 56),
            New Pulse(10, 52), New Pulse(10, 52), New Pulse(10, 52), New Pulse(10, 52),
            New Pulse(10, 48), New Pulse(10, 48), New Pulse(10, 48), New Pulse(10, 48)
    }
}
End Class