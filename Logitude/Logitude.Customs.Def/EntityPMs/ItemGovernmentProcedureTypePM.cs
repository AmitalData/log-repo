using Logitude.Customs.Def.Contracts;
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
    public partial class ItemGovernmentProcedureTypePM : IIIGClosedTable  
    {
        [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
        [DataMember]
        public string LeadingDocumentTypeID { get; set; }
    }
}
