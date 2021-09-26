
using System;


namespace Logitude.FullAccounting.Test.Models
{

    public partial class BankDepositLinePM
    {

        public int Tenant { get; set; }
        public string DepositId { get; set; }
        public int Line { get; set; }
        public string ARPChequeId { get; set; }
        public string ARPaymentChequeId { get; set; }
        public bool? IsOutOfDeposit { get; set; }
        public DateTime? OutOfDepositeDate { get; set; }
        public string Notes { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal LocalAmount { get; set; }
        public string Currency { get; set; }
        public decimal ForeignAmount { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string ARPaymentNumber { get; set; }
        public string CompositId { get; set; }
        public string ARPaymentId { get; set; }
        public string SearchFields { get; set; }
        public string ChequeStatusName { get; set; }
        public string ChequeStatusCode { get; set; }
    }

}
