
Namespace PHASE3

    Public Class AUBResponse

        Private pTxnNo As String = ""
        Public Property TxnNo As String
            Get
                Return pTxnNo
            End Get
            Set(ByVal value As String)
                pTxnNo = value
            End Set
        End Property

        Private pRefTxnNo As String = ""
        Public Property RefTxnNo As String
            Get
                Return pRefTxnNo
            End Get
            Set(ByVal value As String)
                pRefTxnNo = value
            End Set
        End Property

        Private pexp As String = ""
        Public Property exp As String
            Get
                Return pexp
            End Get
            Set(ByVal value As String)
                pexp = value
            End Set
        End Property

        Private paud As String = ""
        Public Property aud As String
            Get
                Return paud
            End Get
            Set(ByVal value As String)
                paud = value
            End Set
        End Property

        Private pjti As String = ""
        Public Property jti As String
            Get
                Return pjti
            End Get
            Set(ByVal value As String)
                pjti = value
            End Set
        End Property

        Private presultCode As Integer = -1
        Public Property resultCode As Integer
            Get
                Return presultCode
            End Get
            Set(ByVal value As Integer)
                presultCode = value
            End Set
        End Property

        Private presultMessage As String = ""
        Public Property resultMessage As String
            Get
                Return presultMessage
            End Get
            Set(ByVal value As String)
                presultMessage = value
            End Set
        End Property

        Private presponse_sub As String = ""
        Public Property response_sub As String
            Get
                Return presponse_sub
            End Get
            Set(ByVal value As String)
                presponse_sub = value
            End Set
        End Property

        Private pcardNo As String = ""
        Public Property cardNo As String
            Get
                Return pcardNo
            End Get
            Set(ByVal value As String)
                pcardNo = value
            End Set
        End Property


        Private paccountNo As String = ""
        Public Property accountNo As String
            Get
                Return paccountNo
            End Get
            Set(ByVal value As String)
                paccountNo = value
            End Set
        End Property

        Private pTokenRequest As String = ""
        Public Property TokenRequest As String
            Get
                Return pTokenRequest
            End Get
            Set(ByVal value As String)
                pTokenRequest = value
            End Set
        End Property

        Private pDateTimeClientRequest As String = Now.ToString
        Public Property DateTimeClientRequest As String
            Get
                Return pDateTimeClientRequest
            End Get
            Set(ByVal value As String)
                pDateTimeClientRequest = value
            End Set
        End Property

        Private pDateTimeAUBResponse As String = Now.ToString
        Public Property DateTimeAUBResponse As String
            Get
                Return pDateTimeAUBResponse
            End Get
            Set(ByVal value As String)
                pDateTimeAUBResponse = value
            End Set
        End Property

    End Class

End Namespace


