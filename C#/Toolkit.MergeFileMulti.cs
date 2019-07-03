
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

                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.MergeMultipleFiles.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Set whether the fields should be read only in the output PDF
                // 0 leave fields as they are, 1 mark all fields as read-only
                // Fields set with SetFormFieldData will not be effected
                toolkit.ReadOnlyOnMerge = 1;

                // MergeFile is the equivalent of OpenInputFile and CopyForm
                // Merge the cover page (0 for all pages)
                result = toolkit.MergeFile($"{strPath}Toolkit.Input.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging first file: {result}", toolkit);
                }

                // Merge the second PDF
                result = toolkit.MergeFile($"{strPath}Toolkit.FormsInput.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging second file: {result}", toolkit);
                }

                // Merge the third PDF
                result = toolkit.MergeFile($"{strPath}Toolkit.DBTemplate.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging third file: {result}", toolkit);
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
