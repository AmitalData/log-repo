
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

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AccountingEntitiesJournalDataMapping: IMapping<AccountingEntitiesJournalPM, AccountingEntitiesJournal>
   {

        public void CustomPMToPOCO(AccountingEntitiesJournalPM entityPM, AccountingEntitiesJournal entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(AccountingEntitiesJournalPM entityPM, AccountingEntitiesJournal entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   