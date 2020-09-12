using System;
using System.Runtime.InteropServices;

namespace SalesProcessor.Infrastructure.Configuration
{
    public class GenericSettings
    {
        public string homePath {
            get {
                //Suport for Windows or other systems
                var envHome = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "HOMEPATH" : "HOME";
                return Environment.GetEnvironmentVariable(envHome);
            }
        }
    }
}