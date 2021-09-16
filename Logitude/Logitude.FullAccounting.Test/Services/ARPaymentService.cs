using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.Test.Base.Models.BillingsPreparation;
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
    public class ARPaymentService
    {
        const string JournalAccountingEntityCode = "1";
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

        public ARPaymentPM Create(Table table)
        {
            dynamic arPayment = table.CreateDynamicInstance();
            return new ARPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode((string)arPayment.Branch)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode((string)arPayment.PaymentCurrency)
                .PaymentCurrencyExchangeRate((double)arPayment.PaymentCurrencyExchangeRate)
                .BillToId()
                .PartnerId((double)arPayment.PaymentCurrencyExchangeRate)
                .IsFullAccounting(true)
                .BillToId(FullAccountingData.CustomerId)
                .BillToPartnerTypeId("CS")
                .ProfitCurrencyExchangeRate((double)arInvoice.ProfitCurrencyExchangeRate)
                .SetApproved(true)
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
