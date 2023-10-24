Imports ClosedXML.Excel
Imports System.IO
Imports System.Xml

Public Class frmEligueMes
    Public gMes As Integer
    Public gAnio As String

    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click
        Try

        Catch ex As Exception

        End Try

        Dim Mes As Integer = cboMes.SelectedIndex
        Dim Anio As String = cboAnio.SelectedItem

        gMes = Mes
        gAnio = Anio
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        

    End Sub
End Class