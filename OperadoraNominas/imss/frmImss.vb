Public Class frmPension
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Dim idAcuse As String
    Dim Tipo As String
    Dim clave As String
    Dim blnNuevo As Boolean

    Private Sub frmImss_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cargarhistorial()
        CargarSueldos()
        CargarIncapacidades()

        cmdAgregar.Enabled = False
        blnNuevo = True
        cbotipo.SelectedIndex = 0
        cboopcion.SelectedIndex = 0
    End Sub
    Private Sub CargarIncapacidades()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvincapacidad.Items.Clear()
            SQL = "select * from IncapacidadAlta "
            SQL &= " where fkiIdEmpleado=" & gIdEmpleado
            SQL &= " order by iIdIncapacidad "
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvincapacidad.Items.Add(Fila.Item("Folio"))
                    item.SubItems.Add("" & Fila.Item("fkTipoIncapacidad"))
                    item.SubItems.Add("" & Fila.Item("iNumeroDias"))
                    item.SubItems.Add("" & Fila.Item("dFechaInicial"))
                    item.SubItems.Add("" & Fila.Item("dFechaFinal"))
                    item.SubItems.Add("" & Fila.Item("Noaplicada"))
                    item.SubItems.Add("" & Fila.Item("observacion"))
                    item.Tag = Fila.Item("iIdIncapacidad")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
            End If
            If lsvincapacidad.Items.Count > 0 Then
                lsvincapacidad.Focus()
                lsvincapacidad.Items(0).Selected = True
            Else
                'txtbuscar.Focus()
                'txtbuscar.SelectAll()
            End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub Cargarhistorial()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvHistorial.Items.Clear()
            SQL = "select * from ingresobajaAlta "
            SQL &= " where fkiIdEmpleado=" & gIdEmpleado
            SQL &= " order by iIdIngresoBaja "
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvHistorial.Items.Add(Fila.Item("Clave"))
                    item.SubItems.Add("" & Fila.Item("fecha"))
                    item.SubItems.Add("" & Fila.Item("fechabajaimss"))
                    item.SubItems.Add("" & Fila.Item("acuse"))
                    item.SubItems.Add("" & Fila.Item("observaciones"))
                    item.Tag = Fila.Item("iIdIngresoBaja")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
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

    Private Sub CargarSueldos()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvSalario.Items.Clear()

            SQL = "select * from sueldoAlta "
            SQL &= " where fkiIdEmpleado=" & gIdEmpleado
            SQL &= " order by iIdCambioSueldo "
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvSalario.Items.Add(Fila.Item("dFecha"))
                    item.SubItems.Add("" & Fila.Item("fSD"))
                    item.SubItems.Add("" & Fila.Item("fFactor"))
                    item.SubItems.Add("" & Fila.Item("fSDI"))
                    item.SubItems.Add("" & Fila.Item("dFechaImss"))
                    item.SubItems.Add("" & Fila.Item("acuse"))
                    item.SubItems.Add("" & Fila.Item("Observaciones"))
                    item.Tag = Fila.Item("iIdCambioSueldo")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
            End If
            If lsvSalario.Items.Count > 0 Then
                lsvSalario.Focus()
                lsvSalario.Items(0).Selected = True
            Else
                'txtbuscar.Focus()
                'txtbuscar.SelectAll()
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub lsvHistorial_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvHistorial.ItemActivate
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

                    idAcuse = lsvHistorial.SelectedItems(0).Tag
                    clave = lsvHistorial.SelectedItems(0).SubItems(0).Text
                    If clave = "A" Then
                        rbAlta.Checked = True
                    Else
                        rbBaja.Checked = True
                    End If
                    txtNumacuse.Text = lsvHistorial.SelectedItems(0).SubItems(3).Text
                    txtObservaciones.Text = lsvHistorial.SelectedItems(0).SubItems(4).Text
                    dtpFecha.Value = lsvHistorial.SelectedItems(0).SubItems(2).Text
                    cmdAgregar.Enabled = True
                    Tipo = "1"
                    MessageBox.Show("Alta/Baja lista para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

        Catch ex As Exception

        End Try





    End Sub



    Private Sub lsvSalario_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvSalario.ItemActivate
        'Verificar si se tienen permisos
        Dim Sql As String
        Sql = "select * from usuarios where idUsuario = " & idUsuario
        Dim rwFilas As DataRow() = nConsulta(Sql)
        'Dim Forma As New frmTipoEmpresa
        Try
            If rwFilas Is Nothing = False Then


                Dim Fila As DataRow = rwFilas(0)
                If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3" Or Fila.Item("fkIdPerfil") = "4" Or Fila.Item("fkIdPerfil") = "5") Then

                    'Pasar los datos
                    idAcuse = lsvSalario.SelectedItems(0).Tag
                    clave = "M"
                    rbModificación.Checked = True

                    txtNumacuse.Text = lsvSalario.SelectedItems(0).SubItems(5).Text
                    txtObservaciones.Text = lsvSalario.SelectedItems(0).SubItems(6).Text
                    dtpFecha.Value = lsvSalario.SelectedItems(0).SubItems(4).Text
                    cmdAgregar.Enabled = True
                    Tipo = "2"
                    MessageBox.Show("Modificacion lista para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmdAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgregar.Click
        Dim SQL As String = "SELECT "
        If Tipo = "1" Then
            'Agregar alta/baja
            SQL = "UPDATE IngresoBajaAlta SET "
            SQL &= "fechabajaimss='" & Format(dtpFecha.Value.Date, "yyyy/dd/MM") & "',"
            SQL &= "acuse='" & txtNumacuse.Text & "',observaciones='" & txtObservaciones.Text & "'"

            SQL &= " WHERE iIdIngresoBaja=" & idAcuse
        Else
            'Agregar modificación
            SQL = "UPDATE SueldoAlta SET "
            SQL &= "dFechaImss='" & Format(dtpFecha.Value.Date, "yyyy/dd/MM") & "',"
            SQL &= "acuse='" & txtNumacuse.Text & "',observaciones='" & txtObservaciones.Text & "'"
            SQL &= " WHERE iIdCambioSueldo=" & idAcuse
        End If
        If nExecute(SQL) = False Then
            Exit Sub
        End If
        cmdAgregar.Enabled = False
        dtpFecha.Value = Date.Today
        txtNumacuse.Text = ""
        txtObservaciones.Text = ""
        MessageBox.Show("Datos guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Cargarhistorial()
        CargarSueldos()
    End Sub

    Private Sub cmdIncapacidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdIncapacidad.Click
        Dim SQL As String, Mensaje As String = ""
        Try

            SQL = "select * from usuarios where idUsuario = " & idUsuario
            Dim rwFilas As DataRow() = nConsulta(SQL)
            'Dim Forma As New frmTipoEmpresa

            If rwFilas Is Nothing = False Then


                Dim Fila As DataRow = rwFilas(0)
                If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3" Or Fila.Item("fkIdPerfil") = "4" Or Fila.Item("fkIdPerfil") = "5") Then
                    'Validar
                    If txtfolio.Text.Trim.Length = 0 And Mensaje = "" Then
                        Mensaje = "Por favor indique el folio a guardar"
                    End If



                    If Mensaje <> "" Then
                        MessageBox.Show(Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If





                    If blnNuevo Then
                        'Insertar nuevo
                        SQL = "EXEC setIncapacidadAltaInsertar 0,'" & txtfolio.Text & "'," & cbotipo.SelectedIndex
                        SQL &= "," & NudDias.Value & ",'" & Format(dtpinicial.Value.Date, "yyyy/dd/MM")
                        SQL &= "','" & Format(dtpfinal.Value.Date, "yyyy/dd/MM") & "','" & Date.Today.ToShortDateString
                        SQL &= "'," & cboopcion.SelectedIndex & ",'" & txtObservacion.Text & "'," & gIdEmpleado


                    Else
                        'Actualizar




                    End If
                    If nExecute(SQL) = False Then
                        Exit Sub
                    End If
                    CargarIncapacidades()



                    MessageBox.Show("Datos Guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Limpiar(Me)
                Else

                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If



        Catch ex As Exception

        End Try
    End Sub


    Private Sub Limpiar(ByVal Contenedor As Object)

        txtfolio.Text = ""


        cbotipo.SelectedIndex = 0
        cboopcion.SelectedIndex = 0
        NudDias.Value = 1
        dtpfinal.Value = Date.Today.ToShortDateString()
        dtpinicial.Value = Date.Today.ToShortDateString()
    End Sub

    Private Sub lsvHistorial_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsvHistorial.SelectedIndexChanged

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBorrar.Click
        Dim sql As String
        Dim datos As ListView.SelectedListViewItemCollection = lsvincapacidad.SelectedItems
        If datos.Count = 1 Then
            Dim resultado As Integer = MessageBox.Show("¿Desea borrar la incapacidad " & datos(0).SubItems(0).Text & "?", "Pregunta", MessageBoxButtons.YesNo)


            If resultado = DialogResult.Yes Then


                'datos(0).Remove()

                sql = " DELETE FROM  IncapacidadAlta "
                sql &= "WHERE iIdIncapacidad=" & datos(0).Tag
                If nExecute(Sql) = False Then
                    MessageBox.Show("Hubo un error al eliminar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                CargarIncapacidades()
                MessageBox.Show("Datos borrados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


            End If
        Else
            MessageBox.Show("No hay una incapacidad seleccionada para borrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class