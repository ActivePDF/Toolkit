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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.NewPDF.pdf")
If intResult = 0 Then
    ' Each time a new page is required call NewPage
    oTK.NewPage

    ' Get the current version of Toolkit and save it to print on
    ' the PDF
    toolkitVersion = oTK.ToolkitVersion

    ' Text can be added onto the new page with
    ' SetFont, PrintText and PrintMultilineText functions
    oTK.SetFont "Helvetica", 24
    oTK.PrintText 72.0, 720.0, toolkitVersion

    ' Images can be added onto the new page with PrintImage,
    ' PrintJPEG and PrintTIFF
    oTK.PrintJPEG strPath + "Toolkit.Input.jpg", 72.0, 300.0, _
        468.0, 400.0, true, 0
Else
    WriteResult "OpenOutputFile", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

' Release Object
Set oTK = Nothing

Wscript.Echo "Success!"

Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo(errorString)
  WScript.Quit
End Sub