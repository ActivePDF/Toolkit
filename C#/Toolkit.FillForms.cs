
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
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.FillForms.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening output file: {result.ToString()}");
                    return;
                }

                // Open the template PDF
                result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // Set how you want form field data formatting to be handled
                // By default the field will remain the same (1)
                // You can have the field formatting be ignored (0)
                toolkit.DoFormFormatting = 1;

                // As we will be using the same PDF form for multiple pages we
                // do not want the field names to conflict as different data
                // will be in same named fields Using FormNumbering will rename
                // the fields so they do not conflict field 'name' would become
                // 'name__1', 'name__2', etc. 0 = False, 1 = True
                toolkit.FormNumbering = 1;

                // Populate form fields for the first page with data
                // Refer to the documentation for possible flag options
                toolkit.SetFormFieldData("Text1", "This text field's content was updated using ActivePDF Toolkit.", 1);

                // Copy the template (with any changes) to the new file
                // Start page and end page, 0 = all pages
                result = toolkit.CopyForm(FirstPage: 0, LastPage: 0);
                if (result != 1)
                {
                    WriteResult($"Error copying file: {result.ToString()}", toolkit);
                    return;
                }

                // Use ResetFormFields to clear the form data we previously
                // used
                toolkit.ResetFormFields();

                // Populate form fields for the second page with data
                toolkit.SetFormFieldData("Text1", "This second text field's content was updated using ActivePDF Toolkit.", 1);

                // Set whether to flatten all other fields not touched by
                // SetFormFieldData
                toolkit.FlattenRemainingFormFields = 1;

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
