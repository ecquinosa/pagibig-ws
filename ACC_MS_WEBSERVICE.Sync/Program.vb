Imports ACC_MS_WEBSERVICE.Constants
Imports ACC_MS_WEBSERVICE.PHASE3
Imports System.Data.SqlClient
Imports System.IO
Module Program

    Dim wsAuth As PHASE3.RequestAuth
    Dim memAccount As DCS_Card_Account
    Public Sub Main()

        Dim DAL As New DAL
        Dim memberUploadList = DCS_Upload.GetForUploadList(DAL)


        Dim memTotal = memberUploadList.Count()
        Dim memFailed = 0
        Dim memSuccess = 0


        wsAuth = New RequestAuth
        wsAuth.KioskID = 0
        wsAuth.User = 0
        wsAuth.wsUser = My.Settings.WsUser
        wsAuth.wsPass = My.Settings.WsPassword


        If memberUploadList.Count > 0 Then
            Utilities.SaveToSystemLog(wsAuth, "ACC_MS_WEBSERVICE.Sync Start processing....")
            Dim logmessage = String.Empty
            For Each memUpload As DCS_Upload In memberUploadList
                Try
                    Dim errorMessage = String.Empty
                    Dim pushCardResult As New RequestResponse
                    Dim packUpDataResult As New RequestResponse

                    logmessage += memUpload.RefNum + " "

                    Dim pushCard As New PushCardInfo
                    Dim packUpData As New PackUpData


                    pushCardResult = pushCard.ManualPushCardInfo(wsAuth, memUpload.RefNum)
                    If pushCardResult.IsSuccess Then
                        logmessage += "PushCardInfo() success! "
                    Else
                        errorMessage += "PushCardInfo() has been failed. Error:" + pushCardResult.ErrorMessage
                    End If




                    memAccount = New DCS_Card_Account
                    If memAccount.Load(DAL, memUpload.RefNum) Then
                        packUpDataResult = packUpData.ManualPackUpData(memUpload.RefNum, memAccount.AccountNumber)
                        If packUpDataResult.IsSuccess Then
                            logmessage += "PackUpData() success! "
                        Else
                            errorMessage += "PackUpData() has been failed. Error:" + packUpDataResult.ErrorMessage
                        End If
                    Else
                        errorMessage += "DCS_Account.Load() has been failed. Error:" + memAccount.ErrorMessage
                    End If




                    Dim myTrans As New List(Of SqlTransaction)
                    memUpload.IsPushCardInfo = pushCardResult.IsSuccess
                    memUpload.PushCardInfoDate = IIf(pushCardResult.IsSuccess, DateTime.Now, Nothing)
                    memUpload.IsPackUpData = packUpDataResult.IsSuccess
                    memUpload.PackUpDataDate = IIf(packUpDataResult.IsSuccess, DateTime.Now, Nothing)
                    memUpload.Status = IIf(pushCardResult.IsSuccess And packUpDataResult.IsSuccess, Constants.MEMBER_UPLOAD_COMPLETE, memUpload.Status)
                    If memUpload.SaveToDbase(DAL, myTrans) Then
                        For i As Short = 0 To myTrans.Count - 1
                            myTrans(i).Commit()
                        Next
                    End If

                    If pushCardResult.IsSuccess And packUpDataResult.IsSuccess Then
                        memSuccess += 1
                        logmessage += " has been success!" + vbNewLine
                    Else
                        memFailed += 1
                        logmessage += "Something went wrong." + errorMessage + vbNewLine
                    End If

                Catch ex As Exception
                    memFailed += 1
                    logmessage += "Something went wrong." + ex.StackTrace.ToString() + vbNewLine
                End Try
                Utilities.SaveToSystemLog(wsAuth, logmessage)
            Next
            Utilities.SaveToSystemLog(wsAuth, String.Format("ACC_MS_WEBSERVICE.Sync Done. Success:{0} Failed:{1} Total:{2}", memSuccess, memFailed, memberUploadList.Count))
        End If
    End Sub






End Module
