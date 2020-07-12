Imports System.Reflection
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports ACC_MS_WEBSERVICE.MiddleServer
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
'<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://allcardtech.com.ph/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ACC_MS_WEBSERVICE
    Inherits System.Web.Services.WebService
    Private DA As New ODBCDataAccess

    Enum SelectServer
        acc_pag_ibig_ms
        ACC_OP_SCHED
        Phase1_Pagibig
    End Enum

#Region "Private Declaration"
    Private PAGIBIGWS As New MiddleServer.PagIbig_WSSoapClient
    'Private MemberInfo As New MiddleServer.SearchResult
    'Private MemberEMPClass As New MiddleServer.MultipleRecord

    Private cn As New ConnectionString

#End Region
    <WebMethod()> _
    Public Function is_MID_RTN_Exist(ByVal MID_RTN As String,
                                     ByVal webuser As String,
                                     ByVal webpass As String) As SubmitResult
        

        is_MID_RTN_Exist = New SubmitResult
        is_MID_RTN_Exist.IsSent = True
        If (webuser.Trim <> My.Settings.ACC_WEB_Uname Or webpass.Trim <> My.Settings.ACC_WEB_Pass) Then
            is_MID_RTN_Exist.ErrorMessage = "Authentication Failed for Webservice Function"
            Return is_MID_RTN_Exist
        End If
        Try

            '--To check if MID or RTN exist in MiddleServer
            Dim dt As Boolean
            dt = Is_Mid_Rtn_Exist_In_MS(MID_RTN)
            If dt = False Then
                is_MID_RTN_Exist.SearchResult = PAGIBIGWS.is_MID_RTN_Exist(MID_RTN, webuser, webpass).SearchResult
                If Not IsNothing(is_MID_RTN_Exist.ErrorMessage) Then
                    is_MID_RTN_Exist.ErrorMessage = is_MID_RTN_Exist.SearchResult.GetErrorMsg
                End If
            Else
                is_MID_RTN_Exist.AlreadyExist = True
            End If
        Catch ex As Exception
            is_MID_RTN_Exist.ErrorMessage = ex.Message
        End Try
        Return is_MID_RTN_Exist
    End Function

    '<WebMethod()> _
    'Public Function is_MID_RTN_Exist_NoValidation(ByVal MID_RTN As String,
    '                                 ByVal webuser As String,
    '                                 ByVal webpass As String) As SubmitResult


    '    is_MID_RTN_Exist_NoValidation = New SubmitResult
    '    is_MID_RTN_Exist_NoValidation.IsSent = True
    '    If (webuser.Trim <> My.Settings.ACC_WEB_Uname Or webpass.Trim <> My.Settings.ACC_WEB_Pass) Then
    '        is_MID_RTN_Exist_NoValidation.ErrorMessage = "Authentication Failed for Webservice Function"
    '        Return is_MID_RTN_Exist_NoValidation
    '    End If
    '    Try
    '        is_MID_RTN_Exist_NoValidation.SearchResult = PAGIBIGWS.is_MID_RTN_Exist(MID_RTN, webuser, webpass).SearchResult
    '        If Not IsNothing(is_MID_RTN_Exist_NoValidation.ErrorMessage) Then
    '            is_MID_RTN_Exist_NoValidation.ErrorMessage = is_MID_RTN_Exist_NoValidation.SearchResult.GetErrorMsg
    '        End If
    '    Catch ex As Exception
    '        is_MID_RTN_Exist_NoValidation.ErrorMessage = ex.Message
    '    End Try
    '    Return is_MID_RTN_Exist_NoValidation
    'End Function

    <WebMethod()> _
    Public Function ConvertMID_To_RTN(ByVal RTN As String,
                                     ByVal webuser As String,
                                     ByVal webpass As String) As Result_GetMemberID
        If (webuser.Trim = My.Settings.ACC_WEB_Uname Or webpass.Trim = My.Settings.ACC_WEB_Pass) Then
            ConvertMID_To_RTN = PAGIBIGWS.updateRTN(RTN)
            Return ConvertMID_To_RTN
        End If
    End Function

    <WebMethod()> _
    Public Sub UpdateDCSVersion(ByVal KioskID As String, ByVal DCSVersion As String)
        Dim SQL As String

        DA.ConnectionString = SQLServerConnectionScheduling()

        If KioskID <> "" Then
            SQL = "Update tbl_kiosk set version = @version where KioskID ='" & KioskID & "'"

            Dim cmd As New SqlCommand(SQL)

            cmd.Parameters.Add("@version", SqlDbType.VarChar).Value = DCSVersion
            DA.RunSQLNonQuery(cmd)
        End If
        
    End Sub

    <WebMethod()> _
    Public Function DeactivateSchedule(ByVal IDNumber As String, ByVal SchedID As Integer) As Boolean
        Dim SQL As String

        DA.ConnectionString = SQLServerConnectionScheduling()

        If DA.RecordFound("Select * from Operator_Schedule where IDNumber = '" & IDNumber & "'") = True Then
            SQL = "Update ScheduleTab set IsSchedDone = @IsSchedDone where ID = @ID"

            Dim cmd As New SqlCommand(SQL)

            cmd.Parameters.AddWithValue("@IsSchedDone", 1)
            cmd.Parameters.AddWithValue("@ID", SchedID)

            If DA.RunSQLNonQuery(cmd) = False Then
                Return False
            End If
        Else
            Return False
        End If
        Return True
    End Function

    'Checking by Member Fullname and Birthdate
    <WebMethod()> _
    Public Function is_MemberFullName_Exist(ByVal LName As String,
                                            ByVal MName As String,
                                            ByVal FName As String,
                                            ByVal DOB As Date) As ACCMultipleRecord


        Dim mclist As New List(Of MultipleMember)
        
        Dim pwsmcres As New MiddleServer.PrimaryMultiple
        Dim CheckBool As Boolean
        CheckBool = CheckFullMember(LName, FName, DOB)

        is_MemberFullName_Exist = New ACCMultipleRecord
        pwsmcres = PAGIBIGWS.is_MemberFullName_Exist(Trim(LName),
                                                           Trim(MName),
                                                           Trim(FName),
                                                           DOB)

        If CheckBool = False Then

            
            Try
                For i As Integer = 0 To pwsmcres.ACCMCMultipleRec.Count - 1
                    Dim mc As New MultipleMember

                    If Not IsNothing(pwsmcres.ACCMCMultipleRec(i)) Then
                        mc.RecRTN = pwsmcres.ACCMCMultipleRec(i).RecRTN
                        mc.RecMID = pwsmcres.ACCMCMultipleRec(i).RecMID
                        mc.RecFirstName = pwsmcres.ACCMCMultipleRec(i).RecFirstName
                        mc.RecMiddleName = pwsmcres.ACCMCMultipleRec(i).RecMiddleName
                        mc.RecLastName = pwsmcres.ACCMCMultipleRec(i).RecLastName
                        mc.RecDateOfBirth = pwsmcres.ACCMCMultipleRec(i).RecDateOfBirth
                        mc.RecStatus = pwsmcres.ACCMCMultipleRec(i).RecStatus
                        mclist.Add(mc)
                    End If
                Next
            Catch ex As Exception
                is_MemberFullName_Exist.ErrorMessage = "Error : No Records Found"
            End Try
            is_MemberFullName_Exist.ACCMCMultipleRec = New List(Of MultipleMember)
            is_MemberFullName_Exist.ACCMCMultipleRec = mclist
        Else
            is_MemberFullName_Exist.AlreadyExist = True
        End If
        
        Return is_MemberFullName_Exist
    End Function

    Private Function CheckFullMember(ByVal LastName As String, ByVal FirstName As String, ByVal BirthDate As Date) As Boolean
        DA.ConnectionString = SQLServerConnectionServer()

        If DA.RecordFound("Select Member_LastName,Member_FirstName,BirthDate from tbl_member where Member_lastName = '" & LastName & "' and Member_FirstName = '" & FirstName & "' and BirthDate = '" & Format(BirthDate, "yyyy-MM-dd") & "'") = False Then
            CheckFullMember = False
            Dim DAPHI As New ODBCDataAccess
            DAPHI.ConnectionString = SQLServerConnectionServerPhaseI()
            If DAPHI.RecordFound("Select Member_LastName,Member_FirstName,BirthDate from tbl_member where Member_lastName = '" & LastName & "' and Member_FirstName = '" & FirstName & "' and BirthDate = '" & Format(BirthDate, "yyyy-MM-dd") & "'") = False Then
                CheckFullMember = False
            Else
                CheckFullMember = True
            End If
        Else
            CheckFullMember = True
        End If
        Return CheckFullMember
    End Function

    'Checking if Member is Active or Inactive ''Return Member Contribution
    <WebMethod()> _
    Public Function Is_Member_Active(ByVal UID As String, ByVal LN As String, ByVal FN As String, ByVal MN As String) As ACCMCRecordClassResult
    
        Dim pwsmcres As New MiddleServer.ACCMCRecordClassResult
        Dim mclist As New List(Of MemberContribution)
        Is_Member_Active = New ACCMCRecordClassResult
        Try
            pwsmcres = PAGIBIGWS.Is_Member_Active(UID, LN, FN, MN)
            For i As Integer = 0 To pwsmcres.ACCMCRecordClass.Count - 1
                Dim mc As New MemberContribution
                If Not IsNothing(pwsmcres.ACCMCRecordClass(i)) Then
                    mc.IngresID = pwsmcres.ACCMCRecordClass(i).MCIngresID
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
                    mclist.Add(mc)
                End If
            Next
        Catch ex As Exception
            Is_Member_Active.ErrorMessage = "Error : No Records Found"
        End Try
        Is_Member_Active.ACCMCRecordClass = New List(Of MemberContribution)
        Is_Member_Active.ACCMCRecordClass = mclist
        Return Is_Member_Active
    End Function


    Function Is_Mid_Rtn_Exist_In_MS(ByVal MIDRTN As String) As Boolean
        DA.ConnectionString = SQLServerConnectionServer()
        If DA.RecordFound("Select * from tbl_member where MIDRTN = '" & MIDRTN & "'") = False Then
            Is_Mid_Rtn_Exist_In_MS = False
            Dim DAPhaseI As New ODBCDataAccess
            DAPhaseI.ConnectionString = SQLServerConnectionServerPhaseI()
            If DAPhaseI.RecordFound("Select * from tbl_member where MIDRTN = '" & MIDRTN & "'") = False Then
                Is_Mid_Rtn_Exist_In_MS = False
            Else
                Is_Mid_Rtn_Exist_In_MS = True
            End If
        Else
            Is_Mid_Rtn_Exist_In_MS = True
        End If
        Return Is_Mid_Rtn_Exist_In_MS
    End Function

    Public Function SQLServerConnectionServer() As String
        SQLServerConnectionServer = "Server=" & My.Settings.Server.ToString & ";Database =" & My.Settings.Database.ToString & ";User Id=" & My.Settings.ServerUser.ToString & ";Password=" & My.Settings.ServerPassword.ToString
    End Function

    Public Function SQLServerConnectionScheduling() As String
        SQLServerConnectionScheduling = "Server=" & My.Settings.ServerSched & ";Database =" & My.Settings.DatabaseSched & ";User Id=" & My.Settings.ServerUserSched & ";Password=" & My.Settings.ServerPasswordSched
    End Function

    Public Function SQLServerConnectionServerPhaseI() As String
        SQLServerConnectionServerPhaseI = "Server=" & My.Settings.Server.ToString & ";Database =" & "Phase1_Pagibig" & ";User Id=" & My.Settings.ServerUser.ToString & ";Password=" & My.Settings.ServerPassword.ToString
    End Function

    
    <WebMethod()>
    Public Function PagIBIGLogIN(ByVal UID As String, ByVal PWD As String, ByVal ApprovedID As String, ByVal ApprovedPW As String, ByVal MID As String) As MiddleServer.Result_AuthenticateMCIFApprover
        PagIBIGLogIN = New MiddleServer.Result_AuthenticateMCIFApprover
        PagIBIGLogIN = PAGIBIGWS.PagIBIGLogIN(ApprovedID, ApprovedPW, MID)
        Try
            If PagIBIGLogIN.ErrorCode = "00001" Then
                PagIBIGLogIN.ErrorMessage = "Invalid User ID or Password"
                Return PagIBIGLogIN
            ElseIf PagIBIGLogIN.ErrorCode = "11001" Then
                PagIBIGLogIN.ErrorMessage = "Database error"
                Return PagIBIGLogIN
            ElseIf PagIBIGLogIN.ErrorCode = "12001" Then
                PagIBIGLogIN.ErrorMessage = "Contains null in approver account"
                Return PagIBIGLogIN
            ElseIf PagIBIGLogIN.ErrorCode = "12002" Then
                PagIBIGLogIN.ErrorMessage = "Invalid approver ID"
                Return PagIBIGLogIN
            ElseIf PagIBIGLogIN.ErrorCode = "12003" Then
                PagIBIGLogIN.ErrorMessage = "Incorect approver password"
                Return PagIBIGLogIN
            End If
            If PagIBIGLogIN.IsSuccessful = True Then
                Return PagIBIGLogIN
            Else
                PagIBIGLogIN.ErrorMessage = "Log in not successful"
                Return PagIBIGLogIN
            End If
        Catch ex As Exception

        End Try
    End Function

    <WebMethod()>
    Public Function IsMember_Exist_In_PrimaryDB(ByVal MIDRTN As String)
        IsMember_Exist_In_PrimaryDB = Is_Mid_Rtn_Exist_In_MS(MIDRTN)
    End Function

    <WebMethod()>
    Public Function GetServerDateTime() As DateTime
        DA.ConnectionString = SQLServerConnectionServer()
        GetServerDateTime = DA.ExecuteSQLQueryDataTable("Select GetDate()", "TBDateTime").Rows(0)(0)
    End Function

#Region "Get Signature"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetTableSignature(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetTableSignature = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableSignature = DA.ExecuteSQLQueryDataTable("Select * from tbl_Signature where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Signature where MIDRTN = '" & MIDRTN & "')", "tbl_Signature")
            If GetTableSignature.Rows.Count = 0 Then
                GetTableSignature = GetTableSignaturePhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetTableSignature = Nothing
        End If
        Return GetTableSignature
    End Function

    Private Function GetTableSignaturePhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetTableSignaturePhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableSignaturePhaseI = DA.ExecuteSQLQueryDataTable("Select [ID],[RefNum],[MIDRTN],[fld_Signiture] as [fld_Signature],[EntryDate],[LastUpdate] from tbl_Signature where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Signature where MIDRTN = '" & MIDRTN & "')", "tbl_Signature")
        Else
            GetTableSignaturePhaseI = Nothing
        End If
        Return GetTableSignaturePhaseI
    End Function
#End Region
    
#Region "Get Survey"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetTableSurvey(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetTableSurvey = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableSurvey = DA.ExecuteSQLQueryDataTable("Select * from tbl_Survey where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Survey where MIDRTN = '" & MIDRTN & "')", "tbl_Survey")
            If GetTableSurvey.Rows.Count = 0 Then
                GetTableSurvey = GetTableSurveyPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetTableSurvey = Nothing
        End If
        Return GetTableSurvey
    End Function

    Private Function GetTableSurveyPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetTableSurveyPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableSurveyPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_Survey where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Survey where MIDRTN = '" & MIDRTN & "')", "tbl_Survey")
        Else
            GetTableSurveyPhaseI = Nothing
        End If
        Return GetTableSurveyPhaseI
    End Function
#End Region

#Region "Get Employement History"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetEmploymentHistory(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetEmploymentHistory = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetEmploymentHistory = DA.ExecuteSQLQueryDataTable("Select * from tbl_employmenthistory where MIDRTN = '" & MIDRTN & "' and LastUpdate = (Select MAX(LastUpdate) from tbl_employmenthistory where MIDRTN = '" & MIDRTN & "')", "tbl_employmenthistory")
            If GetEmploymentHistory.Rows.Count = 0 Then
                GetEmploymentHistory = GetEmploymentHistoryPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetEmploymentHistory = Nothing
        End If
        Return GetEmploymentHistory
    End Function

    Private Function GetEmploymentHistoryPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetEmploymentHistoryPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetEmploymentHistoryPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_employmenthistory where MIDRTN = '" & MIDRTN & "' and LastUpdate = (Select MAX(LastUpdate) from tbl_employmenthistory where MIDRTN = '" & MIDRTN & "')", "tbl_employmenthistory")
        Else
            GetEmploymentHistoryPhaseI = Nothing
        End If
        Return GetEmploymentHistoryPhaseI
    End Function
#End Region
    
#Region "Get tbl_member"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetTableMember(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetTableMember = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableMember = DA.ExecuteSQLQueryDataTable("Select * from tbl_member where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_member where MIDRTN = '" & MIDRTN & "')", "tbl_member")
            If GetTableMember.Rows.Count = 0 Then
                GetTableMember = GetTableMemberPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetTableMember = Nothing
        End If
        Return GetTableMember
    End Function


    Private Function GetTableMemberPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetTableMemberPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetTableMemberPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_member where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_member where MIDRTN = '" & MIDRTN & "')", "tbl_member")
        Else
            GetTableMemberPhaseI = Nothing
        End If
        Return GetTableMemberPhaseI
    End Function
#End Region

#Region "Get tbl_membercontactinfo"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetMembercontactinfo(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetMembercontactinfo = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMembercontactinfo = DA.ExecuteSQLQueryDataTable("Select * from tbl_membercontactinfo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_membercontactinfo where MIDRTN = '" & MIDRTN & "')", "tbl_membercontactinfo")
            If GetMembercontactinfo.Rows.Count = 0 Then
                GetMembercontactinfo = GetMembercontactinfoPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetMembercontactinfo = Nothing
        End If
        Return GetMembercontactinfo
    End Function

    Private Function GetMembercontactinfoPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetMembercontactinfoPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMembercontactinfoPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_membercontactinfo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_membercontactinfo where MIDRTN = '" & MIDRTN & "')", "tbl_membercontactinfo")
        Else
            GetMembercontactinfoPhaseI = Nothing
        End If
        Return GetMembercontactinfoPhaseI
    End Function
#End Region

#Region "Get PreferredMailing"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetMemberPreferredMailing(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetMemberPreferredMailing = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMemberPreferredMailing = DA.ExecuteSQLQueryDataTable("Select * from tbl_PreferredMailingAddress where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_PreferredMailingAddress where MIDRTN = '" & MIDRTN & "')", "tbl_PreferredMailingAddress")
            If GetMemberPreferredMailing.Rows.Count = 0 Then
                GetMemberPreferredMailing = GetMemberPreferredMailingPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetMemberPreferredMailing = Nothing
        End If
        Return GetMemberPreferredMailing
    End Function

    Public Function GetMemberPreferredMailingPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetMemberPreferredMailingPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMemberPreferredMailingPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_PreferredMailingAddress where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_PreferredMailingAddress where MIDRTN = '" & MIDRTN & "')", "tbl_PreferredMailingAddress")
        Else
            GetMemberPreferredMailingPhaseI = Nothing
        End If
        Return GetMemberPreferredMailingPhaseI
    End Function
#End Region

#Region "Get tbl_membershipcategoryinfo"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetMembershipcategoryinfo(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetMembershipcategoryinfo = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMembershipcategoryinfo = DA.ExecuteSQLQueryDataTable("Select * from tbl_membershipcategoryinfo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_membershipcategoryinfo where MIDRTN = '" & MIDRTN & "')", "tbl_membershipcategoryinfo")
            If GetMembershipcategoryinfo.Rows.Count = 0 Then
                GetMembershipcategoryinfo = GetMembershipcategoryinfoPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetMembershipcategoryinfo = Nothing
        End If
        Return GetMembershipcategoryinfo
    End Function

    Private Function GetMembershipcategoryinfoPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetMembershipcategoryinfoPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMembershipcategoryinfoPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_membershipcategoryinfo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_membershipcategoryinfo where MIDRTN = '" & MIDRTN & "')", "tbl_membershipcategoryinfo")
        Else
            GetMembershipcategoryinfoPhaseI = Nothing
        End If
        Return GetMembershipcategoryinfoPhaseI
    End Function

#End Region

#Region "Get Memcontribution"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetMemcontribution(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetMemcontribution = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMemcontribution = DA.ExecuteSQLQueryDataTable("Select * from tbl_memcontribution where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_memcontribution where MIDRTN = '" & MIDRTN & "')", "tbl_memcontribution")
            If GetMemcontribution.Rows.Count = 0 Then
                GetMemcontribution = GetMemcontributionPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetMemcontribution = Nothing
        End If
        Return GetMemcontribution
    End Function

    Private Function GetMemcontributionPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetMemcontributionPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetMemcontributionPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_memcontribution where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_memcontribution where MIDRTN = '" & MIDRTN & "')", "tbl_memcontribution")
        Else
            GetMemcontributionPhaseI = Nothing
        End If
        Return GetMemcontributionPhaseI
    End Function
#End Region

#Region "Get Photo"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetPhoto(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetPhoto = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetPhoto = DA.ExecuteSQLQueryDataTable("Select * from tbl_Photo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Photo where MIDRTN = '" & MIDRTN & "')", "tbl_Photo")
            If GetPhoto.Rows.Count = 0 Then
                GetPhoto = GetPhotoPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetPhoto = Nothing
        End If
        Return GetPhoto
    End Function

    Private Function GetPhotoPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetPhotoPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetPhotoPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_Photo where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_Photo where MIDRTN = '" & MIDRTN & "')", "tbl_Photo")
            GetPhotoPhaseI = DA.ExecuteSQLQueryDataTable("Select * from tbl_Photo where MIDRTN = '" & MIDRTN & "'", "tbl_Photo")
        Else
            GetPhotoPhaseI = Nothing
        End If
        Return GetPhotoPhaseI
    End Function
#End Region

#Region "Get BioLI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLI.Rows.Count = 0 Then
                GetBioLI = GetBioLIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLI = Nothing
        End If
        Return GetBioLI
    End Function

    Private Function GetBioLIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLIPhaseI = Nothing
        End If
        Return GetBioLIPhaseI
    End Function

#End Region

#Region "Get BioLT"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLT(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLT = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLT = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftSecondaryFP_template],[fld_LeftSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLT.Rows.Count = 0 Then
                GetBioLT = GetBioLTPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLT = Nothing
        End If
        Return GetBioLT
    End Function

    Private Function GetBioLTPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLTPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLTPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftSecondaryFP_template],[fld_LeftSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLTPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftSecondaryFP_template],[fld_LeftSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLTPhaseI = Nothing
        End If
        Return GetBioLTPhaseI
    End Function

#End Region

#Region "Get BioRI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightPrimaryFP_template],[fld_RightPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRI.Rows.Count = 0 Then
                GetBioRI = GetBioRIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRI = Nothing
        End If
        Return GetBioRI
    End Function

    Public Function GetBioRIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioRIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightPrimaryFP_template],[fld_RightPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightPrimaryFP_template],[fld_RightPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRIPhaseI = Nothing
        End If
        Return GetBioRIPhaseI
    End Function
#End Region
    
#Region "Get BioRT"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRT(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRT = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRT = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightSecondaryFP_template],[fld_RightSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRT.Rows.Count = 0 Then
                GetBioRT = GetBioRTPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRT = Nothing
        End If
        Return GetBioRT
    End Function

    Private Function GetBioRTPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRTPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioRTPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightSecondaryFP_template],[fld_RightSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRTPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_RightSecondaryFP_template],[fld_RightSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRTPhaseI = Nothing
        End If
        Return GetBioRTPhaseI
    End Function
#End Region

#Region "Get BioLI WSQ"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLIWSQ(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLIWSQ = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLIWSQ = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLIWSQ.Rows.Count = 0 Then
                GetBioLIWSQ = GetBioLIWSQPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLIWSQ = Nothing
        End If
        Return GetBioLIWSQ
    End Function

    Private Function GetBioLIWSQPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLIWSQPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLIWSQPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLIWSQPhaseI = Nothing
        End If
        Return GetBioLIWSQPhaseI
    End Function

#End Region

#Region "Get BioLT WSQ"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLTWSQ(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLTWSQ = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLTWSQ = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftSecondaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLTWSQ.Rows.Count = 0 Then
                GetBioLTWSQ = GetBioLTWSQPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLTWSQ = Nothing
        End If
        Return GetBioLTWSQ
    End Function

    Private Function GetBioLTWSQPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLTWSQPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLTWSQPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftSecondaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLTWSQPhaseI = Nothing
        End If
        Return GetBioLTWSQPhaseI
    End Function

#End Region

#Region "Get BioRI WSQ"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRIWSQ(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRIWSQ = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRIWSQ = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightPrimaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRIWSQ.Rows.Count = 0 Then
                GetBioRIWSQ = GetBioRIWSQPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRIWSQ = Nothing
        End If
        Return GetBioRIWSQ
    End Function

    Private Function GetBioRIWSQPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRIWSQPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRIWSQPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightPrimaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRIWSQPhaseI = Nothing
        End If
        Return GetBioRIWSQPhaseI
    End Function

#End Region

#Region "Get BioRT WSQ"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRTWSQ(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRTWSQ = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRTWSQ = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightSecondaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRTWSQ.Rows.Count = 0 Then
                GetBioRTWSQ = GetBioRTWSQPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRTWSQ = Nothing
        End If
        Return GetBioRTWSQ
    End Function

    Private Function GetBioRTWSQPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRTWSQPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRTWSQPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightSecondaryFP_Wsq] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRTWSQPhaseI = Nothing
        End If
        Return GetBioRTWSQPhaseI
    End Function

#End Region

#Region "Get BioLI ANSI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLIANSI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLIANSI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLIANSI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLIANSI.Rows.Count = 0 Then
                GetBioLIANSI = GetBioLIANSIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLIANSI = Nothing
        End If
        Return GetBioLIANSI
    End Function

    Private Function GetBioLIANSIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLIANSIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLIANSIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLIANSIPhaseI = Nothing
        End If
        Return GetBioLIANSIPhaseI
    End Function

#End Region

#Region "Get BioLT ANSI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioLTANSI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioLTANSI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioLTANSI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioLTANSI.Rows.Count = 0 Then
                GetBioLTANSI = GetBioLTANSIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioLTANSI = Nothing
        End If
        Return GetBioLTANSI
    End Function

    Private Function GetBioLTANSIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioLTANSIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioLTANSIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioLTANSIPhaseI = Nothing
        End If
        Return GetBioLTANSIPhaseI
    End Function

#End Region

#Region "Get BioRI ANSI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRIANSI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRIANSI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRIANSI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRIANSI.Rows.Count = 0 Then
                GetBioRIANSI = GetBioRIANSIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRIANSI = Nothing
        End If
        Return GetBioRIANSI
    End Function

    Private Function GetBioRIANSIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRIANSIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRIANSIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRIANSIPhaseI = Nothing
        End If
        Return GetBioRIANSIPhaseI
    End Function

#End Region

#Region "Get BioRT ANSI"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetBioRTANSI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetBioRTANSI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetBioRTANSI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            If GetBioRTANSI.Rows.Count = 0 Then
                GetBioRTANSI = GetBioRTANSIPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetBioRTANSI = Nothing
        End If
        Return GetBioRTANSI
    End Function

    Private Function GetBioRTANSIPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetBioRTANSIPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            'GetBioLIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "' and ID = (Select MAX(ID) from tbl_bio where MIDRTN = '" & MIDRTN & "')", "tbl_bio")
            GetBioRTANSIPhaseI = DA.ExecuteSQLQueryDataTable("Select [RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_RightSecondaryFP_Ansi] from tbl_bio where MIDRTN = '" & MIDRTN & "'", "tbl_bio")
        Else
            GetBioRTANSIPhaseI = Nothing
        End If
        Return GetBioRTANSIPhaseI
    End Function

#End Region

#Region "GetCardCounter"
    'updated 4/29/2015
    <WebMethod()>
    Public Function GetCardCounter(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServer()
        GetCardCounter = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetCardCounter = DA.ExecuteSQLQueryDataTable("Select count(MIDRTN) from tbl_member where MIDRTN  = '" & MIDRTN & "'", "tbl_member")
            If GetCardCounter.Rows(0)(0) = 0 Then
                GetCardCounter = GetCardCounterPhaseI(My.Settings.ACC_WEB_Uname, My.Settings.ACC_WEB_Pass, MIDRTN)
            End If
        Else
            GetCardCounter = Nothing
        End If
        Return GetCardCounter
    End Function


    Private Function GetCardCounterPhaseI(ByVal User As String, Pass As String, ByVal MIDRTN As String) As DataTable
        DA.ConnectionString = SQLServerConnectionServerPhaseI()
        GetCardCounterPhaseI = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetCardCounterPhaseI = DA.ExecuteSQLQueryDataTable("Select count(MIDRTN) from tbl_member where MIDRTN  = '" & MIDRTN & "'", "tbl_member")
        Else
            GetCardCounterPhaseI = Nothing
        End If
        Return GetCardCounterPhaseI
    End Function
#End Region
    
    <WebMethod()>
    Public Function GetSched(ByVal User As String, Pass As String, ByVal UserName As String) As DataTable
        DA.ConnectionString = SQLServerConnectionScheduling()
        GetSched = New DataTable()

        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetSched = DA.ExecuteSQLQueryDataTable("Select * from Operator_Schedule where [username] = '" & UserName & "' and DateTo = '" & Format(Now, "yyyy-MM-dd") & "'", "Operator_Schedule")
        Else
            GetSched = Nothing
        End If
        Return GetSched
    End Function

    <WebMethod()>
    Public Function GetSystemUser(ByVal User As String, ByVal Pass As String) As DataTable
        Dim DTSystemUser As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetSystemUser = DA.ExecuteSQLQueryDataTable("Select * from tbl_systemuser", "tbl_systemuser")
        Else
            GetSystemUser = Nothing
        End If
    End Function

    <WebMethod()>
    Public Function GetPagIBIGBranch(ByVal User As String, ByVal Pass As String) As DataTable
        Dim DTSystemUser As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetPagIBIGBranch = DA.ExecuteSQLQueryDataTable("Select * from tbl_branch", "tbl_branch")
        Else
            GetPagIBIGBranch = Nothing
        End If
    End Function

    <WebMethod()>
    Public Function GetEmployerData(ByVal User As String, ByVal Pass As String) As DataTable
        Dim DTSystemUser As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetEmployerData = DA.ExecuteSQLQueryDataTable("Select * from tbl_Employer", "tbl_Employer")
        Else
            GetEmployerData = Nothing
        End If
    End Function

    <WebMethod()>
    Public Function GetEmployerBranch(ByVal User As String, ByVal Pass As String) As DataTable
        Dim DTSystemUser As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetEmployerBranch = DA.ExecuteSQLQueryDataTable("Select * from tbl_Employer_Branch", "tbl_Employer_Branch")
        Else
            GetEmployerBranch = Nothing
        End If
    End Function


    <WebMethod()>
    Public Function GetKiosk(ByVal User As String, ByVal Pass As String) As DataTable
        Dim DTSystemUser As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()
        If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
            GetKiosk = DA.ExecuteSQLQueryDataTable("Select * from tbl_Kiosk", "tbl_Kiosk")
        Else
            GetKiosk = Nothing
        End If
    End Function

    <WebMethod()>
    Public Function DataConfirmation(ByVal RefNum As String, ByVal isRTN As Boolean) As Boolean
        DA.ConnectionString = SQLServerConnectionServer()
        If DA.RecordFound("Select RefNum from tbl_member where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_membercontactinfo where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_membershipcategoryinfo where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        'If DA.RecordFound("Select RefNum from tbl_memcontribution where RefNum = '" & RefNum & "'") = False Then
        '    Return False
        'Else
        '    DataConfirmation = True
        'End If

        If DA.RecordFound("Select RefNum from tbl_Photo where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_Signature where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_Bio where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_Survey where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If DA.RecordFound("Select RefNum from tbl_PreferredMailingAddress where RefNum = '" & RefNum & "'") = False Then
            Return False
        Else
            DataConfirmation = True
        End If

        If isRTN = False Then
            If DA.RecordFound("Select RefNum from tbl_Card where RefNum = '" & RefNum & "'") = False Then
                Return False
            Else
                DataConfirmation = True
            End If
        Else
            DataConfirmation = True
        End If

        Return DataConfirmation
    End Function


    <WebMethod()>
    Public Function InsertData(ByVal MemberDT As DataTable, ByVal MembercontactinfoDT As DataTable, ByVal MembershipCategoryInfoDT As DataTable, ByVal MemberContributionDT As DataTable, ByVal MemberSurveyDT As DataTable, ByVal EmploymentHisDT As DataTable, ByVal CardDT As DataTable, ByVal PhotoDT As DataTable, ByVal SignatureDT As DataTable, ByVal BioDT As DataTable, ByVal PMailingDT As DataTable, ByVal MIDDT As DataTable, ByVal RTNDT As DataTable, ByVal isRTN As Boolean, ByVal User As String, ByVal Pass As String) As String
        Dim SQL As String
        Dim NewMemberDT = New DataTable
        Dim NewMemberContactInfo = New DataTable
        Dim NewMemberShipCategory = New DataTable
        Dim NewMemberContribution = New DataTable
        Dim NewMemberSurvey = New DataTable
        Dim NewMemberEmployment = New DataTable
        Dim NewCard = New DataTable
        Dim NewPhoto = New DataTable
        Dim NewSignature = New DataTable
        Dim NewBio = New DataTable
        Dim NewPMailing = New DataTable
        Dim NewMID = New DataTable
        Dim NewRTN = New DataTable


        DA.ConnectionString = SQLServerConnectionServer()
        Try
            If My.Settings.ACC_WEB_Pass = Pass And My.Settings.ACC_WEB_Uname = User Then
                'Set all datatables
                NewMemberDT = MemberDT
                NewMemberContactInfo = MembercontactinfoDT
                NewMemberShipCategory = MembershipCategoryInfoDT
                NewMemberContribution = MemberContributionDT
                NewMemberSurvey = MemberSurveyDT
                NewMemberEmployment = EmploymentHisDT
                NewCard = CardDT
                NewPhoto = PhotoDT
                NewSignature = SignatureDT
                NewBio = BioDT
                NewPMailing = PMailingDT
                NewMID = MIDDT


                If NewMemberDT.Rows.Count = 0 Or NewMemberContactInfo.Rows.Count = 0 Or NewMemberShipCategory.Rows.Count = 0 Or NewMemberSurvey.Rows.Count = 0 Or NewPhoto.Rows.Count = 0 Or NewSignature.Rows.Count = 0 Or NewBio.Rows.Count = 0 Or NewPMailing.Rows.Count = 0 Then
                    'NewMemberContribution.Rows.Count = 0 Or
                    'Response code for rows = 0
                    InsertData = "Incomplete rows"
                    Return InsertData
                End If

                'If isRTN = False Then
                '    If NewMID.Rows.Count = 0 Then
                '        InsertData = "Incomplete rows"
                '        Return InsertData
                '    End If
                'Else
                '    If NewRTN.Rows.Count = 0 Then
                '        InsertData = "Incomplete rows"
                '        Return InsertData
                '    End If
                'End If

                'tbl_member

                If DA.RecordFound("Select * from tbl_member where RefNum = '" & NewMemberDT.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [dbo].[tbl_member](" & _
                           "[RefNum]" & _
                           ",[MIDRTN]" & _
                           ",[Member_LastName]" & _
                           ",[Member_FirstName]" & _
                           ",[Member_MiddleName]" & _
                           ",[Member_Extension]" & _
                           ",[Member_NoMiddleName]" & _
                           ",[Birth_LastName]" & _
                           ",[Birth_FirstName]" & _
                           ",[Birth_MiddleName]" & _
                           ",[Birth_NoMiddleName]" & _
                           ",[Birth_Extension]" & _
                           ",[Mother_LastName]" & _
                           ",[Mother_FirstName]" & _
                           ",[Mother_MiddleName]" & _
                           ",[Mother_Extension]" & _
                           ",[Mother_NoMiddleName]" & _
                           ",[Spouse_LastName]" & _
                           ",[Spouse_FirstName]" & _
                           ",[Spouse_MiddleName]" & _
                           ",[Spouse_Extension]" & _
                           ",[Spouse_NoMiddleName]" & _
                           ",[BirthDate]" & _
                           ",[BirthCity]" & _
                           ",[BirthCountry]" & _
                           ",[Gender]" & _
                           ",[CivilStatus]" & _
                           ",[Citizenship]" & _
                           ",[CommonRefNo]" & _
                           ",[SSSID]" & _
                           ",[GSISID]" & _
                           ",[TIN]" & _
                           ",[MembershipCategory]" & _
                           ",[ApplicationDate]" & _
                           ",[KioskID]" & _
                           ",[Transaction_Ref_No]" & _
                           ",[Capture_Type]" & _
                           ",[PFR_Number]" & _
                           ",[PFR_Amount]" & _
                           ",[PFR_Date]" & _
                           ",[IsMemberActive]" & _
                           ",[IsComplete]" & _
                           ",[Card_PFRNumber]" & _
                           ",[Card_PFRAmount]" & _
                           ",[Card_PFRDate]" & _
                           ",[Application_Remarks]" & _
                           ",[requesting_branchcode]" & _
                           ",[BranchCode]" & _
                           ",[UserName]" & _
                           ",[DocumentSubmitted]" & _
                           ",[EntryDate]" & _
                           ",[LastUpdate]" & _
                           ",[isSent]" & _
                           ",[SentDate]" & _
                           ",[PaymentStatus]" & _
                           ",[BillingCtrlNum]" & _
                           ",[PaymentRemarks]" & _
                           ",[CardCounter]" & _
                           ",[CitizenshipCode]" & _
                           ",[PreferredMiddleInitial]) " & _
                            "VALUES(" & _
                            "@RefNum" & _
                           ",@MIDRTN" & _
                           ",@Member_LastName" & _
                           ",@Member_FirstName" & _
                           ",@Member_MiddleName" & _
                           ",@Member_Extension" & _
                           ",@Member_NoMiddleName" & _
                           ",@Birth_LastName" & _
                           ",@Birth_FirstName" & _
                           ",@Birth_MiddleName" & _
                           ",@Birth_NoMiddleName" & _
                           ",@Birth_Extension" & _
                           ",@Mother_LastName" & _
                           ",@Mother_FirstName" & _
                           ",@Mother_MiddleName" & _
                           ",@Mother_Extension" & _
                           ",@Mother_NoMiddleName" & _
                           ",@Spouse_LastName" & _
                           ",@Spouse_FirstName" & _
                           ",@Spouse_MiddleName" & _
                           ",@Spouse_Extension" & _
                           ",@Spouse_NoMiddleName" & _
                           ",@BirthDate" & _
                           ",@BirthCity" & _
                           ",@BirthCountry" & _
                           ",@Gender" & _
                           ",@CivilStatus" & _
                           ",@Citizenship" & _
                           ",@CommonRefNo" & _
                           ",@SSSID" & _
                           ",@GSISID" & _
                           ",@TIN" & _
                           ",@MembershipCategory" & _
                           ",@ApplicationDate" & _
                           ",@KioskID" & _
                           ",@Transaction_Ref_No" & _
                           ",@Capture_Type" & _
                           ",@PFR_Number" & _
                           ",@PFR_Amount" & _
                           ",@PFR_Date" & _
                           ",@IsMemberActive" & _
                           ",@IsComplete" & _
                           ",@Card_PFRNumber" & _
                           ",@Card_PFRAmount" & _
                           ",@Card_PFRDate" & _
                           ",@Application_Remarks" & _
                           ",@requesting_branchcode" & _
                           ",@BranchCode" & _
                           ",@UserName" & _
                           ",@DocumentSubmitted" & _
                           ",@EntryDate" & _
                           ",@LastUpdate" & _
                           ",@isSent" & _
                           ",@SentDate" & _
                           ",@PaymentStatus" & _
                           ",@BillingCtrlNum" & _
                           ",@PaymentRemarks" & _
                           ",@CardCounter" & _
                           ",@CitizenshipCode" & _
                           ",@PreferredMiddleInitial)"

                    Dim cmd As New SqlCommand(SQL)

                    cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("RefNum")
                    cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("MIDRTN")
                    cmd.Parameters.Add("@Member_LastName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Member_LastName")
                    cmd.Parameters.Add("@Member_FirstName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Member_FirstName")
                    cmd.Parameters.Add("@Member_MiddleName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Member_MiddleName")
                    cmd.Parameters.Add("@Member_Extension", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Member_Extension")
                    cmd.Parameters.Add("@Member_NoMiddleName", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("Member_NoMiddleName")
                    cmd.Parameters.Add("@Birth_LastName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Birth_LastName")
                    cmd.Parameters.Add("@Birth_FirstName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Birth_FirstName")
                    cmd.Parameters.Add("@Birth_MiddleName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Birth_MiddleName")
                    cmd.Parameters.Add("@Birth_NoMiddleName", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("Birth_NoMiddleName")
                    cmd.Parameters.Add("@Birth_Extension", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Birth_Extension")
                    cmd.Parameters.Add("@Mother_LastName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Mother_LastName")
                    cmd.Parameters.Add("@Mother_FirstName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Mother_FirstName")
                    cmd.Parameters.Add("@Mother_MiddleName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Mother_MiddleName")
                    cmd.Parameters.Add("@Mother_Extension", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Mother_Extension")
                    cmd.Parameters.Add("@Mother_NoMiddleName", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("Mother_NoMiddleName")
                    cmd.Parameters.Add("@Spouse_LastName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Spouse_LastName")
                    cmd.Parameters.Add("@Spouse_FirstName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Spouse_FirstName")
                    cmd.Parameters.Add("@Spouse_MiddleName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Spouse_MiddleName")
                    cmd.Parameters.Add("@Spouse_Extension", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Spouse_Extension")
                    cmd.Parameters.Add("@Spouse_NoMiddleName", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("Spouse_NoMiddleName")
                    cmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = NewMemberDT.Rows(0)("BirthDate")
                    cmd.Parameters.Add("@BirthCity", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("BirthCity")
                    cmd.Parameters.Add("@BirthCountry", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("BirthCountry")
                    cmd.Parameters.Add("@Gender", SqlDbType.Char).Value = NewMemberDT.Rows(0)("Gender")
                    cmd.Parameters.Add("@CivilStatus", SqlDbType.Char).Value = NewMemberDT.Rows(0)("CivilStatus")
                    cmd.Parameters.Add("@Citizenship", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Citizenship")
                    cmd.Parameters.Add("@CommonRefNo", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("CommonRefNo")
                    cmd.Parameters.Add("@SSSID", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("SSSID")
                    cmd.Parameters.Add("@GSISID", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("GSISID")
                    cmd.Parameters.Add("@TIN", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("TIN")
                    cmd.Parameters.Add("@MembershipCategory", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("MembershipCategory")
                    cmd.Parameters.Add("@ApplicationDate", SqlDbType.Date).Value = NewMemberDT.Rows(0)("ApplicationDate")
                    cmd.Parameters.Add("@KioskID", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("KioskID")
                    cmd.Parameters.Add("@Transaction_Ref_No", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Transaction_Ref_No")
                    cmd.Parameters.Add("@Capture_Type", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Capture_Type")
                    cmd.Parameters.Add("@PFR_Number", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("PFR_Number")
                    cmd.Parameters.Add("@PFR_Amount", SqlDbType.Decimal).Value = NewMemberDT.Rows(0)("PFR_Amount")
                    cmd.Parameters.Add("@PFR_Date", SqlDbType.Date).Value = NewMemberDT.Rows(0)("PFR_Date")
                    cmd.Parameters.Add("@IsMemberActive", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("IsMemberActive")
                    cmd.Parameters.Add("@IsComplete", SqlDbType.TinyInt).Value = NewMemberDT.Rows(0)("IsComplete")
                    cmd.Parameters.Add("@Card_PFRNumber", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Card_PFRNumber")
                    cmd.Parameters.Add("@Card_PFRAmount", SqlDbType.Decimal).Value = NewMemberDT.Rows(0)("Card_PFRAmount")
                    cmd.Parameters.Add("@Card_PFRDate", SqlDbType.Date).Value = NewMemberDT.Rows(0)("Card_PFRDate")
                    cmd.Parameters.Add("@Application_Remarks", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("Application_Remarks")
                    cmd.Parameters.Add("@requesting_branchcode", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("requesting_branchcode")
                    cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("BranchCode")
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("UserName")
                    cmd.Parameters.Add("@DocumentSubmitted", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("DocumentSubmitted")
                    cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewMemberDT.Rows(0)("EntryDate")
                    cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now
                    cmd.Parameters.Add("@isSent", SqlDbType.TinyInt).Value = 0
                    cmd.Parameters.Add("@SentDate", SqlDbType.DateTime).Value = Now
                    cmd.Parameters.Add("@PaymentStatus", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("PaymentStatus")
                    cmd.Parameters.Add("@BillingCtrlNum", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("BillingCtrlNum")
                    cmd.Parameters.Add("@PaymentRemarks", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("PaymentRemarks")
                    cmd.Parameters.Add("@CardCounter", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("CardCounter")
                    cmd.Parameters.Add("@CitizenshipCode", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("CitizenshipCode")
                    cmd.Parameters.Add("@PreferredMiddleInitial", SqlDbType.VarChar).Value = NewMemberDT.Rows(0)("PreferredMiddleInitial")

                    DA.RunSQLNonQuery(cmd)

                    '    SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_member] VALUES('" & _
                    '    NewMemberDT.Rows(0)(1) & "','" & _
                    '    NewMemberDT.Rows(0)(2) & "','" & _
                    '    NewMemberDT.Rows(0)(3) & "','" & _
                    '    NewMemberDT.Rows(0)(4) & "','" & _
                    '    NewMemberDT.Rows(0)(5) & "','" & _
                    '    NewMemberDT.Rows(0)(6) & "'," & _
                    '    NewMemberDT.Rows(0)(7) & ",'" & _
                    '    NewMemberDT.Rows(0)(8) & "','" & _
                    '    NewMemberDT.Rows(0)(9) & "','" & _
                    '    NewMemberDT.Rows(0)(10) & "'," & _
                    '    NewMemberDT.Rows(0)(11) & ",'" & _
                    '    NewMemberDT.Rows(0)(12) & "','" & _
                    '    NewMemberDT.Rows(0)(13) & "','" & _
                    '    NewMemberDT.Rows(0)(14) & "','" & _
                    '    NewMemberDT.Rows(0)(15) & "','" & _
                    '    NewMemberDT.Rows(0)(16) & "'," & _
                    '    NewMemberDT.Rows(0)(17) & ",'" & _
                    '    NewMemberDT.Rows(0)(18) & "','" & _
                    '    NewMemberDT.Rows(0)(19) & "','" & _
                    '    NewMemberDT.Rows(0)(20) & "','" & _
                    '    NewMemberDT.Rows(0)(21) & "'," & _
                    '    NewMemberDT.Rows(0)(22) & ",'" & _
                    '    NewMemberDT.Rows(0)(23) & "','" & _
                    '    NewMemberDT.Rows(0)(24).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberDT.Rows(0)(25).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberDT.Rows(0)(26) & "','" & _
                    '    NewMemberDT.Rows(0)(27) & "','" & _
                    '    NewMemberDT.Rows(0)(28) & "','" & _
                    '    NewMemberDT.Rows(0)(29) & "','" & _
                    '    NewMemberDT.Rows(0)(30) & "','" & _
                    '    NewMemberDT.Rows(0)(31) & "','" & _
                    '    NewMemberDT.Rows(0)(32) & "','" & _
                    '    NewMemberDT.Rows(0)(33) & "','" & _
                    '    NewMemberDT.Rows(0)(34) & "','" & _
                    '    NewMemberDT.Rows(0)(35) & "','" & _
                    '    NewMemberDT.Rows(0)(36) & "','" & _
                    '    NewMemberDT.Rows(0)(37) & "','" & _
                    '    NewMemberDT.Rows(0)(38) & "'," & _
                    '    NewMemberDT.Rows(0)(39) & ",'" & _
                    '    NewMemberDT.Rows(0)(40) & "'," & _
                    '    NewMemberDT.Rows(0)(41) & "," & _
                    '    NewMemberDT.Rows(0)(42) & ",'" & _
                    '    NewMemberDT.Rows(0)(43) & "'," & _
                    '    NewMemberDT.Rows(0)(44) & ",'" & _
                    '    NewMemberDT.Rows(0)(45) & "','" & _
                    '    NewMemberDT.Rows(0)(46) & "','" & _
                    '    NewMemberDT.Rows(0)(47) & "','" & _
                    '    NewMemberDT.Rows(0)(48) & "','" & _
                    '    NewMemberDT.Rows(0)(49) & "','" & _
                    '    NewMemberDT.Rows(0)(50).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberDT.Rows(0)(51) & "'," & _
                    '    "GetDate()," & _
                    '    "0,'" & _
                    '    NewMemberDT.Rows(0)(54) & "','" & _
                    '    NewMemberDT.Rows(0)(55) & "','" & _
                    '    NewMemberDT.Rows(0)(56) & "','" & _
                    '    NewMemberDT.Rows(0)(57) & "','" & _
                    '    NewMemberDT.Rows(0)(58) & "','" & _
                    '    NewMemberDT.Rows(0)(59) & "')"
                    '    InsertData = DA.RunSQLWithReturn(SQL)
                End If

                'Threading.Thread.Sleep(3000)

                'tbl_membercontactinfo
                If DA.RecordFound("Select * from tbl_membercontactinfo where RefNum = '" & NewMemberContactInfo.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [dbo].[tbl_membercontactinfo]" & _
                           "([RefNum]" & _
                           ",[MIDRTN]" & _
                           ",[Permanent_HBUR]" & _
                           ",[Permanent_Building]" & _
                           ",[Permanent_LotNo]" & _
                           ",[Permanent_BlockNo]" & _
                           ",[Permanent_PhaseNo]" & _
                           ",[Permanent_HouseNo]" & _
                           ",[Permanent_StreetName]" & _
                           ",[Permanent_Subdivision]" & _
                           ",[Permanent_Barangay]" & _
                           ",[Permanent_CityMunicipality]" & _
                           ",[Permanent_Province]" & _
                           ",[Permanent_ZipCode]" & _
                           ",[Present_HBUR]" & _
                           ",[Present_Building]" & _
                           ",[Present_LotNo]" & _
                           ",[Present_BlockNo]" & _
                           ",[Present_PhaseNo]" & _
                           ",[Present_HouseNo]" & _
                           ",[Present_StreetName]" & _
                           ",[Present_Subdivision]" & _
                           ",[Present_Barangay]" & _
                           ",[Present_CityMunicipality]" & _
                           ",[Present_Province]" & _
                           ",[Present_ZipCode]" & _
                           ",[PreferredMailingAddress]" & _
                           ",[Home_CountryCode]" & _
                           ",[Home_AreaCode]" & _
                           ",[Home_TelNo]" & _
                           ",[Mobile_CountryCode]" & _
                           ",[Mobile_AreaCode]" & _
                           ",[Mobile_CelNo]" & _
                           ",[Business_Direct_CountryCode]" & _
                           ",[Business_Direct_AreaCode]" & _
                           ",[Business_Direct_TelNo]" & _
                           ",[Business_Trunk_CountryCode]" & _
                           ",[Business_Trunk_AreaCode]" & _
                           ",[Business_Trunk_TelNo]" & _
                           ",[EmailAddress]" & _
                           ",[EntryDate]" & _
                           ",[LastUpdate]" & _
                           ",[Business_Trunk_Local]) " & _
                            "VALUES(" & _
                            "@RefNum" & _
                           ",@MIDRTN" & _
                           ",@Permanent_HBUR" & _
                           ",@Permanent_Building" & _
                           ",@Permanent_LotNo" & _
                           ",@Permanent_BlockNo" & _
                           ",@Permanent_PhaseNo" & _
                           ",@Permanent_HouseNo" & _
                           ",@Permanent_StreetName" & _
                           ",@Permanent_Subdivision" & _
                           ",@Permanent_Barangay" & _
                           ",@Permanent_CityMunicipality" & _
                           ",@Permanent_Province" & _
                           ",@Permanent_ZipCode" & _
                           ",@Present_HBUR" & _
                           ",@Present_Building" & _
                           ",@Present_LotNo" & _
                           ",@Present_BlockNo" & _
                           ",@Present_PhaseNo" & _
                           ",@Present_HouseNo" & _
                           ",@Present_StreetName" & _
                           ",@Present_Subdivision" & _
                           ",@Present_Barangay" & _
                           ",@Present_CityMunicipality" & _
                           ",@Present_Province" & _
                           ",@Present_ZipCode" & _
                           ",@PreferredMailingAddress" & _
                           ",@Home_CountryCode" & _
                           ",@Home_AreaCode" & _
                           ",@Home_TelNo" & _
                           ",@Mobile_CountryCode" & _
                           ",@Mobile_AreaCode" & _
                           ",@Mobile_CelNo" & _
                           ",@Business_Direct_CountryCode" & _
                           ",@Business_Direct_AreaCode" & _
                           ",@Business_Direct_TelNo" & _
                           ",@Business_Trunk_CountryCode" & _
                           ",@Business_Trunk_AreaCode" & _
                           ",@Business_Trunk_TelNo" & _
                           ",@EmailAddress" & _
                           ",@EntryDate" & _
                           ",@LastUpdate" & _
                           ",@Business_Trunk_Local)"



                    Dim cmd As New SqlCommand(SQL)

                    cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("RefNum")
                    cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("MIDRTN")
                    cmd.Parameters.Add("@Permanent_HBUR", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_HBUR")
                    cmd.Parameters.Add("@Permanent_Building", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_Building")
                    cmd.Parameters.Add("@Permanent_LotNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_LotNo")
                    cmd.Parameters.Add("@Permanent_BlockNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_BlockNo")
                    cmd.Parameters.Add("@Permanent_PhaseNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_PhaseNo")
                    cmd.Parameters.Add("@Permanent_HouseNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_HouseNo")
                    cmd.Parameters.Add("@Permanent_StreetName", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_StreetName")
                    cmd.Parameters.Add("@Permanent_Subdivision", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_Subdivision")
                    cmd.Parameters.Add("@Permanent_Barangay", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_Barangay")
                    cmd.Parameters.Add("@Permanent_CityMunicipality", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_CityMunicipality")
                    cmd.Parameters.Add("@Permanent_Province", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_Province")
                    cmd.Parameters.Add("@Permanent_ZipCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Permanent_ZipCode")
                    cmd.Parameters.Add("@Present_HBUR", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_HBUR")
                    cmd.Parameters.Add("@Present_Building", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_Building")
                    cmd.Parameters.Add("@Present_LotNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_LotNo")
                    cmd.Parameters.Add("@Present_BlockNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_BlockNo")
                    cmd.Parameters.Add("@Present_PhaseNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_PhaseNo")
                    cmd.Parameters.Add("@Present_HouseNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_HouseNo")
                    cmd.Parameters.Add("@Present_StreetName", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_StreetName")
                    cmd.Parameters.Add("@Present_Subdivision", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_Subdivision")
                    cmd.Parameters.Add("@Present_Barangay", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_Barangay")
                    cmd.Parameters.Add("@Present_CityMunicipality", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_CityMunicipality")
                    cmd.Parameters.Add("@Present_Province", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_Province")
                    cmd.Parameters.Add("@Present_ZipCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Present_ZipCode")
                    cmd.Parameters.Add("@PreferredMailingAddress", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("PreferredMailingAddress")
                    cmd.Parameters.Add("@Home_CountryCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Home_CountryCode")
                    cmd.Parameters.Add("@Home_AreaCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Home_AreaCode")
                    cmd.Parameters.Add("@Home_TelNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Home_TelNo")
                    cmd.Parameters.Add("@Mobile_CountryCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Mobile_CountryCode")
                    cmd.Parameters.Add("@Mobile_AreaCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Mobile_AreaCode")
                    cmd.Parameters.Add("@Mobile_CelNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Mobile_CelNo")
                    cmd.Parameters.Add("@Business_Direct_CountryCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Direct_CountryCode")
                    cmd.Parameters.Add("@Business_Direct_AreaCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Direct_AreaCode")
                    cmd.Parameters.Add("@Business_Direct_TelNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Direct_TelNo")
                    cmd.Parameters.Add("@Business_Trunk_CountryCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Trunk_CountryCode")
                    cmd.Parameters.Add("@Business_Trunk_AreaCode", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Trunk_AreaCode")
                    cmd.Parameters.Add("@Business_Trunk_TelNo", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Trunk_TelNo")
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("EmailAddress")
                    cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewMemberContactInfo.Rows(0)("EntryDate")
                    cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now
                    cmd.Parameters.Add("@Business_Trunk_Local", SqlDbType.VarChar).Value = NewMemberContactInfo.Rows(0)("Business_Trunk_Local")


                    DA.RunSQLNonQuery(cmd)

                    'SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_membercontactinfo] VALUES('" & _
                    'NewMemberContactInfo.Rows(0)(1) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(2) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(3).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(4).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(5).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(6).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(7).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(8).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(9).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(10).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(11).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(12).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(13).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(14).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(15).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(16).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(17).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(18).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(19).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(20).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(21).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(22).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(23).ToString.Replace("'", "''") & "','" & _
                    ' NewMemberContactInfo.Rows(0)(24) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(25) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(26) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(27) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(28) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(29) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(30) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(31) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(32) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(33) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(34) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(35) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(36) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(37) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(38) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(39) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(40) & "','" & _
                    ' NewMemberContactInfo.Rows(0)(41) & "'," & _
                    ' "GetDate(),'" & _
                    ' NewMemberContactInfo.Rows(0)(43) & "')"
                    'InsertData = DA.RunSQLWithReturn(SQL)
                End If
                'Threading.Thread.Sleep(3000)

                'tbl_membershipcategoryinfo
                If DA.RecordFound("Select * from tbl_membershipcategoryinfo where RefNum = '" & NewMemberShipCategory.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [dbo].[tbl_membershipcategoryinfo]" & _
                           "([RefNum]" & _
                           ",[MIDRTN]" & _
                           ",[EmployeeID]" & _
                           ",[EmployerName]" & _
                           ",[HBUR]" & _
                           ",[Building]" & _
                           ",[LotNo]" & _
                           ",[BlockNo]" & _
                           ",[PhaseNo]" & _
                           ",[HouseNo]" & _
                           ",[StreetName]" & _
                           ",[Subdivision]" & _
                           ",[Barangay]" & _
                           ",[CityMunicipality]" & _
                           ",[Province]" & _
                           ",[DateEmployed]" & _
                           ",[Employment_Status]" & _
                           ",[Occupation]" & _
                           ",[AFPSerialBadgeNo]" & _
                           ",[DepEdDivCodeStnCode]" & _
                           ",[TypeOfWork]" & _
                           ",[ManningAgency]" & _
                           ",[Office_Assignment]" & _
                           ",[ZipCode]" & _
                           ",[Monthly_Income]" & _
                           ",[Country_Assignment]" & _
                           ",[NatureOfWork]" & _
                           ",[EntryDate]" & _
                           ",[LastUpdate]" & _
                           ",[OFWCountryCode]" & _
                           ",[OccupationCode]" & _
                           ",[EmpStatusCode]) " & _
                             "VALUES(" & _
                           "@RefNum" & _
                           ",@MIDRTN" & _
                           ",@EmployeeID" & _
                           ",@EmployerName" & _
                           ",@HBUR" & _
                           ",@Building" & _
                           ",@LotNo" & _
                           ",@BlockNo" & _
                           ",@PhaseNo" & _
                           ",@HouseNo" & _
                           ",@StreetName" & _
                           ",@Subdivision" & _
                           ",@Barangay" & _
                           ",@CityMunicipality" & _
                           ",@Province" & _
                           ",@DateEmployed" & _
                           ",@Employment_Status" & _
                           ",@Occupation" & _
                           ",@AFPSerialBadgeNo" & _
                           ",@DepEdDivCodeStnCode" & _
                           ",@TypeOfWork" & _
                           ",@ManningAgency" & _
                           ",@Office_Assignment" & _
                           ",@ZipCode" & _
                           ",@Monthly_Income" & _
                           ",@Country_Assignment" & _
                           ",@NatureOfWork" & _
                           ",@EntryDate" & _
                           ",@LastUpdate" & _
                           ",@OFWCountryCode" & _
                           ",@OccupationCode" & _
                           ",@EmpStatusCode) "

                    Dim cmd As New SqlCommand(SQL)

                    cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("RefNum")
                    cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("MIDRTN")
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("EmployeeID")
                    cmd.Parameters.Add("@EmployerName", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("EmployerName")
                    cmd.Parameters.Add("@HBUR", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("HBUR")
                    cmd.Parameters.Add("@Building", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Building")
                    cmd.Parameters.Add("@LotNo", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("LotNo")
                    cmd.Parameters.Add("@BlockNo", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("BlockNo")
                    cmd.Parameters.Add("@PhaseNo", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("PhaseNo")
                    cmd.Parameters.Add("@HouseNo", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("HouseNo")
                    cmd.Parameters.Add("@StreetName", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("StreetName")
                    cmd.Parameters.Add("@Subdivision", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Subdivision")
                    cmd.Parameters.Add("@Barangay", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Barangay")
                    cmd.Parameters.Add("@CityMunicipality", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("CityMunicipality")
                    cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Province")
                    cmd.Parameters.Add("@DateEmployed", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("DateEmployed")
                    cmd.Parameters.Add("@Employment_Status", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Employment_Status")
                    cmd.Parameters.Add("@Occupation", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Occupation")
                    cmd.Parameters.Add("@AFPSerialBadgeNo", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("AFPSerialBadgeNo")
                    cmd.Parameters.Add("@DepEdDivCodeStnCode", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("DepEdDivCodeStnCode")
                    cmd.Parameters.Add("@TypeOfWork", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("TypeOfWork")
                    cmd.Parameters.Add("@ManningAgency", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("ManningAgency")
                    cmd.Parameters.Add("@Office_Assignment", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Office_Assignment")
                    cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("ZipCode")
                    cmd.Parameters.Add("@Monthly_Income", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Monthly_Income")
                    cmd.Parameters.Add("@Country_Assignment", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("Country_Assignment")
                    cmd.Parameters.Add("@NatureOfWork", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("NatureOfWork")
                    cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewMemberShipCategory.Rows(0)("EntryDate")
                    cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now
                    cmd.Parameters.Add("@OFWCountryCode", SqlDbType.Int).Value = NewMemberShipCategory.Rows(0)("OFWCountryCode")
                    cmd.Parameters.Add("@OccupationCode", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("OccupationCode")
                    cmd.Parameters.Add("@EmpStatusCode", SqlDbType.VarChar).Value = NewMemberShipCategory.Rows(0)("EmpStatusCode")

                    DA.RunSQLNonQuery(cmd)

                    'SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_membershipcategoryinfo] VALUES('" & _
                    '    NewMemberShipCategory.Rows(0)(1) & "','" & _
                    '    NewMemberShipCategory.Rows(0)(2) & "','" & _
                    '    NewMemberShipCategory.Rows(0)(3) & "','" & _
                    '    NewMemberShipCategory.Rows(0)(4).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(5).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(6).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(7).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(8).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(9).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(10).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(11).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(12).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(13).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(14).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(15).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(16).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(17).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(18).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(19).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(20).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(21).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(22).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(23).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(24).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(25).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(26).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(27).ToString.Replace("'", "''") & "','" & _
                    '    NewMemberShipCategory.Rows(0)(28).ToString.Replace("'", "''") & "'," & _
                    '    "GetDate()," & _
                    '    NewMemberShipCategory.Rows(0)(30) & ",'" & _
                    '    NewMemberShipCategory.Rows(0)(31) & "','" & _
                    '    NewMemberShipCategory.Rows(0)(32) & "')"

                    'InsertData = DA.RunSQLWithReturn(SQL)

                End If

                'Threading.Thread.Sleep(3000)

                'tbl_memcontribution
                If NewMemberContribution.Rows.Count = 1 Then
                    If DA.RecordFound("Select * from tbl_memcontribution where RefNum = '" & NewMemberContribution.Rows(0)(1) & "'") = False Then

                        SQL = "INSERT INTO [dbo].[tbl_memcontribution]" & _
                               "([RefNum]" & _
                               ",[MIDRTN]" & _
                               ",[DOB]" & _
                               ",[InitialPFR_Number]" & _
                               ",[InitialPFR_Date]" & _
                               ",[InitialPFR_Amount]" & _
                               ",[LastPeriodCover]" & _
                               ",[LastPFR_Number]" & _
                               ",[LastPFR_Date]" & _
                               ",[LastPFR_Amount]" & _
                               ",[TAV_Balance]" & _
                               ",[EmployerName]" & _
                               ",[Branch]" & _
                               ",[Status]" & _
                               ",[IngresID]" & _
                               ",[EntryDate]" & _
                               ",[LastUpdate]) " & _
                               "VALUES(" & _
                               "@RefNum" & _
                               ",@MIDRTN" & _
                               ",@DOB" & _
                               ",@InitialPFR_Number" & _
                               ",@InitialPFR_Date" & _
                               ",@InitialPFR_Amount" & _
                               ",@LastPeriodCover" & _
                               ",@LastPFR_Number" & _
                               ",@LastPFR_Date" & _
                               ",@LastPFR_Amount" & _
                               ",@TAV_Balance" & _
                               ",@EmployerName" & _
                               ",@Branch" & _
                               ",@Status" & _
                               ",@IngresID" & _
                               ",@EntryDate" & _
                               ",@LastUpdate) "

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("RefNum")
                        cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("MIDRTN")
                        cmd.Parameters.Add("@DOB", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("DOB")
                        cmd.Parameters.Add("@InitialPFR_Number", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("InitialPFR_Number")
                        cmd.Parameters.Add("@InitialPFR_Date", SqlDbType.Date).Value = NewMemberContribution.Rows(0)("InitialPFR_Date")
                        cmd.Parameters.Add("@InitialPFR_Amount", SqlDbType.Decimal).Value = NewMemberContribution.Rows(0)("InitialPFR_Amount")
                        cmd.Parameters.Add("@LastPeriodCover", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("LastPeriodCover")
                        cmd.Parameters.Add("@LastPFR_Number", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("LastPFR_Number")
                        cmd.Parameters.Add("@LastPFR_Date", SqlDbType.Date).Value = NewMemberContribution.Rows(0)("LastPFR_Date")
                        cmd.Parameters.Add("@LastPFR_Amount", SqlDbType.Decimal).Value = NewMemberContribution.Rows(0)("LastPFR_Amount")
                        cmd.Parameters.Add("@TAV_Balance", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("TAV_Balance")
                        cmd.Parameters.Add("@EmployerName", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("EmployerName")
                        cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("Branch")
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("Status")
                        cmd.Parameters.Add("@IngresID", SqlDbType.VarChar).Value = NewMemberContribution.Rows(0)("IngresID")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewMemberContribution.Rows(0)("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now

                        DA.RunSQLNonQuery(cmd)
                        'SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_memcontribution] VALUES('" & _
                        'NewMemberContribution.Rows(0)(1) & "','" & _
                        'NewMemberContribution.Rows(0)(2) & "','" & _
                        'NewMemberContribution.Rows(0)(3).ToString.Replace("'", "''") & "','" & _
                        'NewMemberContribution.Rows(0)(4) & "','" & _
                        'NewMemberContribution.Rows(0)(5) & "'," & _
                        'NewMemberContribution.Rows(0)(6) & ",'" & _
                        'NewMemberContribution.Rows(0)(7) & "','" & _
                        'NewMemberContribution.Rows(0)(8) & "','" & _
                        'NewMemberContribution.Rows(0)(9) & "'," & _
                        'NewMemberContribution.Rows(0)(10) & ",'" & _
                        'NewMemberContribution.Rows(0)(11) & "','" & _
                        'NewMemberContribution.Rows(0)(12).ToString.Replace("'", "''") & "','" & _
                        'NewMemberContribution.Rows(0)(13).ToString.Replace("'", "''") & "','" & _
                        'NewMemberContribution.Rows(0)(14) & "','" & _
                        'NewMemberContribution.Rows(0)(15) & "','" & _
                        'NewMemberContribution.Rows(0)(16) & "'," & _
                        '"GetDate())"

                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                End If

                Threading.Thread.Sleep(3000)

                'tbl_Photo
                If DA.RecordFound("Select * from tbl_Photo where RefNum = '" & NewPhoto.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_Photo]([RefNum],[MIDRTN],[fld_Photo],[EntryDate],[LastUpdate]) VALUES(@RefNum,@MIDRTN,@Photo,@GDate,GetDate())"
                    Dim cmd As New SqlCommand(SQL)

                    cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewPhoto.Rows(0)(1)
                    cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewPhoto.Rows(0)(2)
                    cmd.Parameters.Add("@Photo", SqlDbType.Image).Value = NewPhoto.Rows(0)(3)
                    cmd.Parameters.Add("@GDate", SqlDbType.DateTime).Value = NewPhoto.Rows(0)(4)

                    DA.RunSQLNonQuery(cmd)
                End If
                'Threading.Thread.Sleep(3000)

                'tbl_Signature
                If DA.RecordFound("Select * from tbl_Signature where RefNum = '" & NewSignature.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_Signature]([RefNum],[MIDRTN],[fld_Signature],[EntryDate],[LastUpdate]) Values(@RefNum,@MIDRTN,@Signature,@GDate,GetDate())"
                    Dim cmdSig As New SqlCommand(SQL)

                    cmdSig.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewSignature.Rows(0)(1)
                    cmdSig.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewSignature.Rows(0)(2)
                    cmdSig.Parameters.Add("@Signature", SqlDbType.Image).Value = NewSignature.Rows(0)(3)
                    cmdSig.Parameters.Add("@GDate", SqlDbType.DateTime).Value = NewSignature.Rows(0)(4)
                    DA.RunSQLNonQuery(cmdSig)
                End If
                'Threading.Thread.Sleep(3000)

                'tbl_Bio
                If DA.RecordFound("Select * from tbl_Bio where RefNum = '" & NewBio.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_bio]([RefNum],[MIDRTN],[fld_LeftPrimaryFP_template],[fld_LeftPrimaryFP_IsOverride],[fld_LeftPrimaryFP_Ansi],[fld_LeftPrimaryFP_Wsq]" & _
                    ",[fld_LeftSecondaryFP_template],[fld_LeftSecondaryFP_IsOverride],[fld_LeftSecondaryFP_Ansi],[fld_LeftSecondaryFP_Wsq]" & _
                    ",[fld_RightPrimaryFP_template],[fld_RightPrimaryFP_IsOverride],[fld_RightPrimaryFP_Ansi],[fld_RightPrimaryFP_Wsq]" & _
                    ",[fld_RightSecondaryFP_template],[fld_RightSecondaryFP_IsOverride],[fld_RightSecondaryFP_Ansi],[fld_RightSecondaryFP_Wsq],[EntryDate],[LastUpdate]) " & _
                    "VALUES(@RefNum,@MIDRTN,@LeftPriTemp,@LeftPriOverride,@LeftPriAnsi,@LeftPriWsq,@LeftSecTemp,@LeftSecOverride,@LeftSecAnsi,@LeftSecWsq,@RightPriTemp,@RightPriOverride,@RightPriAnsi,@RightPriWsq,@RightSecTemp,@RightSecOverride,@RightSecAnsi,@RightSecWsq,@GDate,GetDate())"
                    Dim cmdBio As New SqlCommand(SQL)

                    cmdBio.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewBio.Rows(0)(1)
                    cmdBio.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewBio.Rows(0)(2)

                    cmdBio.Parameters.Add("@LeftPriTemp", SqlDbType.VarChar).Value = NewBio.Rows(0)(3)
                    cmdBio.Parameters.Add("@LeftPriOverride", SqlDbType.Int).Value = NewBio.Rows(0)(4)
                    cmdBio.Parameters.Add("@LeftPriAnsi", SqlDbType.Image).Value = NewBio.Rows(0)(5)
                    cmdBio.Parameters.Add("@LeftPriWsq", SqlDbType.Image).Value = NewBio.Rows(0)(6)

                    cmdBio.Parameters.Add("@LeftSecTemp", SqlDbType.VarChar).Value = NewBio.Rows(0)(7)
                    cmdBio.Parameters.Add("@LeftSecOverride", SqlDbType.Int).Value = NewBio.Rows(0)(8)
                    cmdBio.Parameters.Add("@LeftSecAnsi", SqlDbType.Image).Value = NewBio.Rows(0)(9)
                    cmdBio.Parameters.Add("@LeftSecWsq", SqlDbType.Image).Value = NewBio.Rows(0)(10)

                    cmdBio.Parameters.Add("@RightPriTemp", SqlDbType.VarChar).Value = NewBio.Rows(0)(11)
                    cmdBio.Parameters.Add("@RightPriOverride", SqlDbType.Int).Value = NewBio.Rows(0)(12)
                    cmdBio.Parameters.Add("@RightPriAnsi", SqlDbType.Image).Value = NewBio.Rows(0)(13)
                    cmdBio.Parameters.Add("@RightPriWsq", SqlDbType.Image).Value = NewBio.Rows(0)(14)

                    cmdBio.Parameters.Add("@RightSecTemp", SqlDbType.VarChar).Value = NewBio.Rows(0)(15)
                    cmdBio.Parameters.Add("@RightSecOverride", SqlDbType.Int).Value = NewBio.Rows(0)(16)
                    cmdBio.Parameters.Add("@RightSecAnsi", SqlDbType.Image).Value = NewBio.Rows(0)(17)
                    cmdBio.Parameters.Add("@RightSecWsq", SqlDbType.Image).Value = NewBio.Rows(0)(18)

                    cmdBio.Parameters.Add("@GDate", SqlDbType.DateTime).Value = NewBio.Rows(0)(19)

                    DA.RunSQLNonQuery(cmdBio)
                End If
                'Threading.Thread.Sleep(3000)

                'tbl_Survey
                If DA.RecordFound("Select * from tbl_Survey where RefNum = '" & NewMemberSurvey.Rows(0)(1) & "'") = False Then
                    SQL = "INSERT INTO [dbo].[tbl_Survey]" & _
                           "([RefNum]" & _
                           ",[MIDRTN]" & _
                           ",[Home_Ownership]" & _
                           ",[Number_Years]" & _
                           ",[Future_Plan_Home]" & _
                           ",[Educational_Attainment]" & _
                           ",[Travels_Abroad]" & _
                           ",[Domestic_Travel]" & _
                           ",[Eat_Resto]" & _
                           ",[Goto_Mall]" & _
                           ",[Number_ChilDependent]" & _
                           ",[Number_CreditCards]" & _
                           ",[Number_Cars]" & _
                           ",[EntryDate]" & _
                           ",[LastUpdate]) " & _
                           "VALUES" & _
                           "(@RefNum" & _
                           ",@MIDRTN" & _
                           ",@Home_Ownership" & _
                           ",@Number_Years" & _
                           ",@Future_Plan_Home" & _
                           ",@Educational_Attainment" & _
                           ",@Travels_Abroad" & _
                           ",@Domestic_Travel" & _
                           ",@Eat_Resto" & _
                           ",@Goto_Mall" & _
                           ",@Number_ChilDependent" & _
                           ",@Number_CreditCards" & _
                           ",@Number_Cars" & _
                           ",@EntryDate" & _
                           ",@LastUpdate) "

                    Dim cmd As New SqlCommand(SQL)

                    cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("RefNum")
                    cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("MIDRTN")
                    cmd.Parameters.Add("@Home_Ownership", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Home_Ownership")
                    cmd.Parameters.Add("@Number_Years", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Number_Years")
                    cmd.Parameters.Add("@Future_Plan_Home", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Future_Plan_Home")
                    cmd.Parameters.Add("@Educational_Attainment", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Educational_Attainment")
                    cmd.Parameters.Add("@Travels_Abroad", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Travels_Abroad")
                    cmd.Parameters.Add("@Domestic_Travel", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Domestic_Travel")
                    cmd.Parameters.Add("@Eat_Resto", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Eat_Resto")
                    cmd.Parameters.Add("@Goto_Mall", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Goto_Mall")
                    cmd.Parameters.Add("@Number_ChilDependent", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Number_ChilDependent")
                    cmd.Parameters.Add("@Number_CreditCards", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Number_CreditCards")
                    cmd.Parameters.Add("@Number_Cars", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("Number_Cars")
                    cmd.Parameters.Add("@EntryDate", SqlDbType.VarChar).Value = NewMemberSurvey.Rows(0)("EntryDate")
                    cmd.Parameters.Add("@LastUpdate", SqlDbType.VarChar).Value = Now

                    DA.RunSQLNonQuery(cmd)
                    'SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_Survey] VALUES('" & _
                    'NewMemberSurvey.Rows(0)(1) & "','" & _
                    'NewMemberSurvey.Rows(0)(2) & "','" & _
                    'NewMemberSurvey.Rows(0)(3) & "','" & _
                    'NewMemberSurvey.Rows(0)(4) & "','" & _
                    'NewMemberSurvey.Rows(0)(5) & "','" & _
                    'NewMemberSurvey.Rows(0)(6) & "','" & _
                    'NewMemberSurvey.Rows(0)(7) & "','" & _
                    'NewMemberSurvey.Rows(0)(8) & "','" & _
                    'NewMemberSurvey.Rows(0)(9) & "','" & _
                    'NewMemberSurvey.Rows(0)(10) & "'," & _
                    'NewMemberSurvey.Rows(0)(11) & "," & _
                    'NewMemberSurvey.Rows(0)(12) & "," & _
                    'NewMemberSurvey.Rows(0)(13) & ",'" & _
                    'NewMemberSurvey.Rows(0)(14) & "'," & _
                    '"GetDate())"
                    'InsertData = DA.RunSQLWithReturn(SQL)
                End If
                'Threading.Thread.Sleep(3000)

                'tbl_employementhistory



                For Each DRH As DataRow In NewMemberEmployment.Rows
                    If DA.RecordFound("Select * from tbl_employementhistory where RefNum = '" & DRH(1) & "'") = False Then
                        SQL = "INSERT INTO [dbo].[tbl_employmenthistory]" & _
                               "([RefNum]" & _
                               ",[MIDRTN]" & _
                               ",[History_EmployerName]" & _
                               ",[History_EmployerAddress]" & _
                               ",[History_DateEmployed]" & _
                               ",[History_DateSeparated]" & _
                               ",[Office_Assignment]" & _
                               ",[EntryDate]" & _
                               ",[LastUpdate]) " & _
                               "VALUES(" & _
                               "@RefNum" & _
                               ",@MIDRTN" & _
                               ",@History_EmployerName" & _
                               ",@History_EmployerAddress" & _
                               ",@History_DateEmployed" & _
                               ",@History_DateSeparated" & _
                               ",@Office_Assignment" & _
                               ",@EntryDate" & _
                               ",@LastUpdate) "

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = DRH("RefNum")
                        cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = DRH("MIDRTN")
                        cmd.Parameters.Add("@History_EmployerName", SqlDbType.VarChar).Value = DRH("History_EmployerName")
                        cmd.Parameters.Add("@History_EmployerAddress", SqlDbType.VarChar).Value = DRH("History_EmployerAddress")
                        cmd.Parameters.Add("@History_DateEmployed", SqlDbType.VarChar).Value = DRH("History_DateEmployed")
                        cmd.Parameters.Add("@History_DateSeparated", SqlDbType.VarChar).Value = DRH("History_DateSeparated")
                        cmd.Parameters.Add("@Office_Assignment", SqlDbType.VarChar).Value = DRH("Office_Assignment")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.VarChar).Value = DRH("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.VarChar).Value = Now

                        DA.RunSQLNonQuery(cmd)

                        'SQL = "INSERT INTO [acc_pag_ibig_ms].[dbo].[tbl_employmenthistory] VALUES('" & _
                        'DRH(1) & "','" & _
                        'DRH(2) & "','" & _
                        'DRH(3).ToString.Replace("'", "''") & "','" & _
                        'DRH(4).ToString.Replace("'", "''") & "','" & _
                        'DRH(5) & "','" & _
                        'DRH(6) & "','" & _
                        'DRH(7).ToString.Replace("'", "''") & "','" & _
                        'DRH(8) & "'," & _
                        '"GetDate())"
                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                    'Threading.Thread.Sleep(3000)
                Next

                'tbl_Card
                If NewCard.Rows.Count > 0 Then
                    If DA.RecordFound("Select * from tbl_Card where RefNum = '" & NewCard.Rows(0)(1) & "'") = False Then
                        SQL = "INSERT INTO [dbo].[tbl_card]" & _
                                "([RefNum]" & _
                                ",[CardNo]" & _
                                ",[CardBin]" & _
                                ",[MID]" & _
                                ",[CurrentBalance]" & _
                                ",[AvailableBalance]" & _
                                ",[ExpiryDate]" & _
                                ",[BarcodeNumber]" & _
                                ",[CardStatus]" & _
                                ",[EntryDate]" & _
                                ",[LastUpdate]) " & _
                                "VALUES(" & _
                                "@RefNum" & _
                                ",@CardNo" & _
                                ",@CardBin" & _
                                ",@MID" & _
                                ",@CurrentBalance" & _
                                ",@AvailableBalance" & _
                                ",@ExpiryDate" & _
                                ",@BarcodeNumber" & _
                                ",@CardStatus" & _
                                ",@EntryDate" & _
                                ",@LastUpdate)"

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewCard.Rows(0)("RefNum")
                        cmd.Parameters.Add("@CardNo", SqlDbType.VarChar).Value = NewCard.Rows(0)("CardNo")
                        cmd.Parameters.Add("@CardBin", SqlDbType.VarChar).Value = NewCard.Rows(0)("CardBin")
                        cmd.Parameters.Add("@MID", SqlDbType.VarChar).Value = NewCard.Rows(0)("MID")
                        cmd.Parameters.Add("@CurrentBalance", SqlDbType.Decimal).Value = NewCard.Rows(0)("CurrentBalance")
                        cmd.Parameters.Add("@AvailableBalance", SqlDbType.Decimal).Value = NewCard.Rows(0)("AvailableBalance")
                        cmd.Parameters.Add("@ExpiryDate", SqlDbType.Date).Value = NewCard.Rows(0)("ExpiryDate")
                        cmd.Parameters.Add("@BarcodeNumber", SqlDbType.VarChar).Value = NewCard.Rows(0)("BarcodeNumber")
                        cmd.Parameters.Add("@CardStatus", SqlDbType.VarChar).Value = NewCard.Rows(0)("CardStatus")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewCard.Rows(0)("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now

                        DA.RunSQLNonQuery(cmd)

                        'SQL = "INSERT INTO [dbo].[tbl_card] VALUES('" & _
                        'NewCard.Rows(0)(1) & "','" & _
                        'NewCard.Rows(0)(2) & "','" & _
                        'NewCard.Rows(0)(3) & "','" & _
                        'NewCard.Rows(0)(4) & "'," & _
                        'NewCard.Rows(0)(5) & "," & _
                        'NewCard.Rows(0)(6) & ",'" & _
                        'NewCard.Rows(0)(7) & "','" & _
                        'NewCard.Rows(0)(8) & "','" & _
                        'NewCard.Rows(0)(9) & "','" & _
                        'NewCard.Rows(0)(10) & "'," & _
                        '"GetDate())"
                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                    'Threading.Thread.Sleep(3000)
                End If

                If NewMID.Rows.Count > 0 Then
                    If DA.RecordFound("Select * from tbl_MID where MID = '" & NewMID.Rows(0)(0) & "'") = False Then
                        SQL = "INSERT INTO [dbo].[tbl_MID]" & _
                               "([MID]" & _
                               ",[RTN]" & _
                               ",[EntryDate]" & _
                               ",[LastUpdate]" & _
                               ",[isSent]" & _
                               ",[SentDate]) " & _
                                "VALUES(" & _
                               "@MID" & _
                               ",@RTN" & _
                               ",@EntryDate" & _
                               ",@LastUpdate" & _
                               ",@isSent" & _
                               ",@SentDate) "

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@MID", SqlDbType.VarChar).Value = NewMID.Rows(0)("MID")
                        cmd.Parameters.Add("@RTN", SqlDbType.VarChar).Value = NewMID.Rows(0)("RTN")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewMID.Rows(0)("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now
                        cmd.Parameters.Add("@isSent", SqlDbType.TinyInt).Value = 0
                        cmd.Parameters.Add("@SentDate", SqlDbType.DateTime).Value = Now

                        DA.RunSQLNonQuery(cmd)
                        'SQL = "INSERT INTO [dbo].[tbl_MID] VALUES('" & _
                        'NewMID.Rows(0)(0) & "','" & _
                        'NewMID.Rows(0)(1) & "','" & _
                        'NewMID.Rows(0)(2) & "'," & _
                        '"GetDate()," & _
                        'NewMID.Rows(0)(4) & "," & _
                        '"GetDate())"

                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                End If

                If NewRTN.Rows.Count > 0 Then
                    If DA.RecordFound("Select * from tbl_RTN where RTN = '" & NewRTN.Rows(0)(0) & "'") = False Then
                        SQL = "INSERT INTO [dbo].[tbl_RTN]" & _
                               "([RTN]" & _
                               ",[EntryDate]" & _
                               ",[LastUpdate]" & _
                               ",[isSent]" & _
                               ",[SentDate]) " & _
                               "VALUES(" & _
                               "@RTN" & _
                               ",@EntryDate" & _
                               ",@LastUpdate" & _
                               ",@isSent" & _
                               ",@SentDate) "

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@RTN", SqlDbType.VarChar).Value = NewRTN.Rows(0)("RTN")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewRTN.Rows(0)("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now
                        cmd.Parameters.Add("@isSent", SqlDbType.TinyInt).Value = 0
                        cmd.Parameters.Add("@SentDate", SqlDbType.DateTime).Value = Now

                        DA.RunSQLNonQuery(cmd)
                        'SQL = "INSERT INTO [dbo].[tbl_RTN] VALUES('" & _
                        'NewRTN.Rows(0)(0) & "','" & _
                        'NewRTN.Rows(0)(1) & "'," & _
                        '"GetDate()," & _
                        'NewRTN.Rows(0)(3) & "," & _
                        '"GetDate())"

                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                End If

                'tbl_PreferredMailingAddress
                If NewPMailing.Rows.Count > 0 Then
                    If DA.RecordFound("Select * from tbl_PreferredMailingAddress where RefNum = '" & NewPMailing.Rows(0)(1) & "'") = False Then
                        SQL = "INSERT INTO [dbo].[tbl_PreferredMailingAddress]" & _
                               "([RefNum]" & _
                               ",[MIDRTN]" & _
                               ",[HBUR]" & _
                               ",[Building]" & _
                               ",[LotNo]" & _
                               ",[BlockNo]" & _
                               ",[PhaseNo]" & _
                               ",[HouseNo]" & _
                               ",[StreetName]" & _
                               ",[Subdivision]" & _
                               ",[Barangay]" & _
                               ",[CityMunicipality]" & _
                               ",[Province]" & _
                               ",[ZipCode]" & _
                               ",[PreferredMailingAddress]" & _
                               ",[Region]" & _
                               ",[CCode]" & _
                               ",[PCode]" & _
                               ",[psgc_region_code]" & _
                               ",[EntryDate]" & _
                               ",[LastUpdate]) " & _
                               "VALUES(" & _
                               "@RefNum" & _
                               ",@MIDRTN" & _
                               ",@HBUR" & _
                               ",@Building" & _
                               ",@LotNo" & _
                               ",@BlockNo" & _
                               ",@PhaseNo" & _
                               ",@HouseNo" & _
                               ",@StreetName" & _
                               ",@Subdivision" & _
                               ",@Barangay" & _
                               ",@CityMunicipality" & _
                               ",@Province" & _
                               ",@ZipCode" & _
                               ",@PreferredMailingAddress" & _
                               ",@Region" & _
                               ",@CCode" & _
                               ",@PCode" & _
                               ",@psgc_region_code" & _
                               ",@EntryDate" & _
                               ",@LastUpdate) "

                        Dim cmd As New SqlCommand(SQL)

                        cmd.Parameters.Add("@RefNum", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("RefNum")
                        cmd.Parameters.Add("@MIDRTN", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("MIDRTN")
                        cmd.Parameters.Add("@HBUR", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("HBUR")
                        cmd.Parameters.Add("@Building", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("Building")
                        cmd.Parameters.Add("@LotNo", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("LotNo")
                        cmd.Parameters.Add("@BlockNo", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("BlockNo")
                        cmd.Parameters.Add("@PhaseNo", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("PhaseNo")
                        cmd.Parameters.Add("@HouseNo", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("HouseNo")
                        cmd.Parameters.Add("@StreetName", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("StreetName")
                        cmd.Parameters.Add("@Subdivision", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("Subdivision")
                        cmd.Parameters.Add("@Barangay", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("Barangay")
                        cmd.Parameters.Add("@CityMunicipality", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("CityMunicipality")
                        cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("Province")
                        cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("ZipCode")
                        cmd.Parameters.Add("@PreferredMailingAddress", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("PreferredMailingAddress")
                        cmd.Parameters.Add("@Region", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("Region")
                        cmd.Parameters.Add("@CCode", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("CCode")
                        cmd.Parameters.Add("@PCode", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("PCode")
                        cmd.Parameters.Add("@psgc_region_code", SqlDbType.VarChar).Value = NewPMailing.Rows(0)("psgc_region_code")
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = NewPMailing.Rows(0)("EntryDate")
                        cmd.Parameters.Add("@LastUpdate", SqlDbType.DateTime).Value = Now

                        DA.RunSQLNonQuery(cmd)
                        'SQL = "INSERT INTO [dbo].[tbl_PreferredMailingAddress] VALUES('" & _
                        '    NewPMailing.Rows(0)(1) & "','" & _
                        '    NewPMailing.Rows(0)(2) & "','" & _
                        '    NewPMailing.Rows(0)(3).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(4).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(5).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(6).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(7).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(8).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(9).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(10).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(11).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(12).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(13).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(14).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(15).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(16).ToString.Replace("'", "''") & "','" & _
                        '    NewPMailing.Rows(0)(17) & "','" & _
                        '    NewPMailing.Rows(0)(18) & "','" & _
                        '    NewPMailing.Rows(0)(19) & "','" & _
                        '    NewPMailing.Rows(0)(20) & "'," & _
                        '    "GetDate())"
                        'InsertData = DA.RunSQLWithReturn(SQL)
                    End If
                End If
                'InsertData = "Success"
            Else
                'Failed Response code
                InsertData = "Invalid Credential"
                Return InsertData
            End If
            Return InsertData
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function




    <WebMethod()> _
    Public Function GetCurrentDataCapture(ByVal UID As String, ByVal PWD As String, ByVal DateOfCapture As Date) As List(Of DataTable)
        GetCurrentDataCapture = New List(Of DataTable)

        Dim DTMember As New DataTable
        Dim DTMemberContact As New DataTable
        Dim DTMemberShipCategory As New DataTable
        Dim DTMemcontribution As New DataTable
        Dim DTSurvey As New DataTable
        Dim DTPhoto As New DataTable
        Dim DTSignature As New DataTable
        Dim DTBio As New DataTable

        DA.ConnectionString = SQLServerConnectionServer()

        If My.Settings.ACC_WEB_Pass = PWD And My.Settings.ACC_WEB_Uname = UID Then
            DTMember = DA.ExecuteSQLQueryDataTable("Select * from tbl_member where applicationdate = '" & Format(DateOfCapture, "yyyy-MM-dd") & "'", "tbl_member")
            DTMemberContact = DA.ExecuteSQLQueryDataTable("Select * from tbl_membercontactinfo where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_membercontactinfo")
            DTMemberShipCategory = DA.ExecuteSQLQueryDataTable("Select * from tbl_membershipcategoryinfo where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_membershipcategoryinfo")
            DTMemcontribution = DA.ExecuteSQLQueryDataTable("Select * from tbl_memcontribution where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_memcontribution")
            DTSurvey = DA.ExecuteSQLQueryDataTable("Select * from tbl_Survey where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_Survey")
            DTPhoto = DA.ExecuteSQLQueryDataTable("Select * from tbl_Photo where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_Photo")
            DTSignature = DA.ExecuteSQLQueryDataTable("Select * from tbl_Signature where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_Signature")
            DTBio = DA.ExecuteSQLQueryDataTable("Select * from tbl_Bio where EntryDate between '" & Format(DateOfCapture, "yyyy-MM-dd") & " 01:00:00" & "' and '" & Format(DateOfCapture, "yyyy-MM-dd") & " 23:59:59" & "'", "tbl_Bio")

            GetCurrentDataCapture.Add(DTMember)
            GetCurrentDataCapture.Add(DTMemberContact)
            GetCurrentDataCapture.Add(DTMemberShipCategory)
            GetCurrentDataCapture.Add(DTMemcontribution)
            GetCurrentDataCapture.Add(DTSurvey)
            GetCurrentDataCapture.Add(DTPhoto)
            GetCurrentDataCapture.Add(DTSignature)
            GetCurrentDataCapture.Add(DTBio)
        Else
            GetCurrentDataCapture = Nothing
        End If
    End Function

    <WebMethod()> _
    Public Function GetQuery(ByVal UID As String, ByVal PWD As String, ByVal xxxQxxx As String) As DataTable
        GetQuery = New DataTable

        Dim DTMember As New DataTable


        DA.ConnectionString = SQLServerConnectionServer()

        If My.Settings.ACC_WEB_Pass = PWD And My.Settings.ACC_WEB_Uname = UID Then
            DTMember = DA.ExecuteSQLQueryDataTable(xxxQxxx, "tbl_member")
            GetQuery = DTMember
        Else
            GetQuery = Nothing
        End If
    End Function

    

    '<WebMethod()> _
    'Public Function GetQueryDev(ByVal UID As String, ByVal PWD As String, ByVal xxxQxxx As String, ByVal SelectDB As SelectServer) As DataTable
    '    GetQueryDev = New DataTable

    '    Dim DTMember As New DataTable

    '    If SelectServer.ACC_OP_SCHED Then
    '        DA.ConnectionString = SQLServerConnectionScheduling()
    '    ElseIf SelectServer.acc_pag_ibig_ms Then
    '        DA.ConnectionString = SQLServerConnectionServer()
    '    Else
    '        DA.ConnectionString = SQLServerConnectionServerPhaseI()
    '    End If


    '    If My.Settings.ACC_WEB_Pass = PWD And My.Settings.ACC_WEB_Uname = UID Then
    '        DTMember = DA.ExecuteSQLQueryDataTable(xxxQxxx, "Temp_table")
    '        GetQueryDev = DTMember
    '    Else
    '        GetQueryDev = Nothing
    '    End If
    'End Function

    <WebMethod()> _
    Public Function InsertParked(ByVal UID As String, ByVal PWD As String, ByVal ParkedDT As DataTable) As Boolean
        Dim ParkedNew As New DataTable
        Dim SQL As String

        DA.ConnectionString = SQLServerConnectionServer()

        If My.Settings.ACC_WEB_Pass = PWD And My.Settings.ACC_WEB_Uname = UID Then
            ParkedNew = ParkedDT
            For Each DR As DataRow In ParkedNew.Rows
                SQL = "INSERT INTO [dbo].[parked_transaction] " & _
                    "([Tracking_No]" & _
                    ",[MIDRTN]" & _
                    ",[KioskID]" & _
                    ",[Application_Remarks]" & _
                    ",[UserName]" & _
                    ",[IsSent]" & _
                    ",[EntryDate]" & _
                    ",[LastUpdate]) " & _
                    "VALUES " & _
                    "('" & DR(0) & "','" & _
                    DR(1) & "','" & _
                    DR(2) & "','" & _
                    DR(3) & "','" & _
                    DR(4) & "'," & _
                    DR(5) & "," & _
                    "GetDate()," & _
                    "GetDate())"

                DA.RunSQL(SQL)
            Next
            InsertParked = True
        Else
            InsertParked = False
        End If
    End Function

    <WebMethod()> _
    Public Function InsertAuditTrail(ByVal UID As String, ByVal PWD As String, ByVal AuditDT As DataTable) As Boolean
        Dim AuditTrailNew As New DataTable
        Dim SQL As String

        DA.ConnectionString = SQLServerConnectionServer()

        If My.Settings.ACC_WEB_Pass = PWD And My.Settings.ACC_WEB_Uname = UID Then
            AuditTrailNew = AuditDT
            For Each DR As DataRow In AuditTrailNew.Rows
                SQL = "INSERT INTO [dbo].[tbl_audittrail] " & _
                    "([RefNum]" & _
                    ",[UserName]" & _
                    ",[Event]" & _
                    ",[KioskID]" & _
                    ",[requesting_branchcode]" & _
                    ",[BranchCode]" & _
                    ",[CaptureStatus]" & _
                    ",[IsSent]" & _
                    ",[EntryDate]" & _
                    ",[LastUpdate]) " & _
                    "VALUES " & _
                    "('" & DR(0) & "','" & _
                    DR(1) & "','" & _
                    DR(2) & "','" & _
                    DR(3) & "','" & _
                    DR(4) & "','" & _
                    DR(5) & "','" & _
                    DR(6) & "'," & _
                    DR(7) & "," & _
                    "GetDate()," & _
                    "GetDate())"

                DA.RunSQL(SQL)
            Next
            InsertAuditTrail = True
        Else
            InsertAuditTrail = False
        End If
    End Function

End Class
