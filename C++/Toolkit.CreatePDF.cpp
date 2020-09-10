
#include <filesystem>
#include <iostream>

#import <C:/Program Files/ActivePDF/Toolkit/bin/x64/APTK.dll>

void WriteResult(const char*, int, APTOOLKITLib::IAPToolkitPtr);

int main(void)
{
    CoInitialize(NULL);

    std::filesystem::path strPath = std::filesystem::current_path();

    {
        // Toolkit COM interface pointer from aptk.tlh
        APTOOLKITLib::IAPToolkitPtr toolkitPtr;

        // Create the Toolkit instance from the ProgID
        // HKEY_CLASSES_ROOT\APToolkit.Object
        HRESULT hr = toolkitPtr.CreateInstance("APToolkit.Object");
        if (SUCCEEDED(hr))
        {
            long result = toolkitPtr->OpenOutputFile((strPath / "Toolkit.NewPDF.pdf").c_str(), 0);
            if (result == 0)
            {                
                // Each time a new page is required call NewPage
                toolkitPtr->NewPage(612.0f, 792.0f, 0);

                // Get the current version of Toolkit and save it to print on
                // the PDF
                _bstr_t toolkitVersion = toolkitPtr->ToolkitVersion;

                // Text can be added onto the new page with
                // SetFont, PrintText and PrintMultilineText functions
                toolkitPtr->SetFont("Helvetica", 24.0f, 0);
                toolkitPtr->PrintText(72.0f, 720.0f, toolkitVersion, 0);

                // Images can be added onto the new page with PrintImage,
                // PrintJPEG and PrintTIFF
                toolkitPtr->PrintJPEG(
                    (strPath / "Toolkit.Input.jpg").c_str(),
                    72.0f,
                    300.0f,
                    468.0f,
                    400.0f,
                    true,
                    0,
                    "");

                // Close the new file to complete PDF creation
                toolkitPtr->CloseOutputFile();
            }
            else
            {
                WriteResult("OpenOutputFile", result, toolkitPtr);
            }
        }
        else
        {
            std::cout << "Failed to create the Toolkit instance." << std::endl;
            std::cout << "Error: " << std::system_category().message(hr) << std::endl;
        }
    }

    CoUninitialize();

    std::cout << "Finished!" << std::endl;

    system("pause");
}

void WriteResult(const char* function, int result, APTOOLKITLib::IAPToolkitPtr toolkit)
{
    std::cout << function << " failed with error code: ";
    std::cout << result << std::endl;
    if (toolkit)
    {
        std::cout << "Extended Error Code: " << toolkit->ExtendedErrorCode << std::endl;
        std::cout << "Extended Error Description: " << toolkit->ExtendedErrorDescription << std::endl;
        std::cout << "Extended Error Location: " << toolkit->ExtendedErrorLocation << std::endl;
    }
}
