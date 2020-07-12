
Namespace PHASE3

    Public Class DCS_Card_Account

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

        Private pBankCode As String = ""
        Public Property BankCode As String
            Get
                Return pBankCode
            End Get
            Set(ByVal value As String)
                pBankCode = value
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

        Private pAccountNumber As String = ""
        Public Property AccountNumber As String
            Get
                Return pAccountNumber
            End Get
            Set(ByVal value As String)
                pAccountNumber = value
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

        Private pEntryUsername As String = ""
        Public Property EntryUsername As String
            Get
                Return pEntryUsername
            End Get
            Set(ByVal value As String)
                pEntryUsername = value
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

        Private pLastUpdateUserName As String = ""
        Public Property LastUpdateUserName As String
            Get
                Return pLastUpdateUserName
            End Get
            Set(ByVal value As String)
                pLastUpdateUserName = value
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
                pErrorMessage = "SaveDCS_Card_AccountRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddDCS_Card_Account(pRefNum, pPagIBIGID, pBankCode, pCardNo, pAccountNumber, pEntryUsername, pLastUpdateUserName, myTrans) Then
                pErrorMessage = "SaveDCS_Card_AccountRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function

        Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
            Dim dt As DataTable = Nothing
            Try

                If DAL.SelectQuery(String.Format("SELECT * FROM tbl_DCS_Card_Account WHERE RefNum = '{0}'", refNumber)) Then
                    dt = DAL.TableResult
                    If dt.Rows.Count > 0 Then
                        Dim value = dt.Rows(0)
                        pRefNum = refNumber
                        pPagIBIGID = value("PagIBIGID")
                        pBankCode = value("BankCode")
                        pCardNo = value("CardNo")
                        pAccountNumber = value("AccountNumber")
                        pEntryUsername = value("EntryUsername")
                        pLastUpdateUserName = value("LastUpdateUserName")
                        Return True
                    Else
                        pErrorMessage = "Reference Not Found."
                    End If

                Else
                    pErrorMessage = "Load DCS_Card_Account(): " & DAL.ErrorMessage
                End If
            Catch ex As Exception
                pErrorMessage = "Load DCS_Card_Account(): " & DAL.ErrorMessage
            End Try


            Return False
        End Function


    End Class

End Namespace


