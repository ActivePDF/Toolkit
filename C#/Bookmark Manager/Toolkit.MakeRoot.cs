
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
                // Get the Toolkit Bookmark Manager
                APToolkitNET.BookmarkManager bookmarkManager = toolkit.GetBookmarkManager();

                // Set CopyBookmarks to true to create bookmarks in the output PDF.
                bookmarkManager.CopyBookmarks = true;

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.MakeRootAll.pdf");
                if (result == 0)
                {
                    // Open the template PDF
                    result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                    if (result == 0)
                    {
                        // Get the number of pages from the input PDF
                        int numPages = toolkit.NumPages(FileName: "");

                        // Create a new root bookmark for all bookmarks to be
                        // placed under.
                        APToolkitNET.Bookmark root = bookmarkManager.MakeRoot(
                            Title: "New Root",
                            Color: "#FFA500",
                            FontStyle: APToolkitNET.FontStyle.ItalicBold);

                        // Add a book mark for each page
                        for (int i = 0; i < numPages; ++i)
                        {
                            // Create a new bookmark for the page. 
                            APToolkitNET.Bookmark newBookmark =
                                bookmarkManager.AddChild(
                                    Parent: root,
                                    Title: $"Page {i + 1}",
                                    Color: "#FFA500",
                                    FontStyle: APToolkitNET.FontStyle.Bold);

                            // Set the page number and location to link
                            newBookmark.SetInternalLink(
                                DestPage: i + 1,
                                LLX: 0,
                                LLY: 0);
                        }

                        // Copy the template (with any changes) to the new file
                        // Start page and end page, 0 = all pages
                        result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
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
