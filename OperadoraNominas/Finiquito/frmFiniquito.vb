Imports ClosedXML.Excel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Mime.MediaTypeNames

Public Class frmFiniquito
    Public gIdPeriodo As Integer
    Public gIdSerie As Integer
    Public gUMA As Double
    Dim sheetIndex As Integer = -1
    Dim SQL As String
    Dim contacolumna As Integer
    Dim ini, fin As String
    Dim rutita As String
    Dim fechadepago As String
    Public gAnioActual As String
    Dim totalsindicato As Double

    Private Sub frmSubirFiniquito_loand(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            
            cargarperiodos()
            cargarEmpleados()
            cargarserie()
            cboPeriodo.SelectedIndex = gIdPeriodo

            initTextbox()


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub initTextbox()

        'STP
        txtIndeminizacion.TabIndex = 1
        txtPrimaAntiguedad.TabIndex = 2
        txtSueldo.TabIndex = 3
        txtAguinaldo.TabIndex = 4
        txtVacacionesProp.TabIndex = 5
        txtPrimaVacacional.TabIndex = 6
        txtVacacionesPendientes.TabIndex = 7
        txtHE2.TabIndex = 8
        txtHE3.TabIndex = 9
        txtDL.TabIndex = 10
        txtBonos.TabIndex = 11
        txtIncidencias.TabIndex = 12
        txtInfonavit.TabIndex = 13
        txtFonacot.TabIndex = 14
        txtVales.TabIndex = 15
        txtISR.TabIndex = 16
        txtISRIndemnizacion.TabIndex = 17
        txtSTP.TabIndex = 18

        txtIndeminizacion.Text = 0.0
        txtPrimaAntiguedad.Text = 0.0
        txtSueldo.Text = 0.0
        txtAguinaldo.Text = 0.0
        txtVacacionesProp.Text = 0.0
        txtPrimaVacacional.Text = 0.0
        txtVacacionesPendientes.Text = 0.0
        txtHE2.Text = 0.0
        txtHE3.Text = 0.0
        txtDL.Text = 0.0
        txtBonos.Text = 0.0
        txtIncidencias.Text = 0.0
        txtInfonavit.Text = 0.0
        txtFonacot.Text = 0.0
        txtVales.Text = 0.0
        txtISR.Text = 0.0
        txtISRIndemnizacion.Text = 0.0
        txtSTP.Text = 0.0

        'EXCEDENTE
        txtindeminizacionExce.TabIndex = 19
        txtAntiguedadExce.TabIndex = 20
        txtSueldoExce.TabIndex = 21
        txtAguinaldoExce.TabIndex = 22
        txtVacacionesPropExce.TabIndex = 23
        txtVacacionesPendientesExce.TabIndex = 24
        txtHE2Exce.TabIndex = 25
        txtHE3Exce.TabIndex = 26
        txtDLExce.TabIndex = 27
        txtBonosExce.TabIndex = 28
        txtIncidenciasExce.TabIndex = 29
        txtInfonavitExce.TabIndex = 30
        txtSindicato.TabIndex = 31




        txtindeminizacionExce.Text = 0.0
        txtAntiguedadExce.Text = 0.0
        txtSueldoExce.Text = 0.0
        txtAguinaldoExce.Text = 0.0
        txtVacacionesPropExce.Text = 0.0
        txtPrimaVacacionalExce.Text = 0.0
        txtVacacionesPendientesExce.Text = 0.0
        txtHE2Exce.Text = 0.0
        txtHE3Exce.Text = 0.0
        txtDLExce.Text = 0.0
        txtBonosExce.Text = 0.0
        txtIncidenciasExce.Text = 0.0
        txtInfonavitExce.Text = 0.0
        txtSindicato.Text = 0.0

        btnAgregar.TabIndex = 32

    End Sub

    Private Sub cargarEmpleados()

        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "select iIdEmpleadoC, cNombre, cApellidoP,cApellidoM,cNombreLargo from empleadosC WHERE iEstatus=1"
            nCargaCBO(cboEmpleado, sql, "cNombreLargo", "iIdEmpleadoC")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
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

    Private Sub cargarserie()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "SELECT TOP 1 fkiIdPeriodo, iEstatusEmpleado FROM NOMINA where fkiIdPeriodo=" & gIdPeriodo + 1
            sql &= " order by iEstatusEmpleado desc"
            Dim rwSerie As DataRow() = nConsulta(sql)
            If rwSerie Is Nothing = False Then
                cboSerie.SelectedIndex = rwSerie(0)("iEstatusEmpleado") + 1
            End If
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
            tsbNuevo.Enabled = True
            tsbCancelar_Click(sender, e)
            MessageBox.Show(ex.Message.ToString)
            Me.Close()

        End Try
    End Sub

    Private Sub tsbCancelar_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancelar.Click
        pnlCatalogo.Enabled = False
        lsvLista.Items.Clear()

        lblRuta.Text = ""
        tsbImportar.Enabled = False
        tsbCancelar.Enabled = False
        tsbNuevo.Enabled = True
        pnlProgreso.Visible = False
    End Sub


    Private Sub tsbGuardar_Click(sender As System.Object, e As System.EventArgs) Handles tsbGuardar.Click
        Try

            SQL = "select  * from empleadosC where " 'fkiIdClienteInter=-1"
            SQL &= "iIdEmpleadoC=" & cboEmpleado.SelectedIndex + 1
            'SQL &= " order by cFuncionesPuesto,cNombreLargo"
            Dim rwDatosEmpleado As DataRow() = nConsulta(SQL)
            If rwDatosEmpleado Is Nothing = False Then

                SQL = "EXEC [setNominaInsertarFiniquito ] 0"
                'periodo
                SQL &= "," & cboPeriodo.SelectedIndex + 1
                'idempleado
                SQL &= "," & cboEmpleado.SelectedIndex + 1
                'idempresa
                SQL &= ",1"
                'Puesto
                SQL &= "," & rwDatosEmpleado(0)("fkiIdPuesto")
                'departamento
                SQL &= "," & rwDatosEmpleado(0)("fkiIdDepartamento")
                'estatus empleado
                SQL &= "," & cboSerie.SelectedIndex
                'edad
                SQL &= "," & CalcularEdad((Date.Parse(rwDatosEmpleado(0)("dFechaNac").ToString)).Day, Month(rwDatosEmpleado(0)("dFechaNac").ToString()), Year(rwDatosEmpleado(0)("dFechaNac").ToString()))
                'puesto
                SQL &= ",'" & rwDatosEmpleado(0)("cPuesto") & "'"
                'buque
                Dim rwDepto As DataRow() = nConsulta("select * from departamentos where iIdDepartamento =" & rwDatosEmpleado(0)("fkiIdDepartamento"))

                SQL &= ",'" & rwDepto(0)("cNombre") & "'"
                'iTipo Infonavit
                SQL &= ",'" & "'"
                'valor infonavit
                SQL &= "," & 0.0
                'fTExtra2V
                SQL &= "," & 0.0
                'fTExtra3V
                SQL &= "," & 0.0
                'fDescansoLV
                SQL &= "," & 0.0
                'fDiaFestivoLV
                SQL &= "," & 0.0
                'fHoras_extras_dobles_V
                SQL &= ",0"
                'fHoras_extras_triples_V
                SQL &= ",0"
                'fDescanso_Laborado_V
                SQL &= ",0"
                'fDia_Festivo_laborado_V
                SQL &= ",0"
                'fPrima_Dominical_V 
                SQL &= "," & 0.0
                'fFalta_Injustificada_V
                SQL &= "," & 0.0
                'fPermiso_Sin_GS_V
                SQL &= "," & 0.0
                'fT_No_laborado_V
                SQL &= "," & 0.0
                'salario base
                SQL &= "," & rwDatosEmpleado(0)("fSueldoOrd")
                'salario diario
                SQL &= "," & rwDatosEmpleado(0)("fSueldoBase")
                'salario integrado
                SQL &= "," & rwDatosEmpleado(0)("fSueldoIntegrado")
                'Dias trabajados
                SQL &= "," & 1
                'tipo incapacidad
                SQL &= ",'" & "Ninguna"
                'numero dias incapacidad
                SQL &= "'," & 0
                'sueldobruto
                SQL &= "," & IIf((txtSueldo.Text) = "", 0, (txtSueldo.Text))

                'fSeptimoDia
                SQL &= "," & 0.0
                'fPrimaDomGravada
                SQL &= "," & 0.0
                'fPrimaDomExenta
                SQL &= "," & 0.0
                'fTExtra2Gravado
                SQL &= "," & IIf((txtHE2.Text) = "", 0, CDbl(txtHE2.Text) / 2)
                'fTExtra2Exento
                SQL &= "," & IIf((txtHE2.Text) = "", 0, CDbl(txtHE2.Text) / 2)
                'fTExtra3
                SQL &= "," & IIf((txtHE3.Text) = "", 0, (txtHE3.Text))
                'fDescansoL
                SQL &= "," & 0.0
                'fDiaFestivoL
                SQL &= "," & IIf((txtDL.Text) = "", 0, (txtDL.Text))
                'fBonoAsistencia
                SQL &= "," & 0.0
                'fBonoProductividad
                SQL &= "," & 0.0
                'fBonoPolivalencia
                SQL &= "," & 0.0
                'fBonoEspecialidad
                SQL &= "," & 0.0
                'fBonoCalidad
                SQL &= "," & 0.0
                'fCompensacion
                SQL &= "," & IIf((txtBonos.Text) = "", 0, (txtBonos.Text))
                'fSemanaFondo
                SQL &= "," & 0.0
                '@fFaltaInjustificada 
                SQL &= "," & IIf((txtIncidencias.Text) = "", 0, (txtIncidencias.Text))
                '@fPermisoSinGS 
                SQL &= "," & 0.0
                '@fIncrementoRetenido 
                SQL &= "," & 0.0

                'Validacion G Y E
                Dim topex30 As Double = gUMA * 30
                Dim topex15 As Double = gUMA * 15
                Dim aguigravado As Double = IIf(CDbl(txtAguinaldo.Text) > topex30, CDbl(txtAguinaldo.Text) - topex30, 0)
                Dim primavacgravado As Double = IIf(CDbl(txtPrimaVacacional.Text) > topex15, CDbl(txtPrimaVacacional.Text) - topex15, 0)
                'vacaciones proporcionales
                SQL &= "," & (txtVacacionesProp.Text)
                'aguinaldo gravado
                SQL &= "," & aguigravado
                'aguinaldo exento
                SQL &= "," & CDbl(txtAguinaldo.Text) - aguigravado
                'prima vacacional gravado
                SQL &= "," & primavacgravado
                'prima vacacional exento
                SQL &= "," & CDbl(txtPrimaVacacional.Text) - primavacgravado
                'totalpercepciones
                SQL &= "," & IIf(txtSueldo.Text = "", 0, txtSueldo.Text) + CDbl(txtVacacionesProp.Text) + aguigravado + (CDbl(txtAguinaldo.Text) - aguigravado) + primavacgravado + (CDbl(txtPrimaVacacional.Text) - primavacgravado) + IIf(txtBonos.Text = "", 0, CDbl(txtBonos.Text)) + IIf(txtDL.Text = "", 0, CDbl(txtDL.Text))
                'totalpercepcionesISR
                SQL &= "," & CDbl(txtSueldo.Text) + CDbl(txtVacacionesProp.Text) + aguigravado + primavacgravado
                'Incapacidad
                SQL &= "," & 0.0
                'isr
                SQL &= "," & CDbl(txtISR.Text) + IIf((txtISRIndemnizacion.Text) = "", 0, (txtISRIndemnizacion.Text))
                'imss
                SQL &= "," & 0.0
                'infonavit
                SQL &= "," & 0.0
                'infonavit anterior
                SQL &= "," & 0.0
                'ajuste infonavit
                SQL &= "," & 0.0
                'Pension alimenticia
                SQL &= "," & 0.0
                'Prestamo
                SQL &= "," & 0.0
                'Fonacot
                SQL &= "," & IIf((txtFonacot.Text) = "", 0, (txtFonacot.Text))
                'fT_No_laborado
                SQL &= "," & 0.0
                'cuota sindical
                SQL &= "," & 0.0

                'Subsidio Generado
                SQL &= "," & 0.0
                'Subsidio Aplicado
                SQL &= "," & 0.0
                'Operadora
                SQL &= "," & IIf((txtSTP.Text) = "", 0, (txtSTP.Text))
                'Prestamo Personal Asimilado
                SQL &= "," & 0.0
                'Adeudo_Infonavit_Asimilado
                SQL &= "," & 0.0
                'excedente variable
                SQL &= "," & 0.0
                'Excedente sindicato/ppp

                totalsindicato = CDbl(txtindeminizacionExce.Text) + CDbl(txtAntiguedadExce.Text) + CDbl(txtSueldoExce.Text) + CDbl(txtAguinaldoExce.Text) + CDbl(txtVacacionesPropExce.Text) + CDbl(txtPrimaVacacionalExce.Text) + CDbl(txtVacacionesPendientesExce.Text) + CDbl(txtHE2Exce.Text) + CDbl(txtHE3Exce.Text) + CDbl(txtDLExce.Text) + CDbl(txtBonosExce.Text) + CDbl(txtBonosExce.Text) + CDbl(txtIncidenciasExce.Text) + CDbl(txtInfonavitExce.text)
                SQL &= "," & CDbl(txtSindicato.Text)

                'Prima excedente
                SQL &= "," & 0.0
                'Pension alimenticia Asimi
                SQL &= "," & 0.0
                'excedente manual
                SQL &= "," & 0.0
                'Comision asimilados
                SQL &= "," & 0.0

                'IMSS_CS
                SQL &= "," & 0.0
                'RCV_CS
                SQL &= "," & 0.0
                'Infonavit_CS
                SQL &= "," & 0.0
                'ISN_CS
                SQL &= "," & 0.0
                'Total Costo Social
                SQL &= "," & 0.0
                'Subtotal
                SQL &= ",0" '& IIf(dtgDatos.Rows(x).Cells(84).Value = "", "0", dtgDatos.Rows(x).Cells(84).Value.ToString.Replace(",", ""))
                'IVA
                SQL &= ",0" '& IIf(dtgDatos.Rows(x).Cells(85).Value = "", "0", dtgDatos.Rows(x).Cells(85).Value.ToString.Replace(",", ""))
                'TOTAL DEPOSITO
                SQL &= ",0" '& IIf(dtgDatos.Rows(x).Cells(86).Value = "", "0", dtgDatos.Rows(x).Cells(86).Value.ToString.Replace(",", ""))
                'Estatus
                SQL &= ",1"
                'Estatus Nomina
                SQL &= ",0"
                'Tipo Nomina
                SQL &= ",0"
                'tipo consecutivo
                SQL &= "," & 1
                'fechainicial
                SQL &= ",'" & Date.Now.ToShortDateString & ""
                'fechafinal
                SQL &= "','" & Date.Now.ToShortDateString & "'"


                If nExecute(SQL) = False Then
                    MessageBox.Show("Ocurrio un error " & rwDatosEmpleado(0)("cNombreLargo"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'pnlProgreso.Visible = False
                    Exit Sub
                End If


                'tabla nueva

                SQL = "EXEC [setNominaComplementoInsertar] 0"
                'periodo
                SQL &= "," & cboPeriodo.SelectedIndex + 1
                'idempleado
                SQL &= "," & rwDatosEmpleado(0)("iIdEmpleadoC")
                'serie
                SQL &= "," & cboSerie.SelectedIndex
                'vales
                SQL &= "," & IIf(txtVales.Text = "", 0, (txtVales.Text))
                'aportacion
                SQL &= "," & 0.0
                'prAguinaldo
                SQL &= "," & 0.0
                'prPrimaVa
                SQL &= "," & 0.0
                'prPrimaAnt
                SQL &= "," & 0.0
                'prIndemnizacion
                SQL &= "," & 0.0
                'SAR
                SQL &= "," & 0.0
                'Cesantia
                SQL &= "," & 0.0
                'PDE
                SQL &= "," & 0.0
                'TE2E
                SQL &= "," & IIf(txtHE2Exce.Text = "", "0", txtHE2Exce.Text)
                'TE3E
                SQL &= "," & IIf(txtHE3Exce.Text = "", "0", txtHE3Exce.Text)
                'Descanso_Laborado_V
                SQL &= "," & IIf(txtDLExce.Text = "", "0", txtDLExce.Text)
                'DiaFestivoLaborado_v
                SQL &= "," & 0.0
                'COSTO
                SQL &= ",''"
                'vALOR1
                SQL &= ",0"
                'VALOR2
                SQL &= ",0"
                'valor3
                SQL &= ",0"
                'valor4
                SQL &= ",0"
                'iEstatusNomina
                SQL &= ",0"
                'iEstatus
                SQL &= ",1"
                If nExecute(SQL) = False Then
                    MessageBox.Show("Ocurrio un error " & rwDatosEmpleado(0)("cNombreLargo"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'pnlProgreso.Visible = False
                    Exit Sub
                End If
            End If
            MessageBox.Show("Se guardo Finiquito Correctamente ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Function CalcularEdad(ByVal DiaNacimiento As Integer, ByVal MesNacimiento As Integer, ByVal AñoNacimiento As Integer)
        ' SE DEFINEN LAS FECHAS ACTUALES
        Dim AñoActual As Integer = Year(Now)
        Dim MesActual As Integer = Month(Now)
        Dim DiaActual As Integer = Now.Day
        Dim Cumplidos As Boolean = False
        ' SE COMPRUEBA CUANDO FUE EL ULTIMOS CUMPLEAÑOS
        ' FORMULA:
        '   Años cumplidos = (Año del ultimo cumpleaños - Año de nacimiento)
        If (MesNacimiento <= MesActual) Then
            If (DiaNacimiento <= DiaActual) Then
                If (DiaNacimiento = DiaActual And MesNacimiento = MesActual) Then
                    'MsgBox("Feliz Cumpleaños!")
                End If
                ' MsgBox("Ya cumplio")
                Cumplidos = True
            End If
        End If

        If (Cumplidos = False) Then
            AñoActual = (AñoActual - 1)
            'MsgBox("Ultimo cumpleaños: " & AñoActual)
        End If
        ' Se realiza la resta de años para definir los años cumplidos
        Dim EdadAños As Integer = (AñoActual - AñoNacimiento)
        ' DEFINICION DE LOS MESES LUEGO DEL ULTIMO CUMPLEAÑOS
        Dim EdadMes As Integer
        If Not (AñoActual = Now.Year) Then
            EdadMes = (12 - MesNacimiento)
            EdadMes = EdadMes + Now.Month
        Else
            EdadMes = Math.Abs(Now.Month - MesNacimiento)
        End If
        'SACAMOS LA CANTIDAD DE DIAS EXACTOS
        Dim EdadDia As Integer = (DiaActual - DiaNacimiento)

        'RETORNAMOS LOS VALORES EN UNA CADENA STRING
        Return (EdadAños)


    End Function

    Sub sumaSindicato()
        Try
            Dim totalsindicato As String

            totalsindicato += CDbl(txtindeminizacionExce.Text.ToString)
            totalsindicato += CDbl(txtAntiguedadExce.Text.ToString)
            totalsindicato += CDbl(txtSueldoExce.Text.ToString)
            totalsindicato += CDbl(txtAguinaldoExce.Text.ToString)
            totalsindicato += CDbl(txtVacacionesPropExce.Text.ToString)
            totalsindicato += CDbl(txtPrimaVacacionalExce.Text.ToString)
            totalsindicato += CDbl(txtVacacionesPendientesExce.Text.ToString)
            totalsindicato += CDbl(txtHE2Exce.Text.ToString)
            totalsindicato += CDbl(txtHE3Exce.Text.ToString)
            totalsindicato += CDbl(txtDLExce.Text.ToString)
            totalsindicato += CDbl(txtBonosExce.Text.ToString)
            totalsindicato -= CDbl(txtIncidenciasExce.Text.ToString)
            totalsindicato -= CDbl(txtInfonavitExce.Text.ToString)


            txtSindicato.Text = totalsindicato
        Catch ex As Exception

        End Try


          
    End Sub
    Sub sumarstp()
        Try
            Dim totalstp As String

            totalstp += CDbl(txtIndeminizacion.Text.ToString)
            totalstp += CDbl(txtPrimaAntiguedad.Text.ToString)
            totalstp += CDbl(txtSueldo.Text.ToString)
            totalstp += CDbl(txtAguinaldo.Text.ToString)
            totalstp += CDbl(txtVacacionesProp.Text.ToString)
            totalstp += CDbl(txtPrimaVacacional.Text.ToString)
            totalstp += CDbl(txtVacacionesPendientes.Text.ToString)
            totalstp += CDbl(txtHE2.Text.ToString)
            totalstp += CDbl(txtHE3.Text.ToString)
            totalstp += CDbl(txtDL.Text.ToString)
            totalstp += CDbl(txtBonos.Text.ToString)
            totalstp -= CDbl(txtIncidencias.Text.ToString)
            totalstp -= CDbl(txtInfonavit.Text.ToString)
            totalstp += CDbl(txtFonacot.Text.ToString)
            'totalstp += CDbl(txtVales.Text.ToString)
            totalstp -= CDbl(txtISR.Text.ToString)
            totalstp -= CDbl(txtISRIndemnizacion.Text.ToString)

            txtSTP.Text = totalstp


        Catch ex As Exception

        End Try



    End Sub
   
    Private Sub txtIndeminizacion_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtIndeminizacion.KeyUp
        sumarstp()
    End Sub
    Private Sub txtPrimaAntiguedad_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPrimaAntiguedad.KeyUp
        sumarstp()
    End Sub

    Private Sub txtSueldo_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtSueldo.KeyUp
        sumarstp()
    End Sub

    Private Sub txtAguinaldo_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAguinaldo.KeyUp
        sumarstp()
    End Sub

    Private Sub txtVacacionesProp_KeyUp(sender As System.Object, e As System.EventArgs) Handles txtVacacionesProp.KeyUp
        sumarstp()
    End Sub

    Private Sub txtPrimaVacacional_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPrimaVacacional.KeyUp
        sumarstp()
    End Sub

    Private Sub txtVacacionesPendientes_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVacacionesPendientes.KeyUp
        sumarstp()
    End Sub

    Private Sub txtHE2_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtHE2.KeyUp
        sumarstp()
    End Sub

    Private Sub txtHE3_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtHE3.KeyUp
        sumarstp()
    End Sub

    Private Sub txtDL_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtDL.KeyUp
        sumarstp()
    End Sub

    Private Sub txtBonos_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtBonos.KeyUp
        sumarstp()
    End Sub
    Private Sub txtIncidencias_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtIncidencias.KeyUp
        sumarstp()
    End Sub

    Private Sub txtInfonavit_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtInfonavit.KeyUp
        sumarstp()
    End Sub

    Private Sub txtFonacot_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFonacot.KeyUp
        sumarstp()
    End Sub
    Private Sub txtISR_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtISR.KeyUp
        sumarstp()
    End Sub

    Private Sub txtISRIndemnizacion_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtISRIndemnizacion.KeyUp
        sumarstp()
    End Sub
    Private Sub txtSTP_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtSTP.KeyUp
        sumarstp()
    End Sub




    Private Sub txtindeminizacionExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtindeminizacionExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtAntiguedadExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAntiguedadExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtSueldoExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtSueldoExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtAguinaldoExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAguinaldoExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtVacacionesPropExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVacacionesPropExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtPrimaVacacionalExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPrimaVacacionalExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtVacacionesPendientesExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVacacionesPendientesExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtHE2Exce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtHE2Exce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtHE3Exce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtHE3Exce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtDLExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtDLExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtBonosExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtBonosExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtIncidenciasExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtIncidenciasExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtInfonavitExce_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtInfonavitExce.KeyUp
        sumaSindicato()
    End Sub

    Private Sub txtVales_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVales.KeyUp
        sumaSindicato()
    End Sub


    Private Sub btnAgregar_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregar.Click

        Try
            lsvLista.Items.Clear()
            Dim item As New ListViewItem
            lsvLista.Columns.Add("Id")
            lsvLista.Columns.Add("Empleado")
            lsvLista.Columns.Add("Periodo")
            lsvLista.Columns.Add("Serie")
            lsvLista.Columns.Add("Indeminizacion")
            lsvLista.Columns.Add("PrimaAntiguedad")
            lsvLista.Columns.Add("Sueldo")
            lsvLista.Columns.Add("Aguinaldo")
            lsvLista.Columns.Add("VacacionesProp")
            lsvLista.Columns.Add("PrimaVacacional")
            lsvLista.Columns.Add("VacacionesPendientes")
            lsvLista.Columns.Add("HE2")
            lsvLista.Columns.Add("HE3")
            lsvLista.Columns.Add("DL")
            lsvLista.Columns.Add("Bonos")
            lsvLista.Columns.Add("Incidencias")
            lsvLista.Columns.Add("Infonavit")
            lsvLista.Columns.Add("Fonacot")
            lsvLista.Columns.Add("Vales")
            lsvLista.Columns.Add("ISR")
            lsvLista.Columns.Add("ISRIndemnizacion")
            lsvLista.Columns.Add("indeminizacionExce")
            lsvLista.Columns.Add("AntiguedadExce")
            lsvLista.Columns.Add("SueldoExce")
            lsvLista.Columns.Add("AguinaldoExce")
            lsvLista.Columns.Add("VacacionesPropExce")
            lsvLista.Columns.Add("PrimaVacacionalExce")
            lsvLista.Columns.Add("VacacionesPendientesExce")
            lsvLista.Columns.Add("HE2Exce")
            lsvLista.Columns.Add("HE3Exce")
            lsvLista.Columns.Add("DLExce")
            lsvLista.Columns.Add("BonosExce")
            lsvLista.Columns.Add("IncidenciasExce")
            lsvLista.Columns.Add("InfonavitExce")

            SQL = "select  * from empleadosC where "
            SQL &= "iIdEmpleadoC=" & cboEmpleado.SelectedIndex + 1

            Dim rwDatosEmpleado As DataRow() = nConsulta(SQL)
            If rwDatosEmpleado Is Nothing = False Then

                item = lsvLista.Items.Add(cboEmpleado.SelectedIndex + 1)
                item.SubItems.Add(cboEmpleado.SelectedIndex + 1)
                item.SubItems.Add(rwDatosEmpleado(0)("cNombreLargo"))
                item.SubItems.Add(cboEmpleado.SelectedText)
                item.SubItems.Add(cboSerie.SelectedIndex)
                item.SubItems.Add(txtSTP.Text)
                item.SubItems.Add(txtSindicato.Text)

            End If

            

            If lsvLista.Items.Count > 0 Then
                lsvLista.Focus()
                lsvLista.Items(0).Selected = True
            Else
                txtIndeminizacion.Focus()
                txtIndeminizacion.SelectAll()
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub cargarFiniqutos()
        
    End Sub

   
    Private Sub cboPeriodo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboPeriodo.SelectedIndexChanged

    End Sub
End Class