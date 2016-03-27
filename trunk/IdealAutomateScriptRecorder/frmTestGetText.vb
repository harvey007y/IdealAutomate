Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.VisualBasic

Public Class frmTestGetText

    Delegate Function EnumWindProc( _
           ByVal hWnd As Int32, _
           ByVal lParam As Int32) As Boolean

    Public intCtr As Integer = 0
    Private Const BUF_SIZE = 512
    Private Declare Function FindWindowW Lib "User32" (ByVal lpClassName As Long, ByVal lpWindow As Long) As Long
    Public Delegate Sub myDelegate(ByVal MsgString As Long, ByVal parm2 As Long)
    Private Declare Function EnumChildWindows Lib "User32" (ByVal hWndParent As IntPtr, ByVal lpEnumFunc As EnumWindProc, ByVal lParam As IntPtr) As Long
    Private Declare Function GetTopWindow Lib "User32" (ByVal hWnd As IntPtr) As IntPtr
    Private Declare Function GetWindowTextW Lib "User32" (ByVal hWnd As Long, ByVal lpString As Long, ByVal nMaxCount As Long) As Long

    Public hWndMDI As Long

    Private Const WM_GETTEXT As Integer = &HD
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, _
    ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindowEx(ByVal parentHandle As IntPtr, _
                                     ByVal childAfter As IntPtr, _
                                     ByVal lclassName As String, _
                                     ByVal windowTitle As String) As IntPtr
    End Function

    Declare Auto Function FindWindow Lib "user32.dll" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Private Declare Function GetFocus Lib "user32.dll" () As IntPtr
    Private Declare Function GetForegroundWindow Lib "user32.dll" () As IntPtr



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Find the running notepad window  hiwmmm xxx
        'System.Threading.Thread.CurrentThread.Sleep(5000)
        Thread.Sleep(1000)
        Dim Hwnd As IntPtr = FindWindow(Nothing, "IdealAutomateScriptRecorder - Microsoft Visual Studio") '"Untitled - Notepad") '

        'Alloc memory for the buffer that recieves the text
        Dim Handle As IntPtr = Marshal.AllocHGlobal(100)

        'send WM_GWTTEXT message to the notepad window
        Dim NumText As Integer = SendMessage(Hwnd, WM_GETTEXT, 50, Handle)

        'copy the characters from the unmanaged memory to a managmed stringactiveactive
        Dim Text As String = Marshal.PtrToStringUni(Handle)

        'Display the string using a label
        Label1.Text = Text

        'Find the Edit control of the Running Notepad
        Dim proc As New EnumWindProc(AddressOf EnumChildProc)
        EnumChildWindows(Hwnd, proc, 0)
        Dim ChildHandle As IntPtr = CType(hWndMDI, IntPtr) 'hWndMDI 'GetTopWindow(Hwnd) 'hWndMDI

        'Dim ChildHandle As IntPtr = GetFocus() ' FindWindowEx(Hwnd, IntPtr.Zero, Nothing, Nothing)

        'Alloc memory for the buffer that recieves the text
        Dim Hndl As IntPtr = Marshal.AllocHGlobal(2000)

        'Send The WM_GETTEXT Message
        NumText = SendMessage(ChildHandle, WM_GETTEXT, 2000, Hndl) 'ChildHandle

        'copy the characters from the unmanaged memory to a managed string
        Text = Marshal.PtrToStringUni(Hndl)

        'Display the string using a label
        Label2.Text = Text

    End Sub
    Public Function GetWindowTitle(ByVal hWnd As Long) As String
        Dim Buffer(BUF_SIZE) As Byte
        Dim cb As Long
        cb = GetWindowTextW(hWnd, VarPtr(Buffer(0)), BUF_SIZE)
        If cb <> 0 Then
            GetWindowTitle = Buffer.ToString().Substring(0, cb)
        Else
            GetWindowTitle = ""
        End If
        Erase Buffer
    End Function

    Public Function EnumChildProc(ByVal hWnd As IntPtr, ByVal lParam As Long) As Long
        Dim hTopWindow As Long = 0
        hTopWindow = GetTopWindow(hWnd)
        'Alloc memory for the buffer that receives the text
        Dim Hndl As IntPtr = Marshal.AllocHGlobal(2000)

        'Send The WM_GETTEXT Message
        Dim NumText As Integer = SendMessage(hWnd, WM_GETTEXT, 2000, Hndl)

        'copy the characters from the unmanaged memory to a managed string
        Text = Marshal.PtrToStringUni(Hndl)

        'Display the string using a label
        If NumText <> 0 Then
            ' MessageBox.Show(NumText.ToString() + Text)
        End If

        If hTopWindow <> 0 Then
            hWndMDI = hTopWindow
            EnumChildProc = 0
            Exit Function
        End If
        EnumChildProc = 1
    End Function

    Public Function VarPtr(ByVal e As Object) As Integer
        Dim GC As GCHandle = GCHandle.Alloc(e, GCHandleType.Pinned)
        Dim GC2 As Integer = GC.AddrOfPinnedObject.ToInt32
        GC.Free()
        Return GC2
    End Function



End Class

