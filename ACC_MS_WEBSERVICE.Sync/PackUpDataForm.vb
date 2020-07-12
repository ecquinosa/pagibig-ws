Imports System.Windows.Forms
Imports ACC_MS_WEBSERVICE.PHASE3

Public Class PackUpDataForm
    Dim wsAuth As RequestAuth
    Private Sub PackUpDataForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblBankID.Text = My.Settings.BankID
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim DAL As New DAL
        Dim packUpData As New PackUpData


        wsAuth = New RequestAuth
        wsAuth.KioskID = 0
        wsAuth.User = 0
        wsAuth.wsUser = My.Settings.WsUser
        wsAuth.wsPass = My.Settings.WsPassword

        Dim logmessage = String.Empty
        Dim result As RequestResponse
        Dim errorMessage = String.Empty

        If ValidaPackUpDate(errorMessage) Then
            If rdbRefNum.Checked = True Then
                result = packUpData.ManualPackUpData(txtRefNum.Text, "")

                If result.IsSuccess Then
                    MessageBox.Show("Data has been packup.")
                    logmessage += String.Format("RefNum:{0} has been successfully pack", txtRefNum.Text)
                Else
                    MessageBox.Show("Something went wrong:" + result.ErrorMessage)
                    logmessage += String.Format("RefNum:{0} Something went wrong:{1}", txtRefNum.Text, result.ErrorMessage)
                End If
                Utilities.SaveToSystemLog(wsAuth, logmessage)

            ElseIf rdbApplication.Checked = True Then

                Dim dateFrom = New DateTime(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day, 0, 0, 0)
                Dim dateTo = New DateTime(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day, 23, 59, 59)

                Dim memberList = PHASE3.Member.GetByApplicationDateRange(DAL, dateFrom, dateTo)
                Dim success = 0
                Dim failed = 0
                Dim processCount = 0

                For Each member In memberList
                    result = packUpData.ManualPackUpData(member.RefNum, "")
                    If result.IsSuccess Then
                        logmessage += String.Format("PagIBIGID:{0} RefNum:{1} has been successfully pack {2}", member.PagIBIGID, member.RefNum, Environment.NewLine)
                        success += 1
                    Else
                        logmessage += String.Format("PagIBIGID:{0} RefNum:{1} Something went wrong:{2} {3}", member.PagIBIGID, member.RefNum, result.ErrorMessage, Environment.NewLine)
                        failed += 1
                    End If
                    processCount += 1
                    progressBar.Value = IIf(memberList.Count > 0, processCount / memberList.Count, 0)
                Next

                Dim processMessage = String.Format("PackUpdata Done Success:{0} Failed:{1} Total{2}", success, failed, memberList.Count)
                logmessage += processMessage
                Utilities.SaveToSystemLog(wsAuth, logmessage)
                MessageBox.Show(processMessage)
            End If
        Else
            MessageBox.Show(errorMessage)
        End If

    End Sub

    Private Function ValidaPackUpDate(ByRef errorMessage) As Boolean
        errorMessage = String.Empty

        If rdbApplication.Checked = True Then
            If dtpFrom.Value > dtpTo.Value Then
                errorMessage += " Invalid date range." + Environment.NewLine
            End If
        ElseIf rdbRefNum.Checked = True Then
            If txtRefNum.Text = String.Empty Or txtRefNum.Text.Length < 22 Then
                errorMessage += " Invalid RefNum." + Environment.NewLine
            End If
        Else
            errorMessage += " Please select PackUpData Type." + Environment.NewLine
        End If


        If errorMessage = String.Empty Then
            Return True
        Else
            Return False
        End If


    End Function

End Class