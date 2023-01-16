Imports System.IO
Imports ClosedXML.Excel

Public Class frmNominaFinalE
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String
    Dim empleadotmp As String
    Public dsReporte As New DataSet

    Private Sub tsbNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNuevo.Click
        Dim dialogo As New OpenFileDialog
        lblRuta.Text = ""
        With dialogo
            .Title = "Búsqueda de archivos de saldos."
            .Filter = "Hoja de cálculo de excel (xlsx)|*.xlsx;"
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

    Private Sub tsbProcesar_Click(ByVal sender As Object, ByVal e As EventArgs)
        lsvLista.Items.Clear()
        lsvLista.Columns.Clear()
        lsvLista.Clear()
        pnlProgreso.Visible = True

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

                    Dim colIni As Integer = sheet.FirstColumnUsed().ColumnNumber() - 1
                    Dim colFin As Integer = sheet.LastColumnUsed().ColumnNumber()
                    Dim Columna As String
                    Dim numerocolumna As Integer = 0

                    pgbProgreso.Minimum = 0
                    pgbProgreso.Value = 0
                    pgbProgreso.Maximum = sheet.LastRowUsed.RowNumber() + 7

                    ' lsvLista.Columns.Add("#")

                    cargarColumnas()
                    For c As Integer = colIni To colFin


                        numerocolumna = numerocolumna + 1

                    Next


                    Dim Filas As Long = sheet.RowsUsed().Count()

                    'Application.DoEvents()

                    For f As Integer = 1 To Filas + 7
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
                                'MessageBox.Show("DENTRO DEL FOR " & f & "-" & c & ex.Message.ToString)
                            End Try


                        Next
                        pgbProgreso.Value += 1
                    Next

                    book.Dispose()
                    book = Nothing
                    GC.Collect()

                    pnlCatalogo.Enabled = True
                    If lsvLista.Items.Count = 0 Then
                        MessageBox.Show("El catálogo no puso ser importado o no contiene registros." & vbCrLf & "¿Por favor verifique?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Se han encontrado " & FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'tsbBuscar.Enabled = True
                        'tsbCancelar.Enabled = True
                        lblRuta.Text = FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo."
                        'Me.Enabled = True
                        ' Me.cmdCerrar.Enabled = True
                        ' Me.Cursor = Cursors.Default
                        'tsbEnviar.Enabled = True
                        'lsvLista.Visible = True
                        pnlProgreso.Visible = False
                        pnlCatalogo.Enabled = True
                    End If

                ElseIf book.Worksheets.Count = 0 Then
                    MessageBox.Show("El archivo no contiene hojas.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("El archivo ya no se encuentra en la ruta indicada.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            'tsbCancelar_Click(sender, e)
            MessageBox.Show(ex.Message.ToString)
        End Try
    End Sub


    Private Sub frmNominaFinalE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lsvLista.Visible = True

        cargarColumnas()
        cargarperiodos()
    End Sub
    Private Sub cargarColumnas()
        lsvLista.Columns.Add("#")
        lsvLista.Columns.Add("CODIGO") '0
        lsvLista.Columns.Add("NOMBRE")
        lsvLista.Columns.Add("ISTATUS")
        lsvLista.Columns.Add("RFC")
        lsvLista.Columns.Add("CURP")
        lsvLista.Columns.Add("NSS")
        lsvLista.Columns.Add("FECHA_NAC")
        lsvLista.Columns.Add("EDAD")
        lsvLista.Columns.Add("PUESTO")
        lsvLista.Columns.Add("BUQUE")
        lsvLista.Columns.Add("TIPO_INFONAVIT")
        lsvLista.Columns.Add("VALOR_INFONAVIT")
        lsvLista.Columns.Add("SALARIO_DIARIO")
        lsvLista.Columns.Add("SDI")
        lsvLista.Columns.Add("DIAS_TRABAJADOS")
        lsvLista.Columns.Add("TIPO_INCAPACIDAD")
        lsvLista.Columns.Add("NUMERO_DIAS")
        lsvLista.Columns.Add("SUELDO_BASE")
        lsvLista.Columns.Add("TIEMPO_EXTRA_FIJO_GRAVADO")
        lsvLista.Columns.Add("TIEMPO_EXTRA_FIJO_EXENTO")
        lsvLista.Columns.Add("TIEMPO_EXTRA_OCASIONAL")
        lsvLista.Columns.Add("DESC_SEM_OBLIGATORIO")
        lsvLista.Columns.Add("VACACIONES_PROPORCIONALES")
        lsvLista.Columns.Add("AGUINALDO_GRAVADO")
        lsvLista.Columns.Add("AGUINALDO_EXENTO")
        lsvLista.Columns.Add("TOTAL_AGUINALDO")
        lsvLista.Columns.Add("P_VAC_GRAVADO")
        lsvLista.Columns.Add("P_VAC_EXENTO")
        lsvLista.Columns.Add("TOTAL_P_VAC")
        lsvLista.Columns.Add("TOTAL_PERCEPCIONES")
        lsvLista.Columns.Add("TOTAL_PERCEPCIONES_P_ISR")
        lsvLista.Columns.Add("INCAPACIDAD")
        lsvLista.Columns.Add("ISR")
        lsvLista.Columns.Add("IMSS")
        lsvLista.Columns.Add("INFONAVIT")
        lsvLista.Columns.Add("INFONAVIT_ANT")
        lsvLista.Columns.Add("INFONAVIT_BIM_ANT")
        lsvLista.Columns.Add("PENSION_ALIMENTICIA")
        lsvLista.Columns.Add("SUBSIDIO")
        lsvLista.Columns.Add("PRESTAMO")
        lsvLista.Columns.Add("FONACOT")
        lsvLista.Columns.Add("NETO_PAGAR")
        lsvLista.Columns.Add("IMSS_CS")
        lsvLista.Columns.Add("RCV_CS")
        lsvLista.Columns.Add("INFONAVIT_CS")
        lsvLista.Columns.Add("ISNOM_CS")
        lsvLista.Columns.Add("TOTAL_CS")
        lsvLista.Columns.Add("COSTO_SOCIAL")
        lsvLista.Columns.Add("Prestamo_Personal_Asimilado")
        lsvLista.Columns.Add("Adeudo_Infonavit_Asimilado")
        lsvLista.Columns.Add("Difencia_infonavit_Asimilado")
        lsvLista.Columns.Add("ASIMILADOS")
        lsvLista.Columns.Add("SUELDO_ORDINARIO")
        lsvLista.Columns.Add("iTipoNomina")
        lsvLista.Columns.Add("iSerie")
        lsvLista.Columns.Add("fkiIdPeriodo")
        lsvLista.Columns.Add("iEstatus")
        lsvLista.Columns.Add("fkiIdEmpleado")
    End Sub

    Private Sub cargarperiodos()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select (CONVERT(nvarchar(12),dFechaInicio,103) + ' - ' + CONVERT(nvarchar(12),dFechaFin,103)) as dFechaInicio,iIdPeriodo  from periodos where iEstatus=1 and iEjercicio>=2020 order by iEjercicio,iNumeroPeriodo"
            nCargaCBO(cboperiodo, sql, "dFechainicio", "iIdPeriodo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub tsbBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbBuscar.Click

        Dim SQL As String, Alter As Boolean = False
        Try
           
            SQL = "SELECT * FROM NominaFinal "
            SQL &= "WHERE fKiIdPeriodo= " & cboperiodo.SelectedValue
            SQL &= " AND iEstatus=1 "
            SQL &= IIf(chkbTodasSeries.Checked, " ", " AND iSerie=" & cboserie.SelectedIndex)
            SQL &= IIf(chkbTodo.Checked, "", "AND iTipoNomina=" & cboTipoNomina.SelectedIndex)
            SQL &= " ORDER BY fKiIdPeriodo,iSerie, iTipoNomina"



            Dim rwFilas As DataRow() = nConsulta(SQL)
            Dim item As ListViewItem
            lsvLista.Items.Clear()
            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    item = lsvLista.Items.Add(Fila.Item("iIdNominaFinal"))


                    item.SubItems.Add("" & Fila.Item("CODIGO"))
                    item.SubItems.Add("" & Fila.Item("NOMBRE"))
                    item.SubItems.Add("" & Fila.Item("ISTATUS"))
                    item.SubItems.Add("" & Fila.Item("RFC"))
                    item.SubItems.Add("" & Fila.Item("CURP"))
                    item.SubItems.Add("" & Fila.Item("NSS"))
                    item.SubItems.Add("" & Fila.Item("FECHA_NAC"))
                    item.SubItems.Add("" & Fila.Item("EDAD"))
                    item.SubItems.Add("" & Fila.Item("PUESTO"))
                    item.SubItems.Add("" & Fila.Item("BUQUE"))
                    item.SubItems.Add("" & Fila.Item("TIPO_INFONAVIT"))
                    item.SubItems.Add("" & Fila.Item("VALOR_INFONAVIT"))
                    item.SubItems.Add("" & Fila.Item("SALARIO_DIARIO"))
                    item.SubItems.Add("" & Fila.Item("SDI"))
                    item.SubItems.Add("" & Fila.Item("DIAS_TRABAJADOS"))
                    item.SubItems.Add("" & Fila.Item("TIPO_INCAPACIDAD"))
                    item.SubItems.Add("" & Fila.Item("NUMERO_DIAS"))
                    item.SubItems.Add("" & Fila.Item("SUELDO_BASE"))
                    item.SubItems.Add("" & Fila.Item("TIEMPO_EXTRA_FIJO_GRAVADO"))
                    item.SubItems.Add("" & Fila.Item("TIEMPO_EXTRA_FIJO_EXENTO"))
                    item.SubItems.Add("" & Fila.Item("TIEMPO_EXTRA_OCASIONAL"))
                    item.SubItems.Add("" & Fila.Item("DESC_SEM_OBLIGATORIO"))
                    item.SubItems.Add("" & Fila.Item("VACACIONES_PROPORCIONALES"))
                    item.SubItems.Add("" & Fila.Item("AGUINALDO_GRAVADO"))
                    item.SubItems.Add("" & Fila.Item("AGUINALDO_EXENTO"))
                    item.SubItems.Add("" & Fila.Item("TOTAL_AGUINALDO"))
                    item.SubItems.Add("" & Fila.Item("P_VAC_GRAVADO"))
                    item.SubItems.Add("" & Fila.Item("P_VAC_EXENTO"))
                    item.SubItems.Add("" & Fila.Item("TOTAL_P_VAC"))
                    item.SubItems.Add("" & Fila.Item("TOTAL_PERCEPCIONES"))
                    item.SubItems.Add("" & Fila.Item("TOTAL_PERCEPCIONES_P_ISR"))
                    item.SubItems.Add("" & Fila.Item("INCAPACIDAD"))
                    item.SubItems.Add("" & Fila.Item("ISR"))
                    item.SubItems.Add("" & Fila.Item("IMSS"))
                    item.SubItems.Add("" & Fila.Item("INFONAVIT"))
                    item.SubItems.Add("" & Fila.Item("INFONAVIT_ANT"))
                    item.SubItems.Add("" & Fila.Item("INFONAVIT_BIM_ANT"))
                    item.SubItems.Add("" & Fila.Item("PENSION_ALIMENTICIA"))
                    item.SubItems.Add("" & Fila.Item("SUBSIDIO"))
                    item.SubItems.Add("" & Fila.Item("PRESTAMO"))
                    item.SubItems.Add("" & Fila.Item("FONACOT"))
                    item.SubItems.Add("" & Fila.Item("NETO_PAGAR"))
                    item.SubItems.Add("" & Fila.Item("IMSS_CS"))
                    item.SubItems.Add("" & Fila.Item("RCV_CS"))
                    item.SubItems.Add("" & Fila.Item("INFONAVIT_CS"))
                    item.SubItems.Add("" & Fila.Item("ISNOM_CS"))
                    item.SubItems.Add("" & Fila.Item("TOTAL_CS"))
                    item.SubItems.Add("" & Fila.Item("COSTO_SOCIAL"))
                    item.SubItems.Add("" & Fila.Item("Prestamo_Personal_Asimilado"))
                    item.SubItems.Add("" & Fila.Item("Adeudo_Infonavit_Asimilado"))
                    item.SubItems.Add("" & Fila.Item("Difencia_infonavit_Asimilado"))
                    item.SubItems.Add("" & Fila.Item("ASIMILADOS"))
                    item.SubItems.Add("" & Fila.Item("SUELDO_ORDINARIO"))
                    item.SubItems.Add("" & Fila.Item("iTipoNomina"))
                    item.SubItems.Add("" & Fila.Item("iSerie"))
                    item.SubItems.Add("" & Fila.Item("fkiIdPeriodo"))
                    item.SubItems.Add("" & Fila.Item("iEstatus"))
                    item.SubItems.Add("" & Fila.Item("fkiIdEmpleado"))
                    

                    item.Tag = Fila.Item("iIdNominaFinal")
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter

                Next
            Else
                MsgBox("No se encontro ningun registro")
            End If
            If lsvLista.Items.Count > 0 Then
                lsvLista.Focus()
                lsvLista.Items(0).Selected = True
                lblRuta.Text = FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo."
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            item.Checked = chkAll.Checked
        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub
    Private Sub tsbEnviar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEnviar.Click
     

        Try
            Dim SQL2 As String, Alter As Boolean = False
            Dim resultado As Integer = MessageBox.Show("Esta a punto de cargar esta nomina, ¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then

                If lsvLista.CheckedItems.Count > 0 Then

                    pnlProgreso.Visible = True
                    pnlCatalogo.Enabled = False
                    'Application.DoEvents()

                    Dim i As Integer = 0
                    Dim conta As Integer = 0

                    pgbProgreso.Minimum = 0
                    pgbProgreso.Value = 0
                    pgbProgreso.Maximum = lsvLista.CheckedItems.Count


                    SQL2 = "SELECT * FROM NominaFinal "
                    SQL2 &= "WHERE iSerie=" & cboserie.SelectedIndex
                    SQL2 &= "AND fKiIdPeriodo= " & cboperiodo.SelectedValue & " AND iEstatus=1 "
                    SQL2 &= "and iTipoNomina=" & cboTipoNomina.SelectedIndex
                    SQL2 &= " ORDER BY fKiIdPeriodo, iTipoNomina"

                    Dim NominaE As DataRow() = nConsulta(SQL2)
                    If NominaE Is Nothing = False Then
                        Dim quest As Integer = MessageBox.Show("Existe una nomina con estos parementros, ¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
                        If quest = DialogResult.No Then
                            pnlProgreso.Visible = False
                            pnlCatalogo.Enabled = True
                            Exit Sub

                        End If
                    End If

                    For Each producto As ListViewItem In lsvLista.CheckedItems
                        SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(1).Text)
                        Dim rwFilas As DataRow() = nConsulta(SQL)

                        If rwFilas Is Nothing = False Then
                            If rwFilas.Length = 1 Then
                                producto.BackColor = Color.Green
                                For Each Fila In rwFilas
                                    SQL = "EXEC setNominaFinalInsertar 0"
                                    SQL &= ",'" & producto.SubItems(1).Text & "'"
                                    SQL &= ",'" & producto.SubItems(2).Text & "'"
                                    SQL &= ",'" & producto.SubItems(3).Text & "'"
                                    SQL &= ",'" & producto.SubItems(4).Text & "'"
                                    SQL &= ",'" & producto.SubItems(5).Text & "'"
                                    SQL &= ",'" & producto.SubItems(6).Text & "'"
                                    SQL &= ",'" & producto.SubItems(7).Text & "'"
                                    SQL &= ",'" & producto.SubItems(8).Text & "'"
                                    SQL &= ",'" & producto.SubItems(9).Text & "'"
                                    SQL &= ",'" & producto.SubItems(10).Text & "'"
                                    SQL &= ",'" & producto.SubItems(11).Text & "'"
                                    SQL &= ",'" & producto.SubItems(12).Text & "'"
                                    SQL &= "," & producto.SubItems(13).Text
                                    SQL &= "," & producto.SubItems(14).Text
                                    SQL &= ",'" & producto.SubItems(15).Text & "'"
                                    SQL &= ",'" & producto.SubItems(16).Text & "'"
                                    SQL &= "," & producto.SubItems(17).Text
                                    SQL &= "," & producto.SubItems(18).Text
                                    SQL &= "," & producto.SubItems(19).Text
                                    SQL &= "," & producto.SubItems(20).Text
                                    SQL &= "," & producto.SubItems(21).Text
                                    SQL &= "," & producto.SubItems(22).Text
                                    SQL &= "," & producto.SubItems(23).Text
                                    SQL &= "," & producto.SubItems(24).Text
                                    SQL &= "," & producto.SubItems(25).Text
                                    SQL &= "," & producto.SubItems(26).Text
                                    SQL &= "," & producto.SubItems(27).Text
                                    SQL &= "," & producto.SubItems(28).Text
                                    SQL &= "," & producto.SubItems(29).Text
                                    SQL &= "," & producto.SubItems(30).Text
                                    SQL &= "," & producto.SubItems(31).Text
                                    SQL &= "," & producto.SubItems(32).Text
                                    SQL &= "," & producto.SubItems(33).Text
                                    SQL &= "," & producto.SubItems(34).Text
                                    SQL &= "," & producto.SubItems(35).Text
                                    SQL &= "," & producto.SubItems(36).Text
                                    SQL &= "," & producto.SubItems(37).Text
                                    SQL &= "," & producto.SubItems(38).Text
                                    SQL &= "," & producto.SubItems(39).Text
                                    SQL &= "," & producto.SubItems(40).Text
                                    SQL &= "," & producto.SubItems(41).Text
                                    SQL &= "," & producto.SubItems(42).Text
                                    SQL &= "," & producto.SubItems(43).Text
                                    SQL &= "," & producto.SubItems(44).Text
                                    SQL &= "," & producto.SubItems(45).Text
                                    SQL &= "," & producto.SubItems(46).Text
                                    SQL &= "," & producto.SubItems(47).Text
                                    SQL &= "," & producto.SubItems(48).Text
                                    SQL &= "," & producto.SubItems(49).Text
                                    SQL &= "," & producto.SubItems(50).Text
                                    SQL &= "," & producto.SubItems(51).Text
                                    SQL &= "," & IIf(producto.SubItems(52).Text = "", 0, producto.SubItems(52).Text)
                                    SQL &= "," & IIf(producto.SubItems(53).Text = "", 0, producto.SubItems(53).Text)
                                    SQL &= "," & IIf(chkbTodo.Checked, producto.SubItems(54).Text, cboTipoNomina.SelectedIndex)
                                    SQL &= "," & IIf(chkbTodasSeries.Checked, producto.SubItems(55).Text, cboserie.SelectedIndex)
                                    SQL &= "," & cboperiodo.SelectedValue
                                    SQL &= ",1"
                                    SQL &= "," & Fila.Item("iIdEmpleadoC")

                                    If nExecute(SQL) = False Then
                                        MessageBox.Show("Ocurrio un error  en " & producto.SubItems(2).Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                                    End If
                                Next




                            End If
                            pgbProgreso.Value += 1
                        End If
                    Next
                    Me.DialogResult = Windows.Forms.DialogResult.OK

                    'If cboTipoNomina.SelectedIndex = 0 Then
                    '    MessageBox.Show("ABORDO se subio correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Else

                    MessageBox.Show("Proceso terminado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'End If

                Else
                    MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
                'pnlCatalogo.Enabled = True
                pnlProgreso.Visible = False
                cargarColumnas()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
        End Try

    End Sub


    Private Sub tsAcumulados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsAcumulados.Click
        Try
            Dim SQL, SQL2 As String
            Dim filaExcel As Integer = 0
            Dim filatmp As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo, iejercicio, iMes As String
            Dim SUMsubsidioGenerado As String
            Dim ruta As String


            Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
            If rwPeriodo0 Is Nothing = False Then
                Dim Fechafin As Date = rwPeriodo0(0).Item("dFechaFin")
                periodo = "1 " & MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " AL " & Fechafin.Day & " " & MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & rwPeriodo0(0).Item("iEjercicio")
                iejercicio = (rwPeriodo0(0).Item("iEjercicio"))
                iMes = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
            End If
            If Usuario.Nombre = "Operadora" Then
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\acumuladosoperadora19.xlsx"
                SQL = "EXEC getAcumuladosOperadoraFinal " & cboperiodo.SelectedValue
            Else
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\acumuladosnavigator19.xlsx"
                SQL = "EXEC getAcumuladosNavigatorFinal " & cboperiodo.SelectedValue
            End If

            Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
            Dim libro As New ClosedXML.Excel.XLWorkbook

            book.Worksheet(1).CopyTo(libro, iMes)

            Dim hoja As IXLWorksheet = libro.Worksheets(0)

            filaExcel = 2
            Dim nombrebuque As String
            Dim inicio As Integer = 0
            Dim contadorexcelbuqueinicial As Integer = 0
            Dim contadorexcelbuquefinal As Integer = 0
            Dim total As Integer = lsvLista.Items.Count
            Dim fecha As String

            recorrerFilasColumnas(hoja, 2, lsvLista.Items.Count + 10, 40, "clear")



            Dim rwFilas As DataRow() = nConsulta(SQL)

            If rwFilas.Length > 0 Then

                For x As Integer = 0 To rwFilas.Length - 1
                    hoja.Range(1, 5, filaExcel + x, 33).Style.NumberFormat.NumberFormatId = 4
                    hoja.Range(1, 1, filaExcel + x, 4).Style.NumberFormat.Format = "@"

                    empleadotmp = rwFilas(x)("cNombreLargo")

                    SQL2 = "select isNULL( sum(fSubsidioGenerado) ,0) AS fSubsidioGenerado, isNULL( sum(fSubsidioAplicado),0) as fSubsidioAplicado "
                    SQL2 &= " from Nomina"
                    SQL2 &= " where(fkiIdPeriodo = " & cboperiodo.SelectedValue & ")"
                    SQL2 &= " and fkiIdEmpleadoC=(select iIdEmpleadoC from empleadosC where cCodigoEmpleado=" & rwFilas(x)("cCodigoEmpleado") & ")"

                    Dim nominaA As DataRow() = nConsulta(SQL2)
                    If nominaA.Count <> 0 Then
                        SUMsubsidioGenerado = nominaA(0)("fSubsidioGenerado")
                    Else
                        SUMsubsidioGenerado = "0.0"
                    End If

                    hoja.Cell(filaExcel + x, 1).Value = rwFilas(x)("cCodigoEmpleado")
                    hoja.Cell(filaExcel + x, 2).Value = rwFilas(x)("cNombreLargo")
                    hoja.Cell(filaExcel + x, 3).Value = rwFilas(x)("cRFC")
                    hoja.Cell(filaExcel + x, 4).Value = rwFilas(x)("ccurp")
                    hoja.Cell(filaExcel + x, 5).Value = rwFilas(x)("fSalarioBase")
                    hoja.Cell(filaExcel + x, 6).Value = rwFilas(x)("fSueldoBruto")
                    hoja.Cell(filaExcel + x, 7).Value = rwFilas(x)("fTExtraFijoGravado")
                    hoja.Cell(filaExcel + x, 8).Value = rwFilas(x)("fTExtraFijoExento")
                    hoja.Cell(filaExcel + x, 9).Value = rwFilas(x)("fTExtraOcasional")
                    hoja.Cell(filaExcel + x, 10).Value = rwFilas(x)("fDescSemObligatorio")
                    hoja.Cell(filaExcel + x, 11).Value = rwFilas(x)("fVacacionesProporcionales")
                    hoja.Cell(filaExcel + x, 12).Value = rwFilas(x)("fAguinaldoGravado")
                    hoja.Cell(filaExcel + x, 13).Value = rwFilas(x)("fAguinaldoExento")
                    hoja.Cell(filaExcel + x, 14).Value = rwFilas(x)("fPrimaVacacionalGravado")
                    hoja.Cell(filaExcel + x, 15).Value = rwFilas(x)("fPrimaVacacionalExento")
                    hoja.Cell(filaExcel + x, 16).Value = rwFilas(x)("fTotalPercepciones")
                    hoja.Cell(filaExcel + x, 17).Value = rwFilas(x)("fTotalPercepcionesISR")
                    hoja.Cell(filaExcel + x, 18).Value = rwFilas(x)("fIncapacidad")
                    hoja.Cell(filaExcel + x, 19).Value = rwFilas(x)("fIsr")
                    hoja.Cell(filaExcel + x, 20).Value = rwFilas(x)("fImss")
                    hoja.Cell(filaExcel + x, 21).Value = rwFilas(x)("fInfonavit")
                    hoja.Cell(filaExcel + x, 22).Value = rwFilas(x)("fInfonavitBanterior")
                    hoja.Cell(filaExcel + x, 23).Value = rwFilas(x)("fAjusteInfonavit")
                    hoja.Cell(filaExcel + x, 24).Value = rwFilas(x)("fPensionAlimenticia")
                    hoja.Cell(filaExcel + x, 25).Value = rwFilas(x)("fPrestamo")
                    hoja.Cell(filaExcel + x, 26).Value = rwFilas(x)("fFonacot")
                    hoja.Cell(filaExcel + x, 27).Value = SUMsubsidioGenerado
                    hoja.Cell(filaExcel + x, 28).Value = rwFilas(x)("fSubsidioAplicado")
                    hoja.Cell(filaExcel + x, 29).Value = rwFilas(x)("fOperadora")
                    hoja.Cell(filaExcel + x, 30).Value = rwFilas(x)("fPrestamoPerA")
                    hoja.Cell(filaExcel + x, 31).Value = rwFilas(x)("fAdeudoInfonavitA")
                    hoja.Cell(filaExcel + x, 32).Value = rwFilas(x)("fDiferenciaInfonavitA")
                    hoja.Cell(filaExcel + x, 33).Value = rwFilas(x)("fAsimilados")

                    ' hoja.Cell(filaExcel + x, 33).FormulaA1 = "=E" & filaExcel + x & "-(U" & filaExcel + x & "+V" & filaExcel + x & "+W" & filaExcel + x & "+X" & filaExcel + x & "+Y" & filaExcel + x & "+Z" & filaExcel + x & ")-AC" & filaExcel + x & "-AD" & filaExcel + x & "-AE" & filaExcel + x & "-AF" & filaExcel + x & ""

                Next
            End If

            dialogo.DefaultExt = "*.xlsx"
            dialogo.FileName = "REPORTE ACUMULADOS" & Usuario.Nombre.ToUpper & " - " & iMes.ToUpper & " " & iejercicio
            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
            dialogo.ShowDialog()
            libro.SaveAs(dialogo.FileName)
            libro = Nothing

            MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            'MsgBox("DATOO: " & empleadotmp & "  ERROR: " & ex.Message)
            MsgBox(ex.Message)
        End Try
    End Sub

    Function MonthString(ByRef month As Integer) As String

        Select Case month
            Case 1 : Return "Enero"
            Case 2 : Return "Febrero"
            Case 3 : Return "Marzo"
            Case 4 : Return "Abril"
            Case 5 : Return "Mayo"
            Case 6 : Return "Junio"
            Case 7 : Return "Julio"
            Case 8 : Return "Agosto"
            Case 9 : Return "Septiembre"
            Case 10 : Return "Octubre"
            Case 11 : Return "Noviembre"
            Case 12, 0 : Return "Diciembre"

        End Select

    End Function

    Public Sub recorrerFilasColumnas(ByRef hoja As IXLWorksheet, ByRef filainicio As Integer, ByRef filafinal As Integer, ByRef colTotal As Integer, ByRef tipo As String, Optional ByVal inicioCol As Integer = 1)
        For f As Integer = filainicio To filafinal
            For c As Integer = IIf(inicioCol = Nothing, 1, inicioCol) To colTotal
                Select Case tipo
                    Case "bold"
                        hoja.Cell(f, c).Style.Font.SetFontColor(XLColor.Black)
                    Case "bold false"
                        hoja.Cell(f, c).Style.Font.SetBold(False)
                    Case "clear"
                        hoja.Cell(f, c).Clear()
                    Case "sin relleno"
                        hoja.Cell(f, c).Style.Fill.BackgroundColor = XLColor.NoColor
                    Case "text black"
                        hoja.Cell(f, c).Style.Font.SetFontColor(XLColor.Black)
                End Select
            Next
        Next

    End Sub

    Private Sub tsbCancelar_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCancelar.Click
        lsvLista.Items.Clear()
        chkAll.Checked = False
        lblRuta.Text = ""
    End Sub

    'Private Sub tsbReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReporte.Click
    '    Try

    '        Dim filaExcel As Integer = 0
    '        Dim dialogo As New SaveFileDialog()
    '        Dim periodo, fechadepago, iejercicio As String
    '        Dim mes As String
    '        Dim fechapagoletra() As String
    '        Dim cedros, jose, miramar, grande, montserrat, blanca, ciari, janitzio, gabriel, amarrados, arboleda, azteca, diego, ignacio, luis, cruz, verde, leon, nevado As Double
    '        Dim creciente, colorada, subsea88 As Integer
    '        Dim cedrospasim, josepasim, miramarpasim, grandepasim, montserratpasim, blancapasim, ciaripasim, janitziopasim, gabrielpasim, amarradospasim, arboledapasim, aztecapasim, diegopasim, ignaciopasim, luispasim, cruzpasim, verdepasim, leonpasim, nevadoasim As Double
    '        Dim crecientepasim, coloradapasim, subsea88pasim As Integer

    '        Dim cedrosretencion, joseretencion, miramarretencion, granderetencion, montserratretencion, blancaretencion, ciariretencion, janitzioretencion, gabrielretencion, amarradosretencion, arboledaretencion, aztecaretencion, diegoretencion, ignacioretencion, luisretencion, cruzretencion, verderetencion, leonretencion, nevadoretencion As Double
    '        Dim crecienteretencion, coloradaretencion, subsea88retencion As Integer

    '        Dim H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X2, Y, Z, AA, AB, AC, AD, AE, AF, AG, AH, AI, AJ As String

    '        pnlProgreso.Visible = True
    '        pnlCatalogo.Enabled = False
    '        Application.DoEvents()

    '        pgbProgreso.Minimum = 0
    '        pgbProgreso.Value = 0
    '        pgbProgreso.Maximum = lsvLista.Items.Count


    '        If lsvLista.Items.Count > 0 Then

    '            Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
    '            If rwPeriodo0 Is Nothing = False Then

    '                periodo = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
    '                iejercicio = (rwPeriodo0(0).Item("iEjercicio"))
    '                mes = rwPeriodo0(0).Item("iMes")
    '                fechapagoletra = rwPeriodo0(0).Item("dFechaFin").ToLongDateString().ToString.Split(" ")
    '                fechadepago = rwPeriodo0(0).Item("dFechaFin")
    '            End If



    '            Dim ruta As String
    '            ruta = My.Application.Info.DirectoryPath() & "\Archivos\Reporte.xlsx"

    '            Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


    '            Dim libro As New ClosedXML.Excel.XLWorkbook

    '            book.Worksheet(1).CopyTo(libro, periodo)
    '            book.Worksheet(2).CopyTo(libro, "DESGLOSE")
    '            book.Worksheet(3).CopyTo(libro, "RESUMEN")


    '            Dim hoja As IXLWorksheet = libro.Worksheets(0)
    '            Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
    '            Dim hoja3 As IXLWorksheet = libro.Worksheets(2)

    '            '<<<<<<DESGLOCE>>>>>>>
    '            filaExcel = 2
    '            Dim nombrebuque As String
    '            Dim inicio As Integer = 0
    '            Dim contadorexcelbuqueinicial As Integer = 0
    '            Dim contadorexcelbuquefinal As Integer = 0
    '            Dim contadorexcelbuquefinalg As Integer = 0
    '            Dim total As Integer = lsvLista.Items.Count - 1
    '            Dim filatmp As Integer = 0




    '            For x As Integer = 0 To lsvLista.Items.Count - 1
    '                If inicio = x Then
    '                    contadorexcelbuqueinicial = filaExcel + x
    '                    nombrebuque = lsvLista.SelectedItems(x).SubItems(9).Text()
    '                End If
    '                If nombrebuque = lsvLista.SelectedItems(x).SubItems(9).Text() Then
    '                    hoja2.Cell(filaExcel + x, 2).Style.NumberFormat.Format = "@"
    '                    hoja2.Cell(filaExcel + x, 4).Style.NumberFormat.Format = "@"
    '                    hoja2.Range(filaExcel + x, 8, filaExcel + x, 36).Style.NumberFormat.NumberFormatId = 4

    '                    hoja2.Cell(filaExcel + x, 1).Value = fechadepago 'FECHA DE PAGO

    '                    hoja2.Cell(filaExcel + x, 2).Value = lsvLista.SelectedItems(x).SubItems(0).Text() 'no empleado
    '                    hoja2.Cell(filaExcel + x, 3).Value = lsvLista.SelectedItems(x).SubItems(1).Text() 'nombre
    '                    hoja2.Cell(filaExcel + x, 4).Value = lsvLista.SelectedItems(x).SubItems(3).Text() 'rfc
    '                    hoja2.Cell(filaExcel + x, 5).Value = lsvLista.SelectedItems(x).SubItems(8).Text() 'puesto
    '                    hoja2.Cell(filaExcel + x, 6).Value = lsvLista.SelectedItems(x).SubItems(14).Text() ' dias pagados
    '                    hoja2.Cell(filaExcel + x, 7).Value = lsvLista.SelectedItems(x).SubItems(9).Text() ' buqyes

    '                    If lsvLista.SelectedItems(x).SubItems(8).Text() = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or lsvLista.SelectedItems(x).SubItems(8).Text() = "SUBALTERNO EN FORMACIÓN" Then
    '                        hoja2.Cell(filaExcel + x, 8).Value = CDbl(lsvLista.SelectedItems(x).SubItems(17).Text()) 'sueldo base pilotin
    '                        If lsvLista.SelectedItems(x).SubItems(18).Text() <> "" And lsvLista.SelectedItems(x).SubItems(19).Text() <> "" Then
    '                            hoja2.Cell(filaExcel + x, 9).Value = (CDbl(lsvLista.SelectedItems(x).SubItems(18).Text())) + (CDbl(lsvLista.SelectedItems(x).SubItems(19).Text()))  'Tiempo fijo extra
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 9).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 10).Value = CDbl(lsvLista.SelectedItems(x).SubItems(20).Text()) 'TIEMPO EXTRA OCASIONAL
    '                        hoja2.Cell(filaExcel + x, 11).Value = CDbl(lsvLista.SelectedItems(x).SubItems(21).Text()) ' DES SEM OBLIG
    '                        hoja2.Cell(filaExcel + x, 12).Value = CDbl(lsvLista.SelectedItems(x).SubItems(22).Text()) ' VACACIONES PROPOC"
    '                        hoja2.Cell(filaExcel + x, 13).Value = CDbl(lsvLista.SelectedItems(x).SubItems(25).Text()) ' TOTAL AGUINALDO
    '                        hoja2.Cell(filaExcel + x, 14).Value = CDbl(lsvLista.SelectedItems(x).SubItems(28).Text()) ' TOTAL P. VACACIONAL
    '                        hoja2.Cell(filaExcel + x, 15).Value = CDbl(lsvLista.SelectedItems(x).SubItems(29).Text()) ' TOAL PERCEPCIONES>>>

    '                        Dim complementoAsim As Double = 0 '= CDbl(lsvLista.SelectedItems(x).SubItems(51).Text()) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "Asimilado", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) ' COMPLEMENTO ASIM

    '                        hoja2.Cell(filaExcel + x, 16).Value = IIf(complementoAsim < 0, 0, complementoAsim) 'p

    '                        hoja2.Cell(filaExcel + x, 17).Value = CDbl(lsvLista.SelectedItems(x).SubItems(41).Text())

    '                        'If dtgDatos.Rows(x).Cells(53).Value <> "" Then
    '                        '    hoja2.Cell(filaExcel + x, 18).FormulaA1 = "=(Q" & filaExcel + x & "+SUM(v" & filaExcel + x & ":z" & filaExcel + x & "))*2%" 'COMISION OPERADORA
    '                        'Else
    '                        '    hoja2.Cell(filaExcel + x, 18).Value = "0"
    '                        'End If

    '                        hoja2.Cell(filaExcel + x, 19).FormulaA1 = "=(" & lsvLista.SelectedItems(x).SubItems(51).Text() & ")*2%" 'COMISION COMPLE


    '                    Else

    '                        ''No es pilotin o subalterno´'
    '                        hoja2.Cell(filaExcel + x, 8).Value = CDbl(lsvLista.SelectedItems(x).SubItems(18).Text) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(1).Text, lsvLista.SelectedItems(x).SubItems(15).Text, "sueldoBruto", "", "", "", lsvLista.SelectedItems(x).SubItems(9).Text)) ' sueldo base
    '                        If lsvLista.SelectedItems(x).SubItems(22).Text <> "" And lsvLista.SelectedItems(x).SubItems(23).Text <> "" Then
    '                            hoja2.Cell(filaExcel + x, 9).Value = (CDbl(lsvLista.SelectedItems(x).SubItems(22).Text) * 2) + (CDbl(lsvLista.SelectedItems(x).SubItems(23).Text) * 2)  'Tiempo fijo extra
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 9).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 10).Value = CDbl(lsvLista.SelectedItems(x).SubItems(24).Text * 2) 'TIEMPO EXTRA OCASIONAL
    '                        hoja2.Cell(filaExcel + x, 11).Value = CDbl(lsvLista.SelectedItems(x).SubItems(25).Text * 2) ' DES SEM OBLIG
    '                        hoja2.Cell(filaExcel + x, 12).Value = CDbl(lsvLista.SelectedItems(x).SubItems(26).Text * 2) ' VACACIONES PROPOC"
    '                        hoja2.Cell(filaExcel + x, 13).Value = CDbl(lsvLista.SelectedItems(x).SubItems(29).Text * 2) ' TOTAL AGUINALDO
    '                        hoja2.Cell(filaExcel + x, 14).Value = CDbl(lsvLista.SelectedItems(x).SubItems(32).Text * 2) ' TOTAL P. VACACIONAL
    '                        hoja2.Cell(filaExcel + x, 15).Value = CDbl(lsvLista.SelectedItems(x).SubItems(33).Text * 2) ' TOAL PERCEPCIONES

    '                        Dim complementoAsim As Double = CDbl(lsvLista.SelectedItems(x).SubItems(50).Text) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Text, lsvLista.SelectedItems(x).SubItems(18).Text, "Asimilado", "", "", "", lsvLista.SelectedItems(x).SubItems(11).Text)) ' COMPLEMENTO ASIM
    '                        hoja2.Cell(filaExcel + x, 16).Value = complementoAsim 'p

    '                        hoja2.Cell(filaExcel + x, 17).Value = CDbl(lsvLista.SelectedItems(x).SubItems(46).Text) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Text, lsvLista.SelectedItems(x).SubItems(18).Text, "fOperadora", "", "", "", lsvLista.SelectedItems(x).SubItems(11).Text)) 'operadora

    '                        If lsvLista.SelectedItems(x).SubItems(53).Text <> "" Then
    '                            hoja2.Cell(filaExcel + x, 18).FormulaA1 = "=(Q" & filaExcel + x & "+SUM(v" & filaExcel + x & ":z" & filaExcel + x & "))*2%" 'COMISION OPERADORA
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 18).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 19).FormulaA1 = "=(P" & filaExcel + x & "+u" & filaExcel + x & ")*2%" 'COMISION COMPLE

    '                    End If



    '                    hoja2.Cell(filaExcel + x, 20).Value = CDbl(lsvLista.SelectedItems(x).SubItems(45).Text) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "subsidio", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'Subsidio
    '                    hoja2.Cell(filaExcel + x, 21).Value = lsvLista.SelectedItems(x).SubItems(47).deeadmv17 + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "prestamoA", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'Descuento ASIM
    '                    hoja2.Cell(filaExcel + x, 22).Value = lsvLista.SelectedItems(x).SubItems(36).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "isr", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'ISR

    '                    hoja2.Cell(filaExcel + x, 23).Value = CDbl(lsvLista.SelectedItems(x).SubItems(38).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(39).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(40).Value) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavit", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavitbim", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + +CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavitajust", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'INFONAVIT
    '                    hoja2.Cell(filaExcel + x, 24).Value = lsvLista.SelectedItems(x).SubItems(41).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "pension", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'PENSION
    '                    hoja2.Cell(filaExcel + x, 25).Value = lsvLista.SelectedItems(x).SubItems(42).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "prestamo", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'PRESTAMO
    '                    hoja2.Cell(filaExcel + x, 26).Value = lsvLista.SelectedItems(x).SubItems(43).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "fonacot", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'FONACOT  
    '                    hoja2.Cell(filaExcel + x, 27).FormulaA1 = "=+SUM(V" & filaExcel + x & ":Z" & filaExcel + x & ")"

    '                    hoja2.Cell(filaExcel + x, 28).Value = lsvLista.SelectedItems(x).SubItems(55).Value  'IMSS
    '                    hoja2.Cell(filaExcel + x, 29).Value = lsvLista.SelectedItems(x).SubItems(56).Value  ' SAR
    '                    hoja2.Cell(filaExcel + x, 30).Value = lsvLista.SelectedItems(x).SubItems(57).Value  'INFONAVIT
    '                    hoja2.Cell(filaExcel + x, 31).Value = lsvLista.SelectedItems(x).SubItems(58).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "INSCS", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) 'IMPTO S/NOMINA
    '                    hoja2.Cell(filaExcel + x, 32).FormulaA1 = "=+AB" & filaExcel + x & "+AC" & filaExcel + x & "+AD" & filaExcel + x & "+AE" & filaExcel + x
    '                    hoja2.Cell(filaExcel + x, 33).FormulaA1 = "=O" & filaExcel + x & "+P" & filaExcel + x & "+R" & filaExcel + x & "+S" & filaExcel + x & "+T" & filaExcel + x & "+U" & filaExcel + x & "+AF" & filaExcel + x  ' SUBTOTAL
    '                    hoja2.Cell(filaExcel + x, 34).FormulaA1 = "=AG" & filaExcel + x & "*16%" 'IVA
    '                    hoja2.Cell(filaExcel + x, 35).FormulaA1 = "=(Q" & filaExcel + x & "+R" & filaExcel + x & "+AA" & filaExcel + x & "+AF" & filaExcel + x & ")*6%"
    '                    hoja2.Cell(filaExcel + x, 36).FormulaA1 = "=AG" & filaExcel + x & "+AH" & filaExcel + x & "-AI" & filaExcel + x ' TOTAL

    '                    ' sumatoriaISR(nombrebuque, dtgDatos)
    '                    Dim operadoraretencion As Double = CDbl(lsvLista.SelectedItems(x).SubItems(46).Value) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "fOperadora", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue))
    '                    Dim retencion As Double = CDbl(lsvLista.SelectedItems(x).SubItems(36).Value) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "isr", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + CDbl(lsvLista.SelectedItems(x).SubItems(38).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(39).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(40).Value) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavit", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavitbim", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + +CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "infonavitajust", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + lsvLista.SelectedItems(x).SubItems(41).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "pension", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + lsvLista.SelectedItems(x).SubItems(42).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "prestamo", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue)) + lsvLista.SelectedItems(x).SubItems(43).Value + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "fonacot", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue))
    '                    Dim comisionretencion As Double = CDbl(CDbl(CDbl(operadoraretencion) + CDbl(retencion)) * 0.02)
    '                    Dim costosocialretencion As Double = CDbl(lsvLista.SelectedItems(x).SubItems(55).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(56).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(57).Value) + CDbl(lsvLista.SelectedItems(x).SubItems(58).Value) + CDbl(getDescanso.GetNominaDescanso(cboTipoNomina.SelectedIndex, lsvLista.SelectedItems(x).SubItems(3).Value, lsvLista.SelectedItems(x).SubItems(18).Value, "INSCS", "", "", "", lsvLista.SelectedItems(x).SubItems(11).FormattedValue))


    '                    Select Case nombrebuque
    '                        Case "ISLA CEDROS"
    '                            cedros += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            cedrospasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            cedrosretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN JOSE"
    '                            jose += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            josepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            joseretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA GRANDE"
    '                            grande += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            grandepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            granderetencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA MIRAMAR"
    '                            miramar += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            miramarpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            miramarretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA MONSERRAT", "ISLA MONTSERRAT"
    '                            montserrat += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            montserratpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            montserratretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA BLANCA"
    '                            blanca += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            blancapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            blancaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA CIARI"
    '                            ciari += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            ciaripasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            ciariretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA JANITZIO"
    '                            janitzio += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            janitziopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            janitzioretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN GABRIEL"
    '                            gabriel += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            gabrielpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            gabrielretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "AMARRADOS"
    '                            amarrados += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            amarradospasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            amarradosretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA ARBOLEDA"
    '                            arboleda += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            arboledapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            arboledaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA AZTECA"
    '                            azteca += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            aztecapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            aztecaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN DIEGO", "ISLA DIEGO"
    '                            diego += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            diegopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            diegoretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN IGNACIO", "ISLA IGNACIO"
    '                            ignacio += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            ignaciopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            ignacioretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN LUIS"
    '                            luis += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            luispasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            luisretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SANTA CRUZ"
    '                            cruz += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            cruzpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            cruzretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA VERDE"
    '                            verde += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            verdepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            verderetencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA CRECIENTE"
    '                            creciente += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            crecientepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            crecienteretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA COLORADA"
    '                            colorada += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            coloradapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            coloradaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "SUBSEA 88"
    '                            subsea88 += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            subsea88pasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            subsea88retencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA LEON"
    '                            leon += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            leonpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            leonretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "NEVADO DE COLIMA"
    '                            nevado += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            nevadoasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            nevadoretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                    End Select

    '                Else
    '                    filatmp = filatmp + 1



    '                    contadorexcelbuquefinal = filaExcel + x - 1
    '                    contadorexcelbuquefinal = contadorexcelbuquefinal
    '                    hoja2.Range(filaExcel + x, 8, filaExcel + x, 35).Style.NumberFormat.NumberFormatId = 4
    '                    hoja2.Cell(filaExcel + x, 7).Value = "SUMA " + nombrebuque
    '                    hoja2.Cell(filaExcel + x, 8).FormulaA1 = "=SUM(H" & contadorexcelbuqueinicial & ":H" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 9).FormulaA1 = "=SUM(I" & contadorexcelbuqueinicial & ":I" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 10).FormulaA1 = "=SUM(J" & contadorexcelbuqueinicial & ":J" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 11).FormulaA1 = "=SUM(K" & contadorexcelbuqueinicial & ":K" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 12).FormulaA1 = "=SUM(L" & contadorexcelbuqueinicial & ":L" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 13).FormulaA1 = "=SUM(M" & contadorexcelbuqueinicial & ":M" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 14).FormulaA1 = "=SUM(N" & contadorexcelbuqueinicial & ":N" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 15).FormulaA1 = "=SUM(O" & contadorexcelbuqueinicial & ":O" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 16).FormulaA1 = "=SUM(P" & contadorexcelbuqueinicial & ":P" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 17).FormulaA1 = "=SUM(Q" & contadorexcelbuqueinicial & ":Q" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 18).FormulaA1 = "=SUM(R" & contadorexcelbuqueinicial & ":R" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 19).FormulaA1 = "=SUM(S" & contadorexcelbuqueinicial & ":S" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 20).FormulaA1 = "=SUM(T" & contadorexcelbuqueinicial & ":T" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 21).FormulaA1 = "=SUM(U" & contadorexcelbuqueinicial & ":U" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 22).FormulaA1 = "=SUM(V" & contadorexcelbuqueinicial & ":V" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 23).FormulaA1 = "=SUM(W" & contadorexcelbuqueinicial & ":W" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 24).FormulaA1 = "=SUM(X" & contadorexcelbuqueinicial & ":X" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 25).FormulaA1 = "=SUM(Y" & contadorexcelbuqueinicial & ":Y" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 26).FormulaA1 = "=SUM(Z" & contadorexcelbuqueinicial & ":Z" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 27).FormulaA1 = "=SUM(AA" & contadorexcelbuqueinicial & ":AA" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 28).FormulaA1 = "=SUM(AB" & contadorexcelbuqueinicial & ":AB" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 29).FormulaA1 = "=SUM(AC" & contadorexcelbuqueinicial & ":AC" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 30).FormulaA1 = "=SUM(AD" & contadorexcelbuqueinicial & ":AD" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 31).FormulaA1 = "=SUM(AE" & contadorexcelbuqueinicial & ":AE" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 32).FormulaA1 = "=SUM(AF" & contadorexcelbuqueinicial & ":AF" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 33).FormulaA1 = "=SUM(AG" & contadorexcelbuqueinicial & ":AG" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 34).FormulaA1 = "=SUM(AH" & contadorexcelbuqueinicial & ":AH" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 35).FormulaA1 = "=SUM(AI" & contadorexcelbuqueinicial & ":AI" & contadorexcelbuquefinal & ")"
    '                    hoja2.Cell(filaExcel + x, 36).FormulaA1 = "=SUM(AJ" & contadorexcelbuqueinicial & ":AJ" & contadorexcelbuquefinal & ")"

    '                    'sUMA
    '                    hoja2.Range(filaExcel + x, 7, filaExcel + x, 36).Style.Font.SetBold(True)
    '                    hoja2.Range(filaExcel + x, 7, filaExcel + x, 36).Style.Border.BottomBorderColor = XLColor.PowderBlue
    '                    hoja2.Range(filaExcel + x, 7, filaExcel + x, 36).Style.Fill.BackgroundColor = XLColor.PowderBlue

    '                    H += " +" & "H" & filaExcel + x
    '                    I += " +" & "I" & filaExcel + x
    '                    J += " +" & "J" & filaExcel + x
    '                    K += " +" & "K" & filaExcel + x
    '                    L += " +" & "L" & filaExcel + x
    '                    M += " +" & "M" & filaExcel + x
    '                    N += " +" & "N" & filaExcel + x
    '                    O += " +" & "O" & filaExcel + x
    '                    P += " +" & "P" & filaExcel + x
    '                    Q += " +" & "Q" & filaExcel + x
    '                    R += " +" & "R" & filaExcel + x
    '                    S += " +" & "S" & filaExcel + x
    '                    T += " +" & "T" & filaExcel + x
    '                    U += " +" & "U" & filaExcel + x
    '                    V += " +" & "V" & filaExcel + x
    '                    W += " +" & "W" & filaExcel + x
    '                    X2 += " +" & "X" & filaExcel + x
    '                    Y += " +" & "Y" & filaExcel + x
    '                    Z += " +" & "Z" & filaExcel + x
    '                    AA += " +" & "AA" & filaExcel + x
    '                    AB += " +" & "AB" & filaExcel + x
    '                    AC += " +" & "AC" & filaExcel + x
    '                    AD += " +" & "AD" & filaExcel + x
    '                    AE += " +" & "AE" & filaExcel + x
    '                    AF += " +" & "AF" & filaExcel + x
    '                    AG += " +" & "AG" & filaExcel + x
    '                    AH += " +" & "AH" & filaExcel + x
    '                    AI += " +" & "AI" & filaExcel + x
    '                    AJ += " +" & "AJ" & filaExcel + x

    '                    '<<<<<<MES>>>>>>>

    '                    hoja.Cell(5, 2).Style.NumberFormat.Format = "@"
    '                    hoja.Cell(5, 2).Value = fechapagoletra(1) & " " & fechapagoletra(2) & " " & fechapagoletra(3)
    '                    hoja.Cell(36, 7).Value = MonthString(mes - 1).ToUpper
    '                    hoja.Cell(36, 32).Style.Font.SetBold(True)

    '                    llenardesgloce(nombrebuque, contadorexcelbuquefinal, hoja)

    '                    nombrebuque = dtgDatos.Rows(x).Cells(12).Value
    '                    filaExcel = filaExcel + 1
    '                    contadorexcelbuqueinicial = filaExcel + x
    '                    contadorexcelbuquefinal = 0

    '                    hoja2.Cell(filaExcel + x, 2).Style.NumberFormat.Format = "@"
    '                    hoja2.Cell(filaExcel + x, 4).Style.NumberFormat.Format = "@"
    '                    hoja2.Range(filaExcel + x, 8, filaExcel + x, 36).Style.NumberFormat.NumberFormatId = 4


    '                    '<<<<<<MES PT2>>>
    '                    hoja2.Cell(filaExcel + x, 1).Value = fechadepago 'FECHA DE PAGO
    '                    hoja2.Cell(filaExcel + x, 2).Value = dtgDatos.Rows(x).Cells(3).Value 'no empleado
    '                    hoja2.Cell(filaExcel + x, 3).Value = dtgDatos.Rows(x).Cells(4).Value 'nombre
    '                    hoja2.Cell(filaExcel + x, 4).Value = dtgDatos.Rows(x).Cells(6).Value 'rfc
    '                    hoja2.Cell(filaExcel + x, 5).Value = dtgDatos.Rows(x).Cells(11).FormattedValue 'puesto
    '                    hoja2.Cell(filaExcel + x, 6).Value = dtgDatos.Rows(x).Cells(18).Value ' dias pagados
    '                    hoja2.Cell(filaExcel + x, 7).Value = dtgDatos.Rows(x).Cells(12).FormattedValue ' buqyes

    '                    If dtgDatos.Rows(x).Cells(11).Value = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or dtgDatos.Rows(x).Cells(11).Value = "SUBALTERNO EN FORMACIÓN" Then
    '                        hoja2.Cell(filaExcel + x, 8).Value = CDbl(dtgDatos.Rows(x).Cells(21).Value) ' sueldo base
    '                        If dtgDatos.Rows(x).Cells(22).Value <> "" And dtgDatos.Rows(x).Cells(23).Value <> "" Then
    '                            hoja2.Cell(filaExcel + x, 9).Value = (CDbl(dtgDatos.Rows(x).Cells(22).Value)) + (CDbl(dtgDatos.Rows(x).Cells(23).Value))  'Tiempo fijo extra
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 9).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 10).Value = CDbl(dtgDatos.Rows(x).Cells(24).Value) 'TIEMPO EXTRA OCASIONAL
    '                        hoja2.Cell(filaExcel + x, 11).Value = CDbl(dtgDatos.Rows(x).Cells(25).Value) ' DES SEM OBLIG
    '                        hoja2.Cell(filaExcel + x, 12).Value = CDbl(dtgDatos.Rows(x).Cells(26).Value) ' VACACIONES PROPOC"
    '                        hoja2.Cell(filaExcel + x, 13).Value = CDbl(dtgDatos.Rows(x).Cells(29).Value) ' TOTAL AGUINALDO
    '                        hoja2.Cell(filaExcel + x, 14).Value = CDbl(dtgDatos.Rows(x).Cells(32).Value) ' TOTAL P. VACACIONAL
    '                        hoja2.Cell(filaExcel + x, 15).Value = CDbl(dtgDatos.Rows(x).Cells(33).Value) ' TOAL PERCEPCIONES
    '                        Dim complementoAsim As Double = CDbl(dtgDatos.Rows(x).Cells(50).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "Asimilado", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) ' COMPLEMENTO ASIM
    '                        hoja2.Cell(filaExcel + x, 16).Value = IIf(complementoAsim < 0, 0, complementoAsim)
    '                        hoja2.Cell(filaExcel + x, 17).Value = CDbl(dtgDatos.Rows(x).Cells(46).Value) '+ CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "fOperadora", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))


    '                        If dtgDatos.Rows(x).Cells(53).Value <> "" Then
    '                            hoja2.Cell(filaExcel + x, 18).FormulaA1 = "=(Q" & filaExcel + x & "+SUM(v" & filaExcel + x & ":z" & filaExcel + x & "))*2%" 'COMISION OPERADORA
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 18).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 19).FormulaA1 = "=(P" & filaExcel + x & "+u" & filaExcel + x & ")*2%" 'COMISION COMPLE



    '                    Else

    '                        ''No es pilotin o subalterno
    '                        hoja2.Cell(filaExcel + x, 8).Value = CDbl(dtgDatos.Rows(x).Cells(21).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "sueldoBruto", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) ' sueldo base
    '                        If dtgDatos.Rows(x).Cells(22).Value <> "" And dtgDatos.Rows(x).Cells(23).Value <> "" Then
    '                            hoja2.Cell(filaExcel + x, 9).Value = (CDbl(dtgDatos.Rows(x).Cells(22).Value) * 2) + (CDbl(dtgDatos.Rows(x).Cells(23).Value) * 2)  'Tiempo fijo extra
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 9).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 10).Value = CDbl(dtgDatos.Rows(x).Cells(24).Value * 2) 'TIEMPO EXTRA OCASIONAL
    '                        hoja2.Cell(filaExcel + x, 11).Value = CDbl(dtgDatos.Rows(x).Cells(25).Value * 2) ' DES SEM OBLIG
    '                        hoja2.Cell(filaExcel + x, 12).Value = CDbl(dtgDatos.Rows(x).Cells(26).Value * 2) ' VACACIONES PROPOC"
    '                        hoja2.Cell(filaExcel + x, 13).Value = CDbl(dtgDatos.Rows(x).Cells(29).Value * 2) ' TOTAL AGUINALDO
    '                        hoja2.Cell(filaExcel + x, 14).Value = CDbl(dtgDatos.Rows(x).Cells(32).Value * 2) ' TOTAL P. VACACIONAL
    '                        hoja2.Cell(filaExcel + x, 15).Value = CDbl(dtgDatos.Rows(x).Cells(33).Value * 2) ' TOAL PERCEPCIONES

    '                        Dim complementoAsim As Double = CDbl(dtgDatos.Rows(x).Cells(50).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "Asimilado", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) ' COMPLEMENTO ASIM
    '                        hoja2.Cell(filaExcel + x, 16).Value = complementoAsim 'p

    '                        hoja2.Cell(filaExcel + x, 17).Value = CDbl(dtgDatos.Rows(x).Cells(46).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "fOperadora", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))


    '                        If dtgDatos.Rows(x).Cells(53).Value <> "" Then
    '                            hoja2.Cell(filaExcel + x, 18).FormulaA1 = "=(Q" & filaExcel + x & "+SUM(v" & filaExcel + x & ":z" & filaExcel + x & "))*2%" 'COMISION OPERADORA
    '                        Else
    '                            hoja2.Cell(filaExcel + x, 18).Value = "0"
    '                        End If

    '                        hoja2.Cell(filaExcel + x, 19).FormulaA1 = "=(P" & filaExcel + x & "+u" & filaExcel + x & ")*2%" 'COMISION COMPLE

    '                    End If



    '                    hoja2.Cell(filaExcel + x, 20).Value = CDbl(dtgDatos.Rows(x).Cells(45).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "subsidio", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'Subsidio
    '                    hoja2.Cell(filaExcel + x, 21).Value = dtgDatos.Rows(x).Cells(47).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'Descuento ASIM
    '                    hoja2.Cell(filaExcel + x, 22).Value = dtgDatos.Rows(x).Cells(36).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'ISR

    '                    hoja2.Cell(filaExcel + x, 23).Value = CDbl(dtgDatos.Rows(x).Cells(38).Value) + CDbl(dtgDatos.Rows(x).Cells(39).Value) + CDbl(dtgDatos.Rows(x).Cells(40).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavit", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavitbim", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + +CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavitajust", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'INFONAVIT
    '                    hoja2.Cell(filaExcel + x, 24).Value = dtgDatos.Rows(x).Cells(41).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "pension", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'PENSION
    '                    hoja2.Cell(filaExcel + x, 25).Value = dtgDatos.Rows(x).Cells(42).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamo", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'PRESTAMO
    '                    hoja2.Cell(filaExcel + x, 26).Value = dtgDatos.Rows(x).Cells(43).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "fonacot", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'FONACOT  
    '                    hoja2.Cell(filaExcel + x, 27).FormulaA1 = "=+SUM(V" & filaExcel + x & ":Z" & filaExcel + x & ")"

    '                    hoja2.Cell(filaExcel + x, 28).Value = dtgDatos.Rows(x).Cells(55).Value  'IMSS
    '                    hoja2.Cell(filaExcel + x, 29).Value = dtgDatos.Rows(x).Cells(56).Value  ' SAR
    '                    hoja2.Cell(filaExcel + x, 30).Value = dtgDatos.Rows(x).Cells(57).Value  'INFONAVIT
    '                    hoja2.Cell(filaExcel + x, 31).Value = dtgDatos.Rows(x).Cells(58).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "INSCS", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) 'IMPTO S/NOMINA
    '                    hoja2.Cell(filaExcel + x, 32).FormulaA1 = "=+AB" & filaExcel + x & "+AC" & filaExcel + x & "+AD" & filaExcel + x & "+AE" & filaExcel + x
    '                    hoja2.Cell(filaExcel + x, 33).FormulaA1 = "=O" & filaExcel + x & "+P" & filaExcel + x & "+R" & filaExcel + x & "+S" & filaExcel + x & "+T" & filaExcel + x & "+U" & filaExcel + x & "+AF" & filaExcel + x  ' SUBTOTAL
    '                    hoja2.Cell(filaExcel + x, 34).FormulaA1 = "=AG" & filaExcel + x & "*16%" 'IVA
    '                    hoja2.Cell(filaExcel + x, 35).FormulaA1 = "=(Q" & filaExcel + x & "+R" & filaExcel + x & "+AA" & filaExcel + x & "+AF" & filaExcel + x & ")*6%"
    '                    hoja2.Cell(filaExcel + x, 36).FormulaA1 = "=AG" & filaExcel + x & "+AH" & filaExcel + x & "-AI" & filaExcel + x ' TOTAL

    '                    ' sumatoriaISR(nombrebuque, dtgDatos)
    '                    Dim operadoraretencion As Double = CDbl(dtgDatos.Rows(x).Cells(46).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "fOperadora", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                    Dim retencion As Double = CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + CDbl(dtgDatos.Rows(x).Cells(38).Value) + CDbl(dtgDatos.Rows(x).Cells(39).Value) + CDbl(dtgDatos.Rows(x).Cells(40).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavit", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavitbim", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + +CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "infonavitajust", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + dtgDatos.Rows(x).Cells(41).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "pension", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + dtgDatos.Rows(x).Cells(42).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamo", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue)) + dtgDatos.Rows(x).Cells(43).Value + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "fonacot", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                    Dim comisionretencion As Double = CDbl(CDbl(CDbl(operadoraretencion) + CDbl(retencion)) * 0.02)
    '                    Dim costosocialretencion As Double = CDbl(dtgDatos.Rows(x).Cells(55).Value) + CDbl(dtgDatos.Rows(x).Cells(56).Value) + CDbl(dtgDatos.Rows(x).Cells(57).Value) + CDbl(dtgDatos.Rows(x).Cells(58).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "INSCS", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))


    '                    Select Case nombrebuque
    '                        Case "ISLA CEDROS"
    '                            cedros += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            cedrospasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            cedrosretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN JOSE"
    '                            jose += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            josepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            joseretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA GRANDE"
    '                            grande += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            grandepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            granderetencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA MIRAMAR"
    '                            miramar += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            miramarpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            miramarretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA MONSERRAT", "ISLA MONTSERRAT"
    '                            montserrat += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            montserratpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            montserratretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA BLANCA"
    '                            blanca += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            blancapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            blancaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA CIARI"
    '                            ciari += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            ciaripasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            ciariretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA JANITZIO"
    '                            janitzio += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            janitziopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            janitzioretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN GABRIEL"
    '                            gabriel += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            gabrielpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            gabrielretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "AMARRADOS"
    '                            amarrados += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            amarradospasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            amarradosretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA ARBOLEDA"
    '                            arboleda += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            arboledapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            arboledaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA AZTECA"
    '                            azteca += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            aztecapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            aztecaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN DIEGO", "ISLA DIEGO"
    '                            diego += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            diegopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            diegoretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN IGNACIO", "ISLA IGNACIO"
    '                            ignacio += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            ignaciopasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            ignacioretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SAN LUIS"
    '                            luis += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            luispasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            luisretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA SANTA CRUZ"
    '                            cruz += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            cruzpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            cruzretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA VERDE"
    '                            verde += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            verdepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            verderetencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA CRECIENTE"
    '                            creciente += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            crecientepasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            crecienteretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA COLORADA"
    '                            colorada += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            coloradapasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            coloradaretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "SUBSEA 88"
    '                            subsea88 += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            subsea88pasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            subsea88retencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "ISLA LEON"
    '                            leon += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            leonpasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            leonretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                        Case "NEVADO DE COLIMA"
    '                            nevado += CDbl(dtgDatos.Rows(x).Cells(36).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "isr"))
    '                            nevadoasim += CDbl(dtgDatos.Rows(x).Cells(47).Value) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value, "prestamoA", "", "", "", dtgDatos.Rows(x).Cells(11).FormattedValue))
    '                            nevadoretencion += CDbl((CDbl(operadoraretencion) + CDbl(retencion) + CDbl(comisionretencion) + CDbl(costosocialretencion)) * 0.06)
    '                    End Select

    '                End If

    '                pgbProgreso.Value += 1
    '                Application.DoEvents()

    '            Next x
    '            filaExcel = filaExcel + 1
    '            contadorexcelbuquefinal = filaExcel + total - 1

    '            hoja2.Range(filaExcel + total, 8, filaExcel + total, 36).Style.NumberFormat.NumberFormatId = 4
    '            hoja2.Range(filaExcel + total, 7, filaExcel + total, 36).Style.Fill.BackgroundColor = XLColor.PowderBlue
    '            hoja2.Cell(filaExcel + total, 7).Value = "SUMA " + nombrebuque
    '            hoja2.Cell(filaExcel + total, 8).FormulaA1 = "=SUM(H" & contadorexcelbuqueinicial & ":H" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 9).FormulaA1 = "=SUM(I" & contadorexcelbuqueinicial & ":I" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 10).FormulaA1 = "=SUM(J" & contadorexcelbuqueinicial & ":J" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 11).FormulaA1 = "=SUM(K" & contadorexcelbuqueinicial & ":K" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 12).FormulaA1 = "=SUM(L" & contadorexcelbuqueinicial & ":L" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 13).FormulaA1 = "=SUM(M" & contadorexcelbuqueinicial & ":M" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 14).FormulaA1 = "=SUM(N" & contadorexcelbuqueinicial & ":N" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 15).FormulaA1 = "=SUM(O" & contadorexcelbuqueinicial & ":O" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 16).FormulaA1 = "=SUM(P" & contadorexcelbuqueinicial & ":P" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 17).FormulaA1 = "=SUM(Q" & contadorexcelbuqueinicial & ":Q" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 18).FormulaA1 = "=SUM(R" & contadorexcelbuqueinicial & ":R" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 19).FormulaA1 = "=SUM(S" & contadorexcelbuqueinicial & ":S" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 20).FormulaA1 = "=SUM(T" & contadorexcelbuqueinicial & ":T" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 21).FormulaA1 = "=SUM(U" & contadorexcelbuqueinicial & ":U" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 22).FormulaA1 = "=SUM(V" & contadorexcelbuqueinicial & ":V" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 23).FormulaA1 = "=SUM(W" & contadorexcelbuqueinicial & ":W" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 24).FormulaA1 = "=SUM(X" & contadorexcelbuqueinicial & ":X" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 25).FormulaA1 = "=SUM(Y" & contadorexcelbuqueinicial & ":Y" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 26).FormulaA1 = "=SUM(Z" & contadorexcelbuqueinicial & ":Z" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 27).FormulaA1 = "=SUM(AA" & contadorexcelbuqueinicial & ":AA" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 28).FormulaA1 = "=SUM(AB" & contadorexcelbuqueinicial & ":AB" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 29).FormulaA1 = "=SUM(AC" & contadorexcelbuqueinicial & ":AC" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 30).FormulaA1 = "=SUM(AD" & contadorexcelbuqueinicial & ":AD" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 31).FormulaA1 = "=SUM(AE" & contadorexcelbuqueinicial & ":AE" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 32).FormulaA1 = "=SUM(AF" & contadorexcelbuqueinicial & ":AF" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 33).FormulaA1 = "=SUM(AG" & contadorexcelbuqueinicial & ":AG" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 34).FormulaA1 = "=SUM(AH" & contadorexcelbuqueinicial & ":AH" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 35).FormulaA1 = "=SUM(AI" & contadorexcelbuqueinicial & ":AI" & contadorexcelbuquefinal & ")"
    '            hoja2.Cell(filaExcel + total, 36).FormulaA1 = "=SUM(AJ" & contadorexcelbuqueinicial & ":AJ" & contadorexcelbuquefinal & ")"

    '            H += " +" & "H" & filaExcel + total ' + 1
    '            I += " +" & "I" & filaExcel + total ' + 1
    '            J += " +" & "J" & filaExcel + total '+ 1
    '            K += " +" & "K" & filaExcel + total '+ 1
    '            L += " +" & "L" & filaExcel + total '+ 1
    '            M += " +" & "M" & filaExcel + total '+ 1
    '            N += " +" & "N" & filaExcel + total '+ 1
    '            O += " +" & "O" & filaExcel + total '+ 1
    '            P += " +" & "P" & filaExcel + total '+ 1
    '            Q += " +" & "Q" & filaExcel + total '+ 1
    '            R += " +" & "R" & filaExcel + total '+ 1
    '            S += " +" & "S" & filaExcel + total '+ 1
    '            T += " +" & "T" & filaExcel + total '+ 1
    '            U += " +" & "U" & filaExcel + total '+ 1
    '            V += " +" & "V" & filaExcel + total '+ 1
    '            W += " +" & "W" & filaExcel + total '+ 1
    '            X2 += " +" & "X" & filaExcel + total ' + 1
    '            Y += " +" & "Y" & filaExcel + total '+ 1
    '            Z += " +" & "Z" & filaExcel + total '+ 1
    '            AA += " +" & "AA" & filaExcel + total ' + 1
    '            AB += " +" & "AB" & filaExcel + total '+ 1
    '            AC += " +" & "AC" & filaExcel + total '+ 1"
    '            AF += " +" & "AF" & filaExcel + total
    '            AG += " +" & "AG" & filaExcel + total
    '            AH += " +" & "AH" & filaExcel + total
    '            AI += " +" & "AI" & filaExcel + total
    '            AJ += " +" & "AJ" & filaExcel + total

    '            hoja2.Range(filaExcel + total, 7, filaExcel + total, 33).Style.Font.SetBold(True)
    '            hoja2.Range(filaExcel + total, 7, filaExcel + total, 36).Style.Border.BottomBorderColor = XLColor.PowderBlue

    '            hoja2.Range(filaExcel + total + 4, 8, filaExcel + total + 4, 36).Style.Border.TopBorderColor = XLColor.Black
    '            hoja2.Range(filaExcel + total + 4, 8, filaExcel + total + 4, 36).Style.Border.BottomBorderColor = XLColor.Black
    '            hoja2.Range(filaExcel + total + 4, 8, filaExcel + total + 4, 36).Style.Border.TopBorder = XLBorderStyleValues.Double
    '            hoja2.Range(filaExcel + total + 4, 8, filaExcel + total + 4, 36).Style.Border.BottomBorder = XLBorderStyleValues.Thin
    '            hoja2.Range(filaExcel + total + 4, 8, filaExcel + total + 4, 36).Style.NumberFormat.NumberFormatId = 4

    '            hoja2.Cell(filaExcel + total + 4, 8).FormulaA1 = "=" & H
    '            hoja2.Cell(filaExcel + total + 4, 9).FormulaA1 = "=" & I
    '            hoja2.Cell(filaExcel + total + 4, 10).FormulaA1 = "=" & J
    '            hoja2.Cell(filaExcel + total + 4, 11).FormulaA1 = "=" & K
    '            hoja2.Cell(filaExcel + total + 4, 12).FormulaA1 = "=" & L
    '            hoja2.Cell(filaExcel + total + 4, 13).FormulaA1 = "=" & M
    '            hoja2.Cell(filaExcel + total + 4, 14).FormulaA1 = "=" & N
    '            hoja2.Cell(filaExcel + total + 4, 15).FormulaA1 = "=" & O
    '            hoja2.Cell(filaExcel + total + 4, 16).FormulaA1 = "=" & P
    '            hoja2.Cell(filaExcel + total + 4, 17).FormulaA1 = "=" & Q
    '            hoja2.Cell(filaExcel + total + 4, 18).FormulaA1 = "=" & R
    '            hoja2.Cell(filaExcel + total + 4, 19).FormulaA1 = "=" & S
    '            hoja2.Cell(filaExcel + total + 4, 20).FormulaA1 = "=" & T
    '            hoja2.Cell(filaExcel + total + 4, 21).FormulaA1 = "=" & U
    '            hoja2.Cell(filaExcel + total + 4, 22).FormulaA1 = "=" & V
    '            hoja2.Cell(filaExcel + total + 4, 23).FormulaA1 = "=" & W
    '            hoja2.Cell(filaExcel + total + 4, 24).FormulaA1 = "=" & X2
    '            hoja2.Cell(filaExcel + total + 4, 25).FormulaA1 = "=" & Y
    '            hoja2.Cell(filaExcel + total + 4, 26).FormulaA1 = "=" & Z
    '            hoja2.Cell(filaExcel + total + 4, 27).FormulaA1 = "=" & AA
    '            hoja2.Cell(filaExcel + total + 4, 28).FormulaA1 = "=" & AB
    '            hoja2.Cell(filaExcel + total + 4, 29).FormulaA1 = "=" & AC
    '            hoja2.Cell(filaExcel + total + 4, 30).FormulaA1 = "=" & AD
    '            hoja2.Cell(filaExcel + total + 4, 31).FormulaA1 = "=" & AE
    '            hoja2.Cell(filaExcel + total + 4, 32).FormulaA1 = "=" & AF
    '            hoja2.Cell(filaExcel + total + 4, 33).FormulaA1 = "=" & AG
    '            hoja2.Cell(filaExcel + total + 4, 34).FormulaA1 = "=" & AH
    '            hoja2.Cell(filaExcel + total + 4, 35).FormulaA1 = "=" & AI
    '            hoja2.Cell(filaExcel + total + 4, 36).FormulaA1 = "=" & AJ

    '            llenardesgloce(nombrebuque, contadorexcelbuquefinal, hoja)



    '            '<<<<<<<<<<RESUMEN>>>>>>>>>
    '            filaExcel = 2

    '            'For x As Integer = 0 To dtgDatos.Rows.Count - 1
    '            hoja3.Cell(1, 2).Style.NumberFormat.Format = "@"
    '            hoja3.Cell(1, 2).Value = fechapagoletra(1) & " " & fechapagoletra(2) & " " & fechapagoletra(3)
    '            ' hoja3.Cell(1, 27).Value = MonthString(mes - 1).ToUpper & " ADICIONALES"
    '            hoja3.Cell(1, 30).Style.Font.SetBold(True)

    '            'Cedros
    '            hoja3.Cell(6, 4).FormulaA1 = "=" & periodo & "!D5"
    '            hoja3.Cell(7, 4).FormulaA1 = "=" & periodo & "!E5"
    '            hoja3.Cell(8, 4).FormulaA1 = "=" & periodo & "!F5"
    '            hoja3.Cell(9, 4).FormulaA1 = "=" & periodo & "!G5"
    '            'hoja3.Cell(10, 4).FormulaA1 = "=" & periodo & "!H5"
    '            hoja3.Cell(11, 4).FormulaA1 = "=" & periodo & "!I5"
    '            hoja3.Cell(12, 4).FormulaA1 = "=" & periodo & "!H5"
    '            hoja3.Cell(13, 4).FormulaA1 = "=" & periodo & "!J5"
    '            hoja3.Cell(15, 4).Value = cedros
    '            hoja3.Cell(16, 4).FormulaA1 = "=" & periodo & "!Q5"
    '            hoja3.Cell(18, 4).Value = cedrospasim
    '            hoja3.Cell(19, 4).FormulaA1 = "=" & periodo & "!O5"
    '            hoja3.Cell(20, 4).FormulaA1 = "=" & periodo & "!L5"
    '            hoja3.Cell(27, 4).Value = cedrosretencion
    '            hoja3.Cell(32, 4).FormulaA1 = "=" & periodo & "!Q5"
    '            hoja3.Cell(33, 4).FormulaA1 = "=" & periodo & "!R5"
    '            hoja3.Cell(34, 4).FormulaA1 = "=" & periodo & "!S5"
    '            hoja3.Cell(35, 4).FormulaA1 = "=" & periodo & "!T5"
    '            'Jose
    '            hoja3.Cell(6, 5).FormulaA1 = "=" & periodo & "!D6"
    '            hoja3.Cell(7, 5).FormulaA1 = "=" & periodo & "!E6"
    '            hoja3.Cell(8, 5).FormulaA1 = "=" & periodo & "!F6"
    '            hoja3.Cell(9, 5).FormulaA1 = "=" & periodo & "!G6"
    '            'hoja3.Cell(10, 5).FormulaA1 = "=" & periodo & "!H6"
    '            hoja3.Cell(11, 5).FormulaA1 = "=" & periodo & "!I6"
    '            hoja3.Cell(12, 5).FormulaA1 = "=" & periodo & "!H6"
    '            hoja3.Cell(13, 5).FormulaA1 = "=" & periodo & "!J6"
    '            hoja3.Cell(15, 5).Value = jose
    '            hoja3.Cell(18, 5).Value = josepasim
    '            hoja3.Cell(16, 5).FormulaA1 = "=" & periodo & "!Q6"
    '            hoja3.Cell(19, 5).FormulaA1 = "=" & periodo & "!O6"
    '            hoja3.Cell(20, 5).FormulaA1 = "=" & periodo & "!L6"
    '            hoja3.Cell(27, 5).Value = joseretencion
    '            hoja3.Cell(32, 5).FormulaA1 = "=" & periodo & "!Q6"
    '            hoja3.Cell(33, 5).FormulaA1 = "=" & periodo & "!R6"
    '            hoja3.Cell(34, 5).FormulaA1 = "=" & periodo & "!S6"
    '            hoja3.Cell(35, 5).FormulaA1 = "=" & periodo & "!T6"
    '            'Grande
    '            hoja3.Cell(6, 6).FormulaA1 = "=" & periodo & "!D7"
    '            hoja3.Cell(7, 6).FormulaA1 = "=" & periodo & "!E7"
    '            hoja3.Cell(8, 6).FormulaA1 = "=" & periodo & "!F7"
    '            hoja3.Cell(9, 6).FormulaA1 = "=" & periodo & "!G7"
    '            'hoja3.Cell(10, 6).FormulaA1 = "=" & periodo & "!H7"
    '            hoja3.Cell(11, 6).FormulaA1 = "=" & periodo & "!I7"
    '            hoja3.Cell(12, 6).FormulaA1 = "=" & periodo & "!H7"
    '            hoja3.Cell(13, 6).FormulaA1 = "=" & periodo & "!J7"
    '            hoja3.Cell(15, 6).Value = grande
    '            hoja3.Cell(18, 6).Value = grandepasim
    '            hoja3.Cell(16, 6).FormulaA1 = "=" & periodo & "!Q7"
    '            hoja3.Cell(19, 6).FormulaA1 = "=" & periodo & "!O7"
    '            hoja3.Cell(20, 6).FormulaA1 = "=" & periodo & "!L7"
    '            hoja3.Cell(27, 6).Value = granderetencion
    '            hoja3.Cell(32, 6).FormulaA1 = "=" & periodo & "!Q7"
    '            hoja3.Cell(33, 6).FormulaA1 = "=" & periodo & "!R7"
    '            hoja3.Cell(34, 6).FormulaA1 = "=" & periodo & "!S7"
    '            hoja3.Cell(35, 6).FormulaA1 = "=" & periodo & "!T7"
    '            'Miramar
    '            hoja3.Cell(6, 7).FormulaA1 = "=" & periodo & "!D8"
    '            hoja3.Cell(7, 7).FormulaA1 = "=" & periodo & "!E8"
    '            hoja3.Cell(8, 7).FormulaA1 = "=" & periodo & "!F8"
    '            hoja3.Cell(9, 7).FormulaA1 = "=" & periodo & "!G8"
    '            'hoja3.Cell(10, 7).FormulaA1 = "=" & periodo & "!H8"
    '            hoja3.Cell(11, 7).FormulaA1 = "=" & periodo & "!I8"
    '            hoja3.Cell(12, 7).FormulaA1 = "=" & periodo & "!H8"
    '            hoja3.Cell(13, 7).FormulaA1 = "=" & periodo & "!J8"
    '            hoja3.Cell(15, 7).Value = miramar
    '            hoja3.Cell(18, 7).Value = miramarpasim
    '            hoja3.Cell(16, 7).FormulaA1 = "=" & periodo & "!Q8"
    '            hoja3.Cell(19, 7).FormulaA1 = "=" & periodo & "!O8"
    '            hoja3.Cell(20, 7).FormulaA1 = "=" & periodo & "!L8"
    '            hoja3.Cell(27, 7).Value = miramarretencion
    '            hoja3.Cell(32, 7).FormulaA1 = "=" & periodo & "!Q8"
    '            hoja3.Cell(33, 7).FormulaA1 = "=" & periodo & "!R8"
    '            hoja3.Cell(34, 7).FormulaA1 = "=" & periodo & "!S8"
    '            hoja3.Cell(35, 7).FormulaA1 = "=" & periodo & "!T8"
    '            'Montserrat
    '            hoja3.Cell(6, 8).FormulaA1 = "=" & periodo & "!D9"
    '            hoja3.Cell(7, 8).FormulaA1 = "=" & periodo & "!E9"
    '            hoja3.Cell(8, 8).FormulaA1 = "=" & periodo & "!F9"
    '            hoja3.Cell(9, 8).FormulaA1 = "=" & periodo & "!G9"
    '            'hoja3.Cell(10, 8).FormulaA1 = "=" & periodo & "!H9"
    '            hoja3.Cell(11, 8).FormulaA1 = "=" & periodo & "!I9"
    '            hoja3.Cell(12, 8).FormulaA1 = "=" & periodo & "!H9"
    '            hoja3.Cell(13, 8).FormulaA1 = "=" & periodo & "!J9"
    '            hoja3.Cell(15, 8).Value = montserrat
    '            hoja3.Cell(18, 8).Value = montserratpasim
    '            hoja3.Cell(16, 8).FormulaA1 = "=" & periodo & "!Q9"
    '            hoja3.Cell(19, 8).FormulaA1 = "=" & periodo & "!O9"
    '            hoja3.Cell(20, 8).FormulaA1 = "=" & periodo & "!L9"
    '            hoja3.Cell(27, 8).Value = montserratretencion
    '            hoja3.Cell(32, 8).FormulaA1 = "=" & periodo & "!Q9"
    '            hoja3.Cell(33, 8).FormulaA1 = "=" & periodo & "!R9"
    '            hoja3.Cell(34, 8).FormulaA1 = "=" & periodo & "!S9"
    '            hoja3.Cell(35, 8).FormulaA1 = "=" & periodo & "!T9"
    '            'Blanca
    '            hoja3.Cell(6, 9).FormulaA1 = "=" & periodo & "!D10"
    '            hoja3.Cell(7, 9).FormulaA1 = "=" & periodo & "!E10"
    '            hoja3.Cell(8, 9).FormulaA1 = "=" & periodo & "!F10"
    '            hoja3.Cell(9, 9).FormulaA1 = "=" & periodo & "!G10"
    '            'hoja3.Cell(10, 9).FormulaA1 = "=" & periodo & "!H10"
    '            hoja3.Cell(11, 9).FormulaA1 = "=" & periodo & "!I10"
    '            hoja3.Cell(12, 9).FormulaA1 = "=" & periodo & "!H10"
    '            hoja3.Cell(13, 9).FormulaA1 = "=" & periodo & "!J10"
    '            hoja3.Cell(15, 9).Value = blanca
    '            hoja3.Cell(18, 9).Value = blancapasim
    '            hoja3.Cell(16, 9).FormulaA1 = "=" & periodo & "!Q10"
    '            hoja3.Cell(19, 9).FormulaA1 = "=" & periodo & "!O10"
    '            hoja3.Cell(20, 9).FormulaA1 = "=" & periodo & "!L10"
    '            hoja3.Cell(27, 9).Value = blancaretencion
    '            hoja3.Cell(32, 9).FormulaA1 = "=" & periodo & "!Q10"
    '            hoja3.Cell(33, 9).FormulaA1 = "=" & periodo & "!R10"
    '            hoja3.Cell(34, 9).FormulaA1 = "=" & periodo & "!S10"
    '            hoja3.Cell(35, 9).FormulaA1 = "=" & periodo & "!T10"

    '            'Ciari
    '            hoja3.Cell(6, 10).FormulaA1 = "=" & periodo & "!D11"
    '            hoja3.Cell(7, 10).FormulaA1 = "=" & periodo & "!E11"
    '            hoja3.Cell(8, 10).FormulaA1 = "=" & periodo & "!F11"
    '            hoja3.Cell(9, 10).FormulaA1 = "=" & periodo & "!G11"
    '            'hoja3.Cell(10, 10).FormulaA1 = "=" & periodo & "!H11"
    '            hoja3.Cell(11, 10).FormulaA1 = "=" & periodo & "!I11"
    '            hoja3.Cell(12, 10).FormulaA1 = "=" & periodo & "!H11"
    '            hoja3.Cell(13, 10).FormulaA1 = "=" & periodo & "!J11"
    '            hoja3.Cell(15, 10).Value = ciari
    '            hoja3.Cell(18, 10).Value = ciaripasim
    '            hoja3.Cell(16, 10).FormulaA1 = "=" & periodo & "!Q11"
    '            hoja3.Cell(19, 10).FormulaA1 = "=" & periodo & "!O11"
    '            hoja3.Cell(20, 10).FormulaA1 = "=" & periodo & "!L11"
    '            hoja3.Cell(27, 10).Value = ciariretencion
    '            hoja3.Cell(32, 10).FormulaA1 = "=" & periodo & "!Q11"
    '            hoja3.Cell(33, 10).FormulaA1 = "=" & periodo & "!R11"
    '            hoja3.Cell(34, 10).FormulaA1 = "=" & periodo & "!S11"
    '            hoja3.Cell(35, 10).FormulaA1 = "=" & periodo & "!T11"
    '            'Janitziio
    '            hoja3.Cell(6, 11).FormulaA1 = "=" & periodo & "!D12"
    '            hoja3.Cell(7, 11).FormulaA1 = "=" & periodo & "!E12"
    '            hoja3.Cell(8, 11).FormulaA1 = "=" & periodo & "!F12"
    '            hoja3.Cell(9, 11).FormulaA1 = "=" & periodo & "!G12"
    '            'hoja3.Cell(10, 11).FormulaA1 = "=" & periodo & "!H12"
    '            hoja3.Cell(11, 11).FormulaA1 = "=" & periodo & "!I12"
    '            hoja3.Cell(12, 11).FormulaA1 = "=" & periodo & "!H12"
    '            hoja3.Cell(13, 11).FormulaA1 = "=" & periodo & "!J12"
    '            hoja3.Cell(15, 11).Value = janitzio
    '            hoja3.Cell(18, 11).Value = janitziopasim
    '            hoja3.Cell(16, 11).FormulaA1 = "=" & periodo & "!Q12"
    '            hoja3.Cell(19, 11).FormulaA1 = "=" & periodo & "!O12"
    '            hoja3.Cell(20, 11).FormulaA1 = "=" & periodo & "!L12"
    '            hoja3.Cell(27, 11).Value = janitzioretencion
    '            hoja3.Cell(32, 11).FormulaA1 = "=" & periodo & "!Q12"
    '            hoja3.Cell(33, 11).FormulaA1 = "=" & periodo & "!R12"
    '            hoja3.Cell(34, 11).FormulaA1 = "=" & periodo & "!S12"
    '            hoja3.Cell(35, 11).FormulaA1 = "=" & periodo & "!T12"
    '            'Gabriel
    '            hoja3.Cell(6, 12).FormulaA1 = "=" & periodo & "!D13"
    '            hoja3.Cell(7, 12).FormulaA1 = "=" & periodo & "!E13"
    '            hoja3.Cell(8, 12).FormulaA1 = "=" & periodo & "!F13"
    '            hoja3.Cell(9, 12).FormulaA1 = "=" & periodo & "!G13"
    '            'hoja3.Cell(10, 12).FormulaA1 = "=" & periodo & "!H13"
    '            hoja3.Cell(11, 12).FormulaA1 = "=" & periodo & "!I13"
    '            hoja3.Cell(12, 12).FormulaA1 = "=" & periodo & "!H13"
    '            hoja3.Cell(13, 12).FormulaA1 = "=" & periodo & "!J13"
    '            hoja3.Cell(15, 12).Value = gabriel
    '            hoja3.Cell(18, 12).Value = gabrielpasim
    '            hoja3.Cell(16, 12).FormulaA1 = "=" & periodo & "!Q13"
    '            hoja3.Cell(19, 12).FormulaA1 = "=" & periodo & "!O13"
    '            hoja3.Cell(20, 12).FormulaA1 = "=" & periodo & "!L13"
    '            hoja3.Cell(27, 12).Value = gabrielretencion
    '            hoja3.Cell(32, 12).FormulaA1 = "=" & periodo & "!Q13"
    '            hoja3.Cell(33, 12).FormulaA1 = "=" & periodo & "!R13"
    '            hoja3.Cell(34, 12).FormulaA1 = "=" & periodo & "!S13"
    '            hoja3.Cell(35, 12).FormulaA1 = "=" & periodo & "!T13"
    '            'AMARRADOS
    '            hoja3.Cell(6, 13).FormulaA1 = "=" & periodo & "!D14"
    '            hoja3.Cell(7, 13).FormulaA1 = "=" & periodo & "!E14"
    '            hoja3.Cell(8, 13).FormulaA1 = "=" & periodo & "!F14"
    '            hoja3.Cell(9, 13).FormulaA1 = "=" & periodo & "!G14"
    '            'hoja3.Cell(10, 13).FormulaA1 = "=" & periodo & "!H14"
    '            hoja3.Cell(11, 13).FormulaA1 = "=" & periodo & "!I14"
    '            hoja3.Cell(12, 13).FormulaA1 = "=" & periodo & "!H14"
    '            hoja3.Cell(13, 13).FormulaA1 = "=" & periodo & "!J14"
    '            hoja3.Cell(15, 13).Value = amarrados
    '            hoja3.Cell(18, 13).Value = amarradospasim
    '            hoja3.Cell(16, 13).FormulaA1 = "=" & periodo & "!Q14"
    '            hoja3.Cell(19, 13).FormulaA1 = "=" & periodo & "!O14"
    '            hoja3.Cell(20, 13).FormulaA1 = "=" & periodo & "!L14"
    '            hoja3.Cell(27, 13).Value = amarradosretencion
    '            hoja3.Cell(32, 13).FormulaA1 = "=" & periodo & "!Q14"
    '            hoja3.Cell(33, 13).FormulaA1 = "=" & periodo & "!R14"
    '            hoja3.Cell(34, 13).FormulaA1 = "=" & periodo & "!S14"
    '            hoja3.Cell(35, 13).FormulaA1 = "=" & periodo & "!T14"
    '            'ISLA ARBOLEADA
    '            hoja3.Cell(6, 14).FormulaA1 = "=" & periodo & "!D15"
    '            hoja3.Cell(7, 14).FormulaA1 = "=" & periodo & "!E15"
    '            hoja3.Cell(8, 14).FormulaA1 = "=" & periodo & "!F15"
    '            hoja3.Cell(9, 14).FormulaA1 = "=" & periodo & "!G15"
    '            'hoja3.Cell(10, 14).FormulaA1 = "=" & periodo & "!H15"
    '            hoja3.Cell(11, 14).FormulaA1 = "=" & periodo & "!I15"
    '            hoja3.Cell(12, 14).FormulaA1 = "=" & periodo & "!H15"
    '            hoja3.Cell(13, 14).FormulaA1 = "=" & periodo & "!J15"
    '            hoja3.Cell(15, 14).Value = arboleda
    '            hoja3.Cell(18, 14).Value = arboledapasim
    '            hoja3.Cell(16, 14).FormulaA1 = "=" & periodo & "!Q15"
    '            hoja3.Cell(19, 14).FormulaA1 = "=" & periodo & "!O15"
    '            hoja3.Cell(20, 14).FormulaA1 = "=" & periodo & "!L15"
    '            hoja3.Cell(27, 14).Value = arboledaretencion
    '            hoja3.Cell(32, 14).FormulaA1 = "=" & periodo & "!Q15"
    '            hoja3.Cell(33, 14).FormulaA1 = "=" & periodo & "!R15"
    '            hoja3.Cell(34, 14).FormulaA1 = "=" & periodo & "!S15"
    '            hoja3.Cell(35, 14).FormulaA1 = "=" & periodo & "!T15"
    '            'ISLA AZTECA
    '            hoja3.Cell(6, 15).FormulaA1 = "=" & periodo & "!D16"
    '            hoja3.Cell(7, 15).FormulaA1 = "=" & periodo & "!E16"
    '            hoja3.Cell(8, 15).FormulaA1 = "=" & periodo & "!F16"
    '            hoja3.Cell(9, 15).FormulaA1 = "=" & periodo & "!G16"
    '            'hoja3.Cell(10, 15).FormulaA1 = "=" & periodo & "!H16"
    '            hoja3.Cell(11, 15).FormulaA1 = "=" & periodo & "!I16"
    '            hoja3.Cell(12, 15).FormulaA1 = "=" & periodo & "!H16"
    '            hoja3.Cell(13, 15).FormulaA1 = "=" & periodo & "!J16"
    '            hoja3.Cell(15, 15).Value = azteca
    '            hoja3.Cell(18, 15).Value = aztecapasim
    '            hoja3.Cell(16, 15).FormulaA1 = "=" & periodo & "!Q16"
    '            hoja3.Cell(19, 15).FormulaA1 = "=" & periodo & "!O16"
    '            hoja3.Cell(20, 15).FormulaA1 = "=" & periodo & "!L16"
    '            hoja3.Cell(27, 15).Value = aztecaretencion
    '            hoja3.Cell(32, 15).FormulaA1 = "=" & periodo & "!Q16"
    '            hoja3.Cell(33, 15).FormulaA1 = "=" & periodo & "!R16"
    '            hoja3.Cell(34, 15).FormulaA1 = "=" & periodo & "!S16"
    '            hoja3.Cell(35, 15).FormulaA1 = "=" & periodo & "!T16"
    '            'ISLA SAN DIEGO
    '            hoja3.Cell(6, 16).FormulaA1 = "=" & periodo & "!D17"
    '            hoja3.Cell(7, 16).FormulaA1 = "=" & periodo & "!E17"
    '            hoja3.Cell(8, 16).FormulaA1 = "=" & periodo & "!F17"
    '            hoja3.Cell(9, 16).FormulaA1 = "=" & periodo & "!G17"
    '            'hoja3.Cell(10, 16).FormulaA1 = "=" & periodo & "!H17"
    '            hoja3.Cell(11, 16).FormulaA1 = "=" & periodo & "!I17"
    '            hoja3.Cell(12, 16).FormulaA1 = "=" & periodo & "!H17"
    '            hoja3.Cell(13, 16).FormulaA1 = "=" & periodo & "!J17"
    '            hoja3.Cell(15, 16).Value = diego
    '            hoja3.Cell(18, 16).Value = diegopasim
    '            hoja3.Cell(16, 16).FormulaA1 = "=" & periodo & "!Q17"
    '            hoja3.Cell(19, 16).FormulaA1 = "=" & periodo & "!O17"
    '            hoja3.Cell(20, 16).FormulaA1 = "=" & periodo & "!L17"
    '            hoja3.Cell(27, 16).Value = diegoretencion
    '            hoja3.Cell(32, 16).FormulaA1 = "=" & periodo & "!Q17"
    '            hoja3.Cell(33, 16).FormulaA1 = "=" & periodo & "!R17"
    '            hoja3.Cell(34, 16).FormulaA1 = "=" & periodo & "!S17"
    '            hoja3.Cell(35, 16).FormulaA1 = "=" & periodo & "!T17"
    '            'ISLA SAN IGNACIO
    '            hoja3.Cell(6, 17).FormulaA1 = "=" & periodo & "!D18"
    '            hoja3.Cell(7, 17).FormulaA1 = "=" & periodo & "!E18"
    '            hoja3.Cell(8, 17).FormulaA1 = "=" & periodo & "!F18"
    '            hoja3.Cell(9, 17).FormulaA1 = "=" & periodo & "!G18"
    '            'hoja3.Cell(10, 17).FormulaA1 = "=" & periodo & "!H18"
    '            hoja3.Cell(11, 17).FormulaA1 = "=" & periodo & "!I18"
    '            hoja3.Cell(12, 17).FormulaA1 = "=" & periodo & "!H18"
    '            hoja3.Cell(13, 17).FormulaA1 = "=" & periodo & "!J18"
    '            hoja3.Cell(15, 17).Value = ignacio
    '            hoja3.Cell(18, 17).Value = ignaciopasim
    '            hoja3.Cell(16, 17).FormulaA1 = "=" & periodo & "!Q18"
    '            hoja3.Cell(19, 17).FormulaA1 = "=" & periodo & "!O18"
    '            hoja3.Cell(20, 17).FormulaA1 = "=" & periodo & "!L18"
    '            hoja3.Cell(27, 17).Value = ignacioretencion
    '            hoja3.Cell(32, 17).FormulaA1 = "=" & periodo & "!Q18"
    '            hoja3.Cell(33, 17).FormulaA1 = "=" & periodo & "!R18"
    '            hoja3.Cell(34, 17).FormulaA1 = "=" & periodo & "!S18"
    '            hoja3.Cell(35, 17).FormulaA1 = "=" & periodo & "!T18"
    '            'ISLA SAN LUIS
    '            hoja3.Cell(6, 18).FormulaA1 = "=" & periodo & "!D19"
    '            hoja3.Cell(7, 18).FormulaA1 = "=" & periodo & "!E19"
    '            hoja3.Cell(8, 18).FormulaA1 = "=" & periodo & "!F19"
    '            hoja3.Cell(9, 18).FormulaA1 = "=" & periodo & "!G19"
    '            'hoja3.Cell(10, 18).FormulaA1 = "=" & periodo & "!H19"
    '            hoja3.Cell(11, 18).FormulaA1 = "=" & periodo & "!I19"
    '            hoja3.Cell(12, 18).FormulaA1 = "=" & periodo & "!H19"
    '            hoja3.Cell(13, 18).FormulaA1 = "=" & periodo & "!J19"
    '            hoja3.Cell(15, 18).Value = luis
    '            hoja3.Cell(18, 18).Value = luispasim
    '            hoja3.Cell(16, 18).FormulaA1 = "=" & periodo & "!Q19"
    '            hoja3.Cell(19, 18).FormulaA1 = "=" & periodo & "!O19"
    '            hoja3.Cell(20, 18).FormulaA1 = "=" & periodo & "!L19"
    '            hoja3.Cell(27, 18).Value = luisretencion
    '            hoja3.Cell(32, 18).FormulaA1 = "=" & periodo & "!Q19"
    '            hoja3.Cell(33, 18).FormulaA1 = "=" & periodo & "!R19"
    '            hoja3.Cell(34, 18).FormulaA1 = "=" & periodo & "!S19"
    '            hoja3.Cell(35, 18).FormulaA1 = "=" & periodo & "!T19"

    '            'ISLA SANTA CRUZ
    '            hoja3.Cell(6, 19).FormulaA1 = "=" & periodo & "!D20"
    '            hoja3.Cell(7, 19).FormulaA1 = "=" & periodo & "!E20"
    '            hoja3.Cell(8, 19).FormulaA1 = "=" & periodo & "!F20"
    '            hoja3.Cell(9, 19).FormulaA1 = "=" & periodo & "!G20"
    '            'hoja3.Cell(10,19).FormulaA1 = "=" & periodo & "!H20"
    '            hoja3.Cell(11, 19).FormulaA1 = "=" & periodo & "!I20"
    '            hoja3.Cell(12, 19).FormulaA1 = "=" & periodo & "!H20"
    '            hoja3.Cell(13, 19).FormulaA1 = "=" & periodo & "!J20"
    '            hoja3.Cell(15, 19).Value = cruz
    '            hoja3.Cell(18, 19).Value = cruzpasim
    '            hoja3.Cell(16, 19).FormulaA1 = "=" & periodo & "!Q20"
    '            hoja3.Cell(19, 19).FormulaA1 = "=" & periodo & "!O20"
    '            hoja3.Cell(20, 19).FormulaA1 = "=" & periodo & "!L20"
    '            hoja3.Cell(27, 19).Value = cruzretencion
    '            hoja3.Cell(32, 19).FormulaA1 = "=" & periodo & "!Q20"
    '            hoja3.Cell(33, 19).FormulaA1 = "=" & periodo & "!R20"
    '            hoja3.Cell(34, 19).FormulaA1 = "=" & periodo & "!S20"
    '            hoja3.Cell(35, 19).FormulaA1 = "=" & periodo & "!T20"
    '            'ISLA VERDE
    '            hoja3.Cell(6, 20).FormulaA1 = "=" & periodo & "!D21"
    '            hoja3.Cell(7, 20).FormulaA1 = "=" & periodo & "!E21"
    '            hoja3.Cell(8, 20).FormulaA1 = "=" & periodo & "!F21"
    '            hoja3.Cell(9, 20).FormulaA1 = "=" & periodo & "!G21"
    '            'hoja3.Cell(10,20).FormulaA1 = "=" & periodo & "!H21"
    '            hoja3.Cell(11, 20).FormulaA1 = "=" & periodo & "!I21"
    '            hoja3.Cell(12, 20).FormulaA1 = "=" & periodo & "!H21"
    '            hoja3.Cell(13, 20).FormulaA1 = "=" & periodo & "!J21"
    '            hoja3.Cell(15, 20).Value = verde
    '            hoja3.Cell(18, 20).Value = verdepasim
    '            hoja3.Cell(16, 20).FormulaA1 = "=" & periodo & "!Q21"
    '            hoja3.Cell(19, 20).FormulaA1 = "=" & periodo & "!O21"
    '            hoja3.Cell(20, 20).FormulaA1 = "=" & periodo & "!L21"
    '            hoja3.Cell(27, 20).Value = verderetencion
    '            hoja3.Cell(32, 20).FormulaA1 = "=" & periodo & "!Q21"
    '            hoja3.Cell(33, 20).FormulaA1 = "=" & periodo & "!R21"
    '            hoja3.Cell(34, 20).FormulaA1 = "=" & periodo & "!S21"
    '            hoja3.Cell(35, 20).FormulaA1 = "=" & periodo & "!T21"

    '            'ISLA CRECIENTE
    '            hoja3.Cell(6, 21).FormulaA1 = "=" & periodo & "!D22"
    '            hoja3.Cell(7, 21).FormulaA1 = "=" & periodo & "!E22"
    '            hoja3.Cell(8, 21).FormulaA1 = "=" & periodo & "!F22"
    '            hoja3.Cell(9, 21).FormulaA1 = "=" & periodo & "!G22"
    '            'hoja3.Cell(10,21).FormulaA1 = "=" & periodo & "!H22"
    '            hoja3.Cell(11, 21).FormulaA1 = "=" & periodo & "!I22"
    '            hoja3.Cell(12, 21).FormulaA1 = "=" & periodo & "!H22"
    '            hoja3.Cell(13, 21).FormulaA1 = "=" & periodo & "!J22"
    '            hoja3.Cell(15, 21).Value = creciente
    '            hoja3.Cell(18, 21).Value = crecientepasim
    '            hoja3.Cell(16, 21).FormulaA1 = "=" & periodo & "!Q22"
    '            hoja3.Cell(19, 21).FormulaA1 = "=" & periodo & "!O22"
    '            hoja3.Cell(20, 21).FormulaA1 = "=" & periodo & "!L22"
    '            hoja3.Cell(27, 21).Value = crecienteretencion
    '            hoja3.Cell(32, 21).FormulaA1 = "=" & periodo & "!Q22"
    '            hoja3.Cell(33, 21).FormulaA1 = "=" & periodo & "!R22"
    '            hoja3.Cell(34, 21).FormulaA1 = "=" & periodo & "!S22"
    '            hoja3.Cell(35, 21).FormulaA1 = "=" & periodo & "!T22"

    '            'ISLA COLORADA
    '            hoja3.Cell(6, 22).FormulaA1 = "=" & periodo & "!D23"
    '            hoja3.Cell(7, 22).FormulaA1 = "=" & periodo & "!E23"
    '            hoja3.Cell(8, 22).FormulaA1 = "=" & periodo & "!F23"
    '            hoja3.Cell(9, 22).FormulaA1 = "=" & periodo & "!G23"
    '            'hoja3.Cell(10,22).FormulaA1 = "=" & periodo & "!H23"
    '            hoja3.Cell(11, 22).FormulaA1 = "=" & periodo & "!I23"
    '            hoja3.Cell(12, 22).FormulaA1 = "=" & periodo & "!H23"
    '            hoja3.Cell(13, 22).FormulaA1 = "=" & periodo & "!J23"
    '            hoja3.Cell(15, 22).Value = colorada
    '            hoja3.Cell(18, 22).Value = coloradapasim
    '            hoja3.Cell(16, 22).FormulaA1 = "=" & periodo & "!Q23"
    '            hoja3.Cell(19, 22).FormulaA1 = "=" & periodo & "!O23"
    '            hoja3.Cell(20, 22).FormulaA1 = "=" & periodo & "!L23"
    '            hoja3.Cell(27, 22).Value = coloradaretencion
    '            hoja3.Cell(32, 22).FormulaA1 = "=" & periodo & "!Q23"
    '            hoja3.Cell(33, 22).FormulaA1 = "=" & periodo & "!R23"
    '            hoja3.Cell(34, 22).FormulaA1 = "=" & periodo & "!S23"
    '            hoja3.Cell(35, 22).FormulaA1 = "=" & periodo & "!T23"

    '            'SUBSEA 88
    '            hoja3.Cell(6, 23).FormulaA1 = "=" & periodo & "!D24"
    '            hoja3.Cell(7, 23).FormulaA1 = "=" & periodo & "!E24"
    '            hoja3.Cell(8, 23).FormulaA1 = "=" & periodo & "!F24"
    '            hoja3.Cell(9, 23).FormulaA1 = "=" & periodo & "!G24"
    '            'hoja3.Cell(10,23).FormulaA1 = "=" & periodo & "!H24"
    '            hoja3.Cell(11, 23).FormulaA1 = "=" & periodo & "!I24"
    '            hoja3.Cell(12, 23).FormulaA1 = "=" & periodo & "!H24"
    '            hoja3.Cell(13, 23).FormulaA1 = "=" & periodo & "!J24"
    '            hoja3.Cell(15, 23).Value = subsea88
    '            hoja3.Cell(18, 23).Value = subsea88pasim
    '            hoja3.Cell(16, 23).FormulaA1 = "=" & periodo & "!Q24"
    '            hoja3.Cell(19, 23).FormulaA1 = "=" & periodo & "!O24"
    '            hoja3.Cell(20, 23).FormulaA1 = "=" & periodo & "!L24"
    '            hoja3.Cell(27, 23).Value = subsea88retencion
    '            hoja3.Cell(32, 23).FormulaA1 = "=" & periodo & "!Q24"
    '            hoja3.Cell(33, 23).FormulaA1 = "=" & periodo & "!R24"
    '            hoja3.Cell(34, 23).FormulaA1 = "=" & periodo & "!S24"
    '            hoja3.Cell(35, 23).FormulaA1 = "=" & periodo & "!T24"

    '            'ISLA LEON
    '            hoja3.Cell(6, 24).FormulaA1 = "=" & periodo & "!D25"
    '            hoja3.Cell(7, 24).FormulaA1 = "=" & periodo & "!E25"
    '            hoja3.Cell(8, 24).FormulaA1 = "=" & periodo & "!F25"
    '            hoja3.Cell(9, 24).FormulaA1 = "=" & periodo & "!G25"
    '            'hoja3.Cell(10,24).FormulaA1 = "=" & periodo & "!H25"
    '            hoja3.Cell(11, 24).FormulaA1 = "=" & periodo & "!I25"
    '            hoja3.Cell(12, 24).FormulaA1 = "=" & periodo & "!H25"
    '            hoja3.Cell(13, 24).FormulaA1 = "=" & periodo & "!J25"
    '            hoja3.Cell(15, 24).Value = leon
    '            hoja3.Cell(18, 24).Value = leonpasim
    '            hoja3.Cell(16, 24).FormulaA1 = "=" & periodo & "!Q25"
    '            hoja3.Cell(19, 24).FormulaA1 = "=" & periodo & "!O25"
    '            hoja3.Cell(20, 24).FormulaA1 = "=" & periodo & "!L25"
    '            hoja3.Cell(27, 24).Value = leonretencion
    '            hoja3.Cell(32, 24).FormulaA1 = "=" & periodo & "!Q25"
    '            hoja3.Cell(33, 24).FormulaA1 = "=" & periodo & "!R25"
    '            hoja3.Cell(34, 24).FormulaA1 = "=" & periodo & "!S25"
    '            hoja3.Cell(35, 24).FormulaA1 = "=" & periodo & "!T25"

    '            'NEVADO DE COLIMA
    '            hoja3.Cell(6, 25).FormulaA1 = "=" & periodo & "!D26"
    '            hoja3.Cell(7, 25).FormulaA1 = "=" & periodo & "!E26"
    '            hoja3.Cell(8, 25).FormulaA1 = "=" & periodo & "!F26"
    '            hoja3.Cell(9, 25).FormulaA1 = "=" & periodo & "!G26"
    '            'hoja3.Cell(10,25).FormulaA1 = "=" & periodo & "!H26"
    '            hoja3.Cell(11, 25).FormulaA1 = "=" & periodo & "!I26"
    '            hoja3.Cell(12, 25).FormulaA1 = "=" & periodo & "!H26"
    '            hoja3.Cell(13, 25).FormulaA1 = "=" & periodo & "!J26"
    '            hoja3.Cell(15, 25).Value = nevado
    '            hoja3.Cell(18, 25).Value = nevadoasim
    '            hoja3.Cell(16, 25).FormulaA1 = "=" & periodo & "!Q26"
    '            hoja3.Cell(19, 25).FormulaA1 = "=" & periodo & "!O26"
    '            hoja3.Cell(20, 25).FormulaA1 = "=" & periodo & "!L26"
    '            hoja3.Cell(27, 25).Value = nevadoretencion
    '            hoja3.Cell(32, 25).FormulaA1 = "=" & periodo & "!Q26"
    '            hoja3.Cell(33, 25).FormulaA1 = "=" & periodo & "!R26"
    '            hoja3.Cell(34, 25).FormulaA1 = "=" & periodo & "!S26"
    '            hoja3.Cell(35, 25).FormulaA1 = "=" & periodo & "!T26"

    '            'Titulo
    '            Dim moment As Date = Date.Now()
    '            Dim month As Integer = moment.Month
    '            Dim year As Integer = moment.Year

    '            Dim rwUsuario As DataRow() = nConsulta("Select * from Usuarios where idUsuario=1")




    '            dialogo.FileName = "REPORTE " & rwUsuario(0).Item("Nombre").ToUpper & " " & periodo.ToUpper & " " & iejercicio & " SERIE " & cboserie.SelectedItem
    '            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
    '            ''  dialogo.ShowDialog()

    '            If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
    '                ' OK button pressed
    '                libro.SaveAs(dialogo.FileName)
    '                libro = Nothing
    '                pnlProgreso.Visible = False
    '                pnlCatalogo.Enabled = True

    '                MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Else
    '                pnlProgreso.Visible = False
    '                pnlCatalogo.Enabled = True

    '                MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

    '            End If
    '        End If

    '    Catch ex As Exception
    '        pnlProgreso.Visible = False
    '        pnlCatalogo.Enabled = True

    '        MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    End Try
    'End Sub

    'Public Sub llenardesgloce(ByRef nombrebuque As String, ByRef contadorexcelbuquefinal As Integer, ByRef hoja As IXLWorksheet)

    '    Select Case nombrebuque
    '        Case "CEDROS", "ISLA CEDROS"
    '            hoja.Cell(5, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(5, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1 'SUBSIDIO
    '            hoja.Cell(5, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 'PRESTAMO ASM   
    '            hoja.Cell(5, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1 'IMSS
    '            hoja.Cell(5, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1 'SAR
    '            hoja.Cell(5, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1 'INFONAVIT
    '            hoja.Cell(5, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(5, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(5, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M5+Q5+R5+S5+T5)*6%" 'retencion

    '        Case "ISLA SAN JOSE"
    '            hoja.Cell(6, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(6, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(6, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(6, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M6+Q6+R6+S6+T6)*6%"
    '        Case "ISLA GRANDE"
    '            hoja.Cell(7, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(7, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(7, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(7, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M7+Q7+R7+S7+T7)*6%"
    '        Case "ISLA MIRAMAR"
    '            hoja.Cell(8, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(8, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(8, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(8, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M8+Q8+R8+S8+T8)*6%"
    '        Case "ISLA MONSERRAT", "ISLA MONTSERRAT"
    '            hoja.Cell(9, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(9, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(9, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(9, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M9+Q9+R9+S9+T9)*6%"
    '        Case "ISLA BLANCA"
    '            hoja.Cell(10, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(10, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(10, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(10, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M10+Q10+R10+S10+T10)*6%"
    '        Case "ISLA CIARI"
    '            hoja.Cell(11, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(11, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(11, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(11, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M11+Q11+R11+S11+T11)*6%"
    '        Case "ISLA JANITZIO"
    '            hoja.Cell(12, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(12, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(12, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(12, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M12+Q12+R12+S12+T12)*6%"

    '        Case "ISLA SAN GABRIEL"
    '            hoja.Cell(13, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(13, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(13, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(13, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M13+Q13+R13+S13+T13)*6%"
    '        Case "AMARRADOS"
    '            hoja.Cell(14, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(14, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(14, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(14, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M14+Q14+R14+S14+T14)*6%"
    '        Case "ISLA ARBOLEDA"
    '            hoja.Cell(15, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(15, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(15, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(15, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M15+Q15+R15+S15+T15)*6%"

    '        Case "ISLA AZTECA"
    '            hoja.Cell(16, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(16, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(16, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(16, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M16+Q16+R16+S16+T16)*6%"

    '        Case "ISLA SAN DIEGO", "ISLA DIEGO"
    '            hoja.Cell(17, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(17, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(17, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(17, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M17+Q17+R17+S17+T17)*6%"
    '        Case "ISLA SAN IGNACIO", "ISLA IGNACIO"
    '            hoja.Cell(18, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(18, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(18, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(18, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M18+Q18+R18+S18+T18)*6%"
    '        Case "ISLA SAN LUIS"
    '            hoja.Cell(19, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(19, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(19, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(19, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M19+Q19+R19+S19+T19)*6%"
    '        Case "ISLA SANTA CRUZ"
    '            hoja.Cell(20, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(20, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(20, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(20, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M20+Q20+R20+S20+T20)*6%"
    '        Case "ISLA VERDE"
    '            hoja.Cell(21, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(21, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(21, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(21, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M21+Q21+R21+S21+T21)*6%"
    '        Case "ISLA CRECIENTE"
    '            hoja.Cell(22, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(22, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(22, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(22, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M22+Q22+R22+S22+T22)*6%"
    '        Case "ISLA COLORADA"
    '            hoja.Cell(23, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(23, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(23, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(23, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M23+Q23+R23+S23+T23)*6%"
    '        Case "SUBSEA 88"
    '            hoja.Cell(24, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(24, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(24, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(24, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M24+Q24+R24+S24+T24)*6%"
    '        Case "ISLA LEON"
    '            hoja.Cell(25, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(25, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(25, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(25, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M25+Q25+R25+S25+T25)*6%"
    '        Case "NEVADO DE COLIMA", "NEVADO COLIMA"
    '            hoja.Cell(26, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 17).FormulaA1 = "=DESGLOSE!AB" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 18).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 19).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
    '            hoja.Cell(26, 20).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1 'IMPTO
    '            hoja.Cell(26, 21).FormulaA1 = "=DESGLOSE!AG" & contadorexcelbuquefinal + 1 'SUBTOTAL
    '            hoja.Cell(26, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AA" & contadorexcelbuquefinal + 1 & "+M26+Q26+R26+S26+T26)*6%"
    '    End Select


    'End Sub

    Private Sub chkbTodo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbTodo.CheckedChanged
        If chkbTodo.Checked Then
            cboTipoNomina.Enabled = False
        Else
            cboTipoNomina.Enabled = True
        End If

    End Sub

   
    Private Sub chkbTodasSeries_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbTodasSeries.CheckedChanged
        If chkbTodasSeries.Checked Then
            cboserie.Enabled = False
        Else
            cboserie.Enabled = True
        End If
    End Sub

  
   
End Class