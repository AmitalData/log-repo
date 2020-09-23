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
    public class ShipmentStoragePricingPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int StepFrom { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? StepTo { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Days { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? SalePrice { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? Amount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? ChargeableDays { get; set; }

        public int LineNumber { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
