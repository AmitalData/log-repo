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
using System.Transactions;
using Logitude.BL.Helpers;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.BL;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.BL.EntityUpdateServices;
using Logitude.Workflow.Data.EntityListQueryServices;
using Logitude.Workflow.BL.EntityQueryServices;
using MicrosoftGraphClient.GraphServices;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class MicrosoftOffice365Controller : ApiController
    {
        public HttpResponseMessage CreateSubscription(string microsoftEmailAccessToken, string workflowNumber)
        {
            if (string.IsNullOrEmpty(microsoftEmailAccessToken) || string.IsNullOrEmpty(workflowNumber))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Empty access token or workflow number");
            }

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("ServiceProviderSubscription", "NEW", authToken.Tenant);




                    var test = new GraphClientProfileService();



                    ServiceProviderSubscriptionPM entityPM = new ServiceProviderSubscriptionPM
                    {
                        Tenant = authToken.Tenant,
                        UserEmail = null,
                        AccessToken = null,
                        RefreshToken = null,
                        EmailProvider = null,
                        WorkflowNumber = null,
                        AdditionalSettings = null,
                        AccessTokenExpirationDateTime = DateTime.Now,
                        SubscriptionExpirationDateTime = DateTime.Now,
                        WebhookParams = null,
                        ChangeSetOp = ChangeSetOperation.Insert
                    };


                    IWorkflowContext workflowContext = WorkflowContext.GetContext(entityPM.Tenant);
                    ServiceProviderSubscriptionUpdateService service = new ServiceProviderSubscriptionUpdateService(workflowContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(entityPM, true);


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
    }
}