' Copyright (c) 2019 ActivePDF, Inc.
Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

numPages = oTK.NumPages(strPath & "Toolkit.Input.pdf")
If numPages = 0 Then
	WriteResult "NumPages", intResult
End If

' The number of pages from the input PDF to place per row.
pagesPerRow = Round(Sqr(numPages) + 0.5)

oTK.CloseInputFile

' Create the new PDF file
intResult = oTK.OpenOutputFile(strPath & "Toolkit.StitchPDF.pdf")
If intResult = 0 Then
    ' Using the default PDF width and height of 612/792
	pageWidth = 612.0
	pageHeight = 792.0

	' The width and height of each page from the original PDF
	' added to the output file.
	width = pageWidth / pagesPerRow
	height = pageHeight / pagesPerRow

	' The rows of images from the original PDF added to the new
	' document.
	numRows = Round((pageHeight / height) + 0.5)
		
	For i = 1 To numRows
		For j = 0 To pagesPerRow
			' Add the page from the original PDF to the output.		
			oTK.StitchPDF strPath & "Toolkit.Input.pdf", i + j, width * j, pageHeight - (height * i), width, height, 0
		Next
	Next
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