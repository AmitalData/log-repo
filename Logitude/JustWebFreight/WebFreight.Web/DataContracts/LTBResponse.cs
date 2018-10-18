using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class LTBResponse
    {
        public decimal? EndBalanceForeign { get; set; }
        public decimal EndBalanceLocal { get; set; }
        public string Have1CurrencyIdInPeriod { get; set; }
        public DateTime? MaxCreateAt { get; set; }
        public decimal? StartBalanceForeign { get; set; }
        public decimal StartBalanceLocal { get; set; }
        public int? TotalRowCount { get; set; }
        public bool SuppressCumulativeDueMultiCurrencyInPeriod { get; set; }
                
    }
}