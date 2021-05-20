
using System;

namespace Ntk.Autoactiva.Greenvideo.BusinessPerformance.Interfaces
{
    public interface IPerformanceLogTools
    {

        void PerformanceHelperTrace(string memo);
        void PerformanceHelperDebug(string memo);
        void PerformanceHelperInfo(string memo);
        void PerformanceHelperWarn(string memo);
        void PerformanceHelperError(string memo);
        void PerformanceHelperError(Exception exception, string contextualMessage);
        void PerformanceHelperFatal(string memo);
    }
}