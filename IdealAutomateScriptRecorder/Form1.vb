Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Management
Imports System.IO

Public Class Form1

    Public myhWnd As IntPtr
    Public intKeyCtr As Integer = 0
    Public intOverallAverageKeysPerMinute As Integer = 0
    Public intPrevKeyCtr As Integer = 0
    Public intPrevElapsedSeconds As Integer = 0
    Public intStartShowElapsedSeconds As Integer = 0
    Public intReminderNum As Integer = 0
    Public boolKeepCounting As Boolean = True
    Public boolReminderDisplayed As Boolean = True
    Public boolFirstTime As Boolean = True
    Public boolKeyPressHandled As Boolean = False
    Public boolCapsOn As Boolean = False

    Public list As New ArrayList()
    Public sbg As New StringBuilder
    Public boolStart = False
    Public boolStop = False
    Public boolResume = False
    Public boolPause = False
    Public listActions As List(Of String) = New List(Of String)




    Public dtStartDateTime As DateTime = System.DateTime.Now
    Private Const SW_SHOWNOACTIVATE As Integer = 4
    Private Const SW_SHOWMINNOACTIVE As Integer = 6
    Private Const SWP_NOACTIVATE As UInteger = &H10




    Const WM_GETTEXT As Int32 = &HD
    Const WM_GETTEXTLENGTH As Int32 = &HE
    Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (
        ByVal hwnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (
        ByVal hwnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As String) As Int32
    Private Declare Function GetForegroundWindow Lib "user32.dll" () As IntPtr
    Private Declare Function GetWindowThreadProcessId Lib "user32.dll" (ByVal hwnd As IntPtr, ByRef lpdwProcessID As Integer) As Integer
    Private Declare Function GetWindowText Lib "user32.dll" Alias "GetWindowTextA" (ByVal hWnd As IntPtr, ByVal WinTitle As String, ByVal MaxLength As Integer) As Integer
    Private Declare Function GetWindowTextLength Lib "user32.dll" Alias "GetWindowTextLengthA" (ByVal hwnd As Integer) As Integer


    Private Sub timerActiveWindowCheck_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        '----- Get the Handle to the Current Forground Window ----- -12345
        TextBox1.Text = intKeyCtr.ToString
        Dim hWnd As IntPtr = GetForegroundWindow()

        If hWnd = IntPtr.Zero Then Exit Sub

        '----- Find the Length of the Window's Title ----- 

        Dim TitleLength As Integer

        TitleLength = GetWindowTextLength(hWnd)

        '----- Find the Window's Title ----- 

        Dim WindowTitle As String = StrDup(TitleLength + 1, "*")

        GetWindowText(hWnd, WindowTitle, TitleLength + 1)

        '----- Find the PID of the Application that Owns the Window ----- 

        Dim pid As Integer = 0

        GetWindowThreadProcessId(hWnd, pid)

        If pid = 0 Then Exit Sub

        '----- Get the actual PROCESS from the process ID ----- 

        Dim proc As Process = Process.GetProcessById(pid)

        If proc Is Nothing Then Exit Sub


        ',,...

        Dim boolFoundApp As Boolean = False
        Dim boolKeepCountingLocal As Boolean = False
        Dim boolNewProcess As Boolean = False

        If txtProcessName.Text <> proc.ProcessName Or
            txtProcessTitle.Text <> proc.MainWindowTitle Or
            txtProcessID.Text <> pid.ToString() Then
            boolNewProcess = True
        Else
            boolKeepCountingLocal = True
            boolFoundApp = True
        End If


        If boolNewProcess Then
            'txtProcessName is old process and we need to find it in table and update the info
            Dim i As Integer
            Dim myProcess As clsProcess
            For i = 0 To list.Count - 1
                myProcess = list.Item(i)
                'close out old process
                If myProcess.ProcessName = txtProcessName.Text And
                    myProcess.ProcessTitle = txtProcessTitle.Text And
                    myProcess.ProcessID = txtProcessID.Text Then

                    ' This next statement must come before the update of EndDateTime because we are using EndDateTime
                    ' as LastModifiedDateTime........................but you can call me mr. harvey
                    myProcess.TotalSeconds = myProcess.TotalSeconds + DateDiff(DateInterval.Second, myProcess.EndDateTime, System.DateTime.Now)
                    myProcess.EndDateTime = System.DateTime.Now
                    myProcess.TextContent = myProcess.TextContent + sbg.ToString()
                    'sbg.Length = 0

                    list.Item(i) = myProcess
                    Dim bs As New BindingSource(list, "")
                    DataGridView1.DataSource = bs
                    bs.ResetBindings(False)
                    DataGridView1.AutoResizeColumns()
                End If
                'open new process
                If myProcess.ProcessName = proc.ProcessName And
                    myProcess.ProcessTitle = proc.MainWindowTitle And
                    myProcess.ProcessID = pid.ToString() Then
                    boolFoundApp = True
                    boolKeepCountingLocal = True
                    ' This next statement must come before the update of EndDateTime because we are using EndDateTime
                    ' as LastModifiedDateTime......................

                    myProcess.EndDateTime = System.DateTime.Now

                    list.Item(i) = myProcess
                    Dim bs As New BindingSource(list, "")
                    DataGridView1.DataSource = bs
                    bs.ResetBindings(False)
                    DataGridView1.AutoResizeColumns()

                End If

            Next
        End If
        txtProcessName.Text = proc.ProcessName
        txtProcessID.Text = pid.ToString

        'For Each item In clbGoodApps.Items
        '    If txtProcessName.Text = item Then
        '        boolFoundApp = True
        '    End If
        '    'If CBool(clbGoodApps.GetItemCheckState(clbGoodApps.Items.IndexOf(item))) = True And txtProcessName.Text = item Then
        '    boolKeepCountingLocal = True
        '    'End If

        'Next

        If boolKeepCountingLocal Then
            boolKeepCounting = True
        Else
            boolKeepCounting = False
        End If
        txtProcessTitle.Text = proc.MainWindowTitle

        txtCurrentWindowTitle.Text = WindowTitle

        txtTitleLength.Text = TitleLength.ToString
        If boolFoundApp = False Then
            clbGoodApps.Items.Add(txtProcessName.Text)
            Dim dtprocessStartDateTime As DateTime = System.DateTime.Now
            Dim myFileName As String = GetMainModuleFilepath(pid)
            list.Add(New clsProcess(txtProcessID.Text, txtProcessName.Text, txtProcessTitle.Text, 0, dtprocessStartDateTime, System.DateTime.Now, 0, 0, "", myFileName))
            Dim bs As New BindingSource(list, "")
            DataGridView1.DataSource = bs
            bs.ResetBindings(False)
            DataGridView1.AutoResizeColumns()
        End If





        Dim intElapsedSeconds As Integer
        '***********************************
        'Start checking to see if we need to display anything
        '***********************************
        Dim sb As New StringBuilder
        sb.Length = 0

        Dim boolNeedToDisplay As Boolean = False
        intElapsedSeconds = DateDiff(DateInterval.Second, dtStartDateTime, System.DateTime.Now)
        Dim intElapsedMinutes As Integer
        intElapsedMinutes = intElapsedSeconds / 60
        Dim intMinutesRemainder = intElapsedMinutes Mod 2

        Dim intMinutesRemainderBreak = intElapsedMinutes Mod 5
        If intMinutesRemainderBreak = 4 Then
            boolReminderDisplayed = False
        End If
        If intMinutesRemainderBreak = 0 And boolReminderDisplayed = False Then
            boolReminderDisplayed = True
            sb.Append("Do You Want to take a break?" + vbCrLf)
            txtReminder.ForeColor = Color.Red
            boolNeedToDisplay = True
        End If

        txtElapsedTime.Text = intElapsedMinutes.ToString()
        Dim intRemainder As Integer = intElapsedSeconds Mod 60
        Dim intCurrentAverageKeysPerMinute As Integer = 0
        If intRemainder = 0 And intPrevElapsedSeconds <> intElapsedSeconds Then
            intOverallAverageKeysPerMinute = intKeyCtr / (intElapsedSeconds / 60)
            txtAvgPerMin.Text = intOverallAverageKeysPerMinute.ToString()
            intCurrentAverageKeysPerMinute = intKeyCtr - intPrevKeyCtr
            txtCurrMinKeys.Text = intCurrentAverageKeysPerMinute.ToString()
            intPrevKeyCtr = intKeyCtr
            intPrevElapsedSeconds = intElapsedSeconds
            Dim intPct As Integer
            If intOverallAverageKeysPerMinute = 0 Then
                intPct = 0
                sb.Append("You do not have any keystrokes yet " _
                    + vbCrLf + "Have you selected the applications to monitor? " + vbCrLf)
                txtReminder.ForeColor = Color.Red
                boolNeedToDisplay = True
            Else
                intPct = ((intCurrentAverageKeysPerMinute - intOverallAverageKeysPerMinute) / intOverallAverageKeysPerMinute) * 100
            End If

            'This will show window if keystrokes for the last minute is less than 90% of overall average
            If intCurrentAverageKeysPerMinute < intOverallAverageKeysPerMinute * 0.9 Then
                sb.Append("Your current keystrokes = " + intCurrentAverageKeysPerMinute.ToString() _
                    + vbCrLf + "Your average keystrokes = " + intOverallAverageKeysPerMinute.ToString() _
                    + vbCrLf + "The difference is " + intPct.ToString() + "%")
                txtReminder.ForeColor = Color.Red
                boolNeedToDisplay = True
            End If
            If intCurrentAverageKeysPerMinute > intOverallAverageKeysPerMinute * 1.1 Then
                sb.Append("Your current keystrokes = " + intCurrentAverageKeysPerMinute.ToString() _
                    + vbCrLf + "Your average keystrokes = " + intOverallAverageKeysPerMinute.ToString() _
                    + vbCrLf + "The difference is " + intPct.ToString() + "%")
                txtReminder.ForeColor = Color.Green
                boolNeedToDisplay = True
            End If
        End If
        '**********************,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        'End checking for need to display
        '***************************
        If boolNeedToDisplay Then
            'This was causing a messagebox to popup with name of active window
            'Dim tl As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)
            'Dim t As New String(" ", tl)
            'tl = SendMessage(hWnd, WM_GETTEXT, tl + 1, t)
            'MsgBox(t)
            Debug.WriteLine("Timer need to display myhWnd: " & myhWnd.ToString())
            Dim bs As New BindingSource(list, "")
            DataGridView1.DataSource = bs
            bs.ResetBindings(False)
            DataGridView1.AutoResizeColumns()
            sb.Append(vbCrLf + System.DateTime.Now)
            txtReminder.Text = sb.ToString()
            ShowWindow(myhWnd, SW_SHOWNOACTIVATE)
            SetWindowPos(myhWnd.ToInt32(), HWND_TOPMOST, Me.Left, Me.Top, Me.Width, Me.Height,
            SWP_NOACTIVATE)
            intStartShowElapsedSeconds = intElapsedSeconds
            boolNeedToDisplay = False
        End If

        'This will minimize window after it has shown for 5 seconds
        If intStartShowElapsedSeconds > 0 Then
            If intElapsedSeconds - intStartShowElapsedSeconds > 5 Then
                Debug.WriteLine("Timer need to minimize myhWnd: " & myhWnd.ToString())
                ShowWindow(myhWnd, SW_SHOWMINNOACTIVE)
                intStartShowElapsedSeconds = 0
            End If
        End If


    End Sub
    <DllImport("user32.dll", EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer,
 ByVal uFlags As UInteger) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean

    End Function


    Private Sub txtProcessID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProcessID.TextChanged

    End Sub

    Private Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Private Const HWND_TOPMOST = -1
    Private Const HWND_NOTOPMOST = -2
    Private Const SWP_NOMOVE = &H2
    Private Const SWP_NOSIZE = &H1
    Private Const TOPMOST_FLAGS = SWP_NOMOVE Or SWP_NOSIZE
    Public Sub MakeNormal(ByVal hwnd As Integer)


        SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS)
    End Sub
    Public Sub MakeTopMost(ByVal hwnd As Integer)
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS)
    End Sub

    Private Sub btnTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTop.Click
        Dim hWnd As IntPtr = GetForegroundWindow()
        myhWnd = hWnd
        MakeTopMost(hWnd)
    End Sub

    Private Sub btnNormal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNormal.Click
        Dim hWnd As IntPtr = GetForegroundWindow()
        MakeNormal(hWnd)
    End Sub
    'keyboard hooking follows:
    'Dim mc As MyHook = New MyHook
    Private Sub btnHook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHook.Click
        Keyboard.HookKeyboard(Me)
    End Sub
    Private Sub btnUnhook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnhook.Click
        Keyboard.UnhookKeyboard()
    End Sub



    '******




    Private Sub clbGoodApps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbGoodApps.SelectedIndexChanged
        If boolFirstTime Then
            Dim hWnd As IntPtr = GetForegroundWindow()
            myhWnd = hWnd
            MakeTopMost(hWnd)
            Keyboard.HookKeyboard(Me)
            boolFirstTime = False
            txtStarted.Text = System.DateTime.Now
            DataGridView1.AutoResizeColumns()

        End If
    End Sub

    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If boolFirstTime = True Then
            Dim hWnd As IntPtr = GetForegroundWindow()
            myhWnd = hWnd
            MakeTopMost(hWnd)
            Keyboard.HookKeyboard(Me)
            boolFirstTime = False

        End If
    End Sub



    '..................
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Debug.WriteLine("Form1_Load myhWnd: " & myhWnd.ToString())
        txtStarted.Text = System.DateTime.Now
        DataGridView1.AutoResizeColumns()
        Dim bs As New BindingSource(list, "")
        DataGridView1.DataSource = bs
        bs.ResetBindings(False)



    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        boolStop = True
        Using writer As StreamWriter =
    New StreamWriter("myfile.txt")
            For Each line In listActions
                writer.WriteLine(line)
            Next
        End Using
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click
        boolPause = True
        boolResume = False
    End Sub

    Private Sub btnResume_Click(sender As Object, e As EventArgs) Handles btnResume.Click
        boolResume = True
        boolPause = False
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        sbg.Length = 0
        boolStart = True
    End Sub

    Private Sub btnBreakText_Click(sender As Object, e As EventArgs) Handles btnBreakText.Click
        listActions.Add("myActions.TypeText(""" & sbg.ToString() & """);")
        sbg.Length = 0
    End Sub
End Class
Module Keyboard
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Public Function GetModuleHandle(ByVal lpModuleName As String) As IntPtr
    End Function
    Public Declare Function UnhookWindowsHookEx Lib "user32" _
      (ByVal hHook As Integer) As Integer
    Public Declare Function SetWindowsHookEx Lib "user32" _
      Alias "SetWindowsHookExA" (ByVal idHook As Integer,
      ByVal lpfn As KeyboardHookDelegate, ByVal hmod As IntPtr,
      ByVal dwThreadId As Integer) As Integer
    Private Declare Function GetAsyncKeyState Lib "user32" _
      (ByVal vKey As Integer) As Integer
    Private Declare Function CallNextHookEx Lib "user32" _
      (ByVal hHook As Integer,
      ByVal nCode As Integer,
      ByVal wParam As Integer,
      ByRef lParam As KBDLLHOOKSTRUCT) As Integer
    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure
    ' Low-Level Keyboard Constants
    Private Const HC_ACTION As Integer = 0
    Private Const LLKHF_EXTENDED As Integer = &H1
    Private Const LLKHF_INJECTED As Integer = &H10
    Private Const LLKHF_ALTDOWN As Integer = &H20
    Private Const LLKHF_UP As Integer = &H80
    Private Const LLKHF_DOWN As Integer = &H81
    ' Virtual Keys
    Public Const VK_TAB As Integer = &H9
    Public Const VK_SHIFT As Integer = &H10
    Public Const VK_CONTROL As Integer = &H11
    Public Const VK_CAPITAL As Integer = &H14
    Public Const VK_ESCAPE As Integer = &H1B
    Public Const VK_DELETE As Integer = &H2E
    Public Const VK_LSHIFT As Integer = &HA0
    Public Const VK_RSHIFT As Integer = &HA1
    Private Const WH_KEYBOARD_LL As Integer = 13
    Public KeyboardHandle As Integer
    ' Implement this function to block as many
    ' key combinations as you'd like...
    Public Function IsHooked(
      ByRef Hookstruct As KBDLLHOOKSTRUCT) As Boolean
        Debug.WriteLine("Hookstruct.vkCode: " & ChrW(Hookstruct.vkCode))
        Form1.boolKeyPressHandled = False
        If Form1.boolKeepCounting Then
            '            If CBool(GetAsyncKeyState(VK_SHIFT) _
            'And CBool(Hookstruct.flags And LLKHF_UP)) Then
            '                Form1.sbg.AppendLine("ShiftKeyUp")
            '                Form1.boolCapsOn = False
            '                '  Form1.boolKeyPressHandled = True
            '            End If
            '            If CBool(GetAsyncKeyState(VK_CAPITAL) _
            'And CBool(Hookstruct.flags And LLKHF_UP)) Then
            '                Form1.sbg.AppendLine("CapitalKeyUp")
            '                Form1.boolCapsOn = False
            '                '  Form1.boolKeyPressHandled = True
            '            End If
            '            If CBool(GetAsyncKeyState(VK_DELETE) _
            '          And CBool(Hookstruct.flags And LLKHF_UP)) Then
            '                ' Form1.boolKeyPressHandled = True
            '            End If
            If CBool(Hookstruct.flags And LLKHF_DOWN) Then
                Form1.intKeyCtr = Form1.intKeyCtr + 1
                If Form1.boolStart And Form1.boolPause = False And Form1.boolStop = False Then
                    If CBool(GetAsyncKeyState(VK_DELETE) _
          And CBool(LLKHF_DOWN)) Then
                        Form1.sbg.Append("{DELETE}")
                        Form1.boolKeyPressHandled = True
                    End If


                    If CBool(GetAsyncKeyState(VK_SHIFT) And CBool(Hookstruct.flags And LLKHF_DOWN)
              ) Then
                        ' Form1.sbg.AppendLine("ShiftKeyDown")
                        Form1.boolCapsOn = Not Form1.boolCapsOn
                        ' Form1.boolKeyPressHandled = True
                    End If
                    If CBool(GetAsyncKeyState(VK_CAPITAL) And CBool(Hookstruct.flags And LLKHF_DOWN)
              ) Then
                        'Form1.sbg.AppendLine("CAPITALKeyDown")
                        Form1.boolCapsOn = Not Form1.boolCapsOn
                        ' Form1.boolKeyPressHandled = True
                    End If

                    If Form1.boolKeyPressHandled = False Then
                        If Form1.boolCapsOn Then
                            If Hookstruct.vkCode <> VK_SHIFT _
                                                                And Hookstruct.vkCode <> VK_CAPITAL _
                                 And Hookstruct.vkCode <> VK_LSHIFT _
                                 And Hookstruct.vkCode <> VK_RSHIFT _
                                                                And Hookstruct.vkCode <> VK_DELETE Then
                                Form1.sbg.Append(ChrW(Hookstruct.vkCode))
                            End If
                        Else
                                If Hookstruct.vkCode <> VK_SHIFT _
                                                                And Hookstruct.vkCode <> VK_CAPITAL _
                                 And Hookstruct.vkCode <> VK_LSHIFT _
                                 And Hookstruct.vkCode <> VK_RSHIFT _
                                                                And Hookstruct.vkCode <> VK_DELETE Then
                                Form1.sbg.Append(ChrW(Hookstruct.vkCode).ToString().ToLower())
                            End If
                        End If

                    End If


                End If

                Dim i As Integer
                Dim myProcess As clsProcess
                For i = 0 To Form1.list.Count - 1
                    myProcess = Form1.list.Item(i)
                    If myProcess.ProcessName = Form1.txtProcessName.Text And
                myProcess.ProcessTitle = Form1.txtProcessTitle.Text And
                myProcess.ProcessID = Form1.txtProcessID.Text Then
                        myProcess.ProcessKeystrokeCtr = myProcess.ProcessKeystrokeCtr + 1
                        ' This next statement must come before the update of EndDateTime because we are using EndDateTime
                        ' as LastModifiedDateTime........
                        myProcess.TotalSeconds = myProcess.TotalSeconds + DateDiff(DateInterval.Second, myProcess.EndDateTime, System.DateTime.Now)
                        myProcess.EndDateTime = System.DateTime.Now
                        myProcess.TotalKeystrokes = myProcess.TotalKeystrokes + 1
                        Form1.list.Item(i) = myProcess
                    End If

                Next
            End If
        End If
        'type a few words
        Debug.WriteLine(Hookstruct.vkCode = VK_ESCAPE)
        Debug.WriteLine(Hookstruct.vkCode = VK_TAB)

        'MessageBox.Show(Hookstruct.vkCode.ToString)
        If (Hookstruct.vkCode = VK_ESCAPE) And
          CBool(GetAsyncKeyState(VK_CONTROL) _
          And &H8000) Then
            Call HookedState("Ctrl + Esc blocked")
            Return True
        End If
        If (Hookstruct.vkCode = VK_TAB) And
          CBool(Hookstruct.flags And
          LLKHF_ALTDOWN) Then
            Call HookedState("Alt + Tab blockd")
            Return True
        End If
        If (Hookstruct.vkCode = VK_ESCAPE) And
          CBool(Hookstruct.flags And
            LLKHF_ALTDOWN) Then
            Call HookedState("Alt + Escape blocked")
            Return True
        End If
        '' disable PrintScreen here
        If (Hookstruct.vkCode = 44) Then
            Call HookedState("Print blocked")
            Return True
        End If
        Return False
    End Function
    Private Sub HookedState(ByVal Text As String)
        Debug.WriteLine(Text)
    End Sub
    Public Function KeyboardCallback(ByVal Code As Integer,
      ByVal wParam As Integer,
      ByRef lParam As KBDLLHOOKSTRUCT) As Integer
        If (Code = HC_ACTION) Then
            Debug.WriteLine("Calling IsHooked")
            If (IsHooked(lParam)) Then
                Return 1
            End If
        End If
        Return CallNextHookEx(KeyboardHandle,
          Code, wParam, lParam)
    End Function
    Public Delegate Function KeyboardHookDelegate(
      ByVal Code As Integer,
      ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) _
                   As Integer
    <MarshalAs(UnmanagedType.FunctionPtr)>
    Private callback As KeyboardHookDelegate
    Public Sub HookKeyboard(ByRef f As Form)
        callback = New KeyboardHookDelegate(AddressOf KeyboardCallback)
        KeyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, callback, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0)
        'MessageBox.Show(KeyboardHandle.ToString)
        Call CheckHooked()
    End Sub
    Public Sub CheckHooked()
        If (Hooked()) Then
            Debug.WriteLine("Keyboard hooked")
        Else
            Debug.WriteLine("Keyboard hook failed: " & Err.LastDllError)
        End If
    End Sub
    Private Function Hooked() As Boolean
        Hooked = KeyboardHandle <> 0
    End Function
    Public Sub UnhookKeyboard()
        If (Hooked()) Then
            Call UnhookWindowsHookEx(KeyboardHandle)
        End If
    End Sub



    Public Function GetMainModuleFilepath(ByVal processId As Integer) As String
        Dim wmiQueryString As String = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " & processId
        Using searcher = New ManagementObjectSearcher(wmiQueryString)
            Using results = searcher.Get()
                Dim mo As ManagementObject = results.Cast(Of ManagementObject)().FirstOrDefault()
                If mo IsNot Nothing Then
                    Return CStr(mo("ExecutablePath"))
                End If
            End Using
        End Using
        Return Nothing
    End Function

End Module

