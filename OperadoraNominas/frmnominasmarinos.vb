Imports ClosedXML.Excel
Imports System.IO
Imports System.Xml

Public Class frmnominasmarinos
    Private m_currentControl As Control = Nothing
    Public gIdEmpresa As String
    Public EmpresaN As String
    Public gIdTipoPeriodo As String
    Public gNombrePeriodo As String
    Public gTipoCalculo As String

    Dim Ruta As String
    Dim nombre As String
    Dim cargado As Boolean = False
    Dim diasperiodo As Integer
    Dim aniocostosocial As Integer
    Dim dgvCombo As DataGridViewComboBoxEditingControl
    Dim campoordenamiento As String
    Dim TipoNomina As Boolean
    Dim IDCalculoInfonavit As Integer
    Dim FechaInicioPeriodoGlobal As Date
    Dim dsPeriodo As New DataSet
    Dim dsPeriodo2 As New DataSet
    Dim Agregartrabajadores As Boolean
    Dim NombrePeriodo As String


    


    Private Sub dvgCombo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            '
            ' se recupera el valor del combo
            ' a modo de ejemplo se escribe en consola el valor seleccionado
            '



            Dim combo As ComboBox = TryCast(sender, ComboBox)

            If dgvCombo IsNot Nothing Then
                Dim sql As String
                'Console.WriteLine(combo.SelectedValue)
                'MessageBox.Show(combo.Text)
                '
                ' se accede a la fila actual, para trabajr con otor de sus campos
                ' en este caso se marca el check si se cambia la seleccion
                '
                Dim row As DataGridViewRow = dtgDatos.CurrentRow

                'Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells("Seleccionado"), DataGridViewCheckBoxCell)
                'cell.Value = True

                'Poner los datos necesarios para poner el nuevo sueldo diario y el integrado


                sql = "Select salariod,sbc,salariodTopado,sbcTopado from costosocial "
                sql &= " where fkiIdPuesto = " & combo.SelectedValue & " and anio=" & aniocostosocial

                Dim rwDatosSalario As DataRow() = nConsulta(sql)

                If rwDatosSalario Is Nothing = False Then
                    If row.Cells(10).Value >= 55 Then
                        row.Cells(16).Value = rwDatosSalario(0)("salariodTopado")
                        row.Cells(17).Value = rwDatosSalario(0)("sbcTopado")
                    Else
                        row.Cells(16).Value = rwDatosSalario(0)("salariod")
                        row.Cells(17).Value = rwDatosSalario(0)("sbc")
                    End If

                Else
                    MessageBox.Show("No se encontraron datos")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub
   
    Private Sub frmcontpaqnominas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim sql As String
            Dim frm As Form = Me
            frm.Text = "Nominas " '& Servidor.Base.ToString.Substring(0, 9)
            cargarperiodos()
            Me.dtgDatos.ContextMenuStrip = Me.cMenu
            cboserie.SelectedIndex = 0

            If gTipoCalculo = "1" Then
                btnReporte.Enabled = False
                cmdBuscarOtraNom.Enabled = False
            Else
                cmdcalcular.Enabled = False
                cmdexcel.Enabled = False

            End If

            sql = "select * from periodos inner join tipos_periodos2 on periodos.fkiIdTipoPeriodo = tipos_periodos2.iIdTipoperiodo2     where iIdPeriodo=" & cboperiodo.SelectedValue
            Dim rwPeriodo As DataRow() = nConsulta(sql)
            If rwPeriodo Is Nothing = False Then

                aniocostosocial = Date.Parse(rwPeriodo(0)("dFechaInicio").ToString).Year
                diasperiodo = Integer.Parse(rwPeriodo(0)("iDiasPago").ToString)
                NombrePeriodo = rwPeriodo(0)("nombre").ToString

            End If
            Agregartrabajadores = False
            campoordenamiento = "cCodigoEmpleado"
            TipoNomina = False
            Me.KeyPreview = True
            gIdEmpresa = 1
            dtgDatos.Columns.Clear()
            llenargridinicial()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub

    Private Sub cargarbancosasociados()
        Dim sql As String
        Try
            sql = "select * from bancos inner join ( select distinct(fkiidBanco) from DatosBanco where fkiIdEmpresa=" & gIdEmpresa & ") bancos2 on bancos.iIdBanco=bancos2.fkiidBanco order by cBanco"
            nCargaCBO(cbobancos, sql, "cBanco", "iIdBanco")
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

    
    

    Private Sub cmdverdatos_Click(sender As Object, e As EventArgs) Handles cmdverdatos.Click
        Try
            'If cargado Then

            '    dtgDatos.DataSource = Nothing
            '    llenargrid()
            'Else
            '    cargado = True
            '    llenargrid()
            'End If
            If dtgDatos.RowCount > 0 Then
                Dim resultado As Integer = MessageBox.Show("ya se tienen empleados cargados en la lista, si continua estos se borraran,¿Desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
                If resultado = DialogResult.Yes Then

                    dtgDatos.Columns.Clear()
                    llenargrid()

                End If
            Else
                dtgDatos.Columns.Clear()
                llenargrid()

            End If
            



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub MATGRID()
        dtgDatos.Columns(0).Width = 30
        dtgDatos.Columns(0).ReadOnly = True
        dtgDatos.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(0).Frozen = True
        'consecutivo
        dtgDatos.Columns(1).Width = 60
        dtgDatos.Columns(1).ReadOnly = True
        dtgDatos.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(1).Frozen = True
        'idempleado
        dtgDatos.Columns(2).Width = 100
        dtgDatos.Columns(2).ReadOnly = True
        dtgDatos.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(2).Frozen = True
        'codigo empleado
        dtgDatos.Columns(3).Width = 100
        dtgDatos.Columns(3).ReadOnly = True
        dtgDatos.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(3).Frozen = True
        'Nombre
        dtgDatos.Columns(4).Width = 250
        dtgDatos.Columns(4).ReadOnly = True
        dtgDatos.Columns(4).Frozen = True
        'Estatus
        dtgDatos.Columns(5).Width = 100
        dtgDatos.Columns(5).ReadOnly = True
        'RFC
        dtgDatos.Columns(6).Width = 100
        dtgDatos.Columns(6).ReadOnly = True
        'dtgDatos.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'CURP
        dtgDatos.Columns(7).Width = 150
        dtgDatos.Columns(7).ReadOnly = True
        'IMSS 

        dtgDatos.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(8).ReadOnly = True
        'Fecha_Nac
        dtgDatos.Columns(9).Width = 150
        dtgDatos.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(9).ReadOnly = True

        'Edad
        dtgDatos.Columns(10).ReadOnly = True
        dtgDatos.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'Puesto
        dtgDatos.Columns(11).ReadOnly = True
        dtgDatos.Columns(11).Width = 200
        dtgDatos.Columns.Remove("Puesto")

        Dim combo As New DataGridViewComboBoxColumn

        sql = "select * from puestos where iTipo=1 order by cNombre"

        'Dim rwPuestos As DataRow() = nConsulta(sql)
        'If rwPuestos Is Nothing = False Then
        '    combo.Items.Add("uno")
        '    combo.Items.Add("dos")
        '    combo.Items.Add("tres")
        'End If

        nCargaCBO(combo, sql, "cNombre", "iIdPuesto")

        combo.HeaderText = "Puesto"

        combo.Width = 150
        dtgDatos.Columns.Insert(11, combo)




        'DirectCast(dtgDatos.Columns(11), DataGridViewComboBoxColumn).Sorted = True
        'Dim combo2 As New DataGridViewComboBoxCell
        'combo2 = CType(Me.dtgDatos.Rows(2).Cells(11), DataGridViewComboBoxCell)
        'combo2.Value = combo.Items(11)



        'dtgDatos.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'Buque
        'dtgDatos.Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(12).ReadOnly = True
        dtgDatos.Columns(12).Width = 150
        dtgDatos.Columns.Remove("Depto")

        Dim combo2 As New DataGridViewComboBoxColumn

        sql = "select * from departamentos where iEstatus=1 order by cNombre"

        'Dim rwPuestos As DataRow() = nConsulta(sql)
        'If rwPuestos Is Nothing = False Then
        '    combo.Items.Add("uno")
        '    combo.Items.Add("dos")
        '    combo.Items.Add("tres")
        'End If

        nCargaCBO(combo2, sql, "cNombre", "iIdDepartamento")

        combo2.HeaderText = "Depto"
        combo2.Width = 150
        dtgDatos.Columns.Insert(12, combo2)

        'Tipo_Infonavit
        dtgDatos.Columns(13).ReadOnly = True
        dtgDatos.Columns(13).Width = 150
        'dtgDatos.Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight



        'Valor_Infonavit
        dtgDatos.Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(14).ReadOnly = True
        dtgDatos.Columns(14).Width = 150
        'Horas_extras_dobles_V
        dtgDatos.Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(15).ReadOnly = True
        dtgDatos.Columns(15).Width = 150
        'Horas_extras_triples_V
        dtgDatos.Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(16).ReadOnly = True
        dtgDatos.Columns(16).Width = 150
        'Descanso_Laborado_V
        dtgDatos.Columns(17).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(17).ReadOnly = True
        dtgDatos.Columns(17).Width = 150
        'Dia_Festivo_laborado_V
        dtgDatos.Columns(18).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(18).Width = 150
        'Prima_Dominical_V
        dtgDatos.Columns(19).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(19).ReadOnly = True
        dtgDatos.Columns(19).Width = 150
        'Falta_Injustificada_V
        dtgDatos.Columns(20).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(20).ReadOnly = True
        dtgDatos.Columns(20).Width = 150
        'Permiso_Sin_GS_V
        dtgDatos.Columns(21).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(21).ReadOnly = True
        dtgDatos.Columns(21).Width = 150
        'T_No_laborado_V
        dtgDatos.Columns(22).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(22).ReadOnly = True
        dtgDatos.Columns(22).Width = 150

        'Sueldo_Base
        dtgDatos.Columns(23).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(23).ReadOnly = True
        dtgDatos.Columns(23).Width = 150

        'Salario_Diario
        dtgDatos.Columns(24).Width = 150
        'dtgDatos.Columns(24).ReadOnly = True
        dtgDatos.Columns(24).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'Salario_Cotización
        dtgDatos.Columns(25).Width = 150
        'dtgDatos.Columns(25).ReadOnly = True
        dtgDatos.Columns(25).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'Dias_Trabajados
        dtgDatos.Columns(26).Width = 150
        'dtgDatos.Columns(26).ReadOnly = True
        dtgDatos.Columns(26).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'Tipo_Incapacidad
        dtgDatos.Columns(27).Width = 150
        'dtgDatos.Columns(27).ReadOnly = True
        dtgDatos.Columns(27).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'Número_días
        dtgDatos.Columns(28).Width = 150
        ' dtgDatos.Columns(28).ReadOnly = True
        dtgDatos.Columns(28).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'Sueldo_Bruto
        dtgDatos.Columns(29).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(29).Width = 150
        dtgDatos.Columns(29).ReadOnly = True

        'Septimo_Dia
        dtgDatos.Columns(30).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(30).ReadOnly = True
        dtgDatos.Columns(30).Width = 150
        'Prima_Dominical_Gravada
        dtgDatos.Columns(31).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(31).ReadOnly = True
        dtgDatos.Columns(31).Width = 150

        'Prima_Dominical_Exenta
        dtgDatos.Columns(32).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(32).ReadOnly = True
        dtgDatos.Columns(32).Width = 150


        'Tiempo_Extra_Doble_Gravado
        dtgDatos.Columns(33).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(33).ReadOnly = True
        dtgDatos.Columns(33).Width = 150
        'Tiempo_Extra_Doble_Exento
        dtgDatos.Columns(34).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(34).ReadOnly = True
        dtgDatos.Columns(34).Width = 150

        'Tiempo_Extra_Triple
        dtgDatos.Columns(35).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(35).ReadOnly = True
        dtgDatos.Columns(35).Width = 150

        'Descanso_Labarado
        dtgDatos.Columns(36).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(36).ReadOnly = True
        dtgDatos.Columns(36).Width = 150


        'Dia_Festivo_laborado
        dtgDatos.Columns(37).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(37).ReadOnly = True
        dtgDatos.Columns(37).Width = 150

        'Bono_Asistencia
        dtgDatos.Columns(38).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(38).ReadOnly = True
        dtgDatos.Columns(38).Width = 150
        'Bono_Productividad
        dtgDatos.Columns(39).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(39).ReadOnly = True
        dtgDatos.Columns(39).Width = 150
        'Bono_Polivalencia
        dtgDatos.Columns(40).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(40).ReadOnly = True
        dtgDatos.Columns(40).Width = 150
        'Bono_Especialidad
        dtgDatos.Columns(41).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(41).ReadOnly = True
        dtgDatos.Columns(41).Width = 150
        'Bono_Calidad
        dtgDatos.Columns(42).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(42).ReadOnly = True
        dtgDatos.Columns(42).Width = 150
        'Compensacion
        dtgDatos.Columns(43).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(43).ReadOnly = True
        dtgDatos.Columns(43).Width = 150
        'Semana_fondo
        dtgDatos.Columns(44).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(44).ReadOnly = True
        dtgDatos.Columns(44).Width = 150
        'Falta_Injustificada
        dtgDatos.Columns(45).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(45).ReadOnly = True
        dtgDatos.Columns(45).Width = 150
        'Permiso_Sin_GS
        dtgDatos.Columns(46).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(46).ReadOnly = True
        dtgDatos.Columns(46).Width = 150

        'Incremento_Retenido
        dtgDatos.Columns(47).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(48).ReadOnly = True
        dtgDatos.Columns(47).Width = 150

        'Vacaciones_proporcionales
        dtgDatos.Columns(48).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(49).ReadOnly = True
        dtgDatos.Columns(48).Width = 150

        '
        dtgDatos.Columns(49).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(50).ReadOnly = True
        dtgDatos.Columns(49).Width = 150

        '
        dtgDatos.Columns(50).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(50).ReadOnly = True
        dtgDatos.Columns(50).Width = 150

        '
        dtgDatos.Columns(51).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(51).ReadOnly = True
        dtgDatos.Columns(51).Width = 150

        '
        dtgDatos.Columns(52).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(52).ReadOnly = True
        dtgDatos.Columns(52).Width = 150

        '
        dtgDatos.Columns(53).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(53).ReadOnly = True
        dtgDatos.Columns(53).Width = 150

        '
        dtgDatos.Columns(54).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(54).ReadOnly = True
        dtgDatos.Columns(54).Width = 150

        '
        dtgDatos.Columns(55).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(55).ReadOnly = True
        dtgDatos.Columns(55).Width = 150

        '
        dtgDatos.Columns(56).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(56).ReadOnly = True
        dtgDatos.Columns(56).Width = 150

        '
        dtgDatos.Columns(57).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(57).ReadOnly = True
        dtgDatos.Columns(57).Width = 150

        '
        dtgDatos.Columns(58).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(58).ReadOnly = True
        dtgDatos.Columns(58).Width = 150

        '
        dtgDatos.Columns(59).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(59).ReadOnly = True
        dtgDatos.Columns(59).Width = 150

        '
        dtgDatos.Columns(60).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(60).ReadOnly = True
        dtgDatos.Columns(60).Width = 150

        'Infonavit_bim_anterior
        dtgDatos.Columns(61).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(61).ReadOnly = True
        dtgDatos.Columns(61).Width = 150

        'Ajuste_infonavit
        dtgDatos.Columns(62).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(62).ReadOnly = True
        dtgDatos.Columns(62).Width = 150

        'Pension alimenticia
        dtgDatos.Columns(63).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dtgDatos.Columns(63).ReadOnly = True
        dtgDatos.Columns(63).Width = 150

        'Prestamo
        dtgDatos.Columns(64).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(64).ReadOnly = True
        dtgDatos.Columns(64).Width = 150

        'Fonacot
        dtgDatos.Columns(65).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(65).ReadOnly = True
        dtgDatos.Columns(65).Width = 150

        'T_No_laborado
        dtgDatos.Columns(66).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(66).ReadOnly = True
        dtgDatos.Columns(66).Width = 150

        'Cuota_Sindical
        dtgDatos.Columns(67).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(67).ReadOnly = True
        dtgDatos.Columns(67).Width = 150

        '
        dtgDatos.Columns(68).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(68).ReadOnly = True
        dtgDatos.Columns(68).Width = 150

        '
        dtgDatos.Columns(69).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(69).ReadOnly = True
        dtgDatos.Columns(69).Width = 150

        '
        dtgDatos.Columns(70).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(70).ReadOnly = True
        dtgDatos.Columns(70).Width = 150

        '
        dtgDatos.Columns(71).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(71).ReadOnly = True
        dtgDatos.Columns(71).Width = 150

        '
        dtgDatos.Columns(72).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'dtgDatos.Columns(72).ReadOnly = True
        dtgDatos.Columns(72).Width = 150

        'calcular()

        'Cambiamos index del combo en el grid

        For x As Integer = 0 To dtgDatos.Rows.Count - 1

            sql = "select iIdEmpleadoC,iIdDepartamento,departamentos.cNombre as cDepto ,iIdPuesto,Puestos.cnombre as cPuesto from empleadosC"
            sql &= " inner join Puestos on     empleadosc.fkiIdPuesto =Puestos.iIdPuesto"
            sql &= " inner join departamentos on empleadosc.fkiIdDepartamento = departamentos.iIdDepartamento"

            sql &= " where iIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value

            Dim rwFila As DataRow() = nConsulta(sql)



            CType(Me.dtgDatos.Rows(x).Cells(11), DataGridViewComboBoxCell).Value = rwFila(0)("cPuesto").ToString()
            CType(Me.dtgDatos.Rows(x).Cells(12), DataGridViewComboBoxCell).Value = rwFila(0)("cDepto").ToString()
        Next


    End Sub
    Private Sub DSP()
       

        dsPeriodo.Tables.Add("Tabla")
        dsPeriodo.Tables("Tabla").Columns.Add("Consecutivo")
        dsPeriodo.Tables("Tabla").Columns.Add("Id_empleado")
        dsPeriodo.Tables("Tabla").Columns.Add("CodigoEmpleado")
        dsPeriodo.Tables("Tabla").Columns.Add("Nombre")
        dsPeriodo.Tables("Tabla").Columns.Add("Status")
        dsPeriodo.Tables("Tabla").Columns.Add("RFC")
        dsPeriodo.Tables("Tabla").Columns.Add("CURP")
        dsPeriodo.Tables("Tabla").Columns.Add("Num_IMSS")
        dsPeriodo.Tables("Tabla").Columns.Add("Fecha_Nac")
        dsPeriodo.Tables("Tabla").Columns.Add("Edad")
        dsPeriodo.Tables("Tabla").Columns.Add("Puesto")
        dsPeriodo.Tables("Tabla").Columns.Add("Depto")
        dsPeriodo.Tables("Tabla").Columns.Add("Tipo_Infonavit")
        dsPeriodo.Tables("Tabla").Columns.Add("Valor_Infonavit")

        dsPeriodo.Tables("Tabla").Columns.Add("Horas_extras_dobles_V")
        dsPeriodo.Tables("Tabla").Columns.Add("Horas_extras_triples_V")
        dsPeriodo.Tables("Tabla").Columns.Add("Descanso_Laborado_V")
        dsPeriodo.Tables("Tabla").Columns.Add("Dia_Festivo_laborado_V")

        dsPeriodo.Tables("Tabla").Columns.Add("Prima_Dominical_V")
        dsPeriodo.Tables("Tabla").Columns.Add("Falta_Injustificada_V")
        dsPeriodo.Tables("Tabla").Columns.Add("Permiso_Sin_GS_V")
        dsPeriodo.Tables("Tabla").Columns.Add("T_No_laborado_V")


        dsPeriodo.Tables("Tabla").Columns.Add("Sueldo_Base")
        dsPeriodo.Tables("Tabla").Columns.Add("Salario_Diario")
        dsPeriodo.Tables("Tabla").Columns.Add("Salario_Cotización")
        dsPeriodo.Tables("Tabla").Columns.Add("Dias_Trabajados")
        dsPeriodo.Tables("Tabla").Columns.Add("Tipo_Incapacidad")
        dsPeriodo.Tables("Tabla").Columns.Add("Número_días")
        dsPeriodo.Tables("Tabla").Columns.Add("Sueldo_Bruto")
        dsPeriodo.Tables("Tabla").Columns.Add("Septimo_Dia")
        dsPeriodo.Tables("Tabla").Columns.Add("Prima_Dominical_Gravada")
        dsPeriodo.Tables("Tabla").Columns.Add("Prima_Dominical_Exenta")
        dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Doble_Gravado")
        dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Doble_Exento")
        dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Triple")

        dsPeriodo.Tables("Tabla").Columns.Add("Descanso_Labarado")
        dsPeriodo.Tables("Tabla").Columns.Add("Dia_Festivo_laborado")

        dsPeriodo.Tables("Tabla").Columns.Add("Bono_Asistencia")
        dsPeriodo.Tables("Tabla").Columns.Add("Bono_Productividad")
        dsPeriodo.Tables("Tabla").Columns.Add("Bono_Polivalencia")
        dsPeriodo.Tables("Tabla").Columns.Add("Bono_Especialidad")
        dsPeriodo.Tables("Tabla").Columns.Add("Bono_Calidad")

        dsPeriodo.Tables("Tabla").Columns.Add("Compensacion")
        dsPeriodo.Tables("Tabla").Columns.Add("Semana_fondo")
        dsPeriodo.Tables("Tabla").Columns.Add("Falta_Injustificada")
        dsPeriodo.Tables("Tabla").Columns.Add("Permiso_Sin_GS")

        dsPeriodo.Tables("Tabla").Columns.Add("Incremento_Retenido")
        dsPeriodo.Tables("Tabla").Columns.Add("Vacaciones_proporcionales")
        dsPeriodo.Tables("Tabla").Columns.Add("Aguinaldo_gravado")
        dsPeriodo.Tables("Tabla").Columns.Add("Aguinaldo_exento")
        dsPeriodo.Tables("Tabla").Columns.Add("Total_Aguinaldo")
        dsPeriodo.Tables("Tabla").Columns.Add("Prima_vac_gravado")
        dsPeriodo.Tables("Tabla").Columns.Add("Prima_vac_exento")

        dsPeriodo.Tables("Tabla").Columns.Add("Total_Prima_vac")
        dsPeriodo.Tables("Tabla").Columns.Add("Total_percepciones")
        dsPeriodo.Tables("Tabla").Columns.Add("Total_percepciones_p/isr")
        dsPeriodo.Tables("Tabla").Columns.Add("Incapacidad")
        dsPeriodo.Tables("Tabla").Columns.Add("ISR")
        dsPeriodo.Tables("Tabla").Columns.Add("IMSS")
        dsPeriodo.Tables("Tabla").Columns.Add("Infonavit")
        dsPeriodo.Tables("Tabla").Columns.Add("Infonavit_bim_anterior")
        dsPeriodo.Tables("Tabla").Columns.Add("Ajuste_infonavit")
        dsPeriodo.Tables("Tabla").Columns.Add("Pension_Alimenticia")
        dsPeriodo.Tables("Tabla").Columns.Add("Prestamo")
        dsPeriodo.Tables("Tabla").Columns.Add("Fonacot")
        dsPeriodo.Tables("Tabla").Columns.Add("T_No_laborado")
        dsPeriodo.Tables("Tabla").Columns.Add("Cuota_Sindical")
        dsPeriodo.Tables("Tabla").Columns.Add("Subsidio_Generado")
        dsPeriodo.Tables("Tabla").Columns.Add("Subsidio_Aplicado")
        dsPeriodo.Tables("Tabla").Columns.Add("Neto_SA")
        dsPeriodo.Tables("Tabla").Columns.Add("Prestamo_Personal_A")
        dsPeriodo.Tables("Tabla").Columns.Add("Adeudo_Infonavit_A")
        'dsPeriodo.Tables("Tabla").Columns.Add("Diferencia_Infonavit_A")
        dsPeriodo.Tables("Tabla").Columns.Add("PA_A")
        dsPeriodo.Tables("Tabla").Columns.Add("SINDICATO/PPP")
        dsPeriodo.Tables("Tabla").Columns.Add("PRIMA_EXCEN")
        dsPeriodo.Tables("Tabla").Columns.Add("%_Comisión")
        dsPeriodo.Tables("Tabla").Columns.Add("Comisión_SA")
        dsPeriodo.Tables("Tabla").Columns.Add("Comisión_Beneficio")

        dsPeriodo.Tables("Tabla").Columns.Add("IMSS_CS")
        dsPeriodo.Tables("Tabla").Columns.Add("RCV_CS")
        dsPeriodo.Tables("Tabla").Columns.Add("Infonavit_CS")
        dsPeriodo.Tables("Tabla").Columns.Add("ISN_CS")
        dsPeriodo.Tables("Tabla").Columns.Add("Total_Costo_Social")
        dsPeriodo.Tables("Tabla").Columns.Add("Subtotal")
        dsPeriodo.Tables("Tabla").Columns.Add("IVA")
        dsPeriodo.Tables("Tabla").Columns.Add("TOTAL_DEPOSITO")
        dsPeriodo.Tables("Tabla").Columns.Add("fecha_inicio")
        dsPeriodo.Tables("Tabla").Columns.Add("fecha_fin")

        dsPeriodo2.Tables.Add("Tabla")
        dsPeriodo2.Tables("Tabla").Columns.Add("Consecutivo")
        dsPeriodo2.Tables("Tabla").Columns.Add("Id_empleado")
        dsPeriodo2.Tables("Tabla").Columns.Add("CodigoEmpleado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Nombre")
        dsPeriodo2.Tables("Tabla").Columns.Add("Status")
        dsPeriodo2.Tables("Tabla").Columns.Add("RFC")
        dsPeriodo2.Tables("Tabla").Columns.Add("CURP")
        dsPeriodo2.Tables("Tabla").Columns.Add("Num_IMSS")
        dsPeriodo2.Tables("Tabla").Columns.Add("Fecha_Nac")
        dsPeriodo2.Tables("Tabla").Columns.Add("Edad")
        dsPeriodo2.Tables("Tabla").Columns.Add("Puesto")
        dsPeriodo2.Tables("Tabla").Columns.Add("Depto")
        dsPeriodo2.Tables("Tabla").Columns.Add("Tipo_Infonavit")
        dsPeriodo2.Tables("Tabla").Columns.Add("Valor_Infonavit")

        dsPeriodo2.Tables("Tabla").Columns.Add("Horas_extras_dobles_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("Horas_extras_triples_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("Descanso_Laborado_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("Dia_Festivo_laborado_V")

        dsPeriodo2.Tables("Tabla").Columns.Add("Prima_Dominical_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("Falta_Injustificada_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("Permiso_Sin_GS_V")
        dsPeriodo2.Tables("Tabla").Columns.Add("T_No_laborado_V")


        dsPeriodo2.Tables("Tabla").Columns.Add("Sueldo_Base")
        dsPeriodo2.Tables("Tabla").Columns.Add("Salario_Diario")
        dsPeriodo2.Tables("Tabla").Columns.Add("Salario_Cotización")
        dsPeriodo2.Tables("Tabla").Columns.Add("Dias_Trabajados")
        dsPeriodo2.Tables("Tabla").Columns.Add("Tipo_Incapacidad")
        dsPeriodo2.Tables("Tabla").Columns.Add("Número_días")
        dsPeriodo2.Tables("Tabla").Columns.Add("Sueldo_Bruto")
        dsPeriodo2.Tables("Tabla").Columns.Add("Septimo_Dia")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prima_Dominical_Gravada")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prima_Dominical_Exenta")
        dsPeriodo2.Tables("Tabla").Columns.Add("Tiempo_Extra_Doble_Gravado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Tiempo_Extra_Doble_Exento")
        dsPeriodo2.Tables("Tabla").Columns.Add("Tiempo_Extra_Triple")

        dsPeriodo2.Tables("Tabla").Columns.Add("Descanso_Labarado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Dia_Festivo_laborado")

        dsPeriodo2.Tables("Tabla").Columns.Add("Bono_Asistencia")
        dsPeriodo2.Tables("Tabla").Columns.Add("Bono_Productividad")
        dsPeriodo2.Tables("Tabla").Columns.Add("Bono_Polivalencia")
        dsPeriodo2.Tables("Tabla").Columns.Add("Bono_Especialidad")
        dsPeriodo2.Tables("Tabla").Columns.Add("Bono_Calidad")

        dsPeriodo2.Tables("Tabla").Columns.Add("Compensacion")
        dsPeriodo2.Tables("Tabla").Columns.Add("Semana_fondo")
        dsPeriodo2.Tables("Tabla").Columns.Add("Falta_Injustificada")
        dsPeriodo2.Tables("Tabla").Columns.Add("Permiso_Sin_GS")

        dsPeriodo2.Tables("Tabla").Columns.Add("Incremento_Retenido")
        dsPeriodo2.Tables("Tabla").Columns.Add("Vacaciones_proporcionales")
        dsPeriodo2.Tables("Tabla").Columns.Add("Aguinaldo_gravado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Aguinaldo_exento")
        dsPeriodo2.Tables("Tabla").Columns.Add("Total_Aguinaldo")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prima_vac_gravado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prima_vac_exento")

        dsPeriodo2.Tables("Tabla").Columns.Add("Total_Prima_vac")
        dsPeriodo2.Tables("Tabla").Columns.Add("Total_percepciones")
        dsPeriodo2.Tables("Tabla").Columns.Add("Total_percepciones_p/isr")
        dsPeriodo2.Tables("Tabla").Columns.Add("Incapacidad")
        dsPeriodo2.Tables("Tabla").Columns.Add("ISR")
        dsPeriodo2.Tables("Tabla").Columns.Add("IMSS")
        dsPeriodo2.Tables("Tabla").Columns.Add("Infonavit")
        dsPeriodo2.Tables("Tabla").Columns.Add("Infonavit_bim_anterior")
        dsPeriodo2.Tables("Tabla").Columns.Add("Ajuste_infonavit")
        dsPeriodo2.Tables("Tabla").Columns.Add("Pension_Alimenticia")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prestamo")
        dsPeriodo2.Tables("Tabla").Columns.Add("Fonacot")
        dsPeriodo2.Tables("Tabla").Columns.Add("T_No_laborado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Cuota_Sindical")
        dsPeriodo2.Tables("Tabla").Columns.Add("Subsidio_Generado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Subsidio_Aplicado")
        dsPeriodo2.Tables("Tabla").Columns.Add("Neto_SA")
        dsPeriodo2.Tables("Tabla").Columns.Add("Prestamo_Personal_A")
        dsPeriodo2.Tables("Tabla").Columns.Add("Adeudo_Infonavit_A")
        'dsPeriodo.Tables("Tabla").Columns.Add("Diferencia_Infonavit_A")
        dsPeriodo2.Tables("Tabla").Columns.Add("PA_A")
        dsPeriodo2.Tables("Tabla").Columns.Add("SINDICATO/PPP")
        dsPeriodo2.Tables("Tabla").Columns.Add("PRIMA_EXCEN")
        dsPeriodo2.Tables("Tabla").Columns.Add("%_Comisión")
        dsPeriodo2.Tables("Tabla").Columns.Add("Comisión_SA")
        dsPeriodo2.Tables("Tabla").Columns.Add("Comisión_Beneficio")

        dsPeriodo2.Tables("Tabla").Columns.Add("IMSS_CS")
        dsPeriodo2.Tables("Tabla").Columns.Add("RCV_CS")
        dsPeriodo2.Tables("Tabla").Columns.Add("Infonavit_CS")
        dsPeriodo2.Tables("Tabla").Columns.Add("ISN_CS")
        dsPeriodo2.Tables("Tabla").Columns.Add("Total_Costo_Social")
        dsPeriodo2.Tables("Tabla").Columns.Add("Subtotal")
        dsPeriodo2.Tables("Tabla").Columns.Add("IVA")
        dsPeriodo2.Tables("Tabla").Columns.Add("TOTAL_DEPOSITO")
        dsPeriodo2.Tables("Tabla").Columns.Add("fecha_inicio")
        dsPeriodo2.Tables("Tabla").Columns.Add("fecha_fin")

    End Sub


    Private Sub llenargridinicial(Optional ByRef tiponom As String = "")
        Try

            DSP()


            dtgDatos.DataSource = dsPeriodo.Tables("Tabla")

            
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub llenargrid(Optional ByRef tiponom As String = "")
        'Cargar grid
        Try
            Dim sql As String
            Dim sql2 As String
            Dim infonavit As Double
            Dim prestamo As Double
            Dim incidencia As Double
            Dim bCalcular As Boolean
            Dim PrimaSA As Double
            Dim cadenabanco As String
            dtgDatos.Columns.Clear()
            dtgDatos.DataSource = Nothing

            
            dtgDatos.DefaultCellStyle.Font = New Font("Calibri", 8)
            dtgDatos.ColumnHeadersDefaultCellStyle.Font = New Font("Calibri", 9)
            Dim chk As New DataGridViewCheckBoxColumn()
            dtgDatos.Columns.Add(chk)
            chk.HeaderText = ""
            chk.Name = "chk"
            


            'verificamos que no sea una nomina ya guardada como final
            sql = "select * from Nomina inner join EmpleadosC on fkiIdEmpleadoC=iIdEmpleadoC"
            sql &= " where Nomina.fkiIdEmpresa = 1 And fkiIdPeriodo = " & cboperiodo.SelectedValue
            sql &= " and Nomina.iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
            sql &= " and iTipoNomina=0"
            sql &= " order by " & campoordenamiento 'cNombreLargo"
            'sql = "EXEC getNominaXEmpresaXPeriodo " & gIdEmpresa & "," & cboperiodo.SelectedValue & ",1"

            bCalcular = True
            Dim rwNominaGuardada As DataRow() = nConsulta(sql)

            'If rwNominaGuardadaFinal Is Nothing = False Then
            If rwNominaGuardada Is Nothing = False Then
                'Cargamos los datos de guardados como final
                dsPeriodo.Tables("Tabla").Rows.Clear()


                For x As Integer = 0 To rwNominaGuardada.Count - 1





                    Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow


                    fila.Item("Consecutivo") = (x + 1).ToString
                    fila.Item("Id_empleado") = rwNominaGuardada(x)("iIdEmpleadoC").ToString
                    fila.Item("CodigoEmpleado") = rwNominaGuardada(x)("cCodigoEmpleado").ToString
                    fila.Item("Nombre") = rwNominaGuardada(x)("cNombreLargo").ToString.ToUpper()
                    fila.Item("Status") = IIf(rwNominaGuardada(x)("iOrigen").ToString = "1", "CONFIANZA", "SINDICALIZADO")
                    fila.Item("RFC") = rwNominaGuardada(x)("cRFC").ToString
                    fila.Item("CURP") = rwNominaGuardada(x)("cCURP").ToString
                    fila.Item("Num_IMSS") = rwNominaGuardada(x)("cIMSS").ToString

                    fila.Item("Fecha_Nac") = Date.Parse(rwNominaGuardada(x)("dFechaNac").ToString).ToShortDateString()
                    'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                    fila.Item("Edad") = CalcularEdad(Date.Parse(rwNominaGuardada(x)("dFechaNac").ToString).Day, Date.Parse(rwNominaGuardada(x)("dFechaNac").ToString).Month, Date.Parse(rwNominaGuardada(x)("dFechaNac").ToString).Year)
                    fila.Item("Puesto") = rwNominaGuardada(x)("cPuesto").ToString
                    fila.Item("Depto") = "uno"

                    fila.Item("Tipo_Infonavit") = rwNominaGuardada(x)("cTipoFactor").ToString
                    fila.Item("Valor_Infonavit") = rwNominaGuardada(x)("fFactor").ToString


                    fila.Item("Horas_extras_dobles_V") = rwNominaGuardada(x)("fTExtra2V").ToString
                    fila.Item("Horas_extras_triples_V") = rwNominaGuardada(x)("fTExtra3V").ToString
                    fila.Item("Descanso_Laborado_V") = rwNominaGuardada(x)("fDescansoLV").ToString
                    fila.Item("Dia_Festivo_laborado_V") = rwNominaGuardada(x)("fDiaFestivoLV").ToString
                    fila.Item("Prima_Dominical_V") = rwNominaGuardada(x)("fPrima_Dominical_V").ToString
                    fila.Item("Falta_Injustificada_V") = rwNominaGuardada(x)("fFalta_Injustificada_V").ToString
                    fila.Item("Permiso_Sin_GS_V") = rwNominaGuardada(x)("fPermiso_Sin_GS_V").ToString
                    fila.Item("T_No_laborado_V") = rwNominaGuardada(x)("fT_No_laborado_V").ToString





                    fila.Item("Sueldo_Base") = rwNominaGuardada(x)("fSueldoOrd").ToString
                    fila.Item("Salario_Diario") = rwNominaGuardada(x)("fSalarioDiario").ToString
                    'fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fFactorIntegracion").ToString
                    fila.Item("Salario_Cotización") = rwNominaGuardada(x)("fSalarioBC").ToString
                    fila.Item("Dias_Trabajados") = rwNominaGuardada(x)("iDiasTrabajados").ToString
                    fila.Item("Tipo_Incapacidad") = TipoIncapacidad(rwNominaGuardada(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                    fila.Item("Número_días") = NumDiasIncapacidad(rwNominaGuardada(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                    fila.Item("Sueldo_Bruto") = rwNominaGuardada(x)("fSueldoBruto").ToString
                    fila.Item("Septimo_Dia") = rwNominaGuardada(x)("fSeptimoDia").ToString
                    fila.Item("Prima_Dominical_Gravada") = rwNominaGuardada(x)("fPrimaDomGravada").ToString
                    fila.Item("Prima_Dominical_Exenta") = rwNominaGuardada(x)("fPrimaDomExenta").ToString
                    fila.Item("Tiempo_Extra_Doble_Gravado") = rwNominaGuardada(x)("fTExtra2Gravado").ToString
                    fila.Item("Tiempo_Extra_Doble_Exento") = rwNominaGuardada(x)("fTExtra2Exento").ToString
                    fila.Item("Tiempo_Extra_Triple") = rwNominaGuardada(x)("fTExtra3").ToString

                    fila.Item("Descanso_Labarado") = rwNominaGuardada(x)("fDescansoL").ToString
                    fila.Item("Dia_Festivo_laborado") = rwNominaGuardada(x)("fDiaFestivoL").ToString
                    fila.Item("Bono_Asistencia") = rwNominaGuardada(x)("fBonoAsistencia").ToString
                    fila.Item("Bono_Productividad") = rwNominaGuardada(x)("fBonoProductividad").ToString
                    fila.Item("Bono_Polivalencia") = rwNominaGuardada(x)("fBonoPolivalencia").ToString
                    fila.Item("Bono_Especialidad") = rwNominaGuardada(x)("fBonoEspecialidad").ToString
                    fila.Item("Bono_Calidad") = rwNominaGuardada(x)("fBonoCalidad").ToString
                    fila.Item("Compensacion") = rwNominaGuardada(x)("fCompensacion").ToString
                    fila.Item("Semana_fondo") = rwNominaGuardada(x)("fSemanaFondo").ToString
                    fila.Item("Falta_Injustificada") = rwNominaGuardada(x)("fFaltaInjustificada").ToString
                    fila.Item("Permiso_Sin_GS") = rwNominaGuardada(x)("fPermisoSinGS").ToString
                    fila.Item("Incremento_Retenido") = rwNominaGuardada(x)("fIncrementoRetenido").ToString


                    fila.Item("Vacaciones_proporcionales") = rwNominaGuardada(x)("fVacacionesProporcionales").ToString
                    fila.Item("Aguinaldo_gravado") = rwNominaGuardada(x)("fAguinaldoGravado").ToString
                    fila.Item("Aguinaldo_exento") = rwNominaGuardada(x)("fAguinaldoExento").ToString
                    fila.Item("Total_Aguinaldo") = Math.Round(Double.Parse(rwNominaGuardada(x)("fAguinaldoGravado").ToString) + Double.Parse(rwNominaGuardada(x)("fAguinaldoExento").ToString))
                    fila.Item("Prima_vac_gravado") = rwNominaGuardada(x)("fPrimaVacacionalGravado").ToString
                    fila.Item("Prima_vac_exento") = rwNominaGuardada(x)("fPrimaVacacionalExento").ToString
                    fila.Item("Total_Prima_vac") = Math.Round(Double.Parse(rwNominaGuardada(x)("fPrimaVacacionalGravado").ToString) + Double.Parse(rwNominaGuardada(x)("fPrimaVacacionalExento").ToString))

                    fila.Item("Total_percepciones") = rwNominaGuardada(x)("fTotalPercepciones").ToString
                    fila.Item("Total_percepciones_p/isr") = rwNominaGuardada(x)("fTotalPercepcionesISR").ToString

                    fila.Item("Incapacidad") = rwNominaGuardada(x)("fIncapacidad").ToString
                    fila.Item("ISR") = rwNominaGuardada(x)("fIsr").ToString
                    fila.Item("IMSS") = rwNominaGuardada(x)("fImss").ToString
                    fila.Item("Infonavit") = rwNominaGuardada(x)("fInfonavit").ToString
                    fila.Item("Infonavit_bim_anterior") = rwNominaGuardada(x)("fInfonavitBanterior").ToString
                    fila.Item("Ajuste_infonavit") = rwNominaGuardada(x)("fAjusteInfonavit").ToString
                    fila.Item("Pension_Alimenticia") = rwNominaGuardada(x)("fPensionAlimenticia").ToString
                    fila.Item("Prestamo") = rwNominaGuardada(x)("fPrestamo").ToString
                    fila.Item("Fonacot") = rwNominaGuardada(x)("fFonacot").ToString
                    fila.Item("T_No_laborado") = rwNominaGuardada(x)("fT_No_laborado").ToString
                    fila.Item("Cuota_Sindical") = rwNominaGuardada(x)("fCuotaSindical").ToString

                    fila.Item("Subsidio_Generado") = rwNominaGuardada(x)("fSubsidioGenerado").ToString
                    fila.Item("Subsidio_Aplicado") = rwNominaGuardada(x)("fSubsidioAplicado").ToString
                    fila.Item("Neto_SA") = rwNominaGuardada(x)("fOperadora").ToString
                    fila.Item("Prestamo_Personal_A") = rwNominaGuardada(x)("fPrestamoPerA").ToString
                    fila.Item("Adeudo_Infonavit_A") = rwNominaGuardada(x)("fAdeudoInfonavitA").ToString
                    fila.Item("PA_A") = rwNominaGuardada(x)("fDiferenciaInfonavitA").ToString
                    fila.Item("SINDICATO/PPP") = rwNominaGuardada(x)("fAsimilados").ToString
                    fila.Item("PRIMA_EXCEN") = rwNominaGuardada(x)("fRetencionOperadora").ToString
                    fila.Item("%_Comisión") = rwNominaGuardada(x)("fPorComision").ToString
                    fila.Item("Comisión_SA") = rwNominaGuardada(x)("fComisionOperadora").ToString
                    fila.Item("Comisión_Beneficio") = rwNominaGuardada(x)("fComisionAsimilados").ToString
                    fila.Item("IMSS_CS") = rwNominaGuardada(x)("fImssCS").ToString
                    fila.Item("RCV_CS") = rwNominaGuardada(x)("fRcvCS").ToString
                    fila.Item("Infonavit_CS") = rwNominaGuardada(x)("fInfonavitCS").ToString
                    fila.Item("ISN_CS") = rwNominaGuardada(x)("fInsCS").ToString
                    fila.Item("Total_Costo_Social") = rwNominaGuardada(x)("fTotalCostoSocial").ToString
                    fila.Item("Subtotal") = rwNominaGuardada(x)("fSubtotal").ToString
                    fila.Item("IVA") = rwNominaGuardada(x)("fIVA").ToString
                    fila.Item("TOTAL_DEPOSITO") = rwNominaGuardada(x)("fTotalDeposito").ToString

                    fila.Item("fecha_inicio") = Date.Parse(IIf(rwNominaGuardada(x)("dFechaInicial").ToString = "", "01/01/2019", rwNominaGuardada(x)("dFechaInicial").ToString)).ToShortDateString
                    fila.Item("fecha_fin") = Date.Parse(IIf(rwNominaGuardada(x)("dFechaFinal").ToString = "", "01/01/2019", rwNominaGuardada(x)("dFechaFinal").ToString)).ToShortDateString

                    dsPeriodo.Tables("Tabla").Rows.Add(fila)

                Next





                dtgDatos.DataSource = dsPeriodo.Tables("Tabla")

                MATGRID()



                If tiponom = "" Then
                    MessageBox.Show("Datos cargados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If


            Else

                If 0 = 0 Then
                    If cboserie.SelectedIndex = 0 Then
                        'Buscamos los datos de sindicato solamente
                        sql = "select  * from empleadosC where fkiIdClienteInter=1 "
                        'sql &= " iEstatus= " & 1
                        'sql = "select  * from empleadosTMM where fkiIdClienteInter=-1"
                        'sql = "select iIdEmpleadoC,NumCuenta, (cApellidoP + ' ' + cApellidoM + ' ' + cNombre) as nombre, fkiIdEmpresa,fSueldoOrd,fCosto from empleadosC"
                        'sql &= " where empleadosC.iOrigen=2 and empleadosC.iEstatus=1"
                        'sql &= " and empleadosC.fkiIdEmpresa =" & gIdEmpresa
                        sql &= " order by cCodigoEmpleado"

                    ElseIf cboserie.SelectedIndex > 0 Or cboserie.SelectedIndex - 1 Then

                    End If

                    Dim rwDatosEmpleados As DataRow() = nConsulta(sql)
                    If rwDatosEmpleados Is Nothing = False Then
                        dsPeriodo.Tables("Tabla").Rows.Clear()
                        For x As Integer = 0 To rwDatosEmpleados.Length - 1




                            Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow

                            fila.Item("Consecutivo") = (x + 1).ToString
                            fila.Item("Id_empleado") = rwDatosEmpleados(x)("iIdEmpleadoC").ToString
                            fila.Item("CodigoEmpleado") = rwDatosEmpleados(x)("cCodigoEmpleado").ToString
                            fila.Item("Nombre") = rwDatosEmpleados(x)("cNombreLargo").ToString.ToUpper()
                            fila.Item("Status") = IIf(rwDatosEmpleados(x)("iOrigen").ToString = "1", "CONFIANZA", "SINDICALIZADO")
                            fila.Item("RFC") = rwDatosEmpleados(x)("cRFC").ToString
                            fila.Item("CURP") = rwDatosEmpleados(x)("cCURP").ToString
                            fila.Item("Num_IMSS") = rwDatosEmpleados(x)("cIMSS").ToString

                            fila.Item("Fecha_Nac") = Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).ToShortDateString()
                            'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                            fila.Item("Edad") = CalcularEdad(Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Day, Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Month, Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Year)
                            fila.Item("Puesto") = rwDatosEmpleados(x)("cPuesto").ToString
                            fila.Item("Depto") = "uno"

                            fila.Item("Tipo_Infonavit") = rwDatosEmpleados(x)("cTipoFactor").ToString
                            fila.Item("Valor_Infonavit") = rwDatosEmpleados(x)("fFactor").ToString


                            fila.Item("Horas_extras_dobles_V") = ""
                            fila.Item("Horas_extras_triples_V") = ""
                            fila.Item("Descanso_Laborado_V") = ""
                            fila.Item("Dia_Festivo_laborado_V") = ""
                            fila.Item("Prima_Dominical_V") = ""
                            fila.Item("Falta_Injustificada_V") = ""
                            fila.Item("Permiso_Sin_GS_V") = ""
                            fila.Item("T_No_laborado_V") = ""





                            fila.Item("Sueldo_Base") = rwDatosEmpleados(x)("fSueldoOrd").ToString
                            fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fSueldoBase").ToString
                            'fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fFactorIntegracion").ToString
                            fila.Item("Salario_Cotización") = rwDatosEmpleados(x)("fSueldoIntegrado").ToString
                            fila.Item("Dias_Trabajados") = IIf(diasperiodo > 7, 15, diasperiodo)
                            fila.Item("Tipo_Incapacidad") = TipoIncapacidad(rwDatosEmpleados(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Número_días") = NumDiasIncapacidad(rwDatosEmpleados(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Sueldo_Bruto") = ""
                            fila.Item("Septimo_Dia") = ""
                            fila.Item("Prima_Dominical_Gravada") = ""
                            fila.Item("Prima_Dominical_Exenta") = ""
                            fila.Item("Tiempo_Extra_Doble_Gravado") = ""
                            fila.Item("Tiempo_Extra_Doble_Exento") = ""
                            fila.Item("Tiempo_Extra_Triple") = ""

                            fila.Item("Descanso_Labarado") = ""
                            fila.Item("Dia_Festivo_laborado") = ""
                            fila.Item("Bono_Asistencia") = ""
                            fila.Item("Bono_Productividad") = ""
                            fila.Item("Bono_Polivalencia") = ""
                            fila.Item("Bono_Especialidad") = ""
                            fila.Item("Bono_Calidad") = ""
                            fila.Item("Compensacion") = ""
                            fila.Item("Semana_fondo") = ""
                            fila.Item("Falta_Injustificada") = ""
                            fila.Item("Permiso_Sin_GS") = ""
                            fila.Item("Incremento_Retenido") = ""


                            fila.Item("Vacaciones_proporcionales") = ""
                            fila.Item("Aguinaldo_gravado") = ""
                            fila.Item("Aguinaldo_exento") = ""
                            fila.Item("Total_Aguinaldo") = ""
                            fila.Item("Prima_vac_gravado") = ""
                            fila.Item("Prima_vac_exento") = ""
                            fila.Item("Total_Prima_vac") = ""

                            fila.Item("Total_percepciones") = ""
                            fila.Item("Total_percepciones_p/isr") = ""

                            fila.Item("Incapacidad") = ""
                            fila.Item("ISR") = ""
                            fila.Item("IMSS") = ""
                            fila.Item("Infonavit") = ""
                            fila.Item("Infonavit_bim_anterior") = ""
                            fila.Item("Ajuste_infonavit") = ""
                            fila.Item("Pension_Alimenticia") = ""
                            fila.Item("Prestamo") = ""
                            fila.Item("Fonacot") = ""
                            fila.Item("T_No_laborado") = ""
                            fila.Item("Cuota_Sindical") = ""


                            fila.Item("Subsidio_Generado") = ""
                            fila.Item("Subsidio_Aplicado") = ""
                            fila.Item("Neto_SA") = ""
                            fila.Item("Prestamo_Personal_A") = ""
                            fila.Item("Adeudo_Infonavit_A") = ""
                            fila.Item("PA_A") = ""
                            fila.Item("SINDICATO/PPP") = ""
                            fila.Item("PRIMA_EXCEN") = ""
                            fila.Item("%_Comisión") = ""
                            fila.Item("Comisión_SA") = ""
                            fila.Item("Comisión_Beneficio") = ""
                            fila.Item("IMSS_CS") = ""
                            fila.Item("RCV_CS") = ""
                            fila.Item("Infonavit_CS") = ""
                            fila.Item("ISN_CS") = ""
                            fila.Item("Total_Costo_Social") = ""
                            fila.Item("Subtotal") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("fecha_inicio") = ""
                            fila.Item("fecha_fin") = ""

                            dsPeriodo.Tables("Tabla").Rows.Add(fila)




                        Next




                        dtgDatos.DataSource = dsPeriodo.Tables("Tabla")

                        MATGRID()
                        'Cambiamos el index del combro de departamentos

                        'For x As Integer = 0 To dtgDatos.Rows.Count - 1

                        '    sql = "select * from empleadosC where iIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        '    Dim rwFila As DataRow() = nConsulta(sql)




                        'Next

                        If tiponom = "" Then
                            MessageBox.Show("Datos cargados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("No hay datos en este período", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        MessageBox.Show("No hay datos de empleados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If





                    'No hay datos en este período
                Else
                    MessageBox.Show("Para la nomina Descanso, solo se mostraran datos guardados, no se podrá calcular de 0", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If



            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Function TipoIncapacidad(idempleado As String, periodo As Integer) As String
        Dim sql As String
        Dim cadena As String = "Ninguno"

        Try
            sql = "select * from periodos where iIdPeriodo= " & periodo
            Dim rwPeriodo As DataRow() = nConsulta(Sql)

            If rwPeriodo Is Nothing = False Then

                sql = "select * from incapacidad where iIdIncapacidad= "
                sql &= " (select Max(iIdIncapacidad) from incapacidad where iEstatus=1 and fkiIdEmpleado=" & idempleado & ") "
                Dim rwIncapacidad As DataRow() = nConsulta(sql)

                If rwIncapacidad Is Nothing = False Then
                    Dim FechaBuscar As Date = Date.Parse(rwIncapacidad(0)("FechaInicio"))
                    Dim FechaInicial As Date = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                    Dim FechaFinal As Date = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    'Dim FechaAntiguedad As Date = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))

                    If FechaBuscar.CompareTo(FechaInicial) >= 0 And FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                        'Estamos dentro del rango inicial
                        Return Identificadorincapacidad(rwIncapacidad(0)("RamoRiesgo"))

                    ElseIf FechaBuscar.CompareTo(FechaInicial) <= 0 Then
                        FechaBuscar = Date.Parse(rwIncapacidad(0)("fechafin"))
                        If FechaBuscar.CompareTo(FechaFinal) > 0 Then
                            Return Identificadorincapacidad(rwIncapacidad(0)("RamoRiesgo"))
                        End If

                    End If

                Else
                    cadena = "Ninguno"
                    Return cadena
                End If

                
            Else
                Return "Ninguno"

            End If
            Return "Ninguno"
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        
    End Function

    Private Function NumDiasIncapacidad(idempleado As String, periodo As Integer) As String
        Dim sql As String
        Dim cadena As String

        Try
            sql = "select * from periodos where iIdPeriodo= " & periodo
            Dim rwPeriodo As DataRow() = nConsulta(sql)

            If rwPeriodo Is Nothing = False Then
                'If idempleado = 42 Then
                '    MsgBox("llege")
                'End If

                sql = "select * from incapacidad where iIdIncapacidad= "
                sql &= " (select Max(iIdIncapacidad) from incapacidad where iEstatus=1 and fkiIdEmpleado=" & idempleado & ") "
                Dim rwIncapacidad As DataRow() = nConsulta(sql)

                If rwIncapacidad Is Nothing = False Then
                    Dim FechaBuscar As Date = Date.Parse(rwIncapacidad(0)("FechaInicio"))
                    Dim FechaInicial As Date = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                    Dim FechaFinal As Date = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    'Dim FechaAntiguedad As Date = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))

                    If FechaBuscar.CompareTo(FechaInicial) >= 0 And FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                        'Estamos dentro del rango inicial
                        FechaBuscar = Date.Parse(rwIncapacidad(0)("fechafin"))
                        If FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                            'Restamos entre final incapacidad menos la inicial incapacidad
                            Return (DateDiff(DateInterval.Day, Date.Parse(rwIncapacidad(0)("FechaInicio")), Date.Parse(rwIncapacidad(0)("fechafin"))) + 1).ToString
                        Else
                            'restamos final del periodo menos inicial incapacidad
                            Return (DateDiff(DateInterval.Day, Date.Parse(rwIncapacidad(0)("FechaInicio")), Date.Parse(rwPeriodo(0)("dFechaFin"))) + 1).ToString


                        End If

                    ElseIf FechaBuscar.CompareTo(FechaInicial) <= 0 Then
                        FechaBuscar = Date.Parse(rwIncapacidad(0)("fechafin"))
                        If FechaBuscar.CompareTo(FechaFinal) > 0 Then
                            'Restamos fecha final incapacidad menos la fechainicial  periodo
                            Return (DateDiff(DateInterval.Day, Date.Parse(rwPeriodo(0)("dFechaInicio")), Date.Parse(rwIncapacidad(0)("fechafin"))) + 1).ToString
                        Else
                            'todos los dias del periodo tiene incapaciddad
                            Return 0
                        End If

                    End If
                Else
                    cadena = "0"
                    Return cadena
                End If


            Else
                Return "0"

            End If
            Return "0"
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Function

    Private Function Identificadorincapacidad(identificador As String) As String
        Try
            Dim TipoIncidencia As String = ""

            If identificador = "0" Then
                TipoIncidencia = "Riesgo de trabajo"
            ElseIf identificador = "1" Then
                TipoIncidencia = "Enfermedad general"
            ElseIf identificador = "2" Then
                TipoIncidencia = "Maternidad"

            End If

            Return TipoIncidencia
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Function


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

    Private Sub cmdguardarnomina_Click(sender As Object, e As EventArgs) Handles cmdguardarnomina.Click

        Try
            Dim sql As String
            Dim sql2 As String
            Dim NOCALCULAR As Boolean
            Dim consecutivo1 As String

            sql = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
            sql &= " and iTipoNomina=0"
            'Dim sueldobase, salariodiario, salariointegrado, sueldobruto, TiempoExtraFijoGravado, TiempoExtraFijoExento As Double
            'Dim TiempoExtraOcasional, DesSemObligatorio, VacacionesProporcionales, AguinaldoGravado, AguinaldoExento As Double
            'Dim PrimaVacGravada, PrimaVacExenta, TotalPercepciones, TotalPercepcionesISR As Double
            'Dim incapacidad, ISR, IMSS, Infonavit, InfonavitAnterior, InfonavitAjuste, PensionAlimenticia As Double
            'Dim Prestamo, Fonacot, NetoaPagar, Excedente, Total, ImssCS, RCVCS, InfonavitCS, ISNCS
            'sql = "EXEC getNominaXEmpresaXPeriodo " & gIdEmpresa & "," & cboperiodo.SelectedValue & ",1"

            Dim rwNominaGuardadaFinal As DataRow() = nConsulta(sql)

            If rwNominaGuardadaFinal Is Nothing = False Then
                MessageBox.Show("La nomina ya esta marcada como final, no  se pueden guardar cambios", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else

                If chkCalSoloMarcados.Checked = False Then
                    sql = "delete from Nomina"
                    sql &= " where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iEstatusNomina=0 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
                    sql &= " and iTipoNomina=0"
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    sql = "delete from DetalleDescInfonavit"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    'sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and iTipoNomina=0"

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    sql = " delete from DetalleFonacot"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and iTipoNomina=0"

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from PagoPrestamo"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and iTipoNomina=0"
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from PagoPrestamoSA"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and iTipoNomina=0"
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                End If





                pnlProgreso.Visible = True

                Application.DoEvents()
                'pnlCatalogo.Enabled = False
                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = dtgDatos.Rows.Count

                For x As Integer = 0 To dtgDatos.Rows.Count - 1

                    If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
                        consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)

                    Else
                        consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
                    End If

                    NOCALCULAR = False
                    'If dtgDatos.Rows(x).Cells(2).Value = "321" Then
                    '    MessageBox.Show(dtgDatos.Rows(x).Cells(2).Value = "4090")
                    'End If



                    If chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "1" Then
                        'activo para borrar los datos de esse trabajador y calcularlo despues

                        sql = "delete from Nomina"
                        sql &= " where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iEstatusNomina=0 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If

                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If

                        sql = " delete from DetalleFonacot"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1
                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If


                        sql = " delete from PagoPrestamo"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina0="
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1
                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If


                        sql = " delete from PagoPrestamoSA"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If

                        NOCALCULAR = True



                        'si calcular

                    ElseIf chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "" Then
                        'No calcular
                        NOCALCULAR = False
                    ElseIf chkCalSoloMarcados.Checked = False Then
                        NOCALCULAR = True
                        'si calcular
                    End If



                    If NOCALCULAR Then
                        sql = "EXEC [setNominaInsertar ] 0"
                        'periodo
                        sql &= "," & cboperiodo.SelectedValue
                        'idempleado
                        sql &= "," & dtgDatos.Rows(x).Cells(2).Value
                        'idempresa
                        sql &= ",1"
                        'Puesto
                        'buscamos el valor en la tabla
                        sql2 = "select * from puestos where cNombre='" & dtgDatos.Rows(x).Cells(11).FormattedValue & "'"

                        Dim rwPuesto As DataRow() = nConsulta(sql2)

                        sql &= "," & rwPuesto(0)("iIdPuesto")


                        'departamento
                        'buscamos el valor en la tabla
                        sql2 = "select * from departamentos where cNombre='" & dtgDatos.Rows(x).Cells(12).FormattedValue & "'"

                        Dim rwDepto As DataRow() = nConsulta(sql2)

                        sql &= "," & rwDepto(0)("iIdDepartamento")

                        'estatus empleado
                        sql &= "," & cboserie.SelectedIndex
                        'edad
                        sql &= "," & dtgDatos.Rows(x).Cells(10).Value
                        'puesto
                        sql &= ",'" & dtgDatos.Rows(x).Cells(11).FormattedValue & "'"
                        'buque
                        sql &= ",'" & dtgDatos.Rows(x).Cells(12).FormattedValue & "'"
                        'iTipo Infonavit
                        sql &= ",'" & dtgDatos.Rows(x).Cells(13).Value & "'"
                        'valor infonavit
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(14).Value = "", "0", dtgDatos.Rows(x).Cells(14).Value.ToString.Replace(",", ""))
                        'fTExtra2V
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(15).Value = "", "0", dtgDatos.Rows(x).Cells(15).Value.ToString.Replace(",", ""))
                        'fTExtra3V
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(16).Value = "", "0", dtgDatos.Rows(x).Cells(16).Value.ToString.Replace(",", ""))
                        'fDescansoLV
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(17).Value = "", "0", dtgDatos.Rows(x).Cells(17).Value.ToString.Replace(",", ""))
                        'fDiaFestivoLV
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value.ToString.Replace(",", ""))
                        'fHoras_extras_dobles_V
                        sql &= ",0"
                        'fHoras_extras_triples_V
                        sql &= ",0"
                        'fDescanso_Laborado_V
                        sql &= ",0"
                        'fDia_Festivo_laborado_V
                        sql &= ",0"
                        'fPrima_Dominical_V 
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(19).Value = "", "0", dtgDatos.Rows(x).Cells(19).Value.ToString.Replace(",", ""))
                        'fFalta_Injustificada_V
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(20).Value = "", "0", dtgDatos.Rows(x).Cells(20).Value.ToString.Replace(",", ""))
                        'fPermiso_Sin_GS_V
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(21).Value = "", "0", dtgDatos.Rows(x).Cells(21).Value.ToString.Replace(",", ""))
                        'fT_No_laborado_V
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(22).Value = "", "0", dtgDatos.Rows(x).Cells(22).Value.ToString.Replace(",", ""))

                        'salario base
                        sql &= "," & (IIf(dtgDatos.Rows(x).Cells(23).Value = "", "0", dtgDatos.Rows(x).Cells(23).Value.ToString.Replace(",", "")))
                        'salario diario
                        sql &= "," & (IIf(dtgDatos.Rows(x).Cells(24).Value = "", "0", dtgDatos.Rows(x).Cells(24).Value.ToString.Replace(",", "")))
                        'salario integrado
                        sql &= "," & (IIf(dtgDatos.Rows(x).Cells(25).Value = "", "0", dtgDatos.Rows(x).Cells(25).Value.ToString.Replace(",", "")))
                        'Dias trabajados
                        sql &= "," & (IIf(dtgDatos.Rows(x).Cells(26).Value = "", "0", dtgDatos.Rows(x).Cells(26).Value.ToString.Replace(",", "")))
                        'tipo incapacidad
                        sql &= ",'" & (IIf(dtgDatos.Rows(x).Cells(27).Value = "", "0", dtgDatos.Rows(x).Cells(27).Value.ToString.Replace(",", "")))
                        'numero dias incapacidad
                        sql &= "'," & (IIf(dtgDatos.Rows(x).Cells(28).Value = "", "0", dtgDatos.Rows(x).Cells(28).Value.ToString.Replace(",", "")))
                        'sueldobruto
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(29).Value = "", "0", dtgDatos.Rows(x).Cells(29).Value.ToString.Replace(",", ""))

                        'fSeptimoDia
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(30).Value = "", "0", dtgDatos.Rows(x).Cells(30).Value.ToString.Replace(",", ""))
                        'fPrimaDomGravada
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(31).Value = "", "0", dtgDatos.Rows(x).Cells(31).Value.ToString.Replace(",", ""))
                        'fPrimaDomExenta
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(32).Value = "", "0", dtgDatos.Rows(x).Cells(32).Value.ToString.Replace(",", ""))
                        'fTExtra2Gravado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", ""))
                        'fTExtra2Exento
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(34).Value = "", "0", dtgDatos.Rows(x).Cells(34).Value.ToString.Replace(",", ""))
                        'fTExtra3
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value.ToString.Replace(",", ""))
                        'fDescansoL
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value.ToString.Replace(",", ""))
                        'fDiaFestivoL
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value.ToString.Replace(",", ""))
                        'fBonoAsistencia
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value.ToString.Replace(",", ""))
                        'fBonoProductividad
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(39).Value = "", "0", dtgDatos.Rows(x).Cells(39).Value.ToString.Replace(",", ""))
                        'fBonoPolivalencia
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(40).Value = "", "0", dtgDatos.Rows(x).Cells(40).Value.ToString.Replace(",", ""))
                        'fBonoEspecialidad
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(41).Value = "", "0", dtgDatos.Rows(x).Cells(41).Value.ToString.Replace(",", ""))
                        'fBonoCalidad
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(42).Value = "", "0", dtgDatos.Rows(x).Cells(42).Value.ToString.Replace(",", ""))
                        'fCompensacion
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(43).Value = "", "0", dtgDatos.Rows(x).Cells(43).Value.ToString.Replace(",", ""))
                        'fSemanaFondo
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(44).Value = "", "0", dtgDatos.Rows(x).Cells(44).Value.ToString.Replace(",", ""))
                        '@fFaltaInjustificada 
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(45).Value = "", "0", dtgDatos.Rows(x).Cells(45).Value.ToString.Replace(",", ""))
                        '@fPermisoSinGS 
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(46).Value = "", "0", dtgDatos.Rows(x).Cells(46).Value.ToString.Replace(",", ""))
                        '@fIncrementoRetenido 
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(47).Value = "", "0", dtgDatos.Rows(x).Cells(47).Value.ToString.Replace(",", ""))



                        'vacaciones proporcionales
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(48).Value = "", "0", dtgDatos.Rows(x).Cells(48).Value.ToString.Replace(",", ""))
                        'aguinaldo gravado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(49).Value = "", "0", dtgDatos.Rows(x).Cells(49).Value.ToString.Replace(",", ""))
                        'aguinaldo exento
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(50).Value = "", "0", dtgDatos.Rows(x).Cells(50).Value.ToString.Replace(",", ""))
                        'prima vacacional gravado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(52).Value = "", "0", dtgDatos.Rows(x).Cells(52).Value.ToString.Replace(",", ""))
                        'prima vacacional exento
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(53).Value = "", "0", dtgDatos.Rows(x).Cells(53).Value.ToString.Replace(",", ""))

                        'totalpercepciones
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(55).Value = "", "0", dtgDatos.Rows(x).Cells(55).Value.ToString.Replace(",", ""))
                        'totalpercepcionesISR
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(56).Value = "", "0", dtgDatos.Rows(x).Cells(56).Value.ToString.Replace(",", ""))
                        'Incapacidad
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(57).Value = "", "0", dtgDatos.Rows(x).Cells(57).Value.ToString.Replace(",", ""))
                        'isr
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(58).Value = "", "0", dtgDatos.Rows(x).Cells(58).Value.ToString.Replace(",", ""))
                        'imss
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(59).Value = "", "0", dtgDatos.Rows(x).Cells(59).Value.ToString.Replace(",", ""))
                        'infonavit
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(60).Value = "", "0", dtgDatos.Rows(x).Cells(60).Value.ToString.Replace(",", ""))
                        'infonavit anterior
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(61).Value = "", "0", dtgDatos.Rows(x).Cells(61).Value.ToString.Replace(",", ""))
                        'ajuste infonavit
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(62).Value = "", "0", dtgDatos.Rows(x).Cells(62).Value.ToString.Replace(",", ""))
                        'Pension alimenticia
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(63).Value = "", "0", dtgDatos.Rows(x).Cells(63).Value.ToString.Replace(",", ""))
                        'Prestamo
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(64).Value = "", "0", dtgDatos.Rows(x).Cells(64).Value.ToString.Replace(",", ""))
                        'Fonacot
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(65).Value = "", "0", dtgDatos.Rows(x).Cells(65).Value.ToString.Replace(",", ""))
                        'fT_No_laborado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(66).Value = "", "0", dtgDatos.Rows(x).Cells(66).Value.ToString.Replace(",", ""))
                        'cuota sindical
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(67).Value.ToString = "", "0", dtgDatos.Rows(x).Cells(67).Value.ToString.Replace(",", ""))

                        'Subsidio Generado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(68).Value = "", "0", dtgDatos.Rows(x).Cells(68).Value.ToString.Replace(",", ""))
                        'Subsidio Aplicado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(69).Value = "", "0", dtgDatos.Rows(x).Cells(69).Value.ToString.Replace(",", ""))
                        'Operadora
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(70).Value = "", "0", dtgDatos.Rows(x).Cells(70).Value.ToString.Replace(",", ""))
                        'Prestamo Personal Asimilado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(71).Value = "", "0", dtgDatos.Rows(x).Cells(71).Value.ToString.Replace(",", ""))
                        'Adeudo_Infonavit_Asimilado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(72).Value = "", "0", dtgDatos.Rows(x).Cells(72).Value.ToString.Replace(",", ""))
                        'Pension alimenticia Asimi
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(73).Value = "", "0", dtgDatos.Rows(x).Cells(73).Value.ToString.Replace(",", ""))
                        'Complemento Asimilado
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(74).Value = "", "0", dtgDatos.Rows(x).Cells(74).Value.ToString.Replace(",", ""))



                        'Retenciones_Operadora
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(75).Value = "", "0", dtgDatos.Rows(x).Cells(75).Value.ToString.Replace(",", ""))
                        '% Comision
                        sql &= ",0.06" '& IIf(dtgDatos.Rows(x).Cells(52).Value = "", "0", dtgDatos.Rows(x).Cells(52).Value.ToString.Replace(",", ""))
                        'Comision_Operadora
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(77).Value = "", "0", dtgDatos.Rows(x).Cells(77).Value.ToString.Replace(",", ""))
                        'Comision asimilados
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(78).Value = "", "0", dtgDatos.Rows(x).Cells(78).Value.ToString.Replace(",", ""))


                        'IMSS_CS
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(79).Value = "", "0", dtgDatos.Rows(x).Cells(79).Value.ToString.Replace(",", ""))
                        'RCV_CS
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(80).Value = "", "0", dtgDatos.Rows(x).Cells(80).Value.ToString.Replace(",", ""))
                        'Infonavit_CS
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(81).Value = "", "0", dtgDatos.Rows(x).Cells(81).Value.ToString.Replace(",", ""))
                        'ISN_CS
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(82).Value = "", "0", dtgDatos.Rows(x).Cells(82).Value.ToString.Replace(",", ""))
                        'Total Costo Social
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(83).Value = "", "0", dtgDatos.Rows(x).Cells(83).Value.ToString.Replace(",", ""))
                        'Subtotal
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(84).Value = "", "0", dtgDatos.Rows(x).Cells(84).Value.ToString.Replace(",", ""))
                        'IVA
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(85).Value = "", "0", dtgDatos.Rows(x).Cells(85).Value.ToString.Replace(",", ""))
                        'TOTAL DEPOSITO
                        sql &= "," & IIf(dtgDatos.Rows(x).Cells(86).Value = "", "0", dtgDatos.Rows(x).Cells(86).Value.ToString.Replace(",", ""))
                        'Estatus
                        sql &= ",1"
                        'Estatus Nomina
                        sql &= ",0"
                        'Tipo Nomina
                        sql &= ",0"
                        'tipo consecutivo
                        sql &= "," & consecutivo1
                        'fechainicial
                        sql &= ",'" & Date.Now.ToShortDateString
                        'fechafinal
                        sql &= "','" & Date.Now.ToShortDateString & "'"





                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error " & dtgDatos.Rows(x).Cells(3).Value, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            'pnlProgreso.Visible = False
                            Exit Sub
                        End If


                    End If



                    pgbProgreso.Value += 1
                    Application.DoEvents()
                Next
                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True

                MessageBox.Show("Datos guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)




            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub


    Private Sub cmdcalcular_Click(sender As Object, e As EventArgs) Handles cmdcalcular.Click
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Try
            Dim sql As String
            sql = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
            sql &= " and iTipoNomina=0"
            'Dim sueldobase, salariodiario, salariointegrado, sueldobruto, TiempoExtraFijoGravado, TiempoExtraFijoExento As Double
            'Dim TiempoExtraOcasional, DesSemObligatorio, VacacionesProporcionales, AguinaldoGravado, AguinaldoExento As Double
            'Dim PrimaVacGravada, PrimaVacExenta, TotalPercepciones, TotalPercepcionesISR As Double
            'Dim incapacidad, ISR, IMSS, Infonavit, InfonavitAnterior, InfonavitAjuste, PensionAlimenticia As Double
            'Dim Prestamo, Fonacot, NetoaPagar, Excedente, Total, ImssCS, RCVCS, InfonavitCS, ISNCS
            'sql = "EXEC getNominaXEmpresaXPeriodo " & gIdEmpresa & "," & cboperiodo.SelectedValue & ",1"

            Dim rwNominaGuardadaFinal As DataRow() = nConsulta(sql)

            If rwNominaGuardadaFinal Is Nothing = False Then
                MessageBox.Show("La nomina ya esta marcada como final, no  se puede calcular", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If chkCalSoloMarcados.Checked = False Then
                    If 0 = 0 Then
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex


                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex


                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex




                    Else
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"

                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and iTipo=0"

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and iTipoNomina=0"

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and iTipoNomina=0"

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and iTipoNomina=0"
                    End If


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql2) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql3) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql4) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql5) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                End If

                calcular()
            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub calcular()
        Dim Sueldo As Double
        Dim SueldoBase As Double
        Dim ValorIncapacidad As Double
        Dim TotalPercepciones As Double
        Dim Incapacidad As Double
        Dim isr As Double
        Dim imss As Double
        Dim infonavitvalor As Double
        Dim infonavitanterior As Double
        Dim ajusteinfonavit As Double
        Dim pension As Double
        Dim prestamo As Double
        Dim fonacot As Double
        Dim subsidiogenerado As Double
        Dim subsidioaplicado As Double
        Dim RetencionOperadora As Double
        Dim InfonavitNormal As Double
        Dim PrestamoPersonalAsimilados As Double
        Dim PrestamoPersonalSA As Double
        Dim AdeudoINfonavitAsimilados As Double
        Dim DiferenciaInfonavitAsimilados As Double
        Dim PensionAlimenticia As Double
        Dim PensionAlimenticiaInsertar As Double

        Dim Operadora As Double
        Dim ComplementoAsimilados As Double

        Dim SueldoBaseTMM As Double
        Dim CostoSocialTotal As Double
        Dim ComisionOperadora As Double
        Dim ComisionAsimilados As Double
        Dim subtotal As Double
        Dim iva As Double



        Dim sql As String
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Dim ValorUMA As Double
        Dim primavacacionesgravada As Double
        Dim primavacacionesexenta As Double
        Dim diastrabajados As Double
        Dim Sueldobruto As Double
        Dim TEFG As Double
        Dim TEFE As Double
        Dim TEO As Double
        Dim DSO As Double
        Dim VACAPRO As Double
        Dim numbimestre As Integer
        Dim NOCALCULAR As Boolean
        Dim consecutivo1 As String
        Dim plantaoNO As String

        Dim PensionAntesVariable As Double

        Try
            'verificamos que tenga dias a calcular
            'For x As Integer = 0 To dtgDatos.Rows.Count - 1
            '    If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value)) <= 0 Then
            '        MessageBox.Show("Existen trabajadores que no tiene dias trabajados, favor de verificar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'Next



            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If


            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count




            For x As Integer = 0 To dtgDatos.Rows.Count - 1
                NOCALCULAR = True

                If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
                    consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text))

                Else
                    consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value
                End If
                'verificar

                'verificamos los sueldos
                'sql = "Select salariod,sbc,salariodTopado,sbcTopado from costosocial inner join puestos on costosocial.fkiIdPuesto=puestos.iIdPuesto "
                'sql &= " where cNombre = '" & dtgDatos.Rows(x).Cells(11).FormattedValue & "' and anio=" & aniocostosocial

                'Dim rwDatosSalario As DataRow() = nConsulta(sql)

                'If rwDatosSalario Is Nothing = False Then
                '    If dtgDatos.Rows(x).Cells(10).Value >= 55 Then
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariodTopado")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbcTopado")
                '    Else
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariod")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbc")
                '    End If

                'Else
                '    MessageBox.Show("No se encontraron datos")
                'End If


                'validar que si ese desactivado el calculo y activa el trabajador
                If chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "1" Then
                    'activo para borrar los datos de esse trabajador y calcularlo despues
                    If 0 = 0 Then
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1


                        '' borrar el seguro si solo tiene un registro
                        'For x As Integer = 0 To dtgDatos.Rows.Count - 1
                        '    Dim ValorInfo As Double
                        '    ValorInfo = IIf(dtgDatos.Rows(x).Cells(14).Value = "", "0", dtgDatos.Rows(x).Cells(14).Value)
                        '    If ValorInfo > 0 Then
                        '        Dim numbimestre As Integer

                        '        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
                        '            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
                        '        Else
                        '            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
                        '        End If

                        '        sql = "select * from DetalleDescInfonavit inner join nomina on DetalleDescInfonavit.fkiIdEmpleado"
                        '        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue & "or fkiIdPeriodo = "
                        '        sql &= " and fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value

                        '    End If
                        'Next

                    Else
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and iTipo=0"
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and iTipoNomina=0"
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and iTipoNomina=0"
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and iTipoNomina=0"
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1
                    End If


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql2) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql3) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql4) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql5) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    'si calcular

                ElseIf chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "" Then
                    'No calcular
                    NOCALCULAR = False
                ElseIf chkCalSoloMarcados.Checked = False Then
                    'si calcular
                End If
                'If dtgDatos.Rows(x).Cells(2).Value = "704" Then
                '    MsgBox("aqui")
                'End If
                If NOCALCULAR Then
                    If dtgDatos.Rows(x).Cells(11).FormattedValue = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or dtgDatos.Rows(x).Cells(11).FormattedValue = "SUBALTERNO EN FORMACIÓN" Then






                        '####################################################################################################################################
                        '###################################################################################################################################
                        '##################################################################################################################################
                        '###############################################################################################################################
                        '###########################################################################################################################
                        '########################################################################################################################
                        '###################################################################################################################
                        '##########################################################################################################
                        '####################################################################################################
                        '#########################################################################################
                        '#############################################################################
                        '##################################################################

                        'Empieza el calculo normal
                    Else
                        diastrabajados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(26).Value = "", "0", dtgDatos.Rows(x).Cells(26).Value.ToString))
                        Dim SUELDOBRUTON As Double
                        Dim SEPTIMO As Double
                        Dim PRIDOMGRAVADA As Double
                        Dim PRIDOMEXENTA As Double
                        Dim TE2G As Double
                        Dim TE2E As Double
                        Dim TE3 As Double
                        Dim DESCANSOLABORADO As Double
                        Dim FESTIVOTRAB As Double
                        Dim BONOASISTENCIA As Double
                        Dim BONOPRODUCTIVIDAD As Double
                        Dim BONOPOLIVALENCIA As Double
                        Dim BONOESPECIALIDAD As Double
                        Dim BONOCALIDAD As Double
                        Dim COMPENSACION As Double
                        Dim SEMANAFONDO As Double
                        Dim INCREMENTORETENIDO As Double
                        Dim VACACIONESPRO As Double
                        Dim AGUINALDOGRA As Double
                        Dim AGUINALDOEXEN As Double
                        Dim PRIMAVACGRA As Double
                        Dim PRIMAVACEXEN As Double
                        Dim SUMAPERCEPCIONES As Double
                        Dim SUMAPERCEPCIONESPISR As Double
                        Dim FINJUSTIFICADA As Double
                        Dim PERMISOSINGOCEDESUELDO As Double
                        Dim PRIMADOMINICAL As Double
                        Dim SDEMPLEADO As Double

                        Dim DiasCadaPeriodo As Integer
                        Dim FechaInicioPeriodo As Date
                        Dim FechaFinPeriodo As Date
                        Dim FechaAntiguedad As Date
                        Dim FechaBuscar As Date
                        Dim TipoPeriodoinfoonavit As Integer

                        Dim INCAPACIDADD As Double
                        Dim ISRD As Double
                        Dim IMMSSD As Double
                        Dim INFONAVITD As Double
                        Dim INFOBIMANT As Double
                        Dim AJUSTEINFO As Double
                        Dim PENSIONAD As Double
                        Dim PRESTAMOD As Double
                        Dim FONACOTD As Double
                        Dim TNOLABORADOD As Double
                        Dim CUOTASINDICALD As Double
                        Dim SUBSIDIOG As Double
                        Dim SUBSIDIOA As Double
                        Dim SUMADEDUCCIONES As Double
                        Dim dias As Integer
                        Dim BanPeriodo As Boolean
                        If diastrabajados = -10 Then


                        Else
                            dias = 0
                            BanPeriodo = False
                            sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                            Dim rwPeriodo As DataRow() = nConsulta(sql)

                            If rwPeriodo Is Nothing = False Then
                                FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                                FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                                DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                                sql = "select *"
                                sql &= " from empleadosC"
                                sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                                Dim rwDatosBanco As DataRow() = nConsulta(sql)


                                If rwDatosBanco Is Nothing = False Then
                                    FechaAntiguedad = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    FechaBuscar = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    If FechaBuscar.CompareTo(FechaInicioPeriodo) > 0 And FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then
                                        'Estamos dentro del rango 
                                        'Calculamos la prima

                                        dias = (DateDiff("y", FechaBuscar, FechaFinPeriodo)) + 1

                                        BanPeriodo = True

                                    ElseIf FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then


                                        BanPeriodo = False

                                    End If
                                End If

                            End If

                            ' dtgDatos.Rows(x).Cells(3).Style.BackColor = Color.Chocolate

                            SDEMPLEADO = Double.Parse(dtgDatos.Rows(x).Cells(24).Value)
                            'dtgDatos.Rows(x).Cells(21).Value = Math.Round(Sueldo * (26.19568006 / 100), 2).ToString("###,##0.00")

                            FINJUSTIFICADA = 0
                            PERMISOSINGOCEDESUELDO = 0
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value))
                                If NombrePeriodo = "Quincenal" Then
                                    diastrabajados = diastrabajados - FINJUSTIFICADA
                                Else

                                    diastrabajados = 6
                                End If

                                dtgDatos.Rows(x).Cells(45).Value = "-" + Math.Round(SDEMPLEADO * FINJUSTIFICADA, 2).ToString("###,##0.00")
                                'Mandar la falta a la resta
                            Else
                                dtgDatos.Rows(x).Cells(45).Value = 0.0
                            End If

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value))
                                If NombrePeriodo = "Quincenal" Then
                                    diastrabajados = diastrabajados - PERMISOSINGOCEDESUELDO
                                Else

                                    diastrabajados = 6
                                End If
                                dtgDatos.Rows(x).Cells(46).Value = "-" + Math.Round(SDEMPLEADO * PERMISOSINGOCEDESUELDO, 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(46).Value = 0.0
                            End If
                            If BanPeriodo Then
                                diastrabajados = dias - 1
                            End If
                            'solo falta injustificada juega para el septimo dia
                            If NombrePeriodo = "Quincenal" Then

                                dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * Double.Parse(dtgDatos.Rows(x).Cells(26).Value), 2).ToString("###,##0.00")
                                'dtgDatos.Rows(x).Cells(26).Value = "15"
                                dtgDatos.Rows(x).Cells(30).Value = "0.00"
                            ElseIf NombrePeriodo = "Semanal" Then

                                'If dtgDatos.Rows(x).Cells(2).Value = "42" Then
                                'MsgBox("llego")
                                ' End If
                                If chkDias.Checked = False Then
                                    dtgDatos.Rows(x).Cells(26).Value = "7"
                                End If


                                If diastrabajados = 7 Then
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * 6, 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                    If ValorIncapacidad = 6 Then
                                        dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                    End If
                                Else
                                    'dtgDatos.Rows(x).Cells(29).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(24).Value) * (diastrabajados - FINJUSTIFICADA - PERMISOSINGOCEDESUELDO), 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * (diastrabajados), 2).ToString("###,##0.00")
                                    If BanPeriodo Then
                                        dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    Else
                                        If PERMISOSINGOCEDESUELDO > 0 Then

                                        End If
                                        'sacar factor de septimo dia solo en el caso de falta injustifica 
                                        If (diastrabajados - FINJUSTIFICADA) = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = SDEMPLEADO
                                        Else
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO * (0.166 * (diastrabajados - FINJUSTIFICADA)), 2).ToString("###,##0.00")
                                        End If


                                        If PERMISOSINGOCEDESUELDO = 7 Then
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                        End If
                                        ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                        If ValorIncapacidad = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                        End If

                                    End If

                                End If

                            End If


                            'Incapacidad
                            ValorIncapacidad = 0.0
                            'If dtgDatos.Rows(x).Cells(2).Value = 91 Then
                            '    'MsgBox("lol")
                            'End If
                            If dtgDatos.Rows(x).Cells(27).Value <> "Ninguno" Then

                                ValorIncapacidad = dtgDatos.Rows(x).Cells(28).Value * SDEMPLEADO

                            End If
                            dtgDatos.Rows(x).Cells(57).Value = Math.Round(ValorIncapacidad, 2).ToString("###,##0.00")

                            'PrimaDominical
                            If chkPrimaDominical.Checked = False Then
                                If Double.Parse(IIf(dtgDatos.Rows(x).Cells(19).Value = "", 0, dtgDatos.Rows(x).Cells(19).Value)) > 0 Then
                                    PRIMADOMINICAL = Double.Parse(dtgDatos.Rows(x).Cells(19).Value) * SDEMPLEADO * 0.25
                                    If PRIMADOMINICAL > ValorUMA Then
                                        dtgDatos.Rows(x).Cells(31).Value = Math.Round(PRIMADOMINICAL - ValorUMA, 2).ToString("###,##0.00")
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(ValorUMA, 2).ToString("###,##0.00")
                                    Else
                                        dtgDatos.Rows(x).Cells(31).Value = "0.00"
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(PRIMADOMINICAL, 2).ToString("###,##0.00")
                                    End If
                                End If
                            End If


                            'Tiempo Extra Doble
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(15).Value = "", 0, dtgDatos.Rows(x).Cells(15).Value)) > 0 Then


                                dtgDatos.Rows(x).Cells(33).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADO / 8) * 2) / 2, 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(34).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADO / 8) * 2) / 2, 2).ToString("###,##0.00")

                            Else
                                dtgDatos.Rows(x).Cells(33).Value = "0.00"
                                dtgDatos.Rows(x).Cells(34).Value = "0.00"
                            End If

                            'Tiempo Extra triple
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(16).Value = "", 0, dtgDatos.Rows(x).Cells(16).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(35).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(16).Value) * (SDEMPLEADO / 8) * 3), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(35).Value = "0.00"
                            End If

                            'Descanso Laborado

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(17).Value = "", 0, dtgDatos.Rows(x).Cells(17).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(36).Value = Math.Round(SDEMPLEADO * 2 * Double.Parse(dtgDatos.Rows(x).Cells(17).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(36).Value = "0.00"
                            End If
                            'Dia Festivo laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", 0, dtgDatos.Rows(x).Cells(18).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(37).Value = Math.Round(SDEMPLEADO * 2 * Double.Parse(dtgDatos.Rows(x).Cells(18).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(37).Value = "0.00"
                            End If

                            'Tiempo No laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(66).Value = Math.Round(SDEMPLEADO / 8 * Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(66).Value = "0.00"
                            End If

                            'Calcular la prima
                            If chkPrimaVacacional.Checked = False Then
                                If DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)

                                ElseIf DiasCadaPeriodo = 6 Or DiasCadaPeriodo = 7 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)
                                End If
                            End If



                            'sumar Para ISR

                            SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            SEPTIMO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(30).Value = "", 0, dtgDatos.Rows(x).Cells(30).Value))
                            PRIDOMGRAVADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(31).Value = "", 0, dtgDatos.Rows(x).Cells(31).Value))
                            PRIDOMEXENTA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(32).Value = "", 0, dtgDatos.Rows(x).Cells(32).Value))
                            TE2G = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", 0, dtgDatos.Rows(x).Cells(33).Value))
                            TE2E = Double.Parse(IIf(dtgDatos.Rows(x).Cells(34).Value = "", 0, dtgDatos.Rows(x).Cells(34).Value))
                            TE3 = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", 0, dtgDatos.Rows(x).Cells(35).Value))
                            DESCANSOLABORADO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", 0, dtgDatos.Rows(x).Cells(36).Value))
                            FESTIVOTRAB = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", 0, dtgDatos.Rows(x).Cells(37).Value))
                            BONOASISTENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", 0, dtgDatos.Rows(x).Cells(38).Value))
                            BONOPRODUCTIVIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", 0, dtgDatos.Rows(x).Cells(39).Value))
                            BONOPOLIVALENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", 0, dtgDatos.Rows(x).Cells(40).Value))
                            BONOESPECIALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", 0, dtgDatos.Rows(x).Cells(41).Value))
                            BONOCALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", 0, dtgDatos.Rows(x).Cells(42).Value))
                            COMPENSACION = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", 0, dtgDatos.Rows(x).Cells(43).Value))
                            SEMANAFONDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", 0, dtgDatos.Rows(x).Cells(44).Value))
                            FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", 0, dtgDatos.Rows(x).Cells(45).Value))
                            PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(46).Value = "", 0, dtgDatos.Rows(x).Cells(46).Value))
                            INCREMENTORETENIDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", 0, dtgDatos.Rows(x).Cells(47).Value))
                            VACACIONESPRO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", 0, dtgDatos.Rows(x).Cells(48).Value))
                            AGUINALDOGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", 0, dtgDatos.Rows(x).Cells(49).Value))
                            AGUINALDOEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(50).Value = "", 0, dtgDatos.Rows(x).Cells(50).Value))
                            PRIMAVACGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(52).Value = "", 0, dtgDatos.Rows(x).Cells(52).Value))
                            PRIMAVACEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(53).Value = "", 0, dtgDatos.Rows(x).Cells(53).Value))



                            SUMAPERCEPCIONES = SUELDOBRUTON + SEPTIMO + PRIDOMGRAVADA + PRIDOMEXENTA + TE2G + TE2E + TE3 + DESCANSOLABORADO + FESTIVOTRAB
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + FINJUSTIFICADA + PERMISOSINGOCEDESUELDO + INCREMENTORETENIDO + VACACIONESPRO + AGUINALDOGRA + AGUINALDOEXEN
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + PRIMAVACGRA + PRIMAVACEXEN - ValorIncapacidad
                            dtgDatos.Rows(x).Cells(55).Value = Math.Round(SUMAPERCEPCIONES, 2).ToString("###,##0.00")
                            SUMAPERCEPCIONESPISR = SUMAPERCEPCIONES - PRIDOMEXENTA - TE2E - AGUINALDOEXEN - PRIMAVACEXEN
                            dtgDatos.Rows(x).Cells(56).Value = Math.Round(SUMAPERCEPCIONESPISR, 2).ToString("###,##0.00")
                            Dim ADICIONALES As Double = PRIDOMGRAVADA + TE2G + TE3 + DESCANSOLABORADO + FESTIVOTRAB + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                            ADICIONALES = ADICIONALES + VACACIONESPRO + AGUINALDOGRA + PRIMAVACGRA
                            'ISR
                            If DiasCadaPeriodo = 7 Then
                                TipoPeriodoinfoonavit = 3
                                dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                            ElseIf DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                TipoPeriodoinfoonavit = 2
                                If EmpresaN = "NOSEOCUPARA" Then
                                    Dim diastra As Double = Double.Parse(dtgDatos.Rows(x).Cells(26).Value)
                                    Dim incapa As Double = Double.Parse(dtgDatos.Rows(x).Cells(28).Value)
                                    Dim falta As Double = Double.Parse(dtgDatos.Rows(x).Cells(20).Value)
                                    Dim permiso As Double = Double.Parse(dtgDatos.Rows(x).Cells(21).Value)
                                    Dim ISRT As Double = Double.Parse(isrmontodadosinsubsidio(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    Dim Subsidioaparte As Double = Double.Parse(subsidiocalculomensual(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    'If dtgDatos.Rows(x).Cells(2).Value = "58" Then
                                    '    MsgBox("llego")

                                    'End If
                                    If Subsidioaparte > ISRT Then

                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte)).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte - ISRT)).ToString("###,##0.00")
                                        End If

                                    Else
                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(69).Value = "0.00"
                                        End If

                                    End If


                                    If ISRT > Subsidioaparte Then
                                        ISRT = ISRT - Subsidioaparte
                                    Else
                                        ISRT = 0
                                    End If

                                    Dim ISRA As Double
                                    ISRA = 0
                                    If ADICIONALES > 0 Then
                                        ISRA = Double.Parse(isrmontodadosinsubsidio(ADICIONALES, 1, x))
                                    End If

                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(ISRT + ISRA, 2).ToString("###,##0.00")
                                Else
                                    'todos menos ademsa
                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                                End If








                            Else
                                TipoPeriodoinfoonavit = 1
                            End If


                            'IMSS
                            dtgDatos.Rows(x).Cells(59).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 1, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")


                            ' buscamos la pension
                            PensionAntesVariable = 0
                            sql = "select * from PensionAlimenticia where fkiIdEmpleadoC=" & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value) & " and iEstatus=1"
                            Dim rwPensionAntes As DataRow() = nConsulta(sql)

                            If rwPensionAntes Is Nothing = False Then

                                TotalPercepciones = SUMAPERCEPCIONES
                                Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", "0", dtgDatos.Rows(x).Cells(57).Value))
                                isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", "0", dtgDatos.Rows(x).Cells(58).Value))
                                imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", "0", dtgDatos.Rows(x).Cells(59).Value))
                                Dim SubtotalAntesPensioVariable As Double = TotalPercepciones - Incapacidad - isr - imss

                                pension = 0
                                For y As Integer = 0 To rwPensionAntes.Length - 1

                                    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    'Insertar la pension
                                    'Insertamos los datos

                                    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    'Id Empleado
                                    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    'id Pension
                                    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    'id Periodo
                                    sql &= ",'" & cboperiodo.SelectedValue
                                    'serie
                                    sql &= "'," & cboserie.SelectedIndex
                                    'tipo
                                    sql &= ",0"
                                    'Monto
                                    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    'Estatus
                                    sql &= ",1"
                                    sql &= "," & consecutivo1






                                    If nExecute(sql) = False Then
                                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    End If

                                    'If rwPensionAntes(y)("antesDescuento") = "1" Then

                                    '    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    '    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    '    'Insertar la pension
                                    '    'Insertamos los datos

                                    '    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    '    'Id Empleado
                                    '    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    '    'id Pension
                                    '    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    '    'id Periodo
                                    '    sql &= ",'" & cboperiodo.SelectedValue
                                    '    'serie
                                    '    sql &= "'," & cboserie.SelectedIndex
                                    '    'tipo
                                    '    sql &= "," & cboTipoNomina.SelectedIndex
                                    '    'Monto
                                    '    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    '    'Estatus
                                    '    sql &= ",1"
                                    '    sql &= "," & consecutivo1






                                    '    If nExecute(sql) = False Then
                                    '        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    '    End If

                                    'End If




                                Next
                                dtgDatos.Rows(x).Cells(63).Value = pension
                                PensionAntesVariable = pension
                            End If



                            'INFONAVIT
                            '##### VERIFICAR SI ESTA YA CALCULADO EL INFONAVIT DEL BIMESTRE
                            'Aqui verificamos si esta activo el calcular o no el infonavit



                            If chkNoinfonavit.Checked = False Then



                                If dtgDatos.Rows(x).Tag = "" Then
                                    'borramos el calculo previo del infonavit para tener siempre que generar el calculo por cualquier cambio que se requiera
                                    'este cambio va dentro de la funcion verificacalculoinfonavit

                                    If VerificarCalculoInfonavit(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)) = 2 Then
                                        Dim MontoInfonavit As Double = CalcularInfonavitMonto(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(25).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))
                                        If MontoInfonavit > 0 Then
                                            dtgDatos.Rows(x).Cells(60).Value = Math.Round(MontoInfonavit * DiasCadaPeriodo, 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                        End If
                                    Else
                                        dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                    End If




                                Else

                                End If
                            End If

                            'No laborado

                            'If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                            '    dtgDatos.Rows(x).Cells(36).Value = Math.Round((SDEMPLEADO / 8) - Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            'End If



                            'cuota sindical
                            If dtgDatos.Rows(x).Cells(5).Value = "SINDICALIZADO" Then
                                dtgDatos.Rows(x).Cells(67).Value = Math.Round((SUELDOBRUTON + SEPTIMO) * 0.015).ToString("###,##0.00")

                                'SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            Else
                                dtgDatos.Rows(x).Cells(67).Value = "0.00"
                            End If



                            INCAPACIDADD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", 0, dtgDatos.Rows(x).Cells(57).Value))
                            ISRD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", 0, dtgDatos.Rows(x).Cells(58).Value))
                            IMMSSD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", 0, dtgDatos.Rows(x).Cells(59).Value))
                            INFONAVITD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(60).Value = "", 0, dtgDatos.Rows(x).Cells(60).Value))
                            INFOBIMANT = Double.Parse(IIf(dtgDatos.Rows(x).Cells(61).Value = "", 0, dtgDatos.Rows(x).Cells(61).Value))
                            AJUSTEINFO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(62).Value = "", 0, dtgDatos.Rows(x).Cells(62).Value))
                            PENSIONAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(63).Value = "", 0, dtgDatos.Rows(x).Cells(63).Value))
                            PRESTAMOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(64).Value = "", 0, dtgDatos.Rows(x).Cells(64).Value))
                            FONACOTD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(65).Value = "", 0, dtgDatos.Rows(x).Cells(65).Value))
                            TNOLABORADOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(66).Value = "", 0, dtgDatos.Rows(x).Cells(66).Value))
                            CUOTASINDICALD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(67).Value = "", 0, dtgDatos.Rows(x).Cells(67).Value))
                            SUBSIDIOG = Double.Parse(IIf(dtgDatos.Rows(x).Cells(68).Value = "", 0, dtgDatos.Rows(x).Cells(68).Value))
                            SUBSIDIOA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(69).Value = "", 0, dtgDatos.Rows(x).Cells(69).Value))



                            'Verificar si tiene excedente y de que tipo
                            If NombrePeriodo = "Semanal" And EmpresaN = "IDN" Then
                                SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD + IMMSSD
                                dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                            Else
                                SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD
                                dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                            End If





                            sql = "select isnull( fsindicatoExtra,0) as  fsindicatoExtra from EmpleadosC where iIdEmpleadoC= " & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)

                            Dim rwDatos As DataRow() = nConsulta(sql)
                            If rwDatos Is Nothing = False Then
                                If Double.Parse(rwDatos(0)("fsindicatoExtra").ToString) > 0 Then
                                    Dim sumadescuentosexcedente As Double
                                    Dim excedenteperiodo As Double
                                    sumadescuentosexcedente = 0
                                    excedenteperiodo = 0
                                    If DiasCadaPeriodo > 7 Then
                                        excedenteperiodo = Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * diastrabajados

                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * diastrabajados, 2)
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(71).Value = "", 0, dtgDatos.Rows(x).Cells(71).Value))
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(72).Value = "", 0, dtgDatos.Rows(x).Cells(72).Value))
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(73).Value = "", 0, dtgDatos.Rows(x).Cells(73).Value))

                                        If sumadescuentosexcedente > excedenteperiodo Then
                                            MessageBox.Show("Los descuentos por excendente son mas que el mismo excedente, verifica ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Else
                                            excedenteperiodo = excedenteperiodo - sumadescuentosexcedente
                                            dtgDatos.Rows(x).Cells(74).Value = Math.Round(excedenteperiodo, 2)
                                        End If
                                    Else
                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * DiasCadaPeriodo, 2)
                                    End If

                                End If

                            End If

                            If chkDiasCS.Checked = True Then
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            Else
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            End If



                        End If




                    End If
                    'Fin calculo renglon
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################

                    '#############################################################################################################################




                End If

                'Dim cadena As String = dgvCombo.Text





                pgbProgreso.Value += 1
                Application.DoEvents()
            Next

            'verificar costo social

            Dim contador, Posicion1, Posicion2, Posicion3, Posicion4, Posicion5 As Integer



            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            pnlCatalogo.Enabled = True

        End Try
    End Sub

    Private Sub calcularexcedente()
        Dim Sueldo As Double
        Dim SueldoBase As Double
        Dim ValorIncapacidad As Double
        Dim TotalPercepciones As Double
        Dim Incapacidad As Double
        Dim isr As Double
        Dim imss As Double
        Dim infonavitvalor As Double
        Dim infonavitanterior As Double
        Dim ajusteinfonavit As Double
        Dim pension As Double
        Dim prestamo As Double
        Dim fonacot As Double
        Dim subsidiogenerado As Double
        Dim subsidioaplicado As Double
        Dim RetencionOperadora As Double
        Dim InfonavitNormal As Double
        Dim PrestamoPersonalAsimilados As Double
        Dim PrestamoPersonalSA As Double
        Dim AdeudoINfonavitAsimilados As Double
        Dim DiferenciaInfonavitAsimilados As Double
        Dim PensionAlimenticia As Double
        Dim PensionAlimenticiaInsertar As Double

        Dim Operadora As Double
        Dim ComplementoAsimilados As Double

        Dim SueldoBaseTMM As Double
        Dim CostoSocialTotal As Double
        Dim ComisionOperadora As Double
        Dim ComisionAsimilados As Double
        Dim subtotal As Double
        Dim iva As Double



        Dim sql As String
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Dim ValorUMA As Double
        Dim primavacacionesgravada As Double
        Dim primavacacionesexenta As Double
        Dim diastrabajados As Double
        Dim Sueldobruto As Double
        Dim TEFG As Double
        Dim TEFE As Double
        Dim TEO As Double
        Dim DSO As Double
        Dim VACAPRO As Double
        Dim numbimestre As Integer
        Dim NOCALCULAR As Boolean
        Dim consecutivo1 As String
        Dim plantaoNO As String

        Dim PensionAntesVariable As Double

        Try
            'verificamos que tenga dias a calcular
            'For x As Integer = 0 To dtgDatos.Rows.Count - 1
            '    If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value)) <= 0 Then
            '        MessageBox.Show("Existen trabajadores que no tiene dias trabajados, favor de verificar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'Next



            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If


            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count




            For x As Integer = 0 To dtgDatos.Rows.Count - 1
                NOCALCULAR = True

                If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
                    consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text))

                Else
                    consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value
                End If
                'verificar

                'verificamos los sueldos
                'sql = "Select salariod,sbc,salariodTopado,sbcTopado from costosocial inner join puestos on costosocial.fkiIdPuesto=puestos.iIdPuesto "
                'sql &= " where cNombre = '" & dtgDatos.Rows(x).Cells(11).FormattedValue & "' and anio=" & aniocostosocial

                'Dim rwDatosSalario As DataRow() = nConsulta(sql)

                'If rwDatosSalario Is Nothing = False Then
                '    If dtgDatos.Rows(x).Cells(10).Value >= 55 Then
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariodTopado")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbcTopado")
                '    Else
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariod")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbc")
                '    End If

                'Else
                '    MessageBox.Show("No se encontraron datos")
                'End If


                'validar que si ese desactivado el calculo y activa el trabajador
                If chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "1" Then
                    'activo para borrar los datos de esse trabajador y calcularlo despues
                    If 0 = 0 Then
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1


                        '' borrar el seguro si solo tiene un registro
                        'For x As Integer = 0 To dtgDatos.Rows.Count - 1
                        '    Dim ValorInfo As Double
                        '    ValorInfo = IIf(dtgDatos.Rows(x).Cells(14).Value = "", "0", dtgDatos.Rows(x).Cells(14).Value)
                        '    If ValorInfo > 0 Then
                        '        Dim numbimestre As Integer

                        '        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
                        '            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
                        '        Else
                        '            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
                        '        End If

                        '        sql = "select * from DetalleDescInfonavit inner join nomina on DetalleDescInfonavit.fkiIdEmpleado"
                        '        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue & "or fkiIdPeriodo = "
                        '        sql &= " and fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value

                        '    End If
                        'Next

                    Else
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and iTipo=0"
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and iTipoNomina=0"
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and iTipoNomina=0"
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and iTipoNomina=0"
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1
                    End If


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql2) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql3) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql4) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql5) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    'si calcular

                ElseIf chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "" Then
                    'No calcular
                    NOCALCULAR = False
                ElseIf chkCalSoloMarcados.Checked = False Then
                    'si calcular
                End If
                'If dtgDatos.Rows(x).Cells(2).Value = "704" Then
                '    MsgBox("aqui")
                'End If
                If NOCALCULAR Then
                    If dtgDatos.Rows(x).Cells(11).FormattedValue = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or dtgDatos.Rows(x).Cells(11).FormattedValue = "SUBALTERNO EN FORMACIÓN" Then






                        '####################################################################################################################################
                        '###################################################################################################################################
                        '##################################################################################################################################
                        '###############################################################################################################################
                        '###########################################################################################################################
                        '########################################################################################################################
                        '###################################################################################################################
                        '##########################################################################################################
                        '####################################################################################################
                        '#########################################################################################
                        '#############################################################################
                        '##################################################################

                        'Empieza el calculo normal
                    Else
                        diastrabajados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(26).Value = "", "0", dtgDatos.Rows(x).Cells(26).Value.ToString))
                        Dim SUELDOBRUTON As Double
                        Dim SEPTIMO As Double
                        Dim PRIDOMGRAVADA As Double
                        Dim PRIDOMEXENTA As Double
                        Dim TE2G As Double
                        Dim TE2E As Double
                        Dim TE3 As Double
                        Dim DESCANSOLABORADO As Double
                        Dim FESTIVOTRAB As Double
                        Dim BONOASISTENCIA As Double
                        Dim BONOPRODUCTIVIDAD As Double
                        Dim BONOPOLIVALENCIA As Double
                        Dim BONOESPECIALIDAD As Double
                        Dim BONOCALIDAD As Double
                        Dim COMPENSACION As Double
                        Dim SEMANAFONDO As Double
                        Dim INCREMENTORETENIDO As Double
                        Dim VACACIONESPRO As Double
                        Dim AGUINALDOGRA As Double
                        Dim AGUINALDOEXEN As Double
                        Dim PRIMAVACGRA As Double
                        Dim PRIMAVACEXEN As Double
                        Dim SUMAPERCEPCIONES As Double
                        Dim SUMAPERCEPCIONESPISR As Double
                        Dim FINJUSTIFICADA As Double
                        Dim PERMISOSINGOCEDESUELDO As Double
                        Dim PRIMADOMINICAL As Double
                        Dim SDEMPLEADO As Double
                        Dim SDEMPLEADOREAL As Double

                        Dim DiasCadaPeriodo As Integer
                        Dim FechaInicioPeriodo As Date
                        Dim FechaFinPeriodo As Date
                        Dim FechaAntiguedad As Date
                        Dim FechaBuscar As Date
                        Dim TipoPeriodoinfoonavit As Integer

                        Dim INCAPACIDADD As Double
                        Dim ISRD As Double
                        Dim IMMSSD As Double
                        Dim INFONAVITD As Double
                        Dim INFOBIMANT As Double
                        Dim AJUSTEINFO As Double
                        Dim PENSIONAD As Double
                        Dim PRESTAMOD As Double
                        Dim FONACOTD As Double
                        Dim TNOLABORADOD As Double
                        Dim CUOTASINDICALD As Double
                        Dim SUBSIDIOG As Double
                        Dim SUBSIDIOA As Double
                        Dim SUMADEDUCCIONES As Double
                        Dim dias As Integer
                        Dim BanPeriodo As Boolean
                        If diastrabajados = -10 Then


                        Else
                            dias = 0
                            BanPeriodo = False
                            sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                            Dim rwPeriodo As DataRow() = nConsulta(sql)

                            If rwPeriodo Is Nothing = False Then
                                FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                                FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                                DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                                sql = "select *"
                                sql &= " from empleadosC"
                                sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                                Dim rwDatosBanco As DataRow() = nConsulta(sql)


                                If rwDatosBanco Is Nothing = False Then
                                    FechaAntiguedad = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    FechaBuscar = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    If FechaBuscar.CompareTo(FechaInicioPeriodo) > 0 And FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then
                                        'Estamos dentro del rango 
                                        'Calculamos la prima

                                        dias = (DateDiff("y", FechaBuscar, FechaFinPeriodo)) + 1

                                        BanPeriodo = True

                                    ElseIf FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then


                                        BanPeriodo = False

                                    End If
                                End If

                            End If

                            ' dtgDatos.Rows(x).Cells(3).Style.BackColor = Color.Chocolate

                            SDEMPLEADO = Double.Parse(dtgDatos.Rows(x).Cells(24).Value)
                            SDEMPLEADOREAL = Double.Parse(dtgDatos.Rows(x).Cells(23).Value) / 30

                            'dtgDatos.Rows(x).Cells(21).Value = Math.Round(Sueldo * (26.19568006 / 100), 2).ToString("###,##0.00")

                            FINJUSTIFICADA = 0
                            PERMISOSINGOCEDESUELDO = 0
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value))
                                If NombrePeriodo = "Quincenal" Then
                                    diastrabajados = diastrabajados - FINJUSTIFICADA
                                Else

                                    diastrabajados = 6
                                End If

                                dtgDatos.Rows(x).Cells(45).Value = "-" + Math.Round(SDEMPLEADO * FINJUSTIFICADA, 2).ToString("###,##0.00")
                                'Mandar la falta a la resta
                            Else
                                dtgDatos.Rows(x).Cells(45).Value = 0.0
                            End If

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value))
                                If NombrePeriodo = "Quincenal" Then
                                    diastrabajados = diastrabajados - PERMISOSINGOCEDESUELDO
                                Else

                                    diastrabajados = 6
                                End If
                                dtgDatos.Rows(x).Cells(46).Value = "-" + Math.Round(SDEMPLEADO * PERMISOSINGOCEDESUELDO, 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(46).Value = 0.0
                            End If
                            If BanPeriodo Then
                                diastrabajados = dias - 1
                            End If
                            'solo falta injustificada juega para el septimo dia
                            If NombrePeriodo = "Quincenal" Then

                                dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * Double.Parse(dtgDatos.Rows(x).Cells(26).Value), 2).ToString("###,##0.00")
                                'dtgDatos.Rows(x).Cells(26).Value = "15"
                                dtgDatos.Rows(x).Cells(30).Value = "0.00"
                            ElseIf NombrePeriodo = "Semanal" Then

                                'If dtgDatos.Rows(x).Cells(2).Value = "42" Then
                                'MsgBox("llego")
                                ' End If
                                If chkDias.Checked = False Then
                                    dtgDatos.Rows(x).Cells(26).Value = "7"
                                End If


                                If diastrabajados = 7 Then
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * 6, 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                    If ValorIncapacidad = 6 Then
                                        dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                    End If
                                Else
                                    'dtgDatos.Rows(x).Cells(29).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(24).Value) * (diastrabajados - FINJUSTIFICADA - PERMISOSINGOCEDESUELDO), 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * (diastrabajados), 2).ToString("###,##0.00")
                                    If BanPeriodo Then
                                        dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    Else
                                        If PERMISOSINGOCEDESUELDO > 0 Then

                                        End If
                                        'sacar factor de septimo dia solo en el caso de falta injustifica 
                                        If (diastrabajados - FINJUSTIFICADA) = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = SDEMPLEADO
                                        Else
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO * (0.166 * (diastrabajados - FINJUSTIFICADA)), 2).ToString("###,##0.00")
                                        End If


                                        If PERMISOSINGOCEDESUELDO = 7 Then
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                        End If
                                        ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                        If ValorIncapacidad = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                        End If

                                    End If

                                End If

                            End If


                            'Incapacidad
                            ValorIncapacidad = 0.0
                            If dtgDatos.Rows(x).Cells(2).Value = 91 Then
                                MsgBox("lol")
                            End If
                            If dtgDatos.Rows(x).Cells(27).Value <> "Ninguno" Then

                                ValorIncapacidad = dtgDatos.Rows(x).Cells(28).Value * SDEMPLEADO

                            End If
                            dtgDatos.Rows(x).Cells(57).Value = Math.Round(ValorIncapacidad, 2).ToString("###,##0.00")

                            'PrimaDominical
                            If chkPrimaDominical.Checked = False Then
                                If Double.Parse(IIf(dtgDatos.Rows(x).Cells(19).Value = "", 0, dtgDatos.Rows(x).Cells(19).Value)) > 0 Then
                                    PRIMADOMINICAL = Double.Parse(dtgDatos.Rows(x).Cells(19).Value) * SDEMPLEADOREAL * 0.25
                                    If PRIMADOMINICAL > ValorUMA Then
                                        dtgDatos.Rows(x).Cells(31).Value = Math.Round(PRIMADOMINICAL - ValorUMA, 2).ToString("###,##0.00")
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(ValorUMA, 2).ToString("###,##0.00")
                                    Else
                                        dtgDatos.Rows(x).Cells(31).Value = "0.00"
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(PRIMADOMINICAL, 2).ToString("###,##0.00")
                                    End If
                                End If
                            End If


                            'Tiempo Extra Doble
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(15).Value = "", 0, dtgDatos.Rows(x).Cells(15).Value)) > 0 Then


                                dtgDatos.Rows(x).Cells(33).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADOREAL / 8) * 2) / 2, 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(34).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADOREAL / 8) * 2) / 2, 2).ToString("###,##0.00")

                            Else
                                dtgDatos.Rows(x).Cells(33).Value = "0.00"
                                dtgDatos.Rows(x).Cells(34).Value = "0.00"
                            End If

                            'Tiempo Extra triple
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(16).Value = "", 0, dtgDatos.Rows(x).Cells(16).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(35).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(16).Value) * (SDEMPLEADOREAL / 8) * 3), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(35).Value = "0.00"
                            End If

                            'Descanso Laborado

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(17).Value = "", 0, dtgDatos.Rows(x).Cells(17).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(36).Value = Math.Round(SDEMPLEADOREAL * 2 * Double.Parse(dtgDatos.Rows(x).Cells(17).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(36).Value = "0.00"
                            End If
                            'Dia Festivo laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", 0, dtgDatos.Rows(x).Cells(18).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(37).Value = Math.Round(SDEMPLEADOREAL * 2 * Double.Parse(dtgDatos.Rows(x).Cells(18).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(37).Value = "0.00"
                            End If

                            'Tiempo No laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(66).Value = Math.Round(SDEMPLEADOREAL / 8 * Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(66).Value = "0.00"
                            End If

                            'Calcular la prima
                            If chkPrimaVacacional.Checked = False Then
                                If DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)

                                ElseIf DiasCadaPeriodo = 6 Or DiasCadaPeriodo = 7 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)
                                End If
                            End If



                            'sumar Para ISR

                            SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            SEPTIMO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(30).Value = "", 0, dtgDatos.Rows(x).Cells(30).Value))
                            PRIDOMGRAVADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(31).Value = "", 0, dtgDatos.Rows(x).Cells(31).Value))
                            PRIDOMEXENTA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(32).Value = "", 0, dtgDatos.Rows(x).Cells(32).Value))
                            TE2G = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", 0, dtgDatos.Rows(x).Cells(33).Value))
                            TE2E = Double.Parse(IIf(dtgDatos.Rows(x).Cells(34).Value = "", 0, dtgDatos.Rows(x).Cells(34).Value))
                            TE3 = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", 0, dtgDatos.Rows(x).Cells(35).Value))
                            DESCANSOLABORADO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", 0, dtgDatos.Rows(x).Cells(36).Value))
                            FESTIVOTRAB = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", 0, dtgDatos.Rows(x).Cells(37).Value))
                            BONOASISTENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", 0, dtgDatos.Rows(x).Cells(38).Value))
                            BONOPRODUCTIVIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", 0, dtgDatos.Rows(x).Cells(39).Value))
                            BONOPOLIVALENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", 0, dtgDatos.Rows(x).Cells(40).Value))
                            BONOESPECIALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", 0, dtgDatos.Rows(x).Cells(41).Value))
                            BONOCALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", 0, dtgDatos.Rows(x).Cells(42).Value))
                            COMPENSACION = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", 0, dtgDatos.Rows(x).Cells(43).Value))
                            SEMANAFONDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", 0, dtgDatos.Rows(x).Cells(44).Value))
                            FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", 0, dtgDatos.Rows(x).Cells(45).Value))
                            PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(46).Value = "", 0, dtgDatos.Rows(x).Cells(46).Value))
                            INCREMENTORETENIDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", 0, dtgDatos.Rows(x).Cells(47).Value))
                            VACACIONESPRO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", 0, dtgDatos.Rows(x).Cells(48).Value))
                            AGUINALDOGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", 0, dtgDatos.Rows(x).Cells(49).Value))
                            AGUINALDOEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(50).Value = "", 0, dtgDatos.Rows(x).Cells(50).Value))
                            PRIMAVACGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(52).Value = "", 0, dtgDatos.Rows(x).Cells(52).Value))
                            PRIMAVACEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(53).Value = "", 0, dtgDatos.Rows(x).Cells(53).Value))



                            SUMAPERCEPCIONES = SUELDOBRUTON + SEPTIMO
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + SEMANAFONDO
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + FINJUSTIFICADA + PERMISOSINGOCEDESUELDO + INCREMENTORETENIDO + VACACIONESPRO + AGUINALDOGRA + AGUINALDOEXEN
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + PRIMAVACGRA + PRIMAVACEXEN - ValorIncapacidad
                            dtgDatos.Rows(x).Cells(55).Value = Math.Round(SUMAPERCEPCIONES, 2).ToString("###,##0.00")
                            SUMAPERCEPCIONESPISR = SUMAPERCEPCIONES
                            dtgDatos.Rows(x).Cells(56).Value = Math.Round(SUMAPERCEPCIONESPISR, 2).ToString("###,##0.00")
                            Dim ADICIONALES As Double = PRIDOMGRAVADA + TE2G + TE3 + DESCANSOLABORADO + FESTIVOTRAB + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                            ADICIONALES = ADICIONALES + VACACIONESPRO + AGUINALDOGRA + PRIMAVACGRA
                            'ISR
                            If DiasCadaPeriodo = 7 Then
                                TipoPeriodoinfoonavit = 3
                                dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                            ElseIf DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                TipoPeriodoinfoonavit = 2
                                If EmpresaN = "NOSEOCUPARA" Then
                                    Dim diastra As Double = Double.Parse(dtgDatos.Rows(x).Cells(26).Value)
                                    Dim incapa As Double = Double.Parse(dtgDatos.Rows(x).Cells(28).Value)
                                    Dim falta As Double = Double.Parse(dtgDatos.Rows(x).Cells(20).Value)
                                    Dim permiso As Double = Double.Parse(dtgDatos.Rows(x).Cells(21).Value)
                                    Dim ISRT As Double = Double.Parse(isrmontodadosinsubsidio(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    Dim Subsidioaparte As Double = Double.Parse(subsidiocalculomensual(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    'If dtgDatos.Rows(x).Cells(2).Value = "58" Then
                                    '    MsgBox("llego")

                                    'End If
                                    If Subsidioaparte > ISRT Then

                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte)).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte - ISRT)).ToString("###,##0.00")
                                        End If

                                    Else
                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(69).Value = "0.00"
                                        End If

                                    End If


                                    If ISRT > Subsidioaparte Then
                                        ISRT = ISRT - Subsidioaparte
                                    Else
                                        ISRT = 0
                                    End If

                                    Dim ISRA As Double
                                    ISRA = 0
                                    If ADICIONALES > 0 Then
                                        ISRA = Double.Parse(isrmontodadosinsubsidio(ADICIONALES, 1, x))
                                    End If

                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(ISRT + ISRA, 2).ToString("###,##0.00")
                                Else
                                    'todos menos ademsa
                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                                End If








                            Else
                                TipoPeriodoinfoonavit = 1
                            End If


                            'IMSS
                            dtgDatos.Rows(x).Cells(59).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 1, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")


                            ' buscamos la pension
                            PensionAntesVariable = 0
                            sql = "select * from PensionAlimenticia where fkiIdEmpleadoC=" & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value) & " and iEstatus=1"
                            Dim rwPensionAntes As DataRow() = nConsulta(sql)

                            If rwPensionAntes Is Nothing = False Then

                                TotalPercepciones = SUMAPERCEPCIONES
                                Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", "0", dtgDatos.Rows(x).Cells(57).Value))
                                isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", "0", dtgDatos.Rows(x).Cells(58).Value))
                                imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", "0", dtgDatos.Rows(x).Cells(59).Value))
                                Dim SubtotalAntesPensioVariable As Double = TotalPercepciones - Incapacidad - isr - imss

                                pension = 0
                                For y As Integer = 0 To rwPensionAntes.Length - 1

                                    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    'Insertar la pension
                                    'Insertamos los datos

                                    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    'Id Empleado
                                    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    'id Pension
                                    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    'id Periodo
                                    sql &= ",'" & cboperiodo.SelectedValue
                                    'serie
                                    sql &= "'," & cboserie.SelectedIndex
                                    'tipo
                                    sql &= ",0"
                                    'Monto
                                    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    'Estatus
                                    sql &= ",1"
                                    sql &= "," & consecutivo1






                                    If nExecute(sql) = False Then
                                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    End If

                                    'If rwPensionAntes(y)("antesDescuento") = "1" Then

                                    '    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    '    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    '    'Insertar la pension
                                    '    'Insertamos los datos

                                    '    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    '    'Id Empleado
                                    '    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    '    'id Pension
                                    '    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    '    'id Periodo
                                    '    sql &= ",'" & cboperiodo.SelectedValue
                                    '    'serie
                                    '    sql &= "'," & cboserie.SelectedIndex
                                    '    'tipo
                                    '    sql &= "," & cboTipoNomina.SelectedIndex
                                    '    'Monto
                                    '    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    '    'Estatus
                                    '    sql &= ",1"
                                    '    sql &= "," & consecutivo1






                                    '    If nExecute(sql) = False Then
                                    '        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    '    End If

                                    'End If




                                Next
                                dtgDatos.Rows(x).Cells(63).Value = pension
                                PensionAntesVariable = pension
                            End If



                            'INFONAVIT
                            '##### VERIFICAR SI ESTA YA CALCULADO EL INFONAVIT DEL BIMESTRE
                            'Aqui verificamos si esta activo el calcular o no el infonavit



                            If chkNoinfonavit.Checked = False Then



                                If dtgDatos.Rows(x).Tag = "" Then
                                    'borramos el calculo previo del infonavit para tener siempre que generar el calculo por cualquier cambio que se requiera
                                    'este cambio va dentro de la funcion verificacalculoinfonavit

                                    If VerificarCalculoInfonavit(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)) = 2 Then
                                        Dim MontoInfonavit As Double = CalcularInfonavitMonto(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(25).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))
                                        If MontoInfonavit > 0 Then
                                            dtgDatos.Rows(x).Cells(60).Value = Math.Round(MontoInfonavit * DiasCadaPeriodo, 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                        End If
                                    Else
                                        dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                    End If




                                Else

                                End If
                            End If

                            'No laborado

                            'If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                            '    dtgDatos.Rows(x).Cells(36).Value = Math.Round((SDEMPLEADO / 8) - Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            'End If



                            'cuota sindical
                            If dtgDatos.Rows(x).Cells(5).Value = "SINDICALIZADO" Then
                                dtgDatos.Rows(x).Cells(67).Value = Math.Round((SUELDOBRUTON + SEPTIMO) * 0.015).ToString("###,##0.00")

                                'SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            Else
                                dtgDatos.Rows(x).Cells(67).Value = "0.00"
                            End If



                            INCAPACIDADD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", 0, dtgDatos.Rows(x).Cells(57).Value))
                            ISRD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", 0, dtgDatos.Rows(x).Cells(58).Value))
                            IMMSSD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", 0, dtgDatos.Rows(x).Cells(59).Value))
                            INFONAVITD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(60).Value = "", 0, dtgDatos.Rows(x).Cells(60).Value))
                            INFOBIMANT = Double.Parse(IIf(dtgDatos.Rows(x).Cells(61).Value = "", 0, dtgDatos.Rows(x).Cells(61).Value))
                            AJUSTEINFO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(62).Value = "", 0, dtgDatos.Rows(x).Cells(62).Value))
                            PENSIONAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(63).Value = "", 0, dtgDatos.Rows(x).Cells(63).Value))
                            PRESTAMOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(64).Value = "", 0, dtgDatos.Rows(x).Cells(64).Value))
                            FONACOTD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(65).Value = "", 0, dtgDatos.Rows(x).Cells(65).Value))
                            TNOLABORADOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(66).Value = "", 0, dtgDatos.Rows(x).Cells(66).Value))
                            CUOTASINDICALD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(67).Value = "", 0, dtgDatos.Rows(x).Cells(67).Value))
                            SUBSIDIOG = Double.Parse(IIf(dtgDatos.Rows(x).Cells(68).Value = "", 0, dtgDatos.Rows(x).Cells(68).Value))
                            SUBSIDIOA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(69).Value = "", 0, dtgDatos.Rows(x).Cells(69).Value))



                            'Verificar si tiene excedente y de que tipo
                            If NombrePeriodo = "Semanal" And EmpresaN = "IDN" Then
                                SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD + IMMSSD
                                dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                            Else
                                SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD
                                dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                            End If





                            sql = "select isnull( fsindicatoExtra,0) as  fsindicatoExtra from EmpleadosC where iIdEmpleadoC= " & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)

                            Dim rwDatos As DataRow() = nConsulta(sql)
                            If rwDatos Is Nothing = False Then
                                If Double.Parse(rwDatos(0)("fsindicatoExtra").ToString) > 0 Then
                                    Dim sumadescuentosexcedente As Double
                                    Dim excedenteperiodo As Double
                                    sumadescuentosexcedente = 0
                                    excedenteperiodo = 0
                                    If DiasCadaPeriodo > 7 Then
                                        excedenteperiodo = Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * diastrabajados

                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * diastrabajados, 2)
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(71).Value = "", 0, dtgDatos.Rows(x).Cells(71).Value))
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(72).Value = "", 0, dtgDatos.Rows(x).Cells(72).Value))
                                        sumadescuentosexcedente += Double.Parse(IIf(dtgDatos.Rows(x).Cells(73).Value = "", 0, dtgDatos.Rows(x).Cells(73).Value))

                                        If sumadescuentosexcedente > excedenteperiodo Then
                                            MessageBox.Show("Los descuentos por excendente son mas que el mismo excedente, verifica ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Else
                                            excedenteperiodo = excedenteperiodo - sumadescuentosexcedente
                                            dtgDatos.Rows(x).Cells(74).Value = Math.Round(excedenteperiodo, 2)
                                        End If
                                    Else
                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * DiasCadaPeriodo, 2)
                                    End If

                                End If

                            End If

                            If chkDiasCS.Checked = True Then
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            Else
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            End If



                        End If




                    End If
                    'Fin calculo renglon
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################

                    '#############################################################################################################################




                End If

                'Dim cadena As String = dgvCombo.Text





                pgbProgreso.Value += 1
                Application.DoEvents()
            Next

            'verificar costo social

            Dim contador, Posicion1, Posicion2, Posicion3, Posicion4, Posicion5 As Integer



            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            pnlCatalogo.Enabled = True

        End Try
    End Sub

    Function calculoimss(sdi As Double, totalp As Double, tipo As Integer, uma As Double, diasimss As Integer, valorISN As Double) As Double

        Dim SQL As String
        Dim TC25 As Double = Double.Parse(uma) * 25
        Dim TC22 As Double = Double.Parse(uma) * 25
        Dim TC15 As Double = Double.Parse(uma) * 15
        Dim TCF As Double = Double.Parse(uma) * 3
        Dim imss As Double
        Dim tope As Double
        Dim cuotafija As Double
        Dim excedentep As Double
        Dim excedentet As Double
        Dim prestaciondinerop As Double
        Dim prestaciondinerot As Double
        Dim gastosmedicosp As Double
        Dim gastosmedicost As Double
        Dim riesgotrabajo As Double
        Dim invalidezp As Double
        Dim invalidezt As Double
        Dim guarderia As Double
        Dim cuotasimssp As Double
        Dim cuotasimsst As Double
        Dim totalcuotasimss As Double
        Dim retiro As Double
        Dim vejezp As Double
        Dim vejezt As Double
        Dim tope22 As Double
        Dim totalrcvp As Double
        Dim totalrcvt As Double
        Dim infonavitp As Double
        Dim impuestonomina As Double
        Dim totaltotalp As Double
        Dim totaltotalt As Double
        Dim costosocial As Double
        Dim FactorRiesgo As Double
        Try
            SQL = "select * from enfermedades where anio=" & aniocostosocial
            Dim rwIMSS As DataRow() = nConsulta(SQL)
            If rwIMSS Is Nothing = False Then
                'tope
                If sdi > TC25 Then
                    tope = TC25
                Else
                    tope = sdi
                End If
                'cuota fija patron
                If sdi > 0 Then
                    cuotafija = Double.Parse(uma) * Double.Parse(diasimss) * (Double.Parse(rwIMSS(0)("cfpatron").ToString) / 100)
                Else
                    cuotafija = 0
                End If
                'excendente patron
                If tope > TCF Then
                    excedentep = (tope - TCF) * Double.Parse(diasimss) * (Double.Parse(rwIMSS(0)("excedentep").ToString) / 100)
                Else
                    excedentep = 0
                End If
                'excedente trabajador
                If tope > TCF Then
                    excedentet = (tope - TCF) * Double.Parse(diasimss) * (Double.Parse(rwIMSS(0)("excedentet").ToString) / 100)
                Else
                    excedentet = 0
                End If

                'PRESTACIONES DE DINERO
                'patron

                If tope > Double.Parse(uma) Then
                    prestaciondinerop = (tope * (Double.Parse(rwIMSS(0)("prestaciondp").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    prestaciondinerop = (tope * ((Double.Parse(rwIMSS(0)("prestaciondp").ToString) / 100) + (Double.Parse(rwIMSS(0)("prestaciondt").ToString) / 100))) * Double.Parse(diasimss)
                End If
                'trabajador
                If tope > Double.Parse(uma) Then
                    prestaciondinerot = (tope * (Double.Parse(rwIMSS(0)("prestaciondt").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    prestaciondinerot = 0
                End If
                'GASTOS MEDICOS
                'patron
                If tope > Double.Parse(uma) Then
                    gastosmedicosp = (tope * (Double.Parse(rwIMSS(0)("prestacionep").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    gastosmedicosp = (tope * ((Double.Parse(rwIMSS(0)("prestacionep").ToString) / 100) + (Double.Parse(rwIMSS(0)("prestacionet").ToString) / 100))) * Double.Parse(diasimss)
                End If
                'trabajador
                If tope > Double.Parse(uma) Then
                    gastosmedicost = (tope * (Double.Parse(rwIMSS(0)("prestacionet").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    gastosmedicost = 0
                End If

                'RIESGO DE TRABAJO

                SQL = "select * from Salario "
                SQL &= " where Anio=" & aniocostosocial
                SQL &= " and iEstatus=1"
                Dim rwValorUMA As DataRow() = nConsulta(SQL)
                If rwValorUMA Is Nothing = False Then
                    FactorRiesgo = Double.Parse(rwValorUMA(0)("Factorriesgo").ToString)
                Else
                    FactorRiesgo = 0
                    MessageBox.Show("No se encontro factor de riesgo de la empresa en el año: " & aniocostosocial)
                End If

                riesgotrabajo = Double.Parse(diasimss) * tope * FactorRiesgo

                'TOPE 22
                If sdi > TC22 Then
                    tope22 = TC22
                Else
                    tope22 = sdi
                End If

                'INVALIDEZ Y VIDA
                'patron
                If tope22 > Double.Parse(uma) Then
                    invalidezp = (tope22 * (Double.Parse(rwIMSS(0)("invalidezp").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    invalidezp = (tope22 * ((Double.Parse(rwIMSS(0)("invalidezp").ToString) / 100) + (Double.Parse(rwIMSS(0)("invalidezt").ToString) / 100))) * Double.Parse(diasimss)
                End If
                'trabajador
                If tope22 > Double.Parse(uma) Then
                    invalidezt = (tope22 * (Double.Parse(rwIMSS(0)("invalidezt").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    invalidezt = 0
                End If

                'GUARDERIAS
                guarderia = tope * (Double.Parse(rwIMSS(0)("guarderiasp").ToString) / 100) * Double.Parse(diasimss)

                'CUOTAS IMSS
                'patron
                cuotasimssp = cuotafija + excedentep + prestaciondinerop + gastosmedicosp + riesgotrabajo + invalidezp + guarderia

                'trabajador

                cuotasimsst = excedentet + prestaciondinerot + gastosmedicost + invalidezt

                'TOTAL CUOTAS IMSS
                totalcuotasimss = cuotasimssp + cuotasimsst

                'RETIRO
                retiro = tope * Double.Parse(diasimss) * (Double.Parse(rwIMSS(0)("retirop").ToString) / 100)

                'VEJEZ
                'patron
                If tope22 > Double.Parse(uma) Then
                    vejezp = (tope22 * (Double.Parse(rwIMSS(0)("cesantiap").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    vejezp = (tope22 * ((Double.Parse(rwIMSS(0)("cesantiat").ToString) / 100) + (Double.Parse(rwIMSS(0)("invalidezt").ToString) / 100))) * Double.Parse(diasimss)
                End If
                'trabajador
                If tope22 > Double.Parse(uma) Then
                    vejezt = (tope22 * (Double.Parse(rwIMSS(0)("cesantiat").ToString) / 100)) * Double.Parse(diasimss)
                Else
                    vejezt = 0
                End If

                'TOTALRCV PATRON
                totalrcvp = retiro + vejezp

                'TOTALRCV TRABAJADOR

                totalrcvt = vejezt

                'INFONAVIT PATRON

                infonavitp = tope22 * Double.Parse(diasimss) * (Double.Parse(rwIMSS(0)("infonavitp").ToString) / 100)

                'IMPUESTO SOBRE NOMINA

                impuestonomina = (Double.Parse(valorISN) / 100) * totalp

                'TOTAL PATRON
                totaltotalp = cuotasimssp + totalrcvp + infonavitp + impuestonomina

                'TOTAL TRABAJADOR
                totaltotalt = cuotasimsst + totalrcvt

                'COSTO SOCIAL
                costosocial = totaltotalp + totaltotalt
                If tipo = 2 Then
                    totaltotalt = cuotasimssp
                ElseIf tipo = 3 Then
                    totaltotalt = totalrcvp
                ElseIf tipo = 4 Then
                    totaltotalt = infonavitp
                ElseIf tipo = 5 Then
                    totaltotalt = impuestonomina

                ElseIf tipo = 6 Then
                    totaltotalt = retiro


                ElseIf tipo = 7 Then
                    totaltotalt = vejezp
                End If

                Return totaltotalt
            Else
                MsgBox("No hay datos")
            End If


        Catch ex As Exception

        End Try

        Return 0
    End Function


    Private Sub calcularcostosocial(posicion1 As Integer, dias As Integer)
        Dim Sql = "select * from puestos inner join costosocial on puestos.iidPuesto= costosocial.fkiIdPuesto where puestos.cnombre='" & dtgDatos.Rows(posicion1).Cells(11).FormattedValue & "' and anio=" & aniocostosocial
        Dim rwCostoSocial As DataRow() = nConsulta(Sql)
        If rwCostoSocial Is Nothing = False Then
            If dtgDatos.Rows(Posicion1).Cells(10).Value >= 55 Then

                If dtgDatos.Rows(Posicion1).Cells(5).Tag = "" Then
                    'verificar los dias del mes
                    dtgDatos.Rows(Posicion1).Cells(55).Value = Math.Round(Double.Parse(rwCostoSocial(0)("imsstopado")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(56).Value = Math.Round(Double.Parse(rwCostoSocial(0)("RCVtopado")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(57).Value = Math.Round(Double.Parse(rwCostoSocial(0)("infonavittopado")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(58).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 + (Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 * 0.33), 2)
                    dtgDatos.Rows(Posicion1).Cells(59).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(55).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(56).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(57).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(58).Value), 2)
                Else
                    dtgDatos.Rows(Posicion1).Cells(55).Value = Math.Round(Double.Parse(rwCostoSocial(0)("imsstopado")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(56).Value = Math.Round(Double.Parse(rwCostoSocial(0)("RCVtopado")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(57).Value = Math.Round(Double.Parse(rwCostoSocial(0)("infonavittopado")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(58).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 + (Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 * 0.33), 2)
                    dtgDatos.Rows(Posicion1).Cells(59).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(55).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(56).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(57).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(58).Value), 2)
                End If
            Else
                If dtgDatos.Rows(Posicion1).Cells(5).Tag = "" Then
                    dtgDatos.Rows(Posicion1).Cells(55).Value = Math.Round(Double.Parse(rwCostoSocial(0)("imss")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(56).Value = Math.Round(Double.Parse(rwCostoSocial(0)("RCV")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(57).Value = Math.Round(Double.Parse(rwCostoSocial(0)("Infonavit")) / 30 * dias, 2)
                    dtgDatos.Rows(Posicion1).Cells(58).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 + (Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 * 0.33), 2)
                    dtgDatos.Rows(Posicion1).Cells(59).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(55).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(56).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(57).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(58).Value), 2)
                Else
                    dtgDatos.Rows(Posicion1).Cells(55).Value = Math.Round(Double.Parse(rwCostoSocial(0)("imss")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(56).Value = Math.Round(Double.Parse(rwCostoSocial(0)("RCV")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(57).Value = Math.Round(Double.Parse(rwCostoSocial(0)("Infonavit")) / 30 * dtgDatos.Rows(Posicion1).Cells(18).Value, 2)
                    dtgDatos.Rows(Posicion1).Cells(58).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 + (Double.Parse(dtgDatos.Rows(Posicion1).Cells(33).Value) * 0.03 * 0.33), 2)
                    dtgDatos.Rows(Posicion1).Cells(59).Value = Math.Round(Double.Parse(dtgDatos.Rows(Posicion1).Cells(55).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(56).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(57).Value) + Double.Parse(dtgDatos.Rows(Posicion1).Cells(58).Value), 2)
                End If
            End If
            dtgDatos.Rows(Posicion1).Cells(59).Style.BackColor = Color.Chocolate
            dtgDatos.Rows(Posicion1).Cells(59).Tag = "1"
        End If
    End Sub

    Function Bisiesto(Num As Integer) As Boolean
        If Num Mod 4 = 0 And (Num Mod 100 Or Num Mod 400 = 0) Then
            Bisiesto = True
        Else
            Bisiesto = False
        End If
    End Function
    Function DiasMes(NumPeriodo As Integer) As Integer
        Try
            Dim sql As String

            Dim FechaInicioPeriodo1 As Date
            Dim dias As Integer
            sql = "select * from periodos where iIdPeriodo= " & NumPeriodo
            Dim rwPeriodo As DataRow() = nConsulta(Sql)
            dias = 0
            If rwPeriodo Is Nothing = False Then
                FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                dias = DateTime.DaysInMonth(Year(FechaInicioPeriodo1), Month(FechaInicioPeriodo1))





            End If
            Return dias

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Private Function infonavit(tipo As String, valor As Double, sdi As Double, fechapago As Date, periodo As String, diastrabajados As Integer, idempleado As Integer, consecutivo As Integer) As Double
        Try
            Dim numbimestre As Integer
            Dim numbimestre2 As Integer
            Dim numdias As Integer
            Dim numdias2 As Integer
            Dim DiasCadaPeriodo As Integer
            Dim DiasCadaPeriodo2 As Integer
            Dim diasfebrero As Integer
            Dim valorinfonavit As Double
            Dim sql As String
            Dim FechaInicioPeriodo1 As Date
            Dim FechaFinPeriodo1 As Date
            Dim FechaInicioPeriodo2 As Date
            Dim FechaFinPeriodo2 As Date
            Dim Seguro1 As Double
            Dim Seguro2 As Double
            Dim ValorInfonavitTabla As Double
            Dim contador As Integer

            'Validamos si el trabajador tiene o no activo el infonavit
            sql = "select iPermanente from empleadosC where iIdEmpleadoC=" & idempleado
            Dim rwCalcularInfonavit As DataRow() = nConsulta(sql)
            If rwCalcularInfonavit Is Nothing = False Then
                If rwCalcularInfonavit(0)("iPermanente") = "1" Then
                    sql = "select * from periodos where iIdPeriodo= " & periodo
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then

                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                        FechaFinPeriodo2 = Date.Parse(rwPeriodo(0)("dFechaFin"))
                        'DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo1, FechaFinPeriodo2) + 1






                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If

                        If numbimestre = 1 Then
                            If Bisiesto(Year(FechaInicioPeriodo1)) = True Then
                                diasfebrero = 29
                            Else
                                diasfebrero = 28
                            End If
                            'diasfebrero = Day(DateSerial(Year(fechapago), 3, 0))
                            numdias = 31 + diasfebrero
                        End If

                        If numbimestre = 2 Then
                            numdias = 61
                        End If

                        If numbimestre = 3 Then
                            numdias = 61
                        End If

                        If numbimestre = 4 Then
                            numdias = 62
                        End If

                        If numbimestre = 5 Then
                            numdias = 61
                        End If

                        If numbimestre = 6 Then
                            numdias = 61
                        End If






                        DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo1, FechaFinPeriodo2) + 1

                        'Verificamos si ya existe el seguro en ese bimestre

                        sql = "select * from PagoSeguroInfonavit where fkiIdEmpleadoC= " & idempleado
                        sql &= " And NumBimestre= " & numbimestre & " And Anio=" & FechaInicioPeriodo1.Year.ToString
                        Dim rwSeguro1 As DataRow() = nConsulta(sql)

                        If rwSeguro1 Is Nothing = False Then
                            Seguro1 = 0
                        Else
                            Seguro1 = 15

                        End If





                        sql = "select * from Salario "
                        sql &= " where Anio=" & FechaFinPeriodo2.Year.ToString
                        sql &= " and iEstatus=1"
                        Dim rwValorInfonavit As DataRow() = nConsulta(sql)

                        If rwValorInfonavit Is Nothing = False Then
                            ValorInfonavitTabla = rwValorInfonavit(0)("infonavit")
                        Else
                            MsgBox("No hay valor infonavit para este año, verificar tabla de salarios")
                        End If



                        If tipo = "VSM" And valor > 0 Then
                            valorinfonavit = (((ValorInfonavitTabla * valor * 2) / numdias) * DiasCadaPeriodo) + Seguro1
                            valorinfonavit = valorinfonavit + ((((ValorInfonavitTabla * valor * 2) / numdias2) * DiasCadaPeriodo2) + IIf(DiasCadaPeriodo2 = 0, 0, Seguro2))
                        End If

                        If tipo = "CUOTA FIJA" And valor > 0 Then


                            valorinfonavit = (((valor * 2) / numdias) * DiasCadaPeriodo) + Seguro1
                            valorinfonavit = valorinfonavit + ((((valor * 2) / numdias2) * DiasCadaPeriodo2) + IIf(DiasCadaPeriodo2 = 0, 0, Seguro2))

                        End If

                        If tipo = "PORCENTAJE" And valor > 0 Then

                            valorinfonavit = ((sdi * (valor / 100) * numdias) + 15) / numdias
                        End If


                        Return valorinfonavit

                    End If

                End If

            End If


            Return 0



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Private Function CalcularInfonavit(tipo As String, valor As Double, sdi As Double, fechapago As Date, periodo As String, idempleado As Integer) As Boolean
        Try
            Dim numbimestre As Integer

            Dim numdias As Integer

            Dim DiasCadaPeriodo As Integer

            Dim diasfebrero As Integer
            Dim valorinfonavit As Double
            Dim sql As String
            Dim FechaInicioPeriodo1 As Date
            Dim FechaFinPeriodo1 As Date
            Dim FechaInicioPeriodo2 As Date
            Dim FechaFinPeriodo2 As Date

            Dim ValorInfonavitTabla As Double

            'Validamos si el trabajador tiene o no activo el infonavit
            sql = "select iPermanente from empleadosC where iIdEmpleadoC=" & idempleado
            Dim rwCalcularInfonavit As DataRow() = nConsulta(sql)
            If rwCalcularInfonavit Is Nothing = False Then
                If rwCalcularInfonavit(0)("iPermanente") = "1" Then
                    sql = "select * from periodos where iIdPeriodo= " & periodo
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))




                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If

                        If numbimestre = 1 Then
                            If Bisiesto(Year(FechaInicioPeriodo1)) = True Then
                                diasfebrero = 29
                            Else
                                diasfebrero = 28
                            End If
                            'diasfebrero = Day(DateSerial(Year(fechapago), 3, 0))
                            numdias = 31 + diasfebrero
                        End If

                        If numbimestre = 2 Then
                            numdias = 61
                        End If

                        If numbimestre = 3 Then
                            numdias = 61
                        End If

                        If numbimestre = 4 Then
                            numdias = 62
                        End If

                        If numbimestre = 5 Then
                            numdias = 61
                        End If

                        If numbimestre = 6 Then
                            numdias = 61
                        End If



                        sql = "select * from Salario "
                        sql &= " where Anio=" & IIf(FechaInicioPeriodo1 = Date.Parse("01/01/1900"), FechaInicioPeriodo1.Year.ToString, FechaInicioPeriodo1.Year.ToString)
                        sql &= " and iEstatus=1"
                        Dim rwValorInfonavit As DataRow() = nConsulta(sql)

                        If rwValorInfonavit Is Nothing = False Then
                            ValorInfonavitTabla = rwValorInfonavit(0)("infonavit")
                        Else

                        End If



                        If tipo = "VSM" And valor > 0 Then
                            valorinfonavit = (((ValorInfonavitTabla * valor * 2) / numdias) * numdias) + 15

                        End If

                        If tipo = "CUOTA FIJA" And valor > 0 Then


                            valorinfonavit = (((valor * 2) / numdias) * numdias) + 15


                        End If

                        If tipo = "PORCENTAJE" And valor > 0 Then

                            valorinfonavit = ((sdi * (valor / 100) * numdias) + 15)
                        End If


                        'Insertamos los datos

                        sql = "EXEC [setCalculoInfonavitInsertar  ] 0"
                        'Bimestre
                        sql &= "," & numbimestre
                        'Anio
                        sql &= "," & Year(FechaInicioPeriodo1)
                        'TipoFactor
                        sql &= ",'" & tipo
                        'Factor
                        sql &= "'," & valor
                        'idEmpleado
                        sql &= "," & idempleado
                        'Monto
                        sql &= "," & valorinfonavit
                        'Estatus
                        sql &= ",1"






                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False

                        End If

                        Return True
                    End If

                End If

            End If


            Return False



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Private Function CalcularInfonavitMonto(tipo As String, valor As Double, sdi As Double, fechapago As Date, periodo As String, idempleado As Integer) As Double
        Try
            Dim numbimestre As Integer

            Dim numdias As Integer

            Dim DiasCadaPeriodo As Integer

            Dim diasfebrero As Integer
            Dim valorinfonavit As Double
            Dim sql As String
            Dim FechaInicioPeriodo1 As Date
            Dim FechaFinPeriodo1 As Date
            Dim FechaInicioPeriodo2 As Date
            Dim FechaFinPeriodo2 As Date

            Dim ValorInfonavitTabla As Double

            'Validamos si el trabajador tiene o no activo el infonavit
            sql = "select iPermanente from empleadosC where iIdEmpleadoC=" & idempleado
            Dim rwCalcularInfonavit As DataRow() = nConsulta(sql)
            If rwCalcularInfonavit Is Nothing = False Then
                If rwCalcularInfonavit(0)("iPermanente") = "1" Then
                    sql = "select * from periodos where iIdPeriodo= " & periodo
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))




                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If

                        If numbimestre = 1 Then
                            If Bisiesto(Year(FechaInicioPeriodo1)) = True Then
                                diasfebrero = 29
                            Else
                                diasfebrero = 28
                            End If
                            'diasfebrero = Day(DateSerial(Year(fechapago), 3, 0))
                            numdias = 31 + diasfebrero
                        End If

                        If numbimestre = 2 Then
                            numdias = 61
                        End If

                        If numbimestre = 3 Then
                            numdias = 61
                        End If

                        If numbimestre = 4 Then
                            numdias = 62
                        End If

                        If numbimestre = 5 Then
                            numdias = 61
                        End If

                        If numbimestre = 6 Then
                            numdias = 61
                        End If


                        'Realizamos la busqueda

                        sql = "select * from CalculoInfonavit where iBimestre=" & numbimestre
                        sql &= " And iAnio= " & Year(FechaInicioPeriodo1) & " And fkiIdEmpleadoC=" & idempleado
                        Dim rwCalculoInfonavit As DataRow() = nConsulta(sql)
                        If rwCalculoInfonavit Is Nothing = False Then
                            'lo borramos


                            sql = "delete from Calculoinfonavit where iBimestre=" & numbimestre
                            sql &= " And iAnio= " & Year(FechaInicioPeriodo1) & " And fkiIdEmpleadoC=" & idempleado

                            If nExecute(sql) = False Then
                                MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return False

                            End If
                        End If


                        sql = "select * from Salario "
                        sql &= " where Anio=" & IIf(FechaInicioPeriodo1 = Date.Parse("01/01/1900"), FechaInicioPeriodo1.Year.ToString, FechaInicioPeriodo1.Year.ToString)
                        sql &= " and iEstatus=1"
                        Dim rwValorInfonavit As DataRow() = nConsulta(sql)

                        If rwValorInfonavit Is Nothing = False Then
                            ValorInfonavitTabla = rwValorInfonavit(0)("infonavit")
                        Else

                        End If



                        If tipo = "VSM" And valor > 0 Then
                            valorinfonavit = (((ValorInfonavitTabla * valor * 2) / numdias) * numdias) + 15

                        End If

                        If tipo = "CUOTA FIJA" And valor > 0 Then


                            valorinfonavit = (((valor * 2) / numdias) * numdias) + 15


                        End If

                        If tipo = "PORCENTAJE" And valor > 0 Then

                            valorinfonavit = ((sdi * (valor / 100) * numdias) + 15)
                        End If


                        'Insertamos los datos

                        sql = "EXEC [setCalculoInfonavitInsertar  ] 0"
                        'Bimestre
                        sql &= "," & numbimestre
                        'Anio
                        sql &= "," & Year(FechaInicioPeriodo1)
                        'TipoFactor
                        sql &= ",'" & tipo
                        'Factor
                        sql &= "'," & valor
                        'idEmpleado
                        sql &= "," & idempleado
                        'Monto
                        sql &= "," & valorinfonavit
                        'Estatus
                        sql &= ",1"






                        If nExecute(sql) = False Then
                            MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return 0

                        End If

                        Return valorinfonavit / numdias
                    End If
                Else
                    Return 0
                End If
            Else
                Return 0
            End If


            Return 0



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Private Function VerificarCalculoInfonavit(periodo As String, idempleado As Integer) As Integer

        Try
            Dim numbimestre As Integer

            Dim numdias As Integer

            Dim diasfebrero As Integer

            Dim sql As String
            Dim FechaInicioPeriodo1 As Date


            'Validamos si el trabajador tiene o no activo el infonavit
            sql = "select iPermanente from empleadosC where iIdEmpleadoC=" & idempleado
            Dim rwCalcularInfonavit As DataRow() = nConsulta(sql)
            If rwCalcularInfonavit Is Nothing = False Then
                If rwCalcularInfonavit(0)("iPermanente") = "1" Then
                    sql = "select * from periodos where iIdPeriodo= " & periodo
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If

                        If numbimestre = 1 Then
                            If Bisiesto(Year(FechaInicioPeriodo1)) = True Then
                                diasfebrero = 29
                            Else
                                diasfebrero = 28
                            End If
                            'diasfebrero = Day(DateSerial(Year(fechapago), 3, 0))
                            numdias = 31 + diasfebrero
                        End If

                        If numbimestre = 2 Then
                            numdias = 61
                        End If

                        If numbimestre = 3 Then
                            numdias = 61
                        End If

                        If numbimestre = 4 Then
                            numdias = 62
                        End If

                        If numbimestre = 5 Then
                            numdias = 61
                        End If

                        If numbimestre = 6 Then
                            numdias = 61
                        End If





                        'Realizamos la busqueda

                        sql = "select * from CalculoInfonavit where iBimestre=" & numbimestre
                        sql &= " And iAnio= " & Year(FechaInicioPeriodo1) & " And fkiIdEmpleadoC=" & idempleado
                        Dim rwCalculoInfonavit As DataRow() = nConsulta(sql)
                        If rwCalculoInfonavit Is Nothing = False Then
                            'lo borramos


                            sql = "delete from Calculoinfonavit where iBimestre=" & numbimestre
                            sql &= " And iAnio= " & Year(FechaInicioPeriodo1) & " And fkiIdEmpleadoC=" & idempleado

                            If nExecute(sql) = False Then
                                MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return False

                            End If


                            'Return 1
                            Return 2
                        Else
                            Return 2
                        End If

                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If
            Else
                Return 0
            End If


            Return 0



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Private Function MontoInfonavitF(periodo As String, idempleado As Integer) As Double

        Try
            Dim numbimestre As Integer
            Dim sql As String
            Dim FechaInicioPeriodo1 As Date


            'Validamos si el trabajador tiene o no activo el infonavit
            sql = "select iPermanente from empleadosC where iIdEmpleadoC=" & idempleado
            Dim rwCalcularInfonavit As DataRow() = nConsulta(sql)
            If rwCalcularInfonavit Is Nothing = False Then
                If rwCalcularInfonavit(0)("iPermanente") = "1" Then
                    sql = "select * from periodos where iIdPeriodo= " & periodo
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If


                        'Realizamos la busqueda

                        sql = "select * from CalculoInfonavit where iBimestre=" & numbimestre
                        sql &= " And iAnio= " & Year(FechaInicioPeriodo1) & " And fkiIdEmpleadoC=" & idempleado
                        Dim rwCalculoInfonavit As DataRow() = nConsulta(sql)
                        If rwCalculoInfonavit Is Nothing = False Then
                            Return Double.Parse(rwCalculoInfonavit(0)("Monto"))
                            IDCalculoInfonavit = rwCalculoInfonavit(0)("iIdCalculoInfonavit")
                        Else
                            Return 0
                        End If

                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If
            Else
                Return 0
            End If


            Return 0



        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Private Function baseisrtotal(puesto As String, dias As Integer, sdi As Double, incapacidad As Double) As Double
        Dim sueldo As Double
        Dim sueldobase As Double
        Dim baseisr As Double
        Dim isrcalculado As Double
        Dim aguinaldog As Double
        Dim primag As Double
        Dim sql As String
        Dim ValorUMA As Double
        Try

            Sql = "select * from Salario "
            Sql &= " where Anio=" & aniocostosocial
            Sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(Sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If

            If puesto = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or puesto = "SUBALTERNO EN FORMACIÓN" Then
                sueldo = sdi * dias
                sueldobase = sueldo
                baseisr = sueldobase - incapacidad
                isrcalculado = isrmensual(baseisr)
            Else
                sueldo = sdi * dias
                sueldobase = (sueldo * (26.19568006 / 100)) + ((sueldo * (8.5070471 / 100)) / 2) + ((sueldo * (8.5070471 / 100)) / 2) + (sueldo * (42.89215164 / 100)) + (sueldo * (9.677848468 / 100))

                ''Aguinaldo gravado
                'aguinaldog = Math.Round(((sueldobase / dias) * 15 / 12 * (dias / 30)) - ((ValorUMA * 30 / 12) * (dias / 30)), 2)


                'primag = (sueldobase * 0.25 / 12 * (dias / 30)) - ((ValorUMA * 15 / 12) * (dias / 30))


                'Aguinaldo gravado 

                If ((sueldobase / dias) * 15 / 12 * (dias / 30)) > ((ValorUMA * 30 / 12) * (dias / 30)) Then
                    'Aguinaldo gravado
                    aguinaldog = Math.Round(((sueldobase / dias) * 15 / 12 * (dias / 30)) - ((ValorUMA * 30 / 12) * (dias / 30)), 2)
                Else
                    'Aguinaldo gravado
                    aguinaldog = "0.00"
                End If

                'Prima de vacaciones

                'Calculos prima
                Dim primavacacionesgravada As Double
                Dim primavacacionesexenta As Double

                primavacacionesgravada = (sueldobase * 0.25 / 12 * (dias / 30)) - ((ValorUMA * 15 / 12) * (dias / 30))
                primavacacionesexenta = ((ValorUMA * 15 / 12) * (dias / 30))

                If primavacacionesgravada > 0 Then
                    primag = primavacacionesgravada

                Else
                    primag = 0
                End If


                baseisr = (sueldobase - ((sueldo * (8.5070471 / 100)) / 2)) + (sueldo * (7.272727273 / 100)) + aguinaldog + primag - incapacidad
                isrcalculado = isrmensual(baseisr)

            End If
            Return isrcalculado
        Catch ex As Exception

        End Try
    End Function

    Private Function isrmontodado(monto As Double, periodo As Integer, fila As Integer) As Double

        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try


            'calculos

            'Calculamos isr

            '1.- buscamos datos para el calculo
            isr = 0
            SQL = "select * from isr where ((" & monto & ">=isr.limiteinf and " & monto & "<=isr.limitesup)"
            SQL &= " or (" & monto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=" & periodo & "and anio=" & aniocostosocial


            Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
            If rwISRCALCULO Is Nothing = False Then
                excendente = monto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)
            Else
                MessageBox.Show("No existe la tabla de ISR con el año: " & aniocostosocial)
            End If
            subsidio = 0
            SQL = "select * from subsidio where ((" & monto & ">=subsidio.limiteinf and " & monto & "<=subsidio.limitesup)"
            SQL &= " or (" & monto & ">=subsidio.limiteinf and subsidio.limitesup=0)) and fkiIdTipoPeriodo2=" & periodo


            Dim rwSubsidio As DataRow() = nConsulta(SQL)
            If rwSubsidio Is Nothing = False Then
                subsidio = Double.Parse(rwSubsidio(0)("credito").ToString)

            End If
            'If periodo = 1 Then
            '    'dtgDatos.Rows(fila).Cells(68).Value = "0.00"
            '    'dtgDatos.Rows(fila).Cells(69).Value = "0.00"
            'Else

            'End If
            If subsidio > isr Then

                dtgDatos.Rows(fila).Cells(68).Value = Math.Round(Double.Parse(subsidio)).ToString("###,##0.00")
                If subsidio > 0 Then
                    dtgDatos.Rows(fila).Cells(69).Value = Math.Round(Double.Parse(subsidio - isr)).ToString("###,##0.00")
                End If

            Else
                dtgDatos.Rows(fila).Cells(68).Value = Math.Round(Double.Parse(subsidio), 2).ToString("###,##0.00")
                If subsidio > 0 Then
                    dtgDatos.Rows(fila).Cells(69).Value = Math.Round(Double.Parse(subsidio), 2).ToString("###,##0.00")
                Else
                    dtgDatos.Rows(fila).Cells(69).Value = "0.00"
                End If

            End If


            If periodo = 1 Then
                If isr > subsidio Then
                    Return isr - subsidio
                Else
                    Return 0
                End If
            Else
                If isr > subsidio Then
                    Return isr - subsidio
                Else
                    Return 0
                End If
            End If




        Catch ex As Exception

        End Try
    End Function

    Private Function isrmontodadosinsubsidio(monto As Double, periodo As Integer, fila As Integer) As Double

        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try


            'calculos

            'Calculamos isr

            '1.- buscamos datos para el calculo
            isr = 0
            SQL = "select * from isr where ((" & monto & ">=isr.limiteinf and " & monto & "<=isr.limitesup)"
            SQL &= " or (" & monto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=" & periodo & "and anio=" & aniocostosocial


            Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
            If rwISRCALCULO Is Nothing = False Then
                excendente = monto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)
            Else
                MessageBox.Show("No existe la tabla de ISR con el año: " & aniocostosocial)
            End If
            subsidio = 0

            If subsidio > isr Then

                dtgDatos.Rows(fila).Cells(68).Value = Math.Round(Double.Parse(subsidio)).ToString("###,##0.00")
                If subsidio > 0 Then
                    dtgDatos.Rows(fila).Cells(69).Value = Math.Round(Double.Parse(subsidio - isr)).ToString("###,##0.00")
                End If

            Else
                dtgDatos.Rows(fila).Cells(68).Value = Math.Round(Double.Parse(subsidio), 2).ToString("###,##0.00")
                If subsidio > 0 Then
                    dtgDatos.Rows(fila).Cells(69).Value = Math.Round(Double.Parse(subsidio), 2).ToString("###,##0.00")
                Else
                    dtgDatos.Rows(fila).Cells(69).Value = "0.00"
                End If

            End If


            If periodo = 1 Then
                If isr > subsidio Then
                    Return isr
                Else
                    Return 0
                End If
            Else
                If isr > subsidio Then
                    Return isr
                Else
                    Return 0
                End If
            End If




        Catch ex As Exception

        End Try
    End Function

    Private Function subsidiocalculomensual(monto As Double, periodo As Integer, fila As Integer) As Double

        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try



            subsidio = 0
            SQL = "select * from subsidio where ((" & monto & ">=subsidio.limiteinf and " & monto & "<=subsidio.limitesup)"
            SQL &= " or (" & monto & ">=subsidio.limiteinf and subsidio.limitesup=0)) and fkiIdTipoPeriodo2=" & periodo


            Dim rwSubsidio As DataRow() = nConsulta(SQL)
            If rwSubsidio Is Nothing = False Then
                subsidio = Double.Parse(rwSubsidio(0)("credito").ToString)

            End If
            Return subsidio



        Catch ex As Exception

        End Try
    End Function

    Private Function isrmensual(monto As Double) As Double

        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try


            'calculos

            'Calculamos isr

            '1.- buscamos datos para el calculo
            isr = 0
            SQL = "select * from isr where ((" & monto & ">=isr.limiteinf and " & monto & "<=isr.limitesup)"
            SQL &= " or (" & monto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=1 and anio=" & aniocostosocial


            Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
            If rwISRCALCULO Is Nothing = False Then
                excendente = monto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)
            Else
                MessageBox.Show("No existe la tabla de ISR con el año: " & aniocostosocial)
            End If
            subsidio = 0
            SQL = "select * from subsidio where ((" & monto & ">=subsidio.limiteinf and " & monto & "<=subsidio.limitesup)"
            SQL &= " or (" & monto & ">=subsidio.limiteinf and subsidio.limitesup=0)) and fkiIdTipoPeriodo2=1"


            Dim rwSubsidio As DataRow() = nConsulta(SQL)
            If rwSubsidio Is Nothing = False Then
                subsidio = Double.Parse(rwSubsidio(0)("credito").ToString)

            End If
            If isr > subsidio Then
                Return isr - subsidio
            Else
                Return 0
            End If


        Catch ex As Exception

        End Try
    End Function

    Function subsidiomensual(monto As Double) As Double
        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try


            'calculos

            'Calculamos isr

            '1.- buscamos datos para el calculo
            isr = 0
            SQL = "select * from isr where ((" & monto & ">=isr.limiteinf and " & monto & "<=isr.limitesup)"
            SQL &= " or (" & monto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=1 and anio=" & aniocostosocial


            Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
            If rwISRCALCULO Is Nothing = False Then
                excendente = monto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)
            Else
                MessageBox.Show("No existe la tabla de ISR con el año: " & aniocostosocial)
            End If
            subsidio = 0
            SQL = "select * from subsidio where ((" & monto & ">=subsidio.limiteinf and " & monto & "<=subsidio.limitesup)"
            SQL &= " or (" & monto & ">=subsidio.limiteinf and subsidio.limitesup=0)) and fkiIdTipoPeriodo2=1"


            Dim rwSubsidio As DataRow() = nConsulta(SQL)
            If rwSubsidio Is Nothing = False Then
                subsidio = Double.Parse(rwSubsidio(0)("credito").ToString)

            End If

            If isr >= subsidio Then
                subsidiomensual = 0
            Else
                subsidiomensual = subsidio - isr
            End If


        Catch ex As Exception

        End Try



    End Function

    Private Function baseSubsidiototal(puesto As String, dias As Double, sdi As Double, incapacidad As Double) As Double



        Dim sueldo As Double
        Dim sueldobase As Double
        Dim baseisr As Double
        Dim isrcalculado As Double
        Dim aguinaldog As Double
        Dim primag As Double
        Dim sql As String
        Dim ValorUMA As Double
        Try

            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If

            If puesto = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or puesto = "SUBALTERNO EN FORMACIÓN" Then
                sueldo = sdi * dias
                sueldobase = sueldo
                baseisr = sueldobase - incapacidad
                baseSubsidiototal = subsidiomensual(baseisr)
            Else
                sueldo = sdi * dias
                sueldobase = (sueldo * (26.19568006 / 100)) + ((sueldo * (8.5070471 / 100)) / 2) + ((sueldo * (8.5070471 / 100)) / 2) + (sueldo * (42.89215164 / 100)) + (sueldo * (9.677848468 / 100))

                'Aguinaldo gravado 

                If ((sueldobase / dias) * 15 / 12 * (dias / 30)) > ((ValorUMA * 30 / 12) * (dias / 30)) Then
                    'Aguinaldo gravado
                    aguinaldog = Math.Round(((sueldobase / dias) * 15 / 12 * (dias / 30)) - ((ValorUMA * 30 / 12) * (dias / 30)), 2)
                Else
                    'Aguinaldo gravado
                    aguinaldog = "0.00"
                End If

                'Prima de vacaciones

                'Calculos prima
                Dim primavacacionesgravada As Double
                Dim primavacacionesexenta As Double

                primavacacionesgravada = (sueldobase * 0.25 / 12 * (dias / 30)) - ((ValorUMA * 15 / 12) * (dias / 30))
                primavacacionesexenta = ((ValorUMA * 15 / 12) * (dias / 30))

                If primavacacionesgravada > 0 Then
                    primag = primavacacionesgravada

                Else
                    primag = 0
                End If


                baseisr = (sueldobase - ((sueldo * (8.5070471 / 100)) / 2)) + (sueldo * (7.272727273 / 100)) + aguinaldog + primag - incapacidad
                baseSubsidiototal = subsidiomensual(baseisr)

            End If
            Return baseSubsidiototal
        Catch ex As Exception

        End Try



    End Function


    Function subsidiomensualCausado(monto As Double) As Double
        Dim excendente As Double
        Dim isr As Double
        Dim subsidio As Double



        Dim SQL As String

        Try


            'calculos

            'Calculamos isr

            '1.- buscamos datos para el calculo
            isr = 0
            SQL = "select * from isr where ((" & monto & ">=isr.limiteinf and " & monto & "<=isr.limitesup)"
            SQL &= " or (" & monto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=1 and anio=" & aniocostosocial


            Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
            If rwISRCALCULO Is Nothing = False Then
                excendente = monto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)
            Else
                MessageBox.Show("No existe la tabla de ISR con el año: " & aniocostosocial)
            End If
            subsidio = 0
            SQL = "select * from subsidio where ((" & monto & ">=subsidio.limiteinf and " & monto & "<=subsidio.limitesup)"
            SQL &= " or (" & monto & ">=subsidio.limiteinf and subsidio.limitesup=0)) and fkiIdTipoPeriodo2=1"


            Dim rwSubsidio As DataRow() = nConsulta(SQL)
            If rwSubsidio Is Nothing = False Then
                subsidio = Double.Parse(rwSubsidio(0)("credito").ToString)

            End If

            If isr >= subsidio Then
                subsidiomensualCausado = 0
            Else
                subsidiomensualCausado = subsidio
            End If


        Catch ex As Exception

        End Try



    End Function


    Function baseSubsidio(puesto As String, dias As Double, sdi As Double, incapacidad As Double) As Double
        Dim sueldo As Double
        Dim sueldobase As Double
        Dim baseisr As Double
        Dim isrcalculado As Double
        Dim aguinaldog As Double
        Dim primag As Double
        Dim sql As String
        Dim ValorUMA As Double
        Try

            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If

            If puesto = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or puesto = "SUBALTERNO EN FORMACIÓN" Then
                sueldo = sdi * dias
                sueldobase = sueldo
                baseisr = sueldobase - incapacidad
                baseSubsidio = subsidiomensualCausado(baseisr)
            Else
                sueldo = sdi * dias
                sueldobase = (sueldo * (26.19568006 / 100)) + ((sueldo * (8.5070471 / 100)) / 2) + ((sueldo * (8.5070471 / 100)) / 2) + (sueldo * (42.89215164 / 100)) + (sueldo * (9.677848468 / 100))

                'Aguinaldo gravado 

                If ((sueldobase / dias) * 15 / 12 * (dias / 30)) > ((ValorUMA * 30 / 12) * (dias / 30)) Then
                    'Aguinaldo gravado
                    aguinaldog = Math.Round(((sueldobase / dias) * 15 / 12 * (dias / 30)) - ((ValorUMA * 30 / 12) * (dias / 30)), 2)
                Else
                    'Aguinaldo gravado
                    aguinaldog = "0.00"
                End If

                'Prima de vacaciones

                'Calculos prima
                Dim primavacacionesgravada As Double
                Dim primavacacionesexenta As Double

                primavacacionesgravada = (sueldobase * 0.25 / 12 * (dias / 30)) - ((ValorUMA * 15 / 12) * (dias / 30))
                primavacacionesexenta = ((ValorUMA * 15 / 12) * (dias / 30))

                If primavacacionesgravada > 0 Then
                    primag = primavacacionesgravada

                Else
                    primag = 0
                End If

                baseisr = (sueldobase - ((sueldo * (8.5070471 / 100)) / 2)) + (sueldo * (7.272727273 / 100)) + aguinaldog + primag - incapacidad
                baseSubsidio = subsidiomensualCausado(baseisr)

            End If
            Return baseSubsidio
        Catch ex As Exception

        End Try



    End Function


    Private Function Incapacidades(tipo As String, valor As Double, sd As Double) As Double
        Dim incapacidad As Double
        incapacidad = 0.0
        Try
            If tipo = "Riesgo de trabajo" Then
                Incapacidades = 0
            ElseIf tipo = "Enfermedad general" Then
                Incapacidades = valor * sd '60% apartir 4 dia
            ElseIf tipo = "Maternidad" Then
                Incapacidades = 0
            End If
            Return incapacidad
        Catch ex As Exception

        End Try
    End Function


    Private Sub cmdguardarfinal_Click(sender As Object, e As EventArgs) Handles cmdguardarfinal.Click
        Try
            Dim sql As String


            sql = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
            sql &= " and iTipoNomina=0"

            'Dim sueldobase, salariodiario, salariointegrado, sueldobruto, TiempoExtraFijoGravado, TiempoExtraFijoExento As Double
            'Dim TiempoExtraOcasional, DesSemObligatorio, VacacionesProporcionales, AguinaldoGravado, AguinaldoExento As Double
            'Dim PrimaVacGravada, PrimaVacExenta, TotalPercepciones, TotalPercepcionesISR As Double
            'Dim incapacidad, ISR, IMSS, Infonavit, InfonavitAnterior, InfonavitAjuste, PensionAlimenticia As Double
            'Dim Prestamo, Fonacot, NetoaPagar, Excedente, Total, ImssCS, RCVCS, InfonavitCS, ISNCS
            'sql = "EXEC getNominaXEmpresaXPeriodo " & gIdEmpresa & "," & cboperiodo.SelectedValue & ",1"

            Dim rwNominaGuardadaFinal As DataRow() = nConsulta(sql)

            If rwNominaGuardadaFinal Is Nothing = False Then
                MessageBox.Show("La nomina ya esta marcada como final, no  se pueden guardar cambios", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                'MessageBox.Show("Se borraran los datos tanto de la nomina abordo como la de descanso", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                sql = "update Nomina set iEstatusNomina=1 "
                sql &= " where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                sql &= " and iEstatusNomina=0 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
                'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                If nExecute(sql) = False Then
                    MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'pnlProgreso.Visible = False
                    Exit Sub
                End If

                '################################################
                'BorrarTablas()
                'llenarTablas()
                'llenargrid("1")
                'llenarTablas("1")
                'llenargrid("0")
                pnlProgreso.Visible = True

                Application.DoEvents()
                pnlCatalogo.Enabled = False
                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = dtgDatos.Rows.Count


                For x As Integer = 0 To dtgDatos.Rows.Count - 1
                    '########GUARDAR INFONAVIT

                    Dim numbimestre As Integer
                    If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
                        numbimestre = Month(FechaInicioPeriodoGlobal) / 2
                    Else
                        numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
                    End If

                    '########GUARDAR SEGURO INFONAVIT
                    sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    Dim FechaInicioPeriodo1 As Date


                    'Dim numbimestre As Integer
                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo1 = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                        If Month(FechaInicioPeriodo1) Mod 2 = 0 Then
                            numbimestre = Month(FechaInicioPeriodo1) / 2
                        Else
                            numbimestre = (Month(FechaInicioPeriodo1) + 1) / 2
                        End If

                    End If

                    If Double.Parse(dtgDatos.Rows(x).Cells(38).Value) > 0 Then

                        sql = "select * from PagoSeguroInfonavit where fkiIdEmpleadoC= " & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " And NumBimestre= " & numbimestre & " And Anio=" & FechaInicioPeriodo1.Year.ToString
                        Dim rwSeguro1 As DataRow() = nConsulta(sql)

                        If rwSeguro1 Is Nothing = True Then
                            'Insertar seguro
                            sql = "EXEC setPagoSeguroInfonavitInsertar  0"
                            ' fk Empleado
                            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
                            'bimestre
                            sql &= "," & numbimestre
                            ' anio
                            sql &= "," & FechaInicioPeriodo1.Year.ToString


                            If nExecute(sql) = False Then
                                MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                'pnlProgreso.Visible = False
                                Exit Sub
                            End If
                        End If

                    End If





                    pgbProgreso.Value += 1
                    Application.DoEvents()
                Next
                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True
                MessageBox.Show("Datos guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub cboperiodo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboperiodo.SelectedIndexChanged
        Try
            dtgDatos.DataSource = ""
            dtgDatos.Columns.Clear()
            Dim Sql As String = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
            Dim rwPeriodo As DataRow() = nConsulta(Sql)

            If rwPeriodo Is Nothing = False Then
                FechaInicioPeriodoGlobal = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                aniocostosocial = Date.Parse(rwPeriodo(0)("dFechaInicio").ToString).Year
            End If


        Catch ex As Exception

        End Try

    End Sub


    Private Sub dtgDatos_CellMouseDown(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dtgDatos.CellMouseDown
        Try
            If e.RowIndex > -1 And e.ColumnIndex > -1 Then
                dtgDatos.CurrentCell = dtgDatos.Rows(e.RowIndex).Cells(e.ColumnIndex)


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub dtgDatos_CellMouseUp(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dtgDatos.CellMouseUp

    End Sub



    Private Sub dtgDatos_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtgDatos.KeyPress
        Try

            SoloNumero.NumeroDec(e, sender)
        Catch ex As Exception

        End Try

    End Sub


    Private Sub btnReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReporte.Click
        Try
            Dim filaExcel As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo As String
            Dim pilotin As Boolean
            Dim rwUsuario As DataRow() = nConsulta("Select * from Usuarios where idUsuario=1")
            Dim tiponomina, sueldodescanso As String
            Dim filaexcelnomtotal As Integer = 0


            pnlProgreso.Visible = True
            pnlCatalogo.Enabled = False
            Application.DoEvents()

            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count

            If dtgDatos.Rows.Count > 0 Then


                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\TMM.xlsx"
                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
                Dim libro As New ClosedXML.Excel.XLWorkbook

                book.Worksheet(1).CopyTo(libro, "NOMINA")
                book.Worksheet(2).CopyTo(libro, "DETALLE")
                book.Worksheet(3).CopyTo(libro, "FACT")
                book.Worksheets(4).CopyTo(libro, "PENSION ALIMENTICIA")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)

                Dim fecha, iejercicio, idias, periodom As String
                Dim DiasCadaPeriodo As Integer
                Dim DiasCadaPeriodo2 As Integer
                Dim FechaInicioPeriodo As Date
                Dim FechaFinPeriodo As Date
                Dim tipoperiodos2 As String

                ' <<<<<<<<<<<<<<<<<<<<<<Noina Total>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
                If rwPeriodo0 Is Nothing = False Then
                    periodo = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " DE " & (rwPeriodo0(0).Item("iEjercicio"))
                    fecha = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
                    iejercicio = rwPeriodo0(0).Item("iEjercicio")
                    idias = rwPeriodo0(0).Item("iDiasPago")
                    periodom = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & (rwPeriodo0(0).Item("iEjercicio"))

                    FechaInicioPeriodo = Date.Parse(rwPeriodo0(0)("dFechaInicio"))

                    FechaFinPeriodo = Date.Parse(rwPeriodo0(0)("dFechaFin"))
                    DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1
                    tipoperiodos2 = rwPeriodo0(0).Item("fkiIdTipoPeriodo")

                End If




                filaExcel = 5
                ' contadorfacturas = 1

                For x As Integer = 0 To dtgDatos.Rows.Count - 1
                    'CONSULTAS
                    Dim fSindicatoExtra As Double = 0.0
                    Dim valesDespensa As String = "0.00"
                    'SINDICATO EXEDENTE TOTAL

                    sql = "select isnull( fsindicatoExtra,0) as  fsindicatoExtra from EmpleadosC where iIdEmpleadoC= " & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)

                    Dim rwDatos As DataRow() = nConsulta(sql)
                    If rwDatos Is Nothing = False Then
                        If Double.Parse(rwDatos(0)("fsindicatoExtra").ToString) > 0 Then
                            fSindicatoExtra = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")), 2)

                        End If

                    End If
                    'VALES DE DESPEMSA 
                    Dim numperiodo As Integer = cboperiodo.SelectedValue

                    If validarSiSeCalculanVales(EmpresaN, tipoperiodos2) Then
                        If tipoperiodos2 = 2 Then
                            If cboperiodo.SelectedValue Mod 2 = 0 Then
                                valesDespensa = 0
                            Else
                                'VALIDAR SI SE LE PAGA NETO
                                If dtgDatos.Rows(x).Cells(71).Value > 0 Then
                                    valesDespensa = "=ROUNDUP(IF((X" & filaExcel + x & "*9%)>=3153.70,3153.70,(X" & filaExcel + x & "*9%)),0)" 'VALES

                                End If

                            End If

                        ElseIf tipoperiodos2 = 3 Then
                            If cboperiodo.SelectedValue Mod 4 = 0 Then

                                'VALIDAR SI SE LE PAGA NETO
                                If dtgDatos.Rows(x).Cells(71).Value > 0 Then
                                    valesDespensa = "=ROUNDUP(IF((X" & filaExcel + x & "*9%)>=3153.70,3153.70,(X" & filaExcel + x & "*9%)),0)" 'VALES
                                End If

                            Else
                                valesDespensa = 0
                            End If
                        Else
                            valesDespensa = 0
                        End If

                    Else
                        valesDespensa = "0.0"
                    End If

                    'Llenar EXCEL
                    hoja.Cell(filaExcel + x, 2).Value = x + 1
                    hoja.Cell(filaExcel + x, 3).Value = dtgDatos.Rows(x).Cells(2).Value 'cosec
                    hoja.Cell(filaExcel + x, 4).Value = dtgDatos.Rows(x).Cells(3).Value 'codigo empl
                    hoja.Cell(filaExcel + x, 5).Value = dtgDatos.Rows(x).Cells(4).Value 'nombre
                    hoja.Cell(filaExcel + x, 6).Value = dtgDatos.Rows(x).Cells(5).Value
                    hoja.Cell(filaExcel + x, 7).Value = dtgDatos.Rows(x).Cells(6).Value
                    hoja.Cell(filaExcel + x, 8).Value = dtgDatos.Rows(x).Cells(7).Value
                    hoja.Cell(filaExcel + x, 9).Value = "'" + dtgDatos.Rows(x).Cells(8).Value 'imss
                    hoja.Cell(filaExcel + x, 10).Value = dtgDatos.Rows(x).Cells(9).Value
                    hoja.Cell(filaExcel + x, 11).Value = dtgDatos.Rows(x).Cells(10).Value
                    hoja.Cell(filaExcel + x, 12).Value = dtgDatos.Rows(x).Cells(11).Value
                    hoja.Cell(filaExcel + x, 13).Value = dtgDatos.Rows(x).Cells(12).Value
                    hoja.Cell(filaExcel + x, 14).Value = dtgDatos.Rows(x).Cells(13).Value
                    hoja.Cell(filaExcel + x, 15).Value = dtgDatos.Rows(x).Cells(14).Value
                    hoja.Cell(filaExcel + x, 16).Value = dtgDatos.Rows(x).Cells(15).Value
                    hoja.Cell(filaExcel + x, 17).Value = dtgDatos.Rows(x).Cells(16).Value
                    hoja.Cell(filaExcel + x, 18).Value = dtgDatos.Rows(x).Cells(17).Value
                    hoja.Cell(filaExcel + x, 19).Value = dtgDatos.Rows(x).Cells(18).Value
                    hoja.Cell(filaExcel + x, 20).Value = dtgDatos.Rows(x).Cells(19).Value
                    hoja.Cell(filaExcel + x, 21).Value = dtgDatos.Rows(x).Cells(20).Value
                    hoja.Cell(filaExcel + x, 22).Value = dtgDatos.Rows(x).Cells(21).Value
                    hoja.Cell(filaExcel + x, 23).Value = dtgDatos.Rows(x).Cells(22).Value
                    hoja.Cell(filaExcel + x, 24).Value = dtgDatos.Rows(x).Cells(23).Value
                    hoja.Cell(filaExcel + x, 25).Value = dtgDatos.Rows(x).Cells(24).Value
                    hoja.Cell(filaExcel + x, 26).Value = dtgDatos.Rows(x).Cells(25).Value
                    hoja.Cell(filaExcel + x, 27).Value = dtgDatos.Rows(x).Cells(26).Value
                    hoja.Cell(filaExcel + x, 28).Value = dtgDatos.Rows(x).Cells(27).Value
                    hoja.Cell(filaExcel + x, 29).Value = dtgDatos.Rows(x).Cells(28).Value
                    hoja.Cell(filaExcel + x, 30).Value = dtgDatos.Rows(x).Cells(29).Value 'sueldo bruto
                    hoja.Cell(filaExcel + x, 31).Value = dtgDatos.Rows(x).Cells(30).Value
                    hoja.Cell(filaExcel + x, 32).Value = dtgDatos.Rows(x).Cells(31).Value
                    hoja.Cell(filaExcel + x, 33).Value = dtgDatos.Rows(x).Cells(32).Value
                    hoja.Cell(filaExcel + x, 34).Value = dtgDatos.Rows(x).Cells(33).Value
                    hoja.Cell(filaExcel + x, 35).Value = dtgDatos.Rows(x).Cells(34).Value
                    hoja.Cell(filaExcel + x, 36).Value = dtgDatos.Rows(x).Cells(35).Value
                    hoja.Cell(filaExcel + x, 37).Value = dtgDatos.Rows(x).Cells(36).Value
                    hoja.Cell(filaExcel + x, 38).Value = dtgDatos.Rows(x).Cells(37).Value

                    hoja.Cell(filaExcel + x, 39).Value = dtgDatos.Rows(x).Cells(38).Value
                    hoja.Cell(filaExcel + x, 40).Value = dtgDatos.Rows(x).Cells(39).Value
                    hoja.Cell(filaExcel + x, 41).Value = dtgDatos.Rows(x).Cells(40).Value
                    hoja.Cell(filaExcel + x, 42).Value = dtgDatos.Rows(x).Cells(41).Value
                    hoja.Cell(filaExcel + x, 43).Value = dtgDatos.Rows(x).Cells(42).Value
                    hoja.Cell(filaExcel + x, 44).Value = dtgDatos.Rows(x).Cells(43).Value
                    hoja.Cell(filaExcel + x, 45).Value = dtgDatos.Rows(x).Cells(44).Value
                    hoja.Cell(filaExcel + x, 46).Value = dtgDatos.Rows(x).Cells(45).Value
                    hoja.Cell(filaExcel + x, 47).Value = dtgDatos.Rows(x).Cells(46).Value
                    hoja.Cell(filaExcel + x, 48).Value = dtgDatos.Rows(x).Cells(47).Value
                    hoja.Cell(filaExcel + x, 49).Value = dtgDatos.Rows(x).Cells(48).Value
                    hoja.Cell(filaExcel + x, 50).Value = dtgDatos.Rows(x).Cells(49).Value
                    hoja.Cell(filaExcel + x, 51).Value = dtgDatos.Rows(x).Cells(50).Value
                    hoja.Cell(filaExcel + x, 52).Value = dtgDatos.Rows(x).Cells(51).Value
                    hoja.Cell(filaExcel + x, 53).Value = dtgDatos.Rows(x).Cells(52).Value
                    hoja.Cell(filaExcel + x, 54).Value = dtgDatos.Rows(x).Cells(53).Value 'PRIMA EXE
                    hoja.Cell(filaExcel + x, 55).FormulaA1 = "=BA" & filaExcel + x & "+BB" & filaExcel + x ' dtgDatos.Rows(x).Cells(54).Value TOTAL PRIMA
                    hoja.Cell(filaExcel + x, 56).Value = dtgDatos.Rows(x).Cells(55).Value 'TOTAL PERCEPCIONES
                    hoja.Cell(filaExcel + x, 57).Value = dtgDatos.Rows(x).Cells(56).Value 'TOTAL P/ISR
                    hoja.Cell(filaExcel + x, 58).Value = dtgDatos.Rows(x).Cells(57).Value 'INCAPACIDAD
                    hoja.Cell(filaExcel + x, 59).Value = dtgDatos.Rows(x).Cells(58).Value 'ISR
                    hoja.Cell(filaExcel + x, 60).Value = dtgDatos.Rows(x).Cells(59).Value 'IMSS
                    hoja.Cell(filaExcel + x, 61).Value = dtgDatos.Rows(x).Cells(60).Value 'INFONAVIT
                    hoja.Cell(filaExcel + x, 62).Value = dtgDatos.Rows(x).Cells(61).Value
                    hoja.Cell(filaExcel + x, 63).Value = dtgDatos.Rows(x).Cells(62).Value
                    hoja.Cell(filaExcel + x, 64).Value = dtgDatos.Rows(x).Cells(63).Value 'pension aliemtncia
                    hoja.Cell(filaExcel + x, 65).Value = dtgDatos.Rows(x).Cells(64).Value
                    hoja.Cell(filaExcel + x, 66).Value = dtgDatos.Rows(x).Cells(65).Value
                    hoja.Cell(filaExcel + x, 67).Value = dtgDatos.Rows(x).Cells(66).Value

                    hoja.Cell(filaExcel + x, 68).Value = dtgDatos.Rows(x).Cells(67).Value
                    hoja.Cell(filaExcel + x, 69).Value = dtgDatos.Rows(x).Cells(68).Value
                    hoja.Cell(filaExcel + x, 70).Value = dtgDatos.Rows(x).Cells(69).Value
                    hoja.Cell(filaExcel + x, 71).Value = dtgDatos.Rows(x).Cells(70).Value
                    hoja.Cell(filaExcel + x, 72).Value = dtgDatos.Rows(x).Cells(71).Value 'NETO SA
                    hoja.Cell(filaExcel + x, 73).Value = dtgDatos.Rows(x).Cells(72).Value
                    hoja.Cell(filaExcel + x, 74).FormulaA1 = "=AF" & filaExcel + x & "+AG" & filaExcel + x & "+AH" & filaExcel + x & "+AI" & filaExcel + x & "+AJ" & filaExcel + x & "+AK" & filaExcel + x & "+AL" & filaExcel + x & "+AM" & filaExcel + x & "+AN" & filaExcel + x & "+AO" & filaExcel + x & "+AP" & filaExcel + x & "+AQ" & filaExcel + x & "+AR" & filaExcel + x & "+AV" & filaExcel + x
                    'hoja.Cell(filaExcel + x, 74).Value = dtgDatos.Rows(x).Cells(73).Value



                    'exedente
                    hoja.Cell(filaExcel + x, 75).Value = dtgDatos.Rows(x).Cells(74).Value 'EXEDENTE
                    hoja.Cell(filaExcel + x, 76).FormulaR1C1 = "=IF(X" & filaExcel + x & ">40000,""PPP"",""SIND"")" 'SIND/PPP
                    'hoja.Cell(filaExcel + x, 77).Value = dtgDatos.Rows(x).Cells(75).Value
                    'hoja.Cell(filaExcel + x, 78).FormulaA1 = "=CO" & filaExcel + x & "/30*T" & filaExcel + x & "*0.25"
                    'hoja.Cell(filaExcel + x, 79).FormulaA1 = "=CO" & filaExcel + x & "/30/8*P" & filaExcel + x & "*2" 'Tiempo Extra Doble
                    'hoja.Cell(filaExcel + x, 80).FormulaA1 = "=CO" & filaExcel + x & "/30/8*Q" & filaExcel + x & "*3" 'Tiempo Extra triple
                    'hoja.Cell(filaExcel + x, 81).FormulaA1 = "=CO" & filaExcel + x & "/30*R" & filaExcel + x & "*2" 'desncaso laborado
                    'hoja.Cell(filaExcel + x, 82).FormulaA1 = "=CO" & filaExcel + x & "/30*S" & filaExcel + x & "*2" 'dia festivo
                    hoja.Cell(filaExcel + x, 77).Value = "0.0"
                    hoja.Cell(filaExcel + x, 78).Value = "0.0"
                    hoja.Cell(filaExcel + x, 79).Value = "0.0"
                    hoja.Cell(filaExcel + x, 80).Value = "0.0"
                    hoja.Cell(filaExcel + x, 81).Value = "0.0"
                    hoja.Cell(filaExcel + x, 82).Value = "0.0"
                    hoja.Cell(filaExcel + x, 83).FormulaA1 = "=BW" & filaExcel + x & "+BY" & filaExcel + x & "+BZ" & filaExcel + x & "+CA" & filaExcel + x & "+CB" & filaExcel + x & "+CC" & filaExcel + x & "+CD" & filaExcel + x

                    hoja.Cell(filaExcel + x, 84).Value = dtgDatos.Rows(x).Cells(76).Value 'por comision
                    hoja.Cell(filaExcel + x, 85).Value = dtgDatos.Rows(x).Cells(77).Value 'comision a
                    hoja.Cell(filaExcel + x, 86).Value = dtgDatos.Rows(x).Cells(78).Value 'comision b

                    hoja.Cell(filaExcel + x, 87).Value = dtgDatos.Rows(x).Cells(79).Value 'IMSS
                    hoja.Cell(filaExcel + x, 88).Value = dtgDatos.Rows(x).Cells(80).Value 'RCV
                    hoja.Cell(filaExcel + x, 89).Value = dtgDatos.Rows(x).Cells(81).Value 'INFONAVIT
                    hoja.Cell(filaExcel + x, 90).Value = dtgDatos.Rows(x).Cells(82).Value 'ISN
                    hoja.Cell(filaExcel + x, 91).FormulaA1 = "=+CI" & filaExcel + x & "+CJ" & filaExcel + x & "+CK" & filaExcel + x & "+CL" & filaExcel + x  'TOTAL COSTO SOCIAL

                    hoja.Cell(filaExcel + x, 92).FormulaA1 = valesDespensa 'VALES
                    hoja.Cell(filaExcel + x, 93).Value = fSindicatoExtra 'exedente monto
                    If NombrePeriodo = "Quincenal" Then
                        hoja.Cell(filaExcel + x, 94).FormulaA1 = "=if(BX" & filaExcel + x & "=""PPP"",((Z" & filaExcel + x & "/1.0493)*15.2)*0.03,0)"
                    Else
                        recorrerFilasColumnas(hoja, 1, filaExcel + x, 124, "clear", 94)
                        hoja.Cell(filaExcel + x, 94).Value = "NA"
                    End If

                    recorrerFilasColumnas(hoja, 1, dtgDatos.Rows.Count + 1, 124, "clear", 95)

                Next

                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 16).FormulaA1 = "=SUM(P" & filaExcel & ":P" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 17).FormulaA1 = "=SUM(Q" & filaExcel & ":Q" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 18).FormulaA1 = "=SUM(R" & filaExcel & ":R" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 19).FormulaA1 = "=SUM(S" & filaExcel & ":S" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 20).FormulaA1 = "=SUM(T" & filaExcel & ":T" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 21).FormulaA1 = "=SUM(U" & filaExcel & ":U" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 22).FormulaA1 = "=SUM(V" & filaExcel & ":V" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 23).FormulaA1 = "=SUM(W" & filaExcel & ":W" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 30).FormulaA1 = "=SUM(AD" & filaExcel & ":AD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 31).FormulaA1 = "=SUM(AE" & filaExcel & ":AE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 32).FormulaA1 = "=SUM(AF" & filaExcel & ":AF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 33).FormulaA1 = "=SUM(AG" & filaExcel & ":AG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 34).FormulaA1 = "=SUM(AH" & filaExcel & ":AH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 35).FormulaA1 = "=SUM(AI" & filaExcel & ":AI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 36).FormulaA1 = "=SUM(AJ" & filaExcel & ":AJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 37).FormulaA1 = "=SUM(AK" & filaExcel & ":AK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 38).FormulaA1 = "=SUM(AL" & filaExcel & ":AL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 39).FormulaA1 = "=SUM(AM" & filaExcel & ":AM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 40).FormulaA1 = "=SUM(AN" & filaExcel & ":AN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 41).FormulaA1 = "=SUM(AO" & filaExcel & ":AO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 42).FormulaA1 = "=SUM(AP" & filaExcel & ":AP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 43).FormulaA1 = "=SUM(AQ" & filaExcel & ":AQ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 44).FormulaA1 = "=SUM(AR" & filaExcel & ":AR" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 45).FormulaA1 = "=SUM(AS" & filaExcel & ":AS" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 46).FormulaA1 = "=SUM(AT" & filaExcel & ":AT" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 47).FormulaA1 = "=SUM(AU" & filaExcel & ":AU" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 48).FormulaA1 = "=SUM(AV" & filaExcel & ":AV" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 49).FormulaA1 = "=SUM(AW" & filaExcel & ":AW" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 50).FormulaA1 = "=SUM(AX" & filaExcel & ":AX" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 51).FormulaA1 = "=SUM(AY" & filaExcel & ":AY" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 52).FormulaA1 = "=SUM(AZ" & filaExcel & ":AZ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 53).FormulaA1 = "=SUM(BA" & filaExcel & ":BA" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 54).FormulaA1 = "=SUM(BB" & filaExcel & ":BB" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 55).FormulaA1 = "=SUM(BC" & filaExcel & ":BC" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 56).FormulaA1 = "=SUM(BD" & filaExcel & ":BD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 57).FormulaA1 = "=SUM(BE" & filaExcel & ":BE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 58).FormulaA1 = "=SUM(BF" & filaExcel & ":BF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 59).FormulaA1 = "=SUM(BG" & filaExcel & ":BG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 60).FormulaA1 = "=SUM(BH" & filaExcel & ":BH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 61).FormulaA1 = "=SUM(BI" & filaExcel & ":BI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 62).FormulaA1 = "=SUM(BJ" & filaExcel & ":BJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 63).FormulaA1 = "=SUM(BK" & filaExcel & ":BK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 64).FormulaA1 = "=SUM(BL" & filaExcel & ":BL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 65).FormulaA1 = "=SUM(BM" & filaExcel & ":BM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 66).FormulaA1 = "=SUM(BN" & filaExcel & ":BN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 67).FormulaA1 = "=SUM(BO" & filaExcel & ":BO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 68).FormulaA1 = "=SUM(BP" & filaExcel & ":BP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 69).FormulaA1 = "=SUM(BQ" & filaExcel & ":BQ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 70).FormulaA1 = "=SUM(BR" & filaExcel & ":BR" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 71).FormulaA1 = "=SUM(BS" & filaExcel & ":BS" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 72).FormulaA1 = "=SUM(BT" & filaExcel & ":BT" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 73).FormulaA1 = "=SUM(BU" & filaExcel & ":BU" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 74).FormulaA1 = "=SUM(BV" & filaExcel & ":BV" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 75).FormulaA1 = "=SUM(BW" & filaExcel & ":BW" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 76).FormulaA1 = "=SUM(BX" & filaExcel & ":BX" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 77).FormulaA1 = "=SUM(BY" & filaExcel & ":BY" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 78).FormulaA1 = "=SUM(BZ" & filaExcel & ":BZ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 79).FormulaA1 = "=SUM(CA" & filaExcel & ":CA" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 80).FormulaA1 = "=SUM(CB" & filaExcel & ":CB" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 81).FormulaA1 = "=SUM(CC" & filaExcel & ":CC" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 82).FormulaA1 = "=SUM(CD" & filaExcel & ":CD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 83).FormulaA1 = "=SUM(CE" & filaExcel & ":CE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 84).FormulaA1 = "=SUM(CF" & filaExcel & ":CF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 85).FormulaA1 = "=SUM(CG" & filaExcel & ":CG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"

                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 86).FormulaA1 = "=SUM(CH" & filaExcel & ":CH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 87).FormulaA1 = "=SUM(CI" & filaExcel & ":CI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 88).FormulaA1 = "=SUM(CJ" & filaExcel & ":CJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 89).FormulaA1 = "=SUM(CK" & filaExcel & ":CK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 90).FormulaA1 = "=SUM(CL" & filaExcel & ":CL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 91).FormulaA1 = "=SUM(CM" & filaExcel & ":CM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 92).FormulaA1 = "=SUM(CN" & filaExcel & ":CN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 93).FormulaA1 = "=SUM(CO" & filaExcel & ":CO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 94).FormulaA1 = "=SUM(CP" & filaExcel & ":CP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"


                hoja.Range(filaExcel + dtgDatos.Rows.Count, 5, filaExcel + dtgDatos.Rows.Count, 85).Style.Font.SetBold(True)

                filaexcelnomtotal = filaExcel + dtgDatos.Rows.Count + 1

                ''FACT PREV/ DEPOSITOS
                Dim totalf As Integer = dtgDatos.Rows.Count + 1
                Dim espace As Integer = filaExcel + totalf + 3
                Dim totalbuq As Integer = totalf + filaExcel

                hoja.Cell(espace, "E").Value = "COSTO CLIENTE"
                hoja.Range(espace, 5, espace, 6).Merge()
                hoja.Cell(espace, "E").Style.Font.Bold = True
                hoja.Range(espace, 5, espace, 6).Style.Font.FontColor = XLColor.White
                hoja.Range(espace, 5, espace, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                hoja.Range(espace, 5, espace + 9, 6).Style.Font.FontName = "Century Gothic"
                hoja.Range(espace, 5, espace + 9, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 5, espace + 9, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                hoja.Range(espace + 7, 5, espace + 9, 6).Style.Font.Bold = True
                hoja.Range(espace + 7, 5, espace + 9, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                hoja.Cell(espace + 9, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                hoja.Cell(espace + 9, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)

                hoja.Range(espace, 6, espace + 10, 6).Style.NumberFormat.Format = " #,##0.00"
                hoja.Cell(espace + 2, "E").Value = "NÓMINA SA"
                hoja.Cell(espace + 3, "E").Value = "BENEFICIOSOCIAL"
                'hoja.Cell(espace + 4, "E").Value = "PPP"
                hoja.Cell(espace + 5, "E").Value = "VALES DE DESPENSA"
                hoja.Cell(espace + 6, "E").Value = "COSTO SOCIAL"

                hoja.Cell(espace + 7, "E").Value = "Comision"
                hoja.Cell(espace + 8, "E").Value = "IVA"
                hoja.Cell(espace + 9, "E").Value = "Total"

                hoja.Cell(espace + 2, "F").FormulaA1 = "=BS" & totalbuq & "+I" & espace + 4
                hoja.Cell(espace + 3, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""SIND"",CE5:CE" & totalbuq - 2 & ")"
                ' hoja.Cell(espace + 4, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""PPP"",CE5:CE" & totalbuq - 2 & ")"
                hoja.Cell(espace + 5, "F").FormulaA1 = "=+CN" & totalbuq
                hoja.Cell(espace + 6, "F").FormulaA1 = "=+CM" & totalbuq

                hoja.Cell(espace + 7, "F").FormulaA1 = "=(F" & espace + 2 & "+F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & ")*0.06"
                hoja.Cell(espace + 8, "F").FormulaA1 = "=(F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & "+F" & espace + 7 & ")*0.16"
                hoja.Cell(espace + 9, "F").FormulaA1 = "=F" & espace + 2 & "+F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 6 & "+F" & espace + 7 & "+F" & espace + 8



                'IKE FACT
                If diasperiodo > 14 Then

                    hoja.Cell(espace + 14, "E").Value = "IKE "
                    hoja.Cell(espace + 14, "E").Style.Font.Bold = True
                    hoja.Range(espace + 14, 5, espace + 14, 6).Merge()
                    hoja.Range(espace + 14, 5, espace + 14, 6).Style.Font.FontColor = XLColor.White
                    hoja.Range(espace + 14, 5, espace + 14, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Font.FontName = "Century Gothic"
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                    hoja.Range(espace + 17, 5, espace + 17, 6).Style.Font.Bold = True
                    hoja.Range(espace + 20, 5, espace + 21, 6).Style.Font.Bold = True
                    hoja.Cell(espace + 17, "E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    hoja.Range(espace + 20, 5, espace + 21, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    hoja.Cell(espace + 17, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                    hoja.Cell(espace + 17, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)
                    hoja.Cell(espace + 21, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                    hoja.Cell(espace + 21, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)

                    hoja.Cell(espace + 15, "E").Value = "PPP"
                    hoja.Cell(espace + 16, "E").Value = "3% Largo Plazo (ahorro)"
                    hoja.Cell(espace + 17, "E").Value = "IKE Deposito 1"
                    hoja.Cell(espace + 18, "E").Value = "-"
                    hoja.Cell(espace + 19, "E").Value = "Comision (PPP+Ahorro) 6%"
                    hoja.Cell(espace + 20, "E").Value = "IVA"
                    hoja.Cell(espace + 21, "E").Value = "IKE Deposito 2"

                    hoja.Range(espace + 15, 6, espace + 21, 6).Style.NumberFormat.Format = " #,##0.00"

                    hoja.Cell(espace + 15, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""PPP"",CE5:CE" & totalbuq - 2 & ")"
                    hoja.Cell(espace + 16, "F").FormulaA1 = "=CP" & totalbuq
                    hoja.Cell(espace + 17, "F").FormulaA1 = "=F" & espace + 15 & "+F" & espace + 16
                    ' hoja.Cell(espace + 18, "F").Value = "-"
                    hoja.Cell(espace + 19, "F").FormulaA1 = "=(F" & espace + 15 & "+F" & espace + 16 & ")*0.06"
                    hoja.Cell(espace + 20, "F").FormulaA1 = "=F" & espace + 19 & "*0.16"
                    hoja.Cell(espace + 21, "F").FormulaA1 = "=F" & espace + 19 & "+F" & espace + 20


                End If

                hoja.Range(espace + 23, 5, espace + 25, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thin
                hoja.Range(espace + 23, 5, espace + 25, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                hoja.Range(espace + 23, 6, espace + 25, 6).Style.NumberFormat.Format = " #,##0.00"

                hoja.Cell(espace + 23, "E").Value = "Deposito Cuenta SA"
                hoja.Cell(espace + 24, "E").Value = "Deposito cuenta GROESSINGER"
                hoja.Cell(espace + 25, "E").Value = "Deposito IKE"
                hoja.Cell(espace + 23, "F").FormulaA1 = "=F" & espace + 2
                hoja.Cell(espace + 24, "F").FormulaA1 = "=F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & "+F" & espace + 7 & "+F" & espace + 8
                hoja.Cell(espace + 25, "F").FormulaA1 = "=F" & espace + 17 & "+F" & espace + 21

                'RETENCIONES

                hoja.Range(espace, 8, espace, 9).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                hoja.Range(espace, 8, espace + 6, 9).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 8, espace + 6, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 8, espace + 6, 9).Style.Font.FontName = " Century Gothic"
                hoja.Range(espace, 8, espace + 6, 8).Style.Font.Bold = True
                hoja.Cell(espace, "H").Style.Font.FontColor = XLColor.White
                hoja.Range(espace, 8, espace + 6, 9).Style.NumberFormat.Format = " #,##0.00"

                hoja.Cell(espace, "H").Value = "RETENCIONES"
                hoja.Range(espace, 9, espace, 8).Merge()
                hoja.Cell(espace + 2, "H").Value = "ISR"
                hoja.Cell(espace + 3, "H").Value = "INFONAVIT"
                hoja.Cell(espace + 4, "H").Value = "PENSIÓN"
                hoja.Cell(espace + 5, "H").Value = "FONACOT"
                hoja.Cell(espace + 6, "H").Value = "TOTAL"

                hoja.Cell(espace + 2, "I").FormulaA1 = "=+BG" & totalbuq
                hoja.Cell(espace + 3, "I").FormulaA1 = "=+BI" & totalbuq & "+BJ" & totalbuq
                hoja.Cell(espace + 4, "I").FormulaA1 = "=+BL" & totalbuq
                hoja.Cell(espace + 5, "I").FormulaA1 = "=+BN" & totalbuq
                hoja.Cell(espace + 6, "I").FormulaA1 = "=+I" & espace + 2 & "+I" & espace + 3 & "+I" & espace + 4 & "+I" & espace + 5






                '<<<<<<<<<<<<<<<Detalle>>>>>>>>>>>>>>>>>>

                filaExcel = 6
                Dim filatmp As Integer = 5

                Dim cuenta, banco, clabe, nombrecompleto As String
                Dim codesanta As String = "ND"
                Dim numperiodo2 As Int16 = CInt(cboperiodo.SelectedValue) Mod 2

                'LIMPIAR FILAS
                recorrerFilasColumnas(hoja2, filaExcel - 1, dtgDatos.Rows.Count + 60, 1, "clear")
                recorrerFilasColumnas(hoja2, filaExcel - 1, dtgDatos.Rows.Count + 60, 60, "clear", 14)

                hoja2.Cell(4, 2).Value = "PERIODO " & numperiodo2 & IIf(idias = "15", "Q ", " SEM ") & periodom
                For x As Integer = 0 To dtgDatos.Rows.Count - 1

                    hoja2.Cell(filaExcel, 9).Style.NumberFormat.Format = "@"
                    hoja2.Cell(filaExcel, 8).Style.NumberFormat.Format = "@"
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetBold(False)
                    hoja2.Range(filaExcel, 11, filaExcel, 12).Style.NumberFormat.NumberFormatId = 4
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontColor(XLColor.Black)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontName("Arial")
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontSize(8)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetBold(False)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.General)
                    hoja2.Cell("K5").Value = EmpresaN

                    Dim empleado As DataRow() = nConsulta("Select * from empleadosC where cCodigoEmpleado=" & dtgDatos.Rows(x).Cells(3).Value)
                    If empleado Is Nothing = False Then
                        nombrecompleto = empleado(0).Item("cNombre") & " " & empleado(0).Item("cApellidoP") & " " & empleado(0).Item("cApellidoM")
                        cuenta = empleado(0).Item("NumCuenta")
                        clabe = empleado(0).Item("Clabe")
                        Dim bank As DataRow() = nConsulta("select * from bancos where iIdBanco =" & empleado(0).Item("fkiIdBanco"))
                        If bank Is Nothing = False Then
                            banco = bank(0).Item("cBANCO")
                            codesanta = bank(0).Item("idSantander")
                        End If
                    End If


                    hoja2.Cell(filaExcel, 3).Style.NumberFormat.Format = "@"
                    hoja2.Cell(filaExcel, 2).Value = dtgDatos.Rows(x).Cells(2).Value
                    hoja2.Cell(filaExcel, 3).Value = dtgDatos.Rows(x).Cells(3).Value 'No empleado
                    hoja2.Cell(filaExcel, 4).Value = nombrecompleto.ToUpper 'EMPLEADONOMBRE
                    hoja2.Cell(filaExcel, 5).FormulaA1 = "=+NOMINA!BX" & filatmp
                    hoja2.Cell(filaExcel, 6).FormulaA1 = "=+NOMINA!G" & filatmp
                    hoja2.Cell(filaExcel, 7).Value = banco
                    hoja2.Cell(filaExcel, 8).Value = codesanta
                    hoja2.Cell(filaExcel, 9).Value = clabe
                    hoja2.Cell(filaExcel, 10).Value = cuenta
                    hoja2.Cell(filaExcel, 11).FormulaA1 = "=+'NOMINA'!BS" & filatmp 'sa
                    hoja2.Cell(filaExcel, 12).FormulaA1 = "=+'NOMINA'!CE" & filatmp 'excedente
                    hoja2.Cell(filaExcel, 13).FormulaA1 = "=+NOMINA!CN" & filatmp 'vales

                    filaExcel = filaExcel + 1
                    filatmp = filatmp + 1

                Next x

                'Formulas
                hoja2.Range(filaExcel + 2, 8, filaExcel + 4, 12).Style.Font.SetBold(True)
                hoja2.Cell(filaExcel + 2, 11).FormulaA1 = "=SUM(K6:K" & filaExcel & ")"
                hoja2.Cell(filaExcel + 2, 12).FormulaA1 = "=SUM(L6:L" & filaExcel & ")"
                hoja2.Cell(filaExcel + 2, 13).FormulaA1 = "=SUM(M6:M" & filaExcel & ")"



                ' <<<<<<<<<FACT>>>>>>>>>>>

                hoja3.Cell("G2").Value = "TMM " & EmpresaN.ToUpper & " " & IIf(idias = "15", numperiodo2 & "Q ", cboperiodo.SelectedIndex + 1 & " SEM ") & periodo
                hoja3.Cell("H3").FormulaA1 = "=+NOMINA!F" & espace + 2
                hoja3.Cell("H4").FormulaA1 = "=+NOMINA!F" & espace + 3
                hoja3.Cell("H5").FormulaA1 = "=+NOMINA!F" & espace + 4
                hoja3.Cell("H6").FormulaA1 = "=+NOMINA!F" & espace + 5
                hoja3.Cell("H7").FormulaA1 = "=(H3+H4+H5+H6)*G7"
                hoja3.Cell("H8").Value = EmpresaN.ToUpper


                ' <<<<<<<<<PENSION ALIEMENTICIA>>>>>>>>>>>
                filaExcel = 2
                filatmp = 5

                Dim beneficiaria, porcentaje, pensionmonto As String


                For x As Integer = 0 To dtgDatos.Rows.Count - 1

                    If dtgDatos.Rows(x).Cells(63).Value > 0 Then

                        Dim pensionalimenticia As DataRow() = nConsulta("Select * from pensionAlimenticia where fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value)
                        If pensionalimenticia Is Nothing = False Then
                            beneficiaria = pensionalimenticia(0).Item("Nombrebeneficiario")
                            banco = buscarBanco(pensionalimenticia(0).Item("fkiIdBanco"), "Nombre")
                            clabe = pensionalimenticia(0).Item("Clabe")
                            cuenta = pensionalimenticia(0).Item("Cuenta")
                            porcentaje = pensionalimenticia(0).Item("fPorcentaje")
                        End If
                        hoja4.Cell(filaExcel, 5).Style.NumberFormat.Format = "@"
                        hoja4.Cell(filaExcel, 6).Style.NumberFormat.Format = "@"

                        hoja4.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(3).Value
                        hoja4.Cell(filaExcel, 2).Value = dtgDatos.Rows(x).Cells(4).Value
                        hoja4.Cell(filaExcel, 3).Value = beneficiaria ' "BENEFICIARIA"
                        hoja4.Cell(filaExcel, 4).Value = banco ' "BANCO"
                        hoja4.Cell(filaExcel, 5).Value = cuenta '"CUENTA"
                        hoja4.Cell(filaExcel, 6).Value = clabe '"CLABE"
                        hoja4.Cell(filaExcel, 7).Value = porcentaje ' "PORC"
                        hoja4.Cell(filaExcel, 8).FormulaA1 = "=NOMINA!BL" & filatmp ' "PENSION MONTO"
                        filaExcel = filaExcel + 1
                    End If

                    filatmp = filatmp + 1
                Next x

                hoja4.Cell(filaExcel + dtgDatos.Rows.Count + 1, 8).FormulaA1 = "=SUM(H2:M" & filaExcel + dtgDatos.Rows.Count - 1 & ")"

                '<<<<<CARGAR>>>>>
                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True

                '<<<<<<<<<<<<<<<guardar>>>>>>>>>>>>>>>>

                Dim textoperiodo As String
                If NombrePeriodo = "Quincenal" Then
                    If cboperiodo.SelectedValue Mod 2 = 0 Then
                        textoperiodo = "2 QNA "
                    Else
                        textoperiodo = "1 QNA "
                    End If


                ElseIf NombrePeriodo = "Semanal" Then

                    textoperiodo = "SEMANA " & cboperiodo.SelectedValue
                End If

                dialogo.FileName = "NOMINA " & EmpresaN.ToUpper & " " & textoperiodo & " " & periodo
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

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try



    End Sub
    Public Sub llenardesgloce(ByRef nombrebuque As String, ByRef contadorexcelbuquefinal As Integer, ByRef hoja As IXLWorksheet)

        Select Case nombrebuque
            Case "CEDROS", "ISLA CEDROS"
                hoja.Cell(5, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(5, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1 'SUBSIDIO
                hoja.Cell(5, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1 'DESC ASM   
                hoja.Cell(5, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1  'IMSS
                hoja.Cell(5, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1  'SAR
                hoja.Cell(5, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1  'INFONAVIT
                hoja.Cell(5, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(5, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(5, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M5+Q5+R5+S5+T5)*6%" 'retencion

            Case "ISLA SAN JOSE"
                hoja.Cell(6, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(6, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(6, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(6, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M6+Q6+R6+S6+T6)*6%"
            Case "ISLA GRANDE"
                hoja.Cell(7, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(7, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(7, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(7, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M7+Q7+R7+S7+T7)*6%"
            Case "ISLA MIRAMAR"
                hoja.Cell(8, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(8, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(8, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(8, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M8+Q8+R8+S8+T8)*6%"
            Case "ISLA MONSERRAT", "ISLA MONTSERRAT"
                hoja.Cell(9, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(9, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(9, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(9, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M9+Q9+R9+S9+T9)*6%"
            Case "ISLA BLANCA"
                hoja.Cell(10, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(10, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(10, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(10, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M10+Q10+R10+S10+T10)*6%"
            Case "ISLA CIARI"
                hoja.Cell(11, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(11, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(11, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(11, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M11+Q11+R11+S11+T11)*6%"
            Case "ISLA JANITZIO"
                hoja.Cell(12, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(12, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(12, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(12, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M12+Q12+R12+S12+T12)*6%"

            Case "ISLA SAN GABRIEL"
                hoja.Cell(13, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(13, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(13, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(13, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M13+Q13+R13+S13+T13)*6%"
            Case "AMARRADOS"
                hoja.Cell(14, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(14, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(14, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(14, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M14+Q14+R14+S14+T14)*6%"
            Case "ISLA ARBOLEDA"
                hoja.Cell(15, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(15, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(15, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(15, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M15+Q15+R15+S15+T15)*6%"

            Case "ISLA AZTECA"
                hoja.Cell(16, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(16, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(16, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(16, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M16+Q16+R16+S16+T16)*6%"

            Case "ISLA SAN DIEGO", "ISLA DIEGO"
                hoja.Cell(17, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(17, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(17, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(17, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M17+Q17+R17+S17+T17)*6%"
            Case "ISLA SAN IGNACIO", "ISLA IGNACIO"
                hoja.Cell(18, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(18, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(18, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(18, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M18+Q18+R18+S18+T18)*6%"
            Case "ISLA SAN LUIS"
                hoja.Cell(19, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(19, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(19, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(19, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M19+Q19+R19+S19+T19)*6%"
            Case "ISLA SANTA CRUZ"
                hoja.Cell(20, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(20, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(20, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(20, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M20+Q20+R20+S20+T20)*6%"
            Case "ISLA VERDE"
                hoja.Cell(21, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(21, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(21, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(21, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M21+Q21+R21+S21+T21)*6%"
            Case "ISLA CRECIENTE"
                hoja.Cell(22, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(22, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(22, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(22, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M22+Q22+R22+S22+T22)*6%"
            Case "ISLA COLORADA"
                hoja.Cell(23, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(23, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(23, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(23, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M23+Q23+R23+S23+T23)*6%"
            Case "SUBSEA 88"
                hoja.Cell(24, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(24, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(24, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(24, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M24+Q24+R24+S24+T24)*6%"
            Case "ISLA LEON"
                hoja.Cell(25, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(25, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(25, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(25, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M25+Q25+R25+S25+T25)*6%"
            Case "NEVADO DE COLIMA", "NEVADO COLIMA"
                hoja.Cell(26, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(26, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(26, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(26, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M26+Q26+R26+S26+T26)*6%"
            Case "RED FISH"
                hoja.Cell(27, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(27, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(27, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(27, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M27+Q27+R27+S27+T27)*6%"

            Case "PROYECTO MAERSK"
                hoja.Cell(28, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(28, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(28, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(28, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M28+Q28+R28+S28+T28)*6%"

            Case "PROYECTO BELUGA 2", "PROYECTO BELUGA2", "BELUGA 2", "BELUGA2"
                hoja.Cell(29, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(29, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(29, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(29, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M29+Q29+R29+S29+T29)*6%"

            Case "PROYECTO GO CANOPUS", "GO CANOPUS"
                hoja.Cell(30, 4).FormulaA1 = "=DESGLOSE!H" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 5).FormulaA1 = "=DESGLOSE!I" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 6).FormulaA1 = "=DESGLOSE!J" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 7).FormulaA1 = "=DESGLOSE!K" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 8).FormulaA1 = "=DESGLOSE!L" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 9).FormulaA1 = "=DESGLOSE!M" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 10).FormulaA1 = "=DESGLOSE!N" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 11).FormulaA1 = "=DESGLOSE!O" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 12).FormulaA1 = "=DESGLOSE!P" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 13).FormulaA1 = "=DESGLOSE!R" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 14).FormulaA1 = "=DESGLOSE!S" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 15).FormulaA1 = "=DESGLOSE!T" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 16).FormulaA1 = "=DESGLOSE!U" & contadorexcelbuquefinal + 1 & "+DESGLOSE!V" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 17).FormulaA1 = "=DESGLOSE!AC" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 18).FormulaA1 = "=DESGLOSE!AD" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 19).FormulaA1 = "=DESGLOSE!AE" & contadorexcelbuquefinal + 1
                hoja.Cell(30, 20).FormulaA1 = "=DESGLOSE!AF" & contadorexcelbuquefinal + 1  'IMPTO
                hoja.Cell(30, 21).FormulaA1 = "=DESGLOSE!AH" & contadorexcelbuquefinal + 1  'SUBTOTAL
                hoja.Cell(30, 23).FormulaA1 = "=+(DESGLOSE!Q" & contadorexcelbuquefinal + 1 & "+DESGLOSE!AB" & contadorexcelbuquefinal + 1 & "+M30+Q30+R30+S30+T30)*6%"
        End Select


    End Sub

    Private Sub tsbImportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbImportar.Click

    End Sub

    Private Sub cmdincidencias_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdincidencias.Click
        Try
            Dim Forma As New frmSubirIncidencias
            Forma.gIdPeriodo = cboperiodo.SelectedValue
            Forma.gIdSerie = cboserie.SelectedIndex
            Forma.gAnioActual = aniocostosocial
            Forma.ShowDialog()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cmdreiniciar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdreiniciar.Click
        Try
            Dim sql As String
            Dim resultado As Integer = MessageBox.Show("Se borraran los datos de la nomina y empezara de 0,¿Desea reiniciar la nomina?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then

                sql = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                sql &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
                sql &= " and iTipoNomina=0"

                Dim rwNominaGuardadaFinal As DataRow() = nConsulta(sql)



                If rwNominaGuardadaFinal Is Nothing = False Then
                    MessageBox.Show("La nomina ya esta marcada como final, no  se pueden guardar cambios.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    'MessageBox.Show("Se borraran los datos tanto de la nomina abordo como la de descanso", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    sql = "delete from Nomina"
                    sql &= " where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iEstatusNomina=0 and iEstatus=1 and iEstatusEmpleado=0" '& cboserie.SelectedIndex
                    'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    'borrar el detalle del infonavit


                    sql = "delete from DetalleDescInfonavit"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    'sql &= " and iSerie=" & cboserie.SelectedIndex
                    'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex




                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from DetalleFonacot"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from PagoPrestamo"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Prestamo Asimilados. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If



                    sql = " delete from PagoPrestamoSA"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Prestamo Sa. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from DetallePensionAlimenticia"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Pension. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    MessageBox.Show("Nomina reiniciada correctamente, vuelva a cargar los datos", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    dtgDatos.DataSource = ""
                    dtgDatos.Columns.Clear()
                End If



            End If




        Catch ex As Exception

        End Try


    End Sub

    Private Sub tsbIEmpleados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsbIEmpleados.Click
        Try
            Dim Forma As New frmEmpleados
            Forma.gIdEmpresa = gIdEmpresa
            Forma.gIdPeriodo = cboperiodo.SelectedValue
            Forma.gIdTipoPuesto = 1
            Forma.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dtgDatos_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dtgDatos.CellClick
        Try
            If e.ColumnIndex = 0 Then
                dtgDatos.Rows(e.RowIndex).Cells(0).Value = Not dtgDatos.Rows(e.RowIndex).Cells(0).Value


            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub dtgDatos_CellEnter(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dtgDatos.CellEnter
        'MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    Private Sub TextboxNumeric_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        Try
            'Dim columna As Integer
            'Dim fila As Integer

            'columna = CInt(DirectCast(sender, System.Windows.Forms.DataGridView).CurrentCell.ColumnIndex)
            'Fila = CInt(DirectCast(sender, System.Windows.Forms.DataGridView).CurrentCell.RowIndex)


            Dim nonNumberEntered As Boolean

            nonNumberEntered = True

            If (Convert.ToInt32(e.KeyChar) >= 48 AndAlso Convert.ToInt32(e.KeyChar) <= 57) OrElse Convert.ToInt32(e.KeyChar) = 8 OrElse Convert.ToInt32(e.KeyChar) = 46 Then

                'If Convert.ToInt32(e.KeyChar) = 46 Then
                '    If InStr(dtgDatos.Rows(Fila).Cells(columna).Value, ".") = 0 Then
                '        nonNumberEntered = False
                '    Else
                '        nonNumberEntered = False
                '    End If
                'Else
                '    nonNumberEntered = False
                'End If
                nonNumberEntered = False
            End If

            If nonNumberEntered = True Then
                ' Stop the character from being entered into the control since it is non-numerical.
                e.Handled = True
            Else
                e.Handled = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub

    Private Sub dtgDatos_CellEndEdit(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dtgDatos.CellEndEdit
        Try
            If Not m_currentControl Is Nothing Then
                RemoveHandler m_currentControl.KeyPress, AddressOf TextboxNumeric_KeyPress
            End If
            If Not dgvCombo Is Nothing Then
                RemoveHandler dgvCombo.SelectedIndexChanged, AddressOf dvgCombo_SelectedIndexChanged
            End If
            If dgvCombo IsNot Nothing Then
                RemoveHandler dgvCombo.SelectedIndexChanged, New EventHandler(AddressOf dvgCombo_SelectedIndexChanged)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub dtgDatos_EditingControlShowing(ByVal sender As Object, ByVal e As DataGridViewEditingControlShowingEventArgs) Handles dtgDatos.EditingControlShowing
        Try
            Dim columna As Integer
            m_currentControl = Nothing
            columna = CInt(DirectCast(sender, System.Windows.Forms.DataGridView).CurrentCell.ColumnIndex)
            If columna = 15 Or columna = 18 Or columna = 39 Or columna = 40 Or columna = 41 Or columna = 42 Or columna = 43 Or columna = 10 Then
                AddHandler e.Control.KeyPress, AddressOf TextboxNumeric_KeyPress
                m_currentControl = e.Control
            End If


            dgvCombo = TryCast(e.Control, DataGridViewComboBoxEditingControl)

            If dgvCombo IsNot Nothing Then
                AddHandler dgvCombo.SelectedIndexChanged, New EventHandler(AddressOf dvgCombo_SelectedIndexChanged)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try




    End Sub

    Private Sub dtgDatos_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs) Handles dtgDatos.ColumnHeaderMouseClick
        Try
            'Dim newColumn As DataGridViewColumn = dtgDatos.Columns(e.ColumnIndex)

            'Dim sql As String
            'If e.ColumnIndex = 0 Then
            '    dtgDatos.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            'Else
            '    If e.ColumnIndex = 11 Then
            '        'DirectCast(dtgDatos.Columns(11), DataGridViewComboBoxColumn).Sorted = True
            '        Dim resultado As Integer = MessageBox.Show("Para realizar este ordenamiento es necesario guardar la nomina primeramente, ¿desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            '        If resultado = DialogResult.Yes Then

            '            cmdguardarnomina_Click(sender, e)
            '            campoordenamiento = "nomina.Puesto,cNombreLargo"
            '            llenargrid()
            '        End If

            '    End If

            '    If e.ColumnIndex = 12 Then
            '        Dim resultado As Integer = MessageBox.Show("Para realizar este ordenamiento es necesario guardar la nomina primeramente, ¿desea continuar?", "Pregunta", MessageBoxButtons.YesNo)
            '        If resultado = DialogResult.Yes Then

            '            cmdguardarnomina_Click(sender, e)
            '            campoordenamiento = "Nomina.Buque,cNombreLargo"
            '            llenargrid()
            '        End If
            '    End If
            '    'dtgDatos.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Automatic
            'End If

            'For x As Integer = 0 To dtgDatos.Rows.Count - 1

            '    sql = "select * from empleadosC where iIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
            '    Dim rwFila As DataRow() = nConsulta(sql)



            '    CType(Me.dtgDatos.Rows(x).Cells(11), DataGridViewComboBoxCell).Value = rwFila(0)("cPuesto").ToString()

            '    CType(Me.dtgDatos.Rows(x).Cells(12), DataGridViewComboBoxCell).Value = rwFila(0)("cFuncionesPuesto").ToString()
            '    dtgDatos.Rows(x).Cells(1).Value = x + 1
            'Next


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAll.CheckedChanged
        For x As Integer = 0 To dtgDatos.Rows.Count - 1
            dtgDatos.Rows(x).Cells(0).Value = Not dtgDatos.Rows(x).Cells(0).Value
        Next
        chkAll.Text = IIf(chkAll.Checked, "Desmarcar todos", "Marcar todos")
    End Sub

    Private Sub cmdlayouts_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdlayouts.Click
        Dim dialogo As New SaveFileDialog()
        Dim sRenglon As String = Nothing
        Dim strStreamW As Stream = Nothing
        Dim strStreamWriter As StreamWriter = Nothing
        Dim ContenidoArchivo As String = Nothing
        Dim validar As Boolean
        Dim numcuenta As String
        Dim nombre As String
        Dim PathArchivo As String
        Dim contador As Integer
        Dim contador2 As Integer
        Dim sql As String

        Try

            If cbobancos.SelectedIndex = 0 Then
                dialogo.DefaultExt = "*.txt"
                dialogo.FileName = "Layout"
                dialogo.Filter = "Archivos de texto (*.txt)|*.txt"
                dialogo.ShowDialog()
                PathArchivo = ""
                PathArchivo = dialogo.FileName

                If PathArchivo <> "" Then
                    strStreamW = File.Create(PathArchivo) ' lo creamos
                    strStreamWriter = New StreamWriter(strStreamW, System.Text.Encoding.Default) ' tipo de codificacion para escritura

                    contador = 1
                    sRenglon = ""
                    For x As Integer = 0 To dtgDatos.Rows.Count - 1


                        'BANCO RECEPTOR 

                        numcuenta = ""

                        sql = "select * from EmpleadosC where iIdEmpleadoC = " & dtgDatos.Rows(x).Cells(2).Value
                        Dim rwDatosCuenta As DataRow() = nConsulta(sql)

                        If rwDatosCuenta Is Nothing = False Then
                            If rwDatosCuenta(0)("clabe").ToString.Length < 17 Then
                                MessageBox.Show("La cuenta del trabajador " & dtgDatos.Rows(x).Cells(3).Value & ", no tiene los 18 digitos", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Exit Try
                            Else
                                sRenglon = rwDatosCuenta(0)("clabe").ToString
                            End If



                        Else
                            MessageBox.Show("Falta la cuenta del trabajador " & dtgDatos.Rows(x).Cells(3).Value, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Try
                        End If
                        sRenglon &= ","
                        sRenglon &= dtgDatos.Rows(x).Cells(70).Value
                        sRenglon &= ",,PAGODENOMINA,1234567"


                        strStreamWriter.WriteLine(sRenglon)
                        contador = contador + 1


                    Next
                    'escribimos en el archivo



                    strStreamWriter.Close() ' cerramos

                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


                End If





            Else
                ' es el stp
                dialogo.DefaultExt = "*.txt"
                dialogo.FileName = "Layout"
                dialogo.Filter = "Archivos de texto (*.txt)|*.txt"
                dialogo.ShowDialog()
                PathArchivo = ""
                PathArchivo = dialogo.FileName

                If PathArchivo <> "" Then
                    strStreamW = File.Create(PathArchivo) ' lo creamos
                    strStreamWriter = New StreamWriter(strStreamW, System.Text.Encoding.Default) ' tipo de codificacion para escritura

                    contador = 1
                    sRenglon = ""
                    sRenglon = "INSTITUCION_CONTRAPARTE" & vbTab & "CLAVE_RASTREO" & vbTab & "NOMBRE_BENEFICIARIO" & vbTab & "RFC_CURP_BENEFICIARIO" & vbTab
                    sRenglon = "TIPO_PAGO" & vbTab & "TIPO_CUENTA_BENEFICIARIO" & vbTab & "MONTO" & vbTab & "CUENTA_BENEFICIARIO" & vbTab
                    sRenglon = "CONCEPTO_PAGO" & vbTab & "REFERENCIA_NUMERICA" & vbTab & "INSTITUCION_OPERANTE" & vbTab & "EMPRESA"
                    strStreamWriter.WriteLine(sRenglon)
                    sRenglon = ""
                    For x As Integer = 0 To dtgDatos.Rows.Count - 1


                        'BANCO RECEPTOR 

                        numcuenta = ""


                        sql = "select * from EmpleadosC where iIdEmpleadoC = " & dtgDatos.Rows(x).Cells(2).Value
                        Dim rwDatosCuenta As DataRow() = nConsulta(sql)

                        If rwDatosCuenta Is Nothing = False Then
                            If rwDatosCuenta(0)("clabe").ToString.Length < 17 Then
                                MessageBox.Show("La cuenta del trabajador " & dtgDatos.Rows(x).Cells(3).Value & ", no tiene los 18 digitos", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Exit Try
                            Else
                                sRenglon = rwDatosCuenta(0)("clabe").ToString
                            End If



                        Else
                            MessageBox.Show("Falta la cuenta del trabajador " & dtgDatos.Rows(x).Cells(3).Value, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Try
                        End If
                        sRenglon &= ","
                        sRenglon &= dtgDatos.Rows(x).Cells(70).Value
                        sRenglon &= ",,PAGODENOMINA,1234567"


                        strStreamWriter.WriteLine(sRenglon)
                        contador = contador + 1


                    Next
                    'escribimos en el archivo



                    strStreamWriter.Close() ' cerramos

                    MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Function RemoverBasura(ByVal nombre As String) As String
        Dim COMPSTR As String = "áéíóúÁÉÍÓÚ.ñÑ"
        Dim REPLSTR As String = "aeiouAEIOU nN"
        Dim Posicion As Integer
        Dim cadena As String = ""
        Dim arreglo As Char() = nombre.ToCharArray()
        For x As Integer = 0 To arreglo.Length - 1
            Posicion = COMPSTR.IndexOf(arreglo(x))
            If Posicion <> -1 Then
                arreglo(x) = REPLSTR(Posicion)

            End If
            cadena = cadena & arreglo(x)
        Next
        Return cadena
    End Function

    Function TipoCuentaBanco(ByVal idempleado As String, ByVal idempresa As String) As String
        'Agregar el banco y el tipo de cuenta ya sea a terceros o interbancaria
        'Buscamos el banco y verificarmos el tipo de cuenta a tercero o interbancaria
        Dim Sql As String
        Dim cadenabanco As String
        cadenabanco = ""

        Sql = "select iIdempleadoC,NumCuenta,Clabe,cuenta2,clabe2,fkiIdBanco,fkiIdBanco2"
        Sql &= " from empleadosC"
        Sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & idempleado

        Dim rwDatosBanco As DataRow() = nConsulta(Sql)

        cadenabanco = "@"

        If rwDatosBanco Is Nothing = False Then
            If rwDatosBanco(0)("NumCuenta") = "" Then
                cadenabanco &= "I"
            Else
                cadenabanco &= "T"
            End If

            If rwDatosBanco(0)("fkiIdBanco") = "1" Then
                cadenabanco &= "-BANAMEX"
            ElseIf rwDatosBanco(0)("fkiIdBanco") = "4" Then
                cadenabanco &= "-BANCOMER"
            ElseIf rwDatosBanco(0)("fkiIdBanco") = "13" Then
                cadenabanco &= "-SCOTIABANK"
            ElseIf rwDatosBanco(0)("fkiIdBanco") = "18" Then
                cadenabanco &= "-BANORTE"
            Else
                cadenabanco &= "-OTRO"
            End If

            cadenabanco &= "/"

            If rwDatosBanco(0)("cuenta2") = "" Then
                cadenabanco &= "I"
            Else
                cadenabanco &= "T"
            End If

            If rwDatosBanco(0)("fkiIdBanco2") = "1" Then
                cadenabanco &= "-BANAMEX"
            ElseIf rwDatosBanco(0)("fkiIdBanco2") = "4" Then
                cadenabanco &= "-BANCOMER"
            ElseIf rwDatosBanco(0)("fkiIdBanco2") = "13" Then
                cadenabanco &= "-SCOTIABANK"
            ElseIf rwDatosBanco(0)("fkiIdBanco2") = "18" Then
                cadenabanco &= "-BANORTE"
            Else
                cadenabanco &= "-OTRO"
            End If


        End If

        Return cadenabanco
    End Function

    Function CalculoPrimaSA(ByVal idempleado As String, ByVal idempresa As String, porcentajeprima As Double, parte As String, sd As Double, umavalor As Double) As String
        'Agregar el banco y el tipo de cuenta ya sea a terceros o interbancaria
        'Buscamos el banco y verificarmos el tipo de cuenta a tercero o interbancaria
        Dim Sql As String
        Dim cadenabanco As String
        Dim dia As String
        Dim mes As String
        Dim anio As String
        Dim anios As Integer
        Dim sueldodiario As Double
        Dim dias As Integer
        Dim BaseExento As Double
        Dim Excento As Double
        Dim gravado As Double
        Dim Prima As String


        cadenabanco = ""


        Sql = "select *"
        Sql &= " from empleadosC"
        Sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & idempleado

        Dim rwDatosBanco As DataRow() = nConsulta(Sql)

        cadenabanco = "@"
        Prima = "0"
        If rwDatosBanco Is Nothing = False Then

            If Double.Parse(rwDatosBanco(0)("fsueldoOrd")) > 0 Then
                dia = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Day.ToString("00")
                mes = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Month.ToString("00")
                anio = Date.Today.Year
                'verificar el periodo para saber si queda entre el rango de fecha

                sueldodiario = Double.Parse(rwDatosBanco(0)("fsueldoOrd")) / diasperiodo

                Sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                Dim rwPeriodo As DataRow() = nConsulta(Sql)

                If rwPeriodo Is Nothing = False Then
                    Dim FechaBuscar As Date = Date.Parse(dia & "/" & mes & "/" & anio)
                    Dim FechaInicial As Date = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                    Dim FechaFinal As Date = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    Dim FechaAntiguedad As Date = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))

                    If FechaBuscar.CompareTo(FechaInicial) >= 0 And FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                        'Estamos dentro del rango 
                        'Calculamos la prima

                        anios = DateDiff("yyyy", FechaAntiguedad, FechaBuscar)

                        dias = CalculoDiasVacaciones(anios)

                        'Calcular prima

                        Prima = Math.Round(sd * dias * (porcentajeprima / 100), 2).ToString()
                        BaseExento = 15 * umavalor
                        If Prima >= BaseExento Then
                            Excento = BaseExento
                            gravado = Prima - BaseExento
                        Else
                            Excento = Prima
                            gravado = 0
                        End If

                        If parte = 1 Then
                            Prima = gravado
                        End If
                        If parte = 2 Then
                            Prima = Excento
                        End If

                    End If


                End If


            End If


        End If


        Return Prima


    End Function

    Function CalculoPrimaExcedente(ByVal idempleado As String, ByVal idempresa As String, porcentajeprima As Double) As String
        'Agregar el banco y el tipo de cuenta ya sea a terceros o interbancaria
        'Buscamos el banco y verificarmos el tipo de cuenta a tercero o interbancaria
        Dim Sql As String
        Dim cadenabanco As String
        Dim dia As String
        Dim mes As String
        Dim anio As String
        Dim anios As Integer
        Dim sueldodiario As Double
        Dim dias As Integer

        Dim Prima As String


        cadenabanco = ""


        Sql = "select *"
        Sql &= " from empleadosC"
        Sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & idempleado

        Dim rwDatosBanco As DataRow() = nConsulta(Sql)

        cadenabanco = "@"
        Prima = "0"
        If rwDatosBanco Is Nothing = False Then

            If Double.Parse(rwDatosBanco(0)("fsueldoOrd")) > 0 Then
                dia = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Day.ToString("00")
                mes = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Month.ToString("00")
                anio = Date.Today.Year
                'verificar el periodo para saber si queda entre el rango de fecha

                sueldodiario = Double.Parse(rwDatosBanco(0)("fsueldoOrd")) / 30

                Sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                Dim rwPeriodo As DataRow() = nConsulta(Sql)

                If rwPeriodo Is Nothing = False Then
                    Dim FechaBuscar As Date = Date.Parse(dia & "/" & mes & "/" & anio)
                    Dim FechaInicial As Date = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                    Dim FechaFinal As Date = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    Dim FechaAntiguedad As Date = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))

                    If FechaBuscar.CompareTo(FechaInicial) >= 0 And FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                        'Estamos dentro del rango 
                        'Calculamos la prima

                        anios = DateDiff("yyyy", FechaAntiguedad, FechaBuscar)

                        dias = CalculoDiasVacaciones(anios)

                        'Calcular prima

                        Prima = Math.Round(sueldodiario * dias * (porcentajeprima / 100), 2).ToString()




                    End If


                End If


            End If


        End If


        Return Prima


    End Function


    Function CalculoDiasVacaciones(ByVal anios As Integer) As Integer
        Dim dias As Integer

        If anios = 1 Then
            dias = 12
        End If

        If anios = 2 Then
            dias = 14
        End If

        If anios = 3 Then
            dias = 16
        End If

        If anios = 4 Then
            dias = 18
        End If

        If anios = 5 Then
            dias = 20
        End If

        If anios >= 6 And anios <= 10 Then
            dias = 22
        End If

        If anios >= 11 And anios <= 15 Then
            dias = 24
        End If

        If anios >= 16 And anios <= 20 Then
            dias = 26
        End If

        If anios >= 21 And anios <= 25 Then
            dias = 28
        End If

        If anios >= 26 And anios <= 30 Then
            dias = 30
        End If

        If anios >= 31 And anios <= 35 Then
            dias = 32
        End If

        Return dias
    End Function

    Private Sub dtgDatos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgDatos.CellContentClick
        Try
            If e.RowIndex = -1 And e.ColumnIndex = 0 Then

                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub tsbEmpleados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEmpleados.Click
        Dim frm As New frmImportarEmpleadosAlta
        frm.ShowDialog()
    End Sub

    Private Sub dtgDatos_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dtgDatos.DataError
        Try
            e.Cancel = True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub EliminarDeLaListaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EliminarDeLaListaToolStripMenuItem.Click
        If dtgDatos.CurrentRow Is Nothing = False Then
            Dim resultado As Integer = MessageBox.Show("¿Desea eliminar a este trabajador de la lista?", "Pregunta", MessageBoxButtons.YesNo)
            If resultado = DialogResult.Yes Then

                dtgDatos.Rows.Remove(dtgDatos.CurrentRow)
            End If
        End If


    End Sub

    Private Sub cbodias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cboserie_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboserie.SelectedIndexChanged
        dtgDatos.Columns.Clear()
        dtgDatos.DataSource = ""


    End Sub

    Private Sub cmdrecibosA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActualizarS.Click
        Try
            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count




            For x As Integer = 0 To dtgDatos.Rows.Count - 1
                sql = "select *"
                sql &= " from empleadosC"
                sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                Dim rwDatosBanco As DataRow() = nConsulta(sql)


                If rwDatosBanco Is Nothing = False Then
                    dtgDatos.Rows(x).Cells(24).Value = rwDatosBanco(0)("fSueldoBase")
                    dtgDatos.Rows(x).Cells(25).Value = rwDatosBanco(0)("fSueldoIntegrado")
                    dtgDatos.Rows(x).Cells(23).Value = rwDatosBanco(0)("fSueldoOrd")
                End If





                pgbProgreso.Value += 1
                Application.DoEvents()
            Next

            'verificar costo social

            Dim contador, Posicion1, Posicion2, Posicion3, Posicion4, Posicion5 As Integer



            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception

        End Try
    End Sub


    Private Sub cboTipoNomina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'dtgDatos.Columns.Clear()
        'dtgDatos.DataSource = ""

    End Sub

    Private Sub AgregarTrabajadoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AgregarTrabajadoresToolStripMenuItem.Click
        Try
            Dim Forma As New frmAgregarEmpleado
            Dim ids As String()
            Dim sql As String
            Dim cadenaempleados As String
            If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then






                ids = Forma.gidEmpleados.Split(",")
                If dtgDatos.Rows.Count > 0 Then

                    If Agregartrabajadores Then
                        dsPeriodo.Tables("Tabla").Rows.Clear()
                    Else
                        dsPeriodo2.Tables("Tabla").Rows.Clear()

                    End If

                    'Dim dt As DataTable = DirectCast(dtgDatos.DataSource, DataTable)
                    'dsPeriodo.Tables("Tabla") = dtgDatos.DataSource, DataTable
                    'Dim dt As DataTable = dsPeriodo.Tables("Tabla")


                    'For y As Integer = 0 To dt.Rows.Count - 1
                    '    dsPeriodo.Tables("Tabla").ImportRow(dt.Rows[y])
                    'Next

                    'Pasamos del datagrid al dataset ya creado
                    For y As Integer = 0 To dtgDatos.Rows.Count - 1
                        Dim fila As DataRow
                        If Agregartrabajadores Then

                            fila = dsPeriodo.Tables("Tabla").NewRow
                        Else
                            fila = dsPeriodo2.Tables("Tabla").NewRow

                        End If


                        fila.Item("Consecutivo") = (y + 1).ToString
                        fila.Item("Id_empleado") = dtgDatos.Rows(y).Cells(2).Value
                        fila.Item("CodigoEmpleado") = dtgDatos.Rows(y).Cells(3).Value
                        fila.Item("Nombre") = dtgDatos.Rows(y).Cells(4).Value
                        fila.Item("Status") = dtgDatos.Rows(y).Cells(5).Value
                        fila.Item("RFC") = dtgDatos.Rows(y).Cells(6).Value
                        fila.Item("CURP") = dtgDatos.Rows(y).Cells(7).Value
                        fila.Item("Num_IMSS") = dtgDatos.Rows(y).Cells(8).Value

                        fila.Item("Fecha_Nac") = dtgDatos.Rows(y).Cells(9).Value
                        'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                        fila.Item("Edad") = dtgDatos.Rows(y).Cells(10).Value
                        fila.Item("Puesto") = dtgDatos.Rows(y).Cells(11).FormattedValue
                        fila.Item("Depto") = dtgDatos.Rows(y).Cells(12).FormattedValue

                        fila.Item("Tipo_Infonavit") = dtgDatos.Rows(y).Cells(13).Value
                        fila.Item("Valor_Infonavit") = dtgDatos.Rows(y).Cells(14).Value


                        fila.Item("Horas_extras_dobles_V") = dtgDatos.Rows(y).Cells(15).Value
                        fila.Item("Horas_extras_triples_V") = dtgDatos.Rows(y).Cells(16).Value
                        fila.Item("Descanso_Laborado_V") = dtgDatos.Rows(y).Cells(17).Value
                        fila.Item("Dia_Festivo_laborado_V") = dtgDatos.Rows(y).Cells(18).Value
                        fila.Item("Prima_Dominical_V") = dtgDatos.Rows(y).Cells(19).Value
                        fila.Item("Falta_Injustificada_V") = dtgDatos.Rows(y).Cells(20).Value
                        fila.Item("Permiso_Sin_GS_V") = dtgDatos.Rows(y).Cells(21).Value
                        fila.Item("T_No_laborado_V") = dtgDatos.Rows(y).Cells(22).Value





                        fila.Item("Sueldo_Base") = dtgDatos.Rows(y).Cells(23).Value
                        fila.Item("Salario_Diario") = dtgDatos.Rows(y).Cells(24).Value
                        'fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fFactorIntegracion").ToString
                        fila.Item("Salario_Cotización") = dtgDatos.Rows(y).Cells(25).Value
                        fila.Item("Dias_Trabajados") = dtgDatos.Rows(y).Cells(26).Value
                        fila.Item("Tipo_Incapacidad") = dtgDatos.Rows(y).Cells(27).Value
                        fila.Item("Número_días") = dtgDatos.Rows(y).Cells(28).Value
                        fila.Item("Sueldo_Bruto") = IIf(dtgDatos.Rows(y).Cells(29).Value = "", "0", dtgDatos.Rows(y).Cells(29).Value.ToString.Replace(",", ""))
                        fila.Item("Septimo_Dia") = IIf(dtgDatos.Rows(y).Cells(30).Value = "", "0", dtgDatos.Rows(y).Cells(30).Value.ToString.Replace(",", ""))
                        fila.Item("Prima_Dominical_Gravada") = IIf(dtgDatos.Rows(y).Cells(31).Value = "", "0", dtgDatos.Rows(y).Cells(31).Value.ToString.Replace(",", ""))
                        fila.Item("Prima_Dominical_Exenta") = IIf(dtgDatos.Rows(y).Cells(32).Value = "", "0", dtgDatos.Rows(y).Cells(32).Value.ToString.Replace(",", ""))
                        fila.Item("Tiempo_Extra_Doble_Gravado") = IIf(dtgDatos.Rows(y).Cells(33).Value = "", "0", dtgDatos.Rows(y).Cells(33).Value.ToString.Replace(",", ""))
                        fila.Item("Tiempo_Extra_Doble_Exento") = IIf(dtgDatos.Rows(y).Cells(34).Value = "", "0", dtgDatos.Rows(y).Cells(34).Value.ToString.Replace(",", ""))
                        fila.Item("Tiempo_Extra_Triple") = IIf(dtgDatos.Rows(y).Cells(35).Value = "", "0", dtgDatos.Rows(y).Cells(35).Value.ToString.Replace(",", ""))

                        fila.Item("Descanso_Labarado") = IIf(dtgDatos.Rows(y).Cells(36).Value = "", "0", dtgDatos.Rows(y).Cells(36).Value.ToString.Replace(",", ""))
                        fila.Item("Dia_Festivo_laborado") = IIf(dtgDatos.Rows(y).Cells(37).Value = "", "0", dtgDatos.Rows(y).Cells(37).Value.ToString.Replace(",", ""))
                        fila.Item("Bono_Asistencia") = IIf(dtgDatos.Rows(y).Cells(38).Value = "", "0", dtgDatos.Rows(y).Cells(38).Value.ToString.Replace(",", ""))
                        fila.Item("Bono_Productividad") = IIf(dtgDatos.Rows(y).Cells(39).Value = "", "0", dtgDatos.Rows(y).Cells(39).Value.ToString.Replace(",", ""))
                        fila.Item("Bono_Polivalencia") = IIf(dtgDatos.Rows(y).Cells(40).Value = "", "0", dtgDatos.Rows(y).Cells(40).Value.ToString.Replace(",", ""))
                        fila.Item("Bono_Especialidad") = IIf(dtgDatos.Rows(y).Cells(41).Value = "", "0", dtgDatos.Rows(y).Cells(41).Value.ToString.Replace(",", ""))
                        fila.Item("Bono_Calidad") = IIf(dtgDatos.Rows(y).Cells(42).Value = "", "0", dtgDatos.Rows(y).Cells(42).Value.ToString.Replace(",", ""))
                        fila.Item("Compensacion") = IIf(dtgDatos.Rows(y).Cells(43).Value = "", "0", dtgDatos.Rows(y).Cells(43).Value.ToString.Replace(",", ""))
                        fila.Item("Semana_fondo") = IIf(dtgDatos.Rows(y).Cells(44).Value = "", "0", dtgDatos.Rows(y).Cells(44).Value.ToString.Replace(",", ""))
                        fila.Item("Falta_Injustificada") = IIf(dtgDatos.Rows(y).Cells(45).Value = "", "0", dtgDatos.Rows(y).Cells(45).Value.ToString.Replace(",", ""))
                        fila.Item("Permiso_Sin_GS") = IIf(dtgDatos.Rows(y).Cells(46).Value = "", "0", dtgDatos.Rows(y).Cells(46).Value.ToString.Replace(",", ""))
                        fila.Item("Incremento_Retenido") = IIf(dtgDatos.Rows(y).Cells(47).Value = "", "0", dtgDatos.Rows(y).Cells(47).Value.ToString.Replace(",", ""))


                        fila.Item("Vacaciones_proporcionales") = IIf(dtgDatos.Rows(y).Cells(48).Value = "", "0", dtgDatos.Rows(y).Cells(48).Value.ToString.Replace(",", ""))
                        fila.Item("Aguinaldo_gravado") = IIf(dtgDatos.Rows(y).Cells(49).Value = "", "0", dtgDatos.Rows(y).Cells(49).Value.ToString.Replace(",", ""))
                        fila.Item("Aguinaldo_exento") = IIf(dtgDatos.Rows(y).Cells(50).Value = "", "0", dtgDatos.Rows(y).Cells(50).Value.ToString.Replace(",", ""))
                        fila.Item("Total_Aguinaldo") = IIf(dtgDatos.Rows(y).Cells(51).Value = "", "0", dtgDatos.Rows(y).Cells(51).Value.ToString.Replace(",", ""))
                        fila.Item("Prima_vac_gravado") = IIf(dtgDatos.Rows(y).Cells(52).Value = "", "0", dtgDatos.Rows(y).Cells(52).Value.ToString.Replace(",", ""))
                        fila.Item("Prima_vac_exento") = IIf(dtgDatos.Rows(y).Cells(53).Value = "", "0", dtgDatos.Rows(y).Cells(53).Value.ToString.Replace(",", ""))
                        fila.Item("Total_Prima_vac") = IIf(dtgDatos.Rows(y).Cells(54).Value = "", "0", dtgDatos.Rows(y).Cells(54).Value.ToString.Replace(",", ""))

                        fila.Item("Total_percepciones") = IIf(dtgDatos.Rows(y).Cells(55).Value = "", "0", dtgDatos.Rows(y).Cells(55).Value.ToString.Replace(",", ""))
                        fila.Item("Total_percepciones_p/isr") = IIf(dtgDatos.Rows(y).Cells(56).Value = "", "0", dtgDatos.Rows(y).Cells(56).Value.ToString.Replace(",", ""))

                        fila.Item("Incapacidad") = IIf(dtgDatos.Rows(y).Cells(57).Value = "", "0", dtgDatos.Rows(y).Cells(57).Value.ToString.Replace(",", ""))
                        fila.Item("ISR") = IIf(dtgDatos.Rows(y).Cells(58).Value = "", "0", dtgDatos.Rows(y).Cells(58).Value.ToString.Replace(",", ""))
                        fila.Item("IMSS") = IIf(dtgDatos.Rows(y).Cells(59).Value = "", "0", dtgDatos.Rows(y).Cells(59).Value.ToString.Replace(",", ""))
                        fila.Item("Infonavit") = IIf(dtgDatos.Rows(y).Cells(60).Value = "", "0", dtgDatos.Rows(y).Cells(60).Value.ToString.Replace(",", ""))
                        fila.Item("Infonavit_bim_anterior") = IIf(dtgDatos.Rows(y).Cells(61).Value = "", "0", dtgDatos.Rows(y).Cells(61).Value.ToString.Replace(",", ""))
                        fila.Item("Ajuste_infonavit") = IIf(dtgDatos.Rows(y).Cells(62).Value = "", "0", dtgDatos.Rows(y).Cells(62).Value.ToString.Replace(",", ""))
                        fila.Item("Pension_Alimenticia") = IIf(dtgDatos.Rows(y).Cells(63).Value = "", "0", dtgDatos.Rows(y).Cells(63).Value.ToString.Replace(",", ""))
                        fila.Item("Prestamo") = IIf(dtgDatos.Rows(y).Cells(64).Value = "", "0", dtgDatos.Rows(y).Cells(64).Value.ToString.Replace(",", ""))
                        fila.Item("Fonacot") = IIf(dtgDatos.Rows(y).Cells(65).Value = "", "0", dtgDatos.Rows(y).Cells(65).Value.ToString.Replace(",", ""))
                        fila.Item("T_No_laborado") = IIf(dtgDatos.Rows(y).Cells(66).Value = "", "0", dtgDatos.Rows(y).Cells(66).Value.ToString.Replace(",", ""))
                        fila.Item("Cuota_Sindical") = IIf(dtgDatos.Rows(y).Cells(67).Value = "", "0", dtgDatos.Rows(y).Cells(67).Value.ToString.Replace(",", ""))

                        fila.Item("Subsidio_Generado") = IIf(dtgDatos.Rows(y).Cells(68).Value = "", "0", dtgDatos.Rows(y).Cells(68).Value.ToString.Replace(",", ""))
                        fila.Item("Subsidio_Aplicado") = IIf(dtgDatos.Rows(y).Cells(69).Value = "", "0", dtgDatos.Rows(y).Cells(69).Value.ToString.Replace(",", ""))
                        fila.Item("Neto_SA") = IIf(dtgDatos.Rows(y).Cells(70).Value = "", "0", dtgDatos.Rows(y).Cells(70).Value.ToString.Replace(",", ""))
                        fila.Item("Prestamo_Personal_A") = IIf(dtgDatos.Rows(y).Cells(71).Value = "", "0", dtgDatos.Rows(y).Cells(71).Value.ToString.Replace(",", ""))
                        fila.Item("Adeudo_Infonavit_A") = IIf(dtgDatos.Rows(y).Cells(72).Value = "", "0", dtgDatos.Rows(y).Cells(72).Value.ToString.Replace(",", ""))
                        fila.Item("PA_A") = IIf(dtgDatos.Rows(y).Cells(73).Value = "", "0", dtgDatos.Rows(y).Cells(73).Value.ToString.Replace(",", ""))
                        fila.Item("SINDICATO/PPP") = IIf(dtgDatos.Rows(y).Cells(74).Value = "", "0", dtgDatos.Rows(y).Cells(74).Value.ToString.Replace(",", ""))
                        fila.Item("PRIMA_EXCEN") = IIf(dtgDatos.Rows(y).Cells(75).Value = "", "0", dtgDatos.Rows(y).Cells(75).Value.ToString.Replace(",", ""))
                        fila.Item("%_Comisión") = IIf(dtgDatos.Rows(y).Cells(76).Value = "", "0", dtgDatos.Rows(y).Cells(76).Value.ToString.Replace(",", ""))
                        fila.Item("Comisión_SA") = IIf(dtgDatos.Rows(y).Cells(77).Value = "", "0", dtgDatos.Rows(y).Cells(77).Value.ToString.Replace(",", ""))
                        fila.Item("Comisión_Beneficio") = IIf(dtgDatos.Rows(y).Cells(78).Value = "", "0", dtgDatos.Rows(y).Cells(78).Value.ToString.Replace(",", ""))
                        fila.Item("IMSS_CS") = IIf(dtgDatos.Rows(y).Cells(79).Value = "", "0", dtgDatos.Rows(y).Cells(79).Value.ToString.Replace(",", ""))
                        fila.Item("RCV_CS") = IIf(dtgDatos.Rows(y).Cells(80).Value = "", "0", dtgDatos.Rows(y).Cells(80).Value.ToString.Replace(",", ""))
                        fila.Item("Infonavit_CS") = IIf(dtgDatos.Rows(y).Cells(81).Value = "", "0", dtgDatos.Rows(y).Cells(81).Value.ToString.Replace(",", ""))
                        fila.Item("ISN_CS") = IIf(dtgDatos.Rows(y).Cells(82).Value = "", "0", dtgDatos.Rows(y).Cells(82).Value.ToString.Replace(",", ""))
                        fila.Item("Total_Costo_Social") = IIf(dtgDatos.Rows(y).Cells(83).Value = "", "0", dtgDatos.Rows(y).Cells(83).Value.ToString.Replace(",", ""))
                        fila.Item("Subtotal") = IIf(dtgDatos.Rows(y).Cells(84).Value = "", "0", dtgDatos.Rows(y).Cells(84).Value.ToString.Replace(",", ""))
                        fila.Item("IVA") = IIf(dtgDatos.Rows(y).Cells(85).Value = "", "0", dtgDatos.Rows(y).Cells(85).Value.ToString.Replace(",", ""))
                        fila.Item("TOTAL_DEPOSITO") = IIf(dtgDatos.Rows(y).Cells(86).Value = "", "0", dtgDatos.Rows(y).Cells(86).Value.ToString.Replace(",", ""))

                        fila.Item("fecha_inicio") = IIf(dtgDatos.Rows(y).Cells(87).Value = "", "0", dtgDatos.Rows(y).Cells(87).Value.ToString.Replace(",", ""))
                        fila.Item("fecha_fin") = IIf(dtgDatos.Rows(y).Cells(88).Value = "", "0", dtgDatos.Rows(y).Cells(88).Value.ToString.Replace(",", ""))


                        If Agregartrabajadores Then

                            dsPeriodo.Tables("Tabla").Rows.Add(fila)

                        Else
                            dsPeriodo2.Tables("Tabla").Rows.Add(fila)


                        End If

                    Next



                    'Agregar a la tabla los datos que vienen de la busqueda de empleados
                    For x As Integer = 0 To ids.Length - 1

                        Dim fila As DataRow
                        If Agregartrabajadores Then

                            fila = dsPeriodo.Tables("Tabla").NewRow
                        Else
                            fila = dsPeriodo2.Tables("Tabla").NewRow

                        End If
                        'Dim fila As DataRow = dt.NewRow
                        'Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow
                        sql = "select  * from empleadosC where " 'fkiIdClienteInter=-1"
                        sql &= " iIdEmpleadoC=" & ids(x)
                        sql &= " order by cFuncionesPuesto,cNombreLargo"
                        Dim rwEmpleado As DataRow() = nConsulta(sql)
                        If rwEmpleado Is Nothing = False Then

                            'Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow
                            fila.Item("Consecutivo") = (dtgDatos.Rows.Count + 1).ToString
                            fila.Item("Id_empleado") = rwEmpleado(0)("iIdEmpleadoC").ToString
                            fila.Item("CodigoEmpleado") = rwEmpleado(0)("cCodigoEmpleado").ToString
                            fila.Item("Nombre") = rwEmpleado(0)("cNombreLargo").ToString.ToUpper()
                            fila.Item("Status") = IIf(rwEmpleado(0)("iOrigen").ToString = "1", "CONFIANZA", "SINDICALIZADO")
                            fila.Item("RFC") = rwEmpleado(0)("cRFC").ToString
                            fila.Item("CURP") = rwEmpleado(0)("cCURP").ToString
                            fila.Item("Num_IMSS") = rwEmpleado(0)("cIMSS").ToString

                            fila.Item("Fecha_Nac") = Date.Parse(rwEmpleado(0)("dFechaNac").ToString).ToShortDateString()
                            'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                            fila.Item("Edad") = CalcularEdad(Date.Parse(rwEmpleado(0)("dFechaNac").ToString).Day, Date.Parse(rwEmpleado(0)("dFechaNac").ToString).Month, Date.Parse(rwEmpleado(0)("dFechaNac").ToString).Year)
                            fila.Item("Puesto") = rwEmpleado(0)("cPuesto").ToString
                            fila.Item("Depto") = "uno"

                            fila.Item("Tipo_Infonavit") = rwEmpleado(0)("cTipoFactor").ToString
                            fila.Item("Valor_Infonavit") = rwEmpleado(0)("fFactor").ToString


                            fila.Item("Horas_extras_dobles_V") = ""
                            fila.Item("Horas_extras_triples_V") = ""
                            fila.Item("Descanso_Laborado_V") = ""
                            fila.Item("Dia_Festivo_laborado_V") = ""
                            fila.Item("Prima_Dominical_V") = ""
                            fila.Item("Falta_Injustificada_V") = ""
                            fila.Item("Permiso_Sin_GS_V") = ""
                            fila.Item("T_No_laborado_V") = ""





                            fila.Item("Sueldo_Base") = rwEmpleado(0)("fSueldoOrd").ToString
                            fila.Item("Salario_Diario") = rwEmpleado(0)("fSueldoBase").ToString
                            'fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fFactorIntegracion").ToString
                            fila.Item("Salario_Cotización") = rwEmpleado(0)("fSueldoIntegrado").ToString
                            fila.Item("Dias_Trabajados") = IIf(diasperiodo > 7, 15, diasperiodo)
                            fila.Item("Tipo_Incapacidad") = TipoIncapacidad(rwEmpleado(0)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Número_días") = NumDiasIncapacidad(rwEmpleado(0)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Sueldo_Bruto") = ""
                            fila.Item("Septimo_Dia") = ""
                            fila.Item("Prima_Dominical_Gravada") = ""
                            fila.Item("Prima_Dominical_Exenta") = ""
                            fila.Item("Tiempo_Extra_Doble_Gravado") = ""
                            fila.Item("Tiempo_Extra_Doble_Exento") = ""
                            fila.Item("Tiempo_Extra_Triple") = ""

                            fila.Item("Descanso_Labarado") = ""
                            fila.Item("Dia_Festivo_laborado") = ""
                            fila.Item("Bono_Asistencia") = ""
                            fila.Item("Bono_Productividad") = ""
                            fila.Item("Bono_Polivalencia") = ""
                            fila.Item("Bono_Especialidad") = ""
                            fila.Item("Bono_Calidad") = ""
                            fila.Item("Compensacion") = ""
                            fila.Item("Semana_fondo") = ""
                            fila.Item("Falta_Injustificada") = ""
                            fila.Item("Permiso_Sin_GS") = ""
                            fila.Item("Incremento_Retenido") = ""


                            fila.Item("Vacaciones_proporcionales") = ""
                            fila.Item("Aguinaldo_gravado") = ""
                            fila.Item("Aguinaldo_exento") = ""
                            fila.Item("Total_Aguinaldo") = ""
                            fila.Item("Prima_vac_gravado") = ""
                            fila.Item("Prima_vac_exento") = ""
                            fila.Item("Total_Prima_vac") = ""

                            fila.Item("Total_percepciones") = ""
                            fila.Item("Total_percepciones_p/isr") = ""

                            fila.Item("Incapacidad") = ""
                            fila.Item("ISR") = ""
                            fila.Item("IMSS") = ""
                            fila.Item("Infonavit") = ""
                            fila.Item("Infonavit_bim_anterior") = ""
                            fila.Item("Ajuste_infonavit") = ""
                            fila.Item("Pension_Alimenticia") = ""
                            fila.Item("Prestamo") = ""
                            fila.Item("Fonacot") = ""
                            fila.Item("T_No_laborado") = ""
                            fila.Item("Cuota_Sindical") = ""


                            fila.Item("Subsidio_Generado") = ""
                            fila.Item("Subsidio_Aplicado") = ""
                            fila.Item("Neto_SA") = ""
                            fila.Item("Prestamo_Personal_A") = ""
                            fila.Item("Adeudo_Infonavit_A") = ""
                            fila.Item("PA_A") = ""
                            fila.Item("SINDICATO/PPP") = ""
                            fila.Item("PRIMA_EXCEN") = ""
                            fila.Item("%_Comisión") = ""
                            fila.Item("Comisión_SA") = ""
                            fila.Item("Comisión_Beneficio") = ""
                            fila.Item("IMSS_CS") = ""
                            fila.Item("RCV_CS") = ""
                            fila.Item("Infonavit_CS") = ""
                            fila.Item("ISN_CS") = ""
                            fila.Item("Total_Costo_Social") = ""
                            fila.Item("Subtotal") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("fecha_inicio") = ""
                            fila.Item("fecha_fin") = ""


                            'dt.Rows.Add(fila)

                            If Agregartrabajadores Then

                                dsPeriodo.Tables("Tabla").Rows.Add(fila)
                            Else
                                dsPeriodo2.Tables("Tabla").Rows.Add(fila)

                            End If

                        End If

                    Next
                    'dtgDatos.DataSource = dt
                    dtgDatos.Columns.Clear()
                    Dim chk As New DataGridViewCheckBoxColumn()
                    dtgDatos.Columns.Add(chk)
                    chk.HeaderText = ""
                    chk.Name = "chk"
                    If Agregartrabajadores Then
                        dtgDatos.DataSource = dsPeriodo.Tables("Tabla")
                        Agregartrabajadores = False
                    Else
                        dtgDatos.DataSource = dsPeriodo2.Tables("Tabla")
                        Agregartrabajadores = True

                    End If





                    MATGRID()


                    MessageBox.Show("Datos cargados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else



                    cadenaempleados = ""

                    For x As Integer = 0 To ids.Length - 1
                        If x = 0 Then
                            cadenaempleados = " iIdEmpleadoC=" & ids(x)
                        Else
                            cadenaempleados &= "  or iIdEmpleadoC=" & ids(x)
                        End If
                    Next






                    sql = "select  * from empleadosC where " 'fkiIdClienteInter=-1"
                    sql &= cadenaempleados
                    sql &= " order by cFuncionesPuesto,cNombreLargo"

                    dsPeriodo.Tables("Tabla").Rows.Clear()
                    Dim rwDatosEmpleados As DataRow() = nConsulta(sql)
                    If rwDatosEmpleados Is Nothing = False Then
                        For x As Integer = 0 To rwDatosEmpleados.Length - 1


                            Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow
                            fila.Item("Consecutivo") = (x + 1).ToString
                            fila.Item("Id_empleado") = rwDatosEmpleados(x)("iIdEmpleadoC").ToString
                            fila.Item("CodigoEmpleado") = rwDatosEmpleados(x)("cCodigoEmpleado").ToString
                            fila.Item("Nombre") = rwDatosEmpleados(x)("cNombreLargo").ToString.ToUpper()
                            fila.Item("Status") = IIf(rwDatosEmpleados(x)("iOrigen").ToString = "1", "CONFIANZA", "SINDICALIZADO")
                            fila.Item("RFC") = rwDatosEmpleados(x)("cRFC").ToString
                            fila.Item("CURP") = rwDatosEmpleados(x)("cCURP").ToString
                            fila.Item("Num_IMSS") = rwDatosEmpleados(x)("cIMSS").ToString

                            fila.Item("Fecha_Nac") = Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).ToShortDateString()
                            'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                            fila.Item("Edad") = CalcularEdad(Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Day, Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Month, Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString).Year)
                            fila.Item("Puesto") = rwDatosEmpleados(x)("cPuesto").ToString
                            fila.Item("Depto") = "uno"

                            fila.Item("Tipo_Infonavit") = rwDatosEmpleados(x)("cTipoFactor").ToString
                            fila.Item("Valor_Infonavit") = rwDatosEmpleados(x)("fFactor").ToString


                            fila.Item("Horas_extras_dobles_V") = ""
                            fila.Item("Horas_extras_triples_V") = ""
                            fila.Item("Descanso_Laborado_V") = ""
                            fila.Item("Dia_Festivo_laborado_V") = ""
                            fila.Item("Prima_Dominical_V") = ""
                            fila.Item("Falta_Injustificada_V") = ""
                            fila.Item("Permiso_Sin_GS_V") = ""
                            fila.Item("T_No_laborado_V") = ""





                            fila.Item("Sueldo_Base") = rwDatosEmpleados(x)("fSueldoOrd").ToString
                            fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fSueldoBase").ToString
                            'fila.Item("Salario_Diario") = rwDatosEmpleados(x)("fFactorIntegracion").ToString
                            fila.Item("Salario_Cotización") = rwDatosEmpleados(x)("fSueldoIntegrado").ToString
                            fila.Item("Dias_Trabajados") = "7"
                            fila.Item("Tipo_Incapacidad") = TipoIncapacidad(rwDatosEmpleados(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Número_días") = NumDiasIncapacidad(rwDatosEmpleados(x)("iIdEmpleadoc").ToString, cboperiodo.SelectedValue)
                            fila.Item("Sueldo_Bruto") = ""
                            fila.Item("Septimo_Dia") = ""
                            fila.Item("Prima_Dominical_Gravada") = ""
                            fila.Item("Prima_Dominical_Exenta") = ""
                            fila.Item("Tiempo_Extra_Doble_Gravado") = ""
                            fila.Item("Tiempo_Extra_Doble_Exento") = ""
                            fila.Item("Tiempo_Extra_Triple") = ""

                            fila.Item("Descanso_Labarado") = ""
                            fila.Item("Dia_Festivo_laborado") = ""
                            fila.Item("Bono_Asistencia") = ""
                            fila.Item("Bono_Productividad") = ""
                            fila.Item("Bono_Polivalencia") = ""
                            fila.Item("Bono_Especialidad") = ""
                            fila.Item("Bono_Calidad") = ""
                            fila.Item("Compensacion") = ""
                            fila.Item("Semana_fondo") = ""
                            fila.Item("Falta_Injustificada") = ""
                            fila.Item("Permiso_Sin_GS") = ""
                            fila.Item("Incremento_Retenido") = ""


                            fila.Item("Vacaciones_proporcionales") = ""
                            fila.Item("Aguinaldo_gravado") = ""
                            fila.Item("Aguinaldo_exento") = ""
                            fila.Item("Total_Aguinaldo") = ""
                            fila.Item("Prima_vac_gravado") = ""
                            fila.Item("Prima_vac_exento") = ""
                            fila.Item("Total_Prima_vac") = ""

                            fila.Item("Total_percepciones") = ""
                            fila.Item("Total_percepciones_p/isr") = ""

                            fila.Item("Incapacidad") = ""
                            fila.Item("ISR") = ""
                            fila.Item("IMSS") = ""
                            fila.Item("Infonavit") = ""
                            fila.Item("Infonavit_bim_anterior") = ""
                            fila.Item("Ajuste_infonavit") = ""
                            fila.Item("Pension_Alimenticia") = ""
                            fila.Item("Prestamo") = ""
                            fila.Item("Fonacot") = ""
                            fila.Item("T_No_laborado") = ""
                            fila.Item("Cuota_Sindical") = ""


                            fila.Item("Subsidio_Generado") = ""
                            fila.Item("Subsidio_Aplicado") = ""
                            fila.Item("Neto_SA") = ""
                            fila.Item("Prestamo_Personal_A") = ""
                            fila.Item("Adeudo_Infonavit_A") = ""
                            fila.Item("PA_A") = ""
                            fila.Item("SINDICATO/PPP") = ""
                            fila.Item("PRIMA_EXCEN") = ""
                            fila.Item("%_Comisión") = ""
                            fila.Item("Comisión_SA") = ""
                            fila.Item("Comisión_Beneficio") = ""
                            fila.Item("IMSS_CS") = ""
                            fila.Item("RCV_CS") = ""
                            fila.Item("Infonavit_CS") = ""
                            fila.Item("ISN_CS") = ""
                            fila.Item("Total_Costo_Social") = ""
                            fila.Item("Subtotal") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""
                            fila.Item("fecha_inicio") = ""
                            fila.Item("fecha_fin") = ""

                            dsPeriodo.Tables("Tabla").Rows.Add(fila)




                        Next

                        dtgDatos.Columns.Clear()
                        Dim chk As New DataGridViewCheckBoxColumn()
                        dtgDatos.Columns.Add(chk)
                        chk.HeaderText = ""
                        chk.Name = "chk"
                        dtgDatos.DataSource = dsPeriodo.Tables("Tabla")



                        MATGRID()

                        MessageBox.Show("Datos cargados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("No hay datos en este período", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If




                    'No hay datos en este período


                End If




                'MessageBox.Show("Trabajadores asignados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'If cboempresa.SelectedIndex > -1 Then
                '    cargarlista()
                'End If
                'lsvLista.SelectedItems(0).Tag = ""
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
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


    Private Sub cmdSubirDatos_Click(sender As System.Object, e As System.EventArgs) Handles cmdSubirDatos.Click
        Dim empleadodetectado As String = ""
        Try
            Dim Forma As New frmSubirDatos
            Dim ids As String()
            Dim sql As String
            Dim cadenaempleados As String

            If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then


                Dim dsPeriodo As New DataSet
                dsPeriodo.Tables.Add("Tabla")
                dsPeriodo.Tables("Tabla").Columns.Add("Consecutivo")
                dsPeriodo.Tables("Tabla").Columns.Add("Id_empleado")
                dsPeriodo.Tables("Tabla").Columns.Add("CodigoEmpleado")
                dsPeriodo.Tables("Tabla").Columns.Add("Nombre")
                dsPeriodo.Tables("Tabla").Columns.Add("Status")
                dsPeriodo.Tables("Tabla").Columns.Add("RFC")
                dsPeriodo.Tables("Tabla").Columns.Add("CURP")
                dsPeriodo.Tables("Tabla").Columns.Add("Num_IMSS")
                dsPeriodo.Tables("Tabla").Columns.Add("Fecha_Nac")
                dsPeriodo.Tables("Tabla").Columns.Add("Edad")
                dsPeriodo.Tables("Tabla").Columns.Add("Puesto")
                dsPeriodo.Tables("Tabla").Columns.Add("Buque")
                dsPeriodo.Tables("Tabla").Columns.Add("Tipo_Infonavit")
                dsPeriodo.Tables("Tabla").Columns.Add("Valor_Infonavit")
                dsPeriodo.Tables("Tabla").Columns.Add("Sueldo_Base")
                dsPeriodo.Tables("Tabla").Columns.Add("Salario_Diario")
                dsPeriodo.Tables("Tabla").Columns.Add("Salario_Cotización")
                dsPeriodo.Tables("Tabla").Columns.Add("Dias_Trabajados")
                dsPeriodo.Tables("Tabla").Columns.Add("Tipo_Incapacidad")
                dsPeriodo.Tables("Tabla").Columns.Add("Número_días")
                dsPeriodo.Tables("Tabla").Columns.Add("Sueldo_Bruto")
                dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Fijo_Gravado")
                dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Fijo_Exento")
                dsPeriodo.Tables("Tabla").Columns.Add("Tiempo_Extra_Ocasional")
                dsPeriodo.Tables("Tabla").Columns.Add("Desc_Sem_Obligatorio")
                dsPeriodo.Tables("Tabla").Columns.Add("Vacaciones_proporcionales")
                dsPeriodo.Tables("Tabla").Columns.Add("Aguinaldo_gravado")
                dsPeriodo.Tables("Tabla").Columns.Add("Aguinaldo_exento")
                dsPeriodo.Tables("Tabla").Columns.Add("Total_Aguinaldo")
                dsPeriodo.Tables("Tabla").Columns.Add("Prima_vac_gravado")
                dsPeriodo.Tables("Tabla").Columns.Add("Prima_vac_exento")
                dsPeriodo.Tables("Tabla").Columns.Add("Total_Prima_vac")
                dsPeriodo.Tables("Tabla").Columns.Add("Total_percepciones")
                dsPeriodo.Tables("Tabla").Columns.Add("Total_percepciones_p/isr")
                dsPeriodo.Tables("Tabla").Columns.Add("Incapacidad")
                dsPeriodo.Tables("Tabla").Columns.Add("ISR")
                dsPeriodo.Tables("Tabla").Columns.Add("IMSS")
                dsPeriodo.Tables("Tabla").Columns.Add("Infonavit")
                dsPeriodo.Tables("Tabla").Columns.Add("Infonavit_bim_anterior")
                dsPeriodo.Tables("Tabla").Columns.Add("Ajuste_infonavit")
                dsPeriodo.Tables("Tabla").Columns.Add("Pension_Alimenticia")
                dsPeriodo.Tables("Tabla").Columns.Add("Prestamo")
                dsPeriodo.Tables("Tabla").Columns.Add("Fonacot")
                dsPeriodo.Tables("Tabla").Columns.Add("Subsidio_Generado")
                dsPeriodo.Tables("Tabla").Columns.Add("Subsidio_Aplicado")
                dsPeriodo.Tables("Tabla").Columns.Add("Operadora")
                dsPeriodo.Tables("Tabla").Columns.Add("Prestamo_Personal_A")
                dsPeriodo.Tables("Tabla").Columns.Add("Adeudo_Infonavit_A")
                dsPeriodo.Tables("Tabla").Columns.Add("PA_A")
                dsPeriodo.Tables("Tabla").Columns.Add("Asimilados")
                dsPeriodo.Tables("Tabla").Columns.Add("Retenciones_Operadora")
                dsPeriodo.Tables("Tabla").Columns.Add("%_Comisión")
                dsPeriodo.Tables("Tabla").Columns.Add("Comisión_Operadora")
                dsPeriodo.Tables("Tabla").Columns.Add("Comisión_Asimilados")
                dsPeriodo.Tables("Tabla").Columns.Add("IMSS_CS")
                dsPeriodo.Tables("Tabla").Columns.Add("RCV_CS")
                dsPeriodo.Tables("Tabla").Columns.Add("Infonavit_CS")
                dsPeriodo.Tables("Tabla").Columns.Add("ISN_CS")
                dsPeriodo.Tables("Tabla").Columns.Add("Total_Costo_Social")
                dsPeriodo.Tables("Tabla").Columns.Add("Subtotal")
                dsPeriodo.Tables("Tabla").Columns.Add("IVA")
                dsPeriodo.Tables("Tabla").Columns.Add("TOTAL_DEPOSITO")

                dsPeriodo.Tables("Tabla").Columns.Add("fecha_inicio")
                dsPeriodo.Tables("Tabla").Columns.Add("fecha_fin")


                dtgDatos.Columns.Clear()
                dtgDatos.DataSource = Nothing

                'ids = Forma.gidEmpleados.Split(",")
                If dtgDatos.Rows.Count > 0 Then

                Else



                    cadenaempleados = ""

                    For x As Integer = 0 To Forma.dsReporte.Tables(0).Rows.Count - 1
                        sql = "select  * from empleadosC where " 'fkiIdClienteInter=-1"
                        sql &= "iIdEmpleadoC=" & Forma.dsReporte.Tables(0).Rows(x)("Id_empleado")
                        sql &= " order by cFuncionesPuesto,cNombreLargo"
                        Dim rwDatosEmpleado As DataRow() = nConsulta(sql)
                        If rwDatosEmpleado Is Nothing = False Then
                            Dim fila As DataRow = dsPeriodo.Tables("Tabla").NewRow
                            empleadodetectado = rwDatosEmpleado(0)("cNombreLargo").ToString.ToUpper()


                            fila.Item("Consecutivo") = (x + 1).ToString
                            fila.Item("Id_empleado") = rwDatosEmpleado(0)("iIdEmpleadoC").ToString
                            fila.Item("CodigoEmpleado") = rwDatosEmpleado(0)("cCodigoEmpleado").ToString()
                            fila.Item("Nombre") = rwDatosEmpleado(0)("cNombreLargo").ToString.ToUpper()
                            fila.Item("Status") = IIf(rwDatosEmpleado(0)("iOrigen").ToString = "1", "INTERINO", "PLANTA")
                            fila.Item("RFC") = rwDatosEmpleado(0)("cRFC").ToString
                            fila.Item("CURP") = rwDatosEmpleado(0)("cCURP").ToString
                            fila.Item("Num_IMSS") = rwDatosEmpleado(0)("cIMSS").ToString

                            fila.Item("Fecha_Nac") = Date.Parse(rwDatosEmpleado(0)("dFechaNac").ToString).ToShortDateString()
                            'Dim tiempo As TimeSpan = Date.Now - Date.Parse(rwDatosEmpleados(x)("dFechaNac").ToString)
                            fila.Item("Edad") = CalcularEdad(Date.Parse(rwDatosEmpleado(0)("dFechaNac").ToString).Day, Date.Parse(rwDatosEmpleado(0)("dFechaNac").ToString).Month, Date.Parse(rwDatosEmpleado(0)("dFechaNac").ToString).Year)
                            fila.Item("Puesto") = rwDatosEmpleado(0)("cPuesto").ToString
                            fila.Item("Buque") = "ECO III"

                            fila.Item("Tipo_Infonavit") = rwDatosEmpleado(0)("cTipoFactor").ToString
                            fila.Item("Valor_Infonavit") = rwDatosEmpleado(0)("fFactor").ToString
                            If Forma.dsReporte.Tables(0).Rows(x)("CodigoPuesto") = 12 Or Forma.dsReporte.Tables(0).Rows(x)("CodigoPuesto") = 39 Then
                                fila.Item("Sueldo_Base") = Double.Parse(Forma.dsReporte.Tables(0).Rows(x)("SalarioTMM"))
                            Else
                                fila.Item("Sueldo_Base") = Double.Parse(Forma.dsReporte.Tables(0).Rows(x)("SalarioTMM")) / 2
                            End If

                            fila.Item("Salario_Diario") = rwDatosEmpleado(0)("fSueldoBase").ToString
                            fila.Item("Salario_Cotización") = rwDatosEmpleado(0)("fSueldoIntegrado").ToString
                            fila.Item("Dias_Trabajados") = Forma.dsReporte.Tables(0).Rows(x)("dias")
                            fila.Item("Tipo_Incapacidad") = TipoIncapacidad(rwDatosEmpleado(0)("iIdEmpleadoC").ToString, cboperiodo.SelectedValue)
                            fila.Item("Número_días") = NumDiasIncapacidad(rwDatosEmpleado(0)("iIdEmpleadoC").ToString, cboperiodo.SelectedValue)
                            fila.Item("Sueldo_Bruto") = ""
                            fila.Item("Tiempo_Extra_Fijo_Gravado") = ""
                            fila.Item("Tiempo_Extra_Fijo_Exento") = ""
                            fila.Item("Tiempo_Extra_Ocasional") = ""
                            fila.Item("Desc_Sem_Obligatorio") = ""
                            fila.Item("Vacaciones_proporcionales") = ""
                            fila.Item("Aguinaldo_gravado") = ""
                            fila.Item("Aguinaldo_exento") = ""
                            fila.Item("Total_Aguinaldo") = ""
                            fila.Item("Prima_vac_gravado") = ""
                            fila.Item("Prima_vac_exento") = ""
                            fila.Item("Total_Prima_vac") = ""
                            fila.Item("Total_percepciones") = ""
                            fila.Item("Total_percepciones_p/isr") = ""
                            fila.Item("Incapacidad") = ""
                            fila.Item("ISR") = ""
                            fila.Item("IMSS") = ""
                            fila.Item("Infonavit") = Forma.dsReporte.Tables(0).Rows(x)("InfonavitSA")
                            fila.Item("Infonavit_bim_anterior") = Forma.dsReporte.Tables(0).Rows(x)("InfonavitBIASA")
                            fila.Item("Ajuste_infonavit") = ""
                            fila.Item("Pension_Alimenticia") = ""
                            fila.Item("Prestamo") = Double.Parse(Forma.dsReporte.Tables(0).Rows(x)("AnticipoSA")) / 2
                            fila.Item("Fonacot") = ""
                            fila.Item("Subsidio_Generado") = ""
                            fila.Item("Subsidio_Aplicado") = ""
                            fila.Item("Operadora") = ""
                            fila.Item("Prestamo_Personal_A") = Double.Parse(Forma.dsReporte.Tables(0).Rows(x)("Anticipo")) / 2
                            fila.Item("Adeudo_Infonavit_A") = Forma.dsReporte.Tables(0).Rows(x)("InfonavitASI")
                            fila.Item("PA_A") = "" ' Forma.dsReporte.Tables(0).Rows(x)("InfonavitBIAASI")
                            fila.Item("Asimilados") = ""
                            fila.Item("Retenciones_Operadora") = ""
                            fila.Item("%_Comisión") = ""
                            fila.Item("Comisión_Operadora") = ""
                            fila.Item("Comisión_Asimilados") = ""
                            fila.Item("IMSS_CS") = ""
                            fila.Item("RCV_CS") = ""
                            fila.Item("Infonavit_CS") = ""
                            fila.Item("ISN_CS") = ""
                            fila.Item("Total_Costo_Social") = ""
                            fila.Item("Subtotal") = ""
                            fila.Item("IVA") = ""
                            fila.Item("TOTAL_DEPOSITO") = ""

                            fila.Item("fecha_inicio") = Forma.dsReporte.Tables(0).Rows(x)("Fechainicio")
                            fila.Item("fecha_fin") = Forma.dsReporte.Tables(0).Rows(x)("Fechafin")

                            dsPeriodo.Tables("Tabla").Rows.Add(fila)
                        End If




                    Next




                    dtgDatos.Columns.Clear()
                    Dim chk As New DataGridViewCheckBoxColumn()
                    dtgDatos.Columns.Add(chk)
                    chk.HeaderText = ""
                    chk.Name = "chk"
                    dtgDatos.DataSource = dsPeriodo.Tables("Tabla")

                    dtgDatos.Columns(0).Width = 30
                    dtgDatos.Columns(0).ReadOnly = True
                    dtgDatos.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                    'consecutivo
                    dtgDatos.Columns(1).Width = 60
                    dtgDatos.Columns(1).ReadOnly = True
                    dtgDatos.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'idempleado
                    dtgDatos.Columns(2).Width = 100
                    dtgDatos.Columns(2).ReadOnly = True
                    dtgDatos.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'codigo empleado
                    dtgDatos.Columns(3).Width = 100
                    dtgDatos.Columns(3).ReadOnly = True
                    dtgDatos.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Nombre
                    dtgDatos.Columns(4).Width = 250
                    dtgDatos.Columns(4).ReadOnly = True
                    'Estatus
                    dtgDatos.Columns(5).Width = 100
                    dtgDatos.Columns(5).ReadOnly = True
                    'RFC
                    dtgDatos.Columns(6).Width = 100
                    dtgDatos.Columns(6).ReadOnly = True
                    'dtgDatos.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                    'CURP
                    dtgDatos.Columns(7).Width = 150
                    dtgDatos.Columns(7).ReadOnly = True
                    'IMSS 

                    dtgDatos.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(8).ReadOnly = True
                    'Fecha_Nac
                    dtgDatos.Columns(9).Width = 150
                    dtgDatos.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(9).ReadOnly = True

                    'Edad
                    dtgDatos.Columns(10).ReadOnly = True
                    dtgDatos.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                    'Puesto
                    dtgDatos.Columns(11).ReadOnly = True
                    dtgDatos.Columns(11).Width = 200
                    dtgDatos.Columns.Remove("Puesto")

                    Dim combo As New DataGridViewComboBoxColumn

                    sql = "select * from puestos where iTipo=1 order by cNombre"

                    'Dim rwPuestos As DataRow() = nConsulta(sql)
                    'If rwPuestos Is Nothing = False Then
                    '    combo.Items.Add("uno")
                    '    combo.Items.Add("dos")
                    '    combo.Items.Add("tres")
                    'End If

                    nCargaCBO(combo, sql, "cNombre", "iIdPuesto")

                    combo.HeaderText = "Puesto"

                    combo.Width = 150
                    dtgDatos.Columns.Insert(11, combo)
                    'DirectCast(dtgDatos.Columns(11), DataGridViewComboBoxColumn).Sorted = True
                    'Dim combo2 As New DataGridViewComboBoxCell
                    'combo2 = CType(Me.dtgDatos.Rows(2).Cells(11), DataGridViewComboBoxCell)
                    'combo2.Value = combo.Items(11)



                    'dtgDatos.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                    'Buque
                    'dtgDatos.Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(12).ReadOnly = True
                    dtgDatos.Columns(12).Width = 150
                    dtgDatos.Columns.Remove("Buque")

                    Dim combo2 As New DataGridViewComboBoxColumn

                    sql = "select * from departamentos where iEstatus=1 order by cNombre"

                    'Dim rwPuestos As DataRow() = nConsulta(sql)
                    'If rwPuestos Is Nothing = False Then
                    '    combo.Items.Add("uno")
                    '    combo.Items.Add("dos")
                    '    combo.Items.Add("tres")
                    'End If

                    nCargaCBO(combo2, sql, "cNombre", "iIdDepartamento")

                    combo2.HeaderText = "Buque"
                    combo2.Width = 150
                    dtgDatos.Columns.Insert(12, combo2)

                    'Tipo_Infonavit
                    dtgDatos.Columns(13).ReadOnly = True
                    dtgDatos.Columns(13).Width = 150
                    'dtgDatos.Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight



                    'Valor_Infonavit
                    dtgDatos.Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(14).ReadOnly = True
                    dtgDatos.Columns(14).Width = 150
                    'Sueldo_Base
                    dtgDatos.Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(15).ReadOnly = True
                    dtgDatos.Columns(15).Width = 150
                    'Salario_Diario
                    dtgDatos.Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(16).ReadOnly = True
                    dtgDatos.Columns(16).Width = 150
                    'Salario_Cotización
                    dtgDatos.Columns(17).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(17).ReadOnly = True
                    dtgDatos.Columns(17).Width = 150
                    'Dias_Trabajados
                    dtgDatos.Columns(18).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(18).Width = 150
                    'Tipo_Incapacidad
                    dtgDatos.Columns(19).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(19).ReadOnly = True
                    dtgDatos.Columns(19).Width = 150
                    'Número_días
                    dtgDatos.Columns(20).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(20).ReadOnly = True
                    dtgDatos.Columns(20).Width = 150
                    'Sueldo_Bruto
                    dtgDatos.Columns(21).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(21).ReadOnly = True
                    dtgDatos.Columns(21).Width = 150
                    'Tiempo_Extra_Fijo_Gravado
                    dtgDatos.Columns(22).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(22).ReadOnly = True
                    dtgDatos.Columns(22).Width = 150

                    'Tiempo_Extra_Fijo_Exento
                    dtgDatos.Columns(23).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(23).ReadOnly = True
                    dtgDatos.Columns(23).Width = 150

                    'Tiempo_Extra_Ocasional
                    dtgDatos.Columns(24).Width = 150
                    dtgDatos.Columns(24).ReadOnly = True
                    dtgDatos.Columns(24).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Desc_Sem_Obligatorio
                    dtgDatos.Columns(25).Width = 150
                    dtgDatos.Columns(25).ReadOnly = True
                    dtgDatos.Columns(25).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Vacaciones_proporcionales
                    dtgDatos.Columns(26).Width = 150
                    dtgDatos.Columns(26).ReadOnly = True
                    dtgDatos.Columns(26).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Aguinaldo_gravado
                    dtgDatos.Columns(27).Width = 150
                    dtgDatos.Columns(27).ReadOnly = True
                    dtgDatos.Columns(27).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Aguinaldo_exento
                    dtgDatos.Columns(28).Width = 150
                    dtgDatos.Columns(28).ReadOnly = True
                    dtgDatos.Columns(28).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'Total_Aguinaldo
                    dtgDatos.Columns(29).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(29).Width = 150
                    dtgDatos.Columns(29).ReadOnly = True

                    'Prima_vac_gravado
                    dtgDatos.Columns(30).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(30).ReadOnly = True
                    dtgDatos.Columns(30).Width = 150
                    'Prima_vac_exento 
                    dtgDatos.Columns(31).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(31).ReadOnly = True
                    dtgDatos.Columns(31).Width = 150

                    'Total_Prima_vac
                    dtgDatos.Columns(32).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(32).ReadOnly = True
                    dtgDatos.Columns(32).Width = 150


                    'Total_percepciones
                    dtgDatos.Columns(33).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(33).ReadOnly = True
                    dtgDatos.Columns(33).Width = 150
                    'Total_percepciones_p/isr
                    dtgDatos.Columns(34).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(34).ReadOnly = True
                    dtgDatos.Columns(34).Width = 150

                    'Incapacidad
                    dtgDatos.Columns(35).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(35).ReadOnly = True
                    dtgDatos.Columns(35).Width = 150

                    'ISR
                    dtgDatos.Columns(36).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(36).ReadOnly = True
                    dtgDatos.Columns(36).Width = 150


                    'IMSS
                    dtgDatos.Columns(37).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(37).ReadOnly = True
                    dtgDatos.Columns(37).Width = 150

                    'Infonavit
                    dtgDatos.Columns(38).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(38).ReadOnly = True
                    dtgDatos.Columns(38).Width = 150
                    'Infonavit_bim_anterior
                    dtgDatos.Columns(39).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(39).ReadOnly = True
                    dtgDatos.Columns(39).Width = 150
                    'Ajuste_infonavit
                    dtgDatos.Columns(40).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(40).ReadOnly = True
                    dtgDatos.Columns(40).Width = 150
                    'Pension_Alimenticia
                    dtgDatos.Columns(41).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(40).ReadOnly = True
                    dtgDatos.Columns(41).Width = 150
                    'Prestamo
                    dtgDatos.Columns(42).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(42).ReadOnly = True
                    dtgDatos.Columns(42).Width = 150
                    'Fonacot
                    dtgDatos.Columns(43).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(43).ReadOnly = True
                    dtgDatos.Columns(43).Width = 150
                    'Subsidio_Generado
                    dtgDatos.Columns(44).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(44).ReadOnly = True
                    dtgDatos.Columns(44).Width = 150
                    'Subsidio_Aplicado
                    dtgDatos.Columns(45).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(45).ReadOnly = True
                    dtgDatos.Columns(45).Width = 150
                    'Operadora
                    dtgDatos.Columns(46).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(46).ReadOnly = True
                    dtgDatos.Columns(46).Width = 150

                    'Prestamo Personal Asimilado
                    dtgDatos.Columns(47).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(48).ReadOnly = True
                    dtgDatos.Columns(47).Width = 150

                    'Adeudo_Infonavit_Asimilado
                    dtgDatos.Columns(48).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(49).ReadOnly = True
                    dtgDatos.Columns(48).Width = 150

                    'Difencia infonavit Asimilado
                    dtgDatos.Columns(49).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    'dtgDatos.Columns(50).ReadOnly = True
                    dtgDatos.Columns(49).Width = 150

                    'Complemento Asimilado
                    dtgDatos.Columns(50).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(50).ReadOnly = True
                    dtgDatos.Columns(50).Width = 150

                    'Retenciones_Operadora
                    dtgDatos.Columns(51).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(51).ReadOnly = True
                    dtgDatos.Columns(51).Width = 150

                    '% Comision
                    dtgDatos.Columns(52).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(52).ReadOnly = True
                    dtgDatos.Columns(52).Width = 150

                    'Comision_Operadora
                    dtgDatos.Columns(53).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(53).ReadOnly = True
                    dtgDatos.Columns(53).Width = 150

                    'Comision asimilados
                    dtgDatos.Columns(54).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(54).ReadOnly = True
                    dtgDatos.Columns(54).Width = 150

                    'IMSS_CS
                    dtgDatos.Columns(55).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(55).ReadOnly = True
                    dtgDatos.Columns(55).Width = 150

                    'RCV_CS
                    dtgDatos.Columns(56).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(56).ReadOnly = True
                    dtgDatos.Columns(56).Width = 150

                    'Infonavit_CS
                    dtgDatos.Columns(57).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(57).ReadOnly = True
                    dtgDatos.Columns(57).Width = 150

                    'ISN_CS
                    dtgDatos.Columns(58).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(58).ReadOnly = True
                    dtgDatos.Columns(58).Width = 150

                    'Total Costo Social
                    dtgDatos.Columns(59).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(59).ReadOnly = True
                    dtgDatos.Columns(59).Width = 150

                    'Subtotal
                    dtgDatos.Columns(60).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(60).ReadOnly = True
                    dtgDatos.Columns(60).Width = 150

                    'IVA
                    dtgDatos.Columns(61).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(61).ReadOnly = True
                    dtgDatos.Columns(61).Width = 150

                    'TOTAL DEPOSITO
                    dtgDatos.Columns(62).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(62).ReadOnly = True
                    dtgDatos.Columns(62).Width = 150

                    'FECHA INICIO
                    dtgDatos.Columns(63).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(63).ReadOnly = True
                    dtgDatos.Columns(63).Width = 150

                    'FECHA FIN
                    dtgDatos.Columns(64).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dtgDatos.Columns(64).ReadOnly = True
                    dtgDatos.Columns(64).Width = 150


                    'calcular()

                    'Cambiamos index del combo en el grid

                    For x As Integer = 0 To Forma.dsReporte.Tables(0).Rows.Count - 1

                        'sql = "select * from empleadosC where iIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        'Dim rwFila As DataRow() = nConsulta(sql)


                        'buscar el nombre del puesto

                        sql = "select * from puestos where iIdPuesto=" & Forma.dsReporte.Tables(0).Rows(x)("CodigoPuesto")
                        Dim rwPuesto As DataRow() = nConsulta(sql)


                        CType(Me.dtgDatos.Rows(x).Cells(11), DataGridViewComboBoxCell).Value = rwPuesto(0)("cNombre").ToString()


                        'buscar el nombre del buque
                        sql = "select * from departamentos where iIdDepartamento=" & Forma.dsReporte.Tables(0).Rows(x)("CodigoBuque")
                        Dim rwBuque As DataRow() = nConsulta(sql)

                        CType(Me.dtgDatos.Rows(x).Cells(12), DataGridViewComboBoxCell).Value = rwBuque(0)("cNombre").ToString()

                    Next




                    MessageBox.Show("Datos cargados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)





                    'No hay datos en este período


                End If

            End If
            'Forma.gIdEmpresa = gIdEmpresa
            'Forma.gIdPeriodo = cboperiodo.SelectedValue
            'Forma.gIdTipoPuesto = 1
            'Forma.ShowDialog()
        Catch ex As Exception


            MessageBox.Show(empleadodetectado & " " & ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try
    End Sub

    Private Sub cmdexcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdexcel.Click
        Try
            Dim filaExcel As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo As String
            Dim pilotin As Boolean
            Dim rwUsuario As DataRow() = nConsulta("Select * from Usuarios where idUsuario=1")
            Dim tiponomina, sueldodescanso As String
            Dim filaexcelnomtotal As Integer = 0


            pnlProgreso.Visible = True
            pnlCatalogo.Enabled = False
            Application.DoEvents()

            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count

            If dtgDatos.Rows.Count > 0 Then


                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\TMM.xlsx"
                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
                Dim libro As New ClosedXML.Excel.XLWorkbook

                book.Worksheet(1).CopyTo(libro, "NOMINA")
                book.Worksheet(2).CopyTo(libro, "DETALLE")
                book.Worksheet(3).CopyTo(libro, "FACT")
                book.Worksheets(4).CopyTo(libro, "PENSION ALIMENTICIA")


                Dim hoja As IXLWorksheet = libro.Worksheets(0)
                Dim hoja2 As IXLWorksheet = libro.Worksheets(1)
                Dim hoja3 As IXLWorksheet = libro.Worksheets(2)
                Dim hoja4 As IXLWorksheet = libro.Worksheets(3)

                Dim fecha, iejercicio, idias, periodom As String
                Dim DiasCadaPeriodo As Integer
                Dim DiasCadaPeriodo2 As Integer
                Dim FechaInicioPeriodo As Date
                Dim FechaFinPeriodo As Date
                Dim tipoperiodos2 As String

                ' <<<<<<<<<<<<<<<<<<<<<<Noina Total>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
                If rwPeriodo0 Is Nothing = False Then
                    periodo = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " DE " & (rwPeriodo0(0).Item("iEjercicio"))
                    fecha = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
                    iejercicio = rwPeriodo0(0).Item("iEjercicio")
                    idias = rwPeriodo0(0).Item("iDiasPago")
                    periodom = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & (rwPeriodo0(0).Item("iEjercicio"))

                    FechaInicioPeriodo = Date.Parse(rwPeriodo0(0)("dFechaInicio"))

                    FechaFinPeriodo = Date.Parse(rwPeriodo0(0)("dFechaFin"))
                    DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1
                    tipoperiodos2 = rwPeriodo0(0).Item("fkiIdTipoPeriodo")

                End If




                filaExcel = 5
                ' contadorfacturas = 1

                For x As Integer = 0 To dtgDatos.Rows.Count - 1
                    'CONSULTAS
                    Dim fSindicatoExtra As Double = 0.0
                    Dim valesDespensa As String = "0.00"
                    'SINDICATO EXEDENTE TOTAL

                    sql = "select isnull( fsindicatoExtra,0) as  fsindicatoExtra from EmpleadosC where iIdEmpleadoC= " & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)

                    Dim rwDatos As DataRow() = nConsulta(sql)
                    If rwDatos Is Nothing = False Then
                        If Double.Parse(rwDatos(0)("fsindicatoExtra").ToString) > 0 Then
                            fSindicatoExtra = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")), 2)

                        End If

                    End If
                    'VALES DE DESPEMSA 
                    Dim numperiodo As Integer = cboperiodo.SelectedValue

                    If validarSiSeCalculanVales(EmpresaN, tipoperiodos2) Then
                        If tipoperiodos2 = 2 Then
                            If cboperiodo.SelectedValue Mod 2 = 0 Then
                                valesDespensa = 0
                            Else
                                'VALIDAR SI SE LE PAGA NETO
                                If dtgDatos.Rows(x).Cells(71).Value > 0 Then
                                    valesDespensa = "=ROUNDUP(IF((X" & filaExcel + x & "*9%)>=3153.70,3153.70,(X" & filaExcel + x & "*9%)),0)" 'VALES

                                End If
                                
                            End If

                        ElseIf tipoperiodos2 = 3 Then
                            If cboperiodo.SelectedValue Mod 4 = 0 Then

                                'VALIDAR SI SE LE PAGA NETO
                                If dtgDatos.Rows(x).Cells(71).Value > 0 Then
                                    valesDespensa = "=ROUNDUP(IF((X" & filaExcel + x & "*9%)>=3153.70,3153.70,(X" & filaExcel + x & "*9%)),0)" 'VALES
                                End If

                            Else
                                valesDespensa = 0
                            End If
                        Else
                            valesDespensa = 0
                        End If

                    Else
                        valesDespensa = "0.0"
                    End If

                    'Llenar EXCEL
                    hoja.Cell(filaExcel + x, 2).Value = x + 1
                    hoja.Cell(filaExcel + x, 3).Value = dtgDatos.Rows(x).Cells(2).Value 'cosec
                    hoja.Cell(filaExcel + x, 4).Value = dtgDatos.Rows(x).Cells(3).Value 'codigo empl
                    hoja.Cell(filaExcel + x, 5).Value = dtgDatos.Rows(x).Cells(4).Value 'nombre
                    hoja.Cell(filaExcel + x, 6).Value = dtgDatos.Rows(x).Cells(5).Value
                    hoja.Cell(filaExcel + x, 7).Value = dtgDatos.Rows(x).Cells(6).Value
                    hoja.Cell(filaExcel + x, 8).Value = dtgDatos.Rows(x).Cells(7).Value
                    hoja.Cell(filaExcel + x, 9).Value = "'" + dtgDatos.Rows(x).Cells(8).Value 'imss
                    hoja.Cell(filaExcel + x, 10).Value = dtgDatos.Rows(x).Cells(9).Value
                    hoja.Cell(filaExcel + x, 11).Value = dtgDatos.Rows(x).Cells(10).Value
                    hoja.Cell(filaExcel + x, 12).Value = dtgDatos.Rows(x).Cells(11).Value
                    hoja.Cell(filaExcel + x, 13).Value = dtgDatos.Rows(x).Cells(12).Value
                    hoja.Cell(filaExcel + x, 14).Value = dtgDatos.Rows(x).Cells(13).Value
                    hoja.Cell(filaExcel + x, 15).Value = dtgDatos.Rows(x).Cells(14).Value
                    hoja.Cell(filaExcel + x, 16).Value = dtgDatos.Rows(x).Cells(15).Value
                    hoja.Cell(filaExcel + x, 17).Value = dtgDatos.Rows(x).Cells(16).Value
                    hoja.Cell(filaExcel + x, 18).Value = dtgDatos.Rows(x).Cells(17).Value
                    hoja.Cell(filaExcel + x, 19).Value = dtgDatos.Rows(x).Cells(18).Value
                    hoja.Cell(filaExcel + x, 20).Value = dtgDatos.Rows(x).Cells(19).Value
                    hoja.Cell(filaExcel + x, 21).Value = dtgDatos.Rows(x).Cells(20).Value
                    hoja.Cell(filaExcel + x, 22).Value = dtgDatos.Rows(x).Cells(21).Value
                    hoja.Cell(filaExcel + x, 23).Value = dtgDatos.Rows(x).Cells(22).Value
                    hoja.Cell(filaExcel + x, 24).Value = dtgDatos.Rows(x).Cells(23).Value
                    hoja.Cell(filaExcel + x, 25).Value = dtgDatos.Rows(x).Cells(24).Value
                    hoja.Cell(filaExcel + x, 26).Value = dtgDatos.Rows(x).Cells(25).Value
                    hoja.Cell(filaExcel + x, 27).Value = dtgDatos.Rows(x).Cells(26).Value
                    hoja.Cell(filaExcel + x, 28).Value = dtgDatos.Rows(x).Cells(27).Value
                    hoja.Cell(filaExcel + x, 29).Value = dtgDatos.Rows(x).Cells(28).Value
                    hoja.Cell(filaExcel + x, 30).Value = dtgDatos.Rows(x).Cells(29).Value 'sueldo bruto
                    hoja.Cell(filaExcel + x, 31).Value = dtgDatos.Rows(x).Cells(30).Value
                    hoja.Cell(filaExcel + x, 32).Value = dtgDatos.Rows(x).Cells(31).Value
                    hoja.Cell(filaExcel + x, 33).Value = dtgDatos.Rows(x).Cells(32).Value
                    hoja.Cell(filaExcel + x, 34).Value = dtgDatos.Rows(x).Cells(33).Value
                    hoja.Cell(filaExcel + x, 35).Value = dtgDatos.Rows(x).Cells(34).Value
                    hoja.Cell(filaExcel + x, 36).Value = dtgDatos.Rows(x).Cells(35).Value
                    hoja.Cell(filaExcel + x, 37).Value = dtgDatos.Rows(x).Cells(36).Value
                    hoja.Cell(filaExcel + x, 38).Value = dtgDatos.Rows(x).Cells(37).Value

                    hoja.Cell(filaExcel + x, 39).Value = dtgDatos.Rows(x).Cells(38).Value
                    hoja.Cell(filaExcel + x, 40).Value = dtgDatos.Rows(x).Cells(39).Value
                    hoja.Cell(filaExcel + x, 41).Value = dtgDatos.Rows(x).Cells(40).Value
                    hoja.Cell(filaExcel + x, 42).Value = dtgDatos.Rows(x).Cells(41).Value
                    hoja.Cell(filaExcel + x, 43).Value = dtgDatos.Rows(x).Cells(42).Value
                    hoja.Cell(filaExcel + x, 44).Value = dtgDatos.Rows(x).Cells(43).Value
                    hoja.Cell(filaExcel + x, 45).Value = dtgDatos.Rows(x).Cells(44).Value
                    hoja.Cell(filaExcel + x, 46).Value = dtgDatos.Rows(x).Cells(45).Value
                    hoja.Cell(filaExcel + x, 47).Value = dtgDatos.Rows(x).Cells(46).Value
                    hoja.Cell(filaExcel + x, 48).Value = dtgDatos.Rows(x).Cells(47).Value
                    hoja.Cell(filaExcel + x, 49).Value = dtgDatos.Rows(x).Cells(48).Value
                    hoja.Cell(filaExcel + x, 50).Value = dtgDatos.Rows(x).Cells(49).Value
                    hoja.Cell(filaExcel + x, 51).Value = dtgDatos.Rows(x).Cells(50).Value
                    hoja.Cell(filaExcel + x, 52).Value = dtgDatos.Rows(x).Cells(51).Value
                    hoja.Cell(filaExcel + x, 53).Value = dtgDatos.Rows(x).Cells(52).Value
                    hoja.Cell(filaExcel + x, 54).Value = dtgDatos.Rows(x).Cells(53).Value 'PRIMA EXE
                    hoja.Cell(filaExcel + x, 55).FormulaA1 = "=BA" & filaExcel + x & "+BB" & filaExcel + x ' dtgDatos.Rows(x).Cells(54).Value TOTAL PRIMA
                    hoja.Cell(filaExcel + x, 56).Value = dtgDatos.Rows(x).Cells(55).Value 'TOTAL PERCEPCIONES
                    hoja.Cell(filaExcel + x, 57).Value = dtgDatos.Rows(x).Cells(56).Value 'TOTAL P/ISR
                    hoja.Cell(filaExcel + x, 58).Value = dtgDatos.Rows(x).Cells(57).Value 'INCAPACIDAD
                    hoja.Cell(filaExcel + x, 59).Value = dtgDatos.Rows(x).Cells(58).Value 'ISR
                    hoja.Cell(filaExcel + x, 60).Value = dtgDatos.Rows(x).Cells(59).Value 'IMSS
                    hoja.Cell(filaExcel + x, 61).Value = dtgDatos.Rows(x).Cells(60).Value 'INFONAVIT
                    hoja.Cell(filaExcel + x, 62).Value = dtgDatos.Rows(x).Cells(61).Value
                    hoja.Cell(filaExcel + x, 63).Value = dtgDatos.Rows(x).Cells(62).Value
                    hoja.Cell(filaExcel + x, 64).Value = dtgDatos.Rows(x).Cells(63).Value 'pension aliemtncia
                    hoja.Cell(filaExcel + x, 65).Value = dtgDatos.Rows(x).Cells(64).Value
                    hoja.Cell(filaExcel + x, 66).Value = dtgDatos.Rows(x).Cells(65).Value
                    hoja.Cell(filaExcel + x, 67).Value = dtgDatos.Rows(x).Cells(66).Value

                    hoja.Cell(filaExcel + x, 68).Value = dtgDatos.Rows(x).Cells(67).Value
                    hoja.Cell(filaExcel + x, 69).Value = dtgDatos.Rows(x).Cells(68).Value
                    hoja.Cell(filaExcel + x, 70).Value = dtgDatos.Rows(x).Cells(69).Value
                    hoja.Cell(filaExcel + x, 71).Value = dtgDatos.Rows(x).Cells(70).Value
                    hoja.Cell(filaExcel + x, 72).Value = dtgDatos.Rows(x).Cells(71).Value 'NETO SA
                    hoja.Cell(filaExcel + x, 73).Value = dtgDatos.Rows(x).Cells(72).Value
                    hoja.Cell(filaExcel + x, 74).Value = dtgDatos.Rows(x).Cells(73).Value

                   

                    'exedente
                    hoja.Cell(filaExcel + x, 75).Value = dtgDatos.Rows(x).Cells(74).Value 'EXEDENTE
                    hoja.Cell(filaExcel + x, 76).FormulaR1C1 = "=IF(X" & filaExcel + x & ">40000,""PPP"",""SIND"")" 'SIND/PPP
                    hoja.Cell(filaExcel + x, 77).Value = dtgDatos.Rows(x).Cells(75).Value
                    hoja.Cell(filaExcel + x, 78).FormulaA1 = "=CO" & filaExcel + x & "/30*T" & filaExcel + x & "*0.25"
                    hoja.Cell(filaExcel + x, 79).FormulaA1 = "=CO" & filaExcel + x & "/30/8*P" & filaExcel + x & "*2" 'Tiempo Extra Doble
                    hoja.Cell(filaExcel + x, 80).FormulaA1 = "=CO" & filaExcel + x & "/30/8*Q" & filaExcel + x & "*3" 'Tiempo Extra triple
                    hoja.Cell(filaExcel + x, 81).FormulaA1 = "=CO" & filaExcel + x & "/30*R" & filaExcel + x & "*2" 'desncaso laborado
                    hoja.Cell(filaExcel + x, 82).FormulaA1 = "=CO" & filaExcel + x & "/30*S" & filaExcel + x & "*2" 'dia festivo
                    hoja.Cell(filaExcel + x, 83).FormulaA1 = "=+BW" & filaExcel + x & "+BY" & filaExcel + x & "+BZ" & filaExcel + x & "+CA" & filaExcel + x & "+CB" & filaExcel + x & "+CC" & filaExcel + x & "+CD" & filaExcel + x

                    hoja.Cell(filaExcel + x, 84).Value = dtgDatos.Rows(x).Cells(76).Value 'por comision
                    hoja.Cell(filaExcel + x, 85).Value = dtgDatos.Rows(x).Cells(77).Value 'comision a
                    hoja.Cell(filaExcel + x, 86).Value = dtgDatos.Rows(x).Cells(78).Value 'comision b

                    hoja.Cell(filaExcel + x, 87).Value = dtgDatos.Rows(x).Cells(79).Value 'IMSS
                    hoja.Cell(filaExcel + x, 88).Value = dtgDatos.Rows(x).Cells(80).Value 'RCV
                    hoja.Cell(filaExcel + x, 89).Value = dtgDatos.Rows(x).Cells(81).Value 'INFONAVIT
                    hoja.Cell(filaExcel + x, 90).Value = dtgDatos.Rows(x).Cells(82).Value 'ISN
                    hoja.Cell(filaExcel + x, 91).FormulaA1 = "=+CI" & filaExcel + x & "+CJ" & filaExcel + x & "+CK" & filaExcel + x & "+CL" & filaExcel + x  'TOTAL COSTO SOCIAL

                    hoja.Cell(filaExcel + x, 92).FormulaA1 = valesDespensa 'VALES
                    hoja.Cell(filaExcel + x, 93).Value = fSindicatoExtra 'exedente monto
                    If NombrePeriodo = "Quincenal" Then
                        hoja.Cell(filaExcel + x, 94).FormulaA1 = "=if(BX" & filaExcel + x & "=""PPP"",((Z" & filaExcel + x & "/1.0493)*15.2)*0.03,0)"
                    Else
                        recorrerFilasColumnas(hoja, 1, filaExcel + x, 124, "clear", 94)
                        hoja.Cell(filaExcel + x, 94).Value = "NA"
                    End If

                    recorrerFilasColumnas(hoja, 1, dtgDatos.Rows.Count + 1, 124, "clear", 95)

                Next

                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 16).FormulaA1 = "=SUM(P" & filaExcel & ":P" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 17).FormulaA1 = "=SUM(Q" & filaExcel & ":Q" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 18).FormulaA1 = "=SUM(R" & filaExcel & ":R" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 19).FormulaA1 = "=SUM(S" & filaExcel & ":S" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 20).FormulaA1 = "=SUM(T" & filaExcel & ":T" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 21).FormulaA1 = "=SUM(U" & filaExcel & ":U" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 22).FormulaA1 = "=SUM(V" & filaExcel & ":V" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 23).FormulaA1 = "=SUM(W" & filaExcel & ":W" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 30).FormulaA1 = "=SUM(AD" & filaExcel & ":AD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 31).FormulaA1 = "=SUM(AE" & filaExcel & ":AE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 32).FormulaA1 = "=SUM(AF" & filaExcel & ":AF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 33).FormulaA1 = "=SUM(AG" & filaExcel & ":AG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 34).FormulaA1 = "=SUM(AH" & filaExcel & ":AH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 35).FormulaA1 = "=SUM(AI" & filaExcel & ":AI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 36).FormulaA1 = "=SUM(AJ" & filaExcel & ":AJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 37).FormulaA1 = "=SUM(AK" & filaExcel & ":AK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 38).FormulaA1 = "=SUM(AL" & filaExcel & ":AL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 39).FormulaA1 = "=SUM(AM" & filaExcel & ":AM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 40).FormulaA1 = "=SUM(AN" & filaExcel & ":AN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 41).FormulaA1 = "=SUM(AO" & filaExcel & ":AO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 42).FormulaA1 = "=SUM(AP" & filaExcel & ":AP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 43).FormulaA1 = "=SUM(AQ" & filaExcel & ":AQ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 44).FormulaA1 = "=SUM(AR" & filaExcel & ":AR" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 45).FormulaA1 = "=SUM(AS" & filaExcel & ":AS" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 46).FormulaA1 = "=SUM(AT" & filaExcel & ":AT" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 47).FormulaA1 = "=SUM(AU" & filaExcel & ":AU" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 48).FormulaA1 = "=SUM(AV" & filaExcel & ":AV" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 49).FormulaA1 = "=SUM(AW" & filaExcel & ":AW" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 50).FormulaA1 = "=SUM(AX" & filaExcel & ":AX" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 51).FormulaA1 = "=SUM(AY" & filaExcel & ":AY" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 52).FormulaA1 = "=SUM(AZ" & filaExcel & ":AZ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 53).FormulaA1 = "=SUM(BA" & filaExcel & ":BA" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 54).FormulaA1 = "=SUM(BB" & filaExcel & ":BB" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 55).FormulaA1 = "=SUM(BC" & filaExcel & ":BC" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 56).FormulaA1 = "=SUM(BD" & filaExcel & ":BD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 57).FormulaA1 = "=SUM(BE" & filaExcel & ":BE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 58).FormulaA1 = "=SUM(BF" & filaExcel & ":BF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 59).FormulaA1 = "=SUM(BG" & filaExcel & ":BG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 60).FormulaA1 = "=SUM(BH" & filaExcel & ":BH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 61).FormulaA1 = "=SUM(BI" & filaExcel & ":BI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 62).FormulaA1 = "=SUM(BJ" & filaExcel & ":BJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 63).FormulaA1 = "=SUM(BK" & filaExcel & ":BK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 64).FormulaA1 = "=SUM(BL" & filaExcel & ":BL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 65).FormulaA1 = "=SUM(BM" & filaExcel & ":BM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 66).FormulaA1 = "=SUM(BN" & filaExcel & ":BN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 67).FormulaA1 = "=SUM(BO" & filaExcel & ":BO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 68).FormulaA1 = "=SUM(BP" & filaExcel & ":BP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 69).FormulaA1 = "=SUM(BQ" & filaExcel & ":BQ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 70).FormulaA1 = "=SUM(BR" & filaExcel & ":BR" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 71).FormulaA1 = "=SUM(BS" & filaExcel & ":BS" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 72).FormulaA1 = "=SUM(BT" & filaExcel & ":BT" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 73).FormulaA1 = "=SUM(BU" & filaExcel & ":BU" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 74).FormulaA1 = "=SUM(BV" & filaExcel & ":BV" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 75).FormulaA1 = "=SUM(BW" & filaExcel & ":BW" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 76).FormulaA1 = "=SUM(BX" & filaExcel & ":BX" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 77).FormulaA1 = "=SUM(BY" & filaExcel & ":BY" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 78).FormulaA1 = "=SUM(BZ" & filaExcel & ":BZ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 79).FormulaA1 = "=SUM(CA" & filaExcel & ":CA" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 80).FormulaA1 = "=SUM(CB" & filaExcel & ":CB" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 81).FormulaA1 = "=SUM(CC" & filaExcel & ":CC" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 82).FormulaA1 = "=SUM(CD" & filaExcel & ":CD" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 83).FormulaA1 = "=SUM(CE" & filaExcel & ":CE" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 84).FormulaA1 = "=SUM(CF" & filaExcel & ":CF" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 85).FormulaA1 = "=SUM(CG" & filaExcel & ":CG" & filaExcel + dtgDatos.Rows.Count - 1 & ")"

                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 86).FormulaA1 = "=SUM(CH" & filaExcel & ":CH" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 87).FormulaA1 = "=SUM(CI" & filaExcel & ":CI" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 88).FormulaA1 = "=SUM(CJ" & filaExcel & ":CJ" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 89).FormulaA1 = "=SUM(CK" & filaExcel & ":CK" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 90).FormulaA1 = "=SUM(CL" & filaExcel & ":CL" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 91).FormulaA1 = "=SUM(CM" & filaExcel & ":CM" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 92).FormulaA1 = "=SUM(CN" & filaExcel & ":CN" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 93).FormulaA1 = "=SUM(CO" & filaExcel & ":CO" & filaExcel + dtgDatos.Rows.Count - 1 & ")"
                hoja.Cell(filaExcel + dtgDatos.Rows.Count + 1, 94).FormulaA1 = "=SUM(CP" & filaExcel & ":CP" & filaExcel + dtgDatos.Rows.Count - 1 & ")"


                hoja.Range(filaExcel + dtgDatos.Rows.Count, 5, filaExcel + dtgDatos.Rows.Count, 85).Style.Font.SetBold(True)

                filaexcelnomtotal = filaExcel + dtgDatos.Rows.Count + 1

                ''FACT PREV/ DEPOSITOS
                Dim totalf As Integer = dtgDatos.Rows.Count + 1
                Dim espace As Integer = filaExcel + totalf + 3
                Dim totalbuq As Integer = totalf + filaExcel

                hoja.Cell(espace, "E").Value = "COSTO CLIENTE"
                hoja.Range(espace, 5, espace, 6).Merge()
                hoja.Cell(espace, "E").Style.Font.Bold = True
                hoja.Range(espace, 5, espace, 6).Style.Font.FontColor = XLColor.White
                hoja.Range(espace, 5, espace, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                hoja.Range(espace, 5, espace + 9, 6).Style.Font.FontName = "Century Gothic"
                hoja.Range(espace, 5, espace + 9, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 5, espace + 9, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                hoja.Range(espace + 7, 5, espace + 9, 6).Style.Font.Bold = True
                hoja.Range(espace + 7, 5, espace + 9, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                hoja.Cell(espace + 9, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                hoja.Cell(espace + 9, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)

                hoja.Range(espace, 6, espace + 10, 6).Style.NumberFormat.Format = " #,##0.00"
                hoja.Cell(espace + 2, "E").Value = "NÓMINA SA"
                hoja.Cell(espace + 3, "E").Value = "BENEFICIOSOCIAL"
                'hoja.Cell(espace + 4, "E").Value = "PPP"
                hoja.Cell(espace + 5, "E").Value = "VALES DE DESPENSA"
                hoja.Cell(espace + 6, "E").Value = "COSTO SOCIAL"

                hoja.Cell(espace + 7, "E").Value = "Comision"
                hoja.Cell(espace + 8, "E").Value = "IVA"
                hoja.Cell(espace + 9, "E").Value = "Total"

                hoja.Cell(espace + 2, "F").FormulaA1 = "=BS" & totalbuq & "+I" & espace + 4
                hoja.Cell(espace + 3, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""SIND"",CE5:CE" & totalbuq - 2 & ")"
                ' hoja.Cell(espace + 4, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""PPP"",CE5:CE" & totalbuq - 2 & ")"
                hoja.Cell(espace + 5, "F").FormulaA1 = "=+CN" & totalbuq
                hoja.Cell(espace + 6, "F").FormulaA1 = "=+CM" & totalbuq

                hoja.Cell(espace + 7, "F").FormulaA1 = "=(F" & espace + 2 & "+F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & ")*0.06"
                hoja.Cell(espace + 8, "F").FormulaA1 = "=(F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & "+F" & espace + 7 & ")*0.16"
                hoja.Cell(espace + 9, "F").FormulaA1 = "=F" & espace + 2 & "+F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 6 & "+F" & espace + 7 & "+F" & espace + 8



                'IKE FACT
                If diasperiodo > 14 Then

                    hoja.Cell(espace + 14, "E").Value = "IKE "
                    hoja.Cell(espace + 14, "E").Style.Font.Bold = True
                    hoja.Range(espace + 14, 5, espace + 14, 6).Merge()
                    hoja.Range(espace + 14, 5, espace + 14, 6).Style.Font.FontColor = XLColor.White
                    hoja.Range(espace + 14, 5, espace + 14, 6).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Font.FontName = "Century Gothic"
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                    hoja.Range(espace + 14, 5, espace + 21, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                    hoja.Range(espace + 17, 5, espace + 17, 6).Style.Font.Bold = True
                    hoja.Range(espace + 20, 5, espace + 21, 6).Style.Font.Bold = True
                    hoja.Cell(espace + 17, "E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    hoja.Range(espace + 20, 5, espace + 21, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                    hoja.Cell(espace + 17, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                    hoja.Cell(espace + 17, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)
                    hoja.Cell(espace + 21, "E").Style.Fill.BackgroundColor = XLColor.FromArgb(183, 222, 232)
                    hoja.Cell(espace + 21, "F").Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80)

                    hoja.Cell(espace + 15, "E").Value = "PPP"
                    hoja.Cell(espace + 16, "E").Value = "3% Largo Plazo (ahorro)"
                    hoja.Cell(espace + 17, "E").Value = "IKE Deposito 1"
                    hoja.Cell(espace + 18, "E").Value = "-"
                    hoja.Cell(espace + 19, "E").Value = "Comision (PPP+Ahorro) 6%"
                    hoja.Cell(espace + 20, "E").Value = "IVA"
                    hoja.Cell(espace + 21, "E").Value = "IKE Deposito 2"

                    hoja.Range(espace + 15, 6, espace + 21, 6).Style.NumberFormat.Format = " #,##0.00"

                    hoja.Cell(espace + 15, "F").FormulaA1 = "=SUMIF(BX5:BX" & totalbuq - 2 & ",""PPP"",CE5:CE" & totalbuq - 2 & ")"
                    hoja.Cell(espace + 16, "F").FormulaA1 = "=CP" & totalbuq
                    hoja.Cell(espace + 17, "F").FormulaA1 = "=F" & espace + 15 & "+F" & espace + 16
                    ' hoja.Cell(espace + 18, "F").Value = "-"
                    hoja.Cell(espace + 19, "F").FormulaA1 = "=(F" & espace + 15 & "+F" & espace + 16 & ")*0.06"
                    hoja.Cell(espace + 20, "F").FormulaA1 = "=F" & espace + 19 & "*0.16"
                    hoja.Cell(espace + 21, "F").FormulaA1 = "=F" & espace + 19 & "+F" & espace + 20


                End If

                hoja.Range(espace + 23, 5, espace + 25, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thin
                hoja.Range(espace + 23, 5, espace + 25, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                hoja.Range(espace + 23, 6, espace + 25, 6).Style.NumberFormat.Format = " #,##0.00"

                hoja.Cell(espace + 23, "E").Value = "Deposito Cuenta SA"
                hoja.Cell(espace + 24, "E").Value = "Deposito cuenta GROESSINGER"
                hoja.Cell(espace + 25, "E").Value = "Deposito IKE"
                hoja.Cell(espace + 23, "F").FormulaA1 = "=F" & espace + 2
                hoja.Cell(espace + 24, "F").FormulaA1 = "=F" & espace + 3 & "+F" & espace + 4 & "+F" & espace + 5 & "+F" & espace + 7 & "+F" & espace + 8
                hoja.Cell(espace + 25, "F").FormulaA1 = "=F" & espace + 17 & "+F" & espace + 21

                'RETENCIONES

                hoja.Range(espace, 8, espace, 9).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 240)
                hoja.Range(espace, 8, espace + 6, 9).Style.Border.InsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 8, espace + 6, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thick
                hoja.Range(espace, 8, espace + 6, 9).Style.Font.FontName = " Century Gothic"
                hoja.Range(espace, 8, espace + 6, 8).Style.Font.Bold = True
                hoja.Cell(espace, "H").Style.Font.FontColor = XLColor.White
                hoja.Range(espace, 8, espace + 6, 9).Style.NumberFormat.Format = " #,##0.00"

                hoja.Cell(espace, "H").Value = "RETENCIONES"
                hoja.Range(espace, 9, espace, 8).Merge()
                hoja.Cell(espace + 2, "H").Value = "ISR"
                hoja.Cell(espace + 3, "H").Value = "INFONAVIT"
                hoja.Cell(espace + 4, "H").Value = "PENSIÓN"
                hoja.Cell(espace + 5, "H").Value = "FONACOT"
                hoja.Cell(espace + 6, "H").Value = "TOTAL"

                hoja.Cell(espace + 2, "I").FormulaA1 = "=+BG" & totalbuq
                hoja.Cell(espace + 3, "I").FormulaA1 = "=+BI" & totalbuq & "+BJ" & totalbuq
                hoja.Cell(espace + 4, "I").FormulaA1 = "=+BL" & totalbuq
                hoja.Cell(espace + 5, "I").FormulaA1 = "=+BN" & totalbuq
                hoja.Cell(espace + 6, "I").FormulaA1 = "=+I" & espace + 2 & "+I" & espace + 3 & "+I" & espace + 4 & "+I" & espace + 5






                '<<<<<<<<<<<<<<<Detalle>>>>>>>>>>>>>>>>>>

                filaExcel = 6
                Dim filatmp As Integer = 5

                Dim cuenta, banco, clabe, nombrecompleto As String
                Dim codesanta As String = "ND"
                Dim numperiodo2 As Int16 = CInt(cboperiodo.SelectedValue) Mod 2
                Dim textoperiodo As String
                'LIMPIAR FILAS
                recorrerFilasColumnas(hoja2, filaExcel - 1, dtgDatos.Rows.Count + 60, 1, "clear")
                recorrerFilasColumnas(hoja2, filaExcel - 1, dtgDatos.Rows.Count + 60, 60, "clear", 14)

                If tipoperiodos2 = 2 Then

                    If cboperiodo.SelectedItem Mod 2 = 0 Then

                        hoja2.Cell(4, 2).Value = "PERIODO " & "2QNA " & periodom
                        textoperiodo = "PERIODO " & "2QNA " & periodom

                    Else
                        hoja2.Cell(4, 2).Value = "PERIODO " & "1QNA " & periodom
                        textoperiodo = "PERIODO " & "1QNA " & periodom
                    End If

                ElseIf tipoperiodos2 = 3 Then
                    textoperiodo = "PERIODO " & numperiodo2 & "SEM" & periodom
                    hoja2.Cell(4, 2).Value = "PERIODO " & numperiodo2 & "SEM" & periodom
                End If


                For x As Integer = 0 To dtgDatos.Rows.Count - 1

                    hoja2.Cell(filaExcel, 9).Style.NumberFormat.Format = "@"
                    hoja2.Cell(filaExcel, 8).Style.NumberFormat.Format = "@"
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetBold(False)
                    hoja2.Range(filaExcel, 11, filaExcel, 12).Style.NumberFormat.NumberFormatId = 4
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontColor(XLColor.Black)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontName("Arial")
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetFontSize(8)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Font.SetBold(False)
                    hoja2.Range(filaExcel, 2, filaExcel, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.General)
                    hoja2.Cell("K5").Value = EmpresaN

                    Dim empleado As DataRow() = nConsulta("Select * from empleadosC where cCodigoEmpleado=" & dtgDatos.Rows(x).Cells(3).Value)
                    If empleado Is Nothing = False Then
                        nombrecompleto = empleado(0).Item("cNombre") & " " & empleado(0).Item("cApellidoP") & " " & empleado(0).Item("cApellidoM")
                        cuenta = empleado(0).Item("NumCuenta")
                        clabe = empleado(0).Item("Clabe")
                        Dim bank As DataRow() = nConsulta("select * from bancos where iIdBanco =" & empleado(0).Item("fkiIdBanco"))
                        If bank Is Nothing = False Then
                            banco = bank(0).Item("cBANCO")
                            codesanta = bank(0).Item("idSantander")
                        End If
                    End If


                    hoja2.Cell(filaExcel, 3).Style.NumberFormat.Format = "@"
                    hoja2.Cell(filaExcel, 2).Value = dtgDatos.Rows(x).Cells(2).Value
                    hoja2.Cell(filaExcel, 3).Value = dtgDatos.Rows(x).Cells(3).Value 'No empleado
                    hoja2.Cell(filaExcel, 4).Value = nombrecompleto.ToUpper 'EMPLEADONOMBRE
                    hoja2.Cell(filaExcel, 5).FormulaA1 = "=+NOMINA!BX" & filatmp
                    hoja2.Cell(filaExcel, 6).FormulaA1 = "=+NOMINA!G" & filatmp
                    hoja2.Cell(filaExcel, 7).Value = banco
                    hoja2.Cell(filaExcel, 8).Value = codesanta
                    hoja2.Cell(filaExcel, 9).Value = clabe
                    hoja2.Cell(filaExcel, 10).Value = cuenta
                    hoja2.Cell(filaExcel, 11).FormulaA1 = "=+'NOMINA'!BS" & filatmp 'sa
                    hoja2.Cell(filaExcel, 12).FormulaA1 = "=+'NOMINA'!CE" & filatmp 'excedente
                    hoja2.Cell(filaExcel, 13).FormulaA1 = "=+NOMINA!CN" & filatmp 'vales

                    filaExcel = filaExcel + 1
                    filatmp = filatmp + 1

                Next x

                'Formulas
                hoja2.Range(filaExcel + 2, 8, filaExcel + 4, 12).Style.Font.SetBold(True)
                hoja2.Cell(filaExcel + 2, 11).FormulaA1 = "=SUM(K6:K" & filaExcel & ")"
                hoja2.Cell(filaExcel + 2, 12).FormulaA1 = "=SUM(L6:L" & filaExcel & ")"
                hoja2.Cell(filaExcel + 2, 13).FormulaA1 = "=SUM(M6:M" & filaExcel & ")"



                ' <<<<<<<<<FACT>>>>>>>>>>>
                

                hoja3.Cell("G2").Value = "TMM " & EmpresaN.ToUpper & " " & textoperiodo
                hoja3.Cell("H3").FormulaA1 = "=+NOMINA!F" & espace + 2
                hoja3.Cell("H4").FormulaA1 = "=+NOMINA!F" & espace + 3
                hoja3.Cell("H5").FormulaA1 = "=+NOMINA!F" & espace + 4
                hoja3.Cell("H6").FormulaA1 = "=+NOMINA!F" & espace + 5
                hoja3.Cell("H7").FormulaA1 = "=(H3+H4+H5+H6)*G7"
                hoja3.Cell("H8").Value = EmpresaN.ToUpper


                ' <<<<<<<<<PENSION ALIEMENTICIA>>>>>>>>>>>
                filaExcel = 2
                filatmp = 5

                Dim beneficiaria, porcentaje, pensionmonto As String
               

                For x As Integer = 0 To dtgDatos.Rows.Count - 1

                    If dtgDatos.Rows(x).Cells(63).Value > 0 Then

                        Dim pensionalimenticia As DataRow() = nConsulta("Select * from pensionAlimenticia where fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value)
                        If pensionalimenticia Is Nothing = False Then
                            beneficiaria = pensionalimenticia(0).Item("Nombrebeneficiario")
                            banco = buscarBanco(pensionalimenticia(0).Item("fkiIdBanco"), "Nombre")
                            clabe = pensionalimenticia(0).Item("Clabe")
                            cuenta = pensionalimenticia(0).Item("Cuenta")
                            porcentaje = pensionalimenticia(0).Item("fPorcentaje")
                        End If
                        hoja4.Cell(filaExcel, 5).Style.NumberFormat.Format = "@"
                        hoja4.Cell(filaExcel, 6).Style.NumberFormat.Format = "@"

                        hoja4.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(3).Value
                        hoja4.Cell(filaExcel, 2).Value = dtgDatos.Rows(x).Cells(4).Value
                        hoja4.Cell(filaExcel, 3).Value = beneficiaria ' "BENEFICIARIA"
                        hoja4.Cell(filaExcel, 4).Value = banco ' "BANCO"
                        hoja4.Cell(filaExcel, 5).Value = cuenta '"CUENTA"
                        hoja4.Cell(filaExcel, 6).Value = clabe '"CLABE"
                        hoja4.Cell(filaExcel, 7).Value = porcentaje ' "PORC"
                        hoja4.Cell(filaExcel, 8).FormulaA1 = "=NOMINA!BL" & filatmp ' "PENSION MONTO"
                        filaExcel = filaExcel + 1
                    End If

                    filatmp = filatmp + 1
                Next x
               
                hoja4.Cell(filaExcel + dtgDatos.Rows.Count + 1, 8).FormulaA1 = "=SUM(H2:M" & filaExcel + dtgDatos.Rows.Count - 1 & ")"

                '<<<<<CARGAR>>>>>
                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True

                '<<<<<<<<<<<<<<<guardar>>>>>>>>>>>>>>>>

                'Dim textoperiodo As String
                If NombrePeriodo = "Quincenal" Then
                    If cboperiodo.SelectedValue Mod 2 = 0 Then
                        textoperiodo = "2 QNA "
                    Else
                        textoperiodo = "1 QNA "
                    End If


                ElseIf NombrePeriodo = "Semanal" Then

                    textoperiodo = "SEMANA " & cboperiodo.SelectedValue
                End If

                dialogo.FileName = "NOMINA " & EmpresaN.ToUpper & " " & textoperiodo & " " & periodo
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

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try



    End Sub


    Function validarSiSeCalculanVales(ByVal empresa As String, ByVal tipoperiodo As String) As Boolean
        ' tipoperiodo 2 Quincenal
        ' tipoperiodo 3 Semanal
        Dim tienevales As Boolean
        Select Case empresa.ToUpper
            Case "IDN"
                If tipoperiodo = 2 Then
                    tienevales = True
                ElseIf tipoperiodo = 3 Then
                    tienevales = False
                End If
            Case "LOGISTIC"
                If tipoperiodo = 2 Then
                    tienevales = True
                ElseIf tipoperiodo = 3 Then
                    tienevales = True
                End If
            Case "ADEMSA"
                If tipoperiodo = 2 Then
                    tienevales = True
                ElseIf tipoperiodo = 3 Then
                    tienevales = False
                End If
            Case "ALMACENADORA"
                tienevales = True
            Case "TMMDC"
                tienevales = True
            Case "TMM"
                tienevales = True
            Case "TMMDC Logistic"
                tienevales = True
            Case Else
                tienevales = False
        End Select
       
        Return tienevales
    End Function

    Private Function buscarBanco(id As String, dato As String) As String
        Dim datoreturn As String
        Dim dtBanco As DataRow() = nConsulta("select * from bancos where iIdBanco =" & id)
        If dtBanco Is Nothing = False Then
            Select Case dato
                Case "Nombre"
                    datoreturn = dtBanco(0).Item("cBanco")
                Case "razon"
                    datoreturn = dtBanco(0).Item("razonsocial")
                Case "clave"
                    datoreturn = dtBanco(0).Item("clave")
                Case "iEstatus"
                    datoreturn = dtBanco(0).Item("iEstatus")
                Case "idSantander"
                    datoreturn = dtBanco(0).Item("idSantander")
            End Select
        End If
        Return datoreturn
    End Function


    Public Sub limpiarCell(ByVal hoja As IXLWorksheet, ByVal celda As Integer) ', ByVal fila As Integer, ByVal filatotal As Integer)

        For x As Integer = celda To 200

            'For y As Integer = fila + 1 To filatotal + 20
            '    hoja.Cell(x, y).Clear()

            'Next y
            hoja.Cell(1, x).Clear()
            hoja.Cell(2, x).Clear()
            hoja.Cell(3, x).Clear()
            hoja.Cell(4, x).Clear()
            hoja.Cell(5, x).Clear()
            hoja.Cell(6, x).Clear()
            hoja.Cell(7, x).Clear()
            hoja.Cell(8, x).Clear()
            hoja.Cell(9, x).Clear()
            hoja.Cell(10, x).Clear()
            hoja.Cell(11, x).Clear()
            hoja.Cell(12, x).Clear()
            hoja.Cell(13, x).Clear()
            hoja.Cell(14, x).Clear()
        Next x
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
    Function ObtenerValoresFila(ByVal fila As DataGridViewRow) As String()

        Dim Contenido(dtgDatos.ColumnCount - 1) As String

        For Ndx As Integer = 0 To Contenido.Length - 1
            If Ndx = 0 Then
                Contenido(Ndx) = "1"
            Else
                Contenido(Ndx) = fila.Cells(Ndx).Value
            End If

        Next
        Return Contenido

    End Function

    Private Sub cmdInfonavit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInfonavit.Click
        Try
            Dim filaExcel As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo As String

            If dtgDatos.Rows.Count > 0 Then


                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\msexcel.xlsx"

                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


                Dim libro As New ClosedXML.Excel.XLWorkbook

                book.Worksheet(1).CopyTo(libro, "IAS (93713)")

                Dim hoja As IXLWorksheet = libro.Worksheets(0)

                '<<<<<<<<<<<<<<<<<<<<<<IAS>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                filaExcel = 17

                Dim nombrebuque As String
                Dim inicio As Integer = 0
                Dim contadorexcelbuqueinicial As Integer = 0
                Dim contadorexcelbuquefinal As Integer = 0
                Dim total As Integer = dtgDatos.Rows.Count - 1
                'Dim filatmp As Integer = 13 - 4
                Dim fecha As String

                hoja.Cell(filaExcel + 1, 1).InsertCellsAbove(total + filaExcel + 2)

                recorrerFilasColumnas(hoja, filaExcel, filaExcel + total, 11, "clear", 2)

                For x As Integer = 0 To dtgDatos.Rows.Count - 1


                    Dim cuenta, banco, clabe As String
                    Dim nom, app, apm As String

                    If (dtgDatos.Rows(x).Cells(3).Value Is Nothing = False) Then

                        Dim rwEmpleado As DataRow() = nConsulta("SELECT * FROM empleadosC where cCodigoEmpleado=" & dtgDatos.Rows(x).Cells(3).Value)
                        If rwEmpleado Is Nothing = False Then

                            clabe = rwEmpleado(0).Item("Clabe")
                            cuenta = rwEmpleado(0).Item("NumCuenta")
                            nom = rwEmpleado(0).Item("cNombre")
                            app = rwEmpleado(0).Item("cApellidoP")
                            apm = rwEmpleado(0).Item("cApellidoM")

                            Dim rwBanco As DataRow() = nConsulta("SELECT* FROM bancos where iIdBanco=" & rwEmpleado(0).Item("fkiIdBanco"))

                            banco = rwBanco(0).Item("cBanco")
                        End If

                    End If

                    Dim asimilado As Double
                    hoja.Cell(filaExcel + x, 8).Style.NumberFormat.Format = "@"

                    asimilado = Double.Parse(dtgDatos.Rows(x).Cells(50).Value) + Double.Parse(dtgDatos.Rows(x).Cells(50).Value)


                    hoja.Cell(filaExcel + x, 2).Value = app 'AP PATERNO
                    hoja.Cell(filaExcel + x, 3).Value = apm 'AP MATERNO
                    hoja.Cell(filaExcel + x, 4).Value = nom ' NOMBRE
                    hoja.Cell(filaExcel + x, 5).Value = banco ' BANCO
                    hoja.Cell(filaExcel + x, 6).Value = IIf(cuenta = 0, "SIN CTA", cuenta) 'CUENTA
                    hoja.Cell(filaExcel + x, 7).Value = "SIN TJT" ' TARJETA
                    hoja.Cell(filaExcel + x, 8).Value = clabe ' CLABE BANARIA
                    hoja.Cell(filaExcel + x, 9).Value = asimilado 'ASIMILADOS
                    hoja.Cell(filaExcel + x, 10).Value = dtgDatos.Rows(x).Cells(7).Value 'CURP
                    hoja.Cell(filaExcel + x, 11).Value = dtgDatos.Rows(x).Cells(6).Value 'RFC
                Next x

                hoja.Cell("I" & total + filaExcel + 1).FormulaA1 = "=SUM(I17:I" & total & ")"

                'STYLE
                hoja.Range("A1", "L1").Style.Fill.BackgroundColor = XLColor.BlueGray


                Dim fechacreacion As Date = Date.Now

                dialogo.FileName = "Nomina_93713_msexcel_" & fechacreacion.ToString("ddMMyy")
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

            End If


        Catch ex As Exception

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



    Private Sub cmdComision_Click(sender As System.Object, e As System.EventArgs) Handles cmdComision.Click
        'Enviar datos a excel
        Dim SQL As String, Alter As Boolean = False

        Dim promotor As String = ""
        Dim filaExcel As Integer = 5
        Dim dialogo As New SaveFileDialog()
        Dim contadorfacturas As Integer
        Dim Operadora As Double
        Dim ISR As Double
        Dim Infonavit As Double
        Dim Pension As Double
        Dim costo As Double
        Dim comision As Double
        Dim retenciones As Double
        Dim Asimilados As Double
        Dim comisionasimilados As Double
        Dim sueldoTMM As Double
        Dim PrestamoPerSA As Double
        Dim AdeudoInfonavitA As Double
        Dim DiferenciaInfonavitA As Double
        Dim Fonacot As Double
        Dim Prestamo As Double

        Alter = True
        Try

            SQL = "select departamentos.cNombre,sum(fsalariobase) as salariobase, sum(fOperadora) as Operadora,"
            SQL &= " sum(fRetencionOperadora) as retencion, sum(fComisionOperadora) as Comoperadora,"
            SQL &= " sum(fInfonavit) as infonavit,sum(fInfonavitBanterior) as infonavitanterior, sum(fAjusteInfonavit) as ajusteinfonavit,"
            SQL &= " sum(fPensionAlimenticia) as pensionalimenticia,sum(fPrestamo) as Prestamo, sum(fFonacot) as Fonacot, sum(fIsr) as ISR,CostoSocial,"
            SQL &= " sum(fPrestamoPerA) as PrestamoPerSA, sum(fAdeudoInfonavitA) as AdeudoInfonavitA,sum(fDiferenciaInfonavitA) as DiferenciaInfonavitA"
            SQL &= " from (nomina inner join departamentos on nomina.fkiIdDepartamento=departamentos.iIdDepartamento)"
            SQL &= " inner join (select fkiIdDepartamento,(sum(fImssCS) +sum(fRcvCS)+sum(fInfonavitCS)+ sum(fInsCS)+sum((case  when fkiIdPuesto=12 then 0 else fInsCS end)) ) as CostoSocial"
            SQL &= " from nomina"
            SQL &= " where fkiIdPeriodo =" & cboperiodo.SelectedValue & " And iEstatusEmpleado =" & cboserie.SelectedIndex & " And iTiponomina = 0"
            SQL &= " group by fkiIdDepartamento"
            SQL &= " ) as CS on departamentos.iIdDepartamento=CS.fkiIdDepartamento"
            SQL &= " where fkiIdPeriodo =" & cboperiodo.SelectedValue & " And iEstatusEmpleado =" & cboserie.SelectedIndex
            SQL &= " group by departamentos.cNombre,CostoSocial"
            SQL &= " order by departamentos.cNombre"

            Dim rwFilas As DataRow() = nConsulta(SQL)

            If rwFilas.Length > 0 Then
                Dim libro As New ClosedXML.Excel.XLWorkbook
                Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")
                'Dim hoja2 As IXLWorksheet = libro.Worksheets.Add("Resumen pago")

                hoja.Column("B").Width = 20
                hoja.Column("C").Width = 15
                hoja.Column("D").Width = 15
                hoja.Column("E").Width = 15
                hoja.Column("F").Width = 15
                hoja.Column("G").Width = 15
                hoja.Column("H").Width = 15
                hoja.Column("I").Width = 15
                hoja.Column("J").Width = 15
                hoja.Column("K").Width = 15
                hoja.Column("L").Width = 15
                hoja.Column("M").Width = 15
                hoja.Column("N").Width = 15
                hoja.Column("O").Width = 15
                hoja.Column("P").Width = 15
                hoja.Column("Q").Width = 18
                hoja.Cell(1, 2).Value = "Comision Nomina"
                hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
                hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
                hoja.Cell(3, 2).Value = "PERIODO: " & cboperiodo.Text
                hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

                'hoja.Cell(3, 2).Value = ":"
                'hoja.Cell(3, 3).Value = ""

                hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
                hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
                hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
                hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
                hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
                hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

                hoja.Range(5, 2, 1000, 26).Style.NumberFormat.NumberFormatId = 4

                'Format = ("$ #,###,##0.00")
                'hoja.Cell(4, 1).Value = "Num"

                hoja.Cell(4, 2).Value = "Barco"
                hoja.Cell(4, 3).Value = "Dispersión"
                hoja.Cell(4, 4).Value = "Costo Social"
                hoja.Cell(4, 5).Value = "Retenciones"
                hoja.Cell(4, 6).Value = "Comisión"
                hoja.Cell(4, 7).Value = "Subtotal Routes"
                hoja.Cell(4, 8).Value = "IVA"
                'hoja.Cell(4, 9).Value = "Retención"
                hoja.Cell(4, 9).Value = "TOTAL"
                hoja.Cell(4, 10).Value = ""

                hoja.Cell(4, 11).Value = "Dispersión"
                hoja.Cell(4, 12).Value = "Comisión"
                hoja.Cell(4, 13).Value = "Subtotal Biryusa"
                hoja.Cell(4, 14).Value = "IVA"
                hoja.Cell(4, 15).Value = "TOTAL"
                hoja.Cell(4, 18).Value = "ISR"
                hoja.Cell(4, 19).Value = "INFONAVIT"
                hoja.Cell(4, 20).Value = "PESION"
                hoja.Cell(4, 21).Value = "FONACOT"
                hoja.Cell(4, 22).Value = "PRESTAMO"

                filaExcel = 5
                contadorfacturas = 1

                For x As Integer = 0 To rwFilas.Length - 1

                    Operadora = Double.Parse(rwFilas(x)("Operadora"))
                    ISR = Double.Parse(rwFilas(x)("ISR"))
                    Infonavit = Double.Parse(rwFilas(x)("Infonavit")) + Double.Parse(rwFilas(x)("infonavitanterior")) + Double.Parse(rwFilas(x)("ajusteinfonavit"))
                    Pension = Double.Parse(rwFilas(x)("pensionalimenticia"))
                    Fonacot = Double.Parse(rwFilas(x)("Fonacot"))
                    costo = Double.Parse(rwFilas(x)("CostoSocial"))
                    'comision = Double.Parse(rwFilas(x)("Comoperadora"))
                    Prestamo = Double.Parse(rwFilas(x)("Prestamo"))
                    retenciones = ISR + Infonavit + Pension + Fonacot + Prestamo


                    PrestamoPerSA = Double.Parse(rwFilas(x)("PrestamoPerSA"))
                    AdeudoInfonavitA = Double.Parse(rwFilas(x)("AdeudoInfonavitA"))
                    DiferenciaInfonavitA = Double.Parse(rwFilas(x)("DiferenciaInfonavitA"))



                    sueldoTMM = Double.Parse(rwFilas(x)("salariobase"))
                    Asimilados = sueldoTMM - Infonavit - Pension - Fonacot - Operadora - Prestamo  ' PrestamoPerSA - AdeudoInfonavitA - DiferenciaInfonavitA
                    comisionasimilados = Asimilados * 0.08






                    'Barco
                    hoja.Cell(filaExcel + x, 2).Value = rwFilas(x)("cNombre")
                    'Dispersion
                    hoja.Cell(filaExcel + x, 3).Value = Operadora

                    'Costo
                    hoja.Cell(filaExcel + x, 4).Value = costo
                    'Retenciones
                    hoja.Cell(filaExcel + x, 5).Value = retenciones
                    'Comision
                    hoja.Cell(filaExcel + x, 6).Value = (Operadora + retenciones) * 0.04

                    'Subtotal
                    hoja.Cell(filaExcel + x, 7).FormulaA1 = "=SUM(C" & filaExcel + x & ":F" & filaExcel + x & ")"
                    'IVA
                    hoja.Cell(filaExcel + x, 8).FormulaA1 = "=(G" & filaExcel + x & "*0.16)"
                    'Retencion
                    'hoja.Cell(filaExcel + x, 9).FormulaA1 = "=(G" & filaExcel + x & "*0.06)"
                    'TOTAL
                    hoja.Cell(filaExcel + x, 9).FormulaA1 = "=G" & filaExcel + x & "+H" & filaExcel + x
                    'nada
                    hoja.Cell(filaExcel + x, 10).Value = ""
                    'Dispersion Asimilados
                    hoja.Cell(filaExcel + x, 11).Value = Asimilados
                    'Comision
                    hoja.Cell(filaExcel + x, 12).Value = comisionasimilados
                    'Subtotal
                    hoja.Cell(filaExcel + x, 13).FormulaA1 = "=SUM(K" & filaExcel + x & ":L" & filaExcel + x & ")"
                    'IVA
                    hoja.Cell(filaExcel + x, 14).FormulaA1 = "=(M" & filaExcel + x & "*0.16)"
                    'TOTAL
                    hoja.Cell(filaExcel + x, 15).FormulaA1 = "=SUM(M" & filaExcel + x & ":N" & filaExcel + x & ")"

                    hoja.Cell(filaExcel + x, 18).Value = ISR
                    hoja.Cell(filaExcel + x, 19).Value = Infonavit
                    hoja.Cell(filaExcel + x, 20).Value = Pension
                    hoja.Cell(filaExcel + x, 21).Value = Fonacot
                    hoja.Cell(filaExcel + x, 22).Value = Prestamo


                Next


                hoja.Cell(filaExcel + rwFilas.Length, 2).FormulaA1 = "=SUM(B" & filaExcel & ":B" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 3).FormulaA1 = "=SUM(C" & filaExcel & ":C" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 4).FormulaA1 = "=SUM(D" & filaExcel & ":D" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 5).FormulaA1 = "=SUM(E" & filaExcel & ":E" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 6).FormulaA1 = "=SUM(F" & filaExcel & ":F" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 7).FormulaA1 = "=SUM(G" & filaExcel & ":G" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 8).FormulaA1 = "=SUM(H" & filaExcel & ":H" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 9).FormulaA1 = "=SUM(I" & filaExcel & ":I" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 10).FormulaA1 = "=SUM(J" & filaExcel & ":J" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 11).FormulaA1 = "=SUM(K" & filaExcel & ":K" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 12).FormulaA1 = "=SUM(L" & filaExcel & ":L" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 13).FormulaA1 = "=SUM(M" & filaExcel & ":M" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 14).FormulaA1 = "=SUM(N" & filaExcel & ":N" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 15).FormulaA1 = "=SUM(O" & filaExcel & ":O" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 16).FormulaA1 = "=(I" & filaExcel + rwFilas.Length & "+O" & filaExcel + rwFilas.Length & ")"

                hoja.Cell(filaExcel + rwFilas.Length, 18).FormulaA1 = "=SUM(R" & filaExcel & ":R" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 19).FormulaA1 = "=SUM(S" & filaExcel & ":S" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 20).FormulaA1 = "=SUM(T" & filaExcel & ":T" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 21).FormulaA1 = "=SUM(U" & filaExcel & ":U" & filaExcel + rwFilas.Length - 1 & ")"
                hoja.Cell(filaExcel + rwFilas.Length, 22).FormulaA1 = "=SUM(V" & filaExcel & ":V" & filaExcel + rwFilas.Length - 1 & ")"


                hoja.Range(filaExcel + rwFilas.Length, 2, filaExcel + dtgDatos.Rows.Count, 18).Style.Font.SetBold(True)


                '##### HOJA NUMERO 2 RESUMEN PAGO


                dialogo.DefaultExt = "*.xlsx"
                dialogo.FileName = "Resumen Comision"
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                dialogo.ShowDialog()
                libro.SaveAs(dialogo.FileName)
                'libro.SaveAs("c:\temp\control.xlsx")
                'libro.SaveAs(dialogo.FileName)
                'apExcel.Quit()
                libro = Nothing

                MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                MessageBox.Show("No hay datos a mostrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception

        End Try

    End Sub



    Private Sub cmdResumenInfo_Click(sender As System.Object, e As System.EventArgs) Handles cmdResumenInfo.Click
        Dim SQL As String
        Dim filaExcel As Integer = 5
        Dim contador As Integer
        Dim dialogo As New SaveFileDialog()

        Dim Forma As New frmEstatusPrestamo

        If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
            SQL = "select iBimestre,iAnio,Calculoinfonavit.fkiIdEmpleadoC,cNombreLargo,Calculoinfonavit.cTipoFactor,Calculoinfonavit.fFactor,Monto,retenido from (calculoinfonavit "
            SQL &= " inner join empleadosC on calculoinfonavit.fkiIdEmpleadoC=empleadosC.iIdEmpleadoC)"
            SQL &= " inner join (select fkiIdEmpleadoC, sum (cantidad) as retenido from (DetalleDescInfonavit "
            SQL &= " inner join empleadosC on DetalleDescInfonavit.fkiIdEmpleadoC=empleadosC.iIdEmpleadoC)"
            SQL &= " where(Numbimestre =" & Forma.gBimestre & " And anio =" & Forma.gAnio & ")"
            SQL &= " group by fkiIdEmpleadoC) as detalle on empleadosC.iIdEmpleadoC=detalle.fkiIdEmpleadoC"
            SQL &= " where(iBimestre = " & Forma.gBimestre & " And iAnio = " & Forma.gAnio & ")"
            SQL &= " order by cnombreLargo"

            Dim rwFilas As DataRow() = nConsulta(SQL)

            If rwFilas.Length > 0 Then
                Dim libro As New ClosedXML.Excel.XLWorkbook
                Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")
                'Dim hoja2 As IXLWorksheet = libro.Worksheets.Add("Resumen pago")

                hoja.Column("B").Width = 15
                hoja.Column("C").Width = 15
                hoja.Column("D").Width = 40
                hoja.Column("E").Width = 15
                hoja.Column("F").Width = 15
                hoja.Column("G").Width = 15
                hoja.Column("H").Width = 15



                hoja.Cell(1, 2).Value = "Concentrado Infonavit"
                hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
                hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
                hoja.Cell(3, 2).Value = "PERIODO: " & cboperiodo.Text
                hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

                'hoja.Cell(3, 2).Value = ":"
                'hoja.Cell(3, 3).Value = ""

                hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
                hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
                hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
                hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
                hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
                hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

                hoja.Range(5, 7, 1000, 8).Style.NumberFormat.NumberFormatId = 4

                'Format = ("$ #,###,##0.00")
                'hoja.Cell(4, 1).Value = "Num"

                hoja.Cell(4, 2).Value = "Año"
                hoja.Cell(4, 3).Value = "Bimestre"
                hoja.Cell(4, 4).Value = "Nombre"
                hoja.Cell(4, 5).Value = "Tipo Factor"
                hoja.Cell(4, 6).Value = "Factor"
                hoja.Cell(4, 7).Value = "Monto Bimestre"
                hoja.Cell(4, 8).Value = "Retenido"



                filaExcel = 5
                contador = 1

                For x As Integer = 0 To rwFilas.Length - 1






                    'Año
                    hoja.Cell(filaExcel + x, 2).Value = rwFilas(x)("iAnio")
                    'bimestre
                    hoja.Cell(filaExcel + x, 3).Value = rwFilas(x)("iBimestre")
                    'nombre
                    hoja.Cell(filaExcel + x, 4).Value = rwFilas(x)("cNombreLargo")
                    'Tipo Factor
                    hoja.Cell(filaExcel + x, 5).Value = rwFilas(x)("cTipoFactor")
                    'Factor
                    hoja.Cell(filaExcel + x, 6).Value = rwFilas(x)("fFactor")
                    'Monto bimestre
                    hoja.Cell(filaExcel + x, 7).Value = rwFilas(x)("Monto")
                    'Retenido
                    hoja.Cell(filaExcel + x, 8).Value = rwFilas(x)("retenido")


                Next




                '##### HOJA NUMERO 2 RESUMEN PAGO


                dialogo.DefaultExt = "*.xlsx"
                dialogo.FileName = "Resumen Infonavit Bimestre " & Forma.gBimestre & " Año " & Forma.gAnio
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                dialogo.ShowDialog()
                libro.SaveAs(dialogo.FileName)
                'libro.SaveAs("c:\temp\control.xlsx")
                'libro.SaveAs(dialogo.FileName)
                'apExcel.Quit()
                libro = Nothing

                MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                MessageBox.Show("No hay datos a mostrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If


        End If
    End Sub

    Private Sub NoCalcularInofnavitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        Dim iFila As Integer = Me.dtgDatos.CurrentRow.Index
        '    iFila.Tag = "1"
        '    iFila.Cells(1).Style.BackColor = Color.Yellow
        'Catch ex As Exception

        'End Try

        Dim Sueldo As Double
        Dim SueldoBase As Double
        Dim ValorIncapacidad As Double
        Dim TotalPercepciones As Double
        Dim Incapacidad As Double
        Dim isr As Double
        Dim imss As Double
        Dim infonavitvalor As Double
        Dim infonavitanterior As Double
        Dim ajusteinfonavit As Double
        Dim pension As Double
        Dim prestamo As Double
        Dim fonacot As Double
        Dim subsidiogenerado As Double
        Dim subsidioaplicado As Double
        Dim RetencionOperadora As Double
        Dim InfonavitNormal As Double
        Dim PrestamoPersonalAsimilados As Double
        Dim PrestamoPersonalSA As Double
        Dim AdeudoINfonavitAsimilados As Double
        Dim DiferenciaInfonavitAsimilados As Double
        Dim PensionAlimenticia As Double
        Dim PensionAlimenticiaInsertar As Double

        Dim Operadora As Double
        Dim ComplementoAsimilados As Double

        Dim SueldoBaseTMM As Double
        Dim CostoSocialTotal As Double
        Dim ComisionOperadora As Double
        Dim ComisionAsimilados As Double
        Dim subtotal As Double
        Dim iva As Double



        Dim sql As String
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Dim ValorUMA As Double
        Dim primavacacionesgravada As Double
        Dim primavacacionesexenta As Double
        Dim diastrabajados As Double
        Dim Sueldobruto As Double
        Dim TEFG As Double
        Dim TEFE As Double
        Dim TEO As Double
        Dim DSO As Double
        Dim VACAPRO As Double
        Dim numbimestre As Integer
        Dim NOCALCULAR As Boolean
        Dim consecutivo1 As String
        Dim plantaoNO As String

        Dim PensionAntesVariable As Double

        Try
            'verificamos que tenga dias a calcular
            'For x As Integer = 0 To dtgDatos.Rows.Count - 1
            '    If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value)) <= 0 Then
            '        MessageBox.Show("Existen trabajadores que no tiene dias trabajados, favor de verificar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'Next



            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If


            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count




            For x As Integer = iFila To iFila
                NOCALCULAR = True

                If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
                    consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text))

                Else
                    consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
                    plantaoNO = dtgDatos.Rows(x).Cells(5).Value
                End If
                'verificar

                'verificamos los sueldos
                'sql = "Select salariod,sbc,salariodTopado,sbcTopado from costosocial inner join puestos on costosocial.fkiIdPuesto=puestos.iIdPuesto "
                'sql &= " where cNombre = '" & dtgDatos.Rows(x).Cells(11).FormattedValue & "' and anio=" & aniocostosocial

                'Dim rwDatosSalario As DataRow() = nConsulta(sql)

                'If rwDatosSalario Is Nothing = False Then
                '    If dtgDatos.Rows(x).Cells(10).Value >= 55 Then
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariodTopado")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbcTopado")
                '    Else
                '        dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariod")
                '        dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbc")
                '    End If

                'Else
                '    MessageBox.Show("No se encontraron datos")
                'End If


                'validar que si ese desactivado el calculo y activa el trabajador
                If chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "1" Then
                    'activo para borrar los datos de esse trabajador y calcularlo despues
                    If 0 = 0 Then
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        'sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1


                        '' borrar el seguro si solo tiene un registro
                        'For x As Integer = 0 To dtgDatos.Rows.Count - 1
                        '    Dim ValorInfo As Double
                        '    ValorInfo = IIf(dtgDatos.Rows(x).Cells(14).Value = "", "0", dtgDatos.Rows(x).Cells(14).Value)
                        '    If ValorInfo > 0 Then
                        '        Dim numbimestre As Integer

                        '        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
                        '            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
                        '        Else
                        '            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
                        '        End If

                        '        sql = "select * from DetalleDescInfonavit inner join nomina on DetalleDescInfonavit.fkiIdEmpleado"
                        '        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue & "or fkiIdPeriodo = "
                        '        sql &= " and fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value

                        '    End If
                        'Next

                    Else
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"
                        sql &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql &= " and iConsecutivo=" & consecutivo1

                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and iTipo=0"
                        sql2 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql2 &= " and iConsecutivo=" & consecutivo1

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and iTipoNomina=0"
                        sql3 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql3 &= " and iConsecutivo=" & consecutivo1

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and iTipoNomina=0"
                        sql4 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql4 &= " and iConsecutivo=" & consecutivo1

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and iTipoNomina=0"
                        sql5 &= " and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value
                        sql5 &= " and iConsecutivo=" & consecutivo1
                    End If


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql2) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql3) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql4) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql5) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    'si calcular

                ElseIf chkCalSoloMarcados.Checked = True And dtgDatos.Rows(x).Cells(4).Tag = "" Then
                    'No calcular
                    NOCALCULAR = False
                ElseIf chkCalSoloMarcados.Checked = False Then
                    'si calcular
                End If
                'If dtgDatos.Rows(x).Cells(2).Value = "704" Then
                '    MsgBox("aqui")
                'End If
                If NOCALCULAR Then
                    If dtgDatos.Rows(x).Cells(11).FormattedValue = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or dtgDatos.Rows(x).Cells(11).FormattedValue = "SUBALTERNO EN FORMACIÓN" Then






                        '####################################################################################################################################
                        '###################################################################################################################################
                        '##################################################################################################################################
                        '###############################################################################################################################
                        '###########################################################################################################################
                        '########################################################################################################################
                        '###################################################################################################################
                        '##########################################################################################################
                        '####################################################################################################
                        '#########################################################################################
                        '#############################################################################
                        '##################################################################

                        'Empieza el calculo normal
                    Else
                        diastrabajados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(26).Value = "", "0", dtgDatos.Rows(x).Cells(26).Value.ToString))
                        Dim SUELDOBRUTON As Double
                        Dim SEPTIMO As Double
                        Dim PRIDOMGRAVADA As Double
                        Dim PRIDOMEXENTA As Double
                        Dim TE2G As Double
                        Dim TE2E As Double
                        Dim TE3 As Double
                        Dim DESCANSOLABORADO As Double
                        Dim FESTIVOTRAB As Double
                        Dim BONOASISTENCIA As Double
                        Dim BONOPRODUCTIVIDAD As Double
                        Dim BONOPOLIVALENCIA As Double
                        Dim BONOESPECIALIDAD As Double
                        Dim BONOCALIDAD As Double
                        Dim COMPENSACION As Double
                        Dim SEMANAFONDO As Double
                        Dim INCREMENTORETENIDO As Double
                        Dim VACACIONESPRO As Double
                        Dim AGUINALDOGRA As Double
                        Dim AGUINALDOEXEN As Double
                        Dim PRIMAVACGRA As Double
                        Dim PRIMAVACEXEN As Double
                        Dim SUMAPERCEPCIONES As Double
                        Dim SUMAPERCEPCIONESPISR As Double
                        Dim FINJUSTIFICADA As Double
                        Dim PERMISOSINGOCEDESUELDO As Double
                        Dim PRIMADOMINICAL As Double
                        Dim SDEMPLEADO As Double

                        Dim DiasCadaPeriodo As Integer
                        Dim FechaInicioPeriodo As Date
                        Dim FechaFinPeriodo As Date
                        Dim FechaAntiguedad As Date
                        Dim FechaBuscar As Date
                        Dim TipoPeriodoinfoonavit As Integer

                        Dim INCAPACIDADD As Double
                        Dim ISRD As Double
                        Dim IMMSSD As Double
                        Dim INFONAVITD As Double
                        Dim INFOBIMANT As Double
                        Dim AJUSTEINFO As Double
                        Dim PENSIONAD As Double
                        Dim PRESTAMOD As Double
                        Dim FONACOTD As Double
                        Dim TNOLABORADOD As Double
                        Dim CUOTASINDICALD As Double
                        Dim SUBSIDIOG As Double
                        Dim SUBSIDIOA As Double
                        Dim SUMADEDUCCIONES As Double
                        Dim dias As Integer
                        Dim BanPeriodo As Boolean
                        If diastrabajados = -10 Then


                        Else
                            dias = 0
                            BanPeriodo = False
                            sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                            Dim rwPeriodo As DataRow() = nConsulta(sql)

                            If rwPeriodo Is Nothing = False Then
                                FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                                FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                                DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                                sql = "select *"
                                sql &= " from empleadosC"
                                sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                                Dim rwDatosBanco As DataRow() = nConsulta(sql)


                                If rwDatosBanco Is Nothing = False Then
                                    FechaAntiguedad = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    FechaBuscar = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                                    If FechaBuscar.CompareTo(FechaInicioPeriodo) > 0 And FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then
                                        'Estamos dentro del rango 
                                        'Calculamos la prima

                                        dias = (DateDiff("y", FechaBuscar, FechaFinPeriodo)) + 1

                                        BanPeriodo = True

                                    ElseIf FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then


                                        BanPeriodo = False

                                    End If
                                End If

                            End If

                            ' dtgDatos.Rows(x).Cells(3).Style.BackColor = Color.Chocolate

                            SDEMPLEADO = Double.Parse(dtgDatos.Rows(x).Cells(24).Value)
                            'dtgDatos.Rows(x).Cells(21).Value = Math.Round(Sueldo * (26.19568006 / 100), 2).ToString("###,##0.00")

                            FINJUSTIFICADA = 0
                            PERMISOSINGOCEDESUELDO = 0
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(20).Value = "", 0, dtgDatos.Rows(x).Cells(20).Value))
                                diastrabajados = 6
                                dtgDatos.Rows(x).Cells(45).Value = "-" + Math.Round(SDEMPLEADO * FINJUSTIFICADA, 2).ToString("###,##0.00")
                                'Mandar la falta a la resta
                            Else
                                dtgDatos.Rows(x).Cells(45).Value = 0.0
                            End If

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value)) > 0 Then
                                'diastrabajados = diastrabajados - 1
                                PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(21).Value = "", 0, dtgDatos.Rows(x).Cells(21).Value))
                                diastrabajados = 6
                                dtgDatos.Rows(x).Cells(46).Value = "-" + Math.Round(SDEMPLEADO * PERMISOSINGOCEDESUELDO, 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(46).Value = 0.0
                            End If
                            If BanPeriodo Then
                                diastrabajados = dias - 1
                            End If
                            'solo falta injustificada juega para el septimo dia
                            If DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Then
                                If dtgDatos.Rows(x).Cells(2).Value = "35" Then
                                    MsgBox("llego")
                                End If
                                dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * Double.Parse(dtgDatos.Rows(x).Cells(26).Value), 2).ToString("###,##0.00")
                                'dtgDatos.Rows(x).Cells(26).Value = "15"
                                dtgDatos.Rows(x).Cells(30).Value = "0.00"
                            ElseIf DiasCadaPeriodo = 6 Or DiasCadaPeriodo = 7 Then
                                'If dtgDatos.Rows(x).Cells(2).Value = "42" Then
                                'MsgBox("llego")
                                ' End If
                                If chkDias.Checked = False Then
                                    dtgDatos.Rows(x).Cells(26).Value = "7"
                                End If

                                If diastrabajados = 7 Then
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * 6, 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                    If ValorIncapacidad = 6 Then
                                        dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                    End If
                                Else
                                    'dtgDatos.Rows(x).Cells(29).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(24).Value) * (diastrabajados - FINJUSTIFICADA - PERMISOSINGOCEDESUELDO), 2).ToString("###,##0.00")
                                    dtgDatos.Rows(x).Cells(29).Value = Math.Round(SDEMPLEADO * (diastrabajados), 2).ToString("###,##0.00")
                                    If BanPeriodo Then
                                        dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                    Else
                                        If PERMISOSINGOCEDESUELDO > 0 Then

                                        End If
                                        'sacar factor de septimo dia solo en el caso de falta injustifica 
                                        If (diastrabajados - FINJUSTIFICADA) = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = SDEMPLEADO
                                        Else
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO * (0.166 * (diastrabajados - FINJUSTIFICADA)), 2).ToString("###,##0.00")
                                        End If


                                        If PERMISOSINGOCEDESUELDO = 7 Then
                                            dtgDatos.Rows(x).Cells(30).Value = Math.Round(SDEMPLEADO, 2).ToString("###,##0.00")
                                        End If
                                        ValorIncapacidad = IIf(dtgDatos.Rows(x).Cells(28).Value = "", 0, dtgDatos.Rows(x).Cells(28).Value)
                                        If ValorIncapacidad = 6 Then
                                            dtgDatos.Rows(x).Cells(30).Value = "0.00"
                                        End If

                                    End If

                                End If

                            End If


                            'Incapacidad
                            ValorIncapacidad = 0.0
                            If dtgDatos.Rows(x).Cells(27).Value <> "Ninguno" Then

                                ValorIncapacidad = dtgDatos.Rows(x).Cells(28).Value * SDEMPLEADO

                            End If
                            dtgDatos.Rows(x).Cells(57).Value = Math.Round(ValorIncapacidad, 2).ToString("###,##0.00")

                            'PrimaDominical
                            If chkPrimaDominical.Checked = False Then
                                If Double.Parse(IIf(dtgDatos.Rows(x).Cells(19).Value = "", 0, dtgDatos.Rows(x).Cells(19).Value)) > 0 Then
                                    PRIMADOMINICAL = Double.Parse(dtgDatos.Rows(x).Cells(19).Value) * SDEMPLEADO * 0.25
                                    If PRIMADOMINICAL > ValorUMA Then
                                        dtgDatos.Rows(x).Cells(31).Value = Math.Round(ValorUMA, 2).ToString("###,##0.00")
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(PRIMADOMINICAL - ValorUMA, 2).ToString("###,##0.00")
                                    Else
                                        dtgDatos.Rows(x).Cells(31).Value = "0.00"
                                        dtgDatos.Rows(x).Cells(32).Value = Math.Round(PRIMADOMINICAL, 2).ToString("###,##0.00")
                                    End If
                                End If
                            End If


                            'Tiempo Extra Doble
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(15).Value = "", 0, dtgDatos.Rows(x).Cells(15).Value)) > 0 Then


                                dtgDatos.Rows(x).Cells(33).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADO / 8) * 2) / 2, 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(34).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(15).Value) * (SDEMPLEADO / 8) * 2) / 2, 2).ToString("###,##0.00")

                            Else
                                dtgDatos.Rows(x).Cells(33).Value = "0.00"
                                dtgDatos.Rows(x).Cells(34).Value = "0.00"
                            End If

                            'Tiempo Extra triple
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(16).Value = "", 0, dtgDatos.Rows(x).Cells(16).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(35).Value = Math.Round((Double.Parse(dtgDatos.Rows(x).Cells(16).Value) * (SDEMPLEADO / 8) * 3), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(35).Value = "0.00"
                            End If

                            'Descanso Laborado

                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(17).Value = "", 0, dtgDatos.Rows(x).Cells(17).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(36).Value = Math.Round(SDEMPLEADO * 2 * Double.Parse(dtgDatos.Rows(x).Cells(17).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(36).Value = "0.00"
                            End If
                            'Dia Festivo laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", 0, dtgDatos.Rows(x).Cells(18).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(37).Value = Math.Round(SDEMPLEADO * 2 * Double.Parse(dtgDatos.Rows(x).Cells(18).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(37).Value = "0.00"
                            End If

                            'Tiempo No laborado
                            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                                dtgDatos.Rows(x).Cells(66).Value = Math.Round(SDEMPLEADO / 8 * Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(66).Value = "0.00"
                            End If

                            'Calcular la prima
                            If chkPrimaVacacional.Checked = False Then
                                If DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 50, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 50)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)

                                ElseIf DiasCadaPeriodo = 6 Or DiasCadaPeriodo = 7 Then
                                    dtgDatos.Rows(x).Cells(52).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 1, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(53).Value = Math.Round(Double.Parse(CalculoPrimaSA(dtgDatos.Rows(x).Cells(2).Value, 1, 25, 2, SDEMPLEADO, ValorUMA)), 2)
                                    dtgDatos.Rows(x).Cells(54).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(52).Value) + Double.Parse(dtgDatos.Rows(x).Cells(53).Value), 2)
                                    dtgDatos.Rows(x).Cells(75).Value = IIf(Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2) > 0.03, Math.Round(Double.Parse(CalculoPrimaExcedente(dtgDatos.Rows(x).Cells(2).Value, 1, 25)) - Double.Parse(dtgDatos.Rows(x).Cells(54).Value), 2), 0)
                                End If
                            End If



                            'sumar Para ISR

                            SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            SEPTIMO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(30).Value = "", 0, dtgDatos.Rows(x).Cells(30).Value))
                            PRIDOMGRAVADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(31).Value = "", 0, dtgDatos.Rows(x).Cells(31).Value))
                            PRIDOMEXENTA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(32).Value = "", 0, dtgDatos.Rows(x).Cells(32).Value))
                            TE2G = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", 0, dtgDatos.Rows(x).Cells(33).Value))
                            TE2E = Double.Parse(IIf(dtgDatos.Rows(x).Cells(34).Value = "", 0, dtgDatos.Rows(x).Cells(34).Value))
                            TE3 = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", 0, dtgDatos.Rows(x).Cells(35).Value))
                            DESCANSOLABORADO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", 0, dtgDatos.Rows(x).Cells(36).Value))
                            FESTIVOTRAB = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", 0, dtgDatos.Rows(x).Cells(37).Value))
                            BONOASISTENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", 0, dtgDatos.Rows(x).Cells(38).Value))
                            BONOPRODUCTIVIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", 0, dtgDatos.Rows(x).Cells(39).Value))
                            BONOPOLIVALENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", 0, dtgDatos.Rows(x).Cells(40).Value))
                            BONOESPECIALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", 0, dtgDatos.Rows(x).Cells(41).Value))
                            BONOCALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", 0, dtgDatos.Rows(x).Cells(42).Value))
                            COMPENSACION = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", 0, dtgDatos.Rows(x).Cells(43).Value))
                            SEMANAFONDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", 0, dtgDatos.Rows(x).Cells(44).Value))
                            FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", 0, dtgDatos.Rows(x).Cells(45).Value))
                            PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(46).Value = "", 0, dtgDatos.Rows(x).Cells(46).Value))
                            INCREMENTORETENIDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", 0, dtgDatos.Rows(x).Cells(47).Value))
                            VACACIONESPRO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", 0, dtgDatos.Rows(x).Cells(48).Value))
                            AGUINALDOGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", 0, dtgDatos.Rows(x).Cells(49).Value))
                            AGUINALDOEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(50).Value = "", 0, dtgDatos.Rows(x).Cells(50).Value))
                            PRIMAVACGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(52).Value = "", 0, dtgDatos.Rows(x).Cells(52).Value))
                            PRIMAVACEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(53).Value = "", 0, dtgDatos.Rows(x).Cells(53).Value))



                            SUMAPERCEPCIONES = SUELDOBRUTON + SEPTIMO + PRIDOMGRAVADA + PRIDOMEXENTA + TE2G + TE2E + TE3 + DESCANSOLABORADO + FESTIVOTRAB
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + FINJUSTIFICADA + PERMISOSINGOCEDESUELDO + INCREMENTORETENIDO + VACACIONESPRO + AGUINALDOGRA + AGUINALDOEXEN
                            SUMAPERCEPCIONES = SUMAPERCEPCIONES + PRIMAVACGRA + PRIMAVACEXEN - ValorIncapacidad
                            dtgDatos.Rows(x).Cells(55).Value = Math.Round(SUMAPERCEPCIONES, 2).ToString("###,##0.00")
                            SUMAPERCEPCIONESPISR = SUMAPERCEPCIONES - PRIDOMEXENTA - TE2E - AGUINALDOEXEN - PRIMAVACEXEN
                            dtgDatos.Rows(x).Cells(56).Value = Math.Round(SUMAPERCEPCIONESPISR, 2).ToString("###,##0.00")
                            Dim ADICIONALES As Double = PRIDOMGRAVADA + TE2G + TE3 + DESCANSOLABORADO + FESTIVOTRAB + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                            ADICIONALES = ADICIONALES + VACACIONESPRO + AGUINALDOGRA + PRIMAVACGRA
                            'ISR
                            If DiasCadaPeriodo = 7 Then
                                TipoPeriodoinfoonavit = 3
                                dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                            ElseIf DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                                TipoPeriodoinfoonavit = 2
                                If EmpresaN = "NOSEOCUPARA" Then
                                    Dim diastra As Double = Double.Parse(dtgDatos.Rows(x).Cells(26).Value)
                                    Dim incapa As Double = Double.Parse(dtgDatos.Rows(x).Cells(28).Value)
                                    Dim falta As Double = Double.Parse(dtgDatos.Rows(x).Cells(20).Value)
                                    Dim permiso As Double = Double.Parse(dtgDatos.Rows(x).Cells(21).Value)
                                    Dim ISRT As Double = Double.Parse(isrmontodadosinsubsidio(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    Dim Subsidioaparte As Double = Double.Parse(subsidiocalculomensual(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                                    'If dtgDatos.Rows(x).Cells(2).Value = "58" Then
                                    '    MsgBox("llego")

                                    'End If
                                    If Subsidioaparte > ISRT Then

                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte)).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte - ISRT)).ToString("###,##0.00")
                                        End If

                                    Else
                                        dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        If Subsidioaparte > 0 Then
                                            dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(69).Value = "0.00"
                                        End If

                                    End If


                                    If ISRT > Subsidioaparte Then
                                        ISRT = ISRT - Subsidioaparte
                                    Else
                                        ISRT = 0
                                    End If

                                    Dim ISRA As Double
                                    ISRA = 0
                                    If ADICIONALES > 0 Then
                                        ISRA = Double.Parse(isrmontodadosinsubsidio(ADICIONALES, 1, x))
                                    End If

                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(ISRT + ISRA, 2).ToString("###,##0.00")
                                Else
                                    'todos menos ademsa
                                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                                End If








                            Else
                                TipoPeriodoinfoonavit = 1
                            End If


                            'IMSS
                            dtgDatos.Rows(x).Cells(59).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 1, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")


                            ' buscamos la pension
                            PensionAntesVariable = 0
                            sql = "select * from PensionAlimenticia where fkiIdEmpleadoC=" & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value) & " and iEstatus=1"
                            Dim rwPensionAntes As DataRow() = nConsulta(sql)

                            If rwPensionAntes Is Nothing = False Then

                                TotalPercepciones = SUMAPERCEPCIONES
                                Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", "0", dtgDatos.Rows(x).Cells(57).Value))
                                isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", "0", dtgDatos.Rows(x).Cells(58).Value))
                                imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", "0", dtgDatos.Rows(x).Cells(59).Value))
                                Dim SubtotalAntesPensioVariable As Double = TotalPercepciones - Incapacidad - isr - imss

                                pension = 0
                                For y As Integer = 0 To rwPensionAntes.Length - 1

                                    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    'Insertar la pension
                                    'Insertamos los datos

                                    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    'Id Empleado
                                    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    'id Pension
                                    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    'id Periodo
                                    sql &= ",'" & cboperiodo.SelectedValue
                                    'serie
                                    sql &= "'," & cboserie.SelectedIndex
                                    'tipo
                                    sql &= ",0"
                                    'Monto
                                    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    'Estatus
                                    sql &= ",1"
                                    sql &= "," & consecutivo1






                                    If nExecute(sql) = False Then
                                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    End If

                                    'If rwPensionAntes(y)("antesDescuento") = "1" Then

                                    '    pension = pension + Math.Round(SubtotalAntesPensioVariable * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)


                                    '    'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)

                                    '    'Insertar la pension
                                    '    'Insertamos los datos

                                    '    sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
                                    '    'Id Empleado
                                    '    sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
                                    '    'id Pension
                                    '    sql &= "," & Integer.Parse(rwPensionAntes(y)("iIdPensionAlimenticia"))
                                    '    'id Periodo
                                    '    sql &= ",'" & cboperiodo.SelectedValue
                                    '    'serie
                                    '    sql &= "'," & cboserie.SelectedIndex
                                    '    'tipo
                                    '    sql &= "," & cboTipoNomina.SelectedIndex
                                    '    'Monto
                                    '    sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionAntes(y)("fPorcentaje")) / 100), 2)
                                    '    'Estatus
                                    '    sql &= ",1"
                                    '    sql &= "," & consecutivo1






                                    '    If nExecute(sql) = False Then
                                    '        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)


                                    '    End If

                                    'End If




                                Next
                                dtgDatos.Rows(x).Cells(63).Value = pension
                                PensionAntesVariable = pension
                            End If



                            'INFONAVIT
                            '##### VERIFICAR SI ESTA YA CALCULADO EL INFONAVIT DEL BIMESTRE
                            'Aqui verificamos si esta activo el calcular o no el infonavit



                            If chkNoinfonavit.Checked = False Then
                                'If dtgDatos.Rows(x).Cells(2).Value = "22" Then
                                '    MsgBox("lleggo")
                                'End If


                                If dtgDatos.Rows(x).Tag = "" Then
                                    'borramos el calculo previo del infonavit para tener siempre que generar el calculo por cualquier cambio que se requiera
                                    'este cambio va dentro de la funcion verificacalculoinfonavit

                                    If VerificarCalculoInfonavit(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)) = 2 Then
                                        Dim MontoInfonavit As Double = CalcularInfonavitMonto(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(25).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))
                                        If MontoInfonavit > 0 Then
                                            dtgDatos.Rows(x).Cells(60).Value = Math.Round(MontoInfonavit * DiasCadaPeriodo, 2).ToString("###,##0.00")
                                        Else
                                            dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                        End If
                                    Else
                                        dtgDatos.Rows(x).Cells(60).Value = "0.00"
                                    End If




                                Else

                                End If
                            End If

                            'No laborado

                            'If Double.Parse(IIf(dtgDatos.Rows(x).Cells(22).Value = "", 0, dtgDatos.Rows(x).Cells(22).Value)) > 0 Then
                            '    dtgDatos.Rows(x).Cells(36).Value = Math.Round((SDEMPLEADO / 8) - Double.Parse(dtgDatos.Rows(x).Cells(22).Value), 2).ToString("###,##0.00")
                            'End If



                            'cuota sindical
                            If dtgDatos.Rows(x).Cells(5).Value = "SINDICALIZADO" Then
                                dtgDatos.Rows(x).Cells(67).Value = Math.Round((SUELDOBRUTON + SEPTIMO) * 0.015).ToString("###,##0.00")

                                'SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                            Else
                                dtgDatos.Rows(x).Cells(67).Value = "0.00"
                            End If



                            INCAPACIDADD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", 0, dtgDatos.Rows(x).Cells(57).Value))
                            ISRD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", 0, dtgDatos.Rows(x).Cells(58).Value))
                            IMMSSD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", 0, dtgDatos.Rows(x).Cells(59).Value))
                            INFONAVITD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(60).Value = "", 0, dtgDatos.Rows(x).Cells(60).Value))
                            INFOBIMANT = Double.Parse(IIf(dtgDatos.Rows(x).Cells(61).Value = "", 0, dtgDatos.Rows(x).Cells(61).Value))
                            AJUSTEINFO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(62).Value = "", 0, dtgDatos.Rows(x).Cells(62).Value))
                            PENSIONAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(63).Value = "", 0, dtgDatos.Rows(x).Cells(63).Value))
                            PRESTAMOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(64).Value = "", 0, dtgDatos.Rows(x).Cells(64).Value))
                            FONACOTD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(65).Value = "", 0, dtgDatos.Rows(x).Cells(65).Value))
                            TNOLABORADOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(66).Value = "", 0, dtgDatos.Rows(x).Cells(66).Value))
                            CUOTASINDICALD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(67).Value = "", 0, dtgDatos.Rows(x).Cells(67).Value))
                            SUBSIDIOG = Double.Parse(IIf(dtgDatos.Rows(x).Cells(68).Value = "", 0, dtgDatos.Rows(x).Cells(68).Value))
                            SUBSIDIOA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(69).Value = "", 0, dtgDatos.Rows(x).Cells(69).Value))



                            'Verificar si tiene excedente y de que tipo
                            SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD
                            dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)



                            sql = "select isnull( fsindicatoExtra,0) as  fsindicatoExtra from EmpleadosC where iIdEmpleadoC= " & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)

                            Dim rwDatos As DataRow() = nConsulta(sql)
                            If rwDatos Is Nothing = False Then
                                If Double.Parse(rwDatos(0)("fsindicatoExtra").ToString) > 0 Then

                                    If DiasCadaPeriodo > 7 Then
                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * diastrabajados, 2)
                                    Else
                                        dtgDatos.Rows(x).Cells(74).Value = Math.Round(Double.Parse(rwDatos(0)("fsindicatoExtra")) / 30 * DiasCadaPeriodo, 2)
                                    End If

                                End If

                            End If

                            If chkDiasCS.Checked = True Then
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, diastrabajados, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            Else
                                dtgDatos.Rows(x).Cells(79).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 2, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")

                                dtgDatos.Rows(x).Cells(80).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 3, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(81).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 4, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(82).Value = Math.Round(calculoimss(dtgDatos.Rows(x).Cells(25).Value, SUMAPERCEPCIONES, 5, ValorUMA, DiasCadaPeriodo, 3), 2).ToString("###,##0.00")
                                dtgDatos.Rows(x).Cells(83).Value = Math.Round(IMMSSD + Double.Parse(dtgDatos.Rows(x).Cells(79).Value) + Double.Parse(dtgDatos.Rows(x).Cells(80).Value) + Double.Parse(dtgDatos.Rows(x).Cells(81).Value) + Double.Parse(dtgDatos.Rows(x).Cells(82).Value), 2)

                            End If



                        End If




                    End If
                    'Fin calculo renglon
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################
                    '#############################################################################################################################

                    '#############################################################################################################################




                End If

                'Dim cadena As String = dgvCombo.Text





                pgbProgreso.Value += 1
                Application.DoEvents()
            Next

            'verificar costo social

            Dim contador, Posicion1, Posicion2, Posicion3, Posicion4, Posicion5 As Integer



            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            pnlCatalogo.Enabled = True

        End Try

    End Sub

    Private Sub ActicarCalculoInfonavitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Tag = ""
            iFila.Cells(1).Style.BackColor = Color.White
        Catch ex As Exception

        End Try
    End Sub

    'Private Sub cmdInfonavitNominaSerie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInfonavitNominaSerie.Click
    '    Dim SQL As String
    '    Dim filaExcel As Integer = 5
    '    Dim contador As Integer
    '    Dim dialogo As New SaveFileDialog()

    '    Dim Forma As New frmEstatusPrestamo

    '    If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
    '        SQL = "select iBimestre,iAnio,Calculoinfonavit.fkiIdEmpleadoC,cNombreLargo,Calculoinfonavit.cTipoFactor,Calculoinfonavit.fFactor,Monto,retenido from (calculoinfonavit "
    '        SQL &= " inner join empleadosC on calculoinfonavit.fkiIdEmpleadoC=empleadosC.iIdEmpleadoC)"
    '        SQL &= " inner join (select fkiIdEmpleadoC, sum (cantidad) as retenido from (DetalleDescInfonavit "
    '        SQL &= " inner join empleadosC on DetalleDescInfonavit.fkiIdEmpleadoC=empleadosC.iIdEmpleadoC)"
    '        SQL &= " where(Numbimestre =" & Forma.gBimestre & " And anio =" & Forma.gAnio & " and fkiIdPeriodo=" & cboperiodo.SelectedValue & " and iSerie=" & cboserie.SelectedIndex & ")"
    '        SQL &= " group by fkiIdEmpleadoC) as detalle on empleadosC.iIdEmpleadoC=detalle.fkiIdEmpleadoC"
    '        SQL &= " where(iBimestre = " & Forma.gBimestre & " And iAnio = " & Forma.gAnio & ")"
    '        SQL &= " order by cnombreLargo"

    '        Dim rwFilas As DataRow() = nConsulta(SQL)

    '        If rwFilas.Length > 0 Then
    '            Dim libro As New ClosedXML.Excel.XLWorkbook
    '            Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")
    '            'Dim hoja2 As IXLWorksheet = libro.Worksheets.Add("Resumen pago")

    '            hoja.Column("B").Width = 15
    '            hoja.Column("C").Width = 15
    '            hoja.Column("D").Width = 40
    '            hoja.Column("E").Width = 15
    '            hoja.Column("F").Width = 15
    '            hoja.Column("G").Width = 15
    '            hoja.Column("H").Width = 15



    '            hoja.Cell(1, 2).Value = "Concentrado Infonavit"
    '            hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
    '            hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
    '            hoja.Cell(3, 2).Value = "PERIODO: " & cboperiodo.Text
    '            hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

    '            'hoja.Cell(3, 2).Value = ":"
    '            'hoja.Cell(3, 3).Value = ""

    '            hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
    '            hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
    '            hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
    '            hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '            hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
    '            'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
    '            hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
    '            hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

    '            hoja.Range(5, 7, 1000, 8).Style.NumberFormat.NumberFormatId = 4

    '            'Format = ("$ #,###,##0.00")
    '            'hoja.Cell(4, 1).Value = "Num"

    '            hoja.Cell(4, 2).Value = "Año"
    '            hoja.Cell(4, 3).Value = "Bimestre"
    '            hoja.Cell(4, 4).Value = "Nombre"
    '            hoja.Cell(4, 5).Value = "Tipo Factor"
    '            hoja.Cell(4, 6).Value = "Factor"
    '            hoja.Cell(4, 7).Value = "Monto Bimestre"
    '            hoja.Cell(4, 8).Value = "Retenido"



    '            filaExcel = 5
    '            contador = 1

    '            For x As Integer = 0 To rwFilas.Length - 1






    '                'Año
    '                hoja.Cell(filaExcel + x, 2).Value = rwFilas(x)("iAnio")
    '                'bimestre
    '                hoja.Cell(filaExcel + x, 3).Value = rwFilas(x)("iBimestre")
    '                'nombre
    '                hoja.Cell(filaExcel + x, 4).Value = rwFilas(x)("cNombreLargo")
    '                'Tipo Factor
    '                hoja.Cell(filaExcel + x, 5).Value = rwFilas(x)("cTipoFactor")
    '                'Factor
    '                hoja.Cell(filaExcel + x, 6).Value = rwFilas(x)("fFactor")
    '                'Monto bimestre
    '                hoja.Cell(filaExcel + x, 7).Value = rwFilas(x)("Monto")
    '                'Retenido
    '                hoja.Cell(filaExcel + x, 8).Value = rwFilas(x)("retenido")


    '            Next




    '            '##### HOJA NUMERO 2 RESUMEN PAGO


    '            dialogo.DefaultExt = "*.xlsx"
    '            dialogo.FileName = "Resumen Infonavit Bimestre " & Forma.gBimestre & " Año " & Forma.gAnio
    '            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
    '            dialogo.ShowDialog()
    '            libro.SaveAs(dialogo.FileName)
    '            'libro.SaveAs("c:\temp\control.xlsx")
    '            'libro.SaveAs(dialogo.FileName)
    '            'apExcel.Quit()
    '            libro = Nothing

    '            MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

    '        Else
    '            MessageBox.Show("No hay datos a mostrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '        End If


    '    End If
    'End Sub




    Function getsueldoordinario(ByRef tiponomina As String, ByRef trabjador As String, ByRef dias As String, ByRef tipe As String, Optional ByRef buque As String = "", Optional ByRef serie As String = "", Optional ByRef periodo As String = "", Optional ByRef puesto As String = "") As Double
        ' getsueldoordinario(tiponomina, dtgDatos.Rows(x).Cells(3).Value, dtgDatos.Rows(x).Cells(18).Value)
        Dim valor As String = 0
        Dim tiponom As String
        Dim sql As String

        If tiponomina = 1 Then
            tiponom = 0
        Else
            tiponom = 1
        End If
        sql = "select * from Nomina inner join EmpleadosC on fkiIdEmpleadoC=iIdEmpleadoC"
        sql &= " where Nomina.fkiIdEmpresa = 1 And fkiIdPeriodo = " & IIf(periodo = "", cboperiodo.SelectedValue, periodo)
        sql &= " and Nomina.iEstatus=1 and iEstatusEmpleado=" & IIf(serie = "", cboserie.SelectedIndex, serie)
        sql &= " and iTipoNomina=" & tiponom
        sql &= " and EmpleadosC.cCodigoEmpleado=" & trabjador
        sql &= " and Nomina.iDiasTrabajados=" & dias
        If puesto <> "" Then
            Dim nombrepuesto As DataRow() = nConsulta("select * FROM Puestos where cNombre like '" & puesto & "'")
            If puesto Is Nothing Then
            Else
                sql &= " and Nomina.fkiIdPuesto=" & nombrepuesto(0).Item("iIdPuesto")
            End If
        End If
        If buque = "" Then
            sql &= " order by " & "Nomina.Buque, cNombreLargo"
        Else
            sql &= " and Nomina.Buque='" & buque & "'"
            sql &= " order by " & "Nomina.Buque, cNombreLargo"
        End If


        Dim rwNominaGuardada As DataRow() = nConsulta(sql)

        'If rwNominaGuardadaFinal Is Nothing = False Then
        If rwNominaGuardada Is Nothing = False Then

            Select Case tipe
                Case "sueldoO"
                    valor = rwNominaGuardada(0)("fSalarioBase").ToString
                Case "prestamoA"
                    valor = rwNominaGuardada(0)("fPrestamoPerA").ToString
                Case "sueldoBruto"
                    valor = rwNominaGuardada(0)("fSueldoBruto").ToString
                Case "Asimilado"
                    valor = rwNominaGuardada(0)("fAsimilados").ToString
                Case "subsidio"
                    valor = rwNominaGuardada(0)("fSubsidioAplicado").ToString
                Case "fFonacot"
                    valor = rwNominaGuardada(0)("fFonacot").ToString
                Case "isr"
                    valor = rwNominaGuardada(0)("fIsr").ToString
                Case "infonavit"
                    valor = rwNominaGuardada(0)("fInfonavit").ToString
                Case "infonavitbim"
                    valor = rwNominaGuardada(0)("fInfonavitBanterior").ToString
                Case "infonavitajust"
                    valor = rwNominaGuardada(0)("fAjusteInfonavit").ToString
                Case "infonavitbimA"
                    valor = rwNominaGuardada(0)("fAdeudoInfonavitA").ToString
                Case "infonavitdifA"
                    valor = rwNominaGuardada(0)("fDiferenciaInfonavitA").ToString
                Case "pension"
                    valor = rwNominaGuardada(0)("fPensionAlimenticia").ToString
                Case "prestamo"
                    valor = rwNominaGuardada(0)("fPrestamo").ToString
                Case "fonacot"
                    valor = rwNominaGuardada(0)("fFonacot").ToString
                Case "fOperadora"
                    valor = rwNominaGuardada(0)("fOperadora").ToString
                Case "IMSSCS"
                    valor = rwNominaGuardada(0)("fImssCS").ToString
                Case "RCVCS"
                    valor = rwNominaGuardada(0)("fRcvCS").ToString
                Case "INFONAVITCS"
                    valor = rwNominaGuardada(0)("fInfonavitCS").ToString
                Case "INSCS"
                    valor = rwNominaGuardada(0)("fInsCS").ToString
            End Select


        End If

        Return CDbl(valor)
    End Function




    Private Sub NoCalcularPresAsiToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(2).Tag = "1"
            iFila.Cells(2).Style.BackColor = Color.Green
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ActivaCalculoPresAsiToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(2).Tag = ""
        iFila.Cells(2).Style.BackColor = Color.White
    End Sub

    Private Sub NoCalcularPresSAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(3).Tag = "1"
            iFila.Cells(3).Style.BackColor = Color.Blue
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ActivarCaluloPresSAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(3).Tag = ""
        iFila.Cells(3).Style.BackColor = Color.White
    End Sub


    'Private Sub cmdImssNomina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImssNomina.Click
    '    Dim filaExcel As Integer = 5
    '    Dim contador As Integer
    '    Dim dialogo As New SaveFileDialog()
    '    Dim sql As String

    '    Try
    '        Dim libro As New ClosedXML.Excel.XLWorkbook
    '        Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")

    '        sql = "EXEC getNominaCargada "
    '        sql &= cboperiodo.SelectedValue & " , "
    '        sql &= cboserie.SelectedIndex & " , "
    '        sql &= cboTipoNomina.SelectedIndex

    '        hoja.Column("B").Width = 15
    '        hoja.Column("C").Width = 40
    '        hoja.Column("D").Width = 10
    '        hoja.Column("E").Width = 15
    '        hoja.Column("F").Width = 15
    '        hoja.Column("G").Width = 30
    '        hoja.Column("H").Width = 15


    '        hoja.Cell(1, 2).Value = "Concentrado IMSS activos en nomina"
    '        hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
    '        hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
    '        hoja.Cell(3, 2).Value = "PERIODO: " & cboperiodo.Text
    '        hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

    '        hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
    '        hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
    '        hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
    '        hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '        hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
    '        'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
    '        hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
    '        hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

    '        hoja.Cell(4, 2).Value = "iIdEmpleado"
    '        hoja.Cell(4, 3).Value = "Nombre"
    '        hoja.Cell(4, 4).Value = "Clave"
    '        hoja.Cell(4, 5).Value = "Fecha"
    '        hoja.Cell(4, 6).Value = "Fecha Baja"
    '        hoja.Cell(4, 7).Value = "Acuse"
    '        hoja.Cell(4, 8).Value = "--"

    '        Dim rwNominaGuardada As DataRow() = nConsulta(sql)
    '        If rwNominaGuardada Is Nothing = False Then

    '            For x As Integer = 0 To rwNominaGuardada.Count - 1

    '                'Trae los registros activos y mas recientes
    '                sql = "EXEC getImssActivos "
    '                sql &= rwNominaGuardada(x)("fkiIdEmpleadoC").ToString

    '                Dim rwActivos As DataRow() = nConsulta(sql)
    '                If rwActivos Is Nothing = False Then
    '                    hoja.Cell(filaExcel + x, 2).Value = rwActivos(0)("fkiIdEmpleado")
    '                    hoja.Cell(filaExcel + x, 3).Value = dtgDatos.Rows(x).Cells(4).Value
    '                    hoja.Cell(filaExcel + x, 4).Value = rwActivos(0)("Clave")
    '                    hoja.Cell(filaExcel + x, 5).Value = rwActivos(0)("fecha")
    '                    hoja.Cell(filaExcel + x, 6).Value = rwActivos(0)("fechabajaimss")
    '                    hoja.Cell(filaExcel + x, 7).Value = rwActivos(0)("acuse")
    '                End If
    '            Next
    '        End If




    '        dialogo.DefaultExt = "*.xlsx"
    '        dialogo.FileName = "RESUMEN IMSS EN NOMINAS "
    '        dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
    '        dialogo.ShowDialog()
    '        libro.SaveAs(dialogo.FileName)
    '        'libro.SaveAs("c:\temp\control.xlsx")
    '        'libro.SaveAs(dialogo.FileName)
    '        'apExcel.Quit()
    '        libro = Nothing

    '        MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


    '    Catch ex As Exception
    '        MessageBox.Show(ex.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try



    'End Sub


    'Private Sub cmdConcentradoFonacot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConcentradoFonacot.Click
    '    Dim filaExcel As Integer = 5
    '    Dim contador As Integer
    '    Dim dialogo As New SaveFileDialog()
    '    Dim sql As String
    '    Dim serietmp, serie, mes, iejercicio, periodo As String
    '    Dim Forma As New frmConcentradoFonacot

    '    If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then

    '        Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & Forma.cbobimestre.SelectedValue)
    '        If rwPeriodo0 Is Nothing = False Then

    '            mes = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
    '            iejercicio = rwPeriodo0(0).Item("iEjercicio")

    '        End If
    '        rwPeriodo0 = nConsulta("Select (CONVERT(nvarchar(12),dFechaInicio,103) + ' - ' + CONVERT(nvarchar(12),dFechaFin,103)) as dFechaInicio,iIdPeriodo  from periodos where iIdPeriodo=" & Forma.cbobimestre.SelectedValue)
    '        If rwPeriodo0 Is Nothing = False Then
    '            periodo = rwPeriodo0(0).Item("dFechainicio")
    '        End If

    '        Try
    '            Dim libro As New ClosedXML.Excel.XLWorkbook
    '            Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")



    '            hoja.Column("B").Width = 15
    '            hoja.Column("C").Width = 15
    '            hoja.Column("D").Width = 40
    '            hoja.Column("E").Width = 15
    '            hoja.Column("F").Width = 15
    '            hoja.Column("G").Width = 15
    '            hoja.Column("H").Width = 15


    '            hoja.Cell(1, 2).Value = "Concentrado FONACOT mensual"
    '            hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
    '            hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
    '            hoja.Cell(3, 2).Value = "PERIODO: " & periodo
    '            hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

    '            hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
    '            hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
    '            hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
    '            hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '            hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
    '            hoja.Range(4, 6, 50, 6).Style.NumberFormat.NumberFormatId = 4
    '            hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
    '            hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

    '            hoja.Cell(4, 2).Value = "iIdEmpleado"
    '            hoja.Cell(4, 3).Value = "Codigo"
    '            hoja.Cell(4, 4).Value = "Nombre"
    '            hoja.Cell(4, 5).Value = "Nomina"
    '            hoja.Cell(4, 6).Value = "Fonacot"
    '            hoja.Cell(4, 7).Value = "Mes"
    '            hoja.Cell(4, 8).Value = "--"




    '            For y As Integer = 0 To cboserie.Items.Count

    '                sql = "EXEC getNominaCargada "
    '                sql &= Forma.cbobimestre.SelectedValue & " , "
    '                sql &= y & " , "
    '                sql &= cboTipoNomina.SelectedIndex

    '                Dim rwNominaGuardada As DataRow() = nConsulta(sql)
    '                If rwNominaGuardada Is Nothing = False Then
    '                    'Trae los registros activos y mas recientes
    '                    sql = "EXEC getFonacotMensual "
    '                    sql &= Forma.cbobimestre.SelectedValue & " , "
    '                    sql &= y & " , "
    '                    sql &= cboTipoNomina.SelectedIndex

    '                    serietmp = y
    '                    Select Case serietmp
    '                        Case 0 : serie = "A"
    '                        Case 1 : serie = "B"
    '                        Case 2 : serie = "C"
    '                        Case 3 : serie = "D"
    '                        Case 4 : serie = "E"

    '                    End Select
    '                    Dim rwActivos As DataRow() = nConsulta(sql)

    '                    If rwActivos Is Nothing = False Then
    '                        For x As Integer = 0 To rwActivos.Count - 1


    '                            hoja.Cell(filaExcel + x, 2).Value = rwActivos(x)("fkiIdEmpleadoC")
    '                            hoja.Cell(filaExcel + x, 3).Value = rwActivos(x)("cCodigoEmpleado")
    '                            hoja.Cell(filaExcel + x, 4).Value = rwActivos(x)("cNombreLargo")
    '                            hoja.Cell(filaExcel + x, 5).Value = serie
    '                            hoja.Cell(filaExcel + x, 6).Value = CDbl(rwActivos(x)("fFonacot")) + CDbl(getsueldoordinario(cboTipoNomina.SelectedIndex, rwActivos(x)("cCodigoEmpleado"), rwActivos(x)("iDiasTrabajados"), "fFonacot", "", serietmp, Forma.cbobimestre.SelectedValue))
    '                            hoja.Cell(filaExcel + x, 7).Value = mes
    '                        Next x
    '                    End If


    '                Else
    '                    'y = cboserie.Items.Count
    '                    Exit For
    '                End If
    '            Next y





    '            dialogo.DefaultExt = "*.xlsx"
    '            dialogo.FileName = "RESUMEN FONACOT " & mes & " " & iejercicio
    '            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
    '            dialogo.ShowDialog()
    '            libro.SaveAs(dialogo.FileName)
    '            'libro.SaveAs("c:\temp\control.xlsx")
    '            'libro.SaveAs(dialogo.FileName)
    '            'apExcel.Quit()
    '            libro = Nothing

    '            MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


    '        Catch ex As Exception
    '            MessageBox.Show(ex.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        End Try
    '    End If

    'End Sub


    Private Sub tsbbuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbbuscar.Click
        Try
            Dim dialogo As New SaveFileDialog()
            Dim sql As String
            Dim Forma As New frmBuscar
            Dim temp As Integer = 0
            Dim encontro As Boolean = False
            If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
                encontro = False
                For Each fila As DataGridViewRow In dtgDatos.Rows

                    fila.DefaultCellStyle.BackColor = Color.White

                    If Forma.rdbNombre.Checked = True Then
                        If fila.Cells.Item(4).Value.ToString().Contains(Forma.txtbuscar.Text.ToUpper) Then
                            fila.DefaultCellStyle.BackColor = Color.Aquamarine
                            dtgDatos.CurrentCell = dtgDatos.Rows(fila.Index + 1).Cells(0)
                            dtgDatos.SendToBack()
                            encontro = True
                            temp = temp + 1

                        End If
                    Else
                        If fila.Cells.Item(3).Value.ToString().Contains(Forma.txtbuscar.Text.ToUpper) Then
                            fila.DefaultCellStyle.BackColor = Color.Aquamarine
                            dtgDatos.CurrentCell = dtgDatos.Rows(fila.Index + 1).Cells(0)
                            dtgDatos.SendToBack()
                            encontro = True
                            temp = temp + 1

                        End If

                    End If

                Next

            End If

            If encontro = False Then
                MsgBox("No se encontro nada")
            Else
                MsgBox("Se encontrarón " & temp & " Registro")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SoloRegistroACalcularToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(4).Tag = "1"
            iFila.Cells(4).Style.BackColor = Color.Brown
            chkCalSoloMarcados.Checked = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DesactivarSoloRegistroACalcularToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(4).Tag = ""
        iFila.Cells(4).Style.BackColor = Color.White
    End Sub

    Private Sub RegistroTotalDiasToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(5).Tag = "1"
            iFila.Cells(5).Style.BackColor = Color.Purple
            'chkCalSoloMarcados.Checked = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DesactivarRegistroTotalDiasToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(5).Tag = ""
        iFila.Cells(5).Style.BackColor = Color.White
    End Sub

    Private Sub chkSoloCostoSocial_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSoloCostoSocial.CheckedChanged

    End Sub

    Private Sub EliminarDeLaBaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        'borramos el registro completamente

        Try
            Dim consecutivo1 As String
            Dim sql As String
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()

            If dtgDatos.CurrentRow Is Nothing = False Then
                Dim resultado As Integer = MessageBox.Show("¿Desea eliminar a este trabajador de la lista y base?", "Pregunta", MessageBoxButtons.YesNo)
                If resultado = DialogResult.Yes Then

                    If InStr(1, iFila.Cells(5).Value, "+", CompareMethod.Text) > 0 Then
                        consecutivo1 = iFila.Cells(5).Value.ToString.Substring(0, InStr(1, iFila.Cells(5).Value, "+", CompareMethod.Text) - 1)

                    Else
                        consecutivo1 = IIf(iFila.Cells(1).Value = "", "0", iFila.Cells(1).Value.ToString.Replace(",", ""))
                    End If

                    sql = "delete from Nomina"
                    sql &= " where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iEstatusNomina=0 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1
                    'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error nomina ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If




                    sql = "delete from DetalleDescInfonavit"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error det infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    sql = " delete from DetalleFonacot"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from PagoPrestamo"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1

                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Prestamo Asimilados. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If



                    sql = " delete from PagoPrestamoSA"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Prestamo Sa. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    sql = " delete from DetallePensionAlimenticia"
                    sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                    sql &= " and iSerie=" & cboserie.SelectedIndex
                    sql &= " and fkiIdEmpleadoC=" & iFila.Cells(2).Value
                    sql &= " and iConsecutivo=" & consecutivo1
                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error borrando Pension. Guardar ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If


                    dtgDatos.Rows.Remove(dtgDatos.CurrentRow)
                End If
            End If



        Catch ex As Exception

        End Try

    End Sub

    Private Sub CostoCeroToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(6).Tag = "1"
            iFila.Cells(6).Style.BackColor = Color.Purple
            chkCalSoloMarcados.Checked = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DesactivarCostoCeroToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(6).Tag = ""
        iFila.Cells(6).Style.BackColor = Color.White
    End Sub

    Private Sub cmdBuscarOtraNom_Click(sender As System.Object, e As System.EventArgs) Handles cmdBuscarOtraNom.Click
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Try
            Dim sql As String
            sql = "select * from Nomina where fkiIdEmpresa=1 and fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql &= " and iEstatusNomina=1 and iEstatus=1 and iEstatusEmpleado=" & cboserie.SelectedIndex
            sql &= " and iTipoNomina=0"
            'Dim sueldobase, salariodiario, salariointegrado, sueldobruto, TiempoExtraFijoGravado, TiempoExtraFijoExento As Double
            'Dim TiempoExtraOcasional, DesSemObligatorio, VacacionesProporcionales, AguinaldoGravado, AguinaldoExento As Double
            'Dim PrimaVacGravada, PrimaVacExenta, TotalPercepciones, TotalPercepcionesISR As Double
            'Dim incapacidad, ISR, IMSS, Infonavit, InfonavitAnterior, InfonavitAjuste, PensionAlimenticia As Double
            'Dim Prestamo, Fonacot, NetoaPagar, Excedente, Total, ImssCS, RCVCS, InfonavitCS, ISNCS
            'sql = "EXEC getNominaXEmpresaXPeriodo " & gIdEmpresa & "," & cboperiodo.SelectedValue & ",1"

            Dim rwNominaGuardadaFinal As DataRow() = nConsulta(sql)

            If rwNominaGuardadaFinal Is Nothing = False Then
                MessageBox.Show("La nomina ya esta marcada como final, no  se puede calcular", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If chkCalSoloMarcados.Checked = False Then
                    If 0 = 0 Then
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex


                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex


                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex




                    Else
                        sql = "delete from DetalleDescInfonavit"
                        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql &= " and iSerie=" & cboserie.SelectedIndex
                        'sql &= " and iSerie=" & cboserie.SelectedIndex
                        sql &= " and iTipoNomina=0"

                        sql2 = " delete from DetallePensionAlimenticia"
                        sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql2 &= " and iSerie=" & cboserie.SelectedIndex
                        sql2 &= " and iTipo=0"

                        sql3 = " delete from DetalleFonacot"
                        sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql3 &= " and iSerie=" & cboserie.SelectedIndex
                        sql3 &= " and iTipoNomina=0"

                        sql4 = " delete from PagoPrestamo"
                        sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql4 &= " and iSerie=" & cboserie.SelectedIndex
                        sql4 &= " and iTipoNomina=0"

                        sql5 = " delete from PagoPrestamoSA"
                        sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
                        sql5 &= " and iSerie=" & cboserie.SelectedIndex
                        sql5 &= " and iTipoNomina=0"
                    End If


                    If nExecute(sql) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql2) = False Then
                        MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql3) = False Then
                        MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql4) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                    If nExecute(sql5) = False Then
                        MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        'pnlProgreso.Visible = False
                        Exit Sub
                    End If

                End If

                calcularexcedente()
            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cmdAcumuladoOperadora_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAcumuladoOperadora.Click
        Dim Sueldo As Double
        Dim SueldoBase As Double
        Dim ValorIncapacidad As Double
        Dim TotalPercepciones As Double
        Dim Incapacidad As Double
        Dim isr As Double
        Dim imss As Double
        Dim infonavitvalor As Double
        Dim infonavitanterior As Double
        Dim ajusteinfonavit As Double
        Dim pension As Double
        Dim prestamo As Double
        Dim fonacot As Double
        Dim subsidiogenerado As Double
        Dim subsidioaplicado As Double
        Dim RetencionOperadora As Double
        Dim InfonavitNormal As Double
        Dim PrestamoPersonalAsimilados As Double
        Dim PrestamoPersonalSA As Double
        Dim AdeudoINfonavitAsimilados As Double
        Dim DiferenciaInfonavitAsimilados As Double
        Dim PensionAlimenticia As Double
        Dim PensionAlimenticiaInsertar As Double

        Dim Operadora As Double
        Dim ComplementoAsimilados As Double

        Dim SueldoBaseTMM As Double
        Dim CostoSocialTotal As Double
        Dim ComisionOperadora As Double
        Dim ComisionAsimilados As Double
        Dim subtotal As Double
        Dim iva As Double



        Dim sql As String
        Dim sql2 As String
        Dim sql3 As String
        Dim sql4 As String
        Dim sql5 As String
        Dim ValorUMA As Double
        Dim primavacacionesgravada As Double
        Dim primavacacionesexenta As Double
        Dim diastrabajados As Double
        Dim Sueldobruto As Double
        Dim TEFG As Double
        Dim TEFE As Double
        Dim TEO As Double
        Dim DSO As Double
        Dim VACAPRO As Double
        Dim numbimestre As Integer
        Dim NOCALCULAR As Boolean
        Dim consecutivo1 As String
        Dim plantaoNO As String

        Dim PensionAntesVariable As Double

        Try
            'verificamos que tenga dias a calcular
            'For x As Integer = 0 To dtgDatos.Rows.Count - 1
            '    If Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value)) <= 0 Then
            '        MessageBox.Show("Existen trabajadores que no tiene dias trabajados, favor de verificar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'Next



            sql = "select * from Salario "
            sql &= " where Anio=" & aniocostosocial
            sql &= " and iEstatus=1"
            Dim rwValorUMA As DataRow() = nConsulta(sql)
            If rwValorUMA Is Nothing = False Then
                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
            Else
                ValorUMA = 0
                MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
            End If


            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count




            For x As Integer = 0 To dtgDatos.Rows.Count - 1

                diastrabajados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(26).Value = "", "0", dtgDatos.Rows(x).Cells(26).Value))
                Dim SUELDOBRUTON As Double
                Dim SEPTIMO As Double
                Dim PRIDOMGRAVADA As Double
                Dim PRIDOMEXENTA As Double
                Dim TE2G As Double
                Dim TE2E As Double
                Dim TE3 As Double
                Dim DESCANSOLABORADO As Double
                Dim FESTIVOTRAB As Double
                Dim BONOASISTENCIA As Double
                Dim BONOPRODUCTIVIDAD As Double
                Dim BONOPOLIVALENCIA As Double
                Dim BONOESPECIALIDAD As Double
                Dim BONOCALIDAD As Double
                Dim COMPENSACION As Double
                Dim SEMANAFONDO As Double
                Dim INCREMENTORETENIDO As Double
                Dim VACACIONESPRO As Double
                Dim AGUINALDOGRA As Double
                Dim AGUINALDOEXEN As Double
                Dim PRIMAVACGRA As Double
                Dim PRIMAVACEXEN As Double
                Dim SUMAPERCEPCIONES As Double
                Dim SUMAPERCEPCIONESPISR As Double
                Dim FINJUSTIFICADA As Double
                Dim PERMISOSINGOCEDESUELDO As Double
                Dim PRIMADOMINICAL As Double
                Dim SDEMPLEADO As Double

                Dim DiasCadaPeriodo As Integer
                Dim FechaInicioPeriodo As Date
                Dim FechaFinPeriodo As Date
                Dim FechaAntiguedad As Date
                Dim FechaBuscar As Date
                Dim TipoPeriodoinfoonavit As Integer

                Dim INCAPACIDADD As Double
                Dim ISRD As Double
                Dim IMMSSD As Double
                Dim INFONAVITD As Double
                Dim INFOBIMANT As Double
                Dim AJUSTEINFO As Double
                Dim PENSIONAD As Double
                Dim PRESTAMOD As Double
                Dim FONACOTD As Double
                Dim TNOLABORADOD As Double
                Dim CUOTASINDICALD As Double
                Dim SUBSIDIOG As Double
                Dim SUBSIDIOA As Double
                Dim SUMADEDUCCIONES As Double
                Dim dias As Integer
                Dim BanPeriodo As Boolean
                If diastrabajados = 0 Then

                    MsgBox("no hay dias")
                Else
                    dias = 0
                    BanPeriodo = False
                    sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                    Dim rwPeriodo As DataRow() = nConsulta(sql)

                    If rwPeriodo Is Nothing = False Then
                        FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                        FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                        DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                        sql = "select *"
                        sql &= " from empleadosC"
                        sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                        Dim rwDatosBanco As DataRow() = nConsulta(sql)


                        If rwDatosBanco Is Nothing = False Then
                            FechaAntiguedad = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                            FechaBuscar = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                            If FechaBuscar.CompareTo(FechaInicioPeriodo) > 0 And FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then
                                'Estamos dentro del rango 
                                'Calculamos la prima

                                dias = (DateDiff("y", FechaBuscar, FechaFinPeriodo)) + 1

                                BanPeriodo = True

                            ElseIf FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then


                                BanPeriodo = False

                            End If
                        End If

                    End If

                    ValorIncapacidad = Double.Parse(dtgDatos.Rows(x).Cells(57).Value)

                    'sumar Para ISR

                    SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                    SEPTIMO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(30).Value = "", 0, dtgDatos.Rows(x).Cells(30).Value))
                    PRIDOMGRAVADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(31).Value = "", 0, dtgDatos.Rows(x).Cells(31).Value))
                    PRIDOMEXENTA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(32).Value = "", 0, dtgDatos.Rows(x).Cells(32).Value))
                    TE2G = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", 0, dtgDatos.Rows(x).Cells(33).Value))
                    TE2E = Double.Parse(IIf(dtgDatos.Rows(x).Cells(34).Value = "", 0, dtgDatos.Rows(x).Cells(34).Value))
                    TE3 = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", 0, dtgDatos.Rows(x).Cells(35).Value))
                    DESCANSOLABORADO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", 0, dtgDatos.Rows(x).Cells(36).Value))
                    FESTIVOTRAB = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", 0, dtgDatos.Rows(x).Cells(37).Value))
                    BONOASISTENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", 0, dtgDatos.Rows(x).Cells(38).Value))
                    BONOPRODUCTIVIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", 0, dtgDatos.Rows(x).Cells(39).Value))
                    BONOPOLIVALENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", 0, dtgDatos.Rows(x).Cells(40).Value))
                    BONOESPECIALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", 0, dtgDatos.Rows(x).Cells(41).Value))
                    BONOCALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", 0, dtgDatos.Rows(x).Cells(42).Value))
                    COMPENSACION = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", 0, dtgDatos.Rows(x).Cells(43).Value))
                    SEMANAFONDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", 0, dtgDatos.Rows(x).Cells(44).Value))
                    FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", 0, dtgDatos.Rows(x).Cells(45).Value))
                    PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(46).Value = "", 0, dtgDatos.Rows(x).Cells(46).Value))
                    INCREMENTORETENIDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", 0, dtgDatos.Rows(x).Cells(47).Value))
                    VACACIONESPRO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", 0, dtgDatos.Rows(x).Cells(48).Value))
                    AGUINALDOGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", 0, dtgDatos.Rows(x).Cells(49).Value))
                    AGUINALDOEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(50).Value = "", 0, dtgDatos.Rows(x).Cells(50).Value))
                    PRIMAVACGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(52).Value = "", 0, dtgDatos.Rows(x).Cells(52).Value))
                    PRIMAVACEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(53).Value = "", 0, dtgDatos.Rows(x).Cells(53).Value))



                    SUMAPERCEPCIONES = SUELDOBRUTON + SEPTIMO + PRIDOMGRAVADA + PRIDOMEXENTA + TE2G + TE2E + TE3 + DESCANSOLABORADO + FESTIVOTRAB
                    SUMAPERCEPCIONES = SUMAPERCEPCIONES + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                    SUMAPERCEPCIONES = SUMAPERCEPCIONES + FINJUSTIFICADA + PERMISOSINGOCEDESUELDO + INCREMENTORETENIDO + VACACIONESPRO + AGUINALDOGRA + AGUINALDOEXEN
                    SUMAPERCEPCIONES = SUMAPERCEPCIONES + PRIMAVACGRA + PRIMAVACEXEN - ValorIncapacidad
                    dtgDatos.Rows(x).Cells(55).Value = Math.Round(SUMAPERCEPCIONES, 2).ToString("###,##0.00")
                    SUMAPERCEPCIONESPISR = SUMAPERCEPCIONES - PRIDOMEXENTA - TE2E - AGUINALDOEXEN - PRIMAVACEXEN
                    dtgDatos.Rows(x).Cells(56).Value = Math.Round(SUMAPERCEPCIONESPISR, 2).ToString("###,##0.00")





                    INCAPACIDADD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(57).Value = "", 0, dtgDatos.Rows(x).Cells(57).Value))
                    ISRD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(58).Value = "", 0, dtgDatos.Rows(x).Cells(58).Value))
                    IMMSSD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(59).Value = "", 0, dtgDatos.Rows(x).Cells(59).Value))
                    INFONAVITD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(60).Value = "", 0, dtgDatos.Rows(x).Cells(60).Value))
                    INFOBIMANT = Double.Parse(IIf(dtgDatos.Rows(x).Cells(61).Value = "", 0, dtgDatos.Rows(x).Cells(61).Value))
                    AJUSTEINFO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(62).Value = "", 0, dtgDatos.Rows(x).Cells(62).Value))
                    PENSIONAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(63).Value = "", 0, dtgDatos.Rows(x).Cells(63).Value))
                    PRESTAMOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(64).Value = "", 0, dtgDatos.Rows(x).Cells(64).Value))
                    FONACOTD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(65).Value = "", 0, dtgDatos.Rows(x).Cells(65).Value))
                    TNOLABORADOD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(66).Value = "", 0, dtgDatos.Rows(x).Cells(66).Value))
                    CUOTASINDICALD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(67).Value = "", 0, dtgDatos.Rows(x).Cells(67).Value))
                    SUBSIDIOG = Double.Parse(IIf(dtgDatos.Rows(x).Cells(68).Value = "", 0, dtgDatos.Rows(x).Cells(68).Value))
                    SUBSIDIOA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(69).Value = "", 0, dtgDatos.Rows(x).Cells(69).Value))



                    'Verificar si tiene excedente y de que tipo
                    'SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD
                    'dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)


                    'Verificar si tiene excedente y de que tipo
                    If NombrePeriodo = "Semanal" And EmpresaN = "IDN" Then
                        SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD + IMMSSD
                        dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                    Else
                        SUMADEDUCCIONES = ISRD + INFONAVITD + INFOBIMANT + AJUSTEINFO + PENSIONAD + PRESTAMOD + FONACOTD + TNOLABORADOD + CUOTASINDICALD
                        dtgDatos.Rows(x).Cells(70).Value = Math.Round(SUMAPERCEPCIONES - SUMADEDUCCIONES, 2)
                    End If

                End If




                pgbProgreso.Value += 1
                Application.DoEvents()
            Next

            'verificar costo social

            Dim contador, Posicion1, Posicion2, Posicion3, Posicion4, Posicion5 As Integer



            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            pnlCatalogo.Enabled = True

        End Try
    End Sub

    Private Sub BorrarTablas()
        Try
            Dim sql As String
            Dim sql2 As String
            Dim sql3 As String
            Dim sql4 As String
            Dim sql5 As String
            Dim sql6 As String
            Dim consecutivo1 As String


            ' Borrar datos de las tablas
            sql = "delete from DetalleDescInfonavit"
            sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql &= " and iSerie=" & cboserie.SelectedIndex
            'sql &= " and iSerie=" & cboserie.SelectedIndex
            'sql &= " and iTipoNomina=" & cboTipoNomina.SelectedIndex
            sql2 = " delete from DetallePensionAlimenticia"
            sql2 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql2 &= " and iSerie=" & cboserie.SelectedIndex

            sql3 = " delete from DetalleFonacot"
            sql3 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql3 &= " and iSerie=" & cboserie.SelectedIndex

            sql4 = " delete from PagoPrestamo"
            sql4 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql4 &= " and iSerie=" & cboserie.SelectedIndex

            sql5 = " delete from PagoPrestamoSA"
            sql5 &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
            sql5 &= " and iSerie=" & cboserie.SelectedIndex



            If nExecute(sql) = False Then
                MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'pnlProgreso.Visible = False
                Exit Sub
            End If

            If nExecute(sql2) = False Then
                MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'pnlProgreso.Visible = False
                Exit Sub
            End If

            If nExecute(sql3) = False Then
                MessageBox.Show("Ocurrio un error borrando fonacot ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'pnlProgreso.Visible = False
                Exit Sub
            End If

            If nExecute(sql4) = False Then
                MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'pnlProgreso.Visible = False
                Exit Sub
            End If

            If nExecute(sql5) = False Then
                MessageBox.Show("Ocurrio un error borrando prestamo asimilados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'pnlProgreso.Visible = False
                Exit Sub
            End If



        Catch ex As Exception

        End Try
    End Sub

    Private Sub llenarTablas(Optional ByRef tiponom As String = "")


        'Try
        '    Dim sql As String
        '    Dim consecutivo1 As String
        '    Dim abordo As Boolean = True
        '    Dim descanso As Boolean = False




        '    'GUARDAR LOS DATOS
        '    '########GUARDAR INFONAVIT
        '    For x As Integer = 0 To dtgDatos.Rows.Count - 1

        '        If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
        '            consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)

        '        Else
        '            consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
        '        End If


        '        Dim numbimestre As Integer
        '        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '        Else
        '            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '        End If


        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value)) > 0 Then

        '            Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '            sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '            'fk Calculo infonavit
        '            sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '            'Cantidad
        '            sql &= "," & dtgDatos.Rows(x).Cells(38).Value
        '            ' fk Empleado
        '            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '            'Numbimestre
        '            sql &= "," & numbimestre
        '            'Anio
        '            sql &= "," & FechaInicioPeriodoGlobal.Year
        '            'fk Periodo
        '            sql &= "," & cboperiodo.SelectedValue
        '            'Serie
        '            sql &= "," & cboserie.SelectedIndex
        '            'Tipo Nomina
        '            sql &= ",0"
        '            'Tipo Pagadora
        '            sql &= ",101"
        '            'iEstatu
        '            sql &= ",1"
        '            'tipo consecutivo
        '            sql &= "," & consecutivo1

        '            If nExecute(sql) = False Then
        '                MessageBox.Show("Ocurrio un error insertar detalle infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                'pnlProgreso.Visible = False
        '                Exit Sub
        '            End If
        '        End If



        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", "0", dtgDatos.Rows(x).Cells(39).Value)) > 0 Then

        '            Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '            sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '            'fk Calculo infonavit
        '            sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '            'Cantidad
        '            sql &= "," & dtgDatos.Rows(x).Cells(39).Value
        '            ' fk Empleado
        '            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '            'Numbimestre
        '            sql &= "," & numbimestre
        '            'Anio
        '            sql &= "," & FechaInicioPeriodoGlobal.Year
        '            'fk Periodo
        '            sql &= "," & cboperiodo.SelectedValue
        '            'Serie
        '            sql &= "," & cboserie.SelectedIndex
        '            'Tipo Nomina
        '            sql &= ",0"
        '            'Tipo Pagadora
        '            sql &= ",102"
        '            'iEstatu
        '            sql &= ",1"
        '            'tipo consecutivo
        '            sql &= "," & consecutivo1

        '            If nExecute(sql) = False Then
        '                MessageBox.Show("Ocurrio un error insertar detalle infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                'pnlProgreso.Visible = False
        '                Exit Sub
        '            End If
        '        End If


        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", "0", dtgDatos.Rows(x).Cells(40).Value)) > 0 Then

        '            Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '            sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '            'fk Calculo infonavit
        '            sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '            'Cantidad
        '            sql &= "," & dtgDatos.Rows(x).Cells(40).Value
        '            ' fk Empleado
        '            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '            'Numbimestre
        '            sql &= "," & numbimestre
        '            'Anio
        '            sql &= "," & FechaInicioPeriodoGlobal.Year
        '            'fk Periodo
        '            sql &= "," & cboperiodo.SelectedValue
        '            'Serie
        '            sql &= "," & cboserie.SelectedIndex
        '            'Tipo Nomina
        '            sql &= ",0"
        '            'Tipo Pagadora
        '            sql &= ",103"
        '            'iEstatu
        '            sql &= ",1"
        '            'tipo consecutivo
        '            sql &= "," & consecutivo1

        '            If nExecute(sql) = False Then
        '                MessageBox.Show("Ocurrio un error insertar detalle infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                'pnlProgreso.Visible = False
        '                Exit Sub
        '            End If
        '        End If


        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", "0", dtgDatos.Rows(x).Cells(48).Value)) > 0 Then

        '            Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '            sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '            'fk Calculo infonavit
        '            sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '            'Cantidad
        '            sql &= "," & dtgDatos.Rows(x).Cells(48).Value
        '            ' fk Empleado
        '            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '            'Numbimestre
        '            sql &= "," & numbimestre
        '            'Anio
        '            sql &= "," & FechaInicioPeriodoGlobal.Year
        '            'fk Periodo
        '            sql &= "," & cboperiodo.SelectedValue
        '            'Serie
        '            sql &= "," & cboserie.SelectedIndex
        '            'Tipo Nomina
        '            sql &= ",0"
        '            'Tipo Pagadora
        '            sql &= ",201"
        '            'iEstatu
        '            sql &= ",1"
        '            'tipo consecutivo
        '            sql &= "," & consecutivo1

        '            If nExecute(sql) = False Then
        '                MessageBox.Show("Ocurrio un error insertar detalle infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                'pnlProgreso.Visible = False
        '                Exit Sub
        '            End If
        '        End If

        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", "0", dtgDatos.Rows(x).Cells(49).Value)) > 0 Then

        '            Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '            sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '            'fk Calculo infonavit
        '            sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '            'Cantidad
        '            sql &= "," & dtgDatos.Rows(x).Cells(49).Value
        '            ' fk Empleado
        '            sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '            'Numbimestre
        '            sql &= "," & numbimestre
        '            'Anio
        '            sql &= "," & FechaInicioPeriodoGlobal.Year
        '            'fk Periodo
        '            sql &= "," & cboperiodo.SelectedValue
        '            'Serie
        '            sql &= "," & cboserie.SelectedIndex
        '            'Tipo Nomina
        '            sql &= ",0"
        '            'Tipo Pagadora
        '            sql &= ",202"
        '            'iEstatu
        '            sql &= ",1"
        '            'tipo consecutivo
        '            sql &= "," & consecutivo1

        '            If nExecute(sql) = False Then
        '                MessageBox.Show("Ocurrio un error insertar detalle infonavit ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                'pnlProgreso.Visible = False
        '                Exit Sub
        '            End If
        '        End If

        '        'GUARDAR FONACOT
        '        If Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", "0", dtgDatos.Rows(x).Cells(43).Value)) > 0 Then
        '            sql = "SELECT * FROM FONACOT WHERE fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value & "and iEstatus=1"

        '            Dim rwFonacotEmpleado As DataRow() = nConsulta(sql)
        '            If rwFonacotEmpleado Is Nothing = False Then
        '                sql = "EXEC setDetalleFonacotInsertar  0"
        '                'fk Calculo infonavit
        '                sql &= "," & rwFonacotEmpleado(0)("iIdFonacot")
        '                ' fk Empleado
        '                sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '                'fk Periodo
        '                sql &= "," & cboperiodo.SelectedValue
        '                'Monto
        '                sql &= "," & dtgDatos.Rows(x).Cells(43).Value
        '                'Serie
        '                sql &= "," & cboserie.SelectedIndex
        '                'Tipo Nomina
        '                sql &= ",0"
        '                'Tipo Pagadora
        '                sql &= ",301"
        '                'iEstatu
        '                sql &= ",1"
        '                'tipo consecutivo
        '                sql &= "," & consecutivo1

        '                If nExecute(sql) = False Then
        '                    MessageBox.Show("Ocurrio un error insertar pago prestamo ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                    'pnlProgreso.Visible = False
        '                    Exit Sub
        '                End If
        '            Else
        '                MessageBox.Show("Existe valor para fonacot, pero no esta el prestamo dado de alta, favor de verificar.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            End If


        '        End If



        '        'sql = "SELECT  * FROM PrestamoSA WHERE iidPrestamosa =("
        '        'sql &= "SELECT max(iidPrestamosa) FROM PrestamoSA WHERE fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value & " and iEstatus=1)"

        '        'Dim rwPrestamoSAEmpleado As DataRow() = nConsulta(sql)
        '        'If rwPrestamoSAEmpleado Is Nothing = False Then


        '        '    sql = "select isnull(sum(monto),0) as monto from PagoPrestamoSA where fkiIdPrestamoSA=" & rwPrestamoSAEmpleado(0)("iIdPrestamoSA") & " and fkiIdPeriodo=" & cboperiodo.SelectedValue
        '        '    Dim rwMontoPrestamoMensualSA As DataRow() = nConsulta(sql)
        '        '    If rwMontoPrestamoMensualSA Is Nothing = False Then
        '        '        If Double.Parse(rwPrestamoSAEmpleado(0)("descuento")) > Double.Parse(rwMontoPrestamoMensualSA(0)("monto")) Then
        '        '            Dim FaltantePagoMes As Double
        '        '            FaltantePagoMes = Double.Parse(rwPrestamoSAEmpleado(0)("descuento")) - Double.Parse(rwMontoPrestamoMensualSA(0)("monto"))


        '        '            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", "0", dtgDatos.Rows(x).Cells(42).Value)) > 0 Then

        '        '                'Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '        '                sql = "EXEC setPagoPrestamoSAInsertar  0"
        '        '                'iIdPrestamo
        '        '                sql &= "," & rwPrestamoSAEmpleado(0)("iIdPrestamoSA")
        '        '                'fk Periodo
        '        '                sql &= "," & cboperiodo.SelectedValue
        '        '                ' fk Empleado
        '        '                sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '        '                'Monto
        '        '                sql &= "," & dtgDatos.Rows(x).Cells(42).Value
        '        '                'Serie
        '        '                sql &= "," & cboserie.SelectedIndex
        '        '                'Tipo Nomina
        '        '                sql &= ",0"
        '        '                'Tipo Pagadora
        '        '                sql &= ",501"
        '        '                'Fecha Calculo
        '        '                sql &= ",'" & Date.Now.ToShortDateString
        '        '                'iEstatu
        '        '                sql &= "',1"
        '        '                sql &= "," & consecutivo1

        '        '                If nExecute(sql) = False Then
        '        '                    MessageBox.Show("Ocurrio un error insertar pago prestamo ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        '                    'pnlProgreso.Visible = False
        '        '                    Exit Sub
        '        '                End If
        '        '            End If
        '        '        Else
        '        '            dtgDatos.Rows(x).Cells(42).Value = "0.00"
        '        '        End If
        '        '    End If
        '        'End If

        '        'Prestamo Personal Asimilado
        '        'sql = "SELECT * FROM prestamo WHERE fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value & " and iEstatus=1"
        '        sql = "SELECT  * FROM prestamo WHERE iidPrestamo =("
        '        sql &= "SELECT max(iidPrestamo) FROM Prestamo WHERE fkiIdEmpleado=" & dtgDatos.Rows(x).Cells(2).Value & " and iEstatus=1)"

        '        Dim rwPrestamoAsiEmpleado As DataRow() = nConsulta(sql)
        '        If rwPrestamoAsiEmpleado Is Nothing = False Then
        '            'ya existe el pago
        '            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", "0", dtgDatos.Rows(x).Cells(47).Value)) > 0 Then
        '                'Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))
        '                sql = "EXEC setPagoPrestamoInsertar  0"
        '                'iIdPrestamo
        '                sql &= "," & rwPrestamoAsiEmpleado(0)("iIdPrestamo")
        '                'fk Periodo
        '                sql &= "," & cboperiodo.SelectedValue
        '                ' fk Empleado
        '                sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '                'Monto
        '                sql &= "," & dtgDatos.Rows(x).Cells(47).Value
        '                'Serie
        '                sql &= "," & cboserie.SelectedIndex
        '                'Tipo Nomina
        '                sql &= ",0"
        '                'Tipo Pagadora
        '                sql &= ",501"
        '                'Fecha Calculo
        '                sql &= ",'" & Date.Now.ToShortDateString
        '                'iEstatu
        '                sql &= "',1"
        '                sql &= "," & consecutivo1

        '                If nExecute(sql) = False Then
        '                    MessageBox.Show("Ocurrio un error insertar pago prestamo ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                    'pnlProgreso.Visible = False
        '                    Exit Sub
        '                End If
        '            End If
        '        End If
        '        'Pensiones
        '        Dim TotalPercepciones As Double
        '        Dim Incapacidad As Double
        '        Dim isr As Double
        '        Dim imss As Double
        '        Dim infonavitvalor As Double
        '        Dim infonavitanterior As Double
        '        Dim ajusteinfonavit As Double
        '        Dim pension As Double
        '        Dim prestamo As Double
        '        Dim fonacot As Double
        '        Dim subsidiogenerado As Double
        '        Dim subsidioaplicado As Double
        '        Dim PensionAlimenticia As Double

        '        TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '        Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '        isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '        imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))
        '        infonavitvalor = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value))
        '        infonavitanterior = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", "0", dtgDatos.Rows(x).Cells(39).Value))
        '        ajusteinfonavit = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", "0", dtgDatos.Rows(x).Cells(40).Value))
        '        ' pension = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", "0", dtgDatos.Rows(x).Cells(41).Value))
        '        prestamo = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", "0", dtgDatos.Rows(x).Cells(42).Value))
        '        fonacot = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", "0", dtgDatos.Rows(x).Cells(43).Value))
        '        subsidioaplicado = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", "0", dtgDatos.Rows(x).Cells(45).Value))

        '        PensionAlimenticia = TotalPercepciones - Incapacidad - isr - imss - infonavitvalor - infonavitanterior - ajusteinfonavit - prestamo - fonacot + subsidioaplicado


        '        sql = "select * from PensionAlimenticia where fkiIdEmpleadoC=" & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value) & " and iEstatus=1"

        '        Dim rwPensionEmpleado As DataRow() = nConsulta(sql)
        '        If rwPensionEmpleado Is Nothing = False Then
        '            For y As Integer = 0 To rwPensionEmpleado.Length - 1
        '                'dtgDatos.Rows(x).Cells(41).Value = PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100)
        '                'Insertar la pension
        '                'Insertamos los datos
        '                sql = "EXEC [setDetallePensionAlimenticiaInsertar] 0"
        '                'Id Empleado
        '                sql &= "," & Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)
        '                'id Pension
        '                sql &= "," & Integer.Parse(rwPensionEmpleado(y)("iIdPensionAlimenticia"))
        '                'id Periodo
        '                sql &= ",'" & cboperiodo.SelectedValue
        '                'serie
        '                sql &= "'," & cboserie.SelectedIndex
        '                'tipo
        '                sql &= ",0"
        '                'Monto
        '                sql &= "," & Math.Round(PensionAlimenticia * (Double.Parse(rwPensionEmpleado(y)("fPorcentaje")) / 100), 2)
        '                'Estatus
        '                sql &= ",1"
        '                sql &= "," & consecutivo1
        '                If nExecute(sql) = False Then
        '                    MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        '                End If
        '            Next

        '        End If


        '    Next






        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub cmdCalculoSoloInfonavit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalculoSoloInfonavit.Click
        'Dim Sueldo As Double
        'Dim SueldoBase As Double
        'Dim ValorIncapacidad As Double
        'Dim TotalPercepciones As Double
        'Dim Incapacidad As Double
        'Dim isr As Double
        'Dim imss As Double
        'Dim infonavitvalor As Double
        'Dim infonavitanterior As Double
        'Dim ajusteinfonavit As Double
        'Dim pension As Double
        'Dim prestamo As Double
        'Dim fonacot As Double
        'Dim subsidiogenerado As Double
        'Dim subsidioaplicado As Double
        'Dim RetencionOperadora As Double
        'Dim InfonavitNormal As Double
        'Dim PrestamoPersonalAsimilados As Double
        'Dim PrestamoPersonalSA As Double
        'Dim AdeudoINfonavitAsimilados As Double
        'Dim DiferenciaInfonavitAsimilados As Double
        'Dim PensionAlimenticia As Double
        'Dim PensionAlimenticiaInsertar As Double

        'Dim Operadora As Double
        'Dim ComplementoAsimilados As Double

        'Dim SueldoBaseTMM As Double
        'Dim CostoSocialTotal As Double
        'Dim ComisionOperadora As Double
        'Dim ComisionAsimilados As Double
        'Dim subtotal As Double
        'Dim iva As Double



        'Dim sql As String
        'Dim sql2 As String
        'Dim sql3 As String
        'Dim sql4 As String
        'Dim sql5 As String
        'Dim ValorUMA As Double
        'Dim primavacacionesgravada As Double
        'Dim primavacacionesexenta As Double
        'Dim diastrabajados As Double
        'Dim Sueldobruto As Double
        'Dim TEFG As Double
        'Dim TEFE As Double
        'Dim TEO As Double
        'Dim DSO As Double
        'Dim VACAPRO As Double
        'Dim numbimestre As Integer
        'Dim NOCALCULAR As Boolean
        'Dim consecutivo1 As String
        'Dim plantaoNO As String
        'Try

        '    pnlProgreso.Visible = True

        '    Application.DoEvents()
        '    pnlCatalogo.Enabled = False
        '    pgbProgreso.Minimum = 0
        '    pgbProgreso.Value = 0
        '    pgbProgreso.Maximum = dtgDatos.Rows.Count
        '    For x As Integer = 0 To dtgDatos.Rows.Count - 1

        '        sql = "delete from DetalleDescInfonavit"
        '        sql &= " where fkiIdPeriodo=" & cboperiodo.SelectedValue
        '        sql &= " and iSerie=" & cboserie.SelectedIndex

        '        If nExecute(sql) = False Then
        '            MessageBox.Show("Ocurrio un error ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '            'pnlProgreso.Visible = False
        '            Exit Sub
        '        End If


        '        If InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) > 0 Then
        '            consecutivo1 = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(0, InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text) - 1)
        '            plantaoNO = dtgDatos.Rows(x).Cells(5).Value.ToString.Substring(InStr(1, dtgDatos.Rows(x).Cells(5).Value, "+", CompareMethod.Text))

        '        Else
        '            consecutivo1 = IIf(dtgDatos.Rows(x).Cells(1).Value = "", "0", dtgDatos.Rows(x).Cells(1).Value.ToString.Replace(",", ""))
        '            plantaoNO = dtgDatos.Rows(x).Cells(5).Value
        '        End If

        '        'verificamos los sueldos
        '        sql = "Select salariod,sbc,salariodTopado,sbcTopado from costosocial inner join puestos on costosocial.fkiIdPuesto=puestos.iIdPuesto "
        '        sql &= " where cNombre = '" & dtgDatos.Rows(x).Cells(11).FormattedValue & "' and anio=" & aniocostosocial

        '        Dim rwDatosSalario As DataRow() = nConsulta(sql)

        '        If rwDatosSalario Is Nothing = False Then
        '            If dtgDatos.Rows(x).Cells(10).Value >= 55 Then
        '                dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariodTopado")
        '                dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbcTopado")
        '            Else
        '                dtgDatos.Rows(x).Cells(16).Value = rwDatosSalario(0)("salariod")
        '                dtgDatos.Rows(x).Cells(17).Value = rwDatosSalario(0)("sbc")
        '            End If

        '        Else
        '            MessageBox.Show("No se encontraron datos")
        '        End If


        '        diastrabajados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(18).Value = "", "0", dtgDatos.Rows(x).Cells(18).Value))
        '        If diastrabajados = 0 Then
        '            'INFONAVIT
        '            '##### VERIFICAR SI ESTA YA CALCULADO EL INFONAVIT DEL BIMESTRE
        '            dtgDatos.Rows(x).Cells(38).Value = "0.00"


        '            '############# CALCULO POR DIAS INFONAVIT
        '            'dtgDatos.Rows(x).Cells(38).Value = Math.Round(infonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Double.Parse(dtgDatos.Rows(x).Cells(18).Value), Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)), 2).ToString("###,##0.00")
        '            '############# CALCULO POR DIAS INFONAVIT

        '            'INFONAVIT BIMESTRE ANTERIOR
        '            'AJUSTE INFONAVIT
        '            'PENSION
        '            'PRESTAMO
        '            'FONACOT
        '            'SUBSIDIO GENERADO
        '            dtgDatos.Rows(x).Cells(44).Value = "0.00"
        '            'SUBSIDIO APLICADO
        '            dtgDatos.Rows(x).Cells(45).Value = "0.00"
        '            'NETO
        '            TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '            Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '            isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '            imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))
        '            infonavitvalor = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value))
        '            infonavitanterior = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", "0", dtgDatos.Rows(x).Cells(39).Value))
        '            ajusteinfonavit = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", "0", dtgDatos.Rows(x).Cells(40).Value))
        '            pension = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", "0", dtgDatos.Rows(x).Cells(41).Value))
        '            prestamo = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", "0", dtgDatos.Rows(x).Cells(42).Value))
        '            fonacot = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", "0", dtgDatos.Rows(x).Cells(43).Value))
        '            subsidiogenerado = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", "0", dtgDatos.Rows(x).Cells(44).Value))
        '            subsidioaplicado = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", "0", dtgDatos.Rows(x).Cells(45).Value))

        '            Operadora = 0
        '            dtgDatos.Rows(x).Cells(46).Value = Operadora

        '        Else


        '            'Aguinaldo total
        '            'dtgDatos.Rows(x).Cells(29).Value = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(27).Value) + Double.Parse(dtgDatos.Rows(x).Cells(28).Value), 2)

        '            'Prima de vacaciones

        '            'Calculos prima

        '            'INFONAVIT
        '            '##### VERIFICAR SI ESTA YA CALCULADO EL INFONAVIT DEL BIMESTRE
        '            'Aqui verificamos si esta activo el calcular o no el infonavit


        '            If chkNoinfonavit.Checked = False Then
        '                If dtgDatos.Rows(x).Tag = "" Then
        '                    'borramos el calculo previo del infonavit para tener siempre que generar el calculo por cualquier cambio que se requiera
        '                    'este cambio va dentro de la funcion verificacalculoinfonavit

        '                    Dim CalculoInfonavit As Integer = VerificarCalculoInfonavit(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                    Select Case CalculoInfonavit
        '                        Case 0
        '                            'No es necesario calcular
        '                            dtgDatos.Rows(x).Cells(38).Value = "0.00"
        '                        Case 1
        '                            'Ya esta Calculado
        '                            'Verificar cuanto le toca para el pago
        '                            If chkInfonavit0.Checked Then
        '                                'Ya esta Calculado
        '                                'Verificar cuanto le toca para el pago
        '                                Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                                If MontoInfonavit > 0 Then

        '                                    'Dim numbimestre As Integer

        '                                    If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '                                        numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '                                    Else
        '                                        numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '                                    End If
        '                                    sql = "select isnull(sum(Cantidad),0) as monto from DetalleDescInfonavit where iTipoPagadora=101 and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value & " and Numbimestre= " & numbimestre & " and Anio=" & FechaInicioPeriodoGlobal.Year
        '                                    Dim rwMontoInfonavit As DataRow() = nConsulta(sql)
        '                                    If rwMontoInfonavit Is Nothing = False Then

        '                                        If Double.Parse(rwMontoInfonavit(0)("monto").ToString) < MontoInfonavit Then
        '                                            'Diferencia
        '                                            Dim FaltanteInfonavit As Double = MontoInfonavit - Double.Parse(rwMontoInfonavit(0)("monto").ToString)

        '                                            TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '                                            Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '                                            isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '                                            imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))

        '                                            Dim SubtotalAntesInfonavit As Double = TotalPercepciones - Incapacidad - isr - imss


        '                                             If SubtotalAntesInfonavit > (FaltanteInfonavit / 2) Then
        '                                                dtgDatos.Rows(x).Cells(38).Value = Math.Round((FaltanteInfonavit / 2), 2)

        '                                            Else
        '                                                dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                            End If



        '                                        Else
        '                                            dtgDatos.Rows(x).Cells(38).Value = "0.00"
        '                                        End If


        '                                    End If
        '                                Else
        '                                    dtgDatos.Rows(x).Cells(38).Value = "0.00"

        '                                End If

        '                            Else
        '                                Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                                If MontoInfonavit > 0 Then
        '                                    'Dim numbimestre As Integer

        '                                    If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '                                        numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '                                    Else
        '                                        numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '                                    End If
        '                                    sql = "select isnull(sum(Cantidad),0) as monto from DetalleDescInfonavit where iTipoPagadora=101 and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value & " and Numbimestre= " & numbimestre & " and Anio=" & FechaInicioPeriodoGlobal.Year
        '                                    Dim rwMontoInfonavit As DataRow() = nConsulta(sql)
        '                                    If rwMontoInfonavit Is Nothing = False Then

        '                                        'Verificamos el monto del infonavit a calcular

        '                                        InfonavitNormal = Math.Round(infonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Double.Parse(dtgDatos.Rows(x).Cells(18).Value), Integer.Parse(dtgDatos.Rows(x).Cells(2).Value), Integer.Parse(dtgDatos.Rows(x).Cells(1).Value) - 1), 2).ToString("###,##0.00")

        '                                        '########


        '                                        If Double.Parse(rwMontoInfonavit(0)("monto").ToString) < MontoInfonavit Then
        '                                            'Diferencia
        '                                            Dim FaltanteInfonavit As Double = MontoInfonavit - Double.Parse(rwMontoInfonavit(0)("monto").ToString)

        '                                            TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '                                            Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '                                            isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '                                            imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))

        '                                            Dim SubtotalAntesInfonavit As Double = TotalPercepciones - Incapacidad - isr - imss


        '                                            'VErificamos el infonavit

        '                                            If FaltanteInfonavit > InfonavitNormal Then

        '                                                If SubtotalAntesInfonavit > InfonavitNormal Then
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((InfonavitNormal), 2)

        '                                                Else
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                                End If
        '                                            Else
        '                                                If SubtotalAntesInfonavit > FaltanteInfonavit Then
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((FaltanteInfonavit), 2)

        '                                                Else
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                                End If

        '                                            End If




        '                                            'If SubtotalAntesInfonavit > (FaltanteInfonavit / 2) Then
        '                                            '    dtgDatos.Rows(x).Cells(38).Value = Math.Round((FaltanteInfonavit / 2), 2)

        '                                            'Else
        '                                            '    dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                            'End If



        '                                        Else
        '                                            dtgDatos.Rows(x).Cells(38).Value = "0.00"
        '                                        End If


        '                                    End If
        '                                Else
        '                                    dtgDatos.Rows(x).Cells(38).Value = "0.00"

        '                                End If
        '                            End If


        '                        Case 2
        '                            If chkInfonavit0.Checked Then
        '                                'No esta calculado
        '                                If CalcularInfonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)) Then
        '                                    'Verificar cuanto le toca para el pago
        '                                    Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                                    If MontoInfonavit > 0 Then
        '                                        'Dim numbimestre As Integer

        '                                        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '                                            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '                                        Else
        '                                            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '                                        End If
        '                                        sql = "select isnull(sum(Cantidad),0) as monto from DetalleDescInfonavit where iTipoPagadora=101 and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value & " and Numbimestre= " & numbimestre & " and Anio=" & FechaInicioPeriodoGlobal.Year
        '                                        Dim rwMontoInfonavit As DataRow() = nConsulta(sql)
        '                                        If rwMontoInfonavit Is Nothing = False Then

        '                                            If Double.Parse(rwMontoInfonavit(0)("monto").ToString) < MontoInfonavit Then
        '                                                'Diferencia
        '                                                Dim FaltanteInfonavit As Double = MontoInfonavit - Double.Parse(rwMontoInfonavit(0)("monto").ToString)

        '                                                TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '                                                Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '                                                isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '                                                imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))

        '                                                Dim SubtotalAntesInfonavit As Double = TotalPercepciones - Incapacidad - isr - imss

        '                                                 If SubtotalAntesInfonavit > (FaltanteInfonavit / 2) Then
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((FaltanteInfonavit / 2), 2)

        '                                                Else
        '                                                    dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                                End If



        '                                            Else
        '                                                dtgDatos.Rows(x).Cells(38).Value = "0.00"
        '                                            End If


        '                                        End If
        '                                    Else
        '                                        dtgDatos.Rows(x).Cells(38).Value = "0.00"

        '                                    End If


        '                                End If
        '                            Else
        '                                'No esta calculado
        '                                If CalcularInfonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)) Then
        '                                    'Verificar cuanto le toca para el pago
        '                                    Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                                    If MontoInfonavit > 0 Then


        '                                        If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '                                            numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '                                        Else
        '                                            numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '                                        End If

        '                                        sql = "select isnull(sum(Cantidad),0) as monto from DetalleDescInfonavit where  iTipoPagadora=101 and fkiIdEmpleadoC=" & dtgDatos.Rows(x).Cells(2).Value & " and Numbimestre= " & numbimestre & " and Anio=" & FechaInicioPeriodoGlobal.Year
        '                                        Dim rwMontoInfonavit As DataRow() = nConsulta(sql)
        '                                        If rwMontoInfonavit Is Nothing = False Then
        '                                            'Verificamos el monto del infonavit a calcular

        '                                            InfonavitNormal = Math.Round(infonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Double.Parse(dtgDatos.Rows(x).Cells(18).Value), Integer.Parse(dtgDatos.Rows(x).Cells(2).Value), Integer.Parse(dtgDatos.Rows(x).Cells(1).Value) - 1), 2).ToString("###,##0.00")

        '                                            '########
        '                                            If Double.Parse(rwMontoInfonavit(0)("monto").ToString) < MontoInfonavit Then
        '                                                'Diferencia
        '                                                Dim FaltanteInfonavit As Double = MontoInfonavit - Double.Parse(rwMontoInfonavit(0)("monto").ToString)

        '                                                TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '                                                Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '                                                isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '                                                imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))

        '                                                Dim SubtotalAntesInfonavit As Double = TotalPercepciones - Incapacidad - isr - imss

        '                                                If FaltanteInfonavit > InfonavitNormal Then

        '                                                    If SubtotalAntesInfonavit > InfonavitNormal Then
        '                                                        dtgDatos.Rows(x).Cells(38).Value = Math.Round((InfonavitNormal), 2)

        '                                                    Else
        '                                                        dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                                    End If
        '                                                Else
        '                                                    If SubtotalAntesInfonavit > FaltanteInfonavit Then
        '                                                        dtgDatos.Rows(x).Cells(38).Value = Math.Round((FaltanteInfonavit), 2)

        '                                                    Else
        '                                                        dtgDatos.Rows(x).Cells(38).Value = Math.Round((SubtotalAntesInfonavit - 1), 2)
        '                                                    End If

        '                                                End If



        '                                            Else
        '                                                dtgDatos.Rows(x).Cells(38).Value = "0.00"
        '                                            End If


        '                                        End If
        '                                    Else
        '                                        dtgDatos.Rows(x).Cells(38).Value = "0.00"

        '                                    End If


        '                                End If
        '                            End If

        '                    End Select
        '                Else

        '                End If
        '            End If



        '            'Guardamos el infonavit en el sistema para  tenerlo actualizado para el siguiente trabajador

        '            '########GUARDAR INFONAVIT

        '            'Dim numbimestre As Integer
        '            If Month(FechaInicioPeriodoGlobal) Mod 2 = 0 Then
        '                numbimestre = Month(FechaInicioPeriodoGlobal) / 2
        '            Else
        '                numbimestre = (Month(FechaInicioPeriodoGlobal) + 1) / 2
        '            End If


        '            If Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value)) Then

        '                Dim MontoInfonavit As Double = MontoInfonavitF(cboperiodo.SelectedValue, Integer.Parse(dtgDatos.Rows(x).Cells(2).Value))

        '                sql = "EXEC setDetalleDescInfonavitInsertar  0"
        '                'fk Calculo infonavit
        '                sql &= "," & IIf(MontoInfonavit > 0, IDCalculoInfonavit, 0)
        '                'Cantidad
        '                sql &= "," & dtgDatos.Rows(x).Cells(38).Value
        '                ' fk Empleado
        '                sql &= "," & dtgDatos.Rows(x).Cells(2).Value
        '                'Numbimestre
        '                sql &= "," & numbimestre
        '                'Anio
        '                sql &= "," & FechaInicioPeriodoGlobal.Year
        '                'fk Periodo
        '                sql &= "," & cboperiodo.SelectedValue
        '                'Serie
        '                sql &= "," & cboserie.SelectedIndex
        '                'Tipo Nomina
        '                sql &= ",0"
        '                'Tipo Pagadora
        '                sql &= ",101"
        '                'iEstatu
        '                sql &= ",1"
        '                sql &= "," & consecutivo1

        '                If nExecute(sql) = False Then
        '                    MessageBox.Show("Ocurrio un error insertar pago prestamo ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                    'pnlProgreso.Visible = False
        '                    Exit Sub
        '                End If
        '            End If


        '            '########GUARDAR SEGURO INFONAVIT





        '            '############# CALCULO POR DIAS INFONAVIT

        '            'dtgDatos.Rows(x).Cells(38).Value = Math.Round(infonavit(dtgDatos.Rows(x).Cells(13).Value, Double.Parse(dtgDatos.Rows(x).Cells(14).Value), Double.Parse(dtgDatos.Rows(x).Cells(17).Value), Date.Parse("01/01/1900"), cboperiodo.SelectedValue, Double.Parse(dtgDatos.Rows(x).Cells(18).Value), Integer.Parse(dtgDatos.Rows(x).Cells(2).Value)), 2).ToString("###,##0.00")
        '            '############# CALCULO POR DIAS INFONAVIT


        '            'SUBSIDIO GENERADO

        '            TotalPercepciones = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", "0", dtgDatos.Rows(x).Cells(33).Value.ToString.Replace(",", "")))
        '            Incapacidad = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", "0", dtgDatos.Rows(x).Cells(35).Value))
        '            isr = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", "0", dtgDatos.Rows(x).Cells(36).Value))
        '            imss = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", "0", dtgDatos.Rows(x).Cells(37).Value))
        '            infonavitvalor = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", "0", dtgDatos.Rows(x).Cells(38).Value))
        '            infonavitanterior = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", "0", dtgDatos.Rows(x).Cells(39).Value))
        '            ajusteinfonavit = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", "0", dtgDatos.Rows(x).Cells(40).Value))



        '            'CALCULAR FONACOT


        '            fonacot = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", "0", dtgDatos.Rows(x).Cells(43).Value))
        '            subsidiogenerado = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", "0", dtgDatos.Rows(x).Cells(44).Value))
        '            subsidioaplicado = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", "0", dtgDatos.Rows(x).Cells(45).Value))



        '            'INFONAVIT BIMESTRE ANTERIOR
        '            'AJUSTE INFONAVIT

        '            'PRESTAMO

        '            prestamo = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", "0", dtgDatos.Rows(x).Cells(42).Value))

        '            'PENSION
        '            PensionAlimenticia = TotalPercepciones - Incapacidad - isr - imss - infonavitvalor - infonavitanterior - ajusteinfonavit - prestamo - fonacot + subsidioaplicado
        '            'Buscamos la Pension
        '            'If dtgDatos.Rows(x).Cells(2).Value = 94 Then
        '            '    MessageBox.Show("EL EMPLEADO ES " & dtgDatos.Rows(x).Cells(4).Value, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '            'End If


        '            pension = 0



        '            'NETO
        '            pension = dtgDatos.Rows(x).Cells(41).Value

        '            'pension = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", "0", dtgDatos.Rows(x).Cells(41).Value))
        '            Operadora = Math.Round(TotalPercepciones - Incapacidad - isr - imss - infonavitvalor - infonavitanterior - ajusteinfonavit - pension - prestamo - fonacot + subsidioaplicado, 2)
        '            dtgDatos.Rows(x).Cells(46).Value = Operadora

        '        End If


        '        'Sueldo Base TMM
        '        SueldoBaseTMM = (Double.Parse(IIf(dtgDatos.Rows(x).Cells(15).Value = "", "0", dtgDatos.Rows(x).Cells(15).Value))) ' / 2

        '        'Prestamo Personal Asimilado

        '        PrestamoPersonalAsimilados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", "0", dtgDatos.Rows(x).Cells(47).Value))

        '        'Adeudo_Infonavit_Asimilado
        '        AdeudoINfonavitAsimilados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", "0", dtgDatos.Rows(x).Cells(48).Value))
        '        'Difencia infonavit Asimilado
        '        DiferenciaInfonavitAsimilados = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", "0", dtgDatos.Rows(x).Cells(49).Value))
        '        'Complemento Asimilado
        '        ComplementoAsimilados = Math.Round(SueldoBaseTMM - infonavitvalor - infonavitanterior - ajusteinfonavit - pension - prestamo - fonacot - PrestamoPersonalAsimilados - AdeudoINfonavitAsimilados - DiferenciaInfonavitAsimilados - Operadora, 2)
        '        If ComplementoAsimilados < 0 And subsidioaplicado > 0 And (dtgDatos.Rows(x).Cells(11).FormattedValue = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or dtgDatos.Rows(x).Cells(11).FormattedValue = "SUBALTERNO EN FORMACIÓN") Then
        '            SueldoBaseTMM = (Double.Parse(IIf(dtgDatos.Rows(x).Cells(15).Value = "", "0", dtgDatos.Rows(x).Cells(15).Value))) - ComplementoAsimilados
        '            dtgDatos.Rows(x).Cells(15).Value = SueldoBaseTMM ' / 2

        '            ComplementoAsimilados = Math.Round(SueldoBaseTMM - infonavitvalor - infonavitanterior - ajusteinfonavit - pension - prestamo - fonacot - PrestamoPersonalAsimilados - AdeudoINfonavitAsimilados - DiferenciaInfonavitAsimilados - Operadora, 2)
        '        End If


        '        dtgDatos.Rows(x).Cells(50).Value = ComplementoAsimilados
        '        'Retenciones_Operadora
        '        RetencionOperadora = Math.Round(Incapacidad + isr + imss + infonavitvalor + infonavitanterior + ajusteinfonavit + pension + prestamo + fonacot, 2)
        '        dtgDatos.Rows(x).Cells(51).Value = RetencionOperadora

        '        '%Comision
        '        dtgDatos.Rows(x).Cells(52).Value = "2%"
        '        'Comision Maecco
        '        ComisionOperadora = Math.Round((Operadora + RetencionOperadora) * 0.02, 2)
        '        dtgDatos.Rows(x).Cells(53).Value = ComisionOperadora

        '        'Comision Complemento
        '        ComisionAsimilados = Math.Round((ComplementoAsimilados + PrestamoPersonalAsimilados + AdeudoINfonavitAsimilados + DiferenciaInfonavitAsimilados) * 0.02, 2)
        '        dtgDatos.Rows(x).Cells(54).Value = ComisionAsimilados


        '        'Calcular el costo social

        '        'Obtenemos los datos del empleado,id puesto
        '        'de acuerdo a la edad y el status

        '        '############################################################################
        '        '#######################################################################
        '        '###########################################################
        '        '#############################################


        '        'TOTAL COSTO SOCIAL
        '        CostoSocialTotal = Math.Round(Double.Parse(dtgDatos.Rows(x).Cells(55).Value) + Double.Parse(dtgDatos.Rows(x).Cells(56).Value) + Double.Parse(dtgDatos.Rows(x).Cells(57).Value) + Double.Parse(dtgDatos.Rows(x).Cells(58).Value), 2)
        '        dtgDatos.Rows(x).Cells(59).Value = CostoSocialTotal

        '        'SUBTOTAL
        '        subtotal = Math.Round(ComplementoAsimilados + PrestamoPersonalAsimilados + AdeudoINfonavitAsimilados + DiferenciaInfonavitAsimilados + Operadora + RetencionOperadora + ComisionOperadora + ComisionAsimilados + CostoSocialTotal, 2)
        '        dtgDatos.Rows(x).Cells(60).Value = subtotal


        '        'IVA
        '        iva = Math.Round(subtotal * 0.16)
        '        dtgDatos.Rows(x).Cells(61).Value = iva
        '        'TOTAL DEPOSITO
        '        dtgDatos.Rows(x).Cells(62).Value = subtotal + iva



        '        pgbProgreso.Value += 1
        '        Application.DoEvents()
        '    Next
        '    pnlProgreso.Visible = False
        '    pnlCatalogo.Enabled = True
        '    MessageBox.Show("Datos calculados ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'End Try

    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim SQL, SQL1 As String
            Dim filaExcel As Integer = 0
            Dim filatmp As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo, iejercicio, iMes As String
            Dim seriecount As Integer

            Dim ruta As String

            Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
            If rwPeriodo0 Is Nothing = False Then
                Dim Fechafin As Date = rwPeriodo0(0).Item("dFechaFin")
                periodo = "1 " & MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " AL " & Fechafin.Day & " " & MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & rwPeriodo0(0).Item("iEjercicio")
                iejercicio = (rwPeriodo0(0).Item("iEjercicio"))
                iMes = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
            End If

            ruta = My.Application.Info.DirectoryPath() & "\Archivos\resumeninfonavitxbim.xlsx"

            Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
            Dim libro As New ClosedXML.Excel.XLWorkbook

            book.Worksheet(1).CopyTo(libro, iMes)

            Dim hoja As IXLWorksheet = libro.Worksheets(0)


            filaExcel = 6
            Dim serieactual As String
            Dim inicio As Integer = 0
            Dim serieinicial As Integer = 0
            Dim seriefinal As Integer = 0
            Dim total As Integer = dtgDatos.Rows.Count - 1
            Dim x As Integer = 0

            SQL = " SELECT *"
            SQL &= " FROM Nomina "
            SQL &= " WHERE fkiIdPeriodo= " & cboperiodo.SelectedValue
            SQL &= " ORDER BY  iEstatusEmpleado DESC, fkiIdEmpleadoC"

            Dim rwFilas As DataRow() = nConsulta(SQL)

            If rwFilas Is Nothing = False Then
                For Each Fila In rwFilas
                    seriecount = Fila.Item("iEstatusEmpelado")

                    If inicio = x Then
                        serieinicial = filaExcel + x
                        serieactual = Fila.Item("iEstatusEmpelado")
                    End If
                    If serieactual = x Then
                        Dim empleado As DataRow() = nConsulta(" SELECT * FROM empleadosC WHERE iIdEmpleadoC=" & Fila.Item("fkiIdEmpleadoC"))
                        hoja.Cell(filaExcel + x, 1).Value = x + 1 'Consecutivo
                        hoja.Cell(filaExcel + x, 2).Value = iejercicio 'AÑO
                        hoja.Cell(filaExcel + x, 3).Value = Fila.Item("fkiIdPeriodo") 'BIMESTRE
                        hoja.Cell(filaExcel + x, 4).Value = empleado(0).Item("cNombreLargo") 'NOMBRE LARGO
                        hoja.Cell(filaExcel + x, 5).Value = Fila.Item("TipoInfonavit")  'TipoFactor
                        hoja.Cell(filaExcel + x, 6).Value = Fila.Item("fvalor")  'Factor
                        hoja.Cell(filaExcel + x, 7).Value = Fila.Item("fInfonavit")  'INFONAVIT sa
                        hoja.Cell(filaExcel + x, 8).Value = Fila.Item("fInfonavitBanterior") 'BIM ANTERIOR sa
                        hoja.Cell(filaExcel + x, 10).Value = Fila.Item("fAjusteInfonavit") 'AJUSTE sa
                        hoja.Cell(filaExcel + x, 11).Value = Fila.Item("fAdeudoInfonavitA") 'INFONAVIT ASIM
                        hoja.Cell(filaExcel + x, 12).Value = Fila.Item("fDiferenciaInfonavitA") 'BIM ANTERIOR ASIM
                    Else
                        seriefinal = filaExcel + x - 1
                        serieactual = x
                        filaExcel = filaExcel + 2
                        serieinicial = filaExcel + x
                        seriefinal = 0
                    End If




                    x = x + 1
                Next
            End If
            'If dtgDatos.Rows.Count > 0 Then



            'End If
            dialogo.FileName = "INFONAVIT x PERIODO- " & periodo.ToUpper
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


        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString())
        End Try
    End Sub

    Private Sub NoCalcularCostoSocialToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Try
            Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
            iFila.Cells(7).Tag = "1"
            iFila.Cells(7).Style.BackColor = Color.DarkBlue
            chkCalSoloMarcados.Checked = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DesactivarNoCalcularCostoSocialToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim iFila As DataGridViewRow = Me.dtgDatos.CurrentRow()
        iFila.Cells(7).Tag = ""
        iFila.Cells(7).Style.BackColor = Color.White
    End Sub




    Private Sub cmdKioskoSove_Click(sender As System.Object, e As System.EventArgs) Handles cmdKioskoSove.Click
        Try
            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()
            Dim pdfasim, xmlasim As String
            Dim listaArchivos As New List(Of String)

            pnlProgreso.Visible = True
            pnlCatalogo.Enabled = False
            Application.DoEvents()


            'Abrimos el machote
            Dim ruta As String
            ruta = My.Application.Info.DirectoryPath() & "\Archivos\subir_kiosko.xlsx"

            Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
            Dim libro As New ClosedXML.Excel.XLWorkbook

            book.Worksheet(1).CopyTo(libro, "Nomina")
            Dim hoja As IXLWorksheet = libro.Worksheets(0)


            hoja.Range("D:D").Style.NumberFormat.NumberFormatId = 4
            hoja.Range("F:F").Style.NumberFormat.NumberFormatId = 4


            Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
            Dim fechafin As String
            If rwPeriodo0 Is Nothing = False Then
                fechafin = rwPeriodo0(0).Item("dFechaFin")
            End If

            'Vicular ruta de XML
            'Dim Forma As New Ruta
            'Dim rutapathxml As String
            'If Forma.ShowDialog Then
            '    rutapathxml = Forma.txtRuta.Text
            '    For Each archivos As String In My.Computer.FileSystem.GetFiles(rutapathxml, FileIO.SearchOption.SearchAllSubDirectories, "*.xml")
            '        listaArchivos.Add(archivos)
            '    Next
            'End If


            filaExcel = 2

            Dim sa, asim As Double
            Dim rutafinal As String = ""
            For x As Integer = 0 To dtgDatos.Rows.Count - 1

                'Rellenar EXCEL
                sa = IIf(dtgDatos.Rows(x).Cells(46).Value = 0, 0, (CDbl(dtgDatos.Rows(x).Cells(46).Value) * 2))
                asim = IIf(dtgDatos.Rows(x).Cells(50).Value = 0, 0, (CDbl(dtgDatos.Rows(x).Cells(50).Value) * 2))

                Dim datosempleado As DataRow() = nConsulta("Select * from empleadosC where iIdEmpleadoC =" & dtgDatos.Rows(x).Cells(2).Value)
                Dim nombrecom, rfc As String
                Dim codigo As String = ""
                If rwPeriodo0 Is Nothing = False Then
                    nombrecom = datosempleado(0).Item("cNombre").ToString.Trim & " " & datosempleado(0).Item("cApellidoP").ToString.Trim & " " & datosempleado(0).Item("cApellidoM").ToString.Trim
                    rfc = datosempleado(0).Item("cRFC".ToString.Trim)
                    codigo = datosempleado(0).Item("cCodigoEmpleado".ToString.Trim)
                End If

                'Asignar xml
                'Dim docXml As New XmlDocument
                'Dim Archivo As System.IO.FileInfo

                'Dim nodos As XmlNodeList
                'For j As Integer = 0 To listaArchivos.Count - 1
                '    Archivo = New System.IO.FileInfo(listaArchivos.Item(j))
                '    'If Archivo.Name = "012022" & codigo & "T.xml" Then
                '    rutafinal = listaArchivos.Item(j)
                '    docXml.Load(rutafinal)
                '    nodos = docXml.GetElementsByTagName("cfdi:Receptor")

                '    '  End If

                '    Dim i As Integer
                '    For i = 0 To nodos.Count - 1
                '        If rfc = nodos(i).Attributes.GetNamedItem("Rfc").Value Then
                '            xmlasim = Archivo.Name.Replace(".xml", "")
                '            pdfasim = xmlasim.Trim
                '        End If
                '    Next i

                'Next



                Dim code As Integer = dtgDatos.Rows(x).Cells(3).Value
                hoja.Cell(filaExcel + x, 2).Style.NumberFormat.SetFormat("0000")
                hoja.Cell(filaExcel + x, 1).Value = fechafin 'fecha
                hoja.Cell(filaExcel + x, 2).Value = dtgDatos.Rows(x).Cells(3).Value 'codigo emple
                hoja.Cell(filaExcel + x, 3).Value = nombrecom 'nombre cmpleto
                hoja.Cell(filaExcel + x, 4).Value = sa 'importe sa
                hoja.Cell(filaExcel + x, 5).Value = IIf(sa = 0, "", nombrecom.Trim & "_FECHA_" & fechafin.Replace("/", "") & "_IMPORTE_" & sa.ToString("####.00") & "_SA") 'nombrearchivo sa
                hoja.Cell(filaExcel + x, 6).Value = asim 'importe asim
                hoja.Cell(filaExcel + x, 7).Value = IIf(asim = 0, "", nombrecom.Trim & "_FECHA_" & fechafin.Replace("/", "") & "_IMPORTE_" & asim.ToString("####.00") & "_ASIM") 'nombre archivo asimilado
                hoja.Cell(filaExcel + x, 8).Value = "NA" 'pdfasim.ToString 'nombre recibo timbrado asimilado pdf
                hoja.Cell(filaExcel + x, 9).Value = "NA" 'xmlasim.ToString 'nombre asimilado xml

                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True

            Next


            dialogo.FileName = "SUBIR KIOSKO SOVER " & MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " SERIE " & cboserie.SelectedItem
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

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try
    End Sub

    Function ValidarRepetidosE(ByVal empleadoNombre As String) As Boolean
        Try

            Dim sql As String
            Dim temp As Integer = 0
            Dim encontro As Boolean = False

            For Each fila As DataGridViewRow In dtgDatos.Rows

                fila.DefaultCellStyle.BackColor = Color.White

                If fila.Cells.Item(4).Value.ToString().Contains(empleadoNombre.ToUpper) Then
                    ' fila.DefaultCellStyle.BackColor = Color.Aquamarine
                    dtgDatos.CurrentCell = dtgDatos.Rows(fila.Index + 1).Cells(0)
                    dtgDatos.SendToBack()
                    encontro = True
                    temp = temp + 1
                    Return encontro

                End If


            Next


            'If encontro = False Then
            '    MsgBox("No se encontro nada")
            'Else
            '    MsgBox("Se encontrarón " & temp & " Registro")
            'End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function



    Private Sub layoutTimbrado_Click(sender As System.Object, e As System.EventArgs) Handles layoutTimbrado.Click
        Try
            Dim ejercicio As String
            Dim mesperiodo, periodo As String
            Dim mesid As String
            Dim idias As String
            Dim fechapagoletra As String
            Dim filaExcel As Integer = 2
            Dim dialogo As New SaveFileDialog()

            Dim pilotin As Boolean = False
            Dim pilotinF As Boolean = False

            Dim PFB_CORTO_PLAZO As Double = 0

            pnlProgreso.Visible = True
            pnlCatalogo.Enabled = False
            Application.DoEvents()

            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count

            Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & cboperiodo.SelectedValue)
            If rwPeriodo0 Is Nothing = False Then
                periodo = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " DE " & (rwPeriodo0(0).Item("iEjercicio"))
                'fecha = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
                ' iejercicio = rwPeriodo0(0).Item("iEjercicio")
                idias = rwPeriodo0(0).Item("iDiasPago")
                'periodom = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & (rwPeriodo0(0).Item("iEjercicio"))
            End If


            If dtgDatos.Rows.Count > 0 Then


                'Abrimos el machote
                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\timbradosSQ.xlsx"

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
                For x As Integer = 0 To dtgDatos.Rows.Count - 1


                    Dim cuenta, clavebanco, fechainiciorelaboral, cCP, nombrecompleto, correo As String

                    If (dtgDatos.Rows(x).Cells(3).Value Is Nothing = False) Then
                        Dim rwEmpleado As DataRow() = nConsulta("SELECT * FROM empleadosC where cCodigoEmpleado=" & dtgDatos.Rows(x).Cells(3).Value)
                        If rwEmpleado Is Nothing = False Then

                            cuenta = rwEmpleado(0).Item("Clabe")
                            correo = rwEmpleado(0).Item("cCorreo")
                            fechainiciorelaboral = rwEmpleado(0).Item("dFechaAntiguedad")
                            cCP = rwEmpleado(0).Item("cCP")
                            nombrecompleto = rwEmpleado(0).Item("cNombre").ToString.Trim + " " + rwEmpleado(0).Item("cApellidoP").ToString.Trim + " " + rwEmpleado(0).Item("cApellidoM").ToString.Trim
                            Dim rwBanco As DataRow() = nConsulta("SELECT* FROM bancos where iIdBanco=" & rwEmpleado(0).Item("fkiIdBanco"))

                            clavebanco = rwBanco(0).Item("clave")
                        End If

                    End If
                    hoja.Range(2, 1, filaExcel, 1).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 5, filaExcel, 5).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 6, filaExcel, 6).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 8, filaExcel, 8).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 13, filaExcel, 13).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 29, filaExcel, 29).Style.NumberFormat.Format = "@"
                    hoja.Range(2, 26, filaExcel, 26).Style.NumberFormat.Format = "@"


                    If dtgDatos.Rows(x).Cells(3).Value <> "" Then

                        ''Generales

                        hoja.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(3).Value 'No Empleado
                        hoja.Cell(filaExcel, 2).Value = dtgDatos.Rows(x).Cells(6).Value 'RFC
                        hoja.Cell(filaExcel, 3).Value = nombrecompleto 'Nombre
                        hoja.Cell(filaExcel, 4).Value = dtgDatos.Rows(x).Cells(7).Value 'CURP
                        hoja.Cell(filaExcel, 5).Value = dtgDatos.Rows(x).Cells(8).Value 'SSA
                        hoja.Cell(filaExcel, 6).Value = cuenta 'cuenta bancaria
                        hoja.Cell(filaExcel, 7).Value = dtgDatos.Rows(x).Cells(25).Value 'SBC //O 17 SALARIO_COTIZACION
                        hoja.Cell(filaExcel, 8).Value = dtgDatos.Rows(x).Cells(24).Value 'SDI
                        hoja.Cell(filaExcel, 9).Value = registropatronal(EmpresaN)
                        hoja.Cell(filaExcel, 10).Value = "DIF" 'ent federativa
                        hoja.Cell(filaExcel, 11).Value = dtgDatos.Rows(x).Cells(26).Value 'Días Pagados
                        hoja.Cell(filaExcel, 12).Value = fechainiciorelaboral 'FechaInicioRelaboral
                        hoja.Cell(filaExcel, 13).Value = "1" 'Tipo Contrato 
                        hoja.Cell(filaExcel, 14).Value = "Contrato de trabajo por tiempo indeterminado"
                        hoja.Cell(filaExcel, 15).Value = ""
                        hoja.Cell(filaExcel, 16).Value = "1"  'Tipo Jornada
                        hoja.Cell(filaExcel, 17).Value = "DIURNA"
                        hoja.Cell(filaExcel, 18).Value = "2"  'Tipo Regimen
                        hoja.Cell(filaExcel, 19).Value = "SUELDOS"
                        hoja.Cell(filaExcel, 20).Value = dtgDatos.Rows(x).Cells(12).FormattedValue 'DPTO
                        hoja.Cell(filaExcel, 21).Value = dtgDatos.Rows(x).Cells(11).FormattedValue 'PUESTO
                        hoja.Cell(filaExcel, 22).Value = "4" 'riesgo puesto
                        hoja.Cell(filaExcel, 23).Value = ""
                        hoja.Cell(filaExcel, 24).Value = IIf(dtgDatos.Rows(x).Cells(26).Value = "7", "2", "4")  'periocidad pago
                        hoja.Cell(filaExcel, 25).Value = ""
                        hoja.Cell(filaExcel, 26).Value = clavebanco 'BANCO
                        hoja.Cell(filaExcel, 27).Value = ""
                        hoja.Cell(filaExcel, 28).Value = "" 'SUBCONTRATACION
                        hoja.Cell(filaExcel, 29).Value = correo  'CORREO

                        filaExcel = filaExcel + 1
                    End If


                    pgbProgreso.Value += 1
                    Application.DoEvents()
                Next


                filaExcel = 4
                For x As Integer = 0 To dtgDatos.Rows.Count - 1
                    Dim nombrecompleto As String
                    If dtgDatos.Rows(x).Cells(3).Value Is Nothing = False Then
                        Dim rwEmpleado As DataRow() = nConsulta("SELECT * FROM empleadosC where cCodigoEmpleado=" & dtgDatos.Rows(x).Cells(3).Value)
                        If rwEmpleado Is Nothing = False Then
                            nombrecompleto = rwEmpleado(0).Item("cNombre").ToString.Trim + " " + rwEmpleado(0).Item("cApellidoP").ToString.Trim + " " + rwEmpleado(0).Item("cApellidoM").ToString.Trim
                            Dim rwBanco As DataRow() = nConsulta("SELECT* FROM bancos where iIdBanco=" & rwEmpleado(0).Item("fkiIdBanco"))
                        End If

                        'Percepcioness
                        hoja2.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(6).Value 'RFC
                        hoja2.Cell(filaExcel, 2).Value = nombrecompleto 'Nombre
                        hoja2.Cell(filaExcel, 3).Value = CDbl(dtgDatos.Rows(x).Cells(30).Value)   ' Septimo dia gravado
                        hoja2.Cell(filaExcel, 4).Value = ""  ' Septimo dia exento
                        hoja2.Cell(filaExcel, 5).Value = CDbl(dtgDatos.Rows(x).Cells(29).Value) ' Sueldo Base Gravado
                        hoja2.Cell(filaExcel, 6).Value = ""  ' Sueldo Base exento
                        hoja2.Cell(filaExcel, 7).Value = "" ' Aguinaldo Gravado
                        hoja2.Cell(filaExcel, 8).Value = "" ' Aguinaldo Exento
                        hoja2.Cell(filaExcel, 9).Value = CDbl(dtgDatos.Rows(x).Cells(35).Value)  ' Horas Triples importe gravado
                        hoja2.Cell(filaExcel, 10).Value = "" ' Horas Triples importe exedente
                        hoja2.Cell(filaExcel, 11).Value = CDbl(dtgDatos.Rows(x).Cells(16).Value) / 3 ' Horas Triples valor dias
                        hoja2.Cell(filaExcel, 12).Value = "2"
                        hoja2.Cell(filaExcel, 13).Value = CDbl(dtgDatos.Rows(x).Cells(16).Value) 'Horas Triples valor 
                        hoja2.Cell(filaExcel, 14).Value = CDbl(dtgDatos.Rows(x).Cells(35).Value) ' Horas Triples importe
                        hoja2.Cell(filaExcel, 15).Value = CDbl(dtgDatos.Rows(x).Cells(33).Value) 'Horas  Doble Gravado
                        hoja2.Cell(filaExcel, 16).Value = CDbl(dtgDatos.Rows(x).Cells(34).Value) 'Horas  Doble Exento
                        hoja2.Cell(filaExcel, 17).FormulaA1 = "=IF(AND(T" & filaExcel & ">0, T" & filaExcel & "<=3),1,IF(AND(T" & filaExcel & ">3,T" & filaExcel & "<=6),2,IF(T" & filaExcel & ">6,3,0)))" 'horas dobles dia
                        hoja2.Cell(filaExcel, 18).Value = "1" 'TIPO
                        hoja2.Cell(filaExcel, 19).Value = CDbl(dtgDatos.Rows(x).Cells(15).Value) 'Horas
                        hoja2.Cell(filaExcel, 20).Value = CDbl(dtgDatos.Rows(x).Cells(33).Value) + CDbl(dtgDatos.Rows(x).Cells(34).Value) 'IMPORTE TOTAL HORAS EXTRAS
                        hoja2.Cell(filaExcel, 21).Value = CDbl(dtgDatos.Rows(x).Cells(31).Value) 'PRIMA DOMINICAL GRAVADO
                        hoja2.Cell(filaExcel, 22).Value = CDbl(dtgDatos.Rows(x).Cells(32).Value) 'PRIMA DOMINICAL EXENTO
                        hoja2.Cell(filaExcel, 23).Value = CDbl(dtgDatos.Rows(x).Cells(52).Value) 'PRIMA VACACIONAL GRAVADO
                        hoja2.Cell(filaExcel, 24).Value = CDbl(dtgDatos.Rows(x).Cells(53).Value) 'PRIMA VACACIONAL EXENTO
                        hoja2.Cell(filaExcel, 25).Value = CDbl(dtgDatos.Rows(x).Cells(44).Value) 'SUELDO PENDIENTE GRAVADO/SEMANA DE FONDO
                        hoja2.Cell(filaExcel, 26).Value = "" 'SUELDO PENDIENTE EXENTO
                        hoja2.Cell(filaExcel, 27).Value = CDbl(dtgDatos.Rows(x).Cells(43).Value)  'COMPENSACION GRAVADO
                        hoja2.Cell(filaExcel, 28).Value = 0 'COMPENSACION EXENTO
                        hoja2.Cell(filaExcel, 29).Value = CDbl(dtgDatos.Rows(x).Cells(48).Value)  'VACACIONES PROPORCIONALES GRAVADO	
                        hoja2.Cell(filaExcel, 30).Value = 0 'VACACIONES PROPORCIONALES EXENTO
                        hoja2.Cell(filaExcel, 31).Value = CDbl(dtgDatos.Rows(x).Cells(36).Value)  'DESC LABORADO GRAVADO	
                        hoja2.Cell(filaExcel, 32).Value = 0 'DESC LABORADO EXENTO
                        hoja2.Cell(filaExcel, 33).Value = CDbl(dtgDatos.Rows(x).Cells(37).Value) 'DIAS FESTIVO LAB GRAVADO	
                        hoja2.Cell(filaExcel, 34).Value = 0 'DIAS FESTIVO LAB EXENGTO

                        hoja2.Cell(filaExcel, 35).Value = 0 'PREVISION_ PFB	gravado
                        hoja2.Cell(filaExcel, 36).Value = 0 'PREVISION_ PFB	 exento
                        hoja2.Cell(filaExcel, 37).Value = 0 'APORT PATRONAL PLAN FLEX LP	gravado
                        hoja2.Cell(filaExcel, 38).Value = 0 'APORT PATRONAL PLAN FLEX LP	exento

                        ''Deducciones
                        hoja3.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(6).Value 'RFC
                        hoja3.Cell(filaExcel, 2).Value = nombrecompleto 'Nombre
                        hoja3.Cell(filaExcel, 3).Value = CDbl(dtgDatos.Rows(x).Cells(59).Value) ' IMSS
                        hoja3.Cell(filaExcel, 4).Value = CDbl(dtgDatos.Rows(x).Cells(58).Value)  'ISR
                        hoja3.Cell(filaExcel, 5).Value = CDbl(dtgDatos.Rows(x).Cells(63).Value) 'PENSION ALIMENTICIA

                        hoja3.Cell(filaExcel, 6).Value = IIf(dtgDatos.Rows(x).Cells(13).Value = "VSM", dtgDatos.Rows(x).Cells(60).Value, 0) ' 'PRESTAMO INFONAVIT CF
                        hoja3.Cell(filaExcel, 7).Value = IIf(dtgDatos.Rows(x).Cells(13).Value = "CUOTA FIJA", dtgDatos.Rows(x).Cells(60).Value, 0) 'PRESTAMO INFONAVIT FD
                        hoja3.Cell(filaExcel, 8).Value = IIf(dtgDatos.Rows(x).Cells(13).Value = "PORCENTAJE", dtgDatos.Rows(x).Cells(60).Value, 0) 'PRESTAMO INFONAVIT PORC
                        hoja3.Cell(filaExcel, 9).Value = 0 'SEGUROS DE VIVIENDA
                        hoja3.Cell(filaExcel, 10).Value = CDbl(dtgDatos.Rows(x).Cells(61).Value) 'INFONAVIT BIM ANT
                        hoja3.Cell(filaExcel, 11).Value = CDbl(dtgDatos.Rows(x).Cells(65).Value) 'FONACOT
                        hoja3.Cell(filaExcel, 12).Value = CDbl(dtgDatos.Rows(x).Cells(64).Value) 'ANTICIPO SUELDO
                        hoja3.Cell(filaExcel, 13).Value = 0 'PLAN FLEX LP
                        hoja3.Cell(filaExcel, 14).Value = 0 'APOR PATRON PLAN FLEX LP

                        ''Otros Pagos
                        hoja4.Columns("A").Width = 20
                        hoja4.Columns("B").Width = 20
                        hoja4.Cell(filaExcel, 1).Value = dtgDatos.Rows(x).Cells(6).Value ' RFC
                        hoja4.Cell(filaExcel, 2).Value = nombrecompleto 'Nombre
                        hoja4.Cell(filaExcel, 3).Value = CDbl(dtgDatos.Rows(x).Cells(68).Value) ' SUBSIDIO IMPORTE
                        hoja4.Cell(filaExcel, 4).Value = CDbl(dtgDatos.Rows(x).Cells(69).Value) ' SUBSIDIO CUSADO

                        filaExcel = filaExcel + 1

                    End If

                Next

                'Se guarda

                pnlProgreso.Visible = False
                pnlCatalogo.Enabled = True

                dialogo.FileName = "LOTE TIMBRADO " & EmpresaN.ToUpper & " " & cboperiodo.SelectedIndex + 1 & IIf(idias = "15", "Q ", " SEM ") & periodo
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

    Private Sub dtgDatos_CellMouseClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dtgDatos.CellMouseClick

        dtgDatos.Rows(e.RowIndex).Selected = True

    End Sub


    Private Function registropatronal(empresa As String) As String
        Select Case empresa
            Case "ADEMSA"
                Return "Y5013112106"
            Case "Almacenadora"
                Return "Y5059744101"
            Case "IDN"
                Return "M5311355104"
            Case "Transportacion"
                Return "Y6470199107"
            Case "TMMDC"
                Return "A1128963101"
            Case "Logistic"
                Return "Y6440770102"
            Case "Logistic Q"
                Return "Y6440770102"

        End Select

    End Function


    Private Sub cmdsoloisr_Click(sender As System.Object, e As System.EventArgs) Handles cmdsoloisr.Click
        Try
            Dim ValorIncapacidad As Double
            Dim SUELDOBRUTON As Double
            Dim SEPTIMO As Double
            Dim PRIDOMGRAVADA As Double
            Dim PRIDOMEXENTA As Double
            Dim TE2G As Double
            Dim TE2E As Double
            Dim TE3 As Double
            Dim DESCANSOLABORADO As Double
            Dim FESTIVOTRAB As Double
            Dim BONOASISTENCIA As Double
            Dim BONOPRODUCTIVIDAD As Double
            Dim BONOPOLIVALENCIA As Double
            Dim BONOESPECIALIDAD As Double
            Dim BONOCALIDAD As Double
            Dim COMPENSACION As Double
            Dim SEMANAFONDO As Double
            Dim INCREMENTORETENIDO As Double
            Dim VACACIONESPRO As Double
            Dim AGUINALDOGRA As Double
            Dim AGUINALDOEXEN As Double
            Dim PRIMAVACGRA As Double
            Dim PRIMAVACEXEN As Double
            Dim SUMAPERCEPCIONES As Double
            Dim SUMAPERCEPCIONESPISR As Double
            Dim FINJUSTIFICADA As Double
            Dim PERMISOSINGOCEDESUELDO As Double
            Dim PRIMADOMINICAL As Double
            Dim SDEMPLEADO As Double

            Dim DiasCadaPeriodo As Integer
            Dim FechaInicioPeriodo As Date
            Dim FechaFinPeriodo As Date
            Dim FechaAntiguedad As Date
            Dim FechaBuscar As Date
            Dim TipoPeriodoinfoonavit As Integer

            Dim INCAPACIDADD As Double
            Dim ISRD As Double
            Dim IMMSSD As Double
            Dim INFONAVITD As Double
            Dim INFOBIMANT As Double
            Dim AJUSTEINFO As Double
            Dim PENSIONAD As Double
            Dim PRESTAMOD As Double
            Dim FONACOTD As Double
            Dim TNOLABORADOD As Double
            Dim CUOTASINDICALD As Double
            Dim SUBSIDIOG As Double
            Dim SUBSIDIOA As Double
            Dim SUMADEDUCCIONES As Double
            Dim dias As Integer
            Dim BanPeriodo As Boolean

            pnlProgreso.Visible = True

            Application.DoEvents()
            pnlCatalogo.Enabled = False
            pgbProgreso.Minimum = 0
            pgbProgreso.Value = 0
            pgbProgreso.Maximum = dtgDatos.Rows.Count



            For x As Integer = 0 To dtgDatos.Rows.Count - 1

                sql = "select * from periodos where iIdPeriodo= " & cboperiodo.SelectedValue
                Dim rwPeriodo As DataRow() = nConsulta(sql)

                If rwPeriodo Is Nothing = False Then
                    FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))

                    FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                    sql = "select *"
                    sql &= " from empleadosC"
                    sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & dtgDatos.Rows(x).Cells(2).Value

                    Dim rwDatosBanco As DataRow() = nConsulta(sql)


                    If rwDatosBanco Is Nothing = False Then
                        FechaAntiguedad = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                        FechaBuscar = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))
                        If FechaBuscar.CompareTo(FechaInicioPeriodo) > 0 And FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then
                            'Estamos dentro del rango 
                            'Calculamos la prima

                            dias = (DateDiff("y", FechaBuscar, FechaFinPeriodo)) + 1

                            BanPeriodo = True

                        ElseIf FechaBuscar.CompareTo(FechaFinPeriodo) <= 0 Then


                            BanPeriodo = False

                        End If
                    End If

                End If

                'Incapacidad
                ValorIncapacidad = 0.0
                If dtgDatos.Rows(x).Cells(27).Value <> "Ninguno" Then

                    ValorIncapacidad = dtgDatos.Rows(x).Cells(28).Value * SDEMPLEADO

                End If

                SUELDOBRUTON = Double.Parse(IIf(dtgDatos.Rows(x).Cells(29).Value = "", 0, dtgDatos.Rows(x).Cells(29).Value))
                SEPTIMO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(30).Value = "", 0, dtgDatos.Rows(x).Cells(30).Value))
                PRIDOMGRAVADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(31).Value = "", 0, dtgDatos.Rows(x).Cells(31).Value))
                PRIDOMEXENTA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(32).Value = "", 0, dtgDatos.Rows(x).Cells(32).Value))
                TE2G = Double.Parse(IIf(dtgDatos.Rows(x).Cells(33).Value = "", 0, dtgDatos.Rows(x).Cells(33).Value))
                TE2E = Double.Parse(IIf(dtgDatos.Rows(x).Cells(34).Value = "", 0, dtgDatos.Rows(x).Cells(34).Value))
                TE3 = Double.Parse(IIf(dtgDatos.Rows(x).Cells(35).Value = "", 0, dtgDatos.Rows(x).Cells(35).Value))
                DESCANSOLABORADO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(36).Value = "", 0, dtgDatos.Rows(x).Cells(36).Value))
                FESTIVOTRAB = Double.Parse(IIf(dtgDatos.Rows(x).Cells(37).Value = "", 0, dtgDatos.Rows(x).Cells(37).Value))
                BONOASISTENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(38).Value = "", 0, dtgDatos.Rows(x).Cells(38).Value))
                BONOPRODUCTIVIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(39).Value = "", 0, dtgDatos.Rows(x).Cells(39).Value))
                BONOPOLIVALENCIA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(40).Value = "", 0, dtgDatos.Rows(x).Cells(40).Value))
                BONOESPECIALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(41).Value = "", 0, dtgDatos.Rows(x).Cells(41).Value))
                BONOCALIDAD = Double.Parse(IIf(dtgDatos.Rows(x).Cells(42).Value = "", 0, dtgDatos.Rows(x).Cells(42).Value))
                COMPENSACION = Double.Parse(IIf(dtgDatos.Rows(x).Cells(43).Value = "", 0, dtgDatos.Rows(x).Cells(43).Value))
                SEMANAFONDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(44).Value = "", 0, dtgDatos.Rows(x).Cells(44).Value))
                FINJUSTIFICADA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(45).Value = "", 0, dtgDatos.Rows(x).Cells(45).Value))
                PERMISOSINGOCEDESUELDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(46).Value = "", 0, dtgDatos.Rows(x).Cells(46).Value))
                INCREMENTORETENIDO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(47).Value = "", 0, dtgDatos.Rows(x).Cells(47).Value))
                VACACIONESPRO = Double.Parse(IIf(dtgDatos.Rows(x).Cells(48).Value = "", 0, dtgDatos.Rows(x).Cells(48).Value))
                AGUINALDOGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(49).Value = "", 0, dtgDatos.Rows(x).Cells(49).Value))
                AGUINALDOEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(50).Value = "", 0, dtgDatos.Rows(x).Cells(50).Value))
                PRIMAVACGRA = Double.Parse(IIf(dtgDatos.Rows(x).Cells(52).Value = "", 0, dtgDatos.Rows(x).Cells(52).Value))
                PRIMAVACEXEN = Double.Parse(IIf(dtgDatos.Rows(x).Cells(53).Value = "", 0, dtgDatos.Rows(x).Cells(53).Value))
                SUMAPERCEPCIONES = SUELDOBRUTON + SEPTIMO + PRIDOMGRAVADA + PRIDOMEXENTA + TE2G + TE2E + TE3 + DESCANSOLABORADO + FESTIVOTRAB
                SUMAPERCEPCIONES = SUMAPERCEPCIONES + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                SUMAPERCEPCIONES = SUMAPERCEPCIONES + FINJUSTIFICADA + PERMISOSINGOCEDESUELDO + INCREMENTORETENIDO + VACACIONESPRO + AGUINALDOGRA + AGUINALDOEXEN
                SUMAPERCEPCIONES = SUMAPERCEPCIONES + PRIMAVACGRA + PRIMAVACEXEN - ValorIncapacidad
                dtgDatos.Rows(x).Cells(55).Value = Math.Round(SUMAPERCEPCIONES, 2).ToString("###,##0.00")
                SUMAPERCEPCIONESPISR = SUMAPERCEPCIONES - PRIDOMEXENTA - TE2E - AGUINALDOEXEN - PRIMAVACEXEN


                SDEMPLEADO = Double.Parse(dtgDatos.Rows(x).Cells(24).Value)


                Dim ADICIONALES As Double = PRIDOMGRAVADA + TE2G + TE3 + DESCANSOLABORADO + FESTIVOTRAB + BONOASISTENCIA + BONOPRODUCTIVIDAD + BONOPOLIVALENCIA + BONOESPECIALIDAD + BONOCALIDAD + COMPENSACION + SEMANAFONDO
                ADICIONALES = ADICIONALES + VACACIONESPRO + AGUINALDOGRA + PRIMAVACGRA
                'ISR
                If DiasCadaPeriodo = 7 Then
                    TipoPeriodoinfoonavit = 3
                    dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                ElseIf DiasCadaPeriodo = 15 Or DiasCadaPeriodo = 16 Or DiasCadaPeriodo = 13 Or DiasCadaPeriodo = 14 Then
                    TipoPeriodoinfoonavit = 2
                    If EmpresaN = "NOSEOCUPARA" Then
                        Dim diastra As Double = Double.Parse(dtgDatos.Rows(x).Cells(26).Value)
                        Dim incapa As Double = Double.Parse(dtgDatos.Rows(x).Cells(28).Value)
                        Dim falta As Double = Double.Parse(dtgDatos.Rows(x).Cells(20).Value)
                        Dim permiso As Double = Double.Parse(dtgDatos.Rows(x).Cells(21).Value)
                        Dim ISRT As Double = Double.Parse(isrmontodadosinsubsidio(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                        Dim Subsidioaparte As Double = Double.Parse(subsidiocalculomensual(SDEMPLEADO * 30, 1, x) / 30 * (diastra - incapa - falta - permiso))
                        If dtgDatos.Rows(x).Cells(2).Value = "58" Then
                            MsgBox("llego")

                        End If
                        If Subsidioaparte > ISRT Then

                            dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte)).ToString("###,##0.00")
                            If Subsidioaparte > 0 Then
                                dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte - ISRT)).ToString("###,##0.00")
                            End If

                        Else
                            dtgDatos.Rows(x).Cells(68).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                            If Subsidioaparte > 0 Then
                                dtgDatos.Rows(x).Cells(69).Value = Math.Round(Double.Parse(Subsidioaparte), 2).ToString("###,##0.00")
                            Else
                                dtgDatos.Rows(x).Cells(69).Value = "0.00"
                            End If

                        End If


                        If ISRT > Subsidioaparte Then
                            ISRT = ISRT - Subsidioaparte
                        Else
                            ISRT = 0
                        End If

                        Dim ISRA As Double
                        ISRA = 0
                        If ADICIONALES > 0 Then
                            ISRA = Double.Parse(isrmontodadosinsubsidio(ADICIONALES, 1, x))
                        End If

                        dtgDatos.Rows(x).Cells(58).Value = Math.Round(ISRT + ISRA, 2).ToString("###,##0.00")
                    Else
                        'todos menos ademsa
                        dtgDatos.Rows(x).Cells(58).Value = Math.Round(Double.Parse(isrmontodado(SUMAPERCEPCIONESPISR, TipoPeriodoinfoonavit, x)), 2).ToString("###,##0.00")
                    End If






                Else
                    TipoPeriodoinfoonavit = 1
                End If

                pgbProgreso.Value += 1
                Application.DoEvents()
            Next
            pnlProgreso.Visible = False
            pnlCatalogo.Enabled = True
            MessageBox.Show("Calculo terminado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub




    Private Sub btnAcumualdos_Click(sender As System.Object, e As System.EventArgs) Handles btnAcumualdos.Click

        Try
            Dim Forma As New SeleccionarPeriodo

            If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim filaExcel As Integer = 0
                Dim dialogo As New SaveFileDialog()
                Dim mes, periodo As String
                Dim fecha, periodom, iejercicio, idias As String
                Dim pilotin As Boolean
                Dim rwUsuario As DataRow() = nConsulta("Select * from Usuarios where idUsuario=1")
                Dim tiponomina, sueldodescanso As String
                Dim filaexcelnomtotal As Integer = 0
                Dim valesDespensa As String

                'dias prov
                Dim DiasCadaPeriodo As Integer
                Dim FechaInicioPeriodo As Date
                Dim FechaFinPeriodo As Date
                Dim FechaAntiguedad As Date
                Dim FechaBuscar As Date
                Dim TipoPeriodoinfoonavit As Integer
                Dim tipoperiodos2 As String
                Dim ValorUMA As Double
                pnlProgreso.Visible = True
                pnlCatalogo.Enabled = False
                Application.DoEvents()




                Dim ruta As String
                ruta = My.Application.Info.DirectoryPath() & "\Archivos\concentradonomina.xlsx"
                Dim book As New ClosedXML.Excel.XLWorkbook(ruta)
                Dim libro As New ClosedXML.Excel.XLWorkbook

                book.Worksheet(1).CopyTo(libro, "NOMINA")
                Dim hoja As IXLWorksheet = libro.Worksheets(0)



                sql = "	select"
                sql &= " iIdEmpleadoC, fkiIdPeriodo,"
                sql &= " EmpleadosC.cCodigoEmpleado, EmpleadosC.clabe2, "
                sql &= " EmpleadosC.cNombreLargo,"
                sql &= " Nomina.Depto, Nomina.Puesto,"
                sql &= " EmpleadosC.cRFC, EmpleadosC.cCURP, EmpleadosC.cIMSS, EmpleadosC.dFechaAntiguedad, EmpleadosC.clabe, "
                sql &= "fTExtra2V, "
                sql &= "fTExtra3V, "
                sql &= "fDescansoLV , "
                sql &= "fDiaFestivoLV , "
                sql &= "fHoras_extras_dobles_V , "
                sql &= "fHoras_extras_triples_V , "
                sql &= "fDescanso_Laborado_V , "
                sql &= "fDia_Festivo_laborado_V , "
                sql &= "fPrima_Dominical_V , "
                sql &= "fFalta_Injustificada_V, "
                sql &= "fPermiso_Sin_GS_V , "
                sql &= "fT_No_laborado_V , "
                sql &= "fSalarioBase , "
                sql &= "fSalarioDiario, "
                sql &= "fSalarioBC , "
                sql &= "iDiasTrabajados , "
                sql &= "fSueldoBruto, "
                sql &= "fSeptimoDia, "
                sql &= "fPrimaDomGravada, "
                sql &= "fPrimaDomExenta, "
                sql &= "fTExtra2Gravado, "
                sql &= "fTExtra2Exento, "
                sql &= "fTExtra3, "
                sql &= "fDescansoL, "
                sql &= "fDiaFestivoL, "
                sql &= "fBonoAsistencia, "
                sql &= "fBonoProductividad, "
                sql &= "fBonoPolivalencia, "
                sql &= "fBonoEspecialidad, "
                sql &= "fBonoCalidad, "
                sql &= "fCompensacion, "
                sql &= "fSemanaFondo, "
                sql &= "fFaltaInjustificada, "
                sql &= "fPermisoSinGS, "
                sql &= "fIncrementoRetenido, "
                sql &= "fVacacionesProporcionales, "
                sql &= "fAguinaldoGravado, "
                sql &= "fAguinaldoExento, "
                sql &= "fPrimaVacacionalGravado, "
                sql &= "fPrimaVacacionalExento, "
                sql &= "fTotalPercepciones, "
                sql &= "fTotalPercepcionesISR, "
                sql &= "fIncapacidad, "
                sql &= "fIsr, "
                sql &= "fImss, "
                sql &= "fInfonavit, "
                sql &= "fInfonavitBanterior, "
                sql &= "fAjusteInfonavit, "
                sql &= "fPensionAlimenticia, "
                sql &= "fPrestamo, "
                sql &= "fFonacot, "
                sql &= "fT_No_laborado, "
                sql &= "fCuotaSindical, "
                sql &= "fSubsidioGenerado, "
                sql &= "fSubsidioAplicado, "
                sql &= "fOperadora AS NETO_SA, "
                sql &= "fPrestamoPerA, "
                sql &= "fAdeudoInfonavitA, "
                sql &= "fDiferenciaInfonavitA, "
                sql &= "fAsimilados AS EXCEDENTE, "
                sql &= "fRetencionOperadora AS fRetencion, "
                sql &= "fPorComision, "
                sql &= "fComisionOperadora, "
                sql &= "fComisionAsimilados, "
                sql &= "fImssCS, "
                sql &= "fRcvCS, "
                sql &= "fInfonavitCS, "
                sql &= "fInsCS, "
                sql &= "fTotalCostoSocial"
                sql &= " FROM	Nomina	inner	join	EmpleadosC	on	fkiIdEmpleadoC=iIdEmpleadoC"
                sql &= " where	Nomina.fkiIdEmpresa	=	1	And	fkiIdPeriodo between " & Forma.gInicial
                sql &= " and " & Forma.gFinal & " and	Nomina.iEstatus=1 and	iTipoNomina=0 AND iEstatusEmpleado=" & Forma.gSerie
                sql &= " ORDER	BY	fkiIdPeriodo,fkiIdEmpleadoC,	EmpleadosC.cCodigoEmpleado,	EmpleadosC.cNombreLargo, Nomina.Depto, Nomina.Puesto, "
                sql &= "EmpleadosC.cRFC, EmpleadosC.cCURP, EmpleadosC.cIMSS, EmpleadosC.dFechaAntiguedad, EmpleadosC.clabe"

                Dim rwFilas As DataRow() = nConsulta(sql)

                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = rwFilas.Count

                filaExcel = 2
                Dim pInicial, pFinal As Integer
                pInicial = Forma.gInicial
                pFinal = Forma.gFinal
                If rwFilas Is Nothing = False Then



                    For x As Integer = 0 To rwFilas.Count - 1

                        'PERIODOS para PROV
                        Dim rwPeriodo As DataRow() = nConsulta("select * from periodos where iIdPeriodo= " & rwFilas(x).Item("fkiIdPeriodo"))
                        FechaInicioPeriodo = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                        FechaFinPeriodo = Date.Parse(rwPeriodo(0)("dFechaFin"))
                        DiasCadaPeriodo = DateDiff(DateInterval.Day, FechaInicioPeriodo, FechaFinPeriodo) + 1

                       
                        'fechasperioodo
                        Dim rwPeriodo0 As DataRow() = nConsulta("Select * from periodos where iIdPeriodo=" & rwFilas(x).Item("fkiIdPeriodo"))
                        If rwPeriodo0 Is Nothing = False Then
                            periodo = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " DE " & (rwPeriodo0(0).Item("iEjercicio"))
                            mes = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper
                            iejercicio = rwPeriodo0(0).Item("iEjercicio")
                            idias = rwPeriodo0(0).Item("iDiasPago")
                            periodom = MonthString(rwPeriodo0(0).Item("iMes")).ToUpper & " " & (rwPeriodo0(0).Item("iEjercicio"))
                            tipoperiodos2 = rwPeriodo0(0).Item("fkiIdTipoPeriodo")
                        End If

                        'UMAS
                        Dim rwValorUMA As DataRow() = nConsulta("select * from Salario where Anio=" & aniocostosocial & " and iEstatus=1")
                        If rwValorUMA Is Nothing = False Then
                            If rwFilas(x).Item("fkiIdPeriodo") = 1 Then
                                ValorUMA = 96.22
                            Else
                                ValorUMA = Double.Parse(rwValorUMA(0)("uma").ToString)
                            End If
                        Else
                            ValorUMA = 0
                            MessageBox.Show("No se encontro valor para UMA en el año: " & aniocostosocial)
                        End If

                        'VALES DE DESPEMSA 
                        Dim numperiodo As Integer = cboperiodo.SelectedValue

                        If validarSiSeCalculanVales(EmpresaN, tipoperiodos2) Then
                            If tipoperiodos2 = 2 Then
                                If rwFilas(x).Item("fkiIdPeriodo") Mod 2 = 0 Then
                                    valesDespensa = 0
                                Else
                                    valesDespensa = "=ROUNDUP(IF((AD" & filaExcel + x & "*9%)>=3153.70,3153.70,(AD" & filaExcel + x & "*9%)),0)" 'VALES

                                End If

                            ElseIf tipoperiodos2 = 3 Then
                                If rwFilas(x).Item("fkiIdPeriodo") Mod 4 = 0 Then
                                    valesDespensa = "=ROUNDUP(IF((AD" & filaExcel + x & "*9%)>=3153.70,3153.70,(AD" & filaExcel + x & "*9%)),0)" 'VALES
                                Else
                                    valesDespensa = 0
                                End If
                            Else
                                valesDespensa = 0
                            End If

                        Else
                            valesDespensa = "0.0"
                        End If


                        hoja.Range("Q2", "Q" & rwFilas.Count + 5).Style.NumberFormat.Format = "@"
                        hoja.Range("D2", "D" & rwFilas.Count + 5).Style.NumberFormat.Format = "@"
                        hoja.Range("M2", "M" & rwFilas.Count + 5).Style.NumberFormat.Format = "@"
                        Dim deduccionestotal As Double = CDbl(rwFilas(x).Item("fIsr")) + CDbl(rwFilas(x).Item("fInfonavit")) + CDbl(rwFilas(x).Item("fInfonavitBanterior")) + CDbl(rwFilas(x).Item("fPensionAlimenticia")) + CDbl(rwFilas(x).Item("fPrestamo")) + CDbl(rwFilas(x).Item("fT_No_laborado")) + CDbl(rwFilas(x).Item("fCuotaSindical"))
                        If rwFilas(x).Item("fkiIdPeriodo") = 2 Then
                            'MsgBox("lol")
                        End If
                        hoja.Cell(filaExcel + x, 1).Value = mes 'MES
                        hoja.Cell(filaExcel + x, 2).Value = rwFilas(x).Item("fkiIdPeriodo") 'PERIOD0
                        hoja.Cell(filaExcel + x, 3).Value = rwFilas(x).Item("cCodigoEmpleado")
                        hoja.Cell(filaExcel + x, 4).Value = rwFilas(x).Item("clabe2") 'ce co
                        hoja.Cell(filaExcel + x, 5).Value = rwFilas(x).Item("cNombreLargo")
                        hoja.Cell(filaExcel + x, 6).Value = EmpresaN
                        hoja.Cell(filaExcel + x, 7).Value = "LIMITADO"
                        hoja.Cell(filaExcel + x, 8).Value = rwFilas(x).Item("Puesto")
                        hoja.Cell(filaExcel + x, 9).Value = rwFilas(x).Item("Depto")
                        hoja.Cell(filaExcel + x, 10).Value = rwFilas(x).Item("fkiIdPeriodo")
                        hoja.Cell(filaExcel + x, 11).Value = "OTRO"
                        hoja.Cell(filaExcel + x, 12).Value = rwFilas(x).Item("cRFC")
                        hoja.Cell(filaExcel + x, 13).Value = rwFilas(x).Item("cIMSS")
                        hoja.Cell(filaExcel + x, 14).Value = LTrim(RTrim(rwFilas(x).Item("cCURP")))
                        hoja.Cell(filaExcel + x, 15).Value = rwFilas(x).Item("dFechaAntiguedad")
                        hoja.Cell(filaExcel + x, 16).FormulaA1 = "=LEFT(Q" & filaExcel + x & ",3)"
                        hoja.Cell(filaExcel + x, 17).Value = rwFilas(x).Item("clabe")
                        hoja.Cell(filaExcel + x, 18).Value = rwFilas(x).Item("fTExtra2V")
                        hoja.Cell(filaExcel + x, 19).Value = rwFilas(x).Item("fTExtra3V")
                        hoja.Cell(filaExcel + x, 20).Value = rwFilas(x).Item("fDescansoLV")
                        hoja.Cell(filaExcel + x, 21).Value = rwFilas(x).Item("fDiaFestivoLV")
                        hoja.Cell(filaExcel + x, 22).Value = rwFilas(x).Item("fHoras_extras_dobles_V")
                        hoja.Cell(filaExcel + x, 23).Value = rwFilas(x).Item("fHoras_extras_triples_V")
                        hoja.Cell(filaExcel + x, 24).Value = rwFilas(x).Item("fDescanso_Laborado_V")
                        hoja.Cell(filaExcel + x, 25).Value = rwFilas(x).Item("fDia_Festivo_laborado_V")
                        hoja.Cell(filaExcel + x, 26).Value = rwFilas(x).Item("fPrima_Dominical_V")
                        hoja.Cell(filaExcel + x, 27).Value = rwFilas(x).Item("fFalta_Injustificada_V")
                        hoja.Cell(filaExcel + x, 28).Value = rwFilas(x).Item("fPermiso_Sin_GS_V")
                        hoja.Cell(filaExcel + x, 29).Value = rwFilas(x).Item("fT_No_laborado_V")
                        hoja.Cell(filaExcel + x, 30).Value = rwFilas(x).Item("fSalarioBase")
                        hoja.Cell(filaExcel + x, 31).Value = rwFilas(x).Item("fSalarioDiario")
                        hoja.Cell(filaExcel + x, 32).Value = rwFilas(x).Item("fSalarioBC")
                        hoja.Cell(filaExcel + x, 33).Value = rwFilas(x).Item("iDiasTrabajados") 'dias trabajsdos
                        hoja.Cell(filaExcel + x, 34).Value = rwFilas(x).Item("fSueldoBruto")
                        hoja.Cell(filaExcel + x, 35).Value = rwFilas(x).Item("fSeptimoDia")
                        hoja.Cell(filaExcel + x, 36).Value = rwFilas(x).Item("fPrimaDomGravada")
                        hoja.Cell(filaExcel + x, 37).Value = rwFilas(x).Item("fPrimaDomExenta")
                        hoja.Cell(filaExcel + x, 38).Value = rwFilas(x).Item("fTExtra2Gravado")
                        hoja.Cell(filaExcel + x, 39).Value = rwFilas(x).Item("fTExtra2Exento")
                        hoja.Cell(filaExcel + x, 40).Value = rwFilas(x).Item("fTExtra3")
                        hoja.Cell(filaExcel + x, 41).Value = rwFilas(x).Item("fDescansoL")
                        hoja.Cell(filaExcel + x, 42).Value = rwFilas(x).Item("fDiaFestivoL")
                        hoja.Cell(filaExcel + x, 43).Value = rwFilas(x).Item("fBonoAsistencia")
                        hoja.Cell(filaExcel + x, 44).Value = rwFilas(x).Item("fBonoProductividad")
                        hoja.Cell(filaExcel + x, 45).Value = rwFilas(x).Item("fBonoPolivalencia")
                        hoja.Cell(filaExcel + x, 46).Value = rwFilas(x).Item("fBonoEspecialidad")
                        hoja.Cell(filaExcel + x, 47).Value = rwFilas(x).Item("fBonoCalidad")
                        hoja.Cell(filaExcel + x, 48).Value = rwFilas(x).Item("fCompensacion")
                        hoja.Cell(filaExcel + x, 49).Value = rwFilas(x).Item("fSemanaFondo")
                        hoja.Cell(filaExcel + x, 50).Value = rwFilas(x).Item("fFaltaInjustificada")
                        hoja.Cell(filaExcel + x, 51).Value = rwFilas(x).Item("fPermisoSinGS")
                        hoja.Cell(filaExcel + x, 52).Value = rwFilas(x).Item("fIncrementoRetenido")
                        hoja.Cell(filaExcel + x, 53).Value = rwFilas(x).Item("fVacacionesProporcionales")
                        hoja.Cell(filaExcel + x, 54).Value = rwFilas(x).Item("fAguinaldoGravado")
                        hoja.Cell(filaExcel + x, 55).Value = rwFilas(x).Item("fAguinaldoExento")
                        hoja.Cell(filaExcel + x, 56).Value = rwFilas(x).Item("fPrimaVacacionalGravado")
                        hoja.Cell(filaExcel + x, 57).Value = rwFilas(x).Item("fPrimaVacacionalExento")
                        hoja.Cell(filaExcel + x, 58).Value = rwFilas(x).Item("fTotalPercepciones")
                        hoja.Cell(filaExcel + x, 59).Value = rwFilas(x).Item("fTotalPercepcionesISR")
                        hoja.Cell(filaExcel + x, 60).Value = CDbl(rwFilas(x).Item("fTotalPercepciones")) - CDbl(rwFilas(x).Item("fTotalPercepcionesISR")) 'total percep no grava		
                        hoja.Cell(filaExcel + x, 61).Value = rwFilas(x).Item("fIncapacidad")
                        hoja.Cell(filaExcel + x, 62).Value = rwFilas(x).Item("fIsr")
                        hoja.Cell(filaExcel + x, 63).Value = rwFilas(x).Item("fImss")
                        hoja.Cell(filaExcel + x, 64).Value = rwFilas(x).Item("fInfonavit")
                        hoja.Cell(filaExcel + x, 65).Value = rwFilas(x).Item("fInfonavitBanterior")
                        hoja.Cell(filaExcel + x, 66).Value = rwFilas(x).Item("fAjusteInfonavit")
                        hoja.Cell(filaExcel + x, 67).Value = rwFilas(x).Item("fPensionAlimenticia")
                        hoja.Cell(filaExcel + x, 68).Value = rwFilas(x).Item("fPrestamo")
                        hoja.Cell(filaExcel + x, 69).Value = rwFilas(x).Item("fFonacot")
                        hoja.Cell(filaExcel + x, 70).Value = rwFilas(x).Item("fT_No_laborado")
                        hoja.Cell(filaExcel + x, 71).Value = rwFilas(x).Item("fCuotaSindical")
                        hoja.Cell(filaExcel + x, 72).Value = rwFilas(x).Item("fSubsidioGenerado")
                        hoja.Cell(filaExcel + x, 73).Value = rwFilas(x).Item("fSubsidioAplicado")
                        hoja.Cell(filaExcel + x, 74).Value = deduccionestotal
                        hoja.Cell(filaExcel + x, 75).Value = rwFilas(x).Item("NETO_SA")
                        hoja.Cell(filaExcel + x, 76).Value = 0 ' rwFilas(x).Item("fPrestamoPerA") 'FONDO PFB 3%
                        hoja.Cell(filaExcel + x, 77).FormulaA1 = valesDespensa 'rwFilas(x).Item("fAdeudoInfonavitA")
                        hoja.Cell(filaExcel + x, 78).Value = IIf(CDbl(rwFilas(x).Item("fSalarioBase")) > 40000, "PPP", "SIND")  ' rwFilas(x).Item("fDiferenciaInfonavitA")'ppp/sind
                        hoja.Cell(filaExcel + x, 79).Value = IIf(CDbl(rwFilas(x).Item("fSalarioBase")) > 40000, rwFilas(x).Item("EXCEDENTE"), 0)
                        hoja.Cell(filaExcel + x, 80).FormulaA1 = IIf(tipoperiodos2 = 2, "=IF(BZ" & filaExcel + x & "=""PPP"",((AF" & filaExcel + x & "/1.0493)*15.2)*0.03,0)", "NA") 'rwFilas(x).Item("fRetencion")
                        hoja.Cell(filaExcel + x, 81).Value = 0 'rwFilas(x).Item("fPorComision")
                        hoja.Cell(filaExcel + x, 82).Value = 0 'rwFilas(x).Item("fComisionOperadora")
                        hoja.Cell(filaExcel + x, 83).Value = 0 'rwFilas(x).Item("fComisionAsimilados")
                        hoja.Cell(filaExcel + x, 84).Value = rwFilas(x).Item("fImssCS")
                        hoja.Cell(filaExcel + x, 85).FormulaA1 = "=+CF" & filaExcel + x & "+CI" & filaExcel + x 'cuota imss
                        hoja.Cell(filaExcel + x, 86).Value = Math.Round(calculoimss(validarTopeSalarioBC(rwFilas(x).Item("fSalarioBC"), mes), rwFilas(x).Item("fTotalPercepciones"), 6, ValorUMA, DiasCadaPeriodo, 3), 2) '"2%  SAR RETIRO 
                        hoja.Cell(filaExcel + x, 87).Value = Math.Round(calculoimss(validarTopeSalarioBC(rwFilas(x).Item("fSalarioBC"), mes), rwFilas(x).Item("fTotalPercepciones"), 7, ValorUMA, DiasCadaPeriodo, 3), 2)  'VEJEZ PROP
                        hoja.Cell(filaExcel + x, 88).Value = rwFilas(x).Item("fRcvCS")
                        hoja.Cell(filaExcel + x, 89).Value = rwFilas(x).Item("fInfonavitCS")
                        hoja.Cell(filaExcel + x, 90).Value = rwFilas(x).Item("fInsCS")
                        hoja.Cell(filaExcel + x, 91).FormulaA1 = "=+CF" & filaExcel + x & "+CJ" & filaExcel + x & "+Ck" & filaExcel + x & "+CL" & filaExcel + x
                        hoja.Cell(filaExcel + x, 92).Value = rwFilas(x).Item("fTotalCostoSocial")
                        hoja.Cell(filaExcel + x, 93).FormulaA1 = IIf(EmpresaN = "IDN", "=((AE" & filaExcel + x & "*15)/365)*AG" & filaExcel + x, "=((AE" & filaExcel + x & "*30)/365)*AG" & filaExcel + x) 'pro agui

                        If tipoperiodos2 = 2 Then

                            hoja.Cell(filaExcel + x, 94).Value = Math.Round(Double.Parse(((CalculoPrimaPROV(rwFilas(x).Item("iIdEmpleadoC"), 1, 50, CDbl(rwFilas(x).Item("fSalarioDiario")), rwFilas(x).Item("fkiIdPeriodo"))) / 365) * CDbl(rwFilas(x).Item("iDiasTrabajados"))), 2) 'pro prima
                            hoja.Cell(filaExcel + x, 95).Value = 0.0 'imss pro
                            hoja.Cell(filaExcel + x, 96).Value = 0.0 'pro cyv pa
                            hoja.Cell(filaExcel + x, 97).Value = 0.0 ' ret pro
                            hoja.Cell(filaExcel + x, 98).Value = 0.0 'provision ?
                            hoja.Cell(filaExcel + x, 99).Value = 0.0 'PRO ISN
                            hoja.Cell(filaExcel + x, 100).Value = 0.0 'pro PE NOD

                        ElseIf tipoperiodos2 = 3 Then

                            hoja.Cell(filaExcel + x, 94).Value = Math.Round(Double.Parse(((CalculoPrimaPROV(rwFilas(x).Item("iIdEmpleadoC"), 1, 25, CDbl(rwFilas(x).Item("fSalarioDiario")), rwFilas(x).Item("fkiIdPeriodo"))) / 365) * CDbl(rwFilas(x).Item("iDiasTrabajados"))), 2) 'pro prima
                            hoja.Cell(filaExcel + x, 95).Value = 0.0 'imss pro
                            hoja.Cell(filaExcel + x, 96).Value = 0.0 'pro cyv pa
                            hoja.Cell(filaExcel + x, 97).Value = 0.0 ' ret pro
                            hoja.Cell(filaExcel + x, 98).Value = 0.0 'provision ?
                            hoja.Cell(filaExcel + x, 99).Value = 0.0 'PRO ISN
                            hoja.Cell(filaExcel + x, 100).Value = 0.0 'pro PE NOD
                        End If

                        pgbProgreso.Value = x
                    Next x


                    '<<<<<CARGAR>>>>>
                    pnlProgreso.Visible = False
                    pnlCatalogo.Enabled = True

                    '<<<<<<<<<<<<<<<guardar>>>>>>>>>>>>>>>>

                    Dim textoperiodo As String
                    If tipoperiodos2 = 2 Then

                        textoperiodo = Forma.gInicial & "-" & Forma.gFinal & " Q "

                    ElseIf tipoperiodos2 = 3 Then

                        textoperiodo = "SEMANA " & Forma.gInicial & "-" & Forma.gFinal
                    End If


                    dialogo.FileName = "CONCENTRADO " & EmpresaN.ToUpper & " " & textoperiodo & periodo
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
                End If



            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        End Try

    End Sub

    Function CalculoPrimaPROV(ByVal idempleado As String, ByVal idempresa As String, porcentajeprima As Double, sd As Double, pSem As Integer) As String
        'Agregar el banco y el tipo de cuenta ya sea a terceros o interbancaria
        'Buscamos el banco y verificarmos el tipo de cuenta a tercero o interbancaria
        Dim Sql As String
        Dim cadenabanco As String
        Dim dia As String
        Dim mes As String
        Dim anio As String
        Dim anios As Integer
        Dim sueldodiario As Double
        Dim dias As Integer
        Dim BaseExento As Double
        Dim Excento As Double
        Dim gravado As Double
        Dim Prima As String


        cadenabanco = ""


        Sql = "select *"
        Sql &= " from empleadosC"
        Sql &= " where fkiIdEmpresa=" & gIdEmpresa & " and iIdempleadoC=" & idempleado

        Dim rwDatosBanco As DataRow() = nConsulta(Sql)

        cadenabanco = "@"
        Prima = "0"
        If rwDatosBanco Is Nothing = False Then

            If Double.Parse(rwDatosBanco(0)("fsueldoOrd")) > 0 Then
                dia = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Day.ToString("00")
                mes = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad").ToString).Month.ToString("00")
                anio = Date.Today.Year
                'verificar el periodo para saber si queda entre el rango de fecha

                sueldodiario = Double.Parse(rwDatosBanco(0)("fsueldoOrd")) / diasperiodo

                Sql = "select * from periodos where iIdPeriodo= " & pSem
                Dim rwPeriodo As DataRow() = nConsulta(Sql)

                If rwPeriodo Is Nothing = False Then
                    Dim FechaBuscar As Date = Date.Parse(dia & "/" & mes & "/" & anio)
                    Dim FechaInicial As Date = Date.Parse(rwPeriodo(0)("dFechaInicio"))
                    Dim FechaFinal As Date = Date.Parse(rwPeriodo(0)("dFechaFin"))
                    Dim FechaAntiguedad As Date = Date.Parse(rwDatosBanco(0)("dFechaAntiguedad"))

                    '  If FechaBuscar.CompareTo(FechaInicial) >= 0 And FechaBuscar.CompareTo(FechaFinal) <= 0 Then
                    'Estamos dentro del rango 
                    'Calculamos la prima

                    anios = DateDiff("yyyy", FechaAntiguedad, FechaBuscar)

                    dias = CalculoDiasVacaciones(anios)

                    'Calcular prima

                    Prima = Math.Round(sd * dias * (porcentajeprima / 100), 2).ToString()



                    'End If


                End If


            End If


        End If


        Return Prima


    End Function


    Private Sub EditarEmpleadoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Function validarTopeSalarioBC(fsalario As Double, mes As String) As Double
        Dim fsalarioBc As Double

        If mes = "ENERO" Then
            If EmpresaN <> "Transportacion" Then
                If CDbl(fsalario) > 2405.5 Then
                    fsalarioBc = 2405.5
                Else
                    fsalarioBc = fsalario
                End If
            Else
                fsalarioBc = fsalario
            End If


        Else
            If CDbl(fsalario) > 2593.5 Then
                fsalarioBc = 2593.5
            Else
                fsalarioBc = fsalario
            End If
        End If

            Return fsalarioBc
    End Function

End Class