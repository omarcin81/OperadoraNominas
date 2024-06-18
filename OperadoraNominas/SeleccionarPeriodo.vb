Public Class SeleccionarPeriodo
    Public gInicial As Integer
    Public gFinal As Integer
    Public gSerie As Integer

    Private Sub SeleccionarPeriodo_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try


            cargarperiodosinicial()
            cargarperiodosfinal()

            cbInicial.SelectedIndex = gInicial
            cbFinal.SelectedIndex = gFinal
            cboserie.SelectedIndex = gSerie
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cargarperiodosinicial()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select (CONVERT(nvarchar(12),dFechaInicio,103) + ' - ' + CONVERT(nvarchar(12),dFechaFin,103)) as dFechaInicio,iIdPeriodo  from periodos where iEstatus=1 order by iEjercicio,iNumeroPeriodo"
            nCargaCBO(cbInicial, sql, "dFechainicio", "iIdPeriodo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub cargarperiodosfinal()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select (CONVERT(nvarchar(12),dFechaInicio,103) + ' - ' + CONVERT(nvarchar(12),dFechaFin,103)) as dFechaInicio,iIdPeriodo  from periodos where iEstatus=1 order by iEjercicio,iNumeroPeriodo"
            nCargaCBO(cbFinal, sql, "dFechainicio", "iIdPeriodo")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            gInicial = cbInicial.SelectedIndex + 1
            gFinal = cbFinal.SelectedIndex + 1
            gSerie = cboserie.SelectedIndex
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception

        End Try
    End Sub


End Class