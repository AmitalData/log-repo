using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{
    public class ShipmentPayableLineStatusPM : BaseEntityPM
    {
        public ShipmentPayableLineStatusPM() :base() { }
        public ShipmentPayableLineStatusPM(ShipmentPayableLineStatus entity) : base()
        {
            this.Code = entity.Code;
            this.Name = entity.Name;
            this.SearchFields = entity.SearchFields;
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}
