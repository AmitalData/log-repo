using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CardContactProductPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardContactId { get; set; }
        public string ProductTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProductTypeName { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
