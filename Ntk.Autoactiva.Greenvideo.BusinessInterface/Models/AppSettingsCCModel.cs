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
        public MicroServicCModel MicroServiceFile { get; set; }
        
        public SwaggerCModel Swagger { get; set; }
        public ElmahCModel Elmah { get; set; }

    }

    public class FfmpegCModel
    {
        public string FileName { get; set; } = "";
        public string BinPath { get; set; } = "";
        public string Command { get; set; } = "";
    }
    public class MicroServicCModel
    {
        public bool Status { get; set; } = true;
        public string ShareFolderInput { get; set; } 
        public string ShareFolderInProcess { get; set; } 
        public string ShareFolderOutput { get; set; } 
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
