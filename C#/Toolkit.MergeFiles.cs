
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
                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.MergeFiles.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                string[] pdfFiles = System.IO.Directory.GetFiles(strPath, "*.pdf");
                result = toolkit.MergeFiles(Files: pdfFiles);
                if (result != 1)
                {
                    WriteResult($"MergeFiles failed: {result}", toolkit);
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
