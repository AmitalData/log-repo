using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ActivityLogWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ActivityLogWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void AddActivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId)
        {
            ActivityLog.AddAcitivityLog(entityId, objectTableId, tenant, activityTypeCode, userId);
        }

        //[WebMethod]
        //public void AddContactActivityLog(string contactId, string module, string activity,int tenant, bool isSharedLogisticsContact)
        //{
        //    ActivityLog.AddContactActivityLog(contactId, module, activity, tenant, isSharedLogisticsContact);
        //}
    }
}
