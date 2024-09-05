using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web
{
    public partial class PasswordChangePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            string url = context.Request.Url.ToString().Split('/')[2];

            bool enableHttps = true;
            
            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development))
            {
                if (LogitudeSettings.WorkEnvironment == "logbox")
                {
                    var isAppServiceENV = Environment.GetEnvironmentVariable("IsAppService") == "true";
                    bool isAppService = ConfigurationManager.AppSettings["IsAppService"] == "true";

                    if (isAppServiceENV || isAppService)
                    {
                        if (!string.IsNullOrEmpty(context.Request.Headers["X-ORIGINAL-HOST"]))
                            url = context.Request.Headers["X-ORIGINAL-HOST"];
                    }

                    if (url.Contains("system.dsv.co.il"))
                    {
                        enableHttps = false;
                    }
                }
            }

            if (enableHttps && LogitudeSettings.ForceHttps)
            {
                SecurityUtility.RedirectToHttps();
            }

            
        }
        

      
    }
}