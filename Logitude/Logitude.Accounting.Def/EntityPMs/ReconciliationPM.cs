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

        [DataMember]
        public bool CreatedByReconciliationAfterConversion { get; set; }

        [DataMember]
        public bool CreatedByReconciliationStageB { get; set; }

        public bool CreateAutoReconcileWhileStreamingService { get; set; }
        
    }
}
