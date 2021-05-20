using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ntk.Autoactiva.Greenvideo.WebApi.Model
{
    public class UploadPart
    {
        [DataType(DataType.Upload)]
        [Required]
        public IFormFile files { get; set; }
    }
}
