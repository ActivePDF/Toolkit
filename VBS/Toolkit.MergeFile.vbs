' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.MergeFile.pdf")
If intResult <> 0 Then
  ErrorHandler "OpenOutputFile", intResult
End If

' Set whether the fields should be read only in the output PDF
' 0 leave fields as they are, 1 mark all fields as read-only
' Fields set with SetFormFieldData will not be effected
oTK.ReadOnlyOnMerge = 1

' MergeFile is the equivalent of OpenInputFile and CopyForm

' Merge the first PDF
intResult = oTK.MergeFile(strPath & "input.pdf", 0, 0)
If intResult <> 1 Then
  ErrorHandler "MergeFile", intResult
End If

' Merge the second PDF
intResult = oTK.MergeFile(strPath & "input.pdf", 0, 0)
If intResult <> 1 Then
  ErrorHandler "MergeFile", intResult
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