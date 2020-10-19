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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.HeaderImages.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then        
        ' Use the Header Image properties to add some images to the footer
        intResult = oTK.SetHeaderImage(strPath & "Toolkit.Input.bmp", 10.0, 692.0, 200.0, 100.0, False)
		If intResult <> 1 Then
		    WriteResult "SetHeaderImage", intResult
		End If
		
        intResult = oTK.SetHeaderJPEG(strPath & "Toolkit.Input.jpg", 20.0, 592.0, 200.0, 100.0, False)
		If intResult <> 1 Then
		    WriteResult "SetHeaderJPEG", intResult
		End If
		
        intResult = oTK.SetHeaderTIFF(strPath & "Toolkit.Input.tif", 30.0, 492.0, 200.0, 100.0, False)
		If intResult <> 1 Then
		    WriteResult "SetHeaderTIFF", intResult
		End If

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

Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo(errorString)
  WScript.Quit
End Sub