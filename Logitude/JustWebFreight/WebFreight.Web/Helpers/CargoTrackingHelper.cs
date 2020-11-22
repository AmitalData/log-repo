using Logitude.BL.GlobalModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class CargoTrackingHelper
    {

        public static string SetBrandingLogo(TenantManagementPM tenantManagement)
        {
            Uploader uploaderService = new Uploader();
            byte[] logodata = uploaderService.DownloadFile("sharedLogtsitcslogo" + tenantManagement.Id, "png", "logos", tenantManagement.Id);
            if (logodata != null)
            {
                return "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(logodata);
            }
            else return null;
        }
    }
}