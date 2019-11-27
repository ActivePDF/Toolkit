
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
                // Open the input PDF
                int result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result == 0)
                {
                    // Get the number of pages in the input PDF.
                    int numPages = toolkit.NumPages(FileName: "");

                    // Get the Extractor object from Toolkit
                    APToolkitNET.Extractor extractor = toolkit.GetExtractor();

                    // Find the coordinates, width, and height of all instances
                    // of the search text in the document. You may also use a
                    //regular expression as the search input.
                    APToolkitNET.APRectangle[] locations =
                        extractor.FindText(Text: "Toolkit");

                    foreach (var location in locations)
                    {
                        Console.WriteLine($"Search String Located.");
                        Console.WriteLine($"\tPage: {location.Page}");
                        Console.WriteLine($"\tWidth: {location.Location.Width}");
                        Console.WriteLine($"\tHeight: {location.Location.Height}");
                        Console.WriteLine($"\tCoordinates: {location.Location.X}, {location.Location.Y}");
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
