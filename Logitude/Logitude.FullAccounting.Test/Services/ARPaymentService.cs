
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
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
    public class ARPaymentService
    {

        

        public ARPaymentPM Create(Table table)
        {
            dynamic arPaymentTable = table.CreateDynamicInstance();
            var arPayment = new ARPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode((string)arPaymentTable.Branch)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode((string)arPaymentTable.PaymentCurrency)
                .PaymentCurrencyExchangeRate((double)arPaymentTable.PaymentCurrencyExchangeRate)
                .PartnerId(FullAccountingData.CustomerId)
                .IsFullAccounting(true)
                .AccountingPaymentMethodId(FullAccountingData.CashPaymentMethodId)
                .AccountingPaymentMethodCode(AccountingPaymentMethodCodes.Cash)
                .BillToId(FullAccountingData.CustomerId)
                .AmountInPaymentCurrency((double)arPaymentTable.AmountInPaymentCurrency)
                .OpenAmount((double)arPaymentTable.AmountInPaymentCurrency)
                .OpenAmountInLocalCurrency((double)arPaymentTable.OpenAmountInLocalCurrency)
                .GLAccountId(FullAccountingData.CustomerGLAccountId)
                .BillToAddressId(FullAccountingData.CustomerMainAddressId)
                .CashbookId(FullAccountingData.CashBookCash1)
                .Build();
            //arPayment.InvoicesLedgerTransactions = GetInvoicesLedgerTransactions(arPayment.GLAccountId, arPayment.PaymentCurrencyId);

            return arPayment;

        }

        private List<LedgerTransactionPM> GetInvoicesLedgerTransactions(string glAccountId, string paymentCurrencyId)
        {
            var Transactions = APICaller.CallGet<ViewResponse<List<LedgerTransactionPM>>>(Urls.GetTransactionsForARPayment(glAccountId, paymentCurrencyId), UserTenant.Token).Data;
            return new List<LedgerTransactionPM>() { Transactions.Result.FirstOrDefault() };
        }

    }
}
