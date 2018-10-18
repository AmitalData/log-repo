using Logitude.Customs.Def.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class CustomsCollateralsAnswerPM
    {
        [DataMember]
        [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
        public string AnswerEntityTypeJoin { get; set; }
    }
}
