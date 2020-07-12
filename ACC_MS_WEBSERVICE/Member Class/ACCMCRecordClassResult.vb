Public Class ACCMCRecordClassResult

    Private _ACCMCRecordClass As New List(Of MemberContribution)
    Public Property ACCMCRecordClass As List(Of MemberContribution)
        Get
            Return _ACCMCRecordClass
        End Get
        Set(ByVal value As List(Of MemberContribution))
            _ACCMCRecordClass = value
        End Set
    End Property

    Private _IsGet As Boolean = False
    Public Property IsGet As Boolean
        Get
            Return _IsGet
        End Get
        Set(ByVal value As Boolean)
            _IsGet = value
        End Set
    End Property

    Private _IsSent As Boolean = False
    Public Property IsSent As Boolean
        Get
            Return _IsSent
        End Get
        Set(ByVal value As Boolean)
            _IsSent = value
        End Set
    End Property

    Private _ErrorMessage As String = ""
    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property
End Class
