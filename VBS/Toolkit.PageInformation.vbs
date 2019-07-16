' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

' Get the page count of the PDF
result = oTK.NumPages(strPath & "Toolkit.Input.pdf")
If result < 1 Then
    WriteResult "NumPages", result
End If

' Get the rotation of the page
' Note: there is no need to open an input file as NumPages
' opened the PDF
result = oTK.GetInputPageRotation(1)
details = "Page Rotation: " & result & vbCrLf

' Close the input file
oTK.CloseInputFile

' Load the page 1 details of the PDF
oTK.GetBoundingBox strPath & "Toolkit.Input.pdf", 1

' Get the Page Width and Height for page one
details = details & "Page Width: " & oTK.BoundingBoxWidth & vbCrLf
details = details & "Page Height: " & oTK.BoundingBoxHeight & vbCrLf

' Get the top left coordinates of the bounding box
details = details & "Page Top Left Coordinates: " & oTK.BoundingBoxLeft & "," & oTK.BoundingBoxTop

' Close the new file to complete PDF creation
oTK.CloseInputFile

' Release Object
Set oTK = Nothing

Wscript.Echo details

' Error Handling
Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo errorString
  WScript.Quit
End Sub