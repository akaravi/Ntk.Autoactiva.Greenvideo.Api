using Microsoft.AspNetCore.Http;
using NLog;
using System;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;

namespace Ntk.Autoactiva.Greenvideo.WebApi.BusinessDependency
{

    public class PerformanceLogTools : IPerformanceLogTools
    {
        private static NLog.ILogger Instance = LogManager.GetCurrentClassLogger();
        #region Save Error
        private HttpContext _httpContext;
        public void SetHttpContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public void SavErroreLog(Exception exception, string contextualMessage)
        {
            try
            {
                if (_httpContext != null)//website is logging the error
                {
                    if (!string.IsNullOrEmpty(contextualMessage))
                    {

                        var annotatedException = new Exception(contextualMessage, exception);
                        ElmahCore.ElmahExtensions.RiseError(_httpContext, annotatedException);

                    }
                    else
                    {
                        ElmahCore.ElmahExtensions.RiseError(_httpContext, exception);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(contextualMessage))
                    {

                        var annotatedException = new Exception(contextualMessage, exception);
                        ElmahCore.ElmahExtensions.RiseError(_httpContext, annotatedException);

                    }
                    else
                    {
                        ElmahCore.ElmahExtensions.RiseError(_httpContext, null);
                    }
                }
            }
            catch //(Exception e)
            {
                // uh oh!just keep going
            }
        }
        #endregion 
        public void PerformanceHelperTrace(string memo)
        {

            Instance.Trace(memo);

        }

        public void PerformanceHelperDebug(string memo)
        {

            Instance.Debug(memo);

        }

        public void PerformanceHelperInfo(string memo)
        {

            Instance.Info(memo);



        }

        public void PerformanceHelperWarn(string memo)
        {

            Instance.Warn(memo);
        }
        public void PerformanceHelperError(string memo)
        {

            Instance.Error(memo);
        }
        public void PerformanceHelperError(Exception exception, string contextualMessage)
        {
            try
            {
                Instance.Error(exception, contextualMessage);
            }
            catch
            {


            }
            try
            {
                SavErroreLog(exception, contextualMessage);
            }
            catch
            {


            }
        }

        public void PerformanceHelperFatal(string memo)
        {
            Instance.Fatal(memo);
        }

    }
   
}