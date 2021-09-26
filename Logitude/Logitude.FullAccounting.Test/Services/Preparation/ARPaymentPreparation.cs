using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class ARPaymentPreparation
    {


        public void Prepare()
        {
            FullAccountingData.ARPaymenId = Create(AccountingPaymentMethodCodes.Cash);
        }
        public void ChequePrepare()
        {
            FullAccountingData.ChequeARPaymenId = Create(AccountingPaymentMethodCodes.Cheque);
        }

        private string Create(string accountingPaymentMethodCodes)
        {
            var arPayment = CreateInstance(accountingPaymentMethodCodes);
            var response = APICaller.CallPost<ARPaymentPM>(arPayment, Urls.ARPaymentController, UserTenant.Token);
            return response.Data?.Id;
        }

        private ARPaymentPM CreateInstance(string accountingPaymentMethodCodes)
        {
            switch (accountingPaymentMethodCodes)
            {
                case AccountingPaymentMethodCodes.Cheque:
                    return CreateChequeInstance();
                case AccountingPaymentMethodCodes.Cash:
                    return CreateCashInstance();
                default: 
                    return null;
            }

        }

        private ARPaymentPM CreateChequeInstance()
        {
            return new ARPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode(BranchCodes.BerzeitU)
                .Account("1922188")
                .Bank("1922188")
                .BankBranch("1922188")
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .OpenAmountInLocalCurrency(1000)
                .PaymentCurrencyIdByCode(CurrencyCodes.NIS)
                .PaymentCurrencyExchangeRate(1000)
                .AmountInLocalCurrency(1000)
                .PartnerId(FullAccountingData.CustomerId)
                .IsFullAccounting(true)
                .AccountingPaymentMethodId(FullAccountingData.ChequePaymentMethodId)
                .AccountingPaymentMethodCode(AccountingPaymentMethodCodes.Cheque)
                .BillToId(FullAccountingData.CustomerId)
                .BillToPartnerTypeId(PartnerTypeCodes.Customer)
                .AmountInPaymentCurrency(1000)
                .OpenAmount(1000)
                .GLAccountId(FullAccountingData.CustomerGLAccountId)
                .BillToAddressId(FullAccountingData.CustomerMainAddressId)
                .CashbookId(FullAccountingData.CashBookCheques1)
                .ChequeOrPaymentRef("2353423")
                .Build();
        }

        private ARPaymentPM CreateCashInstance()
        {
            return new ARPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode(BranchCodes.BerzeitU)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode(CurrencyCodes.NIS)
                .PaymentCurrencyExchangeRate(1000)
                .PartnerId(FullAccountingData.CustomerId)
                .IsFullAccounting(true)
                .AccountingPaymentMethodId(FullAccountingData.CashPaymentMethodId)
                .AccountingPaymentMethodCode(AccountingPaymentMethodCodes.Cash)
                .BillToId(FullAccountingData.CustomerId)
                .AmountInPaymentCurrency(1000)
                .OpenAmount(1000)
                .GLAccountId(FullAccountingData.CustomerGLAccountId)
                .BillToAddressId(FullAccountingData.CustomerMainAddressId)
                .CashbookId(FullAccountingData.CashBookCash1)
                .Build();
        }
    }
}
