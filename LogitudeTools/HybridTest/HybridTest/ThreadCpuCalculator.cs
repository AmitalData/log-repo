using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HypredTest
{
    public class ThreadCpuCalculator
    {
        public static ProcessThread GetProcessThreadFromWin32ThreadId(int? threadId)
        {
            if (!threadId.HasValue)
            {
                threadId = GetCurrentWin32ThreadId();
            }

            foreach (Process process in Process.GetProcesses())
            {
                foreach (ProcessThread processThread in process.Threads)
                {
                    if (processThread.Id == threadId) return processThread;
                }
            }

            throw new Exception();
        }

        [DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        public static extern Int32 GetCurrentWin32ThreadId();
    }
}
