
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
                // Specify the file to retrieve information about
                toolkit.GetPDFInfo(FileName: $"{strPath}Toolkit.Input.pdf");

                // Retrieve various information about the PDF
                Console.WriteLine($"Author: {toolkit.Author}");
                Console.WriteLine($"Title: {toolkit.Title}");
                Console.WriteLine($"Subject: {toolkit.Subject}");
                Console.WriteLine($"Keywords: {toolkit.Keywords}");
                Console.WriteLine($"Producer: {toolkit.Producer}");
                Console.WriteLine($"Creator: {toolkit.Creator}");
                Console.WriteLine($"Creation Date: {toolkit.CreateDate}");
                Console.WriteLine($"Modification Date: {toolkit.ModDate}");

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
