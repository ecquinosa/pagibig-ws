Public Class DCS_Card_Spoiled

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

    Private pBranchCode As String = ""
    Public Property BranchCode As String
        Get
            Return pBranchCode
        End Get
        Set(ByVal value As String)
            pBranchCode = value
        End Set
    End Property
    Private pCardNumber As String = ""
    Public Property CardNumber As String
        Get
            Return pCardNumber
        End Get
        Set(ByVal value As String)
            pCardNumber = value
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
            pErrorMessage = "SaveDCS_Card_SpoiledRequest(): DAL is nothing"
            Return False
        End If

        If Not DAL.AddCardSpoiled(Me, myTrans) Then
            pErrorMessage = "SaveDCS_Card_SpoiledRequest(): " & DAL.ErrorMessage
            Return False
        End If

        Return True
    End Function
End Class
