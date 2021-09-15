using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class JournalService
    {
        const string JournalAccountingEntityCode = "1";
        public List<JournalLinePM> CreateLines(Table table)
        {
            var lines = new List<JournalLinePM>();
            var linsSet = table.CreateDynamicSet();
            foreach (var line in linsSet)
            {
                lines.Add(CreateLine(line));
            }
            return lines;
        }

        private JournalLinePM CreateLine(dynamic line)
        {
            return new JournalLinePMBuilder().WithDefualtValues()
                .Line((int)line.Line)
                .CurrencyIdByCode((string)line.CurrencyId)
                .AccountingDate((DateTime)line.DocumentDate)
                .ExchangeRate((decimal?)line.ExchangeRate)
                .ActionIdByCode(GetActionCodeByName((string)line.Action))
                .ActionCode(GetActionCodeByName((string)line.Action))
                .DueDate((DateTime)line.DueDate)
                .DocumentDate((DateTime)line.DocumentDate)
                .CreditAccountId(FullAccountingData.GLAccount1Id)
                .DebitAccountId(FullAccountingData.GLAccount2Id)
                .LocalAmount((decimal)line.LocalAmount)
                .ForeignAmount((decimal)line.LocalAmount)
                .Build();
        }
        public JournalPM Create(Table table, List<JournalLinePM> journallines)
        {
            dynamic journal = table.CreateDynamicInstance();
            return new JournalPMBuilder()
                .WithDefualtValues()
                .AccountingDate((DateTime)journal.AccountingDate)
                .DueDate((DateTime)journal.AccountingDate)
                .DocumentDate((DateTime)journal.AccountingDate)
                .AccountingDate((DateTime)journal.AccountingDate)
                .StatusCode((int)StatusCodeEnum.Approved + "")
                .AccountingEntityCode(JournalAccountingEntityCode)
                .WithJournalLines(journallines)
                .TypeCode((int)JournalTypeEnum.Regular + "")
                .Build();

        }
        public JournalPM Add(JournalPM approvedJournal)
        {
            var journal = APICaller.CallPost<JournalPM>(approvedJournal, Urls.JournalsController, UserTenant.Token);
            return journal.Data;
        }


        private string GetActionCodeByName(string actionName)
        {
            switch (actionName)
            {
                case "Credit":
                    return "1";
                case "Debit":
                    return "2";
                default:
                    return "1";
            }
        }
    }
}
