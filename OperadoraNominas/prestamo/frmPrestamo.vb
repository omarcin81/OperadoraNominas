Public Class frmPrestamo
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Private idPrestamo As String
    ' Dim Tipo As String
    ' Dim fkidbanco As String
    Dim iEstatus As String
    Dim blnNuevo As Boolean
    Dim SQL As String

    Private Sub frmPrestamo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cargarhistorial()
        TabIndex()
        cargartipoprestamo()

        blnNuevo = True
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtMontoTotal.Enabled = False
        txtDescuento.Enabled = False
        dtpFechaPrestamo.Enabled = False
        dtpInicioPago.Enabled = False
        cbotipoprestamo.Enabled = False
        cboEstatus.Enabled = False
        cboEstatus.Enabled = False
        cmdDeleted.Enabled = False

    End Sub

    Private Sub Cargarhistorial()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvHistorial.Items.Clear()
            SQL = "SELECT * FROM Prestamo"
            SQL &= " where fkiIdEmpleado=" & gIdEmpleado
            SQL &= " ORDER BY iIdPrestamo"

            'SQL &= " AND iEstatus=1"
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem

            Dim tipoprestamo As DataRow()

            If rwFilas Is Nothing = False Then

                For Each Fila In rwFilas
                    idPrestamo = Fila.Item("iIdPrestamo")
                    iEstatus = Fila.Item("iEstatus")
                    item = lsvHistorial.Items.Add(Fila.Item("iIdPrestamo"))

                    tipoprestamo = nConsulta("select * from TipoPrestamo where iIdTipoPrestamo =" & Fila.Item("fkIidTipoPrestamo"))
                    item.Tag = Fila.Item("iIdPrestamo")
                    item.SubItems.Add("" & Fila.Item("fechaPrestamo"))
                    item.SubItems.Add("" & Fila.Item("montototal"))
                    item.SubItems.Add("" & Fila.Item("descuento"))
                    item.SubItems.Add("" & tipoprestamo(0).Item("TipoPrestamo"))
                    item.SubItems.Add("" & Fila.Item("fechainiciopago"))
                    item.SubItems(1).Tag = Fila.Item("iEstatus")

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
    Private Sub TabIndex()
        cboEstatus.Focus()
        cboEstatus.TabIndex = 1
        dtpFechaPrestamo.TabIndex = 2
        txtMontoTotal.TabIndex = 3
        txtDescuento.TabIndex = 4
        dtpInicioPago.TabIndex = 5
        cmdguardar.TabIndex = 6
    End Sub

    Private Sub Limpiar(ByVal Contenedor As Object)

        dtpInicioPago.Value = Date.Now
        dtpFechaPrestamo.Value = Date.Now
        txtMontoTotal.Text = ""
        txtDescuento.Text = ""
        cboEstatus.SelectedIndex = 0
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtMontoTotal.Enabled = False
        txtDescuento.Enabled = False
        cboEstatus.Enabled = False
        dtpFechaPrestamo.Enabled = False
        dtpInicioPago.Enabled = False
        cmdDeleted.Enabled = False
        cbotipoprestamo.Enabled = False
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

                    idPrestamo = lsvHistorial.SelectedItems(0).Tag

                    dtpFechaPrestamo.Value = lsvHistorial.SelectedItems(0).SubItems(1).Text

                    txtMontoTotal.Text = IIf(lsvHistorial.SelectedItems(0).SubItems(2).Text = "", "0", lsvHistorial.SelectedItems(0).SubItems(2).Text)
                    txtDescuento.Text = IIf(lsvHistorial.SelectedItems(0).SubItems(3).Text = "", "0", lsvHistorial.SelectedItems(0).SubItems(3).Text)
                    cbotipoprestamo.SelectedItem = IIf(lsvHistorial.SelectedItems(0).SubItems(4).Text = "", "0", lsvHistorial.SelectedItems(0).SubItems(4).Text)
                    dtpInicioPago.Value = IIf(lsvHistorial.SelectedItems(0).SubItems(4).Text = "", "0", lsvHistorial.SelectedItems(0).SubItems(5).Text)
                    cboEstatus.SelectedIndex = IIf(lsvHistorial.SelectedItems(0).SubItems(1).Tag = "1", 1, 0)


                    txtMontoTotal.Enabled = True
                    txtDescuento.Enabled = True
                    cboEstatus.Enabled = True
                    dtpFechaPrestamo.Enabled = True
                    dtpInicioPago.Enabled = True
                    cmdguardar.Enabled = True
                    cmdcancelar.Enabled = True
                    cmdDeleted.Enabled = True
                    cbotipoprestamo.Enabled = True
                    'Tipo = "1"
                    blnNuevo = False
                    MessageBox.Show("Prestamo listo para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

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
                SQL = "EXEC setPrestamoActualizar "
                SQL &= idPrestamo & ","
                SQL &= "'" & txtMontoTotal.Text & "',"
                SQL &= "'" & txtDescuento.Text & "',"
                SQL &= "'" & dtpFechaPrestamo.Value.ToShortDateString & "',"
                SQL &= "'" & dtpInicioPago.Value.ToShortDateString & "',"
                SQL &= cboEstatus.SelectedIndex & ", "
                SQL &= gIdEmpleado & ", "
                SQL &= cbotipoprestamo.SelectedIndex + 1



            Else
                SQL = "EXEC setPrestamoInsertar 0,"
                SQL &= "'" & txtMontoTotal.Text & "',"
                SQL &= "'" & txtDescuento.Text & "',"
                SQL &= "'" & dtpFechaPrestamo.Value.ToShortDateString & "',"
                SQL &= "'" & dtpInicioPago.Value.ToShortDateString & "',"
                SQL &= cboEstatus.SelectedIndex & ","
                SQL &= gIdEmpleado & ", "
                SQL &= cbotipoprestamo.SelectedIndex + 1
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
        dtpFechaPrestamo.Enabled = True
        dtpInicioPago.Enabled = True
        cboEstatus.Enabled = True
        cbotipoprestamo.Enabled = True
        TabIndex()
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

                SQL = "DELETE FROM Prestamo where iIdPrestamo =" & datos(0).SubItems(0).Text & ""

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

    

    Private Sub cargartipoprestamo()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select tipoPrestamo,iIdTipoPrestamo  from tipoPrestamo order by tipoPrestamo"
            nCargaCBO(cbotipoprestamo, sql, "tipoPrestamo", "iIdTipoPrestamo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub lsvHistorial_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lsvHistorial.SelectedIndexChanged

    End Sub
End Class