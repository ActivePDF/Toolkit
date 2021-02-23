
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
                // Such as adding security, setting page dimensions, etc.

                // Create the new PDF file
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.AddField.pdf");
                if (result == 0)
                {
                    // Open the template PDF
                    result = toolkit.OpenInputFile(InputFileName: $"{strPath}blank.pdf");
                    if (result == 0)
                    {
                        // Here you can call any Toolkit functions that will manipulate
                        // the input file such as text and image stamping, form filling, etc.

                        // Add a new button form to the first page.
                        APToolkitNET.FieldInfo fieldInfo = toolkit.AddField(1, 3, "TEST_BUTTON", 72, 750, 85, 25, "Helvetica", 10);

                        // Set the field properties with the AddField FieldInfo result object
                        // and add a simple JavaScript to execute when clicked.
                        fieldInfo.ButtonTextNormal = "TEST BUTTON";
                        fieldInfo.MouseDownScript = "app.alert(\"Hello World\", 3);";

                        // Add A DropDown with list items
                        fieldInfo = toolkit.AddField(1, 6, "TEST_DROPDOWN", 72, 700, 85, 25, "Helvetica", 10);
                        APToolkitNET.ListItems fieldListItems = fieldInfo.ListItems();
                        fieldListItems.AddNew("FirstItem", "FirstItemValue");
                        fieldListItems.AddNew("SecondItem", "SecondItemValue");
                        fieldListItems.AddNew("ThirdItem", "ThirdItemValue");

                        // Add a Checkbox and use SetFormFieldData to mark checked
                        fieldInfo = toolkit.AddField(1, 4, "TEST_CHECKBOX", 72, 650, 25, 25, "Helvetica", 10);
                        toolkit.SetFormFieldData("TEST_CHECKBOX", "On", 1);

                        // Add a Radio Button Group
                        fieldInfo = toolkit.AddField(1, 5, "TEST_RADIOGROUP", 72, 600, 25, 25, "Helvetica", 10);
                        fieldInfo.ExportValue = "One";
                        fieldInfo = toolkit.AddField(1, 5, "TEST_RADIOGROUP", 72, 625, 25, 25, "Helvetica", 10);
                        fieldInfo.ExportValue = "Two";
                        fieldInfo = toolkit.AddField(1, 5, "TEST_RADIOGROUP", 72, 650, 25, 25, "Helvetica", 10);
                        fieldInfo.ExportValue = "Three";
                        fieldInfo.RadiosInUnison = false;

                        // Add a text field and set it's default value.
                        fieldInfo = toolkit.AddField(1, 1, "TEST_TEXTFIELD", 72, 550, 140, 25, "Helvetica", 10);
                        fieldInfo.DefaultValue = "Made by ActivePDF Toolkit!";

                        // Add a list box
                        fieldInfo = toolkit.AddField(1, 7, "TEST_LISTBOX", 72, 450, 85, 50, "Helvetica", 10);
                        fieldListItems = fieldInfo.ListItems();
                        fieldListItems.AddNew("FirstItem", "FirstItemValue");
                        fieldListItems.AddNew("SecondItem", "SecondItemValue");
                        fieldListItems.AddNew("ThirdItem", "ThirdItemValue");

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
                    else
                    {
                        WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                        return;
                    }
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