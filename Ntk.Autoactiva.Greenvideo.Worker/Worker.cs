using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ntk.Autoactiva.Greenvideo.Core.Config;
using Ntk.Autoactiva.Greenvideo.Core.Helper;
using Ntk.Autoactiva.Greenvideo.Core.Models;

namespace Ntk.Autoactiva.Greenvideo.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            if (!CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.Status)
            {
                _logger.LogInformation("AppSettings.MicroServiceFile.Status Is False at: {time}", DateTimeOffset.Now);
                return;
            }
            if (string.IsNullOrEmpty(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput))
            {
                _logger.LogInformation("AppSettings.MicroServiceFile.ShareFolderInput Is Null at: {time}", DateTimeOffset.Now);
                return;
            }
            if (!System.IO.Directory.Exists(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput))
            {
                _logger.LogInformation("AppSettings.MicroServiceFile.ShareFolderInput Directory Not Exists at: {time}", DateTimeOffset.Now);
                return;
            }


            if (string.IsNullOrEmpty(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput))
            {
                _logger.LogInformation("AppSettings.MicroServiceFile.ShareFolderOutput Is Null at: {time}", DateTimeOffset.Now);
                return;
            }
            if (!System.IO.Directory.Exists(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput))
            {
                _logger.LogInformation("AppSettings.MicroServiceFile.ShareFolderOutput Directory Not Exists at: {time}", DateTimeOffset.Now);
                return;
            }

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                string[] allfiles = Directory.GetFiles(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput, "*.order", SearchOption.AllDirectories);
                foreach (var file in allfiles)
                {
                    var fileInfo = new FileInfo(file);

                    var OrderModel = System.IO.File.ReadAllText(fileInfo.FullName).JsonHelperDeserializeObject<OrderModel>();
                    if (OrderModel!=null && !string.IsNullOrEmpty(OrderModel.InputFileName) && !string.IsNullOrEmpty(OrderModel.OutputFileName))
                    {
                        fileInfo.MoveTo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + fileInfo.Name);
                        var fileInput = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput + "\\" + OrderModel.InputFileName;
                        var fileInProcess = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + OrderModel.InputFileName;
                        if (System.IO.File.Exists(fileInput))
                        {
                            if (System.IO.File.Exists(fileInProcess))
                                System.IO.File.Delete(fileInProcess);

                            File.Move(fileInput, fileInProcess);
                            Task.Run(async () => await this.CheckTheFile(OrderModel));
                        }
                    }
                }
            }
        }

      

        private async Task CheckTheFile(OrderModel model)
        {
            _logger.LogInformation("Worker running CheckTheFile at: {time} : {fileInProcess}", DateTimeOffset.Now, model.JsonHelperSerializeObject());


            try
            {
                //string chormaKey = "ffmpeg -i" + fullPath + " -filter_complex '[1:v]colorkey=0x00FF00:0.3:0.2[ckout];[0:v][ckout]overlay[out]' -map '[out]' " + fullPathNew;
                //string result = Assistant.Execute("C:\\ffmpeg\\bin\\ffmpeg.exe", chormaKey);

                var FileName = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.FileName;
                var Command = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.Command;
                var BinPath = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.BinPath;
                if (string.IsNullOrEmpty(Command))
                {
                    _logger.LogInformation("Ffmpeg.Command  Is Null");
                }
                if (string.IsNullOrEmpty(BinPath))
                {
                    _logger.LogInformation("Ffmpeg.BinPath  Is Null");
                }
                if (string.IsNullOrEmpty(FileName))
                {
                    _logger.LogInformation("Ffmpeg.FileName  Is Null");
                }

                if (System.IO.File.Exists(model.OutputFileName))
                    System.IO.File.Delete(model.OutputFileName);

                if (System.IO.File.Exists(model.OutputFileName + ".error"))
                    System.IO.File.Delete(model.OutputFileName + ".error");

                
                string chormaKey = string.Format(Command, CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + model.InputFileName, CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess+"\\"+ model.OutputFileName);
                var ass = new AssistantHelper();
                string result = ass.Execute(FileName, BinPath, chormaKey);
                var fileOutInfo =new System.IO.FileInfo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + model.OutputFileName);
                if (fileOutInfo.Exists)
                {
                    _logger.LogInformation("نام فایل ارسالی در سیستم at: {time} : {file}", DateTimeOffset.Now, CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + model.OutputFileName);
                    //
                    model.OutputFileSize = fileOutInfo.Length;
                    TextWriter twm = new StreamWriter(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + AssistantHelper.ChangeFileExtention(model.InputFileName, "order"), true);
                    twm.WriteLine(model.JsonHelperSerializeObject());
                    twm.Close();
                    twm.Dispose();
                    //

                    var f=new FileInfo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInProcess + "\\" + model.OutputFileName);
                    f.MoveTo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput + "\\" + model.OutputFileName);
                    return;
                }

                _logger.LogInformation("برروز خطا در ساخت فایل at: {time} : {file}", DateTimeOffset.Now, model.JsonHelperSerializeObject());

                TextWriter tw = new StreamWriter(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput + "\\" + model.OutputFileName + ".error", true);
                tw.WriteLine("Error:برروز خطا در ساخت فایل" + result);
                tw.Close();
                tw.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("برروز خطا در ساخت فایل at: {time} : {error}", DateTimeOffset.Now,  JsonConvert.SerializeObject(ex.Message));
                TextWriter tw = new StreamWriter(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput + "\\" + model.OutputFileName + ".error", true);
                tw.WriteLine("Error:" + JsonConvert.SerializeObject(ex.Message));
                tw.Close();
                tw.Dispose();
            }
        }
    }
}
