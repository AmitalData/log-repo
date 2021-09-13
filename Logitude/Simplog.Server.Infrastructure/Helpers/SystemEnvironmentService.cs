using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class SystemEnvironmentService
    {

        public static string GetLoggedDomain()
        {
            HttpContext context = HttpContext.Current;
            string Url = context.Request.Url.ToString().Split('/')[2];//("http://", "");
            Url = Url.Split(':')[0]; 
            return Url;
        }

        public static bool IsLogBox()
        {
            string url = GetLoggedDomain();
            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxpre" || LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2"))
            {
                if (url.Contains("system.logbox.co.il") || url.Contains("test.logitudeworld.com") || url.Contains("pre.logbox.co.il"))
                {
                    return true;
                }
            }
            return false;
        }
         
    }
}
