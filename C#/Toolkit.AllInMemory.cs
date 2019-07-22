
using System;
using System.IO;
using System.Text;

namespace ToolkitExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Simulate the intput byte array by opening a file into memory.
            byte[] inputPDF = File.ReadAllBytes($"{strPath}Toolkit.Input.pdf");

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
            {
                // Create the new PDF file in memory
                int result = toolkit.OpenOutputFile("MEMORY");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Set the input byte array
                toolkit.InputByteArray = inputPDF;

                // Open the input byte array
                result = toolkit.OpenInputFile("MEMORY");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = toolkit.CopyForm(0, 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}", toolkit);
                    return;
                }

                // Close the new file to complete PDF creation
                toolkit.CloseOutputFile();

                // Here is the output byte stream.
                string outputPDF = toolkit.OutputByteStream;

                // Or save the output to disk
                toolkit.SaveMemoryToDisk($"{strPath}Toolkit.AllInMemory.pdf");
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
