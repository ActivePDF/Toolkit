
using System;

namespace ToolkitExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Instantiate Object
            using (APToolkitNET.Toolkit oTK = new APToolkitNET.Toolkit())
            {
                // Here you can place any code that will alter the output file
                // Such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = oTK.OpenOutputFile($"{strPath}Toolkit.MergeFile.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}");
                    return;
                }

                // Merge the input file
                result = oTK.MergeFile($"{strPath}input.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging first input file: {result.ToString()}");
                }

                // Merge the input file
                result = oTK.MergeFile($"{strPath}input.pdf", 0, 0);
                if (result != 1)
                {
                    WriteResult($"Error merging second input file: {result.ToString()}");
                }

                // Close the new file to complete PDF creation
                oTK.CloseOutputFile();
            }

            // Process Complete
            WriteResult("Success!");
        }

        public static void WriteResult(string result)
        {
            Console.WriteLine(result);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
