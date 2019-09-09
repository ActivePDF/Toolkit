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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.BasicBarcode.pdf")
If intResult = 0 Then
    ' Create the Barcode object
    Set oBC = CreateObject("APToolkit.Barcode")

    ' Add a page to the new PDF for the barcode
    oTK.NewPage

	' Specifies the symbology or barcode format to generate.
    ' Supported Formats:
    ' http://documentation.activepdf.com/Toolkit/Toolkit_API/Content/4_b_barcode_appendix/barcode_format_codes.html
    oBC.Symbology = 0

    ' Please note that different barcodes have different value requirements
    oBC.Value = "*AB-A001-001*"
	
	' Print barcode to the page
    oTK.PrintImage oBC.AsString, 72, 576, 360, 144, 1, 0
Else
    WriteResult "OpenOutputFile", intResult
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