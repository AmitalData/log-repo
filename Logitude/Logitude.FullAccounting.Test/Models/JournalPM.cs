using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{
    public class JournalPM
    {
        public int Tenant { get; set; }
        public string Id { get; set; }
        public string JournalNumber { get; set; }
        public DateTime AccountingDate { get; set; }
        public string TypeCode { get; set; }
        public string StatusCode { get; set; }
        public string AccountingEntityCode { get; set; }
        public string AccountingEntityId { get; set; }
        public string ExternalNo { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        public string AccountingEntityName { get; set; }
        public List<JournalLinePM> JournalLines { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string SearchFields { get; set; }
        public string AccountingEntityReference { get; set; }
        public string OriginalJournalId { get; set; }
        public bool IsLedgerCreated { get; set; }
         
    }
}
