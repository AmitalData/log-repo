using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityQueries;
using System.Collections.Generic;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Web.UI;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Helpers;
using System.IO;
using System.Xml.Serialization;

namespace WebFreight.Web
{
    public partial class Default : System.Web.UI.Page
    {
        public List<string> employees = new List<string>()
        {
            "1 - 4 employees","5 - 10 employees","11 - 20 employees",
            "21 - 30 employees",
            "More than 30"
        };
        List<TextCodePM> textCodeList;
        List<ObjectFieldPM> objectFieldPmsList;
        public Default() {
            textCodeList = new List<TextCodePM>();
            objectFieldPmsList = new List<ObjectFieldPM>();

        }
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

                    if (url.Contains("system.dsv.co.il"))
                    {
                        enableHttps = false;
                    }
                }
            }
            else
                enableHttps = false;

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            string oldIP = HttpContext.Current.Request.UserHostAddress;
            int LastIpPart = 0;
            if (string.IsNullOrEmpty(currentIP))
            {
                var IpParts = oldIP.Split('.');
                if (IpParts.Length == 4)
                {
                    int.TryParse(IpParts[3], out LastIpPart);
                    IGlobalContext objectContext = GlobalContext.GetContext();
                    var settingRepository = new SettingRepository(objectContext);
                    var settingQuery = new SettingQuery(settingRepository);
                    var settings = settingQuery.GetSinglePM();
                    if (settings != null && settings.System2RedirectFraction > 0 && LastIpPart != 0 && (LastIpPart % settings.System2RedirectFraction) == 0)
                    {
                        if (LogitudeSettings.DeploymentStage == "amitalstorage")
                        {
                            context.Response.Redirect("https://cloud2.amital.co.il");
                        }
                        else
                        {
                            context.Response.Redirect("https://system2.logitudeworld.com");
                        }
                    }
                }

            }
            if (enableHttps && LogitudeSettings.ForceHttps)
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