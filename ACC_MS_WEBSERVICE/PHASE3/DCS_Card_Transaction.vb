Public Class DCS_Card_Transaction
    Private pTransactionNo As String = ""
    Public Property TransactionNo As String
        Get
            Return pTransactionNo
        End Get
        Set(ByVal value As String)
            pTransactionNo = value
        End Set
    End Property

    Private pTransactionDate As DateTime = Nothing
    Public Property TransactionDate As DateTime
        Get
            Return pTransactionDate
        End Get
        Set(ByVal value As DateTime)
            pTransactionDate = value
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
    Private pTransactionTypeID As String = ""
    Public Property TransactionTypeID As String
        Get
            Return pTransactionTypeID
        End Get
        Set(ByVal value As String)
            pTransactionTypeID = value
        End Set
    End Property
    Private pQuantity As Decimal = 0.00
    Public Property Quantity As Decimal
        Get
            Return pQuantity
        End Get
        Set(ByVal value As Decimal)
            pQuantity = value
        End Set
    End Property
    Private pRemarks As String = ""
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
            pErrorMessage = "SaveDCS_Card_TransactionRequest(): DAL is nothing"
            Return False
        End If

        If Not DAL.AddCardTransaction(pUsername, pBranchCode, pKioskID, pTransactionTypeID, pQuantity, pRemarks, myTrans) Then
            pErrorMessage = "SaveDCS_Card_TransactionRequest(): " & DAL.ErrorMessage
            Return False
        Else
            Dim dt As DataTable = Nothing
            dt = DAL.TableResult
            If dt.Rows.Count > 0 Then
                Dim value = dt.Rows(0)
                pTransactionNo = value("TransactionNo")
                pTransactionTypeID = value("TransactionTypeID")
                pTransactionDate = value("TransactionDate")
                pBranchCode = value("BranchCode")
                pKioskID = value("KioskID")
                pQuantity = value("Quantity")
                pUsername = value("Username")
                pRemarks = value("Remarks")
            End If
        End If

        Return True
    End Function

End Class
