
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
            using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit(toolkitPath))
            {
                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.PDFViewData.pdf");
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

                // Get the reference to the InitialViewInfo object
                APToolkitNET.InitialViewInfo viewInfo = toolkit.GetInitialViewInfo();

                // Options for viewer window
                Console.WriteLine($"Center Window: {viewInfo.CenterWindow}");
                Console.WriteLine($"Full Screen: {viewInfo.FullScreen}");
                Console.WriteLine($"Resize Window: {viewInfo.ResizeWindow}");
                Console.WriteLine($"Show: {viewInfo.Show}");

                // Show or hide UI elements of the viewer
                Console.WriteLine($"Hide Menu Bar: {viewInfo.HideMenuBar}");
                Console.WriteLine($"Hide Tool Bars: {viewInfo.HideToolBars}");
                Console.WriteLine($"Hide Window Controls: {viewInfo.HideWindowControls}");
                Console.WriteLine($"Navigation Tab: {viewInfo.NavigationTab}");

                // Page settings
                Console.WriteLine($"Magnification: {viewInfo.Magnification}");
                Console.WriteLine($"Open To Page: {viewInfo.OpenToPage}");
                Console.WriteLine($"Page Layout: {viewInfo.PageLayout}");

                // Close the input file
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
