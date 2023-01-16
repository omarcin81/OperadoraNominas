Imports System.Net
Imports System.Net.Mail

Module mdoMail

    Public eMailRoot As String, PWDMailRoot As String

    Public Function Enviar_Mail(ByVal sBody As String, ByVal eMailDest As String, Optional ByVal sSubject As String = "", Optional ByVal Archivos As String = "", Optional ByRef eError As String = "") As Boolean
        Dim aArchivos As String()
        Dim Razon As String
        Dim Email As String

        'Dim SQL As String = "SELECT EmailFAC, PWDMail, Encabezado FROM Configuracion"

        'Razon = ObtencampoDR(SQL, 2)
        'eMailRoot = ObtencampoDR(SQL, 0)
        eMailRoot = "sistema@mbcgroup.mx"
        'PWDMailRoot = ObtencampoDR(SQL, 1)
        'Email = ObtencampoDR(SQL, 3)

        Dim LogonInfo As New NetworkCredential("sistema@mbcgroup.mx", "ade1f0abcd")
        Dim Mensaje As New MailMessage()
        Dim Adjunto As Mail.Attachment
        Dim Correos As String(), i As Integer
        Dim CorreoDe As New MailAddress("sistema@mbcgroup.mx", "Sistema")
        Dim CorreoPara As MailAddress

        Correos = eMailDest.Split(";")
        If Correos.Length = 1 Then
            CorreoPara = New MailAddress(eMailDest)
            Mensaje = New MailMessage(CorreoDe, CorreoPara)
        ElseIf Correos.Length > 1 Then
            CorreoPara = New MailAddress(Correos(0))
            Mensaje = New MailMessage(CorreoDe, CorreoPara)
            For i = 1 To Correos.Length - 1
                Mensaje.CC.Add(Correos(i))
            Next
        End If

        Mensaje.Bcc.Add("o.perez@mbcgroup.mx")
        If Email <> "" Then
            Mensaje.Bcc.Add(Email)
        End If
        Mensaje.Subject = sSubject
        Mensaje.Body = sBody

        Mensaje.IsBodyHtml = True

        Try

            Dim Client As SmtpClient

            If InStr(eMailRoot.ToLower, "gmail.com") > 0 Then
                Client = New SmtpClient("smtp.gmail.com", 587)
                Client.EnableSsl = True
            ElseIf InStr(eMailRoot.ToLower, "mbcgroup.mx") > 0 Then
                Client = New SmtpClient("mail.mbcgroup.mx", 26)
                Client.EnableSsl = False
                Client.UseDefaultCredentials = True
            ElseIf InStr(eMailRoot.ToLower, "hotmail.com") > 0 Then
                Client = New SmtpClient("smtp.live.com", 587)
                Client.EnableSsl = True
            End If

            'Client.EnableSsl = True



            Client.Credentials = LogonInfo
            aArchivos = Archivos.Split("|")
            For i = 0 To aArchivos.Length - 1
                If aArchivos(i) <> "" Then
                    Adjunto = New Mail.Attachment(aArchivos(i))
                    Mensaje.Attachments.Add(Adjunto)
                End If
            Next

            Client.Send(Mensaje)
        Catch ex As SmtpException
            eError = ex.Message
            Return False
        End Try
        Return True
    End Function

    Public Function Enviar_Mail_Pedido(ByVal sBody As String, idProveedor As String, Optional ByRef eError As String = "") As Boolean
        Dim aArchivos As String()
        Dim Razon As String
        Dim Email As String

        Dim SQL As String = "SELECT EmailFAC, PWDMail, Encabezado FROM Configuracion"

        Razon = ObtencampoDR(SQL, 2)
        eMailRoot = ObtencampoDR(SQL, 0)
        PWDMailRoot = ObtencampoDR(SQL, 1)
        'Email = ObtencampoDR(SQL, 3)

        SQL = "select * from catproveedores where iIdProveedor=" & idProveedor

        Dim LogonInfo As New NetworkCredential(eMailRoot, PWDMailRoot)
        Dim Mensaje As New MailMessage()
        Dim Adjunto As Mail.Attachment
        Dim Correos As String(), i As Integer
        Dim CorreoDe As New MailAddress(eMailRoot, Razon)
        Dim CorreoPara As MailAddress

        Correos = ObtencampoDR(SQL, 12).Split(";")
        'eMailDest.Split(";")
        If Correos.Length = 1 Then
            CorreoPara = New MailAddress(ObtencampoDR(SQL, 12))
            'eMailDest)
            Mensaje = New MailMessage(CorreoDe, CorreoPara)
        ElseIf Correos.Length > 1 Then
            CorreoPara = New MailAddress(Correos(0))
            Mensaje = New MailMessage(CorreoDe, CorreoPara)
            For i = 1 To Correos.Length - 1
                Mensaje.CC.Add(Correos(i))
            Next
        End If

        'Mensaje.Bcc.Add("braulio_romero05@hotmail.com")
        If Email <> "" Then
            Mensaje.Bcc.Add(Email)
        End If
        Mensaje.Subject = "Pedido"
        Mensaje.Body = sBody

        Mensaje.IsBodyHtml = True

        Try

            Dim Client As SmtpClient

            If InStr(eMailRoot.ToLower, "gmail.com") > 0 Then
                Client = New SmtpClient("smtp.gmail.com", 587)
                Client.EnableSsl = True
            ElseIf InStr(eMailRoot.ToLower, "cfdimatico.com") > 0 Then
                Client = New SmtpClient("mail.cfdimatico.com.mx", 26)
                Client.EnableSsl = False
                Client.UseDefaultCredentials = True
            ElseIf InStr(eMailRoot.ToLower, "hotmail.com") > 0 Then
                Client = New SmtpClient("smtp.live.com", 587)
                Client.EnableSsl = True
            End If

            'Client.EnableSsl = True



            Client.Credentials = LogonInfo
            'aArchivos = Archivos.Split("|")
            'For i = 0 To aArchivos.Length - 1
            '    If aArchivos(i) <> "" Then
            '        Adjunto = New Mail.Attachment(aArchivos(i))
            '        Mensaje.Attachments.Add(Adjunto)
            '    End If
            'Next

            Client.Send(Mensaje)
        Catch ex As SmtpException
            eError = ex.Message
            Return False
        End Try
        Return True
    End Function

    Public Function Generar_CuerpoCBB(ByVal IdFactura As String, Optional ByVal Empresa As String = "Materiales para construcción Casa Blanca", Optional ByVal Devolucion As Boolean = False) As String
        Dim sumaL As String
        Dim CuerpoMail As String

        CuerpoMail = " <html>"
        CuerpoMail &= "<head>" & vbCrLf
        CuerpoMail &= "<title>Servicio de envío de facturación</title>" & vbCrLf
        CuerpoMail &= " </head>" & vbCrLf
        CuerpoMail &= "<body topmargin=0 leftmargin=0 marginheight=0 marginwidth=0 >"
        CuerpoMail &= "<h1 align=" & Chr(34) & "center" & Chr(34) & "><font color=" & Chr(34) & "Navy" & Chr(34) & ">" & Empresa & "</Font></h1>"
        CuerpoMail &= "<h3 align=" & Chr(34) & "center" & Chr(34) & "><font color=" & Chr(34) & "Black" & Chr(34) & " face=" & Chr(34) & "Calibri" & Chr(34) & ">Servicio de envío de comprobantes</Font></h3>"
        CuerpoMail &= "<font size=2 face=Calibri>"
        CuerpoMail &= "<table width=" & Chr(34) & "700" & Chr(34) & "  cellspacing=0 cellpadinng=0 border=0  align=center>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & ">"
        CuerpoMail &= "Por medio del presente correo electrónico se le envía su factura correspondiente al pago de los siguientes conceptos:"
        CuerpoMail &= "</td></tr>"
        CuerpoMail &= "<tr>     <td  colspan=" & Chr(34) & "3" & Chr(34) & "></td> </tr>"
        CuerpoMail &= "<tr>     <td  colspan=" & Chr(34) & "3" & Chr(34) & "></td> </tr>"
        CuerpoMail &= "<tr style=" & Chr(34) & " color:white;" & Chr(34) & ">	"

        CuerpoMail &= "<th width=" & Chr(34) & "455" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & ">Concepto</th>"
        CuerpoMail &= "<th width=" & Chr(34) & "105" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & ">Cantidad</th>"
        CuerpoMail &= "<th width=" & Chr(34) & "140" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & " >Peso</th>"
        CuerpoMail &= "</tr>"


        'Dim SQL As String = "SELECT * FROM DetFacturacion WHERE IdFactura='" & IdFactura & "'"
        Dim SQL As String = "SELECT * FROM ConceptosTrans WHERE IdFactura='" & IdFactura & "'"
        Dim rwConceptos As DataRow() = nConsultaAC(SQL)

        SQL = "SELECT Facturacion.*,Total*(IVA/100) AS GIVA, Total*(1+(IVA/100)) AS GTotal, FoliosCBB.NoAprobacion, Serie, FoliosCBB.Fecha AS FechaAP, RFC "
        SQL &= " FROM (Facturacion INNER JOIN FoliosCBB ON (Facturacion.IdFolio=FoliosCBB.IdFolio))"
        SQL &= " INNER JOIN Clientes ON (Facturacion.IdCliente=Clientes.IdCliente) WHERE IdFactura='" & IdFactura & "'"
        Dim Factura As DataRow = nConsultaAC(SQL)(0)

        For Each Concepto In rwConceptos
            CuerpoMail &= "<tr><td>" & Concepto!Descripcion & "</td><td align=center>" & Concepto!Cantidad & "</td><td align=right>" & FormatNumber(Concepto!Peso, 2) & " kg</td></tr>"
        Next

        SQL = "SELECT * FROM FacturaComplemento WHERE IdFactura='" & IdFactura & "'"
        Dim rwDatos As DataRow = nConsultaAC(SQL)(0)
        Dim Importe As Double
        Dim IVA As Double
        Dim SubTotal As Double
        Dim RetIVA As Double
        Dim Total As Double
        Dim LetraRet As String = "", Retenedor = ""

        Importe = rwDatos!Flete + rwDatos!Seguro + rwDatos!Maniobras + rwDatos!Otros
        IVA = Importe * (Val(Factura!IVA) / 100)
        SubTotal = Importe + IVA
        RetIVA = IIf(Factura!RFC.ToString().Length = 12, rwDatos!Flete * 0.04, 0)
        If Factura!RFC.ToString().Length = 12 Then
            LetraRet = "(" & ImprimeLetra(FormatNumber(RetIVA, 2)) & ")"
        End If
        Total = SubTotal - RetIVA

        sumaL = ImprimeLetra(Total)
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Flete</b></td><td align=right><b>" & FormatCurrency(rwDatos!Flete) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Seguro</b></td><td align=right><b>" & FormatCurrency(rwDatos!Seguro) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Maniobras</b></td><td align=right><b>" & FormatCurrency(rwDatos!Maniobras) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Otros</b></td><td align=right><b>" & FormatCurrency(rwDatos!Otros) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Importe</b></td><td align=right><b>" & FormatCurrency(Importe) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>I. V. A.(" + FormatNumber(Factura!IVA, 2) + ")</b></td><td align=right><b>" & FormatCurrency(IVA) & "</b></td></tr>"
        If RetIVA > 0 Then
            CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>RET I. V. A.</b></td><td align=right><b>" & FormatCurrency(RetIVA * -1) & "</b></td></tr>"
        End If
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Total</b></td><td align=right><b>" & FormatCurrency(Total) & "</b></td></tr>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & " height=30 align=center>(" & sumaL & ")</td></tr>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & " width=" & Chr(34) & "700" & Chr(34) & ">"

        CuerpoMail &= "<b>Folio de la factura: </b>" & Factura!Serie & Factura!FolioCBB & "<br>"
        CuerpoMail &= "<b>No. de aprobacion: </b>" & Factura!NoAprobacion & "<br>"
        CuerpoMail &= "<b>Fecha de aprobación: </b>" & Factura!FechaAP & "<br>"

        CuerpoMail &= "Adjunto al correo se le envía el correspondiente archivo en formato PDF (para poder visualizar su comprobante puede descargar <a href=" & Chr(34) & "http://get.adobe.com/es/reader/download/" & Chr(34) & " target=" & Chr(34) & "_blank" & Chr(34) & ">Acrobat Reader</a>)."
        CuerpoMail &= "</br></br>"

        CuerpoMail &= "</table>"
        CuerpoMail &= "</font>"
        CuerpoMail &= "</body>"
        CuerpoMail &= "</html> "
        Return CuerpoMail

    End Function

    Public Function GenerarCorreoFlujo(ByVal Encabezado As String, ByVal Destinatario As String, ByVal Cadena As String) As String
        Dim CuerpoMail As String



        CuerpoMail = ""
        CuerpoMail = "<html>" & vbCrLf
        CuerpoMail += "<head>"
        CuerpoMail += "<title>" & Encabezado & "</title>"
        CuerpoMail += "</head>"
        CuerpoMail += "<body>"
        CuerpoMail += "<form>"
        CuerpoMail += "<div>"
        CuerpoMail += "<table border='1' cellspacing='0' cellpadding='2' bordercolor='000000' style=""" & "width :700px; height : auto ; margin : 0 auto; border: 1px solid #000; font-size :11px; font-family :Verdana ;""" & ">"
        CuerpoMail += "<tr>"
        CuerpoMail += "<td colspan='2'>"
        CuerpoMail += "Estimado(a): " & Destinatario
        CuerpoMail += "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr>"
        CuerpoMail += "<td colspan='2' align=""" & "center""" & ">"
        CuerpoMail += "<strong>" & Cadena & "</strong>"
        CuerpoMail += "</td>"
        CuerpoMail += "</tr>"


        'Aqui va el for

        'If rwFilas Is Nothing = False Then
        '    For Each Fila In rwFilas
        '        CuerpoMail += "<tr><td style=""" & "text-align:center;""" & ">" & Fila.Item(1) & "</td>"
        '        CuerpoMail += "<td style=""" & "text-align:center;""" & ">" & Fila.Item(2) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(3) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(4) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(5) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(6) & "</td></tr>"
        '    Next
        'End If
        CuerpoMail += "<tr><td colspan='6'>Por favor no contestar este correo ya que fue generado de manera automatica por el sistema</td></tr>"
        CuerpoMail += "</table>"
        CuerpoMail += "</div>"
        CuerpoMail += "</form>"
        CuerpoMail += "</body>"
        CuerpoMail += "</html>"

        Return CuerpoMail

    End Function
    Public Function GenerarCorreo(ByVal IdEmpresa As String, ByVal IdCliente As String, ByVal idEmpleado As String) As String
        Dim CuerpoMail As String
        Dim Sql As String = ""
        Sql = "select cCodigoEmpleado,cNombreLargo,clientes.nombrefiscal as cliente, empresa.nombrefiscal as empresa "
        Sql &= " from (empleados inner join clientes on"
        Sql &= " empleados.fkiIdCliente= clientes.iIdCliente) inner join empresa"
        Sql &= " on empleados.fkiIdEmpresa= empresa.iIdEmpresa"
        Sql &= " where iIdEmpleado =" & idEmpleado

        Dim rwFilas As DataRow() = nConsulta(Sql)


        CuerpoMail = ""
        CuerpoMail = "<html>" & vbCrLf
        CuerpoMail += "<head>"
        CuerpoMail += "<title>Alta Empleado</title>"
        CuerpoMail += "</head>"
        CuerpoMail += "<body>"
        CuerpoMail += "<form>"
        CuerpoMail += "<div>"
        CuerpoMail += "<table border='1' cellspacing='0' cellpadding='2' bordercolor='000000' style=""" & "width :700px; height : auto ; margin : 0 auto; border: 1px solid #000; font-size :11px; font-family :Verdana ;""" & ">"
        CuerpoMail += "<tr>"
        CuerpoMail += "<td colspan='2'>"
        CuerpoMail += "Estimado(a): Area IMSS, Area Juridico"
        CuerpoMail += "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr>"
        CuerpoMail += "<td colspan='2' align=""" & "center""" & ">"
        CuerpoMail += "<strong>Datos alta</strong>"
        CuerpoMail += "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr style=""" & "font-weight :bold ;""" & ">"
        CuerpoMail += "<td style=""" & "text-align:center; """ & ">Codigo Empleado</td>"
        CuerpoMail += "<td style=""" & "text-align:center; """ & ">" & rwFilas(0).Item(0) & "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr style=""" & "font-weight :bold ;""" & ">"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">Nombre Empleado</td>"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">" & rwFilas(0).Item(1) & "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr style=""" & "font-weight :bold ;""" & ">"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">Cliente</td>"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">" & rwFilas(0).Item(2) & "</td>"
        CuerpoMail += "</tr>"
        CuerpoMail += "<tr style=""" & "font-weight :bold ;""" & ">"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">Empresa patrona</td>"
        CuerpoMail += "<td style=""" & "text-align:center;""" & ">" & rwFilas(0).Item(3) & "</td>"
        CuerpoMail += "</tr>"
        'Aqui va el for

        'If rwFilas Is Nothing = False Then
        '    For Each Fila In rwFilas
        '        CuerpoMail += "<tr><td style=""" & "text-align:center;""" & ">" & Fila.Item(1) & "</td>"
        '        CuerpoMail += "<td style=""" & "text-align:center;""" & ">" & Fila.Item(2) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(3) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(4) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(5) & "</td>"
        '        CuerpoMail += "<td>" & Fila.Item(6) & "</td></tr>"
        '    Next
        'End If
        CuerpoMail += "<tr><td colspan='6'>Por favor no contestar este correo ya que fue generado de manera automatica por el sistema de altas empleados</td></tr>"
        CuerpoMail += "</table>"
        CuerpoMail += "</div>"
        CuerpoMail += "</form>"
        CuerpoMail += "</body>"
        CuerpoMail += "</html>"

        Return CuerpoMail

    End Function

    Public Function Generar_Cuerpo(ByVal IdFactura As String, Optional ByVal Empresa As String = "Materiales para construcción Casa Blanca", Optional ByVal Devolucion As Boolean = False) As String
        Dim sumaL As String
        Dim CuerpoMail As String

        Empresa = ObtencampoDR("SELECT Nombre FROM Configuracion", 0)

        CuerpoMail = " <html>"
        CuerpoMail &= "<head>" & vbCrLf
        CuerpoMail &= "<title>Servicio de envío de facturación digital</title>" & vbCrLf
        CuerpoMail &= " </head>" & vbCrLf
        CuerpoMail &= "<body topmargin=0 leftmargin=0 marginheight=0 marginwidth=0 >"
        CuerpoMail &= "<h1 align=" & Chr(34) & "center" & Chr(34) & "><font color=" & Chr(34) & "Navy" & Chr(34) & ">" & Empresa & "</Font></h1>"
        CuerpoMail &= "<h3 align=" & Chr(34) & "center" & Chr(34) & "><font color=" & Chr(34) & "Black" & Chr(34) & " face=" & Chr(34) & "Calibri" & Chr(34) & ">Servicio de envío de CFDI</Font></h3>"
        CuerpoMail &= "<font size=2 face=Calibri>"
        CuerpoMail &= "<table width=" & Chr(34) & "700" & Chr(34) & "  cellspacing=0 cellpadinng=0 border=0  align=center>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & ">"
        CuerpoMail &= "Por medio del presente correo electrónico se le envía su factura digital correspondiente al pago de los siguientes conceptos:"
        CuerpoMail &= "</td></tr>"
        CuerpoMail &= "<tr>     <td  colspan=" & Chr(34) & "3" & Chr(34) & "></td> </tr>"
        CuerpoMail &= "<tr>     <td  colspan=" & Chr(34) & "3" & Chr(34) & "></td> </tr>"
        CuerpoMail &= "<tr style=" & Chr(34) & " color:white;" & Chr(34) & ">	"

        CuerpoMail &= "<th width=" & Chr(34) & "455" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & ">Concepto</th>"
        CuerpoMail &= "<th width=" & Chr(34) & "105" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & ">Cantidad</th>"
        CuerpoMail &= "<th width=" & Chr(34) & "140" & Chr(34) & " bgcolor=" & Chr(34) & "#004A91" & Chr(34) & " >Monto</th>"
        CuerpoMail &= "</tr>"


        Dim SQL As String = "SELECT * FROM DetFactura WHERE IdFactura='" & IdFactura & "'"
        Dim rwConceptos As DataRow() = nConsultaAC(SQL)

        SQL = "SELECT * FROM Facturas WHERE IdFactura='" & IdFactura & "'"
        Dim Factura As DataRow = nConsultaAC(SQL)(0)

        For Each Concepto In rwConceptos
            CuerpoMail &= "<tr><td>" & Concepto!Descripcion & " " & Concepto!Marca & "</td><td align=center>" & Concepto!Cantidad & "</td><td align=right>" & FormatCurrency(Concepto!Cantidad * Concepto!Importe, 2) & "</td></tr>"
        Next

        sumaL = Factura!Letra
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>SubTotal</b></td><td align=right><b>" & FormatCurrency(Factura!SubTotal) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>IVA</b></td><td align=right><b>" & FormatCurrency(Factura!IVA) & "</b></td></tr>"
        CuerpoMail &= "<tr><td align=right colspan=" & Chr(34) & "2" & Chr(34) & "><b>Total</b></td><td align=right><b>" & FormatCurrency(Factura!Total) & "</b></td></tr>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & " height=30 align=center>(" & sumaL & ")</td></tr>"
        CuerpoMail &= "<tr><td colspan=" & Chr(34) & "3" & Chr(34) & " width=" & Chr(34) & "700" & Chr(34) & ">"


        Dim Sello As String, aSello() As String



        Sello = Factura!SelloCFD
        aSello = PartirCadena(Sello, 80)
        CuerpoMail &= "<b>Folio digital de la factura: </b>" & Factura!UUID & "<br>"
        CuerpoMail &= "<b>Sello digital del CFDI: </b>" & vbCrLf
        For k As Integer = 0 To aSello.Length - 1
            If aSello(k) Is Nothing = False Then
                CuerpoMail &= aSello(k) & "<br>"
            End If
        Next

        CuerpoMail &= "<br>"


        CuerpoMail &= "Adjunto al correo se le envía el correspondiente archivo xml y una representación gráfica en formato PDF (para poder visualizar su comprobante puede descargar <a href=" & Chr(34) & "http://get.adobe.com/es/reader/download/" & Chr(34) & " target=" & Chr(34) & "_blank" & Chr(34) & ">Acrobat Reader</a>)."
        CuerpoMail &= "</br></br>"

        CuerpoMail &= "</table>"
        CuerpoMail &= "</font>"
        CuerpoMail &= "</body>"
        CuerpoMail &= "</html> "
        Return CuerpoMail

    End Function

    Private Function PartirCadena(ByVal Cadena As String, Optional ByVal Longitud As Integer = 110) As String()
        Dim devolver As String(), Index As Integer = 0
        If Cadena.Length <= Longitud Then
            ReDim devolver(1)
            devolver(0) = Cadena
        Else
            Do
                ReDim Preserve devolver(Index)
                devolver(Index) &= Mid(Cadena, 1, Longitud)
                If Cadena.Length > Longitud Then
                    Cadena = Mid(Cadena, Longitud + 1)
                End If
                Index += 1
            Loop While Cadena.Length > Longitud
            If Cadena.Length > 0 Then
                ReDim Preserve devolver(Index)
                devolver(Index) &= Cadena
            End If
        End If
        Return devolver
    End Function

End Module