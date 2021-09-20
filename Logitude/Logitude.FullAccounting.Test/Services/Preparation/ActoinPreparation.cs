using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class ActoinPreparation
    {
        const string CreditActoinCode = "1";
        const string DebitActoinCode = "2";
        public void Prepare()
        {
            FullAccountingData.CreditActoinId = GetByCode(CreditActoinCode);
            FullAccountingData.DebitActoinId = GetByCode(DebitActoinCode);
        }
        private string GetByCode(string code)
        {
            var id = GetIdByCode(code);
            if (string.IsNullOrEmpty(id))
            {
                return Create(code);
            }
            return id;
        }
        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<JournalActionType>>(Urls.JournalActionTypeViewsByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private ApiQueryFilters GetFilterByCode(string code)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Value(code)
                .Build();
        }

        private string Create(string code)
        {
            var journalActionType = CreateInstance(code);
            var response = APICaller.CallPost<JournalActionType>(journalActionType, Urls.JournalActionTypesController, UserTenant.Token);
            return response.Data?.Id;
        }
        private JournalActionType CreateInstance(string code)
        {
            switch (code)
            {
                case CreditActoinCode:
                    return CreateCreditInstance(code);
                case DebitActoinCode:
                    return CreateDebitInstance(code);
                default:
                    return null;
            }
        }

        private JournalActionType CreateCreditInstance(string code)
        {
            return new JournalActionType()
            { 
                Code = code,
                EnglishName = "Credit",
                LocalName = "Credit",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Credit"

            };
        }
        private JournalActionType CreateDebitInstance(string code)
        {
            return new JournalActionType()
            {
                Code = code,
                EnglishName = "Debit",
                LocalName = "Debit",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Debit"

            };
        }
    }
}
