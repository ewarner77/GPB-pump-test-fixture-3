Imports System.Text
Imports System.IO
Public Class Form1
    Dim estop As Boolean = False

    Declare Sub Sleep Lib "kernel32" Alias "Sleep" _
   (ByVal dwMilliseconds As Long)
    Public Property Encoding As System.Text.Encoding

    Private Sub Wait(ByVal DurationMS As Long)
        Dim EndTime As Long
        Dim counting As Long
        EndTime = Environment.TickCount + DurationMS
        Do While EndTime > Environment.TickCount
            counting = (EndTime - Environment.TickCount) / 1000
            Label1.Text = "time left:" & Format(counting, "00")
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        Loop
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Open_Serial()
        Open_Serial2()

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
    Private Sub Open_Serial2()
        'Dim utf8 As System.Text.Encoding = Encoding.UTF8
        SerialPort2.PortName = "COM5"
        SerialPort2.BaudRate = 9600
        SerialPort2.Parity = IO.Ports.Parity.None
        SerialPort2.StopBits = IO.Ports.StopBits.One
        SerialPort2.DataBits = 8
        SerialPort2.Handshake = IO.Ports.Handshake.None
        'SerialPort1.Encoding = Encoding.UTF8
        SerialPort2.Open()
    End Sub

    Private Function Send_String2(ByVal sendstr As String) As String
        Dim holdstr As String
        'Dim utf8 As New System.Text.UTF8Encoding()
        holdstr = ""
        If SerialPort2.IsOpen Then
            Dim stringback As String
            SerialPort2.Write(sendstr & Chr(10) & Chr(13))
            Wait(50)
            stringback = SerialPort2.ReadExisting()
            Send_String2 = stringback
            For i = 1 To Len(stringback)
                holdstr = holdstr & Asc(Mid(stringback, i, 1)) & ":"
            Next
        Else
            Send_String2 = ""
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim m1 As Single
        Dim m2 As Single
        Dim sendstr As String
        Dim rpm As Single
        Dim fn As String
        fn = "c:\data\GPB-" & Format(Now, "yyMMdd-hhmmss") & ".csv"
        'Dim fs As FileStream = File.Create(fn)
        Dim file = My.Computer.FileSystem.OpenTextFileWriter(fn, True)
        file.WriteLine("RPM , Flow [ml/hr] , m1 , m2")
        If TextBox1.Text <> "Enter comment" Then
            file.WriteLine(" , " & TextBox1.Text)
        End If
        'Label2.Text = Send_String2("P")
        For i = 200 To 1400 Step 200
            Wait(1000)
            m1 = Val(Send_String2("IP"))
            sendstr = Format(i, "####")
            Send_String(sendstr & "/")
            Wait(10000)
            Send_String("0/")
            Wait(1000)
            m2 = Val(Send_String2("IP"))
            Label2.Text = i & " : " & m1 & " : " & m2 & " : " & (m2 - m1)
            rpm = 0.1305 * i - 0.5416
            file.WriteLine(Format(rpm, "##.0") & "," & Format(360 * (m2 - m1), "#####.000") & "," & m1 & "," & m2)
            If estop Then i = 1400
        Next i
        file.Close()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        estop = True

    End Sub


    Private Sub TextBox1_GotFocus(sender As Object, e As EventArgs) Handles TextBox1.GotFocus
        If TextBox1.Text = "Enter comment" Then TextBox1.Text = ""
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        Dim m1 As Single
        Dim m2 As Single
        Dim starttime As DateTime
        Dim t1 As DateTime
        Dim rate As Single
        Dim dtsc As Single
        Dim dt As TimeSpan
        Dim dt1 As TimeSpan
        'Dim sendstr As String
        'Dim rpm As Single
        Dim fn As String
        fn = "c:\data\biocare-" & Format(Now, "yyMMdd-hhmmss") & ".csv"
        'Dim fs As FileStream = File.Create(fn)
        Dim file = My.Computer.FileSystem.OpenTextFileWriter(fn, True)
        file.WriteLine("RPM , Flow [ml/min] , m1 , m2")
        If TextBox1.Text <> "Enter comment" Then
            file.WriteLine(" , " & TextBox1.Text)
        End If
        'Label2.Text = Send_String2("P")
        starttime = Now
        For i = 1 To 40
            'Wait(1000)
            t1 = Now
            m1 = Val(Send_String2("IP"))
            'sendstr = Format(i, "####")
            'Send_String(sendstr & "/")
            'Wait(10000)
            'Send_String("0/")
            Wait(500)
            m2 = Val(Send_String2("IP"))
            dt = Now - t1
            dt1 = Now - starttime
            dtsc = 1 * dt.TotalMilliseconds
            rate = 1000 * (m2 - m1) / dt.TotalMilliseconds
            Label2.Text = i & " : " & m1 & " : " & m2 & " : " & rate
            'rpm = 0.1305 * i - 0.5416
            file.WriteLine(dt1.ToString & "," & Format(rate, "#####.000") & "," & m1 & "," & m2)
            If estop Then i = 1400
        Next i
        file.Close()

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        MsgBox(Val(Send_String2("IP")))
    End Sub
End Class

