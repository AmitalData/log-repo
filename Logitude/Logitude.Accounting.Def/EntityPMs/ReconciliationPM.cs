using Logitude.Server.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;


namespace Logitude.Accounting.Def.EntityPMs
{
    //[CustomValidation(typeof(ReconciliationValidator), "IsReconciliationValid")]
    public partial class ReconciliationPM : EntityPM
    {
        [DataMember]
        public object LedgerTransactionPMsUpdated { get; set; }
        
    }
}
