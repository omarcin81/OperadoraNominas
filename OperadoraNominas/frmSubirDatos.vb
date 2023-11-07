Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Public Class frmSubirDatos
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Public dsReporte As New DataSet


    Private Sub tsbNuevo_Click(sender As System.Object, e As System.EventArgs) Handles tsbNuevo.Click
        tsbNuevo.Enabled = False
        tsbImportar.Enabled = True
        tsbImportar_Click(sender, e)
    End Sub

    Private Sub tsbImportar_Click(sender As System.Object, e As System.EventArgs) Handles tsbImportar.Click
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

    Private Sub tsbProcesar_Click(sender As System.Object, e As System.EventArgs) Handles tsbProcesar.Click
        lsvLista.Items.Clear()
        lsvLista.Columns.Clear()
        lsvLista.Clear()

        ' pnlCatalogo.Enabled = False
        tsbGuardar.Enabled = False
        tsbCancelar.Enabled = False
        lsvLista.Visible = False
        tsbImportar.Enabled = False
        Me.cmdCerrar.Enabled = False
        tsbEmpleados.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Application.DoEvents()

        Try
            If File.Exists(lblRuta.Text) Then
                Dim Archivo As String = lblRuta.Text
                Dim Hoja As String


                Dim book As New ClosedXML.Excel.XLWorkbook(Archivo)
                If book.Worksheets.Count >= 1 Then
                    sheetIndex = 1
                    If book.Worksheets.Count > 1 Then
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

                        lsvLista.Columns.Add(sheet.Cell(1, c).Value.ToString)
                        'lsvLista.Columns.Add(numerocolumna)
                        'numerocolumna = numerocolumna + 1

                    Next

                    'lsvLista.Columns.Add("conciliacion")
                    'lsvLista.Columns.Add("color")

                    lsvLista.Columns(1).Width = 90

                    lsvLista.Columns(2).Width = 50
                    lsvLista.Columns(3).Width = 90
                    lsvLista.Columns(4).Width = 90
                    lsvLista.Columns(5).Width = 00
                    lsvLista.Columns(6).Width = 50
                    lsvLista.Columns(7).Width = 150
                    lsvLista.Columns(8).Width = 150
                    lsvLista.Columns(9).Width = 190
                    lsvLista.Columns(10).Width = 90
                    lsvLista.Columns(11).Width = 90
                    lsvLista.Columns(12).Width = 90
                    lsvLista.Columns(13).Width = 90
                    lsvLista.Columns(14).Width = 200
                    lsvLista.Columns(15).Width = 150
                    lsvLista.Columns(16).Width = 100
                    lsvLista.Columns(17).Width = 160
                    lsvLista.Columns(18).Width = 60
                    lsvLista.Columns(19).Width = 60
                    lsvLista.Columns(20).Width = 60
                    lsvLista.Columns(21).Width = 60
                    lsvLista.Columns(22).Width = 60
                    lsvLista.Columns(23).Width = 60
                    lsvLista.Columns(24).Width = 60
                    lsvLista.Columns(25).Width = 60
                    lsvLista.Columns(26).Width = 60
                    lsvLista.Columns(27).Width = 60
                    lsvLista.Columns(28).Width = 60
                    lsvLista.Columns(29).Width = 60
                    lsvLista.Columns(30).Width = 60
                    lsvLista.Columns(31).Width = 60
                    lsvLista.Columns(32).Width = 60
                    lsvLista.Columns(33).Width = 60
                    lsvLista.Columns(34).Width = 60
                    lsvLista.Columns(35).Width = 60
                    lsvLista.Columns(36).Width = 60
                    lsvLista.Columns(37).Width = 60
                    lsvLista.Columns(38).Width = 60
                    lsvLista.Columns(39).Width = 60
                    lsvLista.Columns(40).Width = 60
                    lsvLista.Columns(41).Width = 60
                    lsvLista.Columns(42).Width = 60
                    lsvLista.Columns(43).Width = 60
                    lsvLista.Columns(44).Width = 60
                    lsvLista.Columns(45).Width = 60
                    lsvLista.Columns(46).Width = 60
                    lsvLista.Columns(47).Width = 60
                    lsvLista.Columns(48).Width = 60
                    lsvLista.Columns(49).Width = 60
                    lsvLista.Columns(50).Width = 60
                    lsvLista.Columns(51).Width = 60
                    lsvLista.Columns(52).Width = 60
                    lsvLista.Columns(53).Width = 60
                    lsvLista.Columns(54).Width = 60
                    lsvLista.Columns(55).Width = 60
                    lsvLista.Columns(56).Width = 60
                    lsvLista.Columns(57).Width = 60
                    lsvLista.Columns(58).Width = 60
                    lsvLista.Columns(59).Width = 60
                    lsvLista.Columns(60).Width = 60
                    lsvLista.Columns(61).Width = 60
                    lsvLista.Columns(62).Width = 60
                    lsvLista.Columns(63).Width = 60
                    lsvLista.Columns(64).Width = 60
                    lsvLista.Columns(65).Width = 60
                    lsvLista.Columns(66).Width = 60
                    lsvLista.Columns(67).Width = 60



                    Dim Filas As Long = sheet.RowsUsed().Count()
                    For f As Integer = 2 To Filas
                        Dim item As ListViewItem = lsvLista.Items.Add((f - 1).ToString())
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

                                'If Valor = "" Then

                                '    item.SubItems.Add("0.0")

                                'Else

                                ' item.SubItems.Add(Valor)
                                'End If



                If f = 6 And c >= 12 Then

                    'If Valor <> "" AndAlso InStr(Valor, "-") > 0 Then
                    '    Dim sValores() As String = Valor.Split("-")
                    '    Select Case sValores(0).ToUpper()
                    '        Case "P"
                    '            item.SubItems(item.SubItems.Count - 1).Tag = "1" 'Percepción
                    '        Case "D"
                    '            item.SubItems(item.SubItems.Count - 1).Tag = "2" 'Deducción
                    '        Case "I"
                    '            item.SubItems(item.SubItems.Count - 1).Tag = "3" 'Incapacidad
                    '    End Select
                    '    Valor = sValores(1).Trim()
                    'End If

                    item.SubItems(item.SubItems.Count - 1).Text = valor

                End If



                            Catch ex As Exception

        End Try

                        Next
                    Next

                    book.Dispose()
                    book = Nothing
                    GC.Collect()
                    'If lsvNominaFile.Items.Count >= 9 Then
                    '    If chkTipo.Checked Then
                    '        ProcesarNomina()
                    '    Else
                    '        ProcesarNomina1()
                    '    End If

                    'End If
                    pnlCatalogo.Enabled = True
                    If lsvLista.Items.Count = 0 Then
                        MessageBox.Show("El catálogo no puso ser importado o no contiene registros." & vbCrLf & "¿Por favor verifique?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Se han encontrado " & FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        tsbGuardar.Enabled = True
                        tsbCancelar.Enabled = True
                        tsbAgregar.Enabled = True
                        tsbEmpleados.Enabled = True

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

        End Try
    End Sub

    Private Function getColumnName(index As Single) As String
        Dim numletter As Single = 26
        Dim sGrupo As Single = index / numletter
        Dim Modulo As Single = sGrupo - Math.Truncate(sGrupo)

        If Modulo = 0 Then Modulo = 1
        Dim Grupo As Integer = sGrupo - Modulo

        Dim Indice As Integer = index - (Grupo * numletter)
        Dim ColumnName As String = ""

        If Grupo > 0 Then
            ColumnName = Chr(64 + Grupo)
        End If
        ColumnName &= Chr(64 + Indice)
        Return ColumnName

    End Function


    Private Sub tsbCancelar_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancelar.Click
        'pnlCatalogo.Enabled = False
        lsvLista.Items.Clear()
        chkAll.Checked = False
        lblRuta.Text = ""
        tsbImportar.Enabled = False
        tsbProcesar.Enabled = False
        tsbGuardar.Enabled = False
        tsbCancelar.Enabled = False
        tsbAgregar.Enabled = False
        tsbNuevo.Enabled = True
    End Sub

    Private Sub cmdCerrar_Click(sender As System.Object, e As System.EventArgs) Handles cmdCerrar.Click
        Me.Close()
    End Sub

  

    Private Sub tsbGuardar_Click(sender As System.Object, e As System.EventArgs) Handles tsbGuardar.Click
        Dim SQL As String, nombresistema As String = ""
        Try
            If lsvLista.CheckedItems.Count > 0 Then
                Dim mensaje As String


                pnlProgreso.Visible = True
                'pnlCatalogo.Enabled = False
                Application.DoEvents()


                Dim IdProducto As Long
                Dim i As Integer = 0
                Dim conta As Integer = 0


                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = lsvLista.CheckedItems.Count


               

                For Each producto As ListViewItem In lsvLista.CheckedItems
                    If Len(producto.SubItems(1).Text) < 4 Then
                        SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(1).Text)
                    Else
                        SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(1).Text).Substring(2, 4)
                    End If


                    Dim rwFilas As DataRow() = nConsulta(SQL)

                    If rwFilas Is Nothing = False Then
                        If rwFilas.Length > 1 Then
                            producto.BackColor = Color.Yellow
                        Else
                            producto.BackColor = Color.Green
                        End If



                    Else
                        producto.BackColor = Color.Red
                    End If
                    pgbProgreso.Value += 1

                Next

                'Enviar correo
                'Enviar_Mail(GenerarCorreoFlujo("Importación Flujo-Conceptos", "Área Facturación", "Se importo un flujo con los conceptos necesarios"), "g.gomez@mbcgroup.mx", "Importación")





                'tsbCancelar_Click(sender, e)
                pnlProgreso.Visible = False
                MessageBox.Show("Proceso terminado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            pnlCatalogo.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub tsbAgregar_Click(sender As System.Object, e As System.EventArgs) Handles tsbAgregar.Click
        Try
            Dim resultado As Integer = MessageBox.Show("Solo se agregaran los registros seleccionados y en color verde, ¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then

                If lsvLista.CheckedItems.Count > 0 Then


                    dsReporte.Tables.Add("Tabla")
                    dsReporte.Tables("Tabla").Columns.Add("Id_empleado")
                    dsReporte.Tables("Tabla").Columns.Add("CodigoEmpleado")
                    dsReporte.Tables("Tabla").Columns.Add("dias")
                    dsReporte.Tables("Tabla").Columns.Add("Salario")
                    dsReporte.Tables("Tabla").Columns.Add("Bono")
                    dsReporte.Tables("Tabla").Columns.Add("Refrendo")
                    dsReporte.Tables("Tabla").Columns.Add("SalarioTMM")
                    dsReporte.Tables("Tabla").Columns.Add("CodigoPuesto")
                    dsReporte.Tables("Tabla").Columns.Add("CodigoBuque")
                    dsReporte.Tables("Tabla").Columns.Add("Anticipo")
                    dsReporte.Tables("Tabla").Columns.Add("AnticipoSA")
                    dsReporte.Tables("Tabla").Columns.Add("InfonavitSA")
                    dsReporte.Tables("Tabla").Columns.Add("InfonavitBIASA")
                    dsReporte.Tables("Tabla").Columns.Add("InfonavitASI")
                    dsReporte.Tables("Tabla").Columns.Add("InfonavitBIAASI")
                    dsReporte.Tables("Tabla").Columns.Add("Fechainicio")
                    dsReporte.Tables("Tabla").Columns.Add("Fechafin")


                    Dim mensaje As String

                    pnlProgreso.Visible = True
                    pnlCatalogo.Enabled = False
                    Application.DoEvents()

                    Dim IdProducto As Long
                    Dim i As Integer = 0
                    Dim conta As Integer = 0

                    pgbProgreso.Minimum = 0
                    pgbProgreso.Value = 0
                    pgbProgreso.Maximum = lsvLista.CheckedItems.Count

                    For Each producto As ListViewItem In lsvLista.CheckedItems

                        SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(3).Text)
                        

                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then
                    If rwFilas.Length = 1 Then
                        producto.BackColor = Color.Green
                        Dim fila As DataRow = dsReporte.Tables("Tabla").NewRow


                                fila.Item("Id_empleado") = rwFilas(0)("iIdEmpleadoC")
                                fila.Item("MES") = Trim(producto.SubItems(1).Text)
                                fila.Item("PERIODO") = Trim(producto.SubItems(2).Text)
                                fila.Item("CLAVE") = Trim(producto.SubItems(3).Text)
                                fila.Item("CENTRO_COSTO") = Trim(producto.SubItems(4).Text)
                                fila.Item("NOMBRE") = Trim(producto.SubItems(5).Text)
                                fila.Item("AREA") = Trim(producto.SubItems(6).Text)
                                fila.Item("CATEGORIA") = Trim(producto.SubItems(7).Text)
                                fila.Item("PUESTO") = Trim(producto.SubItems(8).Text)
                                fila.Item("DEPARTAMENTO") = Trim(producto.SubItems(9).Text)
                                fila.Item("NOMINA") = Trim(producto.SubItems(10).Text)
                                fila.Item("CONTRATO") = Trim(producto.SubItems(11).Text)
                                fila.Item("RFC") = Trim(producto.SubItems(12).Text)
                                fila.Item("IMSS") = Trim(producto.SubItems(13).Text)
                                fila.Item("CURP") = Trim(producto.SubItems(14).Text)
                                fila.Item("ALTA") = Trim(producto.SubItems(15).Text)
                                fila.Item("BCO_DEPOSITO") = Trim(producto.SubItems(16).Text)
                                fila.Item("CTA_DEPOSITO") = Trim(producto.SubItems(17).Text)
                                fila.Item("fTExtra2V") = IIf(Trim(producto.SubItems(18).Text) = "", "0", Trim(producto.SubItems(18).Text))
                                fila.Item("fTExtra3V") = IIf(Trim(producto.SubItems(19).Text) = "", "0", Trim(producto.SubItems(19).Text))
                                fila.Item("fDescansoLV") = IIf(Trim(producto.SubItems(20).Text) = "", "0", Trim(producto.SubItems(20).Text))
                                fila.Item("fDiaFestivoLV") = IIf(Trim(producto.SubItems(21).Text) = "", "0", Trim(producto.SubItems(21).Text))
                                fila.Item("fHoras_extras_dobles_V") = IIf(Trim(producto.SubItems(22).Text) = "", "0", Trim(producto.SubItems(22).Text))
                                fila.Item("fHoras_extras_triples_V") = IIf(Trim(producto.SubItems(23).Text) = "", "0", Trim(producto.SubItems(23).Text))
                                fila.Item("fDescanso_Laborado_V") = IIf(Trim(producto.SubItems(24).Text) = "", "0", Trim(producto.SubItems(24).Text))
                                fila.Item("fDia_Festivo_laborado_V") = IIf(Trim(producto.SubItems(25).Text) = "", "0", Trim(producto.SubItems(25).Text))
                                fila.Item("fPrima_Dominical_V") = IIf(Trim(producto.SubItems(26).Text) = "", "0", Trim(producto.SubItems(26).Text))
                                fila.Item("fFalta_Injustificada_V") = IIf(Trim(producto.SubItems(27).Text) = "", "0", Trim(producto.SubItems(27).Text))
                                fila.Item("fPermiso_Sin_GS_V") = IIf(Trim(producto.SubItems(28).Text) = "", "0", Trim(producto.SubItems(28).Text))
                                fila.Item("fT_No_laborado_V") = IIf(Trim(producto.SubItems(29).Text) = "", "0", Trim(producto.SubItems(29).Text))
                                fila.Item("fSalarioBase") = IIf(Trim(producto.SubItems(30).Text) = "", "0", Trim(producto.SubItems(30).Text))
                                fila.Item("fSalarioDiario") = IIf(Trim(producto.SubItems(31).Text) = "", "0", Trim(producto.SubItems(31).Text))
                                fila.Item("fSalarioBC") = IIf(Trim(producto.SubItems(32).Text) = "", "0", Trim(producto.SubItems(32).Text))
                                fila.Item("iDiasTrabajados") = IIf(Trim(producto.SubItems(33).Text) = "", "0", Trim(producto.SubItems(33).Text))
                                fila.Item("fSueldoBruto") = IIf(Trim(producto.SubItems(34).Text) = "", "0", Trim(producto.SubItems(34).Text))
                                fila.Item("fSeptimoDia") = IIf(Trim(producto.SubItems(35).Text) = "", "0", Trim(producto.SubItems(35).Text))
                                fila.Item("fTExtra2Gravado") = IIf(Trim(producto.SubItems(36).Text) = "", "0", Trim(producto.SubItems(36).Text))
                                fila.Item("fTExtra2Exento") = IIf(Trim(producto.SubItems(37).Text) = "", "0", Trim(producto.SubItems(37).Text))
                                fila.Item("fTExtra3Gravado") = IIf(Trim(producto.SubItems(38).Text) = "", "0", Trim(producto.SubItems(38).Text))
                                fila.Item("fTExtra3Exento") = IIf(Trim(producto.SubItems(39).Text) = "", "0", Trim(producto.SubItems(39).Text))
                                fila.Item("fVacacionesPendientes") = IIf(Trim(producto.SubItems(40).Text) = "", "0", Trim(producto.SubItems(40).Text))
                                fila.Item("bonoproductividad") = IIf(Trim(producto.SubItems(41).Text) = "", "0", Trim(producto.SubItems(41).Text))
                                fila.Item("fBonoEspecialidad") = IIf(Trim(producto.SubItems(42).Text) = "", "0", Trim(producto.SubItems(42).Text))
                                fila.Item("fCompensacion") = IIf(Trim(producto.SubItems(43).Text) = "", "0", Trim(producto.SubItems(43).Text))
                                fila.Item("fFaltaInjustificada") = IIf(Trim(producto.SubItems(44).Text) = "", "0", Trim(producto.SubItems(44).Text))
                                fila.Item("fVacacionesProporcionales") = IIf(Trim(producto.SubItems(45).Text) = "", "0", Trim(producto.SubItems(45).Text))
                                fila.Item("fAguinaldoGravado") = IIf(Trim(producto.SubItems(46).Text) = "", "0", Trim(producto.SubItems(46).Text))
                                fila.Item("fAguinaldoExento") = IIf(Trim(producto.SubItems(47).Text) = "", "0", Trim(producto.SubItems(47).Text))
                                fila.Item("fPrimaVacacionalGravado") = IIf(Trim(producto.SubItems(48).Text) = "", "0", Trim(producto.SubItems(48).Text))
                                fila.Item("fPrimaVacacionalExento") = IIf(Trim(producto.SubItems(49).Text) = "", "0", Trim(producto.SubItems(49).Text))
                                fila.Item("fTotalPercepciones") = IIf(Trim(producto.SubItems(50).Text) = "", "0", Trim(producto.SubItems(50).Text))
                                fila.Item("fTotalPercepcionesISR") = IIf(Trim(producto.SubItems(51).Text) = "", "0", Trim(producto.SubItems(51).Text))
                                fila.Item("fTotalPercepcionesNoGrava") = IIf(Trim(producto.SubItems(52).Text) = "", "0", Trim(producto.SubItems(52).Text))
                                fila.Item("fIsr") = IIf(Trim(producto.SubItems(53).Text) = "", "0", Trim(producto.SubItems(53).Text))
                                fila.Item("periodo_no_laborado") = IIf(Trim(producto.SubItems(54).Text) = "", "0", Trim(producto.SubItems(54).Text))
                                fila.Item("fImss") = IIf(Trim(producto.SubItems(55).Text) = "", "0", Trim(producto.SubItems(55).Text))
                                fila.Item("fInfonavit") = IIf(Trim(producto.SubItems(56).Text) = "", "0", Trim(producto.SubItems(56).Text))
                                fila.Item("fInfonavitBanterior") = IIf(Trim(producto.SubItems(57).Text) = "", "0", Trim(producto.SubItems(57).Text))
                                fila.Item("fAjusteInfonavit") = IIf(Trim(producto.SubItems(58).Text) = "", "0", Trim(producto.SubItems(58).Text))
                                fila.Item("fPensionAlimenticia") = IIf(Trim(producto.SubItems(59).Text) = "", "0", Trim(producto.SubItems(59).Text))
                                fila.Item("fSubsidioGenerado") = IIf(Trim(producto.SubItems(60).Text) = "", "0", Trim(producto.SubItems(60).Text))
                                fila.Item("fSubsidioAplicado") = IIf(Trim(producto.SubItems(61).Text) = "", "0", Trim(producto.SubItems(61).Text))
                                fila.Item("DED_TOTAL") = IIf(Trim(producto.SubItems(62).Text) = "", "0", Trim(producto.SubItems(62).Text))
                                fila.Item("NETO_SA") = IIf(Trim(producto.SubItems(63).Text) = "", "0", Trim(producto.SubItems(63).Text))
                                fila.Item("VALES") = IIf(Trim(producto.SubItems(64).Text) = "", "0", Trim(producto.SubItems(64).Text))
                                fila.Item("SIND_PPP") = Trim(producto.SubItems(65).Text)
                                fila.Item("EXCEDENTE") = IIf(Trim(producto.SubItems(66).Text) = "", "0", Trim(producto.SubItems(66).Text))
                                fila.Item("SERIE") = Trim(producto.SubItems(67).Text)
                                dsReporte.Tables("Tabla").Rows.Add(fila)

                            End If

                        End If
                        pgbProgreso.Value += 1

                    Next

                    'Enviar correo
                    'Enviar_Mail(GenerarCorreoFlujo("Importación Flujo-Conceptos", "Área Facturación", "Se importo un flujo con los conceptos necesarios"), "g.gomez@mbcgroup.mx", "Importación")

                    'tsbCancelar_Click(sender, e)
                    pnlProgreso.Visible = False
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                    'MessageBox.Show("Proceso terminado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else

                    MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                pnlCatalogo.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub tsbEmpleados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEmpleados.Click
        Try
            Dim Forma As New frmEmpleados
          
            Forma.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            If item.SubItems(1).Text <> "" Then


                item.Checked = chkAll.Checked
               
                'cambio

            End If

        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub
    
  

    Private Sub chkBeluga2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBeluga2.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            If item.SubItems(10).Text = "30" Then

                item.Checked = True
            Else
                item.Checked = False
            End If

        Next
    End Sub

    Private Sub chkRedFish_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkRedFish.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            If item.SubItems(10).Text = "25" Then

                item.Checked = True
            Else
                item.Checked = False
            End If

        Next
    End Sub

    Private Sub chkMaersk_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMaersk.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            If item.SubItems(10).Text = "27" Then

                item.Checked = True
            Else
                item.Checked = False
            End If

        Next
    End Sub

    Private Sub chkGoCanopus_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkGoCanopus.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            If item.SubItems(10).Text = "31" Then

                item.Checked = True
            Else
                item.Checked = False
            End If

        Next
    End Sub


End Class