
Namespace PHASE3

    Public Class LogTrace

        Private pIndex As Integer = 0
        Public Property Index As Integer
            Get
                Return pIndex
            End Get
            Set(ByVal value As Integer)
                pIndex = value
            End Set
        End Property

        Private oTimestamp As String = ""
        Public Property Timestamp As String
            Get
                Return oTimestamp
            End Get
            Set(ByVal value As String)
                oTimestamp = value
            End Set
        End Property

        Private pLog As String = ""
        Public Property Log As String
            Get
                Return pLog
            End Get
            Set(ByVal value As String)
                pLog = value
            End Set
        End Property

    End Class

End Namespace


