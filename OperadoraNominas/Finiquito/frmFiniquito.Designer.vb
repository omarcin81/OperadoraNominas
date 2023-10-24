<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFiniquito
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFiniquito))
        Me.pnlCatalogo = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.dtpFechaBaja = New System.Windows.Forms.DateTimePicker()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.btnAgregar = New System.Windows.Forms.Button()
        Me.txtFonacot = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtInfonavitExce = New System.Windows.Forms.TextBox()
        Me.txtInfonavit = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtVales = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtBonosExce = New System.Windows.Forms.TextBox()
        Me.txtBonos = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtDLExce = New System.Windows.Forms.TextBox()
        Me.txtDL = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtIncidenciasExce = New System.Windows.Forms.TextBox()
        Me.txtIncidencias = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtHE3Exce = New System.Windows.Forms.TextBox()
        Me.txtHE3 = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtHE2Exce = New System.Windows.Forms.TextBox()
        Me.txtHE2 = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.cboSerie = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cboPeriodo = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cboEmpleado = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtSindicato = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtVacacionesPendientesExce = New System.Windows.Forms.TextBox()
        Me.txtPrimaVacacionalExce = New System.Windows.Forms.TextBox()
        Me.txtVacacionesPropExce = New System.Windows.Forms.TextBox()
        Me.txtAguinaldoExce = New System.Windows.Forms.TextBox()
        Me.txtSueldoExce = New System.Windows.Forms.TextBox()
        Me.txtAntiguedadExce = New System.Windows.Forms.TextBox()
        Me.txtindeminizacionExce = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSTP = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtISRIndemnizacion = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtISR = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtVacacionesPendientes = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtPrimaVacacional = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtVacacionesProp = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtAguinaldo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSueldo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPrimaAntiguedad = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtIndeminizacion = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlProgreso = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pgbProgreso = New System.Windows.Forms.ProgressBar()
        Me.lsvLista = New System.Windows.Forms.ListView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbGuardar = New System.Windows.Forms.ToolStripButton()
        Me.tsbNuevo = New System.Windows.Forms.ToolStripButton()
        Me.tsbImportar = New System.Windows.Forms.ToolStripButton()
        Me.tsbProcesar = New System.Windows.Forms.ToolStripButton()
        Me.tsbCancelar = New System.Windows.Forms.ToolStripButton()
        Me.lblRuta = New System.Windows.Forms.Label()
        Me.cmdCerrar = New System.Windows.Forms.Button()
        Me.pnlCatalogo.SuspendLayout()
        Me.pnlProgreso.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlCatalogo
        '
        Me.pnlCatalogo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlCatalogo.AutoScroll = True
        Me.pnlCatalogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCatalogo.Controls.Add(Me.Button1)
        Me.pnlCatalogo.Controls.Add(Me.dtpFechaBaja)
        Me.pnlCatalogo.Controls.Add(Me.Label26)
        Me.pnlCatalogo.Controls.Add(Me.cboStatus)
        Me.pnlCatalogo.Controls.Add(Me.Label24)
        Me.pnlCatalogo.Controls.Add(Me.btnAgregar)
        Me.pnlCatalogo.Controls.Add(Me.txtFonacot)
        Me.pnlCatalogo.Controls.Add(Me.Label25)
        Me.pnlCatalogo.Controls.Add(Me.txtInfonavitExce)
        Me.pnlCatalogo.Controls.Add(Me.txtInfonavit)
        Me.pnlCatalogo.Controls.Add(Me.Label23)
        Me.pnlCatalogo.Controls.Add(Me.txtVales)
        Me.pnlCatalogo.Controls.Add(Me.Label22)
        Me.pnlCatalogo.Controls.Add(Me.txtBonosExce)
        Me.pnlCatalogo.Controls.Add(Me.txtBonos)
        Me.pnlCatalogo.Controls.Add(Me.Label21)
        Me.pnlCatalogo.Controls.Add(Me.txtDLExce)
        Me.pnlCatalogo.Controls.Add(Me.txtDL)
        Me.pnlCatalogo.Controls.Add(Me.Label20)
        Me.pnlCatalogo.Controls.Add(Me.txtIncidenciasExce)
        Me.pnlCatalogo.Controls.Add(Me.txtIncidencias)
        Me.pnlCatalogo.Controls.Add(Me.Label19)
        Me.pnlCatalogo.Controls.Add(Me.txtHE3Exce)
        Me.pnlCatalogo.Controls.Add(Me.txtHE3)
        Me.pnlCatalogo.Controls.Add(Me.Label18)
        Me.pnlCatalogo.Controls.Add(Me.txtHE2Exce)
        Me.pnlCatalogo.Controls.Add(Me.txtHE2)
        Me.pnlCatalogo.Controls.Add(Me.Label17)
        Me.pnlCatalogo.Controls.Add(Me.cboSerie)
        Me.pnlCatalogo.Controls.Add(Me.Label16)
        Me.pnlCatalogo.Controls.Add(Me.cboPeriodo)
        Me.pnlCatalogo.Controls.Add(Me.Label15)
        Me.pnlCatalogo.Controls.Add(Me.cboEmpleado)
        Me.pnlCatalogo.Controls.Add(Me.Label14)
        Me.pnlCatalogo.Controls.Add(Me.txtSindicato)
        Me.pnlCatalogo.Controls.Add(Me.Label13)
        Me.pnlCatalogo.Controls.Add(Me.txtVacacionesPendientesExce)
        Me.pnlCatalogo.Controls.Add(Me.txtPrimaVacacionalExce)
        Me.pnlCatalogo.Controls.Add(Me.txtVacacionesPropExce)
        Me.pnlCatalogo.Controls.Add(Me.txtAguinaldoExce)
        Me.pnlCatalogo.Controls.Add(Me.txtSueldoExce)
        Me.pnlCatalogo.Controls.Add(Me.txtAntiguedadExce)
        Me.pnlCatalogo.Controls.Add(Me.txtindeminizacionExce)
        Me.pnlCatalogo.Controls.Add(Me.Label12)
        Me.pnlCatalogo.Controls.Add(Me.txtSTP)
        Me.pnlCatalogo.Controls.Add(Me.Label11)
        Me.pnlCatalogo.Controls.Add(Me.txtISRIndemnizacion)
        Me.pnlCatalogo.Controls.Add(Me.Label10)
        Me.pnlCatalogo.Controls.Add(Me.txtISR)
        Me.pnlCatalogo.Controls.Add(Me.Label9)
        Me.pnlCatalogo.Controls.Add(Me.txtVacacionesPendientes)
        Me.pnlCatalogo.Controls.Add(Me.Label8)
        Me.pnlCatalogo.Controls.Add(Me.txtPrimaVacacional)
        Me.pnlCatalogo.Controls.Add(Me.Label7)
        Me.pnlCatalogo.Controls.Add(Me.txtVacacionesProp)
        Me.pnlCatalogo.Controls.Add(Me.Label6)
        Me.pnlCatalogo.Controls.Add(Me.txtAguinaldo)
        Me.pnlCatalogo.Controls.Add(Me.Label5)
        Me.pnlCatalogo.Controls.Add(Me.txtSueldo)
        Me.pnlCatalogo.Controls.Add(Me.Label4)
        Me.pnlCatalogo.Controls.Add(Me.txtPrimaAntiguedad)
        Me.pnlCatalogo.Controls.Add(Me.Label3)
        Me.pnlCatalogo.Controls.Add(Me.txtIndeminizacion)
        Me.pnlCatalogo.Controls.Add(Me.Label1)
        Me.pnlCatalogo.Controls.Add(Me.pnlProgreso)
        Me.pnlCatalogo.Controls.Add(Me.lsvLista)
        Me.pnlCatalogo.Location = New System.Drawing.Point(0, 51)
        Me.pnlCatalogo.Name = "pnlCatalogo"
        Me.pnlCatalogo.Size = New System.Drawing.Size(1453, 698)
        Me.pnlCatalogo.TabIndex = 24
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(504, 35)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 92
        Me.Button1.Text = "Limpiar"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'dtpFechaBaja
        '
        Me.dtpFechaBaja.Location = New System.Drawing.Point(357, 64)
        Me.dtpFechaBaja.Name = "dtpFechaBaja"
        Me.dtpFechaBaja.Size = New System.Drawing.Size(175, 20)
        Me.dtpFechaBaja.TabIndex = 91
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(286, 64)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(64, 13)
        Me.Label26.TabIndex = 90
        Me.Label26.Text = "Fecha Baja:"
        '
        'cboStatus
        '
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Items.AddRange(New Object() {"PENDIENTE", "PAGADO"})
        Me.cboStatus.Location = New System.Drawing.Point(112, 61)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(148, 21)
        Me.cboStatus.TabIndex = 89
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(19, 64)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(87, 13)
        Me.Label24.TabIndex = 88
        Me.Label24.Text = "Estatus Finiquito:"
        '
        'btnAgregar
        '
        Me.btnAgregar.Location = New System.Drawing.Point(504, 4)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(75, 23)
        Me.btnAgregar.TabIndex = 87
        Me.btnAgregar.Text = "Agregar"
        Me.btnAgregar.UseVisualStyleBackColor = True
        '
        'txtFonacot
        '
        Me.txtFonacot.Location = New System.Drawing.Point(172, 510)
        Me.txtFonacot.Name = "txtFonacot"
        Me.txtFonacot.Size = New System.Drawing.Size(160, 20)
        Me.txtFonacot.TabIndex = 86
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(20, 513)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(61, 13)
        Me.Label25.TabIndex = 85
        Me.Label25.Text = "FONACOT:"
        '
        'txtInfonavitExce
        '
        Me.txtInfonavitExce.Location = New System.Drawing.Point(359, 483)
        Me.txtInfonavitExce.Name = "txtInfonavitExce"
        Me.txtInfonavitExce.Size = New System.Drawing.Size(160, 20)
        Me.txtInfonavitExce.TabIndex = 81
        '
        'txtInfonavit
        '
        Me.txtInfonavit.Location = New System.Drawing.Point(172, 484)
        Me.txtInfonavit.Name = "txtInfonavit"
        Me.txtInfonavit.Size = New System.Drawing.Size(160, 20)
        Me.txtInfonavit.TabIndex = 80
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(20, 487)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(67, 13)
        Me.Label23.TabIndex = 79
        Me.Label23.Text = "INFONAVIT:"
        '
        'txtVales
        '
        Me.txtVales.Location = New System.Drawing.Point(359, 533)
        Me.txtVales.Name = "txtVales"
        Me.txtVales.Size = New System.Drawing.Size(160, 20)
        Me.txtVales.TabIndex = 78
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(20, 533)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(36, 13)
        Me.Label22.TabIndex = 77
        Me.Label22.Text = "Vales:"
        '
        'txtBonosExce
        '
        Me.txtBonosExce.Location = New System.Drawing.Point(359, 428)
        Me.txtBonosExce.Name = "txtBonosExce"
        Me.txtBonosExce.Size = New System.Drawing.Size(160, 20)
        Me.txtBonosExce.TabIndex = 76
        '
        'txtBonos
        '
        Me.txtBonos.Location = New System.Drawing.Point(172, 430)
        Me.txtBonos.Name = "txtBonos"
        Me.txtBonos.Size = New System.Drawing.Size(160, 20)
        Me.txtBonos.TabIndex = 75
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(22, 437)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(126, 13)
        Me.Label21.TabIndex = 74
        Me.Label21.Text = "Bonos/Compensaciones:"
        '
        'txtDLExce
        '
        Me.txtDLExce.Location = New System.Drawing.Point(359, 402)
        Me.txtDLExce.Name = "txtDLExce"
        Me.txtDLExce.Size = New System.Drawing.Size(160, 20)
        Me.txtDLExce.TabIndex = 73
        '
        'txtDL
        '
        Me.txtDL.Location = New System.Drawing.Point(173, 402)
        Me.txtDL.Name = "txtDL"
        Me.txtDL.Size = New System.Drawing.Size(160, 20)
        Me.txtDL.TabIndex = 72
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(21, 405)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(106, 13)
        Me.Label20.TabIndex = 71
        Me.Label20.Text = "Descanso Laborado:"
        '
        'txtIncidenciasExce
        '
        Me.txtIncidenciasExce.Location = New System.Drawing.Point(359, 461)
        Me.txtIncidenciasExce.Name = "txtIncidenciasExce"
        Me.txtIncidenciasExce.Size = New System.Drawing.Size(160, 20)
        Me.txtIncidenciasExce.TabIndex = 70
        '
        'txtIncidencias
        '
        Me.txtIncidencias.Location = New System.Drawing.Point(172, 458)
        Me.txtIncidencias.Name = "txtIncidencias"
        Me.txtIncidencias.Size = New System.Drawing.Size(160, 20)
        Me.txtIncidencias.TabIndex = 69
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(20, 461)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(97, 13)
        Me.Label19.TabIndex = 68
        Me.Label19.Text = "Incidencias/Faltas:"
        '
        'txtHE3Exce
        '
        Me.txtHE3Exce.Location = New System.Drawing.Point(359, 376)
        Me.txtHE3Exce.Name = "txtHE3Exce"
        Me.txtHE3Exce.Size = New System.Drawing.Size(160, 20)
        Me.txtHE3Exce.TabIndex = 67
        '
        'txtHE3
        '
        Me.txtHE3.Location = New System.Drawing.Point(172, 376)
        Me.txtHE3.Name = "txtHE3"
        Me.txtHE3.Size = New System.Drawing.Size(160, 20)
        Me.txtHE3.TabIndex = 66
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(20, 379)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(104, 13)
        Me.Label18.TabIndex = 65
        Me.Label18.Text = "Horas Extras Triples:"
        '
        'txtHE2Exce
        '
        Me.txtHE2Exce.Location = New System.Drawing.Point(359, 349)
        Me.txtHE2Exce.Name = "txtHE2Exce"
        Me.txtHE2Exce.Size = New System.Drawing.Size(160, 20)
        Me.txtHE2Exce.TabIndex = 64
        '
        'txtHE2
        '
        Me.txtHE2.Location = New System.Drawing.Point(172, 349)
        Me.txtHE2.Name = "txtHE2"
        Me.txtHE2.Size = New System.Drawing.Size(160, 20)
        Me.txtHE2.TabIndex = 63
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(20, 352)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(106, 13)
        Me.Label17.TabIndex = 62
        Me.Label17.Text = "Horas Extras Dobles:"
        '
        'cboSerie
        '
        Me.cboSerie.FormattingEnabled = True
        Me.cboSerie.Items.AddRange(New Object() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "Y", "Z"})
        Me.cboSerie.Location = New System.Drawing.Point(365, 17)
        Me.cboSerie.Name = "cboSerie"
        Me.cboSerie.Size = New System.Drawing.Size(115, 21)
        Me.cboSerie.TabIndex = 61
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(328, 20)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(31, 13)
        Me.Label16.TabIndex = 60
        Me.Label16.Text = "Serie"
        '
        'cboPeriodo
        '
        Me.cboPeriodo.FormattingEnabled = True
        Me.cboPeriodo.Location = New System.Drawing.Point(56, 17)
        Me.cboPeriodo.Name = "cboPeriodo"
        Me.cboPeriodo.Size = New System.Drawing.Size(248, 21)
        Me.cboPeriodo.TabIndex = 59
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(13, 25)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 58
        Me.Label15.Text = "Periodo:"
        '
        'cboEmpleado
        '
        Me.cboEmpleado.FormattingEnabled = True
        Me.cboEmpleado.Location = New System.Drawing.Point(81, 102)
        Me.cboEmpleado.Name = "cboEmpleado"
        Me.cboEmpleado.Size = New System.Drawing.Size(451, 21)
        Me.cboEmpleado.TabIndex = 57
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(20, 105)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(58, 13)
        Me.Label14.TabIndex = 55
        Me.Label14.Text = "Trabajador"
        '
        'txtSindicato
        '
        Me.txtSindicato.Location = New System.Drawing.Point(359, 623)
        Me.txtSindicato.Name = "txtSindicato"
        Me.txtSindicato.Size = New System.Drawing.Size(160, 20)
        Me.txtSindicato.TabIndex = 54
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(412, 137)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(58, 13)
        Me.Label13.TabIndex = 53
        Me.Label13.Text = "Excedente"
        '
        'txtVacacionesPendientesExce
        '
        Me.txtVacacionesPendientesExce.Location = New System.Drawing.Point(359, 320)
        Me.txtVacacionesPendientesExce.Name = "txtVacacionesPendientesExce"
        Me.txtVacacionesPendientesExce.Size = New System.Drawing.Size(160, 20)
        Me.txtVacacionesPendientesExce.TabIndex = 52
        '
        'txtPrimaVacacionalExce
        '
        Me.txtPrimaVacacionalExce.Location = New System.Drawing.Point(359, 294)
        Me.txtPrimaVacacionalExce.Name = "txtPrimaVacacionalExce"
        Me.txtPrimaVacacionalExce.Size = New System.Drawing.Size(160, 20)
        Me.txtPrimaVacacionalExce.TabIndex = 51
        '
        'txtVacacionesPropExce
        '
        Me.txtVacacionesPropExce.Location = New System.Drawing.Point(359, 268)
        Me.txtVacacionesPropExce.Name = "txtVacacionesPropExce"
        Me.txtVacacionesPropExce.Size = New System.Drawing.Size(160, 20)
        Me.txtVacacionesPropExce.TabIndex = 50
        '
        'txtAguinaldoExce
        '
        Me.txtAguinaldoExce.Location = New System.Drawing.Point(359, 242)
        Me.txtAguinaldoExce.Name = "txtAguinaldoExce"
        Me.txtAguinaldoExce.Size = New System.Drawing.Size(160, 20)
        Me.txtAguinaldoExce.TabIndex = 49
        '
        'txtSueldoExce
        '
        Me.txtSueldoExce.Location = New System.Drawing.Point(359, 216)
        Me.txtSueldoExce.Name = "txtSueldoExce"
        Me.txtSueldoExce.Size = New System.Drawing.Size(160, 20)
        Me.txtSueldoExce.TabIndex = 48
        '
        'txtAntiguedadExce
        '
        Me.txtAntiguedadExce.Location = New System.Drawing.Point(359, 190)
        Me.txtAntiguedadExce.Name = "txtAntiguedadExce"
        Me.txtAntiguedadExce.Size = New System.Drawing.Size(160, 20)
        Me.txtAntiguedadExce.TabIndex = 47
        '
        'txtindeminizacionExce
        '
        Me.txtindeminizacionExce.Location = New System.Drawing.Point(359, 160)
        Me.txtindeminizacionExce.Name = "txtindeminizacionExce"
        Me.txtindeminizacionExce.Size = New System.Drawing.Size(160, 20)
        Me.txtindeminizacionExce.TabIndex = 46
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(229, 137)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(31, 13)
        Me.Label12.TabIndex = 45
        Me.Label12.Text = "STP:"
        '
        'txtSTP
        '
        Me.txtSTP.Location = New System.Drawing.Point(172, 623)
        Me.txtSTP.Name = "txtSTP"
        Me.txtSTP.Size = New System.Drawing.Size(160, 20)
        Me.txtSTP.TabIndex = 44
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(37, 630)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 13)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Total Deposito:"
        '
        'txtISRIndemnizacion
        '
        Me.txtISRIndemnizacion.Location = New System.Drawing.Point(172, 585)
        Me.txtISRIndemnizacion.Name = "txtISRIndemnizacion"
        Me.txtISRIndemnizacion.Size = New System.Drawing.Size(160, 20)
        Me.txtISRIndemnizacion.TabIndex = 42
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(21, 588)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(101, 13)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "ISR Indeminizacion:"
        '
        'txtISR
        '
        Me.txtISR.Location = New System.Drawing.Point(172, 559)
        Me.txtISR.Name = "txtISR"
        Me.txtISR.Size = New System.Drawing.Size(160, 20)
        Me.txtISR.TabIndex = 40
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(21, 555)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(28, 13)
        Me.Label9.TabIndex = 39
        Me.Label9.Text = "ISR:"
        '
        'txtVacacionesPendientes
        '
        Me.txtVacacionesPendientes.Location = New System.Drawing.Point(172, 320)
        Me.txtVacacionesPendientes.Name = "txtVacacionesPendientes"
        Me.txtVacacionesPendientes.Size = New System.Drawing.Size(160, 20)
        Me.txtVacacionesPendientes.TabIndex = 38
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 323)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(122, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Vacaciones Pendientes:"
        '
        'txtPrimaVacacional
        '
        Me.txtPrimaVacacional.Location = New System.Drawing.Point(172, 294)
        Me.txtPrimaVacacional.Name = "txtPrimaVacacional"
        Me.txtPrimaVacacional.Size = New System.Drawing.Size(160, 20)
        Me.txtPrimaVacacional.TabIndex = 36
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 297)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(117, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Prima Vacacional Prop:"
        '
        'txtVacacionesProp
        '
        Me.txtVacacionesProp.Location = New System.Drawing.Point(172, 268)
        Me.txtVacacionesProp.Name = "txtVacacionesProp"
        Me.txtVacacionesProp.Size = New System.Drawing.Size(160, 20)
        Me.txtVacacionesProp.TabIndex = 34
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 271)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(138, 13)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Vacaciones proporcionales:"
        '
        'txtAguinaldo
        '
        Me.txtAguinaldo.Location = New System.Drawing.Point(172, 242)
        Me.txtAguinaldo.Name = "txtAguinaldo"
        Me.txtAguinaldo.Size = New System.Drawing.Size(160, 20)
        Me.txtAguinaldo.TabIndex = 32
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 245)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(125, 13)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Proporcion de aguinaldo:"
        '
        'txtSueldo
        '
        Me.txtSueldo.Location = New System.Drawing.Point(172, 216)
        Me.txtSueldo.Name = "txtSueldo"
        Me.txtSueldo.Size = New System.Drawing.Size(160, 20)
        Me.txtSueldo.TabIndex = 30
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 219)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Sueldo:"
        '
        'txtPrimaAntiguedad
        '
        Me.txtPrimaAntiguedad.Location = New System.Drawing.Point(172, 190)
        Me.txtPrimaAntiguedad.Name = "txtPrimaAntiguedad"
        Me.txtPrimaAntiguedad.Size = New System.Drawing.Size(160, 20)
        Me.txtPrimaAntiguedad.TabIndex = 28
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 193)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(108, 13)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Prima de Antigüedad:"
        '
        'txtIndeminizacion
        '
        Me.txtIndeminizacion.Location = New System.Drawing.Point(172, 160)
        Me.txtIndeminizacion.Name = "txtIndeminizacion"
        Me.txtIndeminizacion.Size = New System.Drawing.Size(160, 20)
        Me.txtIndeminizacion.TabIndex = 26
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 167)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Indeminizacion:"
        '
        'pnlProgreso
        '
        Me.pnlProgreso.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlProgreso.Controls.Add(Me.Label2)
        Me.pnlProgreso.Controls.Add(Me.pgbProgreso)
        Me.pnlProgreso.Location = New System.Drawing.Point(859, 314)
        Me.pnlProgreso.Name = "pnlProgreso"
        Me.pnlProgreso.Size = New System.Drawing.Size(377, 84)
        Me.pnlProgreso.TabIndex = 24
        Me.pnlProgreso.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(179, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Procesando"
        '
        'pgbProgreso
        '
        Me.pgbProgreso.Location = New System.Drawing.Point(21, 22)
        Me.pgbProgreso.Name = "pgbProgreso"
        Me.pgbProgreso.Size = New System.Drawing.Size(413, 30)
        Me.pgbProgreso.TabIndex = 0
        '
        'lsvLista
        '
        Me.lsvLista.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lsvLista.CheckBoxes = True
        Me.lsvLista.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvLista.FullRowSelect = True
        Me.lsvLista.GridLines = True
        Me.lsvLista.HideSelection = False
        Me.lsvLista.Location = New System.Drawing.Point(608, 17)
        Me.lsvLista.MultiSelect = False
        Me.lsvLista.Name = "lsvLista"
        Me.lsvLista.Size = New System.Drawing.Size(819, 674)
        Me.lsvLista.TabIndex = 2
        Me.lsvLista.UseCompatibleStateImageBehavior = False
        Me.lsvLista.View = System.Windows.Forms.View.Details
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbGuardar, Me.tsbNuevo, Me.tsbImportar, Me.tsbProcesar, Me.tsbCancelar})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1465, 54)
        Me.ToolStrip1.TabIndex = 25
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbGuardar
        '
        Me.tsbGuardar.AutoSize = False
        Me.tsbGuardar.Image = CType(resources.GetObject("tsbGuardar.Image"), System.Drawing.Image)
        Me.tsbGuardar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbGuardar.Name = "tsbGuardar"
        Me.tsbGuardar.Size = New System.Drawing.Size(110, 51)
        Me.tsbGuardar.Text = "Guardar Finiqutos"
        Me.tsbGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbGuardar.ToolTipText = "Guardar incidencias"
        '
        'tsbNuevo
        '
        Me.tsbNuevo.Image = CType(resources.GetObject("tsbNuevo.Image"), System.Drawing.Image)
        Me.tsbNuevo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNuevo.Name = "tsbNuevo"
        Me.tsbNuevo.Size = New System.Drawing.Size(83, 51)
        Me.tsbNuevo.Text = "Agregar excel"
        Me.tsbNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbNuevo.ToolTipText = "Agregar excel"
        '
        'tsbImportar
        '
        Me.tsbImportar.Enabled = False
        Me.tsbImportar.Image = CType(resources.GetObject("tsbImportar.Image"), System.Drawing.Image)
        Me.tsbImportar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbImportar.Name = "tsbImportar"
        Me.tsbImportar.Size = New System.Drawing.Size(99, 51)
        Me.tsbImportar.Text = "Importar archivo"
        Me.tsbImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbProcesar
        '
        Me.tsbProcesar.Enabled = False
        Me.tsbProcesar.Image = CType(resources.GetObject("tsbProcesar.Image"), System.Drawing.Image)
        Me.tsbProcesar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProcesar.Name = "tsbProcesar"
        Me.tsbProcesar.Size = New System.Drawing.Size(98, 51)
        Me.tsbProcesar.Text = "Procesar archivo"
        Me.tsbProcesar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbProcesar.Visible = False
        '
        'tsbCancelar
        '
        Me.tsbCancelar.AutoSize = False
        Me.tsbCancelar.Enabled = False
        Me.tsbCancelar.Image = CType(resources.GetObject("tsbCancelar.Image"), System.Drawing.Image)
        Me.tsbCancelar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCancelar.Name = "tsbCancelar"
        Me.tsbCancelar.Size = New System.Drawing.Size(90, 51)
        Me.tsbCancelar.Text = "Cancelar"
        Me.tsbCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'lblRuta
        '
        Me.lblRuta.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRuta.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRuta.Location = New System.Drawing.Point(0, 752)
        Me.lblRuta.Name = "lblRuta"
        Me.lblRuta.Size = New System.Drawing.Size(604, 39)
        Me.lblRuta.TabIndex = 34
        '
        'cmdCerrar
        '
        Me.cmdCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCerrar.Location = New System.Drawing.Point(1391, 755)
        Me.cmdCerrar.Name = "cmdCerrar"
        Me.cmdCerrar.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.cmdCerrar.Size = New System.Drawing.Size(62, 43)
        Me.cmdCerrar.TabIndex = 35
        Me.cmdCerrar.Text = "Cerrar"
        Me.cmdCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCerrar.UseVisualStyleBackColor = True
        '
        'frmFiniquito
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1465, 800)
        Me.Controls.Add(Me.cmdCerrar)
        Me.Controls.Add(Me.lblRuta)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.pnlCatalogo)
        Me.Name = "frmFiniquito"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Subir Finiquito"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlCatalogo.ResumeLayout(False)
        Me.pnlCatalogo.PerformLayout()
        Me.pnlProgreso.ResumeLayout(False)
        Me.pnlProgreso.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlCatalogo As System.Windows.Forms.Panel
    Friend WithEvents pnlProgreso As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pgbProgreso As System.Windows.Forms.ProgressBar
    Friend WithEvents lsvLista As System.Windows.Forms.ListView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNuevo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbImportar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbGuardar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbProcesar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCancelar As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblRuta As System.Windows.Forms.Label
    Friend WithEvents cmdCerrar As System.Windows.Forms.Button
    Friend WithEvents txtVacacionesProp As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtAguinaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSueldo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPrimaAntiguedad As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtIndeminizacion As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtVacacionesPendientes As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPrimaVacacional As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtISR As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSTP As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtISRIndemnizacion As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSindicato As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtVacacionesPendientesExce As System.Windows.Forms.TextBox
    Friend WithEvents txtPrimaVacacionalExce As System.Windows.Forms.TextBox
    Friend WithEvents txtVacacionesPropExce As System.Windows.Forms.TextBox
    Friend WithEvents txtAguinaldoExce As System.Windows.Forms.TextBox
    Friend WithEvents txtSueldoExce As System.Windows.Forms.TextBox
    Friend WithEvents txtAntiguedadExce As System.Windows.Forms.TextBox
    Friend WithEvents txtindeminizacionExce As System.Windows.Forms.TextBox
    Friend WithEvents cboEmpleado As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cboSerie As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cboPeriodo As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtHE3Exce As System.Windows.Forms.TextBox
    Friend WithEvents txtHE3 As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtHE2Exce As System.Windows.Forms.TextBox
    Friend WithEvents txtHE2 As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtVales As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtBonosExce As System.Windows.Forms.TextBox
    Friend WithEvents txtBonos As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtDLExce As System.Windows.Forms.TextBox
    Friend WithEvents txtDL As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtIncidenciasExce As System.Windows.Forms.TextBox
    Friend WithEvents txtIncidencias As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtFonacot As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtInfonavitExce As System.Windows.Forms.TextBox
    Friend WithEvents txtInfonavit As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents btnAgregar As System.Windows.Forms.Button
    Friend WithEvents dtpFechaBaja As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
