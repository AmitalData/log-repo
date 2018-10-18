using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class VolumeUnit
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<Shipment> Shipments { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
}