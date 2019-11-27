
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
                short result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result == 0)
                {
                    // Get the input document page count.
                    int pageCount = toolkit.NumPages("");

                    // Get the Redactor object from Toolkit
                    APToolkitNET.Rasterizer rasterizer = toolkit.GetRasterizer();

                    // Scale the output image
                    rasterizer.ScaleX = 0.5f;
                    rasterizer.ScaleY = 0.75f;

                    for (int currentPage = 1; currentPage <= pageCount; currentPage++)
                    {
                        string outputFile = $"{strPath}ScaleImage.{currentPage}.jpg";
                        if (!rasterizer.ToImage(sFileName: $"{outputFile}", eImageType: APToolkitNET.APImageType.JPEG, currentPage))
                        {
                            WriteResult($"Error writing image file to: {outputFile}", toolkit);
                        }
                        else
                        {
                            Console.WriteLine($"Creating image file: {outputFile}");
                        }
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
