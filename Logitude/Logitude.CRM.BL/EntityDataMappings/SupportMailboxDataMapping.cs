
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.BL.EntityDataMappings
{   
   public partial class SupportMailboxDataMapping: IMapping<SupportMailboxPM, SupportMailbox>
   {
        public void CustomPMToPOCO(SupportMailboxPM entityPM, SupportMailbox entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(SupportMailboxPM entityPM, SupportMailbox entityPOCO)
        {
            //throw new NotImplementedException();
        }
        private void BuildSearchFields(SupportMailboxPM entityPM, SupportMailbox entityPOCO, bool p)
        {
            string result = entityPM.Mailbox;
            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }
    }
}
   