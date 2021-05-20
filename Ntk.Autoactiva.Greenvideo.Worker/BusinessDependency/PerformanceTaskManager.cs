using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;


namespace Ntk.Autoactiva.Greenvideo.Worker.BusinessDependency
{
    public class PerformanceTaskManager : IPerformanceTaskManager
    {
        public void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression)
        {
           
        }

        public void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression)
        {
            
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression)
        {
          
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression)
        {
           
        }

        public bool Delete(string jobId)
        {
            return true;
        }

        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return "";
        }

        public string Enqueue(Expression<Action> methodCall)
        {
          
            return "";
        }

        public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            return "";
        }

        public string Enqueue<T>(Expression<Action<T>> methodCall)
        {
            return "";
        }

        public void RemoveIfExists(string recurringJobId)
        {
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
        {
            return "";
        }

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            return "";
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return "";
        }

        public string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return "";
        }

        public string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            return "";
        }

        public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return "";

        }

        public string Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            return "";

        }

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return "";
        }
    }
}
