using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Performance
{
    public static class PerformanceTaskManagerHelper
    {
        public static IPerformanceTaskManager performanceTaskManager = Config.BusinessPerformanceIocConfig.IocConfig.GetInstancePerformanceTaskManager();

        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        public static string TaskHangfireHelperEnqueue(Expression<Func<Task>> methodCall)
        {
            return performanceTaskManager.Enqueue(methodCall);
        }
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        public static string TaskHangfireHelperEnqueue(Expression<Action> methodCall)
        {
            return performanceTaskManager.Enqueue(methodCall);
        }
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        public static string TaskHangfireHelperEnqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            return performanceTaskManager.Enqueue(methodCall);
        }
        //
        // Summary:
        //     Creates a new fire-and-forget job based on a given method call expression.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to a server.
        //
        // Returns:
        //     Unique identifier of a background job.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     methodCall is null.
        public static string TaskHangfireHelperEnqueue<T>(Expression<Action<T>> methodCall)
        {
            return performanceTaskManager.Enqueue(methodCall);
        }
        #region Schedule
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     The type whose method will be invoked during the job processing.
        //
        // Returns:
        //     Unique identifier of a created job.
        public static string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
        {
            return performanceTaskManager.Schedule<T>(methodCall, enqueueAt);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified instance method call expression
        //     and schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     Type whose method will be invoked during job processing.
        //
        // Returns:
        //     Unique identifier of the created job.
        public static string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            return performanceTaskManager.Schedule<T>(methodCall, delay);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified instance method call expression
        //     and schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     Type whose method will be invoked during job processing.
        //
        // Returns:
        //     Unique identifier of the created job.
        public static string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return performanceTaskManager.Schedule<T>(methodCall, delay);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of a created job.
        public static string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return performanceTaskManager.Schedule(methodCall, enqueueAt);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of a created job.
        public static string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            return performanceTaskManager.Schedule(methodCall, enqueueAt);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of the created job.
        public static string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return performanceTaskManager.Schedule(methodCall, delay);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued after a given delay.
        //
        // Parameters:
        //   methodCall:
        //     Instance method call expression that will be marshalled to the Server.
        //
        //   delay:
        //     Delay, after which the job will be enqueued.
        //
        // Returns:
        //     Unique identifier of the created job.
        public static string Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            return performanceTaskManager.Schedule(methodCall, delay);
        }
        //
        // Summary:
        //     Creates a new background job based on a specified method call expression and
        //     schedules it to be enqueued at the given moment of time.
        //
        // Parameters:
        //   methodCall:
        //     Method call expression that will be marshalled to the Server.
        //
        //   enqueueAt:
        //     The moment of time at which the job will be enqueued.
        //
        // Type parameters:
        //   T:
        //     The type whose method will be invoked during the job processing.
        //
        // Returns:
        //     Unique identifier of a created job.
        public static string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return performanceTaskManager.Schedule<T>(methodCall, enqueueAt);
        }
        //
        // Summary:
        //     Changes state of a job with the specified jobId to the Hangfire.States.DeletedState.
        //     Hangfire.BackgroundJobClientExtensions.Delete(Hangfire.IBackgroundJobClient,System.String)
        //
        // Parameters:
        //   jobId:
        //     An identifier, that will be used to find a job.
        //
        // Returns:
        //     True on a successfull state transition, false otherwise.
        public static bool Delete(string jobId)
        {
            return performanceTaskManager.Delete(jobId);

        }
        #endregion Schedule
        #region aa
        public static string TaskSchedulerHangfireHelperRemoveIfExists(string recurringJobId)
        {
            ////if (Config.BusinessPerformanceIocConfig.IocConfig.GetCmsConfiguration().ApplicationMemory.TaskHangfireStarted)
                performanceTaskManager.RemoveIfExists(recurringJobId);
            return recurringJobId;
        }
        public static string TaskSchedulerHangfireHelperAddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression)
        {
            //AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");
            ///if (Config.BusinessPerformanceIocConfig.IocConfig.GetCmsConfiguration().ApplicationMemory.TaskHangfireStarted)
                performanceTaskManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            return recurringJobId;
        }

        public static string TaskSchedulerHangfireHelperAddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression)
        {
            ////if (Config.BusinessPerformanceIocConfig.IocConfig.GetCmsConfiguration().ApplicationMemory.TaskHangfireStarted)
                //performanceTaskManager.AddOrUpdate(recurringJobId, methodCall, Cron.Hourly);
                performanceTaskManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            return recurringJobId;
        }
        public static string TaskSchedulerHangfireHelperAddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression)
        {
            //if (Config.BusinessPerformanceIocConfig.IocConfig.GetCmsConfiguration().ApplicationMemory.TaskHangfireStarted)
                performanceTaskManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            return recurringJobId;
        }
        public static string TaskSchedulerHangfireHelperAddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression)
        {
            //if (Config.BusinessPerformanceIocConfig.IocConfig.GetCmsConfiguration().ApplicationMemory.TaskHangfireStarted)
                performanceTaskManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            return recurringJobId;
        }
        #endregion
    }
}
