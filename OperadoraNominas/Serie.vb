Public Class Serie
    Public gSerie As Integer
    Public gAnio As Integer

   

    'Private Sub cmdCancelar_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancelar.Click
    '    Me.DialogResult = Windows.Forms.DialogResult.Cancel
    '    Me.Close()
    'End Sub

    Private Sub cmdAceptar_Click(sender As System.Object, e As System.EventArgs) Handles cmdAceptar.Click
        Try
            gSerie = cboserie.SelectedIndex
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub Serie_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class