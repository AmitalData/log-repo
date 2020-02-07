using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Customs
{
    public class CustomsPreparationCalls
    {
        public static async Task PrepareVariables()
        {
            await GetCusstomerGECU();
            await GetCustomsTransportModeA();
            await GetCustomsHouseTypesITEST();

        }



        private static async Task GetCusstomerGECU()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Customerviews" + QueryFiltersPreparation.GetUrlParameters("GE:Cusstomer"));
            CustomerList cardList = RestClientService.ParseResponse<CustomerList>(response);
            if (cardList == null)
                await CreateCusstomerGECU();
            else
            {
                CustomsVariables.CusstomerGECUId = cardList.Id;
                CustomsVariables.CusstomerGECUCode = cardList.Code;
            }
        }
        private static async Task CreateCusstomerGECU()
        {
            CustomerPM cusstomerGECUPM = GetNewCardCusstomerGECU();
            HttpResponseMessage response = await RestClientService.PostAsync(cusstomerGECUPM, "Customers");
            CustomerPM customerPM = RestClientService.ParseResponse<CustomerPM>(response);
            CustomsVariables.CusstomerGECUId = customerPM.Id;
            CustomsVariables.CusstomerGECUCode = customerPM.Code;
        }
        private static CustomerPM GetNewCardCusstomerGECU()
        {
            CustomerPM customerPM = new CustomerPM();
            customerPM.Tenant = IntegrationTestLoginParameters.Tenant;
            customerPM.EnglishName = "GE:Cusstomer";
            customerPM.LocalName = "GE:Cusstomer";
            customerPM.CityName = "Guaynabo";
            customerPM.CountryName = "Guaynabo";
            customerPM.PartnerTypeId = "CS";
            customerPM.InActive = true;
            customerPM.IsCustomer = true;
            customerPM.EnableConsolidationInvoices = false;
            customerPM.IsActiveForMobile = false;
            return customerPM;
        }
        private static async Task GetCustomsTransportModeA()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomsTransportModeViews" + QueryFiltersPreparation.GetUrlParameters("A"));
            CustomsTransportModeList customsTransportModeList = RestClientService.ParseResponse<CustomsTransportModeList>(response);
            CustomsVariables.CustomsTransportModeACode = customsTransportModeList.Code;
        }
        private static async Task GetCustomsHouseTypesITEST()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomsHouseTypes/GetSingle?code=14");
            CustomsHouseTypeList customsHouseTypeList = RestClientService.ParseResponse<CustomsHouseTypeList>(response);
            CustomsVariables.CustomsHouseTypesITESTCode = customsHouseTypeList.Code;
        }


    }
}
