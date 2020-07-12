Public Class CashOnHand
    Private pID As Integer = 0
    Public Property ID As Integer
        Get
            Return pID
        End Get
        Set(ByVal value As Integer)
            pID = value
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
    Private pRemarks As String = ""
    Public Property Remarks As String
        Get
            Return pRemarks
        End Get
        Set(ByVal value As String)
            pRemarks = value
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
    Private pRequestedBy As Integer
    Public Property RequestedBy As Integer
        Get
            Return pRequestedBy
        End Get
        Set(ByVal value As Integer)
            pRequestedBy = value
        End Set
    End Property
    Private pStatus As Integer
    Public Property Status As Integer
        Get
            Return pStatus
        End Get
        Set(ByVal value As Integer)
            pStatus = value
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
    Private pRequestDate As DateTime
    Public Property RequestDate As DateTime
        Get
            Return pRequestDate
        End Get
        Set(ByVal value As DateTime)
            pRequestDate = value
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
            pErrorMessage = "AddCashOnHand(): DAL is nothing"
            Return False
        End If

        If Not DAL.SaveCashOnHand(Me, myTrans) Then
            pErrorMessage = "AddCashOnHand(): " & DAL.ErrorMessage
            Return False
        End If

        Return True
    End Function

    Public Function GetTop10ByUserKiosk(ByVal DAL As DAL, ByVal KioskID As String, ByVal UserID As Integer, ByVal BankID As Integer) As List(Of CashOnHand)
        Dim CashOnHandList As New List(Of CashOnHand)
        Dim cashOnHand As CashOnHand
        Try
            Dim dt As DataTable = Nothing

            If DAL.SelectQueryCentralized(String.Format("EXEC spGet_CashOnHandTop10ByUser @KioskID = '{0}', @RequestedBy = {1}, @BankID = {2}", KioskID, UserID, BankID)) Then
                dt = DAL.TableResult
                For Each row As DataRow In dt.Rows
                    cashOnHand = New CashOnHand
                    cashOnHand.ID = row("ID")
                    cashOnHand.BranchCode = row("BranchCode")
                    cashOnHand.Status = row("Status")
                    cashOnHand.BankID = row("BankID")
                    cashOnHand.KioskID = row("KioskID")
                    cashOnHand.Amount = row("Amount")
                    cashOnHand.Remarks = row("Remarks")
                    cashOnHand.RequestDate = row("InsertedDate")
                    CashOnHandList.Add(cashOnHand)
                Next
            Else
                pErrorMessage = "spGet_CashOnHandTop10ByUser() Error: " & DAL.ErrorMessage
            End If
        Catch ex As Exception
            pErrorMessage = "spGet_CashOnHandTop10ByUser() Error: " & DAL.ErrorMessage
        End Try

        Return CashOnHandList

    End Function

    Public Function IsExistByBranchDate(ByVal DAL As DAL, ByVal requestedDate As DateTime, ByVal branchCode As String, ByVal ID As Int16) As Boolean

        Try
            Dim dt As DataTable = Nothing
            If DAL.SelectQueryCentralized(String.Format("EXEC spGet_isCashOnHandExistByBranchAndDate @RequestDate = '{0}' , @BranchCode='{1}', @pID = {2}", requestedDate.ToString("yyyy-MM-dd"), branchCode, ID)) Then
                dt = DAL.TableResult
                If dt.Rows.Count > 0 Then
                    Dim value = dt.Rows(0)
                    Return value(0)
                Else
                    pErrorMessage = "Result Not Found."
                End If
            Else
                pErrorMessage = "CashOnHand Is Exist(): " & DAL.ErrorMessage
            End If
        Catch ex As Exception
            pErrorMessage = "CashOnHand Is Exist(): " & DAL.ErrorMessage
        End Try

        Return False
    End Function
End Class
