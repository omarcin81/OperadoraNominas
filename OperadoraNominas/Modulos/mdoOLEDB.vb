Imports System.Data
Imports System.Data.OleDb


Module mdoOLEDB
    Public CONDB As New OleDbConnection
    Public TransacODB As OleDbTransaction

    Public Sub nCargaCBOAC(ByRef Combo As Object, ByVal SQL As String, Optional ByVal DisplayM As String = "", Optional ByVal ValueM As String = "")
        Dim dsDatos As New DataSet

        'Dim Comando As SqlDataAdapter
        Dim Comando As OleDb.OleDbDataAdapter

        Comando = New OleDbDataAdapter(SQL, CONDB.ConnectionString)

        Comando.Fill(dsDatos)
        Comando = Nothing
        Combo.ValueMember = IIf(ValueM = "", Combo.ValueMember, ValueM)
        Combo.DisplayMember = IIf(DisplayM = "", Combo.DisplayMember, DisplayM)
        Combo.DataSource = dsDatos.Tables("Table")
    End Sub

    Public Function nConsultaAC(ByRef ds As DataSet, ByVal SQL As String, Optional ByVal Tabla As String = "Table", Optional ByVal Limpiar As Boolean = True) As DataRow()
        nConsultaAC = Nothing
        Try
            'Dim Comando As SqlDataAdapter
            Dim Comando As OleDb.OleDbDataAdapter

            Dim aSQL As String(), i As Integer

            aSQL = SQL.Split("|")

            If Limpiar Then
                ds = New DataSet
            End If

            For i = 0 To aSQL.GetUpperBound(0)
                'Comando = New SqlDataAdapter(aSQL(i), CONEXION.ConnectionString)
                Comando = New OleDb.OleDbDataAdapter(aSQL(i), CONDB)
                Comando.Fill(ds, Tabla)
            Next
            nConsultaAC = Nothing
            If aSQL.GetUpperBound(0) = 0 Then
                If ds.Tables(0) Is Nothing = False Then
                    nConsultaAC = ds.Tables(0).Select("TRUE")
                End If
                If nConsultaAC.Length = 0 Then nConsultaAC = Nothing
            Else
                nConsultaAC = Nothing
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Comando()
        End Try

    End Function

    Public Function nConsultaAC(ByVal SQL As String) As DataRow()
        Dim ds As DataSet, Tabla As String = "Table"
        nConsultaAC = Nothing
        Try
            'Dim Comando As SqlDataAdapter
            Dim Comando As OleDb.OleDbDataAdapter
            ds = New DataSet

            Comando = New OleDbDataAdapter(SQL, CONDB)
            Comando.Fill(ds, Tabla)

            nConsultaAC = Nothing
            If SQL <> "" Then
                If ds.Tables(0) Is Nothing = False Then
                    nConsultaAC = ds.Tables(0).Select("TRUE")
                End If
                If nConsultaAC.Length = 0 Then nConsultaAC = Nothing
            Else
                nConsultaAC = Nothing
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function

    Public Function nExecuteAC(ByVal SQL As String) As Boolean
        Try
            Dim comando As New OleDbCommand
            If CONDB.State = ConnectionState.Closed Then
                CONDB.Open()
            End If
            comando.Connection = CONDB
            comando.CommandType = CommandType.Text
            comando.Transaction = TransacODB

            '       If Not vInsertar Or vSubString = "" Then
            comando.CommandText = SQL
            '       End If
            comando.ExecuteNonQuery()
            nExecuteAC = True
        Catch ex As Exception
            nExecuteAC = False
            MessageBox.Show(ex.Message & vbCrLf & SQL, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Function nObtenCampoAC(ByVal SQL As String, ByVal IdCampo As Integer, Optional ByVal Cual As Integer = 0) As String
        Dim dsDatos As New DataSet
        nConsulta(dsDatos, SQL)
        nObtenCampoAC = ""
        If Not dsDatos.Tables(0) Is Nothing Then
            If dsDatos.Tables(0).Rows.Count > 0 Then
                Select Case Cual
                    Case 0
                        nObtenCampoAC = dsDatos.Tables(0).Rows(0).Item(IdCampo).ToString
                    Case 1
                        nObtenCampoAC = dsDatos.Tables(0).Rows(dsDatos.Tables(0).Rows.Count - 1).Item(IdCampo).ToString
                    Case 2
                        nObtenCampoAC = dsDatos.Tables(0).Columns(IdCampo).ColumnName
                    Case 3
                        nObtenCampoAC = dsDatos.Tables(0).Rows.Count.ToString
                End Select
            Else
                If Cual = 2 Then
                    nObtenCampoAC = dsDatos.Tables(0).Columns(IdCampo).ColumnName
                ElseIf Cual = 3 Then
                    nObtenCampoAC = 0
                Else
                    nObtenCampoAC = ""
                End If
            End If
        Else
            nObtenCampoAC = ""
        End If
    End Function

    Public Class clsConfiguracion
        Public Shared Tickets As String
        Public Shared Doctos As String
        Public Shared EmailFac As String
        Public Shared PwdMail As String
        Public Shared Encabezado As String

        Public Shared Sub Actualizar()
            Dim SQL As String
            SQL = "SELECT * FROM Configuracion"
            Dim CFG() As DataRow = nConsulta(SQL)

            If CFG Is Nothing = False Then
                Tickets = "" & CFG(0)!Tickets
                Doctos = "" & CFG(0)!Doctos
                EmailFac = "" & CFG(0)!EmailFac
                PwdMail = "" & CFG(0)!PwdMail
                Encabezado = "" & CFG(0)!Encabezado
            End If
        End Sub

    End Class
End Module
