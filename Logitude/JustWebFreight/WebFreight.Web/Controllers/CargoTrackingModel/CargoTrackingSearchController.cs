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
using System.Threading;
using Logitude.CargoTracking.Def.DataContracts;
using Logitude.CargoTracking.BL.CoreBL;
using WebFreight.Web.DataContracts;
using Logitude.CargoTracking.BL.Utilities;
using Logitude.CargoTracking.BL.DataContracts;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Collections;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{


    public class CargoTrackingSearchController : ApiController
    {


        [HttpGet]
        public async Task<HttpResponseMessage> GetShipments(string searchKey, int tenant)
        {
            try
            {


                ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(tenant);
                CargoTrackingShipmentSearchListQueryService cargoTrackingShipmentSearchQuery = new CargoTrackingShipmentSearchListQueryService(MyContext);

                List<CargoTrackingShipmentList> shipments = cargoTrackingShipmentSearchQuery.GetShipments(searchKey, tenant).OrderByDescending(s => s.CreateDate).ToList();

                MixPanelTrackingService trackingService = new MixPanelTrackingService();
                MixPanelTrackingEvent trackingEvent = CreateSearchMixPanelEvent(searchKey, shipments.Count());
                await trackingService.SendTrackingEventPostRequest(trackingEvent);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, shipments);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetShipment(string SecurityKey, int tenant)
        {
            try
            {

              
                ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(tenant);
                CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(MyContext);

                CargoTrackingShipmentList shipment = shipmentsQuery.GetShipment(SecurityKey, tenant);
                if (shipment == null) 
                    return Request.CreateResponse(HttpStatusCode.OK);

                List<Milestone> shipmentMilestones = shipmentsQuery.BuildShipmentMilstones(shipment);
                shipmentsQuery.SetMilestonesStatus(shipment, shipmentMilestones);
       
                MixPanelTrackingService trackingService = new MixPanelTrackingService();
                MixPanelTrackingEvent trackingEvent= CreateShipmentZoomMixPanelEvent(shipment.ShipmentNumber);              
                await trackingService.SendTrackingEventPostRequest(trackingEvent);
                CargoTrackingShipmentWithMilestones cargoTrackingShipmentWithMilestones = new CargoTrackingShipmentWithMilestones()
                {
                    ShipmentList = shipment,
                    Milestones = shipmentMilestones,

                };

               
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, cargoTrackingShipmentWithMilestones);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private MixPanelTrackingEvent CreateShipmentZoomMixPanelEvent(string searchKey)
        {
            int shipmentCount = 1;
            MixPanelTrackingService trackingService = new MixPanelTrackingService();
            MixPanelTrackingEvent trackingEvent = trackingService.CreateMixPanelEvent(searchKey, shipmentCount, "Zoom");
            return trackingEvent;
        }
        private MixPanelTrackingEvent CreateSearchMixPanelEvent(string searchKey, int shipmentsCount)
        {
           
            MixPanelTrackingService trackingService = new MixPanelTrackingService();
            MixPanelTrackingEvent trackingEvent = trackingService.CreateMixPanelEvent(searchKey, shipmentsCount, "Search");
            return trackingEvent;
        }
        [HttpGet]
        public HttpResponseMessage GetUserShipments(int pageIndex, int pageSize, [FromUri] CargoTrackingShipmentFilters shipmentFilters)
        {
            try
            {
                CargoTrackingUsersShipmentService usersShipmentService = new CargoTrackingUsersShipmentService();
                CargoTrackingShipmentsResponse response = usersShipmentService.GetUserShipmentsResponse(pageIndex, pageSize, shipmentFilters);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage GetUserShipmentsCount([FromUri] CargoTrackingShipmentFilters shipmentFilters)
        {
            try
            {
               
                CargoTrackingUsersShipmentService usersShipmentService = new CargoTrackingUsersShipmentService();
                CargoTrackingShipmentsCounter counter = usersShipmentService.GetUserShipmentsCounter(shipmentFilters);


                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, counter);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        [HttpGet]
        public HttpResponseMessage GetShipmentReferences(string securityKey, int tenant)
        {
            try
            {
                List<string> references = GetShipmentPublicReferences(securityKey, tenant);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, references);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            
        }


        private static List<string> GetShipmentPublicReferences(string SecurityKey, int tenant)
        {
            ICargoTrackingContext AccountingContext = CargoTrackingContext.GetContext(tenant);
            CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(AccountingContext);

            List<string> references = shipmentsQuery.GetShipmentPublicReferences(SecurityKey, tenant);
            return references;
        }

        [HttpGet]
        public HttpResponseMessage GetCaptchaData()
        {
            try
            {
                CaptchaHelper captchaHelper = new CaptchaHelper();
                UserData data = new UserData();
                captchaHelper.AddCaptchaKey(null,data,"Search");
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, data);
                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
       
        [HttpPut]
        public HttpResponseMessage PutUserValidation(CaptchaParameters captchaParameters)
        {
            try
            {
                UserData userData = CheckCaptchaState(captchaParameters);
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, userData);
                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private UserData CheckCaptchaState(CaptchaParameters loginParameters)
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            UserData data = new UserData();
            if (!captchaHelper.CheckCaptchaCodeValidated(loginParameters.CaptchaCode, loginParameters.CaptchaKey, null, false))
            {
                captchaHelper.AddCaptchaKey(null, data, "Search");               
            }


            return data;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostSearchTrackAsync(string searchKey)
        {
            try
            {
                
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, "ok");
                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

      
    }

    public class CargoTrackingSearchArgs
    {
        public string SearchKey { get; set; }
        public int Tenant { get; set; }
    }

}
