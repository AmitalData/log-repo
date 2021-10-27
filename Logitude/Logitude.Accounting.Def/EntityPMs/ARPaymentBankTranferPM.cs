using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using System.Runtime.Serialization;

namespace Logitude.Accounting.Def.EntityPMs
{
   public partial class ARPaymentBankTranferPM : EntityPM
   {
        [DataMember]
        public BankAccount BankAccount { get; set; }
   }
   
}
	 