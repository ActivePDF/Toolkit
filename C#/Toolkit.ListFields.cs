
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
                // Open the template PDF
                short result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                if (result != 0)
                {
                    WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                    return;
                }

                // List the fields in the input PDF
                APToolkitNET.InputFields inputFields = toolkit.GetInputFields();
                foreach (APToolkitNET.FieldInstances fieldInstances in inputFields.Instances)
                {
                    foreach (APToolkitNET.FieldInfo fieldInstance in fieldInstances.Fields)
                    {
                        WriteFieldInfo(FieldInfo: fieldInstance);
                    }
                }

                // Close the input file
                toolkit.CloseInputFile();
            }

            WriteResult("Success!");
        }

        // List the fields for the input PDF. A list of available FieldInfo
        // properties can be found in the online Toolkit SDK documentation.
        public static void WriteFieldInfo(APToolkitNET.FieldInfo FieldInfo)
        {
            if (FieldInfo != null)
            {
                Console.WriteLine($"Field: {FieldInfo.Name}");
                Console.WriteLine($"  Width: {FieldInfo.Width}");
                Console.WriteLine($"  Height: {FieldInfo.Height}");
                Console.WriteLine($"  Lower Left Coordinate: ({FieldInfo.Left}, {FieldInfo.Bottom})");
                Console.WriteLine();
            }
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