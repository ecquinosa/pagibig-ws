
Namespace PHASE3

    Public Class MemberEmploymentHistoryList

        Private pMemberEmploymentHistoryList As List(Of MemberEmploymentHistory) = Nothing
        Public Property MemberEmploymentHistoryList As List(Of MemberEmploymentHistory)
            Get
                Return pMemberEmploymentHistoryList
            End Get
            Set(ByVal value As List(Of MemberEmploymentHistory))
                pMemberEmploymentHistoryList = value
            End Set
        End Property

    End Class

End Namespace


