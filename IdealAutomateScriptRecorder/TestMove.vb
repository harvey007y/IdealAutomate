Public Class TestMove

    Private Sub TestMove_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strDeskTop As String = My.Computer.FileSystem.SpecialDirectories.Desktop
        MsgBox("Starting")
        'System.IO.File.Move(strDeskTop & "\TestMoveSource.dll", strDeskTop & "\TestMoveDestination.dll")
        'MsgBox("Move Success")
        System.IO.File.Copy("C:\Windows\System32\imageres.dll", strDeskTop & "\imagres.dll")
        MsgBox("Copy Success")
        System.IO.File.Delete(strDeskTop & "\imagres.dll")
        MsgBox("Success")
    End Sub
End Class