using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Domain.EntityPMs
{
    //[CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class MappedShipmentDirectionsPM : BaseEntityPM
    {
        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ShipmentDirectionId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
