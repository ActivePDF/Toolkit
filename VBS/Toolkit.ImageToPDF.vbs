' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Any supported image file can be converted to PDF with ImageToPDF
intResult = oTK.ImageToPDF(strPath & "IMG.jpg", strPath & "Toolkit.ImageToPDF.pdf")
If intResult <> 1 Then
  ErrorHandler "ImageToPDF", intResult
End If

' Release Object
Set oTK = Nothing

' Process Complete
Wscript.Echo("Success!")

' Error Handling
Sub ErrorHandler(method, outputCode)
  Wscript.Echo("Error in " & method & ": " & outputCode)
End Sub