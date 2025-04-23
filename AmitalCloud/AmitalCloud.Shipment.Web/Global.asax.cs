using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using System;
using System.Web;
using System.Web.Http;

namespace AmitalCloud.Shipment.WebAPI
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
    }
}
