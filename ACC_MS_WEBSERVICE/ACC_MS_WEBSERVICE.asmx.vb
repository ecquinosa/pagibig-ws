
#Region " Imports "

Imports System.Web.Services
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Drawing
Imports System.Text.RegularExpressions

#End Region

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
'<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://allcardtech.com.ph/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ACC_MS_WEBSERVICE
    Inherits System.Web.Services.WebService
    'Private DA As New ODBCDataAccess
#Region " Private Declaration "

    'Private PAGIBIGWS As New MiddleServer.PagIbig_WSSoapClient
    'Private MemberInfo As New MiddleServer.SearchResult
    'Private MemberEMPClass As New MiddleServer.MultipleRecord

    Private cn As New ConnectionString
    Dim ph3API As New PHASE3.PHASE3

#End Region

    <WebMethod()>
    Public Function is_MID_RTN_Exist(ByVal requestAuth As PHASE3.RequestAuth, ByVal MID_RTN As String) As SubmitResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        is_MID_RTN_Exist = New SubmitResult
        is_MID_RTN_Exist.IsSent = True

        If requestAuth Is Nothing Then
            is_MID_RTN_Exist.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return is_MID_RTN_Exist
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            is_MID_RTN_Exist.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            Return is_MID_RTN_Exist
        End If

        Try
            '--To check if MID or RTN exist in MiddleServer
            Dim dt As Boolean = False
            'dt = Is_Mid_Rtn_Exist_In_MS(MID_RTN)
            If dt = False Then
                'is_MID_RTN_Exist.SearchResult = PAGIBIGWS.is_MID_RTN_Exist(MID_RTN, webuser, webpass).SearchResult
                ph3API = New PHASE3.PHASE3
                Dim response As String = ""
                Dim blnResponse As Boolean = ph3API.GetMemberInfo(MID_RTN, response)
                If blnResponse Then
                    is_MID_RTN_Exist.SearchResult = ph3API.SearchResult
                    If Not IsNothing(is_MID_RTN_Exist.ErrorMessage) Then
                        is_MID_RTN_Exist.ErrorMessage = is_MID_RTN_Exist.SearchResult.GetErrorMsg
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & is_MID_RTN_Exist.ErrorMessage)
                    End If
                Else
                    is_MID_RTN_Exist.ErrorMessage = response
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & is_MID_RTN_Exist.ErrorMessage)
                End If
            Else
                is_MID_RTN_Exist.AlreadyExist = True
            End If
        Catch ex As Exception
            is_MID_RTN_Exist.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & is_MID_RTN_Exist.ErrorMessage)
        End Try
        Return is_MID_RTN_Exist
    End Function

    'Checking if Member is Active or Inactive ''Return Member Contribution
    <WebMethod()>
    Public Function Is_Member_Active(ByVal requestAuth As PHASE3.RequestAuth, ByVal MID_RTN As String) As ACCMCRecordClassResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Is_Member_Active = New ACCMCRecordClassResult

        If requestAuth Is Nothing Then
            Is_Member_Active.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Is_Member_Active
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Is_Member_Active.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            Return Is_Member_Active
        End If

        Dim pwsmcres As New ACCMCRecordClassResult
        Dim mclist As New List(Of MemberContribution)

        Try
            ph3API = New PHASE3.PHASE3
            Dim response As String = ""
            Dim blnResponse As Boolean = ph3API.GetMemberMCRecord(MID_RTN, response)
            If blnResponse Then
                pwsmcres = ph3API.ACCMCRecordClassResult
                Is_Member_Active.IsGet = pwsmcres.IsGet
                Is_Member_Active.ErrorMessage = pwsmcres.ErrorMessage

                For i As Integer = 0 To pwsmcres.ACCMCRecordClass.Count - 1
                    Dim mc As New MemberContribution
                    If Not IsNothing(pwsmcres.ACCMCRecordClass(i)) Then
                        mc.IngresID = pwsmcres.ACCMCRecordClass(i).IngresID
                        mc.MCEmployerBranch = pwsmcres.ACCMCRecordClass(i).MCEmployerBranch
                        mc.MCDateOfBirth = pwsmcres.ACCMCRecordClass(i).MCDateOfBirth
                        mc.MCEmployerName = pwsmcres.ACCMCRecordClass(i).MCEmployerName
                        mc.MCEmployerStatus = pwsmcres.ACCMCRecordClass(i).MCEmployerStatus
                        mc.MCFirstName = pwsmcres.ACCMCRecordClass(i).MCFirstName
                        mc.MCMiddleName = pwsmcres.ACCMCRecordClass(i).MCMiddleName
                        mc.MCLastName = pwsmcres.ACCMCRecordClass(i).MCLastName
                        mc.MCExt = pwsmcres.ACCMCRecordClass(i).MCExt
                        mc.MCInitialPFRAmount = pwsmcres.ACCMCRecordClass(i).MCInitialPFRAmount
                        mc.MCInitialPFRDate = pwsmcres.ACCMCRecordClass(i).MCInitialPFRDate
                        mc.MCInitialPFRNumber = pwsmcres.ACCMCRecordClass(i).MCInitialPFRNumber
                        mc.MCLastPFRAmount = pwsmcres.ACCMCRecordClass(i).MCLastPFRAmount
                        mc.MCLastPFRDate = pwsmcres.ACCMCRecordClass(i).MCLastPFRDate
                        mc.MCLastPeriodCover = pwsmcres.ACCMCRecordClass(i).MCLastPeriodCover
                        mc.MCTAVBalance = pwsmcres.ACCMCRecordClass(i).MCTAVBalance
                        mc.MCEmployerStatus = pwsmcres.ACCMCRecordClass(i).MCEmployerStatus
                        mc.MCStatus = pwsmcres.ACCMCRecordClass(i).MCStatus
                        mclist.Add(mc)
                    End If
                Next
            Else
                Is_Member_Active.ErrorMessage = response
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & Is_Member_Active.ErrorMessage)
            End If
        Catch ex As Exception
            Is_Member_Active.ErrorMessage = "No record found"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): No record found. Runtime error " & Is_Member_Active.ErrorMessage)
        End Try
        Is_Member_Active.ACCMCRecordClass = New List(Of MemberContribution)
        Is_Member_Active.ACCMCRecordClass = mclist
        Return Is_Member_Active
    End Function

    <WebMethod()>
    Public Function PushCardInfo(ByVal requestAuth As PHASE3.RequestAuth, ByVal pushCardInfoRequest As PHASE3.PushCardInfoRequest) As PHASE3.PushCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        'Dim pwsmcres As New MiddleServer.ACCMCRecordClassResult
        Dim pushCardInfoResult As New PHASE3.PushCardInfoResult

        If requestAuth Is Nothing Then
            pushCardInfoResult.GetErrorMsg = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, "PushCardInfo(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return pushCardInfoResult
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            pushCardInfoResult.GetErrorMsg = PHASE3.PHASE3.WS_AuthFailedMsg
            Return pushCardInfoResult
        End If

        Try
            ph3API = New PHASE3.PHASE3
            Dim response As String = ""
            Dim blnResponse As Boolean = ph3API.PushCardInfo(pushCardInfoRequest.MemberID, pushCardInfoRequest.Lname, pushCardInfoRequest.Fname,
                                                             pushCardInfoRequest.Mname, pushCardInfoRequest.Birthdate, pushCardInfoRequest.Mobileno,
                                                             pushCardInfoRequest.Cardno, pushCardInfoRequest.AccountNo,
                                                             pushCardInfoRequest.ExpiryDate, pushCardInfoRequest.Dateissued,
                                                             pushCardInfoRequest.BankCode, pushCardInfoRequest.MsbCode, response)
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

    Private Function PushCardInfoLocal(ByVal requestAuth As PHASE3.RequestAuth,
                                  ByVal MemberID As String, ByVal Lname As String, ByVal Fname As String,
                                  ByVal Mname As String, ByVal Birthdate As Date, ByVal Mobileno As String,
                                  ByVal Cardno As String, ByVal AccountNo As String, ByVal ExpiryDate As Date,
                                  ByVal Dateissued As Date, ByVal BankCode As String,
                                  ByVal MsbCode As String) As PHASE3.PushCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim pushCardInfoResult As New PHASE3.PushCardInfoResult

        Try
            ph3API = New PHASE3.PHASE3
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

#Region " UpdateContactInfo "

    Private Function UpdateCardInfoLocal(ByVal requestAuth As PHASE3.RequestAuth,
                                     ByVal pagibigid As String, ByVal old_cardno As String, ByVal new_cardno As String,
                                     ByVal accountno As String, ByVal bank_code As String) As PHASE3.UpdateCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim updateCardInfoResult As New PHASE3.UpdateCardInfoResult

        Try
            ph3API = New PHASE3.PHASE3
            Dim response As String = ""


            Dim blnResponse As Boolean = ph3API.UpdateCardInfo(pagibigid, old_cardno, new_cardno, accountno, bank_code, response)
            If blnResponse Then
                updateCardInfoResult.IsSuccess = ph3API.UpdateCardInfoResult.IsSuccess
                updateCardInfoResult.GetErrorMsg = ph3API.UpdateCardInfoResult.GetErrorMsg
            Else
                updateCardInfoResult.GetErrorMsg = response
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & updateCardInfoResult.GetErrorMsg)
            End If

            ' Add new get form pagibig
            'Dim activeResult = ph3API.ActiveCardInfo(pagibigid, bank_code, response)
            'If activeResult Then
            '    If ph3API.ActiveCardInfoResult.IsGet Then
            '        old_cardno = ph3API.ActiveCardInfoResult.CardNumber

            '        Dim blnResponse As Boolean = ph3API.UpdateCardInfo(pagibigid, old_cardno, new_cardno, accountno, bank_code, response)
            '        If blnResponse Then
            '            updateCardInfoResult.IsSuccess = ph3API.UpdateCardInfoResult.IsSuccess
            '            updateCardInfoResult.GetErrorMsg = ph3API.UpdateCardInfoResult.GetErrorMsg
            '        Else
            '            updateCardInfoResult.GetErrorMsg = response
            '            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & updateCardInfoResult.GetErrorMsg)
            '        End If
            '    Else
            '        updateCardInfoResult.GetErrorMsg = "Failed to get active card info."
            '        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & updateCardInfoResult.GetErrorMsg)
            '    End If
            'Else
            '    updateCardInfoResult.GetErrorMsg = response
            '    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & updateCardInfoResult.GetErrorMsg)
            'End If
        Catch ex As Exception
            updateCardInfoResult.GetErrorMsg = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & updateCardInfoResult.GetErrorMsg)
        End Try

        Return updateCardInfoResult
    End Function

#End Region

    Function Is_Mid_Rtn_Exist_In_MS(ByVal requestAuth As PHASE3.RequestAuth, ByVal MIDRTN As String,
                                    Optional ByRef response As String = "") As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            response = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return False
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            response = PHASE3.PHASE3.WS_AuthFailedMsg
            Return False
        End If

        Dim DAL As New DAL
        Try
            Dim intCardCounter As Integer = GetCardCounter(requestAuth, MIDRTN)
            If intCardCounter = -1 Then
                Return Nothing
            Else
                Return intCardCounter
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return Nothing
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    'Public Function GetServerDateTime(ByVal requestAuth As PHASE3.RequestAuth) As DateTime
    <WebMethod()>
    Public Function GetServerDateTime() As DateTime
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        'Dim t As Type = GetType(PHASE3.Member)
        'Dim memberArray As System.Reflection.MemberInfo() = t.GetMembers()
        'For Each member In memberArray
        '    Console.WriteLine("Member {0} declared by {1}", member.Name, member.DeclaringType)
        'Next

        'Dim DAL As New DAL
        'Try

        'Catch ex As Exception

        'End Try


        'Return Now


        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar("SELECT GETDATE()") Then Return DAL.ObjectResult
        Catch ex As Exception
            'PHASE3.PHASE3.SaveToErrorLog(requestAuth, "GetServerDateTime(): Runtime error " & ex.Message)
            PHASE3.PHASE3.SaveToErrorLog(New PHASE3.RequestAuth, methodName & "(): Runtime error " & ex.Message)
            Return Nothing
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
        Return Nothing
    End Function

    <WebMethod()>
    Public Function GetKiosk(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'response = PHASE3.PHASE3.WS_AuthFailedMsg
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Kiosk") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMemberMaxApplicationDate(ByVal requestAuth As PHASE3.RequestAuth) As Date
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar("SELECT MAX(ApplicationDate) FROM tbl_Member") Then
                If DAL.ObjectResult Is Nothing Then
                    Return Nothing
                Else
                    Return CDate(DAL.ObjectResult)
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return Nothing
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return Nothing
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMemberMinApplicationDate(ByVal requestAuth As PHASE3.RequestAuth) As Date
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar("SELECT MIN(ApplicationDate) FROM tbl_Member") Then
                If DAL.ObjectResult Is Nothing Then
                    Return Nothing
                Else
                    Return CDate(DAL.ObjectResult)
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return Nothing
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return Nothing
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetClassification(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'response = PHASE3.PHASE3.WS_AuthFailedMsg
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Classification") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMembershipCategory(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'response = PHASE3.PHASE3.WS_AuthFailedMsg
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_MembershipCategory") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetOccupation(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'response = PHASE3.PHASE3.WS_AuthFailedMsg
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Occupation") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetGroupOfIsland(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_GroupOfIsland") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetBarangay_Zipcode(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Barangay_Zipcode") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetCitizenship(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Citizenship") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetORByMID(ByVal requestAuth As PHASE3.RequestAuth,
                               ByVal MID As String) As String
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return PHASE3.PHASE3.WS_AuthFailedMsg
        End If

        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar(String.Format("SELECT ISNULL(ORNumber,'') FROM tbl_DCS_OR WHERE PagIBIGID='{0}'", MID)) Then
                If DAL.ObjectResult Is Nothing Then
                    Return ""
                Else
                    Return DAL.ObjectResult.ToString.Trim
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return ""
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return ""
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function IsReceiptExist(ByVal requestAuth As PHASE3.RequestAuth,
                                   ByVal ORNumber As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return True
        End If

        Dim DAL As New DAL
        Try
            'If DAL.ExecuteScalarCentralized(String.Format("SELECT ISNULL(ORNumber,'') FROM tbl_DCS_OR WHERE ORNumber='{0}' AND BankID='{1}'", ORNumber, My.Settings.BankID)) Then
            If DAL.ExecuteScalar(String.Format("SELECT ISNULL(ORNumber,'') FROM tbl_DCS_OR WHERE ORNumber='{0}'", ORNumber)) Then
                If DAL.ObjectResult Is Nothing Then
                    Return False
                ElseIf DAL.ObjectResult.ToString.Trim = "" Then
                    Return False
                Else
                    Return True
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return True
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return True
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetCity_Municipality(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_City_Municipality") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMonthlyIncome(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_MonthlyIncome") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetCountry(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Country") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetProvince(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Province") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetRegion(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Region") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMemContribution(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_MemContribution") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetEmploymentStatus(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_Ref_Employment_Status") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetAUB_Country(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_AUB_Country") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetAUB_Nationality(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_AUB_Nationality") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetAUB_NatureOfBusiness(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_AUB_NatureOfBusiness") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetAUB_AddressCode(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_AUB_AddressCode") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetAUB_IDType(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("Select dbo.tbl_AUB_IDType.IDCode, dbo.tbl_DCS_IDType.IDTypeDesc From dbo.tbl_AUB_IDType INNER Join dbo.tbl_DCS_IDType ON dbo.tbl_AUB_IDType.IDTypeID = dbo.tbl_DCS_IDType.IDTypeID") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function Get_IDType(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("Select * FROM tbl_DCS_IDType") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetDCSBank(ByVal requestAuth As PHASE3.RequestAuth) As DataTable
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim dt As DataTable = Nothing
        Dim DAL As New DAL
        Try
            If DAL.SelectQuery("SELECT * FROM tbl_DCS_Bank") Then
                dt = DAL.TableResult
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return dt
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return dt
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetBioByMID(ByVal requestAuth As PHASE3.RequestAuth, ByVal MID As String) As PHASE3.Bio
        Dim bioRequest As New PHASE3.Bio

        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            bioRequest.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & bioRequest.ErrorMessage)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            bioRequest.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & bioRequest.ErrorMessage)
            Return Nothing
        End If

        Dim DAL As New DAL
        Try
            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_bio WHERE RefNum = dbo.fnGetLatestRefNumByPagIBIDID('{0}')", MID)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    bioRequest.fld_LeftPrimaryFP_Ansi = rw("fld_LeftPrimaryFP_Ansi")
                    bioRequest.fld_LeftSecondaryFP_Ansi = rw("fld_LeftSecondaryFP_Ansi")
                    bioRequest.fld_RightPrimaryFP_Ansi = rw("fld_RightPrimaryFP_Ansi")
                    bioRequest.fld_RightSecondaryFP_Ansi = rw("fld_RightSecondaryFP_Ansi")
                    bioRequest.IsSuccess = True
                Else
                    bioRequest.ErrorMessage = "No recound found"
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & bioRequest.ErrorMessage)
                End If
            Else
                bioRequest.ErrorMessage = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return bioRequest
        Catch ex As Exception
            bioRequest.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & bioRequest.ErrorMessage)
            Return bioRequest
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function SaveReceipt(ByVal requestAuth As PHASE3.RequestAuth,
                                ByVal ORNumber As String, ByVal MID As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim DAL As New DAL
        Try
            If DAL.AddOR(ORNumber, MID) Then
                Return True
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return False
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return False
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMemberDetailsByMID(ByVal requestAuth As PHASE3.RequestAuth,
                              ByVal MID As String) As PHASE3.MemberDetails
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim memberDetails As New PHASE3.MemberDetails

        If requestAuth Is Nothing Then
            memberDetails.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & memberDetails.ErrorMessage)
            Return memberDetails
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            memberDetails.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & memberDetails.ErrorMessage)
            Return memberDetails
        End If

        Dim member As PHASE3.Member = Nothing
        Dim membershipCategoryInfo As PHASE3.MembershipCategoryInfo = Nothing
        Dim memberContactinfo As PHASE3.MemberContactinfo = Nothing
        Dim memContribution As PHASE3.MemContribution = Nothing
        Dim photo As PHASE3.Photo = Nothing
        'Dim signature As PHASE3.Signature = Nothing
        'Dim bio As PHASE3.Bio = Nothing
        Dim survey As PHASE3.Survey = Nothing
        'Dim photoValidID As PHASE3.PhotoValidID = Nothing
        Dim dcs_Card_Account As PHASE3.DCS_Card_Account = Nothing
        Dim InstanceIssuance As PHASE3.InstanceIssuance = Nothing
        Dim dcs_Card_ReprintList As PHASE3.DCS_Card_ReprintList = Nothing
        Dim memberEmploymentHistoryList As PHASE3.MemberEmploymentHistoryList = Nothing

        Dim sb As New StringBuilder

        Dim DAL As New DAL
        Try
            Dim RefNum As String = ""

            If DAL.ExecuteScalar(String.Format("SELECT RefNum=dbo.fnGetLatestRefNumByPagIBIDID('{0}')", MID)) Then
                If DAL.ObjectResult Is Nothing Then
                    memberDetails.ErrorMessage = "No record found" & " - " & MID
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & memberDetails.ErrorMessage)
                    Return memberDetails
                ElseIf DAL.ObjectResult.ToString.Trim = "" Then
                    memberDetails.ErrorMessage = "No record found" & " - " & MID
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & memberDetails.ErrorMessage)
                    Return memberDetails
                Else
                    RefNum = DAL.ObjectResult.ToString.Trim
                End If
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Member WHERE RefNum='{0}'", RefNum)) Then
                member = New PHASE3.Member
                BindMember(DAL.TableResult, member)
                memberDetails.Member = member
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_MembershipCategoryInfo WHERE RefNum='{0}'", RefNum)) Then
                membershipCategoryInfo = New PHASE3.MembershipCategoryInfo
                BindMembershipCategoryInfo(DAL.TableResult, membershipCategoryInfo)
                memberDetails.MembershipCategoryInfo = membershipCategoryInfo
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_MemberContactinfo WHERE RefNum='{0}'", RefNum)) Then
                memberContactinfo = New PHASE3.MemberContactinfo
                BindMemberContactinfo(DAL.TableResult, memberContactinfo)
                memberDetails.MemberContactinfo = memberContactinfo
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_MemContribution WHERE RefNum='{0}'", RefNum)) Then
                memContribution = New PHASE3.MemContribution
                BindMemContribution(DAL.TableResult, memContribution)
                memberDetails.MemContribution = memContribution
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Photo WHERE RefNum='{0}'", RefNum)) Then
                photo = New PHASE3.Photo
                BindPhoto(DAL.TableResult, photo)
                memberDetails.Photo = photo
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            'If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Signature WHERE RefNum='{0}'", RefNum)) Then
            '    signature = New PHASE3.Signature
            '    BindSignature(DAL.TableResult, signature)
            'Else
            '    sb.Append(DAL.ErrorMessage & vbNewLine)
            'End If

            'If DAL.SelectQuery(String.Format("SELECT * FROM tbl_bio WHERE RefNum='{0}'", RefNum)) Then
            '    bio = New PHASE3.Bio
            '    BindBio(DAL.TableResult, bio)
            '    memberDetails.Bio = bio
            'Else
            '    sb.Append(DAL.ErrorMessage & vbNewLine)
            'End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Survey WHERE RefNum='{0}'", RefNum)) Then
                survey = New PHASE3.Survey
                BindSurvey(DAL.TableResult, survey)
                memberDetails.Survey = survey
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            'If DAL.SelectQuery(String.Format("SELECT * FROM tbl_PhotoValidID WHERE RefNum='{0}'", RefNum)) Then
            '    photoValidID = New PHASE3.PhotoValidID
            '    BindPhotoValidID(DAL.TableResult, photoValidID)
            'Else
            '    sb.Append(DAL.ErrorMessage & vbNewLine)
            'End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_DCS_Card_Account WHERE RefNum='{0}'", RefNum)) Then
                dcs_Card_Account = New PHASE3.DCS_Card_Account
                BindDCS_Card_Account(DAL.TableResult, dcs_Card_Account)
                memberDetails.DCS_Card_Account = dcs_Card_Account
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_EmploymentHistory WHERE RefNum='{0}'", RefNum)) Then
                memberEmploymentHistoryList = New PHASE3.MemberEmploymentHistoryList
                memberEmploymentHistoryList.MemberEmploymentHistoryList = New List(Of PHASE3.MemberEmploymentHistory)
                BindMemberEmploymentHistoryList(DAL.TableResult, memberEmploymentHistoryList)
                If Not memberEmploymentHistoryList Is Nothing Then _
                    memberDetails.MemberEmploymentHistories = memberEmploymentHistoryList.MemberEmploymentHistoryList
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_DCS_Card_Reprint WHERE RefNum='{0}'", RefNum)) Then
                dcs_Card_ReprintList = New PHASE3.DCS_Card_ReprintList
                dcs_Card_ReprintList.DCS_Card_ReprintList = New List(Of PHASE3.DCS_Card_Reprint)
                BindDCS_Card_ReprintList(DAL.TableResult, dcs_Card_ReprintList)
                If Not dcs_Card_ReprintList Is Nothing Then _
                    memberDetails.DCS_Card_Reprints = dcs_Card_ReprintList.DCS_Card_ReprintList
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_instant_issuance WHERE RefNum='{0}'", RefNum)) Then
                InstanceIssuance = New PHASE3.InstanceIssuance
                Bindinstant_issuance(DAL.TableResult, InstanceIssuance)
                memberDetails.InstanceIssuance = InstanceIssuance
            Else
                sb.Append(DAL.ErrorMessage & vbNewLine)
            End If

            If sb.ToString <> "" Then
                memberDetails.ErrorMessage = sb.ToString
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & memberDetails.ErrorMessage)
            Else
                memberDetails.IsSuccess = True
            End If

            Return memberDetails
        Catch ex As Exception
            memberDetails.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & memberDetails.ErrorMessage)
            Return memberDetails
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Sub BindMember(ByVal dt As DataTable, ByRef objClass As PHASE3.Member)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.Member_LastName = rw("Member_LastName")
            objClass.Member_FirstName = rw("Member_FirstName")
            objClass.Member_MiddleName = rw("Member_MiddleName")
            objClass.Member_Extension = rw("Member_Extension")
            objClass.Member_NoMiddleName = rw("Member_NoMiddleName")
            objClass.Birth_LastName = rw("Birth_LastName")
            objClass.Birth_FirstName = rw("Birth_FirstName")
            objClass.Birth_MiddleName = rw("Birth_MiddleName")
            objClass.Birth_NoMiddleName = rw("Birth_NoMiddleName")
            objClass.Birth_Extension = rw("Birth_Extension")
            objClass.Mother_LastName = rw("Mother_LastName")
            objClass.Mother_FirstName = rw("Mother_FirstName")
            objClass.Mother_MiddleName = rw("Mother_MiddleName")
            objClass.Mother_Extension = rw("Mother_Extension")
            objClass.Mother_NoMiddleName = rw("Mother_NoMiddleName")
            objClass.Spouse_LastName = rw("Spouse_LastName")
            objClass.Spouse_FirstName = rw("Spouse_FirstName")
            objClass.Spouse_MiddleName = rw("Spouse_MiddleName")
            objClass.Spouse_Extension = rw("Spouse_Extension")
            objClass.Spouse_NoMiddleName = rw("Spouse_NoMiddleName")
            objClass.BirthDate = rw("BirthDate")
            objClass.BirthCity = rw("BirthCity")
            objClass.BirthCountry = rw("BirthCountry")
            objClass.Gender = rw("Gender")
            objClass.CivilStatus = rw("CivilStatus")
            objClass.Citizenship = rw("Citizenship")
            objClass.CommonRefNo = rw("CommonRefNo")
            objClass.SSSID = rw("SSSID")
            objClass.GSISID = rw("GSISID")
            objClass.TIN = rw("TIN")
            objClass.MembershipCategory = rw("MembershipCategory")
            objClass.ApplicationDate = rw("ApplicationDate")
            objClass.KioskID = rw("KioskID")
            objClass.Transaction_Ref_No = rw("Transaction_Ref_No")
            objClass.Capture_Type = rw("Capture_Type")
            objClass.PFR_Number = rw("PFR_Number")
            objClass.PFR_Amount = rw("PFR_Amount")
            objClass.PFR_Date = IIf(IsDBNull(rw("PFR_Date")), Nothing, rw("PFR_Date"))
            objClass.IsMemberActive = rw("IsMemberActive")
            objClass.IsComplete = rw("IsComplete")
            objClass.Card_PFRNumber = rw("Card_PFRNumber")
            objClass.Card_PFRAmount = rw("Card_PFRAmount")
            objClass.Card_PFRDate = IIf(IsDBNull(rw("Card_PFRDate")), Nothing, rw("Card_PFRDate"))
            objClass.Application_Remarks = rw("Application_Remarks")
            objClass.requesting_branchcode = rw("requesting_branchcode")
            objClass.BranchCode = rw("BranchCode")
            objClass.UserName = rw("UserName")
            objClass.DocumentSubmitted = rw("DocumentSubmitted")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            objClass.PaymentStatus = rw("PaymentStatus")
            objClass.BillingCtrlNum = rw("BillingCtrlNum")
            objClass.PaymentRemarks = rw("PaymentRemarks")
            objClass.CardCounter = rw("CardCounter")
            objClass.CitizenshipCode = rw("CitizenshipCode")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindMembershipCategoryInfo(ByVal dt As DataTable, ByRef objClass As PHASE3.MembershipCategoryInfo)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.EmployeeID = rw("EmployeeID")
            objClass.EmployerName = rw("EmployerName")
            objClass.Employer_HBUR = rw("Employer_HBUR")
            objClass.Employer_Building = rw("Employer_Building")
            objClass.Employer_LotNo = rw("Employer_LotNo")
            objClass.Employer_BlockNo = rw("Employer_BlockNo")
            objClass.Employer_PhaseNo = rw("Employer_PhaseNo")
            objClass.Employer_HouseNo = rw("Employer_HouseNo")
            objClass.Employer_StreetName = rw("Employer_StreetName")
            objClass.Employer_Subdivision = rw("Employer_Subdivision")
            objClass.Employer_Barangay = rw("Employer_Barangay")
            objClass.Employer_psgc_Barangay_code = rw("Employer_psgc_Barangay_code")
            objClass.Employer_CityMunicipality = rw("Employer_CityMunicipality")
            objClass.Employer_psgc_city_mun_code = rw("Employer_psgc_city_mun_code")
            objClass.Employer_Province = rw("Employer_Province")
            objClass.Employer_psgc_Province_code = rw("Employer_psgc_Province_code")
            objClass.Employer_Region = rw("Employer_Region")
            objClass.Employer_psgc_region_code = rw("Employer_psgc_region_code")
            objClass.Employer_ZipCode = rw("Employer_ZipCode")
            objClass.DateEmployed = rw("DateEmployed")
            objClass.Employment_Status_code = rw("Employment_Status_code")
            objClass.Occupation = rw("Occupation")
            objClass.OccupationCode = rw("OccupationCode")
            objClass.AFPSerialBadgeNo = rw("AFPSerialBadgeNo")
            objClass.DepEdDivCodeStnCode = rw("DepEdDivCodeStnCode")
            objClass.TypeOfWork = rw("TypeOfWork")
            objClass.Income_Code = rw("Income_Code")
            objClass.Country_Assignment = rw("Country_Assignment")
            objClass.NatureOfWork = rw("NatureOfWork")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            objClass.OFWCountryCode = rw("OFWCountryCode")
            objClass.EmpStatusCode = rw("EmpStatusCode")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindMemberContactinfo(ByVal dt As DataTable, ByRef objClass As PHASE3.MemberContactinfo)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.Permanent_HBUR = rw("Permanent_HBUR")
            objClass.Permanent_Building = rw("Permanent_Building")
            objClass.Permanent_LotNo = rw("Permanent_LotNo")
            objClass.Permanent_BlockNo = rw("Permanent_BlockNo")
            objClass.Permanent_PhaseNo = rw("Permanent_PhaseNo")
            objClass.Permanent_HouseNo = rw("Permanent_HouseNo")
            objClass.Permanent_StreetName = rw("Permanent_StreetName")
            objClass.Permanent_Subdivision = rw("Permanent_Subdivision")
            objClass.Permanent_Barangay = rw("Permanent_Barangay")
            objClass.Permanent_PSGC_Barangay_Code = rw("Permanent_PSGC_Barangay_Code")
            objClass.Permanent_CityMunicipality = rw("Permanent_CityMunicipality")
            objClass.Permanent_PSGC_City_Mun_Code = rw("Permanent_PSGC_City_Mun_Code")
            objClass.Permanent_Province = rw("Permanent_Province")
            objClass.Permanent_PSGC_Province_code = rw("Permanent_PSGC_Province_code")
            objClass.Permanent_Region = rw("Permanent_Region")
            objClass.Permanent_PSGC_Region_Code = rw("Permanent_PSGC_Region_Code")
            objClass.Permanent_ZipCode = rw("Permanent_ZipCode")
            objClass.Present_HBUR = rw("Present_HBUR")
            objClass.Present_Building = rw("Present_Building")
            objClass.Present_LotNo = rw("Present_LotNo")
            objClass.Present_BlockNo = rw("Present_BlockNo")
            objClass.Present_PhaseNo = rw("Present_PhaseNo")
            objClass.Present_HouseNo = rw("Present_HouseNo")
            objClass.Present_StreetName = rw("Present_StreetName")
            objClass.Present_Subdivision = rw("Present_Subdivision")
            objClass.Present_Barangay = rw("Present_Barangay")
            objClass.Present_PSGC_Barangay_Code = rw("Present_PSGC_Barangay_Code")
            objClass.Present_CityMunicipality = rw("Present_CityMunicipality")
            objClass.Present_PSGC_City_Mun_Code = rw("Present_PSGC_City_Mun_Code")
            objClass.Present_Province = rw("Present_Province")
            objClass.Present_Province_code = rw("Present_Province_code")
            objClass.Present_Region = rw("Present_Region")
            objClass.Present_PSGC_Region_Code = rw("Present_PSGC_Region_Code")
            objClass.Present_ZipCode = rw("Present_ZipCode")
            objClass.PreferredMailingAddress = rw("PreferredMailingAddress")
            objClass.Home_CountryCode = rw("Home_CountryCode")
            objClass.Home_AreaCode = rw("Home_AreaCode")
            objClass.Home_TelNo = rw("Home_TelNo")
            objClass.Mobile_CountryCode = rw("Mobile_CountryCode")
            objClass.Mobile_AreaCode = rw("Mobile_AreaCode")
            objClass.Mobile_CelNo = rw("Mobile_CelNo")
            objClass.Business_Direct_CountryCode = rw("Business_Direct_CountryCode")
            objClass.Business_Direct_AreaCode = rw("Business_Direct_AreaCode")
            objClass.Business_Direct_TelNo = rw("Business_Direct_TelNo")
            objClass.Business_Trunk_CountryCode = rw("Business_Trunk_CountryCode")
            objClass.Business_Trunk_AreaCode = rw("Business_Trunk_AreaCode")
            objClass.Business_Trunk_TelNo = rw("Business_Trunk_TelNo")
            objClass.EmailAddress = rw("EmailAddress")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            objClass.Business_Trunk_Local = rw("Business_Trunk_Local")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindMemContribution(ByVal dt As DataTable, ByRef objClass As PHASE3.MemContribution)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.InitialPFR_Number = rw("InitialPFR_Number")
            objClass.InitialPFR_Date = rw("InitialPFR_Date")
            objClass.InitialPFR_Amount = rw("InitialPFR_Amount")
            objClass.LastPeriodCover = rw("LastPeriodCover")
            objClass.LastPFR_Number = rw("LastPFR_Number")
            objClass.LastPFR_Date = rw("LastPFR_Date")
            objClass.LastPFR_Amount = rw("LastPFR_Amount")
            objClass.TAV_Balance = rw("TAV_Balance")
            objClass.EmployerName = rw("EmployerName")
            objClass.Branch = rw("Branch")
            objClass.Status = rw("Status")
            objClass.IngresID = rw("IngresID")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindPhoto(ByVal dt As DataTable, ByRef objClass As PHASE3.Photo)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.fld_Photo = rw("fld_Photo")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindSignature(ByVal dt As DataTable, ByRef objClass As PHASE3.Signature)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.fld_Signature = rw("fld_Signature")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Sub BindSurvey(ByVal dt As DataTable, ByRef objClass As PHASE3.Survey)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.Home_Ownership = rw("Home_Ownership")
            objClass.Number_Years = rw("Number_Years")
            objClass.Future_Plan_Home = rw("Future_Plan_Home")
            objClass.Educational_Attainment = rw("Educational_Attainment")
            objClass.Travels_Abroad = rw("Travels_Abroad")
            objClass.Domestic_Travel = rw("Domestic_Travel")
            objClass.Dine_Out = rw("Dine_Out")
            objClass.Mall_Visit = rw("Mall_Visit")
            objClass.Number_Dependent_Studying = rw("Number_Dependent_Studying")
            objClass.Number_Vehicles = rw("Number_Vehicles")
            objClass.Partner_Establishment = rw("Partner_Establishment")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            objClass.Desired_Loan_Amount = rw("Desired_Loan_Amount")
            objClass.Afford_Monthly_Payment_Loan = rw("Afford_Monthly_Payment_Loan")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindBio(ByVal dt As DataTable, ByRef objClass As PHASE3.Bio)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.fld_LeftPrimaryFP_template = rw("fld_LeftPrimaryFP_template")
            objClass.fld_LeftPrimaryFP_IsOverride = rw("fld_LeftPrimaryFP_IsOverride")
            objClass.fld_LeftPrimaryFP_Ansi = rw("fld_LeftPrimaryFP_Ansi")
            objClass.fld_LeftPrimaryFP_Wsq = rw("fld_LeftPrimaryFP_Wsq")
            objClass.fld_LeftSecondaryFP_template = rw("fld_LeftSecondaryFP_template")
            objClass.fld_LeftSecondaryFP_IsOverride = rw("fld_LeftSecondaryFP_IsOverride")
            objClass.fld_LeftSecondaryFP_Ansi = rw("fld_LeftSecondaryFP_Ansi")
            objClass.fld_LeftSecondaryFP_Wsq = rw("fld_LeftSecondaryFP_Wsq")
            objClass.fld_RightPrimaryFP_template = rw("fld_RightPrimaryFP_template")
            objClass.fld_RightPrimaryFP_IsOverride = rw("fld_RightPrimaryFP_IsOverride")
            objClass.fld_RightPrimaryFP_Ansi = rw("fld_RightPrimaryFP_Ansi")
            objClass.fld_RightPrimaryFP_Wsq = rw("fld_RightPrimaryFP_Wsq")
            objClass.fld_RightSecondaryFP_template = rw("fld_RightSecondaryFP_template")
            objClass.fld_RightSecondaryFP_IsOverride = rw("fld_RightSecondaryFP_IsOverride")
            objClass.fld_RightSecondaryFP_Ansi = rw("fld_RightSecondaryFP_Ansi")
            objClass.fld_RightSecondaryFP_Wsq = rw("fld_RightSecondaryFP_Wsq")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindPhotoValidID(ByVal dt As DataTable, ByRef objClass As PHASE3.PhotoValidID)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.fld_PhotoID = rw("fld_PhotoID")
            objClass.fld_IDType = rw("fld_IDType")
            objClass.fld_IDNumber = rw("fld_IDNumber")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")

        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindDCS_Card_Account(ByVal dt As DataTable, ByRef objClass As PHASE3.DCS_Card_Account)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.BankCode = rw("BankCode")
            objClass.CardNo = rw("CardNo")
            objClass.AccountNumber = rw("AccountNumber")
            objClass.EntryDate = rw("EntryDate")
            objClass.EntryUsername = rw("EntryUsername")
            objClass.LastUpdate = rw("LastUpdate")
            objClass.LastUpdateUserName = rw("LastUpdateUserName")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub Bindinstant_issuance(ByVal dt As DataTable, ByRef objClass As PHASE3.InstanceIssuance)
        If dt.DefaultView.Count > 0 Then
            Dim rw As DataRow = dt.Rows(0)
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.MIDRTN = rw("MIDRTN")
            objClass.OCR = rw("OCR")
            objClass.PrintedDate = rw("PrintedDate")
            objClass.PrintCounter = rw("PrintCounter")
            objClass.PrinterSerial = rw("PrinterSerial")
            objClass.EntryDate = rw("EntryDate")
            objClass.IsSent = rw("IsSent")
            objClass.DateSent = rw("DateSent")
            objClass.ApplicationDate = rw("ApplicationDate")
            objClass.PrintCardCounter = rw("PrintCardCounter")
        Else
            objClass.ErrorMessage = "No record found"
        End If
    End Sub

    Private Sub BindDCS_Card_ReprintList(ByVal dt As DataTable, ByRef objClasses As PHASE3.DCS_Card_ReprintList)
        For Each rw As DataRow In dt.Rows
            Dim objClass As New PHASE3.DCS_Card_Reprint
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.NewCardNo = rw("NewCardNo")
            objClass.OldCardNo = rw("OldCardNo")
            objClass.Remarks = rw("Remarks")
            objClass.Username = rw("Username")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            'objClasses = New PHASE3.DCS_Card_ReprintList
            objClasses.DCS_Card_ReprintList.Add(objClass)
        Next
    End Sub


    Private Sub BindMemberEmploymentHistoryList(ByVal dt As DataTable, ByRef objClasses As PHASE3.MemberEmploymentHistoryList)
        For Each rw As DataRow In dt.Rows
            Dim objClass As New PHASE3.MemberEmploymentHistory
            objClass.IsSuccess = True
            objClass.RefNum = rw("RefNum")
            objClass.PagIBIGID = rw("PagIBIGID")
            objClass.History_EmployerName = rw("History_EmployerName")
            objClass.History_EmployerAddress = rw("History_EmployerAddress")
            objClass.History_DateEmployed = rw("History_DateEmployed")
            objClass.History_DateSeparated = rw("History_DateSeparated")
            objClass.Office_Assignment = rw("Office_Assignment")
            objClass.EntryDate = rw("EntryDate")
            objClass.LastUpdate = rw("LastUpdate")
            objClasses.MemberEmploymentHistoryList.Add(objClass)

        Next
    End Sub

    <WebMethod()>
    Public Function GetMemberByMID(ByVal requestAuth As PHASE3.RequestAuth,
                             ByVal MID As String) As PHASE3.Member
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim member As New PHASE3.Member

        If requestAuth Is Nothing Then
            member.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            Return member
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            member.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            Return member
        End If


        Dim DAL As New DAL
        Try
            If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Member WHERE RefNum=dbo.fnGetLatestRefNumByPagIBIDID('{0}')", MID)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    member.IsSuccess = True
                    member.RefNum = rw("RefNum")
                    member.PagIBIGID = rw("PagIBIGID")
                    member.Member_LastName = rw("Member_LastName")
                    member.Member_FirstName = rw("Member_FirstName")
                    member.Member_MiddleName = rw("Member_MiddleName")
                    member.Member_Extension = rw("Member_Extension")
                    member.Member_NoMiddleName = rw("Member_NoMiddleName")
                    member.Birth_LastName = rw("Birth_LastName")
                    member.Birth_FirstName = rw("Birth_FirstName")
                    member.Birth_MiddleName = rw("Birth_MiddleName")
                    member.Birth_NoMiddleName = rw("Birth_NoMiddleName")
                    member.Birth_Extension = rw("Birth_Extension")
                    member.Mother_LastName = rw("Mother_LastName")
                    member.Mother_FirstName = rw("Mother_FirstName")
                    member.Mother_MiddleName = rw("Mother_MiddleName")
                    member.Mother_Extension = rw("Mother_Extension")
                    member.Mother_NoMiddleName = rw("Mother_NoMiddleName")
                    member.Spouse_LastName = rw("Spouse_LastName")
                    member.Spouse_FirstName = rw("Spouse_FirstName")
                    member.Spouse_MiddleName = rw("Spouse_MiddleName")
                    member.Spouse_Extension = rw("Spouse_Extension")
                    member.Spouse_NoMiddleName = rw("Spouse_NoMiddleName")
                    member.BirthDate = rw("BirthDate")
                    member.BirthCity = rw("BirthCity")
                    member.BirthCountry = rw("BirthCountry")
                    member.Gender = rw("Gender")
                    member.CivilStatus = rw("CivilStatus")
                    member.Citizenship = rw("Citizenship")
                    member.CommonRefNo = rw("CommonRefNo")
                    member.SSSID = rw("SSSID")
                    member.GSISID = rw("GSISID")
                    member.TIN = rw("TIN")
                    member.MembershipCategory = rw("MembershipCategory")
                    member.ApplicationDate = rw("ApplicationDate")
                    member.KioskID = rw("KioskID")
                    member.Transaction_Ref_No = rw("Transaction_Ref_No")
                    member.Capture_Type = rw("Capture_Type")
                    member.PFR_Number = rw("PFR_Number")
                    member.PFR_Amount = rw("PFR_Amount")
                    member.PFR_Date = rw("PFR_Date")
                    member.IsMemberActive = rw("IsMemberActive")
                    member.IsComplete = rw("IsComplete")
                    member.Card_PFRNumber = rw("Card_PFRNumber")
                    member.Card_PFRAmount = rw("Card_PFRAmount")
                    member.Card_PFRDate = rw("Card_PFRDate")
                    member.Application_Remarks = rw("Application_Remarks")
                    member.requesting_branchcode = rw("requesting_branchcode")
                    member.BranchCode = rw("BranchCode")
                    member.UserName = rw("UserName")
                    member.DocumentSubmitted = rw("DocumentSubmitted")
                    member.EntryDate = rw("EntryDate")
                    member.LastUpdate = rw("LastUpdate")
                    member.PaymentStatus = rw("PaymentStatus")
                    member.BillingCtrlNum = rw("BillingCtrlNum")
                    member.PaymentRemarks = rw("PaymentRemarks")
                    member.CardCounter = rw("CardCounter")
                    member.CitizenshipCode = rw("CitizenshipCode")

                Else
                    member.ErrorMessage = "No record found"
                End If
            Else
                member.ErrorMessage = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            End If

            Return member
        Catch ex As Exception
            member.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & member.ErrorMessage)
            Return member
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetMemberByMIDAndApplicationDate(ByVal requestAuth As PHASE3.RequestAuth,
                              ByVal MID As String, ByVal ApplicationDate As Date) As PHASE3.Member
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim member As New PHASE3.Member

        If requestAuth Is Nothing Then
            member.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            Return member
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            member.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            Return member
        End If


        Dim DAL As New DAL
        Try
            If DAL.SelectQuery(String.Format("SELECT TOP 1 * FROM tbl_Member WHERE PagIBIGID='{0}' AND ApplicationDate='{1}' ORDER BY ID DESC)", MID, ApplicationDate)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    member.IsSuccess = True
                    member.RefNum = rw("RefNum")
                    member.PagIBIGID = rw("PagIBIGID")
                    member.Member_LastName = rw("Member_LastName")
                    member.Member_FirstName = rw("Member_FirstName")
                    member.Member_MiddleName = rw("Member_MiddleName")
                    member.Member_Extension = rw("Member_Extension")
                    member.Member_NoMiddleName = rw("Member_NoMiddleName")
                    member.Birth_LastName = rw("Birth_LastName")
                    member.Birth_FirstName = rw("Birth_FirstName")
                    member.Birth_MiddleName = rw("Birth_MiddleName")
                    member.Birth_NoMiddleName = rw("Birth_NoMiddleName")
                    member.Birth_Extension = rw("Birth_Extension")
                    member.Mother_LastName = rw("Mother_LastName")
                    member.Mother_FirstName = rw("Mother_FirstName")
                    member.Mother_MiddleName = rw("Mother_MiddleName")
                    member.Mother_Extension = rw("Mother_Extension")
                    member.Mother_NoMiddleName = rw("Mother_NoMiddleName")
                    member.Spouse_LastName = rw("Spouse_LastName")
                    member.Spouse_FirstName = rw("Spouse_FirstName")
                    member.Spouse_MiddleName = rw("Spouse_MiddleName")
                    member.Spouse_Extension = rw("Spouse_Extension")
                    member.Spouse_NoMiddleName = rw("Spouse_NoMiddleName")
                    member.BirthDate = rw("BirthDate")
                    member.BirthCity = rw("BirthCity")
                    member.BirthCountry = rw("BirthCountry")
                    member.Gender = rw("Gender")
                    member.CivilStatus = rw("CivilStatus")
                    member.Citizenship = rw("Citizenship")
                    member.CommonRefNo = rw("CommonRefNo")
                    member.SSSID = rw("SSSID")
                    member.GSISID = rw("GSISID")
                    member.TIN = rw("TIN")
                    member.MembershipCategory = rw("MembershipCategory")
                    member.ApplicationDate = rw("ApplicationDate")
                    member.KioskID = rw("KioskID")
                    member.Transaction_Ref_No = rw("Transaction_Ref_No")
                    member.Capture_Type = rw("Capture_Type")
                    member.PFR_Number = rw("PFR_Number")
                    member.PFR_Amount = rw("PFR_Amount")
                    member.PFR_Date = rw("PFR_Date")
                    member.IsMemberActive = rw("IsMemberActive")
                    member.IsComplete = rw("IsComplete")
                    member.Card_PFRNumber = rw("Card_PFRNumber")
                    member.Card_PFRAmount = rw("Card_PFRAmount")
                    member.Card_PFRDate = rw("Card_PFRDate")
                    member.Application_Remarks = rw("Application_Remarks")
                    member.requesting_branchcode = rw("requesting_branchcode")
                    member.BranchCode = rw("BranchCode")
                    member.UserName = rw("UserName")
                    member.DocumentSubmitted = rw("DocumentSubmitted")
                    member.EntryDate = rw("EntryDate")
                    member.LastUpdate = rw("LastUpdate")
                    member.PaymentStatus = rw("PaymentStatus")
                    member.BillingCtrlNum = rw("BillingCtrlNum")
                    member.PaymentRemarks = rw("PaymentRemarks")
                    member.CardCounter = rw("CardCounter")
                    member.CitizenshipCode = rw("CitizenshipCode")

                Else
                    member.ErrorMessage = "No record found"
                End If
            Else
                member.ErrorMessage = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & member.ErrorMessage)
            End If

            Return member
        Catch ex As Exception
            member.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & member.ErrorMessage)
            Return member
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function IsMemberExistByMIDAndApplicationDate(ByVal requestAuth As PHASE3.RequestAuth,
                                                         ByVal MID As String, ByVal ApplicationDate As Date) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            'member.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'member.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return True
        End If


        Dim DAL As New DAL
        Try
            If DAL.SelectQuery(String.Format("SELECT TOP 1 [RefNum] FROM tbl_Member WHERE PagIBIGID='{0}' AND ApplicationDate='{1}'", MID, ApplicationDate.ToString("yyyy-MM-dd"))) Then
                ' If DAL.SelectQuery(String.Format("SELECT TOP 1 * FROM tbl_Member WHERE PagIBIGID='{0}' AND ApplicationDate='{1}' ORDER BY ID DESC", MID, ApplicationDate.ToString("yyyy-MM-dd"))) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    Return True
                Else
                    'member.ErrorMessage = "No record found"
                    Return False
                End If
            Else
                'member.ErrorMessage = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return True
        Catch ex As Exception
            'member.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return True
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function IsMemberHasAccount(ByVal requestAuth As PHASE3.RequestAuth,
                                                         ByVal MID As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            'member.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            'member.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return True
        End If


        Dim DAL As New DAL
        Try
            If DAL.SelectQuery(String.Format("SELECT TOP 1 [RefNum] FROM tbl_Member WHERE PagIBIGID='{0}' ", MID)) Then
                'If DAL.SelectQuery(String.Format("SELECT TOP 1 * FROM tbl_Member WHERE PagIBIGID='{0}' ORDER BY ID DESC", MID)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    Return True
                Else
                    'member.ErrorMessage = "No record found"
                    Return False
                End If
            Else
                'member.ErrorMessage = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If

            Return True
        Catch ex As Exception
            'member.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return True
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

#Region "GetCardCounter"


    'updated 4/29/2015
    <WebMethod()>
    Public Function GetCardCounter(ByVal requestAuth As PHASE3.RequestAuth, ByVal MIDRTN As String) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar(String.Format("SELECT COUNT(PagIBIGID) FROM tbl_Member WHERE PagIBIGID='{0}'", MIDRTN)) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalCapturedByUser(ByVal requestAuth As PHASE3.RequestAuth,
                                           ByVal UserName As String) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByUser(UserName) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalCapturedByUser_Recapture(ByVal requestAuth As PHASE3.RequestAuth,
                                           ByVal UserName As String) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByUser(UserName, True) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalCapturedByUserAndDate(ByVal requestAuth As PHASE3.RequestAuth,
                                                  ByVal UserName As String, ByVal ApplicationDate As Date) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByUserAndDate(UserName, ApplicationDate) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalCapturedByUserAndDate_Recapture(ByVal requestAuth As PHASE3.RequestAuth,
                                                            ByVal UserName As String, ByVal ApplicationDate As Date) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByUserAndDate(UserName, ApplicationDate, True) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalCapturedByKioskAndDate(ByVal requestAuth As PHASE3.RequestAuth,
                                                   ByVal KioskID As String, ByVal ApplicationDate As Date) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByKioskAndDate(KioskID, ApplicationDate) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function


    <WebMethod()>
    Public Function GetTotalCapturedByKioskAndDate_Recapture(ByVal requestAuth As PHASE3.RequestAuth,
                                                             ByVal KioskID As String, ByVal ApplicationDate As Date) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalCapturedByKioskAndDate(KioskID, ApplicationDate, True) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

#End Region



    <WebMethod()>
    Public Function InsertAuditTrail(ByVal requestAuth As PHASE3.RequestAuth,
                                     ByVal AuditDT As DataTable) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim DAL As New DAL
        Try
            If Not DAL.AddAuditTrail(AuditDT.Rows(0)(0), AuditDT.Rows(0)(1), AuditDT.Rows(0)(2), AuditDT.Rows(0)(3), AuditDT.Rows(0)(4), AuditDT.Rows(0)(5), AuditDT.Rows(0)(6)) Then
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return Nothing
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function ValidateUserAndAccess(ByVal requestAuth As PHASE3.RequestAuth,
                                          ByVal UserName As String, ByVal UserPassword As String,
                                          ByVal KioskID As String, ByVal TerminalMAC As String,
                                          ByVal RequestingBranchCode As String) As PHASE3.LogInResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim loginResponse As New PHASE3.LogInResponse

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            loginResponse.GetErrorMsg = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            Return loginResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            loginResponse.GetErrorMsg = PHASE3.PHASE3.WS_AuthFailedMsg
            Return loginResponse
        End If

        Dim DAL As New DAL
        Try
            If Not DAL.ValidateUserAndAccess(UserName, PHASE3.PHASE3.Encrypt(UserPassword), KioskID, TerminalMAC, RequestingBranchCode) Then
                loginResponse.IsSuccess = False
                loginResponse.GetErrorMsg = DAL.ErrorMessage
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & loginResponse.GetErrorMsg)
            Else
                If DAL.ObjectResult.ToString.Split("|")(0) = 0 Then
                    Dim loginResponseArr() As String = DAL.ObjectResult.ToString.Split("|")
                    loginResponse.IsSuccess = True
                    loginResponse.UserID = loginResponseArr(1)
                    loginResponse.Username = UserName
                    loginResponse.FullName = loginResponseArr(2)
                    loginResponse.UserRole = loginResponseArr(3)
                    'loginResponse.IdNumber = loginResponseArr(4)
                    loginResponse.CaptureType = "Individual"
                    loginResponse.CardPFRAmount = 100.0

                    Dim userBanks As List(Of PHASE3.UserBank) = Nothing
                    If DAL.SelectUserBank(loginResponse.UserID) Then
                        For Each rw As DataRow In DAL.TableResult.Rows
                            If userBanks Is Nothing Then userBanks = New List(Of PHASE3.UserBank)
                            Dim userBank As New PHASE3.UserBank
                            userBank.BankID = rw("BankID")
                            userBank.BankCode = rw("BankCode")
                            userBank.BankName = rw("BankName")
                            userBanks.Add(userBank)
                        Next
                    End If

                    Dim userEmployerBranches As List(Of PHASE3.UserEmployerBranch) = Nothing
                    If DAL.SelectUserEmployerBranch(loginResponse.UserID) Then
                        For Each rw As DataRow In DAL.TableResult.Rows
                            If userEmployerBranches Is Nothing Then userEmployerBranches = New List(Of PHASE3.UserEmployerBranch)
                            Dim userEmployerBranch As New PHASE3.UserEmployerBranch
                            userEmployerBranch.EmployerID = rw("EmployerID")
                            userEmployerBranch.EmployerName = rw("EmployerName")
                            userEmployerBranch.EmployerBranchID = rw("EmployerBranchID")
                            userEmployerBranch.EmployerBranch = rw("EmployerBranch")
                            userEmployerBranches.Add(userEmployerBranch)
                        Next
                    End If

                    loginResponse.DCSUserBank = userBanks
                    loginResponse.DCSEmployerBranch = userEmployerBranches
                Else
                    loginResponse.IsSuccess = False
                    loginResponse.GetErrorMsg = DAL.ObjectResult.ToString.Split("|")(1).Trim
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & loginResponse.GetErrorMsg)
                End If
            End If

            Return loginResponse
        Catch ex As Exception
            loginResponse.IsSuccess = False
            loginResponse.GetErrorMsg = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & loginResponse.GetErrorMsg)
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Function ValidateWebAccess(ByVal User As String, ByVal Pass As String) As Boolean
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GenerateSessionRefTxn(ByVal requestAuth As PHASE3.RequestAuth) As String
        Dim rn As New Random
        Return String.Format("{0}{1}{2}{3}", requestAuth.User.ToString.PadLeft(3, "0"), requestAuth.KioskID, Now.ToString("yyyyMMddhhmmss"), rn.Next(1000, 9999))
    End Function

#Region " AUB "

    'AppDomain.CurrentDomain.BaseDirectory
    Private privateKey As String = System.IO.File.ReadAllText(My.Settings.WS_Repo & "\security\privateKey.key")
    'public key expose to consumer.
    Private publicKey As String = System.IO.File.ReadAllText(My.Settings.WS_Repo & "\security\publicKey.pem")

    'public certificate same as public key but in certificate file.
    Private publicCert As Byte() = System.IO.File.ReadAllBytes(My.Settings.WS_Repo & "\security\certificate.crt")

    'Public key to validate the response data of the AUB API.
    Dim publicKey_aub As String = System.IO.File.ReadAllText(My.Settings.WS_Repo & "\security\aub.pem")


    <WebMethod()>
    Public Function CreateAccount_AUB(ByVal requestAuth As PHASE3.RequestAuth,
                                      ByVal AUBCreateAccountRequest As PHASE3.AUBCreateAccountRequest) As PHASE3.AUBResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim aubResponse As New PHASE3.AUBResponse

        If requestAuth Is Nothing Then
            aubResponse.resultMessage = aubResponse.resultMessage
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & aubResponse.resultMessage)
            Return aubResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            aubResponse.resultMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return aubResponse
        End If

        Try
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

            System.IO.File.WriteAllText(String.Format(My.Settings.WS_Repo & "\AUB\{0}_00.txt", sessionRefTxn), Newtonsoft.Json.JsonConvert.SerializeObject(AUBCreateAccountRequest))

            Dim addressCodes As String = PHASE3.PHASE3.GetAddressCodes(AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.city,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.provinceDesc,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.city,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.provinceDesc)
            'AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.city = addressCodes.Split("|")(0)
            'AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.province = addressCodes.Split("|")(1)
            'AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.city = addressCodes.Split("|")(2)
            'AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.province = addressCodes.Split("|")(3)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.idType2 = PHASE3.PHASE3.GetAUBIDType(AUBCreateAccountRequest.cashCardApplication.clientInfo.idType2)

            Return AUBRequestProcess(requestAuth, AUBCreateAccountRequest, PHASE3.DataKeysEnum.AUBRequest.CreateAccount, sessionRefTxn)
        Catch ex As Exception
            aubResponse.resultMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            Return aubResponse
        End Try
    End Function

    <WebMethod()>
    Public Function CreateAccount_AUB_Prod(ByVal requestAuth As PHASE3.RequestAuth,
                                      ByVal AUBCreateAccountRequest As PHASE3.AUBCreateAccountRequest) As PHASE3.AUBResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim aubResponse As New PHASE3.AUBResponse

        If requestAuth Is Nothing Then
            aubResponse.resultMessage = aubResponse.resultMessage
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & aubResponse.resultMessage)
            Return aubResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            aubResponse.resultMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return aubResponse
        End If

        Try
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

            System.IO.File.WriteAllText(String.Format(My.Settings.WS_Repo & "\AUB\{0}_00.txt", sessionRefTxn), Newtonsoft.Json.JsonConvert.SerializeObject(AUBCreateAccountRequest))

            Dim addressCodes As String = PHASE3.PHASE3.GetAddressCodes(AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.city,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.provinceDesc,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.city,
                                                                       AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.provinceDesc)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.city = addressCodes.Split("|")(0)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.homeAddress.province = addressCodes.Split("|")(1)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.city = addressCodes.Split("|")(2)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.permanentAddress.province = addressCodes.Split("|")(3)
            AUBCreateAccountRequest.cashCardApplication.clientInfo.idType2 = PHASE3.PHASE3.GetAUBIDType(AUBCreateAccountRequest.cashCardApplication.clientInfo.idType2)

            Return AUBRequestProcess(requestAuth, AUBCreateAccountRequest, PHASE3.DataKeysEnum.AUBRequest.CreateAccount, sessionRefTxn)
        Catch ex As Exception
            aubResponse.resultMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            Return aubResponse
        End Try
    End Function

    <WebMethod()>
    Public Function ReplaceCard_AUB(ByVal requestAuth As PHASE3.RequestAuth,
                                    ByVal AUBReplaceCardRequest As PHASE3.AUBReplaceCardRequest) As PHASE3.AUBResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim aubResponse As New PHASE3.AUBResponse

        If requestAuth Is Nothing Then
            aubResponse.resultMessage = aubResponse.resultMessage
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & aubResponse.resultMessage)
            Return aubResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            aubResponse.resultMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return aubResponse
        End If

        Try
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

            Return AUBRequestProcess(requestAuth, AUBReplaceCardRequest, PHASE3.DataKeysEnum.AUBRequest.ReplaceCard, sessionRefTxn)
        Catch ex As Exception
            aubResponse.resultMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            Return aubResponse
        End Try

    End Function

    <WebMethod()>
    Public Function GetCardNo_AUB(ByVal requestAuth As PHASE3.RequestAuth,
                                  ByVal AUBGetCardNoRequest As PHASE3.AUBGetCardNoRequest) As PHASE3.AUBResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim aubResponse As New PHASE3.AUBResponse

        If requestAuth Is Nothing Then
            aubResponse.resultMessage = aubResponse.resultMessage
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & aubResponse.resultMessage)
            Return aubResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            aubResponse.resultMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
            Return aubResponse
        End If

        Try
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

            Return AUBRequestProcess(requestAuth, AUBGetCardNoRequest, PHASE3.DataKeysEnum.AUBRequest.GetCardNo, sessionRefTxn)
        Catch ex As Exception
            aubResponse.resultMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            Return aubResponse
        End Try
    End Function

    'ByVal RequestPayload As String,

    Public Function RemoteCertificateValidationCallback(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Function AUBRequestProcess(ByVal requestAuth As PHASE3.RequestAuth,
                                       ByVal AUBRequestObject As Object,
                                       ByVal RequestType As PHASE3.DataKeysEnum.AUBRequest,
                                       ByVal sessionRefTxn As String) As PHASE3.AUBResponse


        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim aubResponse As New PHASE3.AUBResponse

        Dim RequestPayload As String = ""
        Dim pagIBIGID As String = ""
        Dim cardNo As String = ""

        Select Case RequestType
            Case PHASE3.DataKeysEnum.AUBRequest.CreateAccount
                Dim aubCreateAccountRequest As PHASE3.AUBCreateAccountRequest = CType(AUBRequestObject, PHASE3.AUBCreateAccountRequest)
                pagIBIGID = aubCreateAccountRequest.cashCardApplication.clientInfo.idNo1
                cardNo = aubCreateAccountRequest.cashCardApplication.cardNo
                RequestPayload = Newtonsoft.Json.JsonConvert.SerializeObject(aubCreateAccountRequest)

                System.IO.File.WriteAllText(String.Format(My.Settings.WS_Repo & "\AUB\{0}_01.txt", sessionRefTxn), RequestPayload)
            Case PHASE3.DataKeysEnum.AUBRequest.ReplaceCard
                RequestPayload = Newtonsoft.Json.JsonConvert.SerializeObject(CType(AUBRequestObject, PHASE3.AUBReplaceCardRequest))
            Case PHASE3.DataKeysEnum.AUBRequest.GetCardNo
                RequestPayload = Newtonsoft.Json.JsonConvert.SerializeObject(CType(AUBRequestObject, PHASE3.AUBGetCardNoRequest))
        End Select

        Try
            Dim reqPayload As String = RequestPayload.Replace("request_sub", "sub")

            'Encrypting your payload with private key using RS256
            Dim token = RS256Parser.RSAParserTool.CreateTokenV2(reqPayload, privateKey)

            'If RequestType = PHASE3.DataKeysEnum.AUBRequest.CreateAccount Then
            System.IO.File.WriteAllText(String.Format(My.Settings.WS_Repo & "\AUB\{0}_02.txt", sessionRefTxn), token)
            'End If

            Dim response As String = ""

            Try
                ph3API = New PHASE3.PHASE3
                Dim aubRequest As New PHASE3.AUBRequest
                aubRequest.jwt = token
                Dim blnResponse As Boolean = ph3API.SendAUBRequest(RequestType, aubRequest, response)
                If blnResponse Then
                    'Decoding the response And validate.
                    Dim decryptedResponse = RS256Parser.RSAParserTool.DecodeTokenByPem(response, publicKey_aub)

                    Dim dynObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(decryptedResponse)
                    aubResponse.TxnNo = String.Format("{0}{1}{2}", requestAuth.User.ToString.PadLeft(3, "0"), requestAuth.KioskID, Now.ToString("yyyyMMddhhmmss"))
                    aubResponse.exp = dynObj.Item("exp")
                    aubResponse.aud = dynObj.Item("aud")
                    aubResponse.jti = dynObj.Item("jti")
                    aubResponse.resultCode = CInt(dynObj.Item("resultCode"))
                    aubResponse.resultMessage = dynObj.Item("resultMessage")
                    If dynObj.ContainsKey("cardNo") Then aubResponse.cardNo = dynObj.Item("cardNo")
                    If dynObj.ContainsKey("accountNo") Then aubResponse.accountNo = dynObj.Item("accountNo")
                    If dynObj.ContainsKey("sub") Then aubResponse.response_sub = dynObj.Item("sub")
                    ' get acctNo for getcardNo
                    If dynObj.ContainsKey("acctNo") Then aubResponse.accountNo = dynObj.Item("acctNo")

                    aubResponse.DateTimeAUBResponse = Now.ToString
                    aubResponse.TokenRequest = token

                    System.IO.File.WriteAllText(String.Format(My.Settings.WS_Repo & "\AUB\{0}_03.txt", sessionRefTxn), aubResponse.resultMessage)

                    Dim DAL As New DAL
                    Dim RefTxnNo As String = ""

                    If RequestType = PHASE3.DataKeysEnum.AUBRequest.ReplaceCard Then
                        If DAL.SelectQuery("SELECT AUBTxnID, TxnNo FROM tbl_AUB WHERE PagIBIGID='" & pagIBIGID & "' AND DateTimeCompleted IS NOT NULL ORDER BY AUBTxnID DESC") Then
                            If DAL.TableResult.DefaultView.Count > 0 Then
                                If Not IsDBNull(DAL.TableResult.Rows(0)("TxnNo")) Then RefTxnNo = DAL.TableResult.Rows(0)("TxnNo").ToString.Trim
                            End If
                        End If
                    End If

                    If Not DAL.AddAUB(requestAuth.User, requestAuth.KioskID, aubResponse.TxnNo, System.[Enum].GetName(GetType(PHASE3.DataKeysEnum.AUBRequest), RequestType), pagIBIGID, cardNo, aubResponse.accountNo, "Request", RefTxnNo) Then
                        aubResponse.resultMessage = "Failed to insert aub transaction"
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                    End If
                    DAL.Dispose()
                    DAL = Nothing
                Else
                    aubResponse.resultMessage = response
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & aubResponse.resultMessage)
                End If
            Catch ex As Exception
                aubResponse.resultMessage = ex.Message
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            End Try

            Return aubResponse
        Catch ex As Exception
            aubResponse.resultMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & aubResponse.resultMessage)
            Return aubResponse
        Finally
        End Try
    End Function

#End Region

    <WebMethod()>
    Public Function SPX100(ByVal key1 As String, ByVal key2 As String) As String
        If key1 <> "201906-ACC*" Then Return "Invalid key"

        Return PHASE3.PHASE3.Encrypt(key2)
    End Function

    <WebMethod()>
    Public Function SPX101(ByVal key1 As String, ByVal key2 As String) As String
        If key1 <> "201906-ACC*" Then Return "Invalid key"

        Return PHASE3.PHASE3.Decrypt(key2)
    End Function

    <WebMethod()>
    Public Function SaveMember(ByVal requestAuth As PHASE3.RequestAuth,
                               ByVal Member As PHASE3.Member,
                               ByVal MembershipCategoryInfo As PHASE3.MembershipCategoryInfo,
                               ByVal MemberContactinfo As PHASE3.MemberContactinfo,
                               ByVal MemContribution As PHASE3.MemContribution,
                               ByVal Photo As PHASE3.Photo,
                               ByVal Signature As PHASE3.Signature,
                               ByVal Bio As PHASE3.Bio,
                               ByVal Survey As PHASE3.Survey,
                               ByVal PhotoValidID As PHASE3.PhotoValidID,
                               ByVal DCS_Card_Account As PHASE3.DCS_Card_Account,
                               ByVal Card As PHASE3.Card,
                               ByVal InstanceIssuance As PHASE3.InstanceIssuance,
                               ByVal DCS_Card_ReprintList As PHASE3.DCS_Card_ReprintList,
                               ByVal MemberEmploymentHistoryList As PHASE3.MemberEmploymentHistoryList) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse

        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Member Is Nothing Then
            requestResponse.ErrorMessage = "Member is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MembershipCategoryInfo Is Nothing Then
            requestResponse.ErrorMessage = "MembershipCategoryInfo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MemberContactinfo Is Nothing Then
            requestResponse.ErrorMessage = "MemberContactinfo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MemContribution Is Nothing Then
            requestResponse.ErrorMessage = "MemContribution is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Photo Is Nothing Then
            requestResponse.ErrorMessage = "Photo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Signature Is Nothing Then
            requestResponse.ErrorMessage = "Signature is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Bio Is Nothing Then
            requestResponse.ErrorMessage = "Bio is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Survey Is Nothing Then
            requestResponse.ErrorMessage = "Survey is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf DCS_Card_Account Is Nothing Then
            requestResponse.ErrorMessage = "DCS_Card_Account is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf PhotoValidID Is Nothing Then
            requestResponse.ErrorMessage = "PhotoValidID is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf InstanceIssuance Is Nothing Then
            requestResponse.ErrorMessage = "InstanceIssuance is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        ' Add here account number getter if AUB.
        If My.Settings.BankID = 2 Then
            Dim getCardName As New PHASE3.name
            getCardName.firstName = Member.Member_FirstName
            getCardName.middleName = Member.Member_MiddleName
            getCardName.lastName = Member.Member_LastName

            Dim getCardinquiry As New PHASE3.inquiry
            getCardinquiry.birthdate = Member.BirthDate.ToString("yyyy-MM-dd")
            getCardinquiry.name = getCardName
            getCardinquiry.idNo = Member.PagIBIGID

            Dim AUBGetCardNoRequest As New PHASE3.AUBGetCardNoRequest
            AUBGetCardNoRequest.inquiry = getCardinquiry
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)
            Dim result = AUBRequestProcess(requestAuth, AUBGetCardNoRequest, PHASE3.DataKeysEnum.AUBRequest.GetCardNo, sessionRefTxn)

            If result.resultCode = 0 Then
                DCS_Card_Account.AccountNumber = result.accountNo
            End If
        End If
        If DCS_Card_Account.AccountNumber.StartsWith("000000000000") And Not Member.Application_Remarks.Contains("Re-card") Then
            requestResponse.ErrorMessage = "Invalid account number."
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim binList = My.Settings.BankBin.Split("|")
        Dim cardBin = DCS_Card_Account.CardNo.Substring(0, 6)
        If Not binList.Where(Function(a) a.StartsWith(cardBin)).Any Then
            requestResponse.IsSuccess = False
            requestResponse.ErrorMessage = "Invalid Card Bin. Only allow " + My.Settings.BankBin + " ."
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim IsProcessSuccess As Boolean = False

        Dim oldCardNo As String = ""
        Dim origOldCardNo As String = ""
        Dim newCardNo As String = ""

        Dim DAL As New DAL
        'Dim myTrans As System.Data.SqlClient.SqlTransaction = Nothing

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)

        Try
            '1 save Member
            If Member.Member_NoMiddleName = -1 Then Member.Member_NoMiddleName = 1
            If Member.Mother_NoMiddleName = -1 Then Member.Mother_NoMiddleName = 1
            If Member.Spouse_NoMiddleName = -1 Then Member.Spouse_NoMiddleName = 1
            'If Member.Member_NoMiddleName = -1 Then Member.Member_NoMiddleName = 1
            Member.PFR_Date = Now
            If Not Member.SaveToDbase(DAL, myTrans) Then
                requestResponse.ErrorMessage = Member.ErrorMessage
            Else

                '2 save MemberContactinfo
                If Not MemberContactinfo.SaveToDbase(DAL, myTrans) Then
                    requestResponse.ErrorMessage = MemberContactinfo.ErrorMessage
                Else

                    '3 save MembershipCategoryInfo
                    If Not MembershipCategoryInfo.SaveToDbase(DAL, myTrans) Then
                        requestResponse.ErrorMessage = MembershipCategoryInfo.ErrorMessage
                    Else

                        '4 save MemContribution
                        MemContribution.LastPFR_Date = Now
                        MemContribution.InitialPFR_Date = Now
                        If Not MemContribution.SaveToDbase(DAL, myTrans) Then
                            requestResponse.ErrorMessage = MemContribution.ErrorMessage
                        Else

                            '5 save Photo
                            If Not Photo.SaveToDbase(DAL, myTrans) Then
                                requestResponse.ErrorMessage = Photo.ErrorMessage
                            Else

                                '6 save Signature
                                If Not Signature.SaveToDbase(DAL, myTrans) Then
                                    requestResponse.ErrorMessage = Signature.ErrorMessage
                                Else

                                    '7 save Biometrics
                                    If Not Bio.SaveToDbase(DAL, myTrans) Then
                                        requestResponse.ErrorMessage = Bio.ErrorMessage
                                    Else

                                        '8 save Survey
                                        If Not Survey.SaveToDbase(DAL, myTrans) Then
                                            requestResponse.ErrorMessage = Survey.ErrorMessage
                                        Else

                                            '9 save PhotoValidID
                                            If Not PhotoValidID.SaveToDbase(DAL, myTrans) Then
                                                requestResponse.ErrorMessage = PhotoValidID.ErrorMessage
                                            Else
                                                '10 save DCS_Card_Account
                                                If Not DCS_Card_Account.SaveToDbase(DAL, myTrans) Then
                                                    requestResponse.ErrorMessage = DCS_Card_Account.ErrorMessage
                                                Else

                                                    '11 save Card
                                                    If Not Card.SaveToDbase(DAL, myTrans) Then
                                                        requestResponse.ErrorMessage = Card.ErrorMessage
                                                    Else

                                                        'save Instance Issuance
                                                        If Not InstanceIssuance.SaveToDbase(DAL, myTrans) Then
                                                            requestResponse.ErrorMessage = InstanceIssuance.ErrorMessage
                                                        Else

                                                            Dim bln As Boolean = True

                                                            If Not DCS_Card_ReprintList Is Nothing Then
                                                                '12 save DCS_Card_Reprint

                                                                For i As Short = 0 To DCS_Card_ReprintList.DCS_Card_ReprintList.Count - 1
                                                                    If Not DCS_Card_ReprintList.DCS_Card_ReprintList(i) Is Nothing Then
                                                                        If Not DCS_Card_ReprintList.DCS_Card_ReprintList(i).SaveToDbase(DAL, myTrans) Then
                                                                            requestResponse.ErrorMessage = DCS_Card_ReprintList.DCS_Card_ReprintList(i).ErrorMessage
                                                                            bln = False
                                                                            Exit For
                                                                        Else
                                                                            oldCardNo = DCS_Card_ReprintList.DCS_Card_ReprintList(i).OldCardNo
                                                                            newCardNo = DCS_Card_ReprintList.DCS_Card_ReprintList(i).NewCardNo
                                                                            origOldCardNo = oldCardNo
                                                                        End If
                                                                    End If
                                                                Next
                                                            End If

                                                            If bln Then

                                                                If Not MemberEmploymentHistoryList Is Nothing Then
                                                                    '13 save Employment History

                                                                    For i As Short = 0 To MemberEmploymentHistoryList.MemberEmploymentHistoryList.Count - 1
                                                                        If Not MemberEmploymentHistoryList.MemberEmploymentHistoryList(i) Is Nothing Then
                                                                            If Not MemberEmploymentHistoryList.MemberEmploymentHistoryList(i).SaveToDbase(DAL, myTrans) Then
                                                                                requestResponse.ErrorMessage = MemberEmploymentHistoryList.MemberEmploymentHistoryList(i).ErrorMessage
                                                                                bln = False
                                                                                Exit For
                                                                            End If
                                                                        End If
                                                                    Next

                                                                    'For Each EmploymentHistory As PHASE3.MemberEmploymentHistory In MemberEmploymentHistoryList.MemberEmploymentHistoryList
                                                                    '    If Not EmploymentHistory Is Nothing Then
                                                                    '        If Not EmploymentHistory.SaveToDbase(DAL, myTrans) Then
                                                                    '            requestResponse.ErrorMessage = EmploymentHistory.ErrorMessage
                                                                    '            bln = False
                                                                    '            Exit For
                                                                    '        End If
                                                                    '    End If
                                                                    'Next
                                                                End If
                                                            End If

                                                            IsProcessSuccess = bln
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            ' 041 UBP , 102 AUB
            'Dim bankCode As String = "041"
            Dim bankCode As String = "000"
            If My.Settings.BankID = "1" Then bankCode = "041"
            If My.Settings.BankID = "2" Then bankCode = "102"



            If IsProcessSuccess Then
                ph3API = New PHASE3.PHASE3
                Dim IsReversal = False
                If Not Member.Application_Remarks.Contains("Re-card") Then
                    Dim pushCardInfoResult As PHASE3.PushCardInfoResult = PushCardInfoLocal(requestAuth,
                                                          Member.PagIBIGID, Member.Member_LastName, Member.Member_FirstName, Member.Member_MiddleName,
                                                          Member.BirthDate, (MemberContactinfo.Mobile_AreaCode.Trim & MemberContactinfo.Mobile_CelNo.Trim).Trim,
                                                          DCS_Card_Account.CardNo, DCS_Card_Account.AccountNumber, Card.ExpiryDate, Member.ApplicationDate.Date, bankCode, Member.requesting_branchcode)

                    If pushCardInfoResult.IsSuccess Then
                        For i As Short = 0 To myTrans.Count - 1
                            myTrans(i).Commit()
                        Next
                        requestResponse.IsSuccess = True
                    Else
                        ' This will handle the reversal for PushCardInfo.
                        Dim activeCardErrorMessage = ""
                        ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                        If ph3API.ActiveCardInfoResult.IsGet Then
                            Dim currentActiveCard = ph3API.ActiveCardInfoResult.CardNumber
                            If currentActiveCard = DCS_Card_Account.CardNo Then
                                For i As Short = 0 To myTrans.Count - 1
                                    myTrans(i).Commit()
                                Next
                                requestResponse.ErrorMessage = String.Empty
                                requestResponse.IsSuccess = True
                                IsReversal = True
                            End If
                        End If

                        If Not IsReversal Then
                            For i As Short = 0 To myTrans.Count - 1
                                myTrans(i).Rollback()
                            Next
                            requestResponse.ErrorMessage = pushCardInfoResult.GetErrorMsg
                            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                        End If
                    End If
                Else
                    If oldCardNo = "" Then
                        requestResponse.ErrorMessage = Member.PagIBIGID & " have empty old account no"
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & Member.PagIBIGID & " have empty old account no")
                    ElseIf newCardNo = "" Then
                        requestResponse.ErrorMessage = Member.PagIBIGID & " have empty new account no"
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & Member.PagIBIGID & " have empty new account no")
                    Else
                        ' Active card info update.
                        Dim activeCardErrorMessage = ""
                        Dim activeCardResult = ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                        oldCardNo = IIf(ph3API.ActiveCardInfoResult.IsGet, ph3API.ActiveCardInfoResult.CardNumber, oldCardNo)


                        Dim updateCardInfoResult As PHASE3.UpdateCardInfoResult = UpdateCardInfoLocal(requestAuth, Member.PagIBIGID, oldCardNo, newCardNo, String.Empty, bankCode)
                        If updateCardInfoResult.IsSuccess Then
                            For i As Short = 0 To myTrans.Count - 1
                                myTrans(i).Commit()
                            Next
                            requestResponse.IsSuccess = True
                        Else
                            ' This will handle the reversal for uploadCardInfo.
                            activeCardErrorMessage = ""
                            ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                            If ph3API.ActiveCardInfoResult.IsGet Then
                                Dim currentActiveCard = ph3API.ActiveCardInfoResult.CardNumber
                                If currentActiveCard = DCS_Card_Account.CardNo Then
                                    For i As Short = 0 To myTrans.Count - 1
                                        myTrans(i).Commit()
                                    Next
                                    requestResponse.ErrorMessage = String.Empty
                                    requestResponse.IsSuccess = True
                                    IsReversal = True
                                End If
                            End If

                            If Not IsReversal Then
                                For i As Short = 0 To myTrans.Count - 1
                                    myTrans(i).Rollback()
                                Next
                                requestResponse.ErrorMessage = updateCardInfoResult.GetErrorMsg
                                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                            End If
                        End If
                    End If
                End If

            Else
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Rollback()
                Next
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            End If

            If My.Settings.BankID = "1" And requestResponse.IsSuccess Then
                Try
                    Dim reqBranch As String = ""
                    Dim reqBranchAddress As String = ""

                    If DAL.SelectQuery("SELECT ISNULL(HBUR,'') As HBUR, ISNULL(Building,'') As Building, ISNULL(LotNo,'') As LotNo, ISNULL(BlockNo,'') As BlockNo, ISNULL(PhaseNo,'') As PhaseNo, ISNULL(HouseNo,'') As HouseNo, ISNULL(StreetName,'') As StreetName, ISNULL(Subdivision,'') As Subdivision, ISNULL(Barangay,'') As Barangay, ISNULL(CityMinicipality,'') As CityMinicipality, ISNULL(Province,'') As Province, ISNULL(ZipCode,'') As ZipCode, ISNULL(Region,'') As Region, ISNULL(Branch,'') As Branch FROM tbl_branch WHERE requesting_branchcode='" & Member.requesting_branchcode & "'") Then
                        If DAL.TableResult.DefaultView.Count > 0 Then
                            reqBranch = DAL.TableResult.Rows(0)("Branch").ToString
                            reqBranchAddress = DAL.TableResult.Rows(0)("HBUR").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Building").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("LotNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("BlockNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("PhaseNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("HouseNo").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("StreetName").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Subdivision").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Barangay").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("CityMinicipality").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Province").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("Region").ToString & " " &
                                                                                            DAL.TableResult.Rows(0)("ZipCode").ToString
                        End If
                    End If

                    Dim arrData As String = ""
                    Dim permanentAddress As String = Trim(MemberContactinfo.Permanent_HBUR & " " & MemberContactinfo.Permanent_Building & " " & MemberContactinfo.Permanent_LotNo & " " & MemberContactinfo.Permanent_BlockNo & " " & MemberContactinfo.Permanent_PhaseNo & " " & MemberContactinfo.Permanent_HouseNo & " " & MemberContactinfo.Permanent_StreetName & " " & MemberContactinfo.Permanent_Subdivision & " " & MemberContactinfo.Permanent_Barangay & " " & MemberContactinfo.Permanent_CityMunicipality & " " & MemberContactinfo.Permanent_Province & " " & MemberContactinfo.Permanent_Region & " " & MemberContactinfo.Permanent_ZipCode)
                    Dim presentAddress As String = Trim(MemberContactinfo.Present_HBUR & " " & MemberContactinfo.Present_Building & " " & MemberContactinfo.Present_LotNo & " " & MemberContactinfo.Present_BlockNo & " " & MemberContactinfo.Present_PhaseNo & " " & MemberContactinfo.Present_HouseNo & " " & MemberContactinfo.Present_StreetName & " " & MemberContactinfo.Present_Subdivision & " " & MemberContactinfo.Present_Barangay & " " & MemberContactinfo.Present_CityMunicipality & " " & MemberContactinfo.Present_Province & " " & MemberContactinfo.Present_Region & " " & MemberContactinfo.Present_ZipCode)
                    Dim employerAddress As String = Trim(MembershipCategoryInfo.Employer_HBUR & " " & MembershipCategoryInfo.Employer_Building & " " & MembershipCategoryInfo.Employer_LotNo & " " & MembershipCategoryInfo.Employer_BlockNo & " " & MembershipCategoryInfo.Employer_PhaseNo & " " & MembershipCategoryInfo.Employer_HouseNo & " " & MembershipCategoryInfo.Employer_StreetName & " " & MembershipCategoryInfo.Employer_Subdivision & " " & MembershipCategoryInfo.Employer_Barangay & " " & MembershipCategoryInfo.Employer_CityMunicipality & " " & MembershipCategoryInfo.Employer_Province & " " & MembershipCategoryInfo.Employer_Region & " " & MembershipCategoryInfo.Employer_ZipCode)

                    permanentAddress = permanentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                    presentAddress = presentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                    employerAddress = employerAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")

                    Dim IsErrorFlag As Boolean

                    Dim arrPermanentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(permanentAddress), 50, IsErrorFlag).Split(vbNewLine)
                    Dim arrPresentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(presentAddress), 50, IsErrorFlag).Split(vbNewLine)
                    Dim arrEmployerAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(employerAddress), 50, IsErrorFlag).Split(vbNewLine)
                    Dim arrReqBranchAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(reqBranchAddress), 50, IsErrorFlag).Split(vbNewLine)

                    Dim arrPermanentAddress1 As String = ""
                    Dim arrPermanentAddress2 As String = ""
                    Dim arrPermanentAddress3 As String = ""
                    Dim arrPresentAddress1 As String = ""
                    Dim arrPresentAddress2 As String = ""
                    Dim arrPresentAddress3 As String = ""
                    Dim arrEmployerAddress1 As String = ""
                    Dim arrEmployerAddress2 As String = ""
                    Dim arrEmployerAddress3 As String = ""
                    Dim arrReqBranchAddress1 As String = ""
                    Dim arrReqBranchAddress2 As String = ""
                    Dim arrReqBranchAddress3 As String = ""

                    For i As Short = 0 To arrPermanentAddress.Length - 1
                        Select Case Trim(arrPermanentAddress(i))
                            Case "", " ", vbNewLine
                            Case Else
                                Select Case i
                                    Case 0
                                        arrPermanentAddress1 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 1
                                        arrPermanentAddress2 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 2
                                        arrPermanentAddress3 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                End Select
                        End Select
                    Next

                    For i As Short = 0 To arrPresentAddress.Length - 1
                        Select Case Trim(arrPresentAddress(i))
                            Case "", " ", vbNewLine
                            Case Else
                                Select Case i
                                    Case 0
                                        arrPresentAddress1 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 1
                                        arrPresentAddress2 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 2
                                        arrPresentAddress3 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                End Select
                        End Select
                    Next

                    For i As Short = 0 To arrEmployerAddress.Length - 1
                        Select Case Trim(arrEmployerAddress(i))
                            Case "", " ", vbNewLine
                            Case Else
                                Select Case i
                                    Case 0
                                        arrEmployerAddress1 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 1
                                        arrEmployerAddress2 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 2
                                        arrEmployerAddress3 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                End Select
                        End Select
                    Next

                    For i As Short = 0 To arrReqBranchAddress.Length - 1
                        Select Case Trim(arrReqBranchAddress(i))
                            Case "", " ", vbNewLine
                            Case Else
                                Select Case i
                                    Case 0
                                        arrReqBranchAddress1 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 1
                                        arrReqBranchAddress2 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    Case 2
                                        arrReqBranchAddress3 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                End Select
                        End Select
                    Next
                    'added by edel to handle address length

                    ' Handling for MID
                    Dim sssID = Member.SSSID
                    Dim gsisID = Member.GSISID
                    If sssID = "" And gsisID IsNot "" Then
                        sssID = Member.PagIBIGID
                    ElseIf sssID IsNot "" And gsisID = "" Then
                        gsisID = Member.PagIBIGID
                    ElseIf sssID IsNot "" And gsisID IsNot "" Then
                        gsisID = Member.PagIBIGID
                    Else
                        gsisID = Member.PagIBIGID
                    End If
                    ' Handling for recarding.
                    Dim employeeID = MembershipCategoryInfo.EmployeeID
                    Dim _accountNumber = DCS_Card_Account.AccountNumber
                    If Member.Application_Remarks.Contains("Re-card") Then
                        'employeeID = oldCardNo ' Should replace by old card number.

                        'revised on 07/22/2020 to fix memfiles issue of oldCardNo=newCardNo
                        'If oldCardNo <> newCardNo Then
                        '    employeeID = oldCardNo
                        'Else
                        '    employeeID = origOldCardNo '''origOldCardNo came from DSA
                        'End If

                        employeeID = origOldCardNo

                        _accountNumber = DCS_Card_Account.CardNo
                    End If

                    'added by edel on 06/11/2020
                    Dim firstNameWithSuffix As String = Member.Member_FirstName & " " & Member.Member_Extension

                    arrData =
                                                            CheckFieldLimit(_accountNumber, Constants.ACC_NUMBER) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(Member.Member_LastName), Constants.LAST_NAME) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(firstNameWithSuffix), Constants.FIRST_NAME) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(Member.Member_MiddleName), Constants.MIDDLE_NAME) & "|" &
                                                             "P|" &
                                                             CheckFieldLimit(CDate(Member.BirthDate).ToString("MM/dd/yyyy"), Constants.DATE_OF_BIRTH) & "|" &
                                                             Member.Gender.Substring(0, 1).ToUpper & "|" &
                                                             CheckFieldLimit(Member.CivilStatus.Substring(0, 1).ToUpper, Constants.MARITAL_STATUS) & "|" &
                                                             CheckFieldLimit((MemberContactinfo.Mobile_AreaCode & MemberContactinfo.Mobile_CelNo).Replace("+", "").Replace(" ", ""), Constants.CELL_PHONE_NUMBER) & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(MemberContactinfo.Home_CountryCode & " " & MemberContactinfo.Home_AreaCode & " " & MemberContactinfo.Home_TelNo).Replace(" ", ""), Constants.HOME_TEL_NUMBER) & "|" &
                                                             CheckFieldLimit(MemberContactinfo.EmailAddress, Constants.EMAIL_ADDRESS) & "|" &
                                                             CheckFieldLimit(Member.TIN.ToUpper, Constants.TIN) & "|" &
                                                             arrPermanentAddress1 & "|" &
                                                             arrPermanentAddress2 & "|" &
                                                             arrPermanentAddress3 & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(reqBranch.Trim), Constants.MAILING_ADDRESS1) & "|" &
                                                             arrReqBranchAddress1 & "|" &
                                                             arrReqBranchAddress2 & "|" &
                                                             arrReqBranchAddress3 & "|" &
                                                             "" & "|" &
                                                             "0NBBISCOCHO" & "|" &
                                                             "PI1" & "|" &
                                                             Now.ToString("MM/dd/yyyy") & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(Member.BirthCity), Constants.BPLACE) & "|" &
                                                             "Filipino" & "|" &
                                                             "H1000" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(sssID, Constants.SSS) & "|" &
                                                             CheckFieldLimit(gsisID, Constants.GSIS) & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(employeeID), Constants.EMPLOYEE_ID) & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(MembershipCategoryInfo.EmployerName), Constants.COMP_NAME) & "|" &
                                                             arrEmployerAddress1 & "|" &
                                                             arrEmployerAddress2 & "|" &
                                                             arrEmployerAddress3 & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "|||||||||||"

                    arrData = arrData.Replace("ñ", "n")
                    arrData = arrData.Replace("Ñ", "N")

                    PHASE3.PHASE3.PackUBPData(requestAuth, Trim(arrData), Member.PagIBIGID, DCS_Card_Account.AccountNumber,
                                                                                      Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                                                      Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                                                      Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.Application_Remarks.Contains("Re-card"))

                    Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

                    If DAL.AddUBP(requestAuth.User, requestAuth.KioskID, sessionRefTxn, Member.PagIBIGID, DCS_Card_Account.AccountNumber) Then
                    End If
                Catch ex As Exception
                    requestResponse.IsSuccess = False
                    requestResponse.ErrorMessage = ex.Message
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                End Try
            End If

            If requestResponse.IsSuccess Then
                'Check if data exist on db.
                Dim isRefNumExist = DAL.ExecuteScalar(String.Format("SELECT ISNULL(RefNum,'') FROM tbl_Member WHERE RefNum='{0}'", Member.RefNum))
                If isRefNumExist Then
                    If DAL.ObjectResult Is Nothing Or DAL.ObjectResult.ToString.Trim = "" Then
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = methodName + "(): has been failed RefNum is not exist."
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
                    Else
                        requestResponse.IsSuccess = True
                        'myTrans = New List(Of SqlClient.SqlTransaction)
                        '' New feature.
                        'Dim memberUpload As New DCS_Upload
                        'memberUpload.Status = Constants.MEMBER_UPLOAD_COMPLETE
                        'memberUpload.PagIBIGID = Member.PagIBIGID
                        'memberUpload.RefNum = Member.RefNum
                        'memberUpload.IsPushCardInfo = True
                        'memberUpload.PushCardInfoDate = DateTime.Now
                        'memberUpload.IsPackUpData = True
                        'memberUpload.PackUpDataDate = DateTime.Now
                        'memberUpload.Remarks = ""
                        'memberUpload.SaveToDbase(DAL, myTrans)
                        'For i As Short = 0 To myTrans.Count - 1
                        '    myTrans(i).Commit()
                        'Next
                    End If
                Else
                    requestResponse.IsSuccess = False
                    requestResponse.ErrorMessage = methodName + "(): has been failed RefNum is not exist."
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
                End If
            End If

            Return requestResponse
        Catch ex As Exception

            If myTrans.Count() > 0 Then
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Rollback()
                Next
            End If

            requestResponse.IsSuccess = False
            requestResponse.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
            Return requestResponse
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Function CheckFieldLimit(ByVal obj As String, ByVal size As Integer) As String
        If obj = "" Then
            Return obj
        ElseIf size > obj.Length Then
            Return obj
        Else
            Return obj.Substring(0, size)
        End If
    End Function

    <WebMethod()>
    Public Function WSErrorLogs() As List(Of PHASE3.LogTrace)
        Dim logTraces As New List(Of PHASE3.LogTrace)

        Dim logRepo As String = String.Format("{0}\{1}", PHASE3.PHASE3.LogsRepo, Now.ToString("yyyy-MM-dd"))
        Dim fileLog As String = String.Format("{0}\{1}", logRepo, PHASE3.PHASE3.ErrorLog)

        Dim alllines() As String = System.IO.File.ReadAllLines(fileLog)

        Dim sb As New StringBuilder
        Dim intThreshold As Integer = 10
        If intThreshold > alllines.Length Then intThreshold = alllines.Length

        For i As Short = 0 To intThreshold - 1
            Dim logTrace As New PHASE3.LogTrace
            logTrace.Index = i + 1
            logTrace.Timestamp = alllines((alllines.Length - intThreshold) + i).Split("|")(0)
            logTrace.Log = alllines((alllines.Length - intThreshold) + i).Split("|")(3)
            logTraces.Add(logTrace)
            logTrace = Nothing
        Next

        Return logTraces
    End Function

    <WebMethod()>
    Public Function GetConfig() As String

        Dim bankCode As String = "000"
        If My.Settings.BankID = "1" Then bankCode = "UBP"
        If My.Settings.BankID = "2" Then bankCode = "AUB"
        Dim config = PHASE3.PHASE3.Encrypt(bankCode & "|" & My.Settings.Database & "|" & My.Settings.Server)
        Return bankCode
    End Function
    <WebMethod()>
    Public Function SaveCardTransaction(ByVal requestAuth As PHASE3.RequestAuth, ByVal cardTransaction As DCS_Card_Transaction) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim DAL As New DAL

        If Not cardTransaction.SaveToDbase(DAL, myTrans) Then
            requestResponse.ErrorMessage = cardTransaction.ErrorMessage
        Else
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Commit()
            Next
            requestResponse.IsSuccess = True

        End If

        Return requestResponse
    End Function

    <WebMethod()>
    Public Function GetTotalSpoiledCardByBranchAndDate(ByVal requestAuth As PHASE3.RequestAuth,
                                                             ByVal BranchCode As String, ByVal ApplicationDate As Date) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetSpoiledCardCountByDateAndBranchCode(ApplicationDate, BranchCode) Then
                Return Convert.ToInt32(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetTotalBranchCards(ByVal requestAuth As PHASE3.RequestAuth,
                                                             ByVal BranchCode As String) As Integer
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return -1
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return -1
        End If

        Dim DAL As New DAL
        Try
            If DAL.GetTotalBranchCards(BranchCode) Then
                Return CInt(DAL.ObjectResult)
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return -1
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return -1
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function


    <WebMethod()>
    Public Function IsBranchHasSpoiledCardsTransaction(ByVal requestAuth As PHASE3.RequestAuth,
                                                             ByVal BranchCode As String, ByVal dateValue As DateTime) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return True
        End If

        Dim DAL As New DAL
        Try
            Dim dateFrom = New DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 0, 0, 0)
            Dim dateTo = New DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 23, 59, 59)

            If DAL.ExecuteScalar(String.Format("SELECT ISNULL(TransactionNo,'') FROM tbl_DCS_Card_Transaction WHERE BranchCode='{0}' AND TransactionDate BETWEEN '{1}' AND '{2}' AND TransactionTypeID = '03'", BranchCode, dateFrom, dateTo)) Then
                If DAL.ObjectResult Is Nothing Then
                    Return False
                ElseIf DAL.ObjectResult.ToString.Trim = "" Then
                    Return False
                Else
                    Return True
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return True
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return True
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetActiveCardInfo(ByVal requestAuth As PHASE3.RequestAuth, ByVal MID_RTN As String) As ActiveCardInfoResult
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        Dim bankCode As String = "000"
        If My.Settings.BankID = "1" Then bankCode = "041"
        If My.Settings.BankID = "2" Then bankCode = "102"


        GetActiveCardInfo = New ActiveCardInfoResult

        If requestAuth Is Nothing Then
            GetActiveCardInfo.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return GetActiveCardInfo
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            GetActiveCardInfo.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            Return GetActiveCardInfo
        End If

        Try
            ph3API = New PHASE3.PHASE3
            Dim response As String = ""
            Dim blnResponse As Boolean = ph3API.ActiveCardInfo(MID_RTN, bankCode, response)
            If blnResponse Then
                If Not IsNothing(GetActiveCardInfo.ErrorMessage) Then
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & GetActiveCardInfo.ErrorMessage)
                End If

                If ph3API.ActiveCardInfoResult.IsGet = True Then
                    GetActiveCardInfo = ph3API.ActiveCardInfoResult
                End If
            Else
                GetActiveCardInfo.ErrorMessage = response
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & GetActiveCardInfo.ErrorMessage)
            End If

        Catch ex As Exception
            GetActiveCardInfo.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & GetActiveCardInfo.ErrorMessage)
        End Try
        Return GetActiveCardInfo
    End Function

#Region "Reversal Transaction"
    <WebMethod()>
    Public Function IsRefNumExist(ByVal requestAuth As PHASE3.RequestAuth,
                                                            ByVal RefNum As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return True
        End If

        Dim DAL As New DAL
        Try
            If DAL.ExecuteScalar(String.Format("SELECT ISNULL(RefNum,'') FROM tbl_Member WHERE RefNum='{0}'", RefNum)) Then
                If DAL.ObjectResult Is Nothing Then
                    Return False
                ElseIf DAL.ObjectResult.ToString.Trim = "" Then
                    Return False
                ElseIf DAL.ObjectResult.ToString.Trim = RefNum Then
                    Return True
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return False
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try

        Return False
    End Function


    <WebMethod()>
    Public Function IsReceiptSaved(ByVal requestAuth As PHASE3.RequestAuth,
                                   ByVal ORNumber As String, ByVal PagIBIGID As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return True
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return True
        End If

        Dim DAL As New DAL
        Try
            'If DAL.ExecuteScalarCentralized(String.Format("SELECT ISNULL(ORNumber,'') FROM tbl_DCS_OR WHERE ORNumber='{0}' AND PagIBIGID='{1}' AND BankID='{2}'", ORNumber, PagIBIGID, My.Settings.BankID)) Then
            If DAL.ExecuteScalar(String.Format("SELECT ISNULL(ORNumber,'') FROM tbl_DCS_OR WHERE ORNumber='{0}' AND PagIBIGID='{1}'", ORNumber, PagIBIGID)) Then
                If DAL.ObjectResult Is Nothing Then
                    Return False
                ElseIf DAL.ObjectResult.ToString.Trim = "" Then
                    Return False
                Else
                    Return True
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                Return True
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            Return True
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function
#End Region

    <WebMethod>
    Public Function ManualPushCardInfo(ByVal requestAuth As PHASE3.RequestAuth, ByVal refNumber As String) As PHASE3.RequestResponse
        Dim DAL As New DAL
        Try
            Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
            Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


            Dim MemberContactinfo As New PHASE3.MemberContactinfo
            Dim DCS_Card_Account As New PHASE3.DCS_Card_Account
            Dim Photo As New PHASE3.Photo
            Dim Member As New PHASE3.Member
            Dim requestResponse As New PHASE3.RequestResponse
            Dim Card As New PHASE3.Card



            If MemberContactinfo.Load(DAL, refNumber) Then
                If DCS_Card_Account.Load(DAL, refNumber) Then
                    Dim binList = My.Settings.BankBin.Split("|")
                    Dim cardBin = DCS_Card_Account.CardNo.Substring(0, 6)
                    If Not binList.Where(Function(a) a.StartsWith(cardBin)).Any Then
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = "Invalid Card Bin. Only allow " + My.Settings.BankBin + " ."
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                        Return requestResponse
                    ElseIf DCS_Card_Account.AccountNumber = "0000000000000000" And Not Member.Application_Remarks.Contains("Re-card") Then
                        requestResponse.ErrorMessage = "Invalid account number."
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                        Return requestResponse
                    End If
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

            If requestResponse.IsSuccess And Not Member.Application_Remarks.Contains("Re-card") Then
                Dim pushCardInfoResult As PHASE3.PushCardInfoResult = PushCardInfoLocal(requestAuth,
                                                      Member.PagIBIGID, Member.Member_LastName, Member.Member_FirstName, Member.Member_MiddleName,
                                                      Member.BirthDate, (MemberContactinfo.Mobile_AreaCode.Trim & MemberContactinfo.Mobile_CelNo.Trim).Trim,
                                                      DCS_Card_Account.CardNo, DCS_Card_Account.AccountNumber, Card.ExpiryDate, Member.ApplicationDate.Date, bankCode, Member.requesting_branchcode)
                If pushCardInfoResult.IsSuccess Then
                    requestResponse.IsSuccess = True
                Else
                    requestResponse.IsSuccess = False
                    requestResponse.ErrorMessage = pushCardInfoResult.GetErrorMsg
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                End If

            End If
            Return requestResponse
        Catch ex As Exception
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function ManualPackUpData(ByVal refNumber As String, ByVal accountNumber As String) As PHASE3.RequestResponse
        Dim DAL As New DAL
        Try
            Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
            Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


            Dim MemberContactinfo As New PHASE3.MemberContactinfo
            Dim MembershipCategoryInfo As New PHASE3.MembershipCategoryInfo
            Dim DCS_Card_Account As New PHASE3.DCS_Card_Account
            Dim Photo As New PHASE3.Photo
            Dim Member As New PHASE3.Member
            Dim Signature As New PHASE3.Signature
            Dim PhotoValidID As New PHASE3.PhotoValidID
            Dim Bio As New PHASE3.Bio

            Dim requestAuth As New PHASE3.RequestAuth
            Dim response As New PHASE3.RequestResponse





            If MemberContactinfo.Load(DAL, refNumber) Then
                If MembershipCategoryInfo.Load(DAL, refNumber) Then
                    If DCS_Card_Account.Load(DAL, refNumber) Then
                        Dim binList = My.Settings.BankBin.Split("|")
                        Dim cardBin = DCS_Card_Account.CardNo.Substring(0, 6)
                        If Not binList.Where(Function(a) a.StartsWith(cardBin)).Any And (Not cardBin.Contains("****")) Then
                            response.IsSuccess = False
                            response.ErrorMessage = "Invalid Card Bin. Only allow " + My.Settings.BankBin + " ."
                            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & response.ErrorMessage)
                            Return response
                        ElseIf DCS_Card_Account.AccountNumber = "0000000000000000" And Not Member.Application_Remarks.Contains("Re-card") Then
                            response.ErrorMessage = "Invalid account number."
                            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & response.ErrorMessage)
                            Return response
                        End If

                        If Member.Load(DAL, refNumber) Then
                            If Photo.Load(DAL, refNumber) Then
                                If Signature.Load(DAL, refNumber) Then
                                    If PhotoValidID.Load(DAL, refNumber) Then
                                        If Bio.Load(DAL, refNumber) Then
                                            response.IsSuccess = True
                                        Else
                                            response.IsSuccess = False
                                            response.ErrorMessage = Bio.ErrorMessage
                                        End If
                                    Else
                                        response.IsSuccess = False
                                        response.ErrorMessage = PhotoValidID.ErrorMessage
                                    End If
                                Else
                                    response.IsSuccess = False
                                    response.ErrorMessage = Signature.ErrorMessage
                                End If
                            Else
                                response.IsSuccess = False
                                response.ErrorMessage = Photo.ErrorMessage
                            End If
                        Else
                            response.IsSuccess = False
                            response.ErrorMessage = Member.ErrorMessage
                        End If
                    Else
                        response.IsSuccess = False
                        response.ErrorMessage = DCS_Card_Account.ErrorMessage
                    End If
                Else
                    response.IsSuccess = False
                    response.ErrorMessage = MembershipCategoryInfo.ErrorMessage
                End If
            Else
                response.IsSuccess = False
                response.ErrorMessage = MemberContactinfo.ErrorMessage
            End If

            If response.IsSuccess = True Then
                If Not accountNumber = "" Then
                    Dim account = accountNumber.Substring(accountNumber.Length - 4, 4)
                    Dim dbaccount = DCS_Card_Account.AccountNumber.Substring(DCS_Card_Account.AccountNumber.Length - 4, 4)
                    If account = dbaccount Then
                        DCS_Card_Account.AccountNumber = accountNumber
                    End If
                End If

                If My.Settings.BankID = "1" Then
                    Try
                        Dim reqBranch As String = ""
                        Dim reqBranchAddress As String = ""

                        If DAL.SelectQuery("SELECT ISNULL(HBUR,'') As HBUR, ISNULL(Building,'') As Building, ISNULL(LotNo,'') As LotNo, ISNULL(BlockNo,'') As BlockNo, ISNULL(PhaseNo,'') As PhaseNo, ISNULL(HouseNo,'') As HouseNo, ISNULL(StreetName,'') As StreetName, ISNULL(Subdivision,'') As Subdivision, ISNULL(Barangay,'') As Barangay, ISNULL(CityMinicipality,'') As CityMinicipality, ISNULL(Province,'') As Province, ISNULL(ZipCode,'') As ZipCode, ISNULL(Region,'') As Region, ISNULL(Branch,'') As Branch FROM tbl_branch WHERE requesting_branchcode='" & Member.requesting_branchcode & "'") Then
                            If DAL.TableResult.DefaultView.Count > 0 Then
                                reqBranch = DAL.TableResult.Rows(0)("Branch").ToString
                                reqBranchAddress = DAL.TableResult.Rows(0)("HBUR").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Building").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("LotNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("BlockNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("PhaseNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("HouseNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("StreetName").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Subdivision").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Barangay").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("CityMinicipality").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Province").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Region").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("ZipCode").ToString
                            End If
                        End If

                        Dim arrData As String = ""
                        Dim permanentAddress As String = Trim(MemberContactinfo.Permanent_HBUR & " " & MemberContactinfo.Permanent_Building & " " & MemberContactinfo.Permanent_LotNo & " " & MemberContactinfo.Permanent_BlockNo & " " & MemberContactinfo.Permanent_PhaseNo & " " & MemberContactinfo.Permanent_HouseNo & " " & MemberContactinfo.Permanent_StreetName & " " & MemberContactinfo.Permanent_Subdivision & " " & MemberContactinfo.Permanent_Barangay & " " & MemberContactinfo.Permanent_CityMunicipality & " " & MemberContactinfo.Permanent_Province & " " & MemberContactinfo.Permanent_Region & " " & MemberContactinfo.Permanent_ZipCode)
                        Dim presentAddress As String = Trim(MemberContactinfo.Present_HBUR & " " & MemberContactinfo.Present_Building & " " & MemberContactinfo.Present_LotNo & " " & MemberContactinfo.Present_BlockNo & " " & MemberContactinfo.Present_PhaseNo & " " & MemberContactinfo.Present_HouseNo & " " & MemberContactinfo.Present_StreetName & " " & MemberContactinfo.Present_Subdivision & " " & MemberContactinfo.Present_Barangay & " " & MemberContactinfo.Present_CityMunicipality & " " & MemberContactinfo.Present_Province & " " & MemberContactinfo.Present_Region & " " & MemberContactinfo.Present_ZipCode)
                        Dim employerAddress As String = Trim(MembershipCategoryInfo.Employer_HBUR & " " & MembershipCategoryInfo.Employer_Building & " " & MembershipCategoryInfo.Employer_LotNo & " " & MembershipCategoryInfo.Employer_BlockNo & " " & MembershipCategoryInfo.Employer_PhaseNo & " " & MembershipCategoryInfo.Employer_HouseNo & " " & MembershipCategoryInfo.Employer_StreetName & " " & MembershipCategoryInfo.Employer_Subdivision & " " & MembershipCategoryInfo.Employer_Barangay & " " & MembershipCategoryInfo.Employer_CityMunicipality & " " & MembershipCategoryInfo.Employer_Province & " " & MembershipCategoryInfo.Employer_Region & " " & MembershipCategoryInfo.Employer_ZipCode)




                        permanentAddress = permanentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                        presentAddress = presentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                        employerAddress = employerAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")




                        Dim IsErrorFlag As Boolean

                        Dim arrPermanentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(permanentAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrPresentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(presentAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrEmployerAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(employerAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrReqBranchAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(reqBranchAddress), 50, IsErrorFlag).Split(vbNewLine)

                        Dim arrPermanentAddress1 As String = ""
                        Dim arrPermanentAddress2 As String = ""
                        Dim arrPermanentAddress3 As String = ""
                        Dim arrPresentAddress1 As String = ""
                        Dim arrPresentAddress2 As String = ""
                        Dim arrPresentAddress3 As String = ""
                        Dim arrEmployerAddress1 As String = ""
                        Dim arrEmployerAddress2 As String = ""
                        Dim arrEmployerAddress3 As String = ""
                        Dim arrReqBranchAddress1 As String = ""
                        Dim arrReqBranchAddress2 As String = ""
                        Dim arrReqBranchAddress3 As String = ""

                        For i As Short = 0 To arrPermanentAddress.Length - 1
                            Select Case Trim(arrPermanentAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrPermanentAddress1 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrPermanentAddress2 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrPermanentAddress3 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrPresentAddress.Length - 1
                            Select Case Trim(arrPresentAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrPresentAddress1 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrPresentAddress2 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrPresentAddress3 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrEmployerAddress.Length - 1
                            Select Case Trim(arrEmployerAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrEmployerAddress1 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrEmployerAddress2 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrEmployerAddress3 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrReqBranchAddress.Length - 1
                            Select Case Trim(arrReqBranchAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrReqBranchAddress1 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrReqBranchAddress2 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrReqBranchAddress3 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next
                        'added by edel to handle address length

                        ' Handling for MID
                        Dim sssID = Member.SSSID
                        Dim gsisID = Member.GSISID
                        If sssID = "" And gsisID IsNot "" Then
                            sssID = Member.PagIBIGID
                        ElseIf sssID IsNot "" And gsisID = "" Then
                            gsisID = Member.PagIBIGID
                        ElseIf sssID IsNot "" And gsisID IsNot "" Then
                            gsisID = Member.PagIBIGID
                        Else
                            gsisID = Member.PagIBIGID
                        End If
                        ' Handling for recarding.
                        Dim employeeID = MembershipCategoryInfo.EmployeeID
                        Dim _accountNumber = DCS_Card_Account.AccountNumber
                        If Member.Application_Remarks.Contains("Re-card") Then

                            Dim activeCardErrorMessage = ""
                            Dim activeCardResult = ph3API.ActiveCardInfo(Member.PagIBIGID, "041", activeCardErrorMessage)
                            employeeID = IIf(ph3API.ActiveCardInfoResult.IsGet, ph3API.ActiveCardInfoResult.CardNumber, employeeID)
                            _accountNumber = DCS_Card_Account.CardNo
                        End If

                        arrData =
                                                                _accountNumber & "|" &
                                                                RemoveInvalidCharacters(Member.Member_LastName) & "|" &
                                                                RemoveInvalidCharacters(Member.Member_FirstName) & "|" &
                                                                RemoveInvalidCharacters(Member.Member_MiddleName) & "|" &
                                                                 "P|" &
                                                                 CDate(Member.BirthDate).ToString("MM/dd/yyyy") & "|" &
                                                                 Member.Gender.Substring(0, 1).ToUpper & "|" &
                                                                 Member.CivilStatus.Substring(0, 1).ToUpper & "|" &
                                                                 (MemberContactinfo.Mobile_AreaCode & MemberContactinfo.Mobile_CelNo).Replace("+", "").Replace(" ", "") & "|" &
                                                                 RemoveInvalidCharacters(MemberContactinfo.Home_CountryCode & " " & MemberContactinfo.Home_AreaCode & " " & MemberContactinfo.Home_TelNo).Replace(" ", "") & "|" &
                                                                 MemberContactinfo.EmailAddress & "|" &
                                                                 Member.TIN.ToUpper & "|" &
                                                                 arrPermanentAddress1 & "|" &
                                                                 arrPermanentAddress2 & "|" &
                                                                 arrPermanentAddress3 & "|" &
                                                                 "" & "|" &
                                                                 RemoveInvalidCharacters(reqBranch.Trim) & "|" &
                                                                 arrReqBranchAddress1 & "|" &
                                                                 arrReqBranchAddress2 & "|" &
                                                                 arrReqBranchAddress3 & "|" &
                                                                 "" & "|" &
                                                                 "0NBBISCOCHO" & "|" &
                                                                 "PI1" & "|" &
                                                                 Now.ToString("MM/dd/yyyy") & "|" &
                                                                 "" & "|" &
                                                                 RemoveInvalidCharacters(Member.BirthCity) & "|" &
                                                                 "Filipino" & "|" &
                                                                 "H1000" & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 sssID & "|" &
                                                                 gsisID & "|" &
                                                                 "" & "|" &
                                                                 RemoveInvalidCharacters(employeeID) & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 RemoveInvalidCharacters(MembershipCategoryInfo.EmployerName) & "|" &
                                                                 arrEmployerAddress1 & "|" &
                                                                 arrEmployerAddress2 & "|" &
                                                                 arrEmployerAddress3 & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 "" & "|" &
                                                                 "|||||||||||"

                        arrData = arrData.Replace("ñ", "n")
                        arrData = arrData.Replace("Ñ", "N")

                        PHASE3.PHASE3.PackUBPData(requestAuth, Trim(arrData), Member.PagIBIGID, DCS_Card_Account.AccountNumber,
                                                                                          Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                                                          Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                                                          Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.Application_Remarks.Contains("Re-card"))

                        Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

                        If DAL.AddUBP(requestAuth.User, requestAuth.KioskID, sessionRefTxn, Member.PagIBIGID, DCS_Card_Account.AccountNumber) Then

                        End If
                        response.IsSuccess = True
                    Catch ex As Exception
                        response.IsSuccess = False
                        response.ErrorMessage = ex.Message
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                    End Try
                ElseIf My.Settings.BankID = "2" Then
                    Try
                        PHASE3.PHASE3.PackUBPData2(requestAuth, Member.PagIBIGID,
                                                       Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                       Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                       Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.ApplicationDate)

                        Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

                        response.IsSuccess = True
                    Catch ex As Exception
                        response.IsSuccess = False
                        response.ErrorMessage = ex.Message
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                    End Try
                End If
            End If
            Return response
        Catch ex As Exception
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Public Shared Function FormatDataWithCharLength(ByVal value As String, ByVal intCharLength As Short, ByRef IsErrorFlag As Boolean) As String
        Dim space As String = " "

        Dim sbOld As New System.Text.StringBuilder
        sbOld.Append(value)

        Dim sbNew As New System.Text.StringBuilder

        'check loop to see if it exceeds normal number
        Dim intLoop As Short = 0
        Dim intNormalLoop As Short = 5

        Do While sbOld.ToString <> ""
            Dim intSpaceLastIndex As Short
            If sbOld.ToString.Length > intCharLength Then
                If sbOld.ToString.Substring(intCharLength - 1, 1) = space Then
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intCharLength)))
                    sbOld.Remove(0, intCharLength)
                ElseIf sbOld.ToString.Substring(intCharLength - 1, 1) <> space And sbOld.ToString.Substring(intCharLength, 1) <> space Then
                    intSpaceLastIndex = sbOld.ToString.Substring(0, intCharLength).LastIndexOf(space)
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intSpaceLastIndex)))
                    sbOld.Remove(0, intSpaceLastIndex)
                Else
                    sbNew.AppendLine(Trim(sbOld.ToString.Substring(0, intCharLength)))
                    sbOld.Remove(0, intCharLength)
                End If

                intLoop += 1

                If intLoop > intNormalLoop Then
                    IsErrorFlag = True
                    Return ""
                End If
            Else
                sbNew.Append(Trim(sbOld.ToString.Substring(0)))
                sbOld.Clear()
            End If
        Loop

        IsErrorFlag = False
        Return sbNew.ToString
    End Function

    <WebMethod()>
    Public Function SaveCardSpoiled(ByVal requestAuth As PHASE3.RequestAuth, ByVal cardSpoileds As List(Of DCS_Card_Spoiled), ByVal isReplace As Boolean, ByVal userName As String, ByVal remarks As String, ByVal branchCode As String) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name




        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim DAL As New DAL



        Dim cardTransaction As New DCS_Card_Transaction
        cardTransaction.TransactionTypeID = IIf(isReplace, "06", "03")
        cardTransaction.Quantity = cardSpoileds.Count()
        cardTransaction.KioskID = requestAuth.KioskID
        cardTransaction.Username = userName
        cardTransaction.Remarks = remarks
        cardTransaction.BranchCode = branchCode

        If cardTransaction.SaveToDbase(DAL, myTrans) Then
            Try
                ' Save card spoiled list.
                Dim isSuccessList = True
                Dim errorMessage = String.Empty
                Dim transactionDate = DateTime.Now

                For Each spoiled As DCS_Card_Spoiled In cardSpoileds
                    spoiled.TransactionNo = cardTransaction.TransactionNo
                    spoiled.TransactionDate = transactionDate
                    spoiled.BranchCode = branchCode

                    If Not spoiled.SaveToDbase(DAL, myTrans) Then
                        isSuccessList = False
                        errorMessage = spoiled.ErrorMessage
                        Exit For
                    End If
                Next

                If isSuccessList Then
                    For i As Short = 0 To myTrans.Count - 1
                        myTrans(i).Commit()
                    Next
                    requestResponse.IsSuccess = True
                Else
                    For i As Short = 0 To myTrans.Count - 1
                        myTrans(i).Rollback()
                    Next
                    requestResponse.ErrorMessage = errorMessage
                    requestResponse.IsSuccess = False
                End If

            Catch ex As Exception
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Rollback()
                Next
                requestResponse.ErrorMessage = ex.Message
                requestResponse.IsSuccess = False
            End Try
        End If

        Return requestResponse
    End Function

    <WebMethod()>
    Public Function SaveMemberWithOptions(ByVal requestAuth As PHASE3.RequestAuth,
                               ByVal Member As PHASE3.Member,
                               ByVal MembershipCategoryInfo As PHASE3.MembershipCategoryInfo,
                               ByVal MemberContactinfo As PHASE3.MemberContactinfo,
                               ByVal MemContribution As PHASE3.MemContribution,
                               ByVal Photo As PHASE3.Photo,
                               ByVal Signature As PHASE3.Signature,
                               ByVal Bio As PHASE3.Bio,
                               ByVal Survey As PHASE3.Survey,
                               ByVal PhotoValidID As PHASE3.PhotoValidID,
                               ByVal DCS_Card_Account As PHASE3.DCS_Card_Account,
                               ByVal Card As PHASE3.Card,
                               ByVal InstanceIssuance As PHASE3.InstanceIssuance,
                               ByVal DCS_Card_ReprintList As PHASE3.DCS_Card_ReprintList,
                               ByVal MemberEmploymentHistoryList As PHASE3.MemberEmploymentHistoryList,
                               ByVal IsProcessPAGIBIGCardInfo As Boolean,
                               ByVal IsProcessPackUPData As Boolean) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse

        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Member Is Nothing Then
            requestResponse.ErrorMessage = "Member is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MembershipCategoryInfo Is Nothing Then
            requestResponse.ErrorMessage = "MembershipCategoryInfo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MemberContactinfo Is Nothing Then
            requestResponse.ErrorMessage = "MemberContactinfo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf MemContribution Is Nothing Then
            requestResponse.ErrorMessage = "MemContribution is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Photo Is Nothing Then
            requestResponse.ErrorMessage = "Photo is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Signature Is Nothing Then
            requestResponse.ErrorMessage = "Signature is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Bio Is Nothing Then
            requestResponse.ErrorMessage = "Bio is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Survey Is Nothing Then
            requestResponse.ErrorMessage = "Survey is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf DCS_Card_Account Is Nothing Then
            requestResponse.ErrorMessage = "DCS_Card_Account is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf PhotoValidID Is Nothing Then
            requestResponse.ErrorMessage = "PhotoValidID is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf InstanceIssuance Is Nothing Then
            requestResponse.ErrorMessage = "InstanceIssuance is nothing"
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        ' Add here account number getter if AUB.
        If My.Settings.BankID = 2 Then
            Dim getCardName As New PHASE3.name
            getCardName.firstName = Member.Member_FirstName
            getCardName.middleName = Member.Member_MiddleName
            getCardName.lastName = Member.Member_LastName

            Dim getCardinquiry As New PHASE3.inquiry
            getCardinquiry.birthdate = Member.BirthDate.ToString("yyyy-MM-dd")
            getCardinquiry.name = getCardName
            getCardinquiry.idNo = Member.PagIBIGID

            Dim AUBGetCardNoRequest As New PHASE3.AUBGetCardNoRequest
            AUBGetCardNoRequest.inquiry = getCardinquiry
            Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)
            Dim result = AUBRequestProcess(requestAuth, AUBGetCardNoRequest, PHASE3.DataKeysEnum.AUBRequest.GetCardNo, sessionRefTxn)

            If result.resultCode = 0 Then
                DCS_Card_Account.AccountNumber = result.accountNo
            End If
        End If
        If DCS_Card_Account.AccountNumber.StartsWith("000000000000") And Not Member.Application_Remarks.Contains("Re-card") Then
            requestResponse.ErrorMessage = "Invalid account number."
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim binList = My.Settings.BankBin.Split("|")
        Dim cardBin = DCS_Card_Account.CardNo.Substring(0, 6)
        If Not binList.Where(Function(a) a.StartsWith(cardBin)).Any Then
            requestResponse.IsSuccess = False
            requestResponse.ErrorMessage = "Invalid Card Bin. Only allow " + My.Settings.BankBin + " ."
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim IsProcessSuccess As Boolean = False

        Dim oldCardNo As String = ""
        Dim origOldCardNo As String = ""
        Dim newCardNo As String = ""

        Dim DAL As New DAL
        'Dim myTrans As System.Data.SqlClient.SqlTransaction = Nothing

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)

        Try
            '1 save Member
            If Member.Member_NoMiddleName = -1 Then Member.Member_NoMiddleName = 1
            If Member.Mother_NoMiddleName = -1 Then Member.Mother_NoMiddleName = 1
            If Member.Spouse_NoMiddleName = -1 Then Member.Spouse_NoMiddleName = 1
            'If Member.Member_NoMiddleName = -1 Then Member.Member_NoMiddleName = 1
            Member.PFR_Date = Now
            If Not Member.SaveToDbase(DAL, myTrans) Then
                requestResponse.ErrorMessage = Member.ErrorMessage
            Else

                '2 save MemberContactinfo
                If Not MemberContactinfo.SaveToDbase(DAL, myTrans) Then
                    requestResponse.ErrorMessage = MemberContactinfo.ErrorMessage
                Else

                    '3 save MembershipCategoryInfo
                    If Not MembershipCategoryInfo.SaveToDbase(DAL, myTrans) Then
                        requestResponse.ErrorMessage = MembershipCategoryInfo.ErrorMessage
                    Else

                        '4 save MemContribution
                        MemContribution.LastPFR_Date = Now
                        MemContribution.InitialPFR_Date = Now
                        If Not MemContribution.SaveToDbase(DAL, myTrans) Then
                            requestResponse.ErrorMessage = MemContribution.ErrorMessage
                        Else

                            '5 save Photo
                            If Not Photo.SaveToDbase(DAL, myTrans) Then
                                requestResponse.ErrorMessage = Photo.ErrorMessage
                            Else

                                '6 save Signature
                                If Not Signature.SaveToDbase(DAL, myTrans) Then
                                    requestResponse.ErrorMessage = Signature.ErrorMessage
                                Else

                                    '7 save Biometrics
                                    If Not Bio.SaveToDbase(DAL, myTrans) Then
                                        requestResponse.ErrorMessage = Bio.ErrorMessage
                                    Else

                                        '8 save Survey
                                        If Not Survey.SaveToDbase(DAL, myTrans) Then
                                            requestResponse.ErrorMessage = Survey.ErrorMessage
                                        Else

                                            '9 save PhotoValidID
                                            If Not PhotoValidID.SaveToDbase(DAL, myTrans) Then
                                                requestResponse.ErrorMessage = PhotoValidID.ErrorMessage
                                            Else
                                                '10 save DCS_Card_Account
                                                If Not DCS_Card_Account.SaveToDbase(DAL, myTrans) Then
                                                    requestResponse.ErrorMessage = DCS_Card_Account.ErrorMessage
                                                Else

                                                    '11 save Card
                                                    If Not Card.SaveToDbase(DAL, myTrans) Then
                                                        requestResponse.ErrorMessage = Card.ErrorMessage
                                                    Else

                                                        'save Instance Issuance
                                                        If Not InstanceIssuance.SaveToDbase(DAL, myTrans) Then
                                                            requestResponse.ErrorMessage = InstanceIssuance.ErrorMessage
                                                        Else

                                                            Dim bln As Boolean = True

                                                            If Not DCS_Card_ReprintList Is Nothing Then
                                                                '12 save DCS_Card_Reprint

                                                                For i As Short = 0 To DCS_Card_ReprintList.DCS_Card_ReprintList.Count - 1
                                                                    If Not DCS_Card_ReprintList.DCS_Card_ReprintList(i) Is Nothing Then
                                                                        If Not DCS_Card_ReprintList.DCS_Card_ReprintList(i).SaveToDbase(DAL, myTrans) Then
                                                                            requestResponse.ErrorMessage = DCS_Card_ReprintList.DCS_Card_ReprintList(i).ErrorMessage
                                                                            bln = False
                                                                            Exit For
                                                                        Else
                                                                            oldCardNo = DCS_Card_ReprintList.DCS_Card_ReprintList(i).OldCardNo
                                                                            newCardNo = DCS_Card_ReprintList.DCS_Card_ReprintList(i).NewCardNo
                                                                            origOldCardNo = oldCardNo
                                                                        End If
                                                                    End If
                                                                Next
                                                            End If

                                                            If bln Then

                                                                If Not MemberEmploymentHistoryList Is Nothing Then
                                                                    '13 save Employment History

                                                                    For i As Short = 0 To MemberEmploymentHistoryList.MemberEmploymentHistoryList.Count - 1
                                                                        If Not MemberEmploymentHistoryList.MemberEmploymentHistoryList(i) Is Nothing Then
                                                                            If Not MemberEmploymentHistoryList.MemberEmploymentHistoryList(i).SaveToDbase(DAL, myTrans) Then
                                                                                requestResponse.ErrorMessage = MemberEmploymentHistoryList.MemberEmploymentHistoryList(i).ErrorMessage
                                                                                bln = False
                                                                                Exit For
                                                                            End If
                                                                        End If
                                                                    Next


                                                                End If
                                                            End If

                                                            IsProcessSuccess = bln
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            ' 041 UBP , 102 AUB
            Dim bankCode As String = "000"
            If My.Settings.BankID = "1" Then bankCode = "041"
            If My.Settings.BankID = "2" Then bankCode = "102"

            If IsProcessPAGIBIGCardInfo Then
                If IsProcessSuccess Then
                    ph3API = New PHASE3.PHASE3
                    Dim IsReversal = False
                    If Not Member.Application_Remarks.Contains("Re-card") Then
                        Dim pushCardInfoResult As PHASE3.PushCardInfoResult = PushCardInfoLocal(requestAuth,
                                                              Member.PagIBIGID, Member.Member_LastName, Member.Member_FirstName, Member.Member_MiddleName,
                                                              Member.BirthDate, (MemberContactinfo.Mobile_AreaCode.Trim & MemberContactinfo.Mobile_CelNo.Trim).Trim,
                                                              DCS_Card_Account.CardNo, DCS_Card_Account.AccountNumber, Card.ExpiryDate, Member.ApplicationDate.Date, bankCode, Member.requesting_branchcode)


                        If pushCardInfoResult.IsSuccess Then
                            For i As Short = 0 To myTrans.Count - 1
                                myTrans(i).Commit()
                            Next
                            requestResponse.IsSuccess = True
                        Else
                            Dim activeCardErrorMessage = ""
                            ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                            If ph3API.ActiveCardInfoResult.IsGet Then
                                Dim currentActiveCard = ph3API.ActiveCardInfoResult.CardNumber
                                If currentActiveCard = DCS_Card_Account.CardNo Then
                                    For i As Short = 0 To myTrans.Count - 1
                                        myTrans(i).Commit()
                                    Next
                                    requestResponse.ErrorMessage = String.Empty
                                    requestResponse.IsSuccess = True
                                    IsReversal = True
                                End If
                            End If

                            If Not IsReversal Then
                                For i As Short = 0 To myTrans.Count - 1
                                    myTrans(i).Rollback()
                                Next
                                requestResponse.ErrorMessage = pushCardInfoResult.GetErrorMsg
                                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                            End If
                        End If
                    Else
                        If oldCardNo = "" Then
                            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & Member.PagIBIGID & " have empty old account no")
                        ElseIf newCardNo = "" Then
                            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & Member.PagIBIGID & " have empty new account no")
                        Else

                            ' Active card info update.
                            Dim activeCardErrorMessage = ""
                            Dim activeCardResult = ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                            oldCardNo = IIf(ph3API.ActiveCardInfoResult.IsGet, ph3API.ActiveCardInfoResult.CardNumber, oldCardNo)

                            Dim updateCardInfoResult As PHASE3.UpdateCardInfoResult = UpdateCardInfoLocal(requestAuth, Member.PagIBIGID, oldCardNo, newCardNo, String.Empty, bankCode)
                            If updateCardInfoResult.IsSuccess Then
                                For i As Short = 0 To myTrans.Count - 1
                                    myTrans(i).Commit()
                                Next
                                requestResponse.IsSuccess = True
                            Else

                                activeCardErrorMessage = ""
                                ph3API.ActiveCardInfo(Member.PagIBIGID, bankCode, activeCardErrorMessage)
                                If ph3API.ActiveCardInfoResult.IsGet Then
                                    Dim currentActiveCard = ph3API.ActiveCardInfoResult.CardNumber
                                    If currentActiveCard = DCS_Card_Account.CardNo Then
                                        For i As Short = 0 To myTrans.Count - 1
                                            myTrans(i).Commit()
                                        Next
                                        requestResponse.ErrorMessage = String.Empty
                                        requestResponse.IsSuccess = True
                                        IsReversal = True
                                    End If
                                End If

                                If Not IsReversal Then
                                    For i As Short = 0 To myTrans.Count - 1
                                        myTrans(i).Rollback()
                                    Next
                                    requestResponse.ErrorMessage = updateCardInfoResult.GetErrorMsg
                                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                                End If
                                'requestResponse.ErrorMessage = updateCardInfoResult.GetErrorMsg
                                'PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                            End If
                        End If
                    End If
                ElseIf IsProcessSuccess = False Then
                    For i As Short = 0 To myTrans.Count - 1
                        myTrans(i).Rollback()
                    Next
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
                End If
            Else
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Commit()
                Next
                requestResponse.IsSuccess = True
            End If

            If IsProcessPackUPData Then
                If My.Settings.BankID = "1" And requestResponse.IsSuccess Then
                    Try
                        Dim reqBranch As String = ""
                        Dim reqBranchAddress As String = ""

                        If DAL.SelectQuery("SELECT ISNULL(HBUR,'') As HBUR, ISNULL(Building,'') As Building, ISNULL(LotNo,'') As LotNo, ISNULL(BlockNo,'') As BlockNo, ISNULL(PhaseNo,'') As PhaseNo, ISNULL(HouseNo,'') As HouseNo, ISNULL(StreetName,'') As StreetName, ISNULL(Subdivision,'') As Subdivision, ISNULL(Barangay,'') As Barangay, ISNULL(CityMinicipality,'') As CityMinicipality, ISNULL(Province,'') As Province, ISNULL(ZipCode,'') As ZipCode, ISNULL(Region,'') As Region, ISNULL(Branch,'') As Branch FROM tbl_branch WHERE requesting_branchcode='" & Member.requesting_branchcode & "'") Then
                            If DAL.TableResult.DefaultView.Count > 0 Then
                                reqBranch = DAL.TableResult.Rows(0)("Branch").ToString
                                reqBranchAddress = DAL.TableResult.Rows(0)("HBUR").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Building").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("LotNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("BlockNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("PhaseNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("HouseNo").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("StreetName").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Subdivision").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Barangay").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("CityMinicipality").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Province").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("Region").ToString & " " &
                                                                                                DAL.TableResult.Rows(0)("ZipCode").ToString
                            End If
                        End If

                        Dim arrData As String = ""
                        Dim permanentAddress As String = Trim(MemberContactinfo.Permanent_HBUR & " " & MemberContactinfo.Permanent_Building & " " & MemberContactinfo.Permanent_LotNo & " " & MemberContactinfo.Permanent_BlockNo & " " & MemberContactinfo.Permanent_PhaseNo & " " & MemberContactinfo.Permanent_HouseNo & " " & MemberContactinfo.Permanent_StreetName & " " & MemberContactinfo.Permanent_Subdivision & " " & MemberContactinfo.Permanent_Barangay & " " & MemberContactinfo.Permanent_CityMunicipality & " " & MemberContactinfo.Permanent_Province & " " & MemberContactinfo.Permanent_Region & " " & MemberContactinfo.Permanent_ZipCode)
                        Dim presentAddress As String = Trim(MemberContactinfo.Present_HBUR & " " & MemberContactinfo.Present_Building & " " & MemberContactinfo.Present_LotNo & " " & MemberContactinfo.Present_BlockNo & " " & MemberContactinfo.Present_PhaseNo & " " & MemberContactinfo.Present_HouseNo & " " & MemberContactinfo.Present_StreetName & " " & MemberContactinfo.Present_Subdivision & " " & MemberContactinfo.Present_Barangay & " " & MemberContactinfo.Present_CityMunicipality & " " & MemberContactinfo.Present_Province & " " & MemberContactinfo.Present_Region & " " & MemberContactinfo.Present_ZipCode)
                        Dim employerAddress As String = Trim(MembershipCategoryInfo.Employer_HBUR & " " & MembershipCategoryInfo.Employer_Building & " " & MembershipCategoryInfo.Employer_LotNo & " " & MembershipCategoryInfo.Employer_BlockNo & " " & MembershipCategoryInfo.Employer_PhaseNo & " " & MembershipCategoryInfo.Employer_HouseNo & " " & MembershipCategoryInfo.Employer_StreetName & " " & MembershipCategoryInfo.Employer_Subdivision & " " & MembershipCategoryInfo.Employer_Barangay & " " & MembershipCategoryInfo.Employer_CityMunicipality & " " & MembershipCategoryInfo.Employer_Province & " " & MembershipCategoryInfo.Employer_Region & " " & MembershipCategoryInfo.Employer_ZipCode)

                        permanentAddress = permanentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                        presentAddress = presentAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")
                        employerAddress = employerAddress.Replace(",", " ").Replace(".", " ").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ")

                        Dim IsErrorFlag As Boolean

                        Dim arrPermanentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(permanentAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrPresentAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(presentAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrEmployerAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(employerAddress), 50, IsErrorFlag).Split(vbNewLine)
                        Dim arrReqBranchAddress() As String = FormatDataWithCharLength(RemoveInvalidCharacters(reqBranchAddress), 50, IsErrorFlag).Split(vbNewLine)

                        Dim arrPermanentAddress1 As String = ""
                        Dim arrPermanentAddress2 As String = ""
                        Dim arrPermanentAddress3 As String = ""
                        Dim arrPresentAddress1 As String = ""
                        Dim arrPresentAddress2 As String = ""
                        Dim arrPresentAddress3 As String = ""
                        Dim arrEmployerAddress1 As String = ""
                        Dim arrEmployerAddress2 As String = ""
                        Dim arrEmployerAddress3 As String = ""
                        Dim arrReqBranchAddress1 As String = ""
                        Dim arrReqBranchAddress2 As String = ""
                        Dim arrReqBranchAddress3 As String = ""

                        For i As Short = 0 To arrPermanentAddress.Length - 1
                            Select Case Trim(arrPermanentAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrPermanentAddress1 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrPermanentAddress2 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrPermanentAddress3 = arrPermanentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrPresentAddress.Length - 1
                            Select Case Trim(arrPresentAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrPresentAddress1 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrPresentAddress2 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrPresentAddress3 = arrPresentAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrEmployerAddress.Length - 1
                            Select Case Trim(arrEmployerAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrEmployerAddress1 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrEmployerAddress2 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrEmployerAddress3 = arrEmployerAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next

                        For i As Short = 0 To arrReqBranchAddress.Length - 1
                            Select Case Trim(arrReqBranchAddress(i))
                                Case "", " ", vbNewLine
                                Case Else
                                    Select Case i
                                        Case 0
                                            arrReqBranchAddress1 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 1
                                            arrReqBranchAddress2 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                        Case 2
                                            arrReqBranchAddress3 = arrReqBranchAddress(i).Replace(vbCrLf, "").Replace(Environment.NewLine, "").Replace(vbLf, "")
                                    End Select
                            End Select
                        Next
                        'added by edel to handle address length

                        ' Handling for MID
                        Dim sssID = Member.SSSID
                        Dim gsisID = Member.GSISID
                        If sssID = "" And gsisID IsNot "" Then
                            sssID = Member.PagIBIGID
                        ElseIf sssID IsNot "" And gsisID = "" Then
                            gsisID = Member.PagIBIGID
                        ElseIf sssID IsNot "" And gsisID IsNot "" Then
                            gsisID = Member.PagIBIGID
                        Else
                            gsisID = Member.PagIBIGID
                        End If
                        ' Handling for recarding.
                        Dim employeeID = MembershipCategoryInfo.EmployeeID
                        Dim _accountNumber = DCS_Card_Account.AccountNumber
                        If Member.Application_Remarks.Contains("Re-card") Then
                            'employeeID = oldCardNo ' Should replace by old card number.

                            'revised on 07/22/2020 to fix memfiles issue of oldCardNo=newCardNo
                            'If oldCardNo <> newCardNo Then
                            '    employeeID = oldCardNo
                            'Else
                            '    employeeID = origOldCardNo
                            'End If

                            employeeID = origOldCardNo

                            _accountNumber = DCS_Card_Account.CardNo
                        End If

                        'added by edel on 06/11/2020
                        Dim firstNameWithSuffix As String = Member.Member_FirstName & " " & Member.Member_Extension

                        arrData =
                                                            CheckFieldLimit(_accountNumber, Constants.ACC_NUMBER) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(Member.Member_LastName), Constants.LAST_NAME) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(firstNameWithSuffix), Constants.FIRST_NAME) & "|" &
                                                            CheckFieldLimit(RemoveInvalidCharacters(Member.Member_MiddleName), Constants.MIDDLE_NAME) & "|" &
                                                             "P|" &
                                                             CheckFieldLimit(CDate(Member.BirthDate).ToString("MM/dd/yyyy"), Constants.DATE_OF_BIRTH) & "|" &
                                                             Member.Gender.Substring(0, 1).ToUpper & "|" &
                                                             CheckFieldLimit(Member.CivilStatus.Substring(0, 1).ToUpper, Constants.MARITAL_STATUS) & "|" &
                                                             CheckFieldLimit((MemberContactinfo.Mobile_AreaCode & MemberContactinfo.Mobile_CelNo).Replace("+", "").Replace(" ", ""), Constants.CELL_PHONE_NUMBER) & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(MemberContactinfo.Home_CountryCode & " " & MemberContactinfo.Home_AreaCode & " " & MemberContactinfo.Home_TelNo).Replace(" ", ""), Constants.HOME_TEL_NUMBER) & "|" &
                                                             CheckFieldLimit(MemberContactinfo.EmailAddress, Constants.EMAIL_ADDRESS) & "|" &
                                                             CheckFieldLimit(Member.TIN.ToUpper, Constants.TIN) & "|" &
                                                             arrPermanentAddress1 & "|" &
                                                             arrPermanentAddress2 & "|" &
                                                             arrPermanentAddress3 & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(reqBranch.Trim), Constants.MAILING_ADDRESS1) & "|" &
                                                             arrReqBranchAddress1 & "|" &
                                                             arrReqBranchAddress2 & "|" &
                                                             arrReqBranchAddress3 & "|" &
                                                             "" & "|" &
                                                             "0NBBISCOCHO" & "|" &
                                                             "PI1" & "|" &
                                                             Now.ToString("MM/dd/yyyy") & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(Member.BirthCity), Constants.BPLACE) & "|" &
                                                             "Filipino" & "|" &
                                                             "H1000" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(sssID, Constants.SSS) & "|" &
                                                             CheckFieldLimit(gsisID, Constants.GSIS) & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(employeeID), Constants.EMPLOYEE_ID) & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             CheckFieldLimit(RemoveInvalidCharacters(MembershipCategoryInfo.EmployerName), Constants.COMP_NAME) & "|" &
                                                             arrEmployerAddress1 & "|" &
                                                             arrEmployerAddress2 & "|" &
                                                             arrEmployerAddress3 & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "" & "|" &
                                                             "|||||||||||"

                        arrData = arrData.Replace("ñ", "n")
                        arrData = arrData.Replace("Ñ", "N")

                        PHASE3.PHASE3.PackUBPData(requestAuth, Trim(arrData), Member.PagIBIGID, DCS_Card_Account.AccountNumber,
                                                                                          Photo.fld_Photo, Signature.fld_Signature, PhotoValidID.fld_PhotoID,
                                                                                          Bio.fld_LeftPrimaryFP_Ansi, Bio.fld_LeftSecondaryFP_Ansi, Bio.fld_RightPrimaryFP_Ansi, Bio.fld_RightSecondaryFP_Ansi,
                                                                                          Bio.fld_LeftPrimaryFP_Wsq, Bio.fld_LeftSecondaryFP_Wsq, Bio.fld_RightPrimaryFP_Wsq, Bio.fld_RightSecondaryFP_Wsq, Member.Application_Remarks.Contains("Re-card"))

                        Dim sessionRefTxn As String = GenerateSessionRefTxn(requestAuth)

                        If DAL.AddUBP(requestAuth.User, requestAuth.KioskID, sessionRefTxn, Member.PagIBIGID, DCS_Card_Account.AccountNumber) Then
                        End If
                    Catch ex As Exception
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = ex.Message
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Packing of ubp data. Runtime error " & ex.Message)
                    End Try
                End If
            End If

            'Check if data exist on db.
            If requestResponse.IsSuccess Then
                'Check if data exist on db.
                Dim isRefNumExist = DAL.ExecuteScalar(String.Format("SELECT ISNULL(RefNum,'') FROM tbl_Member WHERE RefNum='{0}'", Member.RefNum))
                If isRefNumExist Then
                    If DAL.ObjectResult Is Nothing Or DAL.ObjectResult.ToString.Trim = "" Then
                        requestResponse.IsSuccess = False
                        requestResponse.ErrorMessage = methodName + "(): has been failed RefNum is not exist."
                        PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
                    Else
                        requestResponse.IsSuccess = True
                        'myTrans = New List(Of SqlClient.SqlTransaction)
                        '' New feature.
                        'Dim memberUpload As New DCS_Upload
                        'memberUpload.Status = Constants.MEMBER_UPLOAD_COMPLETE
                        'memberUpload.PagIBIGID = Member.PagIBIGID
                        'memberUpload.RefNum = Member.RefNum
                        'memberUpload.IsPushCardInfo = True
                        'memberUpload.PushCardInfoDate = DateTime.Now
                        'memberUpload.IsPackUpData = True
                        'memberUpload.PackUpDataDate = DateTime.Now
                        'memberUpload.Remarks = ""
                        'memberUpload.SaveToDbase(DAL, myTrans)
                        'For i As Short = 0 To myTrans.Count - 1
                        '    myTrans(i).Commit()
                        'Next
                    End If
                Else
                    requestResponse.IsSuccess = False
                    requestResponse.ErrorMessage = methodName + "(): has been failed RefNum is not exist."
                    PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
                End If
            End If

            Return requestResponse
        Catch ex As Exception
            requestResponse.IsSuccess = False
            requestResponse.ErrorMessage = ex.Message
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & requestResponse.ErrorMessage)
            Return requestResponse
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
    End Function

    Private Function RemoveSpecialCharacters(ByVal str As String) As String
        Return str.Replace("Ñ", "N").Replace("ñ", "n").Replace("#", " ").Replace("-", " ").Replace("(", " ").Replace(")", " ").Replace("&", " ").Replace("'", " ").Replace("""", " ").Replace("+", " ").Replace("�", "N")
    End Function

    Private Function RemoveInvalidCharacters(ByVal str As String) As String
        '1. replace enye to N/n 
        '2. allow alphanumeric and space only 
        '3. remove double spaces,
        Return String.Join(" ", System.Text.RegularExpressions.Regex.Replace(str.Replace("Ñ", "N").Replace("ñ", "n").Replace("�", "N"), "[^A-Za-z0-9\ ]", " ").Split({" "c}, StringSplitOptions.RemoveEmptyEntries))

    End Function

    <WebMethod()>
    Public Function GetUserIDNumber(ByVal requestAuth As PHASE3.RequestAuth,
                                                         ByVal userID As Integer) As String
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_AuthFailedMsg)
        End If


        Dim DAL As New DAL
        Try
            'If DAL.SelectQueryCentralized (String.Format("SELECT idnumber FROM tbl_DCS_SystemUser WHERE ID='{0}'", userID))
            If DAL.SelectQuery(String.Format("SELECT idnumber FROM tbl_DCS_SystemUser WHERE ID='{0}'", userID)) Then
                If DAL.TableResult.DefaultView.Count > 0 Then
                    Dim rw As DataRow = DAL.TableResult.Rows(0)
                    Return rw("idnumber")
                End If
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)

        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
        Return String.Empty
    End Function

#Region "Bank And Pagibig Processor"

    <WebMethod()>
    Public Function GetMemberForUploadList(ByVal requestAuth As PHASE3.RequestAuth) As List(Of DCS_Upload)
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name
        Dim DAL As New DAL
        Dim memberUpload As New List(Of DCS_Upload)
        Try
            memberUpload = DCS_Upload.GetForUploadList(DAL)
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
        Finally
            DAL.Dispose()
            DAL = Nothing
        End Try
        Return memberUpload
    End Function

    <WebMethod()>
    Public Function SaveDCSUpload(ByVal requestAuth As PHASE3.RequestAuth, ByVal dcsUpload As DCS_Upload) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim DAL As New DAL

        If Not dcsUpload.SaveToDbase(DAL, myTrans) Then
            requestResponse.ErrorMessage = dcsUpload.ErrorMessage
        Else
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Commit()
            Next
            requestResponse.IsSuccess = True

        End If

        Return requestResponse
    End Function

    <WebMethod()>
    Public Function LogEmployee(ByVal requestAuth As PHASE3.RequestAuth, ByVal employeeNumber As String, ByVal picture As Byte(), ByVal kioskId As String, ByRef response As String) As LogEmployeeResult
        ph3API = New PHASE3.PHASE3
        Dim blnResponse As Boolean = ph3API.LogEmployee(employeeNumber, picture, kioskId, response)
        Return ph3API.LogEmployeeResult
    End Function
#End Region

#Region "Deposit Transaction"
    <WebMethod()>
    Public Function SaveDepositTransaction(ByVal requestAuth As PHASE3.RequestAuth, ByVal deposit As DepositTransaction) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim DAL As New DAL


        deposit.RequestedDate = DateTime.Now
        deposit.BankID = My.Settings.BankID
        deposit.TransactionType = Constants.DEPOSIT_TRANSACTIONTYPE_DEPOSIT

        Dim imageFileName = String.Format("{0}{1}.jpg", deposit.KioskID, deposit.RequestedDate.ToString("MMddyyyyhhmmss"))
        Dim filePath = My.Settings.SharedFolder + "Deposit\Images\" + imageFileName
        deposit.DepositImagePath = filePath

        If Not System.IO.Directory.Exists(My.Settings.SharedFolder + "Deposit\Images\") Then
            requestResponse.ErrorMessage = "Directory is not exist."
            requestResponse.IsSuccess = False
        End If


        If Not deposit.IsExist(DAL, deposit.ReferenceNo) Then
            Try
                Dim ms As New IO.MemoryStream(deposit.DepositImage) 'This is correct...
                Dim pic As Image = Image.FromStream(ms)
                Dim SaveImage As New Bitmap(pic)
                SaveImage.Save(deposit.DepositImagePath, Imaging.ImageFormat.Jpeg)
                SaveImage.Dispose()

                If Not deposit.SaveToDbase(DAL, myTrans) Then
                    requestResponse.ErrorMessage = deposit.ErrorMessage
                Else
                    requestResponse.IsSuccess = True
                End If
            Catch ex As Exception
                requestResponse.ErrorMessage = "Failed to save deposit image." + filePath + ex.Message
                requestResponse.IsSuccess = False
            End Try
        Else
            requestResponse.ErrorMessage = String.Format("Reference No. {0} already exist", deposit.ReferenceNo)
            requestResponse.IsSuccess = False
        End If

        If requestResponse.IsSuccess = True Then
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Commit()
            Next
        Else
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Rollback()
            Next
        End If


        Return requestResponse
    End Function


    <WebMethod()>
    Public Function GetDepositByBranchAndDate(ByVal requestAuth As PHASE3.RequestAuth, ByVal branchCode As String, ByVal transactionDate As DateTime) As Decimal
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name
        Dim DAL As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)

        Return DepositTransaction.GetTotalAmountByBranchAndTransactionDate(branchCode, transactionDate, DAL, myTrans)
    End Function
    <WebMethod()>
    Public Function GetTop10DepositHistory(ByVal requestAuth As PHASE3.RequestAuth, ByVal Status As Integer) As List(Of DepositTransaction)
        Dim deposit As New DepositTransaction
        Dim DAL As New DAL
        Dim result = deposit.GetTop10ByUserKiosk(DAL, requestAuth.KioskID, requestAuth.User, My.Settings.BankID, Status)
        Return result
    End Function

    <WebMethod()>
    Public Function GetDepositBankAccount(ByVal requestAuth As PHASE3.RequestAuth) As List(Of DepositBankAccount)
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name
        Dim DAL As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Return DepositBankAccount.GetList(DAL, myTrans)
    End Function
#End Region

    <WebMethod()>
    Public Function SaveVersion(ByVal requestAuth As PHASE3.RequestAuth,
                               ByVal kioskID As String, ByVal Version As String, ByVal Anydesk As String) As Boolean
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name

        If requestAuth Is Nothing Then
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & PHASE3.PHASE3.WS_RequestAuthIsNothingMsg)
            Return Nothing
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            Return Nothing
        End If

        Dim DAL As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim isSuccess = False
        Try
            If DAL.SaveKioskVersion(kioskID, Version, Anydesk, myTrans) Then
                isSuccess = True
            Else
                PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & DAL.ErrorMessage)
                isSuccess = False
            End If
        Catch ex As Exception
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): Runtime error " & ex.Message)
            isSuccess = False
        Finally

            If isSuccess = True Then
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Commit()
                Next
            Else
                For i As Short = 0 To myTrans.Count - 1
                    myTrans(i).Rollback()
                Next
            End If


            DAL.Dispose()
            DAL = Nothing
        End Try
        Return isSuccess
    End Function


#Region "CashOnHand"
    <WebMethod()>
    Public Function SaveCashOnHand(ByVal requestAuth As PHASE3.RequestAuth, ByVal cashOnHand As CashOnHand) As PHASE3.RequestResponse

        Dim requestResponse As New PHASE3.RequestResponse
        Dim methodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod.Name


        If requestAuth Is Nothing Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_RequestAuthIsNothingMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        ElseIf Not ValidateWebAccess(requestAuth.wsUser, requestAuth.wsPass) Then
            requestResponse.ErrorMessage = PHASE3.PHASE3.WS_AuthFailedMsg
            PHASE3.PHASE3.SaveToErrorLog(requestAuth, methodName & "(): " & requestResponse.ErrorMessage)
            Return requestResponse
        End If

        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        Dim DAL As New DAL

        If cashOnHand.ID = 0 Then
            cashOnHand.RequestDate = DateTime.Now
        End If
        cashOnHand.BankID = My.Settings.BankID
        cashOnHand.RequestedBy = requestAuth.User

        Try

            If Not cashOnHand.RequestDate.Date = DateTime.Now.Date Then
                requestResponse.ErrorMessage = String.Format("Only date today is allowed to modify/cancel.")
                requestResponse.IsSuccess = False
            Else
                If cashOnHand.IsExistByBranchDate(DAL, cashOnHand.RequestDate, cashOnHand.BranchCode, cashOnHand.ID) Then
                    requestResponse.ErrorMessage = String.Format("BranchCode:{1} Date:{0} has already existing cash on hand details", cashOnHand.RequestDate.ToString("yyyy-MM-dd"), cashOnHand.BranchCode)
                    requestResponse.IsSuccess = False
                Else
                    If Not cashOnHand.SaveToDbase(DAL, myTrans) Then
                        requestResponse.ErrorMessage = cashOnHand.ErrorMessage
                    Else
                        requestResponse.IsSuccess = True
                    End If
                End If
            End If
        Catch ex As Exception
            requestResponse.ErrorMessage = "Failed to save cash on hand." + ex.Message
            requestResponse.IsSuccess = False
        End Try

        If requestResponse.IsSuccess = True Then
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Commit()
            Next
        Else
            For i As Short = 0 To myTrans.Count - 1
                myTrans(i).Rollback()
            Next
        End If

        Return requestResponse
    End Function
    <WebMethod()>
    Public Function GetTop10CashOnHandHistory(ByVal requestAuth As PHASE3.RequestAuth) As List(Of CashOnHand)
        Dim cashOnHand As New CashOnHand
        Dim DAL As New DAL
        Dim result = cashOnHand.GetTop10ByUserKiosk(DAL, requestAuth.KioskID, requestAuth.User, My.Settings.BankID)
        Return result
    End Function

    <WebMethod()>
    Public Function GetUserRoleByUsernamePassowrd(ByVal requestAuth As PHASE3.RequestAuth, ByVal username As String, ByVal password As String) As Integer

        Dim DAL As New DAL
        Dim dt As DataTable = Nothing
        Dim result = 0
        If DAL.GetUserRoleByUsernamePassword(username, password) Then
            dt = DAL.TableResult
            If dt.Rows.Count > 0 Then
                Dim value = dt.Rows(0)
                result = value(0)
            End If
        End If
        Return result
    End Function

#End Region
End Class
