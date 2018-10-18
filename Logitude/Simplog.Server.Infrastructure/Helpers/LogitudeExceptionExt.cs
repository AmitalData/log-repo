using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
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

    public static ExceptionInErrorLog HandleException(this Exception exception, //DateTime clientDate, int tenant, string userId, string userName, string ExtraMessage, string ip)
        string TypeOrUser, string ExtraMessage)
    {
        if (!LogitudeSettings.IsCostomsDeploy)
        {
            LogitudeSettings.HandleDbExceptionInject(exception, TypeOrUser, ExtraMessage);
            return null;
        }

        var myErrorlogLDC = new ExceptionInErrorLog();
        using (var myScope = new LogtitudeDomainScope(myErrorlogLDC))
        {
            LogitudeSettings.HandleDbExceptionInject(exception, TypeOrUser, ExtraMessage);
            if (!String.IsNullOrWhiteSpace(myErrorlogLDC.ErrorlogId))
            {
                return myErrorlogLDC;
            }
            return null;
        }
    }

}


//}
