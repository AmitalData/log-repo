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
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{ 

    
    public partial class LastRunDetailsExtendedController : ApiController
    {
        public HttpResponseMessage PutLastRunDetails(LastRunDetailPM entityPM, string userId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("LastRunDetail", "READ", authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("LastRunDetail", entityPM.Tenant, authToken.Tenant);

                IInfrastructureContext infrastructureContext = InfrastructureContext.GetContext(entityPM.Tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(authToken.Tenant);
                UserService userService = new UserService(commonContext, authToken.Tenant);
                bool isCustomerCare = userService.CheckIsUserCustomerCareById(userId);
                if (!isCustomerCare)
                {
                    LastRunDetailUpdateService service = new LastRunDetailUpdateService(infrastructureContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.InitializeEntityPM(entityPM);
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    service.Update(entityPM, true);

                }
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, entityPM);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
	 