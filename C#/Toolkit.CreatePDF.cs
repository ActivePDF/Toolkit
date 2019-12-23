
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
                // Set the PDF page Height and Width (72 = 1")
                toolkit.OutputPageHeight = 792.0f;
                toolkit.OutputPageWidth = 612.0f;

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.NewPDF.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Each time a new page is required call NewPage
                toolkit.NewPage();

                // Get the current version of Toolkit and save it to print on
                // the PDF
                string toolkitVersion = toolkit.ToolkitVersion;

                // Text can be added onto the new page with
                // SetFont, PrintText and PrintMultilineText functions
                toolkit.SetFont(FontName: "Helvetica", FontSize: 24);
                toolkit.PrintText(X: 72.0f, Y: 720.0f, Text: toolkitVersion);

                // Images can be added onto the new page with PrintImage,
                // PrintJPEG and PrintTIFF
                toolkit.PrintJPEG(
                    FileName: strPath + "Toolkit.Input.jpg",
                    X: 72.0f,
                    Y: 300.0f,
                    Width: 468.0f,
                    Height: 400.0f,
                    PersistRatio: true,
                    PageNumber: 0);

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
