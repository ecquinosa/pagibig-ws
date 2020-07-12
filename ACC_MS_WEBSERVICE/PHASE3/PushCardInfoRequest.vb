
Namespace PHASE3

    Public Class PushCardInfoRequest

        Private pMemberID As String = ""
        Public Property MemberID As String
            Get
                Return pMemberID
            End Get
            Set(ByVal value As String)
                pMemberID = value
            End Set
        End Property

        Private pLname As String = ""
        Public Property Lname As String
            Get
                Return pLname
            End Get
            Set(ByVal value As String)
                pLname = value
            End Set
        End Property

        Private pFname As String = ""
        Public Property Fname As String
            Get
                Return pFname
            End Get
            Set(ByVal value As String)
                pFname = value
            End Set
        End Property

        Private pMname As String = ""
        Public Property Mname As String
            Get
                Return pMname
            End Get
            Set(ByVal value As String)
                pMname = value
            End Set
        End Property

        Private pBirthdate As Date
        Public Property Birthdate As Date
            Get
                Return pBirthdate
            End Get
            Set(ByVal value As Date)
                pBirthdate = value
            End Set
        End Property

        Private pMobileno As String = ""
        Public Property Mobileno As String
            Get
                Return pMobileno
            End Get
            Set(ByVal value As String)
                pMobileno = value
            End Set
        End Property

        Private pCardno As String = ""
        Public Property Cardno As String
            Get
                Return pCardno
            End Get
            Set(ByVal value As String)
                pCardno = value
            End Set
        End Property

        Private pAccountNo As String = ""
        Public Property AccountNo As String
            Get
                Return pAccountNo
            End Get
            Set(ByVal value As String)
                pAccountNo = value
            End Set
        End Property

        Private pExpiryDate As Date
        Public Property ExpiryDate As Date
            Get
                Return pExpiryDate
            End Get
            Set(ByVal value As Date)
                pExpiryDate = value
            End Set
        End Property

        Private pDateissued As Date
        Public Property Dateissued As Date
            Get
                Return pBirthdate
            End Get
            Set(ByVal value As Date)
                pDateissued = value
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

        Private pMsbCode As String = ""
        Public Property MsbCode As String
            Get
                Return pMsbCode
            End Get
            Set(ByVal value As String)
                pMsbCode = value
            End Set
        End Property

    End Class

End Namespace


