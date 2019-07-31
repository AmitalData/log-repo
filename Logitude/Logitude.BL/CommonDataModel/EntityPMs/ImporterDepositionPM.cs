using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
  public  class ImporterDepositionPM
    {
        public int? Tenant { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set; }
        public string ShipperCountry { get; set; }
        public string ShipperVAT { get; set; }
        public string DepositionNumber { get; set; }
        public string ImporterVat { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
    }
}
