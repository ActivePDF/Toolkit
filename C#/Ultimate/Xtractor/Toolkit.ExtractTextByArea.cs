
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

                    // If you know where on the page you want to look, you can
                    // restrict extractions to just that area. In this example,
                    // we'll just a square 200 units  on each side and pull
                    // from roughly the middle of the page.
                    string extractedText = extractor.ExtractTextByArea(
                        Page: 1,
                        Rect: new System.Drawing.RectangleF(10.0f, 200.0f, 200.0f, 200.0f));

                    Console.WriteLine($"Extracted Text: {extractedText}");

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
