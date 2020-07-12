
Namespace PHASE3

    Public Class AUBReplaceCardRequest

        Private paud As String = "cashcard"
        Public Property aud As String
            Get
                Return paud
            End Get
            Set(ByVal value As String)
                paud = value
            End Set
        End Property

        Private pexp As String = PHASE3.ToHubSpotTimestamp(Now)
        Public Property exp As String
            Get
                Return pexp
            End Get
            Set(ByVal value As String)
                pexp = value
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

        Private preplaceCard As replaceCard = Nothing
        Public Property replaceCard As replaceCard
            Get
                Return preplaceCard
            End Get
            Set(ByVal value As replaceCard)
                preplaceCard = value
            End Set
        End Property

    End Class

    Public Class replaceCard

        Private pidNo As String = ""
        Public Property idNo As String
            Get
                Return pidNo
            End Get
            Set(ByVal value As String)
                pidNo = value
            End Set
        End Property

        Private poldCardNo As String = ""
        Public Property oldCardNo As String
            Get
                Return poldCardNo
            End Get
            Set(ByVal value As String)
                poldCardNo = value
            End Set
        End Property

        Private pnewCardNo As String = ""
        Public Property newCardNo As String
            Get
                Return pnewCardNo
            End Get
            Set(ByVal value As String)
                pnewCardNo = value
            End Set
        End Property

        Private pexpiryDate As String = ""
        Public Property expiryDate As String
            Get
                Return pexpiryDate
            End Get
            Set(ByVal value As String)
                pexpiryDate = value
            End Set
        End Property

    End Class


End Namespace


