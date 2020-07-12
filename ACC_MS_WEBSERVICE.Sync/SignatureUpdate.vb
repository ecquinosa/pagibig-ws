Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports ACC_MS_WEBSERVICE.PHASE3

Public Class SignatureUpdate
    Dim sig As Signature
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openfile As New OpenFileDialog
        openfile.Filter = "Image Files|*.jpg;*.gif;*.png;*.bmp"
        If openfile.ShowDialog = DialogResult.OK Then
            sig = New Signature

            picSignature.Image = Image.FromFile(openfile.FileName)

            Dim ms As New MemoryStream
            Dim arrImage() As Byte
            picSignature.Image.Save(ms, ImageFormat.Jpeg)
            sig.fld_Signature = ms.GetBuffer

        End If



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sigExist As New Signature
        Dim dal As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        If sigExist.Load(dal, txtRefNum.Text) Then
            sigExist.fld_Signature = sig.fld_Signature
            If sigExist.SaveToDbase(dal, myTrans) Then
                MessageBox.Show("Signature replaced")
                For Each tr As SqlClient.SqlTransaction In myTrans
                    tr.Commit()
                Next
            End If

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sigExist As New Signature
        Dim dal As New DAL
        Dim myTrans As New List(Of System.Data.SqlClient.SqlTransaction)
        If sigExist.Load(dal, txtRefNum.Text) Then
            Dim ms As New IO.MemoryStream(CType(sigExist.fld_Signature, Byte())) 'This is correct...
            Dim returnImage As Image = Image.FromStream(ms)
            picSigLoad.Image = returnImage
        End If
    End Sub
End Class