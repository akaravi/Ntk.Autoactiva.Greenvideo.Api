using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Model;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.ParallelProcess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Performance
{
    public class ParallelProcess<TResult> : IPerformanceParallelProcess
    {
        private List<ParallelProcessRegistered<TResult>> ProcessesList;
        public Func<ParallelProcessRegistered<TResult>, bool> InfoProcess;
        public Func<bool, bool> InfoProcessCompleted { get; set; }
        public void RegisterProcess(Action<ParallelProcessRegistered<TResult>> act)
        {
            ParallelProcessRegistered<TResult> obj = new ParallelProcessRegistered<TResult>(this);
            if (ProcessesList == null)
                ProcessesList = new List<ParallelProcessRegistered<TResult>>();
            ProcessesList.Add(obj);
            act.Invoke(obj);
        }


        public void RunAll(bool InBackgrand = false)
        {
            foreach (var item in ProcessesList)
            {
                if (!item.Runed)
                {
                    if (InfoProcess != null)
                        InfoProcess(item);
                    item.GotoRun();
                    if (InfoProcess != null)
                        InfoProcess(item);
                }
            }
            if (InBackgrand)
                return;
            while (!CheckAllProcessCompleted()) ;
        }

        public void Cancel(string guidKey)
        {
            if (ProcessesList == null)
                return;
            var firstItem = ProcessesList.FirstOrDefault(x => x.Guid == guidKey);
            if (firstItem != null)
            {
                if (!firstItem.Runed)
                {
                    firstItem.Canceled = true;

                }
                else if (!firstItem.Completed)
                {
                    firstItem.CancellationToken.Cancel();
                    firstItem.Canceled = true;

                }
            }
        }
        public void CancelGroup(string guidKey)
        {
            if (ProcessesList == null)
                return;
            var ListItem = ProcessesList.Where(x => x.GuidGroup == guidKey).ToList();
            if (ListItem != null)
            {
                foreach (var item in ListItem)
                {

                    if (!item.Runed)
                    {
                        item.Canceled = true;
                    }
                    else if (!item.Completed)
                    {
                        item.CancellationToken.Cancel();
                        item.Canceled = true;
                    }
                }
            }
        }
        private Boolean RunCompletedExecuted;

        public ErrorExceptionParallelProcessResult<TResult> GetResult(string guidKey)
        {
            var firstItem = ProcessesList.FirstOrDefault(x => x.Guid == guidKey);
            if (firstItem != null)
                return firstItem.ResultValue;
            return new ErrorExceptionParallelProcessResult<TResult>() { IsSuccess = false, };
        }

        public Boolean CheckAllProcessCompleted()
        {
            Boolean runCompleted = true;
            foreach (var item in ProcessesList)
            {
                if (!item.Completed)
                    runCompleted = false;
            }
            if (runCompleted)
            {
                if (RunCompletedExecuted)
                    return true;
                RunCompletedExecuted = true;
            }
            return runCompleted;
        }

        public void Dispose()
        {
            ProcessesList.Clear();
        }
    }
}
