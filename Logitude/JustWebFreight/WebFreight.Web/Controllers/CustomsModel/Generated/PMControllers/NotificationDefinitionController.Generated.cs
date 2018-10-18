//------------------------------------------------------------------------------
// -- Generated one time --
// 
// this closed table connected to  opened table (NotificationTenantDefinition)
//                                                
// Abdullah
//------------------------------------------------------------------------------
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{ 

    
    public partial class NotificationDefinitionsController : ApiController
    {
	  
       
        public HttpResponseMessage GetSingle(string code)
        {
		  try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
			    string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "READ", authToken.Tenant);

                NotificationDefinitionPM notificationDefinitionPM = GetSingleNotificationDefinitionPM(code, authToken.Tenant);


                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
            
                return Request.CreateResponse(HttpStatusCode.OK, notificationDefinitionPM);
			 }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
           
        }
        public HttpResponseMessage Put(NotificationDefinitionPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string logKey = PerformanceLogger.LogCurrentTime();
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        UpdateNotificationDefinition(entityPM, authToken.Tenant);


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



        // Private methods
        private NotificationDefinitionPM GetSingleNotificationDefinitionPM(string code, int tenant)
        {
            ICustomContext customContext = CustomContext.GetContext(tenant);
            NotificationDefinitionQueryService notificationDefinitionQuery = new NotificationDefinitionQueryService(customContext);
            NotificationDefinitionPM notificationDefinition = notificationDefinitionQuery.GeNotificationDefinitionwithDefinition(code, tenant);
            return notificationDefinition;
        }
        private void UpdateNotificationDefinition(NotificationDefinitionPM currententityPm, int tenant)
        {
            //SecurityUtility.CheckContactFeature("Customs.NotificationDefinition", "UPDATE", currententityPm.Tenant);
            ICustomContext customContext = CustomContext.GetContext(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            NotificationDefinitionUpdateService service = new NotificationDefinitionUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }

    }
}
	 