using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;


namespace Logitude.TimeManagementTests.Models.Builders
{
    public class JournalLinePMBuilder
    {
        private JournalLinePM journalLinePM;
        
        public JournalLinePMBuilder()
        {
            this.Reset();
        }

        public JournalLinePM Build()
        {
            JournalLinePM result = journalLinePM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            journalLinePM = new JournalLinePM();
        }

        
        public JournalLinePMBuilder AccountingDate(DateTime AccountingDate)
        {
            journalLinePM.AccountingDate = AccountingDate;
            return this;
        }

        public JournalLinePMBuilder Days(string ActionId)
        {
            journalLinePM.ActionId = ActionId;
            return this;
        }

        public JournalLinePMBuilder Description(string ActionTypeCode)
        {
            journalLinePM.ActionTypeCode = ActionTypeCode;
            return this;
        }
        public JournalLinePMBuilder Line(int Line)
        {
            journalLinePM.Line = Line;
            return this;
        }
        public JournalLinePMBuilder ExchangeRate(decimal? ExchangeRate)
        {
            journalLinePM.ExchangeRate = ExchangeRate;
            return this;
        }

        public JournalLinePMBuilder CurrencyId(string CurrencyId)
        {
            journalLinePM.CurrencyId = CurrencyId;
            return this;
        }
        public JournalLinePMBuilder CurrencyIdByCode(string code)
        {
            journalLinePM.CurrencyId = MapCurrencyCode(code);
            return this;
        }
        public JournalLinePMBuilder ActionId(string actionId)
        {
            journalLinePM.ActionId = actionId;
            return this;
        }
        public JournalLinePMBuilder ActionIdByCode(string actionCode)
        {
            journalLinePM.CurrencyId = MapCurrencyCode(actionCode);
            return this;
        }

        
        

        public JournalLinePMBuilder WithModel(JournalLinePM tMEmployeeTime)
        {
            journalLinePM = tMEmployeeTime;
            return this;
        }

        public JournalLinePMBuilder WithDefualtValues()
        {
            journalLinePM = new JournalLinePM
            {
                Tenant = UserTenant.Tenant
            };
            return this;
        }

        public string MapCurrencyCode(string code)
        {
            switch (code)
            {
                case "NIS":
                    return BillingData.CurrencyNISId;
                default:
                    return null;
            }
            
        }
        public string MapActionCode(string code)
        {
            switch (code)
            {
                case "Credit":
                    return FullAccountingData.Credit;
                case "Debit":
                    return FullAccountingData.Debit;
                default:
                    return null;
            }
            
        }


    }
}
