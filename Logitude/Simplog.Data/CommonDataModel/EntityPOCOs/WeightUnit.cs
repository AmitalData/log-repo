using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class WeightUnit
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string PrintAs { get; set; }
        public string SearchFields { get; set; }

        //public List<Shipment> GrossShipments { get; set; }
        //public List<Tenant> GrossTenants { get; set; }
        //public List<Shipment> ChargeableShipments { get; set; }
        //public List<Tenant> ChargeableTenants { get; set; }

        //public List<Measurement> Measurements { get; set; }
        //public List<Tenant> WeightMeasurementTenants { get; set; }
    }
}