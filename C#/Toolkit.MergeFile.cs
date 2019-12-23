
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
                // Here you can place any code that will alter the output file
                // Such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile($"{strPath}Toolkit.MergeFile.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Merge the input file
                result = toolkit.MergeFile($"{strPath}Toolkit.Input.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging first input file: {result.ToString()}", toolkit);
                }

                // Merge the input file
                result = toolkit.MergeFile($"{strPath}Toolkit.Input.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging second input file: {result.ToString()}", toolkit);
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
