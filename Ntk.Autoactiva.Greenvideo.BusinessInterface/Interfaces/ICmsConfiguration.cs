using Ntk.Autoactiva.Greenvideo.BusinessInterface.Model;
using System.Collections.Generic;

namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces
{
    public interface ICmsConfiguration
    {
        string VerConfig { get; set; }
        string ConfigFile { get; set; }
        Dictionary<string, string> ConnectionStrings { get; set; }

        AppMemoryCCModel AppMemory { get; set; }
        AppSettingsCCModel AppSettings { get; set; }
    }
}
