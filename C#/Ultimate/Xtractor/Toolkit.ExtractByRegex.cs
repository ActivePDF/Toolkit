using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Get the Extractor object from Toolkit
                APToolkitNET.Extractor extractor = toolkit.GetExtractor();

                // Simple regex to search for all words starting with an "S"
                Regex re = new Regex(pattern: @"\bs\S*");

                // Count instances on just the first page.
                string[] searchResults =
                    extractor.ExtractByRegex(re: re, Page: 1);
                Console.WriteLine($"{searchResults.Count()} instances found on page 1");
                foreach (string searchResult in searchResults)
                {
                    Console.WriteLine($"\t{searchResult}");
                }

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
