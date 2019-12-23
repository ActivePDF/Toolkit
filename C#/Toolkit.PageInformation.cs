
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
                // Get the page count of the PDF
                int result = toolkit.OpenInputFile(
                    InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Get the rotation of the page
                // Note: there is no need to open an input file as NumPages
                // opened the PDF
                result = toolkit.GetInputPageRotation(PageNumber: 1);
                Console.WriteLine($"Page Rotation: {result}");

                // Close the input file
                toolkit.CloseInputFile();

                // Load the page 1 details of the PDF
                toolkit.GetBoundingBox(
                    FileName: $"{strPath}Toolkit.Input.pdf", PageNbr: 1);

                // Get the Page Width and Height for page one
                Console.WriteLine($"Page Width: {toolkit.BoundingBoxWidth}");
                Console.WriteLine($"Page Height: {toolkit.BoundingBoxHeight}");

                // Get the top left coordinates of the bounding box
                Console.WriteLine($"Page Top Left Coordinates: {toolkit.BoundingBoxLeft}, {toolkit.BoundingBoxTop}");

                // Close the new file to complete PDF creation
                toolkit.CloseInputFile();
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
