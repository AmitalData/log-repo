using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Services.DeploymentPackage;
using Logitude.BL.InfrastructureModel.Services.DeploymentPackage.Dependiencies;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class DeploymentPackageExtendedController : ApiController
    {
        [HttpPut]
        public HttpResponseMessage ValidateDependiencies(DeploymentPackagePM deploymentPackagePM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("DeploymentPackage", authToken.Tenant, deploymentPackagePM.Tenant);
                DeploymentPackageDependienciesService deploymentPackageDependienciesService = new DeploymentPackageDependienciesService(deploymentPackagePM);
                DeploymentPackageDependiency deploymentPackageDependiency = deploymentPackageDependienciesService.Validate();
                
                return Request.CreateResponse(HttpStatusCode.OK, deploymentPackageDependiency);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpGet]
        public HttpResponseMessage GetDeploymentPackageDetailsListByDocumentId(string documentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                List<DeploymentPackageDetailsList> deploymentPackageDetails = new DeploymentPackageDetailsListService(authToken.Tenant).GetDeploymentPackageDetailsListByDocumentId(documentId);
                
                return Request.CreateResponse(HttpStatusCode.OK, deploymentPackageDetails);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpGet]
        public HttpResponseMessage DeleteImportedDocumentById(string documentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                bool result = new DeploymentPackageDocumentService(authToken.Tenant, null).Delete(documentId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}