namespace Ntk.Autoactiva.Greenvideo.BusinessInterface.Model
{
    /// <summary>
    /// تنظیمات از فایل جیسان
    /// </summary>
    public class AppSettingsCCModel
    {
        public AppSettingsCCModel()
        {

        }
        public FfmpegCModel Ffmpeg { get; set; }
        public MicroServiceCloudFileCCModel MicroServiceCloudFile { get; set; }
        
        public SwaggerCModel Swagger { get; set; }
        public ElmahCModel Elmah { get; set; }

    }

    public class FfmpegCModel
    {
        public string BinPath { get; set; } = "";
        public string Command { get; set; } = "";
       
    }
    public class MicroServiceCloudFileCCModel
    {
        public string baseApiUrl { get; set; } = "https://apicms.ir/api/v1/";
        public string CloudFileApiPath { get; set; } = "http://apifile.ir/api/v1/";

        public string CloudFileKey { get; set; } = "li8e23e23eop2e33";
    }
    
    public class SwaggerCModel
    {
        public bool SwaggerJson { get; set; } = false;
        public bool SwaggerUI { get; set; } = false;

    }

    public class ElmahCModel
    {
        public bool Status { get; set; } = false;
        public string ElmahPath { get; set; } = @"elmah";
        public string ElmahLogPath { get; set; } = "~/log"; // OR options.LogPath = "с:\errors";
    }
   
}
