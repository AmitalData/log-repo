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
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web.Helpers
{
    public class ImporterDepositionHelper
    {
        public async void SendImporterDepositionToLogBox(ImporterDepositionPM importerDepositionPM)
        {
            
            CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(0);
            IQueryable<CustomerTenantAccessList> customerTenantAccessLists = customerTenantAccessQuery.GetCustomerTenantAccessesByImporterVat(importerDepositionPM.ImporterVat);
            if (customerTenantAccessLists.Count() > 0)
            {
                int customerTenant = customerTenantAccessLists.FirstOrDefault().CustomerTenant;

                SettingQuery settingQuery = new SettingQuery();
                string URI = settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
                ImporterDepositionAM importerDepositionAM = new ImporterDepositionAM();
                MapImporterDepositionPMToImporterDepositionAM(importerDepositionPM, importerDepositionAM);
                importerDepositionAM.CustomerTenant = customerTenant;

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

                if (!string.IsNullOrEmpty(importerDepositionAM.ShipperCountry))
                {
                    CountryQuery countryQuery = new CountryQuery(tenant);
                    CountryPM countryPM = countryQuery.GetSinglePMByCode(importerDepositionAM.ShipperCountry, tenant);
                    if (countryPM != null)
                    {
                        customsShipperPM.CountryId = countryPM.Id;
                        customsShipperPM.CountryName = countryPM.EnglishName;
                    }
                }

                if (customsShipperPM.ValidityStartDate != null && customsShipperPM.ValidityEndDate != null)
                {
                    if (DateTime.Now < customsShipperPM.ValidityStartDate)
                    {
                        customsShipperPM.FutureDepositionExist = true;
                    }

                }

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
                };
                customerDepositionRepository.Add(customerDeposition);
                customerDepositionRepository.SubmitChanges();

                if (customerDeposition.ValidityStartDate != null && customerDeposition.ValidityEndDate!=null)
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