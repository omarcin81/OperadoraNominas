Public Class frmBuscarEmpleado
    Public gIdEmpresa As String
    Public gIdEmpleado As String
    Public SoloActivo As Boolean = False
    Private Sub cmdBuscar_Click(sender As Object, e As EventArgs) Handles cmdBuscar.Click
        Dim SQL As String, Alter As Boolean = False
        Try
            SQL = "select iIdEmpleadoC, cNombre, cApellidoP,cApellidoM,cRFC,cCURP,cIMSS from empleadosC "
            SQL &= " where (cNombreLargo like '%" & txtbuscar.Text & "%') and fkiIdEmpresa=1" '& gIdEmpresa
            'If SoloActivo Then
            SQL &= " AND iEstatus = 1"
            'End If
            SQL &= " order by cNombreLargo"
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            lsvEmpresas.Items.Clear()
            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvEmpresas.Items.Add(Fila.Item("cNombre"))
                    item.SubItems.Add("" & Fila.Item("cApellidoP"))
                    item.SubItems.Add("" & Fila.Item("cApellidoM"))
                    item.SubItems.Add("" & Fila.Item("cRFC"))
                    item.SubItems.Add("" & Fila.Item("cCURP"))
                    item.SubItems.Add("" & Fila.Item("cIMSS"))
                    item.Tag = Fila.Item("iIdEmpleadoC")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
            End If
            If lsvEmpresas.Items.Count > 0 Then
                lsvEmpresas.Focus()
                lsvEmpresas.Items(0).Selected = True
            Else
                txtbuscar.Focus()
                txtbuscar.SelectAll()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub lsvEmpresas_ItemActivate(sender As Object, e As EventArgs) Handles lsvEmpresas.ItemActivate
        If lsvEmpresas.SelectedItems.Count > 0 Then
            gIdEmpleado = lsvEmpresas.SelectedItems(0).Tag
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub lsvEmpresas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lsvEmpresas.SelectedIndexChanged

    End Sub

    Private Sub cmdCerrar_Click(sender As Object, e As EventArgs) Handles cmdCerrar.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtbuscar_TextChanged(sender As Object, e As EventArgs) Handles txtbuscar.TextChanged

    End Sub

    Private Sub txtbuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbuscar.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdBuscar_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmBuscarEmpleado_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtbuscar.TabIndex = 1
    End Sub
End Class