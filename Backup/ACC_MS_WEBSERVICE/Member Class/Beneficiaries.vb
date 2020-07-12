Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Public Class Beneficiaries
    Inherits SpouseName

    Private pRelationship As String
    Public Property Relationship As String
        Get
            Return pRelationship
        End Get
        Set(ByVal value As String)
            pRelationship = value
        End Set
    End Property

End Class
