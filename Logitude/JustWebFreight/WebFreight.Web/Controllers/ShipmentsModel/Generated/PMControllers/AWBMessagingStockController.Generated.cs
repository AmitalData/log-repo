using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
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

namespace WebFreight.Web.Controllers.ShipmentsModel.Generated.PMControllers
{
    public class AWBMessagingStockController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(0);
                SecurityUtility.CheckContactFeature("AWBMessagingStock", "READ", 0);

                AWBMessagingStockQuery entityQuery = new AWBMessagingStockQuery(tenant);
                AWBMessagingStockPM myResult = entityQuery.GetSinglePM(id, tenant);
                myResult.DummyTenant = tenant;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Post(AWBMessagingStockPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(0);
                    SecurityUtility.CheckContactFeature("AWBMessagingStock", "NEW", 0);

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
                    AWBMessagingStockService service = new AWBMessagingStockService(objectContext, entityPM);
                    service.Create();

                    TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "AWBMessagingStock");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(AWBMessagingStockPM entityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(0);
                    SecurityUtility.CheckContactFeature("AWBMessagingStock", "UPDATE", 0);

                    IShipmentsContext objectContext = ShipmentsContext.GetContext(entityPM.TenantNumber);
                    AWBMessagingStockService service = new AWBMessagingStockService(objectContext, entityPM);
                    service.Update();

                    TableLastUpdateClass.UpdateTableHistory(entityPM.TenantNumber, "AWBMessagingStock");

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}