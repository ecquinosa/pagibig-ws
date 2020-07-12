Public Class EmploymentHistory
    Private pEmployerName As String
    Public Property EmployerName As String
        Get
            Return pEmployerName
        End Get
        Set(ByVal value As String)
            pEmployerName = value
        End Set
    End Property
    Private pEmployerAddress As String
    Public Property EmployerAddress As String
        Get
            Return pEmployerAddress
        End Get
        Set(ByVal value As String)
            pEmployerAddress = value
        End Set
    End Property
    Private pDateEmployed As String
    Public Property DateEmployed As String
        Get
            Return pDateEmployed
        End Get
        Set(ByVal value As String)
            pDateEmployed = value
        End Set
    End Property
    Private pDateSeparated As String
    Public Property DateSeparated As String
        Get
            Return pDateSeparated
        End Get
        Set(ByVal value As String)
            pDateSeparated = value
        End Set
    End Property
    Private pOfficeAssignment As String
    Public Property OfficeAssignment As String
        Get
            Return pOfficeAssignment
        End Get
        Set(ByVal value As String)
            pOfficeAssignment = value
        End Set
    End Property
    Private pCountryAssignment As String
    Public Property CountryAssignment As String
        Get
            Return pCountryAssignment
        End Get
        Set(ByVal value As String)
            pCountryAssignment = value
        End Set
    End Property
End Class
