using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ProductTypeModificationView
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string CostTariffUse { get; set; }
        public string SaleTariffUse { get; set; }

    }
}
