using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.Helpers
{
    public static class LogitudeSettingConfigration
    {
        public static string GetWorkEnvironment()
        {
            if (string.IsNullOrEmpty(LogitudeSettings.WorkEnvironment)) return "logitude";
            return IsLogBoxEnvironment() ? GetLogboxWorkEnvironment() : LogitudeSettings.WorkEnvironment;
        }

        public static bool IsLogBoxEnvironment()
        {
            return (LogitudeSettings.DeploymentStage != null && LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1") || LogitudeSettings.WorkEnvironment?.ToLower() == "logbox";
        }

        private static string GetLogboxWorkEnvironment()
        {
            return HttpContext.Current.Request.Url.Host.ToLower().Contains(".logbox.") ? "logbox" : "privatelabel";
        }
    }
}
