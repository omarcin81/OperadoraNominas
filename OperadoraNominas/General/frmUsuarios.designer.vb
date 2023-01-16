<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsuarios
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsuarios))
        Me.grpUsuarios = New System.Windows.Forms.GroupBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboSucursal = New System.Windows.Forms.ComboBox()
        Me.cboPerfil = New System.Windows.Forms.ComboBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tspOpciones = New System.Windows.Forms.ToolStrip()
        Me.tsbNuevo = New System.Windows.Forms.ToolStripButton()
        Me.tsbGuardar = New System.Windows.Forms.ToolStripButton()
        Me.tsbCancelar = New System.Windows.Forms.ToolStripButton()
        Me.tsbSalir = New System.Windows.Forms.ToolStripButton()
        Me.lsvUsuarios = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.chkVer = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboEstatus = New System.Windows.Forms.ComboBox()
        Me.grpUsuarios.SuspendLayout()
        Me.tspOpciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpUsuarios
        '
        Me.grpUsuarios.Controls.Add(Me.cboEstatus)
        Me.grpUsuarios.Controls.Add(Me.Label5)
        Me.grpUsuarios.Controls.Add(Me.chkVer)
        Me.grpUsuarios.Controls.Add(Me.txtPassword)
        Me.grpUsuarios.Controls.Add(Me.Label4)
        Me.grpUsuarios.Controls.Add(Me.cboSucursal)
        Me.grpUsuarios.Controls.Add(Me.cboPerfil)
        Me.grpUsuarios.Controls.Add(Me.txtNombre)
        Me.grpUsuarios.Controls.Add(Me.Label3)
        Me.grpUsuarios.Controls.Add(Me.Label2)
        Me.grpUsuarios.Controls.Add(Me.Label1)
        Me.grpUsuarios.Enabled = False
        Me.grpUsuarios.Location = New System.Drawing.Point(2, -4)
        Me.grpUsuarios.Name = "grpUsuarios"
        Me.grpUsuarios.Size = New System.Drawing.Size(356, 168)
        Me.grpUsuarios.TabIndex = 0
        Me.grpUsuarios.TabStop = False
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(75, 107)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(241, 27)
        Me.txtPassword.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 108)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 19)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Password"
        '
        'cboSucursal
        '
        Me.cboSucursal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSucursal.FormattingEnabled = True
        Me.cboSucursal.Location = New System.Drawing.Point(75, 76)
        Me.cboSucursal.Name = "cboSucursal"
        Me.cboSucursal.Size = New System.Drawing.Size(275, 27)
        Me.cboSucursal.TabIndex = 5
        '
        'cboPerfil
        '
        Me.cboPerfil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPerfil.FormattingEnabled = True
        Me.cboPerfil.Location = New System.Drawing.Point(75, 43)
        Me.cboPerfil.Name = "cboPerfil"
        Me.cboPerfil.Size = New System.Drawing.Size(275, 27)
        Me.cboPerfil.TabIndex = 4
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(75, 14)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(275, 27)
        Me.txtNombre.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Sucursal"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Perfil"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre"
        '
        'tspOpciones
        '
        Me.tspOpciones.Dock = System.Windows.Forms.DockStyle.Right
        Me.tspOpciones.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tspOpciones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tspOpciones.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.tspOpciones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbNuevo, Me.tsbGuardar, Me.tsbCancelar, Me.tsbSalir})
        Me.tspOpciones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.tspOpciones.Location = New System.Drawing.Point(364, 0)
        Me.tspOpciones.Name = "tspOpciones"
        Me.tspOpciones.Size = New System.Drawing.Size(71, 315)
        Me.tspOpciones.TabIndex = 1
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
        'lsvUsuarios
        '
        Me.lsvUsuarios.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lsvUsuarios.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvUsuarios.FullRowSelect = True
        Me.lsvUsuarios.GridLines = True
        Me.lsvUsuarios.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lsvUsuarios.HideSelection = False
        Me.lsvUsuarios.Location = New System.Drawing.Point(2, 167)
        Me.lsvUsuarios.MultiSelect = False
        Me.lsvUsuarios.Name = "lsvUsuarios"
        Me.lsvUsuarios.Size = New System.Drawing.Size(355, 144)
        Me.lsvUsuarios.SmallImageList = Me.ImageList1
        Me.lsvUsuarios.TabIndex = 2
        Me.lsvUsuarios.UseCompatibleStateImageBehavior = False
        Me.lsvUsuarios.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Width = 324
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "User 16x16Gray.png")
        Me.ImageList1.Images.SetKeyName(1, "User 16x16.png")
        '
        'chkVer
        '
        Me.chkVer.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkVer.Image = CType(resources.GetObject("chkVer.Image"), System.Drawing.Image)
        Me.chkVer.Location = New System.Drawing.Point(319, 107)
        Me.chkVer.Name = "chkVer"
        Me.chkVer.Size = New System.Drawing.Size(27, 26)
        Me.chkVer.TabIndex = 8
        Me.chkVer.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 140)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 19)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Estatus"
        '
        'cboEstatus
        '
        Me.cboEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstatus.FormattingEnabled = True
        Me.cboEstatus.Items.AddRange(New Object() {"Activo", "Inactivo"})
        Me.cboEstatus.Location = New System.Drawing.Point(75, 138)
        Me.cboEstatus.Name = "cboEstatus"
        Me.cboEstatus.Size = New System.Drawing.Size(123, 27)
        Me.cboEstatus.TabIndex = 10
        '
        'frmUsuarios
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(435, 315)
        Me.Controls.Add(Me.lsvUsuarios)
        Me.Controls.Add(Me.tspOpciones)
        Me.Controls.Add(Me.grpUsuarios)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUsuarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Usuarios"
        Me.grpUsuarios.ResumeLayout(False)
        Me.grpUsuarios.PerformLayout()
        Me.tspOpciones.ResumeLayout(False)
        Me.tspOpciones.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpUsuarios As System.Windows.Forms.GroupBox
    Friend WithEvents tspOpciones As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbNuevo As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbGuardar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCancelar As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents cboSucursal As System.Windows.Forms.ComboBox
    Friend WithEvents cboPerfil As System.Windows.Forms.ComboBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lsvUsuarios As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents chkVer As System.Windows.Forms.CheckBox
    Friend WithEvents cboEstatus As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
