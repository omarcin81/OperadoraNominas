<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEstatusPres
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
        Me.cmdAceptar = New System.Windows.Forms.Button()
        Me.cmdCancelar = New System.Windows.Forms.Button()
        Me.rdbStatus = New System.Windows.Forms.RadioButton()
        Me.rdbStatus2 = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'cmdAceptar
        '
        Me.cmdAceptar.Location = New System.Drawing.Point(29, 70)
        Me.cmdAceptar.Name = "cmdAceptar"
        Me.cmdAceptar.Size = New System.Drawing.Size(75, 29)
        Me.cmdAceptar.TabIndex = 1
        Me.cmdAceptar.Text = "Aceptar"
        Me.cmdAceptar.UseVisualStyleBackColor = True
        '
        'cmdCancelar
        '
        Me.cmdCancelar.Location = New System.Drawing.Point(162, 70)
        Me.cmdCancelar.Name = "cmdCancelar"
        Me.cmdCancelar.Size = New System.Drawing.Size(75, 29)
        Me.cmdCancelar.TabIndex = 2
        Me.cmdCancelar.Text = "Cancelar"
        Me.cmdCancelar.UseVisualStyleBackColor = True
        '
        'rdbStatus
        '
        Me.rdbStatus.AutoSize = True
        Me.rdbStatus.Location = New System.Drawing.Point(29, 23)
        Me.rdbStatus.Name = "rdbStatus"
        Me.rdbStatus.Size = New System.Drawing.Size(100, 22)
        Me.rdbStatus.TabIndex = 4
        Me.rdbStatus.TabStop = True
        Me.rdbStatus.Text = "Solo Activos"
        Me.rdbStatus.UseVisualStyleBackColor = True
        '
        'rdbStatus2
        '
        Me.rdbStatus2.AutoSize = True
        Me.rdbStatus2.Location = New System.Drawing.Point(162, 23)
        Me.rdbStatus2.Name = "rdbStatus2"
        Me.rdbStatus2.Size = New System.Drawing.Size(130, 22)
        Me.rdbStatus2.TabIndex = 5
        Me.rdbStatus2.TabStop = True
        Me.rdbStatus2.Text = "Activos/Inactivos"
        Me.rdbStatus2.UseVisualStyleBackColor = True
        '
        'frmEstatusPres
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(308, 126)
        Me.Controls.Add(Me.rdbStatus2)
        Me.Controls.Add(Me.rdbStatus)
        Me.Controls.Add(Me.cmdCancelar)
        Me.Controls.Add(Me.cmdAceptar)
        Me.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmEstatusPres"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reporte Prestamo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdAceptar As System.Windows.Forms.Button
    Friend WithEvents cmdCancelar As System.Windows.Forms.Button
    Friend WithEvents rdbStatus As System.Windows.Forms.RadioButton
    Friend WithEvents rdbStatus2 As System.Windows.Forms.RadioButton
End Class
