Public Class Ruta
    Dim rutap As String

    Private Sub ruta_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'txtRuta.Text=""
    End Sub

    Private Sub btnRutaP_Click(sender As System.Object, e As System.EventArgs) Handles btnRutaP.Click
        Dim dialogo As New OpenFileDialog
        txtRuta.Text = ""


        Dim folder As New FolderBrowserDialog


        If folder.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtRuta.Text = folder.SelectedPath
            Me.Close()
        End If

    End Sub

   
    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click
        If txtRuta.Text <> " " Then
            Me.Close()
        End If
    End Sub
End Class