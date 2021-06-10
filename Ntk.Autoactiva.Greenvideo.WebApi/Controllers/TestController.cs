using System;
using System.IO;
using System.Threading.Tasks;
using Ntk.Autoactiva.Greenvideo.WebApi.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ntk.Autoactiva.Greenvideo.Core.Config;
using Ntk.Autoactiva.Greenvideo.Core.Helper;
using Ntk.Autoactiva.Greenvideo.Core.Models;

namespace Ntk.Autoactiva.Greenvideo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public TestController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpGet("Default")]
        public IActionResult Default()
        {
            var webRootPath = _environment.WebRootPath;
            var defaultFilePath = Path.Combine(webRootPath, "Temp", "default.mp4");
            if (!System.IO.File.Exists(defaultFilePath))
                return BadRequest("فایل پیش فرض یافت نشد");

            var Extension = ".mp4";
            var ExtensionOut = ".webm";

            var Id = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(webRootPath, "Videos", Id + Extension);
            System.IO.File.Copy(defaultFilePath, fullPath);
            var newId = Guid.NewGuid().ToString();

            var uploadFolderPath = Path.Combine(webRootPath, "videos");
            var fullPathNew = Path.Combine(uploadFolderPath, newId + ExtensionOut);
            try
            {
                if (CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.Status)
                {
                    #region MicroService
                    var fileInfo = new FileInfo(fullPath);
                    fileInfo.MoveTo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput + "\\" + fileInfo.Name);

                    var orderModel = new OrderModel()
                    {
                        InputFileName = fileInfo.Name,
                        OutputFileName = AssistantHelper.ChangeFileExtention(fileInfo.Name, "webm"),
                        InputFileSize = fileInfo.Length
                    };
                    TextWriter tw = new StreamWriter(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderInput + "\\" + AssistantHelper.ChangeFileExtention(fileInfo.Name, "order"), true);
                    tw.WriteLine(orderModel.JsonHelperSerializeObject());
                    tw.Close();
                    tw.Dispose();

                    var findOutFile = false;
                    var findOutFileError = "";
                    

                    while (!findOutFile)
                    {
                        Task.Delay(1000);
                        var fileInfoOut = new FileInfo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput + "\\" + orderModel.OutputFileName);
                        if (fileInfoOut.Exists && !fileInfoOut.IsReadOnly)
                        {
                            findOutFile = true;
                            fileInfoOut.MoveTo(fullPathNew);
                        }
                        var fileInfoOutError = new FileInfo(CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.MicroServiceFile.ShareFolderOutput + "\\" + orderModel.OutputFileName + ".error");
                        if (fileInfoOutError.Exists && !fileInfoOutError.IsReadOnly)
                        {
                            findOutFile = true;
                            findOutFileError = System.IO.File.ReadAllText(fileInfoOutError.FullName); ;
                        }
                    }

                    if (System.IO.File.Exists(fullPathNew))
                    {

                        return Ok(new Response()
                        {
                            IsSuccess = true,
                            Message = "نام فایل ارسالی در سیستم",
                            Data = newId + ExtensionOut
                        });
                    }
                    return Ok(new Response()
                    {
                        IsSuccess = false,
                        Message = "برروز خطا در ساخت فایل",
                        Data = findOutFileError
                    });
                    #endregion MicroService
                }
                else
                {
                    #region Normal

                    var FileName = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.FileName;
                    var CommandOptimaze = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.CommandOptimaze;
                    var CommandConvertWebm = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.CommandConvertWebm;
                    var BinPath = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.BinPath;
                    if (string.IsNullOrEmpty(CommandOptimaze))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = false,
                            Message = "Ffmpeg.CommandOptimaze  Is Null"
                        });
                    }
                    if (string.IsNullOrEmpty(CommandConvertWebm))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = false,
                            Message = "Ffmpeg.CommandConvertWebm  Is Null"
                        });
                    }
                    if (string.IsNullOrEmpty(BinPath))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = false,
                            Message = "Ffmpeg.BinPath  Is Null"
                        });
                    }
                    if (string.IsNullOrEmpty(FileName))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = false,
                            Message = "Ffmpeg.FileName  Is Null"
                        });
                    }
                    var ass = new AssistantHelper();
                    //string CmdommandOptimaze = string.Format(CommandOptimaze, fullPath, fullPath);
                    //string result = ass.Execute(FileName, BinPath, CmdommandOptimaze);

                    string CmdCommandConvertWebm = string.Format(CommandConvertWebm, fullPath, fullPathNew);
                    string result = ass.Execute(FileName, BinPath, CmdCommandConvertWebm);
                    if (System.IO.File.Exists(fullPathNew))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = true,
                            Message = "نام فایل ارسالی در سیستم",
                            Data = newId + ExtensionOut
                        });
                    }
                    return Ok(new Response()
                    {
                        IsSuccess = false,
                        Message = "برروز خطا در ساخت فایل",
                        Data = result
                    });
                    #endregion Normal
                }




            }
            catch (Exception ex)
            {
                return Ok(new Response()
                {
                    IsSuccess = false,
                    Message = JsonConvert.SerializeObject(ex.Message)
                });
            }


        }

    }
}