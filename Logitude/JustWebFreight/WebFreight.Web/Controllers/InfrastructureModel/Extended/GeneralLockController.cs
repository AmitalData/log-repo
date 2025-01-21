using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Utils;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http; 
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class GeneralLockController : ApiController
    {
		public HttpResponseMessage GetGeneralLock(string userId ,string entityId,string objectTableName)
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);

				var repo = new GeneralLockRepository(tenant);
				ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

				ObjectTablePM table = tablesQuery.GetObjectTableByName(objectTableName, tenant);

				var lockPoco = repo.GetSingleGeneralLock( tenant,  entityId, table?.Id);

				return Request.CreateResponse(HttpStatusCode.OK, lockPoco);

			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}

		}
		[HttpPost]
		public HttpResponseMessage DeleteGeneralLock(string entityId, string objectTableName)
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				string sessionId = HttpContext.Current.Request.Headers["SessionId"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);

				GeneralLockQueryService generalLockQueryService = new GeneralLockQueryService(tenant);
				generalLockQueryService.DeleteGeneralLockByEntity(tenant, entityId, objectTableName, sessionId);


				return Request.CreateResponse(HttpStatusCode.OK, "DeleteGeneralLock");
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}

		}
		[HttpPost]
		public HttpResponseMessage DeleteGeneralLockBySessionId()
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				string sessionId = HttpContext.Current.Request.Headers["SessionId"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);

				GeneralLockQueryService generalLockQueryService = new GeneralLockQueryService(tenant);
				generalLockQueryService.DeleteGeneralLockBySessionId(tenant, sessionId);


				return Request.CreateResponse(HttpStatusCode.OK, "DeleteGeneralLock");
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}

		}
		[HttpPost]
		public HttpResponseMessage DeleteGeneralLockByGeneralKey(string generalKey)
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];			
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);

				GeneralLockQueryService generalLockQueryService = new GeneralLockQueryService(tenant);
				generalLockQueryService.DeleteGeneralLockByGeneralKey(tenant, generalKey);
				string res = null;

				return Request.CreateResponse(HttpStatusCode.OK, res);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}

		}
		public HttpResponseMessage PostCheckLock(string userId, string entityId, string objectTableName,bool isFromCahnge = false)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
				string sessionId = HttpContext.Current.Request.Headers["SessionId"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);
				GeneralLockQueryService generalLockQueryService = new GeneralLockQueryService(tenant);
				var lockPoco = generalLockQueryService.CheckIsLocked(tenant,sessionId,userId,entityId,objectTableName,isFromCahnge);

				
				return Request.CreateResponse(HttpStatusCode.OK, lockPoco);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }	
	}
	
}