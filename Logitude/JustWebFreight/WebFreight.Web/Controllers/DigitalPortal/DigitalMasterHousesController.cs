using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Simplog.Data.ShipmentsModel;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
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
        public IHttpActionResult GetMasterWithConnectedHouses(string shipmentId, string cardId, string partnerType, int page = 1, int pageSize = 20)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                var housesShipments = shipmentConsoleShipmentQuery.GetDigitalPortalMasterConnectedHouseShipments(shipmentId, tenant).GetPaged(page, pageSize);
                DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();
                List<DigitalMasterHouse> results = new List<DigitalMasterHouse>();

                results.Add(MapMasterData(shipmentId, tenant, partnerType));

                housesShipments.Data.ForEach(item =>
                {
                    var documentArgs = new
                    {
                        Id = item.Id,
                        ShipmentLevelCode = item.ShipmentLevelCode,
                        CustomerId = item.CustomerId,
                        Tenant = tenant,
                        PartnerType = partnerType
                    };

                    results.Add(new DigitalMasterHouse()
                    {
                        Id = item.Id,
                        ShipmentLevelCode = item.ShipmentLevelCode,
                        ShipperName = item.ShipperName,
                        ConsigneeName = item.ConsigneeName,
                        GrossWeight = item.GrossWeight,
                        Quantity = item.PackagesQuantity,
                        Documents = digitalPortalDocumentHelper.GetShipmentSharedDocuments(documentArgs).OrderBy(o => o.Name).ToList()
                    });
                });

                return Ok(results);
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