using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDocumentsController : ApiController
    {
        [HttpGet]
        [Route("DigitalDocuments/GetDigitalEntityDocuments")]
        public IHttpActionResult GetDigitalEntityDocuments(string entityId, string partnerType, string cardId)
        {
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, entityId, true, true);
                entityId = shipmentIdAndTenant.Item1;
                var tenant = shipmentIdAndTenant.Item2;

                List<SharedLogisticDocumentPM> output = new List<SharedLogisticDocumentPM>();
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment shipment = shipmentRepository.GetSingleShipment(entityId, tenant);
                
                DigitalPortalDocumentHelper digitalPortalDocumentHelper = new DigitalPortalDocumentHelper();
                
                if (shipment != null)
                {
                    var args = new
                    {
                        Id = shipment.Id,
                        ShipmentLevelCode = shipment.ShipmentLevelCode,
                        CustomerId = shipment.CustomerId,
                        Tenant = tenant,
                        PartnerType = partnerType
                    };

                    output = digitalPortalDocumentHelper.GetShipmentSharedDocuments(args).OrderBy(o => o.Name).ToList();
                }

                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }
    }
}