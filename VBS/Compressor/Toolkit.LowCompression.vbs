' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Get the Compressor object
Set oCompressor = oTK.GetCompressor

' Compresses images in the output PDF with the default settings.
oCompressor.CompressImages = True

' Compresses eligible objects in the output PDF, which include
' page objects and fonts. Streams (including content, text,
' images, and data) are not affected.
oCompressor.CompressObjects = True

' Compress images to a particular quality, used only with lossy image
' compression. Ranges from 1 to 100 indicate the result image quality.
' A lower value creates an image of lower PPI and smaller file size,
' while a greater value creates images of better quality but larger
' file size. The default is 20.
oCompressor.CompressionQuality = 80

' Images of DPI greater or equal to the TriggerDPI will be downsampled
' to the TargetDPI
oCompressor.TargetDPI = 150.0
oCompressor.TriggerDPI = 300.0

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.LowCompression.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
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