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

                    lsvLista.Columns(2).Width = 150
                    lsvLista.Columns(3).Width = 50
                    lsvLista.Columns(4).Width = 50
                    lsvLista.Columns(5).Width = 50
                    lsvLista.Columns(6).Width = 50
                    lsvLista.Columns(7).Width = 100
                    lsvLista.Columns(7).TextAlign = 1
                    lsvLista.Columns(8).Width = 150
                    lsvLista.Columns(8).TextAlign = 1
                    lsvLista.Columns(9).Width = 150
                    lsvLista.Columns(9).TextAlign = 1
                    lsvLista.Columns(10).Width = 100
                    lsvLista.Columns(11).Width = 400





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
                        If Len(producto.SubItems(1).Text) < 4 Then
                            SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(1).Text)
                        Else
                            SQL = "select * from empleadosC where cCodigoEmpleado = " & Trim(producto.SubItems(1).Text).Substring(2, 4)
                        End If

                        Dim rwFilas As DataRow() = nConsulta(SQL)

                        If rwFilas Is Nothing = False Then
                            If rwFilas.Length = 1 Then
                                producto.BackColor = Color.Green
                                Dim fila As DataRow = dsReporte.Tables("Tabla").NewRow


                                fila.Item("Id_empleado") = rwFilas(0)("iIdEmpleadoC")
                                If Len(producto.SubItems(1).Text) < 4 Then
                                    fila.Item("CodigoEmpleado") = Trim(producto.SubItems(1).Text)
                                Else
                                    fila.Item("CodigoEmpleado") = Trim(producto.SubItems(1).Text).Substring(2, 4)
                                End If
                                fila.Item("dias") = Trim(producto.SubItems(9).Text)
                                fila.Item("Salario") = Trim(producto.SubItems(17).Text)
                                fila.Item("Bono") = Trim(producto.SubItems(17).Text)
                                fila.Item("Refrendo") = Trim(producto.SubItems(17).Text)
                                fila.Item("SalarioTMM") = Trim(producto.SubItems(17).Text)
                                fila.Item("Anticipo") = IIf(Trim(producto.SubItems(20).Text) = "", "0", Trim(producto.SubItems(20).Text))
                                fila.Item("AnticipoSA") = IIf(Trim(producto.SubItems(21).Text) = "", "0", Trim(producto.SubItems(21).Text))

                                fila.Item("InfonavitSA") = IIf(Trim(producto.SubItems(22).Text) = "", "0", Trim(producto.SubItems(22).Text))
                                fila.Item("InfonavitBIASA") = IIf(Trim(producto.SubItems(23).Text) = "", "0", Trim(producto.SubItems(23).Text))
                                fila.Item("InfonavitASI") = IIf(Trim(producto.SubItems(24).Text) = "", "0", Trim(producto.SubItems(24).Text))
                                fila.Item("InfonavitBIAASI") = IIf(Trim(producto.SubItems(25).Text) = "", "0", Trim(producto.SubItems(25).Text))

                                fila.Item("CodigoPuesto") = Trim(producto.SubItems(4).Text)
                                fila.Item("CodigoBuque") = Trim(producto.SubItems(10).Text)
                                fila.Item("Fechainicio") = (Date.Parse(Trim(producto.SubItems(7).Text))).ToShortDateString
                                fila.Item("Fechafin") = (Date.Parse(Trim(producto.SubItems(8).Text))).ToShortDateString
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