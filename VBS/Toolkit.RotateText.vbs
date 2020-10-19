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
intResult = oTK.OpenOutputFile(strPath & "Toolkit.RotateText.pdf")
If intResult = 0 Then
    ' Open the template PDF
    intResult = oTK.OpenInputFile(strPath & "Toolkit.Input.pdf")
    If intResult = 0 Then        
        totalPages = oTK.NumPages("")
        If totalPages < 1 Then
            WriteResult "NumPages", totalPages
            return
        Else
            ' Loop through all pages of the input file
            For currentPage = 1 to totalPages   
                ' Get the current page width, height and rotation
                pageRotation = oTK.GetInputPageRotation(currentPage)
                getBoundingBox = oTK.GetBoundingBox("", currentPage)
                If getBoundingBox <> 0 Then
                    WriteResult"GetBoundingBox", intResult
                End If

                ' Set font properties and text rotation
                oTK.SetFont "Helvetica", 12, currentPage
                oTK.SetTextColor 255, 0, 0, 0, currentPage
                oTK.SetTextRotation pageRotation

                ' Depending on the rotation of the page, adjust coordinates
                ' This only accounts for rotations of 0, 90, 180, 270
                xCoord = 288
                yCoord = 72

                Select Case pageRotation
                    Case 0
                    Case 90
                        xCoord = oTK.BoundingBoxWidth - yCoord
                    Case 180
                        xCoord = oTK.BoundingBoxWidth - 72
                        yCoord = oTK.BoundingBoxHeight - 24
                    Case 270
                        xCoord = yCoord
                        yCoord = oTK.BoundingBoxHeight - 72
                    Case Else
                        WriteResult "PageRotation", pageRotation
                End Select

                ' Add the text stamp
                oTK.PrintText xCoord, yCoord, "Confidential", currentPage			
            Next
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

' Error Handling
Sub WriteResult(method, outputCode)
  errorString = "Error in " & method & ": " & outputCode & vbCrlf
  errorString = errorString & "Extended Error Code: " & oTK.ExtendedErrorCode & vbCrlf
  errorString = errorString & "Extended Error Location: " & oTK.ExtendedErrorLocation & vbCrlf
  errorString = errorString & "Extended Error Description: " & oTK.ExtendedErrorDescription & vbCrlf  
  Wscript.Echo errorString
  WScript.Quit
End Sub