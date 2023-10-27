<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEligueMes
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
        Me.cboMes = New System.Windows.Forms.ComboBox()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.cboAnio = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'cboMes
        '
        Me.cboMes.FormattingEnabled = True
        Me.cboMes.Items.AddRange(New Object() {"SELECCIONE MES", "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"})
        Me.cboMes.Location = New System.Drawing.Point(12, 59)
        Me.cboMes.Name = "cboMes"
        Me.cboMes.Size = New System.Drawing.Size(187, 21)
        Me.cboMes.TabIndex = 0
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(51, 96)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(116, 27)
        Me.btnAceptar.TabIndex = 1
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'cboAnio
        '
        Me.cboAnio.FormattingEnabled = True
        Me.cboAnio.Items.AddRange(New Object() {"SELECCIONE AÑO", "2023", "2024", "2025"})
        Me.cboAnio.Location = New System.Drawing.Point(12, 32)
        Me.cboAnio.Name = "cboAnio"
        Me.cboAnio.Size = New System.Drawing.Size(187, 21)
        Me.cboAnio.TabIndex = 2
        '
        'frmEligueMes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(218, 135)
        Me.Controls.Add(Me.cboAnio)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.cboMes)
        Me.Name = "frmEligueMes"
        Me.Text = "EligueMes"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboMes As System.Windows.Forms.ComboBox
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents cboAnio As System.Windows.Forms.ComboBox
End Class
