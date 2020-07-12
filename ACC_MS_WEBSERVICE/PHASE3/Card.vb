
Namespace PHASE3

    Public Class Card

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

        Private pCardNo As String = ""
        Public Property CardNo As String
            Get
                Return pCardNo
            End Get
            Set(ByVal value As String)
                pCardNo = value
            End Set
        End Property

        Private pCardBin As String = ""
        Public Property CardBin As String
            Get
                Return pCardBin
            End Get
            Set(ByVal value As String)
                pCardBin = value
            End Set
        End Property

        Private pExpiryDate As Date = Nothing
        Public Property ExpiryDate As Date
            Get
                Return pExpiryDate
            End Get
            Set(ByVal value As Date)
                pExpiryDate = value
            End Set
        End Property

        Private pBarcodeNumber As String = ""
        Public Property BarcodeNumber As String
            Get
                Return pBarcodeNumber
            End Get
            Set(ByVal value As String)
                pBarcodeNumber = value
            End Set
        End Property

        Private pCardStatus As String = ""
        Public Property CardStatus As String
            Get
                Return pCardStatus
            End Get
            Set(ByVal value As String)
                pCardStatus = value
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
                pErrorMessage = "SaveCardRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddCard(pRefNum, pPagIBIGID, pCardNo, pCardBin, pExpiryDate, pBarcodeNumber, pCardStatus, myTrans) Then
                pErrorMessage = "SaveCardRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function

        Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
            Dim dt As DataTable = Nothing
            Try

                If DAL.SelectQuery(String.Format("SELECT RefNum, PagIBIGID, CardNo, CardBin, ExpiryDate,BarcodeNumber,CardStatus,EntryDate,LastUpdate FROM tbl_Card WHERE RefNum = '{0}'", refNumber)) Then
                    dt = DAL.TableResult
                    If dt.Rows.Count > 0 Then
                        Dim value = dt.Rows(0)
                        pRefNum = refNumber
                        pPagIBIGID = value("PagIBIGID")
                        CardNo = value("CardNo")
                        CardBin = value("CardBin")
                        ExpiryDate = value("ExpiryDate")
                        BarcodeNumber = value("BarcodeNumber")
                        CardStatus = value("CardStatus")
                        EntryDate = value("EntryDate")
                        LastUpdate = value("LastUpdate")
                        Return True
                    Else
                        pErrorMessage = "Reference Not Found."
                    End If

                Else
                    pErrorMessage = "Load() Card: " & DAL.ErrorMessage
                End If
            Catch ex As Exception
                pErrorMessage = "Load() Card: " & DAL.ErrorMessage
            End Try


            Return False
        End Function
    End Class

End Namespace


