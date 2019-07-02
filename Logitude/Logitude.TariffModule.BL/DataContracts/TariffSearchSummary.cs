using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
   public class TariffSearchSummary
    {
        [Key]
        public string Id { get; set; }
        public string price { get; set; }
        public DateTime? EffictiveDate { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; }
        public decimal? decimalprice { get; set; }
        public string Currency { get; set; }
        public string VersionId { get; set; }


    }
}
