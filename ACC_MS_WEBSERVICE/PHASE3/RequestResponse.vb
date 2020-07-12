
Namespace PHASE3

    Public Class RequestResponse

        Private pIsSuccess As Boolean = False
        Public Property IsSuccess As Boolean
            Get
                Return pIsSuccess
            End Get
            Set(ByVal value As Boolean)
                pIsSuccess = value
            End Set
        End Property

        Private pErrorMessage As String = ""
        Public Property ErrorMessage As String
            Get
                Return pErrorMessage
            End Get
            Set(ByVal value As String)
                pErrorMessage = value
            End Set
        End Property

    End Class

End Namespace


