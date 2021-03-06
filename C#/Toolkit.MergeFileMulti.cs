
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

                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.MergeMultipleFiles.pdf");
                if (result == 0)
                {
                    // Set whether the fields should be read only in the output PDF
                    // 0 leave fields as they are, 1 mark all fields as read-only
                    // Fields set with SetFormFieldData will not be effected
                    toolkit.ReadOnlyOnMerge = 1;

                    // MergeFile is the equivalent of OpenInputFile and CopyForm
                    // Merge the cover page (0 for all pages)
                    result = toolkit.MergeFile(
                        FileName: $"{strPath}Toolkit.Input.pdf",
                        StartPage: 0,
                        EndPage: 0);
                    if (result != 1)
                    {
                        WriteResult($"Error merging first file: {result}", toolkit);
                        return;
                    }

                    // Merge the second PDF
                    result = toolkit.MergeFile(
                        FileName: $"{strPath}Toolkit.FormsInput.pdf",
                        StartPage: 0,
                        EndPage: 0);
                    if (result != 1)
                    {
                        WriteResult($"Error merging second file: {result}", toolkit);
                        return;
                    }

                    // Merge the third PDF
                    result = toolkit.MergeFile(
                        FileName: $"{strPath}Toolkit.DBTemplate.pdf",
                        StartPage: 0,
                        EndPage: 0);
                    if (result != 1)
                    {
                        WriteResult($"Error merging third file: {result}", toolkit);
                        return;
                    }

                    // Close the new file to complete PDF creation
                    toolkit.CloseOutputFile();
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
