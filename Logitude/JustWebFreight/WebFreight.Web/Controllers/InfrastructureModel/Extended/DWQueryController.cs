using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Global
{
    public partial class DWQueryController : ApiController
    {
        public HttpResponseMessage GetSingle(string Id)
        {
            try
            {
                //string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectTable", "READ", authToken.Tenant);
                DWQueryQuery dWQueryQuery = new DWQueryQuery(authToken.Tenant);
                DWQueryPM dWQueryPM = dWQueryQuery.GetSinglePM(Id, authToken.Tenant);
               
                return Request.CreateResponse(HttpStatusCode.OK, dWQueryPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        //public HttpResponseMessage GetAdvancedDWQueryFiltersByTenant(int tenant,string loggedcontactid)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        AdvancedDWQueryFilterDWQuery advancedDWQueryFilterDWQuery = new AdvancedDWQueryFilterDWQuery(tenant);
        //        var result = advancedDWQueryFilterDWQuery.GetAdvancedDWQueryFilterPMsByTenantAndUser(tenant, loggedcontactid);
        //        var temp = result.Where(a => a.DWQueryId == "1-3887");

        //        return Request.CreateResponse(HttpStatusCode.OK, result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

        public HttpResponseMessage Post(DWQueryPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                DWQueryService service = new DWQueryService(objectContext, entityPM.Tenant);

                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(DWQueryPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                DWQueryService service = new DWQueryService(objectContext, entityPM.Tenant);

                service.Update(entityPM);
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Delete(string id, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
                DWQueryRepository Repo = new DWQueryRepository(objectContext);
                var entity = Repo.GetSingleDWQuery(id, authToken.Tenant);
                Repo.Remove(entity);
                Repo.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}