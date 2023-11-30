
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
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.Repositories;
namespace WebFreight.Web.Controllers.CommonDataModel.Generated.ListControllers
{ 

    
    public partial class TenantAdditionalDataViewsExtendedController : ApiController
    {
	  
       
        public HttpResponseMessage GetSingle(int tenant)
        {
		  try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TenantAdditionalData", "READ", authToken.Tenant);
                ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
				TenantAdditionalDataRepository  tenantAdditionalDataRepository = new TenantAdditionalDataRepository(MyContext);
				TenantAdditionalDataList entityList = null;
				TenantAdditionalData entityPoco = tenantAdditionalDataRepository.GetSingleTenantAdditionalDataByTenant(tenant);

				if (entityPoco != null)
				{
									List<TenantAdditionalData> singleEntityList = new List<TenantAdditionalData>();
					singleEntityList.Add(entityPoco);

					TenantAdditionalDataQuery tenantAdditionalDataQuery = new TenantAdditionalDataQuery(tenantAdditionalDataRepository);
					IQueryable<TenantAdditionalData> iQueryable = singleEntityList.AsQueryable();
					IQueryable<TenantAdditionalDataList> iQueryableEntityList = tenantAdditionalDataQuery.GetIQueryableEntityList(iQueryable);
				    entityList = iQueryableEntityList.FirstOrDefault();

			    }

				PerformanceLogger.AddServerExecutionTimeHeader(logKey);  
				               
                return Request.CreateResponse(HttpStatusCode.OK,  entityList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
           
        }


      
    }
}
	 