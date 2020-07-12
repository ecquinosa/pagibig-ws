Imports ACC_MS_WEBSERVICE.PHASE3
Public Class PushCardInfo
    Public Function ManualPushCardInfo(ByVal requestAuth As RequestAuth, ByVal refNumber As String) As PHASE3.RequestResponse
        Dim DAL As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        Dim MemberContactinfo As New MemberContactinfo
        Dim DCS_Card_Account As New DCS_Card_Account
        Dim Photo As New Photo
        Dim Member As New Member
        Dim requestResponse As New RequestResponse
        Dim Card As New Card

        If MemberContactinfo.Load(DAL, refNumber) Then
            If DCS_Card_Account.Load(DAL, refNumber) Then
                If Member.Load(DAL, refNumber) Then
                    If Photo.Load(DAL, refNumber) Then
                        If Card.Load(DAL, refNumber) Then
                            requestResponse.IsSuccess = True
                        Else
                            requestResponse.IsSuccess = False
                            requestResponse.ErrorMessage = Card.ErrorMessage
                        End If
                    Else
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = Photo.ErrorMessage
                    End If
                Else
                    requestResponse.IsSuccess = False
                    requestResponse.ErrorMessage = Member.ErrorMessage
                End If
            Else
                requestResponse.IsSuccess = False
                requestResponse.ErrorMessage = DCS_Card_Account.ErrorMessage
            End If
        Else
            requestResponse.IsSuccess = False
            requestResponse.ErrorMessage = MemberContactinfo.ErrorMessage
        End If

        Dim bankCode As String = "000"
        If My.Settings.BankID = "1" Then bankCode = "041"
        If My.Settings.BankID = "2" Then bankCode = "102"

        Dim oldCardNo As String = ""
        Dim ph3API As New PHASE3.PHASE3
        Dim activeCardErrorMessage = ""


        If requestResponse.IsSuccess Then
            If Member.Application_Remarks.Contains("Re-card") Then
                ' Active card info update.

                Dim activeCardResult = ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                oldCardNo = IIf(activeCardResult, ph3API.ActiveCardInfoResult.CardNumber, oldCardNo)

                Dim updateCardInfoResult As PHASE3.UpdateCardInfoResult = UpdateCardInfoLocal(requestAuth, Member.PagIBIGID, oldCardNo, DCS_Card_Account.CardNo, DCS_Card_Account.AccountNumber, bankCode)
                If updateCardInfoResult.IsSuccess Then
                    requestResponse.IsSuccess = True
                Else

                    ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)

                    If ph3API.ActiveCardInfoResult.CardNumber = DCS_Card_Account.CardNo And Not DCS_Card_Account.CardNo = "" Then
                        ' Means card number is same no need to update.
                        requestResponse.IsSuccess = True
                    Else
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = updateCardInfoResult.GetErrorMsg
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                    End If
                End If
            Else
                Dim pushCardInfoResult As PHASE3.PushCardInfoResult = PushCardInfoLocal(requestAuth,
                                                          Member.PagIBIGID, Member.Member_LastName, Member.Member_FirstName, Member.Member_MiddleName,
                                                          Member.BirthDate, (MemberContactinfo.Mobile_AreaCode.Trim & MemberContactinfo.Mobile_CelNo.Trim).Trim,
                                                          DCS_Card_Account.CardNo, DCS_Card_Account.AccountNumber, Card.ExpiryDate, Member.ApplicationDate.Date, bankCode, Member.requesting_branchcode)
                If pushCardInfoResult.IsSuccess Then
                    requestResponse.IsSuccess = True
                Else
                    ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                    If ph3API.ActiveCardInfoResult.CardNumber = DCS_Card_Account.CardNo And Not DCS_Card_Account.CardNo = "" Then
                        ' Means card number is same no need to update.
                        requestResponse.IsSuccess = True
                    Else
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = pushCardInfoResult.GetErrorMsg
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                    End If
                End If
            End If
        End If
        Return requestResponse
    End Function

    Private Function PushCardInfoLocal(ByVal requestAuth As RequestAuth,
                                  ByVal MemberID As String, ByVal Lname As String, ByVal Fname As String,
                                  ByVal Mname As String, ByVal Birthdate As Date, ByVal Mobileno As String,
                                  ByVal Cardno As String, ByVal AccountNo As String, ByVal ExpiryDate As Date,
                                  ByVal Dateissued As Date, ByVal BankCode As String,
                                  ByVal MsbCode As String) As PushCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim pushCardInfoResult As New PushCardInfoResult

        Try
            Dim ph3API As New PHASE3.PHASE3
            Dim response As String = ""
            Dim blnResponse As Boolean = ph3API.PushCardInfo(MemberID, Lname, Fname, Mname,
                                                             Birthdate, Mobileno, Cardno, AccountNo,
                                                             ExpiryDate, Dateissued, BankCode, MsbCode, response)
            If blnResponse Then
                pushCardInfoResult.IsSuccess = ph3API.PushCardInfoResult.IsSuccess
                pushCardInfoResult.GetErrorMsg = ph3API.PushCardInfoResult.GetErrorMsg
            Else
                pushCardInfoResult.GetErrorMsg = response
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & pushCardInfoResult.GetErrorMsg)
            End If
        Catch ex As Exception
            pushCardInfoResult.GetErrorMsg = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & pushCardInfoResult.GetErrorMsg)
        End Try

        Return pushCardInfoResult
    End Function

    Private Function UpdateCardInfoLocal(ByVal requestAuth As PHASE3.RequestAuth,
                                     ByVal pagibigid As String, ByVal old_cardno As String, ByVal new_cardno As String,
                                     ByVal accountno As String, ByVal bank_code As String) As PHASE3.UpdateCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim updateCardInfoResult As New PHASE3.UpdateCardInfoResult

        Try
            Dim ph3API As New PHASE3.PHASE3
            Dim response As String = ""


            Dim blnResponse As Boolean = ph3API.UpdateCardInfo(pagibigid, old_cardno, new_cardno, accountno, bank_code, response)
            If blnResponse Then
                updateCardInfoResult.IsSuccess = ph3API.UpdateCardInfoResult.IsSuccess
                updateCardInfoResult.GetErrorMsg = ph3API.UpdateCardInfoResult.GetErrorMsg
            Else
                updateCardInfoResult.GetErrorMsg = response
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & updateCardInfoResult.GetErrorMsg)
            End If
        Catch ex As Exception
            updateCardInfoResult.GetErrorMsg = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & updateCardInfoResult.GetErrorMsg)
        End Try

        Return updateCardInfoResult
    End Function

End Class
