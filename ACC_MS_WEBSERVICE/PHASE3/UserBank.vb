
Namespace PHASE3

    Public Class UserBank

        Private pBankID As Integer = 0
        Public Property BankID As Integer
            Get
                Return pBankID
            End Get
            Set(ByVal value As Integer)
                pBankID = value
            End Set
        End Property

        Private pBankCode As String = ""
        Public Property BankCode As String
            Get
                Return pBankCode
            End Get
            Set(ByVal value As String)
                pBankCode = value
            End Set
        End Property

        Private pBankName As String = ""
        Public Property BankName As String
            Get
                Return pBankName
            End Get
            Set(ByVal value As String)
                pBankName = value
            End Set
        End Property

    End Class

End Namespace


