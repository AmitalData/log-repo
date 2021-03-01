using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Logitude.BL.CommonDataModel.Tools.EntityService;
    
namespace Logitude.BL.InfrastructureModel.APIDataContract.Messages
{
    public class RatesUpdateService
    {
        private string errorMsg = "";
        private RatesUpdate ratesUpdate;
        private int tenant;
        private List<RatesTable> ratesList;
        private IWebFreightContext iWebFreightContext;
        private RatesTableService ratesTableService;
        private RatesTableQuery ratesTableQuery;
        private string baseCurrencyId;
        private CurrencyQuery currencyQuery;
        private string foreignCurrencyId;
        private RatesTable rateList;
        private ICommonDataContext objectContext;
        public RatesUpdateService(RatesUpdate ratesUpdate, int tenant)
        {
            this.ratesUpdate = ratesUpdate;
            this.tenant = tenant;
            this.iWebFreightContext = WebFreightContext.GetContext(tenant);
            this.ratesTableService = new RatesTableService(iWebFreightContext, tenant);
            this.currencyQuery = new CurrencyQuery(tenant);
            this.objectContext = CommonDataContext.GetContext(tenant);
        }
        public void CleanXMLText()
        {
            ratesUpdate.ComputingPartnerCode = !string.IsNullOrEmpty(ratesUpdate.ComputingPartnerCode) ?
                                                Regex.Replace(ratesUpdate.ComputingPartnerCode, @"\s+", "") : ratesUpdate.ComputingPartnerCode;
        }
        public void ValidateRatesDataMapping()
        {
            this.ValidateMandatoryFields();
            this.ValidateComputingPartner();
            this.ValidateSendingTheSameCurrency();
            this.ValidateSendingFutureDate();
            this.ValidatCurrencyExistenceOnCurrentTenantOrTenantZero();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new ApplicationException(errorMsg);
            }
        }
        private void ValidateSendingFutureDate()
        {
            var currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            var isFutureDate = ratesUpdate.RateUpdateList.Where(a => a.RateDate > currentDate).Any();
            if(isFutureDate)
            {
                errorMsg = errorMsg + "Cant add future date rate. ";
            }
        }
        private void ValidateMandatoryFields()
        {
            foreach (var item in ratesUpdate.RateUpdateList.ToList())
            {
                if (string.IsNullOrEmpty(item.Currency.Code))
                {
                    errorMsg = errorMsg + "Currency code is required. ";
                }
                if (item.RateDate == null)
                {
                    errorMsg = errorMsg + "RateDate is required. ";
                }
                if (string.IsNullOrEmpty(item.Currency.Code))
                {
                    errorMsg = errorMsg + "Currency code is required. ";
                }
                if (item.Rate == null)
                {
                    errorMsg = errorMsg + "Rate is required. ";
                }
            }
        }
        private void ValidateSendingTheSameCurrency()
        {
            var isSendingTheSameCurrency = ratesUpdate.RateUpdateList.Where(x=>x.Currency != null).GroupBy(x => x.Currency.Code)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key).Any();
            if (isSendingTheSameCurrency)
            {
                errorMsg = errorMsg + "Can't send the same currency more than once. ";
            }
        }
        private void ValidateComputingPartner()
        {
            var isComputingPartnerRequired = ratesUpdate.RateUpdateList.Where(a => !string.IsNullOrEmpty(a.Currency.PartnerCode) && string.IsNullOrEmpty(ratesUpdate.ComputingPartnerCode)).Any();
            if (isComputingPartnerRequired)
            {
                errorMsg = errorMsg + "ComputingPartnerCode is required. ";
            }
            foreach (var item in ratesUpdate.RateUpdateList.ToList())
            {
                if (item.Currency != null && !string.IsNullOrEmpty(item.Currency.PartnerCode))
                {
                    ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(tenant);
                    var MyCode = helper.GetLogitudeCodeTranslation(item.Currency.PartnerCode, ratesUpdate.ComputingPartnerCode, "Currency");
                    if (string.IsNullOrEmpty(MyCode))
                    {
                        errorMsg = errorMsg + "Currency with Partner Code " + item.Currency.PartnerCode + " doesn't match any record. ";
                    }
                }
            }
        }
        private void ValidatCurrencyExistenceOnCurrentTenantOrTenantZero()
        {
            var allCurrencies = (from d in objectContext.Currencies
                                 where (d.Tenant == tenant || d.Tenant == 0)
                                 select d.Code).Distinct().ToList();
            foreach (var item in ratesUpdate.RateUpdateList.ToList())
            {
                var isCurrencyExist = allCurrencies.Where(a => a == item.Currency.Code).FirstOrDefault();
                if (string.IsNullOrEmpty(isCurrencyExist))
                {
                    errorMsg = errorMsg + "Currency code " + item.Currency.Code + " is not exist. ";
                }
            }
        }
        public void UpdateRatesData()
        {
            this.GetRates();
            this.GetBaseCurrency();
            this.HandelUpdateRatesList();
        }
        private void HandelUpdateRatesList()
        {
            foreach (var item in ratesUpdate.RateUpdateList.ToList())
            {
                this.GetRateTable(item);
                if (rateList == null) this.InsertRate(item);
                else this.UpdateRate(item, rateList.Id);
            }
        }
        private void GetRateTable(RateUpdate item)
        {
            this.foreignCurrencyId = GetCurrencyIdByCode(item.Currency);
            this.rateList = ratesList.Find(d => d.ForeignCurrencyId == this.foreignCurrencyId && d.ValueDate == item.RateDate && d.BaseCurrencyId == this.baseCurrencyId);
        }
        private void GetBaseCurrency()
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
            this.baseCurrencyId = currentTenant.CurrencyId;
        }
        private void InsertRate(RateUpdate item)
        {
            RatesTablePM entityPM = new RatesTablePM();
            entityPM.Tenant = tenant;
            entityPM.BaseCurrencyId = this.baseCurrencyId;
            entityPM.ForeignCurrencyId = foreignCurrencyId;
            entityPM.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.ValueDate = item.RateDate;
            entityPM.Rate = item.Rate;
            ratesTableService.Create(entityPM);
        }
        private void UpdateRate(RateUpdate item, string rateId)
        {
            RatesTablePM entityPM = this.ratesTableQuery.GetSinglePM(rateId, tenant);
            entityPM.Rate = item.Rate;
            entityPM.ValueDate = item.RateDate;
            entityPM.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            ratesTableService.Update(entityPM);
        }
        private void GetRates()
        {
            RatesTableRepository ratesTableRepository = new RatesTableRepository(iWebFreightContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);
            ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            entityPocos = entityPocos.OrderByDescending(r => r.ValueDate);
            ratesList = entityPocos.ToList();
        }
        private string GetCurrencyIdByCode(Currency currencyItem)
        {
            string currencyId = "";
            string currencyCode = currencyItem.Code;
            if (!string.IsNullOrEmpty(currencyItem.PartnerCode))
            {
                ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(tenant);
                currencyCode = helper.GetLogitudeCodeTranslation(currencyItem.PartnerCode, this.ratesUpdate.ComputingPartnerCode, "Currency");
            }
            var currencyPM = currencyQuery.GetSinglePMByCode(currencyCode, tenant);
            if (currencyPM == null)
            {
                currencyId = this.CopyCurrencyFromTenantZero(currencyCode);
            }
            else
                currencyId = currencyPM.Id;
            return currencyId;
        }
        private string CopyCurrencyFromTenantZero(string currencyCode)
        {
            var currencyPM = currencyQuery.GetSinglePMByCode(currencyCode, 0);
            CurrencyService currencyService = new CurrencyService(objectContext, tenant);
            var currencyId = currencyService.CopyCurrencyToTenant(currencyPM.Id, tenant);
            return currencyId;
        }
    }
}
