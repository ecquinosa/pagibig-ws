Public Class PermanentAddress

    Private pHBURNumber As String
    Public Property HBURNumber As String
        Get
            Return pHBURNumber
        End Get
        Set(ByVal value As String)
            pHBURNumber = value
        End Set
    End Property

    Private pBuilding As String
    Public Property Building As String
        Get
            Return pBuilding
        End Get
        Set(ByVal value As String)
            pBuilding = value
        End Set
    End Property

    Private pLotNo As String
    Public Property LotNo As String
        Get
            Return pLotNo
        End Get
        Set(ByVal value As String)
            pLotNo = value
        End Set
    End Property

    Private pBlockNo As String
    Public Property BlockNo As String
        Get
            Return pBlockNo
        End Get
        Set(ByVal value As String)
            pBlockNo = value
        End Set
    End Property

    Private pPhaseNo As String
    Public Property PhaseNo As String
        Get
            Return pPhaseNo
        End Get
        Set(ByVal value As String)
            pPhaseNo = value
        End Set
    End Property

    Private pHouseNo As String
    Public Property HouseNo As String
        Get
            Return pHouseNo
        End Get
        Set(ByVal value As String)
            pHouseNo = value
        End Set
    End Property

    Private pStreetName As String
    Public Property StreetName As String
        Get
            Return pStreetName
        End Get
        Set(ByVal value As String)
            pStreetName = value
        End Set
    End Property

    Private pSubdivision As String
    Public Property Subdivision As String
        Get
            Return pSubdivision
        End Get
        Set(ByVal value As String)
            pSubdivision = value
        End Set
    End Property

    Private pBarangay As String
    Public Property Barangay As String
        Get
            Return pBarangay
        End Get
        Set(ByVal value As String)
            pBarangay = value
        End Set
    End Property

    Private pCityMunicipality As String
    Public Property CityMunicipality As String
        Get
            Return pCityMunicipality
        End Get
        Set(ByVal value As String)
            pCityMunicipality = value
        End Set
    End Property

    Private pProvince As String
    Public Property Province As String
        Get
            Return pProvince
        End Get
        Set(ByVal value As String)
            pProvince = value
        End Set
    End Property

    Private pRegion As String
    Public Property Region As String
        Get
            Return pRegion
        End Get
        Set(ByVal value As String)
            pRegion = value
        End Set
    End Property

    Private pZipcode As String
    Public Property Zipcode As String
        Get
            Return pZipcode
        End Get
        Set(ByVal value As String)
            pZipcode = value
        End Set
    End Property

End Class
