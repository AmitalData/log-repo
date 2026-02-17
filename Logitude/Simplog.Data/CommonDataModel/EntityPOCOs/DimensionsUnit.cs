using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DimensionsUnit
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        //public List<Shipment> Shipments { get; set; }
        //public List<Tenant> Tenants { get; set; }


    }
}