using System;
using System.Diagnostics;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Model
{
    public class ErrorExceptionParallelProcessResult<TResult>
    {
        public bool IsSuccess { get; set; }

        public Exception Error { get; set; }

        public Stopwatch ResultTime { get; set; }

        public TResult Value { get; set; }
    }
}
