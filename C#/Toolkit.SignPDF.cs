
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
                int result = toolkit.OpenOutputFile(FileName: $"{strPath}Toolkit.SignPDF.pdf");
                if (result == 0)
                {
                    // Open the template PDF
                    result = toolkit.OpenInputFile(InputFileName: $"{strPath}Toolkit.Input.pdf");
                    if (result == 0)
                    {
                        // Here you can call any Toolkit functions that will manipulate
                        // the input file such as text and image stamping, form filling, etc.

                        // Find certificate, if it doesn't exist it will be created
                        // If you have an existing signature you can find it with
                        // FindCertificate and remove the portion of code that creates
                        // the certificate
                        int certificateID = toolkit.FindCertificate(
                            CertName: "John Doe",
                            Store: "My",
                            LMStore: 0);
                        if (certificateID < 1)
                        {
                            Console.WriteLine("Certificate not found, creating.");

                            // Certificate not found, create a certificate with Toolkit
                            int createCertResult = toolkit.CreateCertificate(
                                CommonName: "John Doe",
                                OrgUnit: "Management",
                                Org: "Doe Enterprises",
                                Local: "Mission Viejo",
                                State: "CA",
                                Country: "US",
                                EMail: "john@doee.com",
                                UseLocalMachine: 0,
                                CertStoreName: "My",
                                DaysCertIsValid: 365,
                                IssuerUseLocalMachine:
                                0, IssuerName: "",
                                IssuerStoreName: "");
                            if (createCertResult == 1)
                            {
                                // New certificate created, find it for use
                                certificateID = toolkit.FindCertificate(
                                    CertName: "John Doe",
                                    Store: "My",
                                    LMStore: 0);
                                if (certificateID < 1)
                                {
                                    WriteResult($"Failed to find new certificate after creation: {certificateID}");
                                }
                            }
                            else
                            {
                                WriteResult($"Failed to create new certificate: {createCertResult}");
                            }
                        }

                        // Invisibly sign the output file after any creation, merge or append operation.
                        toolkit.SignOutputFile(
                            SigNumber: certificateID,
                            Location: "Mission Viejo",
                            Reason: "Testing Toolkit PDF Signing.",
                            ContactInfo: "http://www.activepdf.com",
                            Encoding: 0);

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
