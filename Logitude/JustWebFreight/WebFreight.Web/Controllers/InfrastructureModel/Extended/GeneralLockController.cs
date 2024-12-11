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

				var lockPoco = repo.GetSingleGeneralLockNOWAIT( tenant,  entityId, table?.Id);

				return Request.CreateResponse(HttpStatusCode.OK, lockPoco);

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

				var generalLockQuery = new GeneralLockQuery(tenant);
				ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

				ObjectTablePM objectTable = tablesQuery.GetObjectTableByName(objectTableName, tenant);
				
				var lockPoco = generalLockQuery.GetSingleGeneralLockNOWAIT(tenant, entityId, objectTable?.Id);
				
				if (objectTable.IsLock)
				{
					if (lockPoco == null)
					{
						if (isFromCahnge) 
						{ 
						    string entityId2 = null;
						    string objectTableId2 = null;
						    if (objectTable.RelatedEntity != null && objectTable.ThisKey != null && objectTable.RelatedKey != null)
						    {
						    	ObjectTablePM objectTable2 = tablesQuery.GetObjectTableByDBName(objectTable.RelatedEntity, tenant);
						    	if (objectTable2 != null && objectTable2.IsLock)
						    	{
						    		entityId2 = ExecuteQuery(tenant, entityId, objectTable);
						    		objectTableId2 = objectTable2.Id;
						    	}
						    }
						    var concurrentKiller = new ConcurrentKiller();
						    concurrentKiller.LockByobjectAndUser(tenant, userId, entityId, objectTable?.Id, entityId2, objectTableId2, sessionId);
					    }
					}
					else if (lockPoco.SessionId == sessionId)
					{
						lockPoco = null;
					}
					

				}			
				return Request.CreateResponse(HttpStatusCode.OK, lockPoco);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
		public string ExecuteQuery(int tenant, string entityId, ObjectTablePM objectTable)
		{
			string strConnString  = TenantServerConfigration.GetDbConnection(0);

			var result = string.Empty;
			
				//בכל אחד הראשון זה לפי מה לעשות את הקשר ביניהם והשני לפי מה להחזיר
				string[] ThisKey = objectTable.ThisKey.Split(',');
				string[] RelatedKey = objectTable.RelatedKey.Split(',');

				string sqlQuery = @"SELECT b." + RelatedKey[0] + @" as res
                                 FROM " + objectTable.DBTableName + @" AS a
                                 JOIN " + objectTable.RelatedEntity + @" AS b ON 
                                     a.tenant = b.tenant AND
                                     a." + ThisKey[1] + "= b." + RelatedKey[1] +
								 @" WHERE 
                                     a.tenant = " + tenant + @" AND 
                                     a." + ThisKey[0] + "= '" + entityId+"'";


				using (SqlConnection connection = new SqlConnection())
				{
					connection.ConnectionString = strConnString;
					connection.Open();
					using (var cmd = new SqlCommand(sqlQuery, connection))
					{
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								result = reader["res"].ToString();
							}
						    connection.Close();
						}
					}
				}
			return result;
		}
		[HttpPost]
		public HttpResponseMessage DeleteGeneralLock( string entityId, string objectTableName)
		{
			try
			{
				string token = HttpContext.Current.Request.Headers["Token"];
				string sessionId = HttpContext.Current.Request.Headers["SessionId"];
				AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				SecurityUtility.AuthenticationOnTenant(tenant);

				var generalLockQuery = new GeneralLockQuery(tenant);
				ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

				ObjectTablePM objectTable = tablesQuery.GetObjectTableByName(objectTableName, tenant);

				var concurrentKiller = new ConcurrentKiller();
				concurrentKiller.FreeGeneralLock(tenant, entityId, objectTable?.Id, sessionId);

				return Request.CreateResponse(HttpStatusCode.OK, "DeleteGeneralLock");
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}

		}
	}
}