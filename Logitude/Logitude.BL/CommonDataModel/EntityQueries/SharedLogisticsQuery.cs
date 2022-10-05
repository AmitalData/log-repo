using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
            string resetPasswordURLPath = GetResetPasswordPath(systemURL);

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
                BrandingURLButton = GetBrandingURLButton(tenant, brandingURLPath),
                ResetPasswordButton = GetResetPasswordButton(tenant, resetPasswordURLPath),
            };
        }




        private static string GetBrandingURLBackgroundButton(int tenant)
        {
            string brandingBackgroundColor = string.Empty;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantManagementPM(tenant);
                brandingBackgroundColor = tenantManagementPM?.SecondaryColor;
                scope.Complete();
            }
            return ConvertRgbaToHexColor(brandingBackgroundColor);
        }


        private static string ConvertRgbaToHexColor(string rgbaColor)
        {
            string defultBlueColor = "#1890ff";
            try
            {
                if (string.IsNullOrEmpty(rgbaColor) || string.IsNullOrWhiteSpace(rgbaColor)) return defultBlueColor;
                if (!rgbaColor.ToLower().Contains("rgba")) return rgbaColor;
                var colorVlues = rgbaColor.Replace("rgba(", "").Replace(")", "").Split(',');
                if (colorVlues.Length < 3) return defultBlueColor;
                int red = int.Parse(colorVlues[0], CultureInfo.InvariantCulture);
                int green = int.Parse(colorVlues[1], CultureInfo.InvariantCulture);
                int blue = int.Parse(colorVlues[2], CultureInfo.InvariantCulture);
                return "#" + Color.FromArgb(red, green, blue).Name.Substring(2);
            }
            catch (Exception exception)
            {
                return defultBlueColor;
            }
        }


        public string GetBrandingURLButton(int tenant, string brandingURLPath)
        {
            string brandingBackgroundColor = GetBrandingURLBackgroundButton(tenant);
            return " <table " + "style='margin: 0 auto;cursor: pointer;width:256px;height:30px;border-color:" + brandingBackgroundColor + ";border-radius:5px;border:0px;color:white'" + " width ='256px'  bgcolor='" + brandingBackgroundColor + "' border='0'  cellspacing='0' cellpadding='0'>" +
                "<tr>" +
                "<td align='center'  style='padding: 8px 12px; border-radius: 2px;'>" +
                "<a  style='font-weight: 500; font-size: 14px;text-decoration: none; padding: 0px; display: inline-block; color: #ffffff'" + " href='" + brandingURLPath + "'" + " > Login </ a >" +
                "</ td >" +
                "</ tr >" +
                "</ table >";
        }

        public string GetResetPasswordButton(int tenant, string url)
        {
            string brandingBackgroundColor = GetBrandingURLBackgroundButton(tenant);
            return " <table " + "style='margin: 0 auto;cursor: pointer;width:256px;height:30px;border-color:" + brandingBackgroundColor + ";border-radius:5px;border:0px;color:white'" + " width ='256px'  bgcolor='" + brandingBackgroundColor + "' border='0'  cellspacing='0' cellpadding='0'>" +
                "<tr>" +
                "<td align='center'  style='padding: 8px 12px; border-radius: 2px;'>" +
                "<a  style='font-weight: 500; font-size: 14px;text-decoration: none; padding: 0px; display: inline-block; color: #ffffff'" + " href='" + url + "[ResetPasswordURL]" + "'" + " > Reset my password </ a >" +
                "</ td >" +
                "</ tr >" +
                "</ table >";
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

        private static string GetResetPasswordPath(string systemURL)
        {
            string brandingURLPath = "https://" + systemURL + "/resetForgotPassword";
            return brandingURLPath;
        }
    }
}
