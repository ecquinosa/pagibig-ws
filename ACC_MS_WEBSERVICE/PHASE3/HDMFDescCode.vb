
Namespace PHASE3

    Public Class HDMFDescCode

        Private pCode As String = ""
        Public Property Code As String
            Get
                Return pCode
            End Get
            Set(ByVal value As String)
                pCode = value
            End Set
        End Property

        Private pDescription As String = ""
        Public Property Description As String
            Get
                Return pDescription
            End Get
            Set(ByVal value As String)
                pDescription = value
            End Set
        End Property

    End Class

End Namespace


