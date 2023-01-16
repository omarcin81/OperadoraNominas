Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Mime.MediaTypeNames
Imports Microsoft.Office.Interop
Imports System.Windows.Forms
Imports System.Globalization

Public Class frmExcel
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String

    Private Sub frmExcel_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        'MostrarEmpresasC()
        Dim moment As Date = Date.Now()


        cboMes.SelectedIndex = moment.Month - 1
        cboTipoR.SelectedIndex = 0


    End Sub

    'Private Sub MostrarEmpresasC()
    '    SQL = "select (nombre + ' ' + ruta) AS nombre, iIdEmpresaC from empresaC ORDER BY nombre"
    '    nCargaCBO(cbEmpresasC, SQL, "nombre", "iIdEmpresaC")
    '    cbEmpresasC.SelectedIndex = 0

    'End Sub


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
            .Filter = "Hoja de cálculo de excel (xlsx)|*.xlsm;"
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

    Private Sub tsbProcesar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbProcesar.Click
        lsvLista.Items.Clear()
        lsvLista.Columns.Clear()
        lsvLista.Clear()

        pnlCatalogo.Enabled = False
        tsbGuardar.Enabled = False
        tsbGuardar2.Enabled = False
        tsbMaecco.Enabled = False
        tsbProcesos.Enabled = False

        tsbCancelar.Enabled = False
        lsvLista.Visible = False
        tsbImportar.Enabled = False
        Me.cmdCerrar.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        System.Windows.Forms.Application.DoEvents()

        Try
            If File.Exists(lblRuta.Text) Then
                Dim Archivo As String = lblRuta.Text
                rutita = lblRuta.Text
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

                        lsvLista.Columns.Add(sheet.Cell(5, c).Value.ToString)
                        'lsvLista.Columns.Add(numerocolumna)
                        'numerocolumna = numerocolumna + 1

                    Next


                    lsvLista.Columns(1).Width = 100

                    lsvLista.Columns(2).Width = 250
                    lsvLista.Columns(3).Width = 100
                    lsvLista.Columns(4).Width = 200
                    lsvLista.Columns(5).Width = 100
                    lsvLista.Columns(6).Width = 200
                    lsvLista.Columns(7).Width = 100
                    ''lsvLista.Columns(7).TextAlign = 1
                    lsvLista.Columns(8).Width = 70
                    '' lsvLista.Columns(8).TextAlign = 1
                    lsvLista.Columns(9).Width = 150
                    ''lsvLista.Columns(9).TextAlign = 1
                    lsvLista.Columns(10).Width = 150
                    lsvLista.Columns(11).Width = 90
                    lsvLista.Columns(12).Width = 91
                    lsvLista.Columns(13).Width = 92
                    lsvLista.Columns(14).Width = 93
                    lsvLista.Columns(15).Width = 94
                    lsvLista.Columns(16).Width = 95
                    lsvLista.Columns(17).Width = 96
                    lsvLista.Columns(18).Width = 97
                    lsvLista.Columns(19).Width = 98
                    lsvLista.Columns(20).Width = 99
                    lsvLista.Columns(21).Width = 100
                    lsvLista.Columns(22).Width = 101
                    lsvLista.Columns(23).Width = 102
                    lsvLista.Columns(24).Width = 103
                    lsvLista.Columns(25).Width = 104
                    lsvLista.Columns(26).Width = 105
                    lsvLista.Columns(27).Width = 106
                    lsvLista.Columns(28).Width = 107
                    lsvLista.Columns(29).Width = 108
                    lsvLista.Columns(30).Width = 109
                    lsvLista.Columns(31).Width = 110
                    lsvLista.Columns(32).Width = 111
                    lsvLista.Columns(33).Width = 112
                    lsvLista.Columns(34).Width = 113
                    lsvLista.Columns(35).Width = 114
                    lsvLista.Columns(36).Width = 115
                    lsvLista.Columns(37).Width = 116
                    lsvLista.Columns(38).Width = 117
                    lsvLista.Columns(39).Width = 118
                    lsvLista.Columns(40).Width = 119
                    lsvLista.Columns(41).Width = 121
                    lsvLista.Columns(42).Width = 122
                    lsvLista.Columns(43).Width = 123
                    lsvLista.Columns(44).Width = 124
                    lsvLista.Columns(45).Width = 125
                    lsvLista.Columns(46).Width = 126
                    lsvLista.Columns(47).Width = 127
                    lsvLista.Columns(48).Width = 128
                    '' lsvLista.Columns(49).Width = 129

                    Dim Filas As Long = sheet.RowsUsed().Count() + 1
                    For f As Integer = 6 To Filas
                        Dim item As ListViewItem = lsvLista.Items.Add((f - 1).ToString())
                        For c As Integer = colIni To colFin
                            Try

                                Dim Valor As String = ""
                                If (sheet.Cell(f, c).ValueCached Is Nothing) Then
                                    Valor = sheet.Cell(f, c).Value.ToString()
                                Else
                                    Valor = sheet.Cell(f, c).ValueCached.ToString()
                                    ''Valor = sheet.Cell(f, c).FormulaA1
                                End If
                                Valor = Valor.Trim()
                                item.SubItems.Add(Valor)

                                '' Existe(Valor)

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

                                'If f = 2 And c = 4 Then
                                Dim fecha As String = sheet.Cell(2, 4).Value.ToString 'Format(DateTime.Parse(sheet.Cell(2, 4).Value), "dd/MM/yyyy hh:mm:ss")

                                fechadepago = sheet.Cell(2, 2).Value.ToString()
                                ini = sheet.Cell(2, 3).Value.ToString()
                                fin = sheet.Cell(2, 4).Value.ToString()

                                Dim fec As DateTime = Convert.ToDateTime(fecha)
                                cboMes.SelectedIndex = fec.Month - 1
                                'End If




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
                        MessageBox.Show("El catálogo no pudo ser importado o no contiene registros." & vbCrLf & "¿Por favor verifique?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Se han encontrado " & FormatNumber(lsvLista.Items.Count, 0) & " registros en el archivo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        tsbGuardar.Enabled = True
                        tsbGuardar2.Enabled = True
                        tsbMaecco.Enabled = True
                        tsbProcesos.Enabled = True

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

        End Try
    End Sub

    Private Function Existe(ByVal valor As String) As Boolean

        For Each item As ListViewItem In lsvLista.Items

            If item.Text = valor _
                OrElse item.SubItems(1).Text = valor _
                OrElse item.SubItems(1).Text = valor Then
                item.SubItems(1).BackColor = Color.AliceBlue
                Return True

            End If

        Next

        Return False

    End Function

    Private Function getColumnName(ByVal index As Single) As String
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

    Private Sub tsbGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbGuardar.Click
        Try


            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()

            Dim pilotin As Boolean = False


            If lsvLista.CheckedItems.Count > 0 Then
                'Abrimos el machote
                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\nominas1.xlsx"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


                Dim libro As New ClosedXML.Excel.XLWorkbook


                book.Worksheet(1).CopyTo(libro, "Generales")
                book.Worksheet(2).CopyTo(libro, "Percepciones")
                book.Worksheet(3).CopyTo(libro, "Deducciones")
                book.Worksheet(4).CopyTo(libro, "Otros Pagos")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)




                '' filaExcel = 6
                For Each dato As ListViewItem In lsvLista.CheckedItems
                    hoja.Range(2, 1, filaExcel, 1).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 5, filaExcel, 5).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 6, filaExcel, 6).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 26, filaExcel, 26).Style.NumberFormat.Format = "@"

                    If dato.SubItems(9).Text = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" And cboTipoR.SelectedItem.ToString() = "ND" Then
                        pilotin = True
                    Else
                        pilotin = False
                    End If

                    If pilotin = False Then


                        ''Generales
                        hoja.Cell(filaExcel, 1).Value = dato.SubItems(1).Text 'N° Empleado
                        hoja.Cell(filaExcel, 2).Value = dato.SubItems(4).Text 'RFC
                        hoja.Cell(filaExcel, 3).Value = dato.SubItems(2).Text 'NOMBRE
                        hoja.Cell(filaExcel, 4).Value = dato.SubItems(5).Text 'CURP
                        hoja.Cell(filaExcel, 5).Value = dato.SubItems(6).Text 'SSA
                        hoja.Cell(filaExcel, 6).Value = (dato.SubItems(43).Text.ToString.Replace("'", "")) 'Cuenta
                        hoja.Cell(filaExcel, 7).Value = dato.SubItems(14).Text 'SBC
                        hoja.Cell(filaExcel, 8).Value = dato.SubItems(13).Text 'SDI
                        hoja.Cell(filaExcel, 9).Value = "A1131077105" 'REG. PATRONAL
                        hoja.Cell(filaExcel, 10).Value = "CAM" 'ENT. FEDERATIVA
                        hoja.Cell(filaExcel, 11).Value = dato.SubItems(15).Text 'DIAS PAGADOS
                        hoja.Cell(filaExcel, 12).Value = dato.SubItems(44).Text ' FECHA INICIO RELABORAL
                        hoja.Cell(filaExcel, 13).Value = "3" 'TIPO DE CONTRATO
                        hoja.Cell(filaExcel, 14).Value = ""
                        hoja.Cell(filaExcel, 15).Value = ""   'SINDICALIZADO
                        hoja.Cell(filaExcel, 16).Value = "1"  ''TIPO JORNADA
                        hoja.Cell(filaExcel, 17).Value = ""
                        hoja.Cell(filaExcel, 18).Value = "2" 'TIPO REGIMEN 
                        hoja.Cell(filaExcel, 19).Value = ""
                        hoja.Cell(filaExcel, 20).Value = "" 'DEPARTAMENTO
                        hoja.Cell(filaExcel, 21).Value = dato.SubItems(9).Text  'PUESTO
                        hoja.Cell(filaExcel, 22).Value = "4"  'RIESGO PUESTO
                        hoja.Cell(filaExcel, 23).Value = ""
                        hoja.Cell(filaExcel, 24).Value = "5"  'PERIODICIDAD PAGO
                        hoja.Cell(filaExcel, 25).Value = ""
                        hoja.Cell(filaExcel, 26).Value = dato.SubItems(42).Text  ''Banco
                        hoja.Cell(filaExcel, 27).Value = ""
                        hoja.Cell(filaExcel, 28).Value = "" 'SUBCONTRATACION
                        hoja.Cell(filaExcel, 29).Value = cboTipoR.SelectedItem.ToString() 'TIPO DE RECIBO
                        hoja.Cell(filaExcel, 30).Value = cboMes.SelectedIndex + 1 ' MES DE PAGO
                        hoja.Cell(filaExcel, 31).Value = dato.SubItems(10).Text ' BUQUE
                        filaExcel = filaExcel + 1
                    End If

                Next

                filaExcel = 4
                For Each dato As ListViewItem In lsvLista.CheckedItems

                    If dato.SubItems(9).Text = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" And cboTipoR.SelectedItem.ToString() = "ND" Then
                        pilotin = True
                    Else
                        pilotin = False
                    End If

                    If pilotin = False Then

                        'Percepciones
                        hoja2.Cell(filaExcel, 1).Value = dato.SubItems(4).Text 'RFC
                        hoja2.Cell(filaExcel, 2).Value = dato.SubItems(2).Text 'NOMBRE
                        hoja2.Cell(filaExcel, 3).Value = dato.SubItems(23).Text ' VAC. PORP GRAV
                        hoja2.Cell(filaExcel, 4).Value = ""
                        hoja2.Cell(filaExcel, 5).Value = dato.SubItems(22).Text 'DESC. SEM. OBLIGATORIO
                        hoja2.Cell(filaExcel, 6).Value = ""
                        hoja2.Cell(filaExcel, 7).Value = dato.SubItems(21).Text 'TIEMPO EXTRA OCASIONA
                        hoja2.Cell(filaExcel, 8).Value = ""
                        hoja2.Cell(filaExcel, 9).Value = dato.SubItems(19).Text ' TIEMPO EXTRA FIJO GRAVADO
                        hoja2.Cell(filaExcel, 10).Value = dato.SubItems(20).Text 'EXENTO
                        hoja2.Cell(filaExcel, 11).Value = dato.SubItems(18).Text ' SUELDO BASE
                        hoja2.Cell(filaExcel, 12).Value = ""
                        hoja2.Cell(filaExcel, 13).Value = dato.SubItems(24).Text ' AGUINALDO GRVADO
                        hoja2.Cell(filaExcel, 14).Value = dato.SubItems(25).Text ' EXENTO
                        hoja2.Cell(filaExcel, 15).Value = dato.SubItems(27).Text ' PRIMA VACIONAL
                        hoja2.Cell(filaExcel, 16).Value = dato.SubItems(28).Text 'EXENTO
                        hoja2.Cell(filaExcel, 17).Value = ""
                        hoja2.Cell(filaExcel, 18).Value = ""
                        hoja2.Cell(filaExcel, 19).Value = ""
                        hoja2.Cell(filaExcel, 20).Value = ""
                        hoja2.Cell(filaExcel, 21).Value = ""
                        hoja2.Cell(filaExcel, 22).Value = ""
                        hoja2.Cell(filaExcel, 23).Value = ""

                        ''Deducciones
                        hoja3.Cell(filaExcel, 1).Value = dato.SubItems(4).Text ' RFC
                        hoja3.Cell(filaExcel, 2).Value = dato.SubItems(2).Text ' NOMBRE
                        hoja3.Cell(filaExcel, 3).Value = dato.SubItems(34).Text ' IMSS
                        hoja3.Cell(filaExcel, 4).Value = dato.SubItems(33).Text ' ISR
                        hoja3.Cell(filaExcel, 5).Value = ""
                        hoja3.Cell(filaExcel, 6).Value = ""
                        hoja3.Cell(filaExcel, 7).Value = dato.SubItems(32).Text 'INCAPACIDAD IMPORTE
                        hoja3.Cell(filaExcel, 8).Value = dato.SubItems(36).Text 'PENSION ALIMENTICIA
                        hoja3.Cell(filaExcel, 9).Value = dato.SubItems(35).Text ' INFONAVIT

                        ''Otros Pagos
                        hoja4.Columns("A").Width = 20
                        hoja4.Columns("B").Width = 20
                        hoja4.Cell(filaExcel, 1).Value = dato.SubItems(4).Text ' RFC
                        hoja4.Cell(filaExcel, 2).Value = dato.SubItems(2).Text ' NOMBRE
                        hoja4.Cell(filaExcel, 3).Value = dato.SubItems(37).Text ' IMPORTE SUBSIDIO
                        hoja4.Cell(filaExcel, 4).Value = dato.SubItems(49).Text 'SUBSIDIO CAUSADO

                        filaExcel = filaExcel + 1
                    End If

                Next
                'Dim tmp() = rutita.Split("\")
                'Dim ind As Integer = 0
                'For index As Integer = 1 To 31
                '    ''Debug.Write(index.ToString & " ")
                '    If (tmp(tmp.Length - 1).IndexOf(index.ToString) > -1) Then

                '        ind = tmp(tmp.Length - 1).IndexOf(index.ToString)

                '        Exit For
                '    End If

                'Next
                'Dim fectitulo = Mid(tmp(tmp.Length - 1), ind, 12)

                Dim moment As Date = Date.Now()
                Dim month As Integer = moment.Month
                Dim year As Integer = moment.Year

                dialogo.DefaultExt = "*.xlsx"
                Dim fechita() As String = Date.Parse(fechadepago).ToLongDateString().Split(",")
                dialogo.FileName = "Isla-Arca " & fechita(1).ToUpper() & " " & cboTipoR.SelectedItem.ToString()
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                ''  dialogo.ShowDialog()

                If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' OK button pressed
                    libro.SaveAs(dialogo.FileName)
                    libro = Nothing
                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try




    End Sub


    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAll.CheckedChanged
        For Each item As ListViewItem In lsvLista.Items
            item.Checked = chkAll.Checked
        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub

    Private Sub cmdCerrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCerrar.Click
        Me.Close()
    End Sub

    Private Sub tsbCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbCancelar.Click
        pnlCatalogo.Enabled = False
        lsvLista.Items.Clear()
        chkAll.Checked = False
        lblRuta.Text = ""
        tsbImportar.Enabled = False
        tsbProcesos.Enabled = False
        tsbGuardar.Enabled = False
        tsbGuardar2.Enabled = False
        tsbMaecco.Enabled = False

        tsbCancelar.Enabled = False
        tsbNuevo.Enabled = True
    End Sub



    Private Sub abiriEmpresasC()
        'Declaramos la variable nombre
        Dim nombre As String
        'Entrada de datos mediante un inputbox
        nombre = InputBox("Ingrese Nombre de empresa ",
                         "Registro de Datos Personales",
                         "Nombre", 100, 0)
        MessageBox.Show("Bienvenido Usuario: " + nombre,
                        "Registro de Datos Personales",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)
    End Sub



    Private Sub cmdVerificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVerificar.Click

        recorrerLista()
        ''recorrerLista()

    End Sub


    Public Sub recorrerLista()

        Dim filas, filas2 As Integer
        Dim contador As Integer = 0

        For filas = 0 To lsvLista.Items.Count - 1
            For filas2 = 1 + filas To lsvLista.Items.Count - 1
                ''MsgBox(lsvLista.Items.Item(filas).SubItems(1).Text)

                If lsvLista.Items(filas).SubItems(1).Text = lsvLista.Items(filas2).SubItems(1).Text Then
                    lsvLista.Items(filas2).BackColor = Color.GreenYellow

                    contador = contador + 1
                End If

                If filas2 = lsvLista.Items.Count Then
                    Exit For
                End If

            Next
            If filas = lsvLista.Items.Count Then

                Exit Sub

            End If
        Next
        MsgBox(contador.ToString & " Datos repetidos")


    End Sub
    Public Sub recorrerLista2()
        Try


            Dim lsvDate As New ListView
            Dim lsvDate2 As ListViewItem '' = lsvDate.SelectedItems(0)

            If lsvLista.CheckedItems.Count > 0 Then
                For Each dato As ListViewItem In lsvLista.CheckedItems
                    lsvDate.Items.Add(dato.SubItems(1).Text)
                    '' MsgBox(dato.SubItems(1).Text)
                Next
            End If


            Dim filas, filas2 As Integer
            Dim contador As Integer = 0

            For filas = 1 To lsvDate.Items.Count - 1
                For filas2 = 1 + filas To lsvDate.Items.Count - 1
                    ''MsgBox(lsvDate.Items(filas2).Text)

                    If lsvDate.Items(filas).Text = lsvDate.Items(filas2).Text Then
                        lsvLista.Items(filas2).BackColor = Color.GreenYellow
                        lsvLista.Items(filas).BackColor = Color.Yellow
                        lsvDate2 = lsvDate.Items.Add(filas2)
                        '' lsvLista.Items.Add(filas2)
                        contador = contador + 1
                    End If

                    If filas2 = lsvDate.Items.Count Then
                        Exit For
                    End If

                Next
                If filas = lsvDate.Items.Count Then

                    Exit Sub

                End If
            Next

            MsgBox(contador.ToString & " Datos repetidos")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub







    Private Sub tsbGuardar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbGuardar2.Click

        Try
            Dim tipo As String
            '' Format(Date.Now, "MMMM yyyy") & " " & cboTipoR.SelectedItem.ToString()
            Select Case cboTipoR.SelectedItem.ToString()
                Case "NN"
                    tipo = "ABORDO"
                Case "ND"
                    tipo = "DESCANSO"
                Case Else
                    tipo = "NA"
            End Select


            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()

            If lsvLista.CheckedItems.Count > 0 Then

                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\marinos1.xlsx"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


                Dim libro As New ClosedXML.Excel.XLWorkbook


                book.Worksheet(1).CopyTo(libro, "Generales")
                book.Worksheet(2).CopyTo(libro, "Percepciones")
                book.Worksheet(3).CopyTo(libro, "Deducciones")
                book.Worksheet(4).CopyTo(libro, "Otros Pagos")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)
                For Each dato As ListViewItem In lsvLista.CheckedItems

                    hoja.Range(2, 1, filaExcel, 1).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 5, filaExcel, 5).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 6, filaExcel, 6).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 26, filaExcel, 26).Style.NumberFormat.Format = "@"
                    ''Generales
                    hoja.Cell(filaExcel, 1).Value = dato.SubItems(1).Text
                    hoja.Cell(filaExcel, 2).Value = dato.SubItems(4).Text
                    hoja.Cell(filaExcel, 3).Value = dato.SubItems(2).Text
                    hoja.Cell(filaExcel, 4).Value = dato.SubItems(5).Text
                    hoja.Cell(filaExcel, 5).Value = dato.SubItems(6).Text
                    hoja.Cell(filaExcel, 6).Value = dato.SubItems(44).Text
                    hoja.Cell(filaExcel, 7).Value = dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 8).Value = dato.SubItems(13).Text
                    hoja.Cell(filaExcel, 9).Value = "G0666980109" ''dato.SubItems(8).Text 
                    hoja.Cell(filaExcel, 10).Value = "VER" ''dato.SubItems(9).Text  
                    hoja.Cell(filaExcel, 11).Value = dato.SubItems(15).Text

                    Dim fecha() As String = dato.SubItems(45).Text.Split(" ")
                    hoja.Cell(filaExcel, 12).Value = fecha(0) ''dato.SubItems(45).Text
                    hoja.Cell(filaExcel, 13).Value = "3" ''dato.SubItems(12).Text 
                    hoja.Cell(filaExcel, 14).Value = ""  ''dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 15).Value = ""  ''dato.SubItems(15).Text
                    hoja.Cell(filaExcel, 16).Value = "1"  ''dato.SubItems(16).Text
                    hoja.Cell(filaExcel, 17).Value = ""  ''dato.SubItems(17).Text
                    hoja.Cell(filaExcel, 18).Value = "2"  ''dato.SubItems(18).Text
                    hoja.Cell(filaExcel, 19).Value = ""  ''dato.SubItems(19).Text
                    hoja.Cell(filaExcel, 20).Value = ""
                    hoja.Cell(filaExcel, 21).Value = dato.SubItems(9).Text  '' dato.SubItems(21).Text
                    hoja.Cell(filaExcel, 22).Value = "4"  ''dato.SubItems(22).Text
                    hoja.Cell(filaExcel, 23).Value = ""  ''dato.SubItems(23).Text
                    hoja.Cell(filaExcel, 24).Value = "5"  ''dato.SubItems(24).Text
                    hoja.Cell(filaExcel, 25).Value = ""
                    hoja.Cell(filaExcel, 26).Value = dato.SubItems(43).Text  ''dato.SubItems(26).Text
                    hoja.Cell(filaExcel, 27).Value = ""  ''dato.SubItems(27).Text
                    hoja.Cell(filaExcel, 28).Value = "" ''dato.SubItems(28).Text
                    hoja.Cell(filaExcel, 29).Value = cboTipoR.SelectedItem.ToString() '' dato.SubItems(29).Text MES DE PAGO
                    hoja.Cell(filaExcel, 30).Value = cboMes.SelectedIndex + 1
                    hoja.Cell(filaExcel, 31).Value = dato.SubItems(10).Text
                    pgbProgreso.Value += 1
                    't = t + 1
                    filaExcel = filaExcel + 1
                Next
                pgbProgreso.Value = 0

                filaExcel = 4
                For Each dato As ListViewItem In lsvLista.CheckedItems
                    ''Percepciones
                    hoja2.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja2.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja2.Cell(filaExcel, 3).Value = dato.SubItems(18).Text
                    hoja2.Cell(filaExcel, 4).Value = ""
                    hoja2.Cell(filaExcel, 5).Value = dato.SubItems(19).Text
                    hoja2.Cell(filaExcel, 6).Value = dato.SubItems(20).Text
                    hoja2.Cell(filaExcel, 7).Value = dato.SubItems(21).Text
                    hoja2.Cell(filaExcel, 8).Value = ""
                    hoja2.Cell(filaExcel, 9).Value = dato.SubItems(22).Text
                    hoja2.Cell(filaExcel, 10).Value = ""
                    hoja2.Cell(filaExcel, 11).Value = dato.SubItems(23).Text
                    hoja2.Cell(filaExcel, 12).Value = ""
                    hoja2.Cell(filaExcel, 13).Value = dato.SubItems(24).Text
                    hoja2.Cell(filaExcel, 14).Value = dato.SubItems(25).Text
                    hoja2.Cell(filaExcel, 15).Value = dato.SubItems(27).Text
                    hoja2.Cell(filaExcel, 16).Value = dato.SubItems(28).Text

                    ''Deducciones
                    hoja3.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja3.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja3.Cell(filaExcel, 3).Value = dato.SubItems(34).Text
                    hoja3.Cell(filaExcel, 4).Value = dato.SubItems(33).Text
                    hoja3.Cell(filaExcel, 5).Value = dato.SubItems(38).Text
                    hoja3.Cell(filaExcel, 6).Value = ""
                    hoja3.Cell(filaExcel, 7).Value = ""
                    hoja3.Cell(filaExcel, 8).Value = dato.SubItems(32).Text
                    hoja3.Cell(filaExcel, 9).Value = dato.SubItems(37).Text
                    'hoja3.Cell(filaExcel, 10).Value = dato.SubItems(36).Text
                    If (dato.SubItems(36).Text = "") Then
                        hoja3.Cell(filaExcel, 10).Value = dato.SubItems(36).Text
                        hoja3.Cell(filaExcel, 11).Value = dato.SubItems(35).Text
                    Else
                        hoja3.Cell(filaExcel, 10).Value = " "
                        hoja3.Cell(filaExcel, 11).Value = validateInfonavit(dato.SubItems(36).Text, dato.SubItems(35).Text)
                    End If


                    ''Otros Pagos
                    'hoja4.Columns("A").Width = 20
                    'hoja4.Columns("B").Width = 20
                    'hoja4.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    'hoja4.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    'hoja4.Cell(filaExcel, 3).Value = dato.SubItems(37).Text
                    'hoja4.Cell(filaExcel, 4).Value = dato.SubItems(48).Text

                    filaExcel = filaExcel + 1


                Next
                Dim moment As Date = Date.Now()
                Dim month As Integer = moment.Month
                Dim year As Integer = moment.Year
                dialogo.DefaultExt = "*.xlsx"
                Dim fechita() As String = Date.Parse(fechadepago).ToLongDateString().Split(",")
                dialogo.FileName = fechita(1).ToUpper & " " & "MARINO " & tipo & " "
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                ''  dialogo.ShowDialog()

                If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' OK button pressed

                    libro.SaveAs(dialogo.FileName)
                    libro = Nothing
                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If

            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try

    End Sub



    Private Sub tsbProcesos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbProcesos.Click

        Try
            Dim tipo As String
            '' Format(Date.Now, "MMMM yyyy") & " " & cboTipoR.SelectedItem.ToString()
            Select Case cboTipoR.SelectedItem.ToString()
                Case "NN"
                    tipo = "ABORDO"
                Case "ND"
                    tipo = "DESCANSO"
                Case Else
                    tipo = "NA"
            End Select


            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()

            If lsvLista.CheckedItems.Count > 0 Then

                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\procesos1.xlsx"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


                Dim libro As New ClosedXML.Excel.XLWorkbook


                book.Worksheet(1).CopyTo(libro, "Generales")
                book.Worksheet(2).CopyTo(libro, "Percepciones")
                book.Worksheet(3).CopyTo(libro, "Deducciones")
                book.Worksheet(4).CopyTo(libro, "Otros Pagos")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)
                For Each dato As ListViewItem In lsvLista.CheckedItems

                    hoja.Range(2, 1, filaExcel, 1).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 5, filaExcel, 5).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 6, filaExcel, 6).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 26, filaExcel, 26).Style.NumberFormat.Format = "@"
                    ''Generales
                    hoja.Cell(filaExcel, 1).Value = dato.SubItems(1).Text
                    hoja.Cell(filaExcel, 2).Value = dato.SubItems(4).Text
                    hoja.Cell(filaExcel, 3).Value = dato.SubItems(2).Text
                    hoja.Cell(filaExcel, 4).Value = dato.SubItems(5).Text
                    hoja.Cell(filaExcel, 5).Value = dato.SubItems(6).Text
                    hoja.Cell(filaExcel, 6).Value = dato.SubItems(44).Text
                    hoja.Cell(filaExcel, 7).Value = dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 8).Value = dato.SubItems(13).Text
                    hoja.Cell(filaExcel, 9).Value = "G0666980109" ''dato.SubItems(8).Text 
                    hoja.Cell(filaExcel, 10).Value = "VER" ''dato.SubItems(9).Text  
                    hoja.Cell(filaExcel, 11).Value = dato.SubItems(15).Text

                    Dim fecha() As String = dato.SubItems(45).Text.Split(" ")
                    hoja.Cell(filaExcel, 12).Value = fecha(0) ''dato.SubItems(45).Text
                    hoja.Cell(filaExcel, 13).Value = "3" ''dato.SubItems(12).Text 
                    hoja.Cell(filaExcel, 14).Value = ""  ''dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 15).Value = ""  ''dato.SubItems(15).Text
                    hoja.Cell(filaExcel, 16).Value = "1"  ''dato.SubItems(16).Text
                    hoja.Cell(filaExcel, 17).Value = ""  ''dato.SubItems(17).Text
                    hoja.Cell(filaExcel, 18).Value = "2"  ''dato.SubItems(18).Text
                    hoja.Cell(filaExcel, 19).Value = ""  ''dato.SubItems(19).Text
                    hoja.Cell(filaExcel, 20).Value = ""
                    hoja.Cell(filaExcel, 21).Value = dato.SubItems(9).Text  '' dato.SubItems(21).Text
                    hoja.Cell(filaExcel, 22).Value = "4"  ''dato.SubItems(22).Text
                    hoja.Cell(filaExcel, 23).Value = ""  ''dato.SubItems(23).Text
                    hoja.Cell(filaExcel, 24).Value = "5"  ''dato.SubItems(24).Text
                    hoja.Cell(filaExcel, 25).Value = ""
                    hoja.Cell(filaExcel, 26).Value = dato.SubItems(43).Text  ''dato.SubItems(26).Text
                    hoja.Cell(filaExcel, 27).Value = ""  ''dato.SubItems(27).Text
                    hoja.Cell(filaExcel, 28).Value = "" ''dato.SubItems(28).Text
                    hoja.Cell(filaExcel, 29).Value = cboTipoR.SelectedItem.ToString() '' dato.SubItems(29).Text MES DE PAGO
                    hoja.Cell(filaExcel, 30).Value = cboMes.SelectedIndex + 1
                    hoja.Cell(filaExcel, 31).Value = dato.SubItems(10).Text
                    pgbProgreso.Value += 1
                    't = t + 1
                    filaExcel = filaExcel + 1
                Next
                pgbProgreso.Value = 0

                filaExcel = 4
                For Each dato As ListViewItem In lsvLista.CheckedItems
                    ''Percepciones
                    hoja2.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja2.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja2.Cell(filaExcel, 3).Value = dato.SubItems(25).Text ''VACACIONES PROPORCIONALES
                    hoja2.Cell(filaExcel, 4).Value = ""
                    hoja2.Cell(filaExcel, 5).Value = dato.SubItems(18).Text  ''SUELDO BASE
                    hoja2.Cell(filaExcel, 6).Value = ""
                    hoja2.Cell(filaExcel, 7).Value = dato.SubItems(19).Text  ''AGINALDO
                    hoja2.Cell(filaExcel, 8).Value = dato.SubItems(20).Text
                    hoja2.Cell(filaExcel, 9).Value = dato.SubItems(26).Text  ''BONO DE PUNTUALIDAD
                    hoja2.Cell(filaExcel, 10).Value = ""
                    hoja2.Cell(filaExcel, 11).Value = dato.SubItems(22).Text ''PRIMA VACACIONAL
                    hoja2.Cell(filaExcel, 12).Value = dato.SubItems(23).Text
                    hoja2.Cell(filaExcel, 13).Value = dato.SubItems(29).Text ''BONO PROCESO
                    hoja2.Cell(filaExcel, 14).Value = " " ''dato.SubItems(25).Text
                    hoja2.Cell(filaExcel, 15).Value = dato.SubItems(28).Text ''FOMENTO DEPORTE
                    hoja2.Cell(filaExcel, 16).Value = " "
                    hoja2.Cell(filaExcel, 17).Value = dato.SubItems(27).Text ''bono asistencia
                    hoja2.Cell(filaExcel, 18).Value = " "

                    ''Deducciones
                    hoja3.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja3.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja3.Cell(filaExcel, 3).Value = dato.SubItems(34).Text
                    hoja3.Cell(filaExcel, 4).Value = dato.SubItems(33).Text
                    hoja3.Cell(filaExcel, 5).Value = dato.SubItems(38).Text
                    hoja3.Cell(filaExcel, 6).Value = ""
                    hoja3.Cell(filaExcel, 7).Value = ""
                    hoja3.Cell(filaExcel, 8).Value = dato.SubItems(32).Text
                    hoja3.Cell(filaExcel, 9).Value = dato.SubItems(37).Text
                    If (dato.SubItems(36).Text = "") Then
                        hoja3.Cell(filaExcel, 10).Value = dato.SubItems(36).Text
                        hoja3.Cell(filaExcel, 11).Value = dato.SubItems(35).Text
                    Else
                        hoja3.Cell(filaExcel, 10).Value = " "
                        hoja3.Cell(filaExcel, 11).Value = validateInfonavit(dato.SubItems(36).Text, dato.SubItems(35).Text)
                    End If

                    hoja3.Cell(filaExcel, 12).Value = dato.SubItems(39).Text


                    ''Otros Pagos
                    'hoja4.Columns("A").Width = 20
                    'hoja4.Columns("B").Width = 20
                    'hoja4.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    'hoja4.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    'hoja4.Cell(filaExcel, 3).Value = dato.SubItems(37).Text
                    'hoja4.Cell(filaExcel, 4).Value = dato.SubItems(48).Text

                    filaExcel = filaExcel + 1


                Next
                Dim moment As Date = Date.Now()
                Dim month As Integer = moment.Month
                Dim year As Integer = moment.Year
                dialogo.DefaultExt = "*.xlsx"
                Dim fechita() As String = Date.Parse(fechadepago).ToLongDateString().Split(",")
                dialogo.FileName = fechita(1).ToUpper & " " & "Procesos " & tipo & " "
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                ''  dialogo.ShowDialog()

                If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' OK button pressed
                    libro.SaveAs(dialogo.FileName)
                    libro = Nothing
                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If




            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try


    End Sub


    Private Sub tsbMaecco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMaecco.Click

        Try
            Dim tipo As String = "NOMINA"



            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()

            If lsvLista.CheckedItems.Count > 0 Then

                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\maecco1.xlsx"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


                Dim libro As New ClosedXML.Excel.XLWorkbook


                book.Worksheet(1).CopyTo(libro, "Generales")
                book.Worksheet(2).CopyTo(libro, "Percepciones")
                book.Worksheet(3).CopyTo(libro, "Deducciones")
                book.Worksheet(4).CopyTo(libro, "Otros Pagos")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)
                For Each dato As ListViewItem In lsvLista.CheckedItems

                    hoja.Range(2, 1, filaExcel, 1).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 5, filaExcel, 5).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 6, filaExcel, 6).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 26, filaExcel, 26).Style.NumberFormat.Format = "@"

                    ''Generales
                    hoja.Cell(filaExcel, 1).Value = dato.SubItems(1).Text
                    hoja.Cell(filaExcel, 2).Value = dato.SubItems(4).Text
                    hoja.Cell(filaExcel, 3).Value = dato.SubItems(2).Text
                    hoja.Cell(filaExcel, 4).Value = dato.SubItems(5).Text
                    hoja.Cell(filaExcel, 5).Value = dato.SubItems(6).Text
                    hoja.Cell(filaExcel, 6).Value = dato.SubItems(46).Text
                    hoja.Cell(filaExcel, 7).Value = dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 8).Value = dato.SubItems(13).Text
                    hoja.Cell(filaExcel, 9).Value = "F2115607102" ''dato.SubItems(8).Text 
                    hoja.Cell(filaExcel, 10).Value = "VER" ''dato.SubItems(9).Text  
                    hoja.Cell(filaExcel, 11).Value = dato.SubItems(15).Text

                    Dim fecha() As String = dato.SubItems(47).Text.Split(" ")
                    hoja.Cell(filaExcel, 12).Value = fecha(0) ''dato.SubItems(45).Text
                    hoja.Cell(filaExcel, 13).Value = "3" ''dato.SubItems(12).Text 
                    hoja.Cell(filaExcel, 14).Value = ""  ''dato.SubItems(14).Text
                    hoja.Cell(filaExcel, 15).Value = ""  ''dato.SubItems(15).Text
                    hoja.Cell(filaExcel, 16).Value = "1"  ''dato.SubItems(16).Text
                    hoja.Cell(filaExcel, 17).Value = ""  ''dato.SubItems(17).Text
                    hoja.Cell(filaExcel, 18).Value = "2"  ''dato.SubItems(18).Text
                    hoja.Cell(filaExcel, 19).Value = ""  ''dato.SubItems(19).Text
                    hoja.Cell(filaExcel, 20).Value = ""
                    hoja.Cell(filaExcel, 21).Value = dato.SubItems(9).Text  '' dato.SubItems(21).Text
                    hoja.Cell(filaExcel, 22).Value = "4"  ''dato.SubItems(22).Text
                    hoja.Cell(filaExcel, 23).Value = "Clase IV"  ''dato.SubItems(23).Text
                    hoja.Cell(filaExcel, 24).Value = "5"  ''dato.SubItems(24).Text
                    hoja.Cell(filaExcel, 25).Value = "MENSUAL"
                    hoja.Cell(filaExcel, 26).Value = dato.SubItems(45).Text  ''dato.SubItems(26).Text
                    hoja.Cell(filaExcel, 27).Value = ""  ''dato.SubItems(27).Text
                    hoja.Cell(filaExcel, 28).Value = "" ''dato.SubItems(28).Text
                    hoja.Cell(filaExcel, 29).Value = "NOMINA" '' dato.SubItems(29).Text MES DE PAGO
                    hoja.Cell(filaExcel, 30).Value = cboMes.SelectedIndex + 1
                    hoja.Cell(filaExcel, 31).Value = dato.SubItems(10).Text
                    pgbProgreso.Value += 1
                    't = t + 1
                    filaExcel = filaExcel + 1
                Next
                pgbProgreso.Value = 0

                filaExcel = 4
                For Each dato As ListViewItem In lsvLista.CheckedItems
                    ''Deducciones
                    hoja2.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja2.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja2.Cell(filaExcel, 3).Value = dato.SubItems(22).Text ''VACACIONES PROPORCIONALES
                    hoja2.Cell(filaExcel, 4).Value = ""
                    hoja2.Cell(filaExcel, 5).Value = dato.SubItems(21).Text  ''DESC. SEM OBLIGATORIO
                    hoja2.Cell(filaExcel, 6).Value = ""
                    hoja2.Cell(filaExcel, 7).Value = dato.SubItems(20).Text  ''TIEMPO EXTRA OCASIONAL
                    hoja2.Cell(filaExcel, 8).Value = "" 'dato.SubItems(19).Text
                    hoja2.Cell(filaExcel, 9).Value = dato.SubItems(19).Text  ''TIEMPO EXTRA FIJO
                    hoja2.Cell(filaExcel, 10).Value = ""
                    hoja2.Cell(filaExcel, 11).Value = dato.SubItems(18).Text ''SUELDO BASE
                    hoja2.Cell(filaExcel, 12).Value = "" ' dato.SubItems(23).Text
                    hoja2.Cell(filaExcel, 13).Value = dato.SubItems(24).Text ''AGUINALDO
                    hoja2.Cell(filaExcel, 14).Value = dato.SubItems(25).Text  ''dato.SubItems(25).Text
                    hoja2.Cell(filaExcel, 15).Value = dato.SubItems(27).Text 'PRIMA VACACIONAL
                    hoja2.Cell(filaExcel, 16).Value = dato.SubItems(28).Text
                    hoja2.Cell(filaExcel, 17).Value = " " '' dato.SubItems(27).Text ''PRIMA DE ANTIGËDAD
                    hoja2.Cell(filaExcel, 18).Value = " "

                    ''Percepciones
                    hoja3.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    hoja3.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    hoja3.Cell(filaExcel, 3).Value = dato.SubItems(34).Text 'IMSS
                    hoja3.Cell(filaExcel, 4).Value = dato.SubItems(33).Text 'ISR
                    hoja3.Cell(filaExcel, 5).Value = dato.SubItems(41).Text 'PRESTAMOS
                    hoja3.Cell(filaExcel, 6).Value = ""
                    hoja3.Cell(filaExcel, 7).Value = ""
                    hoja3.Cell(filaExcel, 8).Value = dato.SubItems(32).Text 'INCAPACIDAD *IMPORTE*
                    hoja3.Cell(filaExcel, 9).Value = dato.SubItems(40).Text  'PENSION ALIMENTICIA
                    hoja3.Cell(filaExcel, 10).Value = dato.SubItems(35).Text 'INFONAVIT
                    hoja3.Cell(filaExcel, 11).Value = dato.SubItems(39).Text 'FONACOT
                    hoja3.Cell(filaExcel, 12).Value = dato.SubItems(38).Text 'CUOTA SINDICAL


                    ''Otros Pagos
                    'hoja4.Columns("A").Width = 20
                    'hoja4.Columns("B").Width = 20
                    'hoja4.Cell(filaExcel, 1).Value = dato.SubItems(4).Text
                    'hoja4.Cell(filaExcel, 2).Value = dato.SubItems(2).Text
                    'hoja4.Cell(filaExcel, 3).Value = dato.SubItems(37).Text
                    'hoja4.Cell(filaExcel, 4).Value = dato.SubItems(48).Text

                    filaExcel = filaExcel + 1


                Next
                Dim moment As Date = Date.Now()
                Dim month As Integer = moment.Month
                Dim year As Integer = moment.Year
                dialogo.DefaultExt = "*.xlsx"
                Dim fechita() As String = Date.Parse(fechadepago).ToLongDateString().Split(",")
                '' dialogo.FileName = Format(moment.Date, "MMMM yyyy ").ToUpper & " " & "Maecco " & tipo & " "
                dialogo.FileName = fechita(1).ToUpper & " " & "Maecco " & tipo & " "
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                ''  dialogo.ShowDialog()

                If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' OK button pressed
                    libro.SaveAs(dialogo.FileName)
                    libro = Nothing
                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If


            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try

    End Sub


    Public Function validateInfonavit(ByVal diferencia As Object, ByVal infonavit As Object) As String
        Dim negativo As Integer = diferencia.ToString.IndexOf("-")
        Dim infonavitcalculado As Double
        If negativo <> -1 Then
            Dim diferenciaInfonavit As Double = diferencia


            infonavitcalculado = CDbl(infonavit) + CDbl(diferencia)
            ' Return infonavitcalculado.ToString()
        Else
            'Return infonavit.ToString
            infonavitcalculado = CDbl(infonavit) + CDbl(diferencia)
        End If

        Return infonavitcalculado.ToString()


    End Function

End Class