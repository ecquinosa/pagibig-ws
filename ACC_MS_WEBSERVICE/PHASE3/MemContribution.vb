
Namespace PHASE3

    Public Class MemContribution

        Private pRefNum As String = ""
        Public Property RefNum As String
            Get
                Return pRefNum
            End Get
            Set(ByVal value As String)
                pRefNum = value
            End Set
        End Property

        Private pPagIBIGID As String = ""
        Public Property PagIBIGID As String
            Get
                Return pPagIBIGID
            End Get
            Set(ByVal value As String)
                pPagIBIGID = value
            End Set
        End Property

        Private pInitialPFR_Number As String = ""
        Public Property InitialPFR_Number As String
            Get
                Return pInitialPFR_Number
            End Get
            Set(ByVal value As String)
                pInitialPFR_Number = value
            End Set
        End Property

        Private pInitialPFR_Date As Date = Nothing
        Public Property InitialPFR_Date As Date
            Get
                Return pInitialPFR_Date
            End Get
            Set(ByVal value As Date)
                pInitialPFR_Date = value
            End Set
        End Property

        Private pInitialPFR_Amount As Decimal = 0
        Public Property InitialPFR_Amount As Decimal
            Get
                Return pInitialPFR_Amount
            End Get
            Set(ByVal value As Decimal)
                pInitialPFR_Amount = value
            End Set
        End Property

        Private pLastPeriodCover As String = ""
        Public Property LastPeriodCover As String
            Get
                Return pLastPeriodCover
            End Get
            Set(ByVal value As String)
                pLastPeriodCover = value
            End Set
        End Property

        Private pLastPFR_Number As String = ""
        Public Property LastPFR_Number As String
            Get
                Return pLastPFR_Number
            End Get
            Set(ByVal value As String)
                pLastPFR_Number = value
            End Set
        End Property

        Private pLastPFR_Date As Date = Nothing
        Public Property LastPFR_Date As Date
            Get
                Return pLastPFR_Date
            End Get
            Set(ByVal value As Date)
                pLastPFR_Date = value
            End Set
        End Property

        Private pLastPFR_Amount As Decimal = 0
        Public Property LastPFR_Amount As Decimal
            Get
                Return pLastPFR_Amount
            End Get
            Set(ByVal value As Decimal)
                pLastPFR_Amount = value
            End Set
        End Property

        Private pTAV_Balance As String = ""
        Public Property TAV_Balance As String
            Get
                Return pTAV_Balance
            End Get
            Set(ByVal value As String)
                pTAV_Balance = value
            End Set
        End Property

        Private pEmployerName As String = ""
        Public Property EmployerName As String
            Get
                Return pEmployerName
            End Get
            Set(ByVal value As String)
                pEmployerName = value
            End Set
        End Property

        Private pBranch As String = ""
        Public Property Branch As String
            Get
                Return pBranch
            End Get
            Set(ByVal value As String)
                pBranch = value
            End Set
        End Property

        Private pStatus As String = ""
        Public Property Status As String
            Get
                Return pStatus
            End Get
            Set(ByVal value As String)
                pStatus = value
            End Set
        End Property

        Private pIngresID As String = ""
        Public Property IngresID As String
            Get
                Return pIngresID
            End Get
            Set(ByVal value As String)
                pIngresID = value
            End Set
        End Property

        Private pEntryDate As Date = Nothing
        Public Property EntryDate As Date
            Get
                Return pEntryDate
            End Get
            Set(ByVal value As Date)
                pEntryDate = value
            End Set
        End Property

        Private pLastUpdate As Date = Nothing
        Public Property LastUpdate As Date
            Get
                Return pLastUpdate
            End Get
            Set(ByVal value As Date)
                pLastUpdate = value
            End Set
        End Property


        Private pIsSuccess As Boolean = False
        Public Property IsSuccess As Boolean
            Get
                Return pIsSuccess
            End Get
            Set(ByVal value As Boolean)
                pIsSuccess = value
            End Set
        End Property

        Private pErrorMessage As String = ""
        Public Property ErrorMessage As String
            Get
                Return pErrorMessage
            End Get
            Set(ByVal value As String)
                pErrorMessage = value
            End Set
        End Property

        Public Function SaveToDbase(ByVal DAL As DAL,
                                    ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean
            If DAL Is Nothing Then
                pErrorMessage = "SaveMemContributionRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddMemContribution(pRefNum, pPagIBIGID, pInitialPFR_Number, pInitialPFR_Date, pInitialPFR_Amount, pLastPeriodCover,
                                          pLastPFR_Number, pLastPFR_Date, pLastPFR_Amount, pTAV_Balance, pEmployerName, pBranch, pStatus,
                                          pIngresID, myTrans) Then
                pErrorMessage = "SaveMemContributionRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function


    End Class

End Namespace


