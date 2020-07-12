Public Class DepositTransaction
    Private pBranchCode As String = ""
    Public Property BranchCode As String
        Get
            Return pBranchCode
        End Get
        Set(ByVal value As String)
            pBranchCode = value
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
    'Private pAcctNo As String = ""
    'Public Property AcctNo As String
    '    Get
    '        Return pAcctNo
    '    End Get
    '    Set(ByVal value As String)
    '        pAcctNo = value
    '    End Set
    'End Property
    Private pReferenceNo As String
    Public Property ReferenceNo As String
        Get
            Return pReferenceNo
        End Get
        Set(ByVal value As String)
            pReferenceNo = value
        End Set
    End Property

    Private pAmount As Decimal
    Public Property Amount As Decimal
        Get
            Return pAmount
        End Get
        Set(ByVal value As Decimal)
            pAmount = value
        End Set
    End Property
    Private pDepositBankID As Integer
    Public Property DepositBankID As Integer
        Get
            Return pDepositBankID
        End Get
        Set(ByVal value As Integer)
            pDepositBankID = value
        End Set
    End Property
    Private pDepositBy As Integer
    Public Property DepositBy As Integer
        Get
            Return pDepositBy
        End Get
        Set(ByVal value As Integer)
            pDepositBy = value
        End Set
    End Property
    Private pDepositDate As DateTime
    Public Property DepositDate As DateTime
        Get
            Return pDepositDate
        End Get
        Set(ByVal value As DateTime)
            pDepositDate = value
        End Set
    End Property

    Private pTransactionType As String
    Public Property TransactionType As String
        Get
            Return pTransactionType
        End Get
        Set(ByVal value As String)
            pTransactionType = value
        End Set
    End Property

    Private pTransactionDate As DateTime
    Public Property TransactionDate As DateTime
        Get
            Return pTransactionDate
        End Get
        Set(ByVal value As DateTime)
            pTransactionDate = value
        End Set
    End Property
    Private pBankID As Integer
    Public Property BankID As Integer
        Get
            Return pBankID
        End Get
        Set(ByVal value As Integer)
            pBankID = value
        End Set
    End Property

    Private pRequestedDate As DateTime
    Public Property RequestedDate As DateTime
        Get
            Return pRequestedDate
        End Get
        Set(ByVal value As DateTime)
            pRequestedDate = value
        End Set
    End Property
    Private pRequestedBy As Integer
    Public Property RequestedBy As Integer
        Get
            Return pRequestedBy
        End Get
        Set(ByVal value As Integer)
            pRequestedBy = value
        End Set
    End Property

    Private pDepositImage As Byte()
    Public Property DepositImage As Byte()
        Get
            Return pDepositImage
        End Get
        Set(ByVal value As Byte())
            pDepositImage = value
        End Set
    End Property

    Private pDepositImagePath As String
    Public Property DepositImagePath As String
        Get
            Return pDepositImagePath
        End Get
        Set(ByVal value As String)
            pDepositImagePath = value
        End Set
    End Property

    Private pRemarks As String
    Public Property Remarks As String
        Get
            Return pRemarks
        End Get
        Set(ByVal value As String)
            pRemarks = value
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
            pErrorMessage = "AddDeposit_Transaction(): DAL is nothing"
            Return False
        End If

        If Not DAL.AddDepositTransaction(Me, myTrans) Then
            pErrorMessage = "AddDeposit_Transaction(): " & DAL.ErrorMessage
            Return False
        End If

        Return True
    End Function

    Public Function IsExist(ByVal DAL As DAL, ByVal referenceNo As String) As Boolean

        Try
            Dim dt As DataTable = Nothing
            If DAL.SelectQueryCentralized(String.Format("EXEC spGet_isDepositReferenceNoExist @ReferenceNo = '{0}'", referenceNo)) Then
                dt = DAL.TableResult
                If dt.Rows.Count > 0 Then
                    Dim value = dt.Rows(0)
                    Return value(0)
                Else
                    pErrorMessage = "Reference Not Found."
                End If
            Else
                pErrorMessage = "Deposit Is Exist(): " & DAL.ErrorMessage
            End If
        Catch ex As Exception
            pErrorMessage = "Deposit Is Exist(): " & DAL.ErrorMessage
        End Try

        Return False
    End Function

    Public Shared Function GetTotalAmountByBranchAndTransactionDate(
                               ByVal branchCode As String,
                               ByVal transactionDate As DateTime,
                               ByVal DAL As DAL,
                               ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Decimal
        Dim amount = 0.00

        If DAL.GetDepositTotal(branchCode, transactionDate, myTrans) Then
            Dim dt As DataTable = Nothing
            dt = DAL.TableResult
            If dt.Rows.Count > 0 Then
                Dim value = dt.Rows(0)
                amount = value(0)
            End If
        End If
        Return amount
    End Function


    Public Function GetTop10ByUserKiosk(ByVal DAL As DAL, ByVal KioskID As String, ByVal UserID As Integer, ByVal BankID As Integer, ByVal Status As Integer) As List(Of DepositTransaction)
        Dim depositList As New List(Of DepositTransaction)
        Dim deposit As DepositTransaction
        Try
            Dim dt As DataTable = Nothing

            If DAL.SelectQueryCentralized(String.Format("EXEC spGet_DepositRequestTop10ByUser @KioskID = '{0}', @RequestedBy = {1}, @BankID = {2}, @Status ={3} ", KioskID, UserID, BankID, Status)) Then
                dt = DAL.TableResult
                For Each row As DataRow In dt.Rows
                    deposit = New DepositTransaction
                    deposit.BranchCode = row("BankID")
                    deposit.ReferenceNo = row("ReferenceNo")
                    deposit.DepositBankID = row("DepositBankID")
                    deposit.BankID = row("BankID")
                    deposit.KioskID = row("KioskID")
                    deposit.Amount = row("Amount")
                    deposit.Remarks = IIf(row("Remarks") Is DBNull.Value, String.Empty, row("Remarks"))
                    deposit.DepositBy = row("DepositBy")
                    deposit.DepositDate = row("DepositDate")
                    deposit.TransactionDate = row("TransactionDate")
                    deposit.RequestedDate = row("RequestDate")
                    deposit.RequestedBy = row("RequestedBy")
                    depositList.Add(deposit)
                Next
            Else
                pErrorMessage = "spGet_CashOnHandTop10ByUser() Error: " & DAL.ErrorMessage
            End If
        Catch ex As Exception
            pErrorMessage = "spGet_CashOnHandTop10ByUser() Error: " & DAL.ErrorMessage
        End Try

        Return depositList

    End Function
End Class
