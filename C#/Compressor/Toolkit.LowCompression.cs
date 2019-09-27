
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
                // Instantiate the Compressor object
                APToolkitNET.Compressor compressor = toolkit.GetCompressor();

                // Compresses images in the output PDF.
                compressor.CompressImages = true;

                // Compresses eligible objects in the output PDF, which include
                // page objects and fonts. Streams (including content, text,
                // images, and data) are not affected.
                compressor.CompressObjects = true;

                // Compress images to a particular quality, used only with
                // lossy image compression. Ranges from 1 to 100 indicate
                // the result image quality. A lower value creates an image of
                // lower PPI and smaller file size, while a greater value
                // creates images of better quality but larger file size. The
                // default is 20.
                compressor.CompressionQuality = 80;

                // Images of DPI greater or equal to the TriggerDPI will be
                // downsampled to the TargetDPI
                compressor.TargetDPI = 150.0f;
                compressor.TriggerDPI = 300.0f;

                // Create the new PDF file
                int result = toolkit.OpenOutputFile($"{strPath}Toolkit.LowCompression.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Open the template PDF
                result = toolkit.OpenInputFile($"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = toolkit.CopyForm(0, 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}", toolkit);
                    return;
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
