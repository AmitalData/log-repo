using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Testing;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for TotangoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class TotangoService : System.Web.Services.WebService
    {
        [WebMethod]
        public void Hello()
        { }

        

        [WebMethod]
        public void SendUserActivity(string organizationId, string orgDisplayName, string userName, string module, string activity,string contactId,int tenant,bool isSharedLogisticsContact, string cardId, string partnerTypeId)
        {
            ActivityLog.AddContactActivityWithTotango(organizationId, orgDisplayName, userName, module, activity, contactId, tenant, isSharedLogisticsContact, cardId, partnerTypeId,null);
            // return wc.DownloadString(new Uri(sRequest));
        }

       
    }
}
