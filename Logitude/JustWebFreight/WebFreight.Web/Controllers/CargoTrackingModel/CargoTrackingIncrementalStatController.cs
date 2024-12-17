using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.Controllers.CargoTrackingModel
{
    public class CargoTrackingIncrementalStatController : ApiController
    {
        public int RecordsWaitingForUpdated { get; set; }
        public DateTime? DateOfOldestRecordStillWaiting { get; set; }
        public string  OldestRecordStillWaiting { get; set; }

        public HttpResponseMessage GetCargoTrackingIncrementalData( )
        {
            try
            {
                int tenant = AuthinticateTenant();
                string email = HttpContext.Current.User.Identity.Name;
                CargoTrackingIncrementalArgs IncrementalArgs = new CargoTrackingIncrementalArgs();
                CargoTrackingWatermarkQueryService cargoTrackingWatermarkQueryService = new CargoTrackingWatermarkQueryService(tenant);
                CargoTrackingIncrementalStatQueryService cargoTrackingIncrementalStatQueryService = new CargoTrackingIncrementalStatQueryService(tenant);
                List<CargoTrackingWatermark> cargoTrackingWatermarks = cargoTrackingWatermarkQueryService.GetAllWaterMarks();
                CargoTrackingIncrementalStat CargoTrackingIncrementalStat = cargoTrackingIncrementalStatQueryService.GetLastIncrementalStats();
                LastRecordDateUpdated(tenant,cargoTrackingWatermarks);

                IncrementalArgs.TotalUpdatedLast10Minutes = CargoTrackingIncrementalStat == null ? 0 : CargoTrackingIncrementalStat.Ports + CargoTrackingIncrementalStat.TransportModes + CargoTrackingIncrementalStat.Shipments + CargoTrackingIncrementalStat.Countries + CargoTrackingIncrementalStat.Cards;
                IncrementalArgs.IncrementalLastRun = cargoTrackingWatermarks == null ?null: cargoTrackingWatermarks.OrderByDescending(s => s.LastRun).FirstOrDefault().LastRun;
                IncrementalArgs.OldestUpdateStillWaiting = OldestRecordStillWaiting;
                IncrementalArgs.RecordsWating = RecordsWaitingForUpdated;
                IncrementalArgs.DateOfOldestUpdateStillWaiting = DateOfOldestRecordStillWaiting;

                return Request.CreateResponse(HttpStatusCode.OK, IncrementalArgs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private void LastRecordDateUpdated(int Tenant, List<CargoTrackingWatermark> cargoTrackingWatermark)
        {

            CargoTrackingWatermark WaterMarkForEntity = null;

            WaterMarkForEntity = cargoTrackingWatermark.Where(s=>s.TableName == "CargoTrackingShipments").FirstOrDefault();
            GetLastRecordUpdatedOnShipments(Tenant, WaterMarkForEntity);

            WaterMarkForEntity = cargoTrackingWatermark.Where(s => s.TableName == "CargoTrackingCards").FirstOrDefault();
            GetLastRecordUpdatedOnCards(Tenant, WaterMarkForEntity);

            WaterMarkForEntity = cargoTrackingWatermark.Where(s => s.TableName == "CargoTrackingPorts").FirstOrDefault();
            GetLastRecordUpdatedOnPorts(Tenant, WaterMarkForEntity);

            WaterMarkForEntity = cargoTrackingWatermark.Where(s => s.TableName == "CargoTrackingCountries").FirstOrDefault();
            GetLastRecordUpdatedOnCountries(Tenant, WaterMarkForEntity);


            WaterMarkForEntity = cargoTrackingWatermark.Where(s => s.TableName == "CargoTrackingCountries").FirstOrDefault();
            GetLastRecordUpdatedOnCountries(Tenant, WaterMarkForEntity);
 
            WaterMarkForEntity = cargoTrackingWatermark.Where(s => s.TableName == "CargoTrackingTransportModes").FirstOrDefault();
            GetLastRecordUpdatedOnTransportModes(Tenant, WaterMarkForEntity);

            
        }

        private void GetLastRecordUpdatedOnShipments(int Tenant, CargoTrackingWatermark WaterMarkForEntity)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(Tenant);
            IQueryable<Shipment> Shipments = shipmentQuery.GetAllShipments();
            if (WaterMarkForEntity != null)
            {
               Shipments = Shipments.Where(s => s.AutomaticLastUpdateDate > WaterMarkForEntity.LastUpdateDate);
            }
            RecordsWaitingForUpdated += Shipments.Count();
            Shipment LastShipmentUpdated = Shipments.OrderBy(s => s.AutomaticLastUpdateDate).FirstOrDefault();
            if (LastShipmentUpdated!=null && (DateOfOldestRecordStillWaiting == null || DateOfOldestRecordStillWaiting > LastShipmentUpdated.AutomaticLastUpdateDate))
            {
                DateOfOldestRecordStillWaiting = LastShipmentUpdated.AutomaticLastUpdateDate;
                OldestRecordStillWaiting = "Shipment " + LastShipmentUpdated.ShipmentNumber;
            }
            
        }
        private void GetLastRecordUpdatedOnCards(int Tenant, CargoTrackingWatermark WaterMarkForEntity)
        {
            CardQuery cardQuery = new CardQuery(Tenant);
            IQueryable<Card> Cards = cardQuery.GetAllCards();
            if (WaterMarkForEntity != null)
            {
               Cards = Cards.Where(s => s.AutomaticLastUpdateDate > WaterMarkForEntity.LastUpdateDate);
            }
            RecordsWaitingForUpdated += Cards.Count();
            Card LastCardUpdated = Cards.OrderBy(s => s.AutomaticLastUpdateDate).FirstOrDefault();
            if (LastCardUpdated!=null &&(DateOfOldestRecordStillWaiting == null || DateOfOldestRecordStillWaiting > LastCardUpdated.AutomaticLastUpdateDate))
            {
                DateOfOldestRecordStillWaiting = LastCardUpdated.AutomaticLastUpdateDate;
                OldestRecordStillWaiting = "Card " + LastCardUpdated.Code;

            }
          
        }

        private void GetLastRecordUpdatedOnPorts(int Tenant, CargoTrackingWatermark WaterMarkForEntity)
        {
            PortQuery portQuery = new PortQuery(Tenant);
            IQueryable<Port> Ports = portQuery.GetAllPorts();
            if (WaterMarkForEntity != null)
            {
                Ports = Ports.Where(s => s.AutomaticLastUpdateDate > WaterMarkForEntity.LastUpdateDate);
            }
            RecordsWaitingForUpdated += Ports.Count();
            Port LastPortUpdated = Ports.OrderBy(s => s.AutomaticLastUpdateDate).FirstOrDefault();
            if (LastPortUpdated!=null && (DateOfOldestRecordStillWaiting == null || DateOfOldestRecordStillWaiting > LastPortUpdated.AutomaticLastUpdateDate))
            {
                DateOfOldestRecordStillWaiting = LastPortUpdated.AutomaticLastUpdateDate;
                OldestRecordStillWaiting = "Port " + LastPortUpdated.Code;

            }
            
        }

        private void GetLastRecordUpdatedOnCountries(int Tenant, CargoTrackingWatermark WaterMarkForEntity)
        {
            CountryQuery countryQuery = new CountryQuery(Tenant);
            IQueryable<Country> Countries = countryQuery.GetAllCountries();
            if (WaterMarkForEntity != null)
            {
                Countries= Countries.Where(s => s.AutomaticLastUpdateDate > WaterMarkForEntity.LastUpdateDate);
            }
            RecordsWaitingForUpdated += Countries.Count();
            Country LastCountryUpdated = Countries.OrderBy(s => s.AutomaticLastUpdateDate).FirstOrDefault();
            if (LastCountryUpdated != null &&(DateOfOldestRecordStillWaiting == null || DateOfOldestRecordStillWaiting > LastCountryUpdated.AutomaticLastUpdateDate))
            {
                DateOfOldestRecordStillWaiting = LastCountryUpdated.AutomaticLastUpdateDate;
                OldestRecordStillWaiting = "Country " + LastCountryUpdated.Code;

            }
            
        }
 

        private void GetLastRecordUpdatedOnTransportModes(int Tenant, CargoTrackingWatermark WaterMarkForEntity)
        {
            TransportModeQuery TransportModeQuery = new TransportModeQuery(Tenant);
            IQueryable<TransportMode> TransportModes = TransportModeQuery.GetAllTransportModes();
            if (WaterMarkForEntity != null)
            {
                TransportModes = TransportModes.Where(s => s.AutomaticLastUpdateDate > WaterMarkForEntity.LastUpdateDate);
            }
            RecordsWaitingForUpdated += TransportModes.Count();
            TransportMode LastTransportModeUpdated = TransportModes.OrderBy(s => s.AutomaticLastUpdateDate).FirstOrDefault();
            if (LastTransportModeUpdated !=null && (DateOfOldestRecordStillWaiting == null || DateOfOldestRecordStillWaiting > LastTransportModeUpdated.AutomaticLastUpdateDate))
            {
                DateOfOldestRecordStillWaiting = LastTransportModeUpdated.AutomaticLastUpdateDate;
                OldestRecordStillWaiting = "TransportMode " + LastTransportModeUpdated.Name;

            }
            
        }

        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("InterestReport", "READ", authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }
    }


    
}