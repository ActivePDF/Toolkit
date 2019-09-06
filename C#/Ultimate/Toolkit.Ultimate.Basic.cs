using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivePDF_Desktop_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string newPDF = "GeneratedPDF.pdf";
            int result = -1;

            // Use Toolkit to create a new PDF
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
            {
                toolkit.OutputPageHeight = 792.0f;
                toolkit.OutputPageWidth = 612.0f;

                Console.WriteLine($"Toolkit: Generating new PDF using NewPage");

                // Open the output file in memory
                result = toolkit.OpenOutputFile("MEMORY");
                if (result != 0)
                {
                    WriteResult($"Toolkit: Failed to open output file in-memory, error code {result}");
                    return;
                }

                // Each time a new page is required call NewPage
                toolkit.NewPage();

                // Text can be added onto the new page with
                // SetFont, PrintText and PrintMultilineText functions
                toolkit.SetFont("Helvetica", 24);
                toolkit.PrintText(36.0f, 720.0f, $"Toolkit Version: {toolkit.ToolkitVersion}");

                // Images can be added onto the new page with
                // PrintImage, PrintJPEG and PrintTIFF
                toolkit.PrintJPEG($"{appPath}IMG.jpg", 36.0f, 36.0f, 540.0f, 684.0f, true);

                // Close the new file to complete PDF creation
                toolkit.CloseOutputFile();

                // Save the new PDF to the application path
                result = toolkit.SaveMemoryToDisk($"{appPath}{newPDF}");
                if (result != 0)
                {
                    WriteResult($"Toolkit: SaveMemoryToDisk failed, error code {result}");
                    return;
                }
                Console.WriteLine($"Toolkit: New pdf created {appPath}{newPDF}");

                // Use Toolkit Compressor to compress images
                Console.WriteLine("Toolkit.Compressor: Compressing generated PDF");
                toolkit.OpenOutputFile("MEMORY");
                if (result != 0)
                {
                    WriteResult($"Toolkit Compressor: Failed to open output file in-memory, error code {result}");
                    return;
                }

                // Retrieves the entire PDF as a string variable after you call
                // CloseOutputFile and set the output file name to MEMORY.
                toolkit.InputByteStream = toolkit.OutputByteStream;

                // Open the input file
                toolkit.OpenInputFile("MEMORY");
                if (result != 0)
                {
                    WriteResult($"Toolkit Compressor: Failed to open input file in-memory, error code {result}");
                    return;
                }

                // Instantiate the compressor object
                APToolkitNET.Compressor compressor = toolkit.GetCompressor();

                // Compresses images in the output PDF with the default settings.
                compressor.CompressImages = true;

                result = toolkit.CopyForm(0, 0);
                if (result != 1)
                {
                    WriteResult($"Toolkit Compressor: CopyForm failed, error code {result}");
                    return;
                }
                toolkit.CloseOutputFile();

                // Save the compressed PDF to disk
                result = toolkit.SaveMemoryToDisk($"{appPath}Toolkit.Compressed.pdf");
                if (result != 0)
                {
                    WriteResult($"Toolkit Compressor: SaveMemoryToDisk failed, error code {result}");
                    return;
                }
                Console.WriteLine($"Toolkit Compressor: Compressed pdf created {appPath}Toolkit.Compressed.pdf");
            }

            // Use Rasterizer to convert generated PDF to an image
            Console.WriteLine("\nRasterizer: Converting generated PDF to image format");
            using (APRasterizerNET.Rasterizer rasterizer =
                new APRasterizerNET.Rasterizer())
            {
                // Open PDF
                rasterizer.OpenFile($"{appPath}{newPDF}");

                // Get page count of open file
                int pageCount = rasterizer.NumPages();

                for (int currentPage = 1; currentPage <= pageCount; currentPage++)
                {
                    // Image Format
                    rasterizer.ImageFormat = APRasterizerNET.ImageType.ImgJPEG;

                    // Output Type
                    rasterizer.OutputFormat =
                        APRasterizerNET.OutputFormatType.OutFile;

                    // Other settings
                    rasterizer.OutputFileName =
                        $"{appPath}Rasterizer.ConvertPDFToJPEG.Page.{currentPage}.jpg";

                    // Render the current page
                    rasterizer.RenderPage(currentPage);

                    // Check for errors
                    if (rasterizer.LastError != 0)
                    {
                        WriteResult($"Error rendering page {currentPage}: {rasterizer.LastErrorMessage}");
                    }

                    Console.WriteLine($"Rasterizer: JPG image created at {appPath}Rasterizer.ConvertPDFToJPEG.Page.{currentPage}.jpg");
                }
            }

            // Use Xtractor to extract text and images
            Console.WriteLine("\nXtractor: Extracting images and text from the generated PDF");
            using (Xtractor.Xtractor xtractor = new Xtractor.Xtractor(filename: $"{appPath}{newPDF}"))
            {
                // Saves all images in the entire document to JPG files.
                string[] jpgFileNames = xtractor.ExtractImagesToFile(filenameOrMask: $"{appPath}Xtractor.#PAGE#_#NUM#.jpg");
                Console.WriteLine($"Xtractor: Files extracted from \"{newPDF}\"");
                foreach (string file in jpgFileNames)
                {
                    Console.WriteLine($"\tExtracted File: {file}");
                }

                // This method extracts the text from the whole document at once.
                // The string[] is sorted by page number, where index 'n' is page 'n + 1'.
                string[] allTextArray = xtractor.ExtractText();
                Console.WriteLine($"Xtractor: Text extracted from {newPDF}");
                foreach (string text in allTextArray)
                {
                    Console.WriteLine($"\tExtracted Text: {text}");
                }
            }

            // Use Redactor to redact images and text from generated PDF
            Console.WriteLine("\nRedactor: Redacting images and text from the generated PDF");
            using (APRedactor.Redactor redactor = new APRedactor.Redactor(
                filename: $"{appPath}{newPDF}"))
            {
                redactor.PageLiteralText = new string[]
                    {
                    "Version"
                    };
                redactor.TextMode =
                    APRedactor.Redactor.TextRedactionMode.LiteralText;
                redactor.ImageMode =
                    APRedactor.Redactor.ImageRedactionMode.Unconditional;
                int redactionsPerformed = redactor.Redact();
                redactor.Save($"{appPath}RedactImagesAndText.pdf");
                Console.WriteLine($"Redactor: Redacted PDF created {appPath}RedactedImagesAndText.pdf");
            }

            WriteResult("\nSuccess!");
        }

        public static void WriteResult(string result)
        {
            Console.WriteLine(result);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
