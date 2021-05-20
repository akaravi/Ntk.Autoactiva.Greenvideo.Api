namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Model
{
    /// <summary>
    /// دخیره مقدار های مورد نیاز
    /// </summary>

    public class AppMemoryCCModel
    {
        public AppMemoryCCModel()
        {
            Connection = new MemoryConnectionCCmodel();
        }

        public string Path { get; set; }
        public string Env { get; set; }
        public bool RegisteredIdentityServer { get; set; } = false;
        public string DeviceToken { get; set; }
        public MemoryConnectionCCmodel Connection { get; set; }

        public string ContentRootPath { get; set; }
        public string WebRootPath { get; set; }

        public bool IsDevelopment { get; set; }


    }
   
    public class MemoryConnectionCCmodel
    {
        public bool MicroServiceExtensionSoftwareConected { get; set; } = false;
        public bool MicroServiceHubConected { get; set; } = false;
    }
}