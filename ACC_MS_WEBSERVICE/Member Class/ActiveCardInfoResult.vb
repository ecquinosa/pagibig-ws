Public Class ActiveCardInfoResult

    Private _CardNumber As String = ""
    Public Property CardNumber As String
        Get
            Return _CardNumber
        End Get
        Set(ByVal value As String)
            _CardNumber = value
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

    Private _ErrorMessage As String = ""
    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property

    '<CardNumber>String</CardNumber>
    '    <IsGet>Boolean</IsGet>
    '    <RequestError>String</RequestError>
End Class
