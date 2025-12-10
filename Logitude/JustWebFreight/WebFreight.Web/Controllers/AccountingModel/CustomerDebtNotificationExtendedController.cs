using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.AccountingModel
{ 

    
    public partial class CustomerDebtNotificationExtendedController : ApiController
    {
        public HttpResponseMessage GetCustomerDebtNotificationByAccountId(string accountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", tenant);
				accountId = accountId == "null" ? null : accountId.Trim();
				IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
				CustomerDebtNotificationQueryService customerDebtNotificationQueryServiceQuery = new CustomerDebtNotificationQueryService(MyContext);
                CustomerDebtNotificationPM myResult = customerDebtNotificationQueryServiceQuery.GetCustomerDebtNotificationByAccountId(tenant, accountId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

       

    }

 
}
	 