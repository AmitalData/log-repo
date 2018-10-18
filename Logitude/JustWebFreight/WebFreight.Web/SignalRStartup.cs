using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Simplog.Server.Infrastructure;
using System.Diagnostics;
using System.Threading;
using Logitude.SystemLogs;
using System.Threading.Tasks;
using Logitude.Server.Tools.ExternalServices;
using Microsoft.AspNet.SignalR;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Azure;

[assembly: OwinStartup(typeof(WebFreight.Web.SignalRStartup))]

namespace WebFreight.Web
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                string SignalRStartupSuppres = System.Configuration.ConfigurationManager.AppSettings.Get("SignalRStartupSuppres");
                if (!string.IsNullOrWhiteSpace(SignalRStartupSuppres))
                {
                    return;
                }
                HubConfiguration hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;
                app.MapSignalR(hubConfiguration);
               // https://logitudetest2.servicebus.windows.net/signalrhub
                app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                //StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), StorageAcountDetails.SignalRHubTopicName, subscribtionName);
               // GlobalHost.DependencyResolver.UseServiceBus(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), "SignalRHub");
                app.MapSignalR();

                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());
                    app.MapSignalR();
                    return;
                }
                
                var host = SignatureHubClient.GetHost(LogitudeSettings.LogitudeURL);
                if (string.IsNullOrWhiteSpace(host))
                {
                    return;
                }

              
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    Logitude.Server.Tools.ExternalServices.SignatureHubClient.Create(host, null);
                });
                //Logitude.Server.Tools.ExternalServices.SignatureHubClient.Instance.Send("Test", "-9999");
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WebRole", "SignalRStartup : Configuration", null);

            }
        }

 
    }
}