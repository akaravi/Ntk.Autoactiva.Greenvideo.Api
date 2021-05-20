using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Ntk.Autoactiva.Greenvideo.BusinessPerformance.Performance;

namespace Ntk.Autoactiva.Greenvideo.Core.Helper
{
    public class AssistantHelper
    {

        private DataReceivedEventHandler proc_ErrorDataReceived;
        private DataReceivedEventHandler proc_OutputDataReceived;

        public string Execute(string fileName, string WorkingDirectory, string Parameters, int timeOutMin = 0)
        {
            string retOut = "";
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo(fileName, Parameters);
            oInfo.WorkingDirectory = WorkingDirectory;
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

     

            //try the process
            try
            {
                //run the process
                Process proc = System.Diagnostics.Process.Start(oInfo);
                if (proc_ErrorDataReceived != null)
                    proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                if (proc_OutputDataReceived != null)
                    proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                if (timeOutMin == 0)
                    proc.WaitForExit();
                else
                    proc.WaitForExit(timeOutMin * 60 * 1000);
                
                proc.Close();
                proc.Dispose();
            }
            catch (Exception e)
            {
                // Capture Error
                retOut = e.ErrorToolsHelperExceptionConvertorFull();

            }
            finally
            {
                //now, if we succeeded, close out the streamreader
     
            }



            return retOut;
        }
        public string ExecuteCmd(string exePath, string WorkingDirectory, string command, int timeOutMin = 0)
        {
            string retOut = "";
            try
            {
                //#help# سر جدت دست به ین تظیمات نزن خیلی با دردسر تنظیم شده
                var processInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WorkingDirectory = WorkingDirectory,
                    FileName = "cmd.exe",
                    Arguments = "/c " + "\"" + command + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = false,//false
                    RedirectStandardError = true,//true
                    UseShellExecute = false,//false
                    CreateNoWindow = true,//true
                };
                var process = System.Diagnostics.Process.Start(processInfo);

                if (timeOutMin == 0)
                    process.WaitForExit();
                else
                    process.WaitForExit(timeOutMin * 60 * 1000);

                int exitCode = process.ExitCode;

                //string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                retOut = error;
            }
            catch (Exception e)
            {
                retOut = e.ErrorToolsHelperExceptionConvertorFull();
            }
            return retOut;
        }
    }

}
