'------------------------------------------------------------------------------
' Clase con las funciones del API de Windows                        (14/Ago/05)
' Las funciones están declaradas como compartidas
' para usarlas sin crear una instancia.
'
' ©Guillermo 'guille' Som, 2005
'------------------------------------------------------------------------------
Imports Microsoft.VisualBasic
Imports System

Public Class WinApi
    ' Hace que una ventana sea hija (o esté contenida) en otra
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
    End Function
    ' Devuelve el Handle (hWnd) de una ventana de la que sabemos el título
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function
    ' Cambia el tamaño y la posición de una ventana
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, _
                                ByVal nWidth As Integer, ByVal nHeight As Integer, _
                                ByVal bRepaint As Integer) As Integer
    End Function


End Class
