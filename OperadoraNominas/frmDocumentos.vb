Public Class frmDocumentos
    Dim blnNuevo As Boolean = True
    Dim IdDocumento As String
    Public gIdEmpleado As String




    Private Sub frmDocumentos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListarDocumentos()
        listar()
    End Sub

    Private Sub Limpiar(Optional ByRef Contenedor = Nothing)
        Dim aControl As Control
        For Each aControl In Contenedor.Controls
            If TypeOf aControl Is TextBox Then
                CType(aControl, TextBox).Text = ""
            ElseIf TypeOf aControl Is ComboBox Then
                CType(aControl, ComboBox).SelectedIndex = -1
                CType(aControl, ComboBox).Text = ""
            ElseIf TypeOf aControl Is NumericUpDown Then
                CType(aControl, NumericUpDown).Value = 0
            ElseIf TypeOf aControl Is CheckBox Then
                CType(aControl, CheckBox).Checked = False
            ElseIf TypeOf aControl Is ListView Then
                CType(aControl, ListView).Items.Clear()
            ElseIf TypeOf aControl Is GroupBox Or TypeOf aControl Is Panel Then
                Limpiar(aControl)
            End If
        Next
    End Sub

    Private Sub pnlDatos_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlDatos.EnabledChanged
        Limpiar(pnlDatos)

        tsbNuevo.Enabled = Not pnlDatos.Enabled
        tsbGuardar.Enabled = pnlDatos.Enabled
        tsbCancelar.Enabled = pnlDatos.Enabled
        tsbSalir.Enabled = Not pnlDatos.Enabled

    End Sub

    Private Sub tsbNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNuevo.Click
        blnNuevo = True
        pnlDatos.Enabled = True
    End Sub

    Private Sub tsbGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbGuardar.Click
        Dim SQL As String, Mensaje As String = ""
        Try
            If txtFolio.Text.Trim.Length = 0 And Mensaje = "" Then
                Mensaje = "Por favor indique el Nombre"
            End If
            If dtpFechaVenc.Value.ToString = "" And Mensaje = "" Then
                Mensaje = "Por favor indique la fecha de nacimiento"
            End If

            If Mensaje <> "" Then
                MessageBox.Show(Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If blnNuevo Then
                'Insertar nuevo
                SQL = "EXEC setdocumentosInsertar 0,'" & txtFolio.Text
                SQL &= "','" & cboTipo.SelectedValue
                SQL &= "','" & Format(dtpFechaVenc.Value, "yyyy/dd/MM")
                '' SQL &= "'," & 1
                SQL &= "','" & gIdEmpleado & "'"
                '' SQL &= "'," & cboTipo.SelectedIndex + 1

            Else
                'Actualizar

                SQL = "EXEC setdocumentosActualizar " & IdDocumento & ",'" & txtFolio.Text
                SQL &= "','" & cboTipo.SelectedValue
                SQL &= "','" & Format(dtpFechaVenc.Value, "yyyy/dd/MM")
                '' SQL &= "'," & 1
                SQL &= "','" & gIdEmpleado & "'"
                '' SQL &= "'," & cboTipo.SelectedIndex + 1


            End If
            If nExecute(Sql) = False Then
                Exit Sub
            End If

            MessageBox.Show("Datos guardados correctamente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            listar()

            pnlDatos.Enabled = False
        Catch ex As Exception

        End Try
    End Sub

    Public Sub listarDocumentos()

        Dim sql As String
        sql = "SELECT * FROM TipoDocumento order by cDescripcion"
        nCargaCBO(cboTipo, sql, "cDescripcion", "iIdTipoDocumento")
        cboTipo.SelectedIndex = 0

    End Sub

    Private Sub listar()
        Dim SQL As String
        Dim tipoDoc As String = ""
        Dim Alter As Boolean = False

        Try
            SQL = "SELECT * from documentos WHERE fkiIdEmpleadoC=" & gIdEmpleado
            SQL &= " ORDER BY fkiIdTipoDocumento"

            lsvLista.Items.Clear()

            Dim item As ListViewItem
            Dim rwFolios As DataRow() = nConsulta(SQL)
            If rwFolios Is Nothing = False Then
                For Each Fila In rwFolios

                    Dim cTipo As DataRow() = nConsulta("SELECT * FROM TipoDocumento where iIdTipoDocumento=" & Fila.Item("fkiIdTipoDocumento"))

                    tipoDoc = cTipo(0).Item("cDescripcion")

                    item = lsvLista.Items.Add("" & Fila.Item("cCodigo"))

                    item.SubItems.Add("" & tipoDoc)
                    item.SubItems.Add("" & Fila.Item("dFechaVencimiento"))
                    item.Tag = Fila.Item("iIdDocumentos")

                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter


                Next

                MessageBox.Show(rwFolios.Count & " Registros encontrados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tsbCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCancelar.Click
        pnlDatos.Enabled = False
    End Sub

    Private Sub tsbSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSalir.Click
        Me.Close()
    End Sub




    Private Sub MostrarDatosFamilia(ByVal id As String)
        Dim sql As String
        IdDocumento = id
        sql = "SELECT * from documentos where iIdDocumentos=" & id
        Dim rwFilas As DataRow() = nConsulta(sql)
        Try
            If rwFilas Is Nothing = False Then
                blnNuevo = False
                pnlDatos.Enabled = True
                Dim Fila As DataRow = rwFilas(0)

                txtFolio.Text = Fila.Item("cCodigo")
                cboTipo.SelectedValue = Fila.Item("fkiIdTipoDocumento")
                dtpFechaVenc.Value = Fila.Item("dFechaVencimiento")



            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub lsvLista_ItemActivate1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvLista.ItemActivate
        If lsvLista.SelectedItems.Count > 0 Then
            MostrarDatosFamilia(lsvLista.SelectedItems(0).Tag)
        End If
    End Sub

    

  
End Class
