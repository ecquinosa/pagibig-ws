Public Class ACCMultipleRecord
    Private _ACCMultipleRecord As New List(Of MultipleMember)
    Public Property ACCMCMultipleRec As List(Of MultipleMember)
        Get
            Return _ACCMultipleRecord
        End Get
        Set(ByVal value As List(Of MultipleMember))
            _ACCMultipleRecord = value
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

    Private _AlreadyExist As Boolean = False
    Public Property AlreadyExist As Boolean
        Get
            Return _IsSent
        End Get
        Set(ByVal value As Boolean)
            _IsSent = value
        End Set
    End Property

    Private _ErrorMessage As String = Nothing = ""
    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property
End Class
