
Namespace PHASE3

    Public Class MemberDetails

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

        Private pMember As Member = Nothing
        Public Property Member As Member
            Get
                Return pMember
            End Get
            Set(ByVal value As Member)
                pMember = value
            End Set
        End Property

        Private pMembershipCategoryInfo As MembershipCategoryInfo = Nothing
        Public Property MembershipCategoryInfo As MembershipCategoryInfo
            Get
                Return pMembershipCategoryInfo
            End Get
            Set(ByVal value As MembershipCategoryInfo)
                pMembershipCategoryInfo = value
            End Set
        End Property

        Private pMemberContactinfo As MemberContactinfo = Nothing
        Public Property MemberContactinfo As MemberContactinfo
            Get
                Return pMemberContactinfo
            End Get
            Set(ByVal value As MemberContactinfo)
                pMemberContactinfo = value
            End Set
        End Property

        Private pMemContribution As MemContribution = Nothing
        Public Property MemContribution As MemContribution
            Get
                Return pMemContribution
            End Get
            Set(ByVal value As MemContribution)
                pMemContribution = value
            End Set
        End Property

        Private pPhoto As Photo = Nothing
        Public Property Photo As Photo
            Get
                Return pPhoto
            End Get
            Set(ByVal value As Photo)
                pPhoto = value
            End Set
        End Property

        Private pSignature As Signature = Nothing
        Public Property Signature As Signature
            Get
                Return pSignature
            End Get
            Set(ByVal value As Signature)
                pSignature = value
            End Set
        End Property

        Private pBio As Bio = Nothing
        Public Property Bio As Bio
            Get
                Return pBio
            End Get
            Set(ByVal value As Bio)
                pBio = value
            End Set
        End Property

        Private pSurvey As Survey = Nothing
        Public Property Survey As Survey
            Get
                Return pSurvey
            End Get
            Set(ByVal value As Survey)
                pSurvey = value
            End Set
        End Property

        Private pPhotoValidID As PhotoValidID = Nothing
        Public Property PhotoValidID As PhotoValidID
            Get
                Return pPhotoValidID
            End Get
            Set(ByVal value As PhotoValidID)
                pPhotoValidID = value
            End Set
        End Property

        Private pDCS_Card_Account As DCS_Card_Account = Nothing
        Public Property DCS_Card_Account As DCS_Card_Account
            Get
                Return pDCS_Card_Account
            End Get
            Set(ByVal value As DCS_Card_Account)
                pDCS_Card_Account = value
            End Set
        End Property

        Private pInstanceIssuance As InstanceIssuance = Nothing
        Public Property InstanceIssuance As InstanceIssuance
            Get
                Return pInstanceIssuance
            End Get
            Set(ByVal value As InstanceIssuance)
                pInstanceIssuance = value
            End Set
        End Property

        Private pDCS_Card_Reprints As List(Of DCS_Card_Reprint) = Nothing
        Public Property DCS_Card_Reprints As List(Of DCS_Card_Reprint)
            Get
                Return pDCS_Card_Reprints
            End Get
            Set(ByVal value As List(Of DCS_Card_Reprint))
                pDCS_Card_Reprints = value
            End Set
        End Property

        Private pMemberEmploymentHistories As List(Of MemberEmploymentHistory) = Nothing
        Public Property MemberEmploymentHistories As List(Of MemberEmploymentHistory)
            Get
                Return pMemberEmploymentHistories
            End Get
            Set(ByVal value As List(Of MemberEmploymentHistory))
                pMemberEmploymentHistories = value
            End Set
        End Property

    End Class

End Namespace


