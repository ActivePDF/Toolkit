
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

                // Open the template PDF
                int result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Create version two PDF with append mode enabled
                result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.Version2.pdf", Append: true);
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Stamp version number onto the first page
                toolkit.SetFont("Helvetica", 20, 1);
                toolkit.PrintText(72, 746, "Version 2", 1);

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}", toolkit);
                    return;
                }

                // Close the input file
                toolkit.CloseOutputFile();

                // Create version three PDF with append mode enabled
                result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Version2.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Create version three PDF with append mode enabled
                result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.Version3.pdf", Append: true);
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                }

                // Stamp version number onto the first page
                toolkit.SetFont("Helvetica", 20, 1);
                toolkit.PrintText(72, 720, "Version 3", 1);

                // Copy the input PDF pages to the output PDF
                result = toolkit.CopyForm(0, 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}", toolkit);
                    return;
                }

                // Close the new file to complete PDF creation
                toolkit.CloseOutputFile();

                // Get the count of versions in the PDF
                result = toolkit.GetVersionsCount(FileName: $"{strPath}Toolkit.Version3.pdf");
                if (result < 0)
                {
                    WriteResult($"Error getting version count: {result.ToString()}", toolkit);
                    return;
                }
                WriteResult($"Versions detected: {result.ToString()}", toolkit);
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
