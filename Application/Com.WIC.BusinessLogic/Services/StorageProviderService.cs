using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.WIC.BusinessLogic.Services
{
    public class StorageProviderService
    {
        public readonly string OutputPath;
        public readonly string OutputFolder;
        public StorageProviderService(string webRootPath)
        {
            OutputFolder = "Output";
            OutputPath = Path.Combine(webRootPath, OutputFolder);

            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
        }
    }
}
