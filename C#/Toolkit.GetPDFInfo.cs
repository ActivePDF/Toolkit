
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
                // Specify the file to retrieve information about
                toolkit.GetPDFInfo($"{strPath}Toolkit.Input.pdf");

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
