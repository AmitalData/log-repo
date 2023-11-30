using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class QueueMessagesWebServiceController : ApiController
    {
        public HttpResponseMessage GetUpdateTenantManagementStatistics(int tenantId)
        {
            try
            {
                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.AuthenticationOnTenant(tenantId);
                QueueMessagesWebService myService = new QueueMessagesWebService();
                myService.UpdateTenantManagementStatistics(tenantId);

                //scope.Complete();
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUpdateTenantPriorityStatistics(int tenantId,string CourierMasterId, string InterfaceTypeCode,int TenantPriority)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                  string token = HttpContext.Current.Request.Headers["Token"];
                  AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                  int tenant = authToken.Tenant;
                  CustomsStoredProcedures.UpdateQueueMessageTenantPriority(tenantId, CourierMasterId, InterfaceTypeCode, TenantPriority);
                  
                  scope.Complete();
                  return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}