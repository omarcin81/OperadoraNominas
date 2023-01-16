Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Module mdoInicio
    Public WithEvents CONEXION As New SqlConnection
    
    Public bValidando As Boolean = False
    Public bValidandoContpaq As Boolean = False
    Public bValidandoKiosko As Boolean = False

    Public WithEvents Transaccion As SqlClient.SqlTransaction


    'Public FormInicial As frmPrincipal
    '   Public frmInicial As frmInicial

    Public Usuario As New clsUsuario
    Public Juzgado As New clsJuzgado
    Public SoloNumero As New cSolonumeros
    Public Maestro As New clsMaestro

    Public gControlEscolar As Boolean
    Public gCaja As Boolean
    Public gBecas As Boolean


    Public Sub Main()
        If File.Exists(Application.StartupPath & "\Actualizacion.rar") Then
            File.Delete(Application.StartupPath & "\Actualizacion.rar")
        End If
        Servidor.Remoto = False
        Configurar()
        Conectar()
        'ConectarContpaq()

    End Sub

    Public Function Conectar() As Boolean
        Try
            Servidor.Conectado = False

            CONEXION.ConnectionString = "Server=" & Servidor.IP & ";uid=" & Servidor.User & ";Password=" & Servidor.PWD & ";DataBase=" & Servidor.Base

            CONEXION.Open()

            Servidor.Conectado = True
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Exclamation, "Error al conectar a la base de datos")
            'Dim Forma As New frmConexionL
            'Forma.ShowDialog()
            'If Servidor.Conectado = False Then
            '    End
            'End If
        End Try
    End Function

    



    Public Sub Configurar(Optional ByVal Guardar As Boolean = False)
        Dim strInicio As String, sCLocal() As String, cLocal As String
        Dim Ruta As String, strPWD As String


        Ruta = Application.StartupPath & "\Config.cfg"
        If File.Exists(Ruta) And Not Guardar Then
            Dim strConfig As StreamReader
            strConfig = File.OpenText(Ruta)
            cLocal = ""
            Servidor.IP = strConfig.ReadLine()
            Servidor.Nombre = strConfig.ReadLine()
            Servidor.Base = strConfig.ReadLine()
            Servidor.User = strConfig.ReadLine()
            strPWD = strConfig.ReadLine()
            Servidor.PWD = NewCrypt(strPWD, True)
            

            strConfig.Close()

            Configurar(True)
        Else
            Dim strConfig As StreamWriter
            If File.Exists(Ruta) Then
                File.Delete(Ruta)
            End If

            strConfig = File.CreateText(Ruta)
            If Not Guardar Then
                'Servidor.IP = "facturacion.elchingon.net"
                'Servidor.IP = "192.168.1.222"
                Servidor.IP = "localhost"
                Servidor.Nombre = "Equipo1\sqlexpress"
                Servidor.Base = "OperadoraNominas"
                'Servidor.Base = "NavigatorNominas"
                Servidor.User = "sa"
                Servidor.PWD = "12345"
                'Servidor.PWD = "kiosko2016"

            End If
            strConfig.WriteLine(Servidor.IP)
            strConfig.WriteLine(Servidor.Nombre)
            strConfig.WriteLine(Servidor.Base)
            strConfig.WriteLine(Servidor.User)
            strConfig.WriteLine(NewCrypt(Servidor.PWD))
            strConfig.Close()
        End If
    End Sub

    Private Sub CONEXION_StateChange(ByVal sender As Object, ByVal e As System.Data.StateChangeEventArgs) Handles CONEXION.StateChange

        'If Not bValidando Then
        '    ValidaConexion()
        'End If
    End Sub
End Module
