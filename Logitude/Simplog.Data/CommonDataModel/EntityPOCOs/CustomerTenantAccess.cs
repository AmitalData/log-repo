using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerTenantAccess
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int CustomerTenant { get; set; }
        public string ContactName { get; set; }
        public string CompanyVat { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime? LastShipmentDate { get; set; }


        public string Status { get; set; }
        [ForeignKey("Status")]
        public virtual CustomerTenantAccessStatusType StatusCode { get; set; }

        public string UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string SearchFields { get; set; }

        public bool IsPrivateLabelCustomer { get; set; }

        public string StockTypeCode { get; set; } //A - Agent Stock, C - Customer Stock


    }
}
