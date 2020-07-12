
Imports System.Data.SqlClient

Class ODBCDataAccess
    Private DB As SqlConnection
    Private CString As String
    Private GetErrMessage As String
    Private DA As SqlDataAdapter
    Private cmd As SqlCommand

    ''' <summary>
    ''' Property for connection string
    ''' </summary>
    Public Property ConnectionString() As String
        Get
            Return CString
        End Get
        Set(ByVal value As String)
            CString = Value
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ErrMessage As String
        Get
            Return GetErrMessage
        End Get
        Set(ByVal value As String)
            GetErrMessage = value
        End Set
    End Property

    Public Function DBConnectTest() As Boolean

        Try
            Using DB = New SqlConnection(CString)
                DB.Open()
                Return True
            End Using

        Catch generatedExceptionName As Exception
            GetErrMessage = generatedExceptionName.Message
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Execute Data manipulation query
    ''' </summary>
    ''' <param name="SQL"></param>
    Public Sub RunSQL(ByVal SQL As String)
        Using DB = New SqlConnection(CString)
            Try
                Dim DS As New DataSet
                DB.Open()
                cmd = New SqlCommand(SQL, DB)
                cmd.CommandTimeout = 0
                DA = New SqlDataAdapter(cmd)
                DA.Fill(DS)
                DB.Close()
            Catch generatedExceptionName As Exception
                GetErrMessage = generatedExceptionName.Message
            End Try
        End Using
    End Sub

    Public Function RunSQLWithReturn(ByVal SQL As String) As String
        Using DB = New SqlConnection(CString)
            Try
                Dim DS As New DataSet
                DB.Open()
                cmd = New SqlCommand(SQL, DB)
                cmd.CommandTimeout = 0
                DA = New SqlDataAdapter(cmd)
                DA.Fill(DS)
                DB.Close()
                RunSQLWithReturn = "Success"
                Return RunSQLWithReturn
            Catch generatedExceptionName As Exception
                GetErrMessage = generatedExceptionName.Message
                Return generatedExceptionName.Message & "( " & SQL & " )"
            End Try
        End Using
    End Function

    Public Function RunSQLNonQuery(ByVal cmd As SqlCommand) As Boolean
        Try
            Using DB = New SqlConnection(CString)
                cmd.CommandType = CommandType.Text
                cmd.Connection = DB

                DB.Open()
                cmd.ExecuteNonQuery()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Populate Datagrid
    ''' </summary>
    ''' <param name="SQL"></param>
    ''' <param name="DGrid"></param>
    Public Sub FillGrid(ByVal SQL As String, ByVal DGrid As DataGrid)
        Using DB = New SqlConnection(CString)
            Try
                Dim DS As New DataSet()
                DB.Open()
                cmd = New SqlCommand(SQL, DB)
                cmd.CommandTimeout = 0
                DA = New SqlDataAdapter(cmd)
                DA.SelectCommand = New SqlCommand(SQL, DB)
                DA.Fill(DS)
                DGrid.DataSource = DS.Tables(0)
                DB.Close()
            Catch generatedExceptionName As Exception
                GetErrMessage = generatedExceptionName.Message
            End Try
        End Using
    End Sub

    '''' <summary>
    '''' Populate DevX Datagrid
    '''' </summary>
    '''' <param name="SQL"></param>
    '''' <param name="DGrid"></param>
    ''Public Sub FillGrid(ByVal SQL As String, ByVal DGrid As DevExpress.XtraGrid.GridControl)
    ''    Using DB = New SqlConnection(CString)
    ''        Try
    ''            Dim DS As New DataSet
    ''            DB.Open()
    ''            cmd = New SqlCommand(SQL, DB)
    ''            cmd.CommandTimeout = 0
    ''            DA = New SqlDataAdapter(cmd)
    ''            DA.SelectCommand = New SqlCommand(SQL, DB)

    ''            DA.Fill(DS)
    ''            DGrid.DataSource = DS.Tables(0)
    ''            DB.Close()
    ''        Catch ex As Exception
    ''            GetErrMessage = ex.Message
    ''        End Try
    ''    End Using
    ''End Sub

    ''' <summary>
    ''' use for finding record return value bool
    ''' </summary>
    ''' <param name="SQL"></param>
    ''' <returns></returns>
    Public Function RecordFound(ByVal SQL As String) As Boolean
        Using DB = New SqlConnection(CString)

            Try
                Dim DS As New DataSet
                DB.Open()
                cmd = New SqlCommand(SQL, DB)
                cmd.CommandTimeout = 0
                DA = New SqlDataAdapter(cmd)

                DA.Fill(DS)

                If DS.Tables(0).Rows.Count = 0 Then
                    DB.Close()
                    Return False
                Else
                    DB.Close()
                    Return True
                End If
            Catch generatedExceptionName As Exception
                GetErrMessage = generatedExceptionName.Message
                Return False
            End Try
        End Using
    End Function



    'Public Sub FillComboBox(ByVal cboComboBox As ComboBox, ByVal SQL As String, ByVal Displayed As String, ByVal ValueField As String)
    '    Using DB = New SqlConnection(CString)

    '        Try
    '            cboComboBox.DataSource = Nothing
    '            Dim DT As New DataTable
    '            DB.Open()
    '            cmd = New SqlCommand(SQL, DB)
    '            cmd.CommandTimeout = 0
    '            DA = New SqlDataAdapter(cmd)
    '            DA.Fill(DT)

    '            'cboComboBox.DataSource = DT
    '            'cboComboBox.DisplayMember = Displayed
    '            'cboComboBox.ValueMember = ValueField
    '            If DT.Rows.Count > 0 Then
    '                cboComboBox.DisplayMember = Displayed
    '                cboComboBox.ValueMember = ValueField
    '                cboComboBox.DataSource = DT
    '            Else
    '                cboComboBox.DataSource = Nothing
    '                cboComboBox.Items.Clear()
    '            End If
    '        Catch generatedExceptionName As Exception
    '            GetErrMessage = generatedExceptionName.Message
    '        End Try
    '    End Using
    'End Sub


    Public Function ExecuteSQLQueryDataTable(ByVal SQL As String, ByVal TableName As String) As DataTable
        Using DB = New SqlConnection(CString)
            Dim sqlDT As New DataTable(TableName)
            Try
                DB.Open()
                cmd = New SqlCommand(SQL, DB)
                cmd.CommandTimeout = 0
                DA = New SqlDataAdapter(cmd)
                Dim sqlCB As New SqlCommandBuilder(DA)
                DA.Fill(sqlDT)
                DB.Close()
            Catch ex As Exception
                GetErrMessage = ex.Message
            End Try
            Return sqlDT
        End Using
    End Function

End Class

