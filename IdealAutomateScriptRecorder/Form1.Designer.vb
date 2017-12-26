<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtProcessID = New System.Windows.Forms.TextBox()
        Me.txtProcessName = New System.Windows.Forms.TextBox()
        Me.txtProcessTitle = New System.Windows.Forms.TextBox()
        Me.txtCurrentWindowTitle = New System.Windows.Forms.TextBox()
        Me.txtTitleLength = New System.Windows.Forms.TextBox()
        Me.btnTop = New System.Windows.Forms.Button()
        Me.btnNormal = New System.Windows.Forms.Button()
        Me.btnHook = New System.Windows.Forms.Button()
        Me.btnUnhook = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.clbGoodApps = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtElapsedTime = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtAvgPerMin = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCurrMinKeys = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtReminder = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtStarted = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnResume = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnBreakText = New System.Windows.Forms.Button()
        Me.btnClickMouseOnImage = New System.Windows.Forms.Button()
        Me.btnTypeComment = New System.Windows.Forms.Button()
        Me.btnActivateWindowByTitle = New System.Windows.Forms.Button()
        Me.btnInsertUserInputDialog = New System.Windows.Forms.Button()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.btnRunAsync = New System.Windows.Forms.Button()
        Me.btnScriptGenerator = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'txtProcessID
        '
        Me.txtProcessID.Location = New System.Drawing.Point(112, 6)
        Me.txtProcessID.Margin = New System.Windows.Forms.Padding(2)
        Me.txtProcessID.Name = "txtProcessID"
        Me.txtProcessID.Size = New System.Drawing.Size(169, 20)
        Me.txtProcessID.TabIndex = 0
        '
        'txtProcessName
        '
        Me.txtProcessName.Location = New System.Drawing.Point(112, 44)
        Me.txtProcessName.Margin = New System.Windows.Forms.Padding(2)
        Me.txtProcessName.Name = "txtProcessName"
        Me.txtProcessName.Size = New System.Drawing.Size(169, 20)
        Me.txtProcessName.TabIndex = 1
        '
        'txtProcessTitle
        '
        Me.txtProcessTitle.Location = New System.Drawing.Point(112, 77)
        Me.txtProcessTitle.Margin = New System.Windows.Forms.Padding(2)
        Me.txtProcessTitle.Name = "txtProcessTitle"
        Me.txtProcessTitle.Size = New System.Drawing.Size(169, 20)
        Me.txtProcessTitle.TabIndex = 2
        '
        'txtCurrentWindowTitle
        '
        Me.txtCurrentWindowTitle.Location = New System.Drawing.Point(112, 113)
        Me.txtCurrentWindowTitle.Margin = New System.Windows.Forms.Padding(2)
        Me.txtCurrentWindowTitle.Name = "txtCurrentWindowTitle"
        Me.txtCurrentWindowTitle.Size = New System.Drawing.Size(169, 20)
        Me.txtCurrentWindowTitle.TabIndex = 3
        '
        'txtTitleLength
        '
        Me.txtTitleLength.Location = New System.Drawing.Point(112, 150)
        Me.txtTitleLength.Margin = New System.Windows.Forms.Padding(2)
        Me.txtTitleLength.Name = "txtTitleLength"
        Me.txtTitleLength.Size = New System.Drawing.Size(169, 20)
        Me.txtTitleLength.TabIndex = 4
        '
        'btnTop
        '
        Me.btnTop.Location = New System.Drawing.Point(267, 353)
        Me.btnTop.Name = "btnTop"
        Me.btnTop.Size = New System.Drawing.Size(75, 23)
        Me.btnTop.TabIndex = 5
        Me.btnTop.Text = "Make Top"
        Me.btnTop.UseVisualStyleBackColor = True
        '
        'btnNormal
        '
        Me.btnNormal.Location = New System.Drawing.Point(252, 290)
        Me.btnNormal.Name = "btnNormal"
        Me.btnNormal.Size = New System.Drawing.Size(87, 23)
        Me.btnNormal.TabIndex = 6
        Me.btnNormal.Text = "Make Normal"
        Me.btnNormal.UseVisualStyleBackColor = True
        '
        'btnHook
        '
        Me.btnHook.Location = New System.Drawing.Point(20, 366)
        Me.btnHook.Name = "btnHook"
        Me.btnHook.Size = New System.Drawing.Size(75, 23)
        Me.btnHook.TabIndex = 7
        Me.btnHook.Text = "Hook Keyboard"
        Me.btnHook.UseVisualStyleBackColor = True
        '
        'btnUnhook
        '
        Me.btnUnhook.Location = New System.Drawing.Point(264, 323)
        Me.btnUnhook.Name = "btnUnhook"
        Me.btnUnhook.Size = New System.Drawing.Size(75, 23)
        Me.btnUnhook.TabIndex = 8
        Me.btnUnhook.Text = "Unhook"
        Me.btnUnhook.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(112, 187)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 9
        '
        'clbGoodApps
        '
        Me.clbGoodApps.FormattingEnabled = True
        Me.clbGoodApps.Location = New System.Drawing.Point(112, 323)
        Me.clbGoodApps.Name = "clbGoodApps"
        Me.clbGoodApps.ScrollAlwaysVisible = True
        Me.clbGoodApps.Size = New System.Drawing.Size(120, 79)
        Me.clbGoodApps.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Process Id:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Process Name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Process Title:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Window Title:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(26, 156)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Title Length:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(29, 193)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "KeyPresses:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(29, 323)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Good Apps:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(32, 224)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(74, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Elapsed Time:"
        '
        'txtElapsedTime
        '
        Me.txtElapsedTime.Location = New System.Drawing.Point(112, 216)
        Me.txtElapsedTime.Name = "txtElapsedTime"
        Me.txtElapsedTime.Size = New System.Drawing.Size(100, 20)
        Me.txtElapsedTime.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 253)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Avg/Min:"
        '
        'txtAvgPerMin
        '
        Me.txtAvgPerMin.Location = New System.Drawing.Point(112, 253)
        Me.txtAvgPerMin.Name = "txtAvgPerMin"
        Me.txtAvgPerMin.Size = New System.Drawing.Size(100, 20)
        Me.txtAvgPerMin.TabIndex = 21
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(35, 290)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "CurrMinKeys:"
        '
        'txtCurrMinKeys
        '
        Me.txtCurrMinKeys.Location = New System.Drawing.Point(112, 290)
        Me.txtCurrMinKeys.Name = "txtCurrMinKeys"
        Me.txtCurrMinKeys.Size = New System.Drawing.Size(100, 20)
        Me.txtCurrMinKeys.TabIndex = 23
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(35, 433)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 13)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Reminder:"
        '
        'txtReminder
        '
        Me.txtReminder.Location = New System.Drawing.Point(112, 433)
        Me.txtReminder.Multiline = True
        Me.txtReminder.Name = "txtReminder"
        Me.txtReminder.Size = New System.Drawing.Size(228, 90)
        Me.txtReminder.TabIndex = 25
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(29, 540)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(44, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Started:"
        '
        'txtStarted
        '
        Me.txtStarted.Location = New System.Drawing.Point(112, 537)
        Me.txtStarted.Margin = New System.Windows.Forms.Padding(2)
        Me.txtStarted.Name = "txtStarted"
        Me.txtStarted.Size = New System.Drawing.Size(140, 20)
        Me.txtStarted.TabIndex = 27
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(345, 391)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(342, 243)
        Me.DataGridView1.TabIndex = 28
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(308, 4)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 29
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Location = New System.Drawing.Point(401, 4)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(75, 23)
        Me.btnPause.TabIndex = 30
        Me.btnPause.Text = "Pause"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnResume
        '
        Me.btnResume.Location = New System.Drawing.Point(494, 4)
        Me.btnResume.Name = "btnResume"
        Me.btnResume.Size = New System.Drawing.Size(75, 23)
        Me.btnResume.TabIndex = 31
        Me.btnResume.Text = "Resume"
        Me.btnResume.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(586, 3)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(75, 23)
        Me.btnStop.TabIndex = 32
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnBreakText
        '
        Me.btnBreakText.Location = New System.Drawing.Point(308, 40)
        Me.btnBreakText.Name = "btnBreakText"
        Me.btnBreakText.Size = New System.Drawing.Size(75, 23)
        Me.btnBreakText.TabIndex = 33
        Me.btnBreakText.Text = "Break Text"
        Me.btnBreakText.UseVisualStyleBackColor = True
        '
        'btnClickMouseOnImage
        '
        Me.btnClickMouseOnImage.Location = New System.Drawing.Point(308, 77)
        Me.btnClickMouseOnImage.Name = "btnClickMouseOnImage"
        Me.btnClickMouseOnImage.Size = New System.Drawing.Size(128, 23)
        Me.btnClickMouseOnImage.TabIndex = 34
        Me.btnClickMouseOnImage.Text = "Click Mouse on Image"
        Me.btnClickMouseOnImage.UseVisualStyleBackColor = True
        '
        'btnTypeComment
        '
        Me.btnTypeComment.Location = New System.Drawing.Point(308, 109)
        Me.btnTypeComment.Name = "btnTypeComment"
        Me.btnTypeComment.Size = New System.Drawing.Size(90, 23)
        Me.btnTypeComment.TabIndex = 35
        Me.btnTypeComment.Text = "Type Comment"
        Me.btnTypeComment.UseVisualStyleBackColor = True
        '
        'btnActivateWindowByTitle
        '
        Me.btnActivateWindowByTitle.Location = New System.Drawing.Point(308, 150)
        Me.btnActivateWindowByTitle.Name = "btnActivateWindowByTitle"
        Me.btnActivateWindowByTitle.Size = New System.Drawing.Size(141, 23)
        Me.btnActivateWindowByTitle.TabIndex = 36
        Me.btnActivateWindowByTitle.Text = "Activate Window By Title"
        Me.btnActivateWindowByTitle.UseVisualStyleBackColor = True
        '
        'btnInsertUserInputDialog
        '
        Me.btnInsertUserInputDialog.Location = New System.Drawing.Point(308, 183)
        Me.btnInsertUserInputDialog.Name = "btnInsertUserInputDialog"
        Me.btnInsertUserInputDialog.Size = New System.Drawing.Size(141, 23)
        Me.btnInsertUserInputDialog.TabIndex = 37
        Me.btnInsertUserInputDialog.Text = "Insert User Input Dialog"
        Me.btnInsertUserInputDialog.UseVisualStyleBackColor = True
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(308, 213)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 38
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'btnRunAsync
        '
        Me.btnRunAsync.Location = New System.Drawing.Point(308, 250)
        Me.btnRunAsync.Name = "btnRunAsync"
        Me.btnRunAsync.Size = New System.Drawing.Size(75, 23)
        Me.btnRunAsync.TabIndex = 39
        Me.btnRunAsync.Text = "Run Async"
        Me.btnRunAsync.UseVisualStyleBackColor = True
        '
        'btnScriptGenerator
        '
        Me.btnScriptGenerator.Location = New System.Drawing.Point(401, 39)
        Me.btnScriptGenerator.Name = "btnScriptGenerator"
        Me.btnScriptGenerator.Size = New System.Drawing.Size(103, 23)
        Me.btnScriptGenerator.TabIndex = 40
        Me.btnScriptGenerator.Text = "Script Generator"
        Me.btnScriptGenerator.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(698, 702)
        Me.Controls.Add(Me.btnScriptGenerator)
        Me.Controls.Add(Me.btnRunAsync)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.btnInsertUserInputDialog)
        Me.Controls.Add(Me.btnActivateWindowByTitle)
        Me.Controls.Add(Me.btnTypeComment)
        Me.Controls.Add(Me.btnClickMouseOnImage)
        Me.Controls.Add(Me.btnBreakText)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnResume)
        Me.Controls.Add(Me.btnPause)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.txtStarted)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtReminder)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtCurrMinKeys)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtAvgPerMin)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtElapsedTime)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.clbGoodApps)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnUnhook)
        Me.Controls.Add(Me.btnHook)
        Me.Controls.Add(Me.btnNormal)
        Me.Controls.Add(Me.btnTop)
        Me.Controls.Add(Me.txtTitleLength)
        Me.Controls.Add(Me.txtCurrentWindowTitle)
        Me.Controls.Add(Me.txtProcessTitle)
        Me.Controls.Add(Me.txtProcessName)
        Me.Controls.Add(Me.txtProcessID)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "IdealAutomateScriptRecorder"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtProcessID As System.Windows.Forms.TextBox
    Friend WithEvents txtProcessName As System.Windows.Forms.TextBox
    Friend WithEvents txtProcessTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtCurrentWindowTitle As System.Windows.Forms.TextBox
    Friend WithEvents txtTitleLength As System.Windows.Forms.TextBox
    Friend WithEvents btnTop As System.Windows.Forms.Button
    Friend WithEvents btnNormal As System.Windows.Forms.Button
    Friend WithEvents btnHook As System.Windows.Forms.Button
    Friend WithEvents btnUnhook As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents clbGoodApps As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtElapsedTime As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtAvgPerMin As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtCurrMinKeys As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtReminder As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtStarted As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnStart As Button
    Friend WithEvents btnPause As Button
    Friend WithEvents btnResume As Button
    Friend WithEvents btnStop As Button
    Friend WithEvents btnBreakText As Button
    Friend WithEvents btnClickMouseOnImage As Button
    Friend WithEvents btnTypeComment As Button
    Friend WithEvents btnActivateWindowByTitle As Button
    Friend WithEvents btnInsertUserInputDialog As Button
    Friend WithEvents btnRun As Button
    Friend WithEvents btnRunAsync As Button
    Friend WithEvents btnScriptGenerator As Button
End Class
