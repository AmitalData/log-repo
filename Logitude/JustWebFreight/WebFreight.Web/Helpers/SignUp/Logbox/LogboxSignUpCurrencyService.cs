using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace WebFreight.Web.Helpers.SignUp.Logbox
{
    public class LogboxSignUpCurrencyService
    {
        private Currency Cur;
        private Currency ProfCur;
        private ICommonDataContext commonContext;
        private int tenant;
        public LogboxSignUpCurrencyService(ICommonDataContext CommonContext, int Tenant)
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(0);
            Cur = currencyRepository.GetSingleCurrencyByCode("NIS", 0);
            ProfCur = currencyRepository.GetSingleCurrencyByCode("USD", 0);
            commonContext = CommonContext;
            tenant = Tenant;
        }
        public void UpdateNewTenant(SignUpInfoClass signUpInfoClass, AddressPM TenantAddress)
        {
            TenantService service = new TenantService(commonContext, tenant);
            TenantQuery query = new TenantQuery(tenant);

            var CrmCustomer = CardRepository.GetSingleCard(signUpInfoClass.CustomerId, signUpInfoClass.Tenant, false);

            var newTenant = query.GetSinglePM(tenant);
            newTenant.CurrencyId = Cur.Id;
            newTenant.ProfitCurrencyId = ProfCur.Id;
            newTenant.ProfitCurrencyRate = 4;
            newTenant.VatNumber = CrmCustomer.VatNumber;
            newTenant.AddressId = TenantAddress.Id;
            newTenant.IsDocumentsArchive = true;
            newTenant.CustomerId = signUpInfoClass.CustomerId;
            newTenant.CustomerTenantShareImportFile = true;
            newTenant.AutoArchiveOnInvoice = !string.IsNullOrEmpty(newTenant.PrivateLabelId) ? true : newTenant.AutoArchiveOnInvoice;
            newTenant.DocumentShareAsDefault = !string.IsNullOrEmpty(newTenant.PrivateLabelId) ? true : newTenant.DocumentShareAsDefault;
            service.Update(newTenant);
        }

        public void AddLocalAndProfitCurrency()
        {
            CurrencyRepository currencyRepository = new CurrencyRepository(0);
            CurrencyQuery currencyQuery = new CurrencyQuery(currencyRepository);
            var currencies = currencyQuery.GetCurrenciesByTenantPM(0).Where(d => d.Code == "USD" || d.Code == "NIS").ToList();
            currencyRepository = new CurrencyRepository(tenant);
            Currency newCurrency = new Currency()
            {
                Code = Cur.Code,
                EnglishName = Cur.EnglishName,
                Id = IdCounter.GetNumber("Currency", tenant).ToString(),
                InActive = Cur.InActive,
                LocalName = Cur.LocalName,
                Notes = Cur.Notes,
                Tenant = tenant,
                SearchFields = Cur.SearchFields,
            };
            currencyRepository.Add(newCurrency);
            newCurrency = new Currency()
            {
                Code = ProfCur.Code,
                EnglishName = ProfCur.EnglishName,
                Id = IdCounter.GetNumber("Currency", tenant).ToString(),
                InActive = ProfCur.InActive,
                LocalName = ProfCur.LocalName,
                Notes = ProfCur.Notes,
                Tenant = tenant,
                SearchFields = ProfCur.SearchFields,
            };
            currencyRepository.Add(newCurrency);
            currencyRepository.SubmitChanges();
        }
    }
}