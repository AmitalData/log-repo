using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Newtonsoft.Json;
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
                    string AuthURI = URI + "ImporterDeposition";
                    var serializedObject = JsonConvert.SerializeObject(importerDepositionAM);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(AuthURI, content);
                    var tempUser = result.Content.ReadAsStringAsync().Result;

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









    }
}