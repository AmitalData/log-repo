using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class ShipmentComputedFieldExtendedController : ApiController
    {

        public  async Task<HttpResponseMessage> GetMarkCompleteDepositionRequest(string id , string directionId ,  string forwardershipmentNumber , string forwarderPartnerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                ShipmentComputedFields shipmentComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(id, tenant);

                if (shipmentComputedFields != null && shipmentComputedFields.IsDepositionRequired)
                {
                    shipmentComputedFields.IsDepositionRequired = false;
                    shipmentComputedFieldsRepository.Update(shipmentComputedFields);
                    shipmentComputedFieldsRepository.SubmitChanges();
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                    var objecttable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);

                    HybridPartnerQuery hybridPartnerQuery = new HybridPartnerQuery(tenant);
                    int partnerTenant = hybridPartnerQuery.GetPartnerTenantById(forwarderPartnerId);

                    APILogsPM LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", tenant),
                        CorrelationId = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "O",
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Status = "I",
                        ObjectTableId = objecttable != null ? objecttable.Id : null,
                        EntityId = id,
                        Tenant = tenant,
                        Subject = "Send VDC status to UNF"

                    };

                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                    APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
                    apiLogsService.Create(LogPM);

                    ImporterDepositionHelper importerDepositionHelper = new ImporterDepositionHelper();
                    return await importerDepositionHelper.SendVDCStatusToUNF(directionId, forwardershipmentNumber, tenant, partnerTenant, LogPM);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }

                //return null;

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}