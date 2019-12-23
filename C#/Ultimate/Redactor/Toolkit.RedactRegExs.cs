
using System;
using System.Text;

namespace ToolkitUltimate_Examples
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
            string toolkitPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\ActivePDF\Toolkit Ultimate\bin\x64";

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit(toolkitPath))
            {
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.RedactRegExs.pdf");
                if (result == 0)
                {
                    // Open the input PDF
                    result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                    if (result == 0)
                    {
                        // Get the Redactor object from Toolkit
                        APToolkitNET.Redactor redactor = toolkit.GetRedactor();

                        string[] patterns = new string[]
                        {
                            // Pattern for all words starting with an 's' or
                            // 'S'
                            @"\b[s|S](\S+)\s?",
                            // Pattern for all words starting with an 'a' or
                            // 'A'
                            @"\b[a|A](\S+)\s?"
                        };

                        // Redact the expression pattern from the input PDF.
                        // You may also redact individual pages setting the
                        // page function argument, the default is all pages.
                        redactor.RedactRegexs(regexs: patterns);

                        // Call the Redactor Apply method to execute the
                        // redaction process. All Toolkit methods normally
                        // called between OpenInputFile and CopyForm
                        // (PrintText, PrintImage etc.) must be after
                        // Redactor.Apply
                        redactor.Apply();

                        result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
                        if (result != 1)
                        {
                            WriteResult("CopyForm Failed", toolkit);
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
                else
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
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
