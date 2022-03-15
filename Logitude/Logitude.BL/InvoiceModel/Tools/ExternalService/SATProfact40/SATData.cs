using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATData
    {
        public static string CurrentComprobanteVersion = "4.0";
        public static string TestSATToken = "mvpNUXmQfK8=";
        public static string ComprobanteExportacion = "01";
        public static string ComprobanteCD = "CD";
        public static string ComprobanteG02 = "G02";
        public static string ComprobanteG03 = "G03";
        public static string ComputingPartnerCode = "G-Profact";
        public static string CountryObjectTableName = "Country";
        public static string ComprobanteE = "E";
        public static string ComprobanteI = "I";
        public static string MexicanInvoiceCurrencyCode = "MXN";
        public static string InTransferingSATTransferStatusCode = "TG";
        public static string NoTransferNeedSATTransferStatusCode = "ND";
        public static string InvoiceTransferingEventTypecode = "INTS";
        public static string PaymentTransferingEventTypecode = "PATS";
        public static string SATInterfaceQueueMessageCode = "SATInterface";
        public static string ARInvoiceObjectTableName = "ARInvoice";
        public static string ARPaymentObjectTableName = "ARPayment";
        public static string OutSideMexicoRfc = "XEXX010101000";
        public static string MexicoCountryCode = "MX";
        public static string LogitudeSATPaymentBankTransferMethod = "03";
        public static string SATPaymentBankTransferTypeCode = "01";
        public static string TransferedSATTransferStatusCode = "TD";
        public static string CanceledSATTransferStatusCode = "CS";
        public static string VoidedInvoiceStatusCode = "VD";
        public static string BimestralPeriod = "05";
        public static string IncorporacionFiscalRegimen = "621";
        public static string NotIncludeTaxObjetoImp = "01";
        public static string IncludeTaxObjetoImp = "02";
    }
}
