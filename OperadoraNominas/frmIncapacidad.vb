Public Class frmIncapacidad
    Dim blnNuevo As Boolean = True
    Dim IdIncapacidad As String
    Public gIdEmpleado As String




    Private Sub frmIncapacidad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListarIncapacidad()

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
                Mensaje = "Por favor indique el folio"
            End If
            If nudDias.Value = "0" And Mensaje = "" Then
                Mensaje = "Por favor indique el numero de dias"
            End If

            If Mensaje <> "" Then
                MessageBox.Show(Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If blnNuevo Then
                'Insertar nuevo
                SQL = "EXEC setIncapacidadInsertar 0," & gIdEmpleado & ",'" & txtFolio.Text
                SQL &= "'," & cboTipo.SelectedIndex
                SQL &= "," & nudDias.Value
                SQL &= ",'" & dtpFechaInicio.Value.ToShortDateString
                SQL &= "','" & calculofechafinal(dtpFechaInicio.Value, nudDias.Value)
                SQL &= "'," & cboRamoSeguro.SelectedIndex
                SQL &= "," & cboriesgo.SelectedIndex
                SQL &= "," & nudPorcentaje.Value
                SQL &= ",1"
            Else
                'Actualizar

                SQL = "EXEC setIncapacidadActualizar " & IdIncapacidad & "," & gIdEmpleado & ",'" & txtFolio.Text
                SQL &= "'," & cboTipo.SelectedIndex
                SQL &= "," & nudDias.Value
                SQL &= ",'" & dtpFechaInicio.Value.ToShortDateString
                SQL &= "','" & calculofechafinal(dtpFechaInicio.Value, nudDias.Value)
                SQL &= "'," & cboRamoSeguro.SelectedIndex
                SQL &= "," & cboriesgo.SelectedIndex
                SQL &= "," & nudPorcentaje.Value
                SQL &= ",1"

            End If
            If nExecute(Sql) = False Then
                Exit Sub
            End If

            MessageBox.Show("Los datos de la incapacidad se han dado de alta correctamente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            ListarIncapacidad()
            pnlDatos.Enabled = False
        Catch ex As Exception

        End Try
    End Sub

    Function calculofechafinal(ByVal fechainicio As Date, ByVal dias As Integer) As String
        Dim fechafinal As String = ""
        Dim fechainicial As Date = fechainicio

        Return ((fechainicial.AddDays(dias - 1))).ToShortDateString()

    End Function

    Private Sub ListarIncapacidad()
        Dim SQL As String
        Dim tipoincidencia As String = ""
        Dim Alter As Boolean = False
        Try
            SQL = "SELECT * from Incapacidad WHERE fkiIdEmpleado=" & gIdEmpleado
            SQL &= " ORDER BY FechaInicio"

            lsvLista.Items.Clear()

            Dim item As ListViewItem
            Dim rwFolios As DataRow() = nConsulta(SQL)
            If rwFolios Is Nothing = False Then
                For Each Fila In rwFolios




                    item = lsvLista.Items.Add("" & Fila.Item("Folio"))

                    If Fila.Item("TipoIncidencia") = "0" Then
                        tipoincidencia = "Accidente de trabajo"
                    ElseIf Fila.Item("TipoIncidencia") = "1" Then
                        tipoincidencia = "Accidente de trayecto"
                    ElseIf Fila.Item("TipoIncidencia") = "2" Then
                        tipoincidencia = "Enfermedad general"
                    ElseIf Fila.Item("TipoIncidencia") = "3" Then
                        tipoincidencia = "Incapacidad pagada por la empresa"
                    ElseIf Fila.Item("TipoIncidencia") = "4" Then
                        tipoincidencia = "Incapacidad por maternidad"
                    End If

                    item.SubItems.Add("" & tipoincidencia)

                    item.SubItems.Add("" & Fila.Item("Dias"))

                    item.Tag = Fila.Item("iIdIncapacidad")
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




    Private Sub MostrarDatosIcapacidad(ByVal id As String)
        Dim sql As String
        IdIncapacidad = id
        sql = "select * from Incapacidad where iIdIncapacidad = " & id
        Dim rwFilas As DataRow() = nConsulta(Sql)
        Try
            If rwFilas Is Nothing = False Then
                blnNuevo = False
                pnlDatos.Enabled = True
                Dim Fila As DataRow = rwFilas(0)

                txtFolio.Text = Fila.Item("Folio")
                cboTipo.SelectedIndex = Fila.Item("TipoIncidencia")
                nudDias.Value = Fila.Item("Dias")
                dtpFechaInicio.Value = Fila.Item("Fechainicio")
                cboRamoSeguro.SelectedIndex = Fila.Item("RamoRiesgo")
                cboriesgo.SelectedIndex = Fila.Item("TipoRiesgo")
                nudPorcentaje.Value = Fila.Item("Porcentaje")



            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub lsvLista_ItemActivate1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvLista.ItemActivate
        If lsvLista.SelectedItems.Count > 0 Then
            MostrarDatosIcapacidad(lsvLista.SelectedItems(0).Tag)
        End If
    End Sub

    Private Sub lsvLista_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsvLista.SelectedIndexChanged

    End Sub
End Class
