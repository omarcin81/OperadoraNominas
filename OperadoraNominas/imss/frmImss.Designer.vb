<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPension
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmdAgregar = New System.Windows.Forms.Button()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.txtNumacuse = New System.Windows.Forms.TextBox()
        Me.rbModificación = New System.Windows.Forms.RadioButton()
        Me.rbBaja = New System.Windows.Forms.RadioButton()
        Me.rbAlta = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lsvHistorial = New System.Windows.Forms.ListView()
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lsvSalario = New System.Windows.Forms.ListView()
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.cmdIncapacidad = New System.Windows.Forms.Button()
        Me.lsvincapacidad = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtObservacion = New System.Windows.Forms.TextBox()
        Me.txtfolio = New System.Windows.Forms.TextBox()
        Me.cbotipo = New System.Windows.Forms.ComboBox()
        Me.dtpinicial = New System.Windows.Forms.DateTimePicker()
        Me.dtpfinal = New System.Windows.Forms.DateTimePicker()
        Me.NudDias = New System.Windows.Forms.NumericUpDown()
        Me.cboopcion = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.dtpriesgo = New System.Windows.Forms.DateTimePicker()
        Me.cboEstatus = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdBorrar = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.NudDias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.txtObservaciones)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cmdAgregar)
        Me.Panel1.Controls.Add(Me.dtpFecha)
        Me.Panel1.Controls.Add(Me.txtNumacuse)
        Me.Panel1.Controls.Add(Me.rbModificación)
        Me.Panel1.Controls.Add(Me.rbBaja)
        Me.Panel1.Controls.Add(Me.rbAlta)
        Me.Panel1.Location = New System.Drawing.Point(12, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(985, 63)
        Me.Panel1.TabIndex = 6
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(535, 5)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(103, 18)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "Observaciones:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(538, 25)
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(350, 26)
        Me.txtObservaciones.TabIndex = 32
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(371, 5)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 18)
        Me.Label7.TabIndex = 31
        Me.Label7.Text = "Acuse:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(262, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 18)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "Fecha:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(150, 18)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "Acuse del imss por tipo"
        '
        'cmdAgregar
        '
        Me.cmdAgregar.Location = New System.Drawing.Point(894, 26)
        Me.cmdAgregar.Name = "cmdAgregar"
        Me.cmdAgregar.Size = New System.Drawing.Size(79, 25)
        Me.cmdAgregar.TabIndex = 27
        Me.cmdAgregar.Text = "Agregar"
        Me.cmdAgregar.UseVisualStyleBackColor = True
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(265, 26)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(91, 26)
        Me.dtpFecha.TabIndex = 26
        '
        'txtNumacuse
        '
        Me.txtNumacuse.Location = New System.Drawing.Point(371, 25)
        Me.txtNumacuse.Name = "txtNumacuse"
        Me.txtNumacuse.Size = New System.Drawing.Size(152, 26)
        Me.txtNumacuse.TabIndex = 3
        '
        'rbModificación
        '
        Me.rbModificación.AutoSize = True
        Me.rbModificación.Enabled = False
        Me.rbModificación.Location = New System.Drawing.Point(146, 32)
        Me.rbModificación.Name = "rbModificación"
        Me.rbModificación.Size = New System.Drawing.Size(105, 22)
        Me.rbModificación.TabIndex = 2
        Me.rbModificación.TabStop = True
        Me.rbModificación.Text = "Modificación"
        Me.rbModificación.UseVisualStyleBackColor = True
        '
        'rbBaja
        '
        Me.rbBaja.AutoSize = True
        Me.rbBaja.Enabled = False
        Me.rbBaja.Location = New System.Drawing.Point(79, 32)
        Me.rbBaja.Name = "rbBaja"
        Me.rbBaja.Size = New System.Drawing.Size(52, 22)
        Me.rbBaja.TabIndex = 1
        Me.rbBaja.TabStop = True
        Me.rbBaja.Text = "Baja"
        Me.rbBaja.UseVisualStyleBackColor = True
        '
        'rbAlta
        '
        Me.rbAlta.AutoSize = True
        Me.rbAlta.Enabled = False
        Me.rbAlta.Location = New System.Drawing.Point(13, 32)
        Me.rbAlta.Name = "rbAlta"
        Me.rbAlta.Size = New System.Drawing.Size(51, 22)
        Me.rbAlta.TabIndex = 0
        Me.rbAlta.TabStop = True
        Me.rbAlta.Text = "Alta"
        Me.rbAlta.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lsvHistorial)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(12, 73)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(420, 224)
        Me.Panel2.TabIndex = 7
        '
        'lsvHistorial
        '
        Me.lsvHistorial.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader18})
        Me.lsvHistorial.FullRowSelect = True
        Me.lsvHistorial.GridLines = True
        Me.lsvHistorial.HideSelection = False
        Me.lsvHistorial.Location = New System.Drawing.Point(8, 32)
        Me.lsvHistorial.MultiSelect = False
        Me.lsvHistorial.Name = "lsvHistorial"
        Me.lsvHistorial.Size = New System.Drawing.Size(409, 181)
        Me.lsvHistorial.TabIndex = 4
        Me.lsvHistorial.UseCompatibleStateImageBehavior = False
        Me.lsvHistorial.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Clave"
        Me.ColumnHeader4.Width = 50
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Fecha. Sistema"
        Me.ColumnHeader5.Width = 107
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Fecha Imss"
        Me.ColumnHeader6.Width = 90
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Acuse"
        Me.ColumnHeader7.Width = 135
        '
        'ColumnHeader18
        '
        Me.ColumnHeader18.Text = "Observaciones"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 18)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Historial empleado"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lsvSalario)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Location = New System.Drawing.Point(438, 73)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(559, 224)
        Me.Panel3.TabIndex = 8
        '
        'lsvSalario
        '
        Me.lsvSalario.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader19})
        Me.lsvSalario.FullRowSelect = True
        Me.lsvSalario.GridLines = True
        Me.lsvSalario.HideSelection = False
        Me.lsvSalario.Location = New System.Drawing.Point(7, 29)
        Me.lsvSalario.MultiSelect = False
        Me.lsvSalario.Name = "lsvSalario"
        Me.lsvSalario.Size = New System.Drawing.Size(541, 184)
        Me.lsvSalario.TabIndex = 5
        Me.lsvSalario.UseCompatibleStateImageBehavior = False
        Me.lsvSalario.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Fecha"
        Me.ColumnHeader8.Width = 80
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Salario Diario"
        Me.ColumnHeader9.Width = 97
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Factor"
        Me.ColumnHeader10.Width = 62
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Salario Integrado"
        Me.ColumnHeader11.Width = 128
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Fecha Imss"
        Me.ColumnHeader1.Width = 90
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Acuse"
        Me.ColumnHeader2.Width = 70
        '
        'ColumnHeader19
        '
        Me.ColumnHeader19.Text = "Observaciones"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(131, 18)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Modificación salario"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.cmdBorrar)
        Me.Panel4.Controls.Add(Me.cmdIncapacidad)
        Me.Panel4.Controls.Add(Me.lsvincapacidad)
        Me.Panel4.Controls.Add(Me.txtObservacion)
        Me.Panel4.Controls.Add(Me.txtfolio)
        Me.Panel4.Controls.Add(Me.cbotipo)
        Me.Panel4.Controls.Add(Me.dtpinicial)
        Me.Panel4.Controls.Add(Me.dtpfinal)
        Me.Panel4.Controls.Add(Me.NudDias)
        Me.Panel4.Controls.Add(Me.cboopcion)
        Me.Panel4.Controls.Add(Me.Label14)
        Me.Panel4.Controls.Add(Me.Label13)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Location = New System.Drawing.Point(12, 303)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(985, 196)
        Me.Panel4.TabIndex = 9
        '
        'cmdIncapacidad
        '
        Me.cmdIncapacidad.Location = New System.Drawing.Point(792, 52)
        Me.cmdIncapacidad.Name = "cmdIncapacidad"
        Me.cmdIncapacidad.Size = New System.Drawing.Size(78, 25)
        Me.cmdIncapacidad.TabIndex = 33
        Me.cmdIncapacidad.Text = "Agregar"
        Me.cmdIncapacidad.UseVisualStyleBackColor = True
        '
        'lsvincapacidad
        '
        Me.lsvincapacidad.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader12, Me.ColumnHeader13, Me.ColumnHeader14, Me.ColumnHeader15, Me.ColumnHeader16, Me.ColumnHeader17})
        Me.lsvincapacidad.FullRowSelect = True
        Me.lsvincapacidad.GridLines = True
        Me.lsvincapacidad.Location = New System.Drawing.Point(13, 83)
        Me.lsvincapacidad.MultiSelect = False
        Me.lsvincapacidad.Name = "lsvincapacidad"
        Me.lsvincapacidad.Size = New System.Drawing.Size(960, 110)
        Me.lsvincapacidad.TabIndex = 32
        Me.lsvincapacidad.UseCompatibleStateImageBehavior = False
        Me.lsvincapacidad.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Folio"
        Me.ColumnHeader3.Width = 90
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Tipo Incapacidad"
        Me.ColumnHeader12.Width = 140
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Num Dias"
        Me.ColumnHeader13.Width = 80
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Fecha Inicial"
        Me.ColumnHeader14.Width = 100
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Fecha Final"
        Me.ColumnHeader15.Width = 100
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "Aplicada"
        Me.ColumnHeader16.Width = 80
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "Observaciones"
        Me.ColumnHeader17.Width = 360
        '
        'txtObservacion
        '
        Me.txtObservacion.Location = New System.Drawing.Point(113, 51)
        Me.txtObservacion.Name = "txtObservacion"
        Me.txtObservacion.Size = New System.Drawing.Size(673, 26)
        Me.txtObservacion.TabIndex = 31
        '
        'txtfolio
        '
        Me.txtfolio.Location = New System.Drawing.Point(59, 25)
        Me.txtfolio.Name = "txtfolio"
        Me.txtfolio.Size = New System.Drawing.Size(109, 26)
        Me.txtfolio.TabIndex = 30
        '
        'cbotipo
        '
        Me.cbotipo.FormattingEnabled = True
        Me.cbotipo.Items.AddRange(New Object() {"Riesgo de Trabajo", "Enfermedad", "Embarazo"})
        Me.cbotipo.Location = New System.Drawing.Point(212, 25)
        Me.cbotipo.Name = "cbotipo"
        Me.cbotipo.Size = New System.Drawing.Size(150, 26)
        Me.cbotipo.TabIndex = 29
        '
        'dtpinicial
        '
        Me.dtpinicial.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpinicial.Location = New System.Drawing.Point(454, 25)
        Me.dtpinicial.Name = "dtpinicial"
        Me.dtpinicial.Size = New System.Drawing.Size(91, 26)
        Me.dtpinicial.TabIndex = 28
        '
        'dtpfinal
        '
        Me.dtpfinal.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpfinal.Location = New System.Drawing.Point(623, 25)
        Me.dtpfinal.Name = "dtpfinal"
        Me.dtpfinal.Size = New System.Drawing.Size(91, 26)
        Me.dtpfinal.TabIndex = 27
        '
        'NudDias
        '
        Me.NudDias.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NudDias.Location = New System.Drawing.Point(785, 25)
        Me.NudDias.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.NudDias.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NudDias.Name = "NudDias"
        Me.NudDias.Size = New System.Drawing.Size(57, 27)
        Me.NudDias.TabIndex = 16
        Me.NudDias.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cboopcion
        '
        Me.cboopcion.FormattingEnabled = True
        Me.cboopcion.Items.AddRange(New Object() {"Si", "No"})
        Me.cboopcion.Location = New System.Drawing.Point(915, 26)
        Me.cboopcion.Name = "cboopcion"
        Me.cboopcion.Size = New System.Drawing.Size(58, 26)
        Me.cboopcion.TabIndex = 9
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(14, 54)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(103, 18)
        Me.Label14.TabIndex = 8
        Me.Label14.Text = "Observaciones:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(850, 29)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 18)
        Me.Label13.TabIndex = 7
        Me.Label13.Text = "Aplicada:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(545, 29)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(78, 18)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Fecha final:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(14, 29)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 18)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "Folio:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(174, 29)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(39, 18)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Tipo:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(713, 29)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 18)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = " Num dias:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(368, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 18)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Fecha inicial:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Incapacidad"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.ListView1)
        Me.Panel5.Controls.Add(Me.dtpriesgo)
        Me.Panel5.Controls.Add(Me.cboEstatus)
        Me.Panel5.Controls.Add(Me.Label19)
        Me.Panel5.Controls.Add(Me.Label15)
        Me.Panel5.Controls.Add(Me.Label6)
        Me.Panel5.Location = New System.Drawing.Point(12, 505)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(985, 194)
        Me.Panel5.TabIndex = 10
        '
        'ListView1
        '
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(12, 69)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(960, 120)
        Me.ListView1.TabIndex = 33
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'dtpriesgo
        '
        Me.dtpriesgo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpriesgo.Location = New System.Drawing.Point(265, 41)
        Me.dtpriesgo.Name = "dtpriesgo"
        Me.dtpriesgo.Size = New System.Drawing.Size(91, 26)
        Me.dtpriesgo.TabIndex = 31
        '
        'cboEstatus
        '
        Me.cboEstatus.FormattingEnabled = True
        Me.cboEstatus.Items.AddRange(New Object() {"Solicitado", "Enviado", "Proceso", "Calificado"})
        Me.cboEstatus.Location = New System.Drawing.Point(63, 41)
        Me.cboEstatus.Name = "cboEstatus"
        Me.cboEstatus.Size = New System.Drawing.Size(150, 26)
        Me.cboEstatus.TabIndex = 30
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(219, 44)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(48, 18)
        Me.Label19.TabIndex = 11
        Me.Label19.Text = "Fecha:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(11, 44)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(56, 18)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "Estatus:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(115, 18)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Riesgo de trabajo"
        '
        'cmdBorrar
        '
        Me.cmdBorrar.Location = New System.Drawing.Point(894, 54)
        Me.cmdBorrar.Name = "cmdBorrar"
        Me.cmdBorrar.Size = New System.Drawing.Size(77, 25)
        Me.cmdBorrar.TabIndex = 34
        Me.cmdBorrar.Text = "Borrar"
        Me.cmdBorrar.UseVisualStyleBackColor = True
        '
        'frmImss
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1003, 701)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmImss"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Imss"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.NudDias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtNumacuse As System.Windows.Forms.TextBox
    Friend WithEvents rbModificación As System.Windows.Forms.RadioButton
    Friend WithEvents rbBaja As System.Windows.Forms.RadioButton
    Friend WithEvents rbAlta As System.Windows.Forms.RadioButton
    Friend WithEvents cmdAgregar As System.Windows.Forms.Button
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lsvHistorial As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents lsvSalario As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cboopcion As System.Windows.Forms.ComboBox
    Friend WithEvents cbotipo As System.Windows.Forms.ComboBox
    Friend WithEvents dtpinicial As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpfinal As System.Windows.Forms.DateTimePicker
    Friend WithEvents NudDias As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtfolio As System.Windows.Forms.TextBox
    Friend WithEvents lsvincapacidad As System.Windows.Forms.ListView
    Friend WithEvents txtObservacion As System.Windows.Forms.TextBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents dtpriesgo As System.Windows.Forms.DateTimePicker
    Friend WithEvents cboEstatus As System.Windows.Forms.ComboBox
    Friend WithEvents cmdIncapacidad As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtObservaciones As System.Windows.Forms.TextBox
    Friend WithEvents ColumnHeader18 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader19 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdBorrar As System.Windows.Forms.Button
End Class
