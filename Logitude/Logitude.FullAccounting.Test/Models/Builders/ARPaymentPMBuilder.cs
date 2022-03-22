using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class ARPaymentPMBuilder
    {
        private ARPaymentPM arPaymentPM;

        public ARPaymentPMBuilder()
        {
            this.Reset();
        }

        public ARPaymentPM Build()
        {
            ARPaymentPM result = arPaymentPM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            arPaymentPM = new ARPaymentPM();
        }

        public ARPaymentPMBuilder BranchId(string branchId)
        {
            arPaymentPM.BranchId = branchId;
            return this;
        }
        public ARPaymentPMBuilder BranchIdByCode(string code)
        {
            arPaymentPM.BranchId = MapBranchCode(code);
            return this;
        }
        public ARPaymentPMBuilder Account(string account)
        {
            arPaymentPM.Account = account;
            return this;
        }
        public ARPaymentPMBuilder Bank(string bank)
        {
            arPaymentPM.Bank = bank;
            return this;
        }
        public ARPaymentPMBuilder BankBranch(string bankBranch)
        {
            arPaymentPM.BankBranch = bankBranch;
            return this;
        }
        public ARPaymentPMBuilder ChequeOrPaymentRef(string chequeOrPaymentRef)
        {
            arPaymentPM.ChequeOrPaymentRef = chequeOrPaymentRef;
            return this;
        }

        public ARPaymentPMBuilder AccountingPaymentMethodId(string accountingPaymentMethodId)
        {
            arPaymentPM.AccountingPaymentMethodId = accountingPaymentMethodId;
            return this;
        }
        public ARPaymentPMBuilder AccountingPaymentMethodCode(string accountingPaymentMethod)
        {
            arPaymentPM.AccountingPaymentMethodCode = accountingPaymentMethod;
            return this;
        }

        public ARPaymentPMBuilder LocalCurrencyId(string localCurrencyId)
        {
            arPaymentPM.LocalCurrencyId = localCurrencyId;
            return this;
        }
        public ARPaymentPMBuilder GLAccountId(string gLAccountId)
        {
            arPaymentPM.GLAccountId = gLAccountId;
            return this;
        }
        public ARPaymentPMBuilder CashbookId(string cashbookId)
        {
            arPaymentPM.CashbookId = cashbookId;
            return this;
        }
        public ARPaymentPMBuilder BillToAddressId(string billToAddressId)
        {
            arPaymentPM.BillToAddressId = billToAddressId;
            return this;
        }
        

        public ARPaymentPMBuilder PaymentCurrencyId(string PaymentCurrencyId)
        {
            arPaymentPM.PaymentCurrencyId = PaymentCurrencyId;
            return this;
        }
        public ARPaymentPMBuilder PaymentCurrencyIdByCode(string PaymentCurrencyCode)
        {
            arPaymentPM.PaymentCurrencyId = MapCurrencyCode(PaymentCurrencyCode);
            return this;
        }
        public ARPaymentPMBuilder PaymentCurrencyExchangeRate(double? PaymentCurrencyExchangeRate)
        {
            arPaymentPM.PaymentCurrencyExchangeRate = PaymentCurrencyExchangeRate;
            return this;
        }
        public ARPaymentPMBuilder ProfitCurrencyExchangeRate(double? profitCurrencyExchangeRate)
        {
            arPaymentPM.ProfitCurrencyExchangeRate = profitCurrencyExchangeRate;
            return this;
        }
        
        public ARPaymentPMBuilder AmountInLocalCurrency(double? amountInLocalCurrency)
        {
            arPaymentPM.AmountInLocalCurrency = amountInLocalCurrency;
            return this;
        }
        public ARPaymentPMBuilder AmountInPaymentCurrency(double? amountInPaymentCurrency)
        {
            arPaymentPM.AmountInPaymentCurrency = amountInPaymentCurrency;
            return this;
        }
        public ARPaymentPMBuilder OpenAmount(double? openAmount)
        {
            arPaymentPM.OpenAmount = openAmount;
            return this;
        }
        public ARPaymentPMBuilder OpenAmountInLocalCurrency(double? openAmountInLocalCurrency)
        {
            arPaymentPM.OpenAmountInLocalCurrency = openAmountInLocalCurrency;
            return this;
        }

        public ARPaymentPMBuilder AmountInProfitCurrency(double? amountInProfitCurrency)
        {
            arPaymentPM.AmountInProfitCurrency = amountInProfitCurrency;
            return this;
        }
        
       
        public ARPaymentPMBuilder SetApproved(bool setApproved)
        {
            arPaymentPM.SetApproved = setApproved;
            return this;
        }
       
        public ARPaymentPMBuilder IsFullAccounting(bool isFullAccounting)
        {
            arPaymentPM.IsFullAccounting = isFullAccounting;
            return this;
        }

        

        public ARPaymentPMBuilder PaymentCurrencyCode(string PaymentCurrencyCode)
        {
            arPaymentPM.PaymentCurrencyCode = PaymentCurrencyCode;
            return this;
        }
        

        public ARPaymentPMBuilder StatusCode(string statusCode)
        {
            arPaymentPM.StatusCode = statusCode;
            return this;
        }
        public ARPaymentPMBuilder BillToId(string billToId)
        {
            arPaymentPM.BillToId = billToId;
            return this;
        }
        public ARPaymentPMBuilder PartnerId(string partnerId)
        {
            arPaymentPM.PartnerId = partnerId;
            return this;
        }

        public ARPaymentPMBuilder BillToPartnerTypeId(string billToPartnerTypeId)
        {
            arPaymentPM.BillToPartnerTypeId = billToPartnerTypeId;
            return this;
        }

        public ARPaymentPMBuilder WithInvoicesLedgerTransactions(List<LedgerTransactionPM> ledgerTransactionPMs)
        {
            arPaymentPM.InvoicesLedgerTransactions = ledgerTransactionPMs;
            return this;
        }

        public ARPaymentPMBuilder CalculateAmmount()
        {

            foreach (var item in arPaymentPM.InvoicesLedgerTransactions)
            {
                
                
                
            }
            return this;
        }

       

        public ARPaymentPMBuilder WithModel(ARPaymentPM tMEmployeeTime)
        {
            arPaymentPM = tMEmployeeTime;
            return this;
        }

        public ARPaymentPMBuilder WithDefualtValues()
        {
            arPaymentPM = new ARPaymentPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                RegisterDate = DateTime.Now,
                ValueDate = DateTime.Now,
                StatusCode = "DR",
                AmountInPaymentCurrency = 0,
                
                AmountInLocalCurrency = 0,
               

        };
            return this;
        }
        private string MapCurrencyCode(string PaymentCurrencyCode)
        {
            switch (PaymentCurrencyCode)
            {
                case CurrencyCodes.NIS:
                    return BillingData.CurrencyNISId;
                case CurrencyCodes.EUR:
                    return BillingData.CurrencyEURId;
                default:
                    return null;
            }
        }
        private string MapBranchCode(string branchCode)
        {
            switch (branchCode)
            {
                case BranchCodes.BerzeitU:
                    return FullAccountingData.BZUBranchID;
                case BranchCodes.Ramallah:
                    return FullAccountingData.RamallahBranchID;
                default:
                    return null;
            }
        }


    }
}
