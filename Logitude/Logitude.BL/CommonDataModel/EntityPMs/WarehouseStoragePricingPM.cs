using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class WarehouseStoragePricingPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string WarehouseId { get; set; }
        public int StepFrom { get; set; }
        public int? StepTo { get; set; }
        public int? Days { get; set; }
        public decimal? SalePrice { get; set; }
        public int LineNumber { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
