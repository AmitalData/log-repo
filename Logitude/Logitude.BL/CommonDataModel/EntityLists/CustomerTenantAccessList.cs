using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class CustomerTenantAccessList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int CustomerTenant { get; set; }
        public DateTime? LastShipmentDate { get; set; }
        public string ContactName { get; set; }
        public string CompanyVat { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string UpdatedByUserId { get; set; }
        public string UpdatedByUserName { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string SearchFields { get; set; }
        public bool IsPrivateLabelCustomer { get; set; }
        public string CustomCompanyName { get; set; }
        public string StockTypeCode { get; set; }
        public bool IsCustom { get; set; }
        public bool IsExport { get; set; }
        public string CustomersCodes { get; set; }
    }
}
