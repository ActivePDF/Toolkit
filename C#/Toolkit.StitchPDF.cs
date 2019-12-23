using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string inputFile = $"{strPath}Toolkit.Input.pdf";

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
                // Retrieves the number of pages for the specified PDF file.
                int numPages = toolkit.NumPages(inputFile);
                if (numPages == 0)
                {
                    WriteResult($"Error getting input file page count: {numPages.ToString()}", toolkit);
                    return;
                }

                // The number of pages from the input PDF to place per row.
                float pagesPerRow = Convert.ToSingle(Math.Ceiling(Math.Sqrt((double)numPages)));

                // Close the input file before creating a new document.
                toolkit.CloseInputFile();

                // Create the new PDF file
                int result = toolkit.OpenOutputFile($"{strPath}Toolkit.StitchPDF.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Using the default PDF width and height of 612/792
                float pageWidth = 612.0f, pageHeight = 792.0f;

                // The width and height of each page from the original PDF
                // added to the output file.
                float width = pageWidth / pagesPerRow, height = pageHeight / pagesPerRow;

                // The rows of images from the original PDF added to the new
                // document.
                int numRows = Convert.ToInt32(Math.Ceiling(pageHeight / height));

                for (int i = 1; i < numRows; ++i)
                {
                    for (int j = 0; j < pagesPerRow; ++j)
                    {
                        // Add the page from the original PDF to the output.
                        toolkit.StitchPDF(
                        FileName: inputFile,
                        PageNumber: i + j,
                        X: width * j,
                        Y: pageHeight - (height * i),
                        Width: width,
                        Height: height,
                        Rotation: 0);
                    }
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
