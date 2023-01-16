Public Class frmDeudaInfonavit
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Private idDeuda As String
    ' Dim Tipo As String
    ' Dim fkidbanco As String
    Dim iEstatus As String
    Dim blnNuevo As Boolean
    Dim SQL As String

    Private Sub frmDeudaInfonavit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cargarhistorial()



        blnNuevo = True
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtMontoTotal.Enabled = False
        txtDescuento.Enabled = False
        dtpFechaAlta.Enabled = False
        dtpInicioPago.Enabled = False
        cboEstatus.Enabled = False
        cmdDeleted.Enabled = True
    End Sub

    Private Sub Cargarhistorial()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvHistorial.Items.Clear()
            SQL = "SELECT * FROM DeudaInfonavit"
            SQL &= " where fkiIdEmpleadoC=" & gIdEmpleado
            SQL &= " AND iEstatus=1"
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem

            If rwFilas Is Nothing = False Then

                For Each Fila In rwFilas
                    idDeuda = Fila.Item("iIdDeudaInfonavit")
                    iEstatus = Fila.Item("iEstatus")
                    item = lsvHistorial.Items.Add(Fila.Item("iIdDeudaInfonavit"))
                    'item.SubItems.Add("" & Fila.Item("iIdDeudaInfonavit"))
                    item.SubItems.Add("" & Fila.Item("fechaAlta"))
                    item.SubItems.Add("" & Fila.Item("montototal"))
                    item.SubItems.Add("" & Fila.Item("descuento"))
                    item.SubItems.Add("" & Fila.Item("fechainiciopago"))
                    item.Tag = Fila.Item("iIdDeudaInfonavit")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter
                    blnNuevo = False

                Next
            Else
                blnNuevo = True
            End If

            If lsvHistorial.Items.Count > 0 Then
                lsvHistorial.Focus()
                lsvHistorial.Items(0).Selected = True
            Else
                'txtbuscar.Focus()
                'txtbuscar.SelectAll()
            End If


        Catch ex As Exception

        End Try
    End Sub


    Private Sub Limpiar(ByVal Contenedor As Object)

        dtpInicioPago.Value = Date.Now
        dtpFechaAlta.Value = Date.Now
        txtMontoTotal.Text = ""
        txtDescuento.Text = ""
        cboEstatus.SelectedIndex = 0
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtMontoTotal.Enabled = False
        txtDescuento.Enabled = False
        cboEstatus.Enabled = False
        dtpFechaAlta.Enabled = False
        dtpInicioPago.Enabled = False
        cmdDeleted.Enabled = False
    End Sub





    Private Sub lsvHistorial_ItemActivate_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsvHistorial.ItemActivate

        'Verificar si se tienen permisos
        Dim Sql As String
        Sql = "select * from usuarios where idUsuario = " & idUsuario
        Dim rwFilas As DataRow() = nConsulta(Sql)
        ' Dim Forma As New frmTipoEmpresa
        Try
            If rwFilas Is Nothing = False Then


                Dim Fila As DataRow = rwFilas(0)
                If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3") Then

                    'Pasar los datos

                    idDeuda = lsvHistorial.SelectedItems(0).Tag



                    dtpFechaAlta.Value = lsvHistorial.SelectedItems(0).SubItems(1).Text
                    txtMontoTotal.Text = lsvHistorial.SelectedItems(0).SubItems(2).Text
                    txtDescuento.Text = lsvHistorial.SelectedItems(0).SubItems(3).Text

                    dtpInicioPago.Value = lsvHistorial.SelectedItems(0).SubItems(4).Text
                    cboEstatus.SelectedIndex = IIf(iEstatus = "1", 1, 0)


                    txtMontoTotal.Enabled = True
                    txtDescuento.Enabled = True
                    cboEstatus.Enabled = True
                    dtpFechaAlta.Enabled = True
                    dtpInicioPago.Enabled = True
                    cmdguardar.Enabled = True
                    cmdcancelar.Enabled = True
                    cmdDeleted.Enabled = True
                    'Tipo = "1"
                    blnNuevo = False
                    MessageBox.Show("Deuda Infonavit listo para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdguardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdguardar.Click
        Try
            If txtMontoTotal.Text = "" Then
                MessageBox.Show("Agrege el monto", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If blnNuevo = False Then
                SQL = "EXEC setDeudaInfonavitActualizar "
                SQL &= idDeuda & ","
                SQL &= "'" & txtMontoTotal.Text & "',"
                SQL &= "'" & txtDescuento.Text & "',"
                SQL &= "'" & dtpFechaAlta.Value.ToShortDateString & "',"
                SQL &= "'" & dtpInicioPago.Value.ToShortDateString & "',"
                SQL &= cboEstatus.SelectedIndex



            Else
                SQL = "EXEC setDeudaInfonavitInsertar 0,"
                SQL &= "'" & txtMontoTotal.Text & "',"
                SQL &= "'" & txtDescuento.Text & "',"
                SQL &= "'" & dtpFechaAlta.Value.ToShortDateString & "',"
                SQL &= "'" & dtpInicioPago.Value.ToShortDateString & "',"
                SQL &= cboEstatus.SelectedIndex & ","
                SQL &= gIdEmpleado
            End If

            If nExecute(SQL) = False Then
                Exit Sub
            End If
            Limpiar(Me)
            blnNuevo = True
            MessageBox.Show("Datos guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


            Cargarhistorial()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdnuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdnuevo.Click
        cmdguardar.Enabled = True
        cmdcancelar.Enabled = True
        txtMontoTotal.Enabled = True
        txtDescuento.Enabled = True
        dtpFechaAlta.Enabled = True
        dtpInicioPago.Enabled = True
        cboEstatus.Enabled = True
        cmdDeleted.Enabled = True
    End Sub

    Private Sub cmdsalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsalir.Click
        Me.Close()
    End Sub

    Private Sub cmdcancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdcancelar.Click
        'If blnNuevo Then
        '    'Cargar los datos anteriores
        'Else
        Limpiar(Me)
        'End If
    End Sub

    Private Sub cmdDeleted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleted.Click
        Try


            Dim datos As ListView.SelectedListViewItemCollection = lsvHistorial.SelectedItems
            If datos.Count > 0 Then
                'Dim resultado As Integer = MessageBox.Show("¿Desea borrar el documento, se eliminara del sistema " & datos(0).SubItems(0).Text & "?", "Pregunta", MessageBoxButtons.YesNo)


                'If resultado = DialogResult.Yes Then

                SQL = "DELETE FROM DeudaInfonavit where iIdDeudaInfonavit =" & datos(0).SubItems(0).Text & ""

                If nExecute(SQL) = False Then
                    MessageBox.Show("Hubo un problema al borrar, revise sus datos", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    datos(0).Remove()
                    MessageBox.Show("Datos borrados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Cargarhistorial()
                    Limpiar(e)
                End If

                'End If

            Else

                MessageBox.Show("Seleccione un archivo para borrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception

        End Try
    End Sub
End Class