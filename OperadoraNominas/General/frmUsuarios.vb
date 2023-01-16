Public Class frmUsuarios
    Dim blnNuevo As Boolean = False

    Private Sub tsbCerrar_Click(sender As System.Object, e As System.EventArgs)

    End Sub





    Private Sub tsbSalir_Click(sender As System.Object, e As System.EventArgs) Handles tsbSalir.Click
        Me.Close()
    End Sub

    Private Sub tsbNuevo_Click(sender As System.Object, e As System.EventArgs) Handles tsbNuevo.Click
        grpUsuarios.Enabled = True
        blnNuevo = True
    End Sub

    Private Sub grpUsuarios_EnabledChanged(sender As Object, e As System.EventArgs) Handles grpUsuarios.EnabledChanged
        tsbNuevo.Enabled = Not grpUsuarios.Enabled
        tsbGuardar.Enabled = grpUsuarios.Enabled
        tsbCancelar.Enabled = grpUsuarios.Enabled
        lsvUsuarios.Enabled = Not grpUsuarios.Enabled
        txtNombre.Text = ""
        txtPassword.Text = ""
        cboEstatus.SelectedIndex = -1
        cboSucursal.SelectedIndex = -1
        cboPerfil.SelectedIndex = -1
    End Sub

    Private Sub grpUsuarios_Enter(sender As System.Object, e As System.EventArgs) Handles grpUsuarios.Enter

    End Sub

    Private Sub frmUsuarios_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        MostrarUsuarios()
        CargarCatalogos()
    End Sub

    Private Sub MostrarUsuarios()
        Dim SQL As String
        SQL = "SELECT Nombre, IdUsuario, Password, fkIdPerfil, Status, fkIdSucursal"
        SQL &= " FROM Usuarios  ORDER BY  Nombre"
        Dim Clientes As DataRow() = nConsulta(SQL)
        lsvUsuarios.Items.Clear()
        If Clientes Is Nothing = False Then
            lsvUsuarios.Items.AddRange((From Fila In Clientes Select New ListViewItem((From campo In Fila.ItemArray Select CType(campo, String)).ToArray(), Integer.Parse(Fila!Status.ToString()))).ToArray())
        End If
    End Sub

    Private Sub CargarCatalogos()
        Dim SQL As String
        SQL = "SELECT IdPerfil, Descripcion FROM CatPerfiles WHERE  Status=1 ORDER BY IdPerfil"
        nCargaCBO(cboPerfil, SQL)

        SQL = "SELECT IdTienda,Nombre FROM CatTiendas WHERE Status=1 ORDER BY Nombre"
        nCargaCBO(cboSucursal, SQL, "Nombre", "IdTienda")
    End Sub

    Private Sub tsbGuardar_Click(sender As System.Object, e As System.EventArgs) Handles tsbGuardar.Click
        Dim Mensaje As String = ""
        Dim SQL As String = "SELECT "

        If txtNombre.Text.Trim.Length = 0 Then
            Mensaje = "Por favor indique el nombre del usuario."
        End If
        If Mensaje = "" And cboPerfil.SelectedIndex < 0 Then
            Mensaje = "Por favor indique el perfil del usuario."
        End If
        If Mensaje = "" And cboSucursal.SelectedIndex < 0 Then
            Mensaje = "Por favor indique la sucursal del usuario."
        End If
        If Mensaje = "" And txtPassword.Text.Trim.Length = 0 Then
            Mensaje = "Escriba algun password para el usuario"
        End If
        If Mensaje = "" And cboEstatus.SelectedIndex < 0 Then
            Mensaje = "Por favor indique el estatus del usuario."
        End If
        If Mensaje = "" And blnNuevo Then
            SQL = "SELECT IdUsuario FROM Usuarios WHERE Nombre='" & txtNombre.Text & "'"
            If Val(ObtencampoDR(SQL, 0)) > 0 Then
                Mensaje = "Ya existe usuario con el nombre que ha indicado."
            End If
        End If
        If Mensaje <> "" Then
            MessageBox.Show(Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If blnNuevo Then
            SQL = "INSERT INTO Usuarios (Nombre, Password, fkIdPerfil, Status, fkIdSucursal)"
            SQL &= " VALUES('" & txtNombre.Text & "','" & txtPassword.Text & "'," & cboPerfil.SelectedValue & "," & IIf(cboEstatus.SelectedIndex = 0, 1, 0) & "," & cboSucursal.SelectedValue & ")"
        Else
            SQL = "UPDATE Usuarios SET "
            SQL &= "Nombre='" & txtNombre.Text & "', Password='" & txtPassword.Text & "',"
            SQL &= "fkIdPerfil=" & cboPerfil.SelectedValue & ",Status=" & IIf(cboEstatus.SelectedIndex = 0, 1, 0) & ",fkIdSucursal=" & cboSucursal.SelectedValue
            SQL &= " WHERE IdUsuario=" & lsvUsuarios.SelectedItems(0).SubItems(1).Text
        End If
        If nExecute(SQL) = False Then
            Exit Sub
        End If
        MessageBox.Show("Los datos del usuario se han guardado correctamente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        grpUsuarios.Enabled = False
        MostrarUsuarios()
    End Sub

    Private Sub lsvUsuarios_ItemActivate(sender As Object, e As System.EventArgs) Handles lsvUsuarios.ItemActivate
        If lsvUsuarios.SelectedItems.Count > 0 Then
            Dim item As ListViewItem = lsvUsuarios.SelectedItems(0)
            grpUsuarios.Enabled = True
            blnNuevo = False

            txtNombre.Text = item.SubItems(0).Text
            txtPassword.Text = item.SubItems(2).Text
            cboPerfil.SelectedValue = item.SubItems(3).Text
            cboSucursal.SelectedValue = item.SubItems(5).Text
            cboEstatus.SelectedIndex = IIf(item.SubItems(4).Text = "1", 0, 1)
        End If
    End Sub

    Private Sub lsvUsuarios_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lsvUsuarios.SelectedIndexChanged

    End Sub

    Private Sub tsbCancelar_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancelar.Click
        grpUsuarios.Enabled = False
    End Sub

    Private Sub chkVer_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkVer.CheckedChanged
        txtPassword.PasswordChar = IIf(chkVer.Checked, "", "*")
    End Sub
End Class