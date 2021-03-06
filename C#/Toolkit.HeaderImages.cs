
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
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit(CoreLibPath: toolkitPath))
            {
                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.HeaderImages.pdf");
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

                // Use the Header Image properties to add some images to the footer
                result = toolkit.SetHeaderImage(
                    ImageFileName: $"{strPath}Toolkit.Input.bmp",
                    X: 10.0f,
                    Y: 692.0f,
                    Width: 200.0f,
                    Height: 100.0f,
                    PersistRatio: false);
                if (result != 1)
                {
                    WriteResult($"Error adding image header (bmp): {result.ToString()}", toolkit);
                    return;
                }

                result = toolkit.SetHeaderJPEG(
                    FileName: $"{strPath}Toolkit.Input.jpg",
                    X: 20.0f,
                    Y: 592.0f,
                    Width: 200.0f,
                    Height: 100.0f,
                    PersistRatio: false);
                if (result != 1)
                {
                    WriteResult($"Error adding image header (jpg): {result.ToString()}", toolkit);
                    return;
                }

                result = toolkit.SetHeaderTIFF(
                    FileName: $"{strPath}Toolkit.Input.tif",
                    X: 30.0f,
                    Y: 492.0f,
                    Width: 200.0f,
                    Height: 100.0f,
                    PersistRatio: false);
                if (result != 1)
                {
                    WriteResult($"Error adding image header (tif): {result.ToString()}", toolkit);
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
