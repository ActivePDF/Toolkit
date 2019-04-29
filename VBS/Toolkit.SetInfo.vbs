' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.SetInfo.pdf")
If intResult <> 0 Then
  ErrorHandler "OpenOutputFile", intResult
End If

' Open the template PDF
intResult = oTK.OpenInputFile(strPath & "input.pdf")
If intResult <> 0 Then
  ErrorHandler "OpenInputFile", intResult
End If

' Set the PDF metadata for the output PDF
oTK.SetInfo "Test PDF", "Testing", "John Doe", "test, pdf, sample"

' Copy the template (with any changes) to the new file
intResult = oTK.CopyForm(0, 0)
If intResult <> 1 Then
  ErrorHandler "CopyForm", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

' Release Object
Set oTK = Nothing

' Process Complete
Wscript.Echo("Success!")

' Error Handling
Sub ErrorHandler(method, outputCode)
  Wscript.Echo("Error in " & method & ": " & outputCode)
End Sub