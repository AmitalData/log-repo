using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;


namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class ARInvoiceLinePMBuilder
    {
        private ARInvoiceLinePM arInvoiceLine;
        
        public ARInvoiceLinePMBuilder()
        {
            this.Reset();
        }

        public ARInvoiceLinePM Build()
        {
            ARInvoiceLinePM result = arInvoiceLine;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            arInvoiceLine = new ARInvoiceLinePM();
        }

        
        public ARInvoiceLinePMBuilder ForiegnCurrencyAmount(double? foriegnCurrencyAmount)
        {
            arInvoiceLine.ForiegnCurrencyAmount = foriegnCurrencyAmount;
            return this;
        }
        public ARInvoiceLinePMBuilder LocalCurrencyAmount(double? localCurrencyAmount)
        {
            arInvoiceLine.LocalCurrencyAmount = localCurrencyAmount;
            return this;
        }
        public ARInvoiceLinePMBuilder Quantity(double? quantity)
        {
            arInvoiceLine.Quantity = quantity;
            return this;
        }

        public ARInvoiceLinePMBuilder UnitPrice(double? unitPrice)
        {
            arInvoiceLine.UnitPrice = unitPrice;
            return this;
        }
        public ARInvoiceLinePMBuilder InvoiceCurrencyAmount(double? invoiceCurrencyAmount)
        {
            arInvoiceLine.InvoiceCurrencyAmount = invoiceCurrencyAmount;
            return this;
        }
        public ARInvoiceLinePMBuilder ProfitCurrencyAmount(double? profitCurrencyAmount)
        {
            arInvoiceLine.ProfitCurrencyAmount = profitCurrencyAmount;
            return this;
        }
        public ARInvoiceLinePMBuilder Description(string description)
        {
            arInvoiceLine.Description = description;
            return this;
        }
        public ARInvoiceLinePMBuilder LocalDescription(string localDescription)
        {
            arInvoiceLine.LocalDescription = localDescription;
            return this;
        }
        public ARInvoiceLinePMBuilder VatTypeId(string vatTypeId)
        {
            arInvoiceLine.VatTypeId = vatTypeId;
            return this;
        }
        public ARInvoiceLinePMBuilder VatPercentage(double? vatPercentage)
        {
            arInvoiceLine.VatPercentage = vatPercentage;
            return this;
        }
        public ARInvoiceLinePMBuilder ExchangeRateDate(DateTime exchangeRateDate)
        {
            arInvoiceLine.ExchangeRateDate = exchangeRateDate;
            return this;
        }
        public ARInvoiceLinePMBuilder LineActionCode(ARInvoiceLineActionsEnum lineActionCode)
        {
            arInvoiceLine.LineActionCode = (int)lineActionCode +"";
            return this;
        }
        public ARInvoiceLinePMBuilder InvoiceCurrencyCode(string invoiceCurrencyCode)
        {
            arInvoiceLine.InvoiceCurrencyCode = invoiceCurrencyCode;
            return this;
        }
        public ARInvoiceLinePMBuilder InvoiceLocalCurrencyCode(string invoiceLocalCurrencyCode)
        {
            arInvoiceLine.InvoiceLocalCurrencyCode = invoiceLocalCurrencyCode;
            return this;
        }

        public ARInvoiceLinePMBuilder ChargesTypeIdByCode(string code)
        {
            arInvoiceLine.ChargesTypeId = MapChargesType(code);
            return this;
        }
        public ARInvoiceLinePMBuilder ForiegnExchangeRate(double? foriegnExchangeRate)
        {
            arInvoiceLine.ForiegnExchangeRate = foriegnExchangeRate;
            return this;
        }

        public ARInvoiceLinePMBuilder ForiegnCurrencyIdByCode(string code)
        {
            arInvoiceLine.ForiegnCurrencyId = MapCurrencyCode(code);
            return this;
        }
        public ARInvoiceLinePMBuilder ForiegnCurrencyCode(string code)
        {
            arInvoiceLine.ForiegnCurrencyCode = code;
            return this;
        }
        public ARInvoiceLinePMBuilder WithModel(ARInvoiceLinePM arInvoiceLine)
        {
            this.arInvoiceLine = arInvoiceLine;
            return this;
        }
        

        public ARInvoiceLinePMBuilder WithDefualtValues()
        {
            arInvoiceLine = new ARInvoiceLinePM
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
