Public Class frmPensionA
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Dim idPension As String
    Dim Tipo As String
    Dim fkidbanco As String
    Dim iEstatus As String
    Dim blnNuevo As Boolean
    Dim SQL As String

    Private Sub frmPensionA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cargarhistorial()
        MostrarBancos()
        TabIndex()
        ' cmdAgregar.Enabled = False
        blnNuevo = True
        'txtBeneficiario.Enabled = False
        'txtClabe.Enabled = False
        'txtPorcentaje.Enabled = False
        'txtCuenta.Enabled = False
        'cbobanco.Enabled = False

    End Sub

    Private Sub Cargarhistorial()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvHistorial.Items.Clear()
            SQL = "SELECT * FROM PensionAlimenticia "
            SQL &= " where fkiIdEmpleadoC=" & gIdEmpleado
            SQL &= " order by Nombrebeneficiario "
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem

            If rwFilas Is Nothing = False Then

                For Each Fila In rwFilas

                    fkidbanco = Fila.Item("fkIidBanco")
                    iEstatus = Fila.Item("iEstatus")
                    Dim banco As DataRow() = nConsulta("select * from bancos where iIdBanco = " & fkidbanco)
                    item = lsvHistorial.Items.Add(Fila.Item("Nombrebeneficiario"))
                    ' item.SubItems.Add("" & Fila.Item("Nombrebeneficiario"))
                    item.SubItems.Add("" & Fila.Item("fPorcentaje"))
                    item.SubItems.Add("" & banco(0).Item("cBanco"))
                    item.SubItems.Add("" & Fila.Item("Clabe"))
                    item.SubItems.Add("" & Fila.Item("Cuenta"))
                    item.Tag = Fila.Item("iIdPensionAlimenticia")
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
        txtBeneficiario.Focus()
        txtBeneficiario.TabIndex = 1
        nudPorcentaje.TabIndex = 2
        cbobanco.TabIndex = 3
        txtClabe.TabIndex = 4
        cboEstatus.TabIndex = 5
    End Sub
    Private Sub MostrarBancos()
        SQL = "Select * from bancos order by cBanco"
        nCargaCBO(cbobanco, SQL, "cBanco", "iIdBanco")
        cbobanco.SelectedIndex = 0
    End Sub



    Private Sub Limpiar(ByVal Contenedor As Object)


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

                    idPension = lsvHistorial.SelectedItems(0).Tag


                    txtBeneficiario.Text = lsvHistorial.SelectedItems(0).SubItems(0).Text
                    nudPorcentaje.Value = lsvHistorial.SelectedItems(0).SubItems(1).Text
                    cbobanco.SelectedValue = fkidbanco 'lsvHistorial.SelectedItems(0).SubItems(2).Text
                    txtClabe.Text = lsvHistorial.SelectedItems(0).SubItems(3).Text
                    txtCuenta.Text = lsvHistorial.SelectedItems(0).SubItems(4).Text
                    cboEstatus.SelectedIndex = IIf(iEstatus = "1", 1, 0)

                    cmdAgregar.Enabled = True
                    'Tipo = "1"
                    blnNuevo = False
                    MessageBox.Show("Pensión lista para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdAgregar_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAgregar.Click

        Try
            If txtBeneficiario.Text = "" Then
                MessageBox.Show("Agrege el nombre del beneficiario", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If blnNuevo = False Then
                SQL = "UPDATE PensionAlimenticia SET "
                SQL &= "fPorcentaje=" & nudPorcentaje.Value & ","
                SQL &= "Nombrebeneficiario='" & txtBeneficiario.Text & "', fkiIdBanco=" & cbobanco.SelectedValue & ","
                SQL &= "Clabe='" & txtClabe.Text & "', Cuenta='" & txtCuenta.Text & "',"
                SQL &= "iEstatus=" & cboEstatus.SelectedIndex
                SQL &= " where iIdPensionAlimenticia=" & idPension

            Else
                SQL = "EXEC setPensionAlimenticiaInsertar 0,"
                SQL &= gIdEmpleado & ", " & nudPorcentaje.Value & ","
                SQL &= "'" & txtBeneficiario.Text & "', " & cbobanco.SelectedValue & ","
                SQL &= "'" & txtClabe.Text & "','" & txtCuenta.Text & "', "
                SQL &= cboEstatus.SelectedIndex

            End If

            If nExecute(SQL) = False Then
                Exit Sub
            End If
            'cmdAgregar.Enabled = False
            txtBeneficiario.Text = ""
            txtClabe.Text = ""
            txtCuenta.Text = ""
            nudPorcentaje.Value = "0.0"
            cbobanco.SelectedIndex = 0

            blnNuevo = True
            MessageBox.Show("Datos guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


            Cargarhistorial()
        Catch ex As Exception

        End Try

    End Sub

  
End Class