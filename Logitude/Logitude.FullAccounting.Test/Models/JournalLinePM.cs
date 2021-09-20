using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Models
{
    public class JournalLinePM
    {
        public string JournalId { get; set; }
        public int Tenant { get; set; }
        public int Line { get; set; }
        public string ActionCode { get; set; }
        public string DebitAccountId { get; set; }
        public string CreditAccountId { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime AccountingDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal LocalAmount { get; set; }
        public string CurrencyId { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CurrencyCode { get; set; }
        public string ActionTypeCode { get; set; }
        public string ActionId { get; set; }
    }
}
