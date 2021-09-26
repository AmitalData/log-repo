using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class APInvoiceLineBuilder
    {
        private APInvoiceLinePM apInvoiceLinePM;

        public APInvoiceLineBuilder()
        {
            this.Reset();
        }

        public APInvoiceLinePM Build()
        {
            APInvoiceLinePM result = apInvoiceLinePM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            apInvoiceLinePM = new APInvoiceLinePM();
        }


        public APInvoiceLineBuilder ForiegnCurrencyAmount(double? foriegnCurrencyAmount)
        {
            apInvoiceLinePM.ForiegnCurrencyAmount = foriegnCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder LocalCurrencyAmount(double? localCurrencyAmount)
        {
            apInvoiceLinePM.LocalCurrencyAmount = localCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder ForiegnCurrencyIdByCode(string Code)
        {
            apInvoiceLinePM.ForiegnCurrencyId = MapCurrencyCode(Code);
            return this;
        }
        
        
        public APInvoiceLineBuilder InvoiceCurrencyAmount(double? invoiceCurrencyAmount)
        {
            apInvoiceLinePM.InvoiceCurrencyAmount = invoiceCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder ForiegnExchangeRate(double? foriegnExchangeRate)
        {
            apInvoiceLinePM.ForiegnExchangeRate = foriegnExchangeRate;
            return this;
        }
        public APInvoiceLineBuilder VendorId(string vendorId)
        {
            apInvoiceLinePM.VendorId = vendorId;
            return this;
        }

        public APInvoiceLineBuilder ProfitCurrencyAmount(double? profitCurrencyAmount)
        {
            apInvoiceLinePM.ProfitCurrencyAmount = profitCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder Description(string description)
        {
            apInvoiceLinePM.Description = description;
            return this;
        }
 
        public APInvoiceLineBuilder VatTypeId(string vatTypeId)
        {
            apInvoiceLinePM.VatTypeId = vatTypeId;
            return this;
        }
        public APInvoiceLineBuilder VatPercentage(double? vatPercentage)
        {
            apInvoiceLinePM.VatPercentage = vatPercentage;
            return this;
        }
        

        public APInvoiceLineBuilder ChargesTypeIdByCode(string code)
        {
            apInvoiceLinePM.ChargesTypeId = MapChargesType(code);
            return this;
        }
       
        public APInvoiceLineBuilder WithModel(APInvoiceLinePM apInvoiceLinePM)
        {
            this.apInvoiceLinePM = apInvoiceLinePM;
            return this;
        }


        public APInvoiceLineBuilder WithDefualtValues()
        {
            apInvoiceLinePM = new APInvoiceLinePM
            {
                Tenant = UserTenant.Tenant,

            };
            return this;
        }

        private string MapCurrencyCode(string code)
        {
            switch (code)
            {
                case CurrencyCodes.NIS:
                    return BillingData.CurrencyNISId;
                default:
                    return null;
            }

        }

        private string MapChargesType(string code)
        {
            switch (code)
            {
                case "ITMS":
                    return FullAccountingData.ItemsChargeTypeId;
                default:
                    return null;
            }
        }
    }
}
