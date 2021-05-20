
using Ntk.Autoactiva.Greenvideo.BusinessInterface.Interfaces;
using System.Collections.Generic;

namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Model
{
    public class CmsConfiguration : ICmsConfiguration
    {
        public CmsConfiguration()
        {
            AppSettings = new AppSettingsCCModel();
            AppMemory = new AppMemoryCCModel();
            ConnectionStrings = new Dictionary<string, string>();
        }
        public string VerConfig { get; set; }
        public string ConfigFile { get; set; }
        
        private AppMemoryCCModel _ApplicationMemory { get; set; }
        public AppMemoryCCModel AppMemory
        {
            get
            {
                if (_ApplicationMemory == null)
                    _ApplicationMemory = new AppMemoryCCModel();
                return _ApplicationMemory;
            }
            set { _ApplicationMemory = value; }
        }
        private AppSettingsCCModel _AppSettings { get; set; }
        public AppSettingsCCModel AppSettings
        {
            get
            {
                if (_AppSettings == null)
                    _AppSettings = new AppSettingsCCModel();
                return _AppSettings;
            }
            set { _AppSettings = value; }
        }
        private Dictionary<string, string> _ConnectionStrings { get; set; }
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                if (_ConnectionStrings == null)
                    _ConnectionStrings = new Dictionary<string, string>();
                return _ConnectionStrings;
            }
            set { _ConnectionStrings = value; }
        }


    }

}
