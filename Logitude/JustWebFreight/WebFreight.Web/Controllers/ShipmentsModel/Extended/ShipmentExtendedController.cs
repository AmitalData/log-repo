using Logitude.BL.CommonDataModel.LogitudeGridExportToExcel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using static Logitude.BL.ShipmentsModel.Tools.Validating.ShipmentValidating;


namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class ShipmentExtendedController : ApiController
    {
        public HttpResponseMessage PostCustomShipment(ICustomInputData customEntityPM)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    customEntityPM.Tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (customEntityPM.ActionCode == ActionCode.Disconnect || customEntityPM.ActionCode == ActionCode.CheckAndConnect)
                    {
                        // get the existing shipment to set it in entityPoco
                        ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                        customEntityPM.Id = shipmentRepository.GetShipmentIdByShipmentNumber(customEntityPM.ShipmentNumber, tenant);
                        if (string.IsNullOrEmpty(customEntityPM.Id))
                        {
                            throw new MessageDetailsValidationException(MessageDetailsProvider.NotFoundShipmentNumber);
                        }
                    }

                    customEntityPM.IsCustomShipment = true;
                    IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                    ShipmentService service = new ShipmentService(objectContext, customEntityPM, SecurityUtility.GetAuthenticatedUser());

                    List<MessageDetails> response;
                    ICustomResponse customResponse = new ICustomResponse();

                    switch (customEntityPM.ActionCode)
                    {
                        case ActionCode.Disconnect:
                            service.DisconnectCustomShipment();
                            break;

                        case ActionCode.CheckAndConnect:
                            response = service.UpdateCustomShipment(additionalShipmentData: customEntityPM);
                            customResponse.MessageDetails = response.Where(d => d.MessageType == MessageTypeEnum.Warning).ToList();
                            break;

                        case ActionCode.NewCustomsFile:
                        case ActionCode.NewCustomsFileAfterCheck:
                            service.CreateCustomShipment(additionalShipmentData: customEntityPM);
                            break;

                        default:
                            throw new ValidationException("Action code not supported");
                    }
                    scope.Complete();

                    customResponse.Success = true;
                    customResponse.CustomsFile = new List<string> { customEntityPM.ShipmentNumber };
                    return Request.CreateResponse(HttpStatusCode.OK, customResponse);
                }
            }
            catch (MessageDetailsValidationException ex)
            {
                ICustomResponse customResponse = new ICustomResponse()
                {
                    Success = false,
                    MessageDetails = ex.MessageDetails,
                    CustomsFile = ex.CustomsFile != null ? ex.CustomsFile : null
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, customResponse);
            }
            catch (ValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
    public class ICustomInputData : ShipmentPM, IAdditionalShipmentData
    {
        public ActionCode ActionCode { get; set; }
        public string UnloadPortCode { get; set; }
        public string StorageSiteCode { get; set; }
    }

    public class ICustomResponse
    {
        public bool Success { get; set; }
        public List<string> CustomsFile { get; set; }
        public List<MessageDetails> MessageDetails { get; set; }
    }
}