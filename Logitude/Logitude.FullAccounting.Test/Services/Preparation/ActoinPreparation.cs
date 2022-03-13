using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class ActoinPreparation
    {
        
        public void Prepare()
        {
            FullAccountingData.CreditActoinId = GetCredit();
            FullAccountingData.DebitActoinId = GetDebit();
        }

        private string GetDebit()
        {
            return GetIdByCode(AccountingActionCodes.Credit) ?? Create(CreateDebitInstance());
        }

        private string GetCredit()
        {
            return GetIdByCode(AccountingActionCodes.Credit) ?? Create(CreateCreditInstance());
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

        private string Create(JournalActionType journalActionType)
        {
            var response = APICaller.CallPost<JournalActionType>(journalActionType, Urls.JournalActionTypesController, UserTenant.Token);
            return response.Data?.Id;
        }
       

        private JournalActionType CreateCreditInstance()
        {
            return new JournalActionType()
            { 
                Code = AccountingActionCodes.Credit,
                EnglishName = "Credit",
                LocalName = "Credit",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{AccountingActionCodes.Credit},Credit"

            };
        }
        private JournalActionType CreateDebitInstance()
        {
            return new JournalActionType()
            {
                Code = AccountingActionCodes.Debit,
                EnglishName = "Debit",
                LocalName = "Debit",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{AccountingActionCodes.Debit},Debit"

            };
        }
    }
}
