using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using Simplog.Data.CommonDataModel;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using System.IO.IsolatedStorage;
using System.IO;
using WebFreight.Web.WebServices;
using WebFreight.Web.GlobalModel;
using System.Web.Services;
using System.Windows;
using Simplog.Server.Infrastructure;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web
{
    public partial class Login : System.Web.UI.Page
    {
        //private static readonly string SimplogGuid = Guid.NewGuid().ToString("N");

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            string url = context.Request.Url.ToString().Split('/')[2];

            bool enableHttps = true;
            if (url.Contains("localhost:9996"))
            {
                enableHttps = false;
            }
            //logbox
            if (LogitudeSettings.DeploymentStage != "Dev" && LogitudeSettings.WorkEnvironment != "customs")
            {
                if (LogitudeSettings.WorkEnvironment == "logbox")
                {

                    //if (url.Contains("system.dsv.co.il"))
                    //{
                    //    //enableHttps = false;
                    //}
                }
            }
            else
                enableHttps = false;

            if (!IsPostBack && Request.RawUrl.Contains("?Menu=IdentityShaamLandingPage;"))
            {
                IGlobalContext objectContext = GlobalContext.GetContext();
                SettingRepository MySettingRepository = new SettingRepository(objectContext);
                SettingQuery MySettingQuery = new SettingQuery(MySettingRepository);
                var MySettings = MySettingQuery.GetSinglePM();

                string RedirectUrl = Request.IsLocal ? "http://localhost:4200" : "~/Angular" + MySettings.HtmlVersion + "/index.html";
                RedirectUrl += "?" + HttpUtility.UrlDecode(Request.QueryString.ToString());

                Response.Redirect(RedirectUrl);
            }

            //string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            //string oldIP = HttpContext.Current.Request.UserHostAddress;
            //int LastIpPart = 0;
            //if (string.IsNullOrEmpty(currentIP))
            //{
            //    var IpParts = oldIP.Split('.');
            //    if (IpParts.Length == 4)
            //    {
            //        int.TryParse(IpParts[3], out LastIpPart);
            //        IGlobalContext objectContext = GlobalContext.GetContext();
            //        var settingRepository = new SettingRepository(objectContext);
            //        var settingQuery = new SettingQuery(settingRepository);
            //        var settings = settingQuery.GetSinglePM();
            //        if (settings != null && settings.System2RedirectFraction > 0 && LastIpPart != 0 && (LastIpPart % settings.System2RedirectFraction) == 0)
            //        {
            //            if (LogitudeSettings.DeploymentStage == "amitalstorage")
            //            {
            //                context.Response.Redirect("https://cloud2.amital.co.il"); 
            //            }
            //            else
            //            {
            //                context.Response.Redirect("https://system2.logitudeworld.com");
            //            }
            //        }
            //    }

            //}
            //if (!string.IsNullOrEmpty(currentIP))
            //{
            //    LastIpPart = 0;
            //    var IpParts = currentIP.Split('.');
            //    if (IpParts.Length == 4)
            //    {
            //        int.TryParse(IpParts[3], out LastIpPart);
            //        IGlobalContext objectContext = GlobalContext.GetContext();
            //        var settingRepository = new SettingRepository(objectContext);
            //        var settingQuery = new SettingQuery(settingRepository);
            //        var settings = settingQuery.GetSinglePM();
            //        if (settings != null && settings.System2RedirectFraction > 0 && LastIpPart != 0 && (LastIpPart % settings.System2RedirectFraction) == 0)
            //        {
            //            if (LogitudeSettings.DeploymentStage == "logboxwe1" && !url.Contains("system2"))
            //            {
            //                if (url.Contains("system.dsv.co.il"))
            //                {
            //                    enableHttps = false;
            //                    context.Response.Redirect("http://system2.dsv.co.il");
            //                }
            //                else
            //                {
            //                    context.Response.Redirect("https://system2.logbox.co.il");
            //                }

            //            }
            //        }
            //    }
            //}
            string IsSecureConnection = context.Request.IsSecureConnection.ToString();
            if (context.Request.Headers.AllKeys.Contains("X-IsSecure"))
            {
                IsSecureConnection = context.Request.Headers["X-IsSecure"];
            } 
            if (IsSecureConnection != "true" && enableHttps && LogitudeSettings.ForceHttps)
            {
                SecurityUtility.RedirectToHttps();
            }

            this.SetPartnerEnvironment();
        }

        private void SetPartnerEnvironment()
        {
            if (Request != null)
            {
                string myPartner = Request.QueryString["partner"];

                if (string.IsNullOrEmpty(myPartner))
                {
                    myPartner = "logitude";
                }

                else
                {
                    myPartner = myPartner.ToLower();
                }

                if (PartnerEnvironmentInput != null)
                {
                    PartnerEnvironmentInput.Value = myPartner;
                }
            }
        }
    }
}