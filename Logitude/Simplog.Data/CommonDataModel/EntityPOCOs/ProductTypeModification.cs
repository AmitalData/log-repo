using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class ProductTypeModification
    {
       [Key]
       public string ProductTypeCode { get; set; }
       [Key]
       public int Tenant { get; set; }
       public bool InActive { get; set; }
        
        public string QuotationDefaultTemplateId { get; set; }
        public ProductType ProductType { get; set; }

        public string RoutingRQuoteDefaultTemplateId { get; set; }
        public string CostTariffUse { get; set; }

        public string SaleTariffUse { get; set; }

    }
}
