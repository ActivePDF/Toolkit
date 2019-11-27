
using System;
using System.Text;

namespace ToolkitUltimate_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
            {
                // Open the input PDF
                int result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result == 0)
                {
                    // Get the Spooler object from Toolkit
                    APToolkitNET.Spooler spooler = toolkit.GetSpooler();

                    // Set the duplex mode, only valid if supported by the
                    // printer.
                    spooler.DuplexMode = APToolkitNET.DuplexMode.Vertical;

                    // Set the printer name
                    spooler.PrinterName = @"\\ap-dc-02\Ops - Brother HL-6180DW";

                    // Set the number of copies to print
                    spooler.Copies = 2;

                    result = spooler.PrintFile();
                    if (result != 0)
                    {
                        WriteResult($"Error printing PDF: {result}", toolkit);
                        return;
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
