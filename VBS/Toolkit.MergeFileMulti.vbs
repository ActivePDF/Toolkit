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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.MergeMultipleFiles.pdf")
If intResult = 0 Then
    ' Set whether the fields should be read only in the output PDF
    ' 0 leave fields as they are, 1 mark all fields as read-only
    ' Fields set with SetFormFieldData will not be effected
    oTK.ReadOnlyOnMerge = 1

    ' MergeFile is the equivalent of OpenInputFile and CopyForm
    ' Merge the cover page (0 for all pages)
    result = oTK.MergeFile(strPath & "Toolkit.Input.pdf", 0, 0)
    If result <> 1 Then
        WriteResult "Error merging first file", intResult
    End If

    ' Merge the second PDF
    result = oTK.MergeFile(strPath & "Toolkit.FormsInput.pdf", 0, 0)
    If result <> 1 Then
        WriteResult "Error merging second file", intResult
    End If

    ' Merge the third PDF
    result = oTK.MergeFile(strPath & "Toolkit.DBTemplate.pdf", 0, 0)
    If result <> 1 Then
        WriteResult "Error merging third file", intResult
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