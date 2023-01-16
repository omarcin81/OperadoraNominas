Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Drawing2D
Imports System.Security.Cryptography
Imports System.Text
'Imports ThoughtWorks.QRCode
'Imports ThoughtWorks.QRCode.Codec
'Imports CFDIGenerator.CFDIGenerator
'Primer cambio

Module mdoFunciones
    'Variables y Constantes Agregadas
    '    Public Principalfrm As frmInicial
    Public gNumCausa As String
    Public gIdPeticion As String, gblVisor As Integer
    Public gblBuscar As String
    Public ChangeUSR As Boolean = False
    Public gAlumnos As String = "Alumno"
    Public idUsuario As String

    'Private WithEvents _PDFCreator As PDFCreator.clsPDFCreator
    'Dim pErr As New PDFCreator.clsPDFCreatorError
    Private WithEvents Timer1 As New Timer
    Dim ReadyState As Boolean

    Private Const FLASHW_STOP As Integer = &H0
    Private Const FLASHW_CAPTION As Integer = &H1
    Private Const FLASHW_TRAY As Integer = &H2
    Private Const FLASHW_ALL As Integer = (FLASHW_CAPTION Or FLASHW_TRAY)
    Private Const FLASHW_TIMER As Integer = &H4
    Private Const FLASHW_TIMERNOFG As Integer = &HC

    Structure FLASHWINFO
        Dim cbSize As Integer
        Dim hwnd As System.IntPtr
        Dim dwFlags As Integer
        Dim uCount As Integer
        Dim dwTimeout As Integer
    End Structure

    <DllImport("user32.dll")> _
    Public Function FlashWindowEx(ByRef pfwi As FLASHWINFO) As Integer
    End Function

    Public Function ConfigurarSistema(ByVal Parametro As String, ByVal Valor As String)
        Dim SQL As String
        Try

            SQL = "SELECT * FROM Configuracion WHERE IdCFG=1"
            If ObtencampoDR(SQL, 0) = "" Then
                SQL = "INSERT INTO Configuracion (IdCFG," & Parametro & ") VALUES(1,'" & Valor & "')"
            Else
                SQL = "UPDATE Configuracion SET " & Parametro & "='" & Valor & "' WHERE IdCFG=1"
            End If
            If nExecute(SQL) = False Then
                Return False
            End If
            Return True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Guardar configuración", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        Return False
    End Function

    'Public Function CrearComprobante(ByRef edDocumento As Comprobante) As Boolean

    '    Dim sCertificado As String = "", SQL As String
    '    Dim sLlave As String = ""
    '    Dim sClave As String = ""

    '    SQL = "SELECT Certificado, Llave, Clave FROM Configuracion"


    '    Dim rwCER() As DataRow = nConsultaAC(SQL)
    '    If rwCER Is Nothing = False Then
    '        If rwCER.Length > 0 Then
    '            If IsDBNull(rwCER(0).Item(0)) = False Then
    '                sCertificado = Application.StartupPath & "\Archivos\Certificados\" & rwCER(0)!Certificado
    '            End If
    '            If IsDBNull(rwCER(0).Item(1)) = False Then
    '                sLlave = Application.StartupPath & "\Archivos\Certificados\" & rwCER(0)!Llave
    '            End If
    '            sClave = NewCrypt("" & rwCER(0)!Clave, True)
    '        End If
    '    End If


    '    Dim mensaje As String = ""
    '    If sCertificado = "" Then
    '        mensaje = "No se pudo encontrar el archivo de certificado para generar los comprobantes digitales."
    '    End If
    '    If sLlave = "" And mensaje = "" Then
    '        mensaje = "No se pudo encontrar el archivo llave del certificado para generar los comprobantes digitales."
    '    End If
    '    If sClave = "" And mensaje = "" Then
    '        mensaje = "No se encuentra registrada la clave para la generación de comprobantes"
    '    End If

    '    If mensaje <> "" Then
    '        If MessageBox.Show(mensaje & vbCrLf & vbCrLf & "¿Desea continuar de todas maneras? No podrá generar comprobantes digitales", "Verificación de archivos necesarios.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.No Then
    '            Return False
    '        End If
    '    End If
    '    Try
    '        edDocumento = New Comprobante()
    '        edDocumento.RutaXSLT = Application.StartupPath & "\Archivos\schema\cadenaoriginal_3_2.xml"
    '        edDocumento.RutaSSL = "C:\Program Files (x86)\GnuWin32\bin\openssssl.exe"
    '        edDocumento.RutaXSLTTFD = Application.StartupPath & "\Archivos\schema\cadenaoriginal_TFD_1_0.xslt"
    '        edDocumento.RutaCerFile = sCertificado
    '        edDocumento.RutaKeyFile = sLlave
    '        edDocumento.PrivateKey = sClave
    '        Return True
    '    Catch ex As Exception
    '        MessageBox.Show("Error crítico: No se han podido validar los certificados para la generación de documentos. Pongase en contacto con el proveedor del producto." & vbCrLf & vbCrLf & ex.Message, "Verificación del sello digital", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return False
    '    End Try

    'End Function

    'Public Sub GenerarQRCode(ByVal Archivo As String, ByVal RFCEmisor As String, ByVal RFCReceptor As String, ByVal Total As Double, ByVal UUID As String)
    '    Dim Cadena As String

    '    Cadena = "?re=" & RFCEmisor.ToUpper().Replace("-", "").Replace(" ", "").Trim()
    '    Cadena &= "&rr=" & RFCReceptor.ToUpper().Replace("-", "").Replace(" ", "").Trim()
    '    Cadena &= "&tt=" & Total.ToString(StrDup(10, "0") & "." & StrDup(6, "0"))
    '    Cadena &= "&id=" & UUID

    '    Dim GenerarCBB As QRCodeEncoder = New QRCodeEncoder
    '    GenerarCBB.QRCodeEncodeMode = Codec.QRCodeEncoder.ENCODE_MODE.BYTE
    '    GenerarCBB.QRCodeScale = Int32.Parse(4)
    '    GenerarCBB.QRCodeErrorCorrect = Codec.QRCodeEncoder.ERROR_CORRECTION.M
    '    GenerarCBB.QRCodeVersion = 0
    '    GenerarCBB.QRCodeBackgroundColor = Color.White
    '    GenerarCBB.QRCodeForegroundColor = Color.Black
    '    Try
    '        GenerarCBB.Encode(Cadena, System.Text.Encoding.UTF8).Save(Archivo, Imaging.ImageFormat.Png)
    '    Catch ex As Exception
    '        ShowError(ex, "Generar CBB")
    '    End Try


    'End Sub

    Public Function AlmacenarCiudad(ByVal Nombre As String, ByVal IdEstado As String, Optional ByVal EnTransaccion As Boolean = True) As String
        Try
            If Nombre.Trim.Length <= 0 Then
                Return ""
            End If

            Dim SQL As String, IdCiudad As String

            SQL = "SELECT IdCiudad FROM CatCiudades WHERE Nombre='" & Nombre.Trim() & "' AND IdEstado='" & IdEstado & "'"
            IdCiudad = ObtencampoDR(SQL, 0)
            If IdCiudad = "" Then
                SQL = "SELECT MAX(RIGHT(IdCiudad,4)) FROM CatCiudades WHERE LEFT(IdCiudad,2)='" & Servidor.gIdSec & "'"
                IdCiudad = Servidor.gIdSec & (Val(ObtencampoDR(SQL, 0)) + 1).ToString("0000")
                SQL = "INSERT INTO CatCiudades VALUES('" & IdCiudad & "','" & Nombre.Trim & "','" & IdEstado & "')"
                If nExecute(SQL, "CatCiudades", IdCiudad) = False Then
                    If EnTransaccion Then
                        Transaccion.Rollback()
                    End If
                    Return ""
                End If
            End If
            Return IdCiudad
        Catch ex As Exception
            ShowError(ex, "AlmacenarCiudad")
        End Try
        Return ""
    End Function

    Public Function AlmacenarMunicipio(ByVal Nombre As String, ByVal IdEstado As String, Optional ByVal EnTransaccion As Boolean = True) As String
        Try
            If Nombre.Trim.Length <= 0 Then
                Return ""
            End If

            Dim SQL As String, IdMunicipio As String

            SQL = "SELECT IdMunicipio FROM CatMunicipios WHERE Descripcion='" & Nombre.Trim() & "' AND IdEstado='" & IdEstado & "'"
            IdMunicipio = ObtencampoDR(SQL, 0)
            If IdMunicipio = "" Then
                SQL = "SELECT MAX(RIGHT(IdMunicipio,4)) FROM CatMunicipios WHERE LEFT(IdMunicipio,2)='" & Servidor.gIdSec & "'"
                IdMunicipio = Servidor.gIdSec & (Val(ObtencampoDR(SQL, 0)) + 1).ToString("0000")
                SQL = "INSERT INTO CatMunicipios VALUES('" & IdMunicipio & "','" & Nombre.Trim & "','" & IdEstado & "')"
                If nExecute(SQL, "CatMunicipios", IdMunicipio) = False Then
                    If EnTransaccion Then
                        Transaccion.Rollback()
                    End If
                    Return ""
                End If
            End If
            Return IdMunicipio
        Catch ex As Exception
            ShowError(ex, "AlmacenarMunicipio")
        End Try
        Return ""
    End Function

    Public Function RegistrarFolio(ByVal Folio As String)
        Dim SQL As String
        RegistrarFolio = True

        'RESTAR EL FOLIO
        Dim PrivateKey As String = clsPAC.Password
        SQL = "SELECT * FROM Register WHERE Status=1"
        Dim Licencias = nConsulta(SQL)
        If Licencias Is Nothing = False Then
            Dim sLicencia As String
            Dim Partes() As String
            Dim iFolio As Integer, Usado As Integer

            sLicencia = Desencryptar(Licencias(0)!Key, PrivateKey)
            Partes = sLicencia.Split("|")
            If Partes.Length = 5 Then
                iFolio = Val(Partes(2))
                Usado = Val(Partes(4))
                Usado += 1
                sLicencia = Licencias(0)!RecordID & "|" & Partes(1) & "|" & Partes(2) & "|" & Partes(3) & "|" & Usado.ToString()
                sLicencia = Encryptar(sLicencia, PrivateKey)
                Transaccion = CONEXION.BeginTransaction
                SQL = "DELETE  FROM Register WHERE RecordID='" & Licencias(0)!RecordID & "'"
                If nExecute(SQL) = False Then
                    Transaccion.Rollback()
                    Return False
                    Exit Function
                End If

                SQL = "INSERT INTO Register VALUES('" & Licencias(0)!RecordID & "','" & sLicencia & "'," & IIf(Usado >= iFolio, "0", "1") & ")"
                If nExecute(SQL) = False Then
                    Transaccion.Rollback()
                    Return False
                    Exit Function
                End If

                SQL = "UPDATE Facturacion SET Paquete='" & Licencias(0)!RecordID & "' WHERE Folio='" & Folio & "'"
                If Execute(SQL, Folio, "Facturacion") = False Then
                    Transaccion.Rollback()
                    Return False
                    Exit Function
                End If
                Transaccion.Commit()
            End If
        End If
    End Function

    Public Function ValidarFoliosActivos() As Boolean

        clsPAC.Configurar()
        Dim PrivateKey As String = clsPAC.Password
        Dim SQL As String = ""

        SQL = "SELECT * FROM Register WHERE Status=1"

        Dim iFolio As Integer = 0, Usado As Integer = 0

        Dim Licencias = nConsulta(SQL)
        If Licencias Is Nothing = False Then
            Dim sLicencia As String
            Dim Partes() As String
            clsPAC.Configurar()
            sLicencia = Desencryptar(Licencias(0)!Key, clsPAC.Password)
            Partes = sLicencia.Split("|")
            If Partes.Length = 5 Then
                iFolio = Val(Partes(2))
                Usado = Val(Partes(4))
                If Usado >= iFolio Then
                    MessageBox.Show("¡IMPORTANTE! Ya no cuenta con folios activos. Debe solicitar otra licencia a su proveedor.", "Validación de folios fiscales", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    MessageBox.Show("El CFDI no se ha generado.", "Validación de folios fiscales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                ElseIf (iFolio - Usado) < 100 Then
                    MessageBox.Show("¡IMPORTANTE! Restan menos de 100 folios para ser certificados.", "Validación de folios fiscales", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Else
            MessageBox.Show("¡IMPORTANTE! Ya no cuenta con folios activos. Debe solicitar otra licencia a su proveedor.", "Validación de folios fiscales", MessageBoxButtons.OK, MessageBoxIcon.Information)
            MessageBox.Show("El CFDI no se ha generado.", "Validación de folios fiscales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If
        Return True

    End Function


    Public Function GuardarFacturaDigital(ByVal Folio As String, ByVal Ruta As String, Optional ByVal CFDIT As String = "CFDI") As Boolean
        Try
            Dim SQL As String = "SELECT Folio FROM Facturacion WHERE Folio='" & Folio & "'"
            If ObtencampoDR(SQL, 0) = "" Then

                SQL = "INSERT INTO Facturacion (Folio,FechaTimbrado,Status,IdUsuario) VALUES ('" & Folio & "',GETDATE(),1,'" & Usuario.Id & "')"
                If Execute(SQL, Folio, "Facturacion") = False Then
                    Return False
                End If
            End If

            Dim Cmd As SqlCommand = CONEXION.CreateCommand()
            Cmd.CommandText = "UPDATE Facturacion SET " & CFDIT & "=@CFDI WHERE Folio='" & Folio & "'"

            Dim Lector As New XmlTextReader(Ruta)

            Dim ParametroSQLXML As SqlXml = New SqlXml(Lector)

            Cmd.Parameters.AddWithValue("@CFDI", ParametroSQLXML)

            Cmd.ExecuteNonQuery()
            Lector.Close()

            CargarXMLArchivo(Folio, Ruta, CFDIT)
            If CFDIT.ToUpper = "XCFDIT" Then
                RegistrarFolio(Folio)
            End If


            If Servidor.Central = False And Folio <> "" Then
                SQL = "UPDATE Recibos SET Folio=Folio"
                nExecute(SQL, "Recibos", Folio)
            End If

            Return True
        Catch ex As Exception
            ShowError(ex, "GuardarFacturaDigital")
        End Try
        Return False
    End Function

    Public Function CargarXMLArchivo(ByVal Folio As String, ByVal Ruta As String, ByVal CFDIT As String) As Boolean
        Try
            Dim Documento As New XmlDocument

            Dim cm As New SqlCommand("SELECT " & CFDIT & " FROM Facturacion WHERE Folio='" & Folio & "'", CONEXION)
            Dim dr As SqlDataReader = cm.ExecuteReader
            If dr.Read Then
                Dim Indice = dr.GetOrdinal(CFDIT)
                If IsDBNull(dr.Item(Indice)) = False Then
                    Dim Campo As SqlXml = dr.GetSqlXml(Indice)
                    If IsDBNull(Campo.Value) = False Then
                        Documento.LoadXml(Campo.Value)
                        dr.Close()

                        If System.IO.File.Exists(Ruta) Then
                            System.IO.File.Delete(Ruta)
                        End If

                        Documento.Save(Ruta)
                        Return True
                    Else
                        dr.Close()
                        Return False
                    End If
                Else
                    dr.Close()
                End If

            Else
                dr.Close()
            End If
        Catch ex As Exception
            ShowError(ex, "CargarXMLArchivo")
        End Try
        Return False
    End Function


    Public Function Encryptar(ByVal Texto As String, ByVal PrivateKey As String) As String
        Dim Enc As New UTF8Encoding()
        Dim Datos As Byte() = Enc.GetBytes(Texto)
        Dim Ms As New MemoryStream(Datos)
        Dim r As New DESCryptoServiceProvider()
        r.Key = Encoding.Default.GetBytes(PrivateKey.Substring(0, 8))
        r.IV = Encoding.Default.GetBytes(PrivateKey.Substring(0, 8))
        Dim msTMP As New MemoryStream()
        Dim cs As New CryptoStream(msTMP, r.CreateEncryptor, CryptoStreamMode.Write)
        cs.Write(Datos, 0, Datos.Length)
        cs.Close()
        Return Convert.ToBase64String(Ms.ToArray())

    End Function


    Public Function Desencryptar(ByVal cTextoADesencriptar As String, ByVal PrivateKey As String) As String
        Try
            Dim r As New DESCryptoServiceProvider()
            r.Key = Encoding.Default.GetBytes(PrivateKey.Substring(0, 8))
            r.IV = Encoding.Default.GetBytes(PrivateKey.Substring(0, 8))
            Dim ms As New MemoryStream(Convert.FromBase64String(cTextoADesencriptar), False)
            Dim cs As New CryptoStream(ms, r.CreateEncryptor(), CryptoStreamMode.Read)
            Dim sr As New StreamReader(cs)
            sr = New StreamReader(ms)
            Dim ret As String = sr.ReadToEnd
            sr.Close()
            Return ret
        Catch ex As Exception
            Return ""
        End Try

    End Function


    Public Sub CargarCiclos(ByRef Combo As ComboBox)
        Try
            Combo.Items.Clear()
            For i As Integer = 2000 To Servidor.Fecha.Year
                Combo.Items.Add(i & " - " & (i + 1).ToString)
            Next
            Combo.SelectedIndex = Servidor.Fecha.Year - 2001
        Catch ex As Exception
            ShowError(ex, "CarcarCiclos")
        End Try

    End Sub

    Public Sub Redondeo(ByRef Monto As Double, ByVal Recargo As Boolean)
        'Monto = gDesAum * gMonto
        Dim Entero As Long = Math.Abs(Math.Truncate(Monto))
        If (Math.Abs(Monto) - Entero) >= 0.5 Then
            Monto = Entero + 1
        Else
            Monto = Math.Truncate(Monto)
        End If
    End Sub

    Public Sub gMostrarSecciones(ByRef cboSecciones As ComboBox)
        Dim SQL As String
        SQL = "SELECT IdSeccion, Nombre FROM CatSecciones ORDER BY IdSeccion"
        cboSecciones.DataSource = Nothing
        nCargaCBO(cboSecciones, SQL, "Nombre", "IdSeccion")

    End Sub

    Public Sub ShowError(ByVal ex As Exception, Optional Titulo As String = "Error en el proceso")
        MessageBox.Show(ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Function GenDomFiscal(ByVal Calle As String, ByVal NoExt As String, ByVal NoInt As String) As String

        If Calle <> "" Then
            If (InStr(Calle, " No.", CompareMethod.Text) > 0 Or InStr(Calle, " #", CompareMethod.Text) Or InStr(Calle, "s/n", CompareMethod.Text)) And (Not InStr(Calle, " 1#", CompareMethod.Text) > 0 And Not InStr(Calle, " 2#", CompareMethod.Text) > 0) Then
                Return Calle
                Exit Function
            End If

            Dim Domicilio As String = ""
            Domicilio = Replace(Calle, "1#", IIf(NoExt = "0" Or NoExt = "", "s/n", " No. " & NoExt))
            Domicilio = Domicilio.Replace("2#", IIf(NoInt = "0" Or NoInt = "", "", "-" & NoInt))
            If InStr(Calle, "1#") <= 0 And NoExt <> "0" And NoExt.ToLower <> "s/n" Then
                Domicilio &= " No. " & NoExt
            End If
            If InStr(Calle, "2#") <= 0 And NoInt <> "0" And NoInt <> "" And NoInt.ToLower <> "s/n" Then
                Domicilio &= "-" & NoInt
            End If
            Return Domicilio
        Else
            Return ""
        End If
    End Function

    Public Function NoMes(ByVal Mes As String) As Integer

        For i As Integer = 1 To 12
            If StrComp(Mes, MonthName(i, True), CompareMethod.Text) = 0 Or StrComp(Mes, MonthName(i), CompareMethod.Text) = 0 Then
                Return i
            End If
        Next
        Return 0
    End Function

    Public Function UltimoDia(ByVal Mes As Integer, ByVal Anio As Integer) As Integer
        Return Date.DaysInMonth(Anio, Mes)
        'Select Case Mes
        '    Case 2 : Return 28 + IIf((Anio Mod 4) = 0, 1, 0)
        '    Case 4, 6, 9, 11 : Return 30
        '    Case Else : Return 31
        'End Select
    End Function

    Public Function SinAcentos(ByVal Cadena As String) As String
        Dim Caracteres As String() = Split("A|Á|E|É|I|Í|O|Ó|U|Ú|A|Ä|E|Ë|I|Ï|O|Ö|U|Ü", "|")

        For i As Integer = 0 To Caracteres.Length - 1 Step 2
            Cadena = Replace(Cadena, Caracteres(i + 1), Caracteres(i), 1, , CompareMethod.Text)
        Next

        Return Cadena

    End Function

    Public Sub PonerHandler(ByRef Contenedor As Object)
        'Dim oControl As Object
        Try
            For Each oControl In Contenedor.controls
                If TypeOf oControl Is TabControl Or TypeOf oControl Is GroupBox Or TypeOf oControl Is Panel Then
                    PonerHandler(oControl)
                ElseIf TypeOf oControl Is TextBox Then
                    Dim txtControl As TextBox = oControl
                    AddHandler txtControl.GotFocus, AddressOf txtNombre_GotFocus
                    AddHandler txtControl.LostFocus, AddressOf txtNombre_LostFocus
                    AddHandler txtControl.EnabledChanged, AddressOf txtOtro_EnabledChanged
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Fuente As New Font(sender.font.name.ToString, Convert.ToInt32(sender.font.size), FontStyle.Bold)
        sender.BackColor = Color.Yellow
        'sender.font = Fuente
    End Sub

    Private Sub txtNombre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Fuente As New Font(sender.font.name.ToString, Convert.ToInt32(sender.font.size), FontStyle.Regular)
        If sender.enabled Then
            sender.BackColor = Color.White
        Else
            sender.BackColor = Color.WhiteSmoke
        End If

        ' sender.font = Fuente
    End Sub

    Private Sub txtOtro_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.enabled = False Then
            sender.BackColor = Color.WhiteSmoke
        End If
    End Sub


    Public Function DiffMes(ByVal AnioI As Long, ByVal MesI As Integer, ByVal AnioF As Long, ByVal MesF As Integer) As Long
        Return ((AnioF * 12) + MesF) - ((AnioI * 12) + MesI)
    End Function

    Public Function DiffMes(ByVal FechaI As Date, ByVal FechaF As Date) As Long
        Return ((FechaF.Year * 12) + FechaF.Month) - ((FechaI.Year * 12) + FechaI.Month)
    End Function

    Public Sub SaltaInhabiles(ByRef Fecha As Date)
        'Dim SQL As String, rwFilas As DataRow()
        If Weekday(Fecha, FirstDayOfWeek.Monday) > 5 Then 'SI EL DIA ES SABADO O DOMINGO
            Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            SaltaInhabiles(Fecha)
        Else
            'SQL = "SELECT * FROM DiasFestivos  WHERE DiaFestivo='" & Fecha & "'"
            'rwFilas = nConsulta(SQL)
            'If rwFilas Is Nothing = False Then
            '    If rwFilas.Length > 0 Then 'SI EL DIA SELECCIONADO ESTA MARCADO COMO DIA INHABIL
            '        Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            '        SaltaInhabiles(Fecha)
            '    End If
            'End If
        End If
    End Sub


    Public Sub ValidarArchivo(ByRef Archivo As String)

        Dim nArchivo As Integer = 0
        Dim nNombre As String = Archivo
        Dim CFDDir As String = Mid(Archivo, 1, InStrRev(Archivo, "\"))
        Dim Folio As String = Path.GetFileNameWithoutExtension(Archivo)
        Dim Ext As String = Path.GetExtension(Archivo)

        Do
            If nArchivo > 0 Then
                Archivo = CFDDir & Folio & "_" & nArchivo.ToString & Ext
            End If

            If File.Exists(Archivo) Then
                Application.DoEvents()
                Threading.Thread.Sleep(1000)
                Try
                    File.Delete(Archivo)
                Catch ex As Exception
                    'MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'Exit Sub
                End Try
            End If
            nArchivo += 1
        Loop While File.Exists(Archivo)

    End Sub

    Public Sub Degradado(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal Control As Object, ByVal Color1 As System.Drawing.Color, ByVal Color2 As System.Drawing.Color, Optional ByVal Direccion As LinearGradientMode = LinearGradientMode.Vertical, Optional ByVal b3D As Boolean = False)
        Dim Brocha As LinearGradientBrush
        Dim Superficie As Graphics
        Dim Rectangulo As Rectangle
        Dim Rectangulo2 As Rectangle
        Dim Lapiz As Pen

        Try

            'Aquí igualamos la variable superficie a los argumentos del panel
            Superficie = e.Graphics
            'Aquí seleccionaremos el color del borde (yo lo he puesto azul oscuro)
            '  Lapiz = New Pen(Color.Navy, 1)

            'Le damos el tamaño al rectángulo, cero a la altura y cero a la izquierda y
            'el tamaño usamos las propiedades del Panel
            '----Rectangulo = New Rectangle(0, 0, Control.Width, Control.Height)
            'Aquí elegimos los colores del degradado y la forma del degradado
            'Aquí esta puesto ForwardDiagonal, es decir, de esquina superior a la esquina inferior
            'juega con esa propiedad para ver sus efectos
            '---- Brocha = New System.Drawing.Drawing2D.LinearGradientBrush(Rectangulo, Color1, Color2, Direccion)
            Control.backcolor = Color2
            If b3D Then
                Rectangulo = New Rectangle(0, 0, Control.Width, (Control.Height / 2) + 3)
                Rectangulo2 = New Rectangle(0, (Control.Height / 2), Control.Width, Control.Height / 2)

                Brocha = New System.Drawing.Drawing2D.LinearGradientBrush(Rectangulo2, Color2, Color1, Direccion)
                Superficie.FillRectangle(Brocha, Rectangulo2)

                Brocha = New System.Drawing.Drawing2D.LinearGradientBrush(Rectangulo, Color1, Color2, Direccion)
                Superficie.FillRectangle(Brocha, Rectangulo)

            Else
                Lapiz = New Pen(Color.Navy, 1)
                Rectangulo = New Rectangle(0, 0, Control.Width, Control.Height)
                Brocha = New System.Drawing.Drawing2D.LinearGradientBrush(Rectangulo, Color1, Color2, Direccion)
                'Aquí pintamos el cuadrado y luego el borde
                Superficie.FillRectangle(Brocha, Rectangulo)
                Superficie.DrawRectangle(Lapiz, Rectangulo)
            End If



            'Lo liberamos de la memoria
            Lapiz.Dispose()
            Superficie.Dispose()
        Catch ex As Exception
            'No hacemos nada si falla. Si hay error
        End Try
    End Sub

    Public Function ImprimeLetra(ByVal Numero As Double, Optional ByVal Monedas As Boolean = True, Optional ByVal Mayusculas As Boolean = True) As String
        Dim aCENT As String(), aDEC As String(), aUNI As String(), aOP As String(), aTIPO As String()
        Dim Cadenas(0 To 2) As String
        Dim i As Integer, j As Integer
        Dim aUDCTipos() As Long
        Dim lngCentavos As Integer, aux As Double
        Dim Tercias As Integer, StrNumero As String

        aCENT = (" | Cien | Doscientos | Trescientos | Cuatrocientos | Quinientos |" & _
             " Seiscientos | Setecientos | Ochocientos | Novecientos | Ciento").Split("|")
        aDEC = (" | Diez | Veinte | Treinta | Cuarenta | Cincuenta | Sesenta |" & _
            " Setenta | Ochenta | Noventa ").Split("|")
        aUNI = (" | Cero|Uno|Dos|Tres|Cuatro|Cinco|Seis|Siete|Ocho|Nueve").Split("|")
        aOP = (" Once| Doce| Trece| Catorce| Quince|Dieciseis|Diecisiete|Dieciocho|Diecinueve").Split("|")
        aTIPO = (" | Mil | Millon").Split("|")

        lngCentavos = (Numero - Int(Numero)) * 100
        Numero = Int(Numero)
        aux = Numero
        Tercias = Len(Trim(Str(Numero))) \ 3
        If Len(Trim(Str(Numero))) Mod 3 > 0 Then
            Tercias = Tercias + 1
        End If
        ReDim aUDCTipos(0 To ((Tercias * 3) - 1))
        StrNumero = Format(Numero, StrDup(Tercias * 3, "0"))
        For i = (Tercias * 3) - 1 To 0 Step -1
            'aUDCTipos(i) = Numero \ ((10 ^ i))
            'Numero = Abs(Numero - ((10 ^ i) * aUDCTipos(i)))
            aUDCTipos(i) = Val(Mid(StrNumero, ((Tercias * 3) - i), 1))
        Next
        Numero = IIf(Numero < 0, Numero * -1, Numero)
        i = (Tercias * 3) - 1
        For j = 0 To Tercias - 1
            Cadenas(j) = ""

            If aUDCTipos(i) = 1 And (aUDCTipos(i - 1) < 1 And aUDCTipos(i - 2) < 1) Then
                Cadenas(j) = "Cien"
            ElseIf (aUDCTipos(i) = 1 And (aUDCTipos(i - 1) > 0 Or aUDCTipos(i - 2) > 0)) Then
                Cadenas(j) = "Ciento "
            ElseIf (aUDCTipos(i) > 0) Then
                Cadenas(j) = aCENT(aUDCTipos(i))
            End If
            If (aUDCTipos(i - 1) = 1 And (aUDCTipos(i - 2) > 0 And aUDCTipos(i - 2) > 0)) Then
                Cadenas(j) = Cadenas(j) & aOP(aUDCTipos(i - 2) - 1)
            Else
                If (aUDCTipos(i - 1) = 2 And (aUDCTipos(i - 2) > 0)) Then
                    Cadenas(j) = Cadenas(j) & "Veinti"
                Else
                    Cadenas(j) = Cadenas(j) & aDEC(aUDCTipos(i - 1))
                    If (aUDCTipos(i - 2) > 0 And aUDCTipos(i - 1) > 0) Then Cadenas(j) = Cadenas(j) & " Y "
                End If

                If (StrComp(aUNI(aUDCTipos(i - 2) + 1), " Cero") <> 0) Then
                    If (i > 1 And aUDCTipos(i - 2) = 1) Then
                        Cadenas(j) = Cadenas(j) & "Un" & IIf(Not Monedas, "o", "")
                    Else
                        Cadenas(j) = Cadenas(j) & aUNI(aUDCTipos(i - 2) + 1)
                    End If
                    'Else
                    '    Cadenas(0) = "CERO "
                End If
            End If


            If (Len(LTrim(Cadenas(j))) > 1) Then Cadenas(j) = Cadenas(j) & aTIPO(i \ 3)
            If (aUDCTipos(i - 2) > 1 Or (aUDCTipos(i) > 0 Or aUDCTipos(i - 1) > 0)) Then
                If (i > 5) Then
                    Cadenas(j) = Cadenas(j) & "es "
                End If
            End If
            i = i - 3
        Next
        Cadenas(0) = Cadenas(0) & Cadenas(1)
        Cadenas(0) = Cadenas(0) & Cadenas(2)
        If Len(Cadenas(0)) >= 3 Then
            If Right(Trim(Cadenas(0)), 3) = "Uno" And Monedas Then
                Cadenas(0) = "Un"
            End If
        End If
        Dim sNumero As String
        If Monedas Then
            sNumero = IIf(Trim(Cadenas(0)) = "", "Cero", Trim(Cadenas(0))) & " Peso" & IIf(aux > 1 Or aux = 0, "s ", " ") & Format(lngCentavos, "00") & "/100 M.N."
        Else
            sNumero = Trim(Cadenas(0))
        End If
        Return IIf(Mayusculas, sNumero.ToUpper(), sNumero)

    End Function


    Public Function OC(ByVal Numero As Integer) As String
        Dim Residuo As Single, Rango As Integer
        Dim Enviar As String
        Rango = (Numero \ 26)
        Residuo = Numero - (Rango * 26)
        If Rango >= 1 Then
            Enviar = Chr(Rango + 64) & Chr(Residuo + 64)
            Return Enviar
        Else
            Enviar = Chr(Numero + 64)
            Return Enviar
        End If

    End Function

    Public Function GuardarArchivoenBD(ByVal SQL As String, ByVal Tabla As String, ByVal Campo As String, ByVal Archivo As String, Optional ByVal IdRecibo As String = "") As Boolean
        Try
Reintentar:
            GuardarArchivoenBD = False
            Dim dap As New SqlDataAdapter(SQL, CONEXION)
            Dim cbp As New SqlCommandBuilder(dap)
            Dim dsp As New DataSet
            dap.Fill(dsp, Tabla)
            If dsp.Tables(Tabla).Rows.Count > 0 Then
                Dim rwProyecto As DataRow = dsp.Tables(Tabla).Rows(0)
                With rwProyecto
                    .Item(Campo) = ReadBinaryFile(Archivo)
                End With
                dap.Update(dsp, Tabla)
                If Tabla = "Recibos" And Servidor.Central = False And IdRecibo <> "" Then
                    SQL = "UPDATE Recibos SET Folio=Folio"
                    nExecute(SQL, "Recibos", IdRecibo)
                End If
                GuardarArchivoenBD = True
            End If
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function

    Public Sub Parpadear(ByRef Forma As Windows.Forms.Form)
        Dim FlashInfo As FLASHWINFO
        With FlashInfo
            .cbSize = Convert.ToUInt32(Marshal.SizeOf(GetType(FLASHWINFO)))
            '.dwFlags = CType(FLASHW_ALL Or FLASHW_TIMER, Int32)
            .dwFlags = CType(FLASHW_ALL Or FLASHW_TIMERNOFG, Int32)
            .hwnd = Forma.Handle
            .dwTimeout = 0
            .uCount = 0
        End With
        FlashWindowEx(FlashInfo)
    End Sub

    Public Sub NormalizaXML(ByRef Cadena As String)
        Dim TextoNuevo As String, j As Integer

        TextoNuevo = ""
        For j = 0 To Cadena.Length - 1
            If Asc(Cadena.Chars(j)) <> 0 Then
                TextoNuevo &= Cadena.Chars(j).ToString
            End If
        Next
        Cadena = TextoNuevo
    End Sub

    Public Sub SetOptional(ByRef Barra As ToolStrip, ByVal selBoton As Object)
        Dim i As Integer, Boton As ToolStripButton
        Try
            For Each Boton In Barra.Items
                If Boton.Visible And TypeOf Boton Is ToolStripButton Then
                    Boton.Checked = (Boton Is selBoton)
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

    Public Sub nCargaCBO(ByRef Combo As Object, ByVal SQL As String, Optional ByVal DisplayM As String = "", Optional ByVal ValueM As String = "")
        'ValidaConexion()
        Dim dsDatos As New DataSet
        Try
reintentar:
            Combo.DataSource = Nothing
            Dim Comando As SqlDataAdapter
            Comando = New SqlDataAdapter(SQL, CONEXION)
            Comando.Fill(dsDatos)
            Comando = Nothing
            If dsDatos.Tables.Count > 0 And ValueM = "" Then
                Combo.ValueMember = dsDatos.Tables(0).Columns(0).ColumnName
                Combo.DisplayMember = dsDatos.Tables(0).Columns(1).ColumnName
            ElseIf ValueM <> "" Then
                Combo.ValueMember = ValueM
                Combo.DisplayMember = DisplayM
            End If

            Combo.DataSource = dsDatos.Tables(0)
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo reintentar
            Else
                MessageBox.Show(ex.Message, "Error al cargar el combo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Sub

    Public Function nConsulta(ByRef ds As DataSet, ByVal SQL As String, Optional ByVal Tabla As String = "Table", Optional ByVal Limpiar As Boolean = True) As DataRow()
        ' ValidaConexion()
        nConsulta = Nothing
        Try
Reintentar:
            Dim Comando As SqlDataAdapter
            Dim aSQL As String(), i As Integer

            aSQL = SQL.Split("|")

            If Limpiar Then
                ds = New DataSet
            End If

            For i = 0 To aSQL.GetUpperBound(0)
                Comando = New SqlDataAdapter(aSQL(i), CONEXION)
                Comando.Fill(ds, Tabla)
            Next
            nConsulta = Nothing
            If aSQL.GetUpperBound(0) = 0 Then
                If ds.Tables(Tabla) Is Nothing = False Then
                    nConsulta = ds.Tables(Tabla).Select("TRUE")
                End If
                If nConsulta.Length = 0 Then nConsulta = Nothing
            Else
                nConsulta = Nothing
            End If

        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Function

    



    Public Function nExecute(ByVal SQL As String, Optional ByVal Tabla As String = "", Optional ByVal sId As String = "") As Boolean
        Dim Sentencia As String
        Try
Reintentar:
            Dim comando As New SqlClient.SqlCommand

            Sentencia = SQL
            comando.Connection = CONEXION
            comando.CommandType = CommandType.Text
            comando.Transaction = Transaccion
            comando.CommandText = SQL
            comando.ExecuteNonQuery()

            nExecute = True
            If Servidor.Central = False And InStr(Sentencia, "INTO Bitacora ") = 0 And Tabla <> "" Then
                Try
                    Dim IdSQL As String, Id As Long, Accion As Integer

                    SQL = "SELECT MAX(RIGHT(IdSQL,10)) FROM Bitacora WHERE LEFT(IdSQL,2)='" & Strings.Right(Servidor.IdSeccion, 2) & "'"
                    Id = Val(ObtencampoDR(SQL, 0)) + 1
                    IdSQL = Strings.Right(Servidor.IdSeccion, 2) & Format(Id, StrDup(10, "0"))

                    If InStr(Mid(Sentencia, 1, 10), "INSERT ") > 0 Then
                        Accion = 1
                    End If
                    If InStr(Mid(Sentencia, 1, 10), "UPDATE ") > 0 Then
                        Accion = 2
                    End If
                    If InStr(Mid(Sentencia, 1, 10), "DELETE ") > 0 Then
                        Accion = 3
                    End If

                    If InStr(Sentencia, "GETDATE()") > 0 Then
                        Servidor.Actualizar()
                        Sentencia = Replace(Sentencia, "GETDATE()", "CAST('" & Servidor.Fecha_Hora & "' AS smalldatetime)", , , CompareMethod.Text)
                    End If

                    SQL = "INSERT INTO Bitacora VALUES('" & IdSQL & "','" & Tabla & "','" & sId & "'," & Accion & ",'" & Sentencia.Replace("'", "~") & "',1)"

                    nExecute(SQL)

                Catch ex As Exception
                End Try
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function


   

    Public Function Execute(ByVal SQL As String, ByRef IdGenerate As String, Optional ByVal Tabla As String = "", Optional ByVal sId As String = "") As Boolean
        Dim Sentencia As String
        Dim Aux As String
        Try
Reintentar:
            Aux = IdGenerate
            Dim comando As New SqlClient.SqlCommand

            Sentencia = SQL
            comando.Connection = CONEXION
            comando.CommandType = CommandType.Text
            comando.Transaction = Transaccion
            comando.CommandText = SQL
            IdGenerate = comando.ExecuteScalar
            If IdGenerate Is Nothing Then IdGenerate = Aux

            Execute = True

            If Servidor.Central = False And InStr(Sentencia, "INTO Bitacora ") = 0 And Tabla <> "" Then
                Try
                    Dim IdSQL As String, Id As Long, Accion As Integer
                    If sId = "" Then sId = IdGenerate

                    SQL = "SELECT MAX(RIGHT(IdSQL,10)) FROM Bitacora WHERE LEFT(IdSQL,2)='" & Strings.Right(Servidor.IdSeccion, 2) & "'"
                    Id = Val(ObtencampoDR(SQL, 0)) + 1
                    IdSQL = Strings.Right(Servidor.IdSeccion, 2) & Format(Id, StrDup(10, "0"))

                    If InStr(Sentencia, "INSERTAR ", CompareMethod.Text) > 0 Then
                        Accion = 1
                    End If
                    If InStr(Sentencia, "ACTUALIZAR ", CompareMethod.Text) > 0 Then
                        Accion = 2
                    End If
                    If InStr(Sentencia, "ELIMINAR ", CompareMethod.Text) > 0 Then
                        Accion = 3
                    End If

                    If InStr(Sentencia, "GETDATE()") > 0 Then
                        Servidor.Actualizar()
                        Sentencia = Replace(Sentencia, "GETDATE()", "CAST('" & Servidor.Fecha_Hora & "' AS smalldatetime)", , , CompareMethod.Text)
                    End If

                    SQL = "INSERT INTO Bitacora VALUES('" & IdSQL & "','" & Tabla & "','" & sId & "'," & Accion & ",'" & Sentencia.Replace("'", "~") & "',1)"

                    nExecute(SQL)

                Catch ex As Exception
                End Try
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Function

  

    Public Sub ValidaConexion()
        Dim Conectado As Boolean = False
Revalidar:
        If CONEXION.State = ConnectionState.Closed Then

            Do
                bValidando = True
                Try
                    If Servidor.Central Then
                        CONEXION.ConnectionString = "Server=" & Servidor.IP & ";uid=" & Servidor.User & ";Password=" & Servidor.PWD & ";DataBase=" & Servidor.Base
                    Else
                        CONEXION.ConnectionString = "Server=" & Servidor.sIP & ";uid=" & Servidor.sUser & ";Password=" & Servidor.sPWD & ";DataBase=" & Servidor.sBase
                    End If
                    CONEXION.Open()
                    Conectado = True
                Catch ex As Exception
                    Dim eror As Integer = Err.Number
                    If CONEXION.State = ConnectionState.Closed Then
                        If MessageBox.Show("No se ha podido establecer la conexión." & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & " ¿Desea intentarlo nuevamente?" & vbCrLf & vbCrLf & "Nota: Si cancela el sistema se cerrará.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
                            MessageBox.Show("No se pudo reestablecer la conexión, este problema puede ser ocasionado por problemas en la conexión de su red." & vbCrLf & vbCrLf & "El sistema se cerrará", "Error en la conexión", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End
                        End If
                    Else
                        Conectado = True
                    End If
                End Try
            Loop While CONEXION.State = ConnectionState.Closed Or Not Conectado
            bValidando = False
            MessageBox.Show("El sistema se ha logrado reconectar", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Threading.Thread.Sleep(2000)
        Else
            Try
                ObtencampoDR("SELECT GETDATE() ", 0)
            Catch ex As Exception
                If CONEXION.State = ConnectionState.Closed Then
                    GoTo Revalidar
                End If
            End Try
        End If
    End Sub

  

   

    Public Function nObtenCampo(ByVal SQL As String, ByVal IdCampo As Integer, Optional ByVal Cual As Integer = 0, Optional ByVal ConX As Integer = 0) As String
        'ValidaConexion()
        Dim dsDatos As New DataSet
        Try
Reintentar:
            nConsulta(dsDatos, SQL)
            nObtenCampo = ""
            If dsDatos.Tables.Count > 0 Then
                If Not dsDatos.Tables(0) Is Nothing Then
                    If dsDatos.Tables(0).Rows.Count > 0 Then
                        Select Case Cual
                            Case 0
                                nObtenCampo = dsDatos.Tables(0).Rows(0).Item(IdCampo).ToString
                            Case 1
                                nObtenCampo = dsDatos.Tables(0).Rows(dsDatos.Tables(0).Rows.Count - 1).Item(IdCampo).ToString
                            Case 2
                                nObtenCampo = dsDatos.Tables(0).Columns(IdCampo).ColumnName
                            Case 3
                                nObtenCampo = dsDatos.Tables(0).Rows.Count.ToString
                        End Select
                    Else
                        If Cual = 2 Then
                            nObtenCampo = dsDatos.Tables(0).Columns(IdCampo).ColumnName
                        ElseIf Cual = 3 Then
                            nObtenCampo = 0
                        Else
                            nObtenCampo = ""
                        End If
                    End If
                Else
                    nObtenCampo = ""
                End If
            End If
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error: " & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function

    Public Function nObtenCampo(ByVal SQL As String, ByVal CveCampo As String, Optional ByVal Cual As Integer = 0, Optional ByVal ConX As Integer = 0) As String
        'ValidaConexion()
        Dim dsDatos As New DataSet
        Try
Reintentar:
            nConsulta(dsDatos, SQL)
            nObtenCampo = ""
            If dsDatos.Tables.Count > 0 Then
                If Not dsDatos.Tables(0) Is Nothing Then
                    If dsDatos.Tables(0).Rows.Count > 0 Then
                        Select Case Cual
                            Case 0
                                nObtenCampo = dsDatos.Tables(0).Rows(0).Item(CveCampo).ToString
                            Case 1
                                nObtenCampo = dsDatos.Tables(0).Rows(dsDatos.Tables(0).Rows.Count - 1).Item(CveCampo).ToString
                            Case 2
                                nObtenCampo = dsDatos.Tables(0).Columns(CveCampo).ColumnName
                            Case 3
                                nObtenCampo = dsDatos.Tables(0).Rows.Count.ToString
                        End Select
                    Else
                        If Cual = 2 Then
                            nObtenCampo = dsDatos.Tables(0).Columns(CveCampo).ColumnName
                        ElseIf Cual = 3 Then
                            nObtenCampo = 0
                        Else
                            nObtenCampo = ""
                        End If
                    End If
                Else
                    nObtenCampo = ""
                End If
            End If
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error: " & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function


    Public Function fObtenerCampo(ByVal SQL As String) As String
        'ValidaConexion()
        Try
Reintentar:
            Dim comando As New SqlClient.SqlCommand
            comando.Connection = CONEXION
            comando.CommandType = CommandType.Text
            comando.Transaction = Transaccion
            comando.CommandText = SQL
            fObtenerCampo = comando.ExecuteScalar()
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error: " & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try
    End Function

    Public Sub nConsulta(ByRef ds As DataSet, ByVal SQL As String, ByVal NumCon As Integer)

        Try
Reintentar:
            Dim Comando As SqlDataAdapter
            Dim aSQL As String(), i As Integer, sSQL As String

            aSQL = SQL.Split("|")
            For i = ds.Tables.Count - 1 To 0 Step -1
                ds.Tables(i).Rows.Clear()
            Next

            For i = 1 To NumCon
                sSQL = "SELECT * FROM " & Split(aSQL(i - 1), " ")(0) & " " & aSQL(NumCon)
                Comando = New SqlDataAdapter(sSQL, CONEXION)
                Comando.Fill(ds, Split(aSQL(i - 1), " ")(0))
            Next
            ds.AcceptChanges()

        Catch ex As Exception

            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub


    Public Sub NoAutorizado()
        MessageBox.Show("Tiene permiso restringido a esta opción.", "Control de acceso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    Public Function BITWIN(ByVal Campo As String, ByVal Fecha1 As Date, ByVal Fecha2 As Date) As String
        BITWIN = "(" & Campo & " BETWEEN CONVERT(DATETIME, '" & Format(Fecha1, "yyyy/M/d") & " 00:00:00', 102) AND CONVERT(DATETIME, '" & Format(Fecha2, "yyyy/M/d") & " 23:59:59', 102) )"
    End Function

    Public Function ReadBinaryFile(ByVal fileName As String) As Byte()
        Dim nFileName As String
        Dim Ancho As Long, Alto As Long
        Dim Percent As Double
        ' Si no existe el archivo, abandono la función.
        '
        If Not System.IO.File.Exists(fileName) Then Return Nothing

        Try
            ' Creamos un objeto Stream para poder leer el archivo especificado.
            '
            Dim InfoFile As New IO.FileInfo(fileName)
            Select Case LCase(InfoFile.Extension)

                Case ".bmp", ".jpg", ".png", ".gif"

                    Dim Imagen As System.Drawing.Image
                    Imagen = System.Drawing.Image.FromFile(fileName)
                    nFileName = Path.GetTempFileName
                    Alto = Imagen.Height
                    Ancho = Imagen.Width
                    If Ancho < 770 Then
                        Alto = Alto * (770 / Ancho)
                        Ancho = 770
                        Imagen.GetThumbnailImage(Ancho, Alto, Nothing, Nothing).Save(nFileName, Imaging.ImageFormat.Jpeg)
                    ElseIf Alto < 950 Then
                        Ancho = Ancho * (950 / Alto)
                        Alto = 950
                        Imagen.GetThumbnailImage(Ancho, Alto, Nothing, Nothing).Save(nFileName, Imaging.ImageFormat.Jpeg)
                    ElseIf Ancho >= 1300 And Alto >= 1800 Then
                        Ancho = 1200
                        Alto = 1700
                        Imagen.GetThumbnailImage(Ancho, Alto, Nothing, Nothing).Save(nFileName, Imaging.ImageFormat.Jpeg)
                    Else
                        nFileName = fileName
                    End If
                Case Else
                    nFileName = fileName
            End Select

            Dim fs As New FileStream(nFileName, FileMode.Open, FileAccess.Read)

            ' Creamos un array de bytes, cuyo límite superior se corresponderá
            ' con la longitud en bytes de la secuencia.
            '
            Dim data() As Byte = New Byte(Convert.ToInt32(fs.Length)) {}

            ' Al leer la secuencia, se rellenará la matriz.
            '
            fs.Read(data, 0, Convert.ToInt32(fs.Length))

            ' Cerramos la secuencia.
            '
            fs.Close()

            ' Devolvemos el array de bytes.
            '
            Return data

        Catch ex As Exception
            ' Cualquier excepción producida, hace que la
            ' función devuelva el valor Nothing.
            '
            Return Nothing

        End Try

    End Function



    Public Function NewCrypt(ByVal sCadCrypt As String, Optional ByVal bInvert As Boolean = False) As String
        Dim i%
        Dim sCad$, sCad2$, nHex
        Try
            NewCrypt = "**********"
            If bInvert Then 'descencriptar

                sCad2 = Desencripta(sCadCrypt)
                sCad = ""
                For i = 1 To Len(sCad2) Step 2
                    nHex = "&H" & Mid(sCad2, i, 2)
                    sCad = sCad + Chr(nHex / 2)
                Next

                sCad2 = sCad
                sCad = ""
                For i = 1 To Len(sCad2) Step 2
                    nHex = "&H" & Mid(sCad2, i, 2)
                    sCad = sCad + Chr(nHex / 2)
                Next

            Else  'Encriptar
                sCad = ""
                For i = 1 To Len(sCadCrypt)
                    sCad = sCad + Hex(Asc(Mid(sCadCrypt, i, 1)) * 2)
                Next

                sCad2 = sCad
                sCad = ""
                For i = 1 To Len(sCad2)
                    sCad = sCad + Hex(Asc(Mid(sCad2, i, 1)) * 2)
                Next
                sCad = Encripta(sCad)
            End If
            NewCrypt = sCad
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Function Encripta(ByVal cad As String) As String
        Dim i%, Ds%

        Ds = 20
        If Trim(cad) <> "" Then
            For i = 1 To Len(cad)
                Encripta = Encripta + Chr(Ds + Asc(Mid(cad, i, 1)))
            Next
        End If
    End Function

    Function Desencripta(ByVal cad As String) As String
        Dim i%, Ds%

        Ds = 20
        If Trim(cad) <> "" Then
            For i = 1 To Len(cad)
                Desencripta = Desencripta + Chr(Asc(Mid(cad, i, 1)) - Ds)
            Next
        End If
    End Function


    Public Function FormatoSCK(ByVal Cadena As String, Optional ByVal Enviar As Boolean = True, Optional ByVal isHTML As Boolean = False) As String
        Dim Caracteres As String
        Dim aCaracteres As String()
        Caracteres = "á|a+|é|e+|í|i+|ó|o+|ú|u+|ñ|n+|ä|a-|ë|e-|ï|i-|ö|o-|ü|u-"
        If isHTML Then
            Caracteres = "á|a|é|e|í|i|ó|o|ú|u|ñ|n|ä|a|ë|e|ï|i|ö|o|ü|u"
        End If
        aCaracteres = Split(Caracteres, "|")
        If Enviar Then
            For i = 0 To aCaracteres.Length - 1 Step 2
                Cadena = Replace(Cadena, aCaracteres(i), aCaracteres(i + 1))
                Cadena = Replace(Cadena, UCase(aCaracteres(i)), UCase(aCaracteres(i + 1)))
            Next
        Else
            For i = 1 To aCaracteres.Length - 1 Step 2
                Cadena = Replace(Cadena, aCaracteres(i), aCaracteres(i - 1))
                Cadena = Replace(Cadena, UCase(aCaracteres(i)), UCase(aCaracteres(i - 1)))
            Next
        End If
        Return Cadena
    End Function

    Public Function WriteBinaryFile(ByVal aByte() As Byte, ByVal fileName As String) As Boolean

        ' El procedimiento creará un archivo con la secuencia de bytes
        ' especificada en el argumento.

        ' Compruebo los distintos parámetros pasados a la función.
        '
        If (aByte Is Nothing) OrElse (fileName = "") Then Return False

        Try
            ' Compruebo si existe el archivo especificado.
            '
            If System.IO.File.Exists(fileName) Then
                'If MessageBox.Show("Ya existe un archivo con el mismo nombre. " & _
                '"¿Desea sobrescribirlo?", _
                '"Grabar archivo", _
                'MessageBoxButtons.YesNo, _
                'MessageBoxIcon.Question) = _
                'Windows.Forms.DialogResult.No Then Return False

                ' Elimino el archivo
                System.IO.File.Delete(fileName)
            End If

            ' Número de bytes que se van a escribir
            Dim data As Int64 = aByte.Length

            ' Obtengo el nombre de un archivo temporal, donde
            ' primeramente se guardará el documento.
            '
            Dim tempFileName As String = System.IO.Path.GetTempFileName

            ' Abrimos o creamos el archivo.
            Dim fs As New FileStream(tempFileName, FileMode.OpenOrCreate)

            ' Crea el escritor para los datos.
            Dim bw As New BinaryWriter(fs)

            ' Escribimos en el archivo los datos realmente leídos.
            bw.Write(aByte, 0, Convert.ToInt32(data))

            ' Borra todos los búferes del sistema de escritura actual y hace
            ' que todos los datos almacenados en el búfer se escriban en el
            ' dispositivo subyacente. 
            bw.Flush()

            ' Cerramos los distintos objetos.
            bw.Close()
            fs.Close()
            bw = Nothing
            fs = Nothing

            ' Muevo el archivo a la ruta indicada.
            System.IO.File.Move(tempFileName, fileName)
            Return True
        Catch ex As Exception
            Return False
            MessageBox.Show(ex.Message, "Grabar archivo", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function

    Public Function nConsulta(ByVal SQL As String) As DataRow()



        Dim ds As DataSet
        Dim Tabla As String = "Table", Limpiar As Boolean = True
        nConsulta = Nothing
        Try
reintentar:
            Dim Comando As SqlDataAdapter

            Dim aSQL As String(), i As Integer

            aSQL = SQL.Split("|")

            If Limpiar Then
                ds = New DataSet
            End If

            'For i = 0 To aSQL.GetUpperBound(0)

            'Next
            Comando = New SqlDataAdapter(SQL, CONEXION)
            Comando.Fill(ds, Tabla)

            nConsulta = Nothing
            'If aSQL.GetUpperBound(0) = 0 Then
            If ds.Tables(0) Is Nothing = False Then
                nConsulta = ds.Tables(0).Select("true")
            End If
            If nConsulta.Length = 0 Then nConsulta = Nothing
            'Else
            '    nConsulta = Nothing
            'End If

        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Function

    
    Public Function TextoTOhtml(ByVal FolioAudiencia As String, Optional ByVal CrearHTML As Boolean = False) As String
        Dim Texto As String, rwDatos As DataRow(), SQL As String
        Dim FolioCausa As String, IdAdscripcion As Long
1:
        Texto = "<!DOCTYPE HTML PUBLIC -//W3C//DTD HTML 4.01 Transitional//EN  http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd>"
        Texto &= "<HTML><HEAD>"
        Texto &= "<TITLE>NOTIFICACIÓN DE AGENDACIÓN DE AUDIENCIA</TITLE></HEAD>"
        Texto &= "<BODY>"
        Texto &= "<font size=5 color=maroon><b>NOTIFICACIÓN DE AGENDACIÓN DE AUDIENCIA</b></font>"
        SQL = "SELECT SolicitudesAudiencias.FechaSolicitud, SolicitudesAudiencias.HoraSolicitud, Audiencias.Descripcion AS Audiencia, Causas.FolioCausa, Causas.CarpetaInv,"
        SQL &= "Personas_NombreC.NombreC AS Solicitante, Audiencias.Naturaleza, Audiencias.Tipo AS Prioridad, SolicitudesAudiencias.Observaciones,  SolicitudesAudiencias.IdAdscripcion"

        SQL &= "  FROM ((SolicitudesAudiencias INNER JOIN Audiencias ON (SolicitudesAudiencias.IdAudiencia=Audiencias.IdAudiencia))"
        SQL &= " INNER JOIN Causas ON (SolicitudesAudiencias.FolioCausa=Causas.FolioCausa))"
        SQL &= " INNER JOIN Personas_NombreC ON (SolicitudesAudiencias.IdSolicitante=Personas_NombreC.IdPersona)"
        SQL &= " WHERE FolioAudiencia='" & FolioAudiencia & "'"
        rwDatos = nConsulta(SQL)
        If rwDatos Is Nothing = False Then
            If rwDatos.Length > 0 Then
                IdAdscripcion = rwDatos(0).Item("IdAdscripcion")
                FolioCausa = rwDatos(0).Item("FolioCausa")

                Texto &= "<br/><br/>"
                Texto &= "Atendiendo su solicitud de audiencia realizada el día " & CDate(rwDatos(0).Item("FechaSolicitud")).ToLongDateString & ",</br>"
                Texto &= " através del Sistema de Gestion Judicial y Control Administrativo, se le notifica:"
                Texto &= "<br/><br/><b>"
                Texto &= "DATOS DE LA SOLICITUD</b><br/><br/>"
                Texto &= "<b>Folio:</b> " & FolioAudiencia & "<br/>"
                Texto &= "<b>Fecha y hora:</b> " & CDate(rwDatos(0).Item("FechaSolicitud")).ToShortDateString & " " & CDate(rwDatos(0).Item("HoraSolicitud")).ToShortTimeString & "<br/>"
                Texto &= "<b>Carpeta de investigación:</b> " & rwDatos(0).Item("CarpetaInv") & "<br/>"
                Texto &= "<b>Audiencia solicitada:</b> " & rwDatos(0).Item("Audiencia") & "<br/>"
                Texto &= "<font color=" & IIf(rwDatos(0).Item("Naturaleza") = "PRI", "red", "black") & ">"
                Texto &= "<b>Naturaleza:</b> " & IIf(rwDatos(0).Item("Naturaleza") = "PRI", "PRIVADA", "PÚBLICA") & "</font><br/>"
                Texto &= "<font color=" & IIf(rwDatos(0).Item("Prioridad") = "URG", "red", "black") & ">"
                Texto &= "<b>Prioridad:</b> " & IIf(rwDatos(0).Item("Prioridad") = "URG", "URGENTE", "PROGRAMADA") & "</font><br/>"
                Texto &= "<b>Solicitante:</b> " & rwDatos(0).Item("Solicitante") & "<br/>"
                If "" & rwDatos(0).Item("Observaciones") <> "" Then
                    Texto &= "<b>Observaciones:</b> " & "" & rwDatos(0).Item("Observaciones") & "<br/>"
                End If
                Texto &= "<br/><br/><b>"
                Texto &= "SUJETOS PROCESALES</b><br/><br/>IMPUTADOS<br/>"
                SQL = "SELECT  vRemotoImputados.Nombre, vRemotoImputados.Sexo,"
                SQL &= "Domicilios.Calle+' '+Domicilios.NumExt+' '+Domicilios.Colonia+' '+CASE WHEN Domicilios.Calle<>'' THEN vRemotoImputados.Poblacion+', '+vRemotoImputados.Municipio ELSE '-' END  AS Domicilio,"
                SQL &= " vRemotoImputados.Idioma, TelParticular+'. '+TelCelular+'. '+TelFax+'. '+TelOficina AS Telefonos,"
                SQL &= "ISNULL(Email,'-'), Detenido,vRemotoImputados.Defensor "
                SQL &= " FROM (((vRemotoImputados INNER JOIN Personas ON (vRemotoImputados.IdPersona=Personas.IdPersona AND IdAdscripcion=" & IdAdscripcion & "))"
                SQL &= " INNER JOIN Imputados ON (vRemotoImputados.IdPersona=Imputados.IdPersona AND FolioCausa='" & FolioCausa & "'))"
                SQL &= " LEFT JOIN DomiciliosPersona ON (vRemotoImputados.IdPersona=DomiciliosPersona.IdPersona))"
                SQL &= " LEFT JOIN Domicilios ON (DomiciliosPersona.IdDomicilio=Domicilios.IdDomicilio)"
                rwDatos = nConsulta(SQL)
                If rwDatos Is Nothing = False Then
                    Texto &= "<table cellspacing=2  cellpadding=2 border=1>"
                    Texto &= "<tr>"
                    Texto &= "<td><b>Nombre</td><td>Sexo</td><td>Dirección</td><td>Idioma</td><td>Teléfono</td><td>Email</td><td>Detenido</td><td>Defensor</d>"
                    Texto &= "</tr>"
                    ' aling=" & Chr(34) & "center" & Chr(34) & "
                    For i As Integer = 0 To rwDatos.Length - 1
                        Texto &= "<tr>"
                        For j As Integer = 0 To 7
                            Texto &= "<td>" & IIf("" & rwDatos(i).Item(j) = "", "-", "" & rwDatos(i).Item(j)) & "</td>"
                        Next
                        Texto &= "</tr>"
                    Next
                    Texto &= "</table>"
                    Texto &= "<br/>"
                End If

                Texto &= "VICTIMAS<br/>"
                SQL = "SELECT  vRemotoVictimas.Nombre, vRemotoVictimas.Sexo,"
                SQL &= "Domicilios.Calle+' '+Domicilios.NumExt+' '+Domicilios.Colonia+' '+CASE WHEN Domicilios.Calle<>'' THEN vRemotoVictimas.Poblacion+', '+vRemotoVictimas.Municipio ELSE '-' END  AS Domicilio,"
                SQL &= " vRemotoVictimas.Idioma, TelParticular+'. '+TelCelular+'. '+TelFax+'. '+TelOficina AS Telefonos,"
                SQL &= "ISNULL(Email,'-')"
                SQL &= " FROM (((vRemotoVictimas INNER JOIN Personas ON (vRemotoVictimas.IdPersona=Personas.IdPersona AND IdAdscripcion=" & IdAdscripcion & "))"
                SQL &= " INNER JOIN Victimas ON (vRemotoVictimas.IdPersona=Victimas.IdPersona AND FolioCausa='" & FolioCausa & "'))"
                SQL &= " LEFT JOIN DomiciliosPersona ON (vRemotoVictimas.IdPersona=DomiciliosPersona.IdPersona))"
                SQL &= " LEFT JOIN Domicilios ON (DomiciliosPersona.IdDomicilio=Domicilios.IdDomicilio)"
                rwDatos = nConsulta(SQL)
                If rwDatos Is Nothing = False Then
                    Texto &= "<table cellspacing=2  cellpadding=2 border=1>"
                    Texto &= "<tr><b>"
                    Texto &= "<td>Nombre</td><td>Sexo</td><td>Dirección</td><td>Idioma</td><td>Teléfono</td><td>Email</td></b>"
                    Texto &= "</tr>"
                    ' aling=" & Chr(34) & "center" & Chr(34) & "
                    For i As Integer = 0 To rwDatos.Length - 1
                        Texto &= "<tr>"
                        For j As Integer = 0 To 5
                            Texto &= "<td>" & IIf("" & rwDatos(i).Item(j) = "", "-", "" & rwDatos(i).Item(j)) & "</td>"
                        Next
                        Texto &= "</tr>"
                    Next

                    Texto &= "</table>"
                End If

                Texto &= "<br/>RELACIONES<br/>"
                SQL = "SELECT * FROM  vNotificacionRelaciones WHERE FolioAudiencia='" & FolioAudiencia & "'"
                rwDatos = nConsulta(SQL)
                If rwDatos Is Nothing = False Then
                    Texto &= "<table cellspacing=2  cellpadding=2 border=1>"
                    Texto &= "<tr><b>"
                    Texto &= "<td>Imputado</td><td>Delito</td><td>Víctima</td><td>Observaciones</td>"
                    Texto &= "</tr>"
                    ' aling=" & Chr(34) & "center" & Chr(34) & "
                    For i As Integer = 0 To rwDatos.Length - 1
                        Texto &= "<tr>"
                        For j As Integer = 1 To 4
                            Texto &= "<td>" & IIf("" & rwDatos(i).Item(j) = "", "-", "" & rwDatos(i).Item(j)) & "</td>"
                        Next
                        Texto &= "</tr>"
                    Next

                    Texto &= "</table>"
                End If
                SQL = "SELECT * FROM vNotificacion WHERE FolioAudiencia='" & FolioAudiencia & "'"
                If CrearHTML = False Or nObtenCampo(SQL, 0) <> "" Then
                    Texto &= "<br/><br/> DATOS AGENDADOS<br/>"
                    SQL = "SELECT * FROM vNotificacion WHERE FolioAudiencia='" & FolioAudiencia & "'"
                    rwDatos = nConsulta(SQL)
                    If rwDatos Is Nothing = False Then
                        If rwDatos.Length > 0 Then
                            Texto &= "<b>Número de causa:</b> " & rwDatos(0).Item("FolioCausa") & "<br/>"
                            Texto &= "<b>" & rwDatos(0).Item("Juzgado") & "</b><br/>"
                            Texto &= "<b>Sala de audiencia:</b> " & rwDatos(0).Item("Sala") & "<br/>"
                            Texto &= "<b>Fecha y hora de celebración:</b> " & CDate(rwDatos(0).Item("Fecha")).ToLongDateString & " a las " & CDate(rwDatos(0).Item("Fecha")).ToShortTimeString
                        End If
                    End If
                End If
            End If
        End If

        If CrearHTML = False Then
            Texto &= "<br/><br/>Confirme está notificación pulsando el siguiente botón. <br/><br/><input type=submit value=Confirmar> "
        Else
            Texto &= "<br/><br/>Nota: Version sin acentos por compatibilidad con diversos equipos <br/><br/>"
        End If
        Texto &= "</BODY></HTML>"
        'GoTo 1
        If CrearHTML Then
            Dim Archivo As StreamWriter
            Archivo = File.CreateText(Application.StartupPath & "\ArchivosXML\DetallesCausa.html")
            Archivo.Write(FormatoSCK(Texto.ToUpper, True, True))
            Archivo.Flush()
            Archivo.Close()
            Return "DetallesCausa"
        Else
            Return Texto
        End If

    End Function

    Public Function ConsultaDR(ByVal SQL As String) As SqlDataReader
        ValidaConexion()
        Try
reintentar:
            Dim oComando As New SqlCommand(SQL, CONEXION)
            oComando.Transaction = Transaccion
            Return oComando.ExecuteReader
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function

    Public Function ObtencampoDR(ByVal SQL As String, ByVal Indice As Integer) As String
        Try
Reintentar:
            Dim oComando As New SqlCommand(SQL, CONEXION)
            oComando.Transaction = Transaccion
            Dim drDato As SqlDataReader = oComando.ExecuteReader
            If drDato.Read Then
                ObtencampoDR = drDato.Item(Indice).ToString
            Else
                ObtencampoDR = ""
            End If
            drDato.Close()
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function


    


    Public Function nConsulta(ByVal SQL As String, ByVal Tipo As Integer) As SqlDataReader
        Try
Reintentar:
            Dim oComando As New SqlCommand(SQL, CONEXION)
            oComando.Transaction = Transaccion
            Dim drDato As SqlDataReader = oComando.ExecuteReader
            Return drDato
            'If drDato.Read Then
            '    ObtencampoDR = drDato.Item(Indice).ToString
            'Else
            '    ObtencampoDR = ""
            'End If
            'drDato.Close()
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function


    Public Function ObtencampoDR(ByVal SQL As String, ByVal Nombre As String) As String
        Try
Reintentar:
            Dim oComando As New SqlCommand(SQL, CONEXION)
            oComando.Transaction = Transaccion
            Dim drDato As SqlDataReader = oComando.ExecuteReader
            If drDato.Read Then
                ObtencampoDR = drDato.Item(Nombre).ToString
            Else
                ObtencampoDR = ""
            End If
            drDato.Close()
        Catch ex As Exception
            If CONEXION.State <> ConnectionState.Open Then
                ValidaConexion()
                GoTo Reintentar
            Else
                MessageBox.Show("Ha ocurrido un error:" & vbCrLf & ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End Try

    End Function


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
    End Sub

    Public Function Limpiar_Directorio(ByVal Directorio As String, Optional ByVal Subdirectorios As Boolean = False) As Boolean
        Dim Archivos() As String
        If Directory.Exists(Directorio) Then
            Archivos = Directory.GetFiles(Directorio)
            For i As Integer = 0 To Archivos.Length - 1
                Try
                    File.Delete(Archivos(i))
                Catch ex As Exception

                End Try
            Next
            'Borrar los archivos de los  subdirectorios
            If Subdirectorios Then
                Dim Directorios() As String = Directory.GetDirectories(Directorio)
                For i As Integer = 0 To Directorios.Length - 1
                    Limpiar_Directorio(Directorios(i))
                Next
            Else
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub ImprimeLineasPD(ByRef gfx As System.Drawing.Printing.PrintPageEventArgs, ByVal Cadena As String, ByRef gTop As Long, ByVal Fuente As System.Drawing.Font, Optional ByVal nC As Integer = 50, Optional ByVal iLeft As Integer = 0)
        Dim aDatos() As String = PartirCadena(Cadena, True, nC)

        For i = 0 To aDatos.Length - 1
            If aDatos(i) Is Nothing = False Then
                gfx.Graphics.DrawString(aDatos(i), Fuente, Brushes.Black, iLeft, gTop)
                gTop += 10
            End If
        Next
    End Sub

    Public Function PartirCadena(ByVal Cadena As String, Optional ByVal Longitud As Integer = 110) As String()
        Dim devolver As String(), Index As Integer = 0
        If Cadena.Length <= Longitud Then
            ReDim devolver(1)
            devolver(0) = Cadena
        Else
            Do
                ReDim Preserve devolver(Index)
                devolver(Index) &= Mid(Cadena, 1, Longitud)
                If Cadena.Length > Longitud Then
                    Cadena = Mid(Cadena, Longitud + 1)
                End If
                Index += 1
            Loop While Cadena.Length > Longitud
            If Cadena.Length > 0 Then
                ReDim Preserve devolver(Index)
                devolver(Index) &= Cadena
            End If
        End If
        Return devolver
    End Function

    Public Function PartirCadena(ByVal Cadena As String, ByVal Espacios As Boolean, Optional ByVal Longitud As Integer = 110) As String()
        Dim devolver As String(), Index As Integer = 0
        Dim pos As Integer, res As String
        Dim cFin As Char, cIni As Char

        If Not Espacios Then
            Return PartirCadena(Cadena, Longitud)
        End If

        If Cadena.Length <= Longitud Then
            ReDim devolver(1)
            devolver(0) = Cadena
        Else
            Do
                ReDim Preserve devolver(Index) 'generamos el arreglo de cadenas para ir metiendo las lineas

                devolver(Index) = Mid(Cadena, 1, Longitud)
                cFin = Mid(Cadena, Longitud, 1)
                cIni = Mid(Cadena, Longitud + 1, 1)
                res = ""
                If cFin <> " " And cIni <> " " Then 'Si los extremos del corte no son espaciones
                    pos = InStrRev(devolver(Index), " ")
                    res = Mid(devolver(Index), pos + 1)
                    devolver(Index) = Mid(devolver(Index), 1, pos)
                End If

                If (res.Length + Cadena.Length) > Longitud Then
                    Cadena = res & Mid(Cadena, Longitud + 1)
                End If
                Index += 1
            Loop While Cadena.Length > Longitud

            If Cadena.Length > 0 Then
                ReDim Preserve devolver(Index)
                devolver(Index) = Cadena
            End If
        End If
        Return devolver
    End Function

    Public Function aRight(ByVal Cadena As String, Optional ByVal nCar As Integer = 10)
        If nCar - Cadena.Length > 0 Then
            aRight = StrDup(nCar - Cadena.Length, " ") & Cadena
        Else
            aRight = Cadena
        End If
    End Function

End Module
