Public Class ImageEntity
    Public Property ImageFile() As String
    Public Property Attempts() As Integer
    Public Property Occurrence() As Integer
    Public Property Sleep() As Integer
    Public Property RelativeX() As Integer
    Public Property RelativeY() As Integer
    Public Property Tolerance() As Integer
    Public Property UseGrayScale() As Boolean
    Public Sub New()
        Attempts = 1
        Occurrence = 1
        Sleep = 100
        RelativeX = 10
        RelativeY = 10
        Tolerance = 90
        UseGrayScale = False
    End Sub
End Class

