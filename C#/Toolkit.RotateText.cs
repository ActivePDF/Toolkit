
using System;
using System.Text;

namespace ToolkitExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
            {
                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.RotateText.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Open the template PDF
                result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                int totalPages = toolkit.NumPages("");
                if (totalPages < 1)
                {
                    WriteResult($"Error getting page count: {result}");
                    return;
                }
                else
                {
                    // Loop through all pages of the input file
                    for (int currentPage = 1; currentPage <= totalPages; currentPage++)
                    {
                        // Get the current page width, height and rotation
                        short pageRotation = toolkit.GetInputPageRotation(PageNumber: currentPage);
                        int getBoundingBox = toolkit.GetBoundingBox(FileName: "", PageNbr: currentPage);
                        if (getBoundingBox != 0)
                        {
                            WriteResult($"Bad bounding box: {getBoundingBox}", toolkit);
                        }

                        // Set font properties and text rotation
                        toolkit.SetFont(FontName: "Helvetica", FontSize: 12, PageNumber: currentPage);
                        toolkit.SetTextColor(AmountRed: 255, AmountGreen: 0, AmountBlue: 0, AmountGrey: 0, PageNumber: currentPage);
                        toolkit.SetTextRotation(RotationAngle: pageRotation);

                        // Depending on the rotation of the page, adjust coordinates
                        // This only accounts for rotations of 0, 90, 180, 270
                        float xCoord = 72, yCoord = 24;

                        switch (pageRotation)
                        {
                            case 0:
                                break;
                            case 90:
                                xCoord = toolkit.BoundingBoxWidth - yCoord;
                                break;
                            case 180:
                                xCoord = toolkit.BoundingBoxWidth - 72;
                                yCoord = toolkit.BoundingBoxHeight - 24;
                                break;
                            case 270:
                                xCoord = yCoord;
                                yCoord = toolkit.BoundingBoxHeight - 72;
                                break;
                            default:
                                WriteResult($"Error getting page rotation: {pageRotation}", toolkit);
                                break;
                        }

                        // Add the text stamp
                        toolkit.PrintText(X: xCoord, Y: yCoord, Text: "Confidential", PageNumber: currentPage);
                    }

                    // Copy the template (with any changes) to the new file
                    // Start page and end page, 0 = all pages
                    result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
                    if (result != 1)
                    {
                        WriteResult($"Error copying file: {result.ToString()}", toolkit);
                        return;
                    }

                    // Close the new file to complete PDF creation
                    toolkit.CloseOutputFile();
                }
            }

            // Process Complete
            WriteResult("Success!");
        }

        public static void WriteResult(string result, APToolkitNET.Toolkit toolkit = null)
        {
            StringBuilder resultText = new StringBuilder();
            resultText.AppendLine(result);
            if (toolkit != null)
            {
                resultText.AppendLine($"ErrorCode: {toolkit.ExtendedErrorCode.ToString()}");
                resultText.AppendLine($"Location: {toolkit.ExtendedErrorLocation}");
                resultText.AppendLine($"Description: {toolkit.ExtendedErrorDescription}");
            }
            Console.WriteLine(resultText.ToString());
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
