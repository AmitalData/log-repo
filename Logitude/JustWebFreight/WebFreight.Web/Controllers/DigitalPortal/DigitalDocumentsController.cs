using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDocumentsController : ApiController
    {
        [HttpGet]
        [Route("DigitalDocuments/GetDigitalEntityDocuments")]
        public IHttpActionResult GetDigitalEntityDocuments(string entityId, string partnerType, string cardId, bool isExternal = false)
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
                        PartnerType = partnerType,
                        IsExternal = isExternal
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

        [HttpGet]
        [Route("DigitalDocuments/GetDigitalDocuments")]
        public IHttpActionResult GetDigitalDocuments(string cardId)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(authToken.Tenant);
                var objectTbleName = "Shipment";
                var result  =  documentTypeQuery.GetDigitalDocuments(objectTbleName, authToken.Tenant);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }
    }
}