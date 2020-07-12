
Namespace PHASE3

    Public Class Signature

        Private pRefNum As String = ""
        Public Property RefNum As String
            Get
                Return pRefNum
            End Get
            Set(ByVal value As String)
                pRefNum = value
            End Set
        End Property

        Private pPagIBIGID As String = ""
        Public Property PagIBIGID As String
            Get
                Return pPagIBIGID
            End Get
            Set(ByVal value As String)
                pPagIBIGID = value
            End Set
        End Property

        Private pfld_Signature As Byte() = Nothing
        Public Property fld_Signature As Byte()
            Get
                Return pfld_Signature
            End Get
            Set(ByVal value As Byte())
                pfld_Signature = value
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
                pErrorMessage = "SaveSignatureRequest(): DAL is nothing"
                Return False
            End If
            Dim sigExist As New Signature
            If sigExist.Load(DAL, Me.RefNum) Then
                If Not DAL.UpdateSignature(pRefNum, pPagIBIGID, pfld_Signature, myTrans) Then
                    pErrorMessage = "SaveSignatureRequest(): " & DAL.ErrorMessage
                    Return False
                End If
            Else
                If Not DAL.AddSignature(pRefNum, pPagIBIGID, pfld_Signature, myTrans) Then
                    pErrorMessage = "SaveSignatureRequest(): " & DAL.ErrorMessage
                    Return False
                End If
            End If
            Return True
        End Function


        Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
            Dim dt As DataTable = Nothing
            Try

                If DAL.SelectQuery(String.Format("SELECT * FROM tbl_Signature WHERE RefNum = '{0}'", refNumber)) Then
                    dt = DAL.TableResult
                    If dt.Rows.Count > 0 Then
                        Dim value = dt.Rows(0)
                        pRefNum = refNumber
                        pPagIBIGID = value("PagIBIGID")
                        pfld_Signature = value("fld_Signature")
                        Return True
                    Else
                        pErrorMessage = "Reference Not Found."
                    End If

                Else
                    pErrorMessage = "Load Signature(): " & DAL.ErrorMessage
                End If
            Catch ex As Exception
                pErrorMessage = "Load Signature(): " & DAL.ErrorMessage
            End Try


            Return False
        End Function


    End Class

End Namespace


