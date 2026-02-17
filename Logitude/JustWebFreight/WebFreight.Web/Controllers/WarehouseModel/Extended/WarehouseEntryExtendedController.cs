using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WarehouseModel.Extended
{
    public class WarehouseEntryExtendedController : ApiController
    {
        public HttpResponseMessage GetRecentWarehouseEntries()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("WarehouseEntry", "READ", tenant);


       
                ContactQuery contactQuery = new ContactQuery(tenant);

                string loggedContactId = null;
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedContactId = contact.Id;
                }

                string objectTableId = null;
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("WarehouseEntry", 0, true);
                if (objecttable != null)
                {
                    objectTableId = objecttable.Id;
                }
                
                WarehouseEntryQueryService warehouseEntryQueryService = new WarehouseEntryQueryService(tenant);
                IQueryable<WarehouseEntryList> first = warehouseEntryQueryService.GetRecentWarehouseEntriesListsByTenant(loggedContactId ,objectTableId, tenant).AsQueryable();
                IQueryable<WarehouseEntryList> list = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage GetWarehouseConnectedEntitiesByEntityId(string entityId)
        {
            try  //GetQuoteConnectedEntities
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", tenant);

                List<WarehouseConnectedEntity> myResult = new List<WarehouseConnectedEntity>();

                IShipmentsContext shipmentContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentContext);
                ShipmentDataView myShipment = shipmentRepository.GetSingleShipmentDataView(entityId, tenant);

                myResult.Add(new WarehouseConnectedEntity()
                {
                    EntityId = myShipment.Id,
                    EntityNumber = myShipment.ShipmentNumber,
                    ObjectTable = "Shipment",
                    EntityStatus = myShipment.StatusName,
                    ShipmentType = myShipment.ShipmentTypeName + " " + myShipment.ShipmentLevelName,
                    OpenDate = myShipment.CreateDateTime,
                    House = myShipment.House,
                    Master = myShipment.Master,
                    Customer = myShipment.CustomerName,
                    From = myShipment.MainCarriageFromPortCode,
                    To = myShipment.MainCarriageFinalDestinationPortCode,
                    GrossWeight = myShipment.GrossWeight,
                    VolumeInKG = myShipment.Volume,
                });

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        


    }


    public class WarehouseConnectedEntity
    {
        public string EntityId { get; set; }
        public string EntityNumber { get; set; }
        public string ObjectTable { get; set; }
        public string EntityStatus { get; set; }
        public string ShipmentType { get; set; }
        public DateTime? OpenDate { get; set; }
        public string House { get; set; }
        public string Master { get; set; }
        public string Customer { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Double? GrossWeight { get; set; }
        public Double? VolumeInKG { get; set; }
    }
}