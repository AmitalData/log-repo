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
        public BankAccountLightPM BankAccount { get; set; }
   }

    public class BankAccountLightPM { 
        public string Id { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
    }
}
	 