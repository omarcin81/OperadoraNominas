Public Class frmAgregarEmpleado
    Public gidEmpleados As String

    Private Sub cmdBuscar_Click(sender As System.Object, e As System.EventArgs) Handles cmdBuscar.Click
        Dim SQL As String, Alter As Boolean = False
        Try
            SQL = "select iIdEmpleadoC, cCodigoEmpleado, cNombre, cApellidoP,cApellidoM,cRFC,cCURP,cIMSS from empleadosC "
            SQL &= " where (cNombreLargo like '%" & txtbuscar.Text & "%') and fkiIdEmpresa=1" '& gIdEmpresa
            'If SoloActivo Then
            SQL &= " AND fkiIdClienteInter=1"
            'End If
            SQL &= " order by cNombreLargo"
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            lsvLista.Items.Clear()

            lsvLista.Columns.Add("Código")
            lsvLista.Columns(0).Width = 170
            lsvLista.Columns.Add("Apellido P")
            lsvLista.Columns(1).Width = 170
            lsvLista.Columns.Add("Apellido M")
            lsvLista.Columns(2).Width = 170
            lsvLista.Columns.Add("Nombre")
            lsvLista.Columns(3).Width = 170
            lsvLista.Columns.Add("RFC")
            lsvLista.Columns(4).Width = 170
            lsvLista.Columns.Add("CURP")
            lsvLista.Columns(5).Width = 300

            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvLista.Items.Add(Fila.Item("cCodigoEmpleado"))
                    item.SubItems.Add("" & Fila.Item("cApellidoP"))
                    item.SubItems.Add("" & Fila.Item("cApellidoM"))
                    item.SubItems.Add("" & Fila.Item("cNombre"))
                    item.SubItems.Add("" & Fila.Item("cRFC"))
                    item.SubItems.Add("" & Fila.Item("cCURP"))
                    item.Tag = Fila.Item("iIdEmpleadoC")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
            End If
            If lsvLista.Items.Count > 0 Then
                lsvLista.Focus()
                lsvLista.Items(0).Selected = True
            Else
                txtbuscar.Focus()
                txtbuscar.SelectAll()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cmdagregar_Click(sender As System.Object, e As System.EventArgs) Handles cmdagregar.Click
        Try
            gidEmpleados = ""

            Dim inicio As Boolean = True
            If lsvLista.CheckedItems.Count > 0 Then
                For Each producto As ListViewItem In lsvLista.CheckedItems
                    If inicio Then

                        gidEmpleados = producto.Tag
                        inicio = False
                    Else

                        gidEmpleados &= "," & producto.Tag
                    End If
                Next
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show("Por favor seleccione al menos un empleado.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmAgregarEmpleado_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtbuscar.TabIndex = 1
        cmdBuscar.TabIndex = 2
        lsvLista.TabIndex = 3
        cmdagregar.TabIndex = 4
    End Sub
End Class