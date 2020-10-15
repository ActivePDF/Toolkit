
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

                // Here you can place any code that will alter the output file
                // such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.FillDBFields.pdf");
                if (result == 0)
                {
                    // Specify the template form to populate
                    toolkit.SetDBInputTemplate(InputPDFPath: $"{strPath}Toolkit.DBTemplate.pdf");

                    // Set query command to a variable
                    string strSQL = "Select * From Supplier";

                    // Set connection String to a variable
                    string strConn = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={strPath}Toolkit.Input.mdb" + ";Persist Security Info=False";

                    // Set master query
                    result = toolkit.SetMasterQuery(
                        ConnectionString: strConn,
                        UserID: "Admin",
                        Password: "",
                        Options: -1,
                        QueryString: strSQL);
                    if (result != 0)
                    {
                        WriteResult($"Failed to access the database file: {result}", toolkit);
                        return;
                    }

                    // Set the row Separator
                    toolkit.SetDBMultiRowSeparator(MultiRowSeparator: "");

                    // Related query separator is only needed if different from
                    // default of '|'
                    toolkit.RelatedQuerySeparator = "|";

                    // Add related query
                    result = toolkit.AddRelatedQuery(
                        ConnectionString: strConn,
                        UserID: "Admin",
                        Password: "",
                        Options: -1,
                        QueryString: strSQL,
                        MultiRows: true);
                    if (result != 0)
                    {
                        WriteResult($"Failed to implmenet the query: {result}", toolkit);
                        return;
                    }


                    // If db column names are different then field names a map file
                    // is needed. In this example only the zip/postal code is
                    // different.
                    result = toolkit.LoadDBMapFile(PathToMapFile: $"{strPath}Toolkit.DBFormmap.txt");
                    if (result != 0)
                    {
                        WriteResult($"Failed to load the map file: {result}", toolkit);
                        return;
                    }

                    // Flatten fields that are populated with data
                    toolkit.SetDefaultDBMergeFlag(DefaultMergeFlag: -997);

                    // Flatten all other fields on the form
                    toolkit.FlattenRemainingFormFields = 1;

                    // Fill the template form
                    result = toolkit.DBToForm(MultiPage: false);
                    if (result != 0)
                    {
                        WriteResult($"DBToForm failed: {result}", toolkit);
                        return;
                    }

                    // Clear and close used queries
                    toolkit.ClearQueries();

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
