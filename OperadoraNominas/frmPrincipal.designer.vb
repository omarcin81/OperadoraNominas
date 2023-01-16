<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrincipal
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPrincipal))
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Nomina Operadora"}, 11, System.Drawing.Color.Black, System.Drawing.Color.Empty, Nothing)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Importar Excel"}, 19, System.Drawing.Color.Black, System.Drawing.Color.Empty, Nothing)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Empleados"}, 9, System.Drawing.Color.Black, System.Drawing.Color.Empty, Nothing)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Reporte trabajadores"}, 15, System.Drawing.Color.Black, System.Drawing.Color.Empty, Nothing)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Prestamos"}, 13, System.Drawing.Color.Black, System.Drawing.Color.Empty, Nothing)
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Buscar Datos"}, 14, System.Drawing.Color.Black, System.Drawing.Color.Transparent, Nothing)
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Subir Nomina"}, "binario.png", System.Drawing.Color.Black, System.Drawing.Color.Transparent, Nothing)
        Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Calcular Ajuste"}, 1, System.Drawing.Color.Black, System.Drawing.Color.Transparent, Nothing)
        Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Nomina Admon"}, 12, System.Drawing.Color.Black, System.Drawing.Color.Transparent, Nothing)
        Me.pnlBar = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.chkCBB = New System.Windows.Forms.CheckBox()
        Me.chkCFDI = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblUsuario = New System.Windows.Forms.Label()
        Me.lsvPanel = New System.Windows.Forms.ListView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuInicio = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CatalogosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClientesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSalir = New System.Windows.Forms.ToolStripMenuItem()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pnlBar.SuspendLayout()
        Me.MenuInicio.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlBar
        '
        Me.pnlBar.Controls.Add(Me.CheckBox1)
        Me.pnlBar.Controls.Add(Me.chkCBB)
        Me.pnlBar.Controls.Add(Me.chkCFDI)
        Me.pnlBar.Controls.Add(Me.Label1)
        Me.pnlBar.Controls.Add(Me.lblUsuario)
        Me.pnlBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBar.Location = New System.Drawing.Point(0, 436)
        Me.pnlBar.Name = "pnlBar"
        Me.pnlBar.Size = New System.Drawing.Size(727, 41)
        Me.pnlBar.TabIndex = 5
        '
        'CheckBox1
        '
        Me.CheckBox1.Appearance = System.Windows.Forms.Appearance.Button
        Me.CheckBox1.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckBox1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CheckBox1.Location = New System.Drawing.Point(3, 1)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(84, 39)
        Me.CheckBox1.TabIndex = 6
        Me.CheckBox1.Text = "Menú"
        Me.CheckBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'chkCBB
        '
        Me.chkCBB.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkCBB.BackColor = System.Drawing.SystemColors.Control
        Me.chkCBB.Image = CType(resources.GetObject("chkCBB.Image"), System.Drawing.Image)
        Me.chkCBB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.chkCBB.Location = New System.Drawing.Point(418, 1)
        Me.chkCBB.Name = "chkCBB"
        Me.chkCBB.Size = New System.Drawing.Size(153, 39)
        Me.chkCBB.TabIndex = 4
        Me.chkCBB.Text = "Facturación CBB"
        Me.chkCBB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCBB.UseVisualStyleBackColor = True
        Me.chkCBB.Visible = False
        '
        'chkCFDI
        '
        Me.chkCFDI.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkCFDI.BackColor = System.Drawing.SystemColors.Control
        Me.chkCFDI.Image = CType(resources.GetObject("chkCFDI.Image"), System.Drawing.Image)
        Me.chkCFDI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.chkCFDI.Location = New System.Drawing.Point(264, 1)
        Me.chkCFDI.Name = "chkCFDI"
        Me.chkCFDI.Size = New System.Drawing.Size(153, 39)
        Me.chkCFDI.TabIndex = 3
        Me.chkCFDI.Text = "Facturación CFDI"
        Me.chkCFDI.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCFDI.UseVisualStyleBackColor = True
        Me.chkCFDI.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(91, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Usuario"
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.BackColor = System.Drawing.Color.Transparent
        Me.lblUsuario.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(145, Byte), Integer))
        Me.lblUsuario.Location = New System.Drawing.Point(146, 11)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(71, 20)
        Me.lblUsuario.TabIndex = 0
        Me.lblUsuario.Text = "Usuario"
        '
        'lsvPanel
        '
        Me.lsvPanel.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.lsvPanel.BackColor = System.Drawing.Color.White
        Me.lsvPanel.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lsvPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsvPanel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsvPanel.ForeColor = System.Drawing.Color.White
        Me.lsvPanel.FullRowSelect = True
        Me.lsvPanel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lsvPanel.HideSelection = False
        Me.lsvPanel.HoverSelection = True
        ListViewItem1.ToolTipText = "Calculo de Operadora"
        ListViewItem2.ToolTipText = "Exportar Excel  Nominas"
        ListViewItem3.ToolTipText = "Empleados"
        ListViewItem4.ToolTipText = "Reporte trabajadores"
        ListViewItem5.Tag = "Prestamos"
        ListViewItem7.ToolTipText = "Subir Nomina"
        ListViewItem8.ToolTipText = "Calcular Ajuste"
        ListViewItem9.ToolTipText = "Nomina Admon"
        Me.lsvPanel.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9})
        Me.lsvPanel.LargeImageList = Me.ImageList1
        Me.lsvPanel.Location = New System.Drawing.Point(0, 0)
        Me.lsvPanel.Name = "lsvPanel"
        Me.lsvPanel.ShowItemToolTips = True
        Me.lsvPanel.Size = New System.Drawing.Size(727, 477)
        Me.lsvPanel.TabIndex = 3
        Me.lsvPanel.UseCompatibleStateImageBehavior = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "invoice48x48.png")
        Me.ImageList1.Images.SetKeyName(1, "stock_certificate.png")
        Me.ImageList1.Images.SetKeyName(2, "Contents.png")
        Me.ImageList1.Images.SetKeyName(3, "1304468698_Gnome-Preferences-System-64.png")
        Me.ImageList1.Images.SetKeyName(4, "sales-report48x48.png")
        Me.ImageList1.Images.SetKeyName(5, "ChangeUser.png")
        Me.ImageList1.Images.SetKeyName(6, "1304468741_Gnome-Preferences-Other-64.png")
        Me.ImageList1.Images.SetKeyName(7, "preferences-desktop-wallpaper.png")
        Me.ImageList1.Images.SetKeyName(8, "User 7.png")
        Me.ImageList1.Images.SetKeyName(9, "User 5.png")
        Me.ImageList1.Images.SetKeyName(10, "InBox.png")
        Me.ImageList1.Images.SetKeyName(11, "Cash.png")
        Me.ImageList1.Images.SetKeyName(12, "1362225841_clients.png")
        Me.ImageList1.Images.SetKeyName(13, "1362226941_coins.png")
        Me.ImageList1.Images.SetKeyName(14, "1362227659_userconfig.png")
        Me.ImageList1.Images.SetKeyName(15, "atm-48.png")
        Me.ImageList1.Images.SetKeyName(16, "1474867410_upload.png")
        Me.ImageList1.Images.SetKeyName(17, "1474867386_advantage_cloud.png")
        Me.ImageList1.Images.SetKeyName(18, "1474867277_web.png")
        Me.ImageList1.Images.SetKeyName(19, "sobresalir (1).png")
        Me.ImageList1.Images.SetKeyName(20, "binario.png")
        '
        'MenuInicio
        '
        Me.MenuInicio.BackColor = System.Drawing.Color.Gainsboro
        Me.MenuInicio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.MenuInicio.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.MenuInicio.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuInicio.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.MenuInicio.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CatalogosToolStripMenuItem, Me.mnuSalir})
        Me.MenuInicio.Name = "MenuInicio"
        Me.MenuInicio.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.MenuInicio.Size = New System.Drawing.Size(203, 80)
        '
        'CatalogosToolStripMenuItem
        '
        Me.CatalogosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClientesToolStripMenuItem})
        Me.CatalogosToolStripMenuItem.Name = "CatalogosToolStripMenuItem"
        Me.CatalogosToolStripMenuItem.Size = New System.Drawing.Size(202, 38)
        Me.CatalogosToolStripMenuItem.Text = "Catalogos"
        '
        'ClientesToolStripMenuItem
        '
        Me.ClientesToolStripMenuItem.Name = "ClientesToolStripMenuItem"
        Me.ClientesToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.ClientesToolStripMenuItem.Text = "Empleados"
        '
        'mnuSalir
        '
        Me.mnuSalir.Image = CType(resources.GetObject("mnuSalir.Image"), System.Drawing.Image)
        Me.mnuSalir.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuSalir.Name = "mnuSalir"
        Me.mnuSalir.Size = New System.Drawing.Size(202, 38)
        Me.mnuSalir.Text = "Salir del sistema"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.Location = New System.Drawing.Point(334, 208)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(309, 213)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Visible = False
        '
        'frmPrincipal
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(727, 477)
        Me.Controls.Add(Me.pnlBar)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lsvPanel)
        Me.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Principal"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlBar.ResumeLayout(False)
        Me.pnlBar.PerformLayout()
        Me.MenuInicio.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlBar As System.Windows.Forms.Panel
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkCBB As System.Windows.Forms.CheckBox
    Friend WithEvents chkCFDI As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblUsuario As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lsvPanel As System.Windows.Forms.ListView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents MenuInicio As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSalir As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CatalogosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClientesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
