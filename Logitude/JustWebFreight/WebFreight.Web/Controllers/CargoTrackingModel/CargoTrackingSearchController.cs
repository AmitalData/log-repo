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
using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{


    public class CargoTrackingSearchController : ApiController
    {
        private const string ProjectToken = "99de9de5af6505a670b915020e51380e";
        [HttpGet]// for public search
        public HttpResponseMessage GetShipments([FromUri] CargoTrackingSearchRequest searchRequest)
        {
            try
            {
                CargoTrackingSearchResponse searchResponse = new CargoTrackingSearchResponse();

                var hasCaptcha = !string.IsNullOrEmpty(searchRequest.CaptchaKey);
                if (hasCaptcha)
                    ValidateCaptcha(searchRequest, searchResponse);
                else
                    CheckRequestsLimit(searchRequest, searchResponse);

                if (!searchResponse.CaptchaRequired)
                    searchResponse.Shipments = GetShipmentsBySearchKey(searchRequest);


                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, searchResponse);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static void CheckRequestsLimit(CargoTrackingSearchRequest searchRequest, CargoTrackingSearchResponse searchResponse)
        {
            var exceedLimit = CargoTrackingSecurityService.CheckRequestsLimit(searchRequest.Tenant);
            if (exceedLimit == true)
                AddNewCaptcha(searchResponse);
        }

        private static void ValidateCaptcha(CargoTrackingSearchRequest searchRequest, CargoTrackingSearchResponse searchResponse)
        {
            bool validCaptcha = CheckIfCaptchaIsValid(searchRequest);
            if (!validCaptcha)
            {
                searchResponse.InvalidCaptcha = true;
                AddNewCaptcha(searchResponse);
            }
            else
            {
                CargoTrackingSecurityService.ResetIPSearchCounter();
            }
        }

        private static void AddNewCaptcha(CargoTrackingSearchResponse searchResponse)
        {
            searchResponse.CaptchaRequired = true;

            UserData data = new UserData();
            CaptchaHelper captchaHelper = new CaptchaHelper();
            captchaHelper.AddCaptchaKey(null, data, "Search");

            searchResponse.CaptchaKey = data.CaptchaKey;
            searchResponse.CaptchaImage = data.CaptchaImage;
        }

        private static bool CheckIfCaptchaIsValid(CargoTrackingSearchRequest searchRequest)
        {
            CaptchaHelper captchaHelper = new CaptchaHelper();
            var validCaptcha = captchaHelper.CheckCaptchaCodeValidated(searchRequest.CaptchaCode, searchRequest.CaptchaKey, null, false);
            return validCaptcha;
        }

        private static List<CargoTrackingShipmentList> GetShipmentsBySearchKey(CargoTrackingSearchRequest searchRequest)
        {
            CargoTrackingSecurityService.RecordSearch(searchRequest.Tenant);

            ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(searchRequest.Tenant);
            CargoTrackingShipmentSearchListQueryService cargoTrackingShipmentSearchQuery = new CargoTrackingShipmentSearchListQueryService(MyContext);
            List<CargoTrackingShipmentList> shipments = cargoTrackingShipmentSearchQuery.GetShipments(searchRequest.SearchKey, searchRequest.Tenant).OrderByDescending(s => s.CreateDate).ToList();

            CreateSearchEventForMixPanel(searchRequest.SearchKey, searchRequest.Tenant, shipments, true);

            return shipments;
        }

        private static void CreateSearchEventForMixPanel(string searchKey, int tenant, List<CargoTrackingShipmentList> shipments, bool isPublic)
        {
            if (searchKey != null)
            {
                string userName = isPublic ? null : GetUserEmail();
                MixPanelEventTracker eventTracker = new MixPanelEventTracker(ProjectToken, userName, tenant);
                MixPanelEvent searchEvent = BuildMixPanelSearchEvent(searchKey, shipments, isPublic);
                eventTracker.TrackEvent(searchEvent);
            }
        }

        private static MixPanelEvent BuildMixPanelSearchEvent(string searchKey, List<CargoTrackingShipmentList> shipments, bool isPublic)
        {
            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = "Search";
            mixPanelEvent.AddProperty("search_key", searchKey);
            mixPanelEvent.AddProperty("results_count", shipments.Count().ToString());
            mixPanelEvent.AddProperty("is_public", isPublic.ToString());
            return mixPanelEvent;
        }

        [HttpGet]// for public search
        public async Task<HttpResponseMessage> GetShipment(string SecurityKey, int tenant)
        {
            try
            {


                ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(tenant);
                CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(MyContext);

                CargoTrackingShipmentList shipment = shipmentsQuery.GetShipment(SecurityKey, tenant);
                if (shipment == null)
                    return Request.CreateResponse(HttpStatusCode.OK);

                //List<Milestone> shipmentMilestones = shipmentsQuery.BuildShipmentMilstones(shipment);
                //shipmentsQuery.SetMilestonesStatus(shipment, shipmentMilestones);

                //CargoTrackingShipmentWithMilestones cargoTrackingShipmentWithMilestones = new CargoTrackingShipmentWithMilestones()
                //{
                //    ShipmentList = shipment,
                //    Milestones = shipmentMilestones,

                //};

                CreateZoomEventForMixPanel(tenant, shipment, true);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, 0);
                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet] // for private needs auth.
        public async Task<HttpResponseMessage> GetUserShipment(string SecurityKey, int tenant)
        {
            try
            {
                AuthorizeTenant(tenant);
                ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(tenant);
                CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(MyContext);

                CargoTrackingShipmentList shipment = shipmentsQuery.GetShipment(SecurityKey, tenant);
                if (shipment == null)
                    return Request.CreateResponse(HttpStatusCode.OK);

                //List<Milestone> shipmentMilestones = shipmentsQuery.BuildShipmentMilstones(shipment);
                //shipmentsQuery.SetMilestonesStatus(shipment, shipmentMilestones);

                //CargoTrackingShipmentWithMilestones cargoTrackingShipmentWithMilestones = new CargoTrackingShipmentWithMilestones()
                //{
                //    ShipmentList = shipment,
                //    Milestones = shipmentMilestones,

                //};

                CreateZoomEventForMixPanel(tenant, shipment, false);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, 0);
                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private void AuthorizeTenant(int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
        }

        private static string GetUserEmail() {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            return authToken.Email;
        }

        private static void CreateZoomEventForMixPanel(int tenant, CargoTrackingShipmentList shipment, bool isPublic)
        {
            MixPanelEvent zoomEvent = BuildMixPanelZoomEvent(shipment.ShipmentNumber,isPublic);
            string userName = isPublic ? null : GetUserEmail();
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

        [HttpGet] // for private needs auth.
        public HttpResponseMessage GetUserShipments(int pageIndex, int pageSize, [FromUri] CargoTrackingShipmentFilters shipmentFilters)
        {
            try
            {
                AuthorizeTenant(shipmentFilters.Tenant);
                CargoTrackingUsersShipmentService usersShipmentService = new CargoTrackingUsersShipmentService();
                CargoTrackingShipmentsResponse response = usersShipmentService.GetUserShipmentsResponse(pageIndex, pageSize, shipmentFilters);

                CreateSearchEventForMixPanel(shipmentFilters.SearchText, shipmentFilters.Tenant, response.Shipments, false);
            
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet] // for private needs auth.
        public HttpResponseMessage GetUserShipmentsCount([FromUri] CargoTrackingShipmentFilters shipmentFilters)
        {
            try
            {
                AuthorizeTenant(shipmentFilters.Tenant);
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
        [HttpGet]// for public
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

        
        [HttpGet]
        public HttpResponseMessage GetSingleShipmentList(string SecurityKey, int tenant)
        {
            try
            {


                ICargoTrackingContext MyContext = CargoTrackingContext.GetContext(tenant);
                CargoTrackingShipmentListQueryService shipmentsQuery = new CargoTrackingShipmentListQueryService(MyContext);

                CargoTrackingShipmentList shipment = shipmentsQuery.GetShipment(SecurityKey, tenant);

               
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, shipment);

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
        public CaptchaParameters captchaParameters { get; set; }
    }

    public class CargoTrackingSearchRequest
    {
        public int Tenant { get; set; }
        public string SearchKey { get; set; }
        public string CaptchaKey { get; set; }
        public string CaptchaCode { get; set; }

    }
    public class CargoTrackingSearchResponse
    {
        public List<CargoTrackingShipmentList> Shipments { get; set; }
        public bool CaptchaRequired { get; set; }
        public string CaptchaImage { get; set; }
        public string CaptchaKey { get; set; }
        public bool InvalidCaptcha { get; set; }

    }

}
