using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class DeploymentPackageExecutionLogExtendedController : ApiController
    {
        public HttpResponseMessage GetDeploymentPackageExecutionLogList(string id)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DeploymentPackageExecutionLogList deploymentPackageExecutionLogList = GetDeploymentPackageExecutionLogStatusList(id, authToken);
                return Request.CreateResponse(HttpStatusCode.OK, deploymentPackageExecutionLogList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static DeploymentPackageExecutionLogList GetDeploymentPackageExecutionLogStatusList(string id, AuthenticationToken authToken)
        {
            DeploymentPackageExecutionLogQuery deploymentPackageExecutionLogQuery = new DeploymentPackageExecutionLogQuery(authToken.Tenant);
            DeploymentPackageExecutionLogList deploymentPackageExecutionLogList = deploymentPackageExecutionLogQuery.GetDeploymentPackageExecutionLogList(id, authToken.Tenant);
            if (deploymentPackageExecutionLogList != null && (deploymentPackageExecutionLogList.StatusCode == "P" || deploymentPackageExecutionLogList.StatusCode == "W") && deploymentPackageExecutionLogList.CreateDate < DateTime.Now.AddMinutes(-5))
            {
                deploymentPackageExecutionLogList.StatusCode = "T";
                deploymentPackageExecutionLogList.ExceptionMessage = "The deployment Package failed to Deploy.Please try again.";
                UpdateDeploymentPackageExecutionLogSatusToTimeOut(deploymentPackageExecutionLogList);
            }

            return deploymentPackageExecutionLogList;
        }

        private static void UpdateDeploymentPackageExecutionLogSatusToTimeOut(DeploymentPackageExecutionLogList deploymentPackageExecutionLogList)
        {
            DeploymentPackageExecutionLogRepository deploymentPackageExecutionLogRepository = new DeploymentPackageExecutionLogRepository(deploymentPackageExecutionLogList.Tenant);
            DeploymentPackageExecutionLog deploymentPackageExecutionLog = deploymentPackageExecutionLogRepository.GetSingleDeploymentPackageExecutionLog(deploymentPackageExecutionLogList.Id, deploymentPackageExecutionLogList.Tenant);
            if (deploymentPackageExecutionLog != null)
            {
                deploymentPackageExecutionLog.ExceptionMessage = "Deploying the imported Deployment Package reached Timeout.Please try again.If the issue is persistent then please kindly contact our Customer Support";
                deploymentPackageExecutionLog.StatusCode = "T";
                deploymentPackageExecutionLogRepository.Update(deploymentPackageExecutionLog);
                deploymentPackageExecutionLogRepository.SubmitChanges();
            }
        }
    }
}