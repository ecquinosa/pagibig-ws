
Namespace PHASE3

    Public Class SearchResult

        Private pIsGet As Boolean = False
        Public Property IsGet As Boolean
            Get
                Return pIsGet
            End Get
            Set(ByVal value As Boolean)
                pIsGet = value
            End Set
        End Property

        Private pIsComplete As Boolean = False
        Public Property IsComplete As Boolean
            Get
                Return pIsComplete
            End Get
            Set(ByVal value As Boolean)
                pIsComplete = value
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

        Private pLastName As String = ""
        Public Property LastName As String
            Get
                Return pLastName
            End Get
            Set(ByVal value As String)
                pLastName = value
            End Set
        End Property

        Private mMemberInfo As MemberInfo = Nothing
        Public Property MemberInfo As MemberInfo
            Get
                Return mMemberInfo
            End Get
            Set(ByVal value As MemberInfo)
                mMemberInfo = value
            End Set
        End Property

    End Class

End Namespace


