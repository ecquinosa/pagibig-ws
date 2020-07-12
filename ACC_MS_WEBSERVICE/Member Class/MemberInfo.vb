Imports System.Web.Services
Public Class MemberInfo
    Private pMemberID As String
    Public Property MemberID As String
        Get
            Return pMemberID
        End Get
        Set(ByVal value As String)
            pMemberID = value
        End Set
    End Property

    Private pMemberRTN As String
    Public Property MemberRTN As String
        Get
            Return pMemberRTN
        End Get
        Set(ByVal value As String)
            pMemberRTN = value
        End Set
    End Property

    Private _MemberName As New MemberName
    Public Property MemberName As MemberName
        Get
            Return _MemberName
        End Get
        Set(ByVal value As MemberName)
            _MemberName = value
        End Set
    End Property

    Private _BirthCertName As New BirthCertName
    Public Property BirthCertName As BirthCertName
        Get
            Return _BirthCertName
        End Get
        Set(ByVal value As BirthCertName)
            _BirthCertName = value
        End Set
    End Property

    Private _MotherName As New MotherName
    Public Property MotherName As MotherName
        Get
            Return _MotherName
        End Get
        Set(ByVal value As MotherName)
            _MotherName = value
        End Set
    End Property

    Private _FatherName As New FatherName
    Public Property FatherName As FatherName
        Get
            Return _FatherName
        End Get
        Set(ByVal value As FatherName)
            _FatherName = value
        End Set
    End Property

    Private _SpouseName As New SpouseName
    Public Property SpouseName As SpouseName
        Get
            Return _SpouseName
        End Get
        Set(ByVal value As SpouseName)
            _SpouseName = value
        End Set
    End Property

    Private pMemberBirthdate As Date
    Public Property MemberBirthdate As Date
        Get
            Return pMemberBirthdate
        End Get
        Set(ByVal value As Date)
            pMemberBirthdate = value
        End Set
    End Property

    Private pMemberPlaceOfBirth As String
    Public Property MemberPlaceOfBirth As String
        Get
            Return pMemberPlaceOfBirth
        End Get
        Set(ByVal value As String)
            pMemberPlaceOfBirth = value
        End Set
    End Property

    Private pMemberBirthPlaceCountry As String
    Public Property MemberBirthPlaceCountry As String
        Get
            Return pMemberBirthPlaceCountry
        End Get
        Set(ByVal value As String)
            pMemberBirthPlaceCountry = value
        End Set
    End Property

    Private pMemberBirthPlaceMunicipal As String
    Public Property MemberBirthPlaceMunicipal As String
        Get
            Return pMemberBirthPlaceMunicipal
        End Get
        Set(ByVal value As String)
            pMemberBirthPlaceMunicipal = value
        End Set
    End Property

    Private pMemberGender As String
    Public Property MemberGender As String
        Get
            Return pMemberGender
        End Get
        Set(ByVal value As String)
            pMemberGender = value
        End Set
    End Property

    Private pMemberCivilStatus As String
    Public Property MemberCivilStatus As String
        Get
            Return pMemberCivilStatus
        End Get
        Set(ByVal value As String)
            pMemberCivilStatus = value
        End Set
    End Property

    Private pMemberCitizenshipCode As String
    Public Property MemberCitizenshipCode As String
        Get
            Return pMemberCitizenshipCode
        End Get
        Set(ByVal value As String)
            pMemberCitizenshipCode = value
        End Set
    End Property

    Private pMemberCitizenship As String
    Public Property MemberCitizenship As String
        Get
            Return pMemberCitizenship
        End Get
        Set(ByVal value As String)
            pMemberCitizenship = value
        End Set
    End Property

    Private pMemberFacialFeatures As String
    Public Property MemberFacialFeatures As String
        Get
            Return pMemberFacialFeatures
        End Get
        Set(ByVal value As String)
            pMemberFacialFeatures = value
        End Set
    End Property

    Private pMemberEmail As String
    Public Property MemberEmail As String
        Get
            Return pMemberEmail
        End Get
        Set(ByVal value As String)
            pMemberEmail = value
        End Set
    End Property

    Private pMemberCRN As String
    Public Property MemberCRN As String
        Get
            Return pMemberCRN
        End Get
        Set(ByVal value As String)
            pMemberCRN = value
        End Set
    End Property

    Private pMemberSSS As String
    Public Property MemberSSS As String
        Get
            Return pMemberSSS
        End Get
        Set(ByVal value As String)
            pMemberSSS = value
        End Set
    End Property

    Private pMemberGSIS As String
    Public Property MemberGSIS As String
        Get
            Return pMemberGSIS
        End Get
        Set(ByVal value As String)
            pMemberGSIS = value
        End Set
    End Property

    Private pMemberTIN As String
    Public Property MemberTIN As String
        Get
            Return pMemberTIN
        End Get
        Set(ByVal value As String)
            pMemberTIN = value
        End Set
    End Property

    Private _MembershipCategory As New MembershipCategory
    Public Property MembershipCategory As MembershipCategory
        Get
            Return _MembershipCategory
        End Get
        Set(ByVal value As MembershipCategory)
            _MembershipCategory = value
        End Set
    End Property

    'Private _EmploymentHistory As New List(Of EmploymentHistory)
    'Public Property EmploymentHistory As List(Of EmploymentHistory)
    '    Get
    '        Return _EmploymentHistory
    '    End Get
    '    Set(ByVal value As List(Of EmploymentHistory))
    '        _EmploymentHistory = value
    '    End Set
    'End Property

    Private pEmploymentHistoryList As New EmploymentHistoryList
    Public Property EmploymentHistoryList As EmploymentHistoryList
        Get
            Return pEmploymentHistoryList
        End Get
        Set(ByVal value As EmploymentHistoryList)
            pEmploymentHistoryList = value
        End Set
    End Property

    Private _PermanentAddress As New PermanentAddress
    Public Property PermanentAddress As PermanentAddress
        Get
            Return _PermanentAddress
        End Get
        Set(ByVal value As PermanentAddress)
            _PermanentAddress = value
        End Set
    End Property

    Private _PresentAddress As New PresentAddress
    Public Property PresentAddress As PresentAddress
        Get
            Return _PresentAddress
        End Get
        Set(ByVal value As PresentAddress)
            _PresentAddress = value
        End Set
    End Property

    Private pMemberPrefferedMailingAddress As String = ""
    Public Property MemberPrefferedMailingAddress As String
        Get
            Return pMemberPrefferedMailingAddress
        End Get
        Set(ByVal value As String)
            pMemberPrefferedMailingAddress = value
        End Set
    End Property

    Private _HomeTelNo As New HomeTelNo
    Public Property HomeTelNo As HomeTelNo
        Get
            Return _HomeTelNo
        End Get
        Set(ByVal value As HomeTelNo)
            _HomeTelNo = value
        End Set
    End Property

    Private _MobileTelNo As New MobileTelNo
    Public Property MobileTelNo As MobileTelNo
        Get
            Return _MobileTelNo
        End Get
        Set(ByVal value As MobileTelNo)
            _MobileTelNo = value
        End Set
    End Property

    Private _BusinessDirectTelNo As New BusinessDirectTelNo
    Public Property BusinessDirectTelNo As BusinessDirectTelNo
        Get
            Return _BusinessDirectTelNo
        End Get
        Set(ByVal value As BusinessDirectTelNo)
            _BusinessDirectTelNo = value
        End Set
    End Property

    Private _BusinessTrunkTelNo As New BusinessTrunkTelNo
    Public Property BusinessTrunkTelNo As BusinessTrunkTelNo
        Get
            Return _BusinessTrunkTelNo
        End Get
        Set(ByVal value As BusinessTrunkTelNo)
            _BusinessTrunkTelNo = value
        End Set
    End Property

    Private _Beneficiaries As New List(Of Beneficiaries)
    Public Property Beneficiaries As List(Of Beneficiaries)
        Get
            Return _Beneficiaries
        End Get
        Set(ByVal value As List(Of Beneficiaries))
            _Beneficiaries = value
        End Set
    End Property

    Private _CapturedData As New CapturedImages
    Public Property CapturedData As CapturedImages
        Get
            Return _CapturedData
        End Get
        Set(ByVal value As CapturedImages)
            _CapturedData = value
        End Set
    End Property

    Private _MemberContribution As New List(Of MemberContribution)
    Public Property MemberContribution As List(Of MemberContribution)
        Get
            Return _MemberContribution
        End Get
        Set(ByVal value As List(Of MemberContribution))
            _MemberContribution = value
        End Set
    End Property

    Private _MemberOther As New MemberOther
    Public Property MemberOther As MemberOther
        Get
            Return _MemberOther
        End Get
        Set(ByVal value As MemberOther)
            _MemberOther = value
        End Set
    End Property

    Private pDateApplication As Date
    Public Property DateApplication As Date
        Get
            Return pDateApplication
        End Get
        Set(ByVal value As Date)
            pDateApplication = value
        End Set
    End Property

    Private _KioskID As String
    Public Property KioskID As String
        Get
            Return _KioskID
        End Get
        Set(ByVal value As String)
            _KioskID = value
        End Set
    End Property

    Private _IsComplete As Boolean
    Public Property IsComplete As Boolean
        Get
            Return _IsComplete
        End Get
        Set(ByVal value As Boolean)
            _IsComplete = value
        End Set
    End Property

    Private _IsMemberActive As Boolean
    Public Property IsMemberActive As Boolean
        Get
            Return _IsMemberActive
        End Get
        Set(ByVal value As Boolean)
            _IsMemberActive = value
        End Set
    End Property

    Private _BranchCode As String
    Public Property BranchCode As String
        Get
            Return _BranchCode
        End Get
        Set(ByVal value As String)
            _BranchCode = value
        End Set
    End Property

    Private _TrackingNumber As String
    Public Property TrackingNumber As String
        Get
            Return _TrackingNumber
        End Get
        Set(ByVal value As String)
            _TrackingNumber = value
        End Set
    End Property

    Private _Transaction_Ref_No As String
    Public Property Transaction_Ref_No As String
        Get
            Return _Transaction_Ref_No
        End Get
        Set(ByVal value As String)
            _Transaction_Ref_No = value
        End Set
    End Property

    Private _Capture_Type As String
    Public Property Capture_Type As String
        Get
            Return _Capture_Type
        End Get
        Set(ByVal value As String)
            _Capture_Type = value
        End Set
    End Property

    Private _EmployerName As String
    Public Property EmployerName As String
        Get
            Return _EmployerName
        End Get
        Set(ByVal value As String)
            _EmployerName = value
        End Set
    End Property

    Private _EmployerBranchAddress As String
    Public Property EmployerBranchAddress As String
        Get
            Return _EmployerBranchAddress
        End Get
        Set(ByVal value As String)
            _EmployerBranchAddress = value
        End Set
    End Property

    Private _ApplicationRemarks As String
    Public Property ApplicationRemarks As String
        Get
            Return _ApplicationRemarks
        End Get
        Set(ByVal value As String)
            _ApplicationRemarks = value
        End Set
    End Property

    Private _PFR_Number As String
    Public Property PFR_Number As String
        Get
            Return _PFR_Number
        End Get
        Set(ByVal value As String)
            _PFR_Number = value
        End Set
    End Property

    Private _PFR_Amount As Decimal
    Public Property PFR_Amount As Decimal
        Get
            Return _PFR_Amount
        End Get
        Set(ByVal value As Decimal)
            _PFR_Amount = value
        End Set
    End Property

    Private _PFR_Date As Date
    Public Property PFR_Date As Date
        Get
            Return _PFR_Date
        End Get
        Set(ByVal value As Date)
            _PFR_Date = value
        End Set
    End Property

    Private _CardPFR_Number As String
    Public Property CardPFR_Number As String
        Get
            Return _CardPFR_Number
        End Get
        Set(ByVal value As String)
            _CardPFR_Number = value
        End Set
    End Property

    Private _CardPFR_Amount As Decimal
    Public Property CardPFR_Amount As Decimal
        Get
            Return _CardPFR_Amount
        End Get
        Set(ByVal value As Decimal)
            _CardPFR_Amount = value
        End Set
    End Property

    Private _CardPFR_Date As Date
    Public Property CardPFR_Date As Date
        Get
            Return _CardPFR_Date
        End Get
        Set(ByVal value As Date)
            _CardPFR_Date = value
        End Set
    End Property

    Private _RecordUpdatedField As New RecordUpdatedFields
    Public Property RecordUpdatedField As RecordUpdatedFields
        Get
            Return _RecordUpdatedField
        End Get
        Set(ByVal value As RecordUpdatedFields)
            _RecordUpdatedField = value
        End Set
    End Property

    Private _CardExpiry As Date
    Public Property CardExpiry As Date
        Get
            Return _CardExpiry
        End Get
        Set(ByVal value As Date)
            _CardExpiry = value
        End Set
    End Property
    Private _pagibigBrachCode As String
    Public Property PagibigBranch As String
        Get
            Return _pagibigBrachCode
        End Get
        Set(ByVal value As String)
            _pagibigBrachCode = value
        End Set
    End Property
    Private _companyCode As String
    Public Property companycode As String
        Get
            Return _companyCode
        End Get
        Set(ByVal value As String)
            _companyCode = value
        End Set
    End Property

End Class
