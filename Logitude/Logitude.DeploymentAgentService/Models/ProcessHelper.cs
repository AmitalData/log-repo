using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class ProcessHelper
    {
        private readonly string FilePath;
        private readonly string Arguments;
        private readonly string WorkingDirectory;
        private readonly Encoding DataReceivedEncoding;

        private string OutputDataReceived = "";
        private string ErrorDataReceived = "";

        public ProcessHelper(string filePath, string arguments, string workingDirectory, Encoding dataReceivedEncoding)
        {
            FilePath = filePath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
            DataReceivedEncoding = dataReceivedEncoding;
        }

        public ProcessRunResult RunProcess()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(FilePath)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                if (!String.IsNullOrEmpty(WorkingDirectory))
                {
                    startInfo.WorkingDirectory = WorkingDirectory;
                }

                if (!String.IsNullOrEmpty(Arguments))
                {
                    startInfo.Arguments = Arguments;
                }

                if (DataReceivedEncoding != null)
                {
                    startInfo.StandardOutputEncoding = DataReceivedEncoding;
                    startInfo.StandardErrorEncoding = DataReceivedEncoding;
                }

                Process process = new Process
                {
                    StartInfo = startInfo
                };

                process.OutputDataReceived += ReceiveOutputData;
                process.ErrorDataReceived += ReceiveErrorData;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                return new ProcessRunResult
                {
                    ProcessResult = new ProcessResult
                    {
                        ExitCode = process.ExitCode,
                        OutputDataReceived = OutputDataReceived.TrimEnd('\n'),
                        ErrorDataReceived = ErrorDataReceived.TrimEnd('\n')
                    }
                };
            }
            catch (Exception exception)
            {
                return new ProcessRunResult
                {
                    ProcessResult = null,
                    Exception = exception.ToString(),
                    ExceptionMessage = exception.Message
                };
            }
        }

        private void ReceiveOutputData(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived += e.Data + "\n";
        }

        private void ReceiveErrorData(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived += e.Data + "\n";
        }
    }
}