Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Office.Interop.Word 'control de office
Imports Microsoft.Office.Interop
Imports System.Data
Public Class frmImportarEmpleadosAlta
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer

    Private Sub frmImportarEmpleadosAlta_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ''MostrarEmpresasC()
        tsbImportar_Click(sender, e)
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
            .Filter = "Hoja de cálculo de excel (xlsx)|*.xlsx;"
            .CheckFileExists = True
            If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
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
        tsbCancelar.Enabled = False
        lsvLista.Visible = False
        tsbImportar.Enabled = False
        Me.cmdCerrar.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        'Application.DoEvents()

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
                        If Forma.ShowDialog = System.Windows.Forms.DialogResult.OK Then
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

                    lsvLista.Columns(1).Width = 120
                    lsvLista.Columns(2).Width = 120
                    lsvLista.Columns(3).Width = 120
                    lsvLista.Columns(4).Width = 120
                    lsvLista.Columns(5).Width = 120
                    'lsvLista.Columns(6).Width = 120
                    'lsvLista.Columns(7).Width = 120
                    'lsvLista.Columns(8).Width = 120
                    'lsvLista.Columns(9).Width = 120
                    'lsvLista.Columns(10).Width = 120
                    'lsvLista.Columns(11).Width = 120
                    'lsvLista.Columns(12).Width = 120
                    'lsvLista.Columns(13).Width = 120
                    'lsvLista.Columns(14).Width = 120
                    'lsvLista.Columns(15).Width = 120
                    'lsvLista.Columns(16).Width = 120
                    'lsvLista.Columns(17).Width = 120
                    'lsvLista.Columns(18).Width = 120
                    'lsvLista.Columns(19).Width = 120
                    'lsvLista.Columns(20).Width = 120
                    'lsvLista.Columns(21).Width = 120
                    'lsvLista.Columns(22).Width = 120
                    'lsvLista.Columns(23).Width = 120
                    'lsvLista.Columns(24).Width = 120
                    'lsvLista.Columns(25).Width = 120
                    'lsvLista.Columns(26).Width = 120
                    'lsvLista.Columns(27).Width = 120
                    'lsvLista.Columns(28).Width = 120
                    'lsvLista.Columns(29).Width = 120
                    'lsvLista.Columns(30).Width = 120
                    'lsvLista.Columns(31).Width = 120
                    'lsvLista.Columns(32).Width = 120
                    'lsvLista.Columns(33).Width = 120
                    'lsvLista.Columns(34).Width = 120
                    'lsvLista.Columns(35).Width = 120
                    'lsvLista.Columns(36).Width = 120
                    'lsvLista.Columns(37).Width = 120
                    'lsvLista.Columns(38).Width = 120
                    'lsvLista.Columns(39).Width = 120
                    'lsvLista.Columns(40).Width = 120
                    'lsvLista.Columns(41).Width = 120
                    'lsvLista.Columns(42).Width = 120






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
        Dim SQL As String, nombresistema As String = ""
        Dim bandera As Boolean

        Dim x As Integer

        Try
            If lsvLista.CheckedItems.Count > 0 Then
                Dim mensaje As String


                pnlProgreso.Visible = True
                pnlCatalogo.Enabled = False
                'Application.DoEvents()


                Dim IdEmpleado As Long
                Dim i As Integer = 0

                Dim t As Integer = 0
                Dim conta As Integer = 0



                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = lsvLista.CheckedItems.Count


                'Dim fila As New DataRow
                SQL = "Select * from usuarios where idUsuario = " & idUsuario
                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then
                    Dim Fila As DataRow = rwFilas(0)
                    nombresistema = Fila.Item("nombre")
                End If

                Dim empleadofull As ListViewItem
                Dim mensa As String
                '' mensa = "Datos incompletos en el empleado: "

                For Each empleado As ListViewItem In lsvLista.CheckedItems


                    For x = 0 To empleado.SubItems.Count - 1

                        If empleado.SubItems(x).Text = "" Then
                            mensa = " Datos incompletos en el empleado: Empleado: " & empleado.Text & " Columna:" & x.ToString() & " "


                            '' MessageBox.Show(mensa, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            bandera = False
                            x = empleado.SubItems.Count - 1

                        Else

                            empleadofull = empleado

                            '' MessageBox.Show("Pasa" & empleado.SubItems(x).Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                            bandera = True

                        End If
                    Next x

                    If bandera <> False Then

                        Dim b As String = Trim(empleadofull.SubItems(24).Text)
                        Dim idbanco As Integer
                        If b <> "" Then
                            Dim banco As DataRow() = nConsulta("select * from bancos where cBanco like '%" & b & "%'") ' clave =" & b)
                            If banco Is Nothing Then
                                idbanco = 1
                                mensa = "Revise el tipo de banco"
                                ' bandera = False
                            Else
                                idbanco = banco(0).Item("iIdBanco")
                            End If

                        Else
                            b = 0
                        End If
                        Dim p As String = Trim(empleadofull.SubItems(15).Text) ''CPuesto
                        Dim cPuesto As String
                        If p <> "" Then
                            Dim puesto As DataRow() = nConsulta("select * FROM Puestos where cNombre like '" & p & "'")
                            If puesto Is Nothing Then
                                cPuesto = ""
                                mensa = "Revise el tipo de Puesto"
                                ' bandera = False
                            Else
                                cPuesto = puesto(0).Item("cNombre")
                                p = puesto(0).Item("iIdPuesto")
                            End If

                        Else
                            p = 0
                        End If

                        'Nombre depto

                        Dim d As String = Trim(empleadofull.SubItems(38).Text) ''Depto
                        Dim iIdDpto As String
                        If d <> "" Then
                            Dim dpto As DataRow() = nConsulta("SELECT * FROM Departamentos where cNombre LIKE '" & d & "'")
                            If dpto Is Nothing Then
                                iIdDpto = " "
                                mensa = "Revise el tipo de dapartamento"
                                'bandera = False
                            Else
                                iIdDpto = dpto(0).Item("iIdDepartamento")
                            End If

                        Else
                            d = 0
                        End If
                        'Clave del estado
                        Dim l As String = Trim(empleadofull.SubItems(19).Text) ''Code
                        Dim cLugar As String
                        If l <> "" Then
                            Dim lugar As DataRow() = nConsulta("SELECT * FROM Cat_Estados WHERE cClave LIKE'" & l & "'")
                            If lugar Is Nothing Then
                                cLugar = ""
                                mensa = "Revise el tipo de Puesto"
                                'bandera = False
                            Else
                                cLugar = lugar(0).Item("cEstado")
                            End If

                        Else
                            l = 0
                        End If

                        Dim factor As String
                        Select Case Trim(empleadofull.SubItems(32).Text)
                            Case "VSM"
                                factor = Trim(empleadofull.SubItems(32).Text)
                                ' The following is the only Case clause that evaluates to True.
                            Case "PORCENTAJE"
                                factor = Trim(empleadofull.SubItems(32).Text)
                            Case "CUOTA FIJA"
                                factor = Trim(empleadofull.SubItems(32).Text)
                            Case Else
                                factor = "(No asignado)"
                        End Select

                        Dim number As Integer
                        Select Case Trim(empleadofull.SubItems(33).Text)
                            Case "QUINCENAL"
                                number = 4
                                ' The following is the only Case clause that evaluates to True.
                            Case "MENSUAL"
                                number = 5
                            Case "SEMANAL"
                                number = 2
                            Case Else
                                ' number = 10
                                number = 5
                        End Select

                        'Cuenta o clabe

                        Dim cuenta As String
                        Dim clabe As String

                        If Len(Trim(empleadofull.SubItems(25).Text)) = 18 Then
                            cuenta = 0
                           
                            clabe = Trim(empleadofull.SubItems(25).Text)

                        ElseIf Len(Trim(empleadofull.SubItems(25).Text) < 12) Then
                            'clabe = 0
                            cuenta = Trim(empleadofull.SubItems(25).Text)
                        Else
                            clabe = 0
                            cuenta = 0

                        End If
                        If Len(Trim(empleadofull.SubItems(30).Text)) = 18 Then 'clabe
                            clabe = Trim(empleadofull.SubItems(30).Text)
                        End If

                        'CHECK INFO
                        Dim permanente As Integer
                        If (Trim(empleadofull.SubItems(44).Text) <> 0) Then
                            permanente = 1
                        Else
                            permanente = 0
                        End If



                        Dim dFechaNac, dFechaCap, dFechaPlanta As String, dFechaPatrona ''--, dFechaTerminoContrato, dFechaSindicato, dFechaAntiguedad As String

                        dFechaNac = Date.Parse(Trim(empleadofull.SubItems(13).Text).ToString) ''Format(Trim(empleadofull.SubItems(18).Text), "yyyy/dd/MM"))
                        dFechaCap = Date.Parse((Trim(empleadofull.SubItems(14).Text)).ToString)
                        dFechaPlanta = Date.Parse(Trim(empleadofull.SubItems(40).Text).ToString)
                        dFechaPatrona = Date.Parse((Trim(empleadofull.SubItems(14).Text).ToString))
                        'dFechaTerminoContrato = ((Trim(empleadofull.SubItems(44).Text))) ''No asignado
                        'dFechaSindicato = (Trim(empleadofull.SubItems(14).Text))
                        'dFechaAntiguedad = Trim(empleadofull.SubItems(14).Text)




                        '***********************************'
                        SQL = "select max(iIdEmpleadoC) as id from empleadosC"

                        Dim salario As String = "0"  '= Trim(empleadofull.SubItems(17).Text)
                        Dim sdi As String = Trim(empleadofull.SubItems(18).Text)
                        Dim sd As String = Trim(empleadofull.SubItems(17).Text)
                        Dim status As String = Trim(empleadofull.SubItems(12).Text)

                        'CUANDO SE AGREGA EL SUELDO ORDINARIO
                        Dim rwFilas2 As DataRow() = nConsulta(SQL)

                        If rwFilas2 Is Nothing = False Then
                            Dim Fila As DataRow = rwFilas2(0)
                            SQL = "EXEC setSueldoAltaInsertar  0," & IIf(salario = "", 0, salario) & ",'" & dFechaPatrona ' Format(dtppatrona.Value.Date, "yyyy/dd/MM")
                            SQL += "',0,''," & IIf(sd = "", 0, sd) & "," & IIf(sdi = "", 0, sdi) & "," & Fila.Item("id") '"1" 
                            SQL += ",'01/01/1900',''"

                        End If

                        If SQL <> "" Then
                            If nExecute(SQL) = False Then
                                Exit Sub
                            End If
                        End If

                        '***********************************'

                        SQL = "EXEC setempleadosCInsertar 0,'" & Trim(empleadofull.SubItems(1).Text) & "','" & Trim(empleadofull.SubItems(2).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(3).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(4).Text) & "','" & Trim(empleadofull.SubItems(3).Text) & " " & Trim(empleadofull.SubItems(4).Text) & " " & Trim(empleadofull.SubItems(2).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(5).Text) & "','" & Trim(empleadofull.SubItems(6).Text) & "','" & Trim(empleadofull.SubItems(7).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(8).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(9).Text) & "'," & Trim(empleadofull.SubItems(10).Text) & ",'" & Trim(empleadofull.SubItems(11).Text)
                        SQL &= "'," & IIf(Trim(empleadofull.SubItems(12).Text) = "FEMENINO", 0, 1) & ",'" & dFechaNac & "','" & dFechaCap
                        SQL &= "','" & cPuesto & "','" & d
                        SQL &= "'," & IIf(Trim(empleadofull.SubItems(17).Text) = "", 0, Trim(empleadofull.SubItems(17).Text)) & "," & IIf(Trim(empleadofull.SubItems(18).Text) = "", 0, Trim(empleadofull.SubItems(18).Text))
                        SQL &= ",'" & cLugar & "','" & Trim(empleadofull.SubItems(20).Text) & "','','','" & Trim(empleadofull.SubItems(21).Text) & "','" & Trim(empleadofull.SubItems(22).Text)
                        SQL &= "',1," & IIf((empleadofull.SubItems(23).Text) = "", 0, (empleadofull.SubItems(23).Text)) & ",0" & ",-1" & "," & 1 & "," & idbanco
                        SQL &= ",'" & cuenta & "',1,'" & Trim(empleadofull.SubItems(26).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(27).Text) & "'," & Trim(empleadofull.SubItems(28).Text) & ",'" & Trim(empleadofull.SubItems(29).Text)
                        SQL &= "','" & dFechaPlanta & "','" & dFechaPlanta & "','" & dFechaPlanta
                        SQL &= "'," & permanente & ",'" & clabe & "','" & " "
                        SQL &= "'," & 1 & ",'" & Trim(empleadofull.SubItems(31).Text) & "','" & factor ''factor
                        SQL &= "'," & Trim(empleadofull.SubItems(44).Text) & ",'" & Trim(empleadofull.SubItems(33).Text) & "','" & Trim(empleadofull.SubItems(34).Text)
                        SQL &= "','" & Trim(empleadofull.SubItems(35).Text) & "','" & Trim(empleadofull.SubItems(36).Text) & "','" & Trim(empleadofull.SubItems(37).Text) & "'," & -1 ''cliente 
                        SQL &= "," & p & ",'" & iIdDpto
                        SQL &= "'," & IIf(Trim(empleadofull.SubItems(39).Text) = "SOLTERO", 0, 1)
                        SQL &= "," & 36 ''BANCO 2
                        SQL &= ",'" & " "
                        SQL &= "','" & "" & "'"
                        SQL &= "," & 0 & ",'" & dFechaPlanta & "','" & Trim(empleadofull.SubItems(41).Text) & "','" & Trim(empleadofull.SubItems(42).Text) & "'"
                        SQL &= ",'" & Trim(empleadofull.SubItems(43).Text) & "','" & " " & "'"

                        If nExecute(SQL) = False Then
                            MessageBox.Show("Error en el registro con los siguiente datos:   Empleado:  " & Trim(empleado.SubItems(2).Text), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            tsbCancelar_Click(sender, e)
                            pnlProgreso.Visible = False
                            Exit Sub
                        End If

                        '*********************
                        'Agregar alta/baja
                        'If blnNuevo Then
                        'Obtener id

                        SQL = "select max(iIdEmpleadoC) as id from empleadosC"
                        Dim rwFilas3 As DataRow() = nConsulta(SQL)

                        If rwFilas3 Is Nothing = False Then
                            Dim Fila As DataRow = rwFilas3(0)
                            SQL = "EXEC setIngresoBajaAltaInsertar  0," & Fila.Item("id") & ",'" & "A" & "','" & dFechaPatrona & "','01/01/1900','',''"
                            'Enviar correo

                            'Enviar_Mail(GenerarCorreo2(epat, ec, Trim(empleadofull.SubItems(1).Text), List), correo, "Empleado Alta")
                            'Enviar_Mail(GenerarCorreo(gIdEmpresa, cboclientefiscal.SelectedValue, Fila.Item("id")), "p.isidro@mbcgroup.mx;l.aquino@mbcgroup.mx;r.garcia@mbcgroup.mx", "Alta de empleado")
                        End If



                        If SQL <> "" Then
                            If nExecute(SQL) = False Then
                                Exit Sub
                            End If
                        End If
                        '**********************************************

                        pgbProgreso.Value += 1
                        ''Application.DoEvents()
                        t = t + 1
                    Else
                        MessageBox.Show(mensa, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        tsbCancelar_Click(sender, e)
                        pnlProgreso.Visible = False
                    End If





                Next

                If bandera <> False Then
                    tsbCancelar_Click(sender, e)
                    pnlProgreso.Visible = False

                    MessageBox.Show(t.ToString() & "  Proceso terminado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    pnlProgreso.Visible = False
                    MessageBox.Show("No se guardo ninguna dato, revise y vuelva a intentarlo ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If


            Else

                MessageBox.Show("Por favor seleccione al menos una registro para importar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            pnlCatalogo.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
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
        tsbProcesar.Enabled = False
        tsbGuardar.Enabled = False
        tsbCancelar.Enabled = False
        tsbNuevo.Enabled = True
    End Sub

    Private Sub frmImportarEmpladosAlta_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

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

    Private Sub tsbContrato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim MSWord As New Word.Application
        Dim Documento As Word.Document
        Dim Ruta As String, strPWD As String
        Dim SQL As String
        Try

            Ruta = System.Windows.Forms.Application.StartupPath & "\Archivos\test.docx"

            FileCopy(Ruta, "C:\Temp\TMM.docx")
            Documento = MSWord.Documents.Open("C:\Temp\TMM.docx")

            'SQL = "select iIdEmpleadoAlta,cCodigoEmpleado,empleadosAlta.cNombre,cApellidoP,cApellidoM,cRFC,cCURP,"
            'SQL &= "cIMSS,cDescanso,cCalleNumero,cCiudadP,cCP,iSexo,iEstadoCivil, dFechaNac,puestos.cNombre as cPuesto,fSueldoBase,"
            'SQL &= "cNacionalidad,empleadosAlta.cFuncionesPuesto, fSueldoOrd, iOrigen,empresa.iIdEmpresa ,empresa.calle +' '+ empresa.numero AS cDireccionP, empresa.localidad as cCiudadP, empresa.cp as cCPP, iCategoria, cJornada, cHorario,"
            'SQL &= "cHoras, cDescanso, cFechaPago, cFormaPago, cLugarPago, cLugarFirmaContrato,empleadosAlta.cLugarPrestacion, dFechaPatrona,"
            'SQL &= "empresa.nombrefiscal, empresa.RFC AS cRFCP, empresa.cRepresentanteP, empresa.cObjetoSocialP,  Cat_SindicatosAlta.cNombre AS cNombreSindicato"
            'SQL &= " from ((empleadosAlta"
            'SQL &= " inner join empresa on fkiIdEmpresa= iIdEmpresa)"
            'SQL &= " inner join puestos on fkiIdPuesto= iIdPuesto)"
            'SQL &= " inner join (clientes inner join Cat_SindicatosAlta on fkiIdSindicato= iIdSindicato) on fkiIdCliente=iIdCliente"
            'SQL &= " where iIdEmpleadoAlta = " & gIdEmpleado
            SQL = "SELECT * FROM (empleadosC INNER JOIN familiar on iIdEmpleadoC=fkiIdEmpleadoC) WHERE iIdEmpleado="
            Dim rwEmpleado As DataRow() = nConsulta(SQL)

            If rwEmpleado Is Nothing = False Then
                Dim fEmpleado As DataRow = rwEmpleado(0)


                Documento.Bookmarks.Item("cNombreLargo").Range.Text = fEmpleado.Item("cNombre") & " " & fEmpleado.Item("cApellidoP") & " " & fEmpleado.Item("cApellidoM")
                Documento.Bookmarks.Item("cNombreLargo2").Range.Text = fEmpleado.Item("cNombre") & " " & fEmpleado.Item("cApellidoP") & " " & fEmpleado.Item("cApellidoM")
                Documento.Bookmarks.Item("cNombreFiscal").Range.Text = fEmpleado.Item("nombrefiscal")
                Documento.Bookmarks.Item("cFecha").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("cFecha2").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("cLugarFirma").Range.Text = fEmpleado.Item("cLugarFirmaContrato")
                Documento.Bookmarks.Item("cCURP").Range.Text = fEmpleado.Item("cCURP")
                Documento.Bookmarks.Item("cRFC").Range.Text = fEmpleado.Item("cRFC")
                Documento.Bookmarks.Item("cRFCP").Range.Text = fEmpleado.Item("cRFCP")

                Documento.Bookmarks.Item("cDireccionP").Range.Text = fEmpleado.Item("cDireccionP") & ", " & fEmpleado.Item("cCiudadP") & ", " & fEmpleado.Item("cCPP")
                Documento.Bookmarks.Item("cDireccionP2").Range.Text = fEmpleado.Item("cDireccionP") & ", " & fEmpleado.Item("cCiudadP") & ", " & fEmpleado.Item("cCPP")
                Documento.Bookmarks.Item("cDireccion").Range.Text = fEmpleado.Item("cCalleNumero") & ", " & fEmpleado.Item("cCiudadP") & ", " & fEmpleado.Item("cCP")
                If IsDBNull(fEmpleado.Item("cRepresentanteP")) = False Then
                    Documento.Bookmarks.Item("cRepresentanteP").Range.Text = fEmpleado.Item("cRepresentanteP")
                    Documento.Bookmarks.Item("cRepresentanteP2").Range.Text = fEmpleado.Item("cRepresentanteP")
                Else
                    MessageBox.Show("Falta agregar Representante Patrona", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
                Documento.Save()
                MSWord.Visible = True
            End If

        Catch ex As Exception
            Documento.Close()
            MessageBox.Show(ex.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try
    End Sub

    Private Sub tsbVerificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbVerificar.Click
        

        Dim contador As Integer = 0

        For filitas = 0 To lsvLista.Items.Count - 1

            SQL = "SELECT * FROM empleadosC WHERE cCodigoEmpleado= " & lsvLista.Items(filitas).SubItems(1).Text
            Dim rwFilas As DataRow() = nConsulta(SQL)
            If rwFilas Is Nothing = False Then
                For Each f In rwFilas

                    lsvLista.Items(filitas).BackColor = Color.GreenYellow

                    contador = contador + 1

                Next
            End If

        Next
        MsgBox(contador.ToString & " Datos repetidos")


    End Sub

    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        Dim contador As Integer = 0
        For filitas = 0 To lsvLista.Items.Count - 1

            SQL = "update empleadosC set iEstatus=0 where cCodigoEmpleado='" & lsvLista.Items(filitas).SubItems(1).Text & "'"


            If nExecute(SQL) = False Then
                Exit Sub
            Else

                lsvLista.Items(filitas).BackColor = Color.Red

                contador = contador + 1

            End If

        Next
        MsgBox(contador.ToString & " ELIMINADOS")
    End Sub
End Class