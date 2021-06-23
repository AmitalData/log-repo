using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public class WarehouseEntryPackagePM
    {
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string WarehouseEntryId { get; set; }
        public bool IsContainer { get; set; }
        public int Quantity { get; set; }
        public int Instock { get; set; }
        public string ChangeSetOp { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }

    }
}
