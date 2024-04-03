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
using System.Net.Http;
using System.Net;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDocumentsController : ApiController
    {
        [HttpGet]
        [Route("DigitalDocuments/GetDigitalEntityDocuments")]
        public HttpResponseMessage GetDigitalEntityDocuments(string entityId, string partnerType, string cardId, bool isExternal = false)
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

                return Request.CreateResponse(HttpStatusCode.OK, output);
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

        [HttpGet]
        [Route("DigitalDocuments/GetDigitalDocuments")]
        public HttpResponseMessage GetDigitalDocuments(string cardId)
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

                return Request.CreateResponse(HttpStatusCode.OK, result);
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


        [HttpGet]
        [Route("DigitalDocuments/GetDocumentDownloadToken")]
        public HttpResponseMessage GetDocumentDownloadToken()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                DocumentDownloadTokenService documentDownloadTokenService = new DocumentDownloadTokenService(authToken);
                string documentDownloadToken = documentDownloadTokenService.Get();

                return Request.CreateResponse(HttpStatusCode.OK, documentDownloadToken);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}