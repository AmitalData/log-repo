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

     [CustomValidation(typeof(PaymentOrderValidator), "IsAccountingCustomFileExist")]
    public partial class PaymentOrderPM: EntityPM
    {
        public string CustomsRequestsSheetId { get; set; }
        public List<string> CustomsRequestsDeclarationId { get; set; }


        // [DataMember]
        //public string DocumentPaymentId { get; set; }
    }
}
