
Namespace PHASE3

    Public Class RequestAuth

        Private pwsUser As String = ""
        Public Property wsUser As String
            Get
                Return pwsUser
            End Get
            Set(ByVal value As String)
                pwsUser = value
            End Set
        End Property

        Private pwsPass As String = ""
        Public Property wsPass As String
            Get
                Return pwsPass
            End Get
            Set(ByVal value As String)
                pwsPass = value
            End Set
        End Property

        Private pUser As String = 0
        Public Property User As Integer
            Get
                Return pUser
            End Get
            Set(ByVal value As Integer)
                pUser = value
            End Set
        End Property

        Private pKioskID As String = ""
        Public Property KioskID As String
            Get
                Return pKioskID
            End Get
            Set(ByVal value As String)
                pKioskID = value
            End Set
        End Property

        'Private pBankID As Integer = 0
        'Public Property BankID As Integer
        '    Get
        '        Return pBankID
        '    End Get
        '    Set(ByVal value As Integer)
        '        pBankID = value
        '    End Set
        'End Property

    End Class

End Namespace


