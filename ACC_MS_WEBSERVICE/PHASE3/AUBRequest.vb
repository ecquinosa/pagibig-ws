
Namespace PHASE3

    Public Class AUBRequest

        Private pjwt As String = ""
        Public Property jwt As String
            Get
                Return pjwt
            End Get
            Set(ByVal value As String)
                pjwt = value
            End Set
        End Property

        Private psystemId As String = "HDMF"
        Public Property systemId As String
            Get
                Return psystemId
            End Get
            Set(ByVal value As String)
                psystemId = value
            End Set
        End Property

        Private psystemSubPartner As String = "000"
        Public Property systemSubPartner As String
            Get
                Return psystemSubPartner
            End Get
            Set(ByVal value As String)
                psystemSubPartner = value
            End Set
        End Property

    End Class

End Namespace


