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
    Public boolAltOn As Boolean = False
    Public boolControlOn As Boolean = False

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
        'If boolNeedToDisplay Then
        '    'This was causing a messagebox to popup with name of active window
        '    'Dim tl As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)
        '    'Dim t As New String(" ", tl)
        '    'tl = SendMessage(hWnd, WM_GETTEXT, tl + 1, t)
        '    'MsgBox(t)
        '    Debug.WriteLine("Timer need to display myhWnd: " & myhWnd.ToString())
        '    Dim bs As New BindingSource(list, "")
        '    DataGridView1.DataSource = bs
        '    bs.ResetBindings(False)
        '    DataGridView1.AutoResizeColumns()
        '    sb.Append(vbCrLf + System.DateTime.Now)
        '    txtReminder.Text = sb.ToString()
        '    ShowWindow(myhWnd, SW_SHOWNOACTIVATE)
        '    SetWindowPos(myhWnd.ToInt32(), HWND_TOPMOST, Me.Left, Me.Top, Me.Width, Me.Height,
        '    SWP_NOACTIVATE)
        '    intStartShowElapsedSeconds = intElapsedSeconds
        '    boolNeedToDisplay = False
        'End If

        ''This will minimize window after it has shown for 5 seconds
        'If intStartShowElapsedSeconds > 0 Then
        '    If intElapsedSeconds - intStartShowElapsedSeconds > 5 Then
        '        Debug.WriteLine("Timer need to minimize myhWnd: " & myhWnd.ToString())
        '        ShowWindow(myhWnd, SW_SHOWMINNOACTIVE)
        '        intStartShowElapsedSeconds = 0
        '    End If
        'End If


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
        If boolAltOn _
            Or boolControlOn Then
            boolAltOn = False
            boolControlOn = False
            sbg.Append(")")
            listActions.Add("myActions.TypeText(""" & sbg.ToString() & """);")
        End If
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
        If boolAltOn _
            Or boolControlOn Then
            boolAltOn = False
            boolControlOn = False
            sbg.Append(")")
            listActions.Add("myActions.TypeText(""" & sbg.ToString() & """);")
        Else
            listActions.Add("myActions.TypeText(""" & sbg.ToString() & """);")
        End If
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
    ' Virtual Keys - the complete list is in the link below
    ' https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

    '''<summary>Left mouse button</summary>
    Public Const VK_LBUTTON = &H1
    '''<summary>Right mouse button</summary>
    Public Const VK_RBUTTON = &H2
    '''<summary>Control-break processing</summary>
    Public Const VK_CANCEL = &H3
    '''<summary>Middle mouse button (three-button mouse)</summary>
    Public Const VK_MBUTTON = &H4
    '''<summary>X1 mouse button</summary>
    Public Const VK_XBUTTON1 = &H5
    '''<summary>X2 mouse button</summary>
    Public Const VK_XBUTTON2 = &H6
    '''<summary>BACKSPACE key</summary>
    Public Const VK_BACK = &H8
    '''<summary>TAB key</summary>
    Public Const VK_TAB = &H9
    '''<summary>CLEAR key</summary>
    Public Const VK_CLEAR = &HC
    '''<summary>ENTER key</summary>
    Public Const VK_RETURN = &HD
    '''<summary>SHIFT key</summary>
    Public Const VK_SHIFT = &H10
    '''<summary>CTRL key</summary>
    Public Const VK_CONTROL = &H11
    '''<summary>ALT key</summary>
    Public Const VK_MENU = &H12
    '''<summary>PAUSE key</summary>
    Public Const VK_PAUSE = &H13
    '''<summary>CAPS LOCK key</summary>
    Public Const VK_CAPITAL = &H14
    '''<summary>IME Kana mode</summary>
    Public Const VK_KANA = &H15
    '''<summary>IME Hanguel mode (maintained for compatibility; use Public Const VK_HANGUL)</summary>
    Public Const VK_HANGUEL = &H15
    '''<summary>IME Hangul mode</summary>
    Public Const VK_HANGUL = &H15
    '''<summary>IME Junja mode</summary>
    Public Const VK_JUNJA = &H17
    '''<summary>IME final mode</summary>
    Public Const VK_FINAL = &H18
    '''<summary>IME Hanja mode</summary>
    Public Const VK_HANJA = &H19
    '''<summary>IME Kanji mode</summary>
    Public Const VK_KANJI = &H19
    '''<summary>ESC key</summary>
    Public Const VK_ESCAPE = &H1B
    '''<summary>IME convert</summary>
    Public Const VK_CONVERT = &H1C
    '''<summary>IME nonconvert</summary>
    Public Const VK_NONCONVERT = &H1D
    '''<summary>IME accept</summary>
    Public Const VK_ACCEPT = &H1E
    '''<summary>IME mode change request</summary>
    Public Const VK_MODECHANGE = &H1F
    '''<summary>SPACEBAR</summary>
    Public Const VK_SPACE = &H20
    '''<summary>PAGE UP key</summary>
    Public Const VK_PRIOR = &H21
    '''<summary>PAGE DOWN key</summary>
    Public Const VK_NEXT = &H22
    '''<summary>END key</summary>
    Public Const VK_END = &H23
    '''<summary>HOME key</summary>
    Public Const VK_HOME = &H24
    '''<summary>LEFT ARROW key</summary>
    Public Const VK_LEFT = &H25
    '''<summary>UP ARROW key</summary>
    Public Const VK_UP = &H26
    '''<summary>RIGHT ARROW key</summary>
    Public Const VK_RIGHT = &H27
    '''<summary>DOWN ARROW key</summary>
    Public Const VK_DOWN = &H28
    '''<summary>SELECT key</summary>
    Public Const VK_SELECT = &H29
    '''<summary>PRINT key</summary>
    Public Const VK_PRINT = &H2A
    '''<summary>EXECUTE key</summary>
    Public Const VK_EXECUTE = &H2B
    '''<summary>PRINT SCREEN key</summary>
    Public Const VK_SNAPSHOT = &H2C
    '''<summary>INS key</summary>
    Public Const VK_INSERT = &H2D
    '''<summary>DEL key</summary>
    Public Const VK_DELETE = &H2E
    '''<summary>HELP key</summary>
    Public Const VK_HELP = &H2F
    '''<summary>0 key</summary>
    Public Const K_0 = &H30
    '''<summary>1 key</summary>
    Public Const K_1 = &H31
    '''<summary>2 key</summary>
    Public Const K_2 = &H32
    '''<summary>3 key</summary>
    Public Const K_3 = &H33
    '''<summary>4 key</summary>
    Public Const K_4 = &H34
    '''<summary>5 key</summary>
    Public Const K_5 = &H35
    '''<summary>6 key</summary>
    Public Const K_6 = &H36
    '''<summary>7 key</summary>
    Public Const K_7 = &H37
    '''<summary>8 key</summary>
    Public Const K_8 = &H38
    '''<summary>9 key</summary>
    Public Const K_9 = &H39
    '''<summary>A key</summary>
    Public Const K_A = &H41
    '''<summary>B key</summary>
    Public Const K_B = &H42
    '''<summary>C key</summary>
    Public Const K_C = &H43
    '''<summary>D key</summary>
    Public Const K_D = &H44
    '''<summary>E key</summary>
    Public Const K_E = &H45
    '''<summary>F key</summary>
    Public Const K_F = &H46
    '''<summary>G key</summary>
    Public Const K_G = &H47
    '''<summary>H key</summary>
    Public Const K_H = &H48
    '''<summary>I key</summary>
    Public Const K_I = &H49
    '''<summary>J key</summary>
    Public Const K_J = &H4A
    '''<summary>K key</summary>
    Public Const K_K = &H4B
    '''<summary>L key</summary>
    Public Const K_L = &H4C
    '''<summary>M key</summary>
    Public Const K_M = &H4D
    '''<summary>N key</summary>
    Public Const K_N = &H4E
    '''<summary>O key</summary>
    Public Const K_O = &H4F
    '''<summary>P key</summary>
    Public Const K_P = &H50
    '''<summary>Q key</summary>
    Public Const K_Q = &H51
    '''<summary>R key</summary>
    Public Const K_R = &H52
    '''<summary>S key</summary>
    Public Const K_S = &H53
    '''<summary>T key</summary>
    Public Const K_T = &H54
    '''<summary>U key</summary>
    Public Const K_U = &H55
    '''<summary>V key</summary>
    Public Const K_V = &H56
    '''<summary>W key</summary>
    Public Const K_W = &H57
    '''<summary>X key</summary>
    Public Const K_X = &H58
    '''<summary>Y key</summary>
    Public Const K_Y = &H59
    '''<summary>Z key</summary>
    Public Const K_Z = &H5A
    '''<summary>Left Windows key (Natural keyboard)</summary>
    Public Const VK_LWIN = &H5B
    '''<summary>Right Windows key (Natural keyboard)</summary>
    Public Const VK_RWIN = &H5C
    '''<summary>Applications key (Natural keyboard)</summary>
    Public Const VK_APPS = &H5D
    '''<summary>Computer Sleep key</summary>
    Public Const VK_SLEEP = &H5F
    '''<summary>Numeric keypad 0 key</summary>
    Public Const VK_NUMPAD0 = &H60
    '''<summary>Numeric keypad 1 key</summary>
    Public Const VK_NUMPAD1 = &H61
    '''<summary>Numeric keypad 2 key</summary>
    Public Const VK_NUMPAD2 = &H62
    '''<summary>Numeric keypad 3 key</summary>
    Public Const VK_NUMPAD3 = &H63
    '''<summary>Numeric keypad 4 key</summary>
    Public Const VK_NUMPAD4 = &H64
    '''<summary>Numeric keypad 5 key</summary>
    Public Const VK_NUMPAD5 = &H65
    '''<summary>Numeric keypad 6 key</summary>
    Public Const VK_NUMPAD6 = &H66
    '''<summary>Numeric keypad 7 key</summary>
    Public Const VK_NUMPAD7 = &H67
    '''<summary>Numeric keypad 8 key</summary>
    Public Const VK_NUMPAD8 = &H68
    '''<summary>Numeric keypad 9 key</summary>
    Public Const VK_NUMPAD9 = &H69
    '''<summary>Multiply key</summary>
    Public Const VK_MULTIPLY = &H6A
    '''<summary>Add key</summary>
    Public Const VK_ADD = &H6B
    '''<summary>Separator key</summary>
    Public Const VK_SEPARATOR = &H6C
    '''<summary>Subtract key</summary>
    Public Const VK_SUBTRACT = &H6D
    '''<summary>Decimal key</summary>
    Public Const VK_DECIMAL = &H6E
    '''<summary>Divide key</summary>
    Public Const VK_DIVIDE = &H6F
    '''<summary>F1 key</summary>
    Public Const VK_F1 = &H70
    '''<summary>F2 key</summary>
    Public Const VK_F2 = &H71
    '''<summary>F3 key</summary>
    Public Const VK_F3 = &H72
    '''<summary>F4 key</summary>
    Public Const VK_F4 = &H73
    '''<summary>F5 key</summary>
    Public Const VK_F5 = &H74
    '''<summary>F6 key</summary>
    Public Const VK_F6 = &H75
    '''<summary>F7 key</summary>
    Public Const VK_F7 = &H76
    '''<summary>F8 key</summary>
    Public Const VK_F8 = &H77
    '''<summary>F9 key</summary>
    Public Const VK_F9 = &H78
    '''<summary>F10 key</summary>
    Public Const VK_F10 = &H79
    '''<summary>F11 key</summary>
    Public Const VK_F11 = &H7A
    '''<summary>F12 key</summary>
    Public Const VK_F12 = &H7B
    '''<summary>F13 key</summary>
    Public Const VK_F13 = &H7C
    '''<summary>F14 key</summary>
    Public Const VK_F14 = &H7D
    '''<summary>F15 key</summary>
    Public Const VK_F15 = &H7E
    '''<summary>F16 key</summary>
    Public Const VK_F16 = &H7F
    '''<summary>F17 key</summary>
    Public Const VK_F17 = &H80
    '''<summary>F18 key</summary>
    Public Const VK_F18 = &H81
    '''<summary>F19 key</summary>
    Public Const VK_F19 = &H82
    '''<summary>F20 key</summary>
    Public Const VK_F20 = &H83
    '''<summary>F21 key</summary>
    Public Const VK_F21 = &H84
    '''<summary>F22 key</summary>
    Public Const VK_F22 = &H85
    '''<summary>F23 key</summary>
    Public Const VK_F23 = &H86
    '''<summary>F24 key</summary>
    Public Const VK_F24 = &H87
    '''<summary>NUM LOCK key</summary>
    Public Const VK_NUMLOCK = &H90
    '''<summary>SCROLL LOCK key</summary>
    Public Const VK_SCROLL = &H91
    '''<summary>Left SHIFT key</summary>
    Public Const VK_LSHIFT = &HA0
    '''<summary>Right SHIFT key</summary>
    Public Const VK_RSHIFT = &HA1
    '''<summary>Left CONTROL key</summary>
    Public Const VK_LCONTROL = &HA2
    '''<summary>Right CONTROL key</summary>
    Public Const VK_RCONTROL = &HA3
    '''<summary>Left MENU key</summary>
    Public Const VK_LMENU = &HA4
    '''<summary>Right MENU key</summary>
    Public Const VK_RMENU = &HA5
    '''<summary>Browser Back key</summary>
    Public Const VK_BROWSER_BACK = &HA6
    '''<summary>Browser Forward key</summary>
    Public Const VK_BROWSER_FORWARD = &HA7
    '''<summary>Browser Refresh key</summary>
    Public Const VK_BROWSER_REFRESH = &HA8
    '''<summary>Browser Stop key</summary>
    Public Const VK_BROWSER_STOP = &HA9
    '''<summary>Browser Search key</summary>
    Public Const VK_BROWSER_SEARCH = &HAA
    '''<summary>Browser Favorites key</summary>
    Public Const VK_BROWSER_FAVORITES = &HAB
    '''<summary>Browser Start and Home key</summary>
    Public Const VK_BROWSER_HOME = &HAC
    '''<summary>Volume Mute key</summary>
    Public Const VK_VOLUME_MUTE = &HAD
    '''<summary>Volume Down key</summary>
    Public Const VK_VOLUME_DOWN = &HAE
    '''<summary>Volume Up key</summary>
    Public Const VK_VOLUME_UP = &HAF
    '''<summary>Next Track key</summary>
    Public Const VK_MEDIA_NEXT_TRACK = &HB0
    '''<summary>Previous Track key</summary>
    Public Const VK_MEDIA_PREV_TRACK = &HB1
    '''<summary>Stop Media key</summary>
    Public Const VK_MEDIA_STOP = &HB2
    '''<summary>Play/Pause Media key</summary>
    Public Const VK_MEDIA_PLAY_PAUSE = &HB3
    '''<summary>Start Mail key</summary>
    Public Const VK_LAUNCH_MAIL = &HB4
    '''<summary>Select Media key</summary>
    Public Const VK_LAUNCH_MEDIA_SELECT = &HB5
    '''<summary>Start Application 1 key</summary>
    Public Const VK_LAUNCH_APP1 = &HB6
    '''<summary>Start Application 2 key</summary>
    Public Const VK_LAUNCH_APP2 = &HB7
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ';:' key</summary>
    Public Const VK_OEM_1 = &HBA
    '''<summary>For any country/region, the '+' key</summary>
    Public Const VK_OEM_PLUS = &HBB
    '''<summary>For any country/region, the ',' key</summary>
    Public Const VK_OEM_COMMA = &HBC
    '''<summary>For any country/region, the '-' key</summary>
    Public Const VK_OEM_MINUS = &HBD
    '''<summary>For any country/region, the '.' key</summary>
    Public Const VK_OEM_PERIOD = &HBE
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '/?' key</summary>
    Public Const VK_OEM_2 = &HBF
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '`~' key</summary>
    Public Const VK_OEM_3 = &HC0
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '[{' key</summary>
    Public Const VK_OEM_4 = &HDB
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '\\|' key</summary>
    Public Const VK_OEM_5 = &HDC
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ']}' key</summary>
    Public Const VK_OEM_6 = &HDD
    '''<summary>Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the 'single-quote/double-quote' key</summary>
    Public Const VK_OEM_7 = &HDE
    '''<summary>Used for miscellaneous characters; it can vary by keyboard.</summary>
    Public Const VK_OEM_8 = &HDF
    '''<summary>Either the angle bracket key or the backslash key on the RT 102-key keyboard</summary>
    Public Const VK_OEM_102 = &HE2
    '''<summary>IME PROCESS key</summary>
    Public Const VK_PROCESSKEY = &HE5
    '''<summary>Used to pass Unicode characters as if they were keystrokes. The Public Const VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP</summary>
    Public Const VK_PACKET = &HE7
    '''<summary>Attn key</summary>
    Public Const VK_ATTN = &HF6
    '''<summary>CrSel key</summary>
    Public Const VK_CRSEL = &HF7
    '''<summary>ExSel key</summary>
    Public Const VK_EXSEL = &HF8
    '''<summary>Erase EOF key</summary>
    Public Const VK_EREOF = &HF9
    '''<summary>Play key</summary>
    Public Const VK_PLAY = &HFA
    '''<summary>Zoom key</summary>
    Public Const VK_ZOOM = &HFB
    '''<summary>PA1 key</summary>
    Public Const VK_PA1 = &HFD
    '''<summary>Clear key</summary>
    Public Const VK_OEM_CLEAR = &HFE
    Private Const WH_KEYBOARD_LL As Integer = 13
    Public KeyboardHandle As Integer
    ' Implement this function to block as many
    ' key combinations as you'd like...
    Public Function IsHooked(
      ByRef Hookstruct As KBDLLHOOKSTRUCT) As Boolean
        Debug.WriteLine("Hookstruct.vkCode: " & ChrW(Hookstruct.vkCode))
        Form1.boolKeyPressHandled = False
        If Form1.boolKeepCounting Then

            If CBool(Hookstruct.flags And LLKHF_DOWN) Then
                Form1.intKeyCtr = Form1.intKeyCtr + 1
                If Form1.boolStart And Form1.boolPause = False And Form1.boolStop = False Then
                    If CBool(GetAsyncKeyState(VK_DELETE)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{DELETE}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_BACK)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{BACKSPACE}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_DOWN)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{DOWN}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_UP)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{UP}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_RIGHT)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{RIGHT}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_LEFT)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{LEFT}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_END)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{END}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_ESCAPE)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{ESC}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_HOME)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{HOME}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_INSERT)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{INSERT}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_NEXT)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{PGDN}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_PRIOR)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{PGUP}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_TAB)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{TAB}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_RETURN)) Then
                        If Form1.sbg.Length > 0 Then
                            Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                            Form1.sbg.Length = 0
                        End If
                        Form1.sbg.Append("{RETURN}")
                        Form1.listActions.Add("myActions.TypeText(""" & Form1.sbg.ToString() & """);")
                        Form1.sbg.Length = 0
                        Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_SHIFT)) Then
                        Form1.boolCapsOn = Not Form1.boolCapsOn
                    End If

                    If CBool(GetAsyncKeyState(VK_CAPITAL)) Then
                        Form1.boolCapsOn = Not Form1.boolCapsOn
                    End If

                    If Not CBool(GetAsyncKeyState(VK_MENU)) _
                        And Form1.boolAltOn = True Then
                        Form1.sbg.Append(")")
                        Form1.boolAltOn = False
                        'Form1.boolKeyPressHandled = True
                    End If

                    If Not CBool(GetAsyncKeyState(VK_CONTROL)) _
                        And Form1.boolControlOn = True Then
                        Form1.sbg.Append(")")
                        Form1.boolControlOn = False
                        'Form1.boolKeyPressHandled = True
                    End If

                    If CBool(GetAsyncKeyState(VK_MENU)) And Not Form1.boolAltOn Then
                        Form1.sbg.Append("%(")
                        Form1.boolAltOn = True
                    End If

                    If CBool(GetAsyncKeyState(VK_CONTROL)) _
                        And Not Form1.boolControlOn Then
                        Form1.sbg.Append("^(")
                        Form1.boolControlOn = True
                    End If



                    If Hookstruct.vkCode = VK_SHIFT _
                        Or Hookstruct.vkCode = VK_CAPITAL _
                        Or Hookstruct.vkCode = VK_LSHIFT _
                        Or Hookstruct.vkCode = VK_RSHIFT _
                        Or Hookstruct.vkCode = VK_MENU _
                        Or Hookstruct.vkCode = VK_LMENU _
                        Or Hookstruct.vkCode = VK_CONTROL _
                        Or Hookstruct.vkCode = VK_LCONTROL _
                        Or Hookstruct.vkCode = VK_DELETE Then
                        Form1.boolKeyPressHandled = True
                    End If

                    If Form1.boolKeyPressHandled = False Then
                        If Form1.boolCapsOn Then
                            Form1.sbg.Append(ChrW(Hookstruct.vkCode))
                        Else
                            Form1.sbg.Append(ChrW(Hookstruct.vkCode).ToString().ToLower())
                            ' Next line is for debugging
                            ' Form1.sbg.Append(Hookstruct.vkCode.ToString().ToLower())
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

