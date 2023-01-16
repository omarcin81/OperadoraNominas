Imports Microsoft.Office.Interop.Word 'control de office
Imports Microsoft.Office.Interop.Excel
Imports System.IO 'sistema de archivos
Imports Microsoft.Office.Interop
Imports System.Data
Imports System.Data.SqlClient

Public Class frmJuridico
    Public gIdEmpresa As String
    Public gIdCliente As String
    Public gIdEmpleado As String

    Private Sub frmJuridico_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim SQL As String
        'SQL = "select * from clientes where iIdCliente=" & gIdCliente
        'Dim rwFilas As DataRow() = nConsulta(SQL)

        'If rwFilas Is Nothing = False Then
        '    Dim Fila As DataRow = rwFilas(0)

        '    If Fila.Item("iTipoCliente") = 3 Then
        '        btnAsimilados.Visible = True

        '    End If
        'End If



    End Sub


    Private Sub cmdDeterminado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeterminado.Click

        Dim MSWord As New Word.Application
        Dim Documento As Word.Document
        Dim Ruta As String, strPWD As String
        Dim SQL As String

      
        Try
            Ruta = System.Windows.Forms.Application.StartupPath & "\Archivos\" & "XurtepDM" & ".docx"


            FileCopy(Ruta, "C:\Temp\Xurtep_Determinado_D.docx")
            Documento = MSWord.Documents.Open("C:\Temp\Xurtep_Determinado_D.docx")

            'Buscamos datos del empleado


            '' SQL = "SELECT * FROM (empleadosC INNER JOIN familiar on iIdEmpleadoC=fkiIdEmpleadoC) WHERE iIdEmpleadoC="
            SQL = "SELECT * FROM empleadosC WHERE iIdEmpleadoC="
            SQL &= gIdEmpleado
            Dim rwEmpleado As DataRow() = nConsulta(SQL)

            If rwEmpleado Is Nothing = False Then

                Dim fEmpleado As DataRow = rwEmpleado(0)


                Documento.Bookmarks.Item("cCodigoEmpleado").Range.Text = Trim(fEmpleado.Item("cCodigoEmpleado"))
                Documento.Bookmarks.Item("cCURP").Range.Text = Trim(fEmpleado.Item("cCURP"))
                Documento.Bookmarks.Item("cRFC").Range.Text = Trim(fEmpleado.Item("cRFC"))
                Documento.Bookmarks.Item("cIMSS").Range.Text = Trim(fEmpleado.Item("cIMSS"))
                Documento.Bookmarks.Item("cNacionalidad").Range.Text = Trim(fEmpleado.Item("cNacionalidad"))

                Documento.Bookmarks.Item("cDireccion").Range.Text = Trim(fEmpleado.Item("cDireccion"))
                Documento.Bookmarks.Item("cMunicipio").Range.Text = Trim(fEmpleado.Item("cCiudadL"))
                Documento.Bookmarks.Item("cCP").Range.Text = Trim(fEmpleado.Item("cCP"))
                Documento.Bookmarks.Item("cTelefono").Range.Text = Trim(fEmpleado.Item("cTelefono"))
                Documento.Bookmarks.Item("cNombreLargo").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cNombreLargo2").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cNombreLargo3").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cPuesto").Range.Text = Trim(fEmpleado.Item("cPuesto"))
                Documento.Bookmarks.Item("cLugarNac").Range.Text = Trim(fEmpleado.Item("cLugarNac"))

                Documento.Bookmarks.Item("cCategoria").Range.Text = IIf(fEmpleado.Item("iCategoria") = "0", "A", "B")
                Documento.Bookmarks.Item("iSexo").Range.Text = IIf(fEmpleado.Item("iSexo") = "0", "FEMENINO", "MASCULINO")

                Dim cEstado As DataRow() = nConsulta("SELECT * FROM Cat_Estados where iIdEstado=" & fEmpleado.Item("fkiIdEstado"))
                Documento.Bookmarks.Item("cEstado").Range.Text = Trim(cEstado(0).Item("cEstado"))

                
                Documento.Bookmarks.Item("iEstadoCivil").Range.Text = IIf(fEmpleado.Item("iEstadoCivil") = "0", "SOLTERO", "CASADO")
                Dim fechanac As Date
                fechanac = Trim(fEmpleado.Item("dFechaNac"))
                Dim edad As Integer = DateDiff(DateInterval.Year, fechanac, Date.Today)
                Documento.Bookmarks.Item("cEdad").Range.Text = edad
                Documento.Bookmarks.Item("dFecha").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("dFechaNac").Range.Text = fechanac
                Documento.Bookmarks.Item("dFecha2").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("fSueldoBase").Range.Text = Trim(fEmpleado.Item("fSueldoBase"))

                Documento.Bookmarks.Item("cNumeroCuenta").Range.Text = Trim(fEmpleado.Item("NumCuenta"))
                Documento.Bookmarks.Item("dFechaInicioContrato").Range.Text = DateTime.Now.ToString("dd/MM/yyyy") ''Trim(fEmpleado.Item("dFechaCap"))
                ''dFechaCap
                Documento.Bookmarks.Item("cTelefono2").Range.Text = Trim(fEmpleado.Item("cTelefono"))

                Dim cFamilia As DataRow() = nConsulta("SELECT * FROM familiar where fkiIdEmpleadoC=" & fEmpleado.Item("iIdEmpleadoC"))
                '' Dim tipoFamily As Integer = cFamilia(0).Item("fkIdTipoFamiliar")
                'Dim Conyuge As Boolean = False
                'Dim Padre As Boolean = False
                'Dim Madre As Boolean = False
                Dim Hijo1 As Boolean = False
                Dim hijo2 As Boolean = False
                Dim hijo3 As Boolean = False
                Dim hijo4 As Boolean = False

                If cFamilia Is Nothing = False Then

                    For Each Fila In cFamilia
                        Dim tipoFamily As Integer = Trim(Fila.Item("fkIdTipoFamiliar"))
                        If tipoFamily = 1 Then
                            If Hijo1 <> True Then
                                Documento.Bookmarks.Item("cHijo").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo").Range.Text = Trim(Fila.Item("dFechaNac"))
                                Hijo1 = True
                            ElseIf hijo2 <> True Then
                                Documento.Bookmarks.Item("cHijo2").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo2").Range.Text = Fila.Item("dFechaNac")
                                hijo2 = True
                            ElseIf hijo3 <> True Then
                                Documento.Bookmarks.Item("cHijo3").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo3").Range.Text = Trim(Fila.Item("dFechaNac"))
                                hijo3 = True
                            ElseIf hijo4 <> True Then
                                Documento.Bookmarks.Item("cHijo4").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo4").Range.Text = Trim(Fila.Item("dFechaNac"))
                                hijo4 = True

                            End If

                        End If
                        If tipoFamily = 2 Then
                            Documento.Bookmarks.Item("cPadre").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacPadre").Range.Text = Trim(Fila.Item("dFechaNac"))
                        End If
                        If tipoFamily = 3 Then
                            Documento.Bookmarks.Item("cMadre").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacMadre").Range.Text = Fila.Item("dFechaNac")
                        End If
                        If tipoFamily = 4 Then
                            Documento.Bookmarks.Item("cNombreConyuge").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacConyuge").Range.Text = Trim(Fila.Item("dFechaNac"))

                        End If
                    Next
                End If

                Dim cDocumento As DataRow() = nConsulta("SELECT * FROM documentos where fkiIdEmpleadoC=" & fEmpleado.Item("iIdEmpleadoC"))
                If cDocumento Is Nothing = False Then
                    For Each Fila In cDocumento
                        Dim cTipo As DataRow() = nConsulta("SELECT * FROM TipoDocumento where iIdTipoDocumento=" & Trim(Fila.Item("fkiIdTipoDocumento")))

                        If cTipo(0).Item("iIdTipoDocumento") = 1 Then
                            Documento.Bookmarks.Item("cReferendo").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cReferendoVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If
                        If cTipo(0).Item("iIdTipoDocumento") = 2 Then
                            Documento.Bookmarks.Item("cIdentidadM").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cIdentidadMVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If
                        If cTipo(0).Item("iIdTipoDocumento") = 3 Then
                            Documento.Bookmarks.Item("cCertificadoM").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cCertificadoMVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If

                    Next

                End If

                Documento.Save()
                MSWord.Visible = True

            End If

        Catch ex As Exception

            Documento.Close()

            MessageBox.Show(ex.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End Try
    End Sub

    Private Sub cmdPlanta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPlanta.Click

        Dim MSWord As New Word.Application
        Dim Documento As Word.Document
        Dim Ruta As String, strPWD As String
        Dim SQL As String

        Try
            Ruta = System.Windows.Forms.Application.StartupPath & "\Archivos\xurtepPlanta_ProcesoD.docx"


            FileCopy(Ruta, "C:\Temp\XURTEP_PLANTA_PROCESO_D.docx")
            Documento = MSWord.Documents.Open("C:\Temp\XURTEP_PLANTA_PROCESO_D.docx")

            'Buscamos datos del empleado


            '' SQL = "SELECT * FROM (empleadosC INNER JOIN familiar on iIdEmpleadoC=fkiIdEmpleadoC) WHERE iIdEmpleadoC="
            SQL = "SELECT * FROM empleadosC WHERE iIdEmpleadoC="
            SQL &= gIdEmpleado
            Dim rwEmpleado As DataRow() = nConsulta(SQL)

            If rwEmpleado Is Nothing = False Then

                Dim fEmpleado As DataRow = rwEmpleado(0)

                Documento.Bookmarks.Item("cCodigoEmpleado").Range.Text = Trim(fEmpleado.Item("cCodigoEmpleado"))
                Documento.Bookmarks.Item("cCURP").Range.Text = Trim(fEmpleado.Item("cCURP"))
                Documento.Bookmarks.Item("cRFC").Range.Text = Trim(fEmpleado.Item("cRFC"))
                Documento.Bookmarks.Item("cIMSS").Range.Text = Trim(fEmpleado.Item("cIMSS"))
                Documento.Bookmarks.Item("cNacionalidad").Range.Text = Trim(fEmpleado.Item("cNacionalidad"))

                Documento.Bookmarks.Item("cDireccion").Range.Text = Trim(fEmpleado.Item("cDireccion"))
                Documento.Bookmarks.Item("cCiudad").Range.Text = Trim(fEmpleado.Item("cCiudadL"))
                Documento.Bookmarks.Item("cCP").Range.Text = Trim(fEmpleado.Item("cCP"))
                Documento.Bookmarks.Item("cTelefono").Range.Text = Trim(fEmpleado.Item("cTelefono"))
                Documento.Bookmarks.Item("cNombreLargo").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cNombreLargo2").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cNombreLargo3").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cPuesto").Range.Text = Trim(fEmpleado.Item("cPuesto"))
                Documento.Bookmarks.Item("cLugarNac").Range.Text = Trim(fEmpleado.Item("cLugarNac"))

                Documento.Bookmarks.Item("cCategoria").Range.Text = IIf(fEmpleado.Item("iCategoria") = "0", "A", "B")
                Documento.Bookmarks.Item("iSexo").Range.Text = IIf(fEmpleado.Item("iSexo") = "0", "FEMENINO", "MASCULINO")

                Dim cEstado As DataRow() = nConsulta("SELECT * FROM Cat_Estados where iIdEstado=" & fEmpleado.Item("fkiIdEstado"))
                Documento.Bookmarks.Item("cEstado").Range.Text = Trim(cEstado(0).Item("cEstado"))


                Documento.Bookmarks.Item("iEstadoCivil").Range.Text = IIf(fEmpleado.Item("iEstadoCivil") = "0", "SOLTERO", "CASADO")
                Dim fechanac As Date
                fechanac = Trim(fEmpleado.Item("dFechaNac"))
                Dim edad As Integer = DateDiff(DateInterval.Year, fechanac, Date.Today)
                Documento.Bookmarks.Item("cEdad").Range.Text = edad
                Documento.Bookmarks.Item("dFecha").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("dFechaNac").Range.Text = fechanac
                Documento.Bookmarks.Item("dFecha2").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                Documento.Bookmarks.Item("fSueldoBase").Range.Text = Trim(fEmpleado.Item("fSueldoBase"))

                Documento.Bookmarks.Item("cNumeroCuenta").Range.Text = Trim(fEmpleado.Item("NumCuenta"))
                Documento.Bookmarks.Item("dFechaInicioContrato").Range.Text = DateTime.Now.ToString("dd/MM/yyyy") ''Trim(fEmpleado.Item("dFechaCap"))
                ''dFechaCap
                Documento.Bookmarks.Item("dFechaInicioContrato").Range.Text = DateTime.Now.ToString("dd/MM/yyyy")
                'dFechaFinContrato
                Documento.Bookmarks.Item("dFechaFinContrato").Range.Text = Trim(fEmpleado.Item("dFechaFin"))

                Dim cFamilia As DataRow() = nConsulta("SELECT * FROM familiar where fkiIdEmpleadoC=" & fEmpleado.Item("iIdEmpleadoC"))          

                Dim Hijo1 As Boolean = False
                Dim hijo2 As Boolean = False
                Dim hijo3 As Boolean = False
                Dim hijo4 As Boolean = False

                If cFamilia Is Nothing = False Then

                    For Each Fila In cFamilia
                        Dim tipoFamily As Integer = Trim(Fila.Item("fkIdTipoFamiliar"))
                        If tipoFamily = 1 Then
                            If Hijo1 <> True Then
                                Documento.Bookmarks.Item("cHijo").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo").Range.Text = Trim(Fila.Item("dFechaNac"))
                                Hijo1 = True
                            ElseIf hijo2 <> True Then
                                Documento.Bookmarks.Item("cHijo2").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo2").Range.Text = Fila.Item("dFechaNac")
                                hijo2 = True
                            ElseIf hijo3 <> True Then
                                Documento.Bookmarks.Item("cHijo3").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo3").Range.Text = Trim(Fila.Item("dFechaNac"))
                                hijo3 = True
                            ElseIf hijo4 <> True Then
                                Documento.Bookmarks.Item("cHijo4").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                                Documento.Bookmarks.Item("dFecNacHijo4").Range.Text = Trim(Fila.Item("dFechaNac"))
                                hijo4 = True
                            End If

                        End If
                        If tipoFamily = 2 Then
                            Documento.Bookmarks.Item("cPadre").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacPadre").Range.Text = Trim(Fila.Item("dFechaNac"))
                        End If
                        If tipoFamily = 3 Then
                            Documento.Bookmarks.Item("cMadre").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacMadre").Range.Text = Fila.Item("dFechaNac")
                        End If
                        If tipoFamily = 4 Then
                            Documento.Bookmarks.Item("cNombreConyuge").Range.Text = Trim(Fila.Item("cNombre")) & " " & Trim(Fila.Item("cApellidoP")) & " " & Trim(Fila.Item("cApellidoM"))
                            Documento.Bookmarks.Item("dFecNacConyuge").Range.Text = Trim(Fila.Item("dFechaNac"))

                        End If
                    Next
                End If

                Dim cDocumento As DataRow() = nConsulta("SELECT * FROM documentos where fkiIdEmpleadoC=" & fEmpleado.Item("iIdEmpleadoC"))
                If cDocumento Is Nothing = False Then


                    For Each Fila In cDocumento
                        Dim cTipo As DataRow() = nConsulta("SELECT * FROM TipoDocumento where iIdTipoDocumento=" & Trim(Fila.Item("fkiIdTipoDocumento")))

                        If cTipo(0).Item("iIdTipoDocumento") = 1 Then
                            Documento.Bookmarks.Item("cReferendo").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cReferendoVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If
                        If cTipo(0).Item("iIdTipoDocumento") = 2 Then
                            Documento.Bookmarks.Item("cIdentidadM").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cIdentidadMVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If
                        If cTipo(0).Item("iIdTipoDocumento") = 3 Then
                            Documento.Bookmarks.Item("cCertificadoM").Range.Text = Trim(Fila.Item("cCodigo"))
                            Documento.Bookmarks.Item("cCertificadoMVencimiento").Range.Text = Trim(Fila.Item("dFechaVencimiento"))
                        End If

                    Next
                End If

                Documento.Save()
                MSWord.Visible = True

            End If

        Catch ex As Exception

            Documento.Close()

            MessageBox.Show(ex.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    
   
    Private Sub cmdConvenios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConvenios.Click
        Dim MSWord As New Word.Application
        Dim Documento As Word.Document
        Dim Ruta As String, strPWD As String
        Dim SQL As String
        Try
            Ruta = System.Windows.Forms.Application.StartupPath & "\Archivos\convenio_practicas.doc"


            FileCopy(Ruta, "C:\Temp\XURTEP_CONVENIO_OFICIAL_PRACTICAS.doc")
            Documento = MSWord.Documents.Open("C:\Temp\XURTEP_CONVENIO_OFICIAL_PRACTICAS.doc")

            'Buscamos datos del empleado


            '' SQL = "SELECT * FROM (empleadosC INNER JOIN familiar on iIdEmpleadoC=fkiIdEmpleadoC) WHERE iIdEmpleadoC="
            SQL = "SELECT * FROM empleadosC WHERE iIdEmpleadoC="
            SQL &= gIdEmpleado
            Dim rwEmpleado As DataRow() = nConsulta(SQL)
            If rwEmpleado Is Nothing = False Then

                Dim fEmpleado As DataRow = rwEmpleado(0)

                Documento.Bookmarks.Item("cNombreLargo").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))
                Documento.Bookmarks.Item("cNombreLargo2").Range.Text = Trim(fEmpleado.Item("cNombre")) & " " & Trim(fEmpleado.Item("cApellidoP")) & " " & Trim(fEmpleado.Item("cApellidoM"))

                Documento.Bookmarks.Item("cNacionalidad").Range.Text = Trim(fEmpleado.Item("cNacionalidad"))
                Documento.Bookmarks.Item("cDireccion").Range.Text = Trim(fEmpleado.Item("cDireccion"))
                Documento.Bookmarks.Item("iSexo").Range.Text = IIf(fEmpleado.Item("iSexo") = "0", "FEMENINO", "MASCULINO")
                Documento.Bookmarks.Item("iEstadoCivil").Range.Text = IIf(fEmpleado.Item("iEstadoCivil") = "0", "SOLTERO", "CASADO")

                Dim fechanac As Date
                fechanac = Trim(fEmpleado.Item("dFechaNac"))
                Dim hoy As Date = Date.Now.ToLongDateString() ''.ToString("dd/MM/yyyy")
                Dim hoys As String = Date.Now.ToLongDateString()
                Dim hoyA As String() = hoys.Split(",")

                Dim edad As Integer = DateDiff(DateInterval.Year, fechanac, Date.Today)
                Documento.Bookmarks.Item("cEdad").Range.Text = edad

                Documento.Bookmarks.Item("dFechaIn").Range.Text = hoyA(1)
                Documento.Bookmarks.Item("dFechaIn2").Range.Text = hoyA(1)
                Dim fin As Date = Trim(fEmpleado.Item("dFechaFin"))
                hoyA = fin.ToLongDateString().Split(",")
                Documento.Bookmarks.Item("dFechaFin").Range.Text = hoyA(1)

                Documento.Bookmarks.Item("cPuesto").Range.Text = fEmpleado.Item("cFuncionesPuesto")


                Documento.Save()
                MSWord.Visible = True


            End If


        Catch ex As Exception

            Documento.Close()

            MessageBox.Show(ex.ToString(), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub

  
End Class