' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set oFile = FSO.GetFile(strPath & "Toolkit.Input.pdf")
Set FSO = Nothing

' Instantiate Toolkit Object
Set oTK = CreateObject("APToolkit.Object")

' Simulate the intput byte array by opening a file into memory.
intResult = oTK.OpenOutputFile("MEMORY")
If intResult = 0 Then
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then
        intResult = oTK.CopyForm(0, 0)
        If intResult <> 1 Then
            WriteResult "CopyForm", intResult
        End If
        oTK.CloseOutputFile
        inputFileStream = oTK.OutputByteStream
    End If
End If

' Create the new PDF file
intResult = oTK.OpenOutputFile("MEMORY")
If intResult = 0 Then

    ' Set the input byte array
    oTK.InputByteStream = inputFileStream

    ' Open the template PDF
    intResult = oTK.OpenInputFile("MEMORY")
    If intResult = 0 Then
        ' Copy the template (with any changes) to the new file
        ' Start page and end page, 0 = all pages 
        intResult = oTK.CopyForm(0, 0)
        If intResult <> 1 Then
            WriteResult "CopyForm", intResult
        End If
    Else
        WriteResult "OpenInputFile", intResult
    End If
Else
    WriteResult "OpenOutputFile", intResult
End If

' Close the new file to complete PDF creation
oTK.CloseOutputFile

' Here is the output byte stream.
outputPDF = oTK.OutputByteStream

' Or save the output to disk
oTK.SaveMemoryToDisk strPath & "Toolkit.AllInMemory.pdf"

' Release Objects
Set oFile = Nothing
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