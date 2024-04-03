using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class APInvoiceService
    {

        public List<APInvoiceLinePM> CreateLines(Table table)
        {
            var lines = new List<APInvoiceLinePM>();
            var linsSet = table.CreateDynamicSet();
            foreach (var line in linsSet)
            {
                lines.Add(CreateLine(line));
            }
            return lines;
        }

        private APInvoiceLinePM CreateLine(dynamic line)
        {
            return new APInvoiceLineBuilder().WithDefualtValues()
                .ChargesTypeIdByCode((string)line.ChargesType)
                .Description((string)line.Description)
                .VatTypeId(BillingData.VATTypeZeroId)
                .VatPercentage((double)line.VatPercentage)
                .InvoiceCurrencyAmount((double)line.InvoiceCurrencyAmount)
                .ProfitCurrencyAmount((double)line.ProfitCurrencyAmount)
                .LocalCurrencyAmount((double)line.LocalCurrencyAmount)
                .ForiegnCurrencyAmount((double)line.ForiegnCurrencyAmount)
                .ForiegnCurrencyIdByCode((string)line.ForiegnCurrencyIdByCode)
                .ForiegnExchangeRate(1)
                .Build();
        }
        public APInvoicePM Create(Table table, List<APInvoiceLinePM> apInvoiceLines)
        {
            dynamic arInvoice = table.CreateDynamicInstance();
            return new APInvoiceBuilder().WithDefualtValues()
                .VendorId(FullAccountingData.VendorId)
                .BranchIdByCode((string)arInvoice.Branch)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .IsGeneralInvoice(true)
                .AccountingDate(DateTime.Now)
                .VATNumber((string)arInvoice.VATNumber.ToString())
                .InvoiceNumber(DateTime.Now.Ticks.ToString())
                .InvoiceCurrencyIdByCode((string)arInvoice.InvoiceCurrency)
                .InvoiceCurrencyExchangeRate((double)arInvoice.InvoiceCurrencyExchangeRate)
                .PaymentTermId(BillingData.PaymentTermCashId)
                .ProfitCurrencyIdByCode((string)arInvoice.ProfitCurrency)
                .ProfitCurrencyExchangeRate((double)arInvoice.ProfitCurrencyExchangeRate)
                .SetApproved(true)
                .DueDate(DateTime.Now.AddDays(1))
                .WithAPInvoiceLines(apInvoiceLines)
                .CalculateAmmount()
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
