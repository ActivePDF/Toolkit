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
strFieldNames = ""
intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
If intResult = 0 Then
	Set oFields = oTK.GetInputFields()
	For i = 0 To oFields.Count - 1
	   Set oFieldInstances = ofields.Item(i)
	   For j = 0 To oFieldInstances.Count - 1
	       strFieldNames = strFieldNames & vbCrLf & oFieldInstances.Item(j).Name
	   Next
	Next
	Wscript.Echo("Fields: " & strFieldNames)
Else
	WriteResult "OpenInputFile", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

' Release Object
Set oTK = Nothing

MsgBox "Success!"

' Error Handling
Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo(errorString)
  WScript.Quit
End Sub