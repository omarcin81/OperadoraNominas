<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdministrativos
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdministrativos))
        Me.cmdConcentradoFonacot = New System.Windows.Forms.Button()
        Me.cmdImssNomina = New System.Windows.Forms.Button()
        Me.chkNoinfonavit = New System.Windows.Forms.CheckBox()
        Me.btnAsimilados = New System.Windows.Forms.Button()
        Me.cmdInfonavitNominaSerie = New System.Windows.Forms.Button()
        Me.cmdComision = New System.Windows.Forms.Button()
        Me.cmdReporteInfonavit = New System.Windows.Forms.Button()
        Me.cmdInfonavit = New System.Windows.Forms.Button()
        Me.layoutTimbrado = New System.Windows.Forms.Button()
        Me.pnlProgreso = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pgbProgreso = New System.Windows.Forms.ProgressBar()
        Me.pnlCatalogo = New System.Windows.Forms.Panel()
        Me.chkPrestamoSA = New System.Windows.Forms.CheckBox()
        Me.cmdsoloisr = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmdCalculoSoloInfonavit = New System.Windows.Forms.Button()
        Me.cmdAcumuladoOperadora = New System.Windows.Forms.Button()
        Me.cmdBuscarOtraNom = New System.Windows.Forms.Button()
        Me.chkSoloCostoSocial = New System.Windows.Forms.CheckBox()
        Me.chkNofonacot = New System.Windows.Forms.CheckBox()
        Me.chkCalSoloMarcados = New System.Windows.Forms.CheckBox()
        Me.chkPrestamosAsi = New System.Windows.Forms.CheckBox()
        Me.chkInfonavit0 = New System.Windows.Forms.CheckBox()
        Me.cmdResumenInfo = New System.Windows.Forms.Button()
        Me.cmdSubirDatos = New System.Windows.Forms.Button()
        Me.btnReporte = New System.Windows.Forms.Button()
        Me.cboserie = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkgrupo = New System.Windows.Forms.CheckBox()
        Me.chkinter = New System.Windows.Forms.CheckBox()
        Me.cbobancos = New System.Windows.Forms.ComboBox()
        Me.chkSindicato = New System.Windows.Forms.CheckBox()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.cmdreiniciar = New System.Windows.Forms.Button()
        Me.cmdincidencias = New System.Windows.Forms.Button()
        Me.cmdexcel = New System.Windows.Forms.Button()
        Me.cmdlayouts = New System.Windows.Forms.Button()
        Me.cmdrecibosA = New System.Windows.Forms.Button()
        Me.cmdguardarfinal = New System.Windows.Forms.Button()
        Me.cmdguardarnomina = New System.Windows.Forms.Button()
        Me.cmdcalcular = New System.Windows.Forms.Button()
        Me.dtgDatos = New System.Windows.Forms.DataGridView()
        Me.cmdverdatos = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboperiodo = New System.Windows.Forms.ComboBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbEmpleados = New System.Windows.Forms.ToolStripButton()
        Me.tsbPeriodos = New System.Windows.Forms.ToolStripButton()
        Me.tsbpuestos = New System.Windows.Forms.ToolStripButton()
        Me.tsbdeptos = New System.Windows.Forms.ToolStripButton()
        Me.tsbImportar = New System.Windows.Forms.ToolStripButton()
        Me.tsbIEmpleados = New System.Windows.Forms.ToolStripButton()
        Me.tsbbuscar = New System.Windows.Forms.ToolStripButton()
        Me.tsbLayout = New System.Windows.Forms.ToolStripButton()
        Me.cMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EliminarDeLaListaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AgregarTrabajadoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditarEmpleadoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoCalcularInofnavitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActicarCalculoInfonavitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoCalcularPresAsiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActivaCalculoPresAsiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoCalcularPresSAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActivarCaluloPresSAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SoloRegistroACalcularToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesactivarSoloRegistroACalcularToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RegistroTotalDiasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesactivarRegistroTotalDiasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EliminarDeLaBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CostoCeroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesactivarCostoCeroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NoCalcularCostoSocialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesactivarNoCalcularCostoSocialToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlProgreso.SuspendLayout()
        Me.pnlCatalogo.SuspendLayout()
        CType(Me.dtgDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.cMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdConcentradoFonacot
        '
        Me.cmdConcentradoFonacot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdConcentradoFonacot.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConcentradoFonacot.Location = New System.Drawing.Point(1001, 519)
        Me.cmdConcentradoFonacot.Name = "cmdConcentradoFonacot"
        Me.cmdConcentradoFonacot.Size = New System.Drawing.Size(147, 28)
        Me.cmdConcentradoFonacot.TabIndex = 46
        Me.cmdConcentradoFonacot.Text = "Concentrado Fonacot"
        Me.cmdConcentradoFonacot.UseVisualStyleBackColor = True
        Me.cmdConcentradoFonacot.Visible = False
        '
        'cmdImssNomina
        '
        Me.cmdImssNomina.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdImssNomina.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImssNomina.Location = New System.Drawing.Point(864, 519)
        Me.cmdImssNomina.Name = "cmdImssNomina"
        Me.cmdImssNomina.Size = New System.Drawing.Size(131, 27)
        Me.cmdImssNomina.TabIndex = 44
        Me.cmdImssNomina.Text = "IMSS/Nomina"
        Me.cmdImssNomina.UseVisualStyleBackColor = True
        Me.cmdImssNomina.Visible = False
        '
        'chkNoinfonavit
        '
        Me.chkNoinfonavit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkNoinfonavit.AutoSize = True
        Me.chkNoinfonavit.BackColor = System.Drawing.Color.Transparent
        Me.chkNoinfonavit.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoinfonavit.Location = New System.Drawing.Point(1203, 524)
        Me.chkNoinfonavit.Name = "chkNoinfonavit"
        Me.chkNoinfonavit.Size = New System.Drawing.Size(154, 22)
        Me.chkNoinfonavit.TabIndex = 45
        Me.chkNoinfonavit.Text = "No calcular infonavit"
        Me.chkNoinfonavit.UseVisualStyleBackColor = False
        '
        'btnAsimilados
        '
        Me.btnAsimilados.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAsimilados.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAsimilados.Location = New System.Drawing.Point(141, 519)
        Me.btnAsimilados.Name = "btnAsimilados"
        Me.btnAsimilados.Size = New System.Drawing.Size(176, 27)
        Me.btnAsimilados.TabIndex = 42
        Me.btnAsimilados.Text = "Layout Timbrado Asimilados"
        Me.btnAsimilados.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnAsimilados.UseVisualStyleBackColor = True
        '
        'cmdInfonavitNominaSerie
        '
        Me.cmdInfonavitNominaSerie.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdInfonavitNominaSerie.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInfonavitNominaSerie.Location = New System.Drawing.Point(655, 519)
        Me.cmdInfonavitNominaSerie.Name = "cmdInfonavitNominaSerie"
        Me.cmdInfonavitNominaSerie.Size = New System.Drawing.Size(203, 27)
        Me.cmdInfonavitNominaSerie.TabIndex = 43
        Me.cmdInfonavitNominaSerie.Text = "Concentrado INfonavit x nomina"
        Me.cmdInfonavitNominaSerie.UseVisualStyleBackColor = True
        '
        'cmdComision
        '
        Me.cmdComision.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdComision.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdComision.Location = New System.Drawing.Point(547, 519)
        Me.cmdComision.Name = "cmdComision"
        Me.cmdComision.Size = New System.Drawing.Size(99, 27)
        Me.cmdComision.TabIndex = 41
        Me.cmdComision.Text = "Comisión"
        Me.cmdComision.UseVisualStyleBackColor = True
        '
        'cmdReporteInfonavit
        '
        Me.cmdReporteInfonavit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdReporteInfonavit.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReporteInfonavit.Location = New System.Drawing.Point(442, 519)
        Me.cmdReporteInfonavit.Name = "cmdReporteInfonavit"
        Me.cmdReporteInfonavit.Size = New System.Drawing.Size(99, 27)
        Me.cmdReporteInfonavit.TabIndex = 40
        Me.cmdReporteInfonavit.Text = "Saldo Infonavit"
        Me.cmdReporteInfonavit.UseVisualStyleBackColor = True
        '
        'cmdInfonavit
        '
        Me.cmdInfonavit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdInfonavit.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInfonavit.Location = New System.Drawing.Point(324, 519)
        Me.cmdInfonavit.Name = "cmdInfonavit"
        Me.cmdInfonavit.Size = New System.Drawing.Size(112, 27)
        Me.cmdInfonavit.TabIndex = 39
        Me.cmdInfonavit.Text = "Reporte Infonavit"
        Me.cmdInfonavit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdInfonavit.UseVisualStyleBackColor = True
        '
        'layoutTimbrado
        '
        Me.layoutTimbrado.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.layoutTimbrado.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.layoutTimbrado.Location = New System.Drawing.Point(12, 519)
        Me.layoutTimbrado.Name = "layoutTimbrado"
        Me.layoutTimbrado.Size = New System.Drawing.Size(123, 27)
        Me.layoutTimbrado.TabIndex = 38
        Me.layoutTimbrado.Text = "Layout Timbrado SA"
        Me.layoutTimbrado.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.layoutTimbrado.UseVisualStyleBackColor = True
        '
        'pnlProgreso
        '
        Me.pnlProgreso.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlProgreso.Controls.Add(Me.Label2)
        Me.pnlProgreso.Controls.Add(Me.pgbProgreso)
        Me.pnlProgreso.Location = New System.Drawing.Point(454, 239)
        Me.pnlProgreso.Name = "pnlProgreso"
        Me.pnlProgreso.Size = New System.Drawing.Size(449, 84)
        Me.pnlProgreso.TabIndex = 37
        Me.pnlProgreso.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(154, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Procesando..."
        '
        'pgbProgreso
        '
        Me.pgbProgreso.Location = New System.Drawing.Point(17, 12)
        Me.pgbProgreso.Name = "pgbProgreso"
        Me.pgbProgreso.Size = New System.Drawing.Size(413, 30)
        Me.pgbProgreso.TabIndex = 0
        '
        'pnlCatalogo
        '
        Me.pnlCatalogo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlCatalogo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCatalogo.Controls.Add(Me.chkPrestamoSA)
        Me.pnlCatalogo.Controls.Add(Me.cmdsoloisr)
        Me.pnlCatalogo.Controls.Add(Me.Button1)
        Me.pnlCatalogo.Controls.Add(Me.cmdCalculoSoloInfonavit)
        Me.pnlCatalogo.Controls.Add(Me.cmdAcumuladoOperadora)
        Me.pnlCatalogo.Controls.Add(Me.cmdBuscarOtraNom)
        Me.pnlCatalogo.Controls.Add(Me.chkSoloCostoSocial)
        Me.pnlCatalogo.Controls.Add(Me.chkNofonacot)
        Me.pnlCatalogo.Controls.Add(Me.chkCalSoloMarcados)
        Me.pnlCatalogo.Controls.Add(Me.chkPrestamosAsi)
        Me.pnlCatalogo.Controls.Add(Me.chkInfonavit0)
        Me.pnlCatalogo.Controls.Add(Me.cmdResumenInfo)
        Me.pnlCatalogo.Controls.Add(Me.cmdSubirDatos)
        Me.pnlCatalogo.Controls.Add(Me.btnReporte)
        Me.pnlCatalogo.Controls.Add(Me.cboserie)
        Me.pnlCatalogo.Controls.Add(Me.Label3)
        Me.pnlCatalogo.Controls.Add(Me.chkgrupo)
        Me.pnlCatalogo.Controls.Add(Me.chkinter)
        Me.pnlCatalogo.Controls.Add(Me.cbobancos)
        Me.pnlCatalogo.Controls.Add(Me.chkSindicato)
        Me.pnlCatalogo.Controls.Add(Me.chkAll)
        Me.pnlCatalogo.Controls.Add(Me.cmdreiniciar)
        Me.pnlCatalogo.Controls.Add(Me.cmdincidencias)
        Me.pnlCatalogo.Controls.Add(Me.cmdexcel)
        Me.pnlCatalogo.Controls.Add(Me.cmdlayouts)
        Me.pnlCatalogo.Controls.Add(Me.cmdrecibosA)
        Me.pnlCatalogo.Controls.Add(Me.cmdguardarfinal)
        Me.pnlCatalogo.Controls.Add(Me.cmdguardarnomina)
        Me.pnlCatalogo.Controls.Add(Me.cmdcalcular)
        Me.pnlCatalogo.Controls.Add(Me.dtgDatos)
        Me.pnlCatalogo.Controls.Add(Me.cmdverdatos)
        Me.pnlCatalogo.Controls.Add(Me.Label1)
        Me.pnlCatalogo.Controls.Add(Me.cboperiodo)
        Me.pnlCatalogo.Location = New System.Drawing.Point(0, 56)
        Me.pnlCatalogo.Name = "pnlCatalogo"
        Me.pnlCatalogo.Size = New System.Drawing.Size(1357, 451)
        Me.pnlCatalogo.TabIndex = 36
        '
        'chkPrestamoSA
        '
        Me.chkPrestamoSA.AutoSize = True
        Me.chkPrestamoSA.BackColor = System.Drawing.Color.Transparent
        Me.chkPrestamoSA.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrestamoSA.Location = New System.Drawing.Point(223, 70)
        Me.chkPrestamoSA.Name = "chkPrestamoSA"
        Me.chkPrestamoSA.Size = New System.Drawing.Size(123, 22)
        Me.chkPrestamoSA.TabIndex = 37
        Me.chkPrestamoSA.Text = "No CAL PRES SA"
        Me.chkPrestamoSA.UseVisualStyleBackColor = False
        '
        'cmdsoloisr
        '
        Me.cmdsoloisr.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdsoloisr.Location = New System.Drawing.Point(520, 67)
        Me.cmdsoloisr.Name = "cmdsoloisr"
        Me.cmdsoloisr.Size = New System.Drawing.Size(163, 27)
        Me.cmdsoloisr.TabIndex = 36
        Me.cmdsoloisr.Text = "Calcular solo ISR"
        Me.cmdsoloisr.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(847, 68)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(156, 26)
        Me.Button1.TabIndex = 35
        Me.Button1.Text = "Infonavit x periodo"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmdCalculoSoloInfonavit
        '
        Me.cmdCalculoSoloInfonavit.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCalculoSoloInfonavit.Location = New System.Drawing.Point(692, 68)
        Me.cmdCalculoSoloInfonavit.Name = "cmdCalculoSoloInfonavit"
        Me.cmdCalculoSoloInfonavit.Size = New System.Drawing.Size(146, 27)
        Me.cmdCalculoSoloInfonavit.TabIndex = 34
        Me.cmdCalculoSoloInfonavit.Text = "Calcular Infonavit Solo"
        Me.cmdCalculoSoloInfonavit.UseVisualStyleBackColor = True
        '
        'cmdAcumuladoOperadora
        '
        Me.cmdAcumuladoOperadora.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAcumuladoOperadora.Location = New System.Drawing.Point(1191, 68)
        Me.cmdAcumuladoOperadora.Name = "cmdAcumuladoOperadora"
        Me.cmdAcumuladoOperadora.Size = New System.Drawing.Size(129, 26)
        Me.cmdAcumuladoOperadora.TabIndex = 33
        Me.cmdAcumuladoOperadora.Text = "Acumulado Mary"
        Me.cmdAcumuladoOperadora.UseVisualStyleBackColor = True
        '
        'cmdBuscarOtraNom
        '
        Me.cmdBuscarOtraNom.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBuscarOtraNom.Location = New System.Drawing.Point(1012, 68)
        Me.cmdBuscarOtraNom.Name = "cmdBuscarOtraNom"
        Me.cmdBuscarOtraNom.Size = New System.Drawing.Size(173, 26)
        Me.cmdBuscarOtraNom.TabIndex = 32
        Me.cmdBuscarOtraNom.Text = "Buscar en otra nomina"
        Me.cmdBuscarOtraNom.UseVisualStyleBackColor = True
        '
        'chkSoloCostoSocial
        '
        Me.chkSoloCostoSocial.AutoSize = True
        Me.chkSoloCostoSocial.BackColor = System.Drawing.Color.Transparent
        Me.chkSoloCostoSocial.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSoloCostoSocial.Location = New System.Drawing.Point(382, 69)
        Me.chkSoloCostoSocial.Name = "chkSoloCostoSocial"
        Me.chkSoloCostoSocial.Size = New System.Drawing.Size(128, 22)
        Me.chkSoloCostoSocial.TabIndex = 31
        Me.chkSoloCostoSocial.Text = "Solo costo social"
        Me.chkSoloCostoSocial.UseVisualStyleBackColor = False
        '
        'chkNofonacot
        '
        Me.chkNofonacot.AutoSize = True
        Me.chkNofonacot.BackColor = System.Drawing.Color.Transparent
        Me.chkNofonacot.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNofonacot.Location = New System.Drawing.Point(126, 69)
        Me.chkNofonacot.Name = "chkNofonacot"
        Me.chkNofonacot.Size = New System.Drawing.Size(95, 22)
        Me.chkNofonacot.TabIndex = 30
        Me.chkNofonacot.Text = "No fonacot"
        Me.chkNofonacot.UseVisualStyleBackColor = False
        '
        'chkCalSoloMarcados
        '
        Me.chkCalSoloMarcados.AutoSize = True
        Me.chkCalSoloMarcados.BackColor = System.Drawing.Color.Transparent
        Me.chkCalSoloMarcados.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCalSoloMarcados.Location = New System.Drawing.Point(7, 36)
        Me.chkCalSoloMarcados.Name = "chkCalSoloMarcados"
        Me.chkCalSoloMarcados.Size = New System.Drawing.Size(116, 22)
        Me.chkCalSoloMarcados.TabIndex = 29
        Me.chkCalSoloMarcados.Text = "Solo marcados"
        Me.chkCalSoloMarcados.UseVisualStyleBackColor = False
        '
        'chkPrestamosAsi
        '
        Me.chkPrestamosAsi.AutoSize = True
        Me.chkPrestamosAsi.BackColor = System.Drawing.Color.Transparent
        Me.chkPrestamosAsi.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrestamosAsi.Location = New System.Drawing.Point(223, 37)
        Me.chkPrestamosAsi.Name = "chkPrestamosAsi"
        Me.chkPrestamosAsi.Size = New System.Drawing.Size(127, 22)
        Me.chkPrestamosAsi.TabIndex = 28
        Me.chkPrestamosAsi.Text = "No CAL PRES ASI"
        Me.chkPrestamosAsi.UseVisualStyleBackColor = False
        '
        'chkInfonavit0
        '
        Me.chkInfonavit0.AutoSize = True
        Me.chkInfonavit0.BackColor = System.Drawing.Color.Transparent
        Me.chkInfonavit0.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInfonavit0.Location = New System.Drawing.Point(128, 36)
        Me.chkInfonavit0.Name = "chkInfonavit0"
        Me.chkInfonavit0.Size = New System.Drawing.Size(93, 22)
        Me.chkInfonavit0.TabIndex = 27
        Me.chkInfonavit0.Text = "Infonavit 0"
        Me.chkInfonavit0.UseVisualStyleBackColor = False
        '
        'cmdResumenInfo
        '
        Me.cmdResumenInfo.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdResumenInfo.Location = New System.Drawing.Point(1176, 34)
        Me.cmdResumenInfo.Name = "cmdResumenInfo"
        Me.cmdResumenInfo.Size = New System.Drawing.Size(147, 28)
        Me.cmdResumenInfo.TabIndex = 26
        Me.cmdResumenInfo.Text = "Concentrado Infonavit"
        Me.cmdResumenInfo.UseVisualStyleBackColor = True
        '
        'cmdSubirDatos
        '
        Me.cmdSubirDatos.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubirDatos.Location = New System.Drawing.Point(1058, 34)
        Me.cmdSubirDatos.Name = "cmdSubirDatos"
        Me.cmdSubirDatos.Size = New System.Drawing.Size(103, 28)
        Me.cmdSubirDatos.TabIndex = 25
        Me.cmdSubirDatos.Text = "Subir datos"
        Me.cmdSubirDatos.UseVisualStyleBackColor = True
        '
        'btnReporte
        '
        Me.btnReporte.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReporte.Location = New System.Drawing.Point(942, 33)
        Me.btnReporte.Name = "btnReporte"
        Me.btnReporte.Size = New System.Drawing.Size(103, 28)
        Me.btnReporte.TabIndex = 24
        Me.btnReporte.Text = "Reporte a Excel"
        Me.btnReporte.UseVisualStyleBackColor = True
        '
        'cboserie
        '
        Me.cboserie.FormattingEnabled = True
        Me.cboserie.Items.AddRange(New Object() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P"})
        Me.cboserie.Location = New System.Drawing.Point(319, 3)
        Me.cboserie.Name = "cboserie"
        Me.cboserie.Size = New System.Drawing.Size(59, 27)
        Me.cboserie.TabIndex = 21
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(270, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 19)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Serie:"
        '
        'chkgrupo
        '
        Me.chkgrupo.AutoSize = True
        Me.chkgrupo.BackColor = System.Drawing.Color.Transparent
        Me.chkgrupo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkgrupo.Location = New System.Drawing.Point(871, 36)
        Me.chkgrupo.Name = "chkgrupo"
        Me.chkgrupo.Size = New System.Drawing.Size(65, 22)
        Me.chkgrupo.TabIndex = 19
        Me.chkgrupo.Text = "Grupo"
        Me.chkgrupo.UseVisualStyleBackColor = False
        '
        'chkinter
        '
        Me.chkinter.AutoSize = True
        Me.chkinter.BackColor = System.Drawing.Color.Transparent
        Me.chkinter.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkinter.Location = New System.Drawing.Point(446, 37)
        Me.chkinter.Name = "chkinter"
        Me.chkinter.Size = New System.Drawing.Size(110, 22)
        Me.chkinter.TabIndex = 18
        Me.chkinter.Text = "Interbancario"
        Me.chkinter.UseVisualStyleBackColor = False
        '
        'cbobancos
        '
        Me.cbobancos.FormattingEnabled = True
        Me.cbobancos.Location = New System.Drawing.Point(573, 33)
        Me.cbobancos.Name = "cbobancos"
        Me.cbobancos.Size = New System.Drawing.Size(220, 27)
        Me.cbobancos.TabIndex = 17
        '
        'chkSindicato
        '
        Me.chkSindicato.AutoSize = True
        Me.chkSindicato.BackColor = System.Drawing.Color.Transparent
        Me.chkSindicato.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSindicato.Location = New System.Drawing.Point(356, 37)
        Me.chkSindicato.Name = "chkSindicato"
        Me.chkSindicato.Size = New System.Drawing.Size(84, 22)
        Me.chkSindicato.TabIndex = 16
        Me.chkSindicato.Text = "Sindicato"
        Me.chkSindicato.UseVisualStyleBackColor = False
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.BackColor = System.Drawing.Color.Transparent
        Me.chkAll.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAll.Location = New System.Drawing.Point(6, 67)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(107, 22)
        Me.chkAll.TabIndex = 15
        Me.chkAll.Text = "Marcar todos"
        Me.chkAll.UseVisualStyleBackColor = False
        '
        'cmdreiniciar
        '
        Me.cmdreiniciar.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdreiniciar.Location = New System.Drawing.Point(1228, 3)
        Me.cmdreiniciar.Name = "cmdreiniciar"
        Me.cmdreiniciar.Size = New System.Drawing.Size(111, 27)
        Me.cmdreiniciar.TabIndex = 14
        Me.cmdreiniciar.Text = "Reiniciar Nomina"
        Me.cmdreiniciar.UseVisualStyleBackColor = True
        '
        'cmdincidencias
        '
        Me.cmdincidencias.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdincidencias.Location = New System.Drawing.Point(1111, 3)
        Me.cmdincidencias.Name = "cmdincidencias"
        Me.cmdincidencias.Size = New System.Drawing.Size(111, 27)
        Me.cmdincidencias.TabIndex = 13
        Me.cmdincidencias.Text = "Excel Incidencias"
        Me.cmdincidencias.UseVisualStyleBackColor = True
        '
        'cmdexcel
        '
        Me.cmdexcel.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdexcel.Location = New System.Drawing.Point(1012, 3)
        Me.cmdexcel.Name = "cmdexcel"
        Me.cmdexcel.Size = New System.Drawing.Size(93, 27)
        Me.cmdexcel.TabIndex = 12
        Me.cmdexcel.Text = "Enviar a Excel"
        Me.cmdexcel.UseVisualStyleBackColor = True
        '
        'cmdlayouts
        '
        Me.cmdlayouts.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdlayouts.Location = New System.Drawing.Point(799, 34)
        Me.cmdlayouts.Name = "cmdlayouts"
        Me.cmdlayouts.Size = New System.Drawing.Size(66, 27)
        Me.cmdlayouts.TabIndex = 11
        Me.cmdlayouts.Text = "Layout"
        Me.cmdlayouts.UseVisualStyleBackColor = True
        '
        'cmdrecibosA
        '
        Me.cmdrecibosA.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdrecibosA.Location = New System.Drawing.Point(886, 3)
        Me.cmdrecibosA.Name = "cmdrecibosA"
        Me.cmdrecibosA.Size = New System.Drawing.Size(122, 27)
        Me.cmdrecibosA.TabIndex = 10
        Me.cmdrecibosA.Text = "Asimilado Simple"
        Me.cmdrecibosA.UseVisualStyleBackColor = True
        '
        'cmdguardarfinal
        '
        Me.cmdguardarfinal.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdguardarfinal.Location = New System.Drawing.Point(789, 3)
        Me.cmdguardarfinal.Name = "cmdguardarfinal"
        Me.cmdguardarfinal.Size = New System.Drawing.Size(92, 27)
        Me.cmdguardarfinal.TabIndex = 9
        Me.cmdguardarfinal.Text = "Guardar Final"
        Me.cmdguardarfinal.UseVisualStyleBackColor = True
        '
        'cmdguardarnomina
        '
        Me.cmdguardarnomina.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdguardarnomina.Location = New System.Drawing.Point(722, 3)
        Me.cmdguardarnomina.Name = "cmdguardarnomina"
        Me.cmdguardarnomina.Size = New System.Drawing.Size(63, 27)
        Me.cmdguardarnomina.TabIndex = 8
        Me.cmdguardarnomina.Text = "Guardar"
        Me.cmdguardarnomina.UseVisualStyleBackColor = True
        '
        'cmdcalcular
        '
        Me.cmdcalcular.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdcalcular.Location = New System.Drawing.Point(649, 3)
        Me.cmdcalcular.Name = "cmdcalcular"
        Me.cmdcalcular.Size = New System.Drawing.Size(63, 27)
        Me.cmdcalcular.TabIndex = 7
        Me.cmdcalcular.Text = "Calcular"
        Me.cmdcalcular.UseVisualStyleBackColor = True
        '
        'dtgDatos
        '
        Me.dtgDatos.AllowUserToAddRows = False
        Me.dtgDatos.AllowUserToDeleteRows = False
        Me.dtgDatos.AllowUserToOrderColumns = True
        Me.dtgDatos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtgDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgDatos.Location = New System.Drawing.Point(1, 104)
        Me.dtgDatos.Name = "dtgDatos"
        Me.dtgDatos.Size = New System.Drawing.Size(1349, 340)
        Me.dtgDatos.TabIndex = 6
        '
        'cmdverdatos
        '
        Me.cmdverdatos.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdverdatos.Location = New System.Drawing.Point(573, 3)
        Me.cmdverdatos.Name = "cmdverdatos"
        Me.cmdverdatos.Size = New System.Drawing.Size(71, 27)
        Me.cmdverdatos.TabIndex = 5
        Me.cmdverdatos.Text = "Ver datos"
        Me.cmdverdatos.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 19)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Periodo:"
        '
        'cboperiodo
        '
        Me.cboperiodo.FormattingEnabled = True
        Me.cboperiodo.Location = New System.Drawing.Point(73, 3)
        Me.cboperiodo.Name = "cboperiodo"
        Me.cboperiodo.Size = New System.Drawing.Size(191, 27)
        Me.cboperiodo.TabIndex = 3
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbEmpleados, Me.tsbPeriodos, Me.tsbpuestos, Me.tsbdeptos, Me.tsbImportar, Me.tsbIEmpleados, Me.tsbbuscar, Me.tsbLayout})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1357, 54)
        Me.ToolStrip1.TabIndex = 35
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbEmpleados
        '
        Me.tsbEmpleados.Image = CType(resources.GetObject("tsbEmpleados.Image"), System.Drawing.Image)
        Me.tsbEmpleados.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEmpleados.Name = "tsbEmpleados"
        Me.tsbEmpleados.Size = New System.Drawing.Size(118, 51)
        Me.tsbEmpleados.Text = "Importar Empleados"
        Me.tsbEmpleados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbPeriodos
        '
        Me.tsbPeriodos.Image = CType(resources.GetObject("tsbPeriodos.Image"), System.Drawing.Image)
        Me.tsbPeriodos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPeriodos.Name = "tsbPeriodos"
        Me.tsbPeriodos.Size = New System.Drawing.Size(106, 51)
        Me.tsbPeriodos.Text = "Importar Períodos"
        Me.tsbPeriodos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbPeriodos.Visible = False
        '
        'tsbpuestos
        '
        Me.tsbpuestos.Image = CType(resources.GetObject("tsbpuestos.Image"), System.Drawing.Image)
        Me.tsbpuestos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbpuestos.Name = "tsbpuestos"
        Me.tsbpuestos.Size = New System.Drawing.Size(101, 51)
        Me.tsbpuestos.Text = "Importar Puestos"
        Me.tsbpuestos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbpuestos.Visible = False
        '
        'tsbdeptos
        '
        Me.tsbdeptos.Image = CType(resources.GetObject("tsbdeptos.Image"), System.Drawing.Image)
        Me.tsbdeptos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbdeptos.Name = "tsbdeptos"
        Me.tsbdeptos.Size = New System.Drawing.Size(96, 51)
        Me.tsbdeptos.Text = "Importar deptos"
        Me.tsbdeptos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbdeptos.Visible = False
        '
        'tsbImportar
        '
        Me.tsbImportar.Image = CType(resources.GetObject("tsbImportar.Image"), System.Drawing.Image)
        Me.tsbImportar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbImportar.Name = "tsbImportar"
        Me.tsbImportar.Size = New System.Drawing.Size(70, 51)
        Me.tsbImportar.Text = "Incidencias"
        Me.tsbImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbImportar.ToolTipText = "Importar incidencias"
        '
        'tsbIEmpleados
        '
        Me.tsbIEmpleados.Image = CType(resources.GetObject("tsbIEmpleados.Image"), System.Drawing.Image)
        Me.tsbIEmpleados.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbIEmpleados.Name = "tsbIEmpleados"
        Me.tsbIEmpleados.Size = New System.Drawing.Size(69, 51)
        Me.tsbIEmpleados.Text = "Empleados"
        Me.tsbIEmpleados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbbuscar
        '
        Me.tsbbuscar.Image = CType(resources.GetObject("tsbbuscar.Image"), System.Drawing.Image)
        Me.tsbbuscar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbbuscar.Name = "tsbbuscar"
        Me.tsbbuscar.Size = New System.Drawing.Size(46, 51)
        Me.tsbbuscar.Text = "Buscar"
        Me.tsbbuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbLayout
        '
        Me.tsbLayout.AutoSize = False
        Me.tsbLayout.Image = CType(resources.GetObject("tsbLayout.Image"), System.Drawing.Image)
        Me.tsbLayout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLayout.Name = "tsbLayout"
        Me.tsbLayout.Size = New System.Drawing.Size(90, 51)
        Me.tsbLayout.Text = "Layouts"
        Me.tsbLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbLayout.Visible = False
        '
        'cMenu
        '
        Me.cMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EliminarDeLaListaToolStripMenuItem, Me.AgregarTrabajadoresToolStripMenuItem, Me.EditarEmpleadoToolStripMenuItem, Me.NoCalcularInofnavitToolStripMenuItem, Me.ActicarCalculoInfonavitToolStripMenuItem, Me.NoCalcularPresAsiToolStripMenuItem, Me.ActivaCalculoPresAsiToolStripMenuItem, Me.NoCalcularPresSAToolStripMenuItem, Me.ActivarCaluloPresSAToolStripMenuItem, Me.SoloRegistroACalcularToolStripMenuItem, Me.DesactivarSoloRegistroACalcularToolStripMenuItem, Me.RegistroTotalDiasToolStripMenuItem, Me.DesactivarRegistroTotalDiasToolStripMenuItem, Me.EliminarDeLaBaseToolStripMenuItem, Me.CostoCeroToolStripMenuItem, Me.DesactivarCostoCeroToolStripMenuItem, Me.NoCalcularCostoSocialToolStripMenuItem, Me.DesactivarNoCalcularCostoSocialToolStripMenuItem})
        Me.cMenu.Name = "cMenu"
        Me.cMenu.Size = New System.Drawing.Size(255, 400)
        '
        'EliminarDeLaListaToolStripMenuItem
        '
        Me.EliminarDeLaListaToolStripMenuItem.Name = "EliminarDeLaListaToolStripMenuItem"
        Me.EliminarDeLaListaToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.EliminarDeLaListaToolStripMenuItem.Text = "Eliminar de la Lista"
        '
        'AgregarTrabajadoresToolStripMenuItem
        '
        Me.AgregarTrabajadoresToolStripMenuItem.Name = "AgregarTrabajadoresToolStripMenuItem"
        Me.AgregarTrabajadoresToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.AgregarTrabajadoresToolStripMenuItem.Text = "Agregar Trabajadores"
        '
        'EditarEmpleadoToolStripMenuItem
        '
        Me.EditarEmpleadoToolStripMenuItem.Name = "EditarEmpleadoToolStripMenuItem"
        Me.EditarEmpleadoToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.EditarEmpleadoToolStripMenuItem.Text = "Editar Empleado"
        '
        'NoCalcularInofnavitToolStripMenuItem
        '
        Me.NoCalcularInofnavitToolStripMenuItem.Name = "NoCalcularInofnavitToolStripMenuItem"
        Me.NoCalcularInofnavitToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.NoCalcularInofnavitToolStripMenuItem.Text = "No Calcular inofnavit"
        '
        'ActicarCalculoInfonavitToolStripMenuItem
        '
        Me.ActicarCalculoInfonavitToolStripMenuItem.Name = "ActicarCalculoInfonavitToolStripMenuItem"
        Me.ActicarCalculoInfonavitToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.ActicarCalculoInfonavitToolStripMenuItem.Text = "Activar Calculo Infonavit"
        '
        'NoCalcularPresAsiToolStripMenuItem
        '
        Me.NoCalcularPresAsiToolStripMenuItem.Name = "NoCalcularPresAsiToolStripMenuItem"
        Me.NoCalcularPresAsiToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.NoCalcularPresAsiToolStripMenuItem.Text = "No Calcular Pres Asi"
        '
        'ActivaCalculoPresAsiToolStripMenuItem
        '
        Me.ActivaCalculoPresAsiToolStripMenuItem.Name = "ActivaCalculoPresAsiToolStripMenuItem"
        Me.ActivaCalculoPresAsiToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.ActivaCalculoPresAsiToolStripMenuItem.Text = "Activa Calculo Pres Asi"
        '
        'NoCalcularPresSAToolStripMenuItem
        '
        Me.NoCalcularPresSAToolStripMenuItem.Name = "NoCalcularPresSAToolStripMenuItem"
        Me.NoCalcularPresSAToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.NoCalcularPresSAToolStripMenuItem.Text = "No Calcular Pres SA"
        '
        'ActivarCaluloPresSAToolStripMenuItem
        '
        Me.ActivarCaluloPresSAToolStripMenuItem.Name = "ActivarCaluloPresSAToolStripMenuItem"
        Me.ActivarCaluloPresSAToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.ActivarCaluloPresSAToolStripMenuItem.Text = "Activar Calulo Pres SA"
        '
        'SoloRegistroACalcularToolStripMenuItem
        '
        Me.SoloRegistroACalcularToolStripMenuItem.Name = "SoloRegistroACalcularToolStripMenuItem"
        Me.SoloRegistroACalcularToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.SoloRegistroACalcularToolStripMenuItem.Text = "Solo registro a calcular"
        '
        'DesactivarSoloRegistroACalcularToolStripMenuItem
        '
        Me.DesactivarSoloRegistroACalcularToolStripMenuItem.Name = "DesactivarSoloRegistroACalcularToolStripMenuItem"
        Me.DesactivarSoloRegistroACalcularToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.DesactivarSoloRegistroACalcularToolStripMenuItem.Text = "Desactivar solo registro a calcular"
        '
        'RegistroTotalDiasToolStripMenuItem
        '
        Me.RegistroTotalDiasToolStripMenuItem.Name = "RegistroTotalDiasToolStripMenuItem"
        Me.RegistroTotalDiasToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.RegistroTotalDiasToolStripMenuItem.Text = "Registro Total dias"
        '
        'DesactivarRegistroTotalDiasToolStripMenuItem
        '
        Me.DesactivarRegistroTotalDiasToolStripMenuItem.Name = "DesactivarRegistroTotalDiasToolStripMenuItem"
        Me.DesactivarRegistroTotalDiasToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.DesactivarRegistroTotalDiasToolStripMenuItem.Text = "Desactivar registro total dias"
        '
        'EliminarDeLaBaseToolStripMenuItem
        '
        Me.EliminarDeLaBaseToolStripMenuItem.Name = "EliminarDeLaBaseToolStripMenuItem"
        Me.EliminarDeLaBaseToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.EliminarDeLaBaseToolStripMenuItem.Text = "Eliminar de la base"
        '
        'CostoCeroToolStripMenuItem
        '
        Me.CostoCeroToolStripMenuItem.Name = "CostoCeroToolStripMenuItem"
        Me.CostoCeroToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.CostoCeroToolStripMenuItem.Text = "Costo cero"
        '
        'DesactivarCostoCeroToolStripMenuItem
        '
        Me.DesactivarCostoCeroToolStripMenuItem.Name = "DesactivarCostoCeroToolStripMenuItem"
        Me.DesactivarCostoCeroToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.DesactivarCostoCeroToolStripMenuItem.Text = "Desactivar costo cero"
        '
        'NoCalcularCostoSocialToolStripMenuItem
        '
        Me.NoCalcularCostoSocialToolStripMenuItem.Name = "NoCalcularCostoSocialToolStripMenuItem"
        Me.NoCalcularCostoSocialToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.NoCalcularCostoSocialToolStripMenuItem.Text = "No calcular Costo Social"
        '
        'DesactivarNoCalcularCostoSocialToolStripMenuItem
        '
        Me.DesactivarNoCalcularCostoSocialToolStripMenuItem.Name = "DesactivarNoCalcularCostoSocialToolStripMenuItem"
        Me.DesactivarNoCalcularCostoSocialToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.DesactivarNoCalcularCostoSocialToolStripMenuItem.Text = "Desactivar no calcular costo social"
        '
        'frmAdministrativos
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1357, 553)
        Me.Controls.Add(Me.cmdConcentradoFonacot)
        Me.Controls.Add(Me.cmdImssNomina)
        Me.Controls.Add(Me.chkNoinfonavit)
        Me.Controls.Add(Me.btnAsimilados)
        Me.Controls.Add(Me.cmdInfonavitNominaSerie)
        Me.Controls.Add(Me.cmdComision)
        Me.Controls.Add(Me.cmdReporteInfonavit)
        Me.Controls.Add(Me.cmdInfonavit)
        Me.Controls.Add(Me.layoutTimbrado)
        Me.Controls.Add(Me.pnlProgreso)
        Me.Controls.Add(Me.pnlCatalogo)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmAdministrativos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Administrativos"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlProgreso.ResumeLayout(False)
        Me.pnlProgreso.PerformLayout()
        Me.pnlCatalogo.ResumeLayout(False)
        Me.pnlCatalogo.PerformLayout()
        CType(Me.dtgDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.cMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdConcentradoFonacot As System.Windows.Forms.Button
    Friend WithEvents cmdImssNomina As System.Windows.Forms.Button
    Friend WithEvents chkNoinfonavit As System.Windows.Forms.CheckBox
    Friend WithEvents btnAsimilados As System.Windows.Forms.Button
    Friend WithEvents cmdInfonavitNominaSerie As System.Windows.Forms.Button
    Friend WithEvents cmdComision As System.Windows.Forms.Button
    Friend WithEvents cmdReporteInfonavit As System.Windows.Forms.Button
    Friend WithEvents cmdInfonavit As System.Windows.Forms.Button
    Friend WithEvents layoutTimbrado As System.Windows.Forms.Button
    Friend WithEvents pnlProgreso As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pgbProgreso As System.Windows.Forms.ProgressBar
    Friend WithEvents pnlCatalogo As System.Windows.Forms.Panel
    Friend WithEvents chkPrestamoSA As System.Windows.Forms.CheckBox
    Friend WithEvents cmdsoloisr As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cmdCalculoSoloInfonavit As System.Windows.Forms.Button
    Friend WithEvents cmdAcumuladoOperadora As System.Windows.Forms.Button
    Friend WithEvents cmdBuscarOtraNom As System.Windows.Forms.Button
    Friend WithEvents chkSoloCostoSocial As System.Windows.Forms.CheckBox
    Friend WithEvents chkNofonacot As System.Windows.Forms.CheckBox
    Friend WithEvents chkCalSoloMarcados As System.Windows.Forms.CheckBox
    Friend WithEvents chkPrestamosAsi As System.Windows.Forms.CheckBox
    Friend WithEvents chkInfonavit0 As System.Windows.Forms.CheckBox
    Friend WithEvents cmdResumenInfo As System.Windows.Forms.Button
    Friend WithEvents cmdSubirDatos As System.Windows.Forms.Button
    Friend WithEvents btnReporte As System.Windows.Forms.Button
    Friend WithEvents cboserie As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkgrupo As System.Windows.Forms.CheckBox
    Friend WithEvents chkinter As System.Windows.Forms.CheckBox
    Friend WithEvents cbobancos As System.Windows.Forms.ComboBox
    Friend WithEvents chkSindicato As System.Windows.Forms.CheckBox
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmdreiniciar As System.Windows.Forms.Button
    Friend WithEvents cmdincidencias As System.Windows.Forms.Button
    Friend WithEvents cmdexcel As System.Windows.Forms.Button
    Friend WithEvents cmdlayouts As System.Windows.Forms.Button
    Friend WithEvents cmdrecibosA As System.Windows.Forms.Button
    Friend WithEvents cmdguardarfinal As System.Windows.Forms.Button
    Friend WithEvents cmdguardarnomina As System.Windows.Forms.Button
    Friend WithEvents cmdcalcular As System.Windows.Forms.Button
    Friend WithEvents dtgDatos As System.Windows.Forms.DataGridView
    Friend WithEvents cmdverdatos As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboperiodo As System.Windows.Forms.ComboBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbEmpleados As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPeriodos As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbpuestos As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbdeptos As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbImportar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbIEmpleados As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbbuscar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbLayout As System.Windows.Forms.ToolStripButton
    Friend WithEvents cMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EliminarDeLaListaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AgregarTrabajadoresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditarEmpleadoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoCalcularInofnavitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActicarCalculoInfonavitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoCalcularPresAsiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActivaCalculoPresAsiToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoCalcularPresSAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActivarCaluloPresSAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SoloRegistroACalcularToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesactivarSoloRegistroACalcularToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RegistroTotalDiasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesactivarRegistroTotalDiasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EliminarDeLaBaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CostoCeroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesactivarCostoCeroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NoCalcularCostoSocialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesactivarNoCalcularCostoSocialToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
