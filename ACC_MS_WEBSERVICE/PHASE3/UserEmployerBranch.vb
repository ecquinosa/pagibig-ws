
Namespace PHASE3

    Public Class UserEmployerBranch

        Private pEmployerID As Integer = 0
        Public Property EmployerID As Integer
            Get
                Return pEmployerID
            End Get
            Set(ByVal value As Integer)
                pEmployerID = value
            End Set
        End Property

        Private pEmployerName As String = ""
        Public Property EmployerName As String
            Get
                Return pEmployerName
            End Get
            Set(ByVal value As String)
                pEmployerName = value
            End Set
        End Property

        Private pEmployerBranchID As Integer = 0
        Public Property EmployerBranchID As Integer
            Get
                Return pEmployerBranchID
            End Get
            Set(ByVal value As Integer)
                pEmployerBranchID = value
            End Set
        End Property

        Private pEmployerBranch As String = ""
        Public Property EmployerBranch As String
            Get
                Return pEmployerBranch
            End Get
            Set(ByVal value As String)
                pEmployerBranch = value
            End Set
        End Property

    End Class

End Namespace


