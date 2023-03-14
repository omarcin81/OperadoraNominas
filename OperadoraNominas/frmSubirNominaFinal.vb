Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Mime.MediaTypeNames

Public Class frmSubirNominaFinal
    Public gIdPeriodo As String
    Public gIdSerie As String
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String
    Public gAnioActual As String
    Private Sub frmSubirIncidencias_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try

            cargarperiodos()
            cboperiodo.SelectedValue = gIdPeriodo
            cboserie.SelectedIndex = gIdSerie
            cboTipo.SelectedIndex = 0
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



                    lsvLista.Columns(1).Width = 75 'Empleado
                    'lsvLista.Columns(2).Width = 100  'ISR
                    lsvLista.Columns(4).Width = 100 '#Control
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
            tsbNuevo.Enabled = True
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
            Dim resultado As Integer = MessageBox.Show("se actualizaran los datos de " & cboTipo.Text & ",¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then


                For Each producto As ListViewItem In lsvLista.CheckedItems
                    pnlProgreso.Visible = True
                    pnlCatalogo.Enabled = False
                    pgbProgreso.Minimum = 0
                    pgbProgreso.Value = 0
                    pgbProgreso.Maximum = lsvLista.CheckedItems.Count

                    If producto.Index >= (CInt(NudFilaI.Value) - 1) And producto.Index <= (CInt(NudFilaF.Value) - 1) Then
                        'If producto.SubItems(CInt(NudColumnaC.Value)).Text <> "" Then
                        SQL = "select * from EmpleadosC where cCodigoEmpleado=" & IIf(producto.SubItems(CInt(NudColumnaN.Value)).Text = "", "0", producto.SubItems(CInt(NudColumnaN.Value)).Text)

                        Dim rwEmpleado As DataRow() = nConsulta(SQL)
                        If rwEmpleado Is Nothing = False Then


                            SQL = "EXEC etConcentradoTimbradoInsertar  0,'"
                            SQL &= producto.SubItems(CInt(NudColumnaN.Value)).Text & "'," ' CODIGO
                            SQL &= rwEmpleado(0)("iIdEmpleadoC").ToString & "', '" 'ID
                            SQL &= rwEmpleado(0)("cNombre").ToString & "', '"
                            SQL &= rwEmpleado(0)("cApellidoP").ToString & "', '"
                            SQL &= rwEmpleado(0)("cApellidoM").ToString & "', '"
                            SQL &= producto.SubItems(2).Text & "', '" 'rfc
                            SQL &= producto.SubItems(4).Text & "', '" 'curp
                            SQL &= rwEmpleado(0)("clabe2").ToString & "', '"
                            SQL &= producto.SubItems(6).Text & "', '" 'clabe
                            SQL &= "0', '" 'gasto
                            SQL &= "0', '" 'costo
                            SQL &= "0'," & producto.SubItems(33).Text & " '" '@Aguinaldo_G
                            SQL &= "0', '" '@Aguinaldo_E


                            If nExecute(SQL) = False Then
                                MessageBox.Show("Error en el registro con los siguiente datos:   Empleado:  " & producto.SubItems(CInt(NudColumnaN.Value)).Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                tsbCancelar_Click(sender, e)
                                pnlProgreso.Visible = False
                                Exit Sub
                            End If
                        End If

                    End If


                    ' End If
                    pgbProgreso.Value += 1
                    'Application.DoEvents()
                    'mandar el reporte
                Next


                tsbCancelar_Click(sender, e)
                pnlProgreso.Visible = False
                MessageBox.Show("Actualizacion procesadas", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

End Class