Public Class MembershipCategory

    Private pMembershipCategory As String = ""
    Public Property MembershipCategory As String
        Get
            Return pMembershipCategory
        End Get
        Set(ByVal value As String)
            pMembershipCategory = value
        End Set
    End Property

    Private pEmployerCategory As String = ""
    Public Property EmployerCategory As String
        Get
            Return pEmployerCategory
        End Get
        Set(ByVal value As String)
            pEmployerCategory = value
        End Set
    End Property

    Private pEmployerName As String = ""
    Public Property EmployerName As String
        Get
            Return pEmployerName
        End Get
        Set(ByVal value As String)
            pEmployerName = value
        End Set
    End Property

    Private pEmployeeID As String = ""
    Public Property EmployeeID As String
        Get
            Return pEmployeeID
        End Get
        Set(ByVal value As String)
            pEmployeeID = value
        End Set
    End Property

    Private pDateEmployed As String = ""
    Public Property DateEmployed As String
        Get
            Return pDateEmployed
        End Get
        Set(ByVal value As String)
            pDateEmployed = value
        End Set
    End Property

    Private pEmploymentStatus As String = ""
    Public Property EmploymentStatus As String
        Get
            Return pEmploymentStatus
        End Get
        Set(ByVal value As String)
            pEmploymentStatus = value
        End Set
    End Property

    Private pOccupationCode As String = ""
    Public Property OccupationCode As String
        Get
            Return pOccupationCode
        End Get
        Set(ByVal value As String)
            pOccupationCode = value
        End Set
    End Property

    Private pOccupation As String = ""
    Public Property Occupation As String
        Get
            Return pOccupation
        End Get
        Set(ByVal value As String)
            pOccupation = value
        End Set
    End Property

    Private pAFPSerialBadgeNo As String = ""
    Public Property AFPSerialBadgeNo As String
        Get
            Return pAFPSerialBadgeNo
        End Get
        Set(ByVal value As String)
            pAFPSerialBadgeNo = value
        End Set
    End Property

    Private pDepEdDivCodeStnCode As String = ""
    Public Property DepEdDivCodeStnCode As String
        Get
            Return pDepEdDivCodeStnCode
        End Get
        Set(ByVal value As String)
            pDepEdDivCodeStnCode = value
        End Set
    End Property

    Private pTypeOfWork As String = ""
    Public Property TypeOfWork As String
        Get
            Return pTypeOfWork
        End Get
        Set(ByVal value As String)
            pTypeOfWork = value
        End Set
    End Property

    Private pManningAgency As String = ""
    Public Property ManningAgency As String
        Get
            Return pManningAgency
        End Get
        Set(ByVal value As String)
            pManningAgency = value
        End Set
    End Property

    Private pCountryOfAssignment As String = ""
    Public Property CountryOfAssignment As String
        Get
            Return pCountryOfAssignment
        End Get
        Set(ByVal value As String)
            pCountryOfAssignment = value
        End Set
    End Property

    Private pOfficeAssignment As String = ""
    Public Property OfficeAssignment As String
        Get
            Return pOfficeAssignment
        End Get
        Set(ByVal value As String)
            pOfficeAssignment = value
        End Set
    End Property

    Private pMonthlyIncome As String = "0.00"
    Public Property MonthlyIncome As String
        Get
            Return pMonthlyIncome
        End Get
        Set(ByVal value As String)
            pMonthlyIncome = value
        End Set
    End Property

    Private _EmployerAddress As New EmployerAddress
    Public Property EmployerAddress As EmployerAddress
        Get
            Return _EmployerAddress
        End Get
        Set(ByVal value As EmployerAddress)
            _EmployerAddress = value
        End Set
    End Property
    Private pNatureofWork As String = ""
    Public Property NatureofWork As String
        Get
            Return pNatureofWork
        End Get
        Set(ByVal value As String)
            pNatureofWork = value
        End Set
    End Property
End Class
