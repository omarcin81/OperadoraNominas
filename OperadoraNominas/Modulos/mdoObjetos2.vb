Public Class mdoObjetos2
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
        mdoObjetos2.Fecha = Format(ObtencampoDR(SQL, 0), "Short Date")
        mdoObjetos2.Hora = Format(CDate(ObtencampoDR(SQL, 0)), "HH:mm:ss")
        mdoObjetos2.Fecha_Hora = mdoObjetos2.Fecha & " " & mdoObjetos2.Hora
    End Sub

    Public Shared Sub IniciaTransaccion()
        ValidaConexion()
        Transaccion = CONEXION.BeginTransaction
    End Sub
End Class
