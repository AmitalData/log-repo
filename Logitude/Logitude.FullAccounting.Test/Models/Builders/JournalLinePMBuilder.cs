using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;


namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class JournalLinePMBuilder
    {
        private JournalLinePM JournalLine;
        
        public JournalLinePMBuilder()
        {
            this.Reset();
        }

        public JournalLinePM Build()
        {
            JournalLinePM result = JournalLine;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            JournalLine = new JournalLinePM();
        }

        
        public JournalLinePMBuilder AccountingDate(DateTime AccountingDate)
        {
            JournalLine.AccountingDate = AccountingDate;
            return this;
        }
        public JournalLinePMBuilder CreditAccountId(string creditAccountId)
        {
            JournalLine.CreditAccountId = creditAccountId;
            return this;
        }
        public JournalLinePMBuilder DebitAccountId(string debitAccountId)
        {
            JournalLine.DebitAccountId = debitAccountId;
            return this;
        }
        public JournalLinePMBuilder LocalAmount(decimal localAmount)
        {
            JournalLine.LocalAmount = localAmount;
            return this;
        }
        public JournalLinePMBuilder ForeignAmount(decimal foreignAmount)
        {
            JournalLine.ForeignAmount = foreignAmount;
            return this;
        }

        

        public JournalLinePMBuilder ActionTypeCode(string ActionTypeCode)
        {
            JournalLine.ActionTypeCode = ActionTypeCode;
            return this;
        }
        public JournalLinePMBuilder Line(int Line)
        {
            JournalLine.Line = Line;
            return this;
        }
        public JournalLinePMBuilder ExchangeRate(decimal? ExchangeRate)
        {
            JournalLine.ExchangeRate = ExchangeRate;
            return this;
        }

        public JournalLinePMBuilder CurrencyId(string CurrencyId)
        {
            JournalLine.CurrencyId = CurrencyId;
            return this;
        }
        public JournalLinePMBuilder CurrencyIdByCode(string code)
        {
            JournalLine.CurrencyId = MapCurrencyCode(code);
            return this;
        }
        public JournalLinePMBuilder ActionId(string actionId)
        {
            JournalLine.ActionId = actionId;
            return this;
        }
        public JournalLinePMBuilder ActionCode(string actionCode)
        {
            JournalLine.ActionCode = actionCode;
            return this;
        }
        public JournalLinePMBuilder ActionIdByCode(string actionCode)
        {
            JournalLine.ActionId = MapActionCode(actionCode);
            return this;
        }
        public JournalLinePMBuilder DocumentDate(DateTime documentDate)
        {
            JournalLine.DocumentDate = documentDate;
            return this;
        }
        public JournalLinePMBuilder DueDate(DateTime dueDate)
        {
            JournalLine.DueDate = dueDate;
            return this;
        }

        public JournalLinePMBuilder WithModel(JournalLinePM journalLine)
        {
            this.JournalLine = journalLine;
            return this;
        }

        public JournalLinePMBuilder WithDefualtValues()
        {
            JournalLine = new JournalLinePM
            {
                Tenant = UserTenant.Tenant,
                
            };
            return this;
        }

        private string MapCurrencyCode(string code)
        {
            switch (code)
            {
                case CurrencyCodes.NIS:
                    return BillingData.CurrencyNISId;
                default:
                    return null;
            }
            
        }
        private string MapActionCode(string code)
        {
            switch (code)
            {
                case "1":
                    return FullAccountingData.CreditActoinId;
                case "2":
                    return FullAccountingData.DebitActoinId;
                default:
                    return null;
            }
            
        }
        


    }
}
