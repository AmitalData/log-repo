using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.DataContract
{
   public class TaxReportData
    {
        [Key]
        public string Id { get; set; }
        public string VATNumber { get; set; }
        public string Reference { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string ReferenceGroup { get; set; }
        public string JournalId { get; set; }
        public string OutputOrInput { get; set; }
        public decimal VATAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public bool IsEquipment { get; set;}
        public bool IsManuallyChanged { get; set; }
        public string AccountingEntity { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public string OppositGLAccount { get; set; }
        public string AccountingEntityId { get; set; }
        public decimal? TaxReportTotalAmount { get; set; }
        public int JournalLineNumber { get; set; }
        public string AccountId { get;  set; }
        public string TransmitStatusCode { get; set; }
    }
}
