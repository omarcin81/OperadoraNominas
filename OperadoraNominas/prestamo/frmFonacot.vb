Public Class frmFonacot
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Private idFonacot As String
    ' Dim Tipo As String
    ' Dim fkidbanco As String
    Dim iEstatus As String
    Dim blnNuevo As Boolean
    Dim SQL As String

    Private Sub frmFonacot_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cargarhistorial()



        blnNuevo = True
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtCredito.Enabled = False
        nudImporte.Enabled = False
        cboEstatus.Enabled = False
        cmdDeleted.Enabled = False
    End Sub

    Private Sub Cargarhistorial()
        Dim SQL As String, Alter As Boolean = False
        Try
            lsvHistorial.Items.Clear()
            SQL = "SELECT * FROM Fonacot"
            SQL &= " where fkiIdEmpleadoC=" & gIdEmpleado
            'SQL &= " AND iEstatus=1"
            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem

            If rwFilas Is Nothing = False Then

                For Each Fila In rwFilas

                    iEstatus = Fila.Item("iEstatus")
                    item = lsvHistorial.Items.Add(Fila.Item("iIdFonacot"))
                    ' item.SubItems.Add("" & Fila.Item("iId"))
                    item.SubItems.Add("" & Fila.Item("NumCredito"))
                    item.SubItems.Add("" & Fila.Item("ImporteMensual"))
                    item.Tag = Fila.Item("iIdFonacot")
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

        txtCredito.Text = ""
        nudImporte.Text = "0.0"
        cboEstatus.SelectedIndex = 0
        cmdguardar.Enabled = False
        cmdcancelar.Enabled = False
        txtCredito.Enabled = False
        nudImporte.Enabled = False
        cboEstatus.Enabled = False
        cmdDeleted.Enabled = False
    End Sub





    Private Sub lsvHistorial_ItemActivate_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsvHistorial.ItemActivate

        'Verificar si se tienen permisos
        Dim Sql As String
        Dim importefonacot As Int32
        Sql = "select * from usuarios where idUsuario = " & idUsuario
        Dim rwFilas As DataRow() = nConsulta(Sql)
        ' Dim Forma As New frmTipoEmpresa
        Try
            If rwFilas Is Nothing = False Then


                Dim Fila As DataRow = rwFilas(0)
                If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3") Then

                    'Pasar los datos

                    idFonacot = lsvHistorial.SelectedItems(0).Tag


                    txtCredito.Text = lsvHistorial.SelectedItems(0).SubItems(1).Text

                    nudImporte.Text = lsvHistorial.SelectedItems(0).SubItems(2).Text
                    cboEstatus.SelectedIndex = IIf(iEstatus = "1", 1, 0)


                    cmdguardar.Enabled = True
                    cmdcancelar.Enabled = True
                    txtCredito.Enabled = True
                    nudImporte.Enabled = True
                    cboEstatus.Enabled = True
                    cmdDeleted.Enabled = True
                    blnNuevo = False
                    MessageBox.Show("FONACOT listop para editar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    MessageBox.Show("No tiene permisos para esta ventana" & vbCrLf & "Comuniquese con el administrador del sistema", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdguardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdguardar.Click
        Try
            If txtCredito.Text = "" Then
                MessageBox.Show("Agrege el número de credito", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If blnNuevo = False Then
                SQL = "EXEC setFonacotActualizar "
                SQL &= idFonacot & ","
                SQL &= "'" & txtCredito.Text & "',"
                SQL &= "'" & nudImporte.Text & "',"
                SQL &= cboEstatus.SelectedIndex



            Else
                SQL = "EXEC setFonacotInsertar 0,"
                SQL &= gIdEmpleado & ", '" & txtCredito.Text & "',"
                SQL &= "'" & nudImporte.Text & "', "
                SQL &= cboEstatus.SelectedIndex

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
        txtCredito.Enabled = True
        nudImporte.Enabled = True
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

                SQL = "DELETE FROM Fonacot where iIdFonacot =" & datos(0).SubItems(0).Text & ""

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