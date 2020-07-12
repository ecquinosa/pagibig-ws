
Namespace PHASE3

    Public Class AUBGetCardNoRequest

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

        Private pinquiry As inquiry = Nothing
        Public Property inquiry As inquiry
            Get
                Return pinquiry
            End Get
            Set(ByVal value As inquiry)
                pinquiry = value
            End Set
        End Property

    End Class

    Public Class inquiry

        Private pidNo As String = ""
        Public Property idNo As String
            Get
                Return pidNo
            End Get
            Set(ByVal value As String)
                pidNo = value
            End Set
        End Property

        Private pname As name = Nothing
        Public Property name As name
            Get
                Return pname
            End Get
            Set(ByVal value As name)
                pname = value
            End Set
        End Property

        Private pbirthdate As String = ""
        Public Property birthdate As String
            Get
                Return pbirthdate
            End Get
            Set(ByVal value As String)
                pbirthdate = value
            End Set
        End Property

    End Class

    Public Class name

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

    End Class


End Namespace


