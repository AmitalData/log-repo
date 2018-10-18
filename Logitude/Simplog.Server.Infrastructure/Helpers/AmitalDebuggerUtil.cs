using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Simplog.Server.Infrastructure.Helpers
//{
    public enum AmitalDebuggerLevel:int
    {
        None = 0,
        Critical = 1,

        Error = 2,

        Warning = 4,

        Information = 8,

        Verbose = 16
    }
    public static class AmitalDebuggerUtil
    {
        public static AmitalDebuggerLevel? MyDebuggerLevel { get; set; }

        public static void Break(AmitalDebuggerLevel CurrentBreak = AmitalDebuggerLevel.Verbose)
        {   
            if (!Debugger.IsAttached) return;
            if (AmitalDebuggerUtil.MyDebuggerLevel == null)
            {
                AmitalDebuggerUtil.MyDebuggerLevel = AmitalDebuggerLevel.Information;
                AmitalDebuggerUtil.MyDebuggerLevel = AmitalDebuggerLevel.Verbose;

                Debug.WriteLine(@"to filter Debug try this
AmitalDebuggerUtil.MyDebuggerLevel = AmitalDebuggerLevel.Information;");
               System.Diagnostics.Debugger.Break();
            }
            if (AmitalDebuggerUtil.MyDebuggerLevel < CurrentBreak)
            {
                return;
            }
            //Debugger.Break();
        }
    }
///}
