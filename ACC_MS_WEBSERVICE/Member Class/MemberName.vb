Public Class MemberName

    Private pFirstName As String = ""
    Public Property FirstName As String
        Get
            Return pFirstName
        End Get
        Set(ByVal value As String)
            pFirstName = value
        End Set
    End Property

    Private pMiddleName As String = ""
    Public Property MiddleName As String
        Get
            Return pMiddleName
        End Get
        Set(ByVal value As String)
            pMiddleName = value
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

    Private pExt As String = ""
    Public Property Ext As String
        Get
            Return pExt
        End Get
        Set(ByVal value As String)
            pExt = value
        End Set
    End Property

    Private pIsNoMiddleName As Boolean
    Public Property IsNoMiddleName As Boolean
        Get
            Return pIsNoMiddleName
        End Get
        Set(ByVal value As Boolean)
            pIsNoMiddleName = value
        End Set
    End Property
End Class
