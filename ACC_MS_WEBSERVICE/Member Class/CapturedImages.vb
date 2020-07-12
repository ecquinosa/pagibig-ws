Public Class CapturedImages

    Private _Temporarypath As String = ""
    Public Property Temporarypath As String
        Get
            Return _Temporarypath
        End Get
        Set(ByVal value As String)
            _Temporarypath = value
        End Set
    End Property

    Private pPicture As Byte()
    Public Property Picture As Byte()
        Get
            Return pPicture
        End Get
        Set(ByVal value As Byte())
            pPicture = value
        End Set
    End Property

    Private _LP_Primary_template As String = ""
    Public Property LP_Primary_template As String
        Get
            Return _LP_Primary_template
        End Get
        Set(ByVal value As String)
            _LP_Primary_template = value
        End Set
    End Property

    Private _LP_Primary As Byte()
    Public Property LP_Primary As Byte()
        Get
            Return _LP_Primary
        End Get
        Set(ByVal value As Byte())
            _LP_Primary = value
        End Set
    End Property

    Private _LP_Primary_IsOverride As Boolean
    Public Property LP_Primary_IsOverride As Boolean
        Get
            Return _LP_Primary_IsOverride
        End Get
        Set(ByVal value As Boolean)
            _LP_Primary_IsOverride = value
        End Set
    End Property

    Private _LP_Primary_IsAmputated As Boolean
    Public Property LP_Primary_IsAmputated As Boolean
        Get
            Return _LP_Primary_IsAmputated
        End Get
        Set(ByVal value As Boolean)
            _LP_Primary_IsAmputated = value
        End Set
    End Property

    Private _LP_LeftPrimaryFP_Ansi As Byte()
    Public Property LP_LeftPrimaryFP_Ansi As Byte()
        Get
            Return _LP_LeftPrimaryFP_Ansi
        End Get
        Set(ByVal value As Byte())
            _LP_LeftPrimaryFP_Ansi = value
        End Set
    End Property

    Private _LP_LeftPrimaryFP_Wsq As Byte()
    Public Property LP_LeftPrimaryFP_Wsq As Byte()
        Get
            Return _LP_LeftPrimaryFP_Wsq
        End Get
        Set(ByVal value As Byte())
            _LP_LeftPrimaryFP_Wsq = value
        End Set
    End Property

    Private _LP_Secondary_template As String
    Public Property LP_Secondary_template As String
        Get
            Return _LP_Secondary_template
        End Get
        Set(ByVal value As String)
            _LP_Secondary_template = value
        End Set
    End Property

    Private _LP_Secondary As Byte()
    Public Property LP_Secondary As Byte()
        Get
            Return _LP_Secondary
        End Get
        Set(ByVal value As Byte())
            _LP_Secondary = value
        End Set
    End Property

    Private _LP_Secondary_IsOverride As Boolean
    Public Property LP_Secondary_IsOverride As Boolean
        Get
            Return _LP_Secondary_IsOverride
        End Get
        Set(ByVal value As Boolean)
            _LP_Secondary_IsOverride = value
        End Set
    End Property

    Private _LP_Secondary_IsAmputated As Boolean
    Public Property LP_Secondary_IsAmputated As Boolean
        Get
            Return _LP_Secondary_IsAmputated
        End Get
        Set(ByVal value As Boolean)
            _LP_Secondary_IsAmputated = value
        End Set
    End Property

    Private _LP_LeftSecondaryFP_Ansi As Byte()
    Public Property LP_LeftSecondaryFP_Ansi As Byte()
        Get
            Return _LP_LeftSecondaryFP_Ansi
        End Get
        Set(ByVal value As Byte())
            _LP_LeftSecondaryFP_Ansi = value
        End Set
    End Property

    Private _LP_LeftSecondaryFP_Wsq As Byte()
    Public Property LP_LeftSecondaryFP_Wsq As Byte()
        Get
            Return _LP_LeftSecondaryFP_Wsq
        End Get
        Set(ByVal value As Byte())
            _LP_LeftSecondaryFP_Wsq = value
        End Set
    End Property

    Private _RP_Primary_template As String
    Public Property RP_Primary_template As String
        Get
            Return _RP_Primary_template
        End Get
        Set(ByVal value As String)
            _RP_Primary_template = value
        End Set
    End Property

    Private _RP_Primary As Byte()
    Public Property RP_Primary As Byte()
        Get
            Return _RP_Primary
        End Get
        Set(ByVal value As Byte())
            _RP_Primary = value
        End Set
    End Property

    Private _RP_Primary_IsOverride As Boolean
    Public Property RP_Primary_IsOverride As Boolean
        Get
            Return _RP_Primary_IsOverride
        End Get
        Set(ByVal value As Boolean)
            _RP_Primary_IsOverride = value
        End Set
    End Property

    Private _RP_Primary_IsAmputated As Boolean
    Public Property RP_Primary_IsAmputated As Boolean
        Get
            Return _RP_Primary_IsAmputated
        End Get
        Set(ByVal value As Boolean)
            _RP_Primary_IsAmputated = value
        End Set
    End Property

    Private _RP_RightPrimaryFP_Ansi As Byte()
    Public Property RP_RightPrimaryFP_Ansi As Byte()
        Get
            Return _RP_RightPrimaryFP_Ansi
        End Get
        Set(ByVal value As Byte())
            _RP_RightPrimaryFP_Ansi = value
        End Set
    End Property

    Private _RP_RightPrimaryFP_Wsq As Byte()
    Public Property RP_RightPrimaryFP_Wsq As Byte()
        Get
            Return _RP_RightPrimaryFP_Wsq
        End Get
        Set(ByVal value As Byte())
            _RP_RightPrimaryFP_Wsq = value
        End Set
    End Property

    Private _RP_Secondary_template As String
    Public Property RP_Secondary_template As String
        Get
            Return _RP_Secondary_template
        End Get
        Set(ByVal value As String)
            _RP_Secondary_template = value
        End Set
    End Property

    Private _RP_Secondary As Byte()
    Public Property RP_Secondary As Byte()
        Get
            Return _RP_Secondary
        End Get
        Set(ByVal value As Byte())
            _RP_Secondary = value
        End Set
    End Property

    Private _RP_Secondary_IsOverride As Boolean
    Public Property RP_Secondary_IsOverride As Boolean
        Get
            Return _RP_Secondary_IsOverride
        End Get
        Set(ByVal value As Boolean)
            _RP_Secondary_IsOverride = value
        End Set
    End Property

    Private _RP_Secondary_IsAmputated As Boolean
    Public Property RP_Secondary_IsAmputated As Boolean
        Get
            Return _RP_Secondary_IsAmputated
        End Get
        Set(ByVal value As Boolean)
            _RP_Secondary_IsAmputated = value
        End Set
    End Property

    Private _RP_RightSecondaryFP_Ansi As Byte()
    Public Property RP_RightSecondaryFP_Ansi As Byte()
        Get
            Return _RP_RightSecondaryFP_Ansi
        End Get
        Set(ByVal value As Byte())
            _RP_RightSecondaryFP_Ansi = value
        End Set
    End Property

    Private _RP_RightSecondaryFP_Wsq As Byte()
    Public Property RP_RightSecondaryFP_Wsq As Byte()
        Get
            Return _RP_RightSecondaryFP_Wsq
        End Get
        Set(ByVal value As Byte())
            _RP_RightSecondaryFP_Wsq = value
        End Set
    End Property

    Private pSigniture As Byte()
    Public Property Signiture As Byte()
        Get
            Return pSigniture
        End Get
        Set(ByVal value As Byte())
            pSigniture = value
        End Set
    End Property

    Private pFingerNumber As Integer
    Public Property FingerNumber As Integer
        Get
            Return pFingerNumber
        End Get
        Set(ByVal value As Integer)
            pFingerNumber = value
        End Set
    End Property
   

End Class
