
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
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

namespace WebFreight.Web.Controllers.InvoiceModel.Extended
{
    public class SATInterfaceSettingsExtendedController : ApiController
    {
        public HttpResponseMessage GetSingle(int id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SATInterfaceSettingQuery sATInterfaceSettingQuery = new SATInterfaceSettingQuery(authToken.Tenant);
                SATInterfaceSettingPM SATInterfaceSettingPM = sATInterfaceSettingQuery.GetSinglePM(id);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, SATInterfaceSettingPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetSATSettingsForTenant(int tenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SATInterfaceSettingQuery sATInterfaceSettingQuery = new SATInterfaceSettingQuery(authToken.Tenant);
                SATInterfaceSettingPM SATInterfaceSettingPM = sATInterfaceSettingQuery.GetSATInterfaceSettingPMByTenant(authToken.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, SATInterfaceSettingPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        //public HttpResponseMessage Post(SATInterfaceSettingPM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string logKey = PerformanceLogger.LogCurrentTime();
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //                IInvoiceContext MyContext = InvoiceContext.GetContext(entityPM.Tenant);
        //                SATInterfaceSettingService service = new SATInterfaceSettingService(MyContext, entityPM.Tenant);
        //                service.Create(entityPM);

                       
        //                scope.Complete();
        //                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

        //                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}


        //public HttpResponseMessage Put(SATInterfaceSettingPM entityPM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string logKey = PerformanceLogger.LogCurrentTime();
        //            using (TransactionScope scope = TransactionFactory.GetTransaction())
        //            {
        //                string token = HttpContext.Current.Request.Headers["Token"];
        //                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //                string entityName = "SATInterfaceSetting" + entityPM.Id + entityPM.Tenant;
        //                string entityPmName = "SATInterfaceSettingPM" + entityPM.Id + entityPM.Tenant;
        //                if (CacheManager.CacheWrapper.Get(entityName) != null)
        //                {
        //                    CacheManager.CacheWrapper.Invalidate(entityName);
        //                }
        //                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
        //                {
        //                    CacheManager.CacheWrapper.Invalidate(entityPmName);
        //                }

        //                IInvoiceContext MyContext = InvoiceContext.GetContext(entityPM.Tenant);
        //                SATInterfaceSettingService service = new SATInterfaceSettingService(MyContext, entityPM.Tenant);

        //                service.Update(entityPM);

                      

        //                scope.Complete();
        //                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

        //                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
        //    }
        //}

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}