using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class OceanInsightsStatusLogPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OceanInsigntRequestId { get; set; }
        public string XML { get; set; }
        public DateTime CreateDate { get; set; }
      
    }
}
