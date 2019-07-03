
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
                if (!toolkit.IsFileLinearized($"{strPath}Toolkit.Input.pdf"))
                {
                    int result = toolkit.LinearizeFile($"{strPath}Toolkit.Input.pdf", $"{strPath}Toolkit.Linearized.pdf", "");
                    if (result != 0)
                    {
                        WriteResult("Failed to linearize the input file.", toolkit);
                        return;
                    }
                }
                else
                {
                    WriteResult("File already linearized.");
                    return;
                }
            }

            // Process Complete
            WriteResult("Success.");
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
