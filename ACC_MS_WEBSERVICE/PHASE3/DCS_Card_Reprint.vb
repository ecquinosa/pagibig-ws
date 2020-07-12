
Namespace PHASE3

    Public Class DCS_Card_Reprint

        Private pRefNum As String = ""
        Public Property RefNum As String
            Get
                Return pRefNum
            End Get
            Set(ByVal value As String)
                pRefNum = value
            End Set
        End Property

        Private pNewCardNo As String = ""
        Public Property NewCardNo As String
            Get
                Return pNewCardNo
            End Get
            Set(ByVal value As String)
                pNewCardNo = value
            End Set
        End Property

        Private pOldCardNo As String = ""
        Public Property OldCardNo As String
            Get
                Return pOldCardNo
            End Get
            Set(ByVal value As String)
                pOldCardNo = value
            End Set
        End Property

        Private pRemarks As String = ""
        Public Property Remarks As String
            Get
                Return pRemarks
            End Get
            Set(ByVal value As String)
                pRemarks = value
            End Set
        End Property

        Private pUsername As String = ""
        Public Property Username As String
            Get
                Return pUsername
            End Get
            Set(ByVal value As String)
                pUsername = value
            End Set
        End Property

        Private pEntryDate As Date = Nothing
        Public Property EntryDate As Date
            Get
                Return pEntryDate
            End Get
            Set(ByVal value As Date)
                pEntryDate = value
            End Set
        End Property

        Private pLastUpdate As Date = Nothing
        Public Property LastUpdate As Date
            Get
                Return pLastUpdate
            End Get
            Set(ByVal value As Date)
                pLastUpdate = value
            End Set
        End Property



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

        Public Function SaveToDbase(ByVal DAL As DAL,
                                    ByRef myTrans As List(Of System.Data.SqlClient.SqlTransaction)) As Boolean
            If DAL Is Nothing Then
                pErrorMessage = "SaveDCS_Card_ReprintRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddDCS_Card_Reprint(pRefNum, pNewCardNo, pOldCardNo, pRemarks, pUsername, myTrans) Then
                pErrorMessage = "SaveDCS_Card_ReprintRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function


    End Class

End Namespace


