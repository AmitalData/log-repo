using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class OceanInsightsStatusLogPM
	{
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OceanInsigntRequestId { get; set; }
        public string XML { get; set; }
        public DateTime CreateDate { get; set; }
      
    }
}
