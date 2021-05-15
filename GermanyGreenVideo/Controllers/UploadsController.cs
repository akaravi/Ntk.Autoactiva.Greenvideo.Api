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

namespace GermanyGreenVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Settings.Ffmpeg _fFmpeg;

        public UploadsController(IWebHostEnvironment environment, IOptions<Settings.Ffmpeg> options)
        {
            _environment = environment;
            _fFmpeg = options.Value;
        }

        /// <summary>
        /// check big file uploaded or not 
        /// </summary>
        /// <param name="CheckSum"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpGet]
        public IActionResult Get([FromQuery] string CheckSum)
        {
            if (String.IsNullOrEmpty(CheckSum))
            {
                return BadRequest("CheckSum can't null or empty value");
            }
            else
            {
                var uploadId = Guid.NewGuid().ToString();
                var webRootPath = _environment.WebRootPath;
                var uploadTempPath = Path.Combine(webRootPath, "Temp", uploadId);
                Directory.CreateDirectory(uploadTempPath);

                return Ok(new Response()
                {
                    IsSuccess = true,
                    Message = "شناسه سیستمی جهت ارسال فایل",
                    Data = uploadId,
                });
            }
        }

        /// <summary>
        /// upload part chunk file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost]
        [ServiceFilter(typeof(UploadPartAttribute))]
        public IActionResult Part([FromForm] UploadPart model)
        {
            if (model.files.Length > 0 &&
                model.files.Length == Convert.ToInt32(Assistant.getHeaderValue(Request, "PartSize")))
            {
                var webRootPath = _environment.WebRootPath;
                var uploadTempPath = Path.Combine(webRootPath, "Temp",
                    Convert.ToString(Assistant.getHeaderValue(Request, "UploadId")));
                if (Directory.Exists(uploadTempPath))
                {
                    var chankPath = Path.Combine(uploadTempPath,
                        Convert.ToString(Assistant.getHeaderValue(Request, "SequenceNumber")) + ".bmp");
                    using (FileStream fileStream = new FileStream(chankPath, FileMode.OpenOrCreate))
                    {
                        model.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok(new Response()
                        {
                            IsSuccess = true,
                            Message = "پارت مورد نظر با موفقیت آپلود شد",
                            Data = Assistant.getHeaderValue(Request, "SequenceNumber")
                        });
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("محتوایی جهت آپلود وجود ندارد");
            }
        }

        /// <summary>
        /// compelete chunk file for create file in system
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpGet("compelete")]
        [ServiceFilter(typeof(UploadCompeleteAttribute))]
        public IActionResult Compelete()
        {
            var webRootPath = _environment.WebRootPath;
            var uploadTempPath = Path.Combine(webRootPath, "Temp",
                Convert.ToString(Assistant.getHeaderValue(Request, "UploadId")));
            if (Directory.Exists(uploadTempPath))
            {
                var Id = Guid.NewGuid().ToString();
                var newId = Guid.NewGuid().ToString();
                var Extension = Assistant.getHeaderValue(Request, "FileExt");

                var uploadFolderPath = Path.Combine(webRootPath, "videos");
                Directory.CreateDirectory(uploadFolderPath);

                var fullPath = Path.Combine(uploadFolderPath, Id + Extension);
                var fullPathNew = Path.Combine(uploadFolderPath, newId + Extension);

                string[] inputFilePaths = Directory.GetFiles(uploadTempPath);
                if (inputFilePaths.Length != 0 && inputFilePaths.Length <=
                    Convert.ToInt32(Assistant.getHeaderValue(Request, "TotalPartNumber")))
                {
                    using (FileStream filestream = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {
                        for (int i = 1; i <= Convert.ToInt32(Assistant.getHeaderValue(Request, "TotalPartNumber")); i++)
                        {
                            using (var inputStream = System.IO.File.OpenRead(uploadTempPath + "\\" + i + ".bmp"))
                            {
                                inputStream.CopyTo(filestream);
                            }
                        }
                        Directory.Delete(uploadTempPath, true);
                        filestream.Flush();

                        try
                        {
                            string chormaKey = _fFmpeg.Command1 + fullPath + _fFmpeg.Command2 + fullPathNew;

                            string result = Assistant.Execute("C:\\ffmpeg\\bin\\ffmpeg.exe", chormaKey);

                            return Ok(new Response()
                            {
                                IsSuccess = true,
                                Message = "نام فایل ارسالی در سیستم",
                                Data = newId + Extension
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
                }
                else
                {
                    List<int> numbres = new List<int>();
                    for (int i = 1; i <= Convert.ToInt32(Assistant.getHeaderValue(Request, "TotalPartNumber")); i++)
                    {
                        if (!System.IO.File.Exists(Path.Combine(uploadTempPath, Convert.ToString(i))))
                        {
                            numbres.Add(i);
                        }
                    }
                    Response response = new Response()
                    {
                        IsSuccess = false,
                        Message = "تکه فایل های خراب در آپلود",
                        Data = numbres
                    };
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("شناسه آپلود اشتباه است");
            }
        }

    }
}