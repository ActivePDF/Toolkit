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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.SetPDFView.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then
        ' Get the reference to the InitialViewInfo object
        Set oVI = oTK.GetInitialViewInfo

        ' Options for viewer window
        oVI.CenterWindow = True
        oVI.FullScreen = False
        oVI.ResizeWindow = True
        oVI.Show = 1

        ' Show or hide UI elements of the viewer
        oVI.HideMenuBar = True
        oVI.HideToolBars = True
        oVI.HideWindowControls = True
        oVI.NavigationTab = 0

        ' Page settings
        oVI.Magnification = 11
        oVI.OpenToPage = 2
        oVI.PageLayout = 2

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