
using System;
using System.Text;


namespace ToolkitExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Starting with Toolkit version 10 native DLLs are no longer
            // copied to the system folder. The Toolkit constructor must
            // be called with the path to the native DLLs or place them
            // in your applications working directory. This example
            // assumes they are located in the default installation folder.
            // (Use x86 in the path for 32b applications)
            string toolkitPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\ActivePDF\Toolkit\bin\x64";

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit(toolkitPath))
            {
                // Create the new PDF file
                int result = toolkit.OpenOutputFile($"{strPath}Toolkit.BasicBarcode.pdf");
                if (result == 0)
                {
                    // Add a page to the new PDF for the barcode
                    toolkit.NewPage();

                    APToolkitNET.BarCode barcode = new APToolkitNET.BarCode(toolkitPath);

                    // Set the encoded value for the barcode.
                    barcode.Value = "*AB-A001-001*";

                    // Specifies the symbology or barcode format to generate.
                    // Supported Formats:
                    // http://documentation.activepdf.com/Toolkit/Toolkit_API/Content/4_b_barcode_appendix/barcode_format_codes.html
                    barcode.Symbology = 0;

                    // Use PrintBarcode() to add the barcode to the new PDF
                   toolkit.PrintBarcode(
                        Barcode: barcode,
                        X: 72,
                        Y: 576,
                        Width: 360,
                        Height: 144,
                        PageNumber: 0);

                    // Close the new file to complete PDF creation
                    toolkit.CloseOutputFile();
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