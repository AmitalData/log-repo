using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class AgingReportDashboardClass
    {
        private static int counter = 0;
        public AgingReportDashboardClass()
        {
            linePrimary = ++counter;
        }
        [Key]
        public int linePrimary { get; set; }
        public double Amount { get; set; }
        public string AmountLabel { get; set; }
        public DateTime DueDate { get; set; }
        public string DateRange { get; set; }
        public string DateRangeLabel { get; set; }
        public int IndexOrder { get; set; }
        public string BranchId { get; set; }
    }
}