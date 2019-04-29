
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
                // Any supported image file can be converted to PDF with ImageToPDF
                int result = oTK.ImageToPDF(strPath + "IMG.jpg", strPath + "Toolkit.ImageToPDF.pdf");
                if (result != 1)
                {
                    WriteResult($"Error converting image to PDF: {result.ToString()}");
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
