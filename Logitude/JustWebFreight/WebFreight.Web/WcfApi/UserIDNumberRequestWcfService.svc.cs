
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{


    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserIDNumberRequestWcfService : IUserIDNumberRequestWcfService

    {

        public Response RequestUserIDNumber(UserIdNumberRequestPM userIdNumberRequestPM)
        {
            var response = new Response(); 

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(userIdNumberRequestPM.Tenant);

                if (userIdNumberRequestPM != null)
                {
                    string ShipmentAdditionalDataId = UpdateShipmentAdditionalDataFromIncomingUserIdNumberRequest(userIdNumberRequestPM, userIdNumberRequestPM.Tenant);
                    if(!string.IsNullOrEmpty(ShipmentAdditionalDataId))
                    {
                        response.Result = ShipmentAdditionalDataId;
                        response.HasError = false;
                    }
                    else
                    {
                        throw new Exception("Shipment is null");
                    }
                    //int ImporterTenant = GetImporterTenantByShipmentNumber(declarationApprovalRequestPM.Tenant, declarationApprovalRequestPM.ForwarderShipmentNumber);
                    //AddQueueToSendApprovalRequestToLogBox(ShipmentAdditionalDataId, declarationApprovalRequestPM.Tenant, ImporterTenant);
                }

                return response;
            }
            catch (Exception ex)
            {
                return HandleExceptionAsResponse(ex);
            }

        }

        private int GetImporterTenantByShipmentNumber(int tenant,string shipmentNumber)
        {
            ShipmentQuery ShipmentQuery = new ShipmentQuery(tenant);
            int? ImporterTenant = ShipmentQuery.GetCustomerTenantByShipmentNumber(shipmentNumber, tenant);
            if (ImporterTenant == null)
            {
                ImporterTenant = -1;
            }
            return (int)ImporterTenant;
        }

        private void AddQueueToSendApprovalRequestToLogBox(string shipmentAdditionalDataId, int tenant,int importerTenant)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DeclarationApprovalRequestQueue", 0);
            
            queueservice.Send(new Dictionary<string, string>() { { "Id", shipmentAdditionalDataId }, { "Tenant", tenant.ToString() }, { "ImporterTenant", importerTenant.ToString() } }, tenant);
        }

        private string UpdateShipmentAdditionalDataFromIncomingUserIdNumberRequest(UserIdNumberRequestPM userIdNumberRequestPM, int tenant)
        {
            string shipmentId = string.Empty;
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM shipmentPM = shipmentQuery.GetSingleShipmentPMByNumber(userIdNumberRequestPM.ForwarderShipmentNumber, tenant);
            if (shipmentPM!=null)
            {
                shipmentPM.ExternalStatuses = GetVIRStatusIfIsFirstUserIdNumberRequest(shipmentPM);
                shipmentPM.IsHybrid = true;
                shipmentPM.IsShipmentAdditionalCloudDataChange = true;
                shipmentPM.UserIdNumberXMLData = LogitudeXmlSerializer.SerializeObjectToUTF8XmlString(userIdNumberRequestPM);
                shipmentPM.IsUserIDNumberRequired = true;
                shipmentId = shipmentPM.Id;
                string email =Logitude.BL.Security.SecurityUtility.GetAuthenticatedUser(tenant);
                IShipmentsContext objectContext = ShipmentsContext.GetContext(shipmentPM.Tenant);
                ShipmentService shipmentService = new ShipmentService(objectContext, shipmentPM, email);
                shipmentService.Update();
            }
            return shipmentId;

        }

        private string GetVIRStatusIfIsFirstUserIdNumberRequest(ShipmentPM shipmentPM)
        {
            string externalStatuses = string.Empty;
            if (IsFirstUserIdNumberRequest(shipmentPM))
            {
                externalStatuses = "VIR";
            }

            return externalStatuses;
        }

        private bool IsFirstUserIdNumberRequest(ShipmentPM shipmentPM)
        {
            return string.IsNullOrEmpty(shipmentPM.UserIdNumber) && shipmentPM.UserIdNumberUpdateDate == null && !shipmentPM.IsUserIDNumberRequired;
        }

        private Response HandleExceptionAsResponse(Exception ex)
        {
            Response response = new Response();
            response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
            response.HasError = true;
            response.ErrorMessage = ex.Message;
            response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                response.ErrorMessage += Environment.NewLine + ex.StackTrace;
            }

            return response;

        }


    }

}
