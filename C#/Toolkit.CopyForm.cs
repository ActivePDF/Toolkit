
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
                int result = oTK.OpenOutputFile($"{strPath}Toolkit.CopyForm.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}");
                    return;
                }

                // Open the template PDF
                result = oTK.OpenInputFile($"{strPath}input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}");
                    return;
                }

                // Here you can call any Toolkit functions that will manipulate
                // the input file such as text and image stamping, form filling, etc.

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = oTK.CopyForm(0, 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}");
                    return;
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
