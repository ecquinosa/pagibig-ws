
Namespace PHASE3

    Public Class Bio

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

        Private pfld_LeftPrimaryFP_template As String = ""
        Public Property fld_LeftPrimaryFP_template As String
            Get
                Return pfld_LeftPrimaryFP_template
            End Get
            Set(ByVal value As String)
                pfld_LeftPrimaryFP_template = value
            End Set
        End Property

        Private pfld_LeftPrimaryFP_IsOverride As Integer = 0
        Public Property fld_LeftPrimaryFP_IsOverride As Integer
            Get
                Return pfld_LeftPrimaryFP_IsOverride
            End Get
            Set(ByVal value As Integer)
                pfld_LeftPrimaryFP_IsOverride = value
            End Set
        End Property

        Private pfld_LeftPrimaryFP_Ansi As Byte() = Nothing
        Public Property fld_LeftPrimaryFP_Ansi As Byte()
            Get
                Return pfld_LeftPrimaryFP_Ansi
            End Get
            Set(ByVal value As Byte())
                pfld_LeftPrimaryFP_Ansi = value
            End Set
        End Property

        Private pfld_LeftPrimaryFP_Wsq As Byte() = Nothing
        Public Property fld_LeftPrimaryFP_Wsq As Byte()
            Get
                Return pfld_LeftPrimaryFP_Wsq
            End Get
            Set(ByVal value As Byte())
                pfld_LeftPrimaryFP_Wsq = value
            End Set
        End Property

        Private pfld_LeftSecondaryFP_template As String = ""
        Public Property fld_LeftSecondaryFP_template As String
            Get
                Return pfld_LeftSecondaryFP_template
            End Get
            Set(ByVal value As String)
                pfld_LeftSecondaryFP_template = value
            End Set
        End Property

        Private pfld_LeftSecondaryFP_IsOverride As Integer = 0
        Public Property fld_LeftSecondaryFP_IsOverride As Integer
            Get
                Return pfld_LeftSecondaryFP_IsOverride
            End Get
            Set(ByVal value As Integer)
                pfld_LeftSecondaryFP_IsOverride = value
            End Set
        End Property

        Private pfld_LeftSecondaryFP_Ansi As Byte() = Nothing
        Public Property fld_LeftSecondaryFP_Ansi As Byte()
            Get
                Return pfld_LeftSecondaryFP_Ansi
            End Get
            Set(ByVal value As Byte())
                pfld_LeftSecondaryFP_Ansi = value
            End Set
        End Property

        Private pfld_LeftSecondaryFP_Wsq As Byte() = Nothing
        Public Property fld_LeftSecondaryFP_Wsq As Byte()
            Get
                Return pfld_LeftSecondaryFP_Wsq
            End Get
            Set(ByVal value As Byte())
                pfld_LeftSecondaryFP_Wsq = value
            End Set
        End Property

        Private pfld_RightPrimaryFP_template As String = ""
        Public Property fld_RightPrimaryFP_template As String
            Get
                Return pfld_RightPrimaryFP_template
            End Get
            Set(ByVal value As String)
                pfld_RightPrimaryFP_template = value
            End Set
        End Property

        Private pfld_RightPrimaryFP_IsOverride As Integer = 0
        Public Property fld_RightPrimaryFP_IsOverride As Integer
            Get
                Return pfld_RightPrimaryFP_IsOverride
            End Get
            Set(ByVal value As Integer)
                pfld_RightPrimaryFP_IsOverride = value
            End Set
        End Property

        Private pfld_RightPrimaryFP_Ansi As Byte() = Nothing
        Public Property fld_RightPrimaryFP_Ansi As Byte()
            Get
                Return pfld_RightPrimaryFP_Ansi
            End Get
            Set(ByVal value As Byte())
                pfld_RightPrimaryFP_Ansi = value
            End Set
        End Property

        Private pfld_RightPrimaryFP_Wsq As Byte() = Nothing
        Public Property fld_RightPrimaryFP_Wsq As Byte()
            Get
                Return pfld_RightPrimaryFP_Wsq
            End Get
            Set(ByVal value As Byte())
                pfld_RightPrimaryFP_Wsq = value
            End Set
        End Property

        Private pfld_RightSecondaryFP_template As String = ""
        Public Property fld_RightSecondaryFP_template As String
            Get
                Return pfld_RightSecondaryFP_template
            End Get
            Set(ByVal value As String)
                pfld_RightSecondaryFP_template = value
            End Set
        End Property

        Private pfld_RightSecondaryFP_IsOverride As Integer = 0
        Public Property fld_RightSecondaryFP_IsOverride As Integer
            Get
                Return pfld_RightSecondaryFP_IsOverride
            End Get
            Set(ByVal value As Integer)
                pfld_RightSecondaryFP_IsOverride = value
            End Set
        End Property

        Private pfld_RightSecondaryFP_Ansi As Byte() = Nothing
        Public Property fld_RightSecondaryFP_Ansi As Byte()
            Get
                Return pfld_RightSecondaryFP_Ansi
            End Get
            Set(ByVal value As Byte())
                pfld_RightSecondaryFP_Ansi = value
            End Set
        End Property

        Private pfld_RightSecondaryFP_Wsq As Byte() = Nothing
        Public Property fld_RightSecondaryFP_Wsq As Byte()
            Get
                Return pfld_RightSecondaryFP_Wsq
            End Get
            Set(ByVal value As Byte())
                pfld_RightSecondaryFP_Wsq = value
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
                pErrorMessage = "SaveBioRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddBio(pRefNum, pPagIBIGID, pfld_LeftPrimaryFP_template, pfld_LeftPrimaryFP_IsOverride,
                              pfld_LeftPrimaryFP_Ansi, pfld_LeftPrimaryFP_Wsq, pfld_LeftSecondaryFP_template,
                              pfld_LeftSecondaryFP_IsOverride, pfld_LeftSecondaryFP_Ansi, pfld_LeftSecondaryFP_Wsq,
                              pfld_RightPrimaryFP_template, pfld_RightPrimaryFP_IsOverride, pfld_RightPrimaryFP_Ansi,
                              pfld_RightPrimaryFP_Wsq, pfld_RightSecondaryFP_template, pfld_RightSecondaryFP_IsOverride,
                              pfld_RightSecondaryFP_Ansi, pfld_RightSecondaryFP_Wsq, myTrans) Then

                pErrorMessage = "SaveBioRequest(): " & DAL.ErrorMessage
                Return False
            End If

            Return True
        End Function

        Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
            Dim dt As DataTable = Nothing
            Try

                If DAL.SelectQuery(String.Format("SELECT * FROM tbl_bio WHERE RefNum = '{0}'", refNumber)) Then
                    dt = DAL.TableResult
                    If dt.Rows.Count > 0 Then
                        Dim value = dt.Rows(0)
                        pRefNum = refNumber
                        pPagIBIGID = value("PagIBIGID")
                        pfld_LeftPrimaryFP_template = value("fld_LeftPrimaryFP_template")
                        pfld_LeftPrimaryFP_IsOverride = value("fld_LeftPrimaryFP_IsOverride")
                        pfld_LeftPrimaryFP_Ansi = value("fld_LeftPrimaryFP_Ansi")
                        pfld_LeftPrimaryFP_Wsq = value("fld_LeftPrimaryFP_Wsq")
                        pfld_LeftSecondaryFP_template = value("fld_LeftSecondaryFP_template")
                        pfld_LeftSecondaryFP_IsOverride = value("fld_LeftSecondaryFP_IsOverride")
                        pfld_LeftSecondaryFP_Ansi = value("fld_LeftSecondaryFP_Ansi")
                        pfld_LeftSecondaryFP_Wsq = value("fld_LeftSecondaryFP_Wsq")
                        pfld_RightPrimaryFP_template = value("fld_RightPrimaryFP_template")
                        pfld_RightPrimaryFP_IsOverride = value("fld_RightPrimaryFP_IsOverride")
                        pfld_RightPrimaryFP_Ansi = value("fld_RightPrimaryFP_Ansi")
                        pfld_RightPrimaryFP_Wsq = value("fld_RightPrimaryFP_Wsq")
                        pfld_RightSecondaryFP_template = value("fld_RightSecondaryFP_template")
                        pfld_RightSecondaryFP_IsOverride = value("fld_RightSecondaryFP_IsOverride")
                        pfld_RightSecondaryFP_Ansi = value("fld_RightSecondaryFP_Ansi")
                        pfld_RightSecondaryFP_Wsq = value("fld_RightSecondaryFP_Wsq")
                        Return True
                    Else
                        pErrorMessage = "Reference Not Found."
                    End If

                Else
                    pErrorMessage = "Load Bio(): " & DAL.ErrorMessage
                End If
            Catch ex As Exception
                pErrorMessage = "Load Bio(): " & DAL.ErrorMessage
            End Try


            Return False
        End Function

    End Class

End Namespace


