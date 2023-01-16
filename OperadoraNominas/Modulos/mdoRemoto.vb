Module mdoRemoto
    Public Sub CargaCBO(ByVal CboDatos As Object, ByVal Tabla As String, Optional ByVal vBandera As Boolean = False, Optional ByVal SubTabla As String = "X")

        Dim dsCat As New DataSet, nID As String, nDescrip As String
        Dim dtbTabla As DataTable, nombreOBJ As String, rwFilas() As DataRow
        '// Leemos del xml

        Try
            dsCat.ReadXml(Application.StartupPath & "\ArchivosXML\" & Tabla & ".xml")
            If SubTabla <> "X" Then Tabla = SubTabla
            dtbTabla = dsCat.Tables(Tabla)
            If Not dtbTabla Is Nothing Then
                nID = dtbTabla.Columns(0).Caption ' Obtener el nombre de la primera conlumna de la tabla
                nDescrip = dtbTabla.Columns(1).Caption 'Obtener el nombre de la segunda columna de la tabla
            End If

            'cboDatos = ObtenControl(objForm, nCombo) ' buscamos en el control a enlazar en la coleccion de controles del form
            If Not CboDatos Is Nothing Then
                CboDatos.DataSource = Nothing
                CboDatos.items.clear()
                CboDatos.ValueMember = nID
                CboDatos.DisplayMember = nDescrip
                CboDatos.DataSource = dsCat.Tables(Tabla)
                CboDatos.SelectedValue = ""
            End If

            If vBandera = True Then
                For vX As Integer = 0 To dtbTabla.Rows.Count - 1
                    CboDatos.SetItemChecked(vX, dtbTabla.Rows(vX).Item("Status"))
                Next
            End If

        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub CargaCBO(ByVal CboDatos As Object, ByVal Tabla As String, ByVal sID As String, ByVal sDescrip As String, Optional ByVal SubTabla As String = "X", Optional ByVal Solo As Integer = -1, Optional ByVal Condicion As String = "TRUE")

        Dim dsCat As New DataSet, nID As String, nDescrip As String, i As Long
        Dim dtbTabla As DataTable, nombreOBJ As String, rwFilas() As DataRow
        '// Leemos del xml

        Try
            If Tabla = "Esquema" Then dsCat.ReadXmlSchema(Application.StartupPath & "\ArchivosXML\schAudiencias.xml")
            dsCat.ReadXml(Application.StartupPath & "\ArchivosXML\" & Tabla & ".xml")
            If SubTabla <> "X" Then Tabla = SubTabla



            dtbTabla = dsCat.Tables(Tabla)
            If Not dtbTabla Is Nothing Then
                If Condicion <> "TRUE" Then
                    For i = dsCat.Tables(Tabla).Rows.Count - 1 To 0 Step -1
                        If dsCat.Tables(Tabla).Rows(i).Item(Solo) <> Condicion Then
                            dsCat.Tables(Tabla).Rows.RemoveAt(i)
                        End If
                    Next
                End If
                nID = sID
                nDescrip = sDescrip
            End If
          
            dsCat.AcceptChanges()
            'cboDatos = ObtenControl(objForm, nCombo) ' buscamos en el control a enlazar en la coleccion de controles del form
            If Not CboDatos Is Nothing Then
                CboDatos.DataSource = Nothing
                CboDatos.ValueMember = nID
                CboDatos.DisplayMember = nDescrip
                CboDatos.DataSource = dsCat.Tables(Tabla)
                'CboDatos.SelectedValue = ""
            End If


        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function Obtencampo(ByVal Tabla As String, ByVal Condicion As String, ByVal Campo As String) As String
        Dim dsCat As New DataSet, rwFila As DataRow()
        dsCat.ReadXml(Application.StartupPath & "\Flash\" & Tabla & ".xml")
        rwFila = dsCat.Tables(Tabla).Select(Condicion)
        If rwFila Is Nothing = False Then
            If rwFila.Length > 0 Then
                Obtencampo = "" & rwFila(0).Item(Campo)
            Else
                Obtencampo = ""
            End If
        Else
            Obtencampo = ""
        End If
    End Function
End Module
