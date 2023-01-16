<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIncapacidad
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIncapacidad))
        Me.tspOpciones = New System.Windows.Forms.ToolStrip()
        Me.tsbNuevo = New System.Windows.Forms.ToolStripButton()
        Me.tsbGuardar = New System.Windows.Forms.ToolStripButton()
        Me.tsbCancelar = New System.Windows.Forms.ToolStripButton()
        Me.tsbSalir = New System.Windows.Forms.ToolStripButton()
        Me.pnlDatos = New System.Windows.Forms.Panel()
        Me.cboriesgo = New System.Windows.Forms.ComboBox()
        Me.nudPorcentaje = New System.Windows.Forms.NumericUpDown()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.nudDias = New System.Windows.Forms.NumericUpDown()
        Me.txtFolio = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Folio = New System.Windows.Forms.Label()
        Me.cboRamoSeguro = New System.Windows.Forms.ComboBox()
        Me.cboTipo = New System.Windows.Forms.ComboBox()
        Me.lsvLista = New System.Windows.Forms.ListView()
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tspOpciones.SuspendLayout()
        Me.pnlDatos.SuspendLayout()
        CType(Me.nudPorcentaje, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tspOpciones
        '
        Me.tspOpciones.Dock = System.Windows.Forms.DockStyle.Right
        Me.tspOpciones.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tspOpciones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tspOpciones.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.tspOpciones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNuevo, Me.tsbGuardar, Me.tsbCancelar, Me.tsbSalir})
        Me.tspOpciones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.tspOpciones.Location = New System.Drawing.Point(456, 0)
        Me.tspOpciones.Name = "tspOpciones"
        Me.tspOpciones.Size = New System.Drawing.Size(71, 283)
        Me.tspOpciones.TabIndex = 14
        '
        'tsbNuevo
        '
        Me.tsbNuevo.Image = CType(resources.GetObject("tsbNuevo.Image"), System.Drawing.Image)
        Me.tsbNuevo.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.tsbNuevo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNuevo.Name = "tsbNuevo"
        Me.tsbNuevo.Size = New System.Drawing.Size(68, 55)
        Me.tsbNuevo.Text = "Nuevo"
        Me.tsbNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbGuardar
        '
        Me.tsbGuardar.Enabled = False
        Me.tsbGuardar.Image = CType(resources.GetObject("tsbGuardar.Image"), System.Drawing.Image)
        Me.tsbGuardar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbGuardar.Name = "tsbGuardar"
        Me.tsbGuardar.Size = New System.Drawing.Size(68, 55)
        Me.tsbGuardar.Text = "Guardar"
        Me.tsbGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbCancelar
        '
        Me.tsbCancelar.Enabled = False
        Me.tsbCancelar.Image = CType(resources.GetObject("tsbCancelar.Image"), System.Drawing.Image)
        Me.tsbCancelar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCancelar.Name = "tsbCancelar"
        Me.tsbCancelar.Size = New System.Drawing.Size(68, 55)
        Me.tsbCancelar.Text = "Cancelar"
        Me.tsbCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.tsbCancelar.ToolTipText = "Cancelar la captura de datos"
        '
        'tsbSalir
        '
        Me.tsbSalir.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbSalir.Image = CType(resources.GetObject("tsbSalir.Image"), System.Drawing.Image)
        Me.tsbSalir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSalir.Name = "tsbSalir"
        Me.tsbSalir.Size = New System.Drawing.Size(68, 55)
        Me.tsbSalir.Text = "Salir"
        Me.tsbSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'pnlDatos
        '
        Me.pnlDatos.Controls.Add(Me.cboriesgo)
        Me.pnlDatos.Controls.Add(Me.nudPorcentaje)
        Me.pnlDatos.Controls.Add(Me.dtpFechaInicio)
        Me.pnlDatos.Controls.Add(Me.nudDias)
        Me.pnlDatos.Controls.Add(Me.txtFolio)
        Me.pnlDatos.Controls.Add(Me.Label6)
        Me.pnlDatos.Controls.Add(Me.Label5)
        Me.pnlDatos.Controls.Add(Me.Label4)
        Me.pnlDatos.Controls.Add(Me.Label3)
        Me.pnlDatos.Controls.Add(Me.Label2)
        Me.pnlDatos.Controls.Add(Me.Label1)
        Me.pnlDatos.Controls.Add(Me.Folio)
        Me.pnlDatos.Controls.Add(Me.cboRamoSeguro)
        Me.pnlDatos.Controls.Add(Me.cboTipo)
        Me.pnlDatos.Enabled = False
        Me.pnlDatos.Location = New System.Drawing.Point(7, 5)
        Me.pnlDatos.Name = "pnlDatos"
        Me.pnlDatos.Size = New System.Drawing.Size(441, 160)
        Me.pnlDatos.TabIndex = 26
        '
        'cboriesgo
        '
        Me.cboriesgo.FormattingEnabled = True
        Me.cboriesgo.Items.AddRange(New Object() {"Accidente", "Enfermedad"})
        Me.cboriesgo.Location = New System.Drawing.Point(10, 108)
        Me.cboriesgo.Name = "cboriesgo"
        Me.cboriesgo.Size = New System.Drawing.Size(99, 26)
        Me.cboriesgo.TabIndex = 39
        '
        'nudPorcentaje
        '
        Me.nudPorcentaje.DecimalPlaces = 2
        Me.nudPorcentaje.Location = New System.Drawing.Point(131, 109)
        Me.nudPorcentaje.Name = "nudPorcentaje"
        Me.nudPorcentaje.Size = New System.Drawing.Size(111, 26)
        Me.nudPorcentaje.TabIndex = 38
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(125, 56)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(105, 26)
        Me.dtpFechaInicio.TabIndex = 37
        '
        'nudDias
        '
        Me.nudDias.Location = New System.Drawing.Point(10, 56)
        Me.nudDias.Name = "nudDias"
        Me.nudDias.Size = New System.Drawing.Size(99, 26)
        Me.nudDias.TabIndex = 36
        '
        'txtFolio
        '
        Me.txtFolio.Location = New System.Drawing.Point(8, 4)
        Me.txtFolio.Name = "txtFolio"
        Me.txtFolio.Size = New System.Drawing.Size(100, 26)
        Me.txtFolio.TabIndex = 35
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(128, 135)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(114, 18)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "% de incapacidad"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(122, 83)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 18)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "Fecha de inicio"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 135)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 18)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "Tipo de riesgo:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(233, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 18)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Ramo de seguro:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(108, 18)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "Días autorizados"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(122, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 18)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Tipo de incidencia"
        '
        'Folio
        '
        Me.Folio.AutoSize = True
        Me.Folio.Location = New System.Drawing.Point(7, 31)
        Me.Folio.Name = "Folio"
        Me.Folio.Size = New System.Drawing.Size(39, 18)
        Me.Folio.TabIndex = 28
        Me.Folio.Text = "Folio"
        '
        'cboRamoSeguro
        '
        Me.cboRamoSeguro.FormattingEnabled = True
        Me.cboRamoSeguro.Items.AddRange(New Object() {"Riesgo de trabajo", "Enfermedad general", "Maternidad"})
        Me.cboRamoSeguro.Location = New System.Drawing.Point(236, 56)
        Me.cboRamoSeguro.Name = "cboRamoSeguro"
        Me.cboRamoSeguro.Size = New System.Drawing.Size(188, 26)
        Me.cboRamoSeguro.TabIndex = 27
        '
        'cboTipo
        '
        Me.cboTipo.FormattingEnabled = True
        Me.cboTipo.Items.AddRange(New Object() {"Accidente de trabajo", "Accidente de trayecto", "Enfermedad general", "Incapacidad pagada por la empresa", "Incapacidad por maternidad"})
        Me.cboTipo.Location = New System.Drawing.Point(125, 4)
        Me.cboTipo.Name = "cboTipo"
        Me.cboTipo.Size = New System.Drawing.Size(299, 26)
        Me.cboTipo.TabIndex = 26
        '
        'lsvLista
        '
        Me.lsvLista.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader12, Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lsvLista.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvLista.FullRowSelect = True
        Me.lsvLista.GridLines = True
        Me.lsvLista.HideSelection = False
        Me.lsvLista.Location = New System.Drawing.Point(17, 171)
        Me.lsvLista.MultiSelect = False
        Me.lsvLista.Name = "lsvLista"
        Me.lsvLista.Size = New System.Drawing.Size(414, 106)
        Me.lsvLista.TabIndex = 41
        Me.lsvLista.UseCompatibleStateImageBehavior = False
        Me.lsvLista.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Folio"
        Me.ColumnHeader12.Width = 100
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Tipo"
        Me.ColumnHeader1.Width = 200
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Días"
        '
        'frmIncapacidad
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(527, 283)
        Me.Controls.Add(Me.lsvLista)
        Me.Controls.Add(Me.pnlDatos)
        Me.Controls.Add(Me.tspOpciones)
        Me.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmIncapacidad"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Incapacidad"
        Me.tspOpciones.ResumeLayout(False)
        Me.tspOpciones.PerformLayout()
        Me.pnlDatos.ResumeLayout(False)
        Me.pnlDatos.PerformLayout()
        CType(Me.nudPorcentaje, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tspOpciones As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNuevo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbGuardar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCancelar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents pnlDatos As System.Windows.Forms.Panel
    Friend WithEvents cboriesgo As System.Windows.Forms.ComboBox
    Friend WithEvents nudPorcentaje As System.Windows.Forms.NumericUpDown
    Friend WithEvents dtpFechaInicio As System.Windows.Forms.DateTimePicker
    Friend WithEvents nudDias As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtFolio As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Folio As System.Windows.Forms.Label
    Friend WithEvents cboRamoSeguro As System.Windows.Forms.ComboBox
    Friend WithEvents cboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents lsvLista As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader

End Class
