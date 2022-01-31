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
using Logitude.BL.Helpers;
using System.Transactions;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.Controllers.InfrastructureModel.Generated.PMControllers
{


    public partial class DWObjectFieldsController : ApiController
    {


        public HttpResponseMessage GetDWObjectFieldsByDWTableId(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(0, DWOTId).OrderBy(a => a.Name).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDWObjectFieldsByDWTableIdGroupedByCategory(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DWObjectFieldsGroupedService dWObjectFieldsGroupedService = new DWObjectFieldsGroupedService(DWOTId, authToken.Tenant);
                List<DWFactGroup> FactFieldsGroups = dWObjectFieldsGroupedService.GetFactFieldsGroups();
   
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);  
                return Request.CreateResponse(HttpStatusCode.OK, FactFieldsGroups);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        } 
         
        public HttpResponseMessage getDWObjectFieldsWithChildren()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldWithChildrenFieldsPMsByTenant(0).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey); 
                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage getDWObjectFieldsWithChildrenByDWTableId(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(0, DWOTId).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            } 
        } 
    } 
}
