using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.Customs.Data.DataContracts
{
    public class ImporterDespositionClass
    {
        [Key]
        public string Id { get; set; }
      public string Status { get; set; }
        public string ImporterDespositionNumber { get; set; }
        public DateTime? EndDate { get; set; }
        public string VendorId { get; set; }
        public string VendorNumber { get;  set; }
        public string VendorName { get;  set; }
        public string CountryCode { get;  set; }
        public string SearchFields { get; set; }
    }
}