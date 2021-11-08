using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
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

namespace WebFreight.Web.Controllers.CommonDataModel.Generated.PMControllers
{
    public class PortTimeZonesController : ApiController
    {
        public HttpResponseMessage GetSingle(string code)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("PortTimeZone", "READ", authToken.Tenant);
                PortTimeZoneQuery portTimeZoneQuery = new PortTimeZoneQuery(authToken.Tenant);
                PortTimeZonePM portTimeZonePM = portTimeZoneQuery.GetSinglePM(code, authToken.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, portTimeZonePM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




        public HttpResponseMessage Post(PortTimeZonePM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("PortTimeZone", "NEW", authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("PortTimeZone", 0, authToken.Tenant);

                        ICommonDataContext MyContext = CommonDataContext.GetContext(0);
                        PortTimeZoneService service = new PortTimeZoneService(MyContext, 0);
                        service.Create(entityPM);

                        TableLastUpdateClass.UpdateTableHistory(0, "PortTimeZone");

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
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage Put(PortTimeZonePM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("PortTimeZone", "UPDATE", authToken.Tenant);
                        SecurityUtility.AuthenticationOnEntityTenant("PortTimeZone", 0, authToken.Tenant);

                        string entityName = "PortTimeZone" + entityPM.Code + 0;
                        string entityPmName = "PortTimeZonePM" + entityPM.Code + 0;
                        if (CacheManager.CacheWrapper.Get(entityName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityName);
                        }
                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                        {
                            CacheManager.CacheWrapper.Invalidate(entityPmName);
                        }

                        ICommonDataContext MyContext = CommonDataContext.GetContext(0);
                        PortTimeZoneService service = new PortTimeZoneService(MyContext, 0);

                        service.Update(entityPM);

                        TableLastUpdateClass.UpdateTableHistory(0, "PortTimeZone");

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
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}