Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Mime.MediaTypeNames

Public Class frmSubirIncidencias
    Public gIdPeriodo As String

    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String
    Private Sub frmSubirIncidencias_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try

            cargarperiodos()
            cboperiodo.SelectedValue = gIdPeriodo
            cboIncidencia.SelectedIndex = 0
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub cargarperiodos()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select (CONVERT(nvarchar(12),dFechaInicio,103) + ' - ' + CONVERT(nvarchar(12),dFechaFin,103)) as dFechaInicio,iIdPeriodo  from periodos where iEstatus=1 order by iEjercicio,iNumeroPeriodo"
            nCargaCBO(cboperiodo, sql, "dFechainicio", "iIdPeriodo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub tsbNuevo_Click(sender As System.Object, e As System.EventArgs) Handles tsbNuevo.Click
        tsbNuevo.Enabled = False
        tsbImportar.Enabled = True
        tsbImportar_Click(sender, e)
    End Sub

    Private Sub tsbImportar_Click(sender As System.Object, e As System.EventArgs) Handles tsbImportar.Click
        Dim dialogo As New OpenFileDialog
        lblRuta.Text = ""
        With dialogo
            .Title = "Excel"
            .Filter = "Hoja de cálculo de excel (.xlsx)|*.xlsx| Excel (*.xls)|*.xls"
            .CheckFileExists = True
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                lblRuta.Text = .FileName
            End If
        End With
        tsbProcesar.Enabled = lblRuta.Text.Length > 0
        If tsbProcesar.Enabled Then
            tsbProcesar_Click(sender, e)
        End If
    End Sub

    Private Sub tsbProcesar_Click(sender As System.Object, e As System.EventArgs) Handles tsbProcesar.Click
        lsvLista.Items.Clear()
        lsvLista.Columns.Clear()
        lsvLista.Clear()

        pnlCatalogo.Enabled = False
        tsbGuardar.Enabled = False
        tsbCancelar.Enabled = False

        lsvLista.Visible = False
        tsbImportar.Enabled = False
        Me.cmdCerrar.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        ' Application.DoEvents()

        Try
            If File.Exists(lblRuta.Text) Then
                Dim Archivo As String = lblRuta.Text
                Dim Hoja As String


                Dim book As New ClosedXML.Excel.XLWorkbook(Archivo)
                If book.Worksheets.Count >= 1 Then
                    sheetIndex = 1
                    If book.Worksheets.Count >= 1 Then
                        Dim Forma As New frmHojasNomina
                        Dim Hojas As String = ""
                        For i As Integer = 0 To book.Worksheets.Count - 1
                            Hojas &= book.Worksheets(i).Name & IIf(i < (book.Worksheets.Count - 1), "|", "")
                        Next
                        Forma.Hojas = Hojas
                        If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
                            sheetIndex = Forma.selectedIndex + 1
                        Else
                            Exit Sub
                        End If
                    End If
                    Hoja = book.Worksheet(sheetIndex).Name
                    Dim sheet As IXLWorksheet = book.Worksheet(sheetIndex)

                    Dim colIni As Integer = sheet.FirstColumnUsed().ColumnNumber()
                    Dim colFin As Integer = sheet.LastColumnUsed().ColumnNumber()
                    Dim Columna As String
                    Dim numerocolumna As Integer = 1


                    lsvLista.Columns.Add("#")
                    For c As Integer = colIni To colFin

                        lsvLista.Columns.Add(numerocolumna)
                        numerocolumna = numerocolumna + 1

                    Next



                    'lsvLista.Columns(1).Width = 400 'Empleado
                    'lsvLista.Columns(2).Width = 100  'ISR
                    'lsvLista.Columns(3).Width = 50 '#Control
                    'lsvLista.Columns(4).Width = 100 'ap
                    'lsvLista.Columns(5).Width = 100 'am
                    'lsvLista.Columns(6).Width = 100 'nombre
                    'lsvLista.Columns(7).Width = 100 'isr
                    'lsvLista.Columns(8).Width = 200 'imss
                    'lsvLista.Columns(9).Width = 50 'dias
                    'lsvLista.Columns(10).Width = 100 'banco
                    'lsvLista.Columns(11).Width = 150 'clabe
                    'lsvLista.Columns(12).Width = 150 'cuenta
                    'lsvLista.Columns(13).Width = 150 'curp
                    'lsvLista.Columns(14).Width = 350 'rfc
                    'lsvLista.Columns(15).Width = 350


                    Dim Filas As Long = sheet.RowsUsed().Count()
                    For f As Integer = 1 To Filas
                        Dim item As ListViewItem = lsvLista.Items.Add(f.ToString())
                        For c As Integer = colIni To colFin
                            Try

                                Dim Valor As String = ""
                                If (sheet.Cell(f, c).ValueCached Is Nothing) Then
                                    Valor = sheet.Cell(f, c).Value.ToString()
                                Else
                                    Valor = sheet.Cell(f, c).ValueCached.ToString()
                                End If
                                Valor = Valor.Trim()
                                item.SubItems.Add(Valor)


                                If f = 6 And c >= 12 Then


                                    item.SubItems(item.SubItems.Count - 1).Text = Valor
                                End If



                            Catch ex As Exception

                            End Try

                        Next
                    Next

                    book.Dispose()
                    book = Nothing
                    GC.Collect()

                    pnlCatalogo.Enabled = True
                    If lsvLista.Items.Count = 0 Then
                        MessageBox.Show("El catálogo no puso ser importado o no contiene registros." & vbCrLf & "¿Por favor verifique?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Se han encontrado " & FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        tsbGuardar.Enabled = True
                        tsbCancelar.Enabled = True
                        lblRuta.Text = FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo."
                        Me.Enabled = True
                        Me.cmdCerrar.Enabled = True
                        Me.Cursor = Cursors.Default
                        tsbImportar.Enabled = True

                        lsvLista.Visible = True
                    End If




                ElseIf book.Worksheets.Count = 0 Then
                    MessageBox.Show("El archivo no contiene hojas.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("El archivo ya no se encuentra en la ruta indicada.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            tsbCancelar_Click(sender, e)
            MessageBox.Show(ex.Message.ToString)
            Me.Close()

        End Try
    End Sub

    Private Sub tsbCancelar_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancelar.Click
        pnlCatalogo.Enabled = False
        lsvLista.Items.Clear()
        chkAll.Checked = False
        lblRuta.Text = ""
        tsbImportar.Enabled = False
        tsbCancelar.Enabled = False
        tsbNuevo.Enabled = True
        pnlProgreso.Visible = False
    End Sub

    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            item.Checked = chkAll.Checked
        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub

    Private Sub tsbGuardar_Click(sender As System.Object, e As System.EventArgs) Handles tsbGuardar.Click
        Try


            
            'Application.DoEvents()
            'preguntar si los datos son correctos
            Dim resultado As Integer = MessageBox.Show("se actualizaran los datos de " & cboIncidencia.Text & ",¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then
                SQL = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                SQL &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=0" '& cboserie.SelectedIndex
                SQL &= " and iTipoNomina=0"

                Dim rwNominaGuardadaFinal As DataRow() = nConsulta(SQL)

                If rwNominaGuardadaFinal Is Nothing = False Then
                    MessageBox.Show("La nomina ya esta marcada como final, no  se pueden guardar cambios", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    pnlProgreso.Visible = True
                    pnlCatalogo.Enabled = False
                    pgbProgreso.Minimum = 0
                    pgbProgreso.Value = 0
                    pgbProgreso.Maximum = lsvLista.CheckedItems.Count

                    Select Case cboIncidencia.SelectedIndex
                        Case 0
                            'tiempo extra doble
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fTExtra2V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Integer = Integer.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fTExtra2V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fTExtra2V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If


                                            
                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 1
                            'tiempo extra triple
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fTExtra3V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Integer = Integer.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fTExtra3V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fTExtra3V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 2
                            'descanso laborado
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fDescansoLV=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Integer = Integer.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fDescansoLV=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fDescansoLV").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 3
                            'descanso laborado
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fDiaFestivoLV=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Integer = Integer.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fDiaFestivoLV=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fDiaFestivoLV").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 4
                            'prima dominical valor
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fPrima_Dominical_V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Integer = Integer.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fPrima_Dominical_V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fPrima_Dominical_V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 5
                            'Bono asistencia
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fBonoAsistencia=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fBonoAsistencia=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fBonoAsistencia").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 6
                            'Bono productividad
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fBonoProductividad=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fBonoProductividad=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fBonoProductividad").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 7
                            'Bono polivalencia
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fBonoPolivalencia=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fBonoPolivalencia=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fBonoPolivalencia").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 8
                            'Bono especialidad
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fBonoEspecialidad=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fBonoEspecialidad=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fBonoEspecialidad").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 9
                            'Bono CALIDAD
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fBonoCalidad=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fBonoCalidad=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fBonoCalidad").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 10
                            'TIEMPO NO LABORADO
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fT_No_laborado_V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fT_No_laborado_V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fT_No_laborado_V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 11
                            'FALTAS INJUSTIFICADA
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fFalta_Injustificada_V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fFalta_Injustificada_V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fFalta_Injustificada_V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 12
                            'PERMISO CON GOCE DE SUELDO
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fPermiso_Sin_GS_V=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fPermiso_Sin_GS_V=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fPermiso_Sin_GS_V").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 13
                            'SUELDO PENDIENTE
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fSemanaFondo=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fSemanaFondo=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fSemanaFondo").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next
                        Case 14
                            'COMPENSACION
                            If chkIncidencia0.Checked Then
                                SQL = " update nomina set fCompensacion=0"
                                SQL &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                                If nExecute(SQL) = False Then
                                    MessageBox.Show("Error al poner en 0 esta incidencia:" & cboIncidencia.Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    'pnlProgreso.Visible = False
                                    Exit Sub
                                End If
                            End If

                            For Each producto As ListViewItem In lsvLista.CheckedItems
                                If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                                    If CDbl(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text)) > 0 Then
                                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                                        If rwEmpleado Is Nothing = False Then
                                            SQL = "select * from nomina where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString
                                            SQL &= " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                            Dim rwEmpleadoNomina As DataRow() = nConsulta(SQL)
                                            If rwEmpleadoNomina Is Nothing = False Then
                                                'verificar si se quiere actualizar a 0

                                                Dim horasextrasdobles As Double = Double.Parse(IIf(producto.SubItems(CInt(NudColumnaC.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaC.Value)).Text))
                                                SQL = " update nomina set fCompensacion=" & horasextrasdobles + Double.Parse(rwEmpleadoNomina(0)("fCompensacion").ToString)
                                                SQL &= " where fkiIdEmpleadoC=" & rwEmpleado(0)("iIdEmpleadoC").ToString & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
                                                If nExecute(SQL) = False Then
                                                    MessageBox.Show("Error al agregar a " & rwEmpleado(0)("cNombreLargo").ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                                    'pnlProgreso.Visible = False
                                                    Exit Sub
                                                End If
                                            Else
                                                MessageBox.Show("El empleado " & rwEmpleado(0)("cNombreLargo").ToString & " no esta en la nomina, verique el alta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                            End If



                                        End If

                                    End If


                                End If
                                pgbProgreso.Value += 1
                                'Application.DoEvents()
                                'mandar el reporte
                            Next


                    End Select
                    tsbCancelar_Click(sender, e)
                    pnlProgreso.Visible = False
                    MessageBox.Show("Incidencias procesadas", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If


                
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class