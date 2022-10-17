using Logitude.BL.ShipmentsModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Simplog.Data.ShipmentsModel;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System.Linq;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Extensions;
using WebFreight.Web.Extensions;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalMasterHousesController : ApiController
    {
        [HttpGet]
        [Route("DigitalMasterHouses/GetMasterWithConnectedHouses")]
        public IHttpActionResult GetMasterWithConnectedHouses(string cardId, string partnerType, int page = 1, int pageSize = 20)
        {
            try
            {
                string shipmentId = null;
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                var housesShipments = shipmentConsoleShipmentQuery.GetDigitalPortalMasterConnectedHouseShipments(shipmentId, tenant).GetPaged(page, pageSize);
                
                DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();
                
                List<DigitalMasterHouse> results = new List<DigitalMasterHouse>();

                DigitalMasterHouseResponse response = new DigitalMasterHouseResponse
                {
                    Master = MapMasterData(shipmentId, tenant, partnerType),
                };

                housesShipments.Data.ForEach(item =>
                {
                    var documentArgs = new
                    {
                        item.Id,
                        item.ShipmentLevelCode,
                        item.CustomerId,
                        Tenant = tenant,
                        PartnerType = partnerType
                    };

                    results.Add(new DigitalMasterHouse()
                    {
                        Id = item.Id,
                        ShipmentNumber = item.ShipmentNumber,
                        TransportModeId = item.TransportModeId,
                        ShipmentLevelCode = item.ShipmentLevelCode,
                        ShipperName = item.ShipperName,
                        ConsigneeName = item.ConsigneeName,
                        GrossWeight = item.GrossWeight,
                        Quantity = item.PackagesQuantity,
                        Documents = digitalPortalDocumentHelper.GetShipmentSharedDocuments(documentArgs).OrderBy(o => o.Name).ToList()
                    });
                });

                response.Houses = new PagedResult<DigitalMasterHouse>();
                response.Houses.Data = results;
                response.Houses.Pagination = housesShipments.Pagination;
                
                return Ok(response);
            }

            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        private DigitalMasterHouse MapMasterData(string shipmentId, int tenant, string partnerType)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterShipment = shipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);
            DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();

            var documentArgs = new
            {
                Id = masterShipment.Id,
                ShipmentLevelCode = masterShipment.ShipmentLevelCode,
                CustomerId = masterShipment.CustomerId,
                Tenant = tenant,
                PartnerType = partnerType
            };

            var master = new DigitalMasterHouse()
            {
                Id = masterShipment.Id,
                ShipmentNumber = masterShipment.ShipmentNumber,
                TransportModeId = masterShipment.TransportModeId,
                Tenant = masterShipment.Tenant,
                ShipmentLevelCode = masterShipment.ShipmentLevelCode,
                ShipperName = masterShipment.ShipperName,
                ConsigneeName = masterShipment.ConsigneeName,
                GrossWeight = masterShipment.GrossWeight,
                Quantity = masterShipment.PackagesQuantity,
                MainCarriageATD = masterShipment.MainCarriageATD,
                MainCarriageCarrierName = masterShipment.MainCarriageCarrierName,
                From = masterShipment.From,
                To = masterShipment.To,
                Documents = digitalPortalDocumentHelper.GetShipmentSharedDocuments(documentArgs).OrderBy(o => o.Name).ToList()
            };

            return master;
        }
    }
}