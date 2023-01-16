Module mdoNumAPal

    '*******************************************
    ' Convierte un número  del 1 al 9 en texto. *
    '*******************************************
    Dim Pointer1 As Integer = 0
    Dim Pointer2 As Integer = 0
    Private Function GetDigit(ByVal Digit)
        Select Case Val(Digit)
            Case 1 : GetDigit = "Un"
            Case 2 : GetDigit = "Dos"
            Case 3 : GetDigit = "Tres"
            Case 4 : GetDigit = "Cuatro"
            Case 5 : GetDigit = "Cinco"
            Case 6 : GetDigit = "Seis"
            Case 7 : GetDigit = "Siete"
            Case 8 : GetDigit = "Ocho"
            Case 9 : GetDigit = "Nueve"
            Case Else : GetDigit = ""
        End Select
    End Function

    '*********************************************
    ' Convierte números de 10 a 99 a texto. *
    '*********************************************
    Private Function GetTens(ByVal TensText)
        Dim Result As String

        Result = ""           'anula el valor temporal de la funcion
        If Val(Microsoft.VisualBasic.Left(TensText, 1)) = 1 Then   ' si el valor esta entre 10-19
            Select Case Val(TensText)
                Case 10 : Result = "Diez"
                Case 11 : Result = "Once"
                Case 12 : Result = "Doce"
                Case 13 : Result = "Trece"
                Case 14 : Result = "Catorce"
                Case 15 : Result = "Quince"
                Case 16 : Result = "Dieciseis"
                Case 17 : Result = "Diecisiete"
                Case 18 : Result = "Dieciocho"
                Case 19 : Result = "Diecinueve"
                Case Else
            End Select
        Else    ' Si el valor esta entre 20-99
            Select Case Val(Microsoft.VisualBasic.Left(TensText, 1))
                Case 2 : Result = "Veinte "
                Case 3 : Result = "Treinta "
                Case 4 : Result = "Cuarenta "
                Case 5 : Result = "Cincuenta "
                Case 6 : Result = "Sesenta "
                Case 7 : Result = "Setenta "
                Case 8 : Result = "Ochenta "
                Case 9 : Result = "Noventa "
                Case Else
            End Select
            Result = Result & GetDigit _
       (Microsoft.VisualBasic.Right(TensText, 1))  'Retrae el lugar
        End If
        GetTens = Result
    End Function
    '*******************************************
    ' Convierte los números de 100-999 a texto *
    '*******************************************
    Private Function GetHundreds(ByVal MyNumber)
        Dim Result As String
        Dim Quinien As String
        Dim Quinien2 As String

        If Val(MyNumber) = 0 Then Exit Function
        MyNumber = Microsoft.VisualBasic.Right("000" & MyNumber, 3)

        'Converte el lugar de las centenas
        Quinien2 = " Ciento "
        If Mid(MyNumber, 1, 1) <> "0" Then
            Quinien = GetDigit(Mid(MyNumber, 1, 1))
            If Quinien = "Cinco" Then
                Quinien = "Quinientos "
                Quinien2 = ""
            End If
            If Quinien = "Uno" Then
                Quinien = ""
                Quinien2 = "Ciento "
            End If
            If Quinien = "Nueve" Then
                Quinien = "Nove"
                Quinien2 = "cientos "
            End If
            If Quinien = "Siete" Then
                Quinien = "Sete"
                Quinien2 = "cientos "
            End If
            Result = Quinien & Quinien2 ' aca le agrega al numero la palabra
        End If

        'Convierte el lugar de los miles
        If Mid(MyNumber, 2, 1) <> "0" Then
            Quinien = GetTens(Mid(MyNumber, 2))
            If Pointer1 = 0 Then
                Pointer1 = 1
                Quinien = Replace(Quinien, " ", " y ")
            End If

            Result = Result & Quinien
        Else
            Quinien = GetDigit(Mid(MyNumber, 3))
            Result = Result & Quinien
        End If

        GetHundreds = Result
    End Function
    '****************
    ' Function Principal *
    '****************
    Public Function SpellNumber(ByVal MyNumber)
        Dim OtroNumero As String = MyNumber
        Dim Pesos, Centavos, Temp As String
        Dim DecimalPlace, Count
        Dim Original As Double = Val(MyNumber)

        Dim Place(9) As String
        Place(2) = " Mil "
        Place(3) = " Millones "
        Place(4) = " Billones "
        Place(5) = " Trillones "

        ' String representa la cantidad
        MyNumber = Trim(Str(MyNumber))

        ' el lugar de la posicion decimal ) si ninguno
        DecimalPlace = InStr(MyNumber, ".")

        'Convierte Centavos and set MyNumber a la cantidad en pesos
        If DecimalPlace > 0 Then
            OtroNumero = Microsoft.VisualBasic.Left(MyNumber, DecimalPlace - 1)
            Centavos = GetTens(Microsoft.VisualBasic.Left(Mid(MyNumber, DecimalPlace + 1) & "00", 2))
            MyNumber = Trim(Microsoft.VisualBasic.Left(MyNumber, DecimalPlace - 1))
        Else
            OtroNumero = MyNumber
        End If

        Dim enta, hacer As String
        Dim BuscaEspacio As Integer
        enta = CStr(MyNumber)
        hacer = ""
        If enta.Length = 2 And (Microsoft.VisualBasic.Right(enta, 1) = "0") Then
            hacer = "cero"
        End If

        Count = 1
        Do While MyNumber <> ""
            Temp = GetHundreds(Microsoft.VisualBasic.Right(MyNumber, 3))
            If Temp <> "" Then Pesos = Temp & Place(Count) & Pesos
            If Len(MyNumber) > 3 Then
                If Len(OtroNumero) = 4 And Microsoft.VisualBasic.Left(OtroNumero, 1) = "1" Then
                    Pointer2 = 1
                End If
                MyNumber = Microsoft.VisualBasic.Left(MyNumber, Len(MyNumber) - 3)
            Else
                MyNumber = ""
            End If
            Count = Count + 1
        Loop

        If Pointer2 = 1 Then
            Pesos = Microsoft.VisualBasic.Right(Pesos, Len(Pesos) - 3)
        End If

        If hacer = "cero" Then
            BuscaEspacio = InStr(Pesos, " ")
            If BuscaEspacio > 0 Then
                Pesos = Microsoft.VisualBasic.Left(Pesos, BuscaEspacio - 1)
            End If
        End If


        Select Case Pesos.Trim
            Case ""
                Pesos = "Ningún Peso"
            Case "One"
                Pesos = "Un peso"
                '    MsgBox("sopas")
            Case "Un"
                Pesos = "un peso"
            Case "Un Ciento"
                Pesos = "cien pesos"
            Case Else
                Pesos = Pesos.Replace("Un", "").Trim & " pesos"
        End Select
        SpellNumber = (Pesos & " " & Format((Original - Int(Original)) * 100, "00") & "/100").ToUpper
    End Function

End Module
