using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GermanyGreenVideo.Util.Attributed
{
    public class UploadCompeleteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string UploadId = context.HttpContext.Request?.Headers["UploadId"];
            if (String.IsNullOrEmpty(UploadId))
            {
                context.Result = new BadRequestObjectResult("UploadId invalid");
            }

            string FileExt = context.HttpContext.Request?.Headers["FileExt"];
            if (String.IsNullOrEmpty(FileExt))
            {
                context.Result = new BadRequestObjectResult("FileExt invalid");
            }
            else if (FileExt.Length < 3 || FileExt.Length > 4)
            {
                context.Result = new BadRequestObjectResult("FileExt invalid");
            }

            string CheckSum = context.HttpContext.Request?.Headers["CheckSum"];
            if (String.IsNullOrEmpty(CheckSum))
            {
                context.Result = new BadRequestObjectResult("CheckSum invalid");
            }

            int TotalPartNumber = Convert.ToInt32(context.HttpContext.Request?.Headers["TotalPartNumber"]);
            if (TotalPartNumber <= 0)
            {
                context.Result = new BadRequestObjectResult("SequenceNumber invalid");
            }
        }
    }
}
