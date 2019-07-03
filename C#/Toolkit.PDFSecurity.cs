
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
                // Add encryption to the output PDF
                // 'DEMO' is appended to the start of the password with the evaluation version
                toolkit.SetPDFSecurity(5, "UserPassword", "OwnerPassword", true, false, true, false, true, false, true, false);

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.PDFSecurity.pdf");
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
