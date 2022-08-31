using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
   public class SharedLogisticsQuery
    {
  

       public SharedLogisticsPM GetSinglePM(int tenant ,string systemURL)
        {
            string styleLink = "'font-family:Arial;font-size:18px;color:#0000FF'";
            string url = systemURL;
            if (systemURL.Contains("login.aspx"))
            {
                string[] test = systemURL.Split('/');
                url = systemURL.Replace("/" + test[test.Length - 1], "");
            }
            string cargoTrackingPath = GetCargoTrackingUrlPath(url);
            string brandingURLPath = GetBrandingURLPath(systemURL);
            string brandingURLButton = "<button "  + " style='cursor: pointer;Background-color:" + GetBrandingURLBackgroundButton (tenant) + ";width:140px;height:30px;border-color:#1890ff;border-radius:5px;border:0px;color:white'>  join </button>";
            return new SharedLogisticsPM()
            {
                SystemURL = "<a style=" + styleLink + " href='" + systemURL + "'" + ">" + url + "</a>",
                InvitationEmail = "[InvitationEmail]",
                InvitationPassword = "[InvitationPassword]",
                IOSAppLink = "<a style=" + styleLink + " href='" + LogitudeSettings.IOSAppLink + "'" + "><img  width='120' height='40' src='cid:AppleStore' /></a>",
                AndroidAppLink = "<a style=" + styleLink + " href='" + LogitudeSettings.AndroidAppLink + "'" + "><img  width='120' height='40' src='cid:GooglePlay' /></a>",
                ResetPasswordURL = "<a style=" + styleLink + " href='" + url + "[ResetPasswordURL]" + "'" + ">Reset my Password</a>",
                InviteeName = "[InviteeName]",
                URLprivateCargoTracking = "<a style=" + styleLink + " href='" + cargoTrackingPath + "'" + ">" + cargoTrackingPath + "</a>",
                BrandingURL = "<a style=" + styleLink + " href='" + brandingURLPath + "'" + ">" + brandingURLPath + "</a>",
                BrandingURLButton = "<a style=" + styleLink + " href='" + brandingURLPath + "'" + ">" + brandingURLButton + "</a>",
            };
        }

        private static string GetCargoTrackingUrlPath(string url)
        {
            string cargoURL = url;
            cargoURL = cargoURL.Replace("https://", "");
            string cargoTrackingPath = "https://" + cargoURL.Split('/')[0] + "/CargoTracking/cargo-tracking/login";
            return cargoTrackingPath;
        }
        private static string GetBrandingURLPath(string systemURL)
        {
            string brandingURLPath = "https://" + systemURL + "/login";
            return brandingURLPath;
        }



        private static string GetBrandingURLBackgroundButton(int tenant)
        {
            string defultColorhex = "#1890ff";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantManagementPM(tenant);
                return (tenantManagementPM != null && !string.IsNullOrEmpty(tenantManagementPM?.SecondaryColor)) ? tenantManagementPM.SecondaryColor : defultColorhex;  
                scope.Complete();
            }
        }

    }
}
