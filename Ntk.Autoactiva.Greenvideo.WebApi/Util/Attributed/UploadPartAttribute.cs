using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ntk.Autoactiva.Greenvideo.WebApi.Util.Attributed
{
    public class UploadPartAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string UploadId = context.HttpContext.Request?.Headers["UploadId"];
            if (String.IsNullOrEmpty(UploadId))
            {
                context.Result = new BadRequestObjectResult("UploadId invalid");
            }

            int PartSize = Convert.ToInt32(context.HttpContext.Request?.Headers["PartSize"]);
            if (PartSize <= 0)
            {
                context.Result = new BadRequestObjectResult("PartSize invalid");
            }

            int SequenceNumber = Convert.ToInt32(context.HttpContext.Request?.Headers["SequenceNumber"]);
            if (SequenceNumber <= 0)
            {
                context.Result = new BadRequestObjectResult("SequenceNumber invalid");
            }

            int TotalPartNumber = Convert.ToInt32(context.HttpContext.Request?.Headers["TotalPartNumber"]);
            if (TotalPartNumber <= 0 || TotalPartNumber > 2999)
            {
                context.Result = new BadRequestObjectResult("TotalPartNumber invalid");
            }

            if (SequenceNumber <= TotalPartNumber)
            {
                if (PartSize > 2048000)
                {
                    context.Result = new BadRequestObjectResult("PartSize not greeter than 2048");
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("SequenceNumber invalid");
            }
        }
    }
}
