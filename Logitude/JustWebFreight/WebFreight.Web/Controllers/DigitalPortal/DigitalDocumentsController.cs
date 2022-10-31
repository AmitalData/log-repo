using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using System;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDocumentsController : ApiController
    {
        [HttpGet]
        [Route("DigitalDocuments/GetDigitalEntityDocuments")]
        public IHttpActionResult GetDigitalEntityDocuments(string entityId, string partnerType, string cardId, bool isExternal = false)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, entityId, true, true);
                entityId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;

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
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalDocuments/GetDigitalDocuments")]
        public IHttpActionResult GetDigitalDocuments(string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(authToken.Tenant);
                var objectTbleName = "Shipment";
                var result  =  documentTypeQuery.GetDigitalDocuments(objectTbleName, authToken.Tenant);

                return Ok(result);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }
    }
}