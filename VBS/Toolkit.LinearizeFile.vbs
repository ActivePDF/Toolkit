' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Check if the input file is linearized
result = oTK.IsFileLinearized(strPath & "Toolkit.Input.pdf")
If result = False Then
    ' Linearize the input file
    result = oTK.LinearizeFile(strPath & "Toolkit.Input.pdf", strPath & "Toolkit.LinearizeFile.pdf", "")
    If result <> 0  Then
        WriteResult "LinearizeFile", intResult
    End If
Else
    Wscript.Echo "Input file is already linearized."
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