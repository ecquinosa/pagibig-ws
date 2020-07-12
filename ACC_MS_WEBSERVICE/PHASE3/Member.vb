
Namespace PHASE3

    Public Class Member

        Private pRefNum As String = ""
        Public Property RefNum As String
            Get
                Return pRefNum
            End Get
            Set(ByVal value As String)
                pRefNum = value
            End Set
        End Property

        Private pPagIBIGID As String = ""
        Public Property PagIBIGID As String
            Get
                Return pPagIBIGID
            End Get
            Set(ByVal value As String)
                pPagIBIGID = value
            End Set
        End Property

        Private pMember_LastName As String = ""
        Public Property Member_LastName As String
            Get
                Return pMember_LastName
            End Get
            Set(ByVal value As String)
                pMember_LastName = value
            End Set
        End Property

        Private pMember_FirstName As String = ""
        Public Property Member_FirstName As String
            Get
                Return pMember_FirstName
            End Get
            Set(ByVal value As String)
                pMember_FirstName = value
            End Set
        End Property

        Private pMember_MiddleName As String = ""
        Public Property Member_MiddleName As String
            Get
                Return pMember_MiddleName
            End Get
            Set(ByVal value As String)
                pMember_MiddleName = value
            End Set
        End Property

        Private pMember_Extension As String = ""
        Public Property Member_Extension As String
            Get
                Return pMember_Extension
            End Get
            Set(ByVal value As String)
                pMember_Extension = value
            End Set
        End Property

        Private pMember_NoMiddleName As Short = 0
        Public Property Member_NoMiddleName As Short
            Get
                Return pMember_NoMiddleName
            End Get
            Set(ByVal value As Short)
                pMember_NoMiddleName = value
            End Set
        End Property

        Private pBirth_LastName As String = ""
        Public Property Birth_LastName As String
            Get
                Return pBirth_LastName
            End Get
            Set(ByVal value As String)
                pBirth_LastName = value
            End Set
        End Property

        Private pBirth_FirstName As String = ""
        Public Property Birth_FirstName As String
            Get
                Return pBirth_FirstName
            End Get
            Set(ByVal value As String)
                pBirth_FirstName = value
            End Set
        End Property

        Private pBirth_MiddleName As String = ""
        Public Property Birth_MiddleName As String
            Get
                Return pBirth_MiddleName
            End Get
            Set(ByVal value As String)
                pBirth_MiddleName = value
            End Set
        End Property

        Private pBirth_NoMiddleName As Short = 0
        Public Property Birth_NoMiddleName As Short
            Get
                Return pBirth_NoMiddleName
            End Get
            Set(ByVal value As Short)
                pBirth_NoMiddleName = value
            End Set
        End Property

        Private pBirth_Extension As String = ""
        Public Property Birth_Extension As String
            Get
                Return pBirth_Extension
            End Get
            Set(ByVal value As String)
                pBirth_Extension = value
            End Set
        End Property

        Private pMother_LastName As String = ""
        Public Property Mother_LastName As String
            Get
                Return pMother_LastName
            End Get
            Set(ByVal value As String)
                pMother_LastName = value
            End Set
        End Property

        Private pMother_FirstName As String = ""
        Public Property Mother_FirstName As String
            Get
                Return pMother_FirstName
            End Get
            Set(ByVal value As String)
                pMother_FirstName = value
            End Set
        End Property

        Private pMother_MiddleName As String = ""
        Public Property Mother_MiddleName As String
            Get
                Return pMother_MiddleName
            End Get
            Set(ByVal value As String)
                pMother_MiddleName = value
            End Set
        End Property

        Private pMother_Extension As String = ""
        Public Property Mother_Extension As String
            Get
                Return pMother_Extension
            End Get
            Set(ByVal value As String)
                pMother_Extension = value
            End Set
        End Property

        Private pMother_NoMiddleName As Short = 0
        Public Property Mother_NoMiddleName As Short
            Get
                Return pMother_NoMiddleName
            End Get
            Set(ByVal value As Short)
                pMother_NoMiddleName = value
            End Set
        End Property

        Private pSpouse_LastName As String = ""
        Public Property Spouse_LastName As String
            Get
                Return pSpouse_LastName
            End Get
            Set(ByVal value As String)
                pSpouse_LastName = value
            End Set
        End Property

        Private pSpouse_FirstName As String = ""
        Public Property Spouse_FirstName As String
            Get
                Return pSpouse_FirstName
            End Get
            Set(ByVal value As String)
                pSpouse_FirstName = value
            End Set
        End Property

        Private pSpouse_MiddleName As String = ""
        Public Property Spouse_MiddleName As String
            Get
                Return pSpouse_MiddleName
            End Get
            Set(ByVal value As String)
                pSpouse_MiddleName = value
            End Set
        End Property

        Private pSpouse_Extension As String = ""
        Public Property Spouse_Extension As String
            Get
                Return pSpouse_Extension
            End Get
            Set(ByVal value As String)
                pSpouse_Extension = value
            End Set
        End Property

        Private pSpouse_NoMiddleName As Short = 0
        Public Property Spouse_NoMiddleName As Short
            Get
                Return pSpouse_NoMiddleName
            End Get
            Set(ByVal value As Short)
                pSpouse_NoMiddleName = value
            End Set
        End Property

        Private pBirthDate As Date = Nothing
        Public Property BirthDate As Date
            Get
                Return pBirthDate
            End Get
            Set(ByVal value As Date)
                pBirthDate = value
            End Set
        End Property

        Private pBirthCity As String = ""
        Public Property BirthCity As String
            Get
                Return pBirthCity
            End Get
            Set(ByVal value As String)
                pBirthCity = value
            End Set
        End Property

        Private pBirthCountry As String = ""
        Public Property BirthCountry As String
            Get
                Return pBirthCountry
            End Get
            Set(ByVal value As String)
                pBirthCountry = value
            End Set
        End Property

        Private pGender As String = ""
        Public Property Gender As String
            Get
                Return pGender
            End Get
            Set(ByVal value As String)
                pGender = value
            End Set
        End Property

        Private pCivilStatus As String = ""
        Public Property CivilStatus As String
            Get
                Return pCivilStatus
            End Get
            Set(ByVal value As String)
                pCivilStatus = value
            End Set
        End Property

        Private pCitizenship As String = ""
        Public Property Citizenship As String
            Get
                Return pCitizenship
            End Get
            Set(ByVal value As String)
                pCitizenship = value
            End Set
        End Property

        Private pCommonRefNo As String = ""
        Public Property CommonRefNo As String
            Get
                Return pCommonRefNo
            End Get
            Set(ByVal value As String)
                pCommonRefNo = value
            End Set
        End Property

        Private pSSSID As String = ""
        Public Property SSSID As String
            Get
                Return pSSSID
            End Get
            Set(ByVal value As String)
                pSSSID = value
            End Set
        End Property

        Private pGSISID As String = ""
        Public Property GSISID As String
            Get
                Return pGSISID
            End Get
            Set(ByVal value As String)
                pGSISID = value
            End Set
        End Property

        Private pTIN As String = ""
        Public Property TIN As String
            Get
                Return pTIN
            End Get
            Set(ByVal value As String)
                pTIN = value
            End Set
        End Property

        Private pMembershipCategory As String = ""
        Public Property MembershipCategory As String
            Get
                Return pMembershipCategory
            End Get
            Set(ByVal value As String)
                pMembershipCategory = value
            End Set
        End Property

        Private pApplicationDate As Date = Nothing
        Public Property ApplicationDate As Date
            Get
                Return pApplicationDate
            End Get
            Set(ByVal value As Date)
                pApplicationDate = value
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

        Private pTransaction_Ref_No As String = ""
        Public Property Transaction_Ref_No As String
            Get
                Return pTransaction_Ref_No
            End Get
            Set(ByVal value As String)
                pTransaction_Ref_No = value
            End Set
        End Property

        Private pCapture_Type As String = ""
        Public Property Capture_Type As String
            Get
                Return pCapture_Type
            End Get
            Set(ByVal value As String)
                pCapture_Type = value
            End Set
        End Property

        Private pPFR_Number As String = ""
        Public Property PFR_Number As String
            Get
                Return pPFR_Number
            End Get
            Set(ByVal value As String)
                pPFR_Number = value
            End Set
        End Property

        Private pPFR_Amount As Decimal = 0
        Public Property PFR_Amount As Decimal
            Get
                Return pPFR_Amount
            End Get
            Set(ByVal value As Decimal)
                pPFR_Amount = value
            End Set
        End Property

        Private pPFR_Date As Date = Nothing
        Public Property PFR_Date As Date
            Get
                Return pPFR_Date
            End Get
            Set(ByVal value As Date)
                pPFR_Date = value
            End Set
        End Property

        Private pIsMemberActive As Short = 0
        Public Property IsMemberActive As Short
            Get
                Return pIsMemberActive
            End Get
            Set(ByVal value As Short)
                pIsMemberActive = value
            End Set
        End Property

        Private pIsComplete As Short = 0
        Public Property IsComplete As Short
            Get
                Return pIsComplete
            End Get
            Set(ByVal value As Short)
                pIsComplete = value
            End Set
        End Property

        Private pCard_PFRNumber As String = ""
        Public Property Card_PFRNumber As String
            Get
                Return pCard_PFRNumber
            End Get
            Set(ByVal value As String)
                pCard_PFRNumber = value
            End Set
        End Property

        Private pCard_PFRAmount As Decimal = 0
        Public Property Card_PFRAmount As Decimal
            Get
                Return pCard_PFRAmount
            End Get
            Set(ByVal value As Decimal)
                pCard_PFRAmount = value
            End Set
        End Property

        Private pCard_PFRDate As Date = Nothing
        Public Property Card_PFRDate As Date
            Get
                Return pCard_PFRDate
            End Get
            Set(ByVal value As Date)
                pCard_PFRDate = value
            End Set
        End Property

        Private pApplication_Remarks As String = ""
        Public Property Application_Remarks As String
            Get
                Return pApplication_Remarks
            End Get
            Set(ByVal value As String)
                pApplication_Remarks = value
            End Set
        End Property

        Private prequesting_branchcode As String = ""
        Public Property requesting_branchcode As String
            Get
                Return prequesting_branchcode
            End Get
            Set(ByVal value As String)
                prequesting_branchcode = value
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

        Private pUserName As String = ""
        Public Property UserName As String
            Get
                Return pUserName
            End Get
            Set(ByVal value As String)
                pUserName = value
            End Set
        End Property

        Private pDocumentSubmitted As String = ""
        Public Property DocumentSubmitted As String
            Get
                Return pDocumentSubmitted
            End Get
            Set(ByVal value As String)
                pDocumentSubmitted = value
            End Set
        End Property

        Private pPaymentStatus As String = ""
        Public Property PaymentStatus As String
            Get
                Return pPaymentStatus
            End Get
            Set(ByVal value As String)
                pPaymentStatus = value
            End Set
        End Property

        Private pBillingCtrlNum As String = ""
        Public Property BillingCtrlNum As String
            Get
                Return pBillingCtrlNum
            End Get
            Set(ByVal value As String)
                pBillingCtrlNum = value
            End Set
        End Property

        Private pPaymentRemarks As String = ""
        Public Property PaymentRemarks As String
            Get
                Return pPaymentRemarks
            End Get
            Set(ByVal value As String)
                pPaymentRemarks = value
            End Set
        End Property

        Private pCardCounter As String = ""
        Public Property CardCounter As String
            Get
                Return pCardCounter
            End Get
            Set(ByVal value As String)
                pCardCounter = value
            End Set
        End Property

        Private pCitizenshipCode As String = ""
        Public Property CitizenshipCode As String
            Get
                Return pCitizenshipCode
            End Get
            Set(ByVal value As String)
                pCitizenshipCode = value
            End Set
        End Property

        Private pEntryDate As Date = Nothing
        Public Property EntryDate As Date
            Get
                Return pEntryDate
            End Get
            Set(ByVal value As Date)
                pEntryDate = value
            End Set
        End Property

        Private pLastUpdate As Date = Nothing
        Public Property LastUpdate As Date
            Get
                Return pLastUpdate
            End Get
            Set(ByVal value As Date)
                pLastUpdate = value
            End Set
        End Property


        Private pIsSuccess As Boolean = False
        Public Property IsSuccess As Boolean
            Get
                Return pIsSuccess
            End Get
            Set(ByVal value As Boolean)
                pIsSuccess = value
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
                pErrorMessage = "SaveMemberRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddMember(pRefNum, pPagIBIGID, pMember_LastName, pMember_FirstName,
                             pMember_MiddleName, pMember_Extension, pMember_NoMiddleName, pBirth_LastName,
                             pBirth_FirstName, pBirth_MiddleName, pBirth_NoMiddleName, pBirth_Extension,
                             pMother_LastName, pMother_FirstName, pMother_MiddleName, pMother_Extension,
                             pMother_NoMiddleName, pSpouse_LastName, pSpouse_FirstName, pSpouse_MiddleName,
                             pSpouse_Extension, pSpouse_NoMiddleName, pBirthDate, pBirthCity,
                             pBirthCountry, pGender, pCivilStatus, pCitizenship,
                             pCommonRefNo, pSSSID, pGSISID, pTIN,
                             pMembershipCategory, pApplicationDate, pKioskID, pTransaction_Ref_No,
                             pCapture_Type, pPFR_Number, pPFR_Amount, pPFR_Date,
                             pIsMemberActive, pIsComplete, pCard_PFRNumber, pCard_PFRAmount,
                             pCard_PFRDate, pApplication_Remarks, prequesting_branchcode, pBranchCode,
                             pUserName, pDocumentSubmitted, pPaymentStatus, pBillingCtrlNum,
                             pPaymentRemarks, pCardCounter, pCitizenshipCode, myTrans) Then
                pErrorMessage = "SaveMemberRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function
        Public Shared Function GetByApplicationDateRange(ByVal DAL As DAL, ByVal fromDate As DateTime, ByVal toDate As DateTime) As List(Of Member)
            Dim memberList As New List(Of Member)
            Dim dt As DataTable = Nothing
            If DAL.SelectQuery(String.Format("SELECT RefNum FROM tbl_Member WHERE ApplicationDate BETWEEN '{0}' AND '{1}'", fromDate, toDate)) Then
                dt = DAL.TableResult
                If dt.Rows.Count > 0 Then
                    Dim value = dt.Rows(0)

                    For Each value In dt.Rows
                        Dim newMember As New Member
                        newMember.Load(DAL, value("RefNum"))
                        memberList.Add(newMember)
                    Next
                End If
            End If
            Return memberList
        End Function

        Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
            Dim dt As DataTable = Nothing
            Try

                If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Member WHERE RefNum = '{0}'", refNumber)) Then
                    dt = DAL.TableResult
                    If dt.Rows.Count > 0 Then
                        Dim value = dt.Rows(0)
                        pRefNum = refNumber
                        pPagIBIGID = value("PagIBIGID")
                        pMember_LastName = value("Member_LastName")
                        pMember_FirstName = value("Member_FirstName")
                        pMember_MiddleName = value("Member_MiddleName")
                        pMember_Extension = IIf(value("Member_Extension") Is DBNull.Value, String.Empty, value("Member_Extension"))
                        pMember_NoMiddleName = value("Member_NoMiddleName")
                        pBirth_LastName = value("Birth_LastName")
                        pBirth_FirstName = value("Birth_FirstName")
                        pBirth_MiddleName = value("Birth_MiddleName")
                        pBirth_NoMiddleName = value("Birth_NoMiddleName")
                        pBirth_Extension = IIf(value("Birth_Extension") Is DBNull.Value, String.Empty, value("Birth_Extension"))
                        pMother_LastName = value("Mother_LastName")
                        pMother_FirstName = value("Mother_FirstName")
                        pMother_MiddleName = value("Mother_MiddleName")
                        pMother_Extension = IIf(value("Mother_Extension") Is DBNull.Value, String.Empty, value("Mother_Extension"))
                        pMother_NoMiddleName = value("Mother_NoMiddleName")
                        pSpouse_LastName = IIf(value("Spouse_LastName") Is DBNull.Value, String.Empty, value("Spouse_LastName"))
                        pSpouse_FirstName = IIf(value("Spouse_FirstName") Is DBNull.Value, String.Empty, value("Spouse_FirstName"))
                        pSpouse_MiddleName = IIf(value("Spouse_MiddleName") Is DBNull.Value, String.Empty, value("Spouse_MiddleName"))
                        pSpouse_Extension = IIf(value("Spouse_Extension") Is DBNull.Value, String.Empty, value("Spouse_Extension"))
                        pSpouse_NoMiddleName = value("Spouse_NoMiddleName")
                        pBirthDate = value("BirthDate")
                        pBirthCity = value("BirthCity")
                        pBirthCountry = value("BirthCountry")
                        pGender = value("Gender")
                        pCivilStatus = value("CivilStatus")
                        pCitizenship = value("Citizenship")
                        pCommonRefNo = IIf(value("CommonRefNo") Is DBNull.Value, String.Empty, value("CommonRefNo"))
                        pSSSID = IIf(value("SSSID") Is DBNull.Value, String.Empty, value("SSSID"))
                        pGSISID = IIf(value("GSISID") Is DBNull.Value, String.Empty, value("GSISID"))
                        pTIN = IIf(value("TIN") Is DBNull.Value, String.Empty, value("TIN"))
                        pMembershipCategory = value("MembershipCategory")
                        pApplicationDate = value("ApplicationDate")
                        pKioskID = value("KioskID")
                        pTransaction_Ref_No = value("Transaction_Ref_No")
                        pCapture_Type = value("Capture_Type")
                        pPFR_Number = IIf(value("PFR_Number") Is DBNull.Value, String.Empty, value("PFR_Number"))
                        pPFR_Amount = value("PFR_Amount")
                        pPFR_Date = value("PFR_Date")
                        pIsMemberActive = value("IsMemberActive")
                        pIsComplete = value("IsComplete")
                        pCard_PFRNumber = value("Card_PFRNumber")
                        pCard_PFRAmount = value("Card_PFRAmount")
                        pCard_PFRDate = IIf(IsDBNull(value("Card_PFRDate")), Nothing, value("Card_PFRDate"))
                        pApplication_Remarks = value("Application_Remarks")
                        prequesting_branchcode = value("requesting_branchcode")
                        pBranchCode = value("BranchCode")
                        pUserName = value("UserName")
                        pDocumentSubmitted = IIf(value("DocumentSubmitted") Is DBNull.Value, String.Empty, value("DocumentSubmitted"))
                        pPaymentStatus = value("PaymentStatus")
                        pBillingCtrlNum = IIf(value("BillingCtrlNum") Is DBNull.Value, String.Empty, value("BillingCtrlNum"))
                        pPaymentRemarks = IIf(value("PaymentRemarks") Is DBNull.Value, String.Empty, value("PaymentRemarks"))
                        pCardCounter = value("CardCounter")
                        pCitizenshipCode = value("CitizenshipCode")
                        Return True
                    Else
                        pErrorMessage = "Reference Not Found."
                    End If

                Else
                    pErrorMessage = "LoadMember(): " & DAL.ErrorMessage
                End If
            Catch ex As Exception
                pErrorMessage = "LoadMember(): " & DAL.ErrorMessage
            End Try


            Return False
        End Function

    End Class

End Namespace


