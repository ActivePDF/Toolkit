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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.SignPDF.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then
        ' Find certificate, if it doesn't exist it will be created
        ' If you have an existing signature you can find it with
        ' FindCertificate and remove the portion of code that creates
        ' the certificate
        certificateID = oTK.FindCertificate("John Doe", "My", 0)
        If certificateID < 1 Then
            ' Certificate not found, create a certificate with Toolkit
            WScript.Echo "Certificate not found, creating."
            createCertResult = oTK.CreateCertificate("John Doe", "Management", "Doe Enterprises", "Mission Viejo", "CA", "US", "john@doee.com", 0, "My", 365, 0, "", "")
            If createCertResult = 1 Then
                ' New certificate created, find it for use
                WScript.Echo "New certificate created"
                certificateID = oTK.FindCertificate("John Doe", "My", 0)
                If certificateID < 1 Then
                    WriteResult "FindCertificate", certificateID
                End If
            Else
                WriteResult "CreateCertificate", createCertResult
            End If
        End If

        ' Invisibly sign the output file after any creation, merge or append operation.
        oTK.SignOutputFile certificateID, "Mission Viejo", "Testing Toolkit PDF Signing.", "http:'www.activepdf.com", 0

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