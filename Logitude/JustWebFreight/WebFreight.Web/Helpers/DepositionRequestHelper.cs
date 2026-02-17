using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class DepositionRequestHelper
    {
        public async Task<Response> SendDepositionRequestTaskToLogBox(DepositionRequestPM depositionRequestPM)
        {
            Response response = new Response();

            ShipmentQuery shipmentQuery = new ShipmentQuery(depositionRequestPM.Tenant);
            int? customerTenant =   shipmentQuery.GetCustomerTenantByShipmentNumber(depositionRequestPM.ForwarderShipmentNumber, depositionRequestPM.Tenant);
            if (customerTenant != null)
            {
                SettingQuery settingQuery = new SettingQuery();
                string URI = settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
                DepositionRequestAM depositionRequestAM = new DepositionRequestAM();
                MapDepositionRequestPMToDepositionRequestAM(depositionRequestPM, depositionRequestAM);
                depositionRequestAM.CustomerTenant = (int)customerTenant;

                string token = string.Empty;
                APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
                {
                    PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
                    SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
                };
                using (var client = new HttpClient())
                {
                    string AuthURI = URI + "APIAuthentication";
                    var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(AuthURI, content);
                    var tempUser = result.Content.ReadAsStringAsync().Result;
                    ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
                    token = User.Token;
                }
                
                if (!string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Token", token);
                        string AuthURI = URI + "DepositionRequest";
                        var serializedObject = JsonConvert.SerializeObject(depositionRequestAM);
                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                        var resultData = await client.PostAsync(AuthURI, content);
                        if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            response.Result = resultData.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            var temp1 = resultData.Content.ReadAsStringAsync().Result;
                            APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                            if (EXC != null)
                            {
                                throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                            }
                        }

                    }
                }
                else
                {
                    response.Result = "Sorry! this user is not authorized!";
                }
            }
            else
            {
                response.Result = "Shipment not found";
            }


            return response;
        }

        public string StartDepositionRequestTask(DepositionRequestAM depositionRequestAM)
        {
            int tenant = depositionRequestAM.CustomerTenant;
            string systemEmail = "system@tenant" + tenant + ".com";

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            string shipmentId = shipmentQuery.GetShipmentIdByForwarderShipmentNumber(depositionRequestAM.ForwarderShipmentNumber, tenant);

            if (shipmentId != null)
            {
                ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                ShipmentComputedFields shipmentComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentId, tenant);
                if (shipmentComputedFields != null)
                {
                    shipmentComputedFields.IsDepositionRequired = true;
                    shipmentComputedFields.ImporterDepositionRequestDetails = depositionRequestAM.VendorCode + "^" + depositionRequestAM.VendorName;
                    ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                    shipmentComputedFieldsHelper.UpdateShipmentComputedFields(shipmentComputedFields);
                }
            }
            return shipmentId;

        }




        private void MapDepositionRequestPMToDepositionRequestAM(DepositionRequestPM depositionRequestPM, DepositionRequestAM depositionRequestAM)
        {
            depositionRequestAM.ForwarderShipmentNumber = depositionRequestPM.ForwarderShipmentNumber;
            depositionRequestAM.VendorCode = depositionRequestPM.VendorCode;
            depositionRequestAM.VendorName = depositionRequestPM.VendorName;
            depositionRequestAM.RequestDateTime = depositionRequestPM.RequestDateTime;
            depositionRequestAM.Tenant = depositionRequestPM.Tenant;
        }

        #region API Logs

        public string AddAPILogs(DepositionRequestAM depositionRequestAM)
        {
            int tenant = depositionRequestAM.CustomerTenant;

            APILogsPM LogPM = new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", depositionRequestAM.CustomerTenant),
                CorrelationId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "I",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                Tenant = tenant,
                Subject = "Deposition request task send to cloud"

            };
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
            apiLogsService.Create(LogPM);


            var msg = "Deposition request tasK send to cloud";
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(depositionRequestAM), null, null, "");
            return LogPM.Id;


        }

        #endregion

    }
}