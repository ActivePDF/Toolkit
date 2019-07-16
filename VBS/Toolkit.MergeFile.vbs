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
If intResult = 0 Then
    ' Set whether the fields should be read only in the output PDF
    ' 0 leave fields as they are, 1 mark all fields as read-only
    ' Fields set with SetFormFieldData will not be effected
    oTK.ReadOnlyOnMerge = 1

    ' Merge the first PDF
    intResult = oTK.MergeFile(strPath & "Toolkit.Input.pdf", 0, 0)
    If intResult <= 0 Then
        WriteResult "MergeFile", intResult
    End If
Else
    WriteResult "OpenOutputFile", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile 

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