
Namespace PHASE3

    Public Class DCS_Card_ReprintList

        Private pDCS_Card_ReprintList As List(Of DCS_Card_Reprint) = Nothing
        Public Property DCS_Card_ReprintList As List(Of DCS_Card_Reprint)
            Get
                Return pDCS_Card_ReprintList
            End Get
            Set(ByVal value As List(Of DCS_Card_Reprint))
                pDCS_Card_ReprintList = value
            End Set
        End Property

    End Class

End Namespace


