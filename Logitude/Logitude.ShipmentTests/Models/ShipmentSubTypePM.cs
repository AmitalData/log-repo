using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Models
{
    public class ShipmentSubTypePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShipmentTypeCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
