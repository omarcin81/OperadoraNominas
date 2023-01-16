Public Class frmBuscar
    Public gNombre As String


    Private Sub frmBuscart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtbuscar.TabIndex = 1
        rdbNombre.Checked = True
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAceptar.Click
        Try
            gNombre = txtbuscar.Text
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception

        End Try
    End Sub


    
   
    Private Sub txtbuscar_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbuscar.KeyDown
        Select Case e.KeyData
            Case Keys.Enter
                cmdAceptar_Click(sender, e)
        End Select
    End Sub

    Private Sub rdbNombre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbNombre.CheckedChanged
        If rdbNombre.Checked = True Then
            rdbCodigo.Checked = False
        End If
    End Sub

    Private Sub rdbCodigo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbCodigo.CheckedChanged
        If rdbCodigo.Checked = True Then
            rdbNombre.Checked = False
        End If
    End Sub
End Class