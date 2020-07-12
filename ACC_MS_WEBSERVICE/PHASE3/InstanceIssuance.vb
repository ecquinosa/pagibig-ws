
Namespace PHASE3

    Public Class InstanceIssuance

        Private pRefNum As String = ""
        Public Property RefNum As String
            Get
                Return pRefNum
            End Get
            Set(ByVal value As String)
                pRefNum = value
            End Set
        End Property

        Private pMIDRTN As String = ""
        Public Property MIDRTN As String
            Get
                Return pMIDRTN
            End Get
            Set(ByVal value As String)
                pMIDRTN = value
            End Set
        End Property

        Private pOCR As String = ""
        Public Property OCR As String
            Get
                Return pOCR
            End Get
            Set(ByVal value As String)
                pOCR = value
            End Set
        End Property

        Private pPrintedDate As Date = Nothing
        Public Property PrintedDate As Date
            Get
                Return pPrintedDate
            End Get
            Set(ByVal value As Date)
                pPrintedDate = value
            End Set
        End Property

        Private pPrintCounter As String = ""
        Public Property PrintCounter As String
            Get
                Return pPrintCounter
            End Get
            Set(ByVal value As String)
                pPrintCounter = value
            End Set
        End Property

        Private pPrinterSerial As String = ""
        Public Property PrinterSerial As String
            Get
                Return pPrinterSerial
            End Get
            Set(ByVal value As String)
                pPrinterSerial = value
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

        Private pIsSent As String = ""
        Public Property IsSent As String
            Get
                Return pIsSent
            End Get
            Set(ByVal value As String)
                pIsSent = value
            End Set
        End Property

        Private pDateSent As Date = Nothing
        Public Property DateSent As Date
            Get
                Return pDateSent
            End Get
            Set(ByVal value As Date)
                pDateSent = value
            End Set
        End Property

        Private pApplicationDate As Date = Nothing
        Public Property ApplicationDate As Date
            Get
                Return pApplicationDate
            End Get
            Set(ByVal value As Date)
                pApplicationDate = value
            End Set
        End Property

        Private pPrintCardCounter As Integer = 0
        Public Property PrintCardCounter As Integer
            Get
                Return pPrintCardCounter
            End Get
            Set(ByVal value As Integer)
                pPrintCardCounter = value
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
                pErrorMessage = "Saveinstant_issuanceRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.Addinstant_issuance(pRefNum, pMIDRTN, pOCR, pPrintedDate, pPrintCounter, pPrinterSerial, pEntryDate, pIsSent,
                                           pDateSent, pApplicationDate, pPrintCardCounter, myTrans) Then
                pErrorMessage = "Saveinstant_issuanceRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function


    End Class

End Namespace


