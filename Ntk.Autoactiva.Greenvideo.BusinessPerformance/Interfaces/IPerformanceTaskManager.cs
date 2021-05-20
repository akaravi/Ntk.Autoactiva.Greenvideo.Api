using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces
{
    public interface IPerformanceTaskManager
    {
        string Enqueue(Expression<Func<Task>> methodCall);
        string Enqueue(Expression<Action> methodCall);
        string Enqueue<T>(Expression<Func<T, Task>> methodCall);
        string Enqueue<T>(Expression<Action<T>> methodCall);

        bool Delete(string jobId);
        void RemoveIfExists(string recurringJobId);
        void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression);
        void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression);
        void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression);
        void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression);
        string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);
        string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
        string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);
        string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt);
        string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);
        string Schedule(Expression<Action> methodCall, TimeSpan delay);
        string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);

    }
}
