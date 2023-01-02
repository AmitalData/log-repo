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
using Logitude.SystemLogs;
using WebFreight.Web.Security;
using System.Net.Http;
using System.Net;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalMasterHousesController : ApiController
    {
        [HttpGet]
        [Route("DigitalMasterHouses/GetMasterWithConnectedHouses")]
        public HttpResponseMessage GetMasterWithConnectedHouses(string cardId, string partnerType = "CS", int page = 1, int pageSize = 20, bool isExternal = false)
        {
            int tenant = 0;
            string email = "";

            try
            {
                if (partnerType == null)
                {
                    partnerType = "CS";
                }

                string shipmentId = null;
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId);
                shipmentId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                var housesShipments = shipmentConsoleShipmentQuery.GetDigitalPortalMasterConnectedHouseShipments(shipmentId, tenant).GetPaged(page, pageSize);
                
                DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();
                
                List<DigitalMasterHouse> results = new List<DigitalMasterHouse>();

                DigitalMasterHouseResponse response = new DigitalMasterHouseResponse
                {
                    Master = MapMasterData(shipmentId, tenant, partnerType, isExternal, cardId),
                };

                housesShipments.Data.ForEach(item =>
                {
                    var documentArgs = new
                    {
                        item.Id,
                        item.ShipmentLevelCode,
                        item.CustomerId,
                        Tenant = tenant,
                        PartnerType = partnerType,
                        IsExternal = isExternal
                    };

                    results.Add(new DigitalMasterHouse()
                    {
                        Id = item.Id,
                        ShipmentNumber = item.ShipmentNumber,
                        TransportModeId = item.TransportModeId,
                        DirectionId = item.DirectionId,
                        ShipmentTypeName = item.ShipmentType?.Name,
                        ShipmentSubTypeName = item.ShipmentSubType?.Name,
                        ShipmentLevelCode = item.ShipmentLevelCode,
                        ShipperName = item.ShipperName,
                        ConsigneeName = item.ConsigneeName,
                        GrossWeight = item.GrossWeight,
                        Quantity = item.PackagesQuantity,
                        Documents = digitalPortalDocumentHelper.GetShipmentSharedDocuments(documentArgs).OrderBy(o => o.Name).ToList(),
                    });
                });

                response.Houses = new PagedResult<DigitalMasterHouse>();
                response.Houses.Data = results;
                response.Houses.Pagination = housesShipments.Pagination;

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private DigitalMasterHouse MapMasterData(string shipmentId, int tenant, string partnerType, bool isExternal, string cardId)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM masterShipment = shipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);
            DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();

            var documentArgs = new
            {
                Id = masterShipment.Id,
                ShipmentLevelCode = masterShipment.ShipmentLevelCode,
                CustomerId = masterShipment.CustomerId != null ? masterShipment.CustomerId : cardId,
                Tenant = tenant,
                PartnerType = partnerType,
                IsExternal = isExternal
            };

            var master = new DigitalMasterHouse()
            {
                Id = masterShipment.Id,
                ShipmentNumber = masterShipment.ShipmentNumber,
                TransportModeId = masterShipment.TransportModeId,
                DirectionId = masterShipment.DirectionId,
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
                FromCountryCode = masterShipment.FromCountryCode,
                FromPortCountry = masterShipment.FromPortCountry,
                ToPortCountry = masterShipment.ToPortCountry,
                ToCountryCode = masterShipment.ToCountryCode,
                ShipmentTypeName = masterShipment.ShipmentTypeName,
                ShipmentSubTypeName = masterShipment.ShipmentSubTypeName,
                Documents = digitalPortalDocumentHelper.GetShipmentSharedDocuments(documentArgs).OrderBy(o => o.Name).ToList()
            };


            return master;
        }
    }
}