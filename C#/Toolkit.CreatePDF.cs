
using System;
using System.Text;

namespace ToolkitExamples
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
            string toolkitPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\ActivePDF\Toolkit\bin\x64";

            // Instantiate Object
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit(CoreLibPath: toolkitPath))
            {
                // Set the PDF page Height and Width (72 = 1")
                toolkit.OutputPageHeight = 792.0f;
                toolkit.OutputPageWidth = 612.0f;

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.NewPDF.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Each time a new page is required call NewPage
                toolkit.NewPage();

                // Get the current version of Toolkit and save it to print on
                // the PDF
                string toolkitVersion = toolkit.ToolkitVersion;

                // Text can be added onto the new page with
                // SetFont, PrintText and PrintMultilineText functions
				float font_size = 24.0f;
                string font_name = "Times New Roman";
                toolkit.SetFont(FontName: font_name, FontSize: font_size);
				
				float y = 720.0f;
                toolkit.PrintText(X: 72.0f, Y: y, 1, $"Toolkit Version: {toolkitVersion}", font_name, font_size);
                toolkit.PrintText(X: 72.0f, Y: y - 25, 1, $"by ActivePDF - An Apryse Company", font_name, font_size);

                // Images can be added onto the new page with PrintImage,
                // PrintJPEG and PrintTIFF
                // Substitute your images here
		toolkit.PrintImage(
			ImageFileName: strPath + "butterfly.png",
			X: 72.0f,
			Y: 500.0f,
			Width: 100.0f,
			Height: 100.0f,
			PersistRatio: true,
			PageNumber: 0);

		toolkit.PrintImage(
			ImageFileName: strPath + "logo_red.png",
			X: 200.0f,
			Y: 500.0f,
			Width:100.0f,
			Height: 100.0f,
			PersistRatio: true,
			PageNumber: 0);

		toolkit.PrintJPEG(
			FileName: strPath + "dice.jpg",
			X: 72.0f,
			Y: 400.0f,
			Width: 100.0f,
			Height: 100.0f,
			PersistRatio: true,
			PageNumber: 0);

		toolkit.PrintTIFF(
			FileName: strPath + "Toolkit.Input.tif",
			X: 200.0f,
			Y: 400.0f,
			Width: 100.0f,
			Height: 100.0f,
			PersistRatio: true);

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
