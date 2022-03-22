using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class ARInvoiceService
    {

        public List<ARInvoiceLinePM> CreateLines(Table table)
        {
            var lines = new List<ARInvoiceLinePM>();
            var linsSet = table.CreateDynamicSet();
            foreach (var line in linsSet)
            {
                lines.Add(CreateLine(line));
            }
            return lines;
        }

        private ARInvoiceLinePM CreateLine(dynamic line)
        {
            return new ARInvoiceLinePMBuilder().WithDefualtValues()
                .ForiegnCurrencyIdByCode((string)line.ForiegnCurrency)
                .ForiegnCurrencyCode((string)line.ForiegnCurrency)
                .ForiegnExchangeRate((double)line.ForiegnExchangeRate)
                .LineActionCode(ARInvoiceLineActionsEnum.Revenue)
                .InvoiceCurrencyCode((string)line.InvoiceCurrency)
                .InvoiceLocalCurrencyCode((string)line.InvoiceCurrency)
                .ChargesTypeIdByCode((string)line.ChargesType)
                .Description((string)line.Description)
                .LocalDescription((string)line.Description)
                .VatTypeId(BillingData.VATTypeZeroId)
                .VatPercentage((double)line.VatPercentage)
                .ExchangeRateDate((DateTime)line.DueDate)
                .Quantity((double)line.Quantity)
                .InvoiceCurrencyAmount((double)line.InvoiceCurrencyAmount)
                .UnitPrice((double)line.UnitPrice)
                .ProfitCurrencyAmount((double)line.ProfitCurrencyAmount)
                .LocalCurrencyAmount((double)line.LocalCurrencyAmount)
                .ForiegnCurrencyAmount((double)line.ForiegnCurrencyAmount)
                .Build();
        }
        public ARInvoicePM Create(Table table, List<ARInvoiceLinePM> aRInvoiceLines)
        {
            dynamic arInvoice = table.CreateDynamicInstance();
            return new ARInvoicePMBuilder().WithDefualtValues()
                .BranchIdByCode((string)arInvoice.Branch)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .ARInvoiceTypeCode("IN")
                .IsFullAccounting(true)
                .IsGeneralInvoice(true)
                .InvoiceCurrencyIdByCode((string)arInvoice.InvoiceCurrency)
                .InvoiceCurrencyExchangeRate((double)arInvoice.InvoiceCurrencyExchangeRate)
                .PaymentTermId(BillingData.PaymentTermCashId)
                .ProfitCurrencyIdByCode((string)arInvoice.ProfitCurrency)
                .BillToId(FullAccountingData.CustomerId)
                .BillToPartnerTypeId(PartnerTypeCodes.Customer)
                .ProfitCurrencyExchangeRate((double)arInvoice.ProfitCurrencyExchangeRate)
                .SetApproved(true)
                .DueDate(DateTime.Now.AddDays(1))
                .WithaRInvoiceLines(aRInvoiceLines)
                .CalculateAmmount()
                .VatNumber((string)(arInvoice.VatNumber.ToString()))
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
