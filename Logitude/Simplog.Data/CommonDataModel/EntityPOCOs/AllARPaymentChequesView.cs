using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class AllARPaymentChequesView
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BillToId { get; set; }
        public string AccountId { get; set; }
        public DateTime ValueDate { get; set; }
        public string Type { get; set; }
        public string AccountingEntityReference { get; set; }
        public decimal LocalAmountCredit { get; set; }
        public decimal LocalAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }
        public decimal ForeignAmountDebit { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Journalnumber { get; set; }
        public string journalId { get; set; }
        public string AccountingEntityCode { get; set; }
        public string AccountingEntityId { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string Notes { get; set; }


    }
}
