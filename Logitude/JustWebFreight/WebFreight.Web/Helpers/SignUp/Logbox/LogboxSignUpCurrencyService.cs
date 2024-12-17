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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Threading;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Security;

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
        public void UpdateNewTenant(SignUpInfoClass signUpInfoClass, AddressPM TenantAddress, string connectedCustomerId)
        {
            int requestTenant = signUpInfoClass.IsCreateLogboxTenantFromCloud ? tenant : signUpInfoClass.Tenant;
            TenantService service = new TenantService(commonContext, tenant);
            TenantQuery query = new TenantQuery(tenant);

            var CrmCustomer = CardRepository.GetSingleCard(signUpInfoClass.CustomerId, requestTenant, false);

            if (CrmCustomer == null)
            {
                throw new ApplicationException("Customer with Id: " + signUpInfoClass.CustomerId + " is not exist!");
            }

            var newTenant = query.GetSinglePM(tenant);
            newTenant.CurrencyId = Cur.Id;
            newTenant.ProfitCurrencyId = ProfCur.Id;
            newTenant.ProfitCurrencyRate = 4;
            newTenant.VatNumber = CrmCustomer.VatNumber;
            newTenant.AddressId = TenantAddress.Id;
            newTenant.IsDocumentsArchive = true;
            newTenant.CustomerId = connectedCustomerId;
            newTenant.AgentId = signUpInfoClass.IsCreateLogboxTenantFromCloud ? GetNewAgentId(signUpInfoClass) : newTenant.AgentId;
            newTenant.CustomerTenantShareImportFile = true;
            newTenant.AutoArchiveOnInvoice = !string.IsNullOrEmpty(newTenant.PrivateLabelId) || signUpInfoClass.IsCreateLogboxTenantFromCloud ? true : newTenant.AutoArchiveOnInvoice;
            newTenant.AutoArchiveOnPODExport = !string.IsNullOrEmpty(newTenant.PrivateLabelId) || signUpInfoClass.IsCreateLogboxTenantFromCloud ? true : newTenant.AutoArchiveOnPODExport;
            newTenant.DocumentShareAsDefault = !string.IsNullOrEmpty(newTenant.PrivateLabelId) || signUpInfoClass.IsCreateLogboxTenantFromCloud ? true : newTenant.DocumentShareAsDefault;
            service.Update(newTenant);
        }

        private string GetNewAgentId(SignUpInfoClass signUpInfoClass)
        {
            CountryRepository countryRepository = new CountryRepository(tenant);
            string countryId = countryRepository.GetCountryIdByCode(signUpInfoClass.CountryCode, tenant);
            const string AgentPartnerTypeCode = "AG";
            string contactPMId = GetContactPMId(signUpInfoClass);

            AgentPM agentPM = new AgentPM
            {
                Id = IdCounter.GetNumber("Card", tenant).ToString(),
                EnglishName = signUpInfoClass.Company,
                LocalName = signUpInfoClass.Company,
                VatNumber = signUpInfoClass.VatNumber,
                Tenant = tenant,
                CountryId = countryId,
                CountryCode = signUpInfoClass.CountryCode,
                CountryName = signUpInfoClass.CountryName,
                CityName = signUpInfoClass.City,
                PrimaryContactPhone = signUpInfoClass.Phone,
                Code = CodeCounter.GetNumber("Agent", tenant).ToString(),
                PartnerTypeId = AgentPartnerTypeCode,
            };

            var thread = new Thread(() =>
            {
                SecurityUtility.IsWorkerRoleCall = true;
                AuthenticationUtil.AuthenticatedUserEmail = signUpInfoClass.Email;
                AgentService agentService = new AgentService(commonContext, agentPM, contactPMId);
                agentService.Create(agentPM);
            });

            thread.Start();
            thread.Join();

            return agentPM.Id;
        }

        private string GetContactPMId(SignUpInfoClass signUpInfoClass)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(signUpInfoClass.Email, tenant);
            if (contactPM == null)
            {
                throw new ApplicationException("Contact With Email: " + signUpInfoClass.Email + " is not exist!");
            }

            string contactPMId = contactPM.Id;
            return contactPMId;
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