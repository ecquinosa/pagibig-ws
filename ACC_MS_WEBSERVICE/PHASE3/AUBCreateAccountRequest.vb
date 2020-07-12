
Namespace PHASE3

    Public Class AUBCreateAccountRequest

        Private pexp As String = PHASE3.ToHubSpotTimestamp(Now)
        Public Property exp As String
            Get
                Return pexp
            End Get
            Set(ByVal value As String)
                pexp = value
            End Set
        End Property

        Private paud As String = "cashcard"
        Public Property aud As String
            Get
                Return paud
            End Get
            Set(ByVal value As String)
                paud = value
            End Set
        End Property

        Private prequest_sub As String = "create account"
        Public Property request_sub As String
            Get
                Return prequest_sub
            End Get
            Set(ByVal value As String)
                prequest_sub = value
            End Set
        End Property

        Private pjti As String = Guid.NewGuid.ToString()
        Public Property jti As String
            Get
                Return pjti
            End Get
            Set(ByVal value As String)
                pjti = value
            End Set
        End Property

        Private pcashCardApplication As cashCardApplication = Nothing
        Public Property cashCardApplication As cashCardApplication
            Get
                Return pcashCardApplication
            End Get
            Set(ByVal value As cashCardApplication)
                pcashCardApplication = value
            End Set
        End Property

    End Class

    Public Class cashCardApplication

        Private pcardNo As String = ""
        Public Property cardNo As String
            Get
                Return pcardNo
            End Get
            Set(ByVal value As String)
                pcardNo = value
            End Set
        End Property

        Private pcardExpiry As String = ""
        Public Property cardExpiry As String
            Get
                Return pcardExpiry
            End Get
            Set(ByVal value As String)
                pcardExpiry = value
            End Set
        End Property

        Private pclientInfo As clientInfo = Nothing
        Public Property clientInfo As clientInfo
            Get
                Return pclientInfo
            End Get
            Set(ByVal value As clientInfo)
                pclientInfo = value
            End Set
        End Property

        Private premarks As String = ""
        Public Property remarks As String
            Get
                Return premarks
            End Get
            Set(ByVal value As String)
                premarks = value
            End Set
        End Property

        Private pacctOpeningMethod As String = "RC"
        Public Property acctOpeningMethod As String
            Get
                Return pacctOpeningMethod
            End Get
            Set(ByVal value As String)
                pacctOpeningMethod = value
            End Set
        End Property

        Private pproductsAndServices As String = "10001001101000000000"
        Public Property productsAndServices As String
            Get
                Return pproductsAndServices
            End Get
            Set(ByVal value As String)
                pproductsAndServices = value
            End Set
        End Property

        Private pmonthlyVolumeOfTxns As String = "100"
        Public Property monthlyVolumeOfTxns As String
            Get
                Return pmonthlyVolumeOfTxns
            End Get
            Set(ByVal value As String)
                pmonthlyVolumeOfTxns = value
            End Set
        End Property

    End Class

    Public Class applicantName

        Private plastName As String = ""
        Public Property lastName As String
            Get
                Return plastName
            End Get
            Set(ByVal value As String)
                plastName = value
            End Set
        End Property

        Private pfirstName As String = ""
        Public Property firstName As String
            Get
                Return pfirstName
            End Get
            Set(ByVal value As String)
                pfirstName = value
            End Set
        End Property

        Private pmiddleName As String = ""
        Public Property middleName As String
            Get
                Return pmiddleName
            End Get
            Set(ByVal value As String)
                pmiddleName = value
            End Set
        End Property

        Private psuffixName As String = ""
        Public Property suffixName As String
            Get
                Return psuffixName
            End Get
            Set(ByVal value As String)
                psuffixName = value
            End Set
        End Property

    End Class

    Public Class clientInfo

        Private papplicantName As applicantName = Nothing
        Public Property applicantName As applicantName
            Get
                Return papplicantName
            End Get
            Set(ByVal value As applicantName)
                papplicantName = value
            End Set
        End Property

        Private pmotherLastName As String = ""
        Public Property motherLastName As String
            Get
                Return pmotherLastName
            End Get
            Set(ByVal value As String)
                pmotherLastName = value
            End Set
        End Property

        Private pmotherFirstName As String = ""
        Public Property motherFirstName As String
            Get
                Return pmotherFirstName
            End Get
            Set(ByVal value As String)
                pmotherFirstName = value
            End Set
        End Property

        Private pbirthDate As String = ""
        Public Property birthDate As String
            Get
                Return pbirthDate
            End Get
            Set(ByVal value As String)
                pbirthDate = value
            End Set
        End Property

        Private pbirthPlace As String = ""
        Public Property birthPlace As String
            Get
                Return pbirthPlace
            End Get
            Set(ByVal value As String)
                pbirthPlace = value
            End Set
        End Property

        Private psourceFunds As String = ""
        Public Property sourceFunds As String
            Get
                Return psourceFunds
            End Get
            Set(ByVal value As String)
                psourceFunds = value
            End Set
        End Property

        Private pnationality As String = "608"
        Public Property nationality As String
            Get
                Return pnationality
            End Get
            Set(ByVal value As String)
                pnationality = value
            End Set
        End Property

        Private psex As String = ""
        Public Property sex As String
            Get
                Return psex
            End Get
            Set(ByVal value As String)
                psex = value
            End Set
        End Property

        Private pcivilStatus As String = ""
        Public Property civilStatus As String
            Get
                Return pcivilStatus
            End Get
            Set(ByVal value As String)
                pcivilStatus = value
            End Set
        End Property

        Private phomeAddress As homeAddress = Nothing
        Public Property homeAddress As homeAddress
            Get
                Return phomeAddress
            End Get
            Set(ByVal value As homeAddress)
                phomeAddress = value
            End Set
        End Property

        Private paub_permanentAddress As aub_permanentAddress = Nothing
        Public Property permanentAddress As aub_permanentAddress
            Get
                Return paub_permanentAddress
            End Get
            Set(ByVal value As aub_permanentAddress)
                paub_permanentAddress = value
            End Set
        End Property

        Private phomePhoneNo As String = ""
        Public Property homePhoneNo As String
            Get
                Return phomePhoneNo
            End Get
            Set(ByVal value As String)
                phomePhoneNo = value
            End Set
        End Property

        Private pmobileNo As String = ""
        Public Property mobileNo As String
            Get
                Return pmobileNo
            End Get
            Set(ByVal value As String)
                pmobileNo = value
            End Set
        End Property

        Private pemail As String = ""
        Public Property email As String
            Get
                Return pemail
            End Get
            Set(ByVal value As String)
                pemail = value
            End Set
        End Property

        Private pemployerName As String = ""
        Public Property employerName As String
            Get
                Return pemployerName
            End Get
            Set(ByVal value As String)
                pemployerName = value
            End Set
        End Property

        Private pempAddress As String = ""
        Public Property empAddress As String
            Get
                Return pempAddress
            End Get
            Set(ByVal value As String)
                pempAddress = value
            End Set
        End Property

        Private pempPhoneNo As String = ""
        Public Property empPhoneNo As String
            Get
                Return pempPhoneNo
            End Get
            Set(ByVal value As String)
                pempPhoneNo = value
            End Set
        End Property

        Private pidType1 As String = "ID28"
        Public Property idType1 As String
            Get
                Return pidType1
            End Get
            Set(ByVal value As String)
                pidType1 = value
            End Set
        End Property

        Private pidNo1 As String = ""
        Public Property idNo1 As String
            Get
                Return pidNo1
            End Get
            Set(ByVal value As String)
                pidNo1 = value
            End Set
        End Property

        Private pidType2 As String = ""
        Public Property idType2 As String
            Get
                Return pidType2
            End Get
            Set(ByVal value As String)
                pidType2 = value
            End Set
        End Property

        Private pidNo2 As String = ""
        Public Property idNo2 As String
            Get
                Return pidNo2
            End Get
            Set(ByVal value As String)
                pidNo2 = value
            End Set
        End Property

        Private pidType3 As String = ""
        Public Property idType3 As String
            Get
                Return pidType3
            End Get
            Set(ByVal value As String)
                pidType3 = value
            End Set
        End Property

        Private pidNo3 As String = ""
        Public Property idNo3 As String
            Get
                Return pidNo3
            End Get
            Set(ByVal value As String)
                pidNo3 = value
            End Set
        End Property

        Private pnatureOfBusiness As String = ""
        Public Property natureOfBusiness As String
            Get
                Return pnatureOfBusiness
            End Get
            Set(ByVal value As String)
                pnatureOfBusiness = value
            End Set
        End Property

    End Class

    Public Class address


        Private pstreet As String = ""
        Public Property street As String
            Get
                Return pstreet
            End Get
            Set(ByVal value As String)
                pstreet = value
            End Set
        End Property

        Private pbrgy As String = ""
        Public Property brgy As String
            Get
                Return pbrgy
            End Get
            Set(ByVal value As String)
                pbrgy = value
            End Set
        End Property

        Private pcity As String = ""
        Public Property city As String
            Get
                Return pcity
            End Get
            Set(ByVal value As String)
                pcity = value
            End Set
        End Property

        Private pprovince As String = ""
        Public Property province As String
            Get
                Return pprovince
            End Get
            Set(ByVal value As String)
                pprovince = value
            End Set
        End Property

        Private pprovinceDesc As String = ""
        Public Property provinceDesc As String
            Get
                Return pprovinceDesc
            End Get
            Set(ByVal value As String)
                pprovinceDesc = value
            End Set
        End Property

        Private pcountry As String = "608"
        Public Property country As String
            Get
                Return pcountry
            End Get
            Set(ByVal value As String)
                pcountry = value
            End Set
        End Property

        Private pzipCode As String = ""
        Public Property zipCode As String
            Get
                Return pzipCode
            End Get
            Set(ByVal value As String)
                pzipCode = value
            End Set
        End Property

    End Class

    Public Class homeAddress

        Inherits address

    End Class

    Public Class aub_permanentAddress

        Inherits address

    End Class


End Namespace


