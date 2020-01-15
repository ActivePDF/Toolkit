
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
                // Get the Toolkit Bookmark Manager
                APToolkitNET.BookmarkManager bookmarkManager = toolkit.GetBookmarkManager();

                // Set CopyBookmarks to true to create bookmarks in the output PDF.
                bookmarkManager.CopyBookmarks = true;

                // Create the new PDF file
                int result = toolkit.OpenOutputFile($"{strPath}Toolkit.LaunchBookmark.pdf");
                if (result == 0)
                {
                    // Open the template PDF
                    result = toolkit.OpenInputFile($"{strPath}Toolkit.Input.pdf");
                    if (result == 0)
                    {
                        // Create a new bookmark for the page. 
                        APToolkitNET.Bookmark newBookmark =
                            bookmarkManager.NewBookmark(
                                Title: "Open your application.");

                        // Set the page number and location to link
                        newBookmark.SetLaunch(CommandName: "your.exe");

                        // Set the bookmark color - orange
                        newBookmark.Color = "#FFA500";

                        // Set the bookmark font style
                        newBookmark.FontStyle =
                            APToolkitNET.FontStyle.ItalicBold;

                        // Copy the template (with any changes) to the new file
                        // Start page and end page, 0 = all pages
                        result = toolkit.CopyForm(0, 0);
                        if (result != 1)
                        {
                            WriteResult($"Error copying file: {result.ToString()}", toolkit);
                            return;
                        }
                    }
                    else
                    {
                        WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                        return;
                    }

                    // Close the new file to complete PDF creation
                    toolkit.CloseOutputFile();
                }
                else
                {
                    WriteResult($"Error opening output file: {result.ToString()}", toolkit);
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
