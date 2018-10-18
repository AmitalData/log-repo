using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for UpdateCustomerTenantAccessStatus
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UpdateCustomerTenantAccessStatus : System.Web.Services.WebService
    {

        [WebMethod]
        public void DenyRequest(string Id,int tenant)
        {
            CustomerTenantAccessQuery Query = new CustomerTenantAccessQuery(tenant);
            var EntityPm = Query.GetSinglePM(Id,tenant);
            if (EntityPm != null)
            {
                EntityPm.Status = "IA";
                ICommonDataContext Context = CommonDataContext.GetContext(EntityPm.Tenant);
                CustomerTenantAccessService service = new CustomerTenantAccessService(Context, EntityPm.Tenant, EntityPm, "system@tenant" + EntityPm.Tenant + ".com");
                service.Update();
            }
           
        }
    }
}
