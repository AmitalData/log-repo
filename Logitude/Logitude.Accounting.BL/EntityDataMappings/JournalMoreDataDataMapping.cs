
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class JournalMoreDataDataMapping: IMapping<JournalMoreDataPM, JournalMoreData>
   {

        public void CustomPMToPOCO(JournalMoreDataPM entityPM, JournalMoreData entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.JournalId = entityPM.JournalId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.IsLedgerCreated = entityPM.IsLedgerCreated;


            }
        }

        public void CustomPOCOToPM(JournalMoreDataPM entityPM, JournalMoreData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   