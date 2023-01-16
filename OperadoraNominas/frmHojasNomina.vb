Public Class frmHojasNomina
    Public Hojas As String
    Public selectedIndex As Integer = -1

    Private Sub cmdCancelar_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancelar.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub cmdAceptar_Click(sender As System.Object, e As System.EventArgs) Handles cmdAceptar.Click
        selectedIndex = lsbHojas.SelectedIndex
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub frmHojasNomina_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        MostrarHojas()
    End Sub

    Private Sub MostrarHojas()
        Dim aHojas As String() = Hojas.Split("|")
        For i As Integer = 0 To aHojas.Length - 1
            lsbHojas.Items.Add(aHojas(i))
        Next
        lsbHojas.SelectedIndex = 0
    End Sub

    Private Sub lsbHojas_Click(sender As Object, e As System.EventArgs) Handles lsbHojas.Click

    End Sub

    Private Sub lsbHojas_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles lsbHojas.MouseDoubleClick
        If lsbHojas.SelectedItem Is Nothing = False Then
            cmdAceptar_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub lsbHojas_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lsbHojas.SelectedIndexChanged

    End Sub
End Class