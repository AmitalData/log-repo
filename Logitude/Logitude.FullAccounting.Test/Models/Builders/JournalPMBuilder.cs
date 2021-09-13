using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.TimeManagementTests.Models.Builders
{
    public class JournalPMBuilder
    {
        private JournalPM journalPM;

        public JournalPMBuilder()
        {
            this.Reset();
        }

        public JournalPM Build()
        {
            JournalPM result = journalPM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            journalPM = new JournalPM();
        }

        public JournalPMBuilder AccountingDate(DateTime accountingDate)
        {
            journalPM.AccountingDate = accountingDate;
            return this;
        }
        public JournalPMBuilder DocumentDate(DateTime documentDate)
        {
            journalPM.DocumentDate = documentDate;
            return this;
        }
        public JournalPMBuilder DueDate(DateTime dueDate)
        {
            journalPM.DueDate = dueDate;
            return this;
        }

        public JournalPMBuilder CurrencyId(string currencyId)
        {
            journalPM.CurrencyId = currencyId;
            return this;
        }
        public JournalPMBuilder StatusCode(string statusCode)
        {
            journalPM.StatusCode = statusCode;
            return this;
        }
        public JournalPMBuilder AccountingEntityCode(string accountingEntityCode)
        {
            journalPM.AccountingEntityCode = accountingEntityCode;
            return this;
        }
        public JournalPMBuilder WithJournalLines(List<JournalLinePM> journalLines)
        {
            journalPM.JournalLines = journalLines;
            return this;
        }
        public JournalPMBuilder TypeCode(string typeCode)
        {
            journalPM.TypeCode = typeCode;
            return this;
        }

        public JournalPMBuilder WithModel(JournalPM tMEmployeeTime)
        {
            journalPM = tMEmployeeTime;
            return this;
        }

        public JournalPMBuilder WithDefualtValues()
        {
            journalPM = new JournalPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId
            };
            return this;
        }

    }
}
