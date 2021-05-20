using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GermanyGreenVideo.Model;
using GermanyGreenVideo.Util;
using GermanyGreenVideo.Util.Attributed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ntk.Autoactiva.Greenvideo.Core.Config;
using Ntk.Autoactiva.Greenvideo.Core.Helper;

namespace GermanyGreenVideo.Controllers
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
            var fullPath = Path.Combine(webRootPath, "Temp", "default.mp4");
            if (System.IO.File.Exists(fullPath))
            {
                var newId = Guid.NewGuid().ToString();
                var Extension = ".webm";

                var uploadFolderPath = Path.Combine(webRootPath, "videos");
                Directory.CreateDirectory(uploadFolderPath);


                var fullPathNew = Path.Combine(uploadFolderPath, newId + Extension);




                try
                {
                    //string chormaKey = "ffmpeg -i" + fullPath + " -filter_complex '[1:v]colorkey=0x00FF00:0.3:0.2[ckout];[0:v][ckout]overlay[out]' -map '[out]' " + fullPathNew;
                    //string result = Assistant.Execute("C:\\ffmpeg\\bin\\ffmpeg.exe", chormaKey);

                    var Command = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.Command;
                    var BinPath = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.BinPath;
                    if (string.IsNullOrEmpty(Command))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = false,
                            Message = "Ffmpeg.Command  Is Null"
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
                    string chormaKey = string.Format(Command, fullPath, fullPathNew);
                    var ass = new AssistantHelper();
                    string result = ass.Execute("ffmpeg.exe",BinPath, chormaKey);
                    if (System.IO.File.Exists(fullPathNew))
                    {
                        return Ok(new Response()
                        {
                            IsSuccess = true,
                            Message = "نام فایل ارسالی در سیستم",
                            Data = newId + Extension
                        });
                    }
                    return Ok(new Response()
                    {
                        IsSuccess = false,
                        Message = "برروز خطا در ساخت فایل",
                        Data = result
                    });
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
            else
            {
                return BadRequest("فایل پیش فرض یافت نشد");
            }
        }

    }
}