' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Any supported image file can be converted to PDF with ImageToPDF
intResult = oTK.ImageToPDF(strPath & "Toolkit.Input.jpg", strPath & "Toolkit.ImageToPDF.pdf")
If intResult <> 1 Then
  WriteResult "ImageToPDF", intResult
End If

' Release Object
Set oTK = Nothing

' Process Complete
Wscript.Echo("Success!")

Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo(errorString)
  WScript.Quit
End Sub