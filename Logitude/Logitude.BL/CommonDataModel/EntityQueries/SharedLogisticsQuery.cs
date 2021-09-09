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
            string cargoTrackingPath = url + "/cargo-tracking/login";
            return new SharedLogisticsPM()
            {
                SystemURL = "<a style=" + styleLink + " href='" + systemURL + "'" + ">" + url  + "</a>",
                InvitationEmail = "[InvitationEmail]",
                InvitationPassword ="[InvitationPassword]",
                IOSAppLink = "<a style=" + styleLink + " href='" + LogitudeSettings.IOSAppLink + "'" + "><img  width='120' height='40' src='cid:AppleStore' /></a>",
                AndroidAppLink = "<a style=" + styleLink + " href='" + LogitudeSettings.AndroidAppLink + "'" + "><img  width='120' height='40' src='cid:GooglePlay' /></a>",
                ResetPasswordURL = "<a style=" + styleLink + " href='" + url + "[ResetPasswordURL]" + "'" + ">Reset my Password</a>", 
                InviteeName = "[InviteeName]",
                CargoTrackingURL = "<a style=" + styleLink + " href='" + cargoTrackingPath + "'" + ">" + cargoTrackingPath + "</a>",
            };
        }

    }
}
