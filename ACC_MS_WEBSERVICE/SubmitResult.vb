Imports System.Reflection
Public Class SubmitResult
    'Private _SearchResult As New MiddleServer.SearchResult
    'Public Property SearchResult As MiddleServer.SearchResult
    '    Get
    '        Return _SearchResult
    '    End Get
    '    Set(ByVal value As MiddleServer.SearchResult)
    '        _SearchResult = value
    '    End Set
    'End Property

    Private _SearchResult As New PHASE3.SearchResult
    Public Property SearchResult As PHASE3.SearchResult
        Get
            Return _SearchResult
        End Get
        Set(ByVal value As PHASE3.SearchResult)
            _SearchResult = value
        End Set
    End Property

    Private _MemInfo As MemberInfo
    Public Property MemberInfo As MemberInfo
        Get
            Return _MemInfo
        End Get
        Set(ByVal value As MemberInfo)
            _MemInfo = value
        End Set
    End Property

    Private _IsSent As Boolean
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

    Private _SuccessfullyExecuted As Boolean
    Public Property SuccessfullyExecuted As Boolean
        Get
            Return _SuccessfullyExecuted
        End Get
        Set(ByVal value As Boolean)
            _SuccessfullyExecuted = value
        End Set
    End Property

    Private _AlreadyExist As Boolean
    Public Property AlreadyExist As Boolean
        Get
            Return _AlreadyExist
        End Get
        Set(ByVal value As Boolean)
            _AlreadyExist = value
        End Set
    End Property

    Private _IsSuccessfully_Deleted_to_parked As Boolean
    Public Property IsSuccessfully_Deleted_to_parked As Boolean
        Get
            Return _IsSuccessfully_Deleted_to_parked
        End Get
        Set(ByVal value As Boolean)
            _IsSuccessfully_Deleted_to_parked = value
        End Set
    End Property


    'Inherits Pagibigws.SearchResult


    ' '' ''Public IsComplete As String
    ' '' ''Public GetErrorMsg As String
    ' '' ''Public isGet As String
    ' '' ''Public MemberInfo As Pagibigws.HDMFMember

    Public Structure SearchResult_samp
        Dim IsComplete As String
        Dim GetErrorMsg As String
        Dim isGet As String
        Dim MemberInfo As MemberInfo

        'Private _SearchResult As MiddleServer.SearchResult
        'Public Property SearchResult As MiddleServer.SearchResult
        '    Get
        '        Return _SearchResult
        '    End Get
        '    Set(ByVal value As MiddleServer.SearchResult)
        '        _SearchResult = value
        '    End Set
        'End Property

        Private _SearchResult As PHASE3.SearchResult
        Public Property SearchResult As PHASE3.SearchResult
            Get
                Return _SearchResult
            End Get
            Set(ByVal value As PHASE3.SearchResult)
                _SearchResult = value
            End Set
        End Property








        '    Private _MemInfo As pagibigws.HDMFMember
        '    Public Property MemberInfo As pagibigws.HDMFMember
        '        Get
        '            Return _MemInfo
        '        End Get
        '        Set(ByVal value As pagibigws.HDMFMember)
        '            _MemInfo = value
        '        End Set
        '    End Property

        '    Dim _SearchResult As pagibigws.SearchResult

        '    Public Property SearchResult As pagibigws.SearchResult
        '        Get
        '            Return (_SearchResult)
        '        End Get
        '        Set(ByVal value As pagibigws.SearchResult)
        '            _SearchResult = value
        '        End Set
        '    End Property

    End Structure

    'Private _MemInfo As New MemberInfo
    'Public Property MemberInfo As MemberInfo
    '    Get
    '        Return _MemInfo
    '    End Get
    '    Set(ByVal value As MemberInfo)
    '        _MemInfo = value
    '    End Set
    'End Property

    'Private _IsSent As Boolean
    'Public Property IsSent As Boolean
    '    Get
    '        Return _IsSent
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _IsSent = value
    '    End Set
    'End Property

    'Private _ErrorMessage As String = Nothing
    'Public Property ErrorMessage As String
    '    Get
    '        Return _ErrorMessage
    '    End Get
    '    Set(ByVal value As String)
    '        _ErrorMessage = value
    '    End Set
    'End Property

    'Private _SuccessfullyExecuted As Boolean
    'Public Property SuccessfullyExecuted As Boolean
    '    Get
    '        Return _SuccessfullyExecuted
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _SuccessfullyExecuted = value
    '    End Set
    'End Property

    'Private _AlreadyExist As Boolean
    'Public Property AlreadyExist As Boolean
    '    Get
    '        Return _AlreadyExist
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _AlreadyExist = value
    '    End Set
    'End Property

    'Private _IsSuccessfully_Deleted_to_parked As Boolean
    'Public Property IsSuccessfully_Deleted_to_parked As Boolean
    '    Get
    '        Return _IsSuccessfully_Deleted_to_parked
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _IsSuccessfully_Deleted_to_parked = value
    '    End Set
    'End Property

End Class
