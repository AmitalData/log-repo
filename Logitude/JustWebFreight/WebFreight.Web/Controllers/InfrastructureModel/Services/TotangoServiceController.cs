using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.InfrastructureModel.Services
{
    public class TotangoServiceController : ApiController
    {
         
        // POST api/<controller>
        public void Post(TotangoActivityInfo activityInfo)
        {
            ActivityLog.AddContactActivityWithTotango(activityInfo.OrganizationId, activityInfo.OrgDisplayName, activityInfo.UserName, activityInfo.Module, 
                activityInfo.Activity, activityInfo.ContactId, activityInfo.Tenant, activityInfo.IsSharedLogisticsContact, activityInfo.CardId, activityInfo.PartnerTypeId, null);
            // return wc.DownloadString(new Uri(sRequest));
        }


         
    }
}