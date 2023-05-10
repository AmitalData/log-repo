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

using Logitude.Server.Tools.StorageService;
using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class CargoTrackingShipmentsController
    {



        public HttpResponseMessage GetCargoShipmentPMByEntityId(string entityId)
        {
            try
            {
                int tenant = GetAuthinticated().Tenant;

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

                CreateZoomEventForMixPanel(tenant, cargoTrackingShipmentPM, false);
                return Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private void CreateZoomEventForMixPanel(int tenant, CargoTrackingShipmentPM cargoTrackingShipmentPM, bool isPublic)
        {
            var userName ="";
            const string ProjectToken = "99de9de5af6505a670b915020e51380e";
            MixPanelEvent zoomEvent = BuildMixPanelZoomEvent(cargoTrackingShipmentPM.ShipmentNumber, isPublic);
            var authinticated = GetAuthinticated();
            if(authinticated==null)
                 userName = "";
            else
                userName = authinticated.Email;
            MixPanelEventTracker eventTracker = new MixPanelEventTracker(ProjectToken, userName, tenant);
            eventTracker.TrackEvent(zoomEvent);
        }
        private static MixPanelEvent BuildMixPanelZoomEvent(string shipmentNumber, bool isPublic)
        {
            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = "Zoom";
            mixPanelEvent.AddProperty("shipment_number", shipmentNumber);
            mixPanelEvent.AddProperty("is_public", isPublic.ToString());
            return mixPanelEvent;
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
        public HttpResponseMessage GetFilingAttachPdfReport(string documentId,int tenant)
        {
            try
            {
                byte[] result = null;
              
                //int tenant = GetAuthinticatedTenant();
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                Document document = documentRepository.GetSingleDocument(tenant, documentId);
                if (document != null)
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,
                    };
                    result = storageservice.Read(fileInfo);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private AuthenticationToken GetAuthinticated()
        {
			string logKey = PerformanceLogger.LogCurrentTime();
			string token = HttpContext.Current.Request.Headers["Token"];
			AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
			SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
			return authToken;
		}
	}



}
