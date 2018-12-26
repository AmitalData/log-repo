using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class ImporterDepositionHelper
    {
        public async Task<Response> SendImporterDepositionToLogBox(ImporterDepositionPM importerDepositionPM)
        {
            Response response = new Response();
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(importerDepositionPM.Tenant);
            IQueryable<CustomerTenantAccessList> customerTenantAccessLists = customerTenantAccessQuery.GetCustomerTenantAccessesByImporterVat(importerDepositionPM.ImporterVat);
            if (customerTenantAccessLists.Count() > 0)
            {
                int? customerTenant = null;

                if (customerTenantAccessLists.Count() > 1)
                {
                    List<int> customerTenantIds = customerTenantAccessLists.Select(d => d.CustomerTenant).ToList();
                    CustomerDepositionRepository customerDepositionRepository = new CustomerDepositionRepository(importerDepositionPM.Tenant);
                    customerTenant = customerDepositionRepository.GetLastCustomerDepositionCreated(customerTenantIds);
                }

                if (customerTenant == null) customerTenant = customerTenantAccessLists.FirstOrDefault().CustomerTenant;

                SettingQuery settingQuery = new SettingQuery();
                string URI = settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
                ImporterDepositionAM importerDepositionAM = new ImporterDepositionAM();
                MapImporterDepositionPMToImporterDepositionAM(importerDepositionPM, importerDepositionAM);
                importerDepositionAM.CustomerTenant = (int)customerTenant;


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
                        string AuthURI = URI + "ImporterDeposition";
                        var serializedObject = JsonConvert.SerializeObject(importerDepositionAM);
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
              
            }
            else
            {
                response.Result = "Importer Deposition Send to cloud Successfully";
            }

            return response;

        }

        public void StartImporterDeposition(ImporterDepositionAM importerDepositionAM)
        {

            int tenant = importerDepositionAM.CustomerTenant;

            CustomsShipperQuery customsShipperQuery = new CustomsShipperQuery(tenant);
            CustomsShipperPM customsShipperPM = customsShipperQuery.GetSinglePMByShipperCode(importerDepositionAM.ShipperCode, tenant);
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            CustomsShipperService customsShipperService = new CustomsShipperService(objectContext, tenant);

            if (customsShipperPM == null)
            {
                customsShipperPM = new CustomsShipperPM();
                customsShipperPM.Tenant = tenant;
                customsShipperPM.CustomsShipperCode = importerDepositionAM.ShipperCode;
                customsShipperPM.EnglishName = importerDepositionAM.ShipperName;
                customsShipperPM.LocalName = importerDepositionAM.ShipperName;
                customsShipperPM.ShipperVAT = importerDepositionAM.ShipperVAT;
                customsShipperPM.CountryCode = importerDepositionAM.ShipperCountry;
                customsShipperPM.ValidDepositionNumber = importerDepositionAM.DepositionNumber;

                UpdateValidityDate(customsShipperPM, importerDepositionAM.ValidityStartDate, importerDepositionAM.ValidityEndDate);

                customsShipperPM.Addresses = new List<AddressPM>();

                var address = new AddressPM()
                {
                    Description = "Main Address",
                    Name = importerDepositionAM.ShipperName,
                    CountryCode = importerDepositionAM.ShipperCountry,
                    AddressTypeId = "M",
                };

                if (!string.IsNullOrEmpty(importerDepositionAM.ShipperCountry))
                {
                    CountryQuery countryQuery = new CountryQuery(tenant);
                    CountryPM countryPM = countryQuery.GetSinglePMByCode(importerDepositionAM.ShipperCountry, tenant);
                    if (countryPM != null)
                    {
                        address.CountryId = countryPM.Id;
                        address.CountryName = countryPM.EnglishName;
                    }
                }

                customsShipperPM.Addresses.Add(address);
                customsShipperService.Create(customsShipperPM);
            }


            CustomerDepositionRepository customerDepositionRepository = new CustomerDepositionRepository(tenant);
            CustomerDeposition customerDeposition = new CustomerDeposition()
            {
                Id = IdCounter.GetNumber("CustomerDeposition", tenant).ToString(),
                Tenant = tenant,
                CustomsShipperId = customsShipperPM.Id,
                DepositionNumber = importerDepositionAM.DepositionNumber,
                ValidityStartDate = importerDepositionAM.ValidityStartDate,
                ValidityEndDate = importerDepositionAM.ValidityEndDate,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
            };
            customerDepositionRepository.Add(customerDeposition);
            customerDepositionRepository.SubmitChanges();

            UpdateValidityDate(customsShipperPM, customerDeposition.ValidityStartDate, customerDeposition.ValidityEndDate);
            if(customsShipperPM.IsChange) customsShipperService.Update(customsShipperPM);


        }

        private static void UpdateValidityDate(CustomsShipperPM customsShipperPM , DateTime? validityStartDate , DateTime? validityEndDate)
        {
            if (validityStartDate != null && validityEndDate != null)
            {
                if (customsShipperPM.ValidityStartDate != validityStartDate || customsShipperPM.ValidityEndDate != validityEndDate)
                {
                    DateTime currentDate = DateTime.Now;
                    if (currentDate >= validityStartDate && currentDate < validityEndDate)
                    {
                        customsShipperPM.ValidityStartDate = validityStartDate;
                        customsShipperPM.ValidityEndDate = validityEndDate;
                        customsShipperPM.IsChange = true;
                    }
                  
                }
            }
        }

        #region API Logs

        public string AddAPILogs(ImporterDepositionAM importerDepositionAM)
        {
            int tenant = importerDepositionAM.CustomerTenant;

            APILogsPM LogPM = new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", importerDepositionAM.CustomerTenant),
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
                
            };
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
            apiLogsService.Create(LogPM);


            var msg = "Importer Deposition Send to cloud";
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "I", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(importerDepositionAM), null, null, "");
            return LogPM.Id;
            

        }

        #endregion

        #region Map ImporterDepositionPM to ImporterDepositionAM
        private void MapImporterDepositionPMToImporterDepositionAM(ImporterDepositionPM importerDepositionPM, ImporterDepositionAM importerDepositionAM)
        {

            importerDepositionAM.DepositionNumber = importerDepositionPM.DepositionNumber;
            importerDepositionAM.ShipperCode = importerDepositionPM.ShipperCode;
            importerDepositionAM.ShipperCountry = importerDepositionPM.ShipperCountry;
            importerDepositionAM.ShipperName = importerDepositionPM.ShipperName;
            importerDepositionAM.ShipperVAT = importerDepositionPM.ShipperVAT;
            importerDepositionAM.ValidityStartDate = importerDepositionPM.ValidityStartDate;
            importerDepositionAM.ValidityEndDate = importerDepositionPM.ValidityEndDate;

        }
        #endregion 

    }
}