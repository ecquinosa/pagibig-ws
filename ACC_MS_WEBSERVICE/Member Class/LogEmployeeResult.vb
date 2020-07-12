Public Class LogEmployeeResult
    Private _DateTime As DateTime = Nothing
    Public Property DateTime As DateTime
        Get
            Return _DateTime
        End Get
        Set(ByVal value As DateTime)
            _DateTime = value
        End Set
    End Property
    Private _Description As String = ""
    Public Property Description As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property
    Private _Status As Integer = Nothing
    Public Property Status As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
        End Set
    End Property

End Class
