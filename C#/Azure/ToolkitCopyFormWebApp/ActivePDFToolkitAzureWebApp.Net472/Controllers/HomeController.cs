using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivePDFToolkitAzureWebApp.Net472.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CopyForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string strPath = Server.MapPath("~");
                    string inputFileName = Path.GetFileName(file.FileName);
                    if (string.Compare(Path.GetExtension($"{strPath}{inputFileName}"), ".pdf", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        WriteResult("Invalid file type.");
                        return View();
                    }
                    file.SaveAs($"{strPath}{inputFileName}");

                    ViewBag.WorkingDirectory = $"WorkingDirectory: {strPath}";

                    // Instantiate Object
                    using (APToolkitNET.Toolkit toolkit = new APToolkitNET.Toolkit())
                    {
                        string outputFileName = $"{Path.GetRandomFileName()}.pdf";
                        ViewBag.OutputFileName = outputFileName;

                        // Create the new PDF file
                        ViewBag.OutputFile = $"OutputFile: {strPath}{outputFileName}";
                        int result = toolkit.OpenOutputFile($"{strPath}{outputFileName}");
                        if (result != 0)
                        {
                            WriteResult($"Error opening output file: {result.ToString()}", toolkit);
                            return View();
                        }

                        // Open the template PDF
                        ViewBag.InputFile = $"InputFile: {strPath}{inputFileName}";
                        result = toolkit.OpenInputFile($"{strPath}{inputFileName}");
                        if (result != 0)
                        {
                            WriteResult($"Error opening input file: {result.ToString()}", toolkit);
                            return View();
                        }

                        // Copy the template (with any changes) to the new file
                        // Start page and end page, 0 = all pages
                        result = toolkit.CopyForm(0, 0);
                        if (result != 1)
                        {
                            WriteResult($"Error copying file: {result.ToString()}", toolkit);
                            return View();
                        }

                        // Close the new file to complete PDF creation
                        toolkit.CloseOutputFile();

                        System.IO.File.Delete($"{strPath}{inputFileName}");
                    }
                    WriteResult("Success!");
                }
                catch (Exception ex)
                {
                    WriteResult($"ERROR: {ex.Message.ToString()}");
                }
            }
            else
            {
                WriteResult("You have not specified a file.");
            }
            return View();
        }

        private void WriteResult(string result, APToolkitNET.Toolkit toolkit = null)
        {
            ViewBag.ResultText = result;
            if (toolkit != null)
            {
                ViewBag.ExtendedErrorCode = toolkit.ExtendedErrorCode.ToString();
                ViewBag.ExtendedErrorLocation = toolkit.ExtendedErrorLocation;
                ViewBag.ExtendedErrorDescription = toolkit.ExtendedErrorDescription;
            }
            return;
        }
    }
}