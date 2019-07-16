' Copyright (c) 2019 ActivePDF, Inc.

Dim FSO, strPath, intResult

' Get current path
Set FSO = CreateObject("Scripting.FileSystemObject")
strPath = FSO.GetFile(Wscript.ScriptFullName).ParentFolder & "\"
Set FSO = Nothing

' Instantiate Object
Set oTK = CreateObject("APToolkit.Object")

oTK.GetPDFInfo strPath & "Toolkit.Input.pdf"

infoString = infoString & "Author: " & oTK.Author & vbCrLf
infoString = infoString & "Title: " & oTK.Title & vbCrLf
infoString = infoString & "Subject: " & oTK.Subject & vbCrLf
infoString = infoString & "Keywords: " & oTK.Keywords & vbCrLf
infoString = infoString & "Producer: " & oTK.Producer & vbCrLf
infoString = infoString & "Creator: " & oTK.Creator & vbCrLf
infoString = infoString & "Creation Date: " & oTK.CreateDate & vbCrLf
infoString = infoString & "Modification Date: " & oTK.ModDate & vbCrLf

' Close the new file to complete PDF creation
oTK.CloseInputFile 

' Release Object
Set oTK = Nothing

Wscript.Echo infoString