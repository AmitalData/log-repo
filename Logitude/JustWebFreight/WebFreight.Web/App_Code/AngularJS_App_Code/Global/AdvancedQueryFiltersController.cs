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
    public partial class AdvancedQueryFiltersController : ApiController
    {

        public HttpResponseMessage GetAdvancedQueryFiltersByTenant(int tenant,string loggedcontactid)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
                var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUser(tenant, loggedcontactid);
            
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetAdvancedQueryFiltersByTenantAndQuery(int tenant, string loggedcontactid, string queryId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
                var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUserAndQuery(tenant, loggedcontactid, queryId);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetAdvancedQueryFiltersByTenantuserobjecttablequery(int tenant,string objecttableCode,string queryid, string loggedcontactid)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
                var result = advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUser(tenant, loggedcontactid);
                AdvancedQueryFilterPM filter = null;
                if (result != null)
                {
                    filter = result.Where(a => a.ObjectFieldCode == objecttableCode && a.QueryId == queryid).FirstOrDefault();
                }

                return Request.CreateResponse(HttpStatusCode.OK, filter);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Post(AdvancedQueryFilterPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                AdvancedQueryFilterService service = new AdvancedQueryFilterService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(AdvancedQueryFilterPM entityPM)
        { 
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                AdvancedQueryFilterRepository repo = new AdvancedQueryFilterRepository(entityPM.Tenant);
                var temp = repo.GetSingleAdvancedQueryfilter(entityPM.Id);
                if (temp != null && temp.Tenant != 0)
                {
                    repo.Remove(temp);
                    repo.SubmitChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //public HttpResponseMessage Delete(string objectfieldid, string queryid,int tenant)
        //{
            //try
            //{
            //    string token = HttpContext.Current.Request.Headers["Token"];
            //    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //    IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
            //    AdvancedQueryFilterRepository repo = new AdvancedQueryFilterRepository(tenant);
            //    var temp = repo.GetSingleAdvancedQueryfilter(entityPM.Id);
            //    if (temp != null)
            //    {
            //        repo.Remove(temp);
            //        repo.SubmitChanges();
            //    }
            //    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            //}
        //}
        
	    
    }
}