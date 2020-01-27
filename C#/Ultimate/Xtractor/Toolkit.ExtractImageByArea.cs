
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
                int result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result == 0)
                {


                    // Extract an image from the input PDF at the inputlocation
                    // randomly named file.
                    // Extractor supports BMP, JPEG, PNG, RGB, and TIFF
                    var image = extractor.ExtractImageByArea(
                        eImageType: APToolkitNET.APImageType.JPEG,
                        Page: 1,
                        Rect: new System.Drawing.RectangleF(10.0f, 500.0f, 200.0f, 200.0f));
                    string fileName = $"{System.IO.Path.GetRandomFileName()}.jpg";
                    try
                    {
                        if (image.Length != 0)
                        {
                            Console.WriteLine($"Extracting Image to: {fileName}");
                            System.IO.File.WriteAllBytes(
                                $"{System.IO.Directory.GetCurrentDirectory()}\\{fileName}",
                                image);
                        }
                        else
                        {
                            Console.WriteLine("No image found at location.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception caught creating image file ({fileName}): {e.Message}");
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
