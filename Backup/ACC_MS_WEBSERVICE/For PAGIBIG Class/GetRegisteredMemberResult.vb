Public Class GetRegisteredMemberResult
    Private _MemInfo As New MemberInfo
    Public Property MemberInfo As MemberInfo
        Get
            Return _MemInfo
        End Get
        Set(ByVal value As MemberInfo)
            _MemInfo = value
        End Set
    End Property
    Private _IsRecordFound As Boolean = Nothing
    Public Property IsRecordFound As Boolean
        Get
            Return _IsRecordFound
        End Get
        Set(ByVal value As Boolean)
            _IsRecordFound = value
        End Set
    End Property

    Private _IsSent As Boolean = Nothing
    Public Property IsSent As Boolean
        Get
            Return _IsSent
        End Get
        Set(ByVal value As Boolean)
            _IsSent = value
        End Set
    End Property
    Private _ErrorMessage As String = Nothing
    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property
    Private _RecordUpdateFields As New RecordUpdatedFields
    Public Property RecordUpdateFields As RecordUpdatedFields
        Get
            Return _RecordUpdateFields
        End Get
        Set(ByVal value As RecordUpdatedFields)
            _RecordUpdateFields = value
        End Set
    End Property
End Class
