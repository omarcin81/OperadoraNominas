Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Mime.MediaTypeNames
Imports Microsoft.Office.Interop

Public Class frmExcelO
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String

    Private Sub frmExcel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        'MostrarEmpresasC()
        Dim moment As Date = Date.Now()



    End Sub

  


    Private Sub tsbNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbNuevo.Click
        tsbNuevo.Enabled = False
        tsbImportar.Enabled = True
        tsbImportar_Click(sender, e)
    End Sub

    Private Sub tsbImportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbImportar.Click
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

    Private Sub tsbProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbProcesar.Click
        lsvLista.Items.Clear()
        lsvLista.Columns.Clear()
        lsvLista.Clear()

        pnlCatalogo.Enabled = False
        tsbGuardar.Enabled = False
        tsbCancelar.Enabled = False
        tsbEnviar.Enabled = False
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

                        lsvLista.Columns.Add("Empleado")
                        lsvLista.Columns.Add("ISR RETENER")
                        lsvLista.Columns.Add("Numero de Control")

                        lsvLista.Columns.Add("AP")
                        lsvLista.Columns.Add("AM")
                        lsvLista.Columns.Add("Nombre")
                        lsvLista.Columns.Add("Neto a Pagar")
                        lsvLista.Columns.Add("IMSS")
                        lsvLista.Columns.Add("Dias")
                        lsvLista.Columns.Add("Banco")
                        lsvLista.Columns.Add("Clabe")
                        lsvLista.Columns.Add("Cuenta")
                        lsvLista.Columns.Add("CURP")
                        lsvLista.Columns.Add("RFC")
                        numerocolumna = numerocolumna + 1

                    Next



                    lsvLista.Columns(1).Width = 400 'Empleado
                    lsvLista.Columns(2).Width = 100  'ISR
                    lsvLista.Columns(3).Width = 50 '#Control
                    lsvLista.Columns(4).Width = 100 'ap
                    lsvLista.Columns(5).Width = 100 'am
                    lsvLista.Columns(6).Width = 100 'nombre
                    lsvLista.Columns(7).Width = 100 'isr
                    lsvLista.Columns(8).Width = 200 'imss
                    lsvLista.Columns(9).Width = 50 'dias
                    lsvLista.Columns(10).Width = 100 'banco
                    lsvLista.Columns(11).Width = 150 'clabe
                    lsvLista.Columns(12).Width = 150 'cuenta
                    lsvLista.Columns(13).Width = 150 'curp
                    lsvLista.Columns(14).Width = 350 'rfc
                    lsvLista.Columns(15).Width = 350


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
                        tsbEnviar.Enabled = True
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


        End Try
    End Sub
    Private Sub frmImportarEmpladosAlta_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            item.Checked = chkAll.Checked
        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub


    Private Sub tsbCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCancelar.Click
        pnlCatalogo.Enabled = False
        lsvLista.Items.Clear()
        chkAll.Checked = False
        lblRuta.Text = ""
        tsbImportar.Enabled = False
        tsbEnviar.Enabled = False
        tsbCancelar.Enabled = False
        tsbNuevo.Enabled = True
    End Sub

    Private Sub tsbGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbGuardar.Click
        Dim SQL As String, nombresistema As String = ""
        Try
            If lsvLista.CheckedItems.Count > 0 Then
                Dim mensaje As String
                Dim pos As Integer = 0

                pnlProgreso.Visible = True
                pnlCatalogo.Enabled = False
                ' Application.DoEvents()


                Dim IdProducto As Long
                Dim i As Integer = 0
                Dim conta As Integer = 0


                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = lsvLista.CheckedItems.Count




                For Each producto As ListViewItem In lsvLista.CheckedItems
                    SQL = "select * from empleadosC where cNombreLargo like '%" & Trim(producto.SubItems(1).Text) & "%'"
                    Dim rwFilas As DataRow() = nConsulta(SQL)

                    If rwFilas Is Nothing = False Then
                        If rwFilas.Length > 1 Then
                            'producto.SubItems(0).Text = rwFilas(0).Item("cCodigoEmpleado")
                            producto.BackColor = Color.Yellow
                        Else
                            producto.BackColor = Color.Azure
                            producto.SubItems.Add(rwFilas(0).Item("cCodigoEmpleado")) '3
                            producto.SubItems.Add(rwFilas(0).Item("cApellidoP"))
                            producto.SubItems.Add(rwFilas(0).Item("cApellidoM"))
                            producto.SubItems.Add(rwFilas(0).Item("cNombre"))
                            producto.SubItems.Add(lsvLista.Items(pos).SubItems(2).Text)
                            producto.SubItems.Add(rwFilas(0).Item("cIMSS"))
                            producto.SubItems.Add("0")
                            Dim bank As DataRow() = nConsulta("select * from bancos where iIdBanco =" & rwFilas(0).Item("fkiIdBanco"))
                            If bank Is Nothing = False Then
                                producto.SubItems.Add(IIf(bank(0).Item("cBANCO") = "BBVA BANCOMER", "BANCOMER", IIf(bank(0).Item("cBANCO") = "AZTECA", "BANCO AZTECA", bank(0).Item("cBANCO"))))
                            End If
                            producto.SubItems.Add(rwFilas(0).Item("Clabe"))

                            producto.SubItems.Add(rwFilas(0).Item("NumCuenta"))
                            producto.SubItems.Add(rwFilas(0).Item("cCURP"))
                            producto.SubItems.Add(rwFilas(0).Item("cRFC"))


                        End If

                    Else
                        producto.BackColor = Color.Red
                        lsvLista.Items(pos).Checked = False
                    End If
                    pgbProgreso.Value += 1
                    pos = pos + 1
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

    Private Sub tsbEnviar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEnviar.Click
        'ExisteEnLista("asim")
        Try

            Dim filaExcel As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo As String
            Dim x As Integer = 0

            If lsvLista.CheckedItems.Count > 0 Then

                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\asimilados2.xlsm"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
                Dim libro As New ClosedXML.Excel.XLWorkbook
                book.Worksheet(1).CopyTo(libro, "ASIMILADOS")
                Dim hoja As IXLWorksheet = libro.Worksheets(0)

                filaExcel = 2

                Dim app, apm, nom, nombrecompleto, clabe, banco, cuenta As String

                'limpiar
                recorrerFilasColumnas(hoja, filaExcel, lsvLista.CheckedItems.Count + 50, 50, "clear")

                hoja.Range(2, 1, lsvLista.Items.Count + 1, 15).Style.Font.FontSize = 10
                hoja.Range(2, 1, lsvLista.Items.Count + 1, 15).Style.Font.SetBold(True)
                hoja.Range(2, 1, lsvLista.Items.Count + 1, 15).Style.Alignment.WrapText = True
                hoja.Range(2, 1, lsvLista.Items.Count + 1, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                hoja.Range(2, 1, lsvLista.Items.Count + 1, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                'hoja.Range(2, 6, lsvLista.Items.Count + 1, 5).Style.NumberFormat.Format = "@"
                hoja.Range(2, 7, lsvLista.Items.Count + 1, 14).Style.NumberFormat.Format = "@"
                hoja.Range(2, 1, lsvLista.Items.Count + 1, 1).Style.NumberFormat.Format = "@"

                'For x As Integer = 0 To lsvLista.CheckedItems.Count - 1
                For Each lsvLista2 As ListViewItem In lsvLista.CheckedItems

                    If (lsvLista2.SubItems(3).Text Is Nothing = False) Then



                        Dim empleado As DataRow() = nConsulta("Select * from empleadosC where cCodigoEmpleado=" & lsvLista2.SubItems(3).Text)
                        If empleado Is Nothing = False Then
                            nombrecompleto = empleado(0).Item("cNombre") & " " & empleado(0).Item("cApellidoP") & " " & empleado(0).Item("cApellidoM")
                            app = empleado(0).Item("cApellidoP")
                            apm = empleado(0).Item("cApellidoM")
                            nom = empleado(0).Item("cNombre")
                            clabe = empleado(0).Item("Clabe")
                            cuenta = empleado(0).Item("NumCuenta")
                            Dim bank As DataRow() = nConsulta("select * from bancos where iIdBanco =" & empleado(0).Item("fkiIdBanco"))
                            If bank Is Nothing = False Then
                                banco = IIf(bank(0).Item("cBANCO") = "BBVA BANCOMER", "BANCOMER", IIf(bank(0).Item("cBANCO") = "AZTECA", "BANCO AZTECA", bank(0).Item("cBANCO")))
                            End If
                        End If


                        hoja.Cell(filaExcel + x, 1).Value = lsvLista2.SubItems(3).Text 'Codigo
                        hoja.Cell(filaExcel + x, 2).Value = lsvLista2.SubItems(4).Text 'Paterno
                        hoja.Cell(filaExcel + x, 3).Value = lsvLista2.SubItems(5).Text 'Materno                        
                        hoja.Cell(filaExcel + x, 4).Value = lsvLista2.SubItems(6).Text 'Nombre
                        hoja.Cell(filaExcel + x, 5).FormulaA1 = "=F" & filaExcel + x & "+1"
                        hoja.Cell(filaExcel + x, 6).Value = lsvLista2.SubItems(2).Text 'ISR A RETENER 
                        hoja.Cell(filaExcel + x, 7).FormulaA1 = "=E" & filaExcel + x & "-F" & filaExcel + x
                        hoja.Cell(filaExcel + x, 8).Value = lsvLista2.SubItems(8).Text 'imss
                        hoja.Cell(filaExcel + x, 9).Value = lsvLista2.SubItems(9).Text ' Dias Trabjados
                        hoja.Cell(filaExcel + x, 10).Value = banco ' Banco
                        hoja.Cell(filaExcel + x, 11).Value = clabe ' Clabe
                        hoja.Cell(filaExcel + x, 12).Value = "" 'cuenta ' Cuenta
                        hoja.Cell(filaExcel + x, 13).Value = lsvLista2.SubItems(13).Text
                        hoja.Cell(filaExcel + x, 14).Value = lsvLista2.SubItems(14).Text
                    End If
                    x = x + 1
                Next

                dialogo.FileName = "ASIMILADOS ISR"
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                '' dialogo.ShowDialog()

                If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' OK button pressed
                    libro.SaveAs(dialogo.FileName)
                    libro = Nothing
                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
                ' libro.SaveAs(ruta)
                'libro = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

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
End Class