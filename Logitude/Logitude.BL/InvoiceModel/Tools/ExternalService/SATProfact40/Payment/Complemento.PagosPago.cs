using Logitude.BL.InvoiceModel.EntityPMs;
using Profact.TimbraCFDI40.Complementos.Pagos20;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ComplementoPagosPago : Complemento
    {
        public static PagosPago Get(Tenant currentTenant)
        {
            Currency paymentCurrency = currencies.FirstOrDefault(c => c.Id == arPaymentPM.PaymentCurrencyId);

            PagosPago pagoItem = new PagosPago
            {
                Monto = GetMonto(arPaymentPM),
                MonedaP = paymentCurrency.Code,
                FormaDePagoP = arPaymentPM.SATPaymentMethodCode,
                FechaPago = ComplementoPagosPagoFechaPago.Get(arPaymentPM),
                NumOperacion = ComplementoPagosPagoNumOperacion.Get(arPaymentPM),
                TipoCadPagoSpecified = GetPagosPagoTipoCadPagoSpecified(arPaymentPM),
                TipoCadPago = GetPagosPagoTipoCadPago(arPaymentPM),
                CertPago = GetPagosPagoCertPago(arPaymentPM),
                CadPago = GetPagosPagoCadPago(arPaymentPM),
                SelloPago = GetPagosPagoSelloPago(arPaymentPM),
                RfcEmisorCtaOrd = ComplementoPagosPagoRfcEmisorCtaOrd.Get(arPaymentPM),
                TipoCambioP = GetTipoCambioP(arPaymentPM, paymentCurrency),
                TipoCambioPSpecified = true,
                DoctoRelacionado = ComplementoPagosPagoDoctoRelacionados.Get(currentTenant).ToArray(),
                NomBancoOrdExt = ComplementoPagosPagoNomBancoOrdExt.Get(),

            };

            pagoItem.ImpuestosP = ComplementoPagosPagoImpuestosPs.Get(pagoItem).ToArray();

            return pagoItem;
        }

        private static decimal GetMonto(ARPaymentPM arPaymentPM)
        {
            return SATBaseProfact40Service.GetDecimalWith2DigitsAfterPoint(arPaymentPM.AmountInPaymentCurrency != null ? (decimal)arPaymentPM.AmountInPaymentCurrency.Value : 0);
        }

        private static bool GetPagosPagoTipoCadPagoSpecified(ARPaymentPM arPaymentPM)
        {
            return ValidatePaymentBankTransfer.IsValid(arPaymentPM);
        }

        private static string GetPagosPagoTipoCadPago(ARPaymentPM arPaymentPM)
        {
            return !ValidatePaymentBankTransfer.IsValid(arPaymentPM) ? null : SATData.SATPaymentBankTransferTypeCode;
        }

        private static string GetPagosPagoCertPago(ARPaymentPM arPaymentPM)
        {
            return !ValidatePaymentBankTransfer.IsValid(arPaymentPM) ? null : arPaymentPM.CertPago;
        }

        private static string GetPagosPagoCadPago(ARPaymentPM arPaymentPM)
        {
            return !ValidatePaymentBankTransfer.IsValid(arPaymentPM)
                ? null
                : !string.IsNullOrEmpty(arPaymentPM.CadPago) ? arPaymentPM.CadPago.Replace("|", "&#124;") : null;
        }

        private static string GetPagosPagoSelloPago(ARPaymentPM arPaymentPM)
        {
            return !ValidatePaymentBankTransfer.IsValid(arPaymentPM) ? null : arPaymentPM.SelloPago;
        }

        private static decimal GetTipoCambioP(ARPaymentPM entityPM, Currency paymentCurrency)
        {
            decimal paymentCurrencyExchangeRate = entityPM.PaymentCurrencyExchangeRate != null ? SATBaseProfact40Service.GetDecimalWith4DigitsAfterPoint(Convert.ToDecimal(entityPM.PaymentCurrencyExchangeRate.Value)) : 0;
            return paymentCurrency.Code != SATData.MexicanInvoiceCurrencyCode ? paymentCurrencyExchangeRate : 1;
        }
    }
}
