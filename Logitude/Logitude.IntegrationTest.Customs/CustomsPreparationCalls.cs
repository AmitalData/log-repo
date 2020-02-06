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
            await GetCardCusstomerGECU();
            await GetCustomsTransportModeA();
            await GetCustomsHouseTypesITEST();

        }



        private static async Task GetCardCusstomerGECU()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CardViews" + QueryFiltersPreparation.GetUrlParameters("GE:Cusstomer"));
            CardList cardList = RestClientService.ParseResponse<CardList>(response);
            if (cardList == null)
                await CreateCardCusstomerGECU();
            else
            {
                CustomsVariables.CardCusstomerGECUId = cardList.Id;
                CustomsVariables.CardCusstomerGECUCode = cardList.Code;
            }
        }
        private static async Task CreateCardCusstomerGECU()
        {
            CardPM cardCusstomerPM = GetNewCardCusstomerGECU();
            HttpResponseMessage response = await RestClientService.PostAsync(cardCusstomerPM, "Cards");
            CardPM cardPM = RestClientService.ParseResponse<CardPM>(response);
            CustomsVariables.CardCusstomerGECUId = cardPM.Id;
            CustomsVariables.CardCusstomerGECUCode = cardPM.Code;
        }
        private static CardPM GetNewCardCusstomerGECU()
        {
            CardPM cardPM = new CardPM();
            cardPM.Tenant = IntegrationTestLoginParameters.Tenant;
            cardPM.EnglishName = "GE:Cusstomer";
            cardPM.LocalName = "GE:Cusstomer";
            cardPM.Code = "GECU";
            cardPM.PartnerTypeId = "CS";
            cardPM.CityName = "Guaynabo";
            cardPM.CountryName = "Guaynabo";
            cardPM.InActive = true;
            cardPM.InternetAccess = true;
            cardPM.SharedLogisticsInvitationStatusCode = 1;
            cardPM.IsCustomer = true;
            cardPM.EnableConsolidationInvoices = false;
            cardPM.IsActiveForMobile = false;
            cardPM.IsInternationalPartner = false;
            cardPM.IsAutonomy = false;
            cardPM.PartnerTypeName = "Cusstomer";
            //cardPM.VatTypeId = "";
            return cardPM;
        }
        private static async Task GetCustomsTransportModeA()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomsTransportModeViews" + QueryFiltersPreparation.GetUrlParameters("A"));
            CustomsTransportModeList customsTransportModeList = RestClientService.ParseResponse<CustomsTransportModeList>(response);
            CustomsVariables.CustomsTransportModeACode = customsTransportModeList.Code;
        }
        private static async Task GetCustomsHouseTypesITEST()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("CustomsHouseTypeViews" + QueryFiltersPreparation.GetUrlParameters("ITEST"));
            CustomsHouseTypeList customsHouseTypeList = RestClientService.ParseResponse<CustomsHouseTypeList>(response);
            if (customsHouseTypeList == null)
                await CreateCustomsHouseTypesITEST();
            else
            {
                CustomsVariables.CustomsHouseTypesITESTCode = customsHouseTypeList.Code;
            }
        }
        private static async Task CreateCustomsHouseTypesITEST()
        {
            CustomsHouseTypePM customsHouseTypeTESTPM = GetNewCustomsHouseTypesITEST();
            HttpResponseMessage response = await RestClientService.PostAsync(customsHouseTypeTESTPM, "CustomsHouseTypes");
            CustomsHouseTypePM customsHouseTypePM = RestClientService.ParseResponse<CustomsHouseTypePM>(response);
            CustomsVariables.CustomsHouseTypesITESTCode = customsHouseTypePM.Code;
        }
        private static CustomsHouseTypePM GetNewCustomsHouseTypesITEST()
        {
            CustomsHouseTypePM customsHouseTypePM = new CustomsHouseTypePM();
            customsHouseTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            customsHouseTypePM.EnglishName = "GE:ITEST";
            customsHouseTypePM.LocalName = "GE:ITEST";
            customsHouseTypePM.SearchFields = "ITEST,GE:ITEST";
            customsHouseTypePM.Code = "ITEST";
            customsHouseTypePM.Inactive = false;
            return customsHouseTypePM;
        }

    }
}
