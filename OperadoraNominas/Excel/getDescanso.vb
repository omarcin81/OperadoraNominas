Module getDescanso
    Public valor As String = 0
    Public tiponom As String
    Public sql As String

    Public Function GetNominaDescanso(ByRef tiponomina As String, ByRef trabjador As String, ByRef dias As String, ByRef tipe As String, Optional ByRef buque As String = "", Optional ByRef serie As String = "", Optional ByRef periodo As String = "", Optional ByRef puesto As String = "") As Double

        If tiponomina = 1 Then
            tiponom = 0
        Else
            tiponom = 1
        End If
        sql = "select * from NominaFinal inner join EmpleadosC on fkiIdEmpleadoC=iIdEmpleadoC"
        sql &= " where Nomina.fkiIdEmpresa = 1 And fkiIdPeriodo = " & periodo
        sql &= " and Nomina.iEstatus=1 and iEstatusEmpleado=" & serie
        sql &= " and iTipoNomina=" & tiponom
        sql &= " and EmpleadosC.cCodigoEmpleado=" & trabjador
        sql &= " and Nomina.iDiasTrabajados=" & dias
        If puesto <> "" Then
            Dim nombrepuesto As DataRow() = nConsulta("select * FROM Puestos where cNombre like '" & puesto & "'")
            If puesto Is Nothing Then
            Else
                sql &= " and Nomina.fkiIdPuesto=" & nombrepuesto(0).Item("iIdPuesto")
            End If
        End If
        If buque = "" Then
            sql &= " order by " & "Nomina.Buque, cNombreLargo"
        Else
            sql &= " and Nomina.Buque='" & buque & "'"
            sql &= " order by " & "Nomina.Buque, cNombreLargo"
        End If


        Dim rwNominaGuardada As DataRow() = nConsulta(sql)

        'If rwNominaGuardadaFinal Is Nothing = False Then
        If rwNominaGuardada Is Nothing = False Then

            Select Case tipe
                Case "sueldoO"
                    valor = rwNominaGuardada(0)("fSalarioBase").ToString
                Case "prestamoA"
                    valor = rwNominaGuardada(0)("fPrestamoPerA").ToString
                Case "sueldoBruto"
                    valor = rwNominaGuardada(0)("fSueldoBruto").ToString
                Case "Asimilado"
                    valor = rwNominaGuardada(0)("fAsimilados").ToString
                Case "subsidio"
                    valor = rwNominaGuardada(0)("fSubsidioAplicado").ToString
                Case "fFonacot"
                    valor = rwNominaGuardada(0)("fFonacot").ToString
                Case "isr"
                    valor = rwNominaGuardada(0)("fIsr").ToString
                Case "infonavit"
                    valor = rwNominaGuardada(0)("fInfonavit").ToString
                Case "infonavitbim"
                    valor = rwNominaGuardada(0)("fInfonavitBanterior").ToString
                Case "infonavitajust"
                    valor = rwNominaGuardada(0)("fAjusteInfonavit").ToString
                Case "pension"
                    valor = rwNominaGuardada(0)("fPensionAlimenticia").ToString
                Case "prestamo"
                    valor = rwNominaGuardada(0)("fPrestamo").ToString
                Case "fonacot"
                    valor = rwNominaGuardada(0)("fFonacot").ToString
                Case "fOperadora"
                    valor = rwNominaGuardada(0)("fOperadora").ToString
                Case "IMSSCS"
                    valor = rwNominaGuardada(0)("fImssCS").ToString
                Case "RCVCS"
                    valor = rwNominaGuardada(0)("fRcvCS").ToString
                Case "INFONAVITCS"
                    valor = rwNominaGuardada(0)("fInfonavitCS").ToString
                Case "INSCS"
                    valor = rwNominaGuardada(0)("fInsCS").ToString
            End Select


        End If

        Return CDbl(valor)
    End Function
End Module
