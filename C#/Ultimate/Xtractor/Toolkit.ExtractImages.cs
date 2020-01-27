
using System;
using System.Text;
using System.Threading.Tasks;

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
                 // Get the Extractor object from Toolkit
                 APToolkitNET.Extractor extractor = toolkit.GetExtractor();
                
                // Open the input PDF
                int result = toolkit.OpenInputFile($"{strPath}Toolkit.Input.pdf");
                if (result == 0)
                {


                    // Extract all images from the input PDF and save them in a
                    // randomly named file.
                    // Extractor supports BMP, JPEG, PNG, RGB, and TIFF
                    var images = extractor.ExtractImages(APToolkitNET.APImageType.JPEG);
                    Parallel.ForEach(images, (image) =>
                    {
                        string fileName = $"{System.IO.Path.GetRandomFileName()}.jpg";
                        try
                        {
                            Console.WriteLine($"Extracting Image to: {fileName}");
                            System.IO.File.WriteAllBytes(
                                $"{System.IO.Directory.GetCurrentDirectory()}\\{fileName}",
                                image);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Exception caught creating image file ({fileName}): {e.Message}");
                        }
                    });

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
