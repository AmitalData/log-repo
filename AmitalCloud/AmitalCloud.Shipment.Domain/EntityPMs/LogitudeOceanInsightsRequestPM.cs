using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class LogitudeOceanInsightsRequestPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContainerNumber { get; set; }
        public string OceanInsigntId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string BLNumber { get; set; }
        public string ShipmentId { get; set; }
    }
}
