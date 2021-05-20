using System;
using System.Collections.Generic;
using System.Text;

namespace Ntk.Autoactiva.Greenvideo.Core.Models
{
    public class OrderModel
    {
        public string InputFileName { get; set; }
        public decimal InputFileSize { get; set; }
        public string OutputFileName { get; set; }
        public decimal OutputFileSize { get; set; }
        public int OutputFileSecend { get; set; }
    }
}
