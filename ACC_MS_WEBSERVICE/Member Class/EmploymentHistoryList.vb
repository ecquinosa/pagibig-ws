

Public Class EmploymentHistoryList

        Private pEmploymentHistoryList As List(Of EmploymentHistory) = Nothing
        Public Property EmploymentHistoryList As List(Of EmploymentHistory)
            Get
                Return pEmploymentHistoryList
            End Get
            Set(ByVal value As List(Of EmploymentHistory))
                pEmploymentHistoryList = value
            End Set
        End Property

    End Class



