using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ntk.Autoactiva.Greenvideo.WebApi.Model;
using Ntk.Autoactiva.Greenvideo.WebApi.Util;
using Ntk.Autoactiva.Greenvideo.WebApi.Util.Attributed;
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
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadsController(IWebHostEnvironment environment)
        {
            _environment = environment;
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
                var ExtensionOut = ".webm";
                var uploadFolderPath = Path.Combine(webRootPath, "videos");
                Directory.CreateDirectory(uploadFolderPath);

                var fullPath = Path.Combine(uploadFolderPath, Id + Extension);
                var fullPathNew = Path.Combine(uploadFolderPath, newId + ExtensionOut);

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
                    }

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
                                OutputFileName = AssistantHelper.ChangeFileExtention(fileInfo.Name, ExtensionOut),
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
                                    Message = "Download File Name",
                                    Data = newId + ExtensionOut
                                });
                            }
                            return Ok(new Response()
                            {
                                IsSuccess = false,
                                Message = "Error On Convert File",
                                Data = findOutFileError
                            });
                            #endregion MicroService
                        }
                        else
                        {
                            #region Normal

                            var FileName = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.FileName;
                            var CommandConvertWebm = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.CommandConvertWebm;
                            var CommandOptimaze = CoreIocConfig.IocConfig.GetCmsConfiguration().AppSettings.Ffmpeg.CommandOptimaze;
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
                                    Message = "Download File Name",
                                    Data = newId + ExtensionOut
                                });
                            }
                            return Ok(new Response()
                            {
                                IsSuccess = false,
                                Message = "Error On Convert File",
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