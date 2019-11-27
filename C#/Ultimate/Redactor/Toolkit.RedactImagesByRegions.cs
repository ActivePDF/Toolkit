
using System;
using System.Text;

namespace ToolkitUltimate_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
            {
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.RedactImagesByRegion.pdf");
                if (result == 0)
                {
                    // Open the input PDF
                    result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                    if (result == 0)
                    {
                        // Get the Redactor object from Toolkit
                        APToolkitNET.Redactor redactor = toolkit.GetRedactor();

                        // Regions within the PDF to redact images from.
                        APToolkitNET.Rectangle[] areaRedactions = new APToolkitNET.Rectangle[]
                        {
                            new APToolkitNET.Rectangle(306.0f, 396.0f, 306.0f, 396.0f)
                        };

                        // Redact all images from the regions specified.
                        redactor.RedactRegion(mode: APToolkitNET.Redactor.RedactionMode.ImageByRegion, rect: areaRedactions);

                        // Call the Redactor Apply method to execute the
                        // redaction process. All Toolkit methods normally
                        // called between OpenInputFile and CopyForm
                        // (PrintText, PrintImage etc.) must be after
                        // Redactor.Apply
                        redactor.Apply();

                        result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
                        if (result != 1)
                        {
                            WriteResult("CopyForm Failed", toolkit);
                        }

                        // Close the new file to complete PDF creation
                        toolkit.CloseInputFile();
                    }
                    else
                    {
                        WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                        return;
                    }
                }
                else
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
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
