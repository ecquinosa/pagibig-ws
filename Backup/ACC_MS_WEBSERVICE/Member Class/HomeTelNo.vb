Public Class HomeTelNo

    Private pCountryCode As String
    Public Property CountryCode As String
        Get
            Return pCountryCode
        End Get
        Set(ByVal value As String)
            pCountryCode = value
        End Set
    End Property

    Private pAreaCode As String
    Public Property AreaCode As String
        Get
            Return pAreaCode
        End Get
        Set(ByVal value As String)
            pAreaCode = value
        End Set
    End Property

    Private pTelNo As String
    Public Property TelNo As String
        Get
            Return pTelNo
        End Get
        Set(ByVal value As String)
            pTelNo = value
        End Set
    End Property

    Private pLocalNo As String
    Public Property LocalNo As String
        Get
            Return pLocalNo
        End Get
        Set(ByVal value As String)
            pLocalNo = value
        End Set
    End Property

End Class
