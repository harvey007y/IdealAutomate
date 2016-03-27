Public Class clsProcess

    Private m_processID As String
    Private m_processName As String
    Private m_processTitle As String
    Private m_ProcessKeystrokeCtr As Integer
    Private m_StartDateTime As DateTime
    Private m_EndDateTime As DateTime
    Private m_TotalSeconds As Integer
    Private m_TotalKeystrokes As Integer
    Private m_TextContent As String
    Private m_FileName As String

    Public Sub New(ByVal ProcessID As String, _
                   ByVal ProcessName As String, _
                   ByVal ProcessTitle As String, _
                   ByVal ProcessKeystrokeCtr As Integer, _
                   ByVal StartDateTime As DateTime, _
                   ByVal EndDateTime As DateTime, _
                   ByVal TotalSeconds As Integer, _
                   ByVal TotalKeystrokes As Integer, _
                   ByVal TextContent As String, _
                   ByVal FileName As String)


        '### Remember to add new fields to New method!!! Signature and assignments
        Me.m_processID = ProcessID
        Me.m_processName = ProcessName
        Me.m_processTitle = ProcessTitle
        Me.m_ProcessKeystrokeCtr = ProcessKeystrokeCtr
        Me.m_StartDateTime = StartDateTime
        Me.m_EndDateTime = EndDateTime
        Me.m_TotalSeconds = TotalSeconds
        Me.m_TotalKeystrokes = TotalKeystrokes
        Me.m_TextContent = TextContent
        Me.m_FileName = FileName

    End Sub
    Public ReadOnly Property ProcessID() As String
        Get
            Return Me.m_processID
        End Get
    End Property
    Public ReadOnly Property ProcessName() As String
        Get
            Return Me.m_processName
        End Get
    End Property
    Public ReadOnly Property ProcessTitle() As String
        Get
            Return Me.m_processTitle
        End Get
    End Property
    Public Property ProcessKeystrokeCtr() As Integer
        Get
            Return Me.m_ProcessKeystrokeCtr
        End Get
        Set(ByVal value As Integer)
            Me.m_ProcessKeystrokeCtr = value
        End Set
    End Property
    Public ReadOnly Property StartDateTime() As DateTime
        Get
            Return Me.m_StartDateTime
        End Get
    End Property
    Public Property EndDateTime() As DateTime
        Get
            Return Me.m_EndDateTime
        End Get
        Set(ByVal value As DateTime)
            Me.m_EndDateTime = value
        End Set
    End Property
    Public Property TotalSeconds() As Integer
        Get
            Return Me.m_TotalSeconds
        End Get
        Set(ByVal value As Integer)
            Me.m_TotalSeconds = value
        End Set
    End Property
    Public Property TotalKeystrokes() As Integer
        Get
            Return Me.m_TotalKeystrokes
        End Get
        Set(ByVal value As Integer)
            Me.m_TotalKeystrokes = value
        End Set
    End Property

    Public Property TextContent() As String
        Get
            Return Me.m_TextContent
        End Get
        Set(ByVal value As String)
            Me.m_TextContent = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return Me.m_FileName
        End Get
        Set(ByVal value As String)
            Me.m_FileName = value
        End Set
    End Property

End Class



