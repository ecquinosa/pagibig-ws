Public Class RecordUpdatedFields


    Private _FieldsUpdated As New List(Of String)
    Public Property FieldsUpdated As List(Of String)
        Get
            Return _FieldsUpdated
        End Get
        Set(ByVal value As List(Of String))
            _FieldsUpdated = value
        End Set
    End Property

End Class
