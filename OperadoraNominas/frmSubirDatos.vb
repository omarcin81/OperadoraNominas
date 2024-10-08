﻿Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Public Class frmSubirDatos
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Public dsReporte As New DataSet

    Private Sub tsbNuevo_Click(sender As System.Object, e As System.EventArgs) Handles tsbNuevo.Click
        Try
            tsbNuevo.Enabled = False
            tsbImportar.Enabled = True
            tsbImportar_Click(sender, e)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
      
    End Sub

    Private Sub tsbImportar_Click(sender As System.Object, e As System.EventArgs) Handles tsbImportar.Click
        Try

       
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
        Catch ex As Exception
            Me.Enabled = False
            MsgBox(ex.Message)
        End Try
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

                    'Ancho de columnas de acuerdo a N columnas
                    For cole As Integer = 0 To colFin
                        If cole = 5 Then
                            lsvLista.Columns(cole).Width = 250
                        Else
                            lsvLista.Columns(cole).Width = 100
                        End If

                    Next

                    



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

                                    item.SubItems(item.SubItems.Count - 1).Text = Valor

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
            MsgBox(ex.Message)
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

                    SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(3).Text)



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
                    dsReporte.Tables("Tabla").Columns.Add("MES")
                    dsReporte.Tables("Tabla").Columns.Add("PERIODO")
                    dsReporte.Tables("Tabla").Columns.Add("CLAVE")
                    dsReporte.Tables("Tabla").Columns.Add("CENTRO_COSTO")
                    dsReporte.Tables("Tabla").Columns.Add("NOMBRE")
                    dsReporte.Tables("Tabla").Columns.Add("CCP")
                    dsReporte.Tables("Tabla").Columns.Add("CATEGORIA")
                    dsReporte.Tables("Tabla").Columns.Add("PUESTO")
                    dsReporte.Tables("Tabla").Columns.Add("DEPARTAMENTO")
                    dsReporte.Tables("Tabla").Columns.Add("RFC")
                    dsReporte.Tables("Tabla").Columns.Add("IMSS")
                    dsReporte.Tables("Tabla").Columns.Add("CURP")
                    dsReporte.Tables("Tabla").Columns.Add("ALTA")

                    dsReporte.Tables("Tabla").Columns.Add("BCO_DEPOSITO")
                    dsReporte.Tables("Tabla").Columns.Add("CTA_DEPOSITO")

                    dsReporte.Tables("Tabla").Columns.Add("fTExtra2V")
                    dsReporte.Tables("Tabla").Columns.Add("fTExtra3V")
                    dsReporte.Tables("Tabla").Columns.Add("fDescansoLV")
                    dsReporte.Tables("Tabla").Columns.Add("fDiaFestivoLV")
                    dsReporte.Tables("Tabla").Columns.Add("fHoras_extras_dobles_V")
                    dsReporte.Tables("Tabla").Columns.Add("fHoras_extras_triples_V")
                    dsReporte.Tables("Tabla").Columns.Add("fDescanso_Laborado_V")
                    dsReporte.Tables("Tabla").Columns.Add("fDia_Festivo_laborado_V")
                    dsReporte.Tables("Tabla").Columns.Add("fPrima_Dominical_V")
                    dsReporte.Tables("Tabla").Columns.Add("fFalta_Injustificada_V")
                    dsReporte.Tables("Tabla").Columns.Add("fPermiso_Sin_GS_V")
                    dsReporte.Tables("Tabla").Columns.Add("fT_No_laborado_V")

                    dsReporte.Tables("Tabla").Columns.Add("fSalarioBase")
                    dsReporte.Tables("Tabla").Columns.Add("fSalarioDiario")
                    dsReporte.Tables("Tabla").Columns.Add("fSalarioBC")
                    dsReporte.Tables("Tabla").Columns.Add("iDiasTrabajados")

                    dsReporte.Tables("Tabla").Columns.Add("fSueldoBruto")
                    dsReporte.Tables("Tabla").Columns.Add("fSeptimoDia")
                    dsReporte.Tables("Tabla").Columns.Add("fTExtra2Gravado")
                    dsReporte.Tables("Tabla").Columns.Add("fTExtra2Exento")
                    dsReporte.Tables("Tabla").Columns.Add("fTExtra3Gravado")
                    dsReporte.Tables("Tabla").Columns.Add("fTExtra3Exento")
                    dsReporte.Tables("Tabla").Columns.Add("PrimaDominicalG")
                    dsReporte.Tables("Tabla").Columns.Add("PrimaDominicalE")
                    dsReporte.Tables("Tabla").Columns.Add("DescansoLaboradoG")
                    dsReporte.Tables("Tabla").Columns.Add("fVacacionesPendientes")
                    dsReporte.Tables("Tabla").Columns.Add("fBonoAsistencia")
                    dsReporte.Tables("Tabla").Columns.Add("fBonoCalidad")
                    dsReporte.Tables("Tabla").Columns.Add("fBonoProductividad")
                    dsReporte.Tables("Tabla").Columns.Add("fBonoPolivalencia")
                    dsReporte.Tables("Tabla").Columns.Add("fBonoEspecialidad")
                    dsReporte.Tables("Tabla").Columns.Add("fCompensacionG")
                    dsReporte.Tables("Tabla").Columns.Add("fVacacionesProporcionales")
                    dsReporte.Tables("Tabla").Columns.Add("fAguinaldoGravado")
                    dsReporte.Tables("Tabla").Columns.Add("fAguinaldoExento")
                    dsReporte.Tables("Tabla").Columns.Add("fPrimaVacacionalGravado")
                    dsReporte.Tables("Tabla").Columns.Add("fPrimaVacacionalExento")
                    dsReporte.Tables("Tabla").Columns.Add("primaAntigüedad")
                    dsReporte.Tables("Tabla").Columns.Add("indemnizaciónGravado")
                    dsReporte.Tables("Tabla").Columns.Add("indemnizaciónExcenta")
                    dsReporte.Tables("Tabla").Columns.Add("fTotalPercepciones")
                    dsReporte.Tables("Tabla").Columns.Add("fTotalPercepcionesISR")
                    dsReporte.Tables("Tabla").Columns.Add("fTotalPercepcionesNoGrava")

                    dsReporte.Tables("Tabla").Columns.Add("fIsr")
                    dsReporte.Tables("Tabla").Columns.Add("fT_No_laborado")
                    dsReporte.Tables("Tabla").Columns.Add("fImss")
                    dsReporte.Tables("Tabla").Columns.Add("fInfonavit")
                    dsReporte.Tables("Tabla").Columns.Add("fInfonavitBanterior")
                    dsReporte.Tables("Tabla").Columns.Add("fAjusteInfonavit")
                    dsReporte.Tables("Tabla").Columns.Add("fPensionAlimenticia")
                    dsReporte.Tables("Tabla").Columns.Add("fFonacot")
                    dsReporte.Tables("Tabla").Columns.Add("fSubsidioGenerado")
                    dsReporte.Tables("Tabla").Columns.Add("fSubsidioAplicado")
                    dsReporte.Tables("Tabla").Columns.Add("fDiasNoLaborados")
                    dsReporte.Tables("Tabla").Columns.Add("DED_TOTAL")
                    dsReporte.Tables("Tabla").Columns.Add("NETO_SA")
                    dsReporte.Tables("Tabla").Columns.Add("VALES")
                    dsReporte.Tables("Tabla").Columns.Add("SIND_PPP")
                    dsReporte.Tables("Tabla").Columns.Add("EXCEDENTE")
                    dsReporte.Tables("Tabla").Columns.Add("SERIE")

                   
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
                                fila.Item("fBonoProductividad") = IIf(Trim(producto.SubItems(41).Text) = "", "0", Trim(producto.SubItems(41).Text))
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
                                fila.Item("fT_No_laborado") = IIf(Trim(producto.SubItems(54).Text) = "", "0", Trim(producto.SubItems(54).Text))

                                fila.Item("fImss") = IIf(Trim(producto.SubItems(55).Text) = "", "0", Trim(producto.SubItems(55).Text))
                                fila.Item("fInfonavit") = IIf(Trim(producto.SubItems(56).Text) = "", "0", Trim(producto.SubItems(56).Text))
                                fila.Item("fInfonavitBanterior") = IIf(Trim(producto.SubItems(57).Text) = "", "0", Trim(producto.SubItems(57).Text))
                                fila.Item("fAjusteInfonavit") = IIf(Trim(producto.SubItems(58).Text) = "", "0", Trim(producto.SubItems(58).Text))
                                fila.Item("fPensionAlimenticia") = IIf(Trim(producto.SubItems(59).Text) = "", "0", Trim(producto.SubItems(59).Text))
                                fila.Item("fFonacot") = IIf(Trim(producto.SubItems(60).Text) = "", "0", Trim(producto.SubItems(59).Text))
                                fila.Item("fSubsidioGenerado") = IIf(Trim(producto.SubItems(61).Text) = "", "0", Trim(producto.SubItems(61).Text))
                                fila.Item("fSubsidioAplicado") = IIf(Trim(producto.SubItems(62).Text) = "", "0", Trim(producto.SubItems(62).Text))

                                fila.Item("DED_TOTAL") = IIf(Trim(producto.SubItems(63).Text) = "", "0", Trim(producto.SubItems(63).Text))
                                fila.Item("NETO_SA") = IIf(Trim(producto.SubItems(64).Text) = "", "0", Trim(producto.SubItems(64).Text))
                                fila.Item("VALES") = IIf(Trim(producto.SubItems(65).Text) = "", "0", Trim(producto.SubItems(65).Text))
                                fila.Item("SIND_PPP") = Trim(producto.SubItems(66).Text)
                                fila.Item("EXCEDENTE") = IIf(Trim(producto.SubItems(67).Text) = "", "0", Trim(producto.SubItems(67).Text))
                                fila.Item("SERIE") = Trim(producto.SubItems(68).Text)


                                dsReporte.Tables("Tabla").Rows.Add(fila)

                            End If
                        Else
                            MsgBox("No se encuentra registrado " & Trim(producto.SubItems(5).Text) & ", Porfavor registre al trabajador para agregar su Nomina/Finiquito", , "No se proceso")
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