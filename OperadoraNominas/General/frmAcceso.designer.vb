<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAcceso
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAcceso))
        Me.lsvUsuario = New System.Windows.Forms.ListView()
        Me.Usuarios = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtClave = New System.Windows.Forms.TextBox()
        Me.cmdEntrar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lsvUsuario
        '
        Me.lsvUsuario.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Usuarios})
        Me.lsvUsuario.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvUsuario.FullRowSelect = True
        Me.lsvUsuario.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lsvUsuario.Location = New System.Drawing.Point(4, 12)
        Me.lsvUsuario.MultiSelect = False
        Me.lsvUsuario.Name = "lsvUsuario"
        Me.lsvUsuario.Size = New System.Drawing.Size(407, 178)
        Me.lsvUsuario.SmallImageList = Me.ImageList1
        Me.lsvUsuario.TabIndex = 0
        Me.lsvUsuario.UseCompatibleStateImageBehavior = False
        Me.lsvUsuario.View = System.Windows.Forms.View.Details
        '
        'Usuarios
        '
        Me.Usuarios.Text = "Usuarios"
        Me.Usuarios.Width = 375
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "user1_16.png")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 199)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Clave"
        '
        'txtClave
        '
        Me.txtClave.Location = New System.Drawing.Point(53, 197)
        Me.txtClave.Name = "txtClave"
        Me.txtClave.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClave.Size = New System.Drawing.Size(278, 27)
        Me.txtClave.TabIndex = 4
        '
        'cmdEntrar
        '
        Me.cmdEntrar.Location = New System.Drawing.Point(340, 196)
        Me.cmdEntrar.Name = "cmdEntrar"
        Me.cmdEntrar.Size = New System.Drawing.Size(70, 28)
        Me.cmdEntrar.TabIndex = 5
        Me.cmdEntrar.Text = "Entrar"
        Me.cmdEntrar.UseVisualStyleBackColor = True
        '
        'frmAcceso
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(417, 231)
        Me.Controls.Add(Me.cmdEntrar)
        Me.Controls.Add(Me.txtClave)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lsvUsuario)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAcceso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Acceso al sistema"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lsvUsuario As System.Windows.Forms.ListView
    Friend WithEvents Usuarios As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtClave As System.Windows.Forms.TextBox
    Friend WithEvents cmdEntrar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
