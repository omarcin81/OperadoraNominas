Module GenPass
    Dim rNum As New Random(100)
    Dim rLowerCase As New Random(500)
    Dim rUpperCase As New Random(50)
    Dim psw As String
    Dim RandomSelect As New Random(50)

    Public Function Gen_Psw(ByVal Lenght As Integer, Optional ByVal Reset As Boolean = False) As String
        Dim i As Integer
        Dim CNT(2) As Integer
        Dim Char_Sel(2) As String
        Dim iSel As Integer

        If Reset = True Then
            psw = ""
        End If
        For i = 1 To Lenght
            CNT(0) = rNum.Next(48, 57)
            CNT(1) = rLowerCase.Next(65, 90)
            CNT(2) = rUpperCase.Next(97, 122)
            Char_Sel(0) = System.Convert.ToChar(CNT(0)).ToString
            Char_Sel(1) = System.Convert.ToChar(CNT(1)).ToString
            Char_Sel(2) = System.Convert.ToChar(CNT(2)).ToString
            iSel = RandomSelect.Next(0, 3)
            psw &= Char_Sel(iSel)
            If Reset = True Then

                psw.Replace(psw, Char_Sel(iSel))
            End If
        Next
        Return psw
    End Function
End Module
