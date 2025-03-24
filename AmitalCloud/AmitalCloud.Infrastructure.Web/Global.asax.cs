using AmitalCloud.Shipment.WebAPI;
using System.Web;
using System;
using System.Web.Http;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Helpers;
using System.Globalization;
using AmitalCloud.Infrastructure.Data.Security;

namespace AmitalCloud.Infrastructure.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (string.IsNullOrEmpty(AmitalCloudSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                AmitalCloudSettings.DatabaseManagementSystem = dbms;
                AmitalCloudSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
            }

            CacheManager.CacheWrapper = new CacheWrapper(HttpContext.Current.Cache);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            //InfraRegistrationHelper.Register();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            if (!string.IsNullOrEmpty(token))
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken != null)
                {
                    HttpContext.Current.Items.Add("authToken", authToken);
                    HttpContext.Current.Items.Add("Tenant", authToken.Tenant);

                    if (!authToken.APIToken)
                    {
                        if (!authToken.InActive)
                        {
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
                        }
                    }
                }
            }
        }

        bool _IAmDebuging_StopOpenNewThreads = false;

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // because of the preflight request (CORS)
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 200;
                Response.End();
            }

            if (AmitalCloudSettings.IsCostomsDeploy)
            {
                var he = new CultureInfo("he-IL");
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";
                System.Threading.Thread.CurrentThread.CurrentCulture = he;

                if (_IAmDebuging_StopOpenNewThreads)
                {
                    HttpContext.Current.Response.End();
                }
            }

            string clientmode = System.Configuration.ConfigurationManager.AppSettings.Get("clientMode");
            if (clientmode == "angular" && SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development)) //islam: please don't remark this !!!!!!!!!!!!!!
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "http://localhost:4200");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }

            //These headers are handling the "pre-flight" OPTIONS call sent by the browser
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, token,ServerTime,TwoFactorkey");
            HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
            HttpContext.Current.Response.AddHeader("Strict-Transport-Security", "max-age=31536000 ; includeSubDomains");

            var systemUrl = AmitalCloudSecurityUtility.getLoggedDomain();
            if (!string.IsNullOrEmpty(systemUrl) && systemUrl.ToLower().Contains("staging") && !SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.LogboxAndAccountingProduction))
            {
                HttpContext.Current.Items.Add("workerrolename", "staging");
                return;
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePath) && HttpContext.Current.Request.CurrentExecutionFilePath.ToLower().Contains("/wcfapi/"))
            {
                if (!HttpContext.Current.Items.Contains("workerrolename"))
                    HttpContext.Current.Items.Add("workerrolename", "production");
            }
            else
            {
                if (HttpContext.Current.Request.Headers["workerrolename"] != null)
                {
                    if (!HttpContext.Current.Items.Contains("workerrolename"))
                        HttpContext.Current.Items.Add("workerrolename", HttpContext.Current.Request.Headers["workerrolename"]);
                    else
                        HttpContext.Current.Items["workerrolename"] = HttpContext.Current.Request.Headers["workerrolename"];
                }
            }
        }

    }
}
