using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.Validators;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
      [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
    public partial class ModificationAndDiscountTypePM : EntityPM, IIIGClosedTable, IIIGClosedTableDummyTenant
    {
          [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
          [DataMember]
          public int Tenant { get; set; }
    }
}
