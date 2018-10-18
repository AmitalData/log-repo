#if move2Infra
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//namespace Logitude.Server.Tools
//{

    public static class LogitudeExceptionExt
    {
        public static void ChangeExceptionMessage(this Exception ex, string messagePrefix)
        {
            var mess = ex.Message;
            ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, messagePrefix + " " + mess);
        }


        
    }

  
//}
#endif