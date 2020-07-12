Public Class DepositBankAccount

    Private pID As Integer
    Public Property ID As Integer
        Get
            Return pID
        End Get
        Set(ByVal value As Integer)
            pID = value
        End Set
    End Property
    Private pAccountName As String = ""
    Public Property AccountName As String
        Get
            Return pAccountName
        End Get
        Set(ByVal value As String)
            pAccountName = value
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


    Public Shared Function GetList(
                              ByVal DAL As DAL,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As List(Of DepositBankAccount)

        Dim DepositBankAccount As New List(Of DepositBankAccount)
        If DAL.GetDepositBankAccount(myTrans) Then
            Dim dt As DataTable = Nothing
            dt = DAL.TableResult

            For Each bank As DataRow In dt.Rows
                Dim newBankAccount As New DepositBankAccount
                newBankAccount.ID = bank("ID")
                newBankAccount.AccountName = bank("AccountName")
                newBankAccount.AccountNumber = bank("AccountNumber")
                DepositBankAccount.Add(newBankAccount)
            Next
        End If
        Return DepositBankAccount
    End Function

End Class
