using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CurrencyRateServiceReference;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CD_NG_8348_Web02_CurrencyRateDetailResponseService : ResponseServiceBase<CD_NG_8348_Web02_CurrencyRateDetailResponseData, CD_NG_8348_Web02_CurrencyRateDetail, CD_NG_8347_Web01_CurrencyRateSearchRequestParams>
    {
        ICustomContext _context;
        public override void Update(CD_NG_8348_Web02_CurrencyRateDetail customResponse, CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams)
        {
            this._context = CustomContext.GetContext(requestParams.Tenant);

            if (customResponse.CurrencyRateList == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Customs Exchange Rate details in the Response");
            }
            if (requestParams.UpdateAllTenants)
            {
                var customsSettingQueryService = new CustomsSettingQueryService(requestParams.Tenant);
                var allCustomsSetting = customsSettingQueryService.GetAll();
                foreach(var customsSetting in allCustomsSetting)
                {
                    foreach (var customsExchangeRateItem in customResponse.CurrencyRateList)
                    {
                        UpdateCustomsExchangeRate(customsExchangeRateItem, customsSetting.Tenant);
                    }
                }
            }
            else
            {
                foreach (var customsExchangeRateItem in customResponse.CurrencyRateList)
                {
                    UpdateCustomsExchangeRate(customsExchangeRateItem, requestParams.Tenant);
                }
            }

       
            //Send the Table to Unifreight in order to update GRTRATE
            SendUpdateTableToUnifreight("GRTRATE", customResponse.CurrencyRateList.ToList(), requestParams.Tenant);            
        }

        private void UpdateCustomsExchangeRate(CD_NG_8348_Web02_CurrencyRateDetailCurrencyRateList customsExchangeRateItem, int tenant)
        {
            var myQueryService = new CustomsExchangeRateQueryService(this._context);
            var myUpdateService = new CustomsExchangeRateUpdateService(this._context, new Dictionary<string, IContext>(), tenant);
            var customsExchangeRatePM = new CustomsExchangeRatePM();
            
            var currencyTypeQuery = new CurrencyTypeQueryService(this._context);
            CurrencyTypePM CurrencyType = currencyTypeQuery.GetSingle(customsExchangeRateItem.currencyTypeID, false, false);
            if (CurrencyType != null)
            {
                string currencyTypeId = null;
                currencyTypeId = myQueryService.GetIdByCurrencyAndDate(customsExchangeRateItem.currencyTypeID, customsExchangeRateItem.startDate, tenant);
                if (currencyTypeId != null)
                {
                    customsExchangeRatePM = myQueryService.GetSingle(currencyTypeId, false, false);
                    customsExchangeRatePM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    customsExchangeRatePM.ChangeSetOp = ChangeSetOperation.Insert;
                    customsExchangeRatePM.CurrencyTypeCode = customsExchangeRateItem.currencyTypeID;
                    customsExchangeRatePM.RateDate = customsExchangeRateItem.startDate;
                    customsExchangeRatePM.Tenant = tenant;
                }
                decimal customsCurrencyRate;
                Decimal.TryParse(customsExchangeRateItem.customsCurrencyRate, out customsCurrencyRate);
                customsExchangeRatePM.ExchangeRate = customsCurrencyRate;

                myUpdateService.Update(customsExchangeRatePM, true);
            }
            else
            {
                decimal customsCurrencyRate;
                Decimal.TryParse(customsExchangeRateItem.customsCurrencyRate, out customsCurrencyRate);
                LogMessagingUtil.Instance.AppendLine("מטבע עם קוד " + customsExchangeRateItem.currencyTypeID + " עם שער" + customsCurrencyRate + " לא קיים ולכן לא נשמר בטבלה");
            }
        }

        private void SendUpdateTableToUnifreight(string TableID, List<CD_NG_8348_Web02_CurrencyRateDetailCurrencyRateList> myResponseTableData, int tenant)
        {
            var myCUSTOMS_TABLE = new CUSTOMS_TABLE();
            myCUSTOMS_TABLE.TABLECODE = new TABLECODE[] { new TABLECODE { TABLECODE_ID = TableID } }; ;
            var myTABLEDATAList = new List<TABLEDATA>();
            myResponseTableData.ForEach(recMehes =>
            {
                var newTABLEDATA = new TABLEDATA()
                {
                    TABLEDATA_ID = recMehes.currencyTypeID,
                    //TABLEDATA_NAME_ENG = recMehes.currencyTypeName,
                    TABLEDATA_NAME_HEB = recMehes.currencyTypeName,
                    TABLEDATA_ADDITIONALCODE1 = recMehes.customsCurrencyRate,
                    TABLEDATA_ADDITIONALCODE2 = recMehes.startDate.ToString(),
                    TABLEDATA_BLOCKED = "F",
                    TABLEDATA_REMARKS = recMehes.endDate.ToString(),
                    TABLEDATA_UNF_TABLE = "",
                };
                myTABLEDATAList.Add(newTABLEDATA);
            }
            );
            myCUSTOMS_TABLE.TABLECODE[0].TABLEDATA = myTABLEDATAList.ToArray();

            var qs = new Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService(1);
            UServerCommunication.SendUpdateTableToUnifreight(tenant, TableID, myCUSTOMS_TABLE, true);

        }


        public override CD_NG_8348_Web02_CurrencyRateDetailResponseData GetResponse(CD_NG_8348_Web02_CurrencyRateDetail customResponse, CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams)
        {

            bool succeeded = false;
            bool hasException = false;
            string exceptionMessage = null;

            if (customResponse.Exception == null)
            {
                succeeded = true;
            }
            else
            {
                string ExceptionDescription = "";
                var ExeptionDescription = "";

                if (customResponse.Exception != null)
                {
                    ExceptionDescription = ExeptionDescription;
                    hasException = true;
                    exceptionMessage = ExceptionDescription;
                }
            }

            CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData = new CD_NG_8348_Web02_CurrencyRateDetailResponseData() { Succeeded = succeeded, HasException = hasException, UserMessage = exceptionMessage };
            responseData.CurrencyRateList = new List<CurrencyRateResult>();
            foreach (var item in customResponse.CurrencyRateList)
            {
                decimal? customsCurrencyRateNull = null;
                decimal customsCurrencyRate;
                if (Decimal.TryParse(item.customsCurrencyRate, out customsCurrencyRate))
                {
                    customsCurrencyRateNull = customsCurrencyRate;
                }
                responseData.CurrencyRateList.Add(new CurrencyRateResult()
                {
                    CurrencyTypeId = item.currencyTypeID,
                    CurrencyTypeName = item.currencyTypeName,
                    CustomsCurrencyRate = customsCurrencyRateNull,
                    StartDate = item.startDate,
                    EndDate = item.endDate,
                    Tenant = requestParams.Tenant
                }
                );
            }
            return responseData;
        }
    }
}
