using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class OceanInsightsRequestPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SCACCode { get; set; }
        public string ContainerNumber { get; set; }
        public string OceanInsigntId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate  { get; set; }
        public string Type { get; set; }
        public string BLNumber { get; set; }
        public bool FromPushPage { get; set; }
		public string System { get; set; }
        public bool IsClosed { get; set; }


    }

    public static class SystemType
    {
        public const string Export = "Export";
        public const string Import = "Import";
    }
}
