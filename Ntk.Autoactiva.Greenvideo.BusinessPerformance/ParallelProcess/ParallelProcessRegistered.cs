using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Model;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Performance;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.ParallelProcess
{
    public class ParallelProcessRegistered<TResult>
    {

        public ParallelProcessRegistered(ParallelProcess<TResult> parent)
        {
            Parent = parent;
        }
        public ParallelProcess<TResult> Parent { get; private set; }
        public string GuidGroup { get; private set; }
        public string Guid { get; private set; }
        public bool Completed { get; private set; }
        public bool Canceled { get; internal set; } = false;
        public bool Runed { get; private set; } = false;
        public ErrorExceptionParallelProcessResult<TResult> ResultValue { get; private set; } = new ErrorExceptionParallelProcessResult<TResult>();

        private Expression<Func<object, TResult>> Act { get; set; }

        public CancellationTokenSource CancellationToken { get; private set; }

        public void Register(string guid, Expression<Func<object, TResult>> act)
        {
            this.Guid = guid;
            Act = act;

        }
        public void Register(string guid, string guidGroup, Expression<Func<object, TResult>> act)
        {
            this.Guid = guid;
            this.GuidGroup = guidGroup;
            Act = act;

        }

        private void runProcess(object state)
        {
            try
            {
                ResultValue.ResultTime = new Stopwatch();
                ResultValue.ResultTime.Start();
                var compile = Act.Compile();
                var result = compile.Invoke(new object());
                ResultValue.Value = result;
                ResultValue.IsSuccess = true;
                ResultValue.ResultTime.Stop();
            }
            catch (Exception e)
            {
                ResultValue.IsSuccess = false;
                ResultValue.Error = e;
            }
            Completed = true;
            Parent.CheckAllProcessCompleted();
        }

        internal void GotoRun()
        {
            if (Canceled)
                return;
            this.CancellationToken = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(runProcess, CancellationToken.Token);
            this.Runed = true;



        }
    }
}
