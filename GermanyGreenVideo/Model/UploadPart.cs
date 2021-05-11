using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GermanyGreenVideo.Model
{
    public class UploadPart
    {
        [DataType(DataType.Upload)]
        [Required]
        public IFormFile files { get; set; }
    }
}
