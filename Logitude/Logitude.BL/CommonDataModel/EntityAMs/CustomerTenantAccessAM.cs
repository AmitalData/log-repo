using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class CustomerTenantAccessAM
    {
        public int Tenant { get; set; }
        public int CustomerTenant { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyVat { get; set; }
        public string ContactName { get; set; }
        public string ContactMobile { get; set; }
        public string ContactPhone { get; set; }
        public bool IsPrivateLabelCustomer { get; set; }
        public string StockTypeCode { get; set; } 
    }
}
