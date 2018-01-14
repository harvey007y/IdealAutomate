Imports IdealAutomate.Core

Public Class ControlEntity
    Public Property ControlType() As ControlType
    Public Property ID() As String
    Public Property Text() As String
    Public Property ListOfKeyValuePairs() As List(Of ComboBoxPair)
    Public Property SelectedKey() As String
    Public Property SelectedValue() As String
    Public Property Checked() As Boolean
    Public Property ButtonPressed() As Boolean
    Public Property ImageFile() As String
    Public Property RowNumber() As Integer
    Public Property ColumnNumber() As Integer
    Public Property Width() As Integer
    Public Property Height() As Integer
    Public Property TextWrap() As Boolean
    Public Property Multiline() As Boolean
    Public Property ShowTextBox() As Boolean
    Public Property ShowFormattedAmount() As Boolean
    Public Property Amount() As Decimal
    Public Property BackgroundColor() As System.Windows.Media.Color?
    Public Property ForegroundColor() As System.Windows.Media.Color?
    Public Property ParentLkDDLNamesItemsInc() As Integer
    Public Property Source() As ImageSource
    Public Property ComboBoxIsEditable() As Boolean
    ''' <summary>
    ''' DDLName is used when you want to use the same dropdownlist on the
    ''' same screen more than once. In that case, you make the ID unique
    ''' for each instance, but you use DDLName to point to the ID of the
    ''' dropdown contained in DDLNames table
    ''' </summary>
    Public Property DDLName() As String

    Public Property ColumnSpan() As Integer

    Public Property ToolTipx() As String

    Public Property FontFamilyx() As System.Windows.Media.FontFamily

    Public Property FontSize() As Double

    Public Property FontStretchx() As FontStretch

    Public Property FontStyle() As System.Windows.FontStyle

    Public Property FontWeight() As FontWeight

    Public Sub New()
        ControlType = ControlType.Label
        ID = ""
        Text = ""
        ListOfKeyValuePairs = New List(Of ComboBoxPair)()
        SelectedKey = "--Select Item ---"
        SelectedValue = "--Select Item ---"
        Checked = False
        ButtonPressed = False
        ImageFile = ""
        RowNumber = 0
        ColumnNumber = 0
        Width = 0
        Height = 0
        TextWrap = True
        Multiline = False
        ShowTextBox = True
        ShowFormattedAmount = True
        BackgroundColor = Nothing
        ForegroundColor = Nothing
        Amount = 0
        ParentLkDDLNamesItemsInc = -1
        ComboBoxIsEditable = False
        DDLName = ""
        ToolTipx = ""
        ColumnSpan = 1
        FontFamilyx = New System.Windows.Media.FontFamily("Segoe UI")
        FontSize = 12
        FontStretchx = FontStretches.Normal
        FontStyle = System.Windows.FontStyles.Normal
        FontWeight = FontWeights.Normal
    End Sub
    Public Sub ControlEntitySetDefaults()
        ControlType = ControlType.Label
        ID = ""
        Text = ""
        ListOfKeyValuePairs = New List(Of ComboBoxPair)()
        SelectedKey = "--Select Item ---"
        SelectedValue = "--Select Item ---"
        Checked = False
        ButtonPressed = False
        ImageFile = ""
        RowNumber = 0
        ColumnNumber = 0
        Width = 0
        Height = 0
        TextWrap = True
        Multiline = False
        ShowTextBox = True
        ShowFormattedAmount = True
        BackgroundColor = Nothing
        ForegroundColor = Nothing
        Amount = 0
        ParentLkDDLNamesItemsInc = -1
        ComboBoxIsEditable = False
        DDLName = ""
        ToolTipx = ""
        ColumnSpan = 1
        FontFamilyx = New System.Windows.Media.FontFamily("Segoe UI")
        FontSize = 12
        FontStretchx = FontStretches.Normal
        FontStyle = System.Windows.FontStyles.Normal
        FontWeight = FontWeights.Normal
    End Sub
    Public Function CreateControlEntity() As ControlEntity
        Dim myControlEntity As New ControlEntity()
        myControlEntity.ControlType = ControlType
        myControlEntity.ID = ID
        myControlEntity.Text = Text
        myControlEntity.ListOfKeyValuePairs = ListOfKeyValuePairs
        myControlEntity.SelectedKey = SelectedKey
        myControlEntity.SelectedValue = SelectedValue
        myControlEntity.Checked = Checked
        myControlEntity.ButtonPressed = ButtonPressed
        myControlEntity.ImageFile = ImageFile
        myControlEntity.RowNumber = RowNumber
        myControlEntity.ColumnNumber = ColumnNumber
        myControlEntity.Width = Width
        myControlEntity.Height = Height
        myControlEntity.TextWrap = TextWrap
        myControlEntity.Multiline = Multiline
        myControlEntity.ShowTextBox = ShowTextBox
        myControlEntity.ShowFormattedAmount = ShowFormattedAmount
        myControlEntity.Amount = Amount
        myControlEntity.BackgroundColor = BackgroundColor
        myControlEntity.ForegroundColor = ForegroundColor
        myControlEntity.ParentLkDDLNamesItemsInc = ParentLkDDLNamesItemsInc
        myControlEntity.ToolTipx = ToolTipx
        myControlEntity.ComboBoxIsEditable = ComboBoxIsEditable
        myControlEntity.DDLName = DDLName
        myControlEntity.ColumnSpan = ColumnSpan
        myControlEntity.FontFamilyx = FontFamilyx
        myControlEntity.FontSize = FontSize
        myControlEntity.FontStretchx = FontStretchx
        myControlEntity.FontWeight = FontWeight
        myControlEntity.Source = Source

        Return myControlEntity
    End Function
End Class

