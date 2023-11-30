using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.WebServices;

namespace WebFreight.Web
{
    public partial class PasswordResetRequestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPartnerEnvironment();
        }

        private void SetPartnerEnvironment()
        {
            if (Request != null)
            {
                string myPartner = Request.QueryString["partner"];

                if (string.IsNullOrEmpty(myPartner))
                {
                    if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2"))
                    {
                        myPartner = "logbox";
                    }
                    else myPartner = "logitude";
                    if (LogitudeSettings.WorkEnvironment == "cloud") myPartner = "cloud";
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