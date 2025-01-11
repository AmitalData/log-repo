using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.DBHelpers
{
    public static class LogitudeExceptionExtU
    {
        public static void ChangeExceptionMess(this Exception ex, string messagePrefix)
        {
            var mess = ex.Message;
            ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, messagePrefix + " " + mess);
        }
    }

}
