using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APInvoiceAnalytic
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InternalNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string VendorId { get; set; }
        public string VATNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MainEntityReference { get; set; }
        public double? AmountDue { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public string BranchId { get; set; }
       
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? OperationalDate { get; set; }
      
        public DateTime? FirstApproveDate { get; set; }

        public bool TotalVATOnly { get; set; }

        public DateTime? PaidDate { get; set; }

        
    }
}


