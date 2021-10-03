using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class APPaymentPMBuilder
    {
        private APPaymentPM apPaymentPM;

        public APPaymentPMBuilder()
        {
            this.Reset();
        }

        public APPaymentPM Build()
        {
            APPaymentPM result = apPaymentPM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            apPaymentPM = new APPaymentPM();
        }

        public APPaymentPMBuilder BranchId(string branchId)
        {
            apPaymentPM.BranchId = branchId;
            return this;
        }
        public APPaymentPMBuilder BranchIdByCode(string code)
        {
            apPaymentPM.BranchId = MapBranchCode(code);
            return this;
        }
        public APPaymentPMBuilder AccountingPaymentMethodId(string accountingPaymentMethodId)
        {
            apPaymentPM.AccountingPaymentMethodId = accountingPaymentMethodId;
            return this;
        }
        public APPaymentPMBuilder VendorAddressId(string vendorAddressId)
        {
            apPaymentPM.VendorAddressId = vendorAddressId;
            return this;
        }
        


        public APPaymentPMBuilder LocalCurrencyId(string localCurrencyId)
        {
            apPaymentPM.LocalCurrencyId = localCurrencyId;
            return this;
        }
        public APPaymentPMBuilder VendorId(string vendorId)
        {
            apPaymentPM.VendorId = vendorId;
            return this;
        }
        public APPaymentPMBuilder PaymentMethodCode(string paymentMethodCode)
        {
            apPaymentPM.PaymentMethodCode = paymentMethodCode;
            return this;
        }



        public APPaymentPMBuilder PaymentCurrencyId(string PaymentCurrencyId)
        {
            apPaymentPM.PaymentCurrencyId = PaymentCurrencyId;
            return this;
        }
        public APPaymentPMBuilder PaymentCurrencyIdByCode(string PaymentCurrencyCode)
        {
            apPaymentPM.PaymentCurrencyId = MapCurrencyCode(PaymentCurrencyCode);
            return this;
        }
        public APPaymentPMBuilder PaymentCurrencyExchangeRate(double? PaymentCurrencyExchangeRate)
        {
            apPaymentPM.PaymentCurrencyExchangeRate = PaymentCurrencyExchangeRate;
            return this;
        }
        public APPaymentPMBuilder ProfitCurrencyExchangeRate(double? profitCurrencyExchangeRate)
        {
            apPaymentPM.ProfitCurrencyExchangeRate = profitCurrencyExchangeRate;
            return this;
        }

        public APPaymentPMBuilder AmountInLocalCurrency(double? amountInLocalCurrency)
        {
            apPaymentPM.AmountInLocalCurrency = amountInLocalCurrency;
            return this;
        }
        public APPaymentPMBuilder AmountInPaymentCurrency(double? amountInPaymentCurrency)
        {
            apPaymentPM.AmountInPaymentCurrency = amountInPaymentCurrency;
            return this;
        }
        public APPaymentPMBuilder OpenAmount(double? openAmount)
        {
            apPaymentPM.OpenAmount = openAmount;
            return this;
        }


        public APPaymentPMBuilder AmountInProfitCurrency(double? amountInProfitCurrency)
        {
            apPaymentPM.AmountInProfitCurrency = amountInProfitCurrency;
            return this;
        }
        public APPaymentPMBuilder PaymentNo(string paymentNo)
        {
            apPaymentPM.PaymentNo = paymentNo;
            return this;
        }


        public APPaymentPMBuilder SetApproved(bool setApproved)
        {
            apPaymentPM.SetApproved = setApproved;
            return this;
        }
        public APPaymentPMBuilder VendorPartnerTypeId(string vendorPartnerTypeId)
        {
            apPaymentPM.VendorPartnerTypeId = vendorPartnerTypeId;
            return this;
        }
        public APPaymentPMBuilder TaxDeductionLocalAmount(decimal? taxDeductionLocalAmount)
        {
            apPaymentPM.TaxDeductionLocalAmount = taxDeductionLocalAmount;
            return this;
        }
        public APPaymentPMBuilder TaxDeductionPercentage(int? taxDeductionPercentage)
        {
            apPaymentPM.TaxDeductionPercentage = taxDeductionPercentage;
            return this;
        }





        public APPaymentPMBuilder PaymentCurrencyCode(string PaymentCurrencyCode)
        {
            apPaymentPM.PaymentCurrencyCode = PaymentCurrencyCode;
            return this;
        }


        public APPaymentPMBuilder StatusCode(string statusCode)
        {
            apPaymentPM.StatusCode = statusCode;
            return this;
        }

        public APPaymentPMBuilder WithModel(APPaymentPM tMEmployeeTime)
        {
            apPaymentPM = tMEmployeeTime;
            return this;
        }

        public APPaymentPMBuilder WithDefualtValues()
        {
            apPaymentPM = new APPaymentPM
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
                PrintDate = DateTime.Now,
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
