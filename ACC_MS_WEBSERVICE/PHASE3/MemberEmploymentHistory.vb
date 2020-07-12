
Namespace PHASE3

    Public Class MemberEmploymentHistory

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

        Private pHistory_EmployerName As String = ""
        Public Property History_EmployerName As String
            Get
                Return pHistory_EmployerName
            End Get
            Set(ByVal value As String)
                pHistory_EmployerName = value
            End Set
        End Property

        Private pHistory_EmployerAddress As String = ""
        Public Property History_EmployerAddress As String
            Get
                Return pHistory_EmployerAddress
            End Get
            Set(ByVal value As String)
                pHistory_EmployerAddress = value
            End Set
        End Property

        'Private pHistory_DateEmployed As Date = Nothing
        'Public Property History_DateEmployed As Date
        '    Get
        '        Return pHistory_DateEmployed
        '    End Get
        '    Set(ByVal value As Date)
        '        pHistory_DateEmployed = value
        '    End Set
        'End Property

        Private pHistory_DateEmployed As String = ""
        Public Property History_DateEmployed As String
            Get
                Return pHistory_DateEmployed
            End Get
            Set(ByVal value As String)
                pHistory_DateEmployed = value
            End Set
        End Property

        Private pHistory_DateSeparated As String = ""
        Public Property History_DateSeparated As String
            Get
                Return pHistory_DateSeparated
            End Get
            Set(ByVal value As String)
                pHistory_DateSeparated = value
            End Set
        End Property

        'Private pHistory_DateSeparated As Date = Nothing
        'Public Property History_DateSeparated As Date
        '    Get
        '        Return pHistory_DateSeparated
        '    End Get
        '    Set(ByVal value As Date)
        '        pHistory_DateSeparated = value
        '    End Set
        'End Property

        Private pOffice_Assignment As String = ""
        Public Property Office_Assignment As String
            Get
                Return pOffice_Assignment
            End Get
            Set(ByVal value As String)
                pOffice_Assignment = value
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
                pErrorMessage = "SaveMemberEmploymentHistoryRequest(): DAL is nothing"
                Return False
            End If

            If Not DAL.AddEmploymentHistory(pRefNum, pPagIBIGID, pHistory_EmployerName, pHistory_EmployerAddress, pHistory_DateEmployed,
                                            pHistory_DateSeparated, pOffice_Assignment, myTrans) Then
                pErrorMessage = "SaveMemberEmploymentHistoryRequest(): " & DAL.ErrorMessage
                Return False

            End If

            Return True
        End Function


    End Class

End Namespace


