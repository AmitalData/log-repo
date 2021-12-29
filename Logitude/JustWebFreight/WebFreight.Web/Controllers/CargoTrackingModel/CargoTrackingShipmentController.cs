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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.BL.EntityUpdateServices;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class CargoTrackingShipmentsController
    {


        public HttpResponseMessage GetCargoShipmentPMByEntityId(string entityId)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();

                CargoTrackingShipmentQueryService cargoTrackingShipmentQuery = new CargoTrackingShipmentQueryService(tenant);
                CargoTrackingShipmentPM cargoTrackingShipmentPM = cargoTrackingShipmentQuery.GetSinglePMById(entityId, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetCargoShipmentPMBySecurityKey(string securityKey, int tenant)
        {
            try
            {


                CargoTrackingShipmentQueryService cargoTrackingShipmentQuery = new CargoTrackingShipmentQueryService(tenant);
                CargoTrackingShipmentPM cargoTrackingShipmentPM = cargoTrackingShipmentQuery.GetSinglePMBySecurityKey(securityKey, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetMainCargoShipmentPMBySecurityKey(string securityKey, int tenant)
        {
            try
            {


                CargoTrackingShipmentQueryService cargoTrackingShipmentQuery = new CargoTrackingShipmentQueryService(tenant);
                CargoTrackingShipmentPM cargoTrackingShipmentPM = cargoTrackingShipmentQuery.GetMainShipmentByShipmentSecurityKey(securityKey, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage PostDeclarationApprovalResponse(DeclarationApprovalArgs declarationApprovalArgs)
        {

            //using (var scope = new TransactionScope(TransactionScopeOption.Required,
            //                         new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //{
                try
                {
                    CargoTrackingShipmentsDeclarationApprovalService declarationApprovalService = new CargoTrackingShipmentsDeclarationApprovalService();
                    declarationApprovalService.HandleDeclarationApproval(declarationApprovalArgs);

                  //  scope.Complete();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
           // }


        }

        private int GetAuthinticatedTenant()
        {
            string logKey = PerformanceLogger.LogCurrentTime();
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken.Tenant;
        }
    }



}
