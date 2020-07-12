
Namespace PHASE3

    Public Class PushCardInfoResult

        Private pIsSuccess As Boolean = False
        Public Property IsSuccess As Boolean
            Get
                Return pIsSuccess
            End Get
            Set(ByVal value As Boolean)
                pIsSuccess = value
            End Set
        End Property

        Private pGetErrorMsg As String = ""
        Public Property GetErrorMsg As String
            Get
                Return pGetErrorMsg
            End Get
            Set(ByVal value As String)
                pGetErrorMsg = value
            End Set
        End Property

    End Class

End Namespace


