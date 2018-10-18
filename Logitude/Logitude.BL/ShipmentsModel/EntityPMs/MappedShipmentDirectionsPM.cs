using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class MappedShipmentDirectionsPM
    {
        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ShipmentDirectionId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
