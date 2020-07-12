Public Class DCS_Upload
    Private pStatus As String = ""
    Public Property Status As String
        Get
            Return pStatus
        End Get
        Set(ByVal value As String)
            pStatus = value
        End Set
    End Property
    Private pRefNum As String = ""
    Public Property RefNum As String
        Get
            Return pRefNum
        End Get
        Set(ByVal value As String)
            pRefNum = value
        End Set
    End Property
    Private pPagIBIGID As String = Nothing
    Public Property PagIBIGID As String
        Get
            Return pPagIBIGID
        End Get
        Set(ByVal value As String)
            pPagIBIGID = value
        End Set
    End Property

    Private pIsPushCardInfo As String = ""
    Public Property IsPushCardInfo As String
        Get
            Return pIsPushCardInfo
        End Get
        Set(ByVal value As String)
            pIsPushCardInfo = value
        End Set
    End Property

    Private pIsPushCardInfoDate As DateTime = Nothing
    Public Property PushCardInfoDate As DateTime
        Get
            Return pIsPushCardInfoDate
        End Get
        Set(ByVal value As DateTime)
            pIsPushCardInfoDate = value
        End Set
    End Property

    Private pIsPackUpData As String = ""
    Public Property IsPackUpData As String
        Get
            Return pIsPackUpData
        End Get
        Set(ByVal value As String)
            pIsPackUpData = value
        End Set
    End Property

    Private pPackUpDataDate As DateTime = Nothing
    Public Property PackUpDataDate As DateTime
        Get
            Return pPackUpDataDate
        End Get
        Set(ByVal value As DateTime)
            pPackUpDataDate = value
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

    Private pRemarks As String = ""
    Public Property Remarks As String
        Get
            Return pRemarks
        End Get
        Set(ByVal value As String)
            pRemarks = value
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
            pErrorMessage = "SaveDCS_Upload(): DAL is nothing"
            Return False
        End If

        If Not DAL.SaveDCSUpload(Me, myTrans) Then
            pErrorMessage = "SaveDCS_Upload(): " & DAL.ErrorMessage
            Return False
        End If

        Return True
    End Function

    Public Function Load(ByVal DAL As DAL, ByVal refNumber As String) As Boolean
        Dim dt As DataTable = Nothing
        Try

            If DAL.SelectQuery(String.Format("EXEC prcSelectDCS_Upload @RefNum = '{0}'", refNumber)) Then
                dt = DAL.TableResult
                If dt.Rows.Count > 0 Then
                    Dim value = dt.Rows(0)
                    pRefNum = refNumber
                    pStatus = value("Status")
                    pPagIBIGID = value("PagIBIGID")
                    pIsPushCardInfo = value("IsPushCardInfo")
                    pIsPushCardInfoDate = value("PushCardInfoDate")
                    pIsPackUpData = value("IsPackUpData")
                    pPackUpDataDate = value("PackUpDataDate")
                    pRemarks = value("Remarks")
                    pUsername = value("Username")

                    Return True
                Else
                    pErrorMessage = "Reference Not Found."
                End If

            Else
                pErrorMessage = "Load DCS Upload(): " & DAL.ErrorMessage
            End If
        Catch ex As Exception
            pErrorMessage = "Load DCS Upload(): " & DAL.ErrorMessage
        End Try

        Return False
    End Function

    Public Shared Function GetForUploadList(ByVal DAL As DAL) As List(Of DCS_Upload)
        Dim dt As DataTable = Nothing
        Dim list As New List(Of DCS_Upload)

        If DAL.SelectQuery("EXEC prcSelectDCS_UploadForUpload ") Then
            dt = DAL.TableResult
            If dt.Rows.Count > 0 Then
                For Each memUpload As DataRow In dt.Rows
                    Dim value = memUpload
                    Dim memberUpload As New DCS_Upload
                    memberUpload.RefNum = value("RefNum")
                    memberUpload.Status = value("Status")
                    memberUpload.PagIBIGID = value("PagIBIGID")
                    memberUpload.IsPushCardInfo = value("IsPushCardInfo")
                    memberUpload.PushCardInfoDate = IIf(IsDBNull(value("PushCardInfoDate")), Nothing, value("PushCardInfoDate"))
                    memberUpload.IsPackUpData = value("IsPackUpData")
                    memberUpload.PackUpDataDate = IIf(IsDBNull(value("PackUpDataDate")), Nothing, value("PackUpDataDate"))
                    memberUpload.Remarks = value("Remarks")
                    memberUpload.Username = value("Username")
                    list.Add(memberUpload)
                Next
            End If
        End If
        Return list
    End Function
End Class
