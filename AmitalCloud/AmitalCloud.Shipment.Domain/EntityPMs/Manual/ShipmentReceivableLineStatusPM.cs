using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{
    public class ShipmentReceivableLineStatusPM : BaseEntityPM
    {
        public ShipmentReceivableLineStatusPM() :base() { }
        public ShipmentReceivableLineStatusPM(ShipmentReceivableLineStatus entity) : base()
        {
            Code = entity.Code;
            Name = entity.Name;
            SearchFields = entity.SearchFields;
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}
