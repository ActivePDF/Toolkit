
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
            // Here you can place any code that will alter the output file
            // Such as adding security, setting page dimensions, etc.

            // Create the new PDF file
            long result = toolkitPtr->OpenOutputFile((strPath / "Toolkit.Output.pdf").c_str(), 0);
            if (result == 0)
            {
                // Open the template PDF
                result = toolkitPtr->OpenInputFile((strPath / "Toolkit.Input.pdf").c_str());
                if (result == 0)
                {
                    // Here you can call any Toolkit functions that will manipulate
                    // the input file such as text and image stamping, form filling, etc.

                    // Copy the template (with any changes) to the new file
                    // Start page and end page, 0 = all pages
                    result = toolkitPtr->CopyForm(0, 0);
                    if (result != 1)
                    {
                        WriteResult("CopyForm", result, toolkitPtr);
                    }

                    // Close the new file to complete PDF creation
                    toolkitPtr->CloseOutputFile();
                }
                else
                {
                    WriteResult("OpenInputFile", result, toolkitPtr);
                }
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
