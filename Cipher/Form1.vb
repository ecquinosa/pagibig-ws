Imports ACC_MS_WEBSERVICE.PHASE3

Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnEncrypt.Click

        txtOut.Text = PHASE3.Encrypt(txtData.Text)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            txtOut.Text = PHASE3.Decrypt(txtData.Text)
        Catch ex As Exception
            txtOut.Text = "Failed to decrypt" + ex.StackTrace.ToString()
        End Try

    End Sub
End Class
