using System;
namespace GermanyGreenVideo.Model
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
