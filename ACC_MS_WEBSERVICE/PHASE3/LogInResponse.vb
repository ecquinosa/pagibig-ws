
Namespace PHASE3

    Public Class LogInResponse

        Private pIsSuccess As Boolean = False
        Public Property IsSuccess As Boolean
            Get
                Return pIsSuccess
            End Get
            Set(ByVal value As Boolean)
                pIsSuccess = value
            End Set
        End Property

        Private pGetErrorMsg As String = ""
        Public Property GetErrorMsg As String
            Get
                Return pGetErrorMsg
            End Get
            Set(ByVal value As String)
                pGetErrorMsg = value
            End Set
        End Property

        Private pUserID As Integer = 0
        Public Property UserID As Integer
            Get
                Return pUserID
            End Get
            Set(ByVal value As Integer)
                pUserID = value
            End Set
        End Property

        Private pUsername As String = ""
        Public Property Username As String
            Get
                Return pUsername
            End Get
            Set(ByVal value As String)
                pUsername = value
            End Set
        End Property

        Private pFullName As String = ""
        Public Property FullName As String
            Get
                Return pFullName
            End Get
            Set(ByVal value As String)
                pFullName = value
            End Set
        End Property

        Private pUserRole As String = ""
        Public Property UserRole As String
            Get
                Return pUserRole
            End Get
            Set(ByVal value As String)
                pUserRole = value
            End Set
        End Property

        'Private pIdNumber As String = ""
        'Public Property IdNumber As String
        '    Get
        '        Return pIdNumber
        '    End Get
        '    Set(ByVal value As String)
        '        pIdNumber = value
        '    End Set
        'End Property

        Private pCaptureType As String = ""
        Public Property CaptureType As String
            Get
                Return pCaptureType
            End Get
            Set(ByVal value As String)
                pCaptureType = value
            End Set
        End Property

        Private pRequestingBranchCode As String = ""
        Public Property RequestingBranchCode As String
            Get
                Return pRequestingBranchCode
            End Get
            Set(ByVal value As String)
                pRequestingBranchCode = value
            End Set
        End Property

        Private pCardPFRAmount As Decimal = 0
        Public Property CardPFRAmount As Decimal
            Get
                Return pCardPFRAmount
            End Get
            Set(ByVal value As Decimal)
                pCardPFRAmount = value
            End Set
        End Property

        Private pDCSUserBank As List(Of UserBank) = Nothing
        Public Property DCSUserBank As List(Of UserBank)
            Get
                Return pDCSUserBank
            End Get
            Set(ByVal value As List(Of UserBank))
                pDCSUserBank = value
            End Set
        End Property

        Private pDCSEmployerBranch As List(Of UserEmployerBranch) = Nothing
        Public Property DCSEmployerBranch As List(Of UserEmployerBranch)
            Get
                Return pDCSEmployerBranch
            End Get
            Set(ByVal value As List(Of UserEmployerBranch))
                pDCSEmployerBranch = value
            End Set
        End Property


    End Class

End Namespace


