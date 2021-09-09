using Logitude.FullAccounting.Test.Models;
using System;


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

        public JournalPMBuilder LocationCode(DateTime AccountingDate)
        {
            journalPM.AccountingDate = AccountingDate;
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
                
            };
            return this;
        }

    }
}
