using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class LogitudeOceanInsightsRequest
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SCACCode { get; set; }
        public string ContainerNumber { get; set; }
        public string OceanInsigntId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Type { get; set; }
        public string BLNumber { get; set; }
        public string ShipmentId { get; set; }
    }
}
