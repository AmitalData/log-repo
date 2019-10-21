using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CustomsTransferLinePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomsTransferHeaderId { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentNumber { get; set; }

        public DateTime? ArrivalDate { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Status { get; set; }
    }
}
