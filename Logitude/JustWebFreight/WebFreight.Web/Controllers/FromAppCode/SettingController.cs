using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class SettingController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        public bool PostUpdateMobileSetting(int tenant, SettingFilters filters)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);

            bool IsScceed = false;
          // SecurityUtility.AuthenticationOnTenant(tenant);
           ContactPasswordRepository rep = new ContactPasswordRepository();
           ContactPassword contactpassword = rep.GetSingleContactPassword(filters.Email);
           
           if (contactpassword != null)
           {
               contactpassword.SharedMobileAppAlertonExceptions = filters.SharedMobileAppAlertonExceptions;
               contactpassword.SharedMobileAppAlertsforFollowedShipment = filters.SharedMobileAppAlertsforFollowedShipment;
               contactpassword.IsSendNotificationForMobile = filters.IsSendNotificationForMobile;
               rep.Update(contactpassword);
               IsScceed = true;
               rep.SubmitChanges();
             
           }
        
           return IsScceed;

        }

        [OperationContract]
        [WebGet(UriTemplate = "getmobilesetting/{tenant}/{email}")]
        public SettingFilters GetMobileSetting(int tenant, string email)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SettingFilters filters = null;
           ContactPasswordRepository rep = new ContactPasswordRepository();
           ContactPassword contactpassword = rep.GetSingleContactPassword(email);
           if (contactpassword != null)
            {
                filters = new SettingFilters()
               {
                   SharedMobileAppAlertonExceptions = contactpassword.SharedMobileAppAlertonExceptions,
                   SharedMobileAppAlertsforFollowedShipment = contactpassword.SharedMobileAppAlertsforFollowedShipment,
                   IsSendNotificationForMobile = contactpassword.IsSendNotificationForMobile,

               };
            }
            return filters;

        }
    }
}