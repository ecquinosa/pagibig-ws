Imports System.Drawing
Imports System.IO
Imports ACC_MS_WEBSERVICE.PHASE3

Public Class Utilities
    Public Shared Sub SaveToSystemLog(ByVal requestAuth As RequestAuth, ByVal data As String)

        Dim logRepo As String = String.Format("{0}\Log\{1}", Directory.GetCurrentDirectory(), Now.ToString("yyyy-MM-dd"))
        Dim fileLog As String = String.Format("{0}\{1}", logRepo, "Sync.txt")


        If Not System.IO.Directory.Exists(logRepo) Then System.IO.Directory.CreateDirectory(logRepo)
        Dim user As String = ""
        Dim kioskID As String = ""

        If Not requestAuth Is Nothing Then
            user = requestAuth.User
            kioskID = requestAuth.KioskID
        End If

        Using sw As New System.IO.StreamWriter(fileLog, True)
            sw.Write(String.Format("{0}|{1}|{2}|{3}", Now.ToString("MM/dd/yyyy hh:mm:ss tt"), requestAuth.User, requestAuth.KioskID, data) & vbNewLine)
            sw.Dispose()
            sw.Close()
        End Using
    End Sub

    Public Shared Function byteArrayToImage(ByVal byteArrayIn As Byte()) As Image
        Using mStream As New MemoryStream(byteArrayIn)
            Return Image.FromStream(mStream)
        End Using
    End Function
End Class
