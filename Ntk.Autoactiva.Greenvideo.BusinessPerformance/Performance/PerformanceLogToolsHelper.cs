using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces;
using System;
using System.Diagnostics;
using System.Text;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Performance
{
    public static class PerformanceLogToolsHelper
    {

        public static IPerformanceLogTools performanceLogTools = Config.BusinessPerformanceIocConfig.IocConfig.GetInstancePerformanceLogTools();
        public static string ErrorToolsHelperExceptionConvertor(this Exception exception)
        {

            if (exception == null)
                return "";

            try
            {
                Exception e = exception;
                StringBuilder s = new StringBuilder();
                while (e != null)
                {
                    s.AppendLine("Exception type: " + e.GetType().FullName);
                    s.AppendLine("Message       : " + e.Message);
                    s.AppendLine("Stacktrace: " + e.StackTrace);
                    s.AppendLine("-NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK-");
                    s.AppendLine();
                    e = e.InnerException;
                }
                return s.ToString();
            }
            catch
            {
                return "";
            }
        }
        public static string ErrorToolsHelperExceptionConvertorFull(this Exception exception)
        {
            if (exception == null)
                return "";

            try
            {
                Exception e = exception;
                StringBuilder s = new StringBuilder();
                while (e != null)
                {
                    s.AppendLine("Exception type: " + e.GetType().FullName);
                    s.AppendLine("Message       : " + e.Message);
                    s.AppendLine("Stacktrace: " + e.StackTrace);
                    s.AppendLine("-NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK- -NTK-");
                    s.AppendLine();
                    e = e.InnerException;
                }
                return s.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static void ErrorToolsHelperElmahSaveLogErrorManually(string Title, string Description)
        {
            var exception = new Exception();
            exception = new Exception(Title, new Exception(Description));
            exception.ErrorToolsHelperElmahSaveLogErrorManually();
        }
        public static void ErrorToolsHelperElmahSaveLogErrorManually(this Exception ex, string contextualMessage = "")
        {
            if (ex == null)
                return;

            if (!string.IsNullOrEmpty(ex.Message) &&
                (ex.Message.ToLower().IndexOf("bot was blocked by the user") >= 0 || ex.Message.ToLower().IndexOf("user is deactivated") >= 0))
                return;

            performanceLogTools.PerformanceHelperError(ex, contextualMessage);
        }
        public static bool PerformanceHelperInfoTimeToRun(this Stopwatch sw, int MinAllowToRunLog)
        {
            sw.Stop();
            if (MinAllowToRunLog > 0 && sw.ElapsedMilliseconds > MinAllowToRunLog)
                return true;
            return false;

        }
        public static void PerformanceHelperInfoTime(this Stopwatch sw, string str)
        {
            performanceLogTools.PerformanceHelperTrace("Stopwatch Elapsed: (" + sw.ElapsedMilliseconds + ")   " + str);
        }

        public static void PerformanceHelperTrace(string str)
        {
            performanceLogTools.PerformanceHelperTrace(str);
        }
        public static void PerformanceHelperDebug(string str)
        {
            performanceLogTools.PerformanceHelperDebug(str);
        }
        public static void PerformanceHelperInfo(string str)
        {
            performanceLogTools.PerformanceHelperInfo(str);
        }
        public static void PerformanceHelperWarn(string str)
        {
            performanceLogTools.PerformanceHelperWarn(str);
        }
        public static void PerformanceHelperError(string str)
        {
            performanceLogTools.PerformanceHelperError(str);
        }
        public static void PerformanceHelperFatal(string str)
        {
            performanceLogTools.PerformanceHelperFatal(str);
        }
    }
}
