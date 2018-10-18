
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ImporterDespositionDataMapping: IMapping<ImporterDespositionPM, ImporterDesposition>
   {

        public void CustomPMToPOCO(ImporterDespositionPM entityPM, ImporterDesposition entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ImporterDespositionPM entityPM, ImporterDesposition entityPOCO)
        {

            CustomMappedPMProperties.Add(PMPropertyNames.ImporterDepositionStatusName);
            if (entityPOCO.ImporterDepositionStatusCode != null)
            {
                ImporterPeriodicDeclarStatusQueryService entityQuery = new ImporterPeriodicDeclarStatusQueryService(entityPM.Tenant);
                ImporterPeriodicDeclarStatusPM entity = entityQuery.GetSingle(entityPOCO.ImporterDepositionStatusCode, false, false);
                if (entity != null)
                    entityPM.ImporterDepositionStatusName = entity.LocalName;
            }
        }
   }


}
   