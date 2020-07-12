
Namespace PHASE3

    Public Class Survey

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

        Private pHome_Ownership As String = ""
        Public Property Home_Ownership As String
            Get
                Return pHome_Ownership
            End Get
            Set(ByVal value As String)
                pHome_Ownership = value
            End Set
        End Property

        Private pNumber_Years As String = ""
        Public Property Number_Years As String
            Get
                Return pNumber_Years
            End Get
            Set(ByVal value As String)
                pNumber_Years = value
            End Set
        End Property

        Private pFuture_Plan_Home As String = ""
        Public Property Future_Plan_Home As String
            Get
                Return pFuture_Plan_Home
            End Get
            Set(ByVal value As String)
                pFuture_Plan_Home = value
            End Set
        End Property

        Private pEducational_Attainment As String = ""
        Public Property Educational_Attainment As String
            Get
                Return pEducational_Attainment
            End Get
            Set(ByVal value As String)
                pEducational_Attainment = value
            End Set
        End Property

        Private pTravels_Abroad As String = ""
        Public Property Travels_Abroad As String
            Get
                Return pTravels_Abroad
            End Get
            Set(ByVal value As String)
                pTravels_Abroad = value
            End Set
        End Property

        Private pDomestic_Travel As String = ""
        Public Property Domestic_Travel As String
            Get
                Return pDomestic_Travel
            End Get
            Set(ByVal value As String)
                pDomestic_Travel = value
            End Set
        End Property

        Private pDine_Out As String = ""
        Public Property Dine_Out As String
            Get
                Return pDine_Out
            End Get
            Set(ByVal value As String)
                pDine_Out = value
            End Set
        End Property

        Private pMall_Visit As String = ""
        Public Property Mall_Visit As String
            Get
                Return pMall_Visit
            End Get
            Set(ByVal value As String)
                pMall_Visit = value
            End Set
        End Property

        Private pNumber_Dependent_Studying As Integer = 0
        Public Property Number_Dependent_Studying As Integer
            Get
                Return pNumber_Dependent_Studying
            End Get
            Set(ByVal value As Integer)
                pNumber_Dependent_Studying = value
            End Set
        End Property

        Private pNumber_Vehicles As Integer = 0
        Public Property Number_Vehicles As Integer
            Get
                Return pNumber_Vehicles
            End Get
            Set(ByVal value As Integer)
                pNumber_Vehicles = value
            End Set
        End Property

        Private pPartner_Establishment As String = ""
        Public Property Partner_Establishment As String
            Get
                Return pPartner_Establishment
            End Get
            Set(ByVal value As String)
                pPartner_Establishment = value
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

        Private pDesired_Loan_Amount As String = ""
        Public Property Desired_Loan_Amount As String
            Get
                Return pDesired_Loan_Amount
            End Get
            Set(ByVal value As String)
                pDesired_Loan_Amount = value
            End Set
        End Property

        Private pAfford_Monthly_Payment_Loan As String = ""
        Public Property Afford_Monthly_Payment_Loan As String
            Get
                Return pAfford_Monthly_Payment_Loan
            End Get
            Set(ByVal value As String)
                pAfford_Monthly_Payment_Loan = value
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
                pErrorMessage = "SaveSurveyRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddSurvey(pRefNum, pPagIBIGID, pHome_Ownership, pNumber_Years,
                                pFuture_Plan_Home, pEducational_Attainment, pTravels_Abroad, pDomestic_Travel,
                                pDine_Out, pMall_Visit, pNumber_Dependent_Studying, pNumber_Vehicles,
                                pPartner_Establishment, pDesired_Loan_Amount, pAfford_Monthly_Payment_Loan, myTrans) Then
                pErrorMessage = "SaveSurveyRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function

    End Class

End Namespace


