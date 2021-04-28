using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class PotentialAddressController : ApiController
    {
        [ActionName("PostAddress")]
        public HttpResponseMessage PostAddress(AddressPM entityPM)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    AddressService service = new AddressService(MyContext, entityPM.Tenant);
                    service.Create(entityPM);

                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutAddress(AddressPM entityPM)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    string entityName = "Address" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "AddressPM" + entityPM.Id + entityPM.Tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    AddressService service = new AddressService(MyContext, entityPM.Tenant);

                    service.Update(entityPM);
                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);

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