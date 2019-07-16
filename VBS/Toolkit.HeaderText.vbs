' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Here you can place any code that will alter the output file
' Such as adding security, setting page dimensions, etc.

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.HeaderText.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then
        ' Add a 'Confidential' watermark by setting text transparency
        ' Rotation and color of the text along with the fill mode are set
        oTK.SetHeaderFont fontName, 90
        oTK.SetHeaderTextTransparency 0.6, 0.6
        oTK.SetHeaderRotation 45
        oTK.SetHeaderTextStrokeColor 255, 0, 0, 0
        oTK.SetHeaderTextFillMode 1
        oTK.SetHeaderText 154, 184, "Confidential"
        oTK.ResetHeaderTextTransparency
        oTK.SetHeaderTextFillMode 0

        ' Add a 'Top Secret' watermark by placing text in the foreground
        oTK.SetHeaderFont fontName, 72
        oTK.SetHeaderTextBackground 1
        oTK.SetHeaderTextColor 200, 200, 200, 0
        oTK.SetHeaderText 154, 300, "Top Secret"
        oTK.ResetHeaderTextColor
        oTK.SetHeaderRotation 0

        ' Add the document title to the bottom center of the page
        oTK.SetHeaderFont fontName, 12
        oTK.SetHeaderTextBackground 0
        title = "ActivePDF Toolkit"
        textWidth = oTK.GetHeaderTextWidth(title)
        oTK.SetHeaderText (612 - textWidth) / 2, 52, title

        ' Add page numbers to the bottom left of the page
        oTK.SetHeaderFont fontName, 12
        oTK.SetHeaderWPgNbr 540, 52, "Page %p", 1

        ' Add a mulitline print box for an 'approved' message in header
        oTK.SetHeaderTextFillMode 2
        oTK.SetHeaderTextColorCMYK 0, 0, 0, 20
        oTK.SetHeaderTextStrokeColorCMYK 0, 0, 0, 80
        oTK.SetHeaderMultilineText fontName, 22, 144, 766, 190, 86, "Approved on January 17th, 2021", 2
        oTK.ForceHeaderColorReset

        ' Copy the template (with any changes) to the new file
        ' Start page and end page, 0 = all pages 
        intResult = oTK.CopyForm(0, 0)
        If intResult <> 1 Then
            WriteResult "CopyForm", intResult
        End If
    Else
        WriteResult "OpenInputFile", intResult
    End If
Else
    WriteResult "OpenOutputFile", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

' Release Object
Set oTK = Nothing

Wscript.Echo "Success!"

' Error Handling
Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo errorString
  WScript.Quit
End Sub