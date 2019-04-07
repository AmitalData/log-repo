using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ARInvoiceStockPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Amount { get; set; }
        public int? Remaining { get; set; }
        public string Notes { get; set; }
        public string StatusName { get; set; }

        public bool NumbersAdded { get; set; }
        public bool NumberRemoved { get; set; }
        public bool SeriesRemoved { get; set; }
        public bool Cancelled { get; set; }
        public bool Reactivated { get; set; }

        private List<ARInvoiceStockLinePM> aRInvoiceStockLines;
        [Include]
        [Composition]
        [Association("ARInvoiceStockARInvoiceStockLines", "Id", "ARInvoiceStockId")]
        public virtual List<ARInvoiceStockLinePM> ARInvoiceStockLines
        {
            get
            {
                if (aRInvoiceStockLines == null)
                {
                    aRInvoiceStockLines = new List<ARInvoiceStockLinePM>();
                }

                return this.aRInvoiceStockLines;
            }
            set
            {
                if (value != null)
                {
                    aRInvoiceStockLines = value;
                }
            }
        }
    }
}
