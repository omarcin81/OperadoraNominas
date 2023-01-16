Public Class frmEstatusPres

    Public gEstatus As String


    Private Sub frmEstatusPres_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        
    End Sub

   

   
    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAceptar.Click
        If rdbStatus.Checked Then
            gEstatus = 1
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf rdbStatus2.Checked Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        Me.Close()
    End Sub
End Class