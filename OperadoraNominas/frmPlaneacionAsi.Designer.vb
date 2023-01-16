<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPlaneacionAsi
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
        Me.Button2 = New System.Windows.Forms.Button()
        Me.NudFinal = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdcalcular = New System.Windows.Forms.Button()
        Me.NudInicio = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmdCerrar = New System.Windows.Forms.Button()
        Me.lblRuta = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboperiodo = New System.Windows.Forms.ComboBox()
        Me.lsvLista = New System.Windows.Forms.ListView()
        Me.pnlProgreso = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pgbProgreso = New System.Windows.Forms.ProgressBar()
        Me.lsvAcumulado = New System.Windows.Forms.ListView()
        Me.cmdProcesar = New System.Windows.Forms.Button()
        CType(Me.NudFinal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NudInicio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlProgreso.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(860, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(144, 36)
        Me.Button2.TabIndex = 83
        Me.Button2.Text = "Descargar Archivo"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'NudFinal
        '
        Me.NudFinal.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NudFinal.Location = New System.Drawing.Point(437, 12)
        Me.NudFinal.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.NudFinal.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NudFinal.Name = "NudFinal"
        Me.NudFinal.Size = New System.Drawing.Size(73, 27)
        Me.NudFinal.TabIndex = 78
        Me.NudFinal.Value = New Decimal(New Integer() {12, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(337, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 19)
        Me.Label6.TabIndex = 77
        Me.Label6.Text = "Periodo final:"
        '
        'cmdcalcular
        '
        Me.cmdcalcular.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdcalcular.Location = New System.Drawing.Point(540, 6)
        Me.cmdcalcular.Name = "cmdcalcular"
        Me.cmdcalcular.Size = New System.Drawing.Size(148, 36)
        Me.cmdcalcular.TabIndex = 74
        Me.cmdcalcular.Text = "Mostrar datos"
        Me.cmdcalcular.UseVisualStyleBackColor = True
        '
        'NudInicio
        '
        Me.NudInicio.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NudInicio.Location = New System.Drawing.Point(258, 12)
        Me.NudInicio.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.NudInicio.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NudInicio.Name = "NudInicio"
        Me.NudInicio.Size = New System.Drawing.Size(73, 27)
        Me.NudInicio.TabIndex = 73
        Me.NudInicio.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(138, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 19)
        Me.Label3.TabIndex = 72
        Me.Label3.Text = "Periodo de inicio:"
        '
        'cmdCerrar
        '
        Me.cmdCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdCerrar.Location = New System.Drawing.Point(1062, 560)
        Me.cmdCerrar.Name = "cmdCerrar"
        Me.cmdCerrar.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.cmdCerrar.Size = New System.Drawing.Size(104, 43)
        Me.cmdCerrar.TabIndex = 71
        Me.cmdCerrar.Text = "Cerrar"
        Me.cmdCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCerrar.UseVisualStyleBackColor = True
        '
        'lblRuta
        '
        Me.lblRuta.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRuta.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRuta.Location = New System.Drawing.Point(8, 560)
        Me.lblRuta.Name = "lblRuta"
        Me.lblRuta.Size = New System.Drawing.Size(604, 39)
        Me.lblRuta.TabIndex = 70
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 19)
        Me.Label1.TabIndex = 66
        Me.Label1.Text = "Año:"
        '
        'cboperiodo
        '
        Me.cboperiodo.FormattingEnabled = True
        Me.cboperiodo.Items.AddRange(New Object() {"2021", "2022", "2023", "2024"})
        Me.cboperiodo.Location = New System.Drawing.Point(49, 12)
        Me.cboperiodo.Name = "cboperiodo"
        Me.cboperiodo.Size = New System.Drawing.Size(80, 27)
        Me.cboperiodo.TabIndex = 65
        '
        'lsvLista
        '
        Me.lsvLista.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lsvLista.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvLista.FullRowSelect = True
        Me.lsvLista.GridLines = True
        Me.lsvLista.HideSelection = False
        Me.lsvLista.Location = New System.Drawing.Point(11, 48)
        Me.lsvLista.MultiSelect = False
        Me.lsvLista.Name = "lsvLista"
        Me.lsvLista.Size = New System.Drawing.Size(1147, 375)
        Me.lsvLista.TabIndex = 5
        Me.lsvLista.UseCompatibleStateImageBehavior = False
        Me.lsvLista.View = System.Windows.Forms.View.Details
        '
        'pnlProgreso
        '
        Me.pnlProgreso.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlProgreso.Controls.Add(Me.Label2)
        Me.pnlProgreso.Controls.Add(Me.pgbProgreso)
        Me.pnlProgreso.Location = New System.Drawing.Point(361, 260)
        Me.pnlProgreso.Name = "pnlProgreso"
        Me.pnlProgreso.Size = New System.Drawing.Size(449, 84)
        Me.pnlProgreso.TabIndex = 84
        Me.pnlProgreso.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(154, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Procesando Calculos"
        '
        'pgbProgreso
        '
        Me.pgbProgreso.Location = New System.Drawing.Point(17, 12)
        Me.pgbProgreso.Name = "pgbProgreso"
        Me.pgbProgreso.Size = New System.Drawing.Size(413, 30)
        Me.pgbProgreso.TabIndex = 0
        '
        'lsvAcumulado
        '
        Me.lsvAcumulado.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lsvAcumulado.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvAcumulado.FullRowSelect = True
        Me.lsvAcumulado.GridLines = True
        Me.lsvAcumulado.HideSelection = False
        Me.lsvAcumulado.Location = New System.Drawing.Point(11, 429)
        Me.lsvAcumulado.MultiSelect = False
        Me.lsvAcumulado.Name = "lsvAcumulado"
        Me.lsvAcumulado.Size = New System.Drawing.Size(1147, 117)
        Me.lsvAcumulado.TabIndex = 85
        Me.lsvAcumulado.UseCompatibleStateImageBehavior = False
        Me.lsvAcumulado.View = System.Windows.Forms.View.Details
        '
        'cmdProcesar
        '
        Me.cmdProcesar.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdProcesar.Location = New System.Drawing.Point(694, 6)
        Me.cmdProcesar.Name = "cmdProcesar"
        Me.cmdProcesar.Size = New System.Drawing.Size(160, 36)
        Me.cmdProcesar.TabIndex = 86
        Me.cmdProcesar.Text = "Generar Acumulados"
        Me.cmdProcesar.UseVisualStyleBackColor = True
        '
        'frmPlaneacionAsi
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1170, 605)
        Me.Controls.Add(Me.cmdProcesar)
        Me.Controls.Add(Me.lsvAcumulado)
        Me.Controls.Add(Me.pnlProgreso)
        Me.Controls.Add(Me.lsvLista)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.NudFinal)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmdcalcular)
        Me.Controls.Add(Me.NudInicio)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdCerrar)
        Me.Controls.Add(Me.lblRuta)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboperiodo)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmPlaneacionAsi"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Calculo asimilados"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.NudFinal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NudInicio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlProgreso.ResumeLayout(False)
        Me.pnlProgreso.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents NudFinal As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmdcalcular As System.Windows.Forms.Button
    Friend WithEvents NudInicio As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdCerrar As System.Windows.Forms.Button
    Friend WithEvents lblRuta As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboperiodo As System.Windows.Forms.ComboBox
    Friend WithEvents lsvLista As System.Windows.Forms.ListView
    Friend WithEvents pnlProgreso As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pgbProgreso As System.Windows.Forms.ProgressBar
    Friend WithEvents lsvAcumulado As System.Windows.Forms.ListView
    Friend WithEvents cmdProcesar As System.Windows.Forms.Button
End Class
