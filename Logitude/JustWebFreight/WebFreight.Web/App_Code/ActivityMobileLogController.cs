using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class ActivityMobileLogController : ApiController
    {
        public bool Post(string email, string module, string activity, int tenant, string cardId)
        {
            if (cardId == "null")
            {
                cardId = null; 
            } 

            if (module == "Help Center")
            {
                ActivityLog.SendTotangoContactActivity(email, module, activity, tenant, false, cardId, null);
            }

            else 
            {
                ActivityLog.SendTotangoContactActivity(email, module, activity, tenant, true, cardId, "Mobile");
            }

            return true;
        }
    }
}