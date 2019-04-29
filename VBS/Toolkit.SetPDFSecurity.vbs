' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Add AES 256 bit encryption to the output PDF. Toolkit also
' supports RC4 40 bit, RC4 128 bit and AES 128 bit.
' 'DEMO' is appended to the start of the password with the evaluation version
oTK.SetPDFSecurity 5, "userpass", "ownerpass", 1, 0, 1, 0, 1, 0, 1, 0

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.SetPDFSecurity.pdf")
If intResult <> 0 Then
  ErrorHandler "OpenOutputFile", intResult
End If

' Open the template PDF
intResult = oTK.OpenInputFile(strPath & "input.pdf")
If intResult <> 0 Then
  ErrorHandler "OpenInputFile", intResult
End If

' Copy the template (with any changes) to the new file
intResult = oTK.CopyForm(0, 0)
If intResult <> 1 Then
  ErrorHandler "CopyForm", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

' Clear SetPDFSecurity options for subsequent output files
oTK.ClearPDFSecurity 

' Release Object
Set oTK = Nothing

' Process Complete
Wscript.Echo("Success!")

' Error Handling
Sub ErrorHandler(method, outputCode)
  Wscript.Echo("Error in " & method & ": " & outputCode)
End Sub