using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityPOCOs
{
    public class CashBookStatusChartDataView
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CashBookTypeCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public DateTime ChequeValueDate { get; set; }
        public decimal? ChequeAmount { get; set; }


    }
}
