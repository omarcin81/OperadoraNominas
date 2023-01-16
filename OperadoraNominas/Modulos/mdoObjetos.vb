Public Class Servidor
    Public Shared IP As String
    Public Shared Nombre As String
    Public Shared User As String
    Public Shared PWD As String
    Public Shared Base As String
    Public Shared Fecha As Date
    Public Shared Hora As String
    Public Shared Fecha_Hora As String
    Public Shared Conectado As Boolean
    Public Shared IdJuzgado As String
    Public Shared fIdJuzgado As String
    Public Shared TAG As String
    Public Shared Remoto As Boolean
    Public Shared IdDistrito As Long
    Public Shared FTP As String
    Public Shared IPSocket As String
    Public Shared PortSocket As String
    Public Shared WebService As String
    Public Shared IdSeccion As String
    Public Shared PrinterDoc As String
    Public Shared PrinterTicket As String
    Public Shared Central As Boolean
    Public Shared sIP As String
    Public Shared sUser As String
    Public Shared sPWD As String
    Public Shared sBase As String
    Public Shared gIdSec As String
    Public Shared gImportacion As String
    Public Shared gImpUsuario As String
    Public Shared gImpClave As String
    Public Shared recBorde As Long
    Public Shared FacPruebas As Boolean


    Public Shared Sub Actualizar()
        Dim SQL As String
        SQL = "SELECT GETDATE()"
        Servidor.Fecha = Format(ObtencampoDR(SQL, 0), "Short Date")
        Servidor.Hora = Format(CDate(ObtencampoDR(SQL, 0)), "HH:mm:ss")
        Servidor.Fecha_Hora = Servidor.Fecha & " " & Servidor.Hora
    End Sub

    Public Shared Sub IniciaTransaccion()
        ValidaConexion()
        Transaccion = CONEXION.BeginTransaction
    End Sub

End Class

Public Class ListItem
    Private _Text As String
    Private _Value As String

    Public Sub New(ByVal text As String, ByVal value As String)
        _Text = text
        _Value = value
    End Sub


    Public ReadOnly Property Text As String
        Get
            Return _Text
        End Get

    End Property

    Public ReadOnly Property Value As String
        Get
            Return _Value
        End Get

    End Property

    Public Overrides Function ToString() As String
        Return _Text
    End Function

End Class

Public Class clsMaestro
    Public IdMaestro As String
    Public Nombre As String

End Class


Public Class clsDigitaliza
    Public Id As String
    Public Ruta As String
    Public Guardado As Boolean 
End Class

Public Class clsUsuario
    Public Nombre As String
    Public Id As String
    Public Perfil As Integer
    Public IdCaja As String
    Public IdSeccion As String

    Public Function fnPermiso(ByVal Forma As Windows.Forms.Form, ByVal Tipo_AIME As String) As Boolean
        'Dim SQL As String, Campo As Integer
        'SQL = "SELECT Permisos.* FROM Permisos INNER JOIN Pantallas ON (Permisos.IdPantalla=Pantallas.IdPantalla)"
        'SQL = SQL & " WHERE Pantalla='" & Forma.Name & "' AND IdAdscripcion=" & Servidor.IdJuzgado
        'SQL = SQL & " AND IdUsuario='" & Id & "'"
        'Select Case Tipo_AIME.ToUpper
        '    Case "A" : Campo = 3
        '    Case "I" : Campo = 4
        '    Case "M" : Campo = 5
        '    Case "E" : Campo = 6
        'End Select
        'fnPermiso = Val(nObtenCampo(SQL, Campo)) <> 0 Or Usuario.Puesto = 6
        'If Not fnPermiso Then
        '    MessageBox.Show("Usted no tiene autorizado el uso o ejecución de esta función." & vbCrLf & "Consulte con el administrador del sistema.", "Control de acceso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Function
    Public Function fnPermiso(ByVal Nombre_Forma As String, ByVal Tipo_AIME As String, Optional ByVal Mensaje As Boolean = True) As Boolean
        'Dim SQL As String, Campo As Integer
        'SQL = "SELECT Permisos.* FROM Permisos INNER JOIN Pantallas ON (Permisos.IdPantalla=Pantallas.IdPantalla)"
        'SQL = SQL & " WHERE Pantalla='" & Nombre_Forma & "' AND IdAdscripcion=" & Servidor.IdJuzgado
        'SQL = SQL & " AND IdUsuario='" & Id & "'"
        'Select Case Tipo_AIME.ToUpper
        '    Case "A" : Campo = 3
        '    Case "I" : Campo = 4
        '    Case "M" : Campo = 5
        '    Case "E" : Campo = 6
        'End Select
        'fnPermiso = Val(nObtenCampo(SQL, Campo)) <> 0 Or Usuario.Puesto = 6
        'If Not fnPermiso And Mensaje Then
        '    MessageBox.Show("Usted no tiene autorizado el uso o ejecución de esta función." & vbCrLf & "Consulte con el administrador del sistema.", "Control de acceso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Function

End Class


Public Class clsJuzgado
    Public Ubicacion As String
    Public Direccion As String
    Public CtrlPuntos As Boolean
    Public ImpresoraDoctos As String
    Public ImpresoraCertifica As String
    Public Nombre As String
End Class

Public Class cSolonumeros
    Function NumeroDec(ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal Text As TextBox) As Integer
        Dim dig As Integer = Len(Text.Text & e.KeyChar)
        Dim a, esDecimal, NumDecimales As Integer
        Dim esDec As Boolean
        ' se verifica si es un digito o un punto para el decimal 
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Then
            e.Handled = False
        ElseIf Char.IsControl(e.KeyChar) Then
            e.Handled = False
            Return a
        Else
            e.Handled = True
        End If
        ' se verifica que el primer digito ingresado no sea un punto al seleccionar 
        If Text.SelectedText <> "" Then
            If e.KeyChar = "." Then
                e.Handled = True
                Return a
            End If
        End If

        If dig = 1 And e.KeyChar = "." Then
            e.Handled = True
            Return a
        End If
        ' aqui se hace la verificacion cuando es seleccionado el valor del texto 
        'y no sea considerado como la adicion de un digito mas al valor ya contenido en el textbox
        If Text.SelectedText = "" Then
            ' aqui se hace el for para controlar que el numero sea de dos digitos - contadose a partir del punto decimal.
            For a = 0 To dig - 1
                Dim car As String = CStr(Text.Text & e.KeyChar)
                If car.Substring(a, 1) = "." Then
                    esDecimal = esDecimal + 1
                    esDec = True
                End If
                If esDec = True Then
                    NumDecimales = NumDecimales + 1
                End If
                ' aqui se controla los digitos a partir del punto numdecimales = 4 si es de dos decimales  
                If NumDecimales >= 4 Or esDecimal >= 2 Then
                    e.Handled = True
                End If
            Next
        End If
    End Function
End Class

Public Class clsPAC
    Public Shared RFC As String
    Public Shared Usuario As String
    Public Shared Password As String
    Public Shared URL As String
    Public Shared PrivateKey As String
    Public Shared RFCEmisor As String


    

    Public Shared Sub Configurar()
        Dim SQL As String

        SQL = "SELECT * FROM CFDI"
        Dim CFG() As DataRow = nConsulta(SQL)

        If CFG Is Nothing = False Then
            RFC = CFG(0)!RFC
            Usuario = NewCrypt(CFG(0)!UserFEL, True)
            Password = NewCrypt(CFG(0)!PassFEL, True)
            URL = CFG(0)!URLWS
        End If

    End Sub
End Class