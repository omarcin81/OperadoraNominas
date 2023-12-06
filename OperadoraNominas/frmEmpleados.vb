Imports System.Text.RegularExpressions
Imports ClosedXML.Excel

Public Class frmEmpleados
    Dim SQL As String
    Dim blnNuevo As Boolean
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String
    Public gIdPeriodo As String
    Public gIdTipoPuesto As String

    Private Sub cmdguardar_Click(sender As Object, e As EventArgs) Handles cmdguardar.Click
        Dim SQL As String, Mensaje As String = ""
        Try
            'Validar
            If txtcodigo.Text.Trim.Length = 0 And Mensaje = "" Then
                Mensaje = "Por favor indique el codigo a guardar"
            End If
            If txtnombre.Text.Trim.Length = 0 And Mensaje = "" Then
                Mensaje = "Por favor indique el nombre del trabajador"
            End If
            If txtpaterno.Text.Trim.Length = 0 And Mensaje = "" Then
                Mensaje = "Indique Apellido paterno"
            End If
            'If txtmaterno.Text.Trim.Length = 0 And Mensaje = "" Then
            '    Mensaje = "Indique Apellido materno"
            'End If


            'If txtcorreo.Text.Trim.Length > 0 And Mensaje = "" Then
            '    If Not Regex.IsMatch(txtcorreo.Text, "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$") Then
            '        Mensaje = "El email no tiene una forma correcta de correo electrónico (usuario@dominio.com)."
            '        Me.txtcorreo.Focus()
            '    End If
            'End If


            If Mensaje <> "" Then
                MessageBox.Show(Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Validar si ya esta el codigo del empleado
            If blnNuevo Then
                SQL = "select * from empleadosC where cCodigoEmpleado=" & txtcodigo.Text
                Dim rwCodigo As DataRow() = nConsulta(SQL)

                If rwCodigo Is Nothing = False Then
                    MessageBox.Show("El codigo de empleado ya existe por favor verifique", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub

                End If
            End If


            'Agregar datos de sueldos para historial


      
            If blnNuevo Then


                SQL = "select max(iIdEmpleadoC) as id from empleadosC"
                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then
                    Dim Fila As DataRow = rwFilas(0)
                    SQL = "EXEC setSueldoAltaInsertar  0," & IIf(txtsalario.Text = "", 0, txtsalario.Text) & ",'" & Format(dtppatrona.Value.Date, "yyyy/dd/MM")
                    SQL += "',0,''," & IIf(txtsd.Text = "", 0, txtsd.Text) & "," & IIf(txtsdi.Text = "", 0, txtsdi.Text) & "," & Fila.Item("id")
                    SQL += ",'01/01/1900',''," & txtExtra.Text

                End If
                'Se le agrego
            Else

                'verificamos el cambio de algun dato
                SQL = "select * from empleadosC where iIdEmpleadoC = " & gIdEmpleado
                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then

                    Dim Fila As DataRow = rwFilas(0)
                    If Fila.Item("fSueldoOrd") <> IIf(txtsalario.Text = "", 0, txtsalario.Text) Or Fila.Item("fSueldoBase") <> IIf(txtsd.Text = "", 0, txtsd.Text) Or Fila.Item("fSueldoIntegrado") <> IIf(txtsdi.Text = "", 0, txtsdi.Text) Then

                        SQL = "EXEC setSueldoAltaInsertar  0," & IIf(txtsalario.Text = "", 0, txtsalario.Text) & ",'" & Date.Today.ToShortDateString()
                        SQL += "',0,''," & IIf(txtsd.Text = "", 0, txtsd.Text) & "," & IIf(txtsdi.Text = "", 0, txtsdi.Text) & "," & gIdEmpleado
                        SQL += ",'01/01/1900',''," & txtExtra.Text


                        ' Enviar_Mail(GenerarCorreo1(gIdEmpresa, gIdCliente, txtcodigo.Text), correo, "Cambio en sueldo")
                    End If


                End If
            End If

            If SQL <> "" Then
                If nExecute(SQL) = False Then
                    Exit Sub
                End If
            End If



            '---


            If blnNuevo Then
                'Insertar nuevo
                SQL = "EXEC setempleadosCInsertar 0,'" & txtcodigo.Text & "','" & txtnombre.Text
                SQL &= "','" & txtpaterno.Text
                SQL &= "','" & txtmaterno.Text & "','" & txtpaterno.Text & " " & txtmaterno.Text & " " & txtnombre.Text
                SQL &= "','" & txtrfc.Text & "','" & txtcurp.Text & "','" & txtimss.Text
                SQL &= "','" & txtdireccion.Text
                SQL &= "','" & txtciudad.Text & "'," & cboestado.SelectedValue & ",'" & txtcp.Text
                SQL &= "'," & cbosexo.SelectedIndex & ",'" & Format(dtpfechanac.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpCaptura.Value.Date, "yyyy/dd/MM")
                SQL &= "','" & cbopuesto.Text & "','" & txtfunciones.Text
                SQL &= "'," & IIf(txtsd.Text = "", 0, txtsd.Text) & "," & IIf(txtsdi.Text = "", 0, txtsdi.Text)
                SQL &= ",'','" & txtnacionalidad.Text & "','','','" & txtduracion.Text & "','" & txtcomentarios.Text
                SQL &= "',1," & IIf(txtsalario.Text = "", 0, txtsalario.Text) & ",0" & ",-1" & "," & cbopertenece.SelectedIndex + 1 & "," & cbobanco.SelectedValue
                SQL &= ",'" & txtcuenta.Text & "',1,'" & txtdireccionP.Text
                SQL &= "','" & txtciudadP.Text & "'," & cboestadoP.SelectedValue & ",'" & txtcp2.Text
                SQL &= "','" & Format(dtppatrona.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpsindicato.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpantiguedad.Value.Date, "yyyy/dd/MM")
                SQL &= "'," & IIf(chkInfonavit.Checked, 1, 0) & ",'" & txtclabe.Text & "','" & txtintegrar.Text
                SQL &= "'," & cbocategoria.SelectedIndex & ",'" & txtcredito.Text & "','" & cbotipofactor.Text
                SQL &= "'," & IIf(txtfactor.Text = "", 0, txtfactor.Text) & ",'" & cbojornada.Text & "','" & txtcorreo.Text
                SQL &= "','" & txthorario.Text & "','" & txthoras.Text & "','" & txtdescanso.Text & "'," & IIf(cbostatus.SelectedIndex = 0, 1, 0)
                SQL &= "," & cbopuesto.SelectedValue & "," & cbodepartamento.SelectedValue
                SQL &= "," & cboedocivil.SelectedIndex
                SQL &= "," & cbobanco2.SelectedValue
                SQL &= ",'" & cboExcedente.Text
                SQL &= "','" & txtclabe2.Text & "'"
                SQL &= "," & IIf(txtExtra.Text = "", 0, txtExtra.Text) & ",'" & Format(dtFecPlanta.Value.Date, "yyyy/dd/MM") & "','" & IIf(chkvales.Checked, "1", "0") & "','" & txtFin.Text & "'"
                SQL &= ", '" & txtTelefono.Text & "','" & Format(dtpFinContrato.Value.Date, "yyyy/dd/MM") & "'"
            Else
                'Actualizar

                SQL = "EXEC setempleadosCActualizar  " & gIdEmpleado & ",'" & txtcodigo.Text & "','" & txtnombre.Text
                SQL &= "','" & txtpaterno.Text
                SQL &= "','" & txtmaterno.Text & "','" & txtpaterno.Text & " " & txtmaterno.Text & " " & txtnombre.Text
                SQL &= "','" & txtrfc.Text & "','" & txtcurp.Text & "','" & txtimss.Text
                SQL &= "','" & txtdireccion.Text
                SQL &= "','" & txtciudad.Text & "'," & cboestado.SelectedValue & ",'" & txtcp.Text
                SQL &= "'," & cbosexo.SelectedIndex & ",'" & Format(dtpfechanac.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpCaptura.Value.Date, "yyyy/dd/MM")
                SQL &= "','" & cbopuesto.Text & "','" & txtfunciones.Text
                SQL &= "'," & IIf(txtsd.Text = "", 0, txtsd.Text) & "," & IIf(txtsdi.Text = "", 0, txtsdi.Text)
                SQL &= ",'','" & txtnacionalidad.Text & "','','','" & txtduracion.Text & "','" & txtcomentarios.Text
                SQL &= "',1," & IIf(txtsalario.Text = "", 0, txtsalario.Text) & ",0" & ",-1" & "," & cbopertenece.SelectedIndex + 1 & "," & cbobanco.SelectedValue
                SQL &= ",'" & txtcuenta.Text & "',1,'" & txtdireccionP.Text
                SQL &= "','" & txtciudadP.Text & "'," & cboestadoP.SelectedValue & ",'" & txtcp2.Text
                SQL &= "','" & Format(dtppatrona.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpsindicato.Value.Date, "yyyy/dd/MM") & "','" & Format(dtpantiguedad.Value.Date, "yyyy/dd/MM")
                SQL &= "'," & IIf(chkInfonavit.Checked, 1, 0) & ",'" & txtclabe.Text & "','" & txtintegrar.Text
                SQL &= "'," & cbocategoria.SelectedIndex & ",'" & txtcredito.Text & "','" & cbotipofactor.Text
                SQL &= "'," & IIf(txtfactor.Text = "", 0, txtfactor.Text) & ",'" & cbojornada.Text & "','" & txtcorreo.Text
                SQL &= "','" & txthorario.Text & "','" & txthoras.Text & "','" & txtdescanso.Text & "'," & IIf(cbostatus.SelectedIndex = 0, 1, 0)
                SQL &= "," & cbopuesto.SelectedValue & "," & cbodepartamento.SelectedValue
                SQL &= "," & cboedocivil.SelectedIndex
                SQL &= "," & cbobanco2.SelectedValue
                SQL &= ",'" & cboExcedente.Text
                SQL &= "','" & txtclabe2.Text & "'"
                SQL &= "," & IIf(txtExtra.Text = "", 0, txtExtra.Text) & ",'" & Format(dtFecPlanta.Value.Date, "yyyy/dd/MM") & "','" & IIf(chkvales.Checked, "1", "0") & "','" & txtFin.Text & "'"
                SQL &= ", '" & txtTelefono.Text & "','" & Format(dtpFinContrato.Value.Date, "yyyy/dd/MM") & "'"

            End If
            If nExecute(SQL) = False Then
                Exit Sub
            End If

            If blnNuevo Then
                'Obtener id
                SQL = "select max(iIdEmpleadoC) as id from empleadosC"
                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then
                    Dim Fila As DataRow = rwFilas(0)
                    SQL = "EXEC setIngresoBajaAltaInsertar  0," & Fila.Item("id") & ",'" & IIf(cbostatus.SelectedIndex = 0, "A", "B") & "','" & Format(dtppatrona.Value.Date, "yyyy/dd/MM") & "','01/01/1900','',''"
                    'Enviar correo

                   End If


            Else
                SQL = "select * from IngresoBajaAlta"
                SQL &= " where iIdIngresoBaja= (select max(iIdIngresoBaja) "
                SQL &= " as maximo from IngresoBajaAlta where fkiIdEmpleado =" & gIdEmpleado & ")"


                Dim rwFilas As DataRow() = nConsulta(SQL)

                If rwFilas Is Nothing = False Then
                    SQL = ""
                    Dim Fila As DataRow = rwFilas(0)
                    If Fila.Item("Clave") <> IIf(cbostatus.SelectedIndex = 0, "A", "B") Then

                        SQL = "EXEC setIngresoBajaAltaInsertar  0," & gIdEmpleado & ",'" & IIf(cbostatus.SelectedIndex = 0, "A", "B") & "','" & Date.Today.ToShortDateString & "','01/01/1900','',''"

                       
                    End If


                End If


            End If

            If SQL <> "" Then
                If nExecute(SQL) = False Then
                    Exit Sub
                End If
            End If





            gIdEmpleado = ""

            gIdCliente = ""


            MessageBox.Show("Datos Guardados correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Limpiar(Me)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdcancelar_Click(sender As Object, e As EventArgs) Handles cmdcancelar.Click
        If blnNuevo Then
            'Cargar los datos anteriores
        Else
            Limpiar(Me)
        End If
    End Sub

    Private Sub cmdbuscar_Click(sender As Object, e As EventArgs) Handles cmdbuscar.Click
        Dim Forma As New frmBuscarEmpleado
        Forma.gIdEmpresa = gIdEmpresa
        If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
            gIdEmpleado = Forma.gIdEmpleado
            MostrarEmpleado(Forma.gIdEmpleado)

        End If
    End Sub
    Private Sub MostrarEmpleado(idempleado As String)
        SQL = "select * from empleadosC where iIdEmpleadoC = " & idempleado
        Dim rwFilas As DataRow() = nConsulta(SQL)
        Try
            If rwFilas Is Nothing = False Then


                Dim Fila As DataRow = rwFilas(0)
                cbostatus.SelectedIndex = IIf(Fila.Item("fkiIdClienteInter") = 1, 0, 1)
                txtcodigo.Text = Fila.Item("cCodigoEmpleado")
                txtnombre.Text = Fila.Item("cNombre")
                txtpaterno.Text = Fila.Item("cApellidoP")
                txtmaterno.Text = Fila.Item("cApellidoM")
                Dim fechanac As Date
                fechanac = Fila.Item("dFechaNac")
                dtpfechanac.Value = Fila.Item("dFechaNac")
                Dim edad As Integer = DateDiff(DateInterval.Year, fechanac, Date.Today)
                txtedad.Text = edad.ToString()

                Dim sexo As String = IIf(Fila.Item("iSexo") = "0", "Femenino", "Masculino")
                cbosexo.SelectedIndex = Fila.Item("iSexo")
                'item.SubItems.Add("" & sexo)
                'Dim civil As String = IIf(Fila.Item("iOrigen") = "0", "Soltero", "Casado")
                cboedocivil.SelectedIndex = Integer.Parse(Fila.Item("iEstadoCivil"))
                'item.SubItems.Add("" & civil)
                cbopertenece.SelectedIndex = Integer.Parse(Fila.Item("iOrigen")) - 1
                'item.SubItems.Add("" & Fila.Item("cPuesto"))
                txtfunciones.Text = Fila.Item("cFuncionesPuesto")
                'item.SubItems.Add("" & Fila.Item("cFuncionesPuesto"))
                cbocategoria.SelectedIndex = Fila.Item("iCategoria")
                'Dim Categoria As String = IIf(Fila.Item("iCategoria") = "0", "A", "B")
                'item.SubItems.Add("" & Categoria)
                dtppatrona.Value = Fila.Item("dFechaPatrona")
                'item.SubItems.Add("" & Fila.Item("dFechaPatrona"))
                dtpsindicato.Value = Fila.Item("dFechaSindicato")
                'item.SubItems.Add("" & Fila.Item("dFechaSindicato"))
                txtintegrar.Text = Fila.Item("cIntegrar")
                'item.SubItems.Add("" & Fila.Item("cIntegrar"))
                txtsd.Text = Fila.Item("fSueldoBase")
                'item.SubItems.Add("" & Fila.Item("fSueldoBase"))
                txtsdi.Text = Fila.Item("fSueldoIntegrado")
                'item.SubItems.Add("" & Fila.Item("fSueldoIntegrado"))

                'item.SubItems.Add("" & Fila.Item("dFechaNac"))
                txtcurp.Text = Fila.Item("cCURP")
                'item.SubItems.Add("" & Fila.Item("cCURP"))
                txtrfc.Text = Fila.Item("cRFC")
                'item.SubItems.Add("" & Fila.Item("cRFC"))
                txtimss.Text = Fila.Item("cIMSS")
                'item.SubItems.Add("" & Fila.Item("cIMSS"))
                chkInfonavit.Checked = IIf(Fila.Item("iPermanente") = "1", True, False)
                'item.SubItems.Add("" & IIf(Fila.Item("iPermanente") = "0", "No", "Si"))
                txtcredito.Text = Fila.Item("cInfonavit")
                'item.SubItems.Add("" & Fila.Item("cInfonavit"))
                'If Fila.Item("cInfonavit") <> Nothing Then
                '    chkInfonavit.Checked = True
                'Else
                '    chkInfonavit.Checked = False
                'End If
                cbotipofactor.Text = Convert.ToString(Fila.Item("cTipoFactor"))
                'item.SubItems.Add("" & Fila.Item("cTipoFactor"))
                txtfactor.Text = Convert.ToString(Fila.Item("fFactor"))
                'item.SubItems.Add("" & Fila.Item("fFactor"))
                txtcuenta.Text = Convert.ToString((Fila.Item("NumCuenta")))
                'item.SubItems.Add("" & Fila.Item("NumCuenta"))
                txtclabe.Text = Convert.ToString(Fila.Item("Clabe"))
                'item.SubItems.Add("" & Fila.Item("Clabe"))

                SQL = "select * from bancos where iIdBanco=" & Fila.Item("fkiIdBanco")
                Dim Banco As DataRow() = nConsulta(SQL)
                cbobanco.SelectedValue = Banco(0).Item("iIdBanco")
                'item.SubItems.Add("" & Banco(0).Item("cBanco"))
                txtnacionalidad.Text = Fila.Item("cNacionalidad")
                'item.SubItems.Add("" & Fila.Item("cNacionalidad"))
                txtdireccion.Text = Fila.Item("cDireccion")
                'item.SubItems.Add("" & Fila.Item("cDireccion"))
                txtciudad.Text = Fila.Item("cCiudadL")
                'item.SubItems.Add("" & Fila.Item("cCiudadL"))

                SQL = "select * from Cat_Estados where iIdEstado=" & Fila.Item("fkiIdEstado")
                Dim Estado As DataRow() = nConsulta(SQL)
                cboestado.SelectedValue = Estado(0).Item("iIdEstado")
                'item.SubItems.Add("" & Estado(0).Item("cEstado"))
                txtcp.Text = Fila.Item("cCP")
                'item.SubItems.Add("" & Fila.Item("cCP"))
                dtpantiguedad.Value = Fila.Item("dFechaAntiguedad")
                'item.SubItems.Add("" & Fila.Item("dFechaAntiguedad"))
                txtdireccionP.Text = Fila.Item("cDireccionP")
                txtciudadP.Text = Fila.Item("cCiudadP")
                'item.SubItems.Add("" & Fila.Item("cDireccionP") & "" & Fila.Item("cCiudadP"))
                txtduracion.Text = Fila.Item("cDuracion")
                'item.SubItems.Add("" & Fila.Item("cDuracion"))
                cbojornada.Text = Fila.Item("cJornada")
                'item.SubItems.Add("" & Fila.Item("cJornada"))
                txtcomentarios.Text = Fila.Item("cObservaciones")
                'item.SubItems.Add("" & Fila.Item("cObservaciones"))
                txtcorreo.Text = Fila.Item("cCorreo")
                'item.SubItems.Add("" & Fila.Item("cCorreo"))
                txthorario.Text = Fila.Item("cHorario")
                'item.SubItems.Add("" & Fila.Item("cHorario"))
                txthoras.Text = Fila.Item("cHoras")
                'item.SubItems.Add("" & Fila.Item("cHoras"))
                txtdescanso.Text = Fila.Item("cDescanso")
                'item.SubItems.Add("" & Fila.Item("cDescanso"))
                'cboClientes.SelectedValue = Fila.Item("fkiIdClienteInter")
                cbopuesto.SelectedValue = Fila.Item("fkiIdPuesto")
                cbodepartamento.SelectedValue = Fila.Item("fkiIdDepartamento")

                cboExcedente.Text = Fila.Item("cuenta2")
                'item.SubItems.Add("" & Fila.Item("NumCuenta"))
                txtclabe2.Text = Fila.Item("clabe2")
                'item.SubItems.Add("" & Fila.Item("Clabe"))
                txtsalario.Text = Fila.Item("fSueldoOrd")
                SQL = "select * from bancos where iIdBanco=" & Fila.Item("fkiIdBanco2")
                Dim Banco2 As DataRow() = nConsulta(SQL)
                cbobanco2.SelectedValue = Banco2(0).Item("iIdBanco")

                txtExtra.Text = Convert.ToString(Fila.Item("fsindicatoExtra"))

                dtFecPlanta.Value = IIf(Convert.ToString(Fila.Item("dFechaPlanta")) = "", Date.Now.ToShortDateString, Convert.ToString(Fila.Item("dFechaPlanta")))
                chkvales.Checked = IIf(Fila.Item("cInicioEmbarque") = "1", True, False)
                'txtInicio.Text = Fila.Item("cInicioEmbarque")
                txtFin.Text = Fila.Item("cFinalEmbarque")

                blnNuevo = False
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub cmdsalir_Click(sender As Object, e As EventArgs) Handles cmdsalir.Click
        Me.Close()
    End Sub

    Private Sub Limpiar(ByVal Contenedor As Object)
        For Each oControl In Contenedor.Controls
            If TypeOf oControl Is TabControl Or TypeOf oControl Is GroupBox Or TypeOf oControl Is Panel Then
                Limpiar(oControl)
            ElseIf TypeOf oControl Is TextBox Then
                Dim txtControl As TextBox = oControl
                txtControl.Text = ""
                txtControl.Tag = ""
            ElseIf TypeOf oControl Is ComboBox Then
                Dim cboControl As ComboBox = oControl
                cboControl.SelectedIndex = -1
                cboControl.Text = ""
            ElseIf TypeOf oControl Is ListView Then
                Dim Lista As ListView = oControl
                Lista.Items.Clear()
            ElseIf TypeOf oControl Is DateTimePicker Then
                Dim dtpControl As DateTimePicker = oControl
                dtpControl.Value = Date.Now

            End If
            chkInfonavit.Checked = False

        Next

        'cboautorizacion.SelectedIndex = 0
        cbobanco.SelectedIndex = 0
        cbobanco2.SelectedIndex = 0

        cbocategoria.SelectedIndex = 0

        cboedocivil.SelectedIndex = 0
        cboestado.SelectedIndex = 0
        cboestadoP.SelectedIndex = 0
        cbojornada.SelectedIndex = 0
        cbosexo.SelectedIndex = 0
        cbostatus.SelectedIndex = 0
        cbotipofactor.SelectedIndex = 0
        'cboclientefiscal.SelectedIndex = 0
        cbodepartamento.SelectedIndex = 0
        cbopuesto.SelectedIndex = 0
        'cboClientes.SelectedIndex = 0



    End Sub

    Private Sub frmEmpleados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MostrarEstados()
        MostrarEstadosP()
        MostrarBancos()
        MostrarBancos2()
        MostrarEmpresa()
        'MostrarCliente()
        'MostrarClienteS()
        MostrarPuesto()
        MostrarDepartamentos()
        blnNuevo = True
        IndexTab()

        'blnNuevo = gIdEmpleado = ""


        If gIdEmpleado = "" Then
            blnNuevo = True
            'cboautorizacion.SelectedIndex = 0
            cbobanco.SelectedIndex = 0
            cbobanco2.SelectedIndex = 0
            cbocategoria.SelectedIndex = 0

            cboedocivil.SelectedIndex = 0
            cboestado.SelectedIndex = 0
            cboestadoP.SelectedIndex = 0
            cbojornada.SelectedIndex = 0
            cbosexo.SelectedIndex = 0
            cbostatus.SelectedIndex = 0
            cbotipofactor.SelectedIndex = 0
            'cboclientefiscal.SelectedIndex = 0
            cbodepartamento.SelectedIndex = 0
            cbopuesto.SelectedIndex = 0
            'cboClientes.SelectedIndex = 0
            cbopertenece.SelectedIndex = 0
            'Limpiar(Me)
        Else
            MsgBox("avisar de este mensaje al administrador, empleado mostrar")

            blnNuevo = False
            'MostrarEmpleado()
        End If
    End Sub

    Private Sub IndexTab()
        cbostatus.TabIndex = 1
        txtcodigo.TabIndex = 2
        dtpCaptura.TabIndex = 3
        cbopertenece.TabIndex = 4
        txtpaterno.TabIndex = 5
        txtmaterno.TabIndex = 6
        txtnombre.TabIndex = 7
        cbosexo.TabIndex = 8
        cboedocivil.TabIndex = 9
        cbopuesto.TabIndex = 10
        txtfunciones.TabIndex = 11
        cbocategoria.TabIndex = 12
        dtppatrona.TabIndex = 13
        dtpsindicato.TabIndex = 14
        txtintegrar.TabIndex = 15
        txtsd.TabIndex = 16
        txtsdi.TabIndex = 17
        dtpfechanac.TabIndex = 18
        txtedad.TabIndex = 19
        txtcurp.TabIndex = 20
        txtrfc.TabIndex = 21
        txtimss.TabIndex = 22
        dtpantiguedad.TabIndex = 23
        txtTelefono.TabIndex = 24
        txtcredito.TabIndex = 25
        cbotipofactor.TabIndex = 26
        txtfactor.TabIndex = 27
        chkInfonavit.TabIndex = 28
        txtcuenta.TabIndex = 29
        txtclabe.TabIndex = 30
        cbobanco.TabIndex = 31
        txtnacionalidad.TabIndex = 32
        gpb1.TabIndex = 33
        txtdireccion.TabIndex = 34
        txtciudad.TabIndex = 35
        cboestado.TabIndex = 36
        txtcp.TabIndex = 37
        gpb2.TabIndex = 38
        txtdireccionP.TabIndex = 39
        txtciudadP.TabIndex = 40
        cboestadoP.TabIndex = 41
        txtcp2.TabIndex = 42
        txtduracion.TabIndex = 43
        cbojornada.TabIndex = 44
        txtsalario.TabIndex = 45
        txtcomentarios.TabIndex = 46
        txtcorreo.TabIndex = 47
        txthorario.TabIndex = 48
        txthoras.TabIndex = 49
        txtdescanso.TabIndex = 50
        cbodepartamento.TabIndex = 51
        txtExtra.TabIndex = 52
        dtFecPlanta.TabIndex = 53
        dtpFinContrato.TabIndex = 54
        cboExcedente.TabIndex = 55
        txtclabe2.TabIndex = 56
        cbobanco2.TabIndex = 57
        chkvales.TabIndex = 58
        'txtInicio.TabIndex = 58
        txtFin.TabIndex = 59



    End Sub

    Private Sub MostrarDepartamentos()

        'If gIdTipoPuesto = 0 Then
        '    SQL = "Select * from departamentos"
        'Else
        '    SQL = "Select * from departamentos where iIdDepartamento=" & gIdTipoPuesto
        'End If
        SQL = "Select * from departamentos"

        If gIdTipoPuesto = 0 Then
            SQL = "Select * from departamentos"
        Else
            SQL = "Select * from departamentos where iEstatus=1" '' & gIdTipoPuesto
        End If

        SQL &= " order by cnombre"
        nCargaCBO(cbodepartamento, SQL, "cnombre", "iIdDepartamento")
        cbodepartamento.SelectedIndex = 0
        'cboClientes.SelectedValue = gIdCliente
    End Sub
    Private Sub MostrarPuesto()
        'SQL = "select * from UsuarioClientes inner join IntClienteEmpresa"
        'SQL &= " on UsuarioClientes.fkiIdCliente= IntClienteEmpresa.fkIdCliente"
        'SQL &= " where UsuarioClientes.fkiIdEmpleado =" & idUsuario
        'SQL &= " And IntClienteEmpresa.fkIdEmpresa =" & gIdEmpresa

        'Dim rwFilas As DataRow() = nConsulta(SQL)
        'Try
        '    If rwFilas Is Nothing = False Then

        '        Dim Fila As DataRow = rwFilas(0)

        If gIdTipoPuesto = 0 Then
            SQL = "Select * from Puestos "
        Else
            SQL = "Select * from Puestos where iTipo=" & gIdTipoPuesto
        End If

        SQL &= " order by cnombre"
        nCargaCBO(cbopuesto, SQL, "cnombre", "iIdPuesto")
        cbopuesto.SelectedIndex = 0
        'cboClientes.SelectedValue = gIdCliente

        '    End If
        'Catch ex As Exception

        'End Try
    End Sub

    'Private Sub MostrarClienteS()
    '    'Verificar si se tienen permisos
    '    SQL = "select * from usuarios where idUsuario = " & idUsuario
    '    Dim rwFilas As DataRow() = nConsulta(SQL)
    '    Dim Forma As New frmTipoEmpresa
    '    Try
    '        If rwFilas Is Nothing = False Then


    '            Dim Fila As DataRow = rwFilas(0)
    '            If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3" Or Fila.Item("fkIdPerfil") = "4" Or Fila.Item("fkIdPerfil") = "5") Then

    '                SQL = "Select nombre,iIdCliente from clientes"
    '            Else
    '                SQL = "Select nombre,iIdCliente from clientes inner join UsuarioClientes "
    '                SQL &= " on clientes.iIdCliente=UsuarioClientes.fkiIdCliente"
    '                SQL &= " where UsuarioClientes.fkiIdEmpleado =" & idUsuario
    '                SQL &= "  order by nombre "


    '            End If
    '            nCargaCBO(cboClientes, SQL, "nombre", "iIdCliente")
    '            cboClientes.SelectedValue = gIdCliente
    '        End If

    '    Catch ex As Exception

    '    End Try

    '    'SQL = "Select nombre,iIdCliente from clientes inner join UsuarioClientes "
    '    'SQL &= " on clientes.iIdCliente=UsuarioClientes.fkiIdCliente"
    '    'SQL &= " where UsuarioClientes.fkiIdEmpleado =" & idUsuario
    '    'SQL &= "  order by nombre "
    '    'nCargaCBO(cboClientes, SQL, "nombre", "iIdCliente")
    '    'SQL = "Select * from clientes order by nombre"
    '    'nCargaCBO(cboClientes, SQL, "nombre", "iIdCliente")
    '    'cboClientes.SelectedValue = gIdCliente
    'End Sub
    'Private Sub MostrarEmpleado()
    '    SQL = "select * from empleados where iIdEmpleado = " & gIdEmpleado
    '    Dim rwFilas As DataRow() = nConsulta(SQL)
    '    Try
    '        If rwFilas Is Nothing = False Then

    '            Dim Fila As DataRow = rwFilas(0)
    '            cbostatus.SelectedIndex = IIf(Fila.Item("iEstatus") = 1, 0, 1)
    '            txtcodigo.Text = Fila.Item("cCodigoEmpleado")
    '            txtnombre.Text = Fila.Item("cNombre")
    '            txtpaterno.Text = Fila.Item("cApellidoP")
    '            txtmaterno.Text = Fila.Item("cApellidoM")
    '            Dim fechanac As Date
    '            fechanac = Fila.Item("dFechaNac")
    '            dtpfechanac.Value = Fila.Item("dFechaNac")
    '            Dim edad As Integer = DateDiff(DateInterval.Year, fechanac, Date.Today)
    '            txtedad.Text = edad.ToString()

    '            Dim sexo As String = IIf(Fila.Item("iSexo") = "0", "Femenino", "Masculino")
    '            cbosexo.SelectedIndex = Fila.Item("iSexo")
    '            'item.SubItems.Add("" & sexo)
    '            'Dim civil As String = IIf(Fila.Item("iOrigen") = "0", "Soltero", "Casado")
    '            cboedocivil.SelectedIndex = Fila.Item("cInicioEmbarque")
    '            'item.SubItems.Add("" & civil)

    '            'item.SubItems.Add("" & Fila.Item("cPuesto"))
    '            txtfunciones.Text = Fila.Item("cFuncionesPuesto")
    '            'item.SubItems.Add("" & Fila.Item("cFuncionesPuesto"))
    '            cbocategoria.SelectedIndex = Fila.Item("iCategoria")
    '            'Dim Categoria As String = IIf(Fila.Item("iCategoria") = "0", "A", "B")
    '            'item.SubItems.Add("" & Categoria)
    '            dtppatrona.Value = Fila.Item("dFechaPatrona")
    '            'item.SubItems.Add("" & Fila.Item("dFechaPatrona"))
    '            dtpsindicato.Value = Fila.Item("dFechaSindicato")
    '            'item.SubItems.Add("" & Fila.Item("dFechaSindicato"))
    '            txtintegrar.Text = Fila.Item("cIntegrar")
    '            'item.SubItems.Add("" & Fila.Item("cIntegrar"))
    '            txtsd.Text = Fila.Item("fSueldoBase")
    '            'item.SubItems.Add("" & Fila.Item("fSueldoBase"))
    '            txtsdi.Text = Fila.Item("fSueldoIntegrado")
    '            'item.SubItems.Add("" & Fila.Item("fSueldoIntegrado"))

    '            'item.SubItems.Add("" & Fila.Item("dFechaNac"))
    '            txtcurp.Text = Fila.Item("cCURP")
    '            'item.SubItems.Add("" & Fila.Item("cCURP"))
    '            txtrfc.Text = Fila.Item("cRFC")
    '            'item.SubItems.Add("" & Fila.Item("cRFC"))
    '            txtimss.Text = Fila.Item("cIMSS")
    '            'item.SubItems.Add("" & Fila.Item("cIMSS"))
    '            chkInfonavit.Checked = IIf(Fila.Item("iPermanente") = "1", True, False)
    '            'item.SubItems.Add("" & IIf(Fila.Item("iPermanente") = "0", "No", "Si"))
    '            txtcredito.Text = Fila.Item("cInfonavit")
    '            'item.SubItems.Add("" & Fila.Item("cInfonavit"))
    '            cbotipofactor.Text = Fila.Item("cTipoFactor")
    '            'item.SubItems.Add("" & Fila.Item("cTipoFactor"))
    '            txtfactor.Text = Fila.Item("fFactor")
    '            'item.SubItems.Add("" & Fila.Item("fFactor"))
    '            txtcuenta.Text = Fila.Item("NumCuenta")
    '            'item.SubItems.Add("" & Fila.Item("NumCuenta"))
    '            txtclabe.Text = Fila.Item("Clabe")
    '            'item.SubItems.Add("" & Fila.Item("Clabe"))

    '            SQL = "select * from bancos where iIdBanco=" & Fila.Item("fkiIdBanco")
    '            Dim Banco As DataRow() = nConsulta(SQL)
    '            cbobanco.SelectedValue = Banco(0).Item("iIdBanco")
    '            'item.SubItems.Add("" & Banco(0).Item("cBanco"))
    '            txtnacionalidad.Text = Fila.Item("cNacionalidad")
    '            'item.SubItems.Add("" & Fila.Item("cNacionalidad"))
    '            txtdireccion.Text = Fila.Item("cDireccion")
    '            'item.SubItems.Add("" & Fila.Item("cDireccion"))
    '            txtciudad.Text = Fila.Item("cCiudadL")
    '            'item.SubItems.Add("" & Fila.Item("cCiudadL"))

    '            SQL = "select * from Cat_Estados where iIdEstado=" & Fila.Item("fkiIdEstado")
    '            Dim Estado As DataRow() = nConsulta(SQL)
    '            cboestado.SelectedValue = Estado(0).Item("iIdEstado")
    '            'item.SubItems.Add("" & Estado(0).Item("cEstado"))
    '            txtcp.Text = Fila.Item("cCP")
    '            'item.SubItems.Add("" & Fila.Item("cCP"))
    '            dtpantiguedad.Value = Fila.Item("dFechaAntiguedad")
    '            'item.SubItems.Add("" & Fila.Item("dFechaAntiguedad"))
    '            txtdireccionP.Text = Fila.Item("cDireccionP")
    '            txtciudadP.Text = Fila.Item("cCiudadP")
    '            txtcp2.Text = Fila.Item("cCPP")
    '            'item.SubItems.Add("" & Fila.Item("cDireccionP") & "" & Fila.Item("cCiudadP"))
    '            txtduracion.Text = Fila.Item("cDuracion")
    '            'item.SubItems.Add("" & Fila.Item("cDuracion"))
    '            cbojornada.Text = Fila.Item("cJornada")
    '            'item.SubItems.Add("" & Fila.Item("cJornada"))
    '            txtcomentarios.Text = Fila.Item("cObservaciones")
    '            'item.SubItems.Add("" & Fila.Item("cObservaciones"))
    '            txtcorreo.Text = Fila.Item("cCorreo")
    '            'item.SubItems.Add("" & Fila.Item("cCorreo"))
    '            txthorario.Text = Fila.Item("cHorario")
    '            'item.SubItems.Add("" & Fila.Item("cHorario"))
    '            txthoras.Text = Fila.Item("cHoras")
    '            'item.SubItems.Add("" & Fila.Item("cHoras"))
    '            txtdescanso.Text = Fila.Item("cDescanso")
    '            'item.SubItems.Add("" & Fila.Item("cDescanso"))
    '            'cboClientes.SelectedValue = Fila.Item("fkiIdClienteInter")
    '            cbopuesto.SelectedValue = Fila.Item("fkiIdPuesto")
    '            cbodepartamento.SelectedValue = Fila.Item("fkiIdDepartamento")


    '            txtcuenta.Text = Fila.Item("cuenta2")
    '            'item.SubItems.Add("" & Fila.Item("NumCuenta"))
    '            txtclabe.Text = Fila.Item("clabe2")
    '            'item.SubItems.Add("" & Fila.Item("Clabe"))

    '            SQL = "select * from bancos where iIdBanco=" & Fila.Item("fkiIdBanco2")
    '            Dim Banco2 As DataRow() = nConsulta(SQL)
    '            cbobanco2.SelectedValue = Banco2(0).Item("iIdBanco")
    '            dtpFinContrato.Value = Fila.Item("dFechaFin")

    '            blnNuevo = False
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub MostrarBancos()
        SQL = "Select * from bancos order by cBanco"
        nCargaCBO(cbobanco, SQL, "cBanco", "iIdBanco")
        cbobanco.SelectedIndex = 0
    End Sub

    Private Sub MostrarBancos2()
        SQL = "Select * from bancos order by cBanco"
        nCargaCBO(cbobanco2, SQL, "cBanco", "iIdBanco")
        cbobanco.SelectedIndex = 0
    End Sub

    Private Sub MostrarEstados()
        SQL = "Select * from Cat_Estados order by iIdEstado"
        nCargaCBO(cboestado, SQL, "cEstado", "iIdEstado")
        cboestado.SelectedIndex = 0
    End Sub

    Private Sub MostrarEstadosP()
        SQL = "Select * from Cat_Estados order by iIdEstado"
        nCargaCBO(cboestadoP, SQL, "cEstado", "iIdEstado")
        cboestadoP.SelectedIndex = 0
    End Sub

    Private Sub MostrarEmpresa()
        'SQL = "select * from empresaC where iIdEmpresaC = " & gIdEmpresa
        'Dim rwFilas As DataRow() = nConsulta(SQL)
        'Try
        '    If rwFilas Is Nothing = False Then


        '        Dim Fila As DataRow = rwFilas(0)
        '        'lblEmpresa.Text = "Empresa: " & Fila.Item("nombre")
        '        'lblDireccion.Text = "Direccion: " & Fila.Item("calle") & " " & Fila.Item("numero") & " " & Fila.Item("numeroint") & " " & Fila.Item("colonia") & " "

        '    End If
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub cmdincidencias_Click(sender As Object, e As EventArgs) Handles cmdincidencias.Click
        'If blnNuevo = False Then
        '    Dim Forma As New frmIncidenciaEmpleado
        '    Forma.gIdEmpresa = gIdEmpresa
        '    Forma.gIdEmpleado = gIdEmpleado
        '    Forma.gIdPeriodo = gIdPeriodo
        '    Forma.gIdCliente = gIdCliente
        '    Forma.ShowDialog()



        'Else
        '    MessageBox.Show("Seleccione un empleado primeramente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        'End If
    End Sub

    Private Sub cmdprestamo_Click(sender As Object, e As EventArgs) Handles cmdprestamo.Click
        'If blnNuevo = False Then
        '    Dim Forma As New frmPrestamoEmpleado
        '    Forma.gIdEmpresa = gIdEmpresa
        '    Forma.gIdEmpleado = gIdEmpleado
        '    Forma.gIdPeriodo = gIdPeriodo
        '    Forma.gIdCliente = gIdCliente
        '    Forma.ShowDialog()



        'Else
        '    MessageBox.Show("Seleccione un empleado primeramente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        'End If
    End Sub

    Private Sub txtExtra_TextChanged(sender As Object, e As EventArgs) Handles txtExtra.TextChanged

    End Sub

    Private Sub txtExtra_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtExtra.KeyPress
        SoloNumero.NumeroDec(e, sender)
    End Sub

    'Private Sub MostrarCliente()
    '    'Verificar si se tienen permisos
    '    SQL = "select * from usuarios where idUsuario = " & idUsuario
    '    Dim rwFilas As DataRow() = nConsulta(SQL)
    '    Dim Forma As New frmTipoEmpresa
    '    Try
    '        If rwFilas Is Nothing = False Then


    '            Dim Fila As DataRow = rwFilas(0)
    '            If (Fila.Item("fkIdPerfil") = "1" Or Fila.Item("fkIdPerfil") = "3" Or Fila.Item("fkIdPerfil") = "4" Or Fila.Item("fkIdPerfil") = "5") Then

    '                SQL = "Select nombre,iIdCliente from clientes"
    '            Else
    '                SQL = "Select nombre,iIdCliente from clientes inner join UsuarioClientes "
    '                SQL &= " on clientes.iIdCliente=UsuarioClientes.fkiIdCliente"
    '                SQL &= " where UsuarioClientes.fkiIdEmpleado =" & idUsuario
    '                SQL &= "  order by nombre "


    '            End If
    '            nCargaCBO(cboclientefiscal, SQL, "nombre", "iIdCliente")
    '            cboclientefiscal.SelectedValue = gIdCliente
    '        End If

    '    Catch ex As Exception

    '    End Try
    '    'SQL = "Select nombre,iIdCliente from clientes inner join UsuarioClientes "
    '    'SQL &= " on clientes.iIdCliente=UsuarioClientes.fkiIdCliente"
    '    'SQL &= " where UsuarioClientes.fkiIdEmpleado =" & idUsuario
    '    'SQL &= "  order by nombre "
    '    'nCargaCBO(cboclientefiscal, SQL, "nombre", "iIdCliente")
    '    'cboclientefiscal.SelectedValue = gIdCliente
    'End Sub

    Private Sub dtpfechanac_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpfechanac.ValueChanged
        ' Dim datenac As Date = CDate(dtpfechanac.Value)
        'txtedad.Text = DateTime.Now.Year - datenac.Year

        Dim canos As Integer
        Dim cmes As Integer
        Dim cdias As Integer
        Dim actual As Date
        Dim nacimiento As Date

        Dim canos1 As Integer
        Dim cmes1 As Integer
        Dim cdias1 As Integer

        Dim anos As Integer
        Dim mes As Integer
        Dim dias As Integer

        Dim nacimientoanos As Integer
        Dim nacimientomes As Integer
        Dim nacimientodias As Integer
        actual = DateTime.Now
        nacimiento = CDate(dtpfechanac.Value)

        canos = Year(actual)
        cmes = Month(actual)
        cdias = DateTime.Now.Day
        nacimientoanos = Year(nacimiento)
        nacimientomes = Month(nacimiento)
        nacimientodias = nacimiento.Day

        If cdias < nacimientodias Then
            cdias1 = cdias + 30
            Dias = cdias1 - nacimientodias

            cmes = cmes - 1

            'TextBox9 = Dias
        Else
            Dias = cdias - nacimientodias

            'TextBox9 = Dias
        End If

        If cmes < nacimientomes Then
            cmes1 = cmes + 12
            mes = cmes1 - nacimientomes
            canos = canos - 1
            anos = canos - nacimientoanos
            'TextBox8 = mes
            'TextBox7 = anos

        Else
            mes = cmes - nacimientomes
            anos = canos - nacimientoanos

            'TextBox8 = mes
            'TextBox7 = anos
        End If
        txtedad.Text = anos


    End Sub

  
  

  
    Private Sub cmdIncapacidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdIncapacidad.Click
        If blnNuevo = False Then
            Dim Forma As New frmIncapacidad
            Forma.gIdEmpleado = gIdEmpleado
            Forma.ShowDialog()

        Else
            MessageBox.Show("Seleccione un empleado primeramente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
    End Sub


    Private Sub cmdFamiliar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFamiliar.Click
        If blnNuevo = False Then
            Dim frm As New frmFamilia
            frm.gIdEmpleado = gIdEmpleado
            frm.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primeramente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub cmdJuridico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdJuridico.Click
        Dim frmJ As New frmJuridico
        frmJ.gIdEmpleado = gIdEmpleado
        frmJ.ShowDialog()

    End Sub

    Private Sub cmdDocumentos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDocumentos.Click
        If blnNuevo = False Then
            Dim frm As New frmDocumentos
            frm.gIdEmpleado = gIdEmpleado
            frm.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primeramente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub cmdlista_Click(sender As System.Object, e As System.EventArgs) Handles cmdlista.Click
        Dim filaExcel As Integer = 5
        Dim dialogo As New SaveFileDialog()
        Dim idtipo As Integer

        SQL = "select cCodigoEmpleado,cNombreLargo,cApellidoP,cApellidoM,cNombre,cRFC,cCURP,cIMSS,cBanco,NumCuenta,Clabe, EmpleadosC.iEstatus, EmpleadosC.fSueldoBase, EmpleadosC.fSueldoIntegrado, EmpleadosC.fSueldoOrd, EmpleadosC.dFechaAntiguedad, "
        SQL &= "iSexo, fkiIdPuesto, fkiIdDepartamento,cPuesto, cFuncionesPuesto, cCorreo, clabe2,cCp"
        SQL &= " from EmpleadosC inner join bancos on EmpleadosC.fkiIdBanco=bancos.iIdBanco "
        SQL &= " order by cNombreLargo"
        Dim rwFilas As DataRow() = nConsulta(SQL)
        If rwFilas Is Nothing = False Then
            Dim libro As New ClosedXML.Excel.XLWorkbook
            Dim hoja As IXLWorksheet = libro.Worksheets.Add("Control")
            hoja.Column("A").Width = 15
            hoja.Column("B").Width = 50
            hoja.Column("C").Width = 25
            hoja.Column("D").Width = 25
            hoja.Column("E").Width = 25
            hoja.Column("F").Width = 30
            hoja.Column("G").Width = 25
            hoja.Column("H").Width = 30
            hoja.Column("I").Width = 25
            hoja.Column("J").Width = 30
            hoja.Column("K").Width = 30
            hoja.Column("L").Width = 10
            hoja.Column("M").Width = 10
            hoja.Column("N").Width = 10
            hoja.Column("O").Width = 30
            hoja.Column("P").Width = 30
            hoja.Column("T").Width = 50
            hoja.Column("U").Width = 30
            hoja.Column("V").Width = 50
            hoja.Column("W").Width = 50
            hoja.Column("X").Width = 30


            hoja.Cell(2, 2).Value = "Fecha: " & Date.Now.ToShortDateString()

            hoja.Cell(3, 2).Value = "LISTA DE EMPLEADOS"
            'hoja.Cell(3, 2).Value = ":"
            'hoja.Cell(3, 3).Value = ""

            hoja.Range(4, 1, 4, 25).Style.Font.FontSize = 10
            hoja.Range(4, 1, 4, 25).Style.Font.SetBold(True)
            hoja.Range(4, 1, 4, 25).Style.Alignment.WrapText = True
            hoja.Range(4, 1, 4, 25).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            hoja.Range(4, 1, 4, 25).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
            hoja.Range(4, 1, 4, 25).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
            hoja.Range(4, 1, 4, 25).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

            'hoja.Cell(4, 1).Value = "Num"
            hoja.Cell(4, 1).Value = "Id"
            hoja.Cell(4, 2).Value = "Apellido Paterno"
            hoja.Cell(4, 3).Value = "Apellido Materno"
            hoja.Cell(4, 4).Value = "Nombre"
            hoja.Cell(4, 5).Value = "RFC"
            hoja.Cell(4, 6).Value = "CURP"
            hoja.Cell(4, 7).Value = "IMSS"
            hoja.Cell(4, 8).Value = "BANCO"
            hoja.Cell(4, 9).Value = "CUENTA"
            hoja.Cell(4, 10).Value = "CLABE"
            hoja.Cell(4, 11).Value = "ESTATUS"
            hoja.Cell(4, 12).Value = "SUELDO BASE"
            hoja.Cell(4, 13).Value = "SBC"
            hoja.Cell(4, 14).Value = "SUELDO BRUTO"
            hoja.Cell(4, 15).Value = "FECHA ANTIGUEDAD"
            hoja.Cell(4, 16).Value = "SEXO"
            hoja.Cell(4, 17).Value = "fkiIdPuesto"
            hoja.Cell(4, 18).Value = "PUESTO"
            hoja.Cell(4, 19).Value = "fkiIdDepartamento"
            hoja.Cell(4, 20).Value = "DEPTO"
            hoja.Cell(4, 21).Value = "cPuesto"
            hoja.Cell(4, 22).Value = "cFuncionesPuesto"
            hoja.Cell(4, 23).Value = "cCorreo"
            hoja.Cell(4, 24).Value = "CE CO"
            hoja.Cell(4, 25).Value = "CODIGO POSTAL"

            filaExcel = 4
            For Each Fila In rwFilas

                Dim rwPuesto As DataRow() = nConsulta("SELECT * FROM puestos where iIdPuesto=" & Fila.Item("fkiIdPuesto"))
                Dim rwDepto As DataRow() = nConsulta("SELECT * FROM departamentos where iIdDepartamento=" & Fila.Item("fkiIdDepartamento"))

                filaExcel = filaExcel + 1
                hoja.Cell(filaExcel, 1).Value = "'" & Fila.Item("cCodigoEmpleado").ToString
                hoja.Cell(filaExcel, 2).Value = Fila.Item("cApellidoP")
                hoja.Cell(filaExcel, 3).Value = Fila.Item("cApellidoM")
                hoja.Cell(filaExcel, 4).Value = Fila.Item("cNombre")
                hoja.Cell(filaExcel, 5).Value = Fila.Item("cRFC")
                hoja.Cell(filaExcel, 6).Value = Fila.Item("cCURP")
                hoja.Cell(filaExcel, 7).Value = Fila.Item("cIMSS")
                hoja.Cell(filaExcel, 8).Value = Fila.Item("cBanco")
                hoja.Cell(filaExcel, 9).Value = "'" & Fila.Item("NumCuenta")
                hoja.Cell(filaExcel, 10).Value = "'" & Fila.Item("Clabe")
                hoja.Cell(filaExcel, 11).Value = IIf(Fila.Item("iEstatus") = 1, "ACTIVO", "BAJA")
                hoja.Cell(filaExcel, 12).Value = Fila.Item("fSueldoBase")
                hoja.Cell(filaExcel, 13).Value = Fila.Item("fSueldoIntegrado")
                hoja.Cell(filaExcel, 14).Value = Fila.Item("fSueldoOrd")
                hoja.Cell(filaExcel, 15).Value = Fila.Item("dFechaAntiguedad")
                hoja.Cell(filaExcel, 16).Value = IIf(Fila.Item("iSexo") = 1, "FEMENINO", "MASCULINO")
                hoja.Cell(filaExcel, 17).Value = Fila.Item("fkiIdPuesto")
                hoja.Cell(filaExcel, 18).Value = rwPuesto(0)("cNombre")
                hoja.Cell(filaExcel, 19).Value = Fila.Item("fkiIdDepartamento")
                hoja.Cell(filaExcel, 20).Value = rwDepto(0)("cNombre")
                hoja.Cell(filaExcel, 21).Value = Fila.Item("cPuesto")
                hoja.Cell(filaExcel, 22).Value = Fila.Item("cFuncionesPuesto")
                hoja.Cell(filaExcel, 23).Value = Fila.Item("cCorreo")
                hoja.Cell(filaExcel, 24).Value = Fila.Item("clabe2")
                hoja.Cell(filaExcel, 24).Value = Fila.Item("cCp")
            Next

            dialogo.DefaultExt = "*.xlsx"
            dialogo.FileName = "Lista de Empleados " & Today.Year.ToString
            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
            dialogo.ShowDialog()
            libro.SaveAs(dialogo.FileName)
            'libro.SaveAs("c:\temp\control.xlsx")
            'libro.SaveAs(dialogo.FileName)
            'apExcel.Quit()
            libro = Nothing
            MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("No hay datos a mostrar", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub cmdPension_Click(sender As System.Object, e As System.EventArgs) Handles cmdPension.Click
        Dim forma As New frmPensionA

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmdimss_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdimss.Click

        Dim forma As New frmImss

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmdFonacot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFonacot.Click

        Dim forma As New frmFonacot

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmdPrestam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrestam.Click
        Dim forma As New frmPrestamo

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmdInfonavit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInfonavit.Click

        Dim forma As New frmDeudaInfonavit

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim forma As New frmPrestamoSA

        If gIdEmpleado Is Nothing = False Then


            forma.gIdEmpleado = gIdEmpleado
            forma.gIdCliente = gIdCliente
            forma.gIdEmpresa = 1
            forma.ShowDialog()
        Else
            MessageBox.Show("Seleccione un empleado primero", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmCECO_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub cmCECO_Click_1(sender As System.Object, e As System.EventArgs) Handles cmCECO.Click
        Try

            Dim Forma As New frmSubirCECO
          
            Forma.ShowDialog()

        Catch ex As Exception

        End Try
    End Sub
End Class