using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class RateClass
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        //public List<Shipment> Shipments { get; set; }
    }
}