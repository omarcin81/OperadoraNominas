Imports ClosedXML.Excel
Imports System.IO
Public Class frmPrincipal
    Dim SQL As String
    Public EmpresaN As String
    Private Sub frmPrincipal_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If sender Is Nothing = False Then
            Dim Forma As Form = CType(sender, Form)
            Dim Nombre As String = Forma.Name
            If Nombre = "frmFacturar" Then
                chkCFDI.Visible = False
                AjustarBarra()
            ElseIf Nombre = "frmFacturaCBB" Then
                chkCBB.Visible = False
                AjustarBarra()
            End If
        End If
    End Sub

    Private Sub AjustarBarra()
        chkCFDI.Left = lblUsuario.Left + lblUsuario.Width + 10
        chkCBB.Left = lblUsuario.Left + lblUsuario.Width + IIf(chkCFDI.Visible, chkCFDI.Width + 5, 0) + 10
    End Sub

    Private Sub frmPrincipal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("¿Desea salir del sistema?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            e.Cancel = True
        End If
    End Sub

    Private Sub frmPrincipal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblUsuario.Text = Usuario.Nombre & " - " & EmpresaN
        clsConfiguracion.Actualizar()
        lsvPanel.Items.Item(0).Text = "Nomina " '& Servidor.Base.ToString.Substring(0, 3)
        'If EmpresaN = "Logistic" Then

        'End If
    End Sub

    Private Sub CatálogoDeClientesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim Forma As New Vendedores
        'Forma.ShowDialog()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim PT As Point = Me.PointToScreen(CheckBox1.Location)

        If CheckBox1.Checked Then
            MenuInicio.Show(CheckBox1.Location.X, (CheckBox1.Location.Y + pnlBar.Location.Y) - ((CheckBox1.Height - 4) * MenuInicio.Items.Count))
            CheckBox1.Checked = False
        End If
    End Sub

    Private Sub frmPrincipal_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If sender Is Nothing = False Then
            Dim Forma As Form = CType(sender, Form)
            Dim Nombre As String = Forma.Name
            If Nombre = "frmFacturar" Then
                'RemoveHandler chkCFDI.CheckedChanged, AddressOf chkCFDI_CheckedChanged
                chkCFDI.Checked = Forma.WindowState <> FormWindowState.Minimized
                'frmFacturacionCFDI.Visible = frmFacturacionCFDI.WindowState <> FormWindowState.Minimized
                'AddHandler chkCFDI.CheckedChanged, AddressOf chkCFDI_CheckedChanged
            ElseIf Nombre = "frmFacturaCBB" Then
                'RemoveHandler chkCBB.CheckedChanged, AddressOf chkCBB_CheckedChanged
                chkCBB.Checked = Forma.WindowState <> FormWindowState.Minimized
                'frmFacturacionCBB.Visible = frmFacturacionCBB.WindowState <> FormWindowState.Minimized
                'AddHandler chkCBB.CheckedChanged, AddressOf chkCBB_CheckedChanged
            End If
        End If
    End Sub

    Private Sub lsvPanel_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvPanel.ItemActivate
        Try
            If lsvPanel.SelectedItems.Count <= 0 Then
                Exit Sub
            End If
            Select Case lsvPanel.SelectedItems(0).Text
                
                Case "Nomina " '& Servidor.Base.ToString.Substring(0, 3)
                    Try
                        If Usuario.Perfil = "1" Or Usuario.Perfil = "2" Then
                            Dim Forma As New frmnominasmarinos
                            Forma.gTipoCalculo = "1"
                            Forma.EmpresaN = EmpresaN
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    Catch ex As Exception
                    End Try
                Case "Importar Excel"
                    Try
                        If Usuario.Perfil = "1" Then
                            Dim Forma As New frmExcel
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        

                    Catch ex As Exception
                    End Try
                Case "Empleados"
                    Try
                        If Usuario.Perfil = "1" Or Usuario.Perfil = "2" Then
                            Dim Forma As New frmEmpleados
                            Forma.gIdTipoPuesto = 0
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        

                    Catch ex As Exception
                    End Try
                Case "Prestamos"
                    Try

                        If Usuario.Perfil = "1" Then
                            Dim dialogo As New SaveFileDialog()

                            Dim Forma As New frmEstatusPres

                            If Forma.ShowDialog = Windows.Forms.DialogResult.OK Then
                                reporteprestamo(Forma.gEstatus)
                            End If
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        

                    Catch ex As Exception
                    End Try
                Case "Reporte trabajadores"
                    Try
                        If Usuario.Perfil = "1" Then
                            generarreporte()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                    Catch ex As Exception
                    End Try

                Case "Buscar Datos"
                    Try
                        If Usuario.Perfil = "1" Then
                            Dim Forma As New frmExcelO
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                       

                    Catch ex As Exception

                    End Try
                Case "Subir Nomina"
                    Try
                        If Usuario.Perfil = "1" Then
                            Dim Forma As New frmNominaFinalE
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        

                    Catch ex As Exception
                        ShowError(ex, Me.Text)
                    End Try
                Case "Calcular Ajuste"
                    Try
                        If Usuario.Perfil = "1" Then
                            Dim Forma As New frmPlaneacionAsi
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    Catch ex As Exception
                        ShowError(ex, Me.Text)
                    End Try
                Case "Nomina Admon"
                    Try
                        If Usuario.Perfil = "1" Then
                            Dim Forma As New frmAdministrativos
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    Catch ex As Exception
                        ShowError(ex, Me.Text)
                    End Try
                    '
                Case "Nomina Excedente"
                    Try
                        If Usuario.Perfil = "1" Or Usuario.Perfil = "2" Then
                            Dim Forma As New frmnominasmarinos
                            Forma.gTipoCalculo = "2"
                            Forma.EmpresaN = EmpresaN
                            Forma.ShowDialog()
                        Else
                            MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                    Catch ex As Exception
                        ShowError(ex, Me.Text)
                    End Try
            End Select

        Catch ex As Exception
            ShowError(ex, Me.Text)
        End Try
    End Sub

    Private Sub generarreporte()
        Try
            Dim sql As String
            Dim filaExcel As Integer = 5
            Dim contador As Integer
            Dim dialogo As New SaveFileDialog()
            Dim SalarioDiario, SalarioBC As Double
            Dim sexo As String
            Dim Puesto As String

            Dim dsReporte As New DataSet
            dsReporte.Tables.Add("Tabla")
            dsReporte.Tables("Tabla").Columns.Add("Consecutivo")
            dsReporte.Tables("Tabla").Columns.Add("Id_empleado")
            dsReporte.Tables("Tabla").Columns.Add("CodigoEmpleado")
            dsReporte.Tables("Tabla").Columns.Add("Nombre")
            dsReporte.Tables("Tabla").Columns.Add("Puesto")
            dsReporte.Tables("Tabla").Columns.Add("Sexo")
            dsReporte.Tables("Tabla").Columns.Add("TipoEmpleado")
            dsReporte.Tables("Tabla").Columns.Add("Fecha_Nac")
            dsReporte.Tables("Tabla").Columns.Add("Fecha_Ing")
            dsReporte.Tables("Tabla").Columns.Add("IMSS")
            dsReporte.Tables("Tabla").Columns.Add("SUELDOBASE")
            dsReporte.Tables("Tabla").Columns.Add("SALARIOMENSUALSA")
            dsReporte.Tables("Tabla").Columns.Add("SALARIOMENSUALASIMILADOS")
            dsReporte.Tables("Tabla").Columns.Add("SUELDOMENSUAL")
            dsReporte.Tables("Tabla").Columns.Add("PARTIDAS")


            sql = "select * from empleadosC order by cNombreLargo"
            Dim rwFilas As DataRow() = nConsulta(sql)
            If rwFilas Is Nothing = False Then
                For x As Integer = 0 To rwFilas.Count - 1

                    Dim fila As DataRow = dsReporte.Tables("Tabla").NewRow

                    fila.Item("Consecutivo") = (x + 1).ToString
                    fila.Item("Id_empleado") = rwFilas(x)("iIdEmpleadoC").ToString
                    fila.Item("CodigoEmpleado") = rwFilas(x)("cCodigoEmpleado").ToString
                    fila.Item("Nombre") = rwFilas(x)("cNombreLargo").ToString.ToUpper()
                    'sacar el ultimo puesto en nominas y de ese puesto sacar los datos para los calculos de sueldo base y los demas que sean posibles
                    sql = "select * from Nomina where fkiIdEmpleadoC=" & rwFilas(x)("iIdEmpleadoC") & " order by iIdNomina desc"
                    Dim rwPuesto As DataRow() = nConsulta(sql)
                    If rwPuesto Is Nothing = False Then
                        fila.Item("Puesto") = rwPuesto(0)("Puesto").ToString
                        'Sacar datos para los sueldos
                        Puesto = rwPuesto(0)("Puesto").ToString
                        SalarioDiario = rwPuesto(0)("fSalarioDiario").ToString
                        SalarioBC = rwPuesto(0)("fSalarioBC").ToString

                    Else
                        fila.Item("Puesto") = rwFilas(x)("cPuesto").ToString.ToUpper()
                        'Sacar datos para los sueldos
                        Puesto = rwFilas(x)("cPuesto").ToString.ToUpper()
                        SalarioDiario = 0
                        SalarioBC = rwFilas(x)("fSueldoIntegrado").ToString
                    End If

                    'sacar con el curp
                    sexo = rwFilas(x)("cCURP").ToString.Substring(10, 1)

                    If sexo = "H" Then
                        fila.Item("Sexo") = "M"
                    Else
                        fila.Item("Sexo") = "F"
                    End If
                    'Tipo empleado

                    fila.Item("TipoEmpleado") = IIf(rwFilas(x)("iOrigen").ToString.ToUpper() = "1", "INTERINO", "PLANTA")


                    'sacar del sistema en caso de estar mal es posible sacar de la curp

                    fila.Item("Fecha_Nac") = Date.Parse(rwFilas(x)("dFechaNac").ToString).ToShortDateString()

                    'Fecha de ingreso sacar de alta baja la ultima alta

                    sql = "select * from ingresoBajaAlta where clave='A' and fkiIdEmpleado=" & rwFilas(x)("iIdEmpleadoC") & " order by iIdIngresoBaja desc"
                    Dim rwAltaIMSS As DataRow() = nConsulta(sql)
                    If rwAltaIMSS Is Nothing = False Then
                        fila.Item("Fecha_Ing") = Date.Parse(rwAltaIMSS(0)("fechabajaimss").ToString).ToShortDateString()
                    Else
                        fila.Item("Fecha_Ing") = ""
                    End If

                    'Num IMSS
                    fila.Item("IMSS") = rwFilas(x)("cIMSS").ToString.ToUpper()




                    'Sueldo base segun datos, hay que verificar la edad para asi saber si es que esta un poco mas
                    If Puesto = "OFICIALES EN PRACTICAS: PILOTIN / ASPIRANTE" Or Puesto = "SUBALTERNO EN FORMACIÓN" Then

                        sql = "select * from puestos inner join DatosPlaneacion on iIdPuesto=fkiIdPuesto where cNombre= '" & Puesto & "'"

                        Dim rwDatosPlaneacion As DataRow() = nConsulta(sql)
                        If rwDatosPlaneacion Is Nothing = False Then
                            fila.Item("SUELDOBASE") = rwDatosPlaneacion(0)("SueldoBruto")
                            fila.Item("SALARIOMENSUALSA") = rwDatosPlaneacion(0)("NetoTradicional")
                            'sacar el datos de acuerdo a los datos de planeacion

                            fila.Item("SALARIOMENSUALASIMILADOS") = rwDatosPlaneacion(0)("NetoAsimilables")
                            fila.Item("SUELDOMENSUAL") = rwDatosPlaneacion(0)("TotalSAAsimilables")
                            fila.Item("PARTIDAS") = "SUELDO BASE"
                        Else
                            fila.Item("SUELDOBASE") = "0.00"
                            fila.Item("SALARIOMENSUALSA") = "0.00"
                            fila.Item("SALARIOMENSUALASIMILADOS") = "0.00"
                            fila.Item("SUELDOMENSUAL") = "0.00"
                            fila.Item("PARTIDAS") = "SUELDO BASE"
                        End If


                    Else

                        sql = "select * from puestos inner join DatosPlaneacion on iIdPuesto=fkiIdPuesto where cNombre= '" & Puesto & "'"

                        Dim rwDatosPlaneacion As DataRow() = nConsulta(sql)
                        If rwDatosPlaneacion Is Nothing = False Then
                            If Double.Parse(rwDatosPlaneacion(0)("SalarioIntegrado").ToString) = SalarioBC Then
                                fila.Item("SUELDOBASE") = rwDatosPlaneacion(0)("SueldoBruto")
                                fila.Item("SALARIOMENSUALSA") = rwDatosPlaneacion(0)("NetoTradicional")
                                fila.Item("SALARIOMENSUALASIMILADOS") = rwDatosPlaneacion(0)("NetoAsimilables")
                                fila.Item("SUELDOMENSUAL") = rwDatosPlaneacion(0)("TotalSAAsimilables")
                                fila.Item("PARTIDAS") = "SUELDO BASE, TIEMPO EXTRA FIJO, TIEMPO EXTRA OCASIONAL, DESC. SEM. OBLIGATORIO, VACACIONES PROPORCIONALES, AGUINALDO,PRIMA VACACIONAL"
                            ElseIf Double.Parse(rwDatosPlaneacion(0)("SalarioIntegradoTopado").ToString) = SalarioBC Then
                                fila.Item("SUELDOBASE") = rwDatosPlaneacion(0)("SueldoBrutoTopado")
                                fila.Item("SALARIOMENSUALSA") = rwDatosPlaneacion(0)("NetoTradicionalTopado")
                                fila.Item("SALARIOMENSUALASIMILADOS") = rwDatosPlaneacion(0)("NetoAsimilablesTopado")
                                fila.Item("SUELDOMENSUAL") = rwDatosPlaneacion(0)("TotalSAAsimilablesTopado")
                                fila.Item("PARTIDAS") = "SUELDO BASE, TIEMPO EXTRA FIJO, TIEMPO EXTRA OCASIONAL, DESC. SEM. OBLIGATORIO, VACACIONES PROPORCIONALES, AGUINALDO,PRIMA VACACIONAL"
                            End If




                        Else


                            fila.Item("SUELDOBASE") = "0.00"
                            fila.Item("SALARIOMENSUALSA") = "0.00"
                            fila.Item("SALARIOMENSUALASIMILADOS") = "0.00"
                            fila.Item("SUELDOMENSUAL") = "0.00"
                            fila.Item("PARTIDAS") = "SUELDO BASE, TIEMPO EXTRA FIJO, TIEMPO EXTRA OCASIONAL, DESC. SEM. OBLIGATORIO, VACACIONES PROPORCIONALES, AGUINALDO,PRIMA VACACIONAL"
                        End If



                    End If


                    dsReporte.Tables("Tabla").Rows.Add(fila)


                Next
                'Generar archivo de excel
                Dim libro As New ClosedXML.Excel.XLWorkbook
                Dim hoja As IXLWorksheet = libro.Worksheets.Add("Nomina")
                'Dim hoja2 As IXLWorksheet = libro.Worksheets.Add("Resumen pago")

                hoja.Column("B").Width = 15
                hoja.Column("C").Width = 15
                hoja.Column("D").Width = 40
                hoja.Column("E").Width = 20
                hoja.Column("F").Width = 15
                hoja.Column("G").Width = 20
                hoja.Column("H").Width = 15
                hoja.Column("I").Width = 15
                hoja.Column("J").Width = 20
                hoja.Column("K").Width = 20
                hoja.Column("L").Width = 20
                hoja.Column("M").Width = 20
                hoja.Column("N").Width = 20
                hoja.Column("O").Width = 140


                hoja.Cell(1, 2).Value = "Listado Trabajadores"
                hoja.Range(1, 2, 1, 2).Style.Font.SetBold(True)
                hoja.Cell(2, 2).Value = "Fecha:" & Date.Now.ToShortDateString & " " & Date.Now.ToShortTimeString
                hoja.Cell(3, 2).Value = "Empresa: " & ""
                hoja.Range(3, 2, 3, 2).Style.Font.SetBold(True)

                'hoja.Cell(3, 2).Value = ":"
                'hoja.Cell(3, 3).Value = ""

                hoja.Range(4, 2, 4, 15).Style.Font.FontSize = 10
                hoja.Range(4, 2, 4, 15).Style.Font.SetBold(True)
                hoja.Range(4, 2, 4, 15).Style.Alignment.WrapText = True
                hoja.Range(4, 2, 4, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                hoja.Range(4, 1, 4, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
                hoja.Range(4, 2, 4, 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
                hoja.Range(4, 2, 4, 15).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")

                hoja.Range(5, 11, 1000, 14).Style.NumberFormat.NumberFormatId = 4

                'Format = ("$ #,###,##0.00")
                'hoja.Cell(4, 1).Value = "Num"

                hoja.Cell(4, 2).Value = "Consecutivo"
                hoja.Cell(4, 3).Value = "Código Empleado"
                hoja.Cell(4, 4).Value = "Nombre"
                hoja.Cell(4, 5).Value = "Puesto"
                hoja.Cell(4, 6).Value = "Sexo"
                hoja.Cell(4, 7).Value = "Tipo Empleado"
                hoja.Cell(4, 8).Value = "Fecha Nacimiento"
                hoja.Cell(4, 9).Value = "Fecha Ingreso"
                hoja.Cell(4, 10).Value = "Num. IMSS"
                hoja.Cell(4, 11).Value = "Sueldo Mensual Base"
                hoja.Cell(4, 12).Value = "Sueldo Mensual SA"
                hoja.Cell(4, 13).Value = "Sueldo Mensual Asimilados"
                hoja.Cell(4, 14).Value = "Total Sueldo Mensual"
                hoja.Cell(4, 15).Value = "Descripción Partidas"


                filaExcel = 5
                contador = 1

                For x As Integer = 0 To dsReporte.Tables("Tabla").Rows.Count - 1

                    'Consecutivo
                    hoja.Cell(filaExcel + x, 2).Value = dsReporte.Tables("Tabla").Rows(x)("Consecutivo")
                    'CodigoEmpleado
                    hoja.Cell(filaExcel + x, 3).Value = "'" & dsReporte.Tables("Tabla").Rows(x)("CodigoEmpleado")
                    'nombre
                    hoja.Cell(filaExcel + x, 4).Value = dsReporte.Tables("Tabla").Rows(x)("Nombre")
                    'Puesto
                    hoja.Cell(filaExcel + x, 5).Value = dsReporte.Tables("Tabla").Rows(x)("Puesto")
                    'Sexo
                    hoja.Cell(filaExcel + x, 6).Value = dsReporte.Tables("Tabla").Rows(x)("Sexo")
                    'TipoEmpleado
                    hoja.Cell(filaExcel + x, 7).Value = dsReporte.Tables("Tabla").Rows(x)("TipoEmpleado")
                    'Fecha_Nac
                    hoja.Cell(filaExcel + x, 8).Value = dsReporte.Tables("Tabla").Rows(x)("Fecha_Nac")
                    'Fecha_Ing
                    hoja.Cell(filaExcel + x, 9).Value = dsReporte.Tables("Tabla").Rows(x)("Fecha_Ing")
                    'IMSS
                    hoja.Cell(filaExcel + x, 10).Value = dsReporte.Tables("Tabla").Rows(x)("IMSS")
                    'SUELDOBASE
                    hoja.Cell(filaExcel + x, 11).Value = dsReporte.Tables("Tabla").Rows(x)("SUELDOBASE")
                    'SALARIOMENSUALSA
                    hoja.Cell(filaExcel + x, 12).Value = dsReporte.Tables("Tabla").Rows(x)("SALARIOMENSUALSA")
                    'SALARIOMENSUALASIMILADOS
                    hoja.Cell(filaExcel + x, 13).Value = dsReporte.Tables("Tabla").Rows(x)("SALARIOMENSUALASIMILADOS")
                    'SUELDOMENSUAL
                    hoja.Cell(filaExcel + x, 14).Value = dsReporte.Tables("Tabla").Rows(x)("SUELDOMENSUAL")
                    'PARTIDAS
                    hoja.Cell(filaExcel + x, 15).Value = dsReporte.Tables("Tabla").Rows(x)("PARTIDAS")
                    

                    
                Next




                '##### HOJA NUMERO 2 RESUMEN PAGO


                dialogo.DefaultExt = "*.xlsx"
                dialogo.FileName = "Resumen Trabajadores"
                dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
                dialogo.ShowDialog()
                libro.SaveAs(dialogo.FileName)
                'libro.SaveAs("c:\temp\control.xlsx")
                'libro.SaveAs(dialogo.FileName)
                'apExcel.Quit()
                libro = Nothing

                MessageBox.Show("Archivo generado", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub


    Private Sub lsvPanel_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsvPanel.SizeChanged
        Dim sRuta As String
        sRuta = System.IO.Path.GetTempPath
        Try
            Me.lsvPanel.BackgroundImage = Me.PictureBox1.Image.GetThumbnailImage(Me.lsvPanel.ClientSize.Width, Me.lsvPanel.ClientSize.Height, Nothing, Nothing)
            Me.BackgroundImage = Me.PictureBox1.Image.GetThumbnailImage(Me.lsvPanel.ClientSize.Width, Me.lsvPanel.ClientSize.Height, Nothing, Nothing)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub pnlBar_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlBar.Paint
        Degradado(e, sender, Color.White, Color.Gray, Drawing2D.LinearGradientMode.Vertical)
    End Sub

    Private Sub lblUsuario_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblUsuario.SizeChanged
        AjustarBarra()
    End Sub

    Private Sub mnuSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSalir.Click
        Me.Close()
    End Sub


    Private Sub CatalogosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CatalogosToolStripMenuItem.Click


    End Sub

    Private Sub ClientesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClientesToolStripMenuItem.Click

        Try

            If Usuario.Perfil = "1" Then
                Dim Forma As New frmEmpleados
                Forma.gIdTipoPuesto = 0
                Forma.ShowDialog()

            Else
                MessageBox.Show("No tiene permisos para esta seccion, consulte al administrador", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub



  

    Private Sub reporteprestamo(ByRef status As Integer)
        Try
            Dim filaExcel As Integer = 0
            Dim dialogo As New SaveFileDialog()
            Dim periodo As String
            Dim pilotin As Boolean

            Dim tiponomina, sueldodescanso As String

            Dim ruta As String
            ruta = My.Application.Info.DirectoryPath() & "\Archivos\reporteprestamos.xlsx"

            Dim book As New ClosedXML.Excel.XLWorkbook(ruta)


            Dim libro As New ClosedXML.Excel.XLWorkbook

            book.Worksheets(1).CopyTo(libro, "PRESTAMO SA")
            book.Worksheets(2).CopyTo(libro, "PRESTAMO ASIM")


            Dim hoja As IXLWorksheet = libro.Worksheets(0)
            Dim hoja2 As IXLWorksheet = libro.Worksheets(1)


            '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Prestamo SA>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            filaExcel = 2
            Dim nombre As String
            Dim tipo As String
            Dim totalcobrado As DataRow()
            Dim rwPrestamoSa As DataRow()

            If status = 1 Then
                rwPrestamoSa = nConsulta("select * from PrestamoSA a left join empleadosC b on a.fkiIdEmpleado=b.iIdEmpleadoC where a.iEstatus=1 ORDER BY iIdPrestamoSA")
            Else

                rwPrestamoSa = nConsulta("select * from PrestamoSA a left join empleadosC b on a.fkiIdEmpleado=b.iIdEmpleadoC  ORDER BY iIdPrestamoSA")
            End If

            If rwPrestamoSa Is Nothing = False Then
                If rwPrestamoSa.Length > 1 Then

                    For Each prestado In rwPrestamoSa

                        hoja.Range(filaExcel, 6, filaExcel, 10).Style.NumberFormat.NumberFormatId = 4

                        Dim tipoprestamo As DataRow() = nConsulta("select * from TipoPrestamo where iIdTipoPrestamo =" & prestado.Item("fkiIdTipoPrestamo"))
                        If tipoprestamo Is Nothing = False Then

                            tipo = tipoprestamo(0).Item("TipoPrestamo")
                            totalcobrado = nConsulta("SELECT SUM(monto) As TotalCobrado FROM PagoPrestamoSA WHERE fkiIdPrestamoSA=" & prestado.Item("iIdPrestamoSA"))
                            'Colores estatus
                            If prestado.Item("iEstatus") = 0 Then
                                hoja.Range(filaExcel, 1, filaExcel, 10).Style.Fill.BackgroundColor = XLColor.RedPigment
                            End If


                            hoja.Cell("A" & filaExcel).Value = prestado.Item("iIdPrestamoSA") ' codigo
                            hoja.Cell("B" & filaExcel).Value = IIf(prestado.Item("iEstatus") = 1, "ACTIVO", "INACTIVO") ' Estatus
                            hoja.Cell("C" & filaExcel).Value = prestado.Item("fechaprestamo") ' alta
                            hoja.Cell("D" & filaExcel).Value = prestado.Item("cNombreLargo") ' Nombre
                            hoja.Cell("E" & filaExcel).Value = tipo 'tipo 'Tipo de prestamo
                            hoja.Cell("F" & filaExcel).Value = prestado.Item("montoTotal") 'Monto
                            hoja.Cell("G" & filaExcel).Value = prestado.Item("descuento")

                            Dim dif As Double = prestado.Item("montoTotal") - prestado.Item("descuento")
                            If dif <= 1 Then
                                hoja.Cell("H" & filaExcel).Value = prestado.Item("descuento")

                            Else
                                hoja.Cell("H" & filaExcel).Value = totalcobrado(0).Item("TotalCobrado")

                            End If


                            hoja.Cell("I" & filaExcel).FormulaA1 = "=F" & filaExcel & "-H" & filaExcel 'FALTANTE
                        End If


                        filaExcel = filaExcel + 1
                    Next
                End If
            End If

            '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Prestamo asim>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            filaExcel = 2
            Dim rwPrestamoASIM As DataRow()
            If status = 1 Then
                rwPrestamoASIM = nConsulta("select * from Prestamo a left join empleadosC b on a.fkiIdEmpleado=b.iIdEmpleadoC where a.iEstatus=1 ORDER BY iIdPrestamo")
            Else
                rwPrestamoASIM = nConsulta("select * from Prestamo a left join empleadosC b on a.fkiIdEmpleado=b.iIdEmpleadoC ORDER BY iIdPrestamo")
            End If
            
            If rwPrestamoASIM Is Nothing = False Then
                If rwPrestamoASIM.Length > 1 Then

                    For Each prestado In rwPrestamoASIM

                        hoja2.Range(filaExcel, 6, filaExcel, 10).Style.NumberFormat.NumberFormatId = 4

                        Dim tipoprestamo As DataRow() = nConsulta("select * from TipoPrestamo where iIdTipoPrestamo =" & prestado.Item("fkiIdTipoPrestamo"))
                        If tipoprestamo Is Nothing = False Then

                            tipo = tipoprestamo(0).Item("TipoPrestamo")
                            totalcobrado = nConsulta("SELECT SUM(monto) As TotalCobrado FROM PagoPrestamo WHERE fkiIdPrestamo=" & prestado.Item("iIdPrestamo"))
                            If prestado.Item("iEstatus") = 0 Then
                                hoja2.Range(filaExcel, 1, filaExcel, 10).Style.Fill.BackgroundColor = XLColor.RedPigment

                            End If
                            hoja2.Cell("A" & filaExcel).Value = prestado.Item("iIdPrestamo") ' codigo
                            hoja2.Cell("B" & filaExcel).Value = IIf(prestado.Item("iEstatus") = 1, "ACTIVO", "INACTIVO") ' Estatus
                            hoja2.Cell("C" & filaExcel).Value = prestado.Item("fechaprestamo") ' alta
                            hoja2.Cell("D" & filaExcel).Value = prestado.Item("cNombreLargo") ' Nombre
                            hoja2.Cell("E" & filaExcel).Value = tipo 'tipo 'Tipo de prestamo
                            hoja2.Cell("F" & filaExcel).Value = prestado.Item("montoTotal") 'Monto
                            hoja2.Cell("G" & filaExcel).Value = prestado.Item("descuento")

                            Dim dif As Double = prestado.Item("montoTotal") - prestado.Item("descuento")
                            If dif <= 1 Then
                                hoja2.Cell("H" & filaExcel).Value = prestado.Item("descuento")
                            Else
                                hoja2.Cell("H" & filaExcel).Value = totalcobrado(0).Item("TotalCobrado")
                            End If


                            hoja2.Cell("I" & filaExcel).FormulaA1 = "=F" & filaExcel & "-H" & filaExcel 'FALTANTE
                        End If


                        filaExcel = filaExcel + 1
                    Next
                End If
            End If

            Dim rwUsuario As DataRow() = nConsulta("Select * from Usuarios where idUsuario=1")


            dialogo.FileName = "Concentrado de Prestamos " & rwUsuario(0).Item("Nombre")
            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
            ''  dialogo.ShowDialog()

            If dialogo.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                ' OK button pressed
                libro.SaveAs(dialogo.FileName)
                libro = Nothing
                MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)



            Else
                MessageBox.Show("No se guardo el archivo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

 
   
End Class

