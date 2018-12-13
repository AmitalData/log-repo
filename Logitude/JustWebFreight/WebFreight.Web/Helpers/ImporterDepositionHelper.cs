using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

namespace WebFreight.Web.Helpers
{
    public class ImporterDepositionHelper
    {
        public async void SendImporterDepositionToLogBox(ImporterDepositionPM importerDepositionPM)
        {
            
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

                using (var client = new HttpClient())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    client.DefaultRequestHeaders.Add("Token", token);

                    string AuthURI = URI + "ImporterDeposition";
                    var serializedObject = JsonConvert.SerializeObject(importerDepositionAM);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(AuthURI, content);
    
                }
            }

  

        }

        public void AddAPILogs(ImporterDepositionAM importerDepositionAM)
        {
            int tenant = importerDepositionAM.CustomerTenant;

            APILogsPM LogPM = new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", importerDepositionAM.CustomerTenant),
                CorrelationId = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                Tenant = tenant
            };
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
            apiLogsService.Create(LogPM);

             var msg = "Request Sent Successfully";
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "D", 0, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(importerDepositionAM), null, null, "");

        }

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
                customsShipperPM.EnglishName =  importerDepositionAM.ShipperName;
                customsShipperPM.LocalName = importerDepositionAM.ShipperName;
                customsShipperPM.ShipperVAT = importerDepositionAM.ShipperVAT;
                customsShipperPM.ValidityStartDate = importerDepositionAM.ValidityStartDate;
                customsShipperPM.ValidityEndDate = importerDepositionAM.ValidityEndDate;
                customsShipperPM.CountryCode = importerDepositionAM.ShipperCountry;
                customsShipperPM.ValidDepositionNumber = importerDepositionAM.DepositionNumber;
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

                if (customsShipperPM.ValidityStartDate != null && customsShipperPM.ValidityEndDate != null)
                {
                    if (DateTime.Now < customsShipperPM.ValidityStartDate)
                    {
                        customsShipperPM.FutureDepositionExist = true;
                    }
                }

                customsShipperPM.Addresses.Add(address);
                customsShipperService.Create(customsShipperPM);
            }

            if (customsShipperPM != null)
            {
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

                if (customerDeposition.ValidityStartDate != null && customerDeposition.ValidityEndDate != null)
                {
                    if (customerDeposition.ValidityStartDate !=  customsShipperPM.ValidityStartDate || customerDeposition.ValidityEndDate !=  customsShipperPM.ValidityEndDate )
                    {
                        DateTime currentDate = DateTime.Now;
                        if (currentDate < customerDeposition.ValidityStartDate)
                        {
                            customsShipperPM.FutureDepositionExist = true;
                        }
                        else if (currentDate > customerDeposition.ValidityStartDate && currentDate < customerDeposition.ValidityEndDate)
                        {
                            customsShipperPM.ValidityStartDate = customerDeposition.ValidityStartDate;
                            customsShipperPM.ValidityEndDate = customerDeposition.ValidityEndDate;
                            customsShipperPM.ValidDepositionNumber = customerDeposition.DepositionNumber;

                        }

                        customsShipperService.Update(customsShipperPM);

                    }
                }
            }
            

            
        }





    }
}