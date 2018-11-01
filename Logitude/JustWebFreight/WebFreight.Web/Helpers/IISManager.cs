using Microsoft.Web.Administration;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class IISManager : I_IISManager
    {
        public void RecycleMe()// require admin user !!!
        {
            try
            {

                var SiteName = ConfigurationManager.AppSettings["20181101.OnMaxPoolRecycle.SiteName"];
                if (string.IsNullOrWhiteSpace(SiteName))
                {
                    Debug.WriteLine("RecycleMe:ConfigurationManager.AppSettings[20181101.OnMaxPoolRecycle.SiteName]is null -- Abort  ");
                    return;
                }

                var uri = new Uri(LogitudeSettings.LogitudeURL);
                var branchEnv = uri.LocalPath.Trim(@"\"[0]).Trim(@"/"[0]);


                using (ServerManager iisManager = new ServerManager())
                {
                    SiteCollection sites = iisManager.Sites;
                    foreach (Site site in sites)
                    {

                        if (site.Name == SiteName)
                        {
                            iisManager.ApplicationPools[site.Applications["/"].ApplicationPoolName].Recycle();
                            LogitudeSettings.HandleLogMe($"SiteName({SiteName}).Recycle() ", false, "RecycleMe", DateTime.MaxValue);
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {

                //throw;
            }
        }


    }

    
}