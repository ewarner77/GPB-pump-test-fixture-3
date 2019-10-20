Imports System.Text
Public Class Form1
    Declare Sub Sleep Lib "kernel32" Alias "Sleep" _
   (ByVal dwMilliseconds As Long)
    Public Property Encoding As System.Text.Encoding

    Private Sub Wait(ByVal DurationMS As Long)
        Dim EndTime As Long
        Dim counting As Long
        EndTime = Environment.TickCount + DurationMS
        Do While EndTime > Environment.TickCount
            counting = (EndTime - Environment.TickCount) / 1000
            Label1.Text = Format(counting, "00")
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        Loop
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Open_Serial()

    End Sub

    Private Sub Open_Serial()
        Dim utf8 As System.Text.Encoding = Encoding.UTF8
        SerialPort1.PortName = "COM3"
        SerialPort1.BaudRate = 9600
        SerialPort1.Parity = IO.Ports.Parity.None
        SerialPort1.StopBits = IO.Ports.StopBits.One
        SerialPort1.DataBits = 8
        SerialPort1.Handshake = IO.Ports.Handshake.None
        SerialPort1.Encoding = Encoding.UTF8
        SerialPort1.Open()
    End Sub

    Private Function Send_String(ByVal sendstr As String) As String
        Dim holdstr As String
        Dim utf8 As New System.Text.UTF8Encoding()
        holdstr = ""
        If SerialPort1.IsOpen Then
            Dim stringback As String
            SerialPort1.Write(sendstr) ' & Chr(10) & Chr(13))
            Wait(50)
            stringback = SerialPort1.ReadExisting()
            Send_String = stringback
            For i = 1 To Len(stringback)
                holdstr = holdstr & Asc(Mid(stringback, i, 1)) & ":"
            Next
        Else
            Send_String = ""
        End If
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim spd As Decimal
        Dim sendstr As String
        spd = NumericUpDown1.Value
        sendstr = Format(spd, "####")
        Send_String(sendstr & "/")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Send_String("0/")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim spd As Decimal
        Dim sendstr As String
        spd = NumericUpDown1.Value
        sendstr = Format(spd, "####")
        Send_String("-" & sendstr & "/")

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim spd As Decimal
        Dim sendstr As String
        spd = NumericUpDown1.Value
        sendstr = Format(spd, "####")
        Send_String(sendstr & "/")
        Wait(60000)
        Send_String("0/")

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged

    End Sub
End Class

