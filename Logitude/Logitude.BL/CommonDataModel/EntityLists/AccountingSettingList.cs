using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AccountingSettingList
    {
        [Key]
        public int Id { get; set; }
        public bool AllowVoidARI { get; set; }
        public bool AllowVoidARP { get; set; }
        public bool AllowVoidAPI { get; set; }
        public bool AllowVoidAPP { get; set; }
        public bool AllowManualInvoiceNumber { get; set; }
        public bool IsVatNumberMandatoryInAR { get; set; }
        public bool IsVatNumberMandatoryInAP { get; set; }
        public bool IsARInvoiceChronologicalDates { get; set; }
        public bool IsARPaymentChronologicalDates { get; set; }
        public string AccountingSystemCode { get; set; }
        public string ReceivableVATableTempCard { get; set; }
        public string ReceivableVATExemptTempCard { get; set; }
        public string PayableVATableTempCard { get; set; }
        public string PayableVATExemptTempCard { get; set; }
        public bool AllowMinusInvoicelines { get; set; }
        public bool AllowClosureWithoutPayables { get; set; }
        public bool IsSingleTaxPerInvoice { get; set; }
        public bool IsARInvoicesTransferEnabled { get; set; }
        public bool IsAPInvoicesTransferEnabled { get; set; }
        public bool IsARPaymentsTransferEnabled { get; set; }
        public DateTime? ARInvoiceTransferStartDate { get; set; }
        public DateTime? APInvoiceTransferStartDate { get; set; }
        public DateTime? ARPaymentTransferStartDate { get; set; }
        public bool AllowPositiveAmountsInTheCreditNote { get; set; }
        public string QBOrealMeID { get; set; }
        public string QBOAccessToken { get; set; }
        public string QBOAccessTokenSecret { get; set; }
        public bool TransferToDropboxActivated { get; set; }
        public bool EnableMultiPercentageVATTypes { get; set; }
        public bool NotifyPastDateOnInvoiceEdit { get; set; }
        public bool EnableMultiRateAPInvoices { get; set; }
        public string RegistryDateTypeCode { get; set; }
        public string ReceivableVATCard { get; set; }
        public string PayableVATCard { get; set; }
        public bool EnableMultiCurrencyARPayments { get; set; }
        public bool EnableMultiCurrencyAPPayments { get; set; }
        public bool IsAPPaymentsTransferEnabled { get; set; }
        public bool EnableNegativeOffsetARPayments { get; set; }
        public bool EnableNegativeOffsetAPPayments { get; set; }
        public bool EnableInvoiceStocksManagement { get; set; }
        public string RefreshToken { get; set; }
        public int QBOOAuth { get; set; }
        public bool AllowManualARPaymentNumber { get; set; }
    }
}
