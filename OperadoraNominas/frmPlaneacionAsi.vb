Imports ClosedXML.Excel

Public Class frmPlaneacionAsi

    Private Sub cmdcalcular_Click(sender As System.Object, e As System.EventArgs) Handles cmdcalcular.Click
        Dim propuesta As Double
        Dim bruto As Double
        Dim excendente As Double
        Dim isr As Double
        Dim perpecionneta As Double
        Dim impuestonomina As Double
        Dim comision As Double
        Dim diferencia As Double
        Dim calculado As Double
        Dim Alter As Boolean = False
        Dim SQL As String
        Try
            lsvLista.Items.Clear()
            lsvLista.Clear()


            SQL = "select cCodigoEmpleado,cNombreLargo,cRFC,cCURP,[fSalarioBase],[fSueldoBruto],[fTExtraFijoGravado],[fTExtraFijoExento],[fTExtraOcasional]"
            SQL &= " ,[fDescSemObligatorio],[fVacacionesProporcionales],[fAguinaldoGravado],[fAguinaldoExento],[fPrimaVacacionalGravado],[fPrimaVacacionalExento],[fTotalPercepciones]"
            SQL &= " ,[fTotalPercepcionesISR],[fIncapacidad],[fIsr],[fImss],[fInfonavit],[fInfonavitBanterior],[fAjusteInfonavit],[fPensionAlimenticia],[fPrestamo]"
            SQL &= " ,[fFonacot],[fSubsidioGenerado],[fSubsidioAplicado],[fOperadora],[fPrestamoPerA],[fAdeudoInfonavitA],[fDiferenciaInfonavitA],[fAsimilados],"
            'SQL &= " [fTotalPercepciones] - fIsr +fPrestamoPerA  + fAdeudoInfonavitA +fDiferenciaInfonavitA +fAsimilados  as totalsalario,"
            'SQL &= " fSalarioBase  -([fTotalPercepciones] - fIsr +fPrestamoPerA  + fAdeudoInfonavitA +fDiferenciaInfonavitA +fAsimilados) as diferencia"
            SQL &= " [fTotalPercepciones] - fIsr +fPrestamoPerA  + fAdeudoInfonavitA +fDiferenciaInfonavitA   as totalsalario,"
            SQL &= " fSalarioBase  -([fTotalPercepciones] - fIsr +fPrestamoPerA  + fAdeudoInfonavitA +fDiferenciaInfonavitA ) as diferencia"
            SQL &= " from empleadosC inner join nomina on empleadosC.iIdEmpleadoC = Nomina.fkiIdEmpleadoC"
            SQL &= " inner join periodos on fkiIdPeriodo =iIdPeriodo"
            SQL &= " where iEjercicio = " & cboperiodo.Text & " and iMes >=" & NudInicio.Value & " and iMes <=" & NudFinal.Value
            'SQL &= " and (iIdEmpleadoC=50 or iIdEmpleadoC=51 ) "
            SQL &= " order by cNombreLargo"


            lsvLista.Columns.Add("#")
            lsvLista.Columns.Add("CODIGO") '0
            lsvLista.Columns.Add("NOMBRE")
            lsvLista.Columns.Add("RFC")
            lsvLista.Columns.Add("CURP")
            lsvLista.Columns.Add("SUELDO_BASE")
            lsvLista.Columns.Add("SUELDO_BRUTO")
            lsvLista.Columns.Add("TIEMPO_EXTRA_FIJO_GRAVADO")
            lsvLista.Columns.Add("TIEMPO_EXTRA_FIJO_EXENTO")
            lsvLista.Columns.Add("TIEMPO_EXTRA_OCASIONAL")
            lsvLista.Columns.Add("DESC_SEM_OBLIGATORIO")
            lsvLista.Columns.Add("VACACIONES_PROPORCIONALES")
            lsvLista.Columns.Add("AGUINALDO_GRAVADO")
            lsvLista.Columns.Add("AGUINALDO_EXENTO")
            lsvLista.Columns.Add("TOTAL_AGUINALDO")
            lsvLista.Columns.Add("P_VAC_GRAVADO")
            lsvLista.Columns.Add("P_VAC_EXENTO")
            lsvLista.Columns.Add("TOTAL_P_VAC")
            lsvLista.Columns.Add("TOTAL_PERCEPCIONES")
            lsvLista.Columns.Add("TOTAL_PERCEPCIONES_P_ISR")
            lsvLista.Columns.Add("INCAPACIDAD")
            lsvLista.Columns.Add("ISR")
            lsvLista.Columns.Add("IMSS")
            lsvLista.Columns.Add("INFONAVIT")
            lsvLista.Columns.Add("INFONAVIT_ANT")
            lsvLista.Columns.Add("INFONAVIT_BIM_ANT")
            lsvLista.Columns.Add("PENSION_ALIMENTICIA")
            lsvLista.Columns.Add("PRESTAMO")
            lsvLista.Columns.Add("FONACOT")
            lsvLista.Columns.Add("SUBSIDIO_GENERADO")
            lsvLista.Columns.Add("SUBSIDIO_APLICADO")
            lsvLista.Columns.Add("OPERADORA")
            lsvLista.Columns.Add("PRESTAMO_PERSONAL_ASI")
            lsvLista.Columns.Add("INFONAVIT_ASI")
            lsvLista.Columns.Add("DIFERENCIA_INFONAVIT_ASI")
            lsvLista.Columns.Add("TOTAL_ASI")
            lsvLista.Columns.Add("ISR_ASIMILADOS")
            lsvLista.Columns.Add("ASIMILADOS")
            lsvLista.Columns.Add("TOTAL_SALARIO")
            lsvLista.Columns.Add("DIFERENCIA")


            Dim item As ListViewItem
            Dim consecutivo As Integer
            consecutivo = 1
            Dim rwFacturasConcepto As DataRow() = nConsulta(SQL)
            If rwFacturasConcepto Is Nothing = False Then
                For Each Fila In rwFacturasConcepto




                    item = lsvLista.Items.Add(consecutivo)


                    item.SubItems.Add(Fila.Item("cCodigoEmpleado"))
                    item.SubItems.Add(Fila.Item("cNombreLargo"))
                    item.SubItems.Add(Fila.Item("cRFC"))
                    item.SubItems.Add(Fila.Item("cCURP"))
                    item.SubItems.Add(Double.Parse(Fila.Item("fSalarioBase")) - Double.Parse(Fila.Item("fAsimilados")))
                    item.SubItems.Add(Fila.Item("fSueldoBruto"))
                    item.SubItems.Add(Fila.Item("fTExtraFijoGravado"))
                    item.SubItems.Add(Fila.Item("fTExtraFijoExento"))
                    item.SubItems.Add(Fila.Item("fTExtraOcasional"))
                    item.SubItems.Add(Fila.Item("fDescSemObligatorio"))
                    item.SubItems.Add(Fila.Item("fVacacionesProporcionales"))
                    item.SubItems.Add(Fila.Item("fAguinaldoGravado"))
                    item.SubItems.Add(Fila.Item("fAguinaldoExento"))
                    item.SubItems.Add(Double.Parse(Fila.Item("fAguinaldoGravado")) + Double.Parse(Fila.Item("fAguinaldoExento")))
                    item.SubItems.Add(Fila.Item("fPrimaVacacionalGravado"))
                    item.SubItems.Add(Fila.Item("fPrimaVacacionalExento"))
                    item.SubItems.Add(Double.Parse(Fila.Item("fPrimaVacacionalGravado")) + Double.Parse(Fila.Item("fPrimaVacacionalExento")))
                    item.SubItems.Add(Fila.Item("fTotalPercepciones"))
                    item.SubItems.Add(Fila.Item("fTotalPercepcionesISR"))
                    item.SubItems.Add(Fila.Item("fIncapacidad"))
                    item.SubItems.Add(Fila.Item("fIsr"))
                    item.SubItems.Add(Fila.Item("fImss"))
                    item.SubItems.Add(Fila.Item("fInfonavit"))
                    item.SubItems.Add(Fila.Item("fInfonavitBanterior"))
                    item.SubItems.Add(Fila.Item("fAjusteInfonavit"))
                    item.SubItems.Add(Fila.Item("fPensionAlimenticia"))
                    item.SubItems.Add(Fila.Item("fPrestamo"))
                    item.SubItems.Add(Fila.Item("fFonacot"))
                    item.SubItems.Add(Fila.Item("fSubsidioGenerado"))
                    item.SubItems.Add(Fila.Item("fSubsidioAplicado"))
                    item.SubItems.Add(Fila.Item("fOperadora"))
                    item.SubItems.Add(Fila.Item("fPrestamoPerA"))
                    item.SubItems.Add(Fila.Item("fAdeudoInfonavitA"))
                    item.SubItems.Add(Fila.Item("fDiferenciaInfonavitA"))
                    item.SubItems.Add("")
                    item.SubItems.Add("")
                    item.SubItems.Add("0")
                    'item.SubItems.Add(Fila.Item("fAsimilados"))
                    item.SubItems.Add(Fila.Item("totalsalario"))
                    item.SubItems.Add(Fila.Item("diferencia"))

                    'item.SubItems.Add(Format(CType(Fila.Item("total"), Decimal), "###,###,##0.#0"))

                    item.Tag = Fila.Item("cCodigoEmpleado")
                    consecutivo = consecutivo + 1
                    item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                    Alter = Not Alter


                Next

                MessageBox.Show(rwFacturasConcepto.Count & " Registros encontrados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                MessageBox.Show("No se encontraron datos en ese rango de fecha", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If



        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Function fisr(bruto As Double) As Double
        Dim sql As String
        Dim excendente As Double
        Dim isr As Double
        Try
            sql = "select * from isr where ((" & bruto & ">=isr.limiteinf and " & bruto & "<=isr.limitesup)"
            sql &= " or (" & bruto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=1 and anio=" & cboperiodo.Text

            Dim rwISRCALCULO As DataRow() = nConsulta(sql)

            If rwISRCALCULO Is Nothing = False Then
                excendente = bruto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)

            End If
            Return isr
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return 0
        End Try






    End Function

    Private Sub cmdProcesar_Click(sender As System.Object, e As System.EventArgs) Handles cmdProcesar.Click
        Dim propuesta As Double
        Dim bruto As Double
        Dim excendente As Double
        Dim isr As Double
        Dim perpecionneta As Double
        Dim impuestonomina As Double
        Dim comision As Double
        Dim diferencia As Double
        Dim calculado As Double

        Dim SQL As String
        Try
            If lsvLista.Items.Count > 0 Then


                pnlProgreso.Visible = True

                Application.DoEvents()



                pgbProgreso.Minimum = 0
                pgbProgreso.Value = 0
                pgbProgreso.Maximum = lsvLista.Items.Count

                For Each elemento As ListViewItem In lsvLista.Items
                    propuesta = Double.Parse(elemento.SubItems(37).Text) / 30
                    bruto = propuesta * Double.Parse(30)

                    Do
                        bruto = propuesta * 30
                        'calculos

                        'Calculamos isr

                        '1.- buscamos datos para el calculo

                        isr = fisr(bruto)

                        elemento.SubItems(36).Text = propuesta
                        calculado = bruto - isr




                        If Math.Round((calculado), 2) = Math.Round(Double.Parse(elemento.SubItems(37).Text), 2) Then
                            'el sueldo de la propuesta es correcto
                            elemento.SubItems(35).Text = propuesta
                        Else
                            If calculado > Double.Parse(elemento.SubItems(37).Text) Then

                                diferencia = (calculado) - Double.Parse(elemento.SubItems(37).Text)
                                If diferencia > 1000 Then
                                    propuesta = propuesta - 150
                                ElseIf diferencia > 500 And diferencia < 999.999 Then
                                    propuesta = propuesta - 70
                                ElseIf diferencia > 300 And diferencia < 499.999 Then
                                    propuesta = propuesta - 15
                                ElseIf diferencia > 100 And diferencia < 299.999 Then
                                    propuesta = propuesta - 10
                                ElseIf diferencia > 50 And diferencia < 99.999 Then
                                    propuesta = propuesta - 5
                                ElseIf diferencia > 20 And diferencia < 49.999 Then
                                    propuesta = propuesta - 1
                                ElseIf diferencia > 10 And diferencia < 19.999 Then
                                    propuesta = propuesta - 0.5
                                ElseIf diferencia > 5 And diferencia < 9.999 Then
                                    propuesta = propuesta - 0.4
                                ElseIf diferencia > 4.5 And diferencia < 4.999 Then
                                    propuesta = propuesta - 0.3
                                ElseIf diferencia > 4 And diferencia < 4.499 Then
                                    propuesta = propuesta - 0.2
                                ElseIf diferencia > 3.5 And diferencia < 3.999 Then
                                    propuesta = propuesta - 0.15
                                ElseIf diferencia > 3 And diferencia < 3.499 Then
                                    propuesta = propuesta - 0.1
                                ElseIf diferencia > 2.5 And diferencia < 2.999 Then
                                    propuesta = propuesta - 0.05
                                ElseIf diferencia > 2 And diferencia < 2.499 Then
                                    propuesta = propuesta - 0.04
                                ElseIf diferencia > 1 And diferencia < 1.999 Then
                                    propuesta = propuesta - 0.01
                                ElseIf diferencia > 0.5 And diferencia < 0.999 Then
                                    propuesta = propuesta - 0.008
                                ElseIf diferencia > 0.2 And diferencia < 0.49 Then
                                    propuesta = propuesta - 0.005
                                ElseIf diferencia > 0.1 And diferencia < 0.19 Then
                                    propuesta = propuesta - 0.0001
                                Else
                                    propuesta = propuesta - 0.0001

                                End If

                            Else
                                diferencia = Double.Parse(elemento.SubItems(37).Text) - (calculado)
                                If diferencia > 1000 Then
                                    propuesta = propuesta + 100
                                ElseIf diferencia > 500 And diferencia < 999.999 Then
                                    propuesta = propuesta + 40
                                ElseIf diferencia > 300 And diferencia < 499.999 Then
                                    propuesta = propuesta + 30
                                ElseIf diferencia > 100 And diferencia < 299.999 Then
                                    propuesta = propuesta + 20
                                ElseIf diferencia > 50 And diferencia < 99.999 Then
                                    propuesta = propuesta + 3
                                ElseIf diferencia > 20 And diferencia < 49.999 Then
                                    propuesta = propuesta + 1
                                ElseIf diferencia > 10 And diferencia < 19.999 Then
                                    propuesta = propuesta + 0.5
                                ElseIf diferencia > 5 And diferencia < 9.999 Then
                                    propuesta = propuesta + 0.3
                                ElseIf diferencia > 3 And diferencia < 4.999 Then
                                    propuesta = propuesta + 0.2
                                ElseIf diferencia > 3 And diferencia < 4.499 Then
                                    propuesta = propuesta + 0.1
                                ElseIf diferencia > 3 And diferencia < 3.999 Then
                                    propuesta = propuesta + 0.001
                                ElseIf diferencia > 3 And diferencia < 3.699 Then
                                    propuesta = propuesta + 0.002
                                ElseIf diferencia > 1 And diferencia < 2.599 Then
                                    propuesta = propuesta + 0.003
                                ElseIf diferencia > 1 And diferencia < 1.799 Then
                                    propuesta = propuesta + 0.004
                                ElseIf diferencia > 0.5 And diferencia < 0.999 Then
                                    propuesta = propuesta + 0.006
                                ElseIf diferencia > 0.2 And diferencia < 0.49 Then
                                    propuesta = propuesta + 0.005
                                ElseIf diferencia > 0.1 And diferencia < 0.19 Then
                                    propuesta = propuesta + 0.001
                                Else
                                    propuesta = propuesta + 0.0001

                                End If




                            End If
                        End If



                    Loop While Math.Round((calculado), 2) <> Math.Round(Double.Parse(elemento.SubItems(37).Text), 2)


                    pgbProgreso.Value += 1
                    Application.DoEvents()
                Next

                For Each elemento As ListViewItem In lsvLista.Items
                    propuesta = Double.Parse(elemento.SubItems(35).Text)


                    bruto = propuesta * 30
                    'calculos

                    'Calculamos isr

                    '1.- buscamos datos para el calculo

                    SQL = "select * from isr where ((" & bruto & ">=isr.limiteinf and " & bruto & "<=isr.limitesup)"
                    SQL &= " or (" & bruto & ">=isr.limiteinf and isr.limitesup=0)) and fkiIdTipoPeriodo2=1"

                    Dim rwISRCALCULO As DataRow() = nConsulta(SQL)
                    If rwISRCALCULO Is Nothing = False Then
                        excendente = bruto - Double.Parse(rwISRCALCULO(0)("limiteinf").ToString)
                        isr = (excendente * (Double.Parse(rwISRCALCULO(0)("porcentaje").ToString) / 100)) + Double.Parse(rwISRCALCULO(0)("cuotafija").ToString)

                    End If

                    elemento.SubItems(35).Text = Math.Round(bruto, 2)
                    elemento.SubItems(36).Text = Math.Round(isr, 2)

                Next


                pnlProgreso.Visible = False
                MessageBox.Show("Calculos terminados", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


                'Realizar la sumatoria
                Dim fSalarioBase, fSueldoBruto, fTExtraFijoGravado, fTExtraFijoExento, fTExtraOcasional, fDescSemObligatorio, fVacacionesProporcionales As Double
                Dim fAguinaldoGravado, fAguinaldoExento, fPrimaVacacionalGravado, fPrimaVacacionalExento, fTotalPercepciones, fTotalPercepcionesISR As Double
                Dim fIncapacidad, fIsrr, fImss, fInfonavit, fInfonavitBanterior, fAjusteInfonavit, fPensionAlimenticia, fPrestamo, fFonacot, fSubsidioGenerado As Double
                Dim fSubsidioAplicado, fOperadora, fPrestamoPerA, fAdeudoInfonavitA, fDiferenciaInfonavitA, TotalAsmilados, ISRAsimilado, fAsimilados As Double
                Dim Alter As Boolean
                Alter = False


                lsvAcumulado.Items.Clear()
                lsvAcumulado.Clear()




                lsvAcumulado.Columns.Add("#")
                lsvAcumulado.Columns.Add("CODIGO") '0
                lsvAcumulado.Columns.Add("NOMBRE")
                lsvAcumulado.Columns.Add("RFC")
                lsvAcumulado.Columns.Add("CURP")
                lsvAcumulado.Columns.Add("SUELDO_BASE")
                lsvAcumulado.Columns.Add("SUELDO_BRUTO")
                lsvAcumulado.Columns.Add("TIEMPO_EXTRA_FIJO_GRAVADO")
                lsvAcumulado.Columns.Add("TIEMPO_EXTRA_FIJO_EXENTO")
                lsvAcumulado.Columns.Add("TIEMPO_EXTRA_OCASIONAL")
                lsvAcumulado.Columns.Add("DESC_SEM_OBLIGATORIO")
                lsvAcumulado.Columns.Add("VACACIONES_PROPORCIONALES")
                lsvAcumulado.Columns.Add("AGUINALDO_GRAVADO")
                lsvAcumulado.Columns.Add("AGUINALDO_EXENTO")
                lsvAcumulado.Columns.Add("TOTAL_AGUINALDO")
                lsvAcumulado.Columns.Add("P_VAC_GRAVADO")
                lsvAcumulado.Columns.Add("P_VAC_EXENTO")
                lsvAcumulado.Columns.Add("TOTAL_P_VAC")
                lsvAcumulado.Columns.Add("TOTAL_PERCEPCIONES")
                lsvAcumulado.Columns.Add("TOTAL_PERCEPCIONES_P_ISR")
                lsvAcumulado.Columns.Add("INCAPACIDAD")
                lsvAcumulado.Columns.Add("ISR")
                lsvAcumulado.Columns.Add("IMSS")
                lsvAcumulado.Columns.Add("INFONAVIT")
                lsvAcumulado.Columns.Add("INFONAVIT_ANT")
                lsvAcumulado.Columns.Add("INFONAVIT_BIM_ANT")
                lsvAcumulado.Columns.Add("PENSION_ALIMENTICIA")
                lsvAcumulado.Columns.Add("PRESTAMO")
                lsvAcumulado.Columns.Add("FONACOT")
                lsvAcumulado.Columns.Add("SUBSIDIO_GENERADO")
                lsvAcumulado.Columns.Add("SUBSIDIO_APLICADO")
                lsvAcumulado.Columns.Add("OPERADORA")
                lsvAcumulado.Columns.Add("PRESTAMO_PERSONAL_ASI")
                lsvAcumulado.Columns.Add("INFONAVIT_ASI")
                lsvAcumulado.Columns.Add("DIFERENCIA_INFONAVIT_ASI")
                lsvAcumulado.Columns.Add("TOTAL_ASI")
                lsvAcumulado.Columns.Add("ISR_ASIMILADOS")
                lsvAcumulado.Columns.Add("ASIMILADOS")
                'lsvAcumulado.Columns.Add("TOTAL_SALARIO")
                'lsvAcumulado.Columns.Add("DIFERENCIA")

                For Each elemento As ListViewItem In lsvLista.Items

                Next

                Dim Bandera As Boolean

                fSalarioBase = 0
                fSueldoBruto = 0
                fTExtraFijoGravado = 0
                fTExtraFijoExento = 0
                fTExtraOcasional = 0
                fDescSemObligatorio = 0
                fVacacionesProporcionales = 0
                fAguinaldoGravado = 0
                fAguinaldoExento = 0
                fPrimaVacacionalGravado = 0
                fPrimaVacacionalExento = 0
                fTotalPercepciones = 0
                fTotalPercepcionesISR = 0
                fIncapacidad = 0
                fIsrr = 0
                fImss = 0
                fInfonavit = 0
                fInfonavitBanterior = 0
                fAjusteInfonavit = 0
                fPensionAlimenticia = 0
                fPrestamo = 0
                fFonacot = 0
                fSubsidioGenerado = 0
                fSubsidioAplicado = 0
                fOperadora = 0
                fPrestamoPerA = 0
                fAdeudoInfonavitA = 0
                fDiferenciaInfonavitA = 0
                TotalAsmilados = 0
                ISRAsimilado = 0
                fAsimilados = 0
                Dim item As ListViewItem
                For x As Integer = 0 To lsvLista.Items.Count - 1
                    'lsvLista.Items(x).SubItems(23).Text = ""
                    If x < lsvLista.Items.Count - 1 Then
                        If lsvLista.Items(x).SubItems(1).Text <> lsvLista.Items(x + 1).SubItems(1).Text Then
                            'pasar el registro y borrar los acumulados
                            fSalarioBase += lsvLista.Items(x).SubItems(5).Text
                            fSueldoBruto += lsvLista.Items(x).SubItems(6).Text
                            fTExtraFijoGravado += lsvLista.Items(x).SubItems(7).Text
                            fTExtraFijoExento += lsvLista.Items(x).SubItems(8).Text
                            fTExtraOcasional += lsvLista.Items(x).SubItems(9).Text
                            fDescSemObligatorio += lsvLista.Items(x).SubItems(10).Text
                            fVacacionesProporcionales += lsvLista.Items(x).SubItems(11).Text
                            fAguinaldoGravado += lsvLista.Items(x).SubItems(12).Text
                            fAguinaldoExento += lsvLista.Items(x).SubItems(13).Text
                            fPrimaVacacionalGravado += lsvLista.Items(x).SubItems(15).Text
                            fPrimaVacacionalExento += lsvLista.Items(x).SubItems(16).Text
                            fTotalPercepciones += lsvLista.Items(x).SubItems(18).Text
                            fTotalPercepcionesISR += lsvLista.Items(x).SubItems(19).Text
                            fIncapacidad += lsvLista.Items(x).SubItems(20).Text
                            fIsrr += lsvLista.Items(x).SubItems(21).Text
                            fImss += lsvLista.Items(x).SubItems(22).Text
                            fInfonavit += IIf(lsvLista.Items(x).SubItems(23).Text = "", 0, lsvLista.Items(x).SubItems(23).Text)
                            fInfonavitBanterior += lsvLista.Items(x).SubItems(24).Text
                            fAjusteInfonavit += lsvLista.Items(x).SubItems(25).Text
                            fPensionAlimenticia += lsvLista.Items(x).SubItems(26).Text
                            fPrestamo += lsvLista.Items(x).SubItems(27).Text
                            fFonacot += lsvLista.Items(x).SubItems(28).Text
                            fSubsidioGenerado += lsvLista.Items(x).SubItems(29).Text
                            fSubsidioAplicado += lsvLista.Items(x).SubItems(30).Text
                            fOperadora += lsvLista.Items(x).SubItems(31).Text
                            fPrestamoPerA += lsvLista.Items(x).SubItems(32).Text
                            fAdeudoInfonavitA += lsvLista.Items(x).SubItems(33).Text
                            fDiferenciaInfonavitA += lsvLista.Items(x).SubItems(34).Text
                            TotalAsmilados += lsvLista.Items(x).SubItems(35).Text
                            ISRAsimilado += lsvLista.Items(x).SubItems(36).Text
                            fAsimilados += lsvLista.Items(x).SubItems(37).Text

                            item = lsvAcumulado.Items.Add(x + 1)


                            item.SubItems.Add(lsvLista.Items(x).SubItems(1).Text)
                            item.SubItems.Add(lsvLista.Items(x).SubItems(2).Text)
                            item.SubItems.Add(lsvLista.Items(x).SubItems(3).Text)
                            item.SubItems.Add(lsvLista.Items(x).SubItems(4).Text)
                            item.SubItems.Add(fSalarioBase)
                            item.SubItems.Add(fSueldoBruto)
                            item.SubItems.Add(fTExtraFijoGravado)
                            item.SubItems.Add(fTExtraFijoExento)
                            item.SubItems.Add(fTExtraOcasional)
                            item.SubItems.Add(fDescSemObligatorio)
                            item.SubItems.Add(fVacacionesProporcionales)
                            item.SubItems.Add(fAguinaldoGravado)
                            item.SubItems.Add(fAguinaldoExento)
                            item.SubItems.Add(fAguinaldoGravado + fAguinaldoExento)
                            item.SubItems.Add(fPrimaVacacionalGravado)
                            item.SubItems.Add(fPrimaVacacionalExento)
                            item.SubItems.Add(fPrimaVacacionalGravado + fPrimaVacacionalExento)
                            item.SubItems.Add(fTotalPercepciones)
                            item.SubItems.Add(fTotalPercepcionesISR)
                            item.SubItems.Add(fIncapacidad)
                            item.SubItems.Add(fIsrr)
                            item.SubItems.Add(fImss)
                            item.SubItems.Add(fInfonavit)
                            item.SubItems.Add(fInfonavitBanterior)
                            item.SubItems.Add(fAjusteInfonavit)
                            item.SubItems.Add(fPensionAlimenticia)
                            item.SubItems.Add(fPrestamo)
                            item.SubItems.Add(fFonacot)
                            item.SubItems.Add(fSubsidioGenerado)
                            item.SubItems.Add(fSubsidioAplicado)
                            item.SubItems.Add(fOperadora)
                            item.SubItems.Add(fPrestamoPerA)
                            item.SubItems.Add(fAdeudoInfonavitA)
                            item.SubItems.Add(fDiferenciaInfonavitA)
                            item.SubItems.Add(TotalAsmilados)
                            item.SubItems.Add(ISRAsimilado)
                            item.SubItems.Add(fAsimilados)

                            'item.SubItems.Add(Format(CType(Fila.Item("total"), Decimal), "###,###,##0.#0"))

                            item.Tag = lsvLista.Items(x).SubItems(1).Text

                            item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                            Alter = Not Alter

                            fSalarioBase = 0
                            fSueldoBruto = 0
                            fTExtraFijoGravado = 0
                            fTExtraFijoExento = 0
                            fTExtraOcasional = 0
                            fDescSemObligatorio = 0
                            fVacacionesProporcionales = 0
                            fAguinaldoGravado = 0
                            fAguinaldoExento = 0
                            fPrimaVacacionalGravado = 0
                            fPrimaVacacionalExento = 0
                            fTotalPercepciones = 0
                            fTotalPercepcionesISR = 0
                            fIncapacidad = 0
                            fIsrr = 0
                            fImss = 0
                            fInfonavit = 0
                            fInfonavitBanterior = 0
                            fAjusteInfonavit = 0
                            fPensionAlimenticia = 0
                            fPrestamo = 0
                            fFonacot = 0
                            fSubsidioGenerado = 0
                            fSubsidioAplicado = 0
                            fOperadora = 0
                            fPrestamoPerA = 0
                            fAdeudoInfonavitA = 0
                            fDiferenciaInfonavitA = 0
                            TotalAsmilados = 0
                            ISRAsimilado = 0
                            fAsimilados = 0

                        Else
                            'Se suma
                            fSalarioBase += lsvLista.Items(x).SubItems(5).Text
                            fSueldoBruto += lsvLista.Items(x).SubItems(6).Text
                            fTExtraFijoGravado += lsvLista.Items(x).SubItems(7).Text
                            fTExtraFijoExento += lsvLista.Items(x).SubItems(8).Text
                            fTExtraOcasional += lsvLista.Items(x).SubItems(9).Text
                            fDescSemObligatorio += lsvLista.Items(x).SubItems(10).Text
                            fVacacionesProporcionales += lsvLista.Items(x).SubItems(11).Text
                            fAguinaldoGravado += lsvLista.Items(x).SubItems(12).Text
                            fAguinaldoExento += lsvLista.Items(x).SubItems(13).Text
                            fPrimaVacacionalGravado += lsvLista.Items(x).SubItems(15).Text
                            fPrimaVacacionalExento += lsvLista.Items(x).SubItems(16).Text
                            fTotalPercepciones += lsvLista.Items(x).SubItems(18).Text
                            fTotalPercepcionesISR += lsvLista.Items(x).SubItems(19).Text
                            fIncapacidad += lsvLista.Items(x).SubItems(20).Text
                            fIsrr += lsvLista.Items(x).SubItems(21).Text
                            fImss += lsvLista.Items(x).SubItems(22).Text
                            fInfonavit += IIf(lsvLista.Items(x).SubItems(23).Text = "", 0, lsvLista.Items(x).SubItems(23).Text)
                            fInfonavitBanterior += lsvLista.Items(x).SubItems(24).Text
                            fAjusteInfonavit += lsvLista.Items(x).SubItems(25).Text
                            fPensionAlimenticia += lsvLista.Items(x).SubItems(26).Text
                            fPrestamo += lsvLista.Items(x).SubItems(27).Text
                            fFonacot += lsvLista.Items(x).SubItems(28).Text
                            fSubsidioGenerado += lsvLista.Items(x).SubItems(29).Text
                            fSubsidioAplicado += lsvLista.Items(x).SubItems(30).Text
                            fOperadora += lsvLista.Items(x).SubItems(31).Text
                            fPrestamoPerA += lsvLista.Items(x).SubItems(32).Text
                            fAdeudoInfonavitA += lsvLista.Items(x).SubItems(33).Text
                            fDiferenciaInfonavitA += lsvLista.Items(x).SubItems(34).Text
                            TotalAsmilados += lsvLista.Items(x).SubItems(35).Text
                            ISRAsimilado += lsvLista.Items(x).SubItems(36).Text
                            fAsimilados += lsvLista.Items(x).SubItems(37).Text
                        End If
                    Else
                        fSalarioBase += lsvLista.Items(x).SubItems(5).Text
                        fSueldoBruto += lsvLista.Items(x).SubItems(6).Text
                        fTExtraFijoGravado += lsvLista.Items(x).SubItems(7).Text
                        fTExtraFijoExento += lsvLista.Items(x).SubItems(8).Text
                        fTExtraOcasional += lsvLista.Items(x).SubItems(9).Text
                        fDescSemObligatorio += lsvLista.Items(x).SubItems(10).Text
                        fVacacionesProporcionales += lsvLista.Items(x).SubItems(11).Text
                        fAguinaldoGravado += lsvLista.Items(x).SubItems(12).Text
                        fAguinaldoExento += lsvLista.Items(x).SubItems(13).Text
                        fPrimaVacacionalGravado += lsvLista.Items(x).SubItems(15).Text
                        fPrimaVacacionalExento += lsvLista.Items(x).SubItems(16).Text
                        fTotalPercepciones += lsvLista.Items(x).SubItems(18).Text
                        fTotalPercepcionesISR += lsvLista.Items(x).SubItems(19).Text
                        fIncapacidad += lsvLista.Items(x).SubItems(20).Text
                        fIsrr += lsvLista.Items(x).SubItems(21).Text
                        fImss += lsvLista.Items(x).SubItems(22).Text
                        fInfonavit += IIf(lsvLista.Items(x).SubItems(23).Text = "", 0, lsvLista.Items(x).SubItems(23).Text)
                        fInfonavitBanterior += lsvLista.Items(x).SubItems(24).Text
                        fAjusteInfonavit += lsvLista.Items(x).SubItems(25).Text
                        fPensionAlimenticia += lsvLista.Items(x).SubItems(26).Text
                        fPrestamo += lsvLista.Items(x).SubItems(27).Text
                        fFonacot += lsvLista.Items(x).SubItems(28).Text
                        fSubsidioGenerado += lsvLista.Items(x).SubItems(29).Text
                        fSubsidioAplicado += lsvLista.Items(x).SubItems(30).Text
                        fOperadora += lsvLista.Items(x).SubItems(31).Text
                        fPrestamoPerA += lsvLista.Items(x).SubItems(32).Text
                        fAdeudoInfonavitA += lsvLista.Items(x).SubItems(33).Text
                        fDiferenciaInfonavitA += lsvLista.Items(x).SubItems(34).Text
                        TotalAsmilados += lsvLista.Items(x).SubItems(35).Text
                        ISRAsimilado += lsvLista.Items(x).SubItems(36).Text
                        fAsimilados += lsvLista.Items(x).SubItems(37).Text

                        item = lsvAcumulado.Items.Add(x + 1)


                        item.SubItems.Add(lsvLista.Items(x).SubItems(1).Text)
                        item.SubItems.Add(lsvLista.Items(x).SubItems(2).Text)
                        item.SubItems.Add(lsvLista.Items(x).SubItems(3).Text)
                        item.SubItems.Add(lsvLista.Items(x).SubItems(4).Text)
                        item.SubItems.Add(fSalarioBase)
                        item.SubItems.Add(fSueldoBruto)
                        item.SubItems.Add(fTExtraFijoGravado)
                        item.SubItems.Add(fTExtraFijoExento)
                        item.SubItems.Add(fTExtraOcasional)
                        item.SubItems.Add(fDescSemObligatorio)
                        item.SubItems.Add(fVacacionesProporcionales)
                        item.SubItems.Add(fAguinaldoGravado)
                        item.SubItems.Add(fAguinaldoExento)
                        item.SubItems.Add(fAguinaldoGravado + fAguinaldoExento)
                        item.SubItems.Add(fPrimaVacacionalGravado)
                        item.SubItems.Add(fPrimaVacacionalExento)
                        item.SubItems.Add(fPrimaVacacionalGravado + fPrimaVacacionalExento)
                        item.SubItems.Add(fTotalPercepciones)
                        item.SubItems.Add(fTotalPercepcionesISR)
                        item.SubItems.Add(fIncapacidad)
                        item.SubItems.Add(fIsrr)
                        item.SubItems.Add(fImss)
                        item.SubItems.Add(fInfonavit)
                        item.SubItems.Add(fInfonavitBanterior)
                        item.SubItems.Add(fAjusteInfonavit)
                        item.SubItems.Add(fPensionAlimenticia)
                        item.SubItems.Add(fPrestamo)
                        item.SubItems.Add(fFonacot)
                        item.SubItems.Add(fSubsidioGenerado)
                        item.SubItems.Add(fSubsidioAplicado)
                        item.SubItems.Add(fOperadora)
                        item.SubItems.Add(fPrestamoPerA)
                        item.SubItems.Add(fAdeudoInfonavitA)
                        item.SubItems.Add(fDiferenciaInfonavitA)
                        item.SubItems.Add(TotalAsmilados)
                        item.SubItems.Add(ISRAsimilado)
                        item.SubItems.Add(fAsimilados)

                        'item.SubItems.Add(Format(CType(Fila.Item("total"), Decimal), "###,###,##0.#0"))

                        item.Tag = lsvLista.Items(x).SubItems(1).Text

                        item.BackColor = IIf(Alter, Color.WhiteSmoke, Color.White)
                        Alter = Not Alter
                    End If

                Next
            Else
                MessageBox.Show("No hay registros para realizar el calculo", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Dim filaExcel As Integer = 5
            Dim dialogo As New SaveFileDialog()
            Dim idtipo As Integer
            Dim Alter As Boolean = False
            

            Dim libro As New ClosedXML.Excel.XLWorkbook
            Dim hoja As IXLWorksheet = libro.Worksheets.Add("Acumulado")
            hoja.Column("A").Width = 10
            hoja.Column("B").Width = 10
            hoja.Column("C").Width = 20
            hoja.Column("D").Width = 15
            hoja.Column("E").Width = 15
            hoja.Column("F").Width = 15
            hoja.Column("G").Width = 15
            hoja.Column("H").Width = 15
            hoja.Column("I").Width = 15
            hoja.Column("J").Width = 15
            hoja.Column("K").Width = 15
            hoja.Column("L").Width = 15
            hoja.Column("M").Width = 15
            hoja.Column("N").Width = 15
            hoja.Column("O").Width = 15

            hoja.Range(1, 1, 1, 37).Style.Font.FontSize = 10
            hoja.Range(1, 1, 1, 37).Style.Font.SetBold(True)
            hoja.Range(1, 1, 1, 37).Style.Alignment.WrapText = True
            hoja.Range(1, 1, 1, 37).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            hoja.Range(1, 1, 1, 37).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            'hoja.Range(4, 1, 4, 18).Style.Fill.BackgroundColor = XLColor.BleuDeFrance
            hoja.Range(1, 1, 1, 37).Style.Fill.BackgroundColor = XLColor.FromHtml("#538DD5")
            hoja.Range(1, 1, 1, 37).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
            hoja.Range(2, 5, 1500, 37).Style.NumberFormat.SetFormat("###,###,##0.#0")
            'hoja.Cell(4, 1).Value = "Num"

            hoja.Cell(1, 1).Value = "Codigo"
            hoja.Cell(1, 2).Value = "Nombre"
            hoja.Cell(1, 3).Value = "RFC"
            hoja.Cell(1, 4).Value = "CURP"
            hoja.Cell(1, 5).Value = "fSalarioBase"
            hoja.Cell(1, 6).Value = "fSueldoBruto"
            hoja.Cell(1, 7).Value = "fTExtraFijoGravado"
            hoja.Cell(1, 8).Value = "fTExtraFijoExento"
            hoja.Cell(1, 9).Value = "fTExtraOcasional"
            hoja.Cell(1, 10).Value = "fDescSemObligatorio"
            hoja.Cell(1, 11).Value = "fVacacionesProporcionales"
            hoja.Cell(1, 12).Value = "fAguinaldoGravado"
            hoja.Cell(1, 13).Value = "fAguinaldoExento"
            hoja.Cell(1, 14).Value = "Total_aguinaldo"
            hoja.Cell(1, 15).Value = "fPrimaVacacionalGravado"
            hoja.Cell(1, 16).Value = "fPrimaVacacionalExento"
            hoja.Cell(1, 17).Value = "Total_prima_vacacional"
            hoja.Cell(1, 18).Value = "fTotalPercepciones"
            hoja.Cell(1, 19).Value = "fTotalPercepcionesISR"
            hoja.Cell(1, 20).Value = "fIncapacidad"
            hoja.Cell(1, 21).Value = "fIsr"
            hoja.Cell(1, 22).Value = "fImss"
            hoja.Cell(1, 23).Value = "fInfonavit"
            hoja.Cell(1, 24).Value = "fInfonavitBanterior"
            hoja.Cell(1, 25).Value = "fAjusteInfonavit"
            hoja.Cell(1, 26).Value = "fPensionAlimenticia"
            hoja.Cell(1, 27).Value = "fPrestamo"
            hoja.Cell(1, 28).Value = "fFonacot"
            hoja.Cell(1, 29).Value = "fSubsidioGenerado"
            hoja.Cell(1, 30).Value = "fSubsidioAplicado"
            hoja.Cell(1, 31).Value = "fOperadora"
            hoja.Cell(1, 32).Value = "fPrestamoPerA"
            hoja.Cell(1, 33).Value = "fAdeudoInfonavitA"
            hoja.Cell(1, 34).Value = "fDiferenciaInfonavitA"
            hoja.Cell(1, 35).Value = "TotalAsmilados"
            hoja.Cell(1, 36).Value = "ISRAsimilado"
            hoja.Cell(1, 37).Value = "fAsimilados"
            



            filaExcel = 1

            For Each elemento As ListViewItem In lsvAcumulado.Items
                filaExcel = filaExcel + 1
                
                hoja.Cell(filaExcel, 1).Value = elemento.SubItems(1).Text
                hoja.Cell(filaExcel, 2).Value = elemento.SubItems(2).Text
                hoja.Cell(filaExcel, 3).Value = elemento.SubItems(3).Text
                hoja.Cell(filaExcel, 4).Value = elemento.SubItems(4).Text
                hoja.Cell(filaExcel, 5).Value = elemento.SubItems(5).Text
                hoja.Cell(filaExcel, 6).Value = elemento.SubItems(6).Text
                hoja.Cell(filaExcel, 7).Value = elemento.SubItems(7).Text
                hoja.Cell(filaExcel, 8).Value = elemento.SubItems(8).Text
                hoja.Cell(filaExcel, 9).Value = elemento.SubItems(9).Text
                hoja.Cell(filaExcel, 10).Value = elemento.SubItems(10).Text
                hoja.Cell(filaExcel, 11).Value = elemento.SubItems(11).Text
                hoja.Cell(filaExcel, 12).Value = elemento.SubItems(12).Text
                hoja.Cell(filaExcel, 13).Value = elemento.SubItems(13).Text
                hoja.Cell(filaExcel, 14).Value = elemento.SubItems(14).Text
                hoja.Cell(filaExcel, 15).Value = elemento.SubItems(15).Text
                hoja.Cell(filaExcel, 16).Value = elemento.SubItems(16).Text
                hoja.Cell(filaExcel, 17).Value = elemento.SubItems(17).Text
                hoja.Cell(filaExcel, 18).Value = elemento.SubItems(18).Text
                hoja.Cell(filaExcel, 19).Value = elemento.SubItems(19).Text
                hoja.Cell(filaExcel, 20).Value = elemento.SubItems(20).Text
                hoja.Cell(filaExcel, 21).Value = elemento.SubItems(21).Text
                hoja.Cell(filaExcel, 22).Value = elemento.SubItems(22).Text
                hoja.Cell(filaExcel, 23).Value = elemento.SubItems(23).Text
                hoja.Cell(filaExcel, 24).Value = elemento.SubItems(24).Text
                hoja.Cell(filaExcel, 25).Value = elemento.SubItems(25).Text
                hoja.Cell(filaExcel, 26).Value = elemento.SubItems(26).Text
                hoja.Cell(filaExcel, 27).Value = elemento.SubItems(27).Text
                hoja.Cell(filaExcel, 28).Value = elemento.SubItems(28).Text
                hoja.Cell(filaExcel, 29).Value = elemento.SubItems(29).Text
                hoja.Cell(filaExcel, 30).Value = elemento.SubItems(30).Text
                hoja.Cell(filaExcel, 31).Value = elemento.SubItems(31).Text
                hoja.Cell(filaExcel, 32).Value = elemento.SubItems(32).Text
                hoja.Cell(filaExcel, 33).Value = elemento.SubItems(33).Text
                hoja.Cell(filaExcel, 34).Value = elemento.SubItems(34).Text
                hoja.Cell(filaExcel, 35).Value = elemento.SubItems(35).Text
                hoja.Cell(filaExcel, 36).Value = elemento.SubItems(36).Text
                hoja.Cell(filaExcel, 37).Value = elemento.SubItems(37).Text
            Next

            

            dialogo.DefaultExt = "*.xlsx"
            dialogo.FileName = "Acumulado"
            dialogo.Filter = "Archivos de Excel (*.xlsx)|*.xlsx"
            dialogo.ShowDialog()
            libro.SaveAs(dialogo.FileName)
            'libro.SaveAs("c:\temp\control.xlsx")
            'libro.SaveAs(dialogo.FileName)
            'apExcel.Quit()
            libro = Nothing
            MessageBox.Show("Archivo generado correctamente", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub frmPlaneacionAsi_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class