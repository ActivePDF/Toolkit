
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
                // Encrypt a PDF with the specified encryption level
                // NOTE: Evaluation keys will append 'DEMO' to the start of the password
                int result = toolkit.EncryptPDF(
                    nEncrLevel: 5,
                    InputFileName: $"{strPath}Toolkit.Input.pdf",
                    OutputFileName: $"{strPath}Toolkit.Encrypted.pdf",
                    UserPassword: "UserPassword",
                    OwnerPassword: "OwnerPassword",
                    CanPrint: true,
                    CanEdit: true,
                    CanCopy: true,
                    CanModify: true,
                    CanFillInFormFields: true,
                    CanMakeAccessible: true,
                    CanAssemble: true,
                    CanReproduce: true);
                if (result != 0)
                {
                    WriteResult($"Error encrypting PDF: {result}", toolkit);
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