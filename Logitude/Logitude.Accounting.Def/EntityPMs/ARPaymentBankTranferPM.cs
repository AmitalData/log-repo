using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using System.Runtime.Serialization;
using Logitude.Accounting.Def.Validators;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.Def.EntityPMs
{
   public partial class ARPaymentBankTranferPM : EntityPM
   {
        [DataMember]
        public BankAccountPM BankAccount { get; set; }
   }
   
}
	 