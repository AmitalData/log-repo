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
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;
using Microsoft.TeamFoundation.Common;
using CWXSD;

namespace JustWebFreight.WebFreight.Web.Controllers.CustomsModel.Generated.ListControllers
{
    public partial class DeclarationCourierStatusViewsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetGroupByStorageSite(string courierMasterId = null, string declarationCourierList = null)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                
				int tenant = authToken.Tenant;

                // validate input
                if (String.IsNullOrEmpty(courierMasterId) && String.IsNullOrEmpty(declarationCourierList))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "missing id");
                }

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
                int storageSiteCount;
                if (!String.IsNullOrEmpty(courierMasterId))
                {
                    // get storage site count by courier ID
                    storageSiteCount = declarationCourierStatusQuery.GetCountGroupByStorageCode(tenant, courierMasterId: courierMasterId);
                }
                else
                {
                    // get storage site count by declaration IDs
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var declarationsList = JsonConvert.Deserialize<List<string>>(declarationCourierList);
                    storageSiteCount = declarationCourierStatusQuery.GetCountGroupByStorageCode(tenant, declarationsList: declarationsList);
                }

                ServiceResponse response = new ServiceResponse();
                response.Count = storageSiteCount;

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
				PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
	 