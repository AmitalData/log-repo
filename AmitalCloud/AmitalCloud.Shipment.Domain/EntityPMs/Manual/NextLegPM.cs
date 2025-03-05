using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Shipment.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{
    public class NextLegPM : BaseEntityPM
    {
        public NextLegPM() : base() { }
        public NextLegPM(NextLeg entity) : base()
        {
            this.Code = entity.Code;
            this.Name = entity.Name;
        }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
