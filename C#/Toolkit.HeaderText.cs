
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
                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.HeaderText.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                    return;
                }

                // Open the template PDF
                result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                string fontName = "Helvetica";

                // Add a 'Confidential' watermark by setting text transparency
                // Rotation and color of the text along with the fill mode are set
                toolkit.SetHeaderFont(FontName: fontName, FontSize: 90);
                toolkit.SetHeaderTextTransparency(PercentStroke: 0.6f, PercentFill: 0.6f);
                toolkit.SetHeaderRotation(RotationAngle: 45);
                toolkit.SetHeaderTextStrokeColor(AmountRed: 255, AmountGreen: 0, AmountBlue: 0, AmountGrey: 0);
                toolkit.SetHeaderTextFillMode(FillMode: 1);
                toolkit.SetHeaderText(X: 154, Y: 184, Text: "Confidential");
                toolkit.ResetHeaderTextTransparency();
                toolkit.SetHeaderTextFillMode(FillMode: 0);

                // Add a 'Top Secret' watermark by placing text in the foreground
                toolkit.SetHeaderFont(FontName: fontName, FontSize: 72);
                toolkit.SetHeaderTextBackground(UseBackground: 1);
                toolkit.SetHeaderTextColor(AmountRed: 200, AmountGreen: 200, AmountBlue: 200, AmountGrey: 0);
                toolkit.SetHeaderText(X: 154, Y: 300, Text: "Top Secret");
                toolkit.ResetHeaderTextColor();
                toolkit.SetHeaderRotation(RotationAngle: 0);

                // Add the document title to the bottom center of the page
                toolkit.SetHeaderFont(FontName: fontName, FontSize: 12);
                toolkit.SetHeaderTextBackground(UseBackground: 0);
                string title = "ActivePDF Toolkit";
                float textWidth = toolkit.GetHeaderTextWidth(title);
                toolkit.SetHeaderText((612 - textWidth) / 2, 52, title);

                // Add page numbers to the bottom left of the page
                toolkit.SetHeaderFont(FontName: fontName, FontSize: 12);
                toolkit.SetHeaderWPgNbr(X: 540, Y: 52, Text: "Page %p", FirstPageNbr: 1);

                // Add a mulitline print box for an 'approved' message in header
                toolkit.SetHeaderTextFillMode(FillMode: 2);
                toolkit.SetHeaderTextColorCMYK(AmountCyan: 0, AmountMagenta: 0, AmountYellow: 0, AmountBlack: 20);
                toolkit.SetHeaderTextStrokeColorCMYK(AmountCyan: 0, AmountMagenta: 0, AmountYellow: 0, AmountBlack: 80);
                toolkit.SetHeaderMultilineText(FontName: fontName, FontSize: 22, X: 144, Y: 766, Width: 190, Height: 86, Text: "Approved on January 17th, 2021", Alignment: 2);
                toolkit.ForceHeaderColorReset();

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
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
