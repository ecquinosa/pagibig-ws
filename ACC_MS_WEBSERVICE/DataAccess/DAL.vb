
Imports System.Data.SqlClient

Public Class DAL
    Implements IDisposable

    Public Shared ConStr As String = "Server=" & My.Settings.Server & ";Database=" & My.Settings.Database & ";User=" & My.Settings.ServerUser & ";Password=" & My.Settings.ServerPassword & ";"
    Public Shared ConStrCentralized As String = "Server=" & My.Settings.CentralizedServer & ";Database=" & My.Settings.CentralizedDatabase & ";User=" & My.Settings.CentralizedServerUser & ";Password=" & PHASE3.PHASE3.Decrypt(My.Settings.CentralizedServerPassword) & ";"
    'Public Shared ConStrCentralized As String = "Server=" & My.Settings.CentralizedServer & ";Database=" & My.Settings.CentralizedDatabase & ";User=" & My.Settings.CentralizedServerUser & ";Password=" & My.Settings.CentralizedServerPassword & ";"


    Private dtResult As DataTable
    Private dsResult As DataSet
    Private objResult As Object
    Private _readerResult As IDataReader
    Private _sqlTrans As SqlTransaction
    Private strErrorMessage As String

    Private con As SqlConnection
    Private cmd As SqlCommand
    Private da As SqlDataAdapter
    Private _UserID As String

    Public Sub New()
        ConStr = "Server=" & My.Settings.Server & ";Database=" & My.Settings.Database & ";User=" & My.Settings.ServerUser & ";Password=" & PHASE3.PHASE3.Decrypt(My.Settings.ServerPassword) & ";"
        'ConStr = "Server=" & My.Settings.Server & ";Database=" & My.Settings.Database & ";User=" & My.Settings.ServerUser & ";Password=" & My.Settings.ServerPassword & ";"
    End Sub

    'Public Sub New(ByVal isMain As Boolean)
    '    ConStr = "Server=" & My.Settings.Server & ";Database=" & My.Settings.Database & ";User=" & My.Settings.ServerUser & ";Password=" & PHASE3.PHASE3.Decrypt(My.Settings.ServerPassword) & ";"
    'End Sub

    Public Sub New(ByVal _UserID As String)
        Me._UserID = _UserID
    End Sub

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return strErrorMessage
        End Get
    End Property

    Public Property SqlTrans As SqlTransaction
        Get
            Return _sqlTrans
        End Get
        Set(ByVal value As SqlTransaction)
            _sqlTrans = value
        End Set
    End Property

    Public ReadOnly Property TableResult() As DataTable
        Get
            Return dtResult
        End Get
    End Property

    Public ReadOnly Property DatasetResult() As DataSet
        Get
            Return dsResult
        End Get
    End Property

    Public ReadOnly Property ObjectResult() As Object
        Get
            Return objResult
        End Get
    End Property

    Public ReadOnly Property ReaderResult() As IDataReader
        Get
            Return _readerResult
        End Get
    End Property

    Public Sub ClearAllPools()
        SqlConnection.ClearAllPools()
    End Sub

    Private Sub OpenConnection()
        If con Is Nothing Then
            con = New SqlConnection(ConStr)
        End If
    End Sub

    Private Sub OpenConnectionCentralizeds()
        If Not con Is Nothing Then
            CloseConnection()
        End If
        con = New SqlConnection(ConStrCentralized)
    End Sub


    Private Sub CloseConnection()
        If Not cmd Is Nothing Then cmd.Dispose()
        If Not da Is Nothing Then da.Dispose()
        If Not _readerResult Is Nothing Then
            _readerResult.Close()
            _readerResult.Dispose()
        End If
        If Not con Is Nothing Then If con.State = ConnectionState.Open Then con.Close()
        ClearAllPools()
    End Sub

    Private Sub ExecuteNonQuery(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        If con.State = ConnectionState.Closed Then con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub

    Private Sub ExecuteNonQueryWithSqlTrans(ByVal cmdType As CommandType, ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction))
        cmd.CommandType = cmdType

        If con.State = ConnectionState.Closed Then con.Open()
        Dim myTran As SqlTransaction

        If myTrans.Count = 0 Then
            myTran = con.BeginTransaction
            cmd.Transaction = myTran
            myTrans.Add(myTran)
        Else
            cmd.Transaction = myTrans(0)
        End If

        cmd.ExecuteNonQuery()
        'con.Close()
    End Sub

    Private Sub _ExecuteScalar(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        If con.State = ConnectionState.Closed Then con.Open()
        Dim _obj As Object
        _obj = cmd.ExecuteScalar()
        con.Close()

        objResult = _obj
    End Sub

    Private Sub _ExecuteReader(ByVal cmdType As CommandType)
        cmd.CommandType = cmdType

        'If con.State = ConnectionState.Open Then con.Close()
        'con.Open()
        If con.State = ConnectionState.Closed Then con.Open()
        Dim reader As SqlDataReader = cmd.ExecuteReader

        _readerResult = reader
    End Sub

    Private Sub FillDataAdapter(ByVal cmdType As CommandType)
        cmd.CommandTimeout = 0
        cmd.CommandType = cmdType
        da = New SqlDataAdapter(cmd)
        Dim _dt As New DataTable
        _dt.TableName = "ResponseTable"
        da.Fill(_dt)
        dtResult = _dt
    End Sub

    Private Sub ExecuteQueryWithResponse(ByVal cmdType As CommandType, ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction))

        cmd.CommandTimeout = 0
        cmd.CommandType = cmdType
        Dim myTran As SqlTransaction

        If con.State = ConnectionState.Closed Then con.Open()
        If myTrans.Count = 0 Then
            myTran = con.BeginTransaction
            cmd.Transaction = myTran
            myTrans.Add(myTran)
        Else
            cmd.Transaction = myTrans(0)
        End If

        da = New SqlDataAdapter(cmd)
        Dim _dt As New DataTable
        _dt.TableName = "ResponseTable"
        da.Fill(_dt)
        dtResult = _dt
    End Sub

    Public Function IsConnectionOK(Optional ByVal strConString As String = "") As Boolean
        Try
            If strConString <> "" Then ConStr = strConString
            OpenConnection()

            con.Open()
            con.Close()

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ExecuteQuery(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddAUB(ByVal UserID As Integer, ByVal KioskID As String, ByVal TxnNo As String,
                           ByVal RequestType As String, ByVal PagIBIGID As String, ByVal CardNo As String, ByVal AcctNo As String,
                           ByVal Status As String, ByVal RefTxnNo As String) As Boolean
        Try
            'If CardNo <> "" Then CardNo = Strings.StrDup(CardNo.Length - 4, "X") & Microsoft.VisualBasic.Right(CardNo, 4)
            'If AcctNo <> "" Then CardNo = Strings.StrDup(CardNo.Length - 4, "X") & Microsoft.VisualBasic.Right(AcctNo, 4)

            OpenConnection()
            cmd = New SqlCommand("prcAddAUB", con)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("KioskID", KioskID)
            cmd.Parameters.AddWithValue("TxnNo", TxnNo)
            cmd.Parameters.AddWithValue("RequestType", RequestType)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("CardNo", CardNo)
            cmd.Parameters.AddWithValue("AcctNo", AcctNo)
            cmd.Parameters.AddWithValue("Status", Status)
            cmd.Parameters.AddWithValue("RefTxnNo", RefTxnNo)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddUBP(ByVal UserID As Integer, ByVal KioskID As String, ByVal TxnNo As String,
                           ByVal PagIBIGID As String, ByVal AcctNo As String) As Boolean
        Try
            'If CardNo <> "" Then CardNo = Strings.StrDup(CardNo.Length - 4, "X") & Microsoft.VisualBasic.Right(CardNo, 4)
            'If AcctNo <> "" Then CardNo = Strings.StrDup(CardNo.Length - 4, "X") & Microsoft.VisualBasic.Right(AcctNo, 4)

            OpenConnection()
            cmd = New SqlCommand("prcAddUBP", con)
            cmd.Parameters.AddWithValue("UserID", UserID)
            cmd.Parameters.AddWithValue("KioskID", KioskID)
            cmd.Parameters.AddWithValue("TxnNo", TxnNo)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("AcctNo", AcctNo)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddOR(ByVal ORNumber As String, ByVal PagIBIGID As String) As Boolean
        Try
            'OpenConnectionCentralizeds()
            'cmd = New SqlCommand("INSERT INTO tbl_DCS_OR (ORNumber,PagIBIGID,DatePost,TimePost,BankID) VALUES (@ORNumber,@PagIBIGID,GETDATE(),GETDATE(), " + My.Settings.BankID + ")", con)
            'cmd.Parameters.AddWithValue("ORNumber", ORNumber)
            'cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID) 
            OpenConnection()
            cmd = New SqlCommand("INSERT INTO tbl_DCS_OR (ORNumber,PagIBIGID,DatePost,TimePost) VALUES (@ORNumber,@PagIBIGID,GETDATE(),GETDATE())", con)
            cmd.Parameters.AddWithValue("ORNumber", ORNumber)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)

            ExecuteNonQuery(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddAuditTrail(ByVal RefNum As String, ByVal UserName As String, ByVal AuditEvent As String,
                                  ByVal KioskID As String, ByVal requesting_branchcode As String,
                                  ByVal BranchCode As String, ByVal CaptureStatus As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand("prcAddAuditTrail", con)
            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("UserName", UserName)
            cmd.Parameters.AddWithValue("Event", AuditEvent)
            cmd.Parameters.AddWithValue("KioskID", KioskID)
            cmd.Parameters.AddWithValue("requesting_branchcode", requesting_branchcode)
            cmd.Parameters.AddWithValue("BranchCode", BranchCode)
            cmd.Parameters.AddWithValue("CaptureStatus", CaptureStatus)

            ExecuteNonQuery(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    'Public Function AddMemberCompleteTxn(ByVal memberInfo As MemberInfo) As Boolean
    '    Dim myTrans As SqlTransaction = Nothing
    '    Dim bln As Boolean = False

    '    bln = AddMember(memberInfo.Transaction_Ref_No, memberInfo.MemberID, memberInfo.MemberName.LastName,
    '                    memberInfo.MemberName.FirstName, memberInfo.MemberName.MiddleName, memberInfo.MemberName.Ext,
    '                    memberInfo.MemberName.IsNoMiddleName, memberInfo.BirthCertName.LastName, memberInfo.BirthCertName.FirstName,
    '                    memberInfo.BirthCertName.MiddleName, memberInfo.BirthCertName.IsNoMiddleName, memberInfo.BirthCertName.Ext,
    '                    memberInfo.MotherName.LastName, memberInfo.MotherName.FirstName, memberInfo.MotherName.MiddleName,
    '                    memberInfo.MotherName.Ext, memberInfo.MotherName.IsNoMiddleName, memberInfo.SpouseName.LastName,
    '                    memberInfo.SpouseName.LastName, memberInfo.SpouseName.MiddleName, memberInfo.SpouseName.Ext,
    '                    memberInfo.SpouseName.IsNoMiddleName, memberInfo.MemberBirthdate, memberInfo.MemberBirthPlaceMunicipal,
    '                    memberInfo.MemberBirthPlaceCountry, memberInfo.MemberGender, memberInfo.MemberCivilStatus,
    '                    memberInfo.MemberCitizenship, memberInfo.MemberCRN, memberInfo.MemberSSS, memberInfo.MemberGSIS,
    '                    memberInfo.MemberTIN, memberInfo.MembershipCategory.MembershipCategory, memberInfo.DateApplication, memberInfo.KioskID,
    '                    memberInfo.Transaction_Ref_No, memberInfo.Capture_Type, memberInfo.PFR_Number,
    '                    memberInfo.PFR_Amount, memberInfo.PFR_Date, memberInfo.IsMemberActive, memberInfo.IsComplete,
    '                    memberInfo.CardPFR_Number, memberInfo.CardPFR_Amount, memberInfo.CardPFR_Date, memberInfo.ApplicationRemarks,
    '                    memberInfo.PagibigBranch, memberInfo.BranchCode, "", "Doc Submitted", 1, Now.ToString, "PAID", "", "",
    '                    memberInfo.MemberCitizenshipCode,
    '                    myTrans)

    '    If Not bln Then myTrans.Rollback()

    '    'bln = AddMemberContactinfo(memberInfo.MemberRTN, memberInfo.MemberID, memberInfo.PermanentAddress.HBURNumber,
    '    '                           memberInfo.PermanentAddress.Building, memberInfo.PermanentAddress.LotNo, memberInfo.PermanentAddress.BlockNo,
    '    '                           memberInfo.PermanentAddress.PhaseNo, memberInfo.PermanentAddress.HouseNo,
    '    '                           memberInfo.PermanentAddress.StreetName, memberInfo.PermanentAddress.Subdivision,
    '    '                           memberInfo.PermanentAddress.Barangay, memberInfo.PermanentAddress.b



    '    If bln Then myTrans.Commit()
    'End Function

    Public Function AddMember(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal Member_LastName As String,
                              ByVal Member_FirstName As String,
                              ByVal Member_MiddleName As String,
                              ByVal Member_Extension As String,
                              ByVal Member_NoMiddleName As Short,
                              ByVal Birth_LastName As String,
                              ByVal Birth_FirstName As String,
                              ByVal Birth_MiddleName As String,
                              ByVal Birth_NoMiddleName As Short,
                              ByVal Birth_Extension As String,
                              ByVal Mother_LastName As String,
                              ByVal Mother_FirstName As String,
                              ByVal Mother_MiddleName As String,
                              ByVal Mother_Extension As String,
                              ByVal Mother_NoMiddleName As Short,
                              ByVal Spouse_LastName As String,
                              ByVal Spouse_FirstName As String,
                              ByVal Spouse_MiddleName As String,
                              ByVal Spouse_Extension As String,
                              ByVal Spouse_NoMiddleName As Short,
                              ByVal BirthDate As Date,
                              ByVal BirthCity As String,
                              ByVal BirthCountry As String,
                              ByVal Gender As String,
                              ByVal CivilStatus As String,
                              ByVal Citizenship As String,
                              ByVal CommonRefNo As String,
                              ByVal SSSID As String,
                              ByVal GSISID As String,
                              ByVal TIN As String,
                              ByVal MembershipCategory As String,
                              ByVal ApplicationDate As Date,
                              ByVal KioskID As String,
                              ByVal Transaction_Ref_No As String,
                              ByVal Capture_Type As String,
                              ByVal PFR_Number As String,
                              ByVal PFR_Amount As Decimal,
                              ByVal PFR_Date As Date,
                              ByVal IsMemberActive As Short,
                              ByVal IsComplete As Short,
                              ByVal Card_PFRNumber As String,
                              ByVal Card_PFRAmount As Decimal,
                              ByVal Card_PFRDate As Date,
                              ByVal Application_Remarks As String,
                              ByVal requesting_branchcode As String,
                              ByVal BranchCode As String,
                              ByVal UserName As String,
                              ByVal DocumentSubmitted As String,
                              ByVal PaymentStatus As String,
                              ByVal BillingCtrlNum As String,
                              ByVal PaymentRemarks As String,
                              ByVal CardCounter As String,
                              ByVal CitizenshipCode As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddMember", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("Member_LastName", Member_LastName)
            cmd.Parameters.AddWithValue("Member_FirstName", Member_FirstName)
            cmd.Parameters.AddWithValue("Member_MiddleName", Member_MiddleName)
            cmd.Parameters.AddWithValue("Member_Extension", Member_Extension)
            cmd.Parameters.AddWithValue("Member_NoMiddleName", Member_NoMiddleName)
            cmd.Parameters.AddWithValue("Birth_LastName", Birth_LastName)
            cmd.Parameters.AddWithValue("Birth_FirstName", Birth_FirstName)
            cmd.Parameters.AddWithValue("Birth_MiddleName", Birth_MiddleName)
            cmd.Parameters.AddWithValue("Birth_NoMiddleName", Birth_NoMiddleName)
            cmd.Parameters.AddWithValue("Birth_Extension", Birth_Extension)
            cmd.Parameters.AddWithValue("Mother_LastName", Mother_LastName)
            cmd.Parameters.AddWithValue("Mother_FirstName", Mother_FirstName)
            cmd.Parameters.AddWithValue("Mother_MiddleName", Mother_MiddleName)
            cmd.Parameters.AddWithValue("Mother_Extension", Mother_Extension)
            cmd.Parameters.AddWithValue("Mother_NoMiddleName", Mother_NoMiddleName)
            cmd.Parameters.AddWithValue("Spouse_LastName", Spouse_LastName)
            cmd.Parameters.AddWithValue("Spouse_FirstName", Spouse_FirstName)
            cmd.Parameters.AddWithValue("Spouse_MiddleName", Spouse_MiddleName)
            cmd.Parameters.AddWithValue("Spouse_Extension", Spouse_Extension)
            cmd.Parameters.AddWithValue("Spouse_NoMiddleName", Spouse_NoMiddleName)
            cmd.Parameters.AddWithValue("BirthDate", BirthDate)
            cmd.Parameters.AddWithValue("BirthCity", BirthCity)
            cmd.Parameters.AddWithValue("BirthCountry", BirthCountry)
            cmd.Parameters.AddWithValue("Gender", Gender)
            cmd.Parameters.AddWithValue("CivilStatus", CivilStatus)
            cmd.Parameters.AddWithValue("Citizenship", Citizenship)
            cmd.Parameters.AddWithValue("CommonRefNo", CommonRefNo)
            cmd.Parameters.AddWithValue("SSSID", SSSID)
            cmd.Parameters.AddWithValue("GSISID", GSISID)
            cmd.Parameters.AddWithValue("TIN", TIN)
            cmd.Parameters.AddWithValue("MembershipCategory", MembershipCategory)
            cmd.Parameters.AddWithValue("ApplicationDate", ApplicationDate)
            cmd.Parameters.AddWithValue("KioskID", KioskID)
            cmd.Parameters.AddWithValue("Transaction_Ref_No", Transaction_Ref_No)
            cmd.Parameters.AddWithValue("Capture_Type", Capture_Type)
            cmd.Parameters.AddWithValue("PFR_Number", PFR_Number)
            cmd.Parameters.AddWithValue("PFR_Amount", PFR_Amount)
            'cmd.Parameters.AddWithValue("PFR_Date", PFR_Date)
            cmd.Parameters.AddWithValue("PFR_Date", IIf(PFR_Date.Date = New DateTime(1, 1, 1).Date, DBNull.Value, PFR_Date))
            'cmd.Parameters.AddWithValue("PFR_Date", PFR_Date)
            cmd.Parameters.AddWithValue("IsMemberActive", IsMemberActive)
            cmd.Parameters.AddWithValue("IsComplete", IsComplete)
            cmd.Parameters.AddWithValue("Card_PFRNumber", Card_PFRNumber)
            cmd.Parameters.AddWithValue("Card_PFRAmount", Card_PFRAmount)
            cmd.Parameters.AddWithValue("Card_PFRDate", IIf(Card_PFRDate.Date = New DateTime(1, 1, 1).Date, DBNull.Value, Card_PFRDate))
            'cmd.Parameters.AddWithValue("Card_PFRDate", Card_PFRDate)
            cmd.Parameters.AddWithValue("Application_Remarks", Application_Remarks)
            cmd.Parameters.AddWithValue("requesting_branchcode", requesting_branchcode)
            cmd.Parameters.AddWithValue("BranchCode", BranchCode)
            cmd.Parameters.AddWithValue("UserName", UserName)
            cmd.Parameters.AddWithValue("DocumentSubmitted", DocumentSubmitted)
            'cmd.Parameters.AddWithValue("isSent", isSent)
            'cmd.Parameters.AddWithValue("SentDate", SentDate)
            cmd.Parameters.AddWithValue("PaymentStatus", PaymentStatus)
            cmd.Parameters.AddWithValue("BillingCtrlNum", BillingCtrlNum)
            cmd.Parameters.AddWithValue("PaymentRemarks", PaymentRemarks)
            cmd.Parameters.AddWithValue("CardCounter", CardCounter)
            cmd.Parameters.AddWithValue("CitizenshipCode", CitizenshipCode)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddMemberContactinfo(ByVal RefNum As String, ByVal PagIBIGID As String, ByVal Permanent_HBUR As String,
                                         ByVal Permanent_Building As String, ByVal Permanent_LotNo As String,
                                         ByVal Permanent_BlockNo As String, ByVal Permanent_PhaseNo As String,
                                         ByVal Permanent_HouseNo As String, ByVal Permanent_StreetName As String,
                                         ByVal Permanent_Subdivision As String, ByVal Permanent_Barangay As String,
                                         ByVal Permanent_PSGC_Barangay_Code As String, ByVal Permanent_CityMunicipality As String,
                                         ByVal Permanent_PSGC_City_Mun_Code As String, ByVal Permanent_Province As String,
                                         ByVal Permanent_PSGC_Province_code As String, ByVal Permanent_Region As String,
                                         ByVal Permanent_PSGC_Region_Code As String, ByVal Permanent_ZipCode As String,
                                         ByVal Present_HBUR As String, ByVal Present_Building As String,
                                         ByVal Present_LotNo As String, ByVal Present_BlockNo As String,
                                         ByVal Present_PhaseNo As String, ByVal Present_HouseNo As String,
                                         ByVal Present_StreetName As String, ByVal Present_Subdivision As String,
                                         ByVal Present_Barangay As String, ByVal Present_PSGC_Barangay_Code As String,
                                         ByVal Present_CityMunicipality As String, ByVal Present_PSGC_City_Mun_Code As String,
                                         ByVal Present_Province As String, ByVal Present_Province_code As String,
                                         ByVal Present_Region As String, ByVal Present_PSGC_Region_Code As String,
                                         ByVal Present_ZipCode As String, ByVal PreferredMailingAddress As String,
                                         ByVal Home_CountryCode As String, ByVal Home_AreaCode As String,
                                         ByVal Home_TelNo As String, ByVal Mobile_CountryCode As String,
                                         ByVal Mobile_AreaCode As String, ByVal Mobile_CelNo As String,
                                         ByVal Business_Direct_CountryCode As String, ByVal Business_Direct_AreaCode As String,
                                         ByVal Business_Direct_TelNo As String, ByVal Business_Trunk_CountryCode As String,
                                         ByVal Business_Trunk_AreaCode As String, ByVal Business_Trunk_TelNo As String,
                                         ByVal EmailAddress As String, ByVal Business_Trunk_Local As String,
                                         ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddMemberContactinfo", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("Permanent_HBUR", Permanent_HBUR)
            cmd.Parameters.AddWithValue("Permanent_Building", Permanent_Building)
            cmd.Parameters.AddWithValue("Permanent_LotNo", Permanent_LotNo)
            cmd.Parameters.AddWithValue("Permanent_BlockNo", Permanent_BlockNo)
            cmd.Parameters.AddWithValue("Permanent_PhaseNo", Permanent_PhaseNo)
            cmd.Parameters.AddWithValue("Permanent_HouseNo", Permanent_HouseNo)
            cmd.Parameters.AddWithValue("Permanent_StreetName", Permanent_StreetName)
            cmd.Parameters.AddWithValue("Permanent_Subdivision", Permanent_Subdivision)
            cmd.Parameters.AddWithValue("Permanent_Barangay", Permanent_Barangay)
            cmd.Parameters.AddWithValue("Permanent_PSGC_Barangay_Code", Permanent_PSGC_Barangay_Code)
            cmd.Parameters.AddWithValue("Permanent_CityMunicipality", Permanent_CityMunicipality)
            cmd.Parameters.AddWithValue("Permanent_PSGC_City_Mun_Code", Permanent_PSGC_City_Mun_Code)
            cmd.Parameters.AddWithValue("Permanent_Province", Permanent_Province)
            cmd.Parameters.AddWithValue("Permanent_PSGC_Province_code", Permanent_PSGC_Province_code)
            cmd.Parameters.AddWithValue("Permanent_Region", Permanent_Region)
            cmd.Parameters.AddWithValue("Permanent_PSGC_Region_Code", Permanent_PSGC_Region_Code)
            cmd.Parameters.AddWithValue("Permanent_ZipCode", Permanent_ZipCode)
            cmd.Parameters.AddWithValue("Present_HBUR", Present_HBUR)
            cmd.Parameters.AddWithValue("Present_Building", Present_Building)
            cmd.Parameters.AddWithValue("Present_LotNo", Present_LotNo)
            cmd.Parameters.AddWithValue("Present_BlockNo", Present_BlockNo)
            cmd.Parameters.AddWithValue("Present_PhaseNo", Present_PhaseNo)
            cmd.Parameters.AddWithValue("Present_HouseNo", Present_HouseNo)
            cmd.Parameters.AddWithValue("Present_StreetName", Present_StreetName)
            cmd.Parameters.AddWithValue("Present_Subdivision", Present_Subdivision)
            cmd.Parameters.AddWithValue("Present_Barangay", Present_Barangay)
            cmd.Parameters.AddWithValue("Present_PSGC_Barangay_Code", Present_PSGC_Barangay_Code)
            cmd.Parameters.AddWithValue("Present_CityMunicipality", Present_CityMunicipality)
            cmd.Parameters.AddWithValue("Present_PSGC_City_Mun_Code", Present_PSGC_City_Mun_Code)
            cmd.Parameters.AddWithValue("Present_Province", Present_Province)
            cmd.Parameters.AddWithValue("Present_Province_code", Present_Province_code)
            cmd.Parameters.AddWithValue("Present_Region", Present_Region)
            cmd.Parameters.AddWithValue("Present_PSGC_Region_Code", Present_PSGC_Region_Code)
            cmd.Parameters.AddWithValue("Present_ZipCode", Present_ZipCode)
            cmd.Parameters.AddWithValue("PreferredMailingAddress", PreferredMailingAddress)
            cmd.Parameters.AddWithValue("Home_CountryCode", Home_CountryCode)
            cmd.Parameters.AddWithValue("Home_AreaCode", Home_AreaCode)
            cmd.Parameters.AddWithValue("Home_TelNo", Home_TelNo)
            cmd.Parameters.AddWithValue("Mobile_CountryCode", Mobile_CountryCode)
            cmd.Parameters.AddWithValue("Mobile_AreaCode", Mobile_AreaCode)
            cmd.Parameters.AddWithValue("Mobile_CelNo", Mobile_CelNo)
            cmd.Parameters.AddWithValue("Business_Direct_CountryCode", Business_Direct_CountryCode)
            cmd.Parameters.AddWithValue("Business_Direct_AreaCode", Business_Direct_AreaCode)
            cmd.Parameters.AddWithValue("Business_Direct_TelNo", Business_Direct_TelNo)
            cmd.Parameters.AddWithValue("Business_Trunk_CountryCode", Business_Trunk_CountryCode)
            cmd.Parameters.AddWithValue("Business_Trunk_AreaCode", Business_Trunk_AreaCode)
            cmd.Parameters.AddWithValue("Business_Trunk_TelNo", Business_Trunk_TelNo)
            cmd.Parameters.AddWithValue("EmailAddress", EmailAddress)
            cmd.Parameters.AddWithValue("Business_Trunk_Local", Business_Trunk_Local)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddMembershipCategoryInfo(ByVal RefNum As String,
                                              ByVal PagIBIGID As String,
                                              ByVal EmployeeID As String,
                                              ByVal EmployerName As String,
                                              ByVal Employer_HBUR As String,
                                              ByVal Employer_Building As String,
                                              ByVal Employer_LotNo As String,
                                              ByVal Employer_BlockNo As String,
                                              ByVal Employer_PhaseNo As String,
                                              ByVal Employer_HouseNo As String,
                                              ByVal Employer_StreetName As String,
                                              ByVal Employer_Subdivision As String,
                                              ByVal Employer_Barangay As String,
                                              ByVal Employer_psgc_Barangay_code As String,
                                              ByVal Employer_CityMunicipality As String,
                                              ByVal Employer_psgc_city_mun_code As String,
                                              ByVal Employer_Province As String,
                                              ByVal Employer_psgc_Province_code As String,
                                              ByVal Employer_Region As String,
                                              ByVal Employer_psgc_region_code As String,
                                              ByVal Employer_ZipCode As String,
                                              ByVal DateEmployed As String,
                                              ByVal Employment_Status_code As String,
                                              ByVal Occupation As String,
                                              ByVal OccupationCode As String,
                                              ByVal AFPSerialBadgeNo As String,
                                              ByVal DepEdDivCodeStnCode As String,
                                              ByVal TypeOfWork As String,
                                              ByVal Income_Code As String,
                                              ByVal Country_Assignment As String,
                                              ByVal NatureOfWork As String,
                                              ByVal OFWCountryCode As Integer,
                                              ByVal EmpStatusCode As String,
                                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddMembershipCategoryInfo", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("EmployeeID", EmployeeID)
            cmd.Parameters.AddWithValue("EmployerName", EmployerName)
            cmd.Parameters.AddWithValue("Employer_HBUR", Employer_HBUR)
            cmd.Parameters.AddWithValue("Employer_Building", Employer_Building)
            cmd.Parameters.AddWithValue("Employer_LotNo", Employer_LotNo)
            cmd.Parameters.AddWithValue("Employer_BlockNo", Employer_BlockNo)
            cmd.Parameters.AddWithValue("Employer_PhaseNo", Employer_PhaseNo)
            cmd.Parameters.AddWithValue("Employer_HouseNo", Employer_HouseNo)
            cmd.Parameters.AddWithValue("Employer_StreetName", Employer_StreetName)
            cmd.Parameters.AddWithValue("Employer_Subdivision", Employer_Subdivision)
            cmd.Parameters.AddWithValue("Employer_Barangay", Employer_Barangay)
            cmd.Parameters.AddWithValue("Employer_psgc_Barangay_code", Employer_psgc_Barangay_code)
            cmd.Parameters.AddWithValue("Employer_CityMunicipality", Employer_CityMunicipality)
            cmd.Parameters.AddWithValue("Employer_psgc_city_mun_code", Employer_psgc_city_mun_code)
            cmd.Parameters.AddWithValue("Employer_Province", Employer_Province)
            cmd.Parameters.AddWithValue("Employer_psgc_Province_code", Employer_psgc_Province_code)
            cmd.Parameters.AddWithValue("Employer_Region", Employer_Region)
            cmd.Parameters.AddWithValue("Employer_psgc_region_code", Employer_psgc_region_code)
            cmd.Parameters.AddWithValue("Employer_ZipCode", Employer_ZipCode)
            cmd.Parameters.AddWithValue("DateEmployed", DateEmployed)
            cmd.Parameters.AddWithValue("Employment_Status_code", Employment_Status_code)
            cmd.Parameters.AddWithValue("Occupation", Occupation)
            cmd.Parameters.AddWithValue("OccupationCode", OccupationCode)
            cmd.Parameters.AddWithValue("AFPSerialBadgeNo", AFPSerialBadgeNo)
            cmd.Parameters.AddWithValue("DepEdDivCodeStnCode", DepEdDivCodeStnCode)
            cmd.Parameters.AddWithValue("TypeOfWork", TypeOfWork)
            cmd.Parameters.AddWithValue("Income_Code", Income_Code)
            cmd.Parameters.AddWithValue("Country_Assignment", Country_Assignment)
            cmd.Parameters.AddWithValue("NatureOfWork", NatureOfWork)
            cmd.Parameters.AddWithValue("OFWCountryCode", OFWCountryCode)
            cmd.Parameters.AddWithValue("EmpStatusCode", EmpStatusCode)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddMemContribution(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal InitialPFR_Number As String,
                              ByVal InitialPFR_Date As Date,
                              ByVal InitialPFR_Amount As Decimal,
                              ByVal LastPeriodCover As String,
                              ByVal LastPFR_Number As String,
                              ByVal LastPFR_Date As Date,
                              ByVal LastPFR_Amount As Decimal,
                              ByVal TAV_Balance As String,
                              ByVal EmployerName As String,
                              ByVal Branch As String,
                              ByVal Status As String,
                              ByVal IngresID As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddMemContribution", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("InitialPFR_Number", InitialPFR_Number)
            cmd.Parameters.AddWithValue("InitialPFR_Date", InitialPFR_Date)
            cmd.Parameters.AddWithValue("InitialPFR_Amount", InitialPFR_Amount)
            cmd.Parameters.AddWithValue("LastPeriodCover", LastPeriodCover)
            cmd.Parameters.AddWithValue("LastPFR_Number", LastPFR_Number)
            cmd.Parameters.AddWithValue("LastPFR_Date", LastPFR_Date)
            cmd.Parameters.AddWithValue("LastPFR_Amount", LastPFR_Amount)
            cmd.Parameters.AddWithValue("TAV_Balance", TAV_Balance)
            cmd.Parameters.AddWithValue("EmployerName", EmployerName)
            cmd.Parameters.AddWithValue("Branch", Branch)
            cmd.Parameters.AddWithValue("Status", Status)
            cmd.Parameters.AddWithValue("IngresID", IngresID)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function Addinstant_issuance(ByVal RefNum As String,
                              ByVal MIDRTN As String,
                              ByVal OCR As String,
                              ByVal PrintedDate As Date,
                              ByVal PrintCounter As String,
                              ByVal PrinterSerial As String,
                              ByVal EntryDate As Date,
                              ByVal IsSent As String,
                              ByVal DateSent As Date,
                              ByVal ApplicationDate As Date,
                              ByVal PrintCardCounter As Integer,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddinstant_issuance", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("MIDRTN", MIDRTN)
            cmd.Parameters.AddWithValue("OCR", OCR)
            cmd.Parameters.AddWithValue("PrintedDate", PrintedDate)
            cmd.Parameters.AddWithValue("PrintCounter", PrintCounter)
            cmd.Parameters.AddWithValue("PrinterSerial", PrinterSerial)
            cmd.Parameters.AddWithValue("ApplicationDate", ApplicationDate)
            cmd.Parameters.AddWithValue("PrintCardCounter", PrintCardCounter)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            'myTrans.Commit()

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            'myTrans.Rollback()
            Return False
        End Try
    End Function

    Public Function AddPhoto(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal fld_Photo As Byte(),
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddPhoto", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("fld_Photo", fld_Photo)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message

            Return False
        End Try
    End Function

    Public Function AddCard(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal CardNo As String,
                              ByVal CardBin As String,
                              ByVal ExpiryDate As Date,
                              ByVal BarcodeNumber As String,
                              ByVal CardStatus As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddCard", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("CardNo", CardNo)
            cmd.Parameters.AddWithValue("CardBin", CardBin)
            cmd.Parameters.AddWithValue("ExpiryDate", ExpiryDate)
            cmd.Parameters.AddWithValue("BarcodeNumber", BarcodeNumber)
            cmd.Parameters.AddWithValue("CardStatus", CardStatus)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message

            Return False
        End Try
    End Function

    Public Function AddEmploymentHistory(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal History_EmployerName As String,
                              ByVal History_EmployerAddress As String,
                              ByVal History_DateEmployed As String,
                              ByVal History_DateSeparated As String,
                              ByVal Office_Assignment As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddEmploymentHistory", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("History_EmployerName", History_EmployerName)
            cmd.Parameters.AddWithValue("History_EmployerAddress", History_EmployerAddress)
            cmd.Parameters.AddWithValue("History_DateEmployed", History_DateEmployed)
            cmd.Parameters.AddWithValue("History_DateSeparated", History_DateSeparated)
            cmd.Parameters.AddWithValue("Office_Assignment", Office_Assignment)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            'myTrans.Rollback()
            Return False
        End Try
    End Function

    Public Function AddPhotoValidID(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal fld_PhotoID As Byte(),
                              ByVal fld_IDType As String,
                              ByVal fld_IDNumber As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddPhotoValidID", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("fld_PhotoID", fld_PhotoID)
            cmd.Parameters.AddWithValue("fld_IDType", fld_IDType)
            cmd.Parameters.AddWithValue("fld_IDNumber", fld_IDNumber)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddDCS_Card_Account(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal BankCode As String,
                              ByVal CardNo As String,
                              ByVal AccountNumber As String,
                              ByVal EntryUsername As String,
                              ByVal LastUpdateUserName As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddDCS_Card_Account", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("BankCode", BankCode)
            cmd.Parameters.AddWithValue("CardNo", CardNo)
            cmd.Parameters.AddWithValue("AccountNumber", AccountNumber)
            cmd.Parameters.AddWithValue("EntryUsername", EntryUsername)
            cmd.Parameters.AddWithValue("LastUpdateUserName", LastUpdateUserName)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddDCS_Card_Reprint(ByVal RefNum As String,
                              ByVal NewCardNo As String,
                              ByVal OldCardNo As String,
                              ByVal Remarks As String,
                              ByVal Username As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddDCS_Card_Reprint", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("NewCardNo", NewCardNo)
            cmd.Parameters.AddWithValue("OldCardNo", OldCardNo)
            cmd.Parameters.AddWithValue("Remarks", Remarks)
            cmd.Parameters.AddWithValue("Username", Username)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddSurvey(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal Home_Ownership As String,
                              ByVal Number_Years As String,
                              ByVal Future_Plan_Home As String,
                              ByVal Educational_Attainment As String,
                              ByVal Travels_Abroad As String,
                              ByVal Domestic_Travel As String,
                              ByVal Dine_Out As String,
                              ByVal Mall_Visit As String,
                              ByVal Number_Dependent_Studying As Integer,
                              ByVal Number_Vehicles As Integer,
                              ByVal Partner_Establishment As String,
                              ByVal Desired_Loan_Amount As String,
                              ByVal Afford_Monthly_Payment_Loan As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddSurvey", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("Home_Ownership", Home_Ownership)
            cmd.Parameters.AddWithValue("Number_Years", Number_Years)
            cmd.Parameters.AddWithValue("Future_Plan_Home", Future_Plan_Home)
            cmd.Parameters.AddWithValue("Educational_Attainment", Educational_Attainment)
            cmd.Parameters.AddWithValue("Travels_Abroad", Travels_Abroad)
            cmd.Parameters.AddWithValue("Domestic_Travel", Domestic_Travel)
            cmd.Parameters.AddWithValue("Dine_Out", Dine_Out)
            cmd.Parameters.AddWithValue("Mall_Visit", Mall_Visit)
            cmd.Parameters.AddWithValue("Number_Dependent_Studying", Number_Dependent_Studying)
            cmd.Parameters.AddWithValue("Number_Vehicles", Number_Vehicles)
            cmd.Parameters.AddWithValue("Partner_Establishment", Partner_Establishment)
            cmd.Parameters.AddWithValue("Desired_Loan_Amount", Desired_Loan_Amount)
            cmd.Parameters.AddWithValue("Afford_Monthly_Payment_Loan", Afford_Monthly_Payment_Loan)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddSignature(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal fld_Signature As Byte(),
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddSignature", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("fld_Signature", fld_Signature)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function
    Public Function UpdateSignature(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal fld_Signature As Byte(),
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcUpdateSignature", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("fld_Signature", fld_Signature)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function


    Public Function AddBio(ByVal RefNum As String,
                              ByVal PagIBIGID As String,
                              ByVal fld_LeftPrimaryFP_template As String,
                              ByVal fld_LeftPrimaryFP_IsOverride As Integer,
                              ByVal fld_LeftPrimaryFP_Ansi As Byte(),
                              ByVal fld_LeftPrimaryFP_Wsq As Byte(),
                              ByVal fld_LeftSecondaryFP_template As String,
                              ByVal fld_LeftSecondaryFP_IsOverride As Integer,
                              ByVal fld_LeftSecondaryFP_Ansi As Byte(),
                              ByVal fld_LeftSecondaryFP_Wsq As Byte(),
                              ByVal fld_RightPrimaryFP_template As String,
                              ByVal fld_RightPrimaryFP_IsOverride As Integer,
                              ByVal fld_RightPrimaryFP_Ansi As Byte(),
                              ByVal fld_RightPrimaryFP_Wsq As Byte(),
                              ByVal fld_RightSecondaryFP_template As String,
                              ByVal fld_RightSecondaryFP_IsOverride As Integer,
                              ByVal fld_RightSecondaryFP_Ansi As Byte(),
                              ByVal fld_RightSecondaryFP_Wsq As Byte(),
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddBio", con)

            cmd.Parameters.AddWithValue("RefNum", RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", PagIBIGID)
            cmd.Parameters.AddWithValue("fld_LeftPrimaryFP_template", fld_LeftPrimaryFP_template)
            cmd.Parameters.AddWithValue("fld_LeftPrimaryFP_IsOverride", fld_LeftPrimaryFP_IsOverride)
            cmd.Parameters.AddWithValue("fld_LeftPrimaryFP_Ansi", fld_LeftPrimaryFP_Ansi)
            cmd.Parameters.AddWithValue("fld_LeftPrimaryFP_Wsq", fld_LeftPrimaryFP_Wsq)
            cmd.Parameters.AddWithValue("fld_LeftSecondaryFP_template", fld_LeftSecondaryFP_template)
            cmd.Parameters.AddWithValue("fld_LeftSecondaryFP_IsOverride", fld_LeftSecondaryFP_IsOverride)
            cmd.Parameters.AddWithValue("fld_LeftSecondaryFP_Ansi", fld_LeftSecondaryFP_Ansi)
            cmd.Parameters.AddWithValue("fld_LeftSecondaryFP_Wsq", fld_LeftSecondaryFP_Wsq)
            cmd.Parameters.AddWithValue("fld_RightPrimaryFP_template", fld_RightPrimaryFP_template)
            cmd.Parameters.AddWithValue("fld_RightPrimaryFP_IsOverride", fld_RightPrimaryFP_IsOverride)
            cmd.Parameters.AddWithValue("fld_RightPrimaryFP_Ansi", fld_RightPrimaryFP_Ansi)
            cmd.Parameters.AddWithValue("fld_RightPrimaryFP_Wsq", fld_RightPrimaryFP_Wsq)
            cmd.Parameters.AddWithValue("fld_RightSecondaryFP_template", fld_RightSecondaryFP_template)
            cmd.Parameters.AddWithValue("fld_RightSecondaryFP_IsOverride", fld_RightSecondaryFP_IsOverride)
            cmd.Parameters.AddWithValue("fld_RightSecondaryFP_Ansi", fld_RightSecondaryFP_Ansi)
            cmd.Parameters.AddWithValue("fld_RightSecondaryFP_Wsq", fld_RightSecondaryFP_Wsq)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ExecuteScalar(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function
    'FOR CENTRALIZED DATABASE
    'Public Function ExecuteScalarCentralized(ByVal strQuery As String) As Boolean
    '    Try
    '        OpenConnectionCentralizeds()
    '        cmd = New SqlCommand(strQuery, con)

    '        _ExecuteScalar(CommandType.Text)

    '        Return True
    '    Catch ex As Exception
    '        strErrorMessage = ex.Message
    '        Return False
    '    End Try
    'End Function

    Public Function GetTotalCapturedByUserAndDate(ByVal UserName As String, ByVal ApplicationDate As Date,
                                                  Optional IsGetRecaptured As Boolean = False) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("SELECT ISNULL(COUNT(PagIBIGID),0) FROM tbl_Member WHERE UserName=@UserName AND ApplicationDate=@ApplicationDate AND Application_Remarks ") & IIf(IsGetRecaptured, " LIKE '%Re-card%'", " NOT LIKE '%Re-card%'"), con)
            cmd.Parameters.AddWithValue("UserName", UserName)
            cmd.Parameters.AddWithValue("ApplicationDate", ApplicationDate)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetTotalCapturedByUser(ByVal UserName As String,
                                                  Optional IsGetRecaptured As Boolean = False) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("SELECT ISNULL(COUNT(PagIBIGID),0) FROM tbl_Member WHERE UserName=@UserName AND Application_Remarks ") & IIf(IsGetRecaptured, " LIKE '%Re-card%'", " NOT LIKE '%Re-card%'"), con)
            cmd.Parameters.AddWithValue("UserName", UserName)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetTotalCapturedByKioskAndDate(ByVal KioskID As String, ByVal ApplicationDate As Date,
                                                  Optional IsGetRecaptured As Boolean = False) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("SELECT ISNULL(COUNT(PagIBIGID),0) FROM tbl_Member WHERE KioskID=@KioskID AND ApplicationDate=@ApplicationDate AND Application_Remarks ") & IIf(IsGetRecaptured, " LIKE '%Re-card%'", " NOT LIKE '%Re-card%'"), con)
            cmd.Parameters.AddWithValue("KioskID", KioskID)
            cmd.Parameters.AddWithValue("ApplicationDate", ApplicationDate)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function ValidateUserAndAccess(ByVal userName As String, ByVal userPassword As String,
                                          ByVal kioskID As String, ByVal terminalMAC As String,
                                          ByVal requestingBranch As String) As Boolean
        Try
            'OpenConnectionCentralizeds()
            'cmd = New SqlCommand("new stored procedure for centralized database", con)
            'cmd.Parameters.AddWithValue("Username", userName)
            'cmd.Parameters.AddWithValue("UserPass", userPassword)
            'cmd.Parameters.AddWithValue("KioskID", kioskID)
            'cmd.Parameters.AddWithValue("TerminalMAC", terminalMAC)
            'cmd.Parameters.AddWithValue("RequestingBranch", requestingBranch)
            'cmd.Parameters.AddWithValue("BankID", My.Settings.BankID)

            '_ExecuteScalar(CommandType.StoredProcedure)

            OpenConnection()
            cmd = New SqlCommand("prcValidateUserAndAccess", con)
            cmd.Parameters.AddWithValue("Username", userName)
            cmd.Parameters.AddWithValue("UserPass", userPassword)
            cmd.Parameters.AddWithValue("KioskID", kioskID)
            cmd.Parameters.AddWithValue("TerminalMAC", terminalMAC)
            cmd.Parameters.AddWithValue("RequestingBranch", requestingBranch)

            _ExecuteScalar(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectQuery(ByVal strQuery As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(strQuery, con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectQueryCentralized(ByVal strQuery As String) As Boolean
        Try
            OpenConnectionCentralizeds()
            cmd = New SqlCommand(strQuery, con)

            FillDataAdapter(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUserEmployerBranch(ByVal UserID As Integer) As Boolean
        Try
            'OpenConnectionCentralizeds()
            'cmd = New SqlCommand("new stored procedure for centralized database", con)
            'cmd.Parameters.AddWithValue("UserID", UserID)

            'FillDataAdapter(CommandType.StoredProcedure)

            OpenConnection()
            cmd = New SqlCommand("prcSelectUserEmployerBranch", con)
            cmd.Parameters.AddWithValue("UserID", UserID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SelectUserBank(ByVal UserID As Integer) As Boolean
        Try
            'OpenConnectionCentralizeds()
            'cmd = New SqlCommand("new stored procedure for centralized database", con)
            'cmd.Parameters.AddWithValue("UserID", UserID)

            'FillDataAdapter(CommandType.StoredProcedure)

            OpenConnection()
            cmd = New SqlCommand("prcSelectUserBank", con)
            cmd.Parameters.AddWithValue("UserID", UserID)

            FillDataAdapter(CommandType.StoredProcedure)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function


    Public Function AddCardTransaction(
                              ByVal Username As String,
                              ByVal BranchCode As String,
                              ByVal KioskID As String,
                              ByVal TransactionTypeID As String,
                              ByVal Quantity As Decimal,
                              ByVal Remarks As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddCardTransaction", con)

            cmd.Parameters.AddWithValue("Username", Username)
            cmd.Parameters.AddWithValue("BranchCode", BranchCode)
            cmd.Parameters.AddWithValue("KioskId", KioskID)
            cmd.Parameters.AddWithValue("TransactionTypeID", TransactionTypeID)
            cmd.Parameters.AddWithValue("Quantity", Quantity)
            cmd.Parameters.AddWithValue("Remarks", Remarks)

            ExecuteQueryWithResponse(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetSpoiledCardCountByDateAndBranchCode(ByVal dateValue As DateTime, ByVal BranchCode As String) As Boolean
        Try
            OpenConnection()

            Dim dateFrom = New DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 0, 0, 0)
            Dim dateTo = New DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 23, 59, 59)

            cmd = New SqlCommand(String.Format("SELECT ISNULL(SUM(Quantity),0) FROM tbl_DCS_Card_Transaction WHERE TransactionDate BETWEEN '{0}' AND '{1}' AND BranchCode = '{2}' AND TransactionTypeID='03'", dateFrom, dateTo, BranchCode), con)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetTotalBranchCards(ByVal BranchCode As String) As Boolean
        Try
            OpenConnection()
            cmd = New SqlCommand(String.Format("EXEC prcSelectBranchCardBalance @BranchCode = {0}", BranchCode), con)

            _ExecuteScalar(CommandType.Text)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function


    Public Function AddCardSpoiled(
                              ByVal cardSpoiled As DCS_Card_Spoiled,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcAddCardSpoiled", con)
            cmd.Parameters.AddWithValue("TransactionNo", cardSpoiled.TransactionNo)
            cmd.Parameters.AddWithValue("TransactionDate", cardSpoiled.TransactionDate)
            cmd.Parameters.AddWithValue("BranchCode", cardSpoiled.BranchCode)
            cmd.Parameters.AddWithValue("CardNumber", cardSpoiled.CardNumber)
            cmd.Parameters.AddWithValue("Username", cardSpoiled.Username)
            cmd.Parameters.AddWithValue("Remarks", cardSpoiled.Remarks)

            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SaveDCSUpload(
                              ByVal dcsUpload As DCS_Upload,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()

            cmd = New SqlCommand("prcSaveDCS_Upload", con)

            cmd.Parameters.AddWithValue("Status", dcsUpload.Status)
            cmd.Parameters.AddWithValue("RefNum", dcsUpload.RefNum)
            cmd.Parameters.AddWithValue("PagIBIGID", dcsUpload.PagIBIGID)
            cmd.Parameters.AddWithValue("IsPushCardInfo", dcsUpload.IsPushCardInfo)
            cmd.Parameters.AddWithValue("PushCardInfoDate", IIf(dcsUpload.PushCardInfoDate = DateTime.MinValue, DBNull.Value, dcsUpload.PushCardInfoDate))
            cmd.Parameters.AddWithValue("IsPackUpData", dcsUpload.IsPackUpData)
            cmd.Parameters.AddWithValue("PackUpDataDate", IIf(dcsUpload.PackUpDataDate = DateTime.MinValue, DBNull.Value, dcsUpload.PackUpDataDate))
            cmd.Parameters.AddWithValue("Remarks", dcsUpload.Remarks)
            cmd.Parameters.AddWithValue("Username", dcsUpload.Username)

            ExecuteQueryWithResponse(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function AddDepositTransaction(
                              ByVal depositTransactions As DepositTransaction,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnectionCentralizeds()

            cmd = New SqlCommand("spSaveDepositTransaction", con)
            cmd.Parameters.AddWithValue("BranchCode", depositTransactions.BranchCode)
            cmd.Parameters.AddWithValue("KioskID", depositTransactions.KioskID)
            cmd.Parameters.AddWithValue("DepositBankID", depositTransactions.DepositBankID)
            cmd.Parameters.AddWithValue("Amount", depositTransactions.Amount)
            cmd.Parameters.AddWithValue("ReferenceNo", depositTransactions.ReferenceNo)
            cmd.Parameters.AddWithValue("DepositBy", depositTransactions.DepositBy)
            cmd.Parameters.AddWithValue("DepositDate", depositTransactions.DepositDate)
            cmd.Parameters.AddWithValue("TransactionType", depositTransactions.TransactionType)
            cmd.Parameters.AddWithValue("TransactionDate", depositTransactions.TransactionDate)
            cmd.Parameters.AddWithValue("BankID", depositTransactions.BankID)
            cmd.Parameters.AddWithValue("RequestedDate", depositTransactions.RequestedDate)
            cmd.Parameters.AddWithValue("RequestedBy", depositTransactions.RequestedBy)
            cmd.Parameters.AddWithValue("DepositImage", depositTransactions.DepositImagePath)
            cmd.Parameters.AddWithValue("Remarks", depositTransactions.Remarks)


            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetDepositTotal(
                              ByVal branchCode As String,
                              ByVal transactionDate As DateTime,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnectionCentralizeds()

            cmd = New SqlCommand("spGetDepositTotalByBranchAndTransactionDate", con)
            cmd.Parameters.AddWithValue("BranchCode", branchCode)
            cmd.Parameters.AddWithValue("TransactionDate", transactionDate)


            ExecuteQueryWithResponse(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetDepositBankAccount(
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnectionCentralizeds()

            cmd = New SqlCommand("spGet_DepositBankAccount", con)
            ExecuteQueryWithResponse(CommandType.StoredProcedure, myTrans)
            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            CloseConnection()

        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Function SaveKioskVersion(
                              ByVal kioskID As String,
                              ByVal version As String,
                              ByVal anyDesk As String,
                              ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnection()
            cmd = New SqlCommand("spSave_KioskVersion", con)
            cmd.Parameters.AddWithValue("pID", kioskID)
            cmd.Parameters.AddWithValue("pVersion", version)
            cmd.Parameters.AddWithValue("@pAnyDesk", anyDesk)
            ExecuteQueryWithResponse(CommandType.StoredProcedure, myTrans)
            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function SaveCashOnHand(
                             ByVal cashOnHand As CashOnHand,
                             ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean

        Try
            OpenConnectionCentralizeds()

            cmd = New SqlCommand("spSave_CashOnHand", con)
            cmd.Parameters.AddWithValue("Status", cashOnHand.Status)
            cmd.Parameters.AddWithValue("ID", cashOnHand.ID)
            cmd.Parameters.AddWithValue("BankID", cashOnHand.BankID)
            cmd.Parameters.AddWithValue("BranchCode", cashOnHand.BranchCode)
            cmd.Parameters.AddWithValue("KioskID", cashOnHand.KioskID)
            cmd.Parameters.AddWithValue("Amount", cashOnHand.Amount)
            cmd.Parameters.AddWithValue("RequestedBy", cashOnHand.RequestedBy)
            cmd.Parameters.AddWithValue("Remarks", cashOnHand.Remarks)
            ExecuteNonQueryWithSqlTrans(CommandType.StoredProcedure, myTrans)

            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
            Return False
        End Try
    End Function


    Public Function GetUserRoleByUsernamePassword(ByVal username As String, ByVal passowrd As String) As Boolean

        Try
            Dim dt As DataTable = Nothing
            SelectQueryCentralized(String.Format("EXEC spGet_UserRoleByUsernamePassword @pUserName = '{0}' , @pPassWord = '{1}'", username, PHASE3.PHASE3.Encrypt(passowrd)))
            Return True
        Catch ex As Exception
            strErrorMessage = ex.Message
        End Try
        Return False
    End Function

End Class
