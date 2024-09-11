Public Class SeleccionarPeriodoAutom
    Public gInicial As Integer
    Public gSerie As Integer

    Private Sub SeleccionarPeriodoAutom_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try

            cargarperiodosinicial()

            cbInicial.SelectedIndex = gInicial
            'cboserie.SelectedIndex = gSerie
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cargarperiodosinicial()
        'Verificar si se tienen permisos
        Dim sql As String
        Try
            sql = "Select dFechaFin, iIdPeriodo, iSeptimos from periodos where iEstatus=1 order by iEjercicio, iNumeroPeriodo"
            nCargaCBO(cbInicial, sql, "dFechaFin", "iIdPeriodo")

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    'Private Sub cargarperiodos()
    'Verificar si se tienen permisos
    'Dim sql As String
    'Try

    'sql = "Select dFechaInicio, iIdPeriodo  from periodos where iEstatus=1 order by iEjercicio, iNumeroPeriodo"
    'nCargaCBO(cbPeriodo, sql, "dFechainicio", "iIdPeriodo")

    'Catch ex As Exception
    'MessageBox.Show(ex.Message)
    'End Try

    'End Sub

    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            gInicial = cbInicial.SelectedValue
            Me.DialogResult = Windows.Forms.DialogResult.OK
            'Me.Close()

        Catch ex As Exception

        End Try
    End Sub

End Class