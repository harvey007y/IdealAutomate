
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.VisualBasic
Public Class frmTestGetChildWindows
    Private Const WM_GETTEXT As Integer = &HD
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, _
    ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr



    Delegate Function EnumWindProc( _
            ByVal hWnd As Int32, _
            ByVal lParam As Int32) As Boolean

    Delegate Function EnumChildWindProc( _
            ByVal hWnd As Int32, _
            ByVal lParam As Int32) As Boolean

    Declare Function EnumWindows Lib "user32.dll" ( _
            ByVal lpEnumProc As EnumWindProc, _
            ByVal lParam As Int32) As Boolean

    Declare Function EnumChildWindows Lib "user32" ( _
         ByVal hWnd As IntPtr, _
         ByVal lpEnumFunc As EnumWindProc, _
         ByRef lParam As IntPtr) As Int32
    Dim Children As String

    Private Function EnumChild( _
         ByVal hWnd As Int32, _
         ByVal lParam As Int32) As Boolean
        Children = Children & "," & hWnd.ToString
        EnumChild = True
        'Alloc memory for the buffer that receives the text
        Dim Hndl As IntPtr = Marshal.AllocHGlobal(2000)

        'Send The WM_GETTEXT Message
        Dim NumText As Integer = SendMessage(hWnd, WM_GETTEXT, 2000, Hndl)

        'copy the characters from the unmanaged memory to a managed string
        Text = Marshal.PtrToStringUni(Hndl)

        'Display the string using a label
        'MessageBox.Show(Text)
        
    End Function

    Private Function EnumWins(ByVal hwnd As Int32, ByVal lParam As Int32) As Boolean
        Dim proc As New EnumWindProc(AddressOf EnumChild)
        Children = String.Empty
        EnumChildWindows(CType(hwnd, IntPtr), proc, IntPtr.Zero)
        'MessageBox.Show(hwnd & Convert.ToChar(Keys.Return) & Children)
        EnumWins = True
        
    End Function

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim proc As New EnumWindProc(AddressOf EnumWins)
        EnumWindows(proc, 0)
    End Sub
End Class